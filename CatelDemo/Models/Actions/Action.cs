using Catel.Data;

namespace RestaurantHelper.Models.Actions
{
	public enum ActionType
	{
		/// <summary>
		/// скидка (в процентах)
		/// название проводимой акции
		/// основной текст
		/// товар, к которому применяется (или признак выбор всех, типа "день рождения"
		/// время действия
		/// </summary>
		Discount,

		/// <summary>
		/// превышение суммы
		/// название акции
		/// товар, который будет бесплатно даваться
		/// непосредственно сумма превышения 
		/// </summary>
		AmountExcess
	}
	public class Action : ModelBase
	{
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		 
	}
}
