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
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Other
{
	class ManagerMenuChanger
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();

		public FastObservableCollection<Dish> LoadAllDishes()
		{
			FastObservableCollection<Dish> dishes = new FastObservableCollection<Dish>();
			dishes.AddItems(_unitOfWork.Dishes.GetAll());
			return dishes;
		}

		public string GetPicturePath()
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				Multiselect = false,
				Filter = "JPG image (*.jpg)|*.jpg"
			};

			if (dialog.ShowDialog() == true)
			{
				return dialog.FileName;
			}

			return string.Empty;
		}

		public void AddNewDish(Dish dish)
		{
			_unitOfWork.Dishes.Insert(dish);
			_unitOfWork.SaveChanges();
		}

		public void DeleteDish(Dish dish)
		{
			_unitOfWork.Dishes.Delete(dish.Id);
			_unitOfWork.SaveChanges();
		}

		public void EditDish(Dish dish)
		{
			_unitOfWork.Dishes.Update(dish);
			_unitOfWork.SaveChanges();
		}
	}
}
