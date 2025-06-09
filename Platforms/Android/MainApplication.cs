using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

[assembly: UsesPermission(Manifest.Permission.WakeLock)]

//Required so that the plugin can reschedule notifications upon a reboot
[assembly: UsesPermission(Manifest.Permission.ReceiveBootCompleted)]
[assembly: UsesPermission(Manifest.Permission.Vibrate)]
[assembly: UsesPermission("android.permission.POST_NOTIFICATIONS")]
 
[assembly: UsesPermission("android.permission.USE_EXACT_ALARM")]
[assembly: UsesPermission("android.permission.SCHEDULE_EXACT_ALARM")]

namespace ExpenseTracker
{
	[Application]
	public class MainApplication : MauiApplication
	{
		public MainApplication(IntPtr handle, JniHandleOwnership ownership)
			: base(handle, ownership)
		{
			AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

		}

		protected override MauiApp CreateMauiApp()
		{

#if ANDROID
			Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CustomEntry", (hander, view) =>
			{ 
				hander.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid()); 
			});
			Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("CustomPicker", (hander, view) =>
			{
				hander.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
			});
			Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("CustomDatePicker", (hander, view) =>
			{
				hander.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
			});
			Microsoft.Maui.Handlers.TimePickerHandler.Mapper.AppendToMapping("CustomTimePicker", (hander, view) =>
			{
				hander.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
			});
#endif

			EnableUriPermissionForSdk25UP();
			return MauiProgram.CreateMauiApp();
		}
		//protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
		private void EnableUriPermissionForSdk25UP()
		{
			StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
			StrictMode.SetVmPolicy(builder.Build());
		}
	}
}
