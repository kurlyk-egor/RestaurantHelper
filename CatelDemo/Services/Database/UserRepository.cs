using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Interfaces;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace RestaurantHelper.Services.Database
{
    class UserRepository : IRepositoryBase<User>
    {
        private const string PATH = @"..\..\CatelDemo\books.xml";
        private static UserRepository _instance;
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<User>));
        private List<User> _users;

        private UserRepository()
        {
            RefreshRepository();
        }

        public static UserRepository GetRepositoryInstance()
        {
	        return _instance ?? (_instance = new UserRepository());
        }

	    public void RefreshRepository()
        {
            using (FileStream stream = new FileStream(PATH, FileMode.Open))
            {
                _users = (List<User>)_serializer.Deserialize(stream);
            }
        }

        public List<User> GetCollection()
        {
            return _users;
        }

        public User GetItem(int itemId)
        {
            return _users.Find(u => u.Id == itemId);
        }

        public void Insert(User item)
        {
            item.Id = NextId();
            _users.Add(item);
        }

        public void Delete(User item)
        {
            _users.Remove(item);
        }

        public void Update(User item)
        {
            User user = _users.Find(u => u.Id == item.Id);
            _users.Remove(user);
            _users.Add(item);          
        }

        public bool SaveChanges()
        {
            try
            {
                using (FileStream stream = new FileStream(PATH, FileMode.Create))
                {
                    _serializer.Serialize(stream, _users);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public void Dispose()
        {
        }

        private int NextId()
        {
            int max = _users.Max(user => user.Id);
            return ++max;
        }
    }
}
