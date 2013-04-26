
using System;
using System.IO;
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

		public CubScoutBears () : base (UITableViewStyle.Grouped, null)
		{
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
							mySection.Add(new StringElement(string.Format("{0} {1} ({2})", s.FirstName.Trim(), s.LastName.Trim(), s.Nickname.Trim())));
						} 
						else
						{
							mySection.Add(new StringElement(string.Format("{0} {1}", s.FirstName.Trim(), s.LastName.Trim())));
						}
					});
				}
			});
		}

		public void FlyoutNavigationHandler(object sender, EventArgs e)
		{
			Pack957ViewController myFlyOutController = new Pack957ViewController();
			myFlyOutController.ToggleMenuView();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIImage.FromBundle("icons/399-list1"), UIBarButtonItemStyle.Plain, FlyoutNavigationHandler), true);

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
				Root = new RootElement ("Bears") {
					new Section ("Add") {
						new RootElement ("Add Scout") {
							new Section ("Your info") {
								(fn = new EntryElement("First Name","First Name","")),
								(ln = new EntryElement("Last Name","Last Name","")),
								(nn = new EntryElement("Nickname","Nickname","")),
								(st = new RootElement("Scout Types", myScoutTypes) {
									myScoutTypesSection
								}),
								new StringElement("Save",delegate {
									CubScout newScout = new CubScout {FirstName = fn.Value.Trim(), LastName = ln.Value.Trim(), Nickname = nn.Value.Trim (), ScoutType = st.RadioSelected.ToString()};
									conn.InsertAsync (newScout).ContinueWith (t => {
										Console.WriteLine ("New scout ID: {0}", newScout.Id);
									});
									fn.Value = "";
									ln.Value = "";
									nn.Value = "";
									st.RadioSelected = 2;
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
