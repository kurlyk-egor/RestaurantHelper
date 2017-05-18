using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Models.Actions
{
	[Serializable]
	public class DiscountAction : Action
	{
		public DiscountAction()
		{
		}
		public int DiscountSum
		{
			get { return GetValue<int>(DiscountSumProperty); }
			set { SetValue(DiscountSumProperty, value); }
		}
		public static readonly PropertyData DiscountSumProperty = RegisterProperty("DiscountSum", typeof(int));

		public int DiscountedDishId
		{
			get { return GetValue<int>(DiscountedDishIdProperty); }
			set { SetValue(DiscountedDishIdProperty, value); }
		}
		public static readonly PropertyData DiscountedDishIdProperty = RegisterProperty("DiscountedDishId", typeof(int));
	}
}
