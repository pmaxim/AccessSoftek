using Domain.Entities;
using System.Linq;

namespace Domain.Repositories.Concrete
{
    public class EfOrderRepository : EfBaseRepository<Order>, IOrderRepository
    {
        public IQueryable<Order> Orders => Context.Orders;

        public EfOrderRepository(EfDbContext db) : base(db)
        {
        }
    }
}
