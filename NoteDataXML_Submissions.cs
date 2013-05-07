// NoteDataXML_Submissions.cs
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
using CoreUtilities;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using CoreUtilities.Links;
using System.ComponentModel;
using Layout;
using System.Xml.Serialization;
using System.Reflection;
using Submissions;
using CoreUtilities.Tables;
using Transactions;
using System.Collections.Generic;
using appframe;

namespace MefAddIns
{



	public class NoteDataXML_Submissions  : NoteDataXML_Table
	{
		#region const

		string DefaultProjectLabel = Loc.Instance.GetString ("<Select a Project>");
		#endregion

		public override int defaultHeight { get { return 500; } }

		public override int defaultWidth { get { return 300; } }
		#region variables
		public override bool IsLinkable { get { return false; } }


		private string SelectedName = Constants.BLANK;
		private string SelectedGUID = Constants.BLANK;
		private string SelectedMarket = Constants.BLANK;
		private string SelectedMarketGuid  = Constants.BLANK;


		private string currentFilter= Constants.BLANK;
		
		public string CurrentFilter {
			get {
				return currentFilter;
			}
			set {
				currentFilter = value;
			}
		}
		#endregion

		#region interface
		dashboardSubs SubmissionPanel = null;
		dashboardMarketEdit MarketEdit = null;
		TabControl Tabs = null;
		ViewProjectSubmissions ViewOfProjectSubmissions = null;
		Label LabelProject = null;
		Label Warnings = null;
		Label LabelMarket = null;
		Button ToggleBetweenListAndEditSubmissions = null;
		#endregion


		public override void Dispose ()
		{
			base.Dispose ();
		
		}

		public override void CommonConstructor ()
		{
			ReadOnly = true;
			base.CommonConstructor();
			Caption = Loc.Instance.GetString ("Submission Panel");

		}

		public NoteDataXML_Submissions () : base()
		{
			CommonConstructor ();
		}

		public NoteDataXML_Submissions (int height, int width):base(height, width)
		{
			CommonConstructor ();
		}

		private string GetProjectGUID ()
		{
			// to pass to other subforms
			// since we are being constanty updated whenever user
			// switches project
			// we can pass this ifnromatin to other places
			return SelectedGUID;
		}


		private void CreateSubmissionTypeTable()
		{
			NoteDataXML_Table randomTables = new NoteDataXML_Table(100, 100, new ColumnDetails[3]{new ColumnDetails("id",100), 
				new ColumnDetails("name",100), new ColumnDetails("code",100)} );
			randomTables.Caption = SubmissionMaster.TABLE_SubmissionTypes;
			randomTables.GuidForNote = SubmissionMaster.TABLE_SubmissionTypes;
			randomTables.Columns = new ColumnDetails[3]{new ColumnDetails("id",100), 
				new ColumnDetails("name",100), new ColumnDetails("code",100)};
			Layout.AddNote(randomTables);
			randomTables.CreateParent(Layout);
			
			randomTables.AddRow(new object[3]{"1", "Submission", @"submission"});
			randomTables.AddRow(new object[3]{"2", "Invalid", @"invalid"});
			randomTables.AddRow(new object[3]{"3", "Contest Entry", @"submission"});
			randomTables.AddRow(new object[3]{"4", "Query", @"none"});
			randomTables.AddRow(new object[3]{"5", "Followup Call", @"none"});
			randomTables.AddRow(new object[3]{"6", "Email Contest", @"submission"});
			randomTables.AddRow(new object[3]{"7", "Followup Letter", @"none"});
			randomTables.AddRow(new object[3]{"8", "Proposal", @"none"});
			// removed because only I need it for legacy work
		//	randomTables.AddRow(new object[3]{"9", "Destination",SubmissionMaster.CODE_DESTINATION });
		}


