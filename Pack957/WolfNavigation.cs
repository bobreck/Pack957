
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Pack957
{
	public partial class WolfNavigation : UINavigationController
	{
		public WolfNavigation () : base ("WolfNavigation", null)
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
			
			this.NavigationBar.TintColor = UIColor.FromRGB(236,25,68);
			this.NavigationItem.Title = "Wolves";
			CubScoutWolves _vc = new CubScoutWolves();
			this.PushViewController(_vc, true);
		}
	}
}

