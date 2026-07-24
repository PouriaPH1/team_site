<div dir="rtl">

# راهنمای دیپلوی — Team Portfolio

## مشخصات پروژه

| آیتم | مقدار |
|------|-------|
| Framework | ASP.NET Core 9 (MVC) |
| Database | SQLite |
| Auth | ASP.NET Identity |
| Web Server | Nginx + Kestrel |
| Process Manager | systemd |
| SSL | Cloudflare Origin Certificate یا Let's Encrypt |

---

## پیش‌نیازهای سرور

- Ubuntu 22.04 LTS
- حداقل 1 vCPU، 1GB RAM، 20GB دیسک
- دسترسی root به سرور

---

## مرحله ۱ — آپدیت سرور

```bash
ssh root@YOUR_SERVER_IP
apt update && apt upgrade -y
```

---

## مرحله ۲ — نصب .NET 9 SDK

```bash
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

apt update
apt install -y dotnet-sdk-9.0

# تأیید نصب
dotnet --version   # باید 9.x.x نشون بده
```

---

## مرحله ۳ — نصب Git و Nginx

```bash
apt install -y git nginx
systemctl enable nginx
systemctl start nginx
```

---

## مرحله ۴ — ساخت یوزر اختصاصی (امنیت)

```bash
useradd -m -s /bin/bash teamportfolio
```

---

## مرحله ۵ — کلون پروژه از GitHub

```bash
mkdir -p /var/www/teamportfolio
cd /var/www/teamportfolio

# اگه repo پابلیک باشه:
git clone https://github.com/YOUR_USERNAME/YOUR_REPO.git .

# اگه repo پرایوت باشه — با SSH key (توصیه‌شده):
ssh-keygen -t ed25519 -C "server@teamportfolio" -f ~/.ssh/github_deploy -N ""
cat ~/.ssh/github_deploy.pub
# این کلید رو در GitHub > Settings > Deploy Keys اضافه کن
GIT_SSH_COMMAND='ssh -i ~/.ssh/github_deploy' git clone git@github.com:YOUR_USERNAME/YOUR_REPO.git .
```

> **نکته:** اگه clone داخل پوشه‌ای ساخت (مثلاً `team_site`)، وارد اون پوشه بشو:
> ```bash
> cd /var/www/teamportfolio/team_site
> ```

---

## مرحله ۶ — Build و Publish

```bash
# از پوشه root پروژه (جایی که src/ وجود داره):
dotnet publish src/TeamPortfolio.Web/TeamPortfolio.Web.csproj \
  -c Release \
  -o /var/www/teamportfolio-app \
  --no-self-contained
```

خروجی موفق:
```
Build succeeded in ~67s
```

---

## مرحله ۷ — ساخت پوشه‌های لازم

این پوشه‌ها در git نیستن و باید روی سرور ساخته بشن:

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

## مرحله ۸ — تنظیم appsettings.Production.json

این فایل در git نیست و باید روی سرور ساخته بشه:

```bash
nano /var/www/teamportfolio-app/appsettings.Production.json
```

محتوا:

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

## مرحله ۹ — اجرای Database Migrations

```bash
dotnet tool install --global dotnet-ef
export PATH="$PATH:$HOME/.dotnet/tools"

# از پوشه source پروژه:
dotnet ef database update \
  --project src/TeamPortfolio.Infrastructure/TeamPortfolio.Infrastructure.csproj \
  --startup-project src/TeamPortfolio.Web/TeamPortfolio.Web.csproj \
  --connection "Data Source=/var/www/teamportfolio-app/data/teamportfolio.db"

# تأیید ساخته شدن db
ls -la /var/www/teamportfolio-app/data/
```

---

## مرحله ۱۰ — سرویس systemd

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

# بررسی وضعیت
systemctl status teamportfolio

