
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;
using System.Collections.Generic;
using System.Timers;

namespace Pack957
{
	public partial class CubScoutMotto : UIViewController
	{

		int currentImage = 0;
		bool playingAnimation = true;
		static int TOTAL_SLIDES = 0;
		
		UIView imagesView;
		
		CALayer titleImage;
		CALayer featureImage;
		CALayer textImage;
		
		Timer timer;
		
		//bool transitioningOut;
		
		List<string> SlideshowImages = new List<string>(){
			"DoYourBest"
		};


		public CubScoutMotto () : base ("CubScoutMotto", null)
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

			this.Title = "Cub Scout Motto";
			this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIImage.FromBundle("icons/399-list1"), UIBarButtonItemStyle.Plain, FlyoutNavigationHandler), true);

			buildUI();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.View.ClipsToBounds = true;
			playingAnimation = true;
			transitionIn();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			
			playingAnimation = false;
			timer.Stop ();
			titleImage.RemoveAllAnimations();
			featureImage.RemoveAllAnimations();
			textImage.RemoveAllAnimations();
		}
		
		void buildUI ()
		{
			createBackground();
			createImagesView();
			
			
			//createBasicSlideshow();
			createSlide ();
		}

		void createBackground ()
		{
			this.View.BackgroundColor = UIColor.FromRGB(247,150,62);
		}
		
		void createImagesView ()
		{
			imagesView = new UIView();
			imagesView.Frame = new RectangleF(0, 20, View.Bounds.Width, 400);
			View.AddSubview( imagesView );
		}
		
		public void createSlide ()
		{
			//Console.WriteLine("createSlide");
			
			if(currentImage == TOTAL_SLIDES){
				currentImage = 0;
			}
			
			UIView.AnimationsEnabled = false;
			
			// TITLE Image
			if(titleImage == null){
				titleImage = new CALayer();
				imagesView.Layer.AddSublayer( titleImage );
			}
			titleImage.Contents = UIImage.FromBundle( SlideshowImages[currentImage] + "Title.png" ).CGImage;
			titleImage.Frame = new RectangleF( 320, 0, titleImage.Contents.Width, titleImage.Contents.Height ); 
			
			// FEATURE IMAGE
			if(featureImage == null){
				featureImage = new CALayer();
				imagesView.Layer.AddSublayer( featureImage );
			}
			featureImage.Contents = UIImage.FromBundle( SlideshowImages[currentImage] + "Logo.png" ).CGImage;
			featureImage.Frame = new RectangleF( 320, titleImage.Bounds.Bottom + 20, featureImage.Contents.Width, featureImage.Contents.Height ); 
			
			// FEATURE IMAGE
			if(textImage == null){
				textImage = new CALayer();
				imagesView.Layer.AddSublayer( textImage );
			}
			textImage.Contents = UIImage.FromBundle( SlideshowImages[currentImage] + "Text.png" ).CGImage;
			textImage.Frame = new RectangleF(320, featureImage.Bounds.Bottom + 100, textImage.Contents.Width, textImage.Contents.Height);
			
			//Console.WriteLine("Y " + textImage.Frame.Y);
			
			//if(textImage.AnimationKeys != null)
			//	Console.WriteLine("Start Length " + textImage.AnimationKeys.Length);	
		}
		
		public void transitionIn ()
		{	
			//Console.WriteLine("transitionIn");
			
			titleImage.RemoveAllAnimations();
			featureImage.RemoveAllAnimations();
			textImage.RemoveAllAnimations();
			
			var localMediaTime = CAAnimation.CurrentMediaTime();
			
			var titleAnim = CABasicAnimation.FromKeyPath("position.x");
			titleAnim.Duration = 1;
			titleAnim.BeginTime = localMediaTime;
			titleAnim.From = NSNumber.FromFloat ( 768f );
			titleAnim.To = NSNumber.FromFloat ( View.Frame.Width * 0.5f );
			titleAnim.RemovedOnCompletion = false;
			titleAnim.FillMode = CAFillMode.Forwards;
			titleAnim.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseOut);
			
			var featureAnim = CABasicAnimation.FromKeyPath("position.x");
			featureAnim.Duration = 1;
			featureAnim.BeginTime = localMediaTime+0.4;
			featureAnim.From = NSNumber.FromFloat ( View.Frame.Width + featureImage.Frame.Width );
			featureAnim.To = NSNumber.FromFloat ( View.Frame.Width * 0.5f );
			featureAnim.RemovedOnCompletion = false;
			featureAnim.FillMode = CAFillMode.Forwards;
			featureAnim.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseOut);
			
			var textAnim = CABasicAnimation.FromKeyPath("position.x");
			textAnim.Duration = 1;
			textAnim.BeginTime = localMediaTime + 0.8;
			textAnim.From = NSNumber.FromFloat ( 768f );
			textAnim.To = NSNumber.FromFloat ( View.Frame.Width * 0.5f );
			textAnim.RemovedOnCompletion = false;
			textAnim.FillMode = CAFillMode.Forwards;
			textAnim.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseOut);
			
			titleImage.AddAnimation ( titleAnim, "position.x" );
			featureImage.AddAnimation ( featureAnim, "position.x" );
			textImage.AddAnimation ( textAnim, "position.x" );
			
			timer = new System.Timers.Timer ();
			timer.Interval = 5000;
			timer.Elapsed += (object sender, ElapsedEventArgs e) => {
				//Console.WriteLine("timer.Elapsed");
				timer.Stop();
				InvokeOnMainThread(()=>{
					transitionOut();
				});
				
			};
			timer.Start();
			
			//Console.WriteLine("In Length " + textImage.AnimationKeys.Length);
		}
		
		void transitionOut ()
		{
			var localMediaTime = CAAnimation.CurrentMediaTime();
			
			var titleAnim = CABasicAnimation.FromKeyPath("position.x");
			titleAnim.Duration = 1;
			titleAnim.BeginTime = localMediaTime;
			titleAnim.From = NSNumber.FromFloat( titleImage.PresentationLayer.Position.X );
			titleAnim.To = NSNumber.FromFloat ( titleImage.Frame.Width * -1f );
			titleAnim.RemovedOnCompletion = false;
			titleAnim.FillMode = CAFillMode.Forwards;
			titleAnim.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseIn);
			
			var featureAnim = CABasicAnimation.FromKeyPath("position.x");
			featureAnim.Duration = 1;
			featureAnim.BeginTime = localMediaTime + 0.4;
			featureAnim.From = NSNumber.FromFloat( featureImage.PresentationLayer.Position.X );
			featureAnim.To = NSNumber.FromFloat ( featureImage.Frame.Width * -1f );
			featureAnim.RemovedOnCompletion = false;
			featureAnim.FillMode = CAFillMode.Both; // b/c of delay, if it was Forwards only, it would disappear and reappear
			featureAnim.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseIn);
			
			var textAnim = CABasicAnimation.FromKeyPath("position.x");
			textAnim.Duration = 1;
			textAnim.BeginTime = localMediaTime + 0.8;
			textAnim.From = NSNumber.FromFloat( textImage.PresentationLayer.Position.X );
			textAnim.To = NSNumber.FromFloat ( textImage.Frame.Width * -1f );
			textAnim.RemovedOnCompletion = false;
			textAnim.FillMode = CAFillMode.Both;
			textAnim.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseIn);
			textAnim.AnimationStopped += (sender, e) => {
				titleImage.RemoveAllAnimations();
				featureImage.RemoveAllAnimations();
				textImage.RemoveAllAnimations();
				
				cycleSlides();
			};
			
			titleImage.AddAnimation ( titleAnim, "position.x" );
			featureImage.AddAnimation ( featureAnim, "position.x" );
			textImage.AddAnimation ( textAnim, "position.x" );
		}
		
		void cycleSlides ()
		{
			if(playingAnimation){
				//currentImage++;
				createSlide();
				transitionIn();
			}
		}

	}
}

