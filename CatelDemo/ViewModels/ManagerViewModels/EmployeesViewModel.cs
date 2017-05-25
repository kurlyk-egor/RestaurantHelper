using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;
using RestaurantHelper.ViewModels.ManagerViewModels.AdditionalWindows;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	public class EmployeesViewModel : ViewModelBase
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		public EmployeesViewModel()
		{
			RefreshEmployeesCollection();

			AddEmployeeCommand = new Command(OnAddEmployeeCommandExecute);
			EditEmployeeCommand = new Command(OnEditEmployeeCommandExecute, OnAnyEmployeeCommandCanExecute);
			DeleteEmployeeCommand = new Command(OnDeleteEmployeeCommandExecute, OnAnyEmployeeCommandCanExecute);
		}

		public FastObservableCollection<Employee> Employees
		{
			get { return GetValue<FastObservableCollection<Employee>>(EmployeesProperty); }
			set { SetValue(EmployeesProperty, value); }
		}
		public static readonly PropertyData EmployeesProperty = RegisterProperty("Employees", typeof(FastObservableCollection<Employee>), 
			new FastObservableCollection<Employee>());


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
			_unitOfWork.Employees.Delete(SelectedEmployee.Id);
			_unitOfWork.SaveChanges();
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
			Employees.AddItems(_unitOfWork.Employees.GetAll());
		}
	}
}
