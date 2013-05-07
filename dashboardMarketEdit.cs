// dashboardMarketEdit.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
using System;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using CoreUtilities;
using Layout;
using System.Collections.Generic;
using System.Reflection;
using MefAddIns;
namespace Submissions
{
	// wrapper for the market editing
	public class dashboardMarketEdit : Panel
	{

		#region variables
		string MARKET_GUID= Constants.BLANK;
		string ProjectGUID = Constants.BLANK;
		int ProjectWords = 0;
		#endregion
		#region gui
		PropertyGrid tmpEditor = null;
		ListBox PreviousSubmissions = null;
		ListBox Destinations = null;
		Button AddMarket = null;
		Button EditMarket = null;	
		Button EditMarketCancel = null;
		Label SubLabel = null;
		Label DestLabel = null;
		RichTextBox richBox = null;

		ListBox ListOfMarkets = null;
		GroupBox MarketFilters = null;
		CheckBox WordBox = null;
		CheckBox RetiredCheck = null;
		CheckBox HideAlreadySentCheck = null;
		CheckBox HideOccupiedCheck = null;

		ComboBox PublishingTypeBox = null;
		ComboBox MarketTypeBox = null;

		DataView ViewOfTheData = null;
		Label Count = null;
#endregion
		#region delegates
		Action<string, string> UpdateSelectedMarket=null;
		//	Func<int> GetProjectWords=null;
		//	Func<string> GetProjectGUID=null;
#endregion
		// take the currently selected ROW and return a valid object
		public Market SelectedMarketAsObject ()
		{
			try {
				//NewMessage.Show(ListOfMarkets.SelectedItem.GetType ().ToString ());
				if (ListOfMarkets.SelectedItem != null && ListOfMarkets.SelectedItem.GetType () == typeof(DataRowView)) {
					return new Market (((DataRowView)ListOfMarkets.SelectedItem).Row);
				} else
					return null;

			} catch (Exception ex) {
				throw new Exception(ex.ToString ());
			}
		}

		// returns true if current market has profanity restrictions
		public  bool Profanity ()
		{
			return SelectedMarketAsObject().ProfanityWarning;
		}
	

//		public object CurrentMarketPrint {
//			get { return "TBD";}
//			set { ;}
//		}
//
//		public object CurrentMarketType {
//			get { return "TBD_2";}
//			set { ;}
//		}

		public void ClearSelection ()
		{
			// clear selection to force a refresh (user has to select a market
			ListOfMarkets.SelectedIndex = -1;
		}


		GroupBox BuildMarketFilterBox ()
		{
			GroupBox _MarketFilters = new GroupBox ();
			_MarketFilters.Text = Loc.Instance.GetString ("Filters");
			_MarketFilters.Dock = DockStyle.Bottom;
			_MarketFilters.Width = 200;
			_MarketFilters.Height = 220;

			Count = new Label ();
			Count.Text = "0";
			Count.Dock = DockStyle.Top;

			WordBox = new CheckBox ();
			WordBox.Checked = false;
			WordBox.Dock = DockStyle.Top;
			WordBox.Click += HandleOptionClick;
			WordBox.Text = Loc.Instance.GetString ("By Words: <project>");


			RetiredCheck = new CheckBox ();
			RetiredCheck.Dock = DockStyle.Top;
			RetiredCheck.Checked = false;
			RetiredCheck.Click += HandleOptionClick;
			RetiredCheck.Text = Loc.Instance.GetString ("Show Retired");


			HideAlreadySentCheck = new CheckBox ();
			HideAlreadySentCheck.Dock = DockStyle.Top;
			HideAlreadySentCheck.Checked = true;
			HideAlreadySentCheck.Click += HandleOptionClick;
			HideAlreadySentCheck.Text = Loc.Instance.GetString ("Hide if Sent To Already");

			HideOccupiedCheck = new CheckBox ();
			HideOccupiedCheck.Dock = DockStyle.Top;
			HideOccupiedCheck.Checked = true;
			HideOccupiedCheck.Click += HandleOptionClick;
			HideOccupiedCheck.Text = Loc.Instance.GetString ("Hide Busy Markets");


			PublishingTypeBox = new ComboBox ();
			PublishingTypeBox.DropDownStyle = ComboBoxStyle.DropDownList;
			List<string> publishtypes = LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable (Addin_Submissions.SYSTEM_PUBLISHTYPES, 1);
			publishtypes.Add (Constants.BLANK);
			PublishingTypeBox.Dock = DockStyle.Top;
			foreach (string s in publishtypes) {
				PublishingTypeBox.Items.Add (s);
			}
			PublishingTypeBox.SelectedIndexChanged+= (object sender, EventArgs e) => UpdateFilter();


			MarketTypeBox = new ComboBox ();
			MarketTypeBox.DropDownStyle = ComboBoxStyle.DropDownList;
			List<string> markettypes = LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable (Addin_Submissions.SYSTEM_MARKETTYPES, 1);
			markettypes.Add (Constants.BLANK);
			MarketTypeBox.Dock = DockStyle.Top;
			foreach (string s in markettypes) {
				MarketTypeBox.Items.Add (s);
			}
			MarketTypeBox.SelectedIndexChanged+= (object sender, EventArgs e) => UpdateFilter();






			_MarketFilters.Controls.Add (MarketTypeBox);
			_MarketFilters.Controls.Add (PublishingTypeBox);
			_MarketFilters.Controls.Add (HideOccupiedCheck);
			_MarketFilters.Controls.Add (HideAlreadySentCheck);
			_MarketFilters.Controls.Add (RetiredCheck);
			_MarketFilters.Controls.Add (WordBox);
			_MarketFilters.Controls.Add (Count);
			return _MarketFilters;
		}

