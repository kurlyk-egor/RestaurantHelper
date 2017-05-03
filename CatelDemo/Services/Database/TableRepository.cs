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
	class TableRepository : IRepositoryBase<Table>
	{
		private const string PATH = @"..\..\CatelDemo\Files\tables.xml";
		private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<Table>));
		private List<Table> _tables = new List<Table>();
		private static TableRepository _instance;

		private TableRepository()
		{
			RefreshRepository();
		}

		public static TableRepository GetRepositoryInstance()
		{
			return _instance ?? (_instance = new TableRepository());
		}
		public void Dispose()
		{
		}

		public void RefreshRepository()
		{
			using (FileStream stream = new FileStream(PATH, FileMode.Open))
			{
				_tables = (List<Table>)_serializer.Deserialize(stream);
			}
		}

		public List<Table> GetCollection()
		{
			return _tables;
		}

		public Table GetItem(int id)
		{
			return _tables.Find(u => u.Id == id);
		}

		public void Insert(Table item)
		{
			item.Id = NextId();
		}

		public void Delete(Table item)
		{
			_tables.Remove(item);
		}

		public void Update(Table item)
		{
			Table table = _tables.Find(u => u.Id == item.Id);
			_tables.Remove(table);
			_tables.Add(item);
		}

		public bool SaveChanges()
		{
			try
			{
				using (FileStream stream = new FileStream(PATH, FileMode.Create))
				{
					_serializer.Serialize(stream, _tables);
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
			int max = _tables.Max(table => table.Id);
			return ++max;
		}
	}
}
