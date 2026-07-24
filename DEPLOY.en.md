# Deployment Guide — Team Portfolio

## Project Specifications

| Item | Value |
|------|-------|
| Framework | ASP.NET Core 9 (MVC) |
| Database | SQLite |
| Auth | ASP.NET Identity |
| Web Server | Nginx + Kestrel |
| Process Manager | systemd |
| SSL | Cloudflare Origin Certificate or Let's Encrypt |

---

## Server Requirements

- Ubuntu 22.04 LTS
- Minimum: 1 vCPU, 1GB RAM, 20GB disk
- Root access to the server

---

## Step 1 — Update Server

```bash
ssh root@YOUR_SERVER_IP
apt update && apt upgrade -y
```

---

## Step 2 — Install .NET 9 SDK

```bash
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

apt update
apt install -y dotnet-sdk-9.0

# Verify
dotnet --version   # should show 9.x.x
```

---

## Step 3 — Install Git and Nginx

```bash
apt install -y git nginx
systemctl enable nginx
systemctl start nginx
```

---

## Step 4 — Create Dedicated User (Security)

```bash
useradd -m -s /bin/bash teamportfolio
```

---

## Step 5 — Clone Project from GitHub

```bash
mkdir -p /var/www/teamportfolio
cd /var/www/teamportfolio

# Public repo:
git clone https://github.com/YOUR_USERNAME/YOUR_REPO.git .

# Private repo — SSH key (recommended):
ssh-keygen -t ed25519 -C "server@teamportfolio" -f ~/.ssh/github_deploy -N ""
cat ~/.ssh/github_deploy.pub
# Add this key to GitHub > Settings > Deploy Keys
GIT_SSH_COMMAND='ssh -i ~/.ssh/github_deploy' git clone git@github.com:YOUR_USERNAME/YOUR_REPO.git .
```

> **Note:** If clone created a subdirectory (e.g. `team_site`), navigate into it:
> ```bash
> cd /var/www/teamportfolio/team_site
> ```

---

## Step 6 — Build and Publish

```bash
# From the project root (where src/ exists):
dotnet publish src/TeamPortfolio.Web/TeamPortfolio.Web.csproj \
  -c Release \
  -o /var/www/teamportfolio-app \
  --no-self-contained
```

Expected output:
```
Build succeeded in ~67s
```

---

## Step 7 — Create Required Directories

These directories are excluded from git and must be created on the server:

```bash
mkdir -p /var/www/teamportfolio-app/wwwroot/uploads/profiles
mkdir -p /var/www/teamportfolio-app/wwwroot/uploads/portfolio
mkdir -p /var/www/teamportfolio-app/wwwroot/uploads/blog
mkdir -p /var/www/teamportfolio-app/data

chown -R teamportfolio:teamportfolio /var/www/teamportfolio-app
chmod -R 775 /var/www/teamportfolio-app/wwwroot/uploads
chmod -R 770 /var/www/teamportfolio-app/data
```

---

## Step 8 — Configure appsettings.Production.json

This file is excluded from git and must be created on the server:

```bash
nano /var/www/teamportfolio-app/appsettings.Production.json
```

Content:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=/var/www/teamportfolio-app/data/teamportfolio.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "yourdomain.com"
}
```

```bash
chown teamportfolio:teamportfolio /var/www/teamportfolio-app/appsettings.Production.json
chmod 600 /var/www/teamportfolio-app/appsettings.Production.json
```

---

## Step 9 — Run Database Migrations

```bash
dotnet tool install --global dotnet-ef
export PATH="$PATH:$HOME/.dotnet/tools"

# Run from the project source directory:
dotnet ef database update \
  --project src/TeamPortfolio.Infrastructure/TeamPortfolio.Infrastructure.csproj \
  --startup-project src/TeamPortfolio.Web/TeamPortfolio.Web.csproj \
  --connection "Data Source=/var/www/teamportfolio-app/data/teamportfolio.db"

# Verify database was created
ls -la /var/www/teamportfolio-app/data/
```

---

## Step 10 — systemd Service

```bash
nano /etc/systemd/system/teamportfolio.service
```

```ini
[Unit]
Description=Team Portfolio ASP.NET Core App
After=network.target

[Service]
WorkingDirectory=/var/www/teamportfolio-app
ExecStart=/usr/bin/dotnet /var/www/teamportfolio-app/TeamPortfolio.Web.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=teamportfolio
User=teamportfolio
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5001
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=HOME=/home/teamportfolio

[Install]
WantedBy=multi-user.target
```

```bash
systemctl daemon-reload
systemctl enable teamportfolio
systemctl start teamportfolio

# Check status
systemctl status teamportfolio

# Direct test
curl -H "Host: yourdomain.com" http://localhost:5001
```

---

## Step 11 — SSL Certificate

### Option A — Cloudflare Origin Certificate (if using Cloudflare)

**In Cloudflare:**
1. SSL/TLS → Origin Server → **Create Certificate**
2. Hostnames: `*.yourdomain.com` or specific subdomain
3. Expiration: 15 years
4. Click **Create** — copy both the certificate and private key

**On the server:**
```bash
mkdir -p /etc/nginx/ssl/teamportfolio

nano /etc/nginx/ssl/teamportfolio/cert.pem
# Paste the Origin Certificate content

nano /etc/nginx/ssl/teamportfolio/key.pem
# Paste the Private Key content

