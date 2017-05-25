using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models.Reviews;

namespace RestaurantHelper.DAL.Repositories
{
	class ManagerAnswerRepository : IRepository<ManagerAnswer>
	{
		private readonly RestaurantDbContext _db;

		public ManagerAnswerRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<ManagerAnswer> GetAll()
		{
			return _db.ManagerAnswers;
		}

		public ManagerAnswer GetById(int id)
		{
			return _db.ManagerAnswers.Find(id);
		}

		public void Insert(ManagerAnswer item)
		{
			_db.ManagerAnswers.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.ManagerAnswers.Find(id);
			if (item != null) _db.ManagerAnswers.Remove(item);
		}

		public void Update(ManagerAnswer item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
