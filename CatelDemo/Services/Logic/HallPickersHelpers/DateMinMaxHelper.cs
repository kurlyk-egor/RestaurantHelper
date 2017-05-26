using System;

namespace RestaurantHelper.Services.Logic.HallPickersHelpers
{
	/// <summary>
	/// Класс считает минимальный и максимальный день в DatePicker
	/// </summary>
	class DateMinMaxHelper
	{
		private readonly DateTime _todayDate;
		public string Minimum { get; set; }
		public string Maximum { get; set; }

		public DateMinMaxHelper()
		{
			_todayDate = DateTime.Now;
			CalcMinAndMaxDays();
		}

		private void CalcMinAndMaxDays()
		{
			DateTime minimum = _todayDate;

			if (_todayDate.Hour >= 22)
			{
				minimum = minimum.AddDays(1);
			}
			Minimum = $"{minimum.Month}.{minimum.Day}.{minimum.Year}";

			DateTime maximum = minimum.AddDays(10).Date;
			Maximum = $"{maximum.Month}.{maximum.Day}.{maximum.Year}";
		}
	}
}