chmod 600 /etc/nginx/ssl/teamportfolio/key.pem
chmod 644 /etc/nginx/ssl/teamportfolio/cert.pem
```

> **Note:** If a wildcard certificate already exists on the server (e.g. `/etc/ssl/yourdomain.pem`), you can reuse it directly.

### Option B — Let's Encrypt (if not using Cloudflare proxy)

```bash
apt install -y certbot python3-certbot-nginx
certbot --nginx -d yourdomain.com -d www.yourdomain.com
certbot renew --dry-run  # test auto-renewal
```

---

## Step 12 — Nginx Configuration

```bash
nano /etc/nginx/sites-available/teamportfolio
```

```nginx
# HTTPS
server {
    listen 443 ssl;
    listen [::]:443 ssl;
    server_name yourdomain.com;

    # Set certificate paths based on your chosen method:
    # Cloudflare Origin Certificate:
    ssl_certificate     /etc/nginx/ssl/teamportfolio/cert.pem;
    ssl_certificate_key /etc/nginx/ssl/teamportfolio/key.pem;
    # Let's Encrypt:
    # ssl_certificate     /etc/letsencrypt/live/yourdomain.com/fullchain.pem;
    # ssl_certificate_key /etc/letsencrypt/live/yourdomain.com/privkey.pem;

    client_max_body_size 20M;

    location / {
        proxy_pass         http://localhost:5001;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_set_header   X-Real-IP $remote_addr;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
}

# HTTP → HTTPS redirect
server {
    listen 80;
    listen [::]:80;
    server_name yourdomain.com;
    return 301 https://$host$request_uri;
}
```

```bash
ln -s /etc/nginx/sites-available/teamportfolio /etc/nginx/sites-enabled/
nginx -t && systemctl reload nginx
```

---

## Step 13 — DNS Configuration (Cloudflare)

| Type | Name | Content | Proxy |
|------|------|---------|-------|
| A | `subdomain` | `YOUR_SERVER_IP` | ✅ Proxied |

**SSL/TLS Settings:**
- Mode: **Full (strict)** — when using Origin Certificate
- Mode: **Full** — when using Let's Encrypt
- Always Use HTTPS: **On**

---

## Step 14 — Firewall

```bash
ufw allow OpenSSH
ufw allow 'Nginx Full'
ufw enable
ufw status
```

---

## Update Script (for future deployments)

```bash
nano /usr/local/bin/deploy-teamportfolio.sh
```

```bash
#!/bin/bash
set -e

SOURCE_DIR="/var/www/teamportfolio/team_site"
APP_DIR="/var/www/teamportfolio-app"

echo "==> Pulling latest changes..."
cd $SOURCE_DIR
git pull origin main

echo "==> Building..."
dotnet publish src/TeamPortfolio.Web/TeamPortfolio.Web.csproj \
  -c Release \
  -o $APP_DIR \
  --no-self-contained

echo "==> Running migrations..."
export PATH="$PATH:$HOME/.dotnet/tools"
dotnet ef database update \
  --project src/TeamPortfolio.Infrastructure/TeamPortfolio.Infrastructure.csproj \
  --startup-project src/TeamPortfolio.Web/TeamPortfolio.Web.csproj \
  --connection "Data Source=$APP_DIR/data/teamportfolio.db"

echo "==> Fixing permissions..."
chown -R teamportfolio:teamportfolio $APP_DIR

echo "==> Restarting service..."
systemctl restart teamportfolio
systemctl status teamportfolio --no-pager

echo ""
echo "✓ Deploy complete!"
```

```bash
chmod +x /usr/local/bin/deploy-teamportfolio.sh

# From now on, to deploy updates:
deploy-teamportfolio.sh
```

---

## Troubleshooting

```bash
# Application logs
journalctl -u teamportfolio -n 50 --no-pager

# Nginx error logs
tail -f /var/log/nginx/error.log

# Service status
systemctl status teamportfolio

# Direct app test (bypassing nginx)
curl -H "Host: yourdomain.com" http://localhost:5001

# Check open ports
ss -tlnp | grep -E ':(80|443|5001)'
```

| Error | Likely Cause | Fix |
|-------|-------------|-----|
| `Bad Request - Invalid Hostname` | AllowedHosts misconfigured | Check `appsettings.Production.json` |
| `502 Bad Gateway` | App is crashing | Check `journalctl -u teamportfolio` |
| `404 Not Found` | Nginx not forwarding to app | Check nginx config |
| `SSL Error` | Wrong cert path or expired | Check ssl_certificate paths in nginx |

---

## Architecture Overview

```
User
  │
  │ HTTPS (443)
  ▼
Cloudflare (SSL Termination + CDN + DDoS Protection)
  │
  │ HTTPS (443) with Origin Certificate
  ▼
Nginx on Server
  │
  │ HTTP (localhost:5001)
  ▼
Kestrel (ASP.NET Core)
  │
  ▼
SQLite Database
(/var/www/teamportfolio-app/data/teamportfolio.db)
```

---

## Important Notes

- `teamportfolio.db` is not in git — **always keep backups**
- `wwwroot/uploads/` is not in git — backup separately
- `appsettings.Production.json` is not in git — lives only on the server
- Change the default admin password (`Admin@1234`) after first login
- Cloudflare Origin Certificate is valid for 15 years — set a calendar reminder
