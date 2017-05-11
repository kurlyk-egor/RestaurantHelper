using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Catel.Data;
using Catel.Runtime.Serialization;

namespace RestaurantHelper.Models
{
	[Serializable]
	public class Dish : ModelBase
	{
		public Dish()
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

		public int Price
		{
			get { return GetValue<int>(PriceProperty); }
			set { SetValue(PriceProperty, value); }
		}
		public static readonly PropertyData PriceProperty = RegisterProperty("Price", typeof(int));

		public int PictureId
		{
			get { return GetValue<int>(PictureIdProperty); }
			set { SetValue(PictureIdProperty, value); }
		}
		public static readonly PropertyData PictureIdProperty = RegisterProperty("PictureId", typeof(int));

		[XmlIgnore]
		public int Quantity
		{
			get { return GetValue<int>(QuantityProperty); }
			set { SetValue(QuantityProperty, value); }
		}
		public static readonly PropertyData QuantityProperty = RegisterProperty("Quantity", typeof(int), 1);

		public override string ToString()
		{
			return $"{Id}: {Name} | {Price} x {Quantity}";
		}
	}
}
