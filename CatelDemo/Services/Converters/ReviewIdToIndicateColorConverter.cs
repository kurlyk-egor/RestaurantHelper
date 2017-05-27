using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.MVVM.Converters;
using RestaurantHelper.DAL;
using RestaurantHelper.Models.Reviews;

namespace RestaurantHelper.Services.Converters
{
	class ReviewIdToIndicateColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string color = "Gray";
			// value - Id отзыва. надо проверить, существует ли на него ответ
			if (value != null)
			{
				var answer = UnitOfWork.GetInstance().ManagerAnswers.GetAll()
					.FirstOrDefault(a => a.ReviewId == (int) value);

				color = (answer == null) ? "LimeGreen" : "OrangeRed";
			}
			return color;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
