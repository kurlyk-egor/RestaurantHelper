using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Catel.Collections;
using Catel.Data;
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Additional;
using RestaurantHelper.Services.Logic;

namespace RestaurantHelper.ViewModels.ClientViewModels
{
	using Catel.MVVM;
	using System.Threading.Tasks;


	/// <summary>
	/// мои заказы
	/// </summary>
	public class ClientOrdersViewModel : ViewModelBase
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private readonly User _user;
		private readonly IViewModel _parentViewModel;
		private readonly IViewModel _rootViewModel;
		private readonly OrderedSumCalculator _orderedSumCalculator;

		public ClientOrdersViewModel(IViewModel parentViewModel, User user)
		{
			_user = user;
			UserLogin = _user.Login;
			_parentViewModel = parentViewModel;
			_orderedSumCalculator = new OrderedSumCalculator(OrderedDishes);
			_rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();

			BackCommand = new Command(OnBackCommandExecute);
			SelectAnotherOrderCommand = new Command(OnSelectAnotherOrderCommandExecute);
			DeleteOrderCommand = new Command(OnDeleteOrderCommandExecute, OnDeleteOrderCommandCanExecute);

			RefreshOrdersWithReservationsCollection();
		}


		public FastObservableCollection<OrderWithReservationInfo> OrdersWithReservations
		{
			get { return GetValue<FastObservableCollection<OrderWithReservationInfo>>(OrdersWithReservationsProperty); }
			set { SetValue(OrdersWithReservationsProperty, value); }
		}
		public static readonly PropertyData OrdersWithReservationsProperty = RegisterProperty("OrdersWithReservations", 
			typeof(FastObservableCollection<OrderWithReservationInfo>),
			new FastObservableCollection<OrderWithReservationInfo>() );

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

		public FastObservableCollection<OrderedDish> OrderedDishes
		{
			get { return GetValue<FastObservableCollection<OrderedDish>>(OrderedDishesProperty); }
			set { SetValue(OrderedDishesProperty, value); }
		}
		public static readonly PropertyData OrderedDishesProperty = RegisterProperty("OrderedDishes", typeof(FastObservableCollection<OrderedDish>),
			new FastObservableCollection<OrderedDish>());

		public int TotalSum
		{
			get { return GetValue<int>(TotalSumProperty); }
			set { SetValue(TotalSumProperty, value); }
		}
		public static readonly PropertyData TotalSumProperty = RegisterProperty("TotalSum", typeof(int), 0);


		public Command SelectAnotherOrderCommand { get; private set; }
		private void OnSelectAnotherOrderCommandExecute()
		{
			if(SelectedOrderWithReservation != null)
			{
				OrderedDishes.Clear();
				// получаем все заказанные блюда
				var orderedDishes = _unitOfWork.OrderedDishes.GetAll()
					.Where(od => od.OrderId == SelectedOrderWithReservation.OrderId);

				// добавляем в коллекцию
				OrderedDishes.AddItems(orderedDishes);
				TotalSum = _orderedSumCalculator.GetCurrentOrderedSum();
			}
		}


		public Command DeleteOrderCommand { get; private set; }
		private bool OnDeleteOrderCommandCanExecute()
		{
			return SelectedOrderWithReservation != null;
		}
		private void OnDeleteOrderCommandExecute()
		{
			var order = _unitOfWork.Orders.GetById(SelectedOrderWithReservation.OrderId);

			_unitOfWork.Orders.Delete(order.Id);
			_unitOfWork.Reservations.Delete(order.ReservationId);
			_unitOfWork.SaveChanges();

			RefreshOrdersWithReservationsCollection();
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

		private void RefreshOrdersWithReservationsCollection()
		{
			OrdersWithReservations.Clear();
			OrderedDishes.Clear();

			var userOrders = _unitOfWork.Orders.GetAll()
				.Join(_unitOfWork.Reservations.GetAll(), 
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
