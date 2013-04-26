
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Pack957
{
	public partial class CubScoutLawOfThePack : UIViewController
	{
		public CubScoutLawOfThePack () : base ("CubScoutLawOfThePack", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			base.ViewDidLoad ();
			this.View.BackgroundColor = UIColor.FromRGB(0,60,114);
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
			lblPromise.TextColor = UIColor.FromRGB (253,220,8);
			lblPromise.TextAlignment = UITextAlignment.Center;
			lblPromise.Text = "The Law of the Pack";
			lblPromise.BackgroundColor = UIColor.Clear;
			myScroll.AddSubview(lblPromise);
			
			UILabel lblText = new UILabel(new RectangleF(10,130,this.View.Bounds.Width-20,200));
			lblText.Font = UIFont.SystemFontOfSize(18);
			lblText.Lines = 6;
			lblText.TextColor = UIColor.FromRGB(253,220,8);
			lblText.BackgroundColor = UIColor.Clear;
			lblText.TextAlignment = UITextAlignment.Center;
			lblText.Text = "The Cub Scout follows Akela." + Environment.NewLine + 
				"The Cub Scout helps the pack go." + Environment.NewLine + 
				"The pack helps the Cub Scout grow." + Environment.NewLine + 
					"The Cub Scout gives goodwill.";
			myScroll.AddSubview(lblText);
		}
	}
}

