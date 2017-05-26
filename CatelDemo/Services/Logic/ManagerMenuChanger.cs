using Catel.Collections;
using Microsoft.Win32;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Logic
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
