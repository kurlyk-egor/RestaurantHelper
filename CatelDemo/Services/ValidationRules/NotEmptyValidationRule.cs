﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RestaurantHelper.Services.ValidationRules
{
	public class NotEmptyValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return (value ?? "").ToString().Length > 0
				? ValidationResult.ValidResult
				: new ValidationResult(false, "Поле пусто");
		}
	}
}
