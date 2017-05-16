using System;
using System.Collections.Generic;
using Catel.Data;
using System.Linq;
using RestaurantHelper.Services.Interfaces;
using Xceed.Wpf.DataGrid.Converters;

namespace RestaurantHelper.Models
{
    [Serializable]
    public class User : ModelBase, IHaveId
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

	    public static bool IsValidLogin(string login)
	    {
		    return !string.IsNullOrWhiteSpace(login);
	    }

	    public static bool IsValidPhone(string phone)
	    {
		    return !string.IsNullOrWhiteSpace(phone) && phone.ToCharArray().Count(c => !char.IsDigit(c)) == 0;
	    }

        public override string ToString()
        {
            return $"{Id} {Login} {Password}\n {Name} {Phone} ";
        }
    }
}
