using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHelper.Services.ValidationRules
{
	public class Validator
	{
		public string LoginInfo { get; set; }

		public static bool IsValidLogin(string str, int min, int max)
		{
			if(!string.IsNullOrWhiteSpace(str) && str.Length >= min && str.Length <= max;
		}

		public static bool IsValidNumericString(string str)
		{
			return !string.IsNullOrWhiteSpace(str) && str.ToCharArray().Count(c => !char.IsDigit(c)) == 0;
		}
	}
}
