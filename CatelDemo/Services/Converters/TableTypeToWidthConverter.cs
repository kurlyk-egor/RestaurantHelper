using System;
using System.Globalization;
using Catel.MVVM.Converters;

namespace RestaurantHelper.Services.Converters
{
	class TableTypeToWidthConverter : IValueConverter
	{
		private const int DEFAULT_WIDTH = 70;

		// value - тип столика
		// 1 - ширина по дефолту
		// 2 - ширина в 2 раза больше
		// 3 - ширина в 2 раза больше
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
			{
				switch ((int) value)
				{
					case 1:
						return DEFAULT_WIDTH;
					case 2:
					case 3:
						return (DEFAULT_WIDTH * 1.8);
				}
			}

			return DEFAULT_WIDTH;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
