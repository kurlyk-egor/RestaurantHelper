using System.Threading.Tasks;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Other;

namespace RestaurantHelper.ViewModels.ManagerViewModels.AdditionalWindows
{
	public class AddReservationViewModel : ViewModelBase
	{
		private readonly Reservation _reservation;

		public AddReservationViewModel(Table table)
		{
			var reservationsCreator = new AdminReservationsCreator();
			_reservation = reservationsCreator.GetReservation(table.Id);

			TableNumber = table.Number;
			TimeString = reservationsCreator.GetTimeString();

			OkCommand = new Command(OnOkCommandExecute);
		}

		public string TimeString
		{
			get { return GetValue<string>(TimeStringProperty); }
			set { SetValue(TimeStringProperty, value); }
		}
		public static readonly PropertyData TimeStringProperty = RegisterProperty("TimeString", typeof(string));

		public int TableNumber
		{
			get { return GetValue<int>(TableNumberProperty); }
			set { SetValue(TableNumberProperty, value); }
		}
		public static readonly PropertyData TableNumberProperty = RegisterProperty("TableNumber", typeof(int));

		public Command OkCommand { get; private set; }

		private async void OnOkCommandExecute()
		{
			// добавляем бронь
			var reservationRepository = new Repository<Reservation>();
			reservationRepository.Insert(_reservation);
			reservationRepository.SaveChanges();
			
			await CloseViewModelAsync(true);
		}
		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}
	}
}
