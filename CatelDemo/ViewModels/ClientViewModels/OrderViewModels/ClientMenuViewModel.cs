using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Services.Logic;

namespace RestaurantHelper.ViewModels.ClientViewModels.OrderViewModels
{
    public class ClientMenuViewModel : ViewModelBase
    {
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private readonly User _user;
		private readonly IViewModel _rootViewModel;
	    private readonly Reservation _reservation;
		private readonly OrderedSumCalculator _sumCalculator;

		public ClientMenuViewModel(User user, Reservation reservation, FastObservableCollection<OrderedDish> orderedDishes = null)
	    {
			_user = user;
			_reservation = reservation;
		    _rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();
			OrderedDishes.AutomaticallyDispatchChangeNotifications = true;
			// передаем в конструктор ссылку на собираемую коллекцию заказанных блюд
			_sumCalculator = new OrderedSumCalculator(OrderedDishes);

			AddCommand = new Command(OnAddCommandExecute);
			SelectionChangedCommand = new Command(OnSelectionChangedCommandExecute);
			DeleteCommand = new Command(OnDeleteCommandExecute, OnDeleteCommandCanExecute);
			BackCommand = new Command(OnBackCommandExecute);
			NextCommand = new Command(OnNextCommandExecute, OnNextCommandCanExecute);

			RefreshDishesCollection();

			if (orderedDishes != null)
			{
				OrderedDishes = orderedDishes;
				TotalSum = _sumCalculator.GetCurrentOrderedSum();
			}
			else
			{
				OrderedDishes.Clear();
			}
	    }


		/// <summary>
		/// список блюд
		/// </summary>
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

		/// <summary>
		/// список заказанных в данный момент блюд
		/// </summary>
		public FastObservableCollection<OrderedDish> OrderedDishes
		{
			get { return GetValue<FastObservableCollection<OrderedDish>>(OrderedDishesProperty); }
			set { SetValue(OrderedDishesProperty, value); }
		}
		public static readonly PropertyData OrderedDishesProperty = RegisterProperty("OrderedDishes", typeof(FastObservableCollection<OrderedDish>), 
			new FastObservableCollection<OrderedDish>());

		public OrderedDish SelectedOrderedDish
		{
			get { return GetValue<OrderedDish>(SelectedOrderedDishProperty); }
			set { SetValue(SelectedOrderedDishProperty, value); }
		}
		public static readonly PropertyData SelectedOrderedDishProperty = RegisterProperty("SelectedOrderedDish", typeof(OrderedDish));

		/// <summary>
		/// итоговая сумма за заказ
		/// </summary>
		public int TotalSum
		{
			get { return GetValue<int>(TotalSumProperty); }
			set { SetValue(TotalSumProperty, value); }
		}
		public static readonly PropertyData TotalSumProperty = RegisterProperty("TotalSum", typeof(int), 0);

		public DiscountAction Discount
		{
			get { return GetValue<DiscountAction>(DiscountProperty); }
			set { SetValue(DiscountProperty, value); }
		}
		public static readonly PropertyData DiscountProperty = RegisterProperty("Discount", typeof(DiscountAction));

		public bool IsVisibleActionInfo
		{
			get { return GetValue<bool>(IsVisibleActionInfoProperty); }
			set { SetValue(IsVisibleActionInfoProperty, value); }
		}
		public static readonly PropertyData IsVisibleActionInfoProperty = RegisterProperty("IsVisibleActionInfo", typeof(bool), false);


	    public Command AddCommand { get; private set; }
		private void OnAddCommandExecute()
		{
			_sumCalculator.AddDishIntoOrderedDishes(SelectedDish);
			TotalSum = _sumCalculator.GetCurrentOrderedSum();
		}

		public Command SelectionChangedCommand { get; private set; }
		private void OnSelectionChangedCommandExecute()
		{
			IsVisibleActionInfo = SelectedDish.IsDiscounted;
			if (IsVisibleActionInfo)
			{
				Discount = _unitOfWork.DiscountActions.GetAll().FirstOrDefault(d => d.DishId == SelectedDish.Id);
			}
		}

		public Command DeleteCommand { get; private set; }
		private bool OnDeleteCommandCanExecute()
		{
			return SelectedOrderedDish != null;
		}
		private void OnDeleteCommandExecute()
		{
			OrderedDishes.Remove(SelectedOrderedDish);
			TotalSum = _sumCalculator.GetCurrentOrderedSum();
		}


		public Command BackCommand { get; private set; }
		private void OnBackCommandExecute()
		{
			_rootViewModel.ChangePage(new ClientHallViewModel(_user, _reservation, OrderedDishes));
		}

		public Command NextCommand { get; private set; }
		private bool OnNextCommandCanExecute()
		{
			return OrderedDishes.Any();
		}
		private void OnNextCommandExecute()
		{
			_rootViewModel.ChangePage(new ClientTotalsViewModel(_user, _reservation, OrderedDishes));
		}

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }

		private void RefreshDishesCollection()
		{
			Dishes.Clear();
			Dishes.AddItems(_unitOfWork.Dishes.GetAll());
			new ActionsHelper().CalculateDiscountsExisting(Dishes);
		}
	}
}
