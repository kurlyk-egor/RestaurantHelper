using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.ValidationRules
{
	public class ExistingLoginValidationRule : ValidationRule
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string str = (value ?? "").ToString();
			if(str.Length < 3) return ValidationResult.ValidResult; // нет смысла проверять

			bool isExist = ((UserRepository)_unitOfWork.Users).IsExistLogin(str);

			return isExist
				? new ValidationResult(false, "Пользователь с таким логином уже зарегистрирован")
				: ValidationResult.ValidResult;
		}
	}
}
