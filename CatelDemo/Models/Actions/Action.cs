using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
		Bonus
	}

	public class Action : MyModelBase
	{
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		[Required]
		public int DishId
		{
			get { return GetValue<int>(DishIdProperty); }
			set { SetValue(DishIdProperty, value); }
		}
		public static readonly PropertyData DishIdProperty = RegisterProperty("DishId", typeof(int));

		[ForeignKey("DishId")]
		public virtual Dish Dish { get; set; }


		[MinLength(5), MaxLength(20), Required]
		public string Name
		{
			get { return GetValue<string>(NameProperty); }
			set { SetValue(NameProperty, value); }
		}
		public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string));

		[MinLength(15), MaxLength(150), Required]
		public string Description
		{
			get { return GetValue<string>(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}
		public static readonly PropertyData DescriptionProperty = RegisterProperty("Description", typeof(string));
	}
}
