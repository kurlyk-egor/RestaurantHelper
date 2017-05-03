using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatelDemo.Models;
using CatelDemo.Services.Database;
using CatelDemo.Services.Interfaces;

namespace CatelDemo.Services.Other
{
    class AuthorizationChecker
    {
        private IEnumerable<User> _users; 
        private User _user;
        private readonly UserRepository _repository;

        public AuthorizationChecker(User user)
        {
            this._user = user;
            _repository = UserRepository.GetRepositoryInstance();
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
            _users = _repository.GetCollection();
        }

        public User GetUser()
        {
            return _user;
        }
    }
}