		void HandleOptionClick (object sender, EventArgs e)
		{
			UpdateFilter();
		}

		public void UpdateProjectInformationForFilterBox (string _ProjectGUID, int _ProjectWords)
		{
			ProjectGUID = _ProjectGUID;
			ProjectWords = _ProjectWords;

			WordBox.Text = Loc.Instance.GetStringFmt("By Words: {0}", ProjectWords);
			UpdateFilter ();
		}
		private string BuildRowFilter ()
		{
			List<string> ExcludedGuids = new List<string> ();


			// FOR BUSY :  TRICK: Build a List of ExcludedGUIDS -- populated by ALreadySentHere and Busy -- queries against the TRANSACTION TABLE
			// We then generate a list of  And if not guid='anexcludeguide', that gets added to the rowfilter
			// These will be more exepsnive, either post processing on the list or some kidn of generated field
			if (true == HideAlreadySentCheck.Checked && Constants.BLANK != ProjectGUID) {
				List<Transactions.TransactionBase> Submissions = SubmissionMaster.GetListOfSubmissionsForProject (ProjectGUID);

				foreach (Transactions.TransactionSubmission sub in Submissions) {
					// we add every market sent to, to this list.
					ExcludedGuids.Add (sub.MarketGuid);

				}

			}

			if (true == HideOccupiedCheck.Checked) {
				List<string> BusyMarkets = SubmissionMaster.GetListOfGuidsOfBusyMarkets(CurrentLayout);
				foreach (string s in BusyMarkets)
				{
					ExcludedGuids.Add (s);
				}
			}

			string filter = "";
			if (true == RetiredCheck.Checked)
				filter = String.Format ("(Retired=1 or Retired=0)");
			else
				filter = String.Format ("Retired=0");

			// because we always put the Retired filter above we KNOW we add any other filters with an And
			if (PublishingTypeBox.Text != Constants.BLANK) {
				filter = String.Format ("{0} and PublishType='{1}'", filter, PublishingTypeBox.Text);
			}
			if (MarketTypeBox.Text != Constants.BLANK) {
				filter = String.Format ("{0} and MarketType='{1}'", filter, MarketTypeBox.Text);
			}

			if (true == WordBox.Checked)
				filter = String.Format ("{0} and MinimumWord <= {1} and MaximumWord >= {1}", filter, ProjectWords);


			string excludefilter = Constants.BLANK;
			// build excluded this
			foreach (string exclude in ExcludedGuids) {
				excludefilter = String.Format ("{0} and Guid<>'{1}'", excludefilter, exclude);
			}

			if (Constants.BLANK != excludefilter) {
				filter = String.Format ("{0} {1}", filter, excludefilter);
			}

			return filter;
		}
		// this is called from internal and also external (NoteDataXML_Submission) to initially populate list
		public void UpdateFilter ()
		{
			ViewOfTheData.RowFilter = BuildRowFilter ();//String.Format ("MinimumWord <= {0} and MaximumWord >= {0}", ProjectWords);

			if (ListOfMarkets.Items.Count > 0) {
				// we always want to select the topmost one
				ListOfMarkets.SelectedIndex = 0;
				UpdateAfterListSelection();
			} else {
				// We clear this if nothing in the list.
				tmpEditor.SelectedObject = null;
			}
		}
		private void OnListChanged (object sender, 
		                           System.ComponentModel.ListChangedEventArgs args)
		{
			Count.Text = Loc.Instance.GetStringFmt("Found: {0}", ViewOfTheData.Count.ToString());
		}

