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
	public class OrderedDishRepository : IRepositoryBase<OrderedDish>
	{
		private const string PATH = @"..\..\CatelDemo\Files\ordered_dishes.xml";
		private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<OrderedDish>));
		private List<OrderedDish> _orderedDishes = new List<OrderedDish>();
		private static OrderedDishRepository _instance;

		private OrderedDishRepository()
		{
			RefreshRepository();
		}

		public static OrderedDishRepository GetRepositoryInstance()
		{
			return _instance ?? (_instance = new OrderedDishRepository());
		}
		public void Dispose()
		{
		}

		public void RefreshRepository()
		{
			using (FileStream stream = new FileStream(PATH, FileMode.OpenOrCreate))
			{
				_orderedDishes = (List<OrderedDish>)_serializer.Deserialize(stream);
			}
		}

		public List<OrderedDish> GetCollection()
		{
			return _orderedDishes;
		}

		public OrderedDish GetItem(int id)
		{
			return _orderedDishes.Find(u => u.Id == id);
		}

		public void Insert(OrderedDish item)
		{
			item.Id = NextId();
			_orderedDishes.Add(item);
		}

		public void Delete(OrderedDish item)
		{
			_orderedDishes.Remove(item);
		}

		public void Update(OrderedDish item)
		{
			OrderedDish orderedDish = _orderedDishes.Find(u => u.Id == item.Id);
			_orderedDishes.Remove(orderedDish);
			_orderedDishes.Add(item);
		}

		public bool SaveChanges()
		{
			try
			{
				using (FileStream stream = new FileStream(PATH, FileMode.Create))
				{
					_serializer.Serialize(stream, _orderedDishes);
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
			if (_orderedDishes.Count == 0) return 1;
			int max = _orderedDishes.Max(table => table.Id);
			return ++max;
		}
	}
}
