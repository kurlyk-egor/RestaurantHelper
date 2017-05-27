using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RestaurantHelper.Models.Actions;

namespace RestaurantHelper.Services.Converters
{
	class ActionToPercentOrSumConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var discount = value as DiscountAction;
			if (discount != null)
			{
				return $"{discount.DiscountSum} %";
			}
			var bonus = value as BonusAction;
			if (bonus != null)
			{
				return $"{bonus.ExcessSum} у.е.";
			}
			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
