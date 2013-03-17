using System;
using CoreUtilities;
using System.Windows.Forms;
using System.Drawing;
using CoreUtilities.Links;
using System.ComponentModel;
using Layout;
using System.Collections.Generic;
using System.Xml.Serialization;
using Transactions;
using Submissions;

namespace MefAddIns
{
	public class ViewProjectSubmissions : Panel
	{
		#region delegates
	//	Label CurrentProject = null; Show Project AND Current Market on Header with button "Submitt this Project to this Market" with
		// a label warning if already submitted. The FILTER panel would be under the market list which would be using a View
		Func<string> GetProjectGUID = null;
		getmarketbyguiddelegate GetMarketByGUID=null;

		public string ProjectGUID {
			get { return GetProjectGUID();}
		}
		public delegate Market getmarketbyguiddelegate(string guid);
		#endregion
		#region gui
		ListBox ListOfSubs = null;
		ListBox  Destinations = null;
		Label Overall = null;
		ToolStripLabel  Count = null;
		ToolStripButton MakeSubmission = null;
		ToolStripButton MakeDestination = null;
		ToolStripButton DeleteSelected = null;
		ToolStripButton GenerateCoverLetter = null;
		#endregion

		public void SetLabel (string main, string update)
		{
			// TODO: main will eventually be the main message
			// TODO: update on the mouse over

			Overall.Text = update;
		}

		public ViewProjectSubmissions(Func<string> _GetProjectGUID, getmarketbyguiddelegate _GetMarketByGUID)
		{

			GetProjectGUID = _GetProjectGUID;
			GetMarketByGUID = _GetMarketByGUID;

			Label Title = new Label();
			Title.Text= "List of Submissions";
			Title.Dock = DockStyle.Top;

			 Overall = new Label();
			Overall.Dock = DockStyle.Top;
			Overall.Text = Loc.Instance.GetString ("OVERALL DETAILS TBD");

			ToolStrip Buttons = new ToolStrip();

			Count = new ToolStripLabel();
			Count.Text = "0";

			 DeleteSelected = new ToolStripButton();
			DeleteSelected.Text = Loc.Instance.GetString ("Delete Submission");
			DeleteSelected.Click+= HandleDeleteSubmissionClick;

			 GenerateCoverLetter = new ToolStripButton();
			GenerateCoverLetter.Text = Loc.Instance.GetString ("Cover Letter");
			GenerateCoverLetter.Click+= HandleGenerateCoverLetterClick;

			 MakeDestination = new ToolStripButton();
			MakeDestination.Text = Loc.Instance.GetString ("Change Submission to Destination");
			MakeDestination.Click+= HandleMakeDestinationsClick;
			MakeDestination.Enabled = false;


			 MakeSubmission = new ToolStripButton();
			MakeSubmission.Text = Loc.Instance.GetString ("Change Destination to Submission");
			MakeSubmission.Click+= HandleChangeDestinationToSubmissionClick;;
			MakeSubmission.Enabled = false;

			Buttons.Items.Add (Count);
			Buttons.Items.Add (DeleteSelected);
			Buttons.Items.Add (GenerateCoverLetter);
			Buttons.Items.Add (MakeDestination);
			Buttons.Items.Add (MakeSubmission);

			 ListOfSubs = new ListBox();
			ListOfSubs.DoubleClick+= HandleListOfSubsDoubleClick;
			ListOfSubs.Click+= HandleListOfSubmissionsClick;
			ListOfSubs.Dock = DockStyle.Fill;
			ListOfSubs.Height = 300;

			Label DestinationsTitle = new Label();
			DestinationsTitle.Text = Loc.Instance.GetString ("Destinations");
			DestinationsTitle.Dock = DockStyle.Bottom;

			Destinations = new ListBox();
			Destinations.Dock = DockStyle.Bottom;
			Destinations.Click+= HandleDestinationsClick;

			this.Controls.Add (DestinationsTitle);
			this.Controls.Add (Destinations);

			this.Controls.Add (ListOfSubs);
			ListOfSubs.BringToFront();
			this.Controls.Add (Buttons);

			this.Controls.Add (Title);
			this.Controls.Add (Overall);
			//just add the transaction objects like workflow
			//BuildList ();


		}

