using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;

namespace RestaurantHelper.Models
{
	[Serializable]
	public class Order : ModelBase
	{
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		public int UserId
		{
			get { return GetValue<int>(UserIdProperty); }
			set { SetValue(UserIdProperty, value); }
		}
		public static readonly PropertyData UserIdProperty = RegisterProperty("UserId", typeof(int));

		public int ReservationId
		{
			get { return GetValue<int>(ReservationIdProperty); }
			set { SetValue(ReservationIdProperty, value); }
		}
		public static readonly PropertyData ReservationIdProperty = RegisterProperty("ReservationId", typeof(int));

		/// <summary>
		/// ключ - Id блюда;
		/// значение - количество заказанных блюд
		/// </summary>
		public Dictionary<int, int> Dishes
		{
			get { return GetValue<Dictionary<int, int>>(DishesProperty); }
			set { SetValue(DishesProperty, value); }
		}
		public static readonly PropertyData DishesProperty = RegisterProperty("Dishes", typeof(Dictionary<int, int>), new Dictionary<int, int>());
	}
}
