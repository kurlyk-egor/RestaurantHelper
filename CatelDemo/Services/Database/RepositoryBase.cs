using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Services.Database
{
	class RepositoryBase<T> : IRepositoryBase<T> 
		where T:class, IHaveId
	{
		private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<T>));
		private List<T> _items = new List<T>();

		public RepositoryBase()
		{
			PathToFile = $@"..\..\CatelDemo\Files\{typeof(T).Name.ToLower()}.xml";
			RefreshRepository();
		}
		public void Dispose()
		{
		}

		public string PathToFile { get; set; }

		public void RefreshRepository()
		{
			using (FileStream stream = new FileStream(PathToFile, FileMode.OpenOrCreate))
			{
				_items = (List<T>)_serializer.Deserialize(stream);
			}
		}

		public List<T> GetCollection()
		{
			return _items;
		}

		public T GetItem(int id)
		{
			return _items.Find(u => u.Id == id);
		}

		public void Insert(T item)
		{
			item.Id = NextId();
			_items.Add(item);
		}

		public void Delete(T item)
		{
			_items.Remove(item);
		}

		public void Update(T item)
		{
			T findedItem = _items.Find(u => u.Id == item.Id);
			_items.Remove(findedItem);
			_items.Add(item);
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

		private int NextId()
		{
			int max = _items.Max(item => item.Id);
			return ++max;
		}
	}
}
