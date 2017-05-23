using System;
using System.Globalization;
using Catel.MVVM.Converters;

namespace RestaurantHelper.Services.Converters
{
	class DateToDateStringConverter  : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value != null)
			{
				return ((DateTime) value).ToShortDateString();
			}
			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
