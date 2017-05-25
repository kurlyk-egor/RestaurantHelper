using System;
using Catel.Data;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Other;

namespace RestaurantHelper.ViewModels.ManagerViewModels.AdditionalWindows
{
	using Catel.MVVM;
	using System.Threading.Tasks;

	public class AddEmployeeViewModel : ViewModelBase
	{
		private readonly DaysParser _daysParser;
		private readonly UnitOfWork _unitOfWork; 

		//делегат, который выбирает, добавить или редактировать работника
		private readonly Action<Employee> _addOrEditAction;

		public AddEmployeeViewModel(Employee employee = null)
		{
			_daysParser = new DaysParser();
			_unitOfWork = UnitOfWork.GetInstance();

			OkCommand = new Command(OnOkCommandExecute, OnOkCommandCanExecute);

			if (employee == null)
			{
				_addOrEditAction = AddEmployee;
				employee = new Employee();
			}
			else
			{
				_addOrEditAction = EditEmployee;
				SetDaysProperties(employee);
			}
			Employee = employee;
		}

		#region CheckBoxProperties
		public bool Monday
		{
			get { return GetValue<bool>(MondayProperty); }
			set { SetValue(MondayProperty, value); }
		}
		public static readonly PropertyData MondayProperty = RegisterProperty("Monday", typeof(bool), false);

		public bool Tuesday
		{
			get { return GetValue<bool>(TuesdayProperty); }
			set { SetValue(TuesdayProperty, value); }
		}
		public static readonly PropertyData TuesdayProperty = RegisterProperty("Tuesday", typeof(bool), false);

		public bool Wednesday
		{
			get { return GetValue<bool>(WednesdayProperty); }
			set { SetValue(WednesdayProperty, value); }
		}
		public static readonly PropertyData WednesdayProperty = RegisterProperty("Wednesday", typeof(bool), false);

		public bool Thursday
		{
			get { return GetValue<bool>(ThursdayProperty); }
			set { SetValue(ThursdayProperty, value); }
		}
		public static readonly PropertyData ThursdayProperty = RegisterProperty("Thursday", typeof(bool), false);

		public bool Friday
		{
			get { return GetValue<bool>(FridayProperty); }
			set { SetValue(FridayProperty, value); }
		}
		public static readonly PropertyData FridayProperty = RegisterProperty("Friday", typeof(bool), false);

		public bool Saturday
		{
			get { return GetValue<bool>(SaturdayProperty); }
			set { SetValue(SaturdayProperty, value); }
		}
		public static readonly PropertyData SaturdayProperty = RegisterProperty("Saturday", typeof(bool), false);

		public bool Sunday
		{
			get { return GetValue<bool>(SundayProperty); }
			set { SetValue(SundayProperty, value); }
		}
		public static readonly PropertyData SundayProperty = RegisterProperty("Sunday", typeof(bool), false);

		#endregion


		[Model]
		public Employee Employee
		{
			get { return GetValue<Employee>(EmployeeProperty); }
			private set { SetValue(EmployeeProperty, value); }
		}
		public static readonly PropertyData EmployeeProperty = RegisterProperty("Employee", typeof(Employee));


		[ViewModelToModel("Employee")]
		public string Name
		{
			get { return GetValue<string>(NameProperty); }
			set { SetValue(NameProperty, value); }
		}
		public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string));

		[ViewModelToModel("Employee")]
		public int Age
		{
			get { return GetValue<int>(AgeProperty); }
			set { SetValue(AgeProperty, value); }
		}
		public static readonly PropertyData AgeProperty = RegisterProperty("Age", typeof(int));

		[ViewModelToModel("Employee")]
		public string Position
		{
			get { return GetValue<string>(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}
		public static readonly PropertyData PositionProperty = RegisterProperty("Position", typeof(string));


		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}

		public Command OkCommand { get; private set; }
		private bool OnOkCommandCanExecute()
		{
			return Age > 15 && (_daysParser.GetDays(Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday) != 0);
		}
		private async void OnOkCommandExecute()
		{
			// вызываем делегат для редактирования или добавления работника
			_addOrEditAction(Employee);
			await CloseViewModelAsync(true);
		}


		private void SetDaysProperties(Employee employee)
		{
			Monday = _daysParser.IsCheckedDay(employee.WorkDays, Days.Monday);
			Tuesday = _daysParser.IsCheckedDay(employee.WorkDays, Days.Tuesday);
			Wednesday = _daysParser.IsCheckedDay(employee.WorkDays, Days.Wednesday);
			Thursday = _daysParser.IsCheckedDay(employee.WorkDays, Days.Thursday);
			Friday = _daysParser.IsCheckedDay(employee.WorkDays, Days.Friday);
			Saturday = _daysParser.IsCheckedDay(employee.WorkDays, Days.Saturday);
			Sunday = _daysParser.IsCheckedDay(employee.WorkDays, Days.Sunday);
		}

		private void AddEmployee(Employee employee)
		{
			Days days = _daysParser.GetDays(Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday);
			employee.WorkDays = _daysParser.ParseDaysToString(days);
			_unitOfWork.Employees.Insert(employee);
			_unitOfWork.SaveChanges();
		}

		private void EditEmployee(Employee employee)
		{
			Days days = _daysParser.GetDays(Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday);
			employee.WorkDays = _daysParser.ParseDaysToString(days);
			_unitOfWork.Employees.Update(employee);
			_unitOfWork.SaveChanges();
		}
	}
}
