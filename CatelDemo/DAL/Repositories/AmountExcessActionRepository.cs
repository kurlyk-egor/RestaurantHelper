using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models.Actions;

namespace RestaurantHelper.DAL.Repositories
{
	class AmountExcessActionRepository : IRepository<AmountExcessAction>
	{
		private readonly RestaurantDbContext _db;

		public AmountExcessActionRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<AmountExcessAction> GetAll()
		{
			return _db.AmountExcessActions;
		}

		public AmountExcessAction GetById(int id)
		{
			return _db.AmountExcessActions.Find(id);
		}

		public void Insert(AmountExcessAction item)
		{
			_db.AmountExcessActions.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.AmountExcessActions.Find(id);
			if (item != null) _db.AmountExcessActions.Remove(item);
		}

		public void Update(AmountExcessAction item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
