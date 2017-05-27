using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models.Actions;

namespace RestaurantHelper.DAL.Repositories
{
	class BonusActionRepository : IRepository<BonusAction>
	{
		private readonly RestaurantDbContext _db;

		public BonusActionRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<BonusAction> GetAll()
		{
			return _db.BonusActions;
		}

		public BonusAction GetById(int id)
		{
			return _db.BonusActions.Find(id);
		}

		public void Insert(BonusAction item)
		{
			_db.BonusActions.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.BonusActions.Find(id);
			if (item != null) _db.BonusActions.Remove(item);
		}

		public void Update(BonusAction item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
