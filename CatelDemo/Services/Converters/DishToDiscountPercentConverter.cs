using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;

namespace RestaurantHelper.Services.Converters
{
	class DishToDiscountPercentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var dish = value as Dish;
			string result = string.Empty;

			if (dish != null)
			{
				var discount = UnitOfWork.GetInstance().DiscountActions.GetAll().FirstOrDefault(da => da.DishId == dish.Id);
			
				result = discount == null ? "" : $"- {discount.DiscountSum}%";
			}
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
