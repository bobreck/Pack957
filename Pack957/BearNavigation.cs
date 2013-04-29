
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Pack957
{
	public partial class BearNavigation : UINavigationController
	{
		public BearNavigation () : base ("BearNavigation", null)
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
			
			this.NavigationBar.TintColor = UIColor.FromRGB(151,213,200);
			this.NavigationItem.Title = "Bears";
			//CubScoutBears _vc = new CubScoutBears();
			ViewScouts _vc = new ViewScouts();
			_vc.ScoutType = "Bear";
			this.PushViewController(_vc, true);
		}
	}
}