		public bool WordsOver (string selectedGUID)
		{
			Market temp = this.SelectedMarketAsObject ();
			if (Constants.BLANK != selectedGUID) {
				int Words = MasterOfLayouts.GetWordsFromGuid (selectedGUID);
				if (temp.MinimumWord > Words || temp.MaximumWord < Words) {
					return true;
				}
			}
			return false;
		}
		LayoutPanelBase CurrentLayout = null;
		public dashboardMarketEdit (DataTable _dataSource, Action<string, string> _UpdateSelectedMarket, LayoutPanelBase _CurrentLayout)
		{
			CurrentLayout = _CurrentLayout;
			if (null == _dataSource)
				throw new Exception ("invalid data source passed in");
			UpdateSelectedMarket = _UpdateSelectedMarket;

			MarketFilters = BuildMarketFilterBox ();


			//	NewMessage.Show ("boo");
			ListOfMarkets = new ListBox ();
		

			if (_dataSource.PrimaryKey.Length == 0) {
				_dataSource.PrimaryKey = new DataColumn[] { _dataSource.Columns ["Guid"] };
			}


			ViewOfTheData = new DataView (_dataSource);
			ViewOfTheData.Sort = "Caption ASC";
			ViewOfTheData.ListChanged += new System.ComponentModel.ListChangedEventHandler (OnListChanged);

			//ViewOfTheData.RowFilter = BuildRowFilter();

			ListOfMarkets.DataSource = ViewOfTheData;//_dataSource;
			ListOfMarkets.SelectedIndexChanged += HandleMarketListSelectedIndexChanged;
			ListOfMarkets.DisplayMember = "Caption";

			//ListOfMarkets.DoubleClick+= HandleListOfDoubleClick;
			//	ListOfMarkets.MouseDown+= HandleListOfMarketsMouseDown;



			//ListOfMarkets.Sorted = true;
			//ListOfMarkets.Width = 200;
			//
			// TAB CONTROL SIDE
			//
		

			TabControl Tabs = new TabControl ();
			Tabs.Dock = DockStyle.Fill;

		
			TabPage MarketEditing = new TabPage ();
			MarketEditing.Text = Loc.Instance.GetString ("Market Details");

			TabPage MarketSubmissions = new TabPage ();
			MarketSubmissions.Text = Loc.Instance.GetString ("Market Submissions");


			TabPage MarketNotes = new TabPage ();
			MarketNotes.Text = Loc.Instance.GetString ("Market Notes");

			Tabs.TabPages.Add (MarketEditing);
			Tabs.TabPages.Add (MarketNotes);
			Tabs.TabPages.Add (MarketSubmissions);



			Tabs.SelectedIndexChanged += HandleMiniTabSelectedIndexChanged;
		

			SubLabel = new Label ();
			SubLabel.Text = Loc.Instance.GetString ("Submissions");
			SubLabel.Dock = DockStyle.Top;
			DestLabel = new Label ();
			DestLabel.Dock = DockStyle.Bottom;
			DestLabel.Text = Loc.Instance.GetString ("Destinations");


			PreviousSubmissions = new ListBox ();
			PreviousSubmissions.DoubleClick += HandlePreviousSubmissionsDoubleClick;
			PreviousSubmissions.Dock = DockStyle.Fill;
			//PreviousSubmissions.Height = 100;

			Destinations = new ListBox ();
			Destinations.Dock = DockStyle.Bottom;
			Destinations.Height = 100;


			tmpEditor = new PropertyGrid ();
			tmpEditor.Dock = DockStyle.Fill;
			tmpEditor.PropertyValueChanged += HandlePropertyValueChanged;
			tmpEditor.Height = 300;


			MarketEditing.Controls.Add (tmpEditor);
			MarketSubmissions.Controls.Add (DestLabel);
			MarketSubmissions.Controls.Add (PreviousSubmissions);
			PreviousSubmissions.BringToFront ();
		
			MarketSubmissions.Controls.Add (SubLabel);
			MarketSubmissions.Controls.Add (Destinations);


			//
			// MAIN TABlE
			//

			//	Panel EasyMarketEdit = new Panel();
			
			//	EasyMarketEdit.BackColor = Color.Green;
			//	EasyMarketEdit.Height = 200;
			
			//	EasyMarketEdit.Dock = DockStyle.Fill;

			//	EasyMarketEdit.Controls.Add (Tabs);

			//	tmpEditor.Enabled = false;
			tmpEditor.BringToFront ();



		


			AddMarket = new Button ();
			AddMarket.Dock = DockStyle.Bottom;
			AddMarket.Text = Loc.Instance.GetString ("Add Market");
			AddMarket.Click += HandleAddMarketClick;

			EditMarket = new Button ();
			EditMarket.Text = Loc.Instance.GetString ("Save Edits");
			EditMarket.Dock = DockStyle.Fill;
			EditMarket.Enabled = false;
			EditMarket.Click += HandleEditMarketClick;

			EditMarketCancel = new Button ();
			EditMarketCancel.Dock = DockStyle.Fill;
			EditMarketCancel.Text = Loc.Instance.GetString ("Cancel Edit");
			EditMarketCancel.Enabled = false;
			EditMarketCancel.Click += HandleEditMarketCancelClick;
			
			TableLayoutPanel MarketListPanel = new TableLayoutPanel ();
			
			MarketListPanel.RowCount = 2;
			MarketListPanel.ColumnCount = 3;


			MarketListPanel.Controls.Add (EditMarket, 1, 0);
			MarketListPanel.Controls.Add (AddMarket, 0, 0);
			MarketListPanel.Controls.Add (EditMarketCancel, 2, 0);


			//MarketListPanel.Controls.Add (EditMarketCancel, 1, 0);

			//MarketListPanel.Controls.Add (ListOfMarkets, 0, 1);
			MarketListPanel.Controls.Add (Tabs, 1, 1);
			MarketListPanel.SetColumnSpan (Tabs, 2);


		
		//	MarketListPanel.Controls.Add (PreviousSubmissions, 1, 2);

		//	MarketListPanel.Controls.Add (AddMarket, 0, 3);



		
		

			
			Panel RightSide = new Panel();

			RightSide.Controls.Add (MarketFilters);
			RightSide.Controls.Add (ListOfMarkets);



			ListOfMarkets.Dock = DockStyle.Fill;
			ListOfMarkets.BringToFront();
			RightSide.Dock = DockStyle.Fill;
			RightSide.BringToFront ();

			MarketListPanel.Controls.Add (RightSide, 0, 1);
			 
			MarketListPanel.SetRowSpan(Tabs, 2);
			MarketListPanel.SetRowSpan(RightSide, 2);
			MarketListPanel.Dock = DockStyle.Fill;



			//this.Controls.Add (AddMarket);
		
			//NewMessage.Show ("boo2");
			this.Controls.Add (MarketListPanel);
			MarketListPanel.BringToFront();
		//	AddMarket.SendToBack();




			//
			// Setup Market Notes Pages
			//

			richBox = new RichTextBox();
			MarketNotes.Controls.Add (richBox);
			richBox.Dock = DockStyle.Fill;
			richBox.KeyDown+= HandleNotesKeyDown;



			MarketListPanel.ColumnStyles.Clear();
			for (int i = 0; i < MarketListPanel.ColumnCount; i++)
			{
				ColumnStyle style = new ColumnStyle(SizeType.Percent, 33.0f);
				MarketListPanel.ColumnStyles.Add(style);
			}
			
			MarketListPanel.RowStyles.Clear();
			for (int i = 0; i < MarketListPanel.RowCount; i++)
			{
				MarketListPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			}
		}

