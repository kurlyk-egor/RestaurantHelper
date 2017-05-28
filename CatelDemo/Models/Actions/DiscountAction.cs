using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;

namespace RestaurantHelper.Models.Actions
{
	public class DiscountAction : Action
	{
		[Required]
		public int DiscountSum
		{
			get { return GetValue<int>(DiscountSumProperty); }
			set { SetValue(DiscountSumProperty, value); }
		}
		public static readonly PropertyData DiscountSumProperty = RegisterProperty("DiscountSum", typeof(int));

		public override string ToString()
		{
			return $"СКИДКА {Id} | {DiscountSum,2}% | {Name,20}";
		}
	}
}
