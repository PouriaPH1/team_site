using TeamPortfolio.Domain.Entities;

namespace TeamPortfolio.Application.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<int> GetPostCountByCategoryAsync(int categoryId);
}