		private void CreateReplyTypeTable()
		{
			NoteDataXML_Table randomTables = new NoteDataXML_Table(100, 100, new ColumnDetails[3]{new ColumnDetails("id",100), 
				new ColumnDetails("name",100), new ColumnDetails("code",100)} );
			randomTables.Caption = SubmissionMaster.TABLE_ReplyTypes;
			randomTables.GuidForNote = SubmissionMaster.TABLE_ReplyTypes;
		//	randomTables.Columns =
			Layout.AddNote(randomTables);
			randomTables.CreateParent(Layout);

			randomTables.AddRow(new object[3]{"1", "Invalid",SubmissionMaster.CODE_NO_REPLY_YET});

			randomTables.AddRow(new object[3]{"2", "Closed", @"rejection"});
			randomTables.AddRow(new object[3]{"3", "Accepted", SubmissionMaster.CODE_ACCEPTANCE});
			randomTables.AddRow(new object[3]{"4", "No Response", @"rejection"});
			randomTables.AddRow(new object[3]{"5", "Query Rejected", @"rejection"});
			randomTables.AddRow(new object[3]{"6", "Rejection", @"rejection"});
			randomTables.AddRow(new object[3]{"7", "Request For Rewrite", SubmissionMaster.CODE_NO_REPLY_YET});
			randomTables.AddRow(new object[3]{"8", "Work Requested", @"rejection"});

		}

		private void CreateReplyFeedbackTable()
		{
			NoteDataXML_Table randomTables = new NoteDataXML_Table(100, 100, new ColumnDetails[3]{new ColumnDetails("id",100), 
				new ColumnDetails("name",100), new ColumnDetails("code",100)} );
			randomTables.Caption = SubmissionMaster.TABLE_ReplyFeedback;
			randomTables.GuidForNote = SubmissionMaster.TABLE_ReplyFeedback;
			//	randomTables.Columns =
			Layout.AddNote(randomTables);
			randomTables.CreateParent(Layout);
			
			randomTables.AddRow(new object[3]{"1", "Form", SubmissionMaster.CODE_FEEDBACK1});
			randomTables.AddRow(new object[3]{"2", "Personal", SubmissionMaster.CODE_FEEDBACK2});
			randomTables.AddRow(new object[3]{"3", "Encouraging",SubmissionMaster.CODE_FEEDBACK3});
			randomTables.AddRow(new object[3]{"4", "Almost",SubmissionMaster.CODE_FEEDBACK4});

		}

		public void PerformAnyActionToLoadTables ()
		{
			// We cannot build Tables during creation of a note. We have to do it when we call them, later.
			if (Layout.FindNoteByGuid (SubmissionMaster.TABLE_SubmissionTypes) == null) {
				CreateSubmissionTypeTable();
			}
			
			if (Layout.FindNoteByGuid (SubmissionMaster.TABLE_ReplyTypes) == null) {
				CreateReplyTypeTable();
			}

			if (Layout.FindNoteByGuid (SubmissionMaster.TABLE_ReplyFeedback) == null) {
				CreateReplyFeedbackTable();
			}
		}

