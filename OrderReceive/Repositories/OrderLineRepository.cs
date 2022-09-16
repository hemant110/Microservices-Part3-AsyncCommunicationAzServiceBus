using Microsoft.EntityFrameworkCore;
using OrderReceive.DBContexts;
using OrderReceive.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderReceive.Repositories
{
    public class OrderLineRepository : IOrderLinesRepository
    {
        private readonly DbContextOptions<OrderReceiveDbContext> orderReceiveDbContext;
        public OrderLineRepository(DbContextOptions<OrderReceiveDbContext> orderReceiveDbContext)
        {
            this.orderReceiveDbContext = orderReceiveDbContext;
        }
        public async Task<OrderLines> AddOrUpdateOrderLine(string orderCode, OrderLines orderLine)
        {

            using (var _orderReceiveDbContext = new OrderReceiveDbContext(orderReceiveDbContext))
            {
                var existingLine = await _orderReceiveDbContext.OrderLines.Where(x => x.Order_Code == orderCode
            && x.ProductCode == orderLine.ProductCode).FirstOrDefaultAsync();

                if (existingLine == null)
                {
                    orderLine.Order_Code = orderCode;
                    _orderReceiveDbContext.OrderLines.Add(orderLine);
                    return orderLine;
                }
                existingLine.Unit = orderLine.Unit;
                existingLine.QtyAllocated = orderLine.QtyAllocated;
                existingLine.UpdatedBy = "User1";
                existingLine.UpdatedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                existingLine.UpdatedTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
                _orderReceiveDbContext.Entry(existingLine).State = EntityState.Modified;
                _orderReceiveDbContext.OrderLines.Update(existingLine);
                await _orderReceiveDbContext.SaveChangesAsync();
                return existingLine;
            }
        }

        public async Task<IEnumerable<OrderLines>> GetOrderLines(string orderCode)
        {
            using (var _orderReceiveDbContext = new OrderReceiveDbContext(orderReceiveDbContext))
            {
                return (await _orderReceiveDbContext.OrderLines.//Inclide(o=>o.Product)
                Where(x => x.Order_Code == orderCode).ToListAsync());
            }
        }

        public async Task<OrderLines> GetOrderLinesById(string lineID)
        {
            using (var _orderReceiveDbContext = new OrderReceiveDbContext(orderReceiveDbContext))
            {
                return await _orderReceiveDbContext.OrderLines.
                Where(x => x.LineId == lineID).FirstOrDefaultAsync();
            }
        }

        public void RemoveOrderLine(OrderLines orderLine)
        {
            using (var _orderReceiveDbContext = new OrderReceiveDbContext(orderReceiveDbContext))
            {
                _orderReceiveDbContext.OrderLines.Remove(orderLine);
                _orderReceiveDbContext.SaveChanges();
            }
        }

        public async Task<bool> SaveChanges()
        {
            using (var _orderReceiveDbContext = new OrderReceiveDbContext(orderReceiveDbContext))
            {
                return (await _orderReceiveDbContext.SaveChangesAsync() > 0);
            }
        }

        public void UpdateOrderLine(OrderLines orderLine)
        {
            throw new NotImplementedException();
        }
    }
}
