using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Catel.Collections;
using Catel.Data;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Reviews;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	using Catel.MVVM;
	using System.Threading.Tasks;

	public class ReviewsViewModel : ViewModelBase
	{
		const string NO_ANSWER = "Ответа еще нет.";
		public ReviewsViewModel()
		{
			RefreshReviwsCollection();

			ClearTextCommand = new Command(OnClearTextCommandExecute);
			SaveAnswerCommand = new Command(OnSaveAnswerCommandExecute);
			SelectionChangedCommand = new Command(OnSelectionChangedCommandExecute);
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
		public static readonly PropertyData AdminAnswerProperty = RegisterProperty("AdminAnswer", typeof(string), NO_ANSWER);




		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}


		public Command ClearTextCommand { get; private set; }
		private void OnClearTextCommandExecute()
		{
			if (AdminAnswer == NO_ANSWER)
			{
				AdminAnswer = string.Empty;
			}
		}

		public Command SelectionChangedCommand { get; private set; }
		private void OnSelectionChangedCommandExecute()
		{
			RefreshAdminAnswer();
		}

		public Command SaveAnswerCommand { get; private set; }
		private bool CanSaveAnswerCommandExecute()
		{
			return !string.IsNullOrEmpty(AdminAnswer) && 
					AdminAnswer != NO_ANSWER && 
					SelectedClientReview != null;
		}
		private void OnSaveAnswerCommandExecute()
		{
			var answersRepo = new Repository<ManagerAnswer>();
			var reviewsRepo = new Repository<ClientReview>();

			// исключить несколько ответов на один и тот же отзыв
			var answer = answersRepo.GetCollection().Find(a => a.ReviewId == SelectedClientReview.Id);

			if (answer != null) // существует - обновить
			{
				answer.Text = AdminAnswer;
				answer.DateTime = DateTime.Now;
				answer.ReviewId = SelectedClientReview.Id;

				answersRepo.Update(answer);
			}
			else			// не существует - добавить
			{
				answersRepo.Insert(new ManagerAnswer
				{
					Text = AdminAnswer,
					DateTime = DateTime.Now,
					ReviewId = SelectedClientReview.Id
				});
			}
			answersRepo.SaveChanges();


			var review = reviewsRepo.GetItem(SelectedClientReview.Id);
			// обратная связь
			review.AnswerId = answersRepo.GetCollection().Find(a => a.ReviewId == SelectedClientReview.Id).Id;
			reviewsRepo.Update(review);
			reviewsRepo.SaveChanges();

			RefreshReviwsCollection();

			// выбираем тот же отзыв
			SelectedClientReview = ClientReviews.First(c => c.AnswerId == review.AnswerId);
		}


		private void RefreshReviwsCollection()
		{
			ClientReviews.Clear();
			((ICollection<ClientReview>) ClientReviews).AddRange(new Repository<ClientReview>().GetCollection());
		}

		private void RefreshAdminAnswer()
		{
			if (SelectedClientReview == null || SelectedClientReview.AnswerId == 0)
			{
				AdminAnswer = NO_ANSWER;
				return;
			}

			var r = new Repository<ManagerAnswer>();
			var answer = r.GetItem(SelectedClientReview.AnswerId);

			AdminAnswer =  (answer != null) ? answer.Text : NO_ANSWER;
		}
	}
}
