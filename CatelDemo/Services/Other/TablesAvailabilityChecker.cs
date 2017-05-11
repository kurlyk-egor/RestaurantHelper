using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.Services.Other
{
	public class TablesAvailabilityChecker
	{
		private readonly List<Table> _tables;
		private readonly ReservationRepository _reservations;

		public TablesAvailabilityChecker(List<Table> tables)
		{
			_tables = tables;
			_reservations = ReservationRepository.GetRepositoryInstance();
		}

		public void FillAvailabilities(DateTime first, DateTime second, DateTime day)
		{
			var reservations = _reservations.GetCollection()
				.Where(r =>	  r.Day.Date == day.Date && 
				((r.FirstTime >= first && r.LastTime <= second) ||
				(r.FirstTime >= first && r.FirstTime < second) ||
				(r.LastTime > first && r.LastTime <= second)));

			ResetValues();

			foreach (var r in reservations) // у недоступных значение меняем на false
			{
				_tables.Find(t => t.Number == r.TableId).Availability = false;
			}
		}

		public void ResetValues()
		{
			for (int i = 0; i < _tables.Count; i++) // заполняем все true
			{
				_tables[i].Availability = true;
			}
		}

		// брони на сегодня
		public List<Reservation> GetDaylyReservationsForTable(DateTime day, int tableNumber)
		{
			return _reservations.GetCollection().Where(r => r.Day.Date == day.Date && r.TableId == tableNumber).ToList();
		} 
	}
}
