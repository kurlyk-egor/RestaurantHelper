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
	public class BonusAction : Action
	{
		[Required]
		public int ExcessSum
		{
			get { return GetValue<int>(ExcessSumProperty); }
			set { SetValue(ExcessSumProperty, value); }
		}
		public static readonly PropertyData ExcessSumProperty = RegisterProperty("ExcessSum", typeof(int));

		public override string ToString()
		{
			return $"БОНУС {Id} | {ExcessSum,6} | {Name,20}";
		}
	}
}
