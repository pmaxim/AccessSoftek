using Domain.Entities;
using System.Linq;

namespace Domain.Repositories
{
    public interface IOrderItemRepository : IBaseRepository<OrderItem>
    {
        IQueryable<OrderItem> OrderItems { get; }
    }
}