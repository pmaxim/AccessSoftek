using Domain.Entities;
using System.Linq;

namespace Domain.Repositories.Concrete
{
    public class EfOrderItemRepository : EfBaseRepository<OrderItem>, IOrderItemRepository
    {
        public IQueryable<OrderItem> OrderItems => Context.OrderItems;

        public EfOrderItemRepository(EfDbContext db) : base(db)
        {
        }
    }
}