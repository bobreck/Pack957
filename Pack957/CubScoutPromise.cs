
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Pack957
{
	public partial class CubScoutPromise : UIViewController
	{
		public CubScoutPromise () : base ("CubScoutPromise", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public void FlyoutNavigationHandler(object sender, EventArgs e)
		{
			AppDelegate.Current.ViewController.ShowMenuView();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Title = "Cub Scout Promise";
			this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIImage.FromBundle("icons/399-list1"), UIBarButtonItemStyle.Plain, FlyoutNavigationHandler), true);

			this.View.BackgroundColor = UIColor.LightGray;
			UIScrollView myScroll = new UIScrollView(new RectangleF(0,0,this.View.Bounds.Width,this.View.Bounds.Height));
			myScroll.UserInteractionEnabled = true;
			myScroll.ScrollsToTop = true;
			View.AddSubview(myScroll);

			UIImageView myImage = new UIImageView(new RectangleF((this.View.Bounds.Width/2)-50,10,100,100));
			myImage.Image = UIImage.FromBundle("CubScouts100");
			myScroll.AddSubview(myImage);

			UILabel lblPromise = new UILabel();
			lblPromise.Frame = new RectangleF(10,120,this.View.Bounds.Width-20,20);
			lblPromise.Font = UIFont.BoldSystemFontOfSize(20);
			lblPromise.TextColor = UIColor.FromRGB (0,60,114);
			lblPromise.TextAlignment = UITextAlignment.Center;
			lblPromise.Text = "The Cub Scout Promise";
			lblPromise.BackgroundColor = UIColor.Clear;
			myScroll.AddSubview(lblPromise);

			UILabel lblText = new UILabel(new RectangleF(10,130,this.View.Bounds.Width-20,200));
			lblText.Font = UIFont.SystemFontOfSize(18);
			lblText.Lines = 6;
			lblText.TextColor = UIColor.FromRGB(0,60,114);
			lblText.BackgroundColor = UIColor.Clear;
			lblText.TextAlignment = UITextAlignment.Center;
			lblText.Text = "I, (Say your name), promise " + Environment.NewLine +  
				"to DO MY BEST" + Environment.NewLine + 
					"to do my DUTY to GOD" + Environment.NewLine + 
					"and my Country" + Environment.NewLine + 
					"to HELP other people, and" + Environment.NewLine + 
					"to OBEY the LAW of the Pack.";
			myScroll.AddSubview(lblText);
			                        

		}
	}
}

