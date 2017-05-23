using System;
using System.Globalization;
using System.Windows.Data;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.Services.Converters
{
	class ActionToDishNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Dish item;

			var discaction = value as DiscountAction;
			if(discaction != null)
			{
				item = new Repository<Dish>().GetItem(discaction.DishId);
				return item.Name;
			}

			var amntaction = value as AmountExcessAction;
			if (amntaction != null)
			{
				item = new Repository<Dish>().GetItem(amntaction.DishId);
				return item.Name;
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
