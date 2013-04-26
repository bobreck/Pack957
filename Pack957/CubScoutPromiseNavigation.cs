
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Pack957
{
	public partial class CubScoutPromiseNavigation : UINavigationController
	{
		public CubScoutPromiseNavigation () : base ("CubScoutPromiseNavigation", null)
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
			
			this.NavigationBar.TintColor = UIColor.FromRGB(255,212,0);
			this.NavigationItem.Title = "Cub Scout Promise";
			CubScoutPromise _vc = new CubScoutPromise();
			this.PushViewController(_vc, true);
		}
	}
}