# تست مستقیم
curl -H "Host: yourdomain.com" http://localhost:5001
```

---

## مرحله ۱۱ — SSL

### روش الف — Cloudflare Origin Certificate (اگه از Cloudflare استفاده می‌کنی)

**توی Cloudflare:**
1. SSL/TLS → Origin Server → **Create Certificate**
2. Hostnames: `*.yourdomain.com` یا دامنه خاص
3. Expiration: 15 years
4. **Create** — دو فایل بهت میده (cert + private key)

**روی سرور:**
```bash
mkdir -p /etc/nginx/ssl/teamportfolio

nano /etc/nginx/ssl/teamportfolio/cert.pem
# محتوای Origin Certificate رو paste کن

nano /etc/nginx/ssl/teamportfolio/key.pem
# محتوای Private Key رو paste کن

chmod 600 /etc/nginx/ssl/teamportfolio/key.pem
chmod 644 /etc/nginx/ssl/teamportfolio/cert.pem
```

> **نکته:** اگه روی سرور گواهی wildcard از قبل موجوده (مثلاً `/etc/ssl/yourdomain.pem`)، از همون استفاده کن.

### روش ب — Let's Encrypt (اگه از Cloudflare استفاده نمی‌کنی)

```bash
apt install -y certbot python3-certbot-nginx
certbot --nginx -d yourdomain.com -d www.yourdomain.com
certbot renew --dry-run  # تست تجدید خودکار
```

---

## مرحله ۱۲ — Nginx Config

```bash
nano /etc/nginx/sites-available/teamportfolio
```

```nginx
# HTTPS
server {
    listen 443 ssl;
    listen [::]:443 ssl;
    server_name yourdomain.com;

    # مسیر گواهی‌ها رو بر اساس روشی که انتخاب کردی تنظیم کن:
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

## مرحله ۱۳ — تنظیم DNS (Cloudflare)

| Type | Name | Content | Proxy |
|------|------|---------|-------|
| A | `subdomain` | `YOUR_SERVER_IP` | ✅ Proxied |

**SSL/TLS Settings:**
- Mode: **Full (strict)** — اگه از Origin Certificate استفاده می‌کنی
- Mode: **Full** — اگه از Let's Encrypt استفاده می‌کنی
- Always Use HTTPS: **روشن**

---

## مرحله ۱۴ — فایروال

```bash
ufw allow OpenSSH
ufw allow 'Nginx Full'
ufw enable
ufw status
```

---

## اسکریپت آپدیت (برای دیپلوی‌های بعدی)

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

# از این به بعد برای هر آپدیت:
deploy-teamportfolio.sh
```

---

## عیب‌یابی سریع

```bash
# لاگ اپ
journalctl -u teamportfolio -n 50 --no-pager

# لاگ nginx
tail -f /var/log/nginx/error.log

# وضعیت سرویس
systemctl status teamportfolio

# تست مستقیم اپ (بدون nginx)
curl -H "Host: yourdomain.com" http://localhost:5001

# چک پورت‌های باز
ss -tlnp | grep -E ':(80|443|5001)'
```

| خطا | علت احتمالی | راه حل |
|-----|-------------|---------|
| `Bad Request - Invalid Hostname` | AllowedHosts درست ست نشده | `appsettings.Production.json` رو چک کن |
| `502 Bad Gateway` | اپ داره crash می‌کنه | `journalctl -u teamportfolio` |
| `404 Not Found` | Nginx به اپ forward نمی‌کنه | nginx config رو چک کن |
| `SSL Error` | گواهی اشتباه یا منقضی | مسیر cert در nginx config |

---

## نمودار معماری

```
کاربر
  │
  │ HTTPS (443)
  ▼
Cloudflare (SSL Termination + CDN)
  │
  │ HTTPS (443) با Origin Certificate
  ▼
Nginx روی سرور
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

## نکات مهم

- فایل `teamportfolio.db` در git نیست — **حتماً بک‌آپ بگیر**
- پوشه `wwwroot/uploads/` در git نیست — بک‌آپ جداگانه لازم داره
- `appsettings.Production.json` در git نیست — روی سرور نگهداری می‌شه
- پسورد admin پیش‌فرض (`Admin@1234`) را بعد از اولین لاگین تغییر بده
- گواهی Cloudflare Origin Certificate هر ۱۵ سال یه بار نیاز به تجدید داره

</div>
