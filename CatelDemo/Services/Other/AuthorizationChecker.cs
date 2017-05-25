using System.Collections.Generic;
using System.Linq;
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Other
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
            var find = _users?.ToList().Find(u => u.Login == _user.Login && u.Password == _user.Password);
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
            var find = _users.ToList().Find(u => u.Login == _user.Login && u.Password != _user.Password);
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
			// TODO: переделать адекватно
		    return _user.Login == "admin" && _user.Password == "admin";
	    }
    }
}
