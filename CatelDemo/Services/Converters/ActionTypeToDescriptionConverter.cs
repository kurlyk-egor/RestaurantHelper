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
				case ActionType.Bonus:
					return "БОНУС - бесплатное блюдо при заказе больше определенной суммы. Например, бесплатный напиток при заказе свыше 100. За один заказ - один бонус. Выбирается самый дорогой бонус (за самую крупную сумму превышения)";
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
