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
			string str = (value ?? "").ToString();
			long i;
			bool hasLegalChars = long.TryParse(str, out i);

			return hasLegalChars ?
				  ValidationResult.ValidResult
				: new ValidationResult(false, "Поле должно содержать только цифры");
		}
	}
}
