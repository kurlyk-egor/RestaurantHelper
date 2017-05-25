using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models;

namespace RestaurantHelper.DAL.Repositories
{
	class TableRepository : IRepository<Table>
	{
		private readonly RestaurantDbContext _db;

		public TableRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<Table> GetAll()
		{
			return _db.Tables;
		}

		public Table GetById(int id)
		{
			return _db.Tables.Find(id);
		}

		public void Insert(Table item)
		{
			_db.Tables.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.Tables.Find(id);
			if (item != null) _db.Tables.Remove(item);
		}

		public void Update(Table item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
