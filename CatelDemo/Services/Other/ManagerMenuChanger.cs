using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Catel.Collections;
using Microsoft.Win32;
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

		public string GetPicturePath()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Multiselect = false;
			dialog.Filter = "JPG image (*.jpg)|*.jpg";

			if (dialog.ShowDialog() == true)
			{
				return dialog.FileName;
			}

			return string.Empty;
		}

		public void AddNewDish(Dish dish)
		{
			_dishesRepository.Insert(dish);
			_dishesRepository.SaveChanges();
		}

		public void DeleteDish(Dish dish)
		{
			_dishesRepository.Delete(dish);
			_dishesRepository.SaveChanges();
		}

		public void EditDish(Dish dish)
		{
			_dishesRepository.Update(dish);
			_dishesRepository.SaveChanges();
		}
	}
}
