using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Catel.Collections;
using Catel.Data;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Interfaces;
using RestaurantHelper.Services.Other;

namespace RestaurantHelper.ViewModels.ManagerViewModels.Actions
{
	using Catel.MVVM;
	using System.Threading.Tasks;

	public class DiscountViewModel : ViewModelBase
	{
		private readonly DiscountAction _discountAction;
		public DiscountViewModel()
		{
			IRepository<Dish> dishesRepository = new Repository<Dish>();
			Dishes.Clear();
			((ICollection<Dish>) Dishes).AddRange(dishesRepository.GetCollection());

			_discountAction = new DiscountAction();
			ApplyAction = new Command(OnApplyActionExecute);
		}

		public ObservableCollection<Dish> Dishes
		{
			get { return GetValue<ObservableCollection<Dish>>(DishesProperty); }
			set { SetValue(DishesProperty, value); }
		}
		public static readonly PropertyData DishesProperty = RegisterProperty("Dishes", typeof(ObservableCollection<Dish>),
			new ObservableCollection<Dish>());

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
		public static readonly PropertyData DiscountValueProperty = RegisterProperty("DiscountValue", typeof(int), 10);

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
		private void OnApplyActionExecute()
		{
			FillDiscountAction();
			ActionsHelper actionsFilter = new ActionsHelper();
			string message;
			if (!actionsFilter.CanAddAction(_discountAction, out message))
			{
				MessageBox.Show(message);
			}
			else
			{
				actionsFilter.SaveAction(_discountAction);
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

		private void FillDiscountAction()
		{
			_discountAction.DishId = SelectedDish.Id;
			_discountAction.DiscountSum = DiscountValue;
			_discountAction.Name = ActionName;
			_discountAction.Description = ActionInfo;
		}
	}
}
