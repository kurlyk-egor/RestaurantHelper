using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models.Reviews;

namespace RestaurantHelper.DAL.Repositories
{
	class ClientReviewRepository : IRepository<ClientReview>
	{
		private readonly RestaurantDbContext _db;

		public ClientReviewRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<ClientReview> GetAll()
		{
			return _db.ClientReviews;
		}

		public ClientReview GetById(int id)
		{
			return _db.ClientReviews.Find(id);
		}

		public void Insert(ClientReview item)
		{
			_db.ClientReviews.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.ClientReviews.Find(id);
			if (item != null) _db.ClientReviews.Remove(item);
		}

		public void Update(ClientReview item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
