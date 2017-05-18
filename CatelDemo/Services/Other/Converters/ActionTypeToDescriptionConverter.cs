using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RestaurantHelper.Models.Actions;

namespace RestaurantHelper.Services.Other.Converters
{
	class ActionTypeToDescriptionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((ActionType)value)
			{
				case ActionType.AmountExcess:
					return "Выдача клиенту бесплатного товара при превышении определенной суммы заказа";
				case ActionType.Discount:
					return "Конкретная скидка на определенный товар";
				default:
					return "";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
