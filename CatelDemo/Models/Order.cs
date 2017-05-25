using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
		public User User { get; set; }

		[ForeignKey("ReservationId")]
		public Reservation Reservation { get; set; }
	}
}
