using System;
using System.ComponentModel.DataAnnotations;
using Catel.Data;
using RestaurantHelper.Models.Additional;

namespace RestaurantHelper.Models.Actions
{
	/// <summary>
	/// тип акции - 
	/// </summary>
	public enum ActionType
	{
		/// <summary>
		/// скидка на любое блюдо
		/// </summary>
		Discount = 1,

		/// <summary>
		/// бесплатное блюдо за превышение суммы заказа
		/// </summary>
		AmountExcess
	}

	public class Action : MyModelBase
	{
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		[MinLength(5), MaxLength(20), Required]
		public string Name
		{
			get { return GetValue<string>(NameProperty); }
			set { SetValue(NameProperty, value); }
		}
		public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string));

		[MinLength(10), MaxLength(150), Required]
		public string Description
		{
			get { return GetValue<string>(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}
		public static readonly PropertyData DescriptionProperty = RegisterProperty("Description", typeof(string));
	}
}
