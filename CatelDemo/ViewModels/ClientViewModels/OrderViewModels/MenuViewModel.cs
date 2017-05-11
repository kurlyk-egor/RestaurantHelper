using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Other;

namespace RestaurantHelper.ViewModels.ClientViewModels.OrderViewModels
{
    public class MenuViewModel : ViewModelBase
    {
		private readonly IViewModel _rootViewModel;
	    private readonly User _user;
	    private readonly Reservation _reservation;
	    private readonly ViewModelProperties _viewModelProperties;
	    private readonly DishRepository _dishRepository;

		public MenuViewModel(User user, Reservation reservation, ViewModelProperties viewModelProperties)
	    {
			_user = user;
			_reservation = reservation;
			_viewModelProperties = viewModelProperties;
			_dishRepository = DishRepository.GetRepositoryInstance();
		    _rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();

			AddCommand = new Command(OnAddCommandExecute, OnAddCommandCanExecute);
			DeleteCommand = new Command(OnDeleteCommandExecute, OnDeleteCommandCanExecute);
			BackCommand = new Command(OnBackCommandExecute);
			NextCommand = new Command(OnNextCommandExecute, OnNextCommandCanExecute);

			AddDishesToCollection();
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

		public int CurrentDishesCount
		{
			get { return GetValue<int>(CurrentDishesCountProperty); }
			set { SetValue(CurrentDishesCountProperty, value); }
		}
		public static readonly PropertyData CurrentDishesCountProperty = RegisterProperty("CurrentDishesCount", typeof(int), 1);

		public ObservableCollection<Dish> OrderedDishes
		{
			get { return GetValue<ObservableCollection<Dish>>(OrderedDishesProperty); }
			set { SetValue(OrderedDishesProperty, value); }
		}
		public static readonly PropertyData OrderedDishesProperty = RegisterProperty("OrderedDishes", typeof(ObservableCollection<Dish>), 
			new ObservableCollection<Dish>());

		public Dish SelectedOrderedDish
		{
			get { return GetValue<Dish>(SelectedOrderedDishProperty); }
			set { SetValue(SelectedOrderedDishProperty, value); }
		}
		public static readonly PropertyData SelectedOrderedDishProperty = RegisterProperty("SelectedOrderedDish", typeof(Dish));

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
			var dish = OrderedDishes.FirstOrDefault(cur => cur.Id == SelectedDish.Id);

			if (dish != null)
			{
				OrderedDishes.Remove(dish);
				OrderedDishes.Add(dish);
				dish.Quantity += CurrentDishesCount;
			}
			else
			{
				SelectedDish.Quantity = CurrentDishesCount;
				OrderedDishes.Add(SelectedDish);
			}

			OrderedDishes.Sort((d1, d2) => d1.Quantity - d2.Quantity);
			CalculateTotals();
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
			CalculateTotals();
		}

		public Command BackCommand { get; private set; }
		private void OnBackCommandExecute()
		{
			_rootViewModel.ChangePage(new HallViewModel(_user, _viewModelProperties));
		}


		public Command NextCommand { get; private set; }
		private bool OnNextCommandCanExecute()
		{
			return true;
		}
		private void OnNextCommandExecute()
		{
			
		}

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }

		private void AddDishesToCollection()
		{
			Dishes.Clear();
			((ICollection<Dish>)Dishes).AddRange(_dishRepository.GetCollection());
		}

		private void CalculateTotals()
		{
			int sum = 0;
			if (OrderedDishes.Any())
			{
				sum += OrderedDishes.Sum(dish => dish.Price * dish.Quantity);
			}
			TotalSum = sum;
		}
	}
}
