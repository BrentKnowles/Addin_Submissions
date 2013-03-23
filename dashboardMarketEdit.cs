using System;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using CoreUtilities;
using Layout;
using System.Collections.Generic;
using System.Reflection;
namespace Submissions
{
	// wrapper for the market editing
	public class dashboardMarketEdit : Panel
	{

		#region variables
		string MARKET_GUID= Constants.BLANK;
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
		#region variables
		string ProjectGUID = Constants.BLANK;
		int ProjectWords = 0;
		#endregion
		#region delegates
		Action<string, string> UpdateSelectedMarket=null;
	//	Func<int> GetProjectWords=null;
	//	Func<string> GetProjectGUID=null;
		#endregion
		#region gui
		ListBox ListOfMarkets = null;
		GroupBox MarketFilters = null;
		CheckBox WordBox = null;
		DataView ViewOfTheData = null;
		Label Count = null;
		#endregion

		GroupBox BuildMarketFilterBox ()
		{
			GroupBox _MarketFilters =	new GroupBox();
			_MarketFilters.Text = Loc.Instance.GetString ("Filters");
			_MarketFilters.Dock = DockStyle.Bottom;
			_MarketFilters.Width = 200;
			_MarketFilters.Height = 150;

			Count = new Label();
			Count.Text = "0";
			Count.Dock = DockStyle.Top;

		    WordBox = new CheckBox();
			WordBox.Dock = DockStyle.Top;
			WordBox.Text =  Loc.Instance.GetString("By Words: <select a project>");

			_MarketFilters.Controls.Add (WordBox);
			_MarketFilters.Controls.Add (Count);
			return _MarketFilters;
		}

