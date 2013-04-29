using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;
using SQLite;

namespace Pack957
{
	public partial class ViewScouts : UIViewController
	{
		public string ScoutType { get; set; }
		string scoutTitle;
		UITableView tvScouts;
		UIScrollView svScouts;
		SQLiteAsyncConnection conn;
		string folder;
		CubScoutsDB myDB;
		string tmpScout;
		UITableView tvAdd;

		public ViewScouts () : base ("ViewScouts", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			try
			{
//				List<ScoutsTableItemGroup> tableItems = new List<ScoutsTableItemGroup>();
//				ScoutsTableItemGroup tGroup;
//				
//				tGroup = new ScoutsTableItemGroup() {Name = scoutTitle, Footer = "" };
//				string strScoutTypeNumber = GetScoutTypeNumber(ScoutType);
//				if (strScoutTypeNumber != "")
//				{
//					var query = conn.Table<CubScout>().Where(v => v.ScoutType.StartsWith(strScoutTypeNumber)).OrderBy(x => x.LastName);
//					query.ToListAsync().ContinueWith (t => {
//						foreach (var scout in t.Result) {
//							CubScout s = scout;
//							if (s.Nickname.Trim() != "")
//							{
//								tmpScout = string.Format("{0} {1} ({2})", s.FirstName.Trim(), s.LastName.Trim(), s.Nickname.Trim());
//							} 
//							else
//							{
//								tmpScout = string.Format("{0} {1}", s.FirstName.Trim(), s.LastName.Trim());
//							}
//							tGroup.Items.Add(tmpScout);
//						}
//					});
//				}
//				tableItems.Add(tGroup);
//				
//				ScoutsTableSource ScoutData = new ScoutsTableSource(tableItems);
//				tvScouts.Source = ScoutData;
				//tvScouts.ReloadData();
			}
			catch (Exception ex)
			{
				using (UIAlertView myAlert = new UIAlertView())
				{
					myAlert.Message = ex.Message;
					myAlert.Title = "Error";
					myAlert.AddButton("OK");
					myAlert.Show();
				}
			}
			finally
			{
			}
		}

		private string GetScoutTypeNumber(string strScoutType)
		{
			switch (strScoutType)
			{
			case "Tiger":
				return "0";
			case "Wolf":
				return "1";
			case "Bear":
				return "2";
			case "Weblo":
				return "3";
			default:
				return "";
			}
		}

		public void FlyoutNavigationHandler(object sender, EventArgs e)
		{
			AppDelegate.Current.ViewController.ShowMenuView();
		}

