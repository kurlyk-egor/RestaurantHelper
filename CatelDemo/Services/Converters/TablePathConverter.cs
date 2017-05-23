using System;
using System.Globalization;
using Catel.MVVM.Converters;

namespace RestaurantHelper.Services.Other.Converters
{
	public class TablePathConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string path = "";
			if (value != null)
			{
				path = $@"../../../Resources/Tables/table{(int)value}.jpg";
			}
			return path;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
