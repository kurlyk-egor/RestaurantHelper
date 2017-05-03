using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;

namespace RestaurantHelper.Models
{
	[Serializable]
	public class Table : ModelBase
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

		public int Top
		{
			get { return GetValue<int>(TopProperty); }
			set { SetValue(TopProperty, value); }
		}
		public static readonly PropertyData TopProperty = RegisterProperty("Top", typeof(int));

		public int Left
		{
			get { return GetValue<int>(LeftProperty); }
			set { SetValue(LeftProperty, value); }
		}
		public static readonly PropertyData LeftProperty = RegisterProperty("Left", typeof(int));
	}
}
