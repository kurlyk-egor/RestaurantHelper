using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Converters
{
	class OrderToTotalSumConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var order = value as Order;
			int sum = 0;

			if (order != null)
			{
				var dishes = order.OrderedDishes;

				dishes.ForEach(d => sum += (d.OrderedPrice * d.Quantity));
			}

			return sum;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}