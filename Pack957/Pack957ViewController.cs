using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FlyoutNavigation;
using MonoTouch.Dialog;
using System.Linq;

namespace Pack957
{
	public partial class Pack957ViewController : UIViewController
	{
		FlyoutNavigationController navigation;

		string[] MenuDens = {
			"Tigers",
			"Wolves",
			"Bears",
			"Weblos",
		};

		string [] MenuFun = {
			"Move the Logos",
			"Cub Scout Promise",
			"Law of the Pack",
			"Cub Scout Motto",
		};

		public Pack957ViewController () : base ("Pack957ViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public void buildMenu()
		{
			navigation = new FlyoutNavigationController();
			navigation.View.Frame = UIScreen.MainScreen.Bounds;
			View.AddSubview (navigation.View);
			
			// Create the menu:
			navigation.NavigationRoot = new RootElement ("Pack 957") {
				new Section ("Pack 957 Dens") {
					from page in MenuDens
						select new StringElement (page) as Element
				},
				new Section ("Fun stuff") {
					from page in MenuFun
						select new StringElement (page) as Element
				},
			};
			
			navigation.ViewControllers = new UIViewController [] {
				new TigerNavigation(),
				new WolfNavigation(),
				new BearNavigation(),
				new WebloNavigation(),
				new MoveLogos(),
				new CubScoutPromise(),
				new CubScoutLawOfThePack(),
				new CubScoutMotto(),
			};

			ToggleMenuView();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			buildMenu();
		}

		public void ToggleMenuView()
		{
			navigation.ToggleMenu();
		}

		class TaskPageController : DialogViewController
		{
			public TaskPageController (FlyoutNavigationController navigation, string title) : base (null)
			{
				Root = new RootElement (title) {
					new Section {
						new CheckboxElement (title)
					}
				};
				NavigationItem.LeftBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Action, delegate {
					navigation.ToggleMenu ();
				});
			}
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Portrait; // | UIInterfaceOrientationMask.LandscapeRight;
		}
	}
}

