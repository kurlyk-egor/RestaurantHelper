using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Other
{
	class OrderedSumCalculator
	{
		public int GetCurrentSum(ICollection<Dish> dishes)
		{
			int sum = 0;
			if (dishes.Any())
			{
				sum += dishes.Sum(dish => dish.Price * dish.Quantity);
			}

			return sum;
		}
			
	}
}
