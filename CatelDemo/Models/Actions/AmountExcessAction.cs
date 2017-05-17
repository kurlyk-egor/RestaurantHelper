using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;

namespace RestaurantHelper.Models.Actions
{
	[Serializable]
	public class AmountExcessAction : Action
	{
		public AmountExcessAction()
		{
		}
		public int ExcessSum
		{
			get { return GetValue<int>(ExcessSumProperty); }
			set { SetValue(ExcessSumProperty, value); }
		}
		public static readonly PropertyData ExcessSumProperty = RegisterProperty("ExcessSum", typeof(int));

		public int DishId
		{
			get { return GetValue<int>(ExtraDishIdProperty); }
			set { SetValue(ExtraDishIdProperty, value); }
		}
		public static readonly PropertyData ExtraDishIdProperty = RegisterProperty("ExtraDishId", typeof(int));


		public override string ToString()
		{
			return $"ПРЕВЫШЕНИЕ {Id} | {ExcessSum,6} | {Name,20}";
		}
	}
}