		void HandleDestinationsClick (object sender, EventArgs e)
		{
			MakeDestination.Enabled = false;
			GenerateCoverLetter.Enabled=false;
			DeleteSelected.Enabled=false;
			MakeSubmission.Enabled = true;
			UpdateDoTheyLikeMe(ProjectGUID);
		}

		void HandleListOfSubmissionsClick (object sender, EventArgs e)
		{
			MakeDestination.Enabled = true;
			GenerateCoverLetter.Enabled=true;
			DeleteSelected.Enabled=true;
			MakeSubmission.Enabled = false;
			UpdateDoTheyLikeMe(ProjectGUID);
		}

		void ChangeToSubmission ()
		{
			
			if (Destinations.SelectedItem != null) {
				SubmissionMaster.ChangeToSubmission ( (TransactionSubmissionDestination)Destinations.SelectedItem);
				BuildList();
			}

		}

		void HandleChangeDestinationToSubmissionClick (object sender, EventArgs e)
		{
			ChangeToSubmission();
		}

		void HandleMakeDestinationsClick (object sender, EventArgs e)
		{
			ChangeToDestination();
		}

		void HandleGenerateCoverLetterClick (object sender, EventArgs e)
		{
			NewMessage.Show ("move repalcement text system over");
		}

		void HandleDeleteSubmissionClick (object sender, EventArgs e)
		{
			if (ListOfSubs.SelectedItem != null) {
				if (NewMessage.Show (Loc.Instance.GetString ("Delete This Submission?"), 
				                     Loc.Instance.GetString ("Do you really want to permanently delete this submission?"),
				                     MessageBoxButtons.YesNo, null) == DialogResult.Yes) {
					Transactions.TransactionSubmission SubmisisonTransaction = (Transactions.TransactionSubmission) ListOfSubs.SelectedItem;
					if (SubmisisonTransaction != null)
					{
						LayoutDetails.Instance.TransactionsList.DeleteEvent(Transactions.TransactionsTable.ID, SubmisisonTransaction.ID);
					BuildList();
					}
				}
			}
		}

