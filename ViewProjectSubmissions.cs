// ViewProjectSubmissions.cs
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

		string Project_Name {
			get { return MasterOfLayouts.GetNameFromGuid(GetProjectGUID());}
		
		}

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
		ToolStripButton GenerateCoverLetter=null;
		#endregion

		public void SetLabel (string main, string update)
		{
		
			Overall.Text = main;
			Overall.Tag = update;
		}
		LayoutPanelBase myLayout = null;
		public ViewProjectSubmissions(Func<string> _GetProjectGUID, getmarketbyguiddelegate _GetMarketByGUID, LayoutPanelBase _Layout)
		{
		
			myLayout = _Layout;
			GetProjectGUID = _GetProjectGUID;
			GetMarketByGUID = _GetMarketByGUID;

			Label Title = new Label();
			Title.Text= "List of Submissions";
			Title.Dock = DockStyle.Top;


			 Overall = new Label();

			Overall.MouseEnter+= HandleMouseEnter;
			Overall.MouseLeave+= HandleMouseLeave;
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

		void HandleMouseLeave (object sender, EventArgs e)
		{
			(sender as Label).Text = textmemory;
		}

		void HandleMouseEnter (object sender, EventArgs e)
		{
			if ((sender as Label).Tag != null) {
				textmemory = (sender as Label).Text;
				(sender as Label).Text = (sender as Label).Tag.ToString (); 
			}
		}
		string textmemory ="";


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

		/// <summary>
		/// Generate_s the cover letter.
		/// 
		/// Looks for a text note named "cover letter"
		/// 
		/// Fills in the appropriate details
		/// MARKET details
		/// [MarketName]
		//		[Address]
		//		[City], [Province]
		//		[PostalCode]
		//		[Country]
		//      [Editor]
		//
		// PROJECT DETAILS
		// [Page]
		// [Words]
		/// </summary>
		void Generate_CoverLetter ()
		{
			if (ListOfSubs.SelectedItem != null && ProjectGUID != Constants.BLANK && LayoutDetails.Instance.CurrentLayout != null) {
				// find the cover to use and get text from it

				string covernote = Constants.BLANK;
				NoteDataXML_RichText TextNote = (NoteDataXML_RichText)LayoutDetails.Instance.CurrentLayout.FindNoteByName("Cover Letter");
				if (TextNote != null)
				{
				covernote = TextNote.GetRichTextBox().Rtf;


				TransactionSubmission Submission = (TransactionSubmission)ListOfSubs.SelectedItem;
				string marketguid = Submission.MarketGuid;
	
				Market market = GetMarketByGUID(marketguid);
				if (market != null)
				{
				MasterOfLayouts.NameAndGuid project = new MasterOfLayouts.NameAndGuid ();
				project.Guid = ProjectGUID;
					project.Caption = MasterOfLayouts.GetNameFromGuid(ProjectGUID);
				project.Words = MasterOfLayouts.GetWordsFromGuid(ProjectGUID);



				if (market != null) {
					try {
						// load it into a temporary richtext that we destroy
						// copy it and paste it into the rich edit?
								string sOutput = TextUtils.RegExternalStr_ObjectLookup (covernote,  market,project, null);
						// sOutput = DocGen.RegExternalStr_ObjectLookup(sOutput, market);
						//sOutput = DocGen.RegExternalStr_ObjectLookup(sOutput, info);


						try {
							//	richText.Rtf = temp.Rtf;
									GenericTextForm Report = LayoutDetails.Instance.GetTextFormToUse();
									Report.GetRichTextBox().Rtf = sOutput;
									Report.ShowDialog();
							/*temp.SelectAll();
                                temp.Copy();
                                richText.ReadOnly = false;
                                richText.Focus();
                                // richText.Rtf = richText.Rtf + "\n";
                                richText.Paste();*/
						} catch (Exception ex) {
							NewMessage.Show (ex.ToString ());
						}
					} catch (Exception ex) {
							NewMessage.Show (ex.ToString());
						// if user info has not had valid data entered for USERINFO then
						// we can't output a letter
						//richText.Text = Loc.Instance.GetString ("error");
					}
				} else {
					lg.Instance.Line ("ViewProjectSubmissions->GenerateCoverLetter", ProblemType.WARNING, "error generating cover letter");
				}
					}	// market not null
				}
				else
				{
					NewMessage.Show (Loc.Instance.GetString ("You cannot generate a cover letter unless you have a text note named Cover Letter on this layout."));
				}
			} 

		}

		void HandleGenerateCoverLetterClick (object sender, EventArgs e)
		{
			Generate_CoverLetter();
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

				lg.Instance.Line ("ViewProjectSubmissions->UpdateListBox", ProblemType.MESSAGE,"Transaction list for this note was empty. Remove me after debugging.");
			}
		}


		private void ChangeToDestination ()
		{
			if (ListOfSubs.SelectedItem != null) {
				SubmissionMaster.ChangeToDestination ( (TransactionSubmission)ListOfSubs.SelectedItem);
				BuildList();
			}


			
		}
		// this is set during UpdateDoTheyLikeme
		public bool Available = true;

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
			// June 2013 - select nothing to avoid confusion about what *should* be selected
			// after an add
			ListOfSubs.SelectedIndex = -1;
			Destinations.SelectedIndex = -1;
		}


		
		/// <summary>
		/// Updates the string that tells the user how well their story is doing
		/// 
		/// For market pages it says how favorably inclined the market is to us
		/// </summary>
		public void UpdateDoTheyLikeMe(string guid)
		{
			Available = true;
			// count number of personal
			try
			{
				if (guid != Constants.BLANK)
				{
				List<Transactions.TransactionBase> LayoutEvents = SubmissionMaster.GetListOfSubmissionsForProject(guid);
				
				// when we grab the list we do some math for the Warnings Label on the main form
				foreach (Transactions.TransactionBase submission in LayoutEvents)
				{
					if (SubmissionMaster.ThisSubmissionNotResolved((Transactions.TransactionSubmission)submission) == true)
					{
						Available = false;
						break;
					}
				}
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
							                                                                  TransactionsTable.DATA1_LAYOUTGUID, guid, TransactionsTable.DATA8, s, TransactionsTable.TYPE,  TransactionSubmission.T_SUBMISSION));
					//int nEncouragingCount = (int)OurTable.Compute(String.Format("COUNT({0})", Data.SubmissionIndexFields.REPLYFEEDBACK), sFilter);
					
					
					//	Think it fis using destinations too
					nHistory[i] = nEncouragingCount;
					
					
					nTotalPoints += (nEncouragingCount*nPoints[i]);
				}
				
				// count sales and add
							//	string sFilter2 = String.Format("{0} = '{1}'", Data.SubmissionIndexFields.REPLYTYPE, classSubmission.DEFAULT_ACCEPTANCE);
								int nCount = 0;

					if (ProjectGUID != Constants.BLANK)
					{
								nCount = SubmissionMaster.CountAcceptances(myLayout, ProjectGUID);
					}
							//	nCount = (int)OurTable.Compute(String.Format("COUNT({0})", Data.SubmissionIndexFields.REPLYFEEDBACK), sFilter2);
				//				
								nTotalPoints += (nCount*10);
								
								string sLabel = "neutral";
								if (nTotalPoints <= 0) sLabel = "neutral";
								
								else if (nTotalPoints < 3) sLabel = "personal";
								else if (nTotalPoints <= 5) sLabel = "encouraging";
								else if (nTotalPoints <= 10) sLabel = "strong";
								else if (nTotalPoints > 10) sLabel = "exceptional";
				//				
				string HistoryOfFeedback = String.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}", sList[0], nHistory[0],
				                                         sList[1], nHistory[1], sList[2], nHistory[2], sList[3], nHistory[3]);
				
			
				//NewMessage.Show (HistoryOfFeedback);
				sList = null; nPoints = null;
				
				// March 2009
				// Also figuring out how much money was made/lost on this submission
								float fEarned = 0;
								float fSpent = 0;



					if (Constants.BLANK != ProjectGUID)
					{
						string SumFilter = String.Format (" {0}='{1}' and {2}='{3}'", TransactionsTable.DATA1_LAYOUTGUID, ProjectGUID
						                                  ,TransactionsTable.TYPE,  TransactionSubmission.T_SUBMISSION);
						fEarned = LayoutDetails.Instance.TransactionsList.Sum(ProjectGUID, TransactionsTable.MONEY2, SumFilter);
						fSpent = LayoutDetails.Instance.TransactionsList.Sum(ProjectGUID, TransactionsTable.MONEY1, SumFilter);
					}
				//				try
				//				{
				//					fSpent = (float)OurTable.Compute(String.Format("SUM({0})", Data.SubmissionIndexFields.EXPENSES), "");
				//					fEarned = (float)OurTable.Compute(String.Format("SUM({0})", Data.SubmissionIndexFields.AMOUNTOFSALE), "");
				//				}
				//				catch (Exception)
				//				{
				//					// we ignore this exception because you might get NULL rows
				//				}
								fEarned = (float)Math.Round( (double)(fEarned - fSpent), 2);
				//				
				//				
				//				
				//				
				//		
				//				{
					string main	 = String.Format("Overall {2} has had {0} responses from potential markets and has earned ${1}", sLabel, fEarned.ToString(), Project_Name.ToUpper ());
				//				}
					SetLabel(main, HistoryOfFeedback);
				}
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
