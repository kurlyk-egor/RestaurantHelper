using System;
using Catel.Data;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Models.Reviews
{
	[Serializable]
	public class ClientReview : ModelBase, IHaveId
	{
		public ClientReview()
		{	
		}

		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));


		public int UserId
		{
			get { return GetValue<int>(UserIdProperty); }
			set { SetValue(UserIdProperty, value); }
		}
		public static readonly PropertyData UserIdProperty = RegisterProperty("UserId", typeof(int));


		public int AnswerId
		{
			get { return GetValue<int>(AnswerIdProperty); }
			set { SetValue(AnswerIdProperty, value); }
		}
		public static readonly PropertyData AnswerIdProperty = RegisterProperty("AnswerId", typeof(int));


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
