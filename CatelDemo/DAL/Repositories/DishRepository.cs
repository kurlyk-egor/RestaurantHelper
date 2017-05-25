using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models;

namespace RestaurantHelper.DAL.Repositories
{
	class DishRepository : IRepository<Dish>
	{
		private readonly RestaurantDbContext _db;

		public DishRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<Dish> GetAll()
		{
			return _db.Dishes;
		}

		public Dish GetById(int id)
		{
			return _db.Dishes.Find(id);
		}

		public void Insert(Dish item)
		{
			_db.Dishes.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.Dishes.Find(id);
			if (item != null) _db.Dishes.Remove(item);
		}

		public void Update(Dish item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
