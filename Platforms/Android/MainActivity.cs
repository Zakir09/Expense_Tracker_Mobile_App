﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ExpenseTracker
{
	[Activity(Theme = "@style/Maui.SplashTheme", Label = "Expense Tracker",  MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)] 

	public class MainActivity : MauiAppCompatActivity
	{
	}
}