		public void DoneButtonHandler(object sender, EventArgs e)
		{
			//myTable.SetEditing(!myTable.Editing, true);
			tvScouts.SetEditing(false,true);
			this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Edit, EditButtonHandler), true);
		}
		
		public void EditButtonHandler(object sender, EventArgs e)
		{
			//myTable.SetEditing(!myTable.Editing, true);
			tvScouts.SetEditing(true,true);
			this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Done, DoneButtonHandler), true);
		}

		void HandleAddDataAddScoutPage (object sender, EventArgs e)
		{
			this.NavigationController.PushViewController(new AddAScout(), true);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			try
			{
				this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIImage.FromBundle("icons/399-list1"), UIBarButtonItemStyle.Plain, FlyoutNavigationHandler), true);
				this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Edit, EditButtonHandler), true);

				myDB = new CubScoutsDB();
				folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				conn = new SQLiteAsyncConnection (System.IO.Path.Combine (folder, "CubScouts.db"));
				if (!myDB.TableExists("CubScout")) {
					conn.CreateTableAsync<CubScout>().ContinueWith (t => {
						Console.WriteLine ("Table created!");
					});
				}

				svScouts = new UIScrollView(new RectangleF(0,0,this.View.Bounds.Width,this.View.Bounds.Height));
				svScouts.UserInteractionEnabled = true;
				svScouts.AlwaysBounceVertical = true;
				svScouts.ScrollsToTop = true;
				this.View.AddSubview(svScouts);
				        
				tvAdd = new UITableView(new RectangleF(10,10,svScouts.Bounds.Width-20, 90), UITableViewStyle.Grouped);
				tvAdd.BackgroundView = null;

				List<AddTableItemGroup> addTableItems = new List<AddTableItemGroup>();
				AddTableItemGroup aGroup;
				aGroup = new AddTableItemGroup() {Name = "Add", Footer = "" };
				aGroup.Items.Add ("Add Scout");
				addTableItems.Add(aGroup);
				AddTableSource AddData = new AddTableSource(addTableItems);
				tvAdd.Source = AddData;
				svScouts.AddSubview(tvAdd);

				AddData.AddScoutPage += HandleAddDataAddScoutPage;

				tvScouts = new UITableView(new RectangleF(10,100,svScouts.Bounds.Width-20,svScouts.Bounds.Height-20), UITableViewStyle.Grouped);
				svScouts.AddSubview(tvScouts);

				if (ScoutType != null)
				{
					scoutTitle = string.Format("{0} Scouts", ScoutType.Trim());
				}
				else
				{
					scoutTitle = "Scouts";
				}
				this.Title = scoutTitle;

				List<ScoutsTableItemGroup> tableItems = new List<ScoutsTableItemGroup>();
				ScoutsTableItemGroup tGroup;

				tGroup = new ScoutsTableItemGroup() {Name = scoutTitle, Footer = "" };
				string strScoutTypeNumber = GetScoutTypeNumber(ScoutType);
				if (strScoutTypeNumber != "")
				{
					var query = conn.Table<CubScout>().Where(v => v.ScoutType.StartsWith(strScoutTypeNumber)).OrderBy(x => x.LastName);
					query.ToListAsync().ContinueWith (t => {
						foreach (var scout in t.Result) {
							CubScout s = scout;
							if (s.Nickname.Trim() != "")
							{
								tmpScout = string.Format("{0} {1} ({2})", s.FirstName.Trim(), s.LastName.Trim(), s.Nickname.Trim());
							} 
							else
							{
								tmpScout = string.Format("{0} {1}", s.FirstName.Trim(), s.LastName.Trim());
							}
							tGroup.Items.Add(tmpScout);
						}
					});
				}
				tableItems.Add(tGroup);

				ScoutsTableSource ScoutData = new ScoutsTableSource(tableItems);
				tvScouts.Source = ScoutData;
				tvScouts.BackgroundView = null;
				svScouts.BackgroundColor = UIColor.Clear;
				this.View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("backgrounds/Pattern_Cloth"));
				svScouts.ContentSize = new SizeF(this.View.Bounds.Width,tvScouts.Frame.Bottom + 25);
				svScouts.ContentInset = new UIEdgeInsets(0, 0, 25, 0);

			}
			catch (Exception ex)
			{
				using (UIAlertView myAlert = new UIAlertView())
				{
					myAlert.Message = ex.Message;
					myAlert.Title = "Error";
					myAlert.AddButton("OK");
					myAlert.Show();
				}
			}
			finally
			{

			}
		}
	}

	public class AddTableItemGroup
	{
		public string Name {get; set;}
		public string Footer {get; set;}
		
		public List<string> Items
		{
			get {return this._items; }
			set {this._items = value; }
		}				
		protected List<string> _items = new List<string>();
	}

	public class AddTableSource: UITableViewSource
	{
		public delegate void AddScoutHandler(object sender, EventArgs e);
		public event AddScoutHandler AddScoutPage;

		protected List<AddTableItemGroup> _tableItems;
		protected string _cellIdentifier = "AddCell";
		public AddTableSource (List<AddTableItemGroup> items) {this._tableItems = items; }
		public override int NumberOfSections (UITableView tableview) { return this._tableItems.Count; }
		public override int RowsInSection (UITableView tableview, int section)
		{
			return this._tableItems[section].Items.Count;
		}

		public override string TitleForHeader (UITableView tableview, int section)
		{
			return this._tableItems[section].Name;
		}
		
		public override string TitleForFooter (UITableView tableview, int section)
		{
			return this._tableItems[section].Footer;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{	
			UIAlertView myAlert = new UIAlertView();					
			var SelectedItemName = this._tableItems[indexPath.Section].Items[indexPath.Row];
			switch (SelectedItemName)
			{
			case "Add Scout":
				AddScoutPage(this, new EventArgs());
				break;  
			default:
				myAlert = new UIAlertView();
				myAlert.Message = "Error: No handler for this item.";
				myAlert.AddButton("OK");
				myAlert.Show();
				break;  
			}			
			tableView.DeselectRow(indexPath, true);
		}
		
		public override UITableViewCell GetCell (UITableView tableview, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableview.DequeueReusableCell(this._cellIdentifier);
			
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, this._cellIdentifier);
			}
			
			cell.TextLabel.Text = this._tableItems[indexPath.Section].Items[indexPath.Row];
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			return cell;
		}
	}

	public class ScoutsTableItemGroup
	{
		public string Name {get; set;}
		public string Footer {get; set;}
		
		public List<string> Items
		{
			get {return this._items; }
			set {this._items = value; }
		}				
		protected List<string> _items = new List<string>();
	}

	public class ScoutsTableSource : UITableViewSource
	{
		protected List<ScoutsTableItemGroup> _tableItems;
		protected string _cellIdentifier = "ScoutCell";
		public ScoutsTableSource (List<ScoutsTableItemGroup> items) {this._tableItems = items; }
		public override int NumberOfSections (UITableView tableview) { return this._tableItems.Count; }
		public override int RowsInSection (UITableView tableview, int section)
		{
			return this._tableItems[section].Items.Count;
		}

		public override string TitleForHeader (UITableView tableview, int section)
		{
			return this._tableItems[section].Name;
		}

		public override string TitleForFooter (UITableView tableview, int section)
		{
			return this._tableItems[section].Footer;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{	
			UIAlertView myAlert = new UIAlertView();					
			var SelectedItemName = this._tableItems[indexPath.Section].Items[indexPath.Row];
			switch (SelectedItemName)
			{
			case "Add Scout":
				break;  
			default:
				myAlert = new UIAlertView();
				myAlert.Message = "Error: No handler for this item.";
				myAlert.AddButton("OK");
				myAlert.Show();
				break;  
			}			
			tableView.DeselectRow(indexPath, true);
		}
		
		public override UITableViewCell GetCell (UITableView tableview, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableview.DequeueReusableCell(this._cellIdentifier);
			
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, this._cellIdentifier);
			}
			
			cell.TextLabel.Text = this._tableItems[indexPath.Section].Items[indexPath.Row];
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			return cell;
		}
	}

}

