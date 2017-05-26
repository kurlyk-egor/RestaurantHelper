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
			_sumCalculator = new OrderedSumCalculator();
		    _rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();

			AddCommand = new Command(OnAddCommandExecute, OnAddCommandCanExecute);
			DeleteCommand = new Command(OnDeleteCommandExecute, OnDeleteCommandCanExecute);
			BackCommand = new Command(OnBackCommandExecute);
			NextCommand = new Command(OnNextCommandExecute, OnNextCommandCanExecute);

			RefreshDishesCollection();

			if (orderedDishes != null)
			{
				OrderedDishes = orderedDishes;
				//TODO: change to another call
				TotalSum = _sumCalculator.GetCurrentOrderedSum(OrderedDishes);
			}
			else
			{
				OrderedDishes.Clear();
			}
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

		public int CurrentDishesCount
		{
			get { return GetValue<int>(CurrentDishesCountProperty); }
			set { SetValue(CurrentDishesCountProperty, value); }
		}
		public static readonly PropertyData CurrentDishesCountProperty = RegisterProperty("CurrentDishesCount", typeof(int), 1);

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

		public int TotalSum
		{
			get { return GetValue<int>(TotalSumProperty); }
			set { SetValue(TotalSumProperty, value); }
		}
		public static readonly PropertyData TotalSumProperty = RegisterProperty("TotalSum", typeof(int), 0);


	    public Command AddCommand { get; private set; }
		private bool OnAddCommandCanExecute()
		{
			return SelectedDish != null;
		}
		private void OnAddCommandExecute()
		{
			// добавить выбранное блюдо
			var dish = OrderedDishes.FirstOrDefault(cur => cur.Id == SelectedDish.Id);

			if (dish != null)
			{
				// TODO: костыль, чтобы оповестить вью, что во вьюмодели данные изменились
				OrderedDishes.Remove(dish);
				OrderedDishes.Add(dish);
				dish.Quantity += CurrentDishesCount;
			}
			else
			{
				SelectedDish.Quantity = CurrentDishesCount;
				// TODO: добавить к заказанным выбранное блюдо. в классе хэлпере сделать метод, кот вернет сформированный
				// объект OrderedDish
				//OrderedDishes.Add(SelectedDish);
			}

			OrderedDishes.Sort((d1, d2) => d1.Quantity - d2.Quantity);
			TotalSum = _sumCalculator.GetCurrentOrderedSum(OrderedDishes);
			CurrentDishesCount = 1;
		}

		public Command DeleteCommand { get; private set; }
		private bool OnDeleteCommandCanExecute()
		{
			return SelectedOrderedDish != null;
		}
		private void OnDeleteCommandExecute()
		{
			SelectedOrderedDish.Quantity = 1;
			OrderedDishes.Remove(SelectedOrderedDish);
			TotalSum = _sumCalculator.GetCurrentOrderedSum(OrderedDishes);
		}

		public Command BackCommand { get; private set; }
		private void OnBackCommandExecute()
		{
			_rootViewModel.ChangePage(new ClientHallViewModel(_user, _reservation, OrderedDishes));
		}


		public Command NextCommand { get; private set; }
		private bool OnNextCommandCanExecute()
		{
			return TotalSum != 0;
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
		}
	}
}
