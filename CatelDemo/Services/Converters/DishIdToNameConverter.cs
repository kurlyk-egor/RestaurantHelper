using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RestaurantHelper.DAL;

namespace RestaurantHelper.Services.Converters
{
	class DishIdToNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			UnitOfWork uow = UnitOfWork.GetInstance();
			if (value is int)
			{
				var dish = uow.Dishes.GetById((int)value);

				if (dish != null)
				{
					return dish.Name;
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
