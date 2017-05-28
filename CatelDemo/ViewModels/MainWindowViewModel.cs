using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using Catel;
using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Views;
using Catel.Services;
using RestaurantHelper.DAL;
using RestaurantHelper.ViewModels;
using RestaurantHelper.Models;
using RestaurantHelper.ViewModels.AuthorizationViewModels;
using RestaurantHelper.ViewModels.ClientViewModels;
using RestaurantHelper.ViewModels.ManagerViewModels;

namespace RestaurantHelper.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;
  
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
			// TODO: запуск авторизации
            //CurrentPage = new StartWindowViewModel(this);

			// TODO: запуск сразу клиентской части
	        GetClient();

			// TODO: запуск сразу из под админа
			//CurrentPage = new ManagerMainViewModel();

			// собственные пространства имен
			var viewModelLocator = ServiceLocator.Default.ResolveType<IViewModelLocator>();
            var viewLocator = ServiceLocator.Default.ResolveType<IViewLocator>();

            viewModelLocator.NamingConventions.Add("[AS].ViewModels.ClientViewModels.[VW]ViewModel");
            viewLocator.NamingConventions.Add("[AS].Views.ClientViews.[VM]View");

            viewModelLocator.NamingConventions.Add("[AS].ViewModels.ClientViewModels.OrderViewModels.[VW]ViewModel");
            viewLocator.NamingConventions.Add("[AS].Views.ClientViews.OrderViews.[VM]View");

            viewModelLocator.NamingConventions.Add("[AS].ViewModels.AuthorizationViewModels.[VW]ViewModel");
            viewLocator.NamingConventions.Add("[AS].Views.AuthorizationViews.[VM]View");

			viewModelLocator.NamingConventions.Add("[AS].ViewModels.ManagerViewModels.[VW]ViewModel");
			viewLocator.NamingConventions.Add("[AS].Views.ManagerViews.[VM]View");

			viewModelLocator.NamingConventions.Add("[AS].ViewModels.ManagerViewModels.Actions.[VW]ViewModel");
			viewLocator.NamingConventions.Add("[AS].Views.ManagerViews.Actions.[VM]View");

			viewModelLocator.NamingConventions.Add("[AS].ViewModels.ManagerViewModels.AdditionalWindows.[VW]ViewModel");
			viewLocator.NamingConventions.Add("[AS].Views.ManagerViews.AdditionalWindows.[VM]View");
		}

	    private void GetClient()
	    {
		    var user = new User() {Id = 1, Login = "Viking", Password = "sobaka", Name = "Курлык", Phone = "33333"};

			//var uow = UnitOfWork.GetInstance();
			//uow.Users.Insert(user);
			//uow.SaveChanges();

			CurrentPage = new ClientMainViewModel(user);
		}

	    // сюда биндится текущий UserControl в ContentControle окна
        public IViewModel CurrentPage
        {
            get { return GetValue<IViewModel>(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }
        public static readonly PropertyData CurrentPageProperty = RegisterProperty("CurrentPage", typeof(IViewModel));

        public override string Title => "RestaurantHelper";

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
	        var uow = UnitOfWork.GetInstance();
			uow.SaveChanges();
			
            await base.CloseAsync();
        }

        public void ChangeCurrentPage(IViewModel pageToChange)
        {
            CurrentPage = pageToChange;
        }
    }
}


// TODO: хорошо бы обработать неудачный каст, но это потом. я ведь знаю, что делаю
public static class ViewModelExtension
{
    public static void ChangePage(this IViewModel iViewModel, IViewModel pageToChange)
    {
        ((MainWindowViewModel)iViewModel).ChangeCurrentPage(pageToChange);
    }

	/// <summary>
	/// метод расширения для IViewModel, отображающий заставку
	/// </summary>
	/// <param name="iViewModel">расширяемый тип</param>
	/// <param name="pageToShow">отображаемая заставка</param>
	/// <param name="milliseconds">время показа заставки</param>
	/// /// <param name="pageToChange">страница, которая загрузится после заставки</param>
	public static async void ChangePageWithDialog(this IViewModel iViewModel, IViewModel pageToShow, int milliseconds, IViewModel pageToChange = null)
	{
		ServiceLocator.Default.ResolveType<IUIVisualizerService>().Show(pageToShow);
		Thread.Sleep(milliseconds);

		// если нужно - переходим на другую страницу
		if (pageToChange != null)
		{
			((MainWindowViewModel)iViewModel).ChangeCurrentPage(pageToChange);
		}
		await pageToShow.CloseViewModelAsync(true);
	}
}