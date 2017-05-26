using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RestaurantHelper.Models;

namespace RestaurantHelper.DAL.Repositories
{
	class UserRepository : IRepository<User>
	{
		private readonly RestaurantDbContext _db;

		public UserRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<User> GetAll()
		{
			return _db.Users;
		}

		public User GetById(int id)
		{
			return _db.Users.Find(id);
		}

		public void Insert(User item)
		{
			_db.Users.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.Users.Find(id);
			if(item != null) _db.Users.Remove(item);
		}

		public void Update(User item)
		{
			try
			{
				_db.Entry(item).State = EntityState.Modified;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		public bool IsExistLogin(string login)
		{
			return _db.Users.Any(u => u.Login == login);
		}
	}
}
