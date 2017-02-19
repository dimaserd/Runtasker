using Logic.Extensions.Repositories.Interfaces;
using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace Runtasker.Logic.Repositories
{
    public class OrderRepository : BaseRepository, IRepository<Order>, IRepositoryExtensive<Order>
    {
        #region Constructors
        public OrderRepository(MyDbContext context) : base(context)
        {
            
        }
        #endregion

        #region IRepository Methods

        public IEnumerable<Order> GetList()
        {
            return db.Orders;
        }

        public Order GetItem(int id)
        {
            return db.Orders.Find(id);
        }

        public void Add(Order order)
        {
            db.Orders.Add(order);
        }

        public void Update(Order order)
        {
            db.Entry(order).State = EntityState.Modified;
        }

        public void Remove(int id)
        {
            Order order = db.Orders.Find(id);
            if (order != null)
            {
                db.Orders.Remove(order);
            }
        }

        #endregion

        #region IRepositoryExtensive Methods
        public void RemoveRange(IEnumerable<Order> items)
        {
            db.Orders.RemoveRange(items);
        }

        public void AddRange(IEnumerable<Order> items)
        {
            db.Orders.AddRange(items);
        }
        #endregion
    }
}
