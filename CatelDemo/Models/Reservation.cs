using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;
using RestaurantHelper.Models.Additional;

namespace RestaurantHelper.Models
{
	public class Reservation : MyModelBase
	{
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		[Required]
		public int TableId
		{
			get { return GetValue<int>(TableIdProperty); }
			set { SetValue(TableIdProperty, value); }
		}
		public static readonly PropertyData TableIdProperty = RegisterProperty("TableId", typeof(int));

		[ForeignKey("TableId")]
		public virtual Table Table { get; set; }

		[Required]
		public DateTime FirstTime
		{
			get { return GetValue<DateTime>(FirstTimeProperty); }
			set { SetValue(FirstTimeProperty, value); }
		}
		public static readonly PropertyData FirstTimeProperty = RegisterProperty("FirstTime", typeof(DateTime));

		[Required]
		public DateTime LastTime
		{
			get { return GetValue<DateTime>(LastTimeProperty); }
			set { SetValue(LastTimeProperty, value); }
		}
		public static readonly PropertyData LastTimeProperty = RegisterProperty("LastTime", typeof(DateTime));

		[Required]
		public DateTime Day
		{
			get { return GetValue<DateTime>(DayProperty); }
			set { SetValue(DayProperty, value); }
		}
		public static readonly PropertyData DayProperty = RegisterProperty("Day", typeof(DateTime));

		public override string ToString()
		{
			return $"{Day} {FirstTime}-{LastTime}";
		}
	}
}
