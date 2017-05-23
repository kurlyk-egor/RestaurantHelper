using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RestaurantHelper.Models.Actions;
using Action = RestaurantHelper.Models.Actions.Action;

namespace RestaurantHelper.Services.Other.Converters
{
	class ActionToDescription : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var val = value as Action;
			if (val != null)
			{
				return val.Description;
			}
			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
