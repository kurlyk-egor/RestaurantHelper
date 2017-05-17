﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Logging;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.Services.Other
{
	/// <summary>
	/// класс выбирает время добавляемой брони
	/// </summary>
	class AdminReservationsCreator
	{
		private readonly Reservation _reservation;
		public Reservation GetReservation(int tableId)
		{
			FillCurrentReservationForTable(tableId);
			return _reservation;
		}

		private int _first, _last;
		private string _firstTime, _lastTime;

		public AdminReservationsCreator()
		{
			_reservation = new Reservation();
		}

		/// <summary>
		/// определить доступность брони для выбранного столика
		/// </summary>
		/// <param name="tableId"> ИД столика</param>
		public bool IsReservationNotAvailable(int tableId)
		{
			FillCurrentReservationForTable(tableId);

			// получаем список броней, отфильтрованный для нужного столика и текущего дня
			var reservations = new Repository<Reservation>().GetCollection()
				.Where(r => r.TableId == tableId && r.Day == DateTime.Today);

			DateTime first = DateTime.Parse(_firstTime);
			DateTime last = DateTime.Parse(_lastTime);

			// существующие и добавляемая брони не должны пересекаться
			// попытка получить хоть одну такую бронь из списка
			var any = reservations.Any(r =>
				(r.FirstTime.Hour >= first.Hour && r.LastTime.Hour <= last.Hour) ||
				(r.FirstTime.Hour >= first.Hour && r.FirstTime.Hour < last.Hour) ||
				(r.LastTime.Hour > first.Hour && r.LastTime.Hour <= last.Hour) );

			// если найдено хоть одно пересечение, бронь недоступна
			return any;
		}
		/// <summary>
		/// проверка времени, когда еще или уже нельзя бронировать столики
		/// </summary>
		/// <param name="tableId">ИД столика</param>
		public bool IsIllegalReservaionTime(int tableId)
		{
			DateTime now = DateTime.Now;
			return now.Hour < 8 || now.Hour > 22;
		}

		public string GetTimeString() => $"{_firstTime,2} - {_lastTime,2}";

		private void FillCurrentReservationForTable(int tableId)
		{
			DateTime current = DateTime.Now;

			// 8 : 30 - c 8 до 9
			// 8 : 50 - с 8 до 10
			_first = current.Hour;
			_last  = current.Minute < 40 ? current.Hour + 1  : current.Hour + 2;


			_firstTime = $"{_first}:00";
			_lastTime = $"{_last}:00";

			_reservation.Day = DateTime.Today;
			_reservation.FirstTime = DateTime.Parse(_firstTime);
			_reservation.LastTime = DateTime.Parse(_lastTime);
			_reservation.TableId = tableId;
			_reservation.UserId = 0;		// 0 - админ
		}
	}
}
