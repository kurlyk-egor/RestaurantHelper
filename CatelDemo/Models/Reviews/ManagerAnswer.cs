using System;
using Catel.Data;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Models.Reviews
{
	[Serializable]
	public class ManagerAnswer : ModelBase, IHaveId
	{
		public ManagerAnswer()
		{
		}
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		public int ReviewId
		{
			get { return GetValue<int>(ReviewIdProperty); }
			set { SetValue(ReviewIdProperty, value); }
		}
		public static readonly PropertyData ReviewIdProperty = RegisterProperty("ReviewId", typeof(int));

		public string Text
		{
			get { return GetValue<string>(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
		public static readonly PropertyData TextProperty = RegisterProperty("Text", typeof(string));


		public DateTime DateTime
		{
			get { return GetValue<DateTime>(DateTimeProperty); }
			set { SetValue(DateTimeProperty, value); }
		}
		public static readonly PropertyData DateTimeProperty = RegisterProperty("DateTime", typeof(DateTime));
	}
}
