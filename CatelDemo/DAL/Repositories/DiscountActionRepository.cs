using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models.Actions;

namespace RestaurantHelper.DAL.Repositories
{
	class DiscountActionRepository : IRepository<DiscountAction>
	{
		private readonly RestaurantDbContext _db;

		public DiscountActionRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<DiscountAction> GetAll()
		{
			return _db.DiscountActions;
		}

		public DiscountAction GetById(int id)
		{
			return _db.DiscountActions.Find(id);
		}

		public void Insert(DiscountAction item)
		{
			_db.DiscountActions.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.DiscountActions.Find(id);
			if (item != null) _db.DiscountActions.Remove(item);
		}

		public void Update(DiscountAction item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
