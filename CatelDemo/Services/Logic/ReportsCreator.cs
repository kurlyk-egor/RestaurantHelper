using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Collections;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Logic
{
	/// <summary>
	/// период времени для отчетности, отсчитываемый от сегодняшнего дня
	/// </summary>
	public enum Period
	{
		Today, Week, Month
	}

	class ReportsCreator
	{
		public ReportsCreator(Period period)
		{
			switch (period)
			{
				case Period.Today:
					_ordersSelector = GetDailyOrders;
					_orderedDishesSelector = GetDailyOrderedDishes;
					break;
				case Period.Week:
					_ordersSelector = GetWeeklyOrders;
					_orderedDishesSelector = GetWeeklyOrderedDishes;
					break;
				case Period.Month:
					_ordersSelector = GetMonthlyOrders;
					_orderedDishesSelector = GetMonthlyOrderedDishes;
					break;
			}
		}

		private readonly Func<Order, bool> _ordersSelector;
		private readonly Func<OrderedDish, bool> _orderedDishesSelector; 

		public bool GetDailyOrders(Order o) => o.Reservation.Day.Date == DateTime.Today;
		public bool GetWeeklyOrders(Order o) => o.Reservation.Day.Date > DateTime.Today.AddDays(-7);
		public bool GetMonthlyOrders(Order o) => o.Reservation.Day.Date > DateTime.Today.AddMonths(-1);

		public bool GetDailyOrderedDishes(OrderedDish od) => od.Order.Reservation.Day.Date == DateTime.Today;
		public bool GetWeeklyOrderedDishes(OrderedDish od) => od.Order.Reservation.Day.Date > DateTime.Today.AddDays(-7);
		public bool GetMonthlyOrderedDishes(OrderedDish od) => od.Order.Reservation.Day.Date > DateTime.Today.AddMonths(-1);


		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();

		public void CreateOrders(FastObservableCollection<Order> orders)
		{
			orders.Clear();
			var currentOrders = _unitOfWork.Orders.GetAll().Where(_ordersSelector).ToList();

			currentOrders.Sort( (o1, o2) => o2.Reservation.Day.CompareTo(o1.Reservation.Day));
			orders.AddItems(currentOrders);
		}

		public void CreateOrderedDishes(FastObservableCollection<OrderedDish> orderedDishes)
		{
			orderedDishes.Clear();
			var currentDishes =
				_unitOfWork.OrderedDishes.GetAll().Where(_orderedDishesSelector)
					.Where(od => od.OrderedPrice != 0).ToList(); // не считаем бонусы

			currentDishes.Sort((d1, d2) => d2.Order.Reservation.Day.CompareTo(d1.Order.Reservation.Day));

			orderedDishes.AddItems(currentDishes);
		}

		public void CreateBonusDishes(FastObservableCollection<OrderedDish> bonusDishes)
		{
			bonusDishes.Clear();
			var currentDishes =
				_unitOfWork.OrderedDishes.GetAll().Where(_orderedDishesSelector)
				.Where(od => od.OrderedPrice == 0).ToList(); // фильтруем только бонусы

			currentDishes.Sort((d1, d2) => d2.Order.Reservation.Day.CompareTo(d1.Order.Reservation.Day));
			bonusDishes.AddItems(currentDishes);
		}

		public int GetIncomes()
		{
			// считаем цены продаж
			var sum = _unitOfWork.OrderedDishes.GetAll().Where(_orderedDishesSelector).Sum(d => (d.OrderedPrice*d.Quantity));
			return sum;
		}

		public int GetCosts()
		{
			// считаем реальную стоимость бонусных блюд, выданных бесплатно (потери)
			var sum = _unitOfWork.OrderedDishes.GetAll().Where(_orderedDishesSelector).Where(o => o.OrderedPrice == 0).Sum(d => d.Dish.Price);
			return sum;
		}

		public int GetClientsCount()
		{
			// количество клиентов
			var count = _unitOfWork.Orders.GetAll().Where(_ordersSelector).Count();
			return count;
		}

		public int GetDishesCount()
		{
			// количество заказов
			var count = _unitOfWork.OrderedDishes.GetAll().Where(_orderedDishesSelector).Sum(od => od.Quantity);
			return count;
		}
	}
}
