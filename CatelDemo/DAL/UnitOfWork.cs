using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RestaurantHelper.DAL.Repositories;

namespace RestaurantHelper.DAL
{
	partial class UnitOfWork : IDisposable
	{
		private static UnitOfWork _instance;
		private UnitOfWork()
		{
		}
		public static UnitOfWork GetInstance()
		{
			return _instance ?? (_instance = new UnitOfWork());
		}


		private readonly RestaurantDbContext _db = new RestaurantDbContext();

		private AmountExcessActionRepository _amountExcessActionRepository;
		private DiscountActionRepository _discountActionRepository;
		private ClientReviewRepository _clientReviewRepository;
		private ManagerAnswerRepository _managerAnswerRepository;
		private UserRepository _userRepository;
		private TableRepository _tableRepository;
		private ReservationRepository _reservationRepository;
		private DishRepository _dishRepository;
		private OrderRepository _orderRepository;
		private OrderedDishRepository _orderedDishRepository;
		private EmployeeRepository _employeeRepository;


		public void SaveChanges()
		{
			try
			{
				_db.SaveChanges();
			}
			catch (DbException ex)
			{
				MessageBox.Show("Произошла ошибка:" + ex.Message, "Операция не выполнена", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool _disposed;
		public void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_db.Dispose();
				}
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

	#region Unit Of Work Public Properties
	partial class UnitOfWork
	{
		public AmountExcessActionRepository AmountExcessActions => 
			_amountExcessActionRepository ?? (_amountExcessActionRepository = new AmountExcessActionRepository(_db));

		public DiscountActionRepository DiscountActions =>
			_discountActionRepository ?? (_discountActionRepository = new DiscountActionRepository(_db));

		public ClientReviewRepository ClientReviews =>
			_clientReviewRepository ?? (_clientReviewRepository = new ClientReviewRepository(_db));

		public ManagerAnswerRepository ManagerAnswers =>
			_managerAnswerRepository ?? (_managerAnswerRepository = new ManagerAnswerRepository(_db));

		public UserRepository Users =>
			_userRepository ?? (_userRepository = new UserRepository(_db));

		public TableRepository Tables =>
			_tableRepository ?? (_tableRepository = new TableRepository(_db));

		public ReservationRepository Reservations =>
			_reservationRepository ?? (_reservationRepository = new ReservationRepository(_db));

		public DishRepository Dishes =>
			_dishRepository ?? (_dishRepository = new DishRepository(_db));

		public OrderRepository Orders =>
			_orderRepository ?? (_orderRepository = new OrderRepository(_db));

		public OrderedDishRepository OrderedDishes =>
			_orderedDishRepository ?? (_orderedDishRepository = new OrderedDishRepository(_db));

		public EmployeeRepository Employees =>
			_employeeRepository ?? (_employeeRepository = new EmployeeRepository(_db));
	}
	#endregion
}
