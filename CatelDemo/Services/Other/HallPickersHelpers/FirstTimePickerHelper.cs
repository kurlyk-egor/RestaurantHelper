using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHelper.Services.Other.HallPickersHelpers
{
	/// <summary>
	/// Класс считает подходящие значения в первом TimePicker
	/// </summary>
	class FirstTimePickerHelper
	{
		private readonly DateTime _date;
		public string FirstTime { get; set; }
		public string StartFirstTime { get; set; }

		public FirstTimePickerHelper(string curDate)
		{
			_date = DateTime.Parse(curDate);
			CalcStartFirstTimeAndFirstTime();
		}

		private void CalcStartFirstTimeAndFirstTime()
		{
			if (_date == DateTime.Today)
			{
				int nowHour = DateTime.Now.Hour;
				nowHour = (nowHour < 8) ? 8 : nowHour + 1;
				FirstTime = StartFirstTime = $"{nowHour}:00";
			}
			else
			{
				FirstTime = StartFirstTime = "8:00";
			}
		}
	}
}