		protected override void DoBuildChildren (LayoutPanelBase Layout)
		{
			base.DoBuildChildren (Layout);

			if (Columns.Length < 5) {
				PropertyInfo[] propertiesInfo = typeof(Market).GetProperties ();
				// rebuild Submision  Market Table
				DataTable Table2 = CreateDataTable (propertiesInfo);
				//	NewMessage.Show (Table.Columns.Count.ToString ());
				//ForceTableUpdate (Table2);
				dataSource = Table2;
				Table = new TablePanel (dataSource, HandleCellBeginEdit, Columns, GoToNote, this.Caption, GetRandomTableResults);
				Table.Dock = DockStyle.Fill;
				Table.BringToFront ();

				dashboardMarketEdit.AddMarketRow (propertiesInfo, Table2, Market.DefaultMarket ());
			}





		
			//ToolTip Tipster = new ToolTip ();
		
			CaptionLabel.Dock = DockStyle.Top;
		
			Tabs = new TabControl ();
			Tabs.Margin = new Padding(5);
			Tabs.Dock = DockStyle.Fill;

			TabPage SubmissionPage = new TabPage (Loc.Instance.GetString ("Submissions"));
			TabPage MarketList = new TabPage (Loc.Instance.GetString ("Markets"));
			TabPage MarketAdvanced = new TabPage (Loc.Instance.GetString ("Advanced"));

			Tabs.SelectedIndexChanged += HandleSelectedIndexTabPagesChanged;
			Tabs.TabPages.Add (SubmissionPage);
			Tabs.TabPages.Add (MarketList);
			Tabs.TabPages.Add (MarketAdvanced);


			//
			// SUBMISSION PAGE SETUP
			//
		
		
			SubmissionPanel = new dashboardSubs (dashBoardsSubmissionsIsUpdatingProject, true);
			SubmissionPanel.SupressRefresh= true;
			SubmissionPanel.Dock = DockStyle.Fill;
			SubmissionPage.Controls.Add (SubmissionPanel);
			SubmissionPanel.BringToFront ();

			 ToggleBetweenListAndEditSubmissions = new Button ();
		
			ToggleBetweenListAndEditSubmissions.Dock = DockStyle.Bottom;
			ToggleBetweenListAndEditSubmissions.Enabled = false;
			// we use tag to disctate the text 0 = pressme to get list of submission
			// 1 mean sgo back to Submission Overview
			ToggleBetweenListAndEditSubmissions.Tag = 0;
			UpdateToggleButtonText();
			ToggleBetweenListAndEditSubmissions.Click += HandleToggleBetweenListAndEditingClick;




			SubmissionPage.Controls.Add (ToggleBetweenListAndEditSubmissions);

			ViewOfProjectSubmissions = new ViewProjectSubmissions (GetProjectGUID, GetMarketObjectByGUID, this.Layout);
			ViewOfProjectSubmissions.Visible = false;
			ViewOfProjectSubmissions.Dock = DockStyle.Fill;


			SubmissionPage.Controls.Add (ViewOfProjectSubmissions);
			ViewOfProjectSubmissions.BringToFront ();

			//
			// MARKET
			//
			if (dataSource == null) throw new Exception("null datasource");

			 MarketEdit = new dashboardMarketEdit (dataSource, dashBoardsMarketUpdating, this.Layout);

			MarketEdit.Dock = DockStyle.Fill;

		

			MarketList.Controls.Add (MarketEdit);
			MarketEdit.BringToFront ();

			//
			// Market Page (Advanced) Setup
			//
			ParentNotePanel.Controls.Remove (this.Table);
			MarketAdvanced.Controls.Add (this.Table);
		
			//
			// Submit Footer
			//
		
		
			GroupBox SubmitPanel = new GroupBox ();
			SubmitPanel.BackColor = Color.Lavender;
			SubmitPanel.ForeColor = Color.Black;
			SubmitPanel.Height = 150;
			SubmitPanel.Padding = new Padding(5);
			SubmitPanel.Dock = DockStyle.Bottom;
			SubmitPanel.Text = Loc.Instance.GetString ("ADD A SUBMISSION");
		
			LabelProject = new Label ();
			LabelProject.Text = DefaultProjectLabel; //Loc.Instance.GetString ("Current Project: ");
			LabelProject.Dock = DockStyle.Top;

			LabelMarket = new Label ();
			LabelMarket.Text = Loc.Instance.GetString ("Current Market: ");
			LabelMarket.Dock = DockStyle.Top;

			Warnings = new Label ();
			Warnings.Text = Loc.Instance.GetString ("Warnings ");
			Warnings.Dock = DockStyle.Bottom;


			Button AddSubmission = new Button ();
			AddSubmission.Text = Loc.Instance.GetString ("Submit this project to this market");
			AddSubmission.Click += HandleAddSubmissionClick;
			AddSubmission.Dock = DockStyle.Bottom;



			SubmitPanel.Controls.Add (LabelMarket);
			SubmitPanel.Controls.Add (LabelProject);

			SubmitPanel.Controls.Add (Warnings);

			SubmitPanel.Controls.Add (AddSubmission);

			ComboBox LastQuery = new ComboBox ();
			System.Collections.Generic.List<string> queries = LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable (LayoutDetails.SYSTEM_QUERIES, 1);
			queries.Sort ();
			LastQuery.DropDownStyle = ComboBoxStyle.DropDownList;
			foreach (string s in queries) {
				LastQuery.Items.Add (s);
			}
			//LastQuery.SelectedItem = CurrentFilter;

			int lastQueryIndex =queries.IndexOf(CurrentFilter);// queries.Find(s=>s==CurrentFilter);
			LastQuery.SelectedIndex = lastQueryIndex;
			LastQuery.SelectedIndexChanged+= HandleSelectedIndexLastQueryChanged;
			SubmissionPanel.CurrentFilter = CurrentFilter;
			//LastQuery.Text = "LastQuery";
			LastQuery.Dock = DockStyle.Top;
			SubmissionPage.Controls.Add (LastQuery);


			ParentNotePanel.Controls.Add (SubmitPanel);
		
			ParentNotePanel.Controls.Add (Tabs);
			Tabs.BringToFront();

			// Had to remove this because the table lookups require
			// the presence of the accessory tables which might not exist
			// at this point in the load cycle
		//	SubmissionPanel.RefreshMe();
			SubmissionPanel.SupressRefresh= false;


			Table.ReadOnly = this.ReadOnly;

			LayoutDetails.Instance.UpdateAfterLoadList.Add (this);
		}

