using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Catel.Data;
using System.Linq;
using RestaurantHelper.Models.Additional;

namespace RestaurantHelper.Models
{
    public class User : MyModelBase
    {
        public int Id
        {
            get { return GetValue< int>(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

		[MinLength(5), MaxLength(20)]
		public string Login
        {
            get { return GetValue<string>( LoginProperty); }
            set { SetValue( LoginProperty, value); }
        }
        public static readonly PropertyData LoginProperty = RegisterProperty("Login", typeof(string));

		[MinLength(1), MaxLength(20)]
		public string Password
        {
            get { return GetValue<string>(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly PropertyData PasswordProperty = RegisterProperty("Password", typeof(string));

		[MinLength(5), MaxLength(30)]
		public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string));

		[MinLength(5), MaxLength(15)]
		public string Phone
        {
            get { return GetValue<string>(PhoneProperty); }
            set { SetValue(PhoneProperty, value); }
        }
        public static readonly PropertyData PhoneProperty = RegisterProperty("Phone", typeof(string));

		/// <summary>
		/// все заказы пользователя
		/// </summary>
		public virtual List<Order> Orders { get; set; }
    }
}
