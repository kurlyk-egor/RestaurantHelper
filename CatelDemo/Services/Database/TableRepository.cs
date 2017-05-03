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
		private const string PATH = @"..\..\CatelDemo\tables.xml";
		private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<User>));
		private List<Table> _tables = new List<Table>();
		private static TableRepository _instance;

		private TableRepository()
		{
			var tables = new List<Table>
			{
				new Table {Id=0, Number = 1, SeatsNumber = 4},
				new Table {Id=1, Number = 2, SeatsNumber = 4},
				new Table {Id=2, Number = 3, SeatsNumber = 4},
				new Table {Id=3, Number = 4, SeatsNumber = 8},
				new Table {Id=4, Number = 5, SeatsNumber = 4},
				new Table {Id=5, Number = 6, SeatsNumber = 4},
				new Table {Id=6, Number = 7, SeatsNumber = 6},
				new Table {Id=7, Number = 8, SeatsNumber = 4},
				new Table {Id=8, Number = 9, SeatsNumber = 4},
				new Table {Id=9, Number = 10, SeatsNumber = 6},
			};

			using (FileStream stream = new FileStream(PATH, FileMode.CreateNew));
			{
				
			}
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
			throw new NotImplementedException();
		}

		public List<Table> GetCollection()
		{
			throw new NotImplementedException();
		}

		public Table GetItem(int id)
		{
			throw new NotImplementedException();
		}

		public void Insert(Table item)
		{
			throw new NotImplementedException();
		}

		public void Delete(Table item)
		{
			throw new NotImplementedException();
		}

		public void Update(Table item)
		{
			throw new NotImplementedException();
		}

		public bool SaveChanges()
		{
			throw new NotImplementedException();
		}
	}
}
