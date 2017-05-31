using System;
using System.ComponentModel.DataAnnotations.Schema;
using Catel.Data;

namespace RestaurantHelper.Models.Additional
{
	[Serializable]
	public class MyModelBase : ModelBase
	{
		[NotMapped]
		public new bool IsDirty { get; set; }

		[NotMapped]
		public new bool IsReadOnly { get; set; }
	}
}