		void HandlePreviousSubmissionsDoubleClick (object sender, EventArgs e)
		{
			DisplayNote();
		}


		/// We display the NOTE associated with the doubleclicked selected object
		void DisplayNote()
		{
		//	NewMessage.Show ("here0");
			if (PreviousSubmissions.SelectedItem != null) {
//NewMessage.Show ("here");
				string rtf_Notes = ((Transactions.TransactionSubmission)PreviousSubmissions.SelectedItem).Notes;
			//	NewMessage.Show (rtf_Notes);
				if (rtf_Notes != null && rtf_Notes!= Constants.BLANK)
				{
					GenericTextForm form = new GenericTextForm();
					try
					{
						form.GetRichTextBox().Rtf = rtf_Notes;
					}
					catch (Exception)
					{
						form.GetRichTextBox().Text = rtf_Notes;
					}
					form.ShowDialog();
				}
			}
		}


		void HandleNotesKeyDown (object sender, KeyEventArgs e)
		{
			EditMode = true;
		}

		void HandleMiniTabSelectedIndexChanged (object sender, EventArgs e)
		{
			BuildListOfMarketSubs();
		}

		void HandleEditMarketCancelClick (object sender, EventArgs e)
		{
			EditMode = false;
		}

		void HandlePropertyValueChanged (object s, PropertyValueChangedEventArgs e)
		{
			EditMode = true;
		}
		//http://forums.asp.net/p/904608/998033.aspx
		public static void AddMarketRow(PropertyInfo[] properties, DataTable dt, Object o)
		{
			DataRow dr = dt.NewRow();
			foreach(PropertyInfo pi in properties)
			{
				dr[pi.Name] = pi.GetValue(o, null);
			}
			dt.Rows.Add(dr); 
		}


