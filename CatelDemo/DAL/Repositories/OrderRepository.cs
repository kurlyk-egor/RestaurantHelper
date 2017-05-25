using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models;

namespace RestaurantHelper.DAL.Repositories
{
	class OrderRepository : IRepository<Order>
	{
		private readonly RestaurantDbContext _db;

		public OrderRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<Order> GetAll()
		{
			return _db.Orders;
		}

		public Order GetById(int id)
		{
			return _db.Orders.Find(id);
		}

		public void Insert(Order item)
		{
			_db.Orders.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.Orders.Find(id);
			if (item != null) _db.Orders.Remove(item);
		}

		public void Update(Order item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
