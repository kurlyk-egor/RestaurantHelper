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
		public int GetCurrentOrderedSum(FastObservableCollection<OrderedDish> dishes)
		{
			int sum = 0;
			if (dishes.Any())
			{
				sum += dishes.Sum(dish => dish.OrderedPrice * dish.Quantity);
			}

			return sum;
		}


		public void CalculateRealPrices(FastObservableCollection<OrderedDish> dishes)
		{
			var discounts = _unitOfWork.DiscountActions.GetAll();

			dishes.ForEach(dish =>
			{
				var currentDiscount = discounts.First(d => d.DishId == dish.Id);
				var defaultPrice = _unitOfWork.Dishes.GetById(dish.Id).Price;

				if (currentDiscount != null)
				{
					//  сумма с учетом скидки
					dish.OrderedPrice = defaultPrice*(currentDiscount.DiscountSum/100);
				}
			});
		}
	}
}
