using System.Collections.Generic;
using System.Linq;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantHelper.Services.Logic
{
    class AuthorizationChecker
    {
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private IEnumerable<User> _users; 
        private User _user;

		public AuthorizationChecker(User user)
        {
            _user = user;
        }

        public bool IsMatchUser()
        {
            Refresh();
            var find = _users?.ToList().Find(u => 
				u.Login == _user.Login && 
				// в бд лежит MD-5 хэш
				u.Password == GetHashPassword(_user.Password));

            if (find == null)
            {
                return false;
            }
            _user = find;
            return true;
        }

        public bool IsExistsLogin()
        {
            Refresh();
            var find = _users.ToList().Find(u => 
				u.Login == _user.Login);
            if (find == null)
            {
                return false;
            }
            return true;
        }

        private void Refresh()
        {
            _users = _unitOfWork.Users.GetAll();
        }

        public User GetUser()
        {
            return _user;
        }

	    public bool IsAdmin()
	    {
			Refresh();
		    return  GetHashPassword(_user.Login) == "19a2854144b63a8f7617a6f225019b12" && 
					GetHashPassword(_user.Password) == "c3d04b34d1f4b27230ff0d7770a3663f";
	    }

		public string GetHashPassword(string s)
		{
			//переводим строку в байт-массив 
			byte[] bytes = Encoding.Unicode.GetBytes(s);

			//создаем объект для получения средств шифрования  
			MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();

			//вычисляем хеш-представление в байтах  
			byte[] byteHash = csp.ComputeHash(bytes);

			string hash = string.Empty;

			//формируем одну цельную строку из массива  
			foreach (byte b in byteHash)
				hash += $"{b:x2}";

			return hash;
		}
	}
}
