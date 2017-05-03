using System;
using System.Collections.Generic;
using Catel.Data;

namespace RestaurantHelper.Models
{
    [Serializable]
    public class User : ModelBase
    {
        public User()
        {
        }
        public int Id
        {
            get { return GetValue< int>(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));
        public string Login
        {
            get { return GetValue<string>( LoginProperty); }
            set { SetValue( LoginProperty, value); }
        }
        public static readonly PropertyData LoginProperty = RegisterProperty("Login", typeof(string));

        public string Password
        {
            get { return GetValue<string>(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly PropertyData PasswordProperty = RegisterProperty("Password", typeof(string));

        
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string));

        public string Phone
        {
            get { return GetValue<string>(PhoneProperty); }
            set { SetValue(PhoneProperty, value); }
        }
        public static readonly PropertyData PhoneProperty = RegisterProperty("Phone", typeof(string));


        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            // TODO: если не сделаю валидацию лучше, пусть будет хоть какая нибудь
            if (string.IsNullOrEmpty(Name))
            {
                validationResults.Add(FieldValidationResult.CreateError(NameProperty, "Name cannot be empty"));
            }

            if (string.IsNullOrEmpty(Phone))
            {
                validationResults.Add(FieldValidationResult.CreateError(PhoneProperty, "Phone cannot be empty"));
            }
        }

        public override string ToString()
        {
            return $"{Id} {Login} {Password}\n {Name} {Phone} ";
        }
    }
}
