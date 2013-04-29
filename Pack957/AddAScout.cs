using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SQLite;
using System.IO;

namespace Pack957
{
	public partial class AddAScout : UIViewController
	{
		UIScrollView myScrollView;
		UILabel myLabel;
		UITextField txtFirstName;
		UITextField txtLastName;
		UITextField txtNickName;
		UIPickerView pickerDen;
		UITextField txtDen;
		DenPicker myDenPicker;
		UIToolbar toolbarDen;
		UIActionSheet myActionSheet;
		SizeF actionSheetSize;
		UITextField txtMomName;
		UITextField txtDadName;
		UITextField txtEmail;
		UITextField txtHomePhone;
		UITextField txtMobilePhone;
		NSObject _keyboardObserverWillShow;
		NSObject _keyboardObserverWillHide;
		SQLiteAsyncConnection conn;
		string folder;
		string strDenNumber;

		public AddAScout () : base ("AddAScout", null)
		{
		}

		protected virtual void RegisterForKeyboardNotifications ()
		{
			_keyboardObserverWillShow = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillShowNotification, KeyboardWillShowNotification);
			_keyboardObserverWillHide = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillHideNotification, KeyboardWillHideNotification);
		}
		
		protected virtual void UnregisterKeyboardNotifications()
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillShow);
			NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillHide);
		}

		protected virtual UIView KeyboardGetActiveView()
		{
			return this.View.FindFirstResponder();
		}

		protected virtual void KeyboardWillShowNotification (NSNotification notification)
		{
			UIView activeView = KeyboardGetActiveView();
			if (activeView == null)
				return;
			
			UIScrollView scrollView = activeView.FindSuperviewOfType(this.View, typeof(UIScrollView)) as UIScrollView;
			if (scrollView == null)
				return;
			
			RectangleF keyboardBounds = UIKeyboard.FrameBeginFromNotification(notification);
			
			UIEdgeInsets contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardBounds.Size.Height, 0.0f);
			scrollView.ContentInset = contentInsets;
			scrollView.ScrollIndicatorInsets = contentInsets;
			
			// If activeField is hidden by keyboard, scroll it so it's visible
			RectangleF viewRectAboveKeyboard = new RectangleF(this.View.Frame.Location, new SizeF(this.View.Frame.Width, this.View.Frame.Size.Height - keyboardBounds.Size.Height));
			
			RectangleF activeFieldAbsoluteFrame = activeView.Superview.ConvertRectToView(activeView.Frame, this.View);
			// activeFieldAbsoluteFrame is relative to this.View so does not include any scrollView.ContentOffset
			
			// Check if the activeField will be partially or entirely covered by the keyboard
			PointF scrollPoint = new PointF (0.0f, activeFieldAbsoluteFrame.Location.Y + activeFieldAbsoluteFrame.Height + scrollView.ContentOffset.Y - viewRectAboveKeyboard.Height);
			// Check if the activeField will be partially or entirely covered by the keyboard
			if (viewRectAboveKeyboard.IntersectsWith (activeFieldAbsoluteFrame) &&
			    ! viewRectAboveKeyboard.Contains (activeFieldAbsoluteFrame)) {
				scrollView.SetContentOffset (scrollPoint, true);
			} else if (!viewRectAboveKeyboard.Contains (activeFieldAbsoluteFrame)) {
				scrollView.SetContentOffset (scrollPoint, true);
			}
		}
		
		protected virtual void KeyboardWillHideNotification (NSNotification notification)
		{
			UIView activeView = KeyboardGetActiveView();
			if (activeView == null)
				return;
			
			UIScrollView scrollView = activeView.FindSuperviewOfType (this.View, typeof(UIScrollView)) as UIScrollView;
			if (scrollView == null)
				return;
			
			// Reset the content inset of the scrollView and animate using the current keyboard animation duration
			double animationDuration = UIKeyboard.AnimationDurationFromNotification(notification);
			UIEdgeInsets contentInsets = new UIEdgeInsets(0.0f, 0.0f, 0.0f, 0.0f);
			UIView.Animate(animationDuration, delegate{
				scrollView.ContentInset = contentInsets;
				scrollView.ScrollIndicatorInsets = contentInsets;
			});
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		protected void SaveButtonHandler(object sender, EventArgs e)
		{
			switch (txtDen.Text)
			{
			case "Tiger":
				strDenNumber = "0";
				break;
			case "Wolf":
				strDenNumber = "1";
				break;
			case "Bear":
				strDenNumber = "2";
				break;
			case "Weblo":
				strDenNumber = "3";
				break;
			default:
				strDenNumber = "";
				break;
			}
			folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			conn = new SQLiteAsyncConnection (System.IO.Path.Combine (folder, "CubScouts.db"));
			CubScout newScout = new CubScout {FirstName = txtFirstName.Text.Trim(), LastName = txtLastName.Text.Trim(), 
				Nickname = txtNickName.Text.Trim (), ScoutType = strDenNumber, 
				MomsName = txtMomName.Text.Trim(), DadsName = txtDadName.Text.Trim(), EmailAddress = txtEmail.Text.Trim(),
				HomePhone = txtHomePhone.Text.Trim(), CellPhone = txtMobilePhone.Text.Trim()};
			conn.InsertAsync (newScout).ContinueWith (t => {
				Console.WriteLine ("New scout ID: {0}", newScout.Id);
			});
			this.NavigationController.PopViewControllerAnimated(true);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			RegisterForKeyboardNotifications();
			this.View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("backgrounds/Pattern_Cloth"));
			this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Save, SaveButtonHandler), true);

			myScrollView = new UIScrollView(new RectangleF(0,0,this.View.Bounds.Width,this.View.Bounds.Height));
			myScrollView.ContentSize = new SizeF(this.View.Bounds.Width,this.View.Bounds.Height+25);
			myScrollView.ContentInset = new UIEdgeInsets(0,0,25,0);
			myScrollView.UserInteractionEnabled = true;
			myScrollView.ScrollsToTop = true;
			myScrollView.AlwaysBounceVertical = true;
			myScrollView.Bounces = true;
			this.View.AddSubview(myScrollView);

			myLabel = new UILabel(new RectangleF(10,10,myScrollView.Bounds.Width-20,31));
			myLabel.Text = "Scout Info";
			myLabel.BackgroundColor = UIColor.Clear;
			myLabel.Font = UIFont.BoldSystemFontOfSize(18);
			myScrollView.AddSubview(myLabel);

			myLabel = new UILabel(new RectangleF(10,51,100,31));
			myLabel.Text = "First Name";
			myLabel.BackgroundColor = UIColor.Clear;
			myLabel.Font = UIFont.SystemFontOfSize(18);
			myScrollView.AddSubview(myLabel);

			txtFirstName = new UITextField(new RectangleF(112,51,190,31));
			txtFirstName.Font = UIFont.SystemFontOfSize(18);
			txtFirstName.BackgroundColor = UIColor.White;
			txtFirstName.BorderStyle = UITextBorderStyle.RoundedRect;
			txtFirstName.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			txtFirstName.ClearButtonMode = UITextFieldViewMode.WhileEditing;
			myScrollView.AddSubview(txtFirstName);

			myLabel = new UILabel(new RectangleF(10,87,100,31));
			myLabel.Text = "Last Name";
			myLabel.BackgroundColor = UIColor.Clear;
			myLabel.Font = UIFont.SystemFontOfSize(18);
			myScrollView.AddSubview(myLabel);
			
			txtLastName = new UITextField(new RectangleF(112,87,190,31));
			txtLastName.Font = UIFont.SystemFontOfSize(18);
			txtLastName.BackgroundColor = UIColor.White;
			txtLastName.BorderStyle = UITextBorderStyle.RoundedRect;
			txtLastName.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			txtLastName.ClearButtonMode = UITextFieldViewMode.WhileEditing;
			myScrollView.AddSubview(txtLastName);

			myLabel = new UILabel(new RectangleF(10,123,100,31));
			myLabel.Text = "Nickname";
			myLabel.BackgroundColor = UIColor.Clear;
			myLabel.Font = UIFont.SystemFontOfSize(18);
			myScrollView.AddSubview(myLabel);
			
			txtNickName = new UITextField(new RectangleF(112,123,190,31));
			txtNickName.Font = UIFont.SystemFontOfSize(18);
			txtNickName.BackgroundColor = UIColor.White;
			txtNickName.BorderStyle = UITextBorderStyle.RoundedRect;
			txtNickName.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			txtNickName.ClearButtonMode = UITextFieldViewMode.WhileEditing;
			myScrollView.AddSubview(txtNickName);

			myLabel = new UILabel(new RectangleF(10,159,100,31));
			myLabel.Text = "Den";
			myLabel.BackgroundColor = UIColor.Clear;
			myLabel.Font = UIFont.SystemFontOfSize(18);
			myScrollView.AddSubview(myLabel);

			myActionSheet = new UIActionSheet();
			myActionSheet.Style = UIActionSheetStyle.BlackTranslucent;
			
			toolbarDen = new UIToolbar(RectangleF.Empty);
			toolbarDen.BarStyle = UIBarStyle.Black;
			toolbarDen.Translucent = true;
			toolbarDen.UserInteractionEnabled = true;
			toolbarDen.SizeToFit();
			UIBarButtonItem btnCancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, PickerButtonCancelHandler);
			UIBarButtonItem btnFlexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null);
			UIBarButtonItem btnDone = new UIBarButtonItem(UIBarButtonSystemItem.Done, PickerButtonDoneHandler);
			UIBarButtonItem[] items = new UIBarButtonItem[] { btnCancel, btnFlexibleSpace, btnDone }; 
			toolbarDen.SetItems(items, true);
			myActionSheet.AddSubview(toolbarDen);
			
			pickerDen = new UIPickerView(RectangleF.Empty);
			myDenPicker = new DenPicker();
			myDenPicker.Items.Add("");
			myDenPicker.Items.Add("Tiger");
			myDenPicker.Items.Add("Wolf");
			myDenPicker.Items.Add("Bear");
			myDenPicker.Items.Add("Weblo");
			pickerDen.Source = myDenPicker;
			pickerDen.UserInteractionEnabled = true;
			pickerDen.ShowSelectionIndicator = true;
			myActionSheet.AddSubview(pickerDen);

			txtDen = new UITextField(new RectangleF(112,159,190,31));
			txtDen.Font = UIFont.SystemFontOfSize(18);
			txtDen.BackgroundColor = UIColor.White;
			txtDen.BorderStyle = UITextBorderStyle.RoundedRect;
			txtDen.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			txtDen.ClearButtonMode = UITextFieldViewMode.WhileEditing;
			txtDen.EditingDidBegin += delegate {
				//prevent keyboard from popping up
				txtDen.ResignFirstResponder();
				txtDen.InputView = pickerDen;
				actionSheetSize = new SizeF (this.View.Frame.Width, pickerDen.Frame.Height + toolbarDen.Frame.Height);
				RectangleF actionSheetFrame = new RectangleF (0, this.View.Frame.Height - actionSheetSize.Height, actionSheetSize.Width, actionSheetSize.Height);
				myActionSheet.ShowInView(this.View);
				myActionSheet.Frame = actionSheetFrame;
				pickerDen.Frame = new RectangleF(0, toolbarDen.Frame.Height, myActionSheet.Frame.Width, myActionSheet.Frame.Height - toolbarDen.Frame.Height);
			};
			myScrollView.AddSubview(txtDen);

			myLabel = new UILabel(new RectangleF(10,195,100,31));
			myLabel.Text = "Mom's Name";
			myLabel.BackgroundColor = UIColor.Clear;
			myLabel.Font = UIFont.SystemFontOfSize(18);
			myLabel.AdjustsFontSizeToFitWidth = true;
			myScrollView.AddSubview(myLabel);

			txtMomName = new UITextField(new RectangleF(112,195,190,31));
			txtMomName.Font = UIFont.SystemFontOfSize(18);
			txtMomName.BackgroundColor = UIColor.White;
			txtMomName.BorderStyle = UITextBorderStyle.RoundedRect;
			txtMomName.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			txtMomName.ClearButtonMode = UITextFieldViewMode.WhileEditing;
			myScrollView.AddSubview(txtMomName);

			myLabel = new UILabel(new RectangleF(10,231,100,31));
			myLabel.Text = "Dad's Name";
			myLabel.BackgroundColor = UIColor.Clear;
			myLabel.Font = UIFont.SystemFontOfSize(18);
			myLabel.AdjustsFontSizeToFitWidth = true;
			myScrollView.AddSubview(myLabel);

			txtDadName = new UITextField(new RectangleF(112,231,190,31));
			txtDadName.Font = UIFont.SystemFontOfSize(18);
			txtDadName.BackgroundColor = UIColor.White;
			txtDadName.BorderStyle = UITextBorderStyle.RoundedRect;
			txtDadName.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			txtDadName.ClearButtonMode = UITextFieldViewMode.WhileEditing;
			myScrollView.AddSubview(txtDadName);

			myLabel = new UILabel(new RectangleF(10,267,100,31));
			myLabel.Text = "Email";
			myLabel.BackgroundColor = UIColor.Clear;
			myLabel.Font = UIFont.SystemFontOfSize(18);
			myScrollView.AddSubview(myLabel);
			
			txtEmail = new UITextField(new RectangleF(112,267,190,31));
			txtEmail.Font = UIFont.SystemFontOfSize(18);
			txtEmail.BackgroundColor = UIColor.White;
			txtEmail.BorderStyle = UITextBorderStyle.RoundedRect;
			txtEmail.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			txtEmail.ClearButtonMode = UITextFieldViewMode.WhileEditing;
			txtEmail.KeyboardType = UIKeyboardType.EmailAddress;
			myScrollView.AddSubview(txtEmail);

			myLabel = new UILabel(new RectangleF(10,303,100,31));
			myLabel.Text = "Home Phone";
			myLabel.BackgroundColor = UIColor.Clear;
			myLabel.Font = UIFont.SystemFontOfSize(18);
			myLabel.AdjustsFontSizeToFitWidth = true;
			myScrollView.AddSubview(myLabel);
			
			txtHomePhone = new UITextField(new RectangleF(112,303,190,31));
			txtHomePhone.Font = UIFont.SystemFontOfSize(18);
			txtHomePhone.BackgroundColor = UIColor.White;
			txtHomePhone.BorderStyle = UITextBorderStyle.RoundedRect;
			txtHomePhone.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			txtHomePhone.ClearButtonMode = UITextFieldViewMode.WhileEditing;
			myScrollView.AddSubview(txtHomePhone);

			myLabel = new UILabel(new RectangleF(10,339,100,31));
			myLabel.Text = "Mobile Phone";
			myLabel.BackgroundColor = UIColor.Clear;
			myLabel.Font = UIFont.SystemFontOfSize(18);
			myLabel.AdjustsFontSizeToFitWidth = true;
			myScrollView.AddSubview(myLabel);
			
			txtMobilePhone = new UITextField(new RectangleF(112,339,190,31));
			txtMobilePhone.Font = UIFont.SystemFontOfSize(18);
			txtMobilePhone.BackgroundColor = UIColor.White;
			txtMobilePhone.BorderStyle = UITextBorderStyle.RoundedRect;
			txtMobilePhone.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			txtMobilePhone.ClearButtonMode = UITextFieldViewMode.WhileEditing;
			myScrollView.AddSubview(txtMobilePhone);

		}

		public void PickerButtonCancelHandler(object sender, EventArgs e)
		{
			myActionSheet.DismissWithClickedButtonIndex(0,true);
		}

		public void PickerButtonDoneHandler(object sender, EventArgs e)
		{
			txtDen.Text = myDenPicker.SelectedItem;
			myActionSheet.DismissWithClickedButtonIndex(0,true);
		}
	}

	public class DenPicker : UIPickerViewModel
	{
		public event EventHandler<EventArgs> ValueChanged;
		
		public List<string> Items
		{
			get { return this._items;} 
			set { this._items = value;}
		}
		List<string> _items = new List<string>();
		
		public string SelectedItem
		{
			get { return this._items[this._selectedIndex]; }
		}
		protected int _selectedIndex = 0;
		
		public DenPicker()
		{	
		}
		
		public override int GetRowsInComponent (UIPickerView picker, int component)
		{
			return this._items.Count;
		}
		
		public override string GetTitle (UIPickerView picker, int row, int component)
		{
			return this._items[row];
		}
		
		public override int GetComponentCount (UIPickerView picker)
		{
			return 1;
		}
		
		public override void Selected (UIPickerView picker, int row, int component)
		{
			this._selectedIndex = row;
			if (this.ValueChanged != null)
			{
				this.ValueChanged (this, new EventArgs());
			}
		}
	}

	public static class ViewExtensions
	{
		public static UIView FindFirstResponder (this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews) {
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}
		
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsAssignableFrom(view.Superview.GetType()))
				{
					return view.Superview;
				}
				
				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}
			
			return null;
		}
	}
}

