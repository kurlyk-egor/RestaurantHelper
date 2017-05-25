using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models;

namespace RestaurantHelper.DAL.Repositories
{
	class EmployeeRepository : IRepository<Employee>
	{
		private readonly RestaurantDbContext _db;

		public EmployeeRepository(RestaurantDbContext context)
		{
			_db = context;
		}

		public IEnumerable<Employee> GetAll()
		{
			return _db.Employees;
		}

		public Employee GetById(int id)
		{
			return _db.Employees.Find(id);
		}

		public void Insert(Employee item)
		{
			_db.Employees.Add(item);
		}

		public void Delete(int id)
		{
			var item = _db.Employees.Find(id);
			if (item != null) _db.Employees.Remove(item);
		}

		public void Update(Employee item)
		{
			_db.Entry(item).State = EntityState.Modified;
		}
	}
}
