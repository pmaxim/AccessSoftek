using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Domain.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<EfDbContext>(options =>
                    options.UseSqlServer(Constants.DefaultConnection))

                .AddSingleton<IOrderItemRepository, EfOrderItemRepository>()
                .AddSingleton<IOrderRepository, EfOrderRepository>()
                .BuildServiceProvider();

            var repoOrder = serviceProvider.GetService<IOrderRepository>();
            var repoOrderItem = serviceProvider.GetService<IOrderItemRepository>();

            //LINQ
            ClearBd(repoOrder, repoOrderItem);
            CreateTestItemsBd(repoOrder, repoOrderItem);
            await ChangeOrderId(repoOrder, repoOrderItem);

            //SQL
            ClearBd(repoOrder, repoOrderItem);
            CreateTestItemsBd(repoOrder, repoOrderItem);
            ChangeOrderIdSql(repoOrder);
        }

        private static void ChangeOrderIdSql(IOrderRepository repoOrder)
        {
            var clone = @"INSERT INTO Orders (Id, Name)
                          SELECT 2, Name
                          FROM Orders
                          WHERE id = 1
                          UPDATE OrderItems
                          SET OrderId=2
                          DELETE FROM Orders
                          WHERE Id=1";
            repoOrder.ExecuteSqlCommand(clone);
        }

        private static async Task ChangeOrderId(IOrderRepository repoOrder, IOrderItemRepository repoOrderItem)
        {
            var order = await repoOrder.Orders.AsNoTracking().FirstAsync();
            order.Id = 2;
            repoOrder.Create(order);
            var list = await repoOrderItem.OrderItems.ToListAsync();
            foreach (var p in list)
            {
                p.Order = order;
            }
            order = await repoOrder.Orders.FirstAsync();
            repoOrder.Remove(order);
            await repoOrder.SaveChangesAsync();
        }

        private static void CreateTestItemsBd(IOrderRepository repoOrder, IOrderItemRepository repoOrderItem)
        {
            var order = new Order {Id = 1, Name = Guid.NewGuid().ToString("n").Substring(0, 8)};
            repoOrder.Create(order);
            for (var i = 0; i < 4; i++)
            {
                repoOrderItem.Create(new OrderItem
                {
                    Name = Guid.NewGuid().ToString("n").Substring(0, 8),
                    Order = order
                });
            }
            repoOrder.SaveChanges();
        }

        private static void ClearBd(IOrderRepository repoOrder, IOrderItemRepository repoOrderItem)
        {
            if (repoOrderItem.OrderItems.Any())
            {
                var list = repoOrderItem.OrderItems.ToList();
                foreach (var p in list)
                {
                    repoOrderItem.Remove(p);
                }
            }
            if (repoOrder.Orders.Any())
            {
                var list = repoOrder.Orders.ToList();
                foreach (var p in list)
                {
                    repoOrder.Remove(p);
                }
            }
            repoOrder.SaveChanges();
        }
    }
}
