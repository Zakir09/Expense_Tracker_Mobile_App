using ExpenseTracker.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace ExpenseTracker
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
			// Local Notification tap event listener
			LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationActionTapped;

			MainPage = new NavigationPage(new MainPage()); Application.Current.UserAppTheme = AppTheme.Light;
			this.RequestedThemeChanged += (s, e) => { Application.Current.UserAppTheme = AppTheme.Light; };
		}
		private void OnNotificationActionTapped(NotificationActionEventArgs e)
		{
			if (e.IsDismissed)
			{ 
				return;
			}
			if (e.IsTapped)
			{
				MainPage = new NavigationPage(new RemindersPage());
				return;
			}

		}
	}
}
