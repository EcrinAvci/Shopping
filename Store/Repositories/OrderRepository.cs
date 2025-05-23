using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;


namespace Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext context) : base(context)
        {

        }

        public IQueryable<Order> Orders => _context.Orders
            .Include(o => o.Lines)
            .ThenInclude(c1 =>c1.Product)
            .OrderBy(o => o.Shipped)
            .ThenByDescending(o => o.OrderId);

        public int NumberOfInProcess => _context.Orders.Count(o =>o.Shipped.Equals(false));

        public void Complete(int id)
        {
            var order= FindByCondition(o=> o.OrderId.Equals(id),true);
            if(order is null)
                throw new Exception("Order could not found!");
            order.Shipped= true;
            
        }

        public Order? GetOneOrder(int id)
        {
           return FindByCondition(o => o.OrderId.Equals(id),false);
        }

        public void SaveOrder(Order order)
        {
            if(order.OrderId == 0)
            {
                _context.Orders.Add(order);
                foreach(var line in order.Lines)
                {
                    _context.Entry(line.Product).State = EntityState.Unchanged;
                }
            }
            _context.SaveChanges();
        }
    }
}
