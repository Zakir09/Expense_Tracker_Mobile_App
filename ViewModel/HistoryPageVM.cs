using ExpenseTracker.Controls;
using ExpenseTracker.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ExpenseTracker.ViewModel
{
    public class HistoryPageVM : BaseViewModel
	{
		private bool canDelete;
		private bool canEdit;
		private ObservableCollection<HistoryModel> historyList;

		public ObservableCollection<HistoryModel> HistoryList { get => historyList; set { SetProperty(ref historyList, value); } }
		public System.Windows.Input.ICommand OnHistory { get; protected set; }
		public System.Windows.Input.ICommand OnButtomTab { get; protected set; }
		public System.Windows.Input.ICommand SwipeCommand { get; protected set; }

		public bool CanDelete { get => canDelete; set { SetProperty(ref canDelete, value); } }
		public bool CanEdit { get => canEdit; set { SetProperty(ref canEdit, value); } }

		public HistoryPageVM()
		{
			RunCommands(); 
		}

		internal void GetHistory()
		{
			var histories = AllModels.GetHistoryList(); if(histories != null)
			{
				foreach (var history in histories)
				{
					if (!history.isIncome && !string.IsNullOrEmpty(history.imageUrl))
					{
						byte[] imageData = ImageStorage.LoadImageFromLocalStorage(history.id);
						if (imageData != null && imageData.Length > 0)
							history.PhotoImageSource = ImageSource.FromStream(() => new MemoryStream(imageData));
					}
				}
				HistoryList = new ObservableCollection<HistoryModel>(histories);
			}
		}

		private void RunCommands()
		{
			OnButtomTab = new Command<string>((string par) =>
			{
				switch (par)
				{
					case "home":
						Application.Current.MainPage = new NavigationPage(new MainPage());
						break;
					case "add":
						Application.Current.MainPage = new NavigationPage(new AddPage());
						break;
					case "history":
						Application.Current.MainPage = new NavigationPage(new HistoryPage());
						break;
					case "reminders":
						Application.Current.MainPage = new NavigationPage(new RemindersPage());
						break;
					case "edit":
						CanEdit = true; CanDelete = false;
						ShowMessage("You can now edit the item by swipping left");
						break;
					case "delete":
						CanDelete = true; CanEdit = false;
						ShowMessage("You can now delete the item by swipping left");
						break;
				}
			});
			OnHistory = new Command<HistoryModel>(async (HistoryModel historyModel) =>
			{
				await Application.Current.MainPage.Navigation.PushAsync(new DetailsPage(historyModel));
			});
			SwipeCommand = new Command<HistoryModel>((HistoryModel historyModel) =>
			{
				if (CanEdit || CanDelete) TryEditDelete(historyModel);
			});
		}

		private async void TryEditDelete(HistoryModel historyModel)
		{
			if (CanDelete)
			{
				var histories = AllModels.GetHistoryList();
				if (histories != null)
				{
					var toDel = histories.Where(p => p.id == historyModel.id).FirstOrDefault();
					if (toDel != null) histories.Remove(toDel);
					HistoryList = new ObservableCollection<HistoryModel>(histories);
					Preferences.Set(AllModels.HISTORY_DB_ID, JsonConvert.SerializeObject(histories));
					ShowMessage("Data Deleted Successfully");
				}
			}
			else if (CanEdit)
			{
				Application.Current.MainPage = new NavigationPage(new AddPage(historyModel));
			}
		}

		private async void ShowMessage(string msg)
		{
			await Application.Current.MainPage.DisplayAlert("Alert", msg, "OK");
		}
	}
}