		void UpdateToggleButtonText ()
		{
			if ((int)ToggleBetweenListAndEditSubmissions.Tag == 0) {
				ToggleBetweenListAndEditSubmissions.Text = Loc.Instance.GetStringFmt ("Flip: Submissions For '{0}'", SelectedName);
			} else {
				ToggleBetweenListAndEditSubmissions.Text = Loc.Instance.GetString ("Flip: Submission Overview");
			}
		}


		public Market GetMarketObjectByGUID (string Guid)
		{
			Market result = null;
			foreach (DataRow row in dataSource.Rows) {
				if (row["guid"].ToString() == Guid)
				{
					result = new Market(row);
				}
			}
			return result;


		}
		void HandleSelectedIndexTabPagesChanged (object sender, EventArgs e)
		{
			PerformAnyActionToLoadTables();
			// I was going to clear the selection here but thought it would make things too inconvenient
			//MarketEdit.ClearSelection();
		}

		void HandleSelectedIndexLastQueryChanged (object sender, EventArgs e)
		{
			PerformAnyActionToLoadTables();
			SetSaveRequired(true);
			if ((sender as ComboBox).SelectedItem != null && (sender as ComboBox).SelectedItem.ToString () != Constants.BLANK) {
				//NewMessage.Show ("Set to " + (sender as ComboBox).SelectedItem.ToString ());
				SubmissionPanel.CurrentFilter = (sender as ComboBox).SelectedItem.ToString ();
				SubmissionPanel.RefreshMe();
			} else {
				//NewMessage.Show ("Did not set");
			}
		}

		private void dashBoardsMarketUpdating (string MarketName, string MarketGUID)
		{
			SelectedMarket = MarketName;
			SelectedMarketGuid = MarketGUID;
		//	NewMessage.Show ("here2");
			LabelMarket.Text = Loc.Instance.GetStringFmt ("Market: {0}", SelectedMarket);
		
			RefreshWarningsAfterMarketAndProjectUpdated();

		}


		enum Warning {PROFANITY, WORDS, PROJECT_BUSY, MARKET_BUSY, PROJECT_HERE_BEFORE};
	

