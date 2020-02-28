using Domain.Entities;
using System.Linq;

namespace Domain.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        IQueryable<Order> Orders { get; }
    }
}