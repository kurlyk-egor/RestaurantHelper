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
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Models.Reviews;

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

		private IRepository<BonusAction> _bonusActionRepository;
		private IRepository<DiscountAction> _discountActionRepository;
		private IRepository<ClientReview> _clientReviewRepository;
		private IRepository<ManagerAnswer> _managerAnswerRepository;
		private IRepository<User> _userRepository;
		private IRepository<Table> _tableRepository;
		private IRepository<Reservation> _reservationRepository;
		private IRepository<Dish> _dishRepository;
		private IRepository<Order> _orderRepository;
		private IRepository<OrderedDish> _orderedDishRepository;
		private IRepository<Employee> _employeeRepository;


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
		public IRepository<BonusAction> BonusActions => 
			_bonusActionRepository ?? (_bonusActionRepository = new BonusActionRepository(_db));

		public IRepository<DiscountAction> DiscountActions =>
			_discountActionRepository ?? (_discountActionRepository = new DiscountActionRepository(_db));

		public IRepository<ClientReview> ClientReviews =>
			_clientReviewRepository ?? (_clientReviewRepository = new ClientReviewRepository(_db));

		public IRepository<ManagerAnswer> ManagerAnswers =>
			_managerAnswerRepository ?? (_managerAnswerRepository = new ManagerAnswerRepository(_db));

		public IRepository<User> Users =>
			_userRepository ?? (_userRepository = new UserRepository(_db));

		public IRepository<Table> Tables =>
			_tableRepository ?? (_tableRepository = new TableRepository(_db));

		public IRepository<Reservation> Reservations =>
			_reservationRepository ?? (_reservationRepository = new ReservationRepository(_db));

		public IRepository<Dish> Dishes =>
			_dishRepository ?? (_dishRepository = new DishRepository(_db));

		public IRepository<Order> Orders =>
			_orderRepository ?? (_orderRepository = new OrderRepository(_db));

		public IRepository<OrderedDish> OrderedDishes =>
			_orderedDishRepository ?? (_orderedDishRepository = new OrderedDishRepository(_db));

		public IRepository<Employee> Employees =>
			_employeeRepository ?? (_employeeRepository = new EmployeeRepository(_db));
	}
	#endregion
}
