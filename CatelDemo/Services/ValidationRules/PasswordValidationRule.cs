using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RestaurantHelper.Services.ValidationRules
{
	public class PasswordValidationRule :ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return Validator.IsValidString((string)value, 5, 20)
				? ValidationResult.ValidResult
				: new ValidationResult(false, "Поле должно содержать минимум 5 символов");
		}
	}
}
