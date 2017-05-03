using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.IO;
using Catel.MVVM.Converters;

namespace RestaurantHelper.Services.Other
{
	public class TableConverter : IValueConverter
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
