using System.Collections.Generic;
using System.Linq;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Services.Other
{
    class AuthorizationChecker
    {
        private IEnumerable<User> _users; 
        private User _user;
        private readonly IRepository<User> _userRepository;

        public AuthorizationChecker(User user)
        {
            _user = user;
			_userRepository = new Repository<User>();
        }

        public bool IsMatchUser()
        {
            Refresh();
            var find = _users.ToList().Find(u => u.Login == _user.Login && u.Password == _user.Password);
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
            _users = _userRepository.GetCollection();
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
