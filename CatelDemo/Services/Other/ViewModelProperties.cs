using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Other
{
	public class ViewModelProperties
	{
		public ViewModelProperties(string firstTime, string lastTime, string dateText, Table selectedTable)
		{
			FirstTime = firstTime;
			LastTime = lastTime;
			DateText = dateText;
			SelectedTable = selectedTable;
		}

		public string FirstTime { get; set; }
		public string LastTime { get; set; }
		public string DateText { get; set; }
		public Table SelectedTable { get; set; }
	}
}