		void HandleListOfSubsDoubleClick (object sender, EventArgs e)
		{
			if (ListOfSubs.SelectedItem != null && GetMarketByGUID != null) {
				TransactionSubmission Submission = (TransactionSubmission)ListOfSubs.SelectedItem;
				string SelectedGUID = Submission.ProjectGUID;
				string SelectedMarketGuid = Submission.MarketGuid;
			
				Market market = GetMarketByGUID(SelectedMarketGuid);
				if (null != market) {


					// We open the AddEdit Submission Panel
					if (SelectedGUID != Constants.BLANK && SelectedMarketGuid != Constants.BLANK) {
						//string ProjectName = CurrentProject.Text;
						//				LayoutDetails.Instance.TransactionsList.AddEvent (new TransactionSubmission
						//				                                                 (DateTime.Now, SelectedGUID, SelectedMarketGuid, 0, 0.0f, 0.0f, 
						//				 DateTime.Now, "Some Note", "no rights", "first draft", "Invalid", "No feedback", "Submission", SelectedMarket));
				
						//	string MarketDetails = String.Format("{0} {1} {2}", SelectedMarket, MarketEdit.CurrentMarketType, MarketEdit.CurrentMarketPrint);
				
						AddEditSubmissionsForm AddForm = new AddEditSubmissionsForm (true);
				
						int words = MasterOfLayouts.GetWordsFromGuid (SelectedGUID);
					
						 //Use this only on AN edit
						AddForm.SubEditPanel.LoadFromExisting(Submission);
						AddForm.SubEditPanel.SubmissionSelected (market, words);
						if (AddForm.ShowDialog () == DialogResult.OK) {
					
							TransactionSubmission EditedSub = new TransactionSubmission(
								AddForm.SubEditPanel.DateSubmitted, 
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
								AddForm.SubEditPanel.SubmissionTypeType, Submission.MarketName);

							EditedSub.SetID(Submission.ID);
							LayoutDetails.Instance.TransactionsList.UpdateEvent(EditedSub);

							// need to rebuild the list otherwise
							// we end up wit hthe old transactions
							BuildList ();
							UpdateDoTheyLikeMe(ProjectGUID);
//						LayoutDetails.Instance.TransactionsList.AddEvent (new TransactionSubmission
//					                                                  (AddForm.SubEditPanel.DateSubmitted, 
//					 SelectedGUID, 
//					 SelectedMarketGuid,
//					 AddForm.SubEditPanel.Priority, 
//					 AddForm.SubEditPanel.Expenses, 
//					 AddForm.SubEditPanel.Earned, 
//					 AddForm.SubEditPanel.DateReplied,
//					 AddForm.SubEditPanel.Note,
//					 AddForm.SubEditPanel.Rights, 
//					 AddForm.SubEditPanel.Draft, 
//					 AddForm.SubEditPanel.ReplyType, 
//					 AddForm.SubEditPanel.ReplyFeedback,
//					 AddForm.SubEditPanel.SubmissionTypeType, SelectedMarket));
						}
				
				
				
				
				
				
						//NewMessage.Show (Loc.Instance.GetString ("Submission Added."));
					} else {
						NewMessage.Show (Loc.Instance.GetString ("Either the market or project were not selected, or were invalid."));
					}
				} else {
					NewMessage.Show (Loc.Instance.GetStringFmt("Was unable to find the market with the GUID {0}", SelectedMarketGuid));
				}
			}
		}

		private void UpdateListBox(List<TransactionBase> Submissions, ListBox ListBoxToUpdate)
		{
			ListBoxToUpdate.DataSource = null;
			//LayoutDetails.Instance.TransactionsList.GetEventsForLayoutGuid (ProjectGUID,String.Format (" and type='{0}' ", TransactionsTable.T_SUBMISSION));
			if (Submissions != null)
			{
				Submissions.Sort ();
				Submissions.Reverse ();
				if (Submissions != null) {
					ListBoxToUpdate.DataSource = Submissions;
					ListBoxToUpdate.DisplayMember = "Display";
					ListBoxToUpdate.ValueMember = "ID";
				}
			}
			else
			{
				//TODO: Remove this message
				NewMessage.Show ("Transaction list for this note was empty. Remove me after debugging.");
			}
		}


		private void ChangeToDestination ()
		{
			if (ListOfSubs.SelectedItem != null) {
				SubmissionMaster.ChangeToDestination ( (TransactionSubmission)ListOfSubs.SelectedItem);
				BuildList();
			}


			
		}