		private void RefreshWarningsAfterMarketAndProjectUpdated ()
		{
			List<Warning> WarningsList = new List<Warning> ();
			//	Warnings.Text = Loc.Instance.GetStringFmt ("Warnings updated");

			if (MarketEdit.Profanity () == true) {
				WarningsList.Add (Warning.PROFANITY);
			}

			if (Constants.BLANK != SelectedGUID) {
				if (MarketEdit.WordsOver (SelectedGUID) == true) {
					WarningsList.Add (Warning.WORDS);
				}
				if ( ViewOfProjectSubmissions.Available == false ) {
					WarningsList.Add (Warning.PROJECT_BUSY);
				}
				bool ProjectSentHereBefore = false;
				bool MarketAvailable = true;
				SubmissionMaster.GetMarketDetailsForWarnings(SelectedMarketGuid, SelectedGUID, out MarketAvailable, out ProjectSentHereBefore);
				if (false ==MarketAvailable)
				{
					WarningsList.Add (Warning.MARKET_BUSY);
				}
				if (true == ProjectSentHereBefore)
				{
					WarningsList.Add (Warning.PROJECT_HERE_BEFORE);
				}
			}



			// now process the warnings

			string WarningText = Constants.BLANK;
			foreach (Warning warning in WarningsList) {

				string adder = Constants.BLANK;
				switch (warning) {
				case Warning.PROFANITY:
					adder = Loc.Instance.GetString ("Profanity -- PG13 or less!");
					break;

				case Warning.WORDS:
					adder = Loc.Instance.GetString ("Project has too many or too few words for this market.");
					break;
				case Warning.PROJECT_BUSY:
					adder = Loc.Instance.GetString ("Project already submitted or has already been sold.");
					break;
				case Warning.MARKET_BUSY:
					adder = Loc.Instance.GetString ("There is already a project at this market.");
					break;
				case Warning.PROJECT_HERE_BEFORE:
					adder = Loc.Instance.GetString ("The project was sent to this market before.");
					break;
				}
				if (adder != Constants.BLANK) {
					WarningText = String.Format ("{0} - {1}", WarningText, adder);
				}
			}
			Warnings.Text = WarningText;
			if (Constants.BLANK == WarningText) {
				Warnings.Text = Loc.Instance.GetString ("No warnings.");
			}


		}


		private void dashBoardsSubmissionsIsUpdatingProject (string ProjectName, string ProjectGUID)
		{

			SelectedName = ProjectName;
			///NewMessage.Show (SelectedName);
			SelectedGUID = ProjectGUID;

			string Messages = SelectedName;//Constants.BLANK;
			if (SelectedGUID == Constants.BLANK) {
				Messages = Loc.Instance.GetString (DefaultProjectLabel);

				ToggleBetweenListAndEditSubmissions.Enabled = false;
			} else {
				ToggleBetweenListAndEditSubmissions.Enabled = true;
		
				MarketEdit.UpdateProjectInformationForFilterBox (SelectedGUID, MasterOfLayouts.GetWordsFromGuid (SelectedGUID));
				//UpdateDoTheyLikeMe(SelectedGUID);
			}

			LabelProject.Text = Loc.Instance.GetStringFmt ("Project: {0}", Messages);
		
		
			UpdateToggleButtonText ();
			// this becomes enabled once we have a valid selection in the list
			ViewOfProjectSubmissions.UpdateDoTheyLikeMe (SelectedGUID);

			// we require running after UpdateDoTheyLikeMe
			if (Constants.BLANK != SelectedGUID) {
				RefreshWarningsAfterMarketAndProjectUpdated ();
			}
		}

		void HandleAddSubmissionClick (object sender, EventArgs e)
		{
			AddSubmissionToProject();
		}
	
