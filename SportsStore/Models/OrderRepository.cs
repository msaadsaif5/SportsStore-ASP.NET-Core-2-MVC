using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Order> Orders { get => context.Orders.Include(o=>o.Lines).ThenInclude(l=>l.Product); }

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0)
                context.Orders.Add(order);
            context.SaveChanges();
        }
    }
}
