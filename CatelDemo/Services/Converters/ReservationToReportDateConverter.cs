using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Converters
{
	class ReservationToReportDateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var val = value as Reservation;
			if (val != null)
			{
				return $"{val.Day.ToShortDateString()} / {val.FirstTime.ToShortTimeString()}-{val.LastTime.ToShortTimeString()}";
			}
			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
