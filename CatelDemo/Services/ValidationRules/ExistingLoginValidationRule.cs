using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.Services.ValidationRules
{
	public class ExistingLoginValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string str = (value ?? "").ToString();
			if(str.Length < 3) return ValidationResult.ValidResult; // нет смысла проверять

			bool isExist = new Repository<User>().GetCollection().Exists(u => u.Login == str);

			return isExist
				? new ValidationResult(false, "Пользователь с таким логином уже зарегистрирован")
				: ValidationResult.ValidResult;
		}
	}
}
