using ExpenseTracker.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpenseTracker.ViewModel
{
	class DetailsPageVM : BaseViewModel
	{
		private ImageSource? photoImageSource;

		public HistoryModel HistoryModel { get; }
		public string Color { get; }
		public bool ShowExpense { get; }
		public ImageSource? PhotoImageSource { get => photoImageSource; set { SetProperty(ref photoImageSource, value); } }

		public ICommand OnButtonClicked { get; protected set; }

		public DetailsPageVM(HistoryModel historyModel)
		{
			HistoryModel = historyModel; RunCommands();
			Color = historyModel.isIncome ? "Green" : "Red";
			ShowExpense = historyModel.isIncome == false;
			PhotoImageSource = historyModel.PhotoImageSource;
		}

		private void RunCommands()
		{ 
			OnButtonClicked = new Command<string>(async (string par) =>
			{
				switch (par)
				{
					case "go_back":
						await Application.Current.MainPage.Navigation.PopAsync();
						break;
					case "picture":
						PickPictureCamera();
						break;
				}
			});
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
					var photoStream = await photo.OpenReadAsync();

					byte[] imageByte = ImageStorage.ConvertStreamToByteArray(photoStream);
					if (imageByte != null && imageByte.Length != 0)
						HistoryModel.imageUrl = await ImageStorage.SaveImageToLocalStorage(HistoryModel.id, imageByte);
					 
					var historyList = AllModels.GetHistoryList(); if (historyList != null)
					{
						var toDel = historyList.Where(p => p.id == HistoryModel.id).FirstOrDefault();
						if (toDel != null)
						{
							historyList.Remove(toDel);
							toDel.imageUrl = HistoryModel.imageUrl;
							historyList.Add(toDel);
						}
					}
					Preferences.Set(AllModels.HISTORY_DB_ID, JsonConvert.SerializeObject(historyList)); 
					await Application.Current.MainPage.DisplayAlert("Success", "Image Saved Successfully", "OK");
					//Application.Current.MainPage = new NavigationPage(new Views.HistoryPage());
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
		}

	}
}
