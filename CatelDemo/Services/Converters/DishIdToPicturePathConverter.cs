using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.Services.Other.Converters
{
	class DishIdToPicturePathConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int)
			{
				return $"Resources/Dishes/{(int) value}.jpg";
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
