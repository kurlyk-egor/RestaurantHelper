﻿using System;
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
using CatelDemo.Models;
using CatelDemo.ViewModels;
using CatelDemo.ViewModels.ClientViewModels;

namespace CatelDemo.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            // TODO: пока что пропускаем авторизацию и запускаем главное окно, передавая существующего юзера
            //CurrentPage = new StartWindowViewModel(this);
            // TODO: user есть в списке
            CurrentPage = new ClientMainViewModel(new User() {Id = 6, Login = "Viking", Password="sukasobaka", Name = "Егор Курлыкин", Phone = "375298933692"});

            // собственные пространства имен
            var viewModelLocator = ServiceLocator.Default.ResolveType<IViewModelLocator>();
            var viewLocator = ServiceLocator.Default.ResolveType<IViewLocator>();

            viewModelLocator.NamingConventions.Add("[AS].ViewModels.ClientViewModels.[VW]ViewModel");
            viewLocator.NamingConventions.Add("[AS].Views.ClientViews.[VM]View");
        }

        // сюда биндится текущий UserControl в ContentControle окна
        public IViewModel CurrentPage
        {
            get { return GetValue<IViewModel>(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }
        public static readonly PropertyData CurrentPageProperty = RegisterProperty("CurrentPage", typeof(IViewModel));

        public override string Title => "CourseProject";

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