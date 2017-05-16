using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;

namespace RestaurantHelper.Models.Actions
{
	class DiscountAction : ModelBase
	{
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