		public void UpdateProjectInformationForFilterBox (string _ProjectGUID, int _ProjectWords)
		{
			ProjectGUID = _ProjectGUID;
			ProjectWords = _ProjectWords;

			WordBox.Text = Loc.Instance.GetStringFmt("By Words: {0}", ProjectWords);
			UpdateFilter ();
		}
		private string BuildRowFilter()
		{
			string filter = "";
			if (WordBox.Checked == true) filter = String.Format ("MinimumWord <= {0} and MaximumWord >= {0}", ProjectWords);
			return filter;
		}
		private void UpdateFilter ()
		{
			ViewOfTheData.RowFilter =  BuildRowFilter();//String.Format ("MinimumWord <= {0} and MaximumWord >= {0}", ProjectWords);
		}
		private void OnListChanged (object sender, 
		                           System.ComponentModel.ListChangedEventArgs args)
		{
			Count.Text = ViewOfTheData.Count.ToString();
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

		public dashboardMarketEdit (DataTable _dataSource, Action<string, string> _UpdateSelectedMarket)
		{
			if (null == _dataSource) throw new Exception("invalid data source passed in");
			UpdateSelectedMarket = _UpdateSelectedMarket;

			MarketFilters = BuildMarketFilterBox();


		//	NewMessage.Show ("boo");
			ListOfMarkets = new ListBox();
		

			if (_dataSource.PrimaryKey.Length == 0) {
				_dataSource.PrimaryKey = new DataColumn[] { _dataSource.Columns ["Guid"] };
			}


			 ViewOfTheData = new DataView(_dataSource);
			ViewOfTheData.Sort = "Caption ASC";
			ViewOfTheData.ListChanged+= new System.ComponentModel.ListChangedEventHandler(OnListChanged);
			ViewOfTheData.RowFilter = BuildRowFilter();
			ListOfMarkets.DataSource = ViewOfTheData;//_dataSource;
			ListOfMarkets.SelectedIndexChanged+= HandleMarketListSelectedIndexChanged;
			ListOfMarkets.DisplayMember = "Caption";




			//ListOfMarkets.Sorted = true;
			//ListOfMarkets.Width = 200;
			//
			// TAB CONTROL SIDE
			//
		

			TabControl Tabs = new TabControl ();
			Tabs.Dock = DockStyle.Fill;

		
			TabPage MarketEditing = new TabPage();
			MarketEditing.Text = Loc.Instance.GetString ("Market Details");

			TabPage MarketSubmissions = new TabPage();
			MarketSubmissions.Text = Loc.Instance.GetString("Market Submissions");
			Tabs.TabPages.Add (MarketEditing);
			Tabs.TabPages.Add (MarketSubmissions);

			Tabs.SelectedIndexChanged+= HandleMiniTabSelectedIndexChanged;
		

			SubLabel = new Label();
			SubLabel.Text = Loc.Instance.GetString ("Submissions");
			SubLabel.Dock = DockStyle.Top;
			DestLabel = new Label();
			DestLabel.Dock = DockStyle.Bottom;
			DestLabel.Text = Loc.Instance.GetString("Destinations");


			PreviousSubmissions = new ListBox();
			PreviousSubmissions.Dock = DockStyle.Fill;
			//PreviousSubmissions.Height = 100;

			Destinations = new ListBox();
			Destinations.Dock = DockStyle.Bottom;
			Destinations.Height = 100;


			tmpEditor = new PropertyGrid();
			tmpEditor.Dock = DockStyle.Fill;
			tmpEditor.PropertyValueChanged+= HandlePropertyValueChanged;
			tmpEditor.Height = 300;


			MarketEditing.Controls.Add (tmpEditor);
			MarketSubmissions.Controls.Add (DestLabel);
			MarketSubmissions.Controls.Add (PreviousSubmissions);
			PreviousSubmissions.BringToFront();
		
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
			//TODO:  would prefer readonly 
		//	tmpEditor.Enabled = false;
			tmpEditor.BringToFront();



		


			AddMarket = new Button();
			AddMarket.Dock = DockStyle.Bottom;
			AddMarket.Text = Loc.Instance.GetString ("Add Market");
			AddMarket.Click+= HandleAddMarketClick;

			EditMarket = new Button();
			EditMarket.Text = Loc.Instance.GetString ("Save Edits");
			EditMarket.Dock = DockStyle.Fill;
			EditMarket.Enabled = false;
			EditMarket.Click+= HandleEditMarketClick;

			EditMarketCancel = new Button();
			EditMarketCancel.Dock = DockStyle.Fill;
			EditMarketCancel.Text = Loc.Instance.GetString ("Cancel Edit");
			EditMarketCancel.Enabled= false;
			EditMarketCancel.Click+= HandleEditMarketCancelClick;
			
			TableLayoutPanel MarketListPanel = new TableLayoutPanel();
			
			MarketListPanel.RowCount = 2;
			MarketListPanel.ColumnCount = 2;

			MarketListPanel.Controls.Add (EditMarket, 0, 0);
			MarketListPanel.Controls.Add (EditMarketCancel, 1, 0);
			//MarketListPanel.Controls.Add (EditMarketCancel, 1, 0);

			//MarketListPanel.Controls.Add (ListOfMarkets, 0, 1);
			MarketListPanel.Controls.Add (Tabs, 1, 1);
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



			this.Controls.Add (AddMarket);
		
			//NewMessage.Show ("boo2");
			this.Controls.Add (MarketListPanel);
			MarketListPanel.BringToFront();
			AddMarket.SendToBack();
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
		void HandleAddMarketClick (object sender, EventArgs e)
		{
			PropertyInfo[] propertiesInfo = typeof(Market).GetProperties ();
			Market newMarket = Market.DefaultMarket ();
			newMarket.Caption = Loc.Instance.GetStringFmt("New Market {0}", DateTime.Now.ToShortDateString());
			AddMarketRow (propertiesInfo, 	 GetDataViewTable(), newMarket);
		}

		void SaveCurrentMarket ()
		{
			this.Cursor = Cursors.WaitCursor;
			PropertyInfo[] propertiesInfo = typeof(Market).GetProperties ();
			EditMarketRow(propertiesInfo, GetDataViewTable(), (Market)tmpEditor.SelectedObject, ((Market)tmpEditor.SelectedObject).Guid);
			this.Cursor = Cursors.Default;
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
		private void BuildListForListBox (ListBox box, List<Transactions.TransactionSubmission> LayoutEvents)
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
				//TODO: Remove this message
				NewMessage.Show ("Transaction list for this note was empty. Remove me after debugging.");
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

					BuildListForListBox(PreviousSubmissions, SubmissionMaster.GetListOfSubmissionsForMarket(MARKET_GUID));
					SubLabel.Text = Loc.Instance.GetStringFmt("Submissions ({0})", PreviousSubmissions.Items.Count);
					BuildListForListBox(Destinations, SubmissionMaster.GetListOfDestinationsForMarket(MARKET_GUID));
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
					tmpEditor.SelectedObject = SelectedMarketAsObject();

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

