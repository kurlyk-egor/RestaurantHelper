using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Catel.Data;
using RestaurantHelper.Models.Additional;

namespace RestaurantHelper.Models.Reviews
{
	public class ClientReview : MyModelBase
	{
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		[Required]
		public int UserId
		{
			get { return GetValue<int>(UserIdProperty); }
			set { SetValue(UserIdProperty, value); }
		}
		public static readonly PropertyData UserIdProperty = RegisterProperty("UserId", typeof(int));

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int OrderId
		{
			get { return GetValue<int>(OrderIdProperty); }
			set { SetValue(OrderIdProperty, value); }
		}
		public static readonly PropertyData OrderIdProperty = RegisterProperty("OrderId", typeof(int));

		[ForeignKey("OrderId")]
		public virtual Order Order { get; set; }

		public string Text
		{
			get { return GetValue<string>(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
		public static readonly PropertyData TextProperty = RegisterProperty("Text", typeof(string));

		[Required]
		public DateTime DateTime
		{
			get { return GetValue<DateTime>(DateTimeProperty); }
			set { SetValue(DateTimeProperty, value); }
		}
		public static readonly PropertyData DateTimeProperty = RegisterProperty("DateTime", typeof(DateTime));
	}
}
