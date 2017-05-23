using System;
using System.Globalization;
using System.Windows.Data;
using Action = RestaurantHelper.Models.Actions.Action;

namespace RestaurantHelper.Services.Converters
{
	class ActionToDescriptionConverter : IValueConverter
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
