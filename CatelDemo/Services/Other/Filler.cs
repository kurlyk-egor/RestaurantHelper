using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Collections;
using RestaurantHelper.DAL;
using RestaurantHelper.Models.Additional;

namespace RestaurantHelper.Services.Other
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
