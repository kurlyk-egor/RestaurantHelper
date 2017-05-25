using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models;

namespace RestaurantHelper.DAL.Repositories
{
	class OrderedDishRepository : IRepository<OrderedDish>
	{
		private readonly RestaurantDbContext _db;

		public OrderedDishRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<OrderedDish> GetAll()
		{
			return _db.OrderedDishes;
		}

		public OrderedDish GetById(int id)
		{
			return _db.OrderedDishes.Find(id);
		}

		public void Insert(OrderedDish item)
		{
			_db.OrderedDishes.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.OrderedDishes.Find(id);
			if (item != null) _db.OrderedDishes.Remove(item);
		}

		public void Update(OrderedDish item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
