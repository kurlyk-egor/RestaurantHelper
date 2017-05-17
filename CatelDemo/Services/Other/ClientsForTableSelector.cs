using System;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Services.Other
{
	/// <summary>
	/// класс для операций с клиентом, привязанным к выбранному столику
	/// </summary>
	class ClientsForTableSelector
	{
		public string GetUserNameForSelectedTable(int tableId)
		{
			var reservations = new Repository<Reservation>().GetCollection();
			var users = new Repository<User>().GetCollection();
			var time = DateTime.Now;
			

			// выделить клиента, который в этот промежуток занимает столик
			var reservation = reservations.Find(r =>  
					r.Day.Day == time.Day &&			// день совпал
					r.FirstTime.Hour <= time.Hour &&	// время в диапазоне брони
					r.LastTime.Hour > time.Hour &&
					r.TableId == tableId);				// для определенного столика

			// нету текущей брони
			if (reservation == null) return string.Empty;

			var user = users.Find(u => u.Id == reservation.UserId);
			return (user == null) ? "" : user.Login;
		}
	}
}
