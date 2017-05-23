using System;
using System.Globalization;
using System.Windows.Data;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.Services.Converters
{
	class UserIdToLoginConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int)
			{
				var user = new Repository<User>().GetItem((int) value);

				if (user != null)
				{
					return user.Login;
				}
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
