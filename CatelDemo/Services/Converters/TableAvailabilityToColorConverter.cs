using System;
using System.Globalization;
using Catel.MVVM.Converters;

namespace RestaurantHelper.Services.Converters
{
	class TableAvailabilityToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
			{
				if ((bool) value) return "LawnGreen";
			}
			return "MediumVioletRed";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
