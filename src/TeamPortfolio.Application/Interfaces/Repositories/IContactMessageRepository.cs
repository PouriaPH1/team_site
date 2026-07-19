using TeamPortfolio.Domain.Entities;

namespace TeamPortfolio.Application.Interfaces.Repositories;

public interface IContactMessageRepository : IBaseRepository<ContactMessage>
{
    Task<IEnumerable<ContactMessage>> GetUnreadAsync();
}
