using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models;

namespace RestaurantHelper.DAL.Repositories
{
	class ReservationRepository : IRepository<Reservation>
	{
		private readonly RestaurantDbContext _db;

		public ReservationRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<Reservation> GetAll()
		{
			return _db.Reservations;
		}

		public Reservation GetById(int id)
		{
			return _db.Reservations.Find(id);
		}

		public void Insert(Reservation item)
		{
			_db.Reservations.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.Reservations.Find(id);
			if (item != null) _db.Reservations.Remove(item);
		}

		public void Update(Reservation item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
