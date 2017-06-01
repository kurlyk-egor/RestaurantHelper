using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;
using RestaurantHelper.Models.Additional;


namespace RestaurantHelper.Models
{
	public class Order : MyModelBase
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

		[Required]
		public int ReservationId
		{
			get { return GetValue<int>(ReservationIdProperty); }
			set { SetValue(ReservationIdProperty, value); }
		}
		public static readonly PropertyData ReservationIdProperty = RegisterProperty("ReservationId", typeof(int));

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		[ForeignKey("ReservationId")]
		public virtual Reservation Reservation { get; set; }

		/// <summary>
		/// для данного заказа - все заказанные блюда
		/// </summary>
		public virtual List<OrderedDish> OrderedDishes { get; set; }

		public override string ToString()
		{
			return $"ЗАКАЗ N {Id :5}  | ДАТА: {Reservation.Day.ToShortDateString()} / {Reservation.FirstTime.ToShortTimeString()}-{Reservation.LastTime.ToShortTimeString()} | СТОЛИК {Reservation.Table.Number}";
		}
	}
}