		private void AddSubmissionToProject ()
		{
			if (SelectedGUID != Constants.BLANK && SelectedMarketGuid != Constants.BLANK) {
				//string ProjectName = CurrentProject.Text;
//				LayoutDetails.Instance.TransactionsList.AddEvent (new TransactionSubmission
//				                                                 (DateTime.Now, SelectedGUID, SelectedMarketGuid, 0, 0.0f, 0.0f, 
//				 DateTime.Now, "Some Note", "no rights", "first draft", "Invalid", "No feedback", "Submission", SelectedMarket));

			//	string MarketDetails = String.Format("{0} {1} {2}", SelectedMarket, MarketEdit.CurrentMarketType, MarketEdit.CurrentMarketPrint);

				AddEditSubmissionsForm AddForm = new AddEditSubmissionsForm(true);

				int words = MasterOfLayouts.GetWordsFromGuid(SelectedGUID);
				AddForm.SubEditPanel.SubmissionSelected(MarketEdit.SelectedMarketAsObject(), words);
				// Use this only on AN editAddForm.SubEditPanel.LoadFromExisting();
				if (AddForm.ShowDialog() == DialogResult.OK)
				{



					LayoutDetails.Instance.TransactionsList.AddEvent (new TransactionSubmission
                          (AddForm.SubEditPanel.DateSubmitted, 
					 SelectedGUID, 
					 SelectedMarketGuid,
					 AddForm.SubEditPanel.Priority, 
					 AddForm.SubEditPanel.Expenses, 
					 AddForm.SubEditPanel.Earned, 
					 AddForm.SubEditPanel.DateReplied,
					 AddForm.SubEditPanel.Note,
					 AddForm.SubEditPanel.Rights, 
					 AddForm.SubEditPanel.Draft, 
					 AddForm.SubEditPanel.ReplyType, 
					 AddForm.SubEditPanel.ReplyFeedback,
					 AddForm.SubEditPanel.SubmissionTypeType, SelectedMarket));

					ViewOfProjectSubmissions.BuildList();
					ViewOfProjectSubmissions.UpdateDoTheyLikeMe(SelectedGUID);
				}






				//NewMessage.Show (Loc.Instance.GetString ("Submission Added."));
			} else {
				NewMessage.Show (Loc.Instance.GetString ("Either the market or project were not selected, or were invalid."));
			}
		}
		void HandleToggleBetweenListAndEditingClick (object sender, EventArgs e)
		{

			// Clicking button does nothing with no selection (though you can always go back

			
			if (SubmissionPanel.Visible == false) {
				ViewOfProjectSubmissions.Visible = false;
				SubmissionPanel.Visible = true;
				ToggleBetweenListAndEditSubmissions.Tag = 0;

			} else {
				if (SubmissionPanel.GetSelectedGUID != null) {
					SubmissionPanel.Visible = false;
					ToggleBetweenListAndEditSubmissions.Tag = 1;
					// we create the SubmissionPanel
					ViewOfProjectSubmissions.Visible = true;
					ViewOfProjectSubmissions.BuildList ();
				//	AddAndEditSubmissionPanel.SetToNewProject (SubmissionPanel.GetSelectedGUID, SubmissionPanel.GetSelectedNAME);

				}

			}
			UpdateToggleButtonText ();
		}

		protected override void DoChildAppearance (AppearanceClass app)
		{
			base.DoChildAppearance (app);
			Tabs.BackColor = app.mainBackground;
			SubmissionPanel.BackColor = app.mainBackground;
		
		}

		public override void Save ()
		{
			base.Save ();
			CurrentFilter = SubmissionPanel.CurrentFilter;
			//CharacterColorInt = CharacterColor.ToArgb();
		}
		private DataTable CreateDataTable(PropertyInfo[] properties)
			
		{
			DataTable dt = new DataTable();
			dt.TableName = CoreUtilities.Tables.TableWrapper.TablePageTableName;
			DataColumn dc = null;
			foreach(PropertyInfo pi in properties)
			{
				dc = new DataColumn();
				dc.ColumnName = pi.Name;
				dc.DataType = pi.PropertyType;
				dt.Columns.Add(dc); 
			}
			return dt;
		}

		public override void UpdateAfterLoad ()
		{
			base.UpdateAfterLoad ();



			PerformAnyActionToLoadTables();
			MarketEdit.UpdateFilter ();
			// March 2013
			// We cannot do this. There are requirements for CurrentLayout to exist and it does not yet at this stage in the load
			// THis is probably fine since we often will want to swtich to a different view.
		//	SubmissionPanel.RefreshMe();
		}
		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType ()
		{
			
			return Loc.Instance.GetString ("not used in an AddIn");
		}
		public NoteDataXML_Submissions(NoteDataInterface Note) : base(Note)
		{
			
		}
		public override void CopyNote (NoteDataInterface Note)
		{
			base.CopyNote (Note);
		}
	}
}
