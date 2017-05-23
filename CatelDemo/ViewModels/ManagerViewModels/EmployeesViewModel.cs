using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.ViewModels.ManagerViewModels.AdditionalWindows;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	public class EmployeesViewModel : ViewModelBase
	{
		public EmployeesViewModel()
		{
			RefreshEmployeesCollection();

			AddEmployeeCommand = new Command(OnAddEmployeeCommandExecute);
			EditEmployeeCommand = new Command(OnEditEmployeeCommandExecute, OnAnyEmployeeCommandCanExecute);
			DeleteEmployeeCommand = new Command(OnDeleteEmployeeCommandExecute, OnAnyEmployeeCommandCanExecute);
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


		public Command AddEmployeeCommand { get; private set; }
		private void OnAddEmployeeCommandExecute()
		{
			var visualizer = DependencyResolver.Resolve<IUIVisualizerService>();
			visualizer.ShowDialog(new AddEmployeeViewModel());
			RefreshEmployeesCollection();
		}

		public Command DeleteEmployeeCommand { get; private set; }
		private void OnDeleteEmployeeCommandExecute()
		{
			var repo = new Repository<Employee>();
			repo.Delete(SelectedEmployee);
			repo.SaveChanges();
			RefreshEmployeesCollection();
		}

		public Command EditEmployeeCommand { get; private set; }
		private void OnEditEmployeeCommandExecute()
		{
			var visualizer = DependencyResolver.Resolve<IUIVisualizerService>();
			visualizer.ShowDialog(new AddEmployeeViewModel(SelectedEmployee));
			RefreshEmployeesCollection();
		}


		private bool OnAnyEmployeeCommandCanExecute()
		{
			return SelectedEmployee != null;
		}

		private void RefreshEmployeesCollection()
		{
			Employees.Clear();
			((ICollection<Employee>)Employees).AddRange(new Repository<Employee>().GetCollection());
		}
	}
}
