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
	class ReservationRepository : IRepositoryBase<Reservation>
	{
		private const string PATH = @"..\..\CatelDemo\Files\reservations.xml";
		private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<Reservation>));
		private List<Reservation> _reservations = new List<Reservation>();
		private static ReservationRepository _instance;

		private ReservationRepository()
		{
			RefreshRepository();
		}

		public static ReservationRepository GetRepositoryInstance()
		{
			return _instance ?? (_instance = new ReservationRepository());
		}
		public void Dispose()
		{
		}

		public void RefreshRepository()
		{
			using (FileStream stream = new FileStream(PATH, FileMode.OpenOrCreate))
			{
				_reservations = (List<Reservation>)_serializer.Deserialize(stream);
			}
		}

		public List<Reservation> GetCollection()
		{
			return _reservations;
		}

		public Reservation GetItem(int id)
		{
			return _reservations.Find(u => u.Id == id);
		}

		public void Insert(Reservation item)
		{
			item.Id = NextId();
			_reservations.Add(item);
		}

		public void Delete(Reservation item)
		{
			_reservations.Remove(item);
		}

		public void Update(Reservation item)
		{
			Reservation reservation = _reservations.Find(u => u.Id == item.Id);
			_reservations.Remove(reservation);
			_reservations.Add(item);
		}

		public bool SaveChanges()
		{
			try
			{
				using (FileStream stream = new FileStream(PATH, FileMode.Create))
				{
					_serializer.Serialize(stream, _reservations);
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
			int max = _reservations.Max(table => table.Id);
			return ++max;
		}
	}
}
