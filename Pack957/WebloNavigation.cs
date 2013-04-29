
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Pack957
{
	public partial class WebloNavigation : UINavigationController
	{
		public WebloNavigation () : base ("WebloNavigation", null)
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
			
			this.NavigationBar.TintColor = UIColor.FromRGB(141,215,250);
			this.NavigationItem.Title = "Weblos";
			//CubScoutWeblos _vc = new CubScoutWeblos();
			ViewScouts _vc = new ViewScouts();
			_vc.ScoutType = "Weblo";
			this.PushViewController(_vc, true);
		}
	}
}

