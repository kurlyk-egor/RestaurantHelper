using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Catel.Collections;
using Catel.Data;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Other;

namespace RestaurantHelper.ViewModels.ClientViewModels
{
	using Catel.MVVM;
	using System.Threading.Tasks;

	public class MyOrdersViewModel : ViewModelBase
	{
		private readonly User _user;
		private readonly IViewModel _parentViewModel;
		private readonly IViewModel _rootViewModel;
		private readonly OrderRepository _orderRepository;
		private readonly ReservationRepository _reservationRepository;
		private readonly DishRepository _dishRepository;
		private readonly OrderedDishRepository _orderedDishRepository;
		private readonly OrderedSumCalculator _orderedSumCalculator;

		public MyOrdersViewModel(IViewModel parentViewModel, User user)
		{
			_user = user;
			UserLogin = _user.Login;
			_parentViewModel = parentViewModel;
			_orderedSumCalculator = new OrderedSumCalculator();
			_rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();

			_dishRepository = DishRepository.GetRepositoryInstance();
			_orderRepository = OrderRepository.GetRepositoryInstance();
			_reservationRepository = ReservationRepository.GetRepositoryInstance();
			_orderedDishRepository = OrderedDishRepository.GetRepositoryInstance();

			BackCommand = new Command(OnBackCommandExecute);
			SelectAnotherOrderCommand = new Command(OnSelectAnotherOrderCommandExecute);

			FillOrdersWithReservationsList();
		}


		public ObservableCollection<OrderWithReservationInfo> OrdersWithReservations
		{
			get { return GetValue<ObservableCollection<OrderWithReservationInfo>>(OrdersWithReservationsProperty); }
			set { SetValue(OrdersWithReservationsProperty, value); }
		}
		public static readonly PropertyData OrdersWithReservationsProperty = RegisterProperty("OrdersWithReservations", 
			typeof(ObservableCollection<OrderWithReservationInfo>),
			new ObservableCollection<OrderWithReservationInfo>() );

		public OrderWithReservationInfo SelectedOrderWithReservation
		{
			get { return GetValue<OrderWithReservationInfo>(SelectedOrderWithReservationProperty); }
			set { SetValue(SelectedOrderWithReservationProperty, value); }
		}
		public static readonly PropertyData SelectedOrderWithReservationProperty = RegisterProperty("SelectedOrderWithReservation", typeof(OrderWithReservationInfo));

		public string UserLogin
		{
			get { return GetValue<string>(UserLoginProperty); }
			set { SetValue(UserLoginProperty, value); }
		}
		public static readonly PropertyData UserLoginProperty = RegisterProperty("UserLogin", typeof(string));

		public ObservableCollection<Dish> Dishes
		{
			get { return GetValue<ObservableCollection<Dish>>(DishesProperty); }
			set { SetValue(DishesProperty, value); }
		}
		public static readonly PropertyData DishesProperty = RegisterProperty("Dishes", typeof(ObservableCollection<Dish>),
			new ObservableCollection<Dish>());

		public int TotalSum
		{
			get { return GetValue<int>(TotalSumProperty); }
			set { SetValue(TotalSumProperty, value); }
		}
		public static readonly PropertyData TotalSumProperty = RegisterProperty("TotalSum", typeof(int), 0);


		public Command SelectAnotherOrderCommand { get; private set; }
		private void OnSelectAnotherOrderCommandExecute()
		{
			Dishes.Clear();
			// получаем все заказанные блюда
			var orderedDishes =
				_orderedDishRepository.GetCollection().Where(od => od.OrderId == SelectedOrderWithReservation.OrderId);
			foreach (var orderedDish in orderedDishes)
			{
				// получаем описание блюда
				var dish = _dishRepository.GetCollection().FirstOrDefault(d => d.Id == orderedDish.DishId);
				// указываем, сколько было заказано
				dish.Quantity = orderedDish.Quantity;
				Dishes.Add(dish);
			}
			TotalSum = _orderedSumCalculator.GetCurrentSum(Dishes);
		}


		public Command BackCommand { get; private set; }
		private void OnBackCommandExecute()
		{
			_rootViewModel.ChangePage(_parentViewModel);
		}

		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}

		private void FillOrdersWithReservationsList()
		{
			OrdersWithReservations.Clear();

			var userOrders = _orderRepository.GetCollection()
				.Join(_reservationRepository.GetCollection(), 
				order => order.ReservationId, 
				reservation => reservation.Id,
				(o, r) => new OrderWithReservationInfo
				{
					OrderId = o.Id,
					TableId = r.TableId,
					UserId = o.UserId,
					Day = r.Day.ToLongDateString(),
					FirstTime = r.FirstTime.ToShortTimeString(),
					LastTime = r.LastTime.ToShortTimeString()
				})
				.Where(or => or.UserId == _user.Id);

			foreach (var us in userOrders)
			{
				OrdersWithReservations.Add(us);
			}
		}
	}
}
