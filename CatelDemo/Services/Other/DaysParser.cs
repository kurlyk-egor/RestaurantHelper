using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Other
{
	using System;

	public class DaysParser
	{
		public string ParseDaysToString(Days days)
		{
			string result = string.Empty;

			if((days & Days.Monday) == Days.Monday) result += "ПН, ";
			if ((days & Days.Tuesday) == Days.Tuesday) result += "ВТ, ";
			if ((days & Days.Wednesday) == Days.Wednesday) result += "СР, ";
			if ((days & Days.Thursday) == Days.Thursday) result += "ЧТ, ";
			if ((days & Days.Friday) == Days.Friday) result += "ПТ, ";
			if ((days & Days.Saturday) == Days.Saturday) result += "СБ, ";
			if ((days & Days.Sunday) == Days.Sunday) result += "ВС, ";

			result = result.Substring(0, result.Length - 2);
			return result;
		}
		/// <summary>
		/// метод считает логическую сумму флагов установленных дней
		/// </summary>
		/// <returns>возвращает 0, если ни один флаг не установлен</returns>
		public Days GetDays(bool d1, bool d2, bool d3, bool d4, bool d5, bool d6, bool d7)
		{
			Days result = 0;

			if (d1) result |= Days.Monday;
			if (d2) result |= Days.Tuesday;
			if (d3) result |= Days.Wednesday;
			if (d4) result |= Days.Thursday;
			if (d5) result |= Days.Friday;
			if (d6) result |= Days.Saturday;
			if (d7) result |= Days.Sunday;

			return result;
		}

		public bool IsCheckedDay(string str, Days day)
		{
			switch (day)
			{
				case Days.Monday:
					return str.Contains("ПН");
				case Days.Tuesday:
					return str.Contains("ВТ");
				case Days.Wednesday:
					return str.Contains("СР");
				case Days.Thursday:
					return str.Contains("ЧТ");
				case Days.Friday:
					return str.Contains("ПТ");
				case Days.Saturday:
					return str.Contains("СБ");
				case Days.Sunday:
					return str.Contains("ВС");
			}

			return false;
		}
	}

}
