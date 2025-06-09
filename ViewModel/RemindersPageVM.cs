using ExpenseTracker.Controls;
using ExpenseTracker.Views;
using Microsoft.Maui.Handlers;
using Newtonsoft.Json; 
using System.Collections.ObjectModel;
using System.Windows.Input;
using Plugin.LocalNotification;

namespace ExpenseTracker.ViewModel
{
	public class RemindersPageVM : BaseViewModel
	{
		private bool canDelete;
		private bool canEdit;
		private string selectedType = "Bill & Utility";
		private DateTime selectedDate = DateTime.Now;
		private string description; 
		NotificationModel? notModel = null;
		private ObservableCollection<NotificationModel>? notList;
		private TimeSpan selectedTime = DateTime.Now.TimeOfDay;

		public DateTime SelectedDate { get => selectedDate; set { SetProperty(ref selectedDate, value); } }
		public string SelectedType { get => selectedType; set { SetProperty(ref selectedType, value); } }
		public List<string> TypeList { get; set; } 
		public string Description { get => description; set { SetProperty(ref description, value); } }
		public TimeSpan SelectedTime { get => selectedTime; set { SetProperty(ref selectedTime, value); } }
		public ObservableCollection<NotificationModel>? NotList { get => notList; set { SetProperty(ref notList, value); } }
		public NotificationModel SelectedNot { get; set; }
		public bool CanDelete { get => canDelete; set { SetProperty(ref canDelete, value); } }
		public bool CanEdit { get => canEdit; set { SetProperty(ref canEdit, value); } }

		public ICommand OnButtomTab { get; protected set; }
		public ICommand SwipeCommand { get; protected set; }
		public ICommand PickerCommand { get; protected set; } 
		 
		public RemindersPageVM()
		{
			TypeList = new List<string>() { "Bill & Utility", "Shopping", "Food", "Debt", "Others" };
			RunCommands(); NotList = new ObservableCollection<NotificationModel>(AllModels.GetNotList());
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
					case "save":
						TrySaving();
						break;
				}
			});
			PickerCommand = new Command<Picker>((Picker picker) => picker.Focus()); 
			SwipeCommand = new Command<NotificationModel>((NotificationModel notModel) =>
			{
				if (CanEdit || CanDelete) TryEditDelete(notModel);
			});
		}
		private async void TryEditDelete(NotificationModel notModel)
		{
			if (CanDelete)
			{
				var vNotList = AllModels.GetNotList();
				if (vNotList != null)
				{
					var toDel = vNotList.Where(p => p.id == notModel.id).FirstOrDefault();
					if (toDel != null) vNotList.Remove(toDel);
					NotList = new ObservableCollection<NotificationModel>(vNotList);
					Preferences.Set(AllModels.NOTIFICATION_DB_ID, JsonConvert.SerializeObject(vNotList));
					ShowMessage("Notification Deleted Successfully");
				}
			}
			else if (CanEdit)
			{
				this.notModel = notModel; SelectedDate = notModel.dateTime;
				SelectedType = notModel.type; Description = notModel.description;
				SelectedTime = notModel.dateTime.TimeOfDay;
			}
		}
		private async void TrySaving()
		{
			if (string.IsNullOrEmpty(Description)) ShowMessage("Pls Enter Description"); 
			else
			{
				try
				{
					string dataID = notModel != null ? notModel.id : DateTime.Now.ToString("yyyyMMddHHmmssfff"); 
					DateTime notDate = new DateTime(DateOnly.FromDateTime(SelectedDate), TimeOnly.FromTimeSpan(SelectedTime));
					NotificationModel notification = new()
					{
						id = dataID, dateTime = notDate, description = Description,  type = SelectedType
					};  
					var vNotList = AllModels.GetNotList(); if (vNotList != null)
					{
						if (notModel != null)
						{  
							var toDel = vNotList.Where(p => p.id == notModel.id).FirstOrDefault();
							if (toDel != null) vNotList.Remove(toDel);
						}
						vNotList.Add(notification); 
					}
					bool savedOnline = await TrySavingOnline(notification);
					if (savedOnline)
					{
						Preferences.Set(AllModels.NOTIFICATION_DB_ID, JsonConvert.SerializeObject(vNotList));
						await Application.Current.MainPage.DisplayAlert("Success", "Notification Saved Successfully", "OK");
						if (notModel != null) notModel = null; Description = ""; 
						NotList = new ObservableCollection<NotificationModel>(AllModels.GetNotList());
					} 
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
				}
			}
		}

		private async Task<bool> TrySavingOnline(NotificationModel notModel )
		{
			try
			{
				if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false) 
					await LocalNotificationCenter.Current.RequestNotificationPermission(); 

				var notification = new NotificationRequest
				{
					NotificationId = int.Parse(notModel.dateTime.ToString("MMddHHmmss")),
					Title = notModel.type, Description = notModel.description,
					ReturningData = notModel.id, //BadgeNumber = 42,
					Schedule =
					{
						NotifyTime = notModel.dateTime
						//NotifyRepeatInterval = TimeSpan.FromDays(1) 
					}
				};
				await LocalNotificationCenter.Current.Show(notification); 
				return true;
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			return false;
		}

		private async void ShowMessage(string msg)
		{
			await Application.Current.MainPage.DisplayAlert("Alert", msg, "OK");
		}

	}
}
