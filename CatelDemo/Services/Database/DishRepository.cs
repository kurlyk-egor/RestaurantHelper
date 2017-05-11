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
	class DishRepository : IRepositoryBase<Dish>
	{
		private const string PATH = @"..\..\CatelDemo\Files\dishes.xml";
		private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<Dish>));
		private List<Dish> _dishes = new List<Dish>();
		private static DishRepository _instance;

		private DishRepository()
		{
			RefreshRepository();
		}

		public static DishRepository GetRepositoryInstance()
		{
			return _instance ?? (_instance = new DishRepository());
		}
		public void Dispose()
		{
		}

		public void RefreshRepository()
		{
			using (FileStream stream = new FileStream(PATH, FileMode.OpenOrCreate))
			{
				_dishes = (List<Dish>)_serializer.Deserialize(stream);
			}
		}

		public List<Dish> GetCollection()
		{
			return _dishes;
		}

		public Dish GetItem(int id)
		{
			return _dishes.Find(u => u.Id == id);
		}

		public void Insert(Dish item)
		{
			item.Id = NextId();
			_dishes.Add(item);
		}

		public void Delete(Dish item)
		{
			_dishes.Remove(item);
		}

		public void Update(Dish item)
		{
			Dish dish = _dishes.Find(u => u.Id == item.Id);
			_dishes.Remove(dish);
			_dishes.Add(item);
		}

		public bool SaveChanges()
		{
			try
			{
				using (FileStream stream = new FileStream(PATH, FileMode.Create))
				{
					_serializer.Serialize(stream, _dishes);
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
			int max = _dishes.Max(table => table.Id);
			return ++max;
		}
	}
}
