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
	public class OrderRepository : IRepositoryBase<Order>
	{
		private const string PATH = @"..\..\CatelDemo\Files\orders.xml";
		private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<Order>));
		private List<Order> _orders = new List<Order>();
		private static OrderRepository _instance;

		private OrderRepository()
		{
			RefreshRepository();
		}

		public static OrderRepository GetRepositoryInstance()
		{
			return _instance ?? (_instance = new OrderRepository());
		}
		public void Dispose()
		{
		}

		public void RefreshRepository()
		{
			using (FileStream stream = new FileStream(PATH, FileMode.OpenOrCreate))
			{
				_orders = (List<Order>)_serializer.Deserialize(stream);
			}
		}

		public List<Order> GetCollection()
		{
			return _orders;
		}

		public Order GetItem(int id)
		{
			return _orders.Find(u => u.Id == id);
		}

		public void Insert(Order item)
		{
			item.Id = NextId();
			_orders.Add(item);
		}

		public void Delete(Order item)
		{
			_orders.Remove(item);
		}

		public void Update(Order item)
		{
			Order order = _orders.Find(u => u.Id == item.Id);
			_orders.Remove(order);
			_orders.Add(item);
		}

		public bool SaveChanges()
		{
			try
			{
				using (FileStream stream = new FileStream(PATH, FileMode.Create))
				{
					_serializer.Serialize(stream, _orders);
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
			int max = _orders.Max(table => table.Id);
			return ++max;
		}
	}
}
