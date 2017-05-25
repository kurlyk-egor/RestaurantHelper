using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Catel.Data;
using RestaurantHelper.Models.Additional;

namespace RestaurantHelper.Models
{
	public class Table : MyModelBase
	{
		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		[Required]
		public int Number
		{
			get { return GetValue<int>(NumberProperty); }
			set { SetValue(NumberProperty, value); }
		}
		public static readonly PropertyData NumberProperty = RegisterProperty("Number", typeof(int));

		public int SeatsNumber
		{
			get { return GetValue<int>(SeatsNumberProperty); }
			set { SetValue(SeatsNumberProperty, value); }
		}
		public static readonly PropertyData SeatsNumberProperty = RegisterProperty("SeatsNumber", typeof(int));

		[Required]
		public int Top
		{
			get { return GetValue<int>(TopProperty); }
			set { SetValue(TopProperty, value); }
		}
		public static readonly PropertyData TopProperty = RegisterProperty("Top", typeof(int));

		[Required]
		public int Left
		{
			get { return GetValue<int>(LeftProperty); }
			set { SetValue(LeftProperty, value); }
		}
		public static readonly PropertyData LeftProperty = RegisterProperty("Left", typeof(int));

		[Required]
		public int Type
		{
			get { return GetValue<int>(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}
		public static readonly PropertyData TypeProperty = RegisterProperty("Type", typeof(int));

		[NotMapped]
		public bool Availability
		{
			get { return GetValue<bool>(AvailabilityProperty); }
			set { SetValue(AvailabilityProperty, value); }
		}
		public static readonly PropertyData AvailabilityProperty = RegisterProperty("Availability", typeof(bool), true);
	}
}
