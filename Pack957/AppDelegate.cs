using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SQLite;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Pack957
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		private readonly SQLiteAsyncConnection db = new SQLiteAsyncConnection (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "CubScouts.db"));

		UIWindow window;
		public Pack957ViewController ViewController;
		public static AppDelegate Current;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Current = this;
			db.CreateTableAsync<CubScout>().Wait();

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			ViewController = new Pack957ViewController ();
			window.RootViewController = ViewController;
			window.MakeKeyAndVisible ();
			
			return true;
		}

		private static int busy;
		public static void AddActivity()
		{
			UIApplication.SharedApplication.InvokeOnMainThread (() => {
				if (busy++ < 1)
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			});
		}
		
		public static void FinishActivity()
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() => {
				if (--busy < 1)
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			});
		}
	}
}

