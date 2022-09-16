using Microsoft.EntityFrameworkCore;
using OrderReceive.DBContexts;
using OrderReceive.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrderReceive.Repositories
{
    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private readonly DbContextOptions<OrderReceiveDbContext> orderReceiveDbContext;

        public OrderHeaderRepository(DbContextOptions<OrderReceiveDbContext> orderReceiveDbContext)
        {
            this.orderReceiveDbContext = orderReceiveDbContext;
        }
        public async void AddOrder(OrderHeader orderHeader)
        {
            using (var _orderReceiveDbContext = new OrderReceiveDbContext(orderReceiveDbContext))
            {
                OrderHeader orderExists = await _orderReceiveDbContext.OrderHeader.Where(x => x.Order_Code == orderHeader.Order_Code).FirstOrDefaultAsync();
                if (orderExists != null)
                {
                    orderExists.UpdatedBy = "QCUpdate";
                    orderExists.UpdatedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    orderExists.UpdatedTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
                    orderExists.Order_Status = orderHeader.Order_Status;

                    _orderReceiveDbContext.OrderHeader.Update(orderHeader);
                }
                else
                    _orderReceiveDbContext.Add(orderHeader);
            }
        }

        public async Task ClearLines(string orderCode)
        {
            using (var _orderReceiveDbContext = new OrderReceiveDbContext(orderReceiveDbContext))
            {
                var linesToClear = _orderReceiveDbContext.OrderLines.Where(x => x.Order_Code == orderCode);
                _orderReceiveDbContext.OrderLines.RemoveRange(linesToClear);
                await SaveChanges();
            }
        }

        public async Task<OrderHeader> GetOrderByID(string OrderCode)
        {
            using (var _orderReceiveDbContext = new OrderReceiveDbContext(orderReceiveDbContext))
            {
                return await _orderReceiveDbContext.OrderHeader.Include("OrderLines").
                Where(x => x.Order_Code == OrderCode).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> OrderExists(string orderCode)
        {
            using (var _orderReceiveDbContext = new OrderReceiveDbContext(orderReceiveDbContext))
            {
                return await _orderReceiveDbContext.OrderHeader.AnyAsync(x => x.Order_Code == orderCode);
            }
        }

        public async Task<bool> SaveChanges()
        {
            using (var _orderReceiveDbContext = new OrderReceiveDbContext(orderReceiveDbContext))
            {
                return (await _orderReceiveDbContext.SaveChangesAsync() > 0);
            }
        }
    }
}