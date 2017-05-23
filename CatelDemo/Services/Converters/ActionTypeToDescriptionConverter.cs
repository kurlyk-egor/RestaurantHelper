using System;
using System.Globalization;
using System.Windows.Data;
using RestaurantHelper.Models.Actions;

namespace RestaurantHelper.Services.Converters
{
	class ActionTypeToDescriptionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((ActionType)value)
			{
				case ActionType.AmountExcess:
					return "Выдача клиенту бесплатного товара при превышении определенной суммы заказа. Например, бесплатный напиток при заказе свыше 100, или дополнительное блюдо при заказе свыше 150";
				case ActionType.Discount:
					return "Конкретная скидка на определенный товар, устанавливаемая в процентах от цены товара. На один товар может быть только одна действующая скидка.";
				default:
					return string.Empty;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
