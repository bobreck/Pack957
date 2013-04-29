using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Pack957
{
	public partial class AddScout : DialogViewController
	{
		SQLiteAsyncConnection conn;
		string folder;

		public override void LoadView ()
		{
			base.LoadView ();
			TableView.BackgroundView = null;
			TableView.BackgroundColor = UIColor.Clear;
			ParentViewController.View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("backgrounds/Pattern_Cloth"));
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			conn = new SQLiteAsyncConnection (System.IO.Path.Combine (folder, "CubScouts.db"));
			
			//Setup Map Types Radio Group
			RadioGroup myScoutTypes = new RadioGroup("scoutTypes",0);
			RadioElement rTiger = new RadioElement("Tiger","scoutTypes");
			RadioElement rWolf = new RadioElement("Wolf","scoutTypes");
			RadioElement rBear = new RadioElement("Bear","scoutTypes");
			RadioElement rWeblo = new RadioElement("Weblo","scoutTypes");
			
			Section myScoutTypesSection = new Section();
			myScoutTypesSection.Add(rTiger);
			myScoutTypesSection.Add(rWolf);
			myScoutTypesSection.Add(rBear);
			myScoutTypesSection.Add(rWeblo);
			
			EntryElement fn;
			EntryElement ln; 
			EntryElement nn;
			RootElement st;
			EntryElement mn;
			EntryElement dn;
			EntryElement ea;
			EntryElement hp;
			EntryElement cp;
			Root = new RootElement ("Add Scout") {
				new Section ("Scout Info") {
					(fn = new EntryElement("First Name","First Name","")),
					(ln = new EntryElement("Last Name","Last Name","")),
					(nn = new EntryElement("Nickname","Nickname","")),
					(st = new RootElement("Den", myScoutTypes) {
						myScoutTypesSection
					}),
					(mn = new EntryElement("Mom's Name","Mom's Name", "")),
					(dn = new EntryElement("Dad's Name","Dad's Name", "")),
					(ea = new EntryElement("Email Address","Email Address", "")),
					(hp = new EntryElement("Home Phone", "(999) 999-9999", "")),
					(cp = new EntryElement("Mobile Phone", "(999) 999-9999", "")),
					new StringElement("Save",delegate {
						CubScout newScout = new CubScout {FirstName = fn.Value.Trim(), LastName = ln.Value.Trim(), 
							Nickname = nn.Value.Trim (), ScoutType = st.RadioSelected.ToString(), 
							MomsName = mn.Value.Trim(), DadsName = dn.Value.Trim(), EmailAddress = ea.Value.Trim(),
							HomePhone = hp.Value.Trim(), CellPhone = cp.Value.Trim()};
						conn.InsertAsync (newScout).ContinueWith (t => {
							Console.WriteLine ("New scout ID: {0}", newScout.Id);
						});
						fn.Value = "";
						ln.Value = "";
						nn.Value = "";
						st.RadioSelected = 0;
						mn.Value = "";
						dn.Value = "";
						ea.Value = "";
						hp.Value = "";
						cp.Value = "";
						this.NavigationController.PopViewControllerAnimated(true);
					}),
				},
			};
		}

		public AddScout () : base (UITableViewStyle.Grouped, null)
		{
		}
	}
}
