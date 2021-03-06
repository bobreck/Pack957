
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Pack957
{
	public partial class CubScoutMottoNavigation : UINavigationController
	{
		public CubScoutMottoNavigation () : base ("CubScoutMottoNavigation", null)
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
			UITextAttributes textAttr = new UITextAttributes();
			textAttr.TextColor = UIColor.FromRGB(0,84,150);
			//textAttr.TextShadowColor = UIColor.FromRGB(0,84,150);
			this.NavigationBar.SetTitleTextAttributes(textAttr);
			this.NavigationItem.Title = "Cub Scout Motto";
			CubScoutMotto _vc = new CubScoutMotto();
			this.PushViewController(_vc, true);
		}
	}
}

