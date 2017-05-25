using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;
using RestaurantHelper.Models.Additional;

namespace RestaurantHelper.Models
{
	[Flags]
	public enum Days
	{
		Monday = 1,
		Tuesday = 2,
		Wednesday = 4,
		Thursday = 8,
		Friday = 16,
		Saturday = 32,
		Sunday = 64
	}

	public class Employee : MyModelBase
	{
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		public string Name
		{
			get { return GetValue<string>(NameProperty); }
			set { SetValue(NameProperty, value); }
		}
		public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string));

		public int Age
		{
			get { return GetValue<int>(AgeProperty); }
			set { SetValue(AgeProperty, value); }
		}
		public static readonly PropertyData AgeProperty = RegisterProperty("Age", typeof(int));

		public string Position
		{
			get { return GetValue<string>(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}
		public static readonly PropertyData PositionProperty = RegisterProperty("Position", typeof(string));

		public string WorkDays
		{
			get { return GetValue<string>(WorkDaysProperty); }
			set { SetValue(WorkDaysProperty, value); }
		}
		public static readonly PropertyData WorkDaysProperty = RegisterProperty("WorkDays", typeof(string));
	}
}
