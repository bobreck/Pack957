
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.AudioToolbox;
using MonoTouch.AVFoundation;

namespace Pack957
{
	public partial class MoveLogos : UIViewController
	{
		UIImageView imgTiger;
		UIImageView imgWolf;
		UIImageView imgBear;
		UIImageView imgWeblo;
		UIImageView imgCub;
		protected bool touchStartedInside;

		public MoveLogos () : base ("MoveLogos", null)
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

			this.Title = "Cub Scout Logos";
			this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIImage.FromBundle("icons/399-list1"), UIBarButtonItemStyle.Plain, FlyoutNavigationHandler), true);

			imgCub = new UIImageView(new RectangleF(10,10,100,100));
			imgCub.Image = UIImage.FromBundle("CubScouts100");
			imgTiger = new UIImageView(new RectangleF(120,120,100,100));
			imgTiger.Image = UIImage.FromBundle("Tiger100");
			imgWolf = new UIImageView(new RectangleF(230,230,100,100));
			imgWolf.Image = UIImage.FromBundle("wolf100");
			imgBear = new UIImageView(new RectangleF(10,340,100,100));
			imgBear.Image = UIImage.FromBundle("bear100");
			imgWeblo = new UIImageView(new RectangleF(120,340,100,100));
			imgWeblo.Image = UIImage.FromBundle("weblos100");

			this.View.AddSubview(imgCub);
			this.View.AddSubview(imgTiger);
			this.View.AddSubview(imgWolf);
			this.View.AddSubview(imgBear);
			this.View.AddSubview(imgWeblo); 
		}

		public void playSound(string type)
		{
			var mediaFile = NSUrl.FromFilename("sounds/Squish.mp3");

			if (type == "Weblo") {
				mediaFile = NSUrl.FromFilename("sounds/Squish.mp3");	
			}
			else if (type == "Tiger") {
				mediaFile = NSUrl.FromFilename("sounds/tiger.mp3");
			} 
			else if (type == "Wolf") {
				mediaFile = NSUrl.FromFilename("sounds/wolf.mp3");
			} 
			else if (type == "Bear") {
				mediaFile = NSUrl.FromFilename("sounds/bear.mp3");
			}
			else { 
				mediaFile = NSUrl.FromFilename("sounds/Squish.mp3");
			}
			var audioPlayer = AVAudioPlayer.FromUrl(mediaFile);
			//audioPlayer.FinishedPlaying += delegate { audioPlayer.Dispose(); };
			audioPlayer.Play();
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			//get the touch event
			UITouch touch = touches.AnyObject as UITouch;
			if (touch != null) {
				if (imgCub.Frame.Contains (touch.LocationInView (View)))
				{
					touchStartedInside = true;
					playSound("CubScouts");
				}
				if (imgTiger.Frame.Contains (touch.LocationInView (View)))
				{
					touchStartedInside = true;
					playSound("Tiger");
				}
				if (imgWolf.Frame.Contains (touch.LocationInView (View)))
				{
					touchStartedInside = true;
					playSound("Wolf");
				}
				if (imgBear.Frame.Contains (touch.LocationInView (View)))
				{
					touchStartedInside = true;
					playSound("Bear");
				}
				if (imgWeblo.Frame.Contains (touch.LocationInView (View)))
				{
					touchStartedInside = true;
					playSound("Weblo");
				}
			}
		}
		
		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
			// get the touch
			UITouch touch = touches.AnyObject as UITouch;
			if (touch != null) {
				
				//==== IMAGE DRAG
				// check to see if the touch started in the dragme image
				if (touchStartedInside) {
					
					// move the shape
					float offsetX = touch.PreviousLocationInView (View).X - touch.LocationInView(View).X;
					float offsetY = touch.PreviousLocationInView (View).Y - touch.LocationInView(View).Y;
					if (imgCub.Frame.Contains (touch.LocationInView (View)))
					{
						imgCub.Frame = new System.Drawing.RectangleF (new PointF (imgCub.Frame.X - offsetX, imgCub.Frame.Y - offsetY), imgCub.Frame.Size);
						
					}		
					if (imgTiger.Frame.Contains (touch.LocationInView (View)))
					{
						imgTiger.Frame = new System.Drawing.RectangleF (new PointF (imgTiger.Frame.X - offsetX, imgTiger.Frame.Y - offsetY), imgTiger.Frame.Size);
						
					}		
					if (imgWolf.Frame.Contains (touch.LocationInView (View)))
					{
						imgWolf.Frame = new System.Drawing.RectangleF (new PointF (imgWolf.Frame.X - offsetX, imgWolf.Frame.Y - offsetY), imgWolf.Frame.Size);
						
					}		
					if (imgBear.Frame.Contains (touch.LocationInView (View)))
					{
						imgBear.Frame = new System.Drawing.RectangleF (new PointF (imgBear.Frame.X - offsetX, imgBear.Frame.Y - offsetY), imgBear.Frame.Size);
						
					}		
					if (imgWeblo.Frame.Contains (touch.LocationInView (View)))
					{
						imgWeblo.Frame = new System.Drawing.RectangleF (new PointF (imgWeblo.Frame.X - offsetX, imgWeblo.Frame.Y - offsetY), imgWeblo.Frame.Size);
						
					}		
				}
			}
		}
		
		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);
			// get the touch
			UITouch touch = touches.AnyObject as UITouch;
			if (touch != null) {
				
				//==== IMAGE TOUCH
//				if (imgCub.Frame.Contains (touch.LocationInView (View)))
//				{
//				}
				// reset our tracking flags
				touchStartedInside = false;
			}
		}
	}
}