		public void BuildList ()
		{
			if (null == LayoutDetails.Instance.TransactionsList) {
				throw new Exception("Transaction Table not created yet");
			}
			try {
				if (Constants.BLANK != ProjectGUID)
				{
				
					List<Transactions.TransactionBase> LayoutEvents = SubmissionMaster.GetListOfSubmissionsForProject(ProjectGUID);
					UpdateListBox (LayoutEvents, ListOfSubs);
					Count.Text = Loc.Instance.GetStringFmt("Submissions: {0}", ListOfSubs.Items.Count.ToString ());
					LayoutEvents = null;
					LayoutEvents = SubmissionMaster.GetListOfDestinationsForProject(ProjectGUID);
					UpdateListBox (LayoutEvents,Destinations);
				
				}
			} catch (Exception ex) {
				NewMessage.Show (ex.ToString());
			}
		}


		
		/// <summary>
		/// Updates the string that tells the user how well their story is doing
		/// 
		/// For market pages it says how favorably inclined the market is to us
		/// </summary>
		public void UpdateDoTheyLikeMe(string guid)
		{
			// count number of personal
			try
			{
				//	DataTable OurTable = SubmissionDataView.ToTable();
				//	if (null == OurTable) return; // trying to get rid of Index error Sep 26 2012
				
				int nTotalPoints = 0;
				string[] sList = new string[4] { "Form", "Encouraging", "Personal", "Almost" };
				int[] nHistory = new int[4] { 0, 0, 0, 0 }; // keep track of what was said
				
				int[] nPoints = new int[4]{-1, 3, 1, 5};
				
				for (int i = 0; i < 4; i++)
				{
					string s = sList[i];
					//string sFilter = String.Format("{0} = '{1}'", Data.SubmissionIndexFields.REPLYFEEDBACK, s);
					int nEncouragingCount = 
						LayoutDetails.Instance.TransactionsList.CountQuery(String.Format ("select Count({0}) from {1} where {2}='{3}' and {4}='{5}' and {6}='{7}'", TransactionsTable.DATA8, TransactionsTable.table_name,
						                                                                  TransactionsTable.DATA1_LAYOUTGUID, guid, TransactionsTable.DATA8, s, TransactionsTable.TYPE, TransactionsTable.T_SUBMISSION));
					//int nEncouragingCount = (int)OurTable.Compute(String.Format("COUNT({0})", Data.SubmissionIndexFields.REPLYFEEDBACK), sFilter);
					
					
					//	Think it fis using destinations too
					nHistory[i] = nEncouragingCount;
					
					
					nTotalPoints += (nEncouragingCount*nPoints[i]);
				}
				
				// count sales and add
				//				string sFilter2 = String.Format("{0} = '{1}'", Data.SubmissionIndexFields.REPLYTYPE, classSubmission.DEFAULT_ACCEPTANCE);
				//				int nCount = 0;
				//				nCount = (int)OurTable.Compute(String.Format("COUNT({0})", Data.SubmissionIndexFields.REPLYFEEDBACK), sFilter2);
				//				
				//				nTotalPoints += (nCount*10);
				//				
				//				string sLabel = "neutral";
				//				if (nTotalPoints <= 0) sLabel = "neutral";
				//				
				//				else if (nTotalPoints < 3) sLabel = "personal";
				//				else if (nTotalPoints <= 5) sLabel = "encouraging";
				//				else if (nTotalPoints <= 10) sLabel = "strong";
				//				else if (nTotalPoints > 10) sLabel = "exceptional";
				//				
				string HistoryOfFeedback = String.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}", sList[0], nHistory[0],
				                                         sList[1], nHistory[1], sList[2], nHistory[2], sList[3], nHistory[3]);
				
				SetLabel("Main", HistoryOfFeedback);
				//NewMessage.Show (HistoryOfFeedback);
				sList = null; nPoints = null;
				
				// March 2009
				// Also figuring out how much money was made/lost on this submission
				//				float fEarned = 0;
				//				float fSpent = 0;
				//				try
				//				{
				//					fSpent = (float)OurTable.Compute(String.Format("SUM({0})", Data.SubmissionIndexFields.EXPENSES), "");
				//					fEarned = (float)OurTable.Compute(String.Format("SUM({0})", Data.SubmissionIndexFields.AMOUNTOFSALE), "");
				//				}
				//				catch (Exception)
				//				{
				//					// we ignore this exception because you might get NULL rows
				//				}
				//				fEarned = (float)Math.Round( (double)(fEarned - fSpent), 2);
				//				
				//				
				//				
				//				
				//		
				//				{
				//					labelhowellisitdoing.Text = String.Format("Overall this submission has had {0} responses from potential markets and has earned ${1}", sLabel, fEarned.ToString());
				//				}
			}
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString());
			}
		}
//		public void SetToNewProject(string GetSelectedGUID, string GetSelectedNAME)
//		{
//			CurrentProject.Text = GetSelectedNAME;
//			ProjectGUID = GetSelectedGUID;
//
//		}
	}

}
