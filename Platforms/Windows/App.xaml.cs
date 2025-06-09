using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExpenseTracker.WinUI
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public partial class App : MauiWinUIApplication
	{
		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeComponent();
		}

		//protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

		protected override MauiApp CreateMauiApp()
		{

#if WINDOWS
			Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CustomEntry", (hander, view) =>
			{
				hander.PlatformView.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
			});
			Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("CustomPicker", (hander, view) =>
			{
				hander.PlatformView.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
			});
			Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("CustomDatePicker", (hander, view) =>
			{
				hander.PlatformView.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
			});
			Microsoft.Maui.Handlers.TimePickerHandler.Mapper.AppendToMapping("CustomTimePicker", (hander, view) =>
			{
				hander.PlatformView.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
			});
#endif
			 
			return MauiProgram.CreateMauiApp();
		}

	}

}
