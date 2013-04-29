using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Pack957
{
	public partial class CubScoutBears : DialogViewController
	{
		CubScoutsDB myDB = new CubScoutsDB();
		string folder;
		SQLiteAsyncConnection conn;
		Section secCurrentScouts;
		ViewScout _vsc;

		public CubScoutBears () : base (UITableViewStyle.Grouped, null)
		{
		}

		public override void LoadView ()
		{
			base.LoadView ();
			TableView.BackgroundView = null;
			TableView.BackgroundColor = UIColor.Clear;
			ParentViewController.View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("backgrounds/Pattern_Cloth"));
		}

		private void ReloadCubScouts(Section mySection)
		{
			conn = new SQLiteAsyncConnection (System.IO.Path.Combine (folder, "CubScouts.db"));
			var query = conn.Table<CubScout>().Where(v => v.ScoutType.StartsWith ("2")).OrderBy(x => x.LastName);
			query.ToListAsync().ContinueWith (t => {
				foreach (var scout in t.Result) {
					CubScout s = scout;
					BeginInvokeOnMainThread(() => {
						if (s.Nickname.Trim() != "")
						{
							mySection.Add(new StringElement(string.Format("{0} {1} ({2})", s.FirstName.Trim(), s.LastName.Trim(), s.Nickname.Trim()), delegate {
								_vsc = new ViewScout();
								_vsc.ScoutName = string.Format("{0} {1}", s.FirstName.Trim(), s.LastName.Trim());
								_vsc.NickName = s.Nickname.Trim();
								this.NavigationController.PushViewController(_vsc, true);
							}));
						} 
						else
						{
							mySection.Add(new StringElement(string.Format("{0} {1}", s.FirstName.Trim(), s.LastName.Trim()), delegate {
								_vsc = new ViewScout();
								_vsc.ScoutName = string.Format("{0} {1}", s.FirstName.Trim(), s.LastName.Trim());
								this.NavigationController.PushViewController(_vsc, true);
							}));
						}
					});
				}
			});
		}

		public void FlyoutNavigationHandler(object sender, EventArgs e)
		{
			AppDelegate.Current.ViewController.ShowMenuView();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIImage.FromBundle("icons/399-list1"), UIBarButtonItemStyle.Plain, FlyoutNavigationHandler), true);
			this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Edit, delegate {
				TableView.SetEditing(true,true);
			}), true);

			try 
			{
				folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				conn = new SQLiteAsyncConnection (System.IO.Path.Combine (folder, "CubScouts.db"));
				if (!myDB.TableExists("CubScout")) {
					conn.CreateTableAsync<CubScout>().ContinueWith (t => {
						Console.WriteLine ("Table created!");
					});
				}

				//Setup Map Types Radio Group
				RadioGroup myScoutTypes = new RadioGroup("scoutTypes",2);
				RadioElement rTiger = new RadioElement("Tiger","scoutTypes");
				RadioElement rWolf = new RadioElement("Wolf","scoutTypes");
				RadioElement rBear = new RadioElement("Bear","scoutTypes");
				RadioElement rWeblo = new RadioElement("Weblo","scoutTypes");
				
				Section myScoutTypesSection = new Section();
				myScoutTypesSection.Add(rTiger);
				myScoutTypesSection.Add(rWolf);
				myScoutTypesSection.Add(rBear);
				myScoutTypesSection.Add(rWeblo);
				
				secCurrentScouts = new Section ("Pack 957 Bears");
				//MultilineElement scoutElement;

				//Call the reload method to re-populate the list.
				ReloadCubScouts(secCurrentScouts);

				EntryElement fn;
				EntryElement ln; 
				EntryElement nn;
				RootElement st;
				EntryElement mn;
				EntryElement dn;
				EntryElement ea;
				EntryElement hp;
				EntryElement cp;
				Root = new RootElement ("Bears") {
					new Section ("Add") {
						new RootElement ("Add Scout") {
							new Section ("Your info") {
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
									st.RadioSelected = 2;
									mn.Value = "";
									dn.Value = "";
									ea.Value = "";
									hp.Value = "";
									cp.Value = "";
									this.NavigationController.PopViewControllerAnimated(true);
								}),
							},
						}
					},
					secCurrentScouts,
				};


			}
			catch (Exception ex)
			{
				using (UIAlertView myAlert = new UIAlertView())
				{
					myAlert.Message = ex.Message;
					myAlert.Title = "Error!";
					myAlert.AddButton("OK");
					myAlert.Show();
				}
			}
			finally
			{

			}
		}
	}
}
