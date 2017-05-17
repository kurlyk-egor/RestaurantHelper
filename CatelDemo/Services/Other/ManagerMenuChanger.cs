using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Collections;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Services.Other
{
	class ManagerMenuChanger
	{
		private readonly IRepository<Dish> _dishesRepository;

		public ManagerMenuChanger()
		{
			_dishesRepository = new Repository<Dish>();
		}

		public ObservableCollection<Dish> LoadAllDishes()
		{
			ObservableCollection<Dish> dishes = new ObservableCollection<Dish>();
			((ICollection<Dish>)dishes).AddRange(_dishesRepository.GetCollection());
			return dishes;
		}
	}
}
