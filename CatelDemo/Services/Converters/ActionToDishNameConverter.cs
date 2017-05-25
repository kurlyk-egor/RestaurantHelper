using System;
using System.Globalization;
using System.Windows.Data;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;

namespace RestaurantHelper.Services.Converters
{
	class ActionToDishNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Dish item;
			UnitOfWork unitOfWork = UnitOfWork.GetInstance();

			var discaction = value as DiscountAction;
			if(discaction != null)
			{
				item = unitOfWork.Dishes.GetById(discaction.DishId);
				return item.Name;
			}

			var amntaction = value as AmountExcessAction;
			if (amntaction != null)
			{
				item = unitOfWork.Dishes.GetById(amntaction.DishId);
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
