using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RestaurantHelper.Services.ValidationRules
{
	public class StringLengthMinMaxValidationRule : ValidationRule
	{
		public int Min { get; set; } = 1;
		public int Max { get; set; } = 150;

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			var str = (value ?? "").ToString();

			if (str.Length < Min)
			{
				return new ValidationResult(false, $"Длина поля должна быть не менее {Min} символов");
			}
			if (str.Length > Max)
			{
				return new ValidationResult(false, $"Длина поля должна быть не более {Max} символов");
			}

			return ValidationResult.ValidResult;
		}
	}
}
