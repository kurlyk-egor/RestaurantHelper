using System.Collections.Generic;
using System.Linq;
using Catel.Collections;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Logic
{
	class OrderedSumCalculator
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		/// <summary>
		/// ссылка на коллекцию заказанных блюд в клиентской вьюмодели
		/// </summary>
		private readonly FastObservableCollection<OrderedDish> _dishes;

		public OrderedSumCalculator(FastObservableCollection<OrderedDish> dishes)
		{
			_dishes = dishes;
		}

		public int GetCurrentOrderedSum()
		{
			int sum = 0;
			if (_dishes.Any())
			{
				sum += _dishes.Sum(dish => dish.OrderedPrice * dish.Quantity);
			}

			return sum;
		}

		public void CalculateRealPrices()
		{
			var discounts = _unitOfWork.DiscountActions.GetAll();

			_dishes.ForEach(dish =>
			{
				var currentDiscount = discounts.First(d => d.DishId == dish.Id);
				var defaultPrice = _unitOfWork.Dishes.GetById(dish.Id).Price;

				// DiscountSum в диапазоне 5...90
				var coeff = 1 + currentDiscount?.DiscountSum / 100.0  ??   1.0;
				
				//  сумма с учетом скидки
				dish.OrderedPrice = (int)(defaultPrice*coeff);
			});
		}

		public int GetRealPrice(Dish dish)
		{
			var discount = _unitOfWork.DiscountActions.GetAll().SingleOrDefault(d => d.DishId == dish.Id);
			var coeff = 1 - (discount?.DiscountSum / 100.0 ?? 0);

			return (int)(dish.Price * coeff);
		}

		public void AddDishIntoOrderedDishes(Dish dish)
		{
			// проверяем, есть ли уже в списке такое блюдо
			var orderedDish = _dishes.FirstOrDefault(cur => cur.DishId == dish.Id);

			if (orderedDish != null)
			{
				orderedDish.Quantity++;
			}
			else
			{
				orderedDish = new OrderedDish
				{
					DishId = dish.Id,
					Quantity = 1,
					OrderedPrice = GetRealPrice(dish)
				};
				_dishes.Add(orderedDish);
			}

		}
	}
}
