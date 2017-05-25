using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Catel.Collections;
using Catel.Data;
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Services.Other;

namespace RestaurantHelper.ViewModels.ManagerViewModels.Actions
{
	using Catel.MVVM;
	using System.Threading.Tasks;

	public class AmountExcessViewModel : ViewModelBase
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private readonly AmountExcessAction _amountExcessAction;
		public AmountExcessViewModel()
		{
			Dishes.Clear();
			Dishes.AddItems(_unitOfWork.Dishes.GetAll());

			_amountExcessAction = new AmountExcessAction();
			ApplyAction = new Command(OnApplyActionExecute, OnApplyActionCanExecute);
		}

		public FastObservableCollection<Dish> Dishes
		{
			get { return GetValue<FastObservableCollection<Dish>>(DishesProperty); }
			set { SetValue(DishesProperty, value); }
		}
		public static readonly PropertyData DishesProperty = RegisterProperty("Dishes", typeof(FastObservableCollection<Dish>),
			new FastObservableCollection<Dish>());

		public Dish SelectedDish
		{
			get { return GetValue<Dish>(SelectedDishProperty); }
			set { SetValue(SelectedDishProperty, value); }
		}
		public static readonly PropertyData SelectedDishProperty = RegisterProperty("SelectedDish", typeof(Dish));

		public int DiscountValue
		{
			get { return GetValue<int>(DiscountValueProperty); }
			set { SetValue(DiscountValueProperty, value); }
		}
		public static readonly PropertyData DiscountValueProperty = RegisterProperty("DiscountValue", typeof(int), 100);

		public string ActionName
		{
			get { return GetValue<string>(ActionNameProperty); }
			set { SetValue(ActionNameProperty, value); }
		}
		public static readonly PropertyData ActionNameProperty = RegisterProperty("ActionName", typeof(string));


		public string ActionInfo
		{
			get { return GetValue<string>(ActionInfoProperty); }
			set { SetValue(ActionInfoProperty, value); }
		}
		public static readonly PropertyData ActionInfoProperty = RegisterProperty("ActionInfo", typeof(string));


		public Command ApplyAction { get; private set; }
		private bool OnApplyActionCanExecute()
		{
			return SelectedDish != null;
		}
		private void OnApplyActionExecute()
		{
			if (SelectedDish == null)
			{
				MessageBox.Show("Не выбрано блюдо!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			FillAmounExcessAction();
			ActionsHelper actionsFilter = new ActionsHelper();
			string message;
			if (!actionsFilter.CanAddAction(_amountExcessAction, out message))
			{
				MessageBox.Show(message);
			}
			else
			{
				actionsFilter.SaveAction(_amountExcessAction);
			}
		}
		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}

		private void FillAmounExcessAction()
		{
			_amountExcessAction.DishId = SelectedDish.Id;
			_amountExcessAction.ExcessSum = DiscountValue;
			_amountExcessAction.Name = ActionName;
			_amountExcessAction.Description = ActionInfo;
		}
	}
}
