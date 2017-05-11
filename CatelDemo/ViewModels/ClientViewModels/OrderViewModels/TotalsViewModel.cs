using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Other;

namespace RestaurantHelper.ViewModels.ClientViewModels.OrderViewModels
{
    public class TotalsViewModel : ViewModelBase
    {
	    private readonly User _user;
	    private readonly Reservation _reservation;
	    private readonly ObservableCollection<Dish> _orderedDishes;
	    private readonly IViewModel _rootViewModel;
	    private readonly TableRepository _tableRepository;
	    private readonly ReservationRepository _reservationRepository;
	    private readonly OrderRepository _orderRepository;
	    private readonly OrderedSumCalculator _sumCalculator;

		public TotalsViewModel(User user, Reservation reservation, ObservableCollection<Dish> orderedDishes)
        {
			_user = user;
			_reservation = reservation;
			_orderedDishes = orderedDishes;
			_sumCalculator = new OrderedSumCalculator();
			_tableRepository = TableRepository.GetRepositoryInstance();
			_orderRepository = OrderRepository.GetRepositoryInstance();
			_reservationRepository = ReservationRepository.GetRepositoryInstance();
			_rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();

			BackCommand = new Command(OnBackCommandExecute);
			OrderCommand = new Command(OnOrderCommandExecute);

			FillViewModelProperties();
        }

	    public string UserLogin
		{
			get { return GetValue<string>(UserLoginProperty); }
			set { SetValue(UserLoginProperty, value); }
		}
		public static readonly PropertyData UserLoginProperty = RegisterProperty("UserLogin", typeof(string));

		public int TableNumber
		{
			get { return GetValue<int>(TableNumberProperty); }
			set { SetValue(TableNumberProperty, value); }
		}
		public static readonly PropertyData TableNumberProperty = RegisterProperty("TableNumber", typeof(int));

		public int TableSeatsNumber
		{
			get { return GetValue<int>(TableSeatsNumberProperty); }
			set { SetValue(TableSeatsNumberProperty, value); }
		}
		public static readonly PropertyData TableSeatsNumberProperty = RegisterProperty("TableSeatsNumber", typeof(int));

		public string VisitDate
		{
			get { return GetValue<string>(VisitDateProperty); }
			set { SetValue(VisitDateProperty, value); }
		}
		public static readonly PropertyData VisitDateProperty = RegisterProperty("VisitDate", typeof(string));

		public string FirstTime
		{
			get { return GetValue<string>(FirstTimeProperty); }
			set { SetValue(FirstTimeProperty, value); }
		}
		public static readonly PropertyData FirstTimeProperty = RegisterProperty("FirstTime", typeof(string));

		public string LastTime
		{
			get { return GetValue<string>(LastTimeProperty); }
			set { SetValue(LastTimeProperty, value); }
		}
		public static readonly PropertyData LastTimeProperty = RegisterProperty("LastTime", typeof(string));

		public ObservableCollection<Dish> OrderedDishes
		{
			get { return GetValue<ObservableCollection<Dish>>(OrderedDishesProperty); }
			set { SetValue(OrderedDishesProperty, value); }
		}
		public static readonly PropertyData OrderedDishesProperty = RegisterProperty("OrderedDishes", typeof(ObservableCollection<Dish>), 
			new ObservableCollection<Dish>());

		public int TotalSum
		{
			get { return GetValue<int>(TotalSumProperty); }
			set { SetValue(TotalSumProperty, value); }
		}
		public static readonly PropertyData TotalSumProperty = RegisterProperty("TotalSum", typeof(int));

		public string PhoneNumber
		{
			get { return GetValue<string>(PhoneNumberProperty); }
			set { SetValue(PhoneNumberProperty, value); }
		}
		public static readonly PropertyData PhoneNumberProperty = RegisterProperty("PhoneNumber", typeof(string));


	    public Command BackCommand { get; private set; }
		private void OnBackCommandExecute()
		{
			_rootViewModel.ChangePage(new MenuViewModel(_user, _reservation, _orderedDishes));
		}


		public Command OrderCommand { get; private set; }
		private void OnOrderCommandExecute()
		{
			SaveReservation();
			SaveOrder();
			_rootViewModel.ChangePage(new ClientMainViewModel(_user));
		}


	    protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }

		private void FillViewModelProperties()
		{
			UserLogin = _user.Login;
			PhoneNumber = _user.Phone;

			TableNumber = _reservation.TableId;
			TableSeatsNumber = _tableRepository.GetItem(_reservation.TableId).SeatsNumber;

			VisitDate = _reservation.Day.ToLongDateString();
			FirstTime = _reservation.FirstTime.ToShortTimeString();
			LastTime = _reservation.LastTime.ToShortTimeString();

			OrderedDishes = _orderedDishes;
			TotalSum = _sumCalculator.GetCurrentSum(OrderedDishes);
		}

		private void SaveReservation()
		{
			_reservationRepository.Insert(_reservation);
			_reservationRepository.SaveChanges();
		}

		private void SaveOrder()
		{
			Order order = new Order();
			order.UserId = _user.Id;
			order.ReservationId = _reservation.Id;
			
			Dictionary<int, int> dishesDictionary = _orderedDishes.ToDictionary(dish => dish.Id, dish => dish.Quantity);
			order.Dishes = dishesDictionary;

			_orderRepository.Insert(order);

			_orderRepository.SaveChanges();
		}
	}
}
