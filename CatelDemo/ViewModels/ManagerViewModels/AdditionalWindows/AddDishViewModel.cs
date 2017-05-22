using System;
using System.Threading.Tasks;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Other;

namespace RestaurantHelper.ViewModels.ManagerViewModels.AdditionalWindows
{
	public class AddDishViewModel : ViewModelBase
	{
		private readonly ManagerMenuChanger _menuChanger;

		//делегат, который выбирает, добавить или редактировать блюдо
		private readonly Action<Dish> _addOrEditAction;

		public AddDishViewModel(Dish dish = null)
		{
			_menuChanger = new ManagerMenuChanger();
			OkCommand = new Command(OnOkCommandExecute, OnOkCommandCanExecute);
			SelectPictureCommand = new Command(OnSelectPictureCommandExecute);

			if (dish == null)
			{
				_addOrEditAction = _menuChanger.AddNewDish;
				dish = new Dish();
			}
			Dish = dish;
			_addOrEditAction = _menuChanger.EditDish;
		}

		[Model]
		public Dish Dish
		{
			get { return GetValue<Dish>(DishProperty); }
			private set { SetValue(DishProperty, value); }
		}
		public static readonly PropertyData DishProperty = RegisterProperty("Dish", typeof(Dish));


		[ViewModelToModel("Dish")]
		public string Name
		{
			get { return GetValue<string>(NameProperty); }
			set { SetValue(NameProperty, value); }
		}
		public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string));


		[ViewModelToModel("Dish")]
		public int Price
		{
			get { return GetValue<int>(PriceProperty); }
			set { SetValue(PriceProperty, value); }
		}
		public static readonly PropertyData PriceProperty = RegisterProperty("Price", typeof(int));


		[ViewModelToModel("Dish")]
		public string PicturePath
		{
			get { return GetValue<string>(PicturePathProperty); }
			set { SetValue(PicturePathProperty, value); }
		}
		public static readonly PropertyData PicturePathProperty = RegisterProperty("PicturePath", typeof(string));



		public Command OkCommand { get; private set; }
		public Command SelectPictureCommand { get; private set; }

		private void OnSelectPictureCommandExecute()
		{
			PicturePath = _menuChanger.GetPicturePath();
		}

		private bool OnOkCommandCanExecute()
		{
			return !(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(PicturePath) || Price < 0);
		}
		private async void OnOkCommandExecute()
		{
			// вызываем делегат для редактирования или добавления блюда
			_addOrEditAction(Dish);
			await CloseViewModelAsync(true);
		}
		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}
	}
}
