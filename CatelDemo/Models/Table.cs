using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;

namespace RestaurantHelper.Models
{
	[Serializable]
	class Table : ModelBase
	{
		public Table()
		{			
		}
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		public int Number
		{
			get { return GetValue<int>(NumberProperty); }
			set { SetValue(NumberProperty, value); }
		}
		public static readonly PropertyData NumberProperty = RegisterProperty("Number", typeof(int));

		public int SeatsNumber
		{
			get { return GetValue<int>(SeatsNumberProperty); }
			set { SetValue(SeatsNumberProperty, value); }
		}
		public static readonly PropertyData SeatsNumberProperty = RegisterProperty("SeatsNumber", typeof(int));
	}
}