		public static void EditMarketRow (PropertyInfo[] properties, DataTable dt, Object o, string guid)
		{
			DataRow[] FoundRows = null;



			// looks for guid and overrides that object
			FoundRows = dt.Select (String.Format ("Guid='{0}'", guid));
			if (FoundRows != null && FoundRows.Length > 0) {
				FoundRows[0].BeginEdit();
				foreach(PropertyInfo pi in properties)
				{

					FoundRows[0][pi.Name] = pi.GetValue(o, null);
				}
				FoundRows[0].EndEdit();
			//	dt.Rows.Edit(FoundRows[0]);
				//dt.Rows.Add(FoundRows[0]); 
			}



		}
		DataTable GetDataViewTable()
		{
			// a wrapper to grab the associated datasource
			return ((DataView)ListOfMarkets.DataSource).Table;
		}

		void GoToMarketInList (string caption)
		{
			int index = 0;
		//	NewMessage.Show (ListOfMarkets.Items[0].ToString ());
			for (int i = 0; i < ListOfMarkets.Items.Count; i++) {
				DataRowView o = (DataRowView)ListOfMarkets.Items [i];
				if (o["Caption"].ToString() == caption) {
					index = i;
				}
			}
			ListOfMarkets.SelectedIndex = index;
		}

		void HandleAddMarketClick (object sender, EventArgs e)
		{
			PropertyInfo[] propertiesInfo = typeof(Market).GetProperties ();
			Market newMarket = Market.DefaultMarket ();
			 string caption = Loc.Instance.GetStringFmt("New Market {0}", DateTime.Now.ToShortDateString());
			newMarket.Caption = caption;
			AddMarketRow (propertiesInfo, 	 GetDataViewTable(), newMarket);
			GoToMarketInList(caption);

		}

		void SaveCurrentMarket ()
		{
			this.Cursor = Cursors.WaitCursor;

			// update text too
			((Market)tmpEditor.SelectedObject).Notes = richBox.Rtf;

			string caption = ((Market)tmpEditor.SelectedObject).Caption;
			PropertyInfo[] propertiesInfo = typeof(Market).GetProperties ();
			EditMarketRow(propertiesInfo, GetDataViewTable(), (Market)tmpEditor.SelectedObject, ((Market)tmpEditor.SelectedObject).Guid);
			this.Cursor = Cursors.Default;
			GoToMarketInList(caption);
		}

