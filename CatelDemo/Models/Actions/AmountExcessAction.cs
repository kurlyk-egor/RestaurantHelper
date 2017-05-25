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
	public class AmountExcessAction : Action
	{
		[Required]
		public int ExcessSum
		{
			get { return GetValue<int>(ExcessSumProperty); }
			set { SetValue(ExcessSumProperty, value); }
		}
		public static readonly PropertyData ExcessSumProperty = RegisterProperty("ExcessSum", typeof(int));

		[Required]
		public int DishId
		{
			get { return GetValue<int>(DishIdProperty); }
			set { SetValue(DishIdProperty, value); }
		}
		public static readonly PropertyData DishIdProperty = RegisterProperty("DishId", typeof(int));

		[ForeignKey("DishId")]
		public Dish Dish { get; set; }

		public override string ToString()
		{
			return $"ПРЕВЫШЕНИЕ {Id} | {ExcessSum,6} | {Name,20}";
		}
	}
}
