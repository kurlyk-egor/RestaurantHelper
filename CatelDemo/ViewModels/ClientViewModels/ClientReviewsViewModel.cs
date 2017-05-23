using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.Services;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Reviews;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.ViewModels.ClientViewModels
{
	using Catel.MVVM;
	using System;
	using System.Threading.Tasks;

	public class ClientReviewsViewModel : ViewModelBase
	{
		private Action _refreshReviewsAction;
		private readonly IViewModel _parentViewModel;
		private readonly User _user;

		public ClientReviewsViewModel(IViewModel parentViewModel, User user)
		{
			_parentViewModel = parentViewModel;
			_user = user;

			BackCommand = new Command(OnBackCommandExecute);
			RefreshCommand = new Command(OnRefreshCommandExecute);
			MyReviewsCommand = new Command(OnMyReviewsCommandExecute);
			AllReviewsCommand = new Command(OnAllReviewsCommandExecute);
			AddReviewCommand = new Command(OnAddReviewCommandExecute);
			EditReviewCommand = new Command(OnEditReviewCommandExecute, OnAnyReviewCommandCanExecute);
			DeleteReviewCommand = new Command(OnDeleteReviewCommandExecute, OnAnyReviewCommandCanExecute);
			SelectionChangedCommand = new Command(OnSelectionChangedCommandExecute);

			// показываем все отзывы
			OnAllReviewsCommandExecute();
		}

		public ObservableCollection<ClientReview> ClientReviews
		{
			get { return GetValue<ObservableCollection<ClientReview>>(ClientReviewsProperty); }
			set { SetValue(ClientReviewsProperty, value); }
		}
		public static readonly PropertyData ClientReviewsProperty = RegisterProperty("ClientReviews", typeof(ObservableCollection<ClientReview>),
			new ObservableCollection<ClientReview>());


		public ClientReview SelectedClientReview
		{
			get { return GetValue<ClientReview>(SelectedClientReviewProperty); }
			set { SetValue(SelectedClientReviewProperty, value); }
		}
		public static readonly PropertyData SelectedClientReviewProperty = RegisterProperty("SelectedClientReview", typeof(ClientReview));

		public string AdminAnswer
		{
			get { return GetValue<string>(AdminAnswerProperty); }
			set { SetValue(AdminAnswerProperty, value); }
		}
		public static readonly PropertyData AdminAnswerProperty = RegisterProperty("AdminAnswer", typeof(string), "Ничего не выбрано");

		public bool IsMyReviews
		{
			get { return GetValue<bool>(IsMyReviewsProperty); }
			set { SetValue(IsMyReviewsProperty, value); }
		}
		public static readonly PropertyData IsMyReviewsProperty = RegisterProperty("IsMyReviews", typeof(bool), false);

		public string ToolTipText
		{
			get { return GetValue<string>(ToolTipTextProperty); }
			set { SetValue(ToolTipTextProperty, value); }
		}
		public static readonly PropertyData ToolTipTextProperty = RegisterProperty("ToolTipText", typeof(string), string.Empty);


		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}

		public Command SelectionChangedCommand { get; private set; }
		private void OnSelectionChangedCommandExecute()
		{
			RefreshAdminAnswer();
		}

		public Command MyReviewsCommand { get; private set; }
		private void OnMyReviewsCommandExecute()
		{
			ClientReviews.Clear();
			((ICollection<ClientReview>)ClientReviews).AddRange(new Repository<ClientReview>().GetCollection()
				.Where(cr => cr.UserId == _user.Id));
			_refreshReviewsAction = OnMyReviewsCommandExecute;
			IsMyReviews = true;
		}

		public Command AllReviewsCommand { get; private set; }
		private void OnAllReviewsCommandExecute()
		{
			ClientReviews.Clear();
			((ICollection<ClientReview>)ClientReviews).AddRange(new Repository<ClientReview>().GetCollection());
			_refreshReviewsAction = OnAllReviewsCommandExecute;
			IsMyReviews = false;
		}

		public Command AddReviewCommand { get; private set; }
		private void OnAddReviewCommandExecute()
		{
			var visualizer = DependencyResolver.Resolve<IUIVisualizerService>();
			visualizer.ShowDialog(new AddReviewViewModel(user: _user));

			_refreshReviewsAction();
		}

		public Command EditReviewCommand { get; private set; }
		private void OnEditReviewCommandExecute()
		{
			var visualizer = DependencyResolver.Resolve<IUIVisualizerService>();
			visualizer.ShowDialog(new AddReviewViewModel(SelectedClientReview));
			int id = SelectedClientReview.Id;
			_refreshReviewsAction();
			SelectedClientReview = ClientReviews.FirstOrDefault(cr => cr.Id == id);
		}

		public Command DeleteReviewCommand { get; private set; }
		private void OnDeleteReviewCommandExecute()
		{
			var reviewsRepo = new Repository<ClientReview>();
			var answersRepo = new Repository<ManagerAnswer>();

			var answer = answersRepo.GetItem(SelectedClientReview.AnswerId);

			reviewsRepo.Delete(SelectedClientReview);
			if (answer != null)
			{
				answersRepo.Delete(answer);
			}

			reviewsRepo.SaveChanges();
			answersRepo.SaveChanges();

			_refreshReviewsAction();
		}

		private bool OnAnyReviewCommandCanExecute()
		{
			if (SelectedClientReview == null)
			{
				ToolTipText = "Не выбран отзыв";
				return false;
			}
			if (!IsMyReviews)
			{
				ToolTipText = "Удалять/редактировать отзывы можно только во вкладке 'МОИ ОТЗЫВЫ'";
				return false;
			}
			if (SelectedClientReview.AnswerId != 0)
			{
				ToolTipText = "Администратор уже ответил на этот отзыв, его нельзя изменить";
				return false;
			}
			ToolTipText = "Изменить/удалить выбранный отзыв";
			return true;
		}

		public Command BackCommand { get; private set; }
		private void OnBackCommandExecute()
		{
			IViewModel root = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();
			root.ChangePage(_parentViewModel);
		}

		public Command RefreshCommand { get; private set; }
		private void OnRefreshCommandExecute()
		{
			_refreshReviewsAction();
		}

		private void RefreshAdminAnswer()
		{
			if (SelectedClientReview == null)
			{
				AdminAnswer = "Ничего не выбрано";
				return;
			}
			if (SelectedClientReview.AnswerId == 0)
			{
				AdminAnswer = "Администратор еще не ответил на этот отзыв";
				return;
			}

			var r = new Repository<ManagerAnswer>();
			var answer = r.GetItem(SelectedClientReview.AnswerId);

			AdminAnswer = (answer != null) ? answer.Text : "Нет ответа";
		}
	}
}
