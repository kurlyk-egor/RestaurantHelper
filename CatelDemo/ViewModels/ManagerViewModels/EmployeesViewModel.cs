using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	public class EmployeesViewModel : ViewModelBase
	{
		public EmployeesViewModel()
		{
		}

		public ObservableCollection<Employee> Employees
		{
			get { return GetValue<ObservableCollection<Employee>>(EmployeesProperty); }
			set { SetValue(EmployeesProperty, value); }
		}
		public static readonly PropertyData EmployeesProperty = RegisterProperty("Employees", typeof(ObservableCollection<Employee>), 
			new ObservableCollection<Employee>());


		public Employee SelectedEmployee
		{
			get { return GetValue<Employee>(SelectedEmployeeProperty); }
			set { SetValue(SelectedEmployeeProperty, value); }
		}
		public static readonly PropertyData SelectedEmployeeProperty = RegisterProperty("SelectedEmployee", typeof(Employee));

		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}

		private void RefreshEmployeesCollection()
		{
			Employees.Clear();
			((ICollection<Employee>)Employees).AddRange(new Repository<Employee>().GetCollection());
		}
	}
}
