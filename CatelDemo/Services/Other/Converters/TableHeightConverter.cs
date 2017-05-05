using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.MVVM.Converters;

namespace RestaurantHelper.Services.Other.Converters
{
	class TableHeightConverter : IValueConverter
	{
		private const int DEFAULT_HEIGHT = 70;

		// value - тип столика
		// 1 - высота по дефолту
		// 2 - высота по дефолту
		// 3 - высота в 2 раза больше
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
			{
				switch ((int)value)
				{
					case 1:
					case 2:
						return DEFAULT_HEIGHT;		
					case 3:
						return DEFAULT_HEIGHT * 2;
				}
			}

			return DEFAULT_HEIGHT;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
