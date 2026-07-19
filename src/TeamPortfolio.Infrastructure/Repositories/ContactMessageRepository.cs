using Microsoft.EntityFrameworkCore;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Domain.Entities;
using TeamPortfolio.Infrastructure.Data;

namespace TeamPortfolio.Infrastructure.Repositories;

public class ContactMessageRepository : BaseRepository<ContactMessage>, IContactMessageRepository
{
    public ContactMessageRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ContactMessage>> GetUnreadAsync()
        => await _dbSet
            .Where(m => !m.IsRead)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
}