		void HandleEditMarketClick (object sender, EventArgs e)
		{
			// Saves the edits
			//NewMessage.Show ("Saves the edits");

			SaveCurrentMarket();

			EditMode = false;
		}
		string GetProjectFromGUID(string ProjectGUID)
		{
			return MasterOfLayouts.GetNameFromGuid(ProjectGUID);
		}
		private void BuildListForListBox (ListBox box, List<Transactions.TransactionBase> LayoutEvents)
		{
			box.DataSource = null;

			foreach (Transactions.TransactionBase tran in LayoutEvents) {
				if (tran is Transactions.TransactionSubmission || tran is Transactions.TransactionSubmissionDestination)
				{
					// hookup a lookup function
					((Transactions.TransactionSubmission)tran).GetProjectFromGUID = GetProjectFromGUID;

				}
			}
			//List<Transactions.TransactionBase> LayoutEvents = SubmissionMaster.GetListOfSubmissionsForMarket(MARKET_GUID);
			//LayoutDetails.Instance.TransactionsList.GetEventsForLayoutGuid (ProjectGUID,String.Format (" and type='{0}' ", TransactionsTable.T_SUBMISSION));
			if (LayoutEvents != null)
			{
				LayoutEvents.Sort ();
				LayoutEvents.Reverse ();
				if (LayoutEvents != null) {
					box.DataSource = LayoutEvents;
					box.DisplayMember = "DisplayVariant"; //DisplayVariant
					box.ValueMember = "ID";
				}
			}
			else
			{

				lg.Instance.Line ("dashboardMarketEdit.BuildListForListbox",ProblemType.MESSAGE, "Transaction list for this note was empty. Remove me after debugging.");
			}
		}

		public void BuildListOfMarketSubs ()
		{
		//	NewMessage.Show ("build list");
			if (null == LayoutDetails.Instance.TransactionsList) {
				throw new Exception("Transaction Table not created yet");
			}
			try {
				if (Constants.BLANK != MARKET_GUID)
				{
					List<Transactions.TransactionBase> LayoutEvents =SubmissionMaster.GetListOfSubmissionsForMarket(MARKET_GUID);
					BuildListForListBox(PreviousSubmissions, LayoutEvents);
					SubLabel.Text = Loc.Instance.GetStringFmt("Submissions ({0})", PreviousSubmissions.Items.Count);

					LayoutEvents =SubmissionMaster.GetListOfDestinationsForMarket(MARKET_GUID);
					BuildListForListBox(Destinations, LayoutEvents);
					DestLabel.Text =Loc.Instance.GetStringFmt("Destinations ({0})", Destinations.Items.Count);
				
				}
			} catch (Exception ex) {
				NewMessage.Show (ex.ToString());
			}
		}
		void HandleMarketListSelectedIndexChanged (object sender, EventArgs e)
		{
			UpdateAfterListSelection();
		}

		bool editMode = false;
		bool EditMode {
			get { return editMode;}
			set {

				editMode = value;
				ListOfMarkets.Enabled = !EditMode;
				AddMarket.Enabled = !EditMode;
				EditMarket.Enabled = EditMode;
				EditMarketCancel.Enabled= EditMode;
				MarketFilters.Enabled = !EditMode;

			}
		}

		private void ReadOnlyEditor(bool ReadOnly)

		{
			// This did not work and I choose to go with a "You started Editing, hence you enter edit mode
		//	System.ComponentModel.TypeDescriptor.AddAttributes(tmpEditor.SelectedObject, new Attribute[]{new System.ComponentModel.ReadOnlyAttribute(ReadOnly)});
		}

		private void UpdateAfterListSelection ()
		{
			//NewMessage.Show("six");
			if (UpdateSelectedMarket != null) {
				if (ListOfMarkets.SelectedItem != null)
				{
					try
					{
					DataRowView row = (DataRowView)ListOfMarkets.SelectedItem;
					string Name = row["Caption"].ToString ();
					MARKET_GUID = row["Guid"].ToString ();
					UpdateSelectedMarket(Name, MARKET_GUID);
				//		NewMessage.Show("seven");
						Market market = SelectedMarketAsObject();
						tmpEditor.SelectedObject = market;

						// load the text file
						richBox.Text = "";
						if (market.Notes != Constants.BLANK)
						{
							try{
								richBox.Rtf = market.Notes;
							}
							catch (Exception)
							{
								richBox.Text = market.Notes;
							}
						}



						//Attempting ReadOnly

						ReadOnlyEditor(true);
				//		NewMessage.Show("eight");
					BuildListOfMarketSubs();
					}
					catch (Exception x)
					{
						throw new Exception(x.ToString ());
					}

				}
			}
		}
	}
}

