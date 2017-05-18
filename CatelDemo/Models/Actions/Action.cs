using System;
using Catel.Data;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Models.Actions
{
	/// <summary>
	/// тип акции - 
	/// или скидка на товар,
	/// или бесплатный товар за превышение суммы заказа
	/// </summary>
	[Serializable]
	public enum ActionType
	{
		/// <summary>
		/// скидка (в процентах)
		/// название проводимой акции
		/// основной текст
		/// товар, к которому применяется 
		/// время действия
		/// </summary>
		Discount = 1,

		/// <summary>
		/// превышение суммы
		/// название акции
		/// товар, который будет бесплатно даваться
		/// непосредственно сумма превышения 
		/// </summary>
		AmountExcess
	}

	[Serializable]
	public class Action : ModelBase, IHaveId
	{
		public Action()
		{
		}
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


		public string Description
		{
			get { return GetValue<string>(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}
		public static readonly PropertyData DescriptionProperty = RegisterProperty("Description", typeof(string));
	}
}
