
using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Pack957
{
	public partial class ViewScout : DialogViewController
	{
		public string ScoutName { get; set; }
		public string NickName { get; set; }

		public ViewScout () : base (UITableViewStyle.Grouped, null)
		{
			Root = new RootElement (ScoutName) {
				new Section (ScoutName){
					new StringElement ("Nickname", NickName),
					//new StringElement ("Name", "Enter your name", String.Empty)
				},
			};
		}
	}
}
