﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Collections;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Services.Other
{
	public class TablesAvailabilityChecker
	{
		private readonly List<Table> _tables;
		private readonly IRepository<Reservation> _reservations;

		public TablesAvailabilityChecker(List<Table> tables)
		{
			_tables = tables;
			_reservations = new Repository<Reservation>();
		}

		public void FillAvailabilities(string firstTime, string secondTime, string currentDay)
		{
			DateTime first, second, day;
			ResetValues(); // все true

			if (DateTime.TryParse(firstTime, out first) &&
			    DateTime.TryParse(secondTime, out second) &&
			    DateTime.TryParse(currentDay, out day))
			{ // если и день, и время выбраны
				var reservations = _reservations.GetCollection()
					.Where(r => r.Day.Date == day.Date &&
					            ((r.FirstTime.Hour >= first.Hour && r.LastTime.Hour <= second.Hour) ||
					             (r.FirstTime.Hour >= first.Hour && r.FirstTime.Hour < second.Hour) ||
					             (r.LastTime.Hour > first.Hour && r.LastTime.Hour <= second.Hour)));

				foreach (var r in reservations) // у недоступных значение меняем на false
				{
					_tables.Find(t => t.Number == r.TableId).Availability = false;
				}
			}
		}

		public void ResetValues()
		{
			foreach (Table t in _tables)
			{
				t.Availability = true;
			}
		}

		// брони переданный день
		public List<Reservation> GetDaylyReservationsForTable(string dayStr, int tableNumber)
		{
			DateTime day;
			List <Reservation> returnList = new List<Reservation>();

			if (DateTime.TryParse(dayStr, out day))
			{
				returnList = _reservations.GetCollection().Where(r => r.Day.Date == day.Date && r.TableId == tableNumber).ToList();
			}

			return returnList;
		}

		public ObservableCollection<Reservation> GetTodayReservationsForTable(int tableNumber)
		{
			
			ObservableCollection<Reservation> reservations = new ObservableCollection<Reservation>();
			// получить брони на сегодня для столика
			((ICollection<Reservation>)reservations).AddRange(GetDaylyReservationsForTable(DateTime.Today.ToShortDateString(), tableNumber));

			return reservations;
		}

		/// <summary>
		/// <returns>возвращает столики, доступные прямо сейчас</returns>
		/// </summary>
		/// <returns></returns>
		public List<Table> GetAvailableNowTables()
		{
			var first = $"{DateTime.Now.Hour}:00";
			var second = $"{DateTime.Now.AddHours(1).Hour}:00";
			var date = DateTime.Today.ToShortDateString();

			FillAvailabilities(first, second, date);
			return _tables;
		}
	}
}
