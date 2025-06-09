using ExpenseTracker.Controls;
using ExpenseTracker.Views; 
using Newtonsoft.Json;
using Plugin.LocalNotification;
using System.Windows.Input;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace ExpenseTracker.ViewModel
{
	public class AddPageVM : BaseViewModel
	{ 
		private bool showIncomeView;
		private bool showExpenseView = true;
		private ImageSource? photoImageSource;
		private string selectedType = "Bill & Utility";
		private DateTime selectedDate = DateTime.Now;
		Stream? photoStream = null;
		private string description;
		private decimal? total;
		HistoryModel? historyModel = null;
        private bool isListening;
        public bool IsListening{get => isListening; set => SetProperty(ref isListening, value);}

        public bool ShowIncomeView { get => showIncomeView; set { SetProperty(ref showIncomeView, value); } }
		public bool ShowExpenseView { get => showExpenseView; set { SetProperty(ref showExpenseView, value); } }
		public ImageSource? PhotoImageSource { get => photoImageSource; set { SetProperty(ref photoImageSource, value); } }

		public DateTime SelectedDate { get => selectedDate; set { SetProperty(ref selectedDate, value); } }
		public string SelectedType { get => selectedType; set { SetProperty(ref selectedType, value); } }
		public List<string> TypeList { get; set; }
		public string Description { get => description; set { SetProperty(ref description, value); } }
		public decimal? Total { get => total; set { SetProperty(ref total, value); } }

        private ICommand recordSpeechCommand;
        public ICommand RecordSpeechCommand => recordSpeechCommand ??= new Command(async () => await RecognizeSpeechAsync());
        public ICommand OnButtomTab { get; protected set; }
		public ICommand OnButtonClicked { get; protected set; }
		public ICommand SwipeCommand { get; protected set; }
		public ICommand PickerCommand { get; protected set; } 

		public AddPageVM(HistoryModel? historyModel)
		{
			this.historyModel = historyModel;
			RunCommands(); RunData();
		}

        public async Task<bool> CheckAndRequestMicrophonePermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Microphone>();
            }
            return status == PermissionStatus.Granted;
        }

        private async Task RecognizeSpeechAsync()
        {
            IsListening = true;
            bool hasPermission = await CheckAndRequestMicrophonePermission();
            if (!hasPermission)
            {
                await Application.Current.MainPage.DisplayAlert("Microphone Permission", "Microphone permission is required to use speech-to-text.", "OK");
                return;
            }

            var speechConfig = SpeechConfig.FromSubscription("6399a4e2075e43bca5a50d4331a50545", "uksouth");
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

            var speechRecognitionResult = await recognizer.RecognizeOnceAsync();
            if (speechRecognitionResult.Reason == ResultReason.RecognizedSpeech)
            {
                Description = speechRecognitionResult.Text;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Speech Recognition", "No speech recognized, try again.", "OK");
            }
            IsListening = false;
        }

        private void RunData()
		{
			TypeList = new List<string>() { "Bill & Utility", "Shopping", "Food", "Debt", "Others" };
			if (historyModel != null)
			{
				ShowIncomeViewClick(historyModel.isIncome);
				SelectedDate = historyModel.dateTime; Description = historyModel.description; Total = historyModel.amount;
				if (!historyModel.isIncome)
				{
					SelectedType = historyModel.type;
					if (!string.IsNullOrEmpty(historyModel.imageUrl))
					{
						byte[] imageData = ImageStorage.LoadImageFromLocalStorage(historyModel.id);
						if (imageData != null && imageData.Length > 0) 
							PhotoImageSource = ImageSource.FromStream(() => new MemoryStream(imageData)); 
					}
				}
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
				}
			});
			OnButtonClicked = new Command<string>((string par) =>
			{
				switch (par)
				{ 
					case "income":
						ShowIncomeViewClick(true);
						break;
					case "expense":
						ShowIncomeViewClick(false);
						break;
					case "picture":
						PickPictureCamera();
						break;
					case "save":
						TrySaving();
						break;
				}
			}); 
			SwipeCommand = new Command<string>((string direction) =>
			{
				switch (direction)
				{
					case "Left":
						ShowIncomeViewClick(true);
						break;
					case "Right":
						ShowIncomeViewClick(false);
						break;
				}
			}); 
			PickerCommand = new Command<Picker>((Picker picker) => picker.Focus()); 
		}
		 
		private async void TrySaving()
		{ 
			if (string.IsNullOrEmpty(Description)) ShowMessage("Pls Enter Description");
			else if (Total == null || Total == 0 ) ShowMessage("Pls Enter Valid Total");
			else
			{
				try
				{
					string dataID = historyModel != null ? historyModel.id : DateTime.Now.ToString("yyyyMMddHHmmssfff");
					HistoryModel history = new()
					{
						id = dataID, dateTime = SelectedDate, description = Description, amount = Total,
						isIncome = ShowIncomeView, type = ShowIncomeView ? "Income" : SelectedType 
					};
					var historyList = AllModels.GetHistoryList(); if (historyList != null)
					{
						if (historyModel != null)
						{
							history.imageUrl = historyModel.imageUrl;
							var toDel = historyList.Where(p => p.id == historyModel.id).FirstOrDefault();
							if (toDel != null) historyList.Remove(toDel);
						}
						if (ShowExpenseView && photoStream != null)
						{
							byte[] imageByte = ImageStorage.ConvertStreamToByteArray(photoStream);
							if (imageByte != null && imageByte.Length != 0)
								history.imageUrl = await ImageStorage.SaveImageToLocalStorage(dataID, imageByte);
						}
						historyList.Add(history);
					}
					Preferences.Set(AllModels.HISTORY_DB_ID, JsonConvert.SerializeObject(historyList));
					await Application.Current.MainPage.DisplayAlert("Success", "Data Saved Successfully", "OK");
					//if (historyModel != null) Application.Current.MainPage = new NavigationPage(new HistoryPage());
					Description = ""; Total = null; PhotoImageSource = null; historyModel = null;
					CheckBudgetExceeding();
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
				}
			} 
		}

		private async void CheckBudgetExceeding()
		{
			double? expense = 0;
			var histories = AllModels.GetHistoryList();
			if (histories != null && histories.Count > 0)
			{ 
				if (histories.Where(p => p.isIncome == false).FirstOrDefault() != null)
					expense = (double?)histories.Where(p => p.isIncome == false).Sum(p => p.amount);
			}
			string CurrentBudget = Preferences.Get(AllModels.CURRENT_BUDGET_DB_ID, "0");
			double.TryParse(CurrentBudget, out double budget);
			if (expense > budget)
			{
				try
				{
					if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false) 
						await LocalNotificationCenter.Current.RequestNotificationPermission(); 

					var notification = new NotificationRequest
					{
						NotificationId = 100,
						Title = "Budget Exceeded",
						Description = $"You have expense:{expense} with buget {budget}.",  
					};
					await LocalNotificationCenter.Current.Show(notification); 
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
				}
			}
		}

		private async void ShowMessage(string msg)
		{ 
			await Application.Current.MainPage.DisplayAlert("Alert", msg, "OK");
		}

		private void ShowIncomeViewClick(bool showIncome)
		{
			ShowIncomeView = showIncome ? true : false;
			ShowExpenseView = showIncome ? false : true;
		} 
		private async void PickPictureCamera()
		{
			string camPhoto = await Application.Current.MainPage.DisplayActionSheet("", "", "Cancel", ["Use Camera", "Choose Image"]);
			if (camPhoto == "Use Camera") ChooseImageCamera(true); else if (camPhoto == "Choose Image") ChooseImageCamera(false);
		}

		private async void ChooseImageCamera(bool isCamera)
		{
			try
			{
				FileResult? photo = null;
				if (isCamera)
				{
					await PermissionService.RequestCameraPermissionAsync();
					photo = await MediaPicker.CapturePhotoAsync();
				}
				else
				{
					await PermissionService.RequestStoragePermissionAsync();
					photo = await MediaPicker.PickPhotoAsync();
				}

				if (photo != null)
				{
					PhotoImageSource = ImageSource.FromStream(() => photo.OpenReadAsync().Result); 
					photoStream = await photo.OpenReadAsync();
				}
			}
			catch (Exception ex)
			{ 
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
		}
		 
	}
}
