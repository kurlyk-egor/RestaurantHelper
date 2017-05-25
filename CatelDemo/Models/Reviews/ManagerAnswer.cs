using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using Catel.Data;
using RestaurantHelper.Models.Additional;

namespace RestaurantHelper.Models.Reviews
{
	public class ManagerAnswer : MyModelBase
	{
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		[Required]
		public int ReviewId
		{
			get { return GetValue<int>(ReviewIdProperty); }
			set { SetValue(ReviewIdProperty, value); }
		}
		public static readonly PropertyData ReviewIdProperty = RegisterProperty("ReviewId", typeof(int));

		[ForeignKey("ReviewId")]
		public ClientReview Review { get; set; }	


		[MinLength(10), MaxLength(255), Required]
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
