using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;

namespace RestaurantHelper.Models
{
	[Serializable]
	public class OrderedDish : ModelBase
	{
		public OrderedDish()
		{
		}

		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		public int OrderId
		{
			get { return GetValue<int>(OrderIdProperty); }
			set { SetValue(OrderIdProperty, value); }
		}
		public static readonly PropertyData OrderIdProperty = RegisterProperty("OrderId", typeof(int));

		public int DishId
		{
			get { return GetValue<int>(DishIdProperty); }
			set { SetValue(DishIdProperty, value); }
		}
		public static readonly PropertyData DishIdProperty = RegisterProperty("DishId", typeof(int));

		public int Quantity
		{
			get { return GetValue<int>(QuantityProperty); }
			set { SetValue(QuantityProperty, value); }
		}
		public static readonly PropertyData QuantityProperty = RegisterProperty("Quantity", typeof(int));
	}
}
