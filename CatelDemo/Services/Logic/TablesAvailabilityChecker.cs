using System;
using System.Collections.Generic;
using System.Linq;
using Catel.Collections;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Logic
{
	public class TablesAvailabilityChecker
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private readonly FastObservableCollection<Table> _tables;

		public TablesAvailabilityChecker(FastObservableCollection<Table> tables)
		{
			_tables = tables;
			_tables.AutomaticallyDispatchChangeNotifications = true;
		}

		public void SetTablesAvailabilities(string firstTime, string secondTime, string currentDay)
		{
			DateTime first, second, day;
			ResetValues(); // все столики доступны

			// если выбраны и время и дата
			if (DateTime.TryParse(firstTime, out first) && DateTime.TryParse(secondTime, out second) && DateTime.TryParse(currentDay, out day))
			{
				var reservations = _unitOfWork.Reservations.GetAll()
					.Where(r => r.Day.Date == day.Date && IsReservationInTheTimeRange(r, first, second));

				foreach (var r in reservations) // у столиков, попавших в этот набор, меням значение на false
				{
					_tables.First(t => t.Id == r.TableId).Availability = false;
				}
			}
		}

		private void ResetValues()
		{
			foreach (Table t in _tables)
			{
				t.Availability = true;
			}
		}

		/// <summary>
		/// получить все брони указанного столика за указанный день
		/// </summary>
		public FastObservableCollection<Reservation> GetDaylyReservationsForTable(string dayStr, int tableId)
		{
			DateTime day;
			FastObservableCollection <Reservation> returnList = new FastObservableCollection<Reservation>();

			if (DateTime.TryParse(dayStr, out day))
			{
				// дополняем список броней
				 returnList.AddItems(_unitOfWork.Reservations.GetAll().Where(r => r.Day.Date == day.Date && r.TableId == tableId));
			}

			return returnList;
		}

		/// <summary>
		/// получить все брони столика на сегодня
		/// </summary>
		public FastObservableCollection<Reservation> GetTodayReservationsForTable(int tableNumber)
		{
			// получить брони на сегодня для столика
			return GetDaylyReservationsForTable(DateTime.Today.ToShortDateString(), tableNumber);
		}

		/// <summary>
		/// обновляет текущую доступность столиков 
		/// </summary>
		public void TablesAvailableNowRefresh()
		{
			var first = $"{DateTime.Now.Hour}:00";
			var second = $"{DateTime.Now.AddHours(1).Hour}:00";
			var date = DateTime.Today.ToShortDateString();

			SetTablesAvailabilities(first, second, date);
		}

		private bool IsReservationInTheTimeRange(Reservation r, DateTime first, DateTime second)
		{
			return
				// время брони полностью внутри диапазона
				(r.FirstTime.Hour >= first.Hour && r.LastTime.Hour <= second.Hour) ||
				// начало в диапазоне
				(r.FirstTime.Hour >= first.Hour && r.FirstTime.Hour < second.Hour) ||
				// конец в диапазоне
				(r.LastTime.Hour > first.Hour && r.LastTime.Hour <= second.Hour);
		}
	}
}
