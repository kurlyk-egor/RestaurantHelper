using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Catel;
using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Views;
using Catel.Services;
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
            CurrentPage = new StartWindowViewModel(this);
			// user есть в списке
			// TODO: запуск сразу клиентской части
			//CurrentPage = new ClientMainViewModel(new User() {Id = 3, Login = "Viking", Password="sobaka", Name = "Курлык", Phone = "375298933692"});
			// // TODO: запуск сразу из под админа
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
		}

        // сюда биндится текущий UserControl в ContentControle окна
        public IViewModel CurrentPage
        {
            get { return GetValue<IViewModel>(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }
        public static readonly PropertyData CurrentPageProperty = RegisterProperty("CurrentPage", typeof(IViewModel));

        public override string Title => "RestaurantHelperClient";

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
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
}