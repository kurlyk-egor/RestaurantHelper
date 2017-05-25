using System.Collections.Generic;

namespace RestaurantHelper.DAL
{
	public interface IRepository<T> where T : class
	{
		IEnumerable<T> GetAll();
		T GetById(int id);
		void Insert(T item);
		void Update(T item);
		void Delete(int id);
	}
}