
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Pack957
{
	public partial class CubScoutLawOfThePackNavigation : UINavigationController
	{
		public CubScoutLawOfThePackNavigation () : base ("CubScoutLawOfThePackNavigation", null)
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
			this.NavigationItem.Title = "Law of the Pack";
			CubScoutLawOfThePack _vc = new CubScoutLawOfThePack();
			this.PushViewController(_vc, true);
		}
	}
}

