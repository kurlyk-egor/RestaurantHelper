using System;
using System.Collections.Generic;

namespace RestaurantHelper.Services.Interfaces
{
    public interface IRepositoryBase <T> : IDisposable
        where T : class
    {
        void RefreshRepository();
        List<T> GetCollection();
        T GetItem(int id);
        void Insert(T item);
        void Delete(T item);
        void Update(T item);     
        bool SaveChanges();
    }
}
