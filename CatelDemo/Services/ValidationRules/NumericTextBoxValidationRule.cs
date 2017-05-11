using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RestaurantHelper.Services.ValidationRules
{
	class NumericTextBoxValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return (value ?? "").ToString().ToCharArray().Count(c => !char.IsDigit(c)) > 0
				? new ValidationResult(false, "This field must contain digits only.")
				: ValidationResult.ValidResult;
		}
	}
}
