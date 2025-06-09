using Foundation;
using Microsoft.Maui.Controls.PlatformConfiguration;
using UIKit;

namespace ExpenseTracker
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp()
		{

#if IOS 
			Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CustomEntry", (hander, view) =>
			{
				hander.PlatformView.BorderStyle = UITextBorderStyle.None;
			});
			Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("CustomPicker", (hander, view) =>
			{
				hander.PlatformView.BorderStyle = UITextBorderStyle.None;
			});
			Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("CustomDatePicker", (hander, view) =>
			{
				hander.PlatformView.BorderStyle = UITextBorderStyle.None;
			});
			Microsoft.Maui.Handlers.TimePickerHandler.Mapper.AppendToMapping("CustomTimePicker", (hander, view) =>
			{
				hander.PlatformView.BorderStyle = UITextBorderStyle.None;
			});
#endif
			return MauiProgram.CreateMauiApp();
		}
		//protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	}
}
