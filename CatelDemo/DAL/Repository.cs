using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.DAL
{
	public class Repository<T> : IRepository<T> 
		where T:class
	{
		private DbContext _db;

		public Repository()
		{
			_db
		}
		public void Dispose()
		{
			_d
		}

		public string PathToFile { get; set; }

		public void RefreshRepository()
		{
			try
			{
				using (FileStream stream = new FileStream(PathToFile, FileMode.OpenOrCreate))
				{
					_items = (List<T>)_serializer.Deserialize(stream);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		public List<T> GetCollection()
		{
			return _items;
		}

		public T GetItem(int id)
		{
			throw new Exception();
			//return _items.Find(u => u.Id == id);
		}

		public void Insert(T item)
		{
			//item.Id = NextId();
			_items.Add(item);
		}

		public void Delete(T item)
		{
			//T findedItem = _items.Find(i => i.Id == item.Id);
			//_items.Remove(findedItem);
		}

		public void Update(T item)
		{
			//T findedItem = _items.Find(u => u.Id == item.Id);
			//_items.Remove(findedItem);
			//_items.Add(item);
			//_items.Sort((i1,i2) => i1.Id - i2.Id);
		}

		public bool SaveChanges()
		{
			try
			{
				using (FileStream stream = new FileStream(PathToFile, FileMode.Create))
				{
					_serializer.Serialize(stream, _items);
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool IsExistItem(T item)
		{
			return _items.Contains(item);
		}

		public int NextId()
		{
			//if (_items.Count == 0) return 1;
			//int max = _items.Max(item => item.Id);
			//return ++max;
			return 1;
		}
	}
}
