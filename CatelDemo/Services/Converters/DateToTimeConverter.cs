﻿using System;
using System.Globalization;
using Catel.MVVM.Converters;

namespace RestaurantHelper.Services.Converters
{
	class DateToTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return string.Empty;
			return ((DateTime)value).ToShortTimeString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
