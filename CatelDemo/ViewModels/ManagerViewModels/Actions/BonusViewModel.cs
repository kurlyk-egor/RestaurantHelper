using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Catel.Collections;
using Catel.Data;
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Services.Logic;

namespace RestaurantHelper.ViewModels.ManagerViewModels.Actions
{
	using Catel.MVVM;
	using System.Threading.Tasks;

	public class BonusViewModel : ViewModelBase
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();

		public BonusViewModel()
		{
			Dishes.Clear();
			Dishes.AddItems(_unitOfWork.Dishes.GetAll());

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
			var root = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();

			if (SelectedDish == null)
			{
				root.ChangePageWithDialog(new ShortMessageViewModel("Блюдо не выбрано!"), 999);
				return;
			}

			ActionsHelper actionsFilter = new ActionsHelper();
			string message;
			var bonus = new BonusAction
			{
				DishId = SelectedDish.Id,
				ExcessSum = DiscountValue,
				Name = ActionName,
				Description = ActionInfo
			};

			if (!actionsFilter.CanAddAction(bonus, out message))
			{
				root.ChangePageWithDialog(new ShortMessageViewModel(message), 1300);
			}
			else
			{
				root.ChangePageWithDialog(new ShortMessageViewModel("Акция успешно добавлена!"), 1111);
				actionsFilter.SaveAction(bonus);
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
	}
}
