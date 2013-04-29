
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Pack957
{
	public partial class TigerNavigation : UINavigationController
	{
		public TigerNavigation () : base ("TigerNavigation", null)
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
			
			this.NavigationBar.TintColor = UIColor.FromRGB(247,150,62);
			this.NavigationItem.Title = "Tigers";
			//CubScoutTigers _vc = new CubScoutTigers();
			ViewScouts _vc = new ViewScouts();
			_vc.ScoutType = "Tiger";
			this.PushViewController(_vc, true);
		}
	}
}

