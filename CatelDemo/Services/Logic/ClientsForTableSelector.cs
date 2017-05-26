using System;
using System.Linq;
using RestaurantHelper.DAL;

namespace RestaurantHelper.Services.Logic
{
	/// <summary>
	/// класс для операций с клиентом, привязанным к выбранному столику
	/// </summary>
	class ClientsForTableSelector
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		public string GetUserNameForSelectedTable(int tableId)
		{
			var reservations = _unitOfWork.Reservations.GetAll().ToList();
			var users = _unitOfWork.Users.GetAll().ToList();
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
