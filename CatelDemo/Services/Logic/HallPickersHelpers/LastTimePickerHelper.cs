using System;

namespace RestaurantHelper.Services.Logic.HallPickersHelpers
{
	/// <summary>
	/// Класс считает подходящие значения во втором TimePicker
	/// </summary>
	class LastTimePickerHelper
	{
		private readonly DateTime _date;
		public string LastTime { get; set; }
		public string StartLastTime { get; set; }

		public LastTimePickerHelper(string firstTime)
		{
			if (firstTime != null)
			{
				_date = DateTime.Parse(firstTime);
				CalcStartLastTimeAndLastTime();
			}
		}

		private void CalcStartLastTimeAndLastTime()
		{
			int currentFirstTime = _date.Hour;
			StartLastTime = LastTime = $"{currentFirstTime + 1}:00";
		}
	}
}
