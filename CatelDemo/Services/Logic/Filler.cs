using Catel.Collections;
using RestaurantHelper.DAL;
using RestaurantHelper.Models.Additional;

namespace RestaurantHelper.Services.Logic
{
	static class Filler<T> where T : MyModelBase
	{
		/// <summary>
		/// Возвращает FastObservableCollection типа параметра
		/// </summary>
		/// <param name="collection">Коллекция для обновления</param>
		/// <param name="repository">Cсылка на репозиторий нужного класса, которая берется из экземпляра UOW</param>
		/// <returns></returns>
		public static void Fill(FastObservableCollection<T> collection, IRepository<T> repository)
		{
			collection.Clear();
			collection.AddItems(repository.GetAll());
		} 
	}
}
