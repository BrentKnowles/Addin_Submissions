// dashboardSubs.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using CoreUtilities;
using Submissions;
using Layout;
using Transactions;

namespace MefAddIns
{
	public partial class dashboardSubs : UserControl
	{
		// a way to sto prefresh during setting up the form
		private bool supressRefresh = false;

		public bool SupressRefresh {
			get {
				return supressRefresh;
			}
			set {
				//NewMessage.Show ("Setting to " + value.ToString());
				supressRefresh = value;
			}
		}
		ListViewColumnSorter lvwColumnSorter;
		Action<string, string> UpdateOtherForms = null;
	

		// TODO: eventually load this from the actual Submission Object
		private string currentFilter = Constants.BLANK;

		public string CurrentFilter {
			get {
				return currentFilter;
			}
			set {
				currentFilter = value;
			}
		}

		public string GetSelectedNAME {
			get {
				if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0 && listView1.SelectedItems [0].Text != Constants.BLANK) {
					return listView1.SelectedItems [0].Text;
				}
				return Constants.BLANK;
			}

		}

		public string GetSelectedGUID {
			get { 
				if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0 && listView1.SelectedItems [0].Text != Constants.BLANK) {
					return ((LittleSubmission)listView1.SelectedItems [0].Tag).GUID;
				}

				return Constants.BLANK;
			}

		}

		/// <summary>
		/// called from former Report option
		/// 
		/// </summary>
		public void ShowSubmissions ()
		{
			//checkBoxStatus.Checked = false;
			checkBoxReadyToSend.Checked = false;
			checkBoxSent.Checked = false;
		}
       
	

		public dashboardSubs (Action<string,string> _UpdateOtherForms, bool PreventRefresh)
		{
			SupressRefresh = PreventRefresh;
			UpdateOtherForms = _UpdateOtherForms;
		

			InitializeComponent ();
			lvwColumnSorter = new ListViewColumnSorter ();
			this.listView1.ListViewItemSorter = lvwColumnSorter;
			checkBoxReadyToSend.Checked = false;



			checkBoxSent.Checked = false;

			//NOTE: listviews do not have datasources



		}

		private LittleSubmission AddTransactionRowDetailsToLittleSub(LittleSubmission Sub, Transactions.TransactionSubmission TransactionInfo)
		{
			//DateTime subDate = ((DateTime)(dr [Data.SubmissionIndexFields.SUBMISSIONDATE]));
			DateTime subDate = TransactionInfo.Date1;
			DateTime today = DateTime.Today;
			int nDays = ((TimeSpan)(today - subDate)).Days;
			Sub.Days = nDays;
			string days = nDays.ToString ();
			// pad with zeroes for proper sorting
			while (days.Length < 3) {
				days = "0" + days;
			}
			Sub.Market = days + Loc.Instance.GetStringFmt (" DAYS: {0}", TransactionInfo.MarketName);
			return Sub;
		}

		void AddItemsSubMethod (List<MasterOfLayouts.NameAndGuid> list, bool AddSubmissionInformation)
		{
			ArrayList submissions = new ArrayList ();
			foreach (MasterOfLayouts.NameAndGuid NameG in list) {
				LittleSubmission sub = new LittleSubmission ();
				sub.Story = NameG.Caption;
				sub.GUID = NameG.Guid;
				sub.Market = "-";
				sub.Words = NameG.Words;
				sub = AddDestinationInformation (sub);
				
				
				// grab Current Submission information for those on this filter
				
				if (true == AddSubmissionInformation)
				{
					sub = AddSubmissionInformationToCurrentFilterItem (sub);
				}
				
				submissions.Add (sub);
			}
			//NewMessage.Show("Adding : " + submissions.Count.ToString ());
			AddItemToListView (submissions);
		}

		void AddItemsFromCurrentFilter (bool AddSubmissionInformation)
		{


			List<MasterOfLayouts.NameAndGuid> LayoutsFound = MasterOfLayouts.GetListOfLayouts (CurrentFilter);

			AddItemsSubMethod(LayoutsFound, AddSubmissionInformation);
		}

		private LittleSubmission AddSubmissionInformationToCurrentFilterItem (LittleSubmission CurrentItem)
		{

			                   
			// go through Transaciton Table looking for matches
			                
			List<Transactions.TransactionBase> LayoutEvents = SubmissionMaster.GetListOfSubmissionsForProject (CurrentItem.GUID);
			if (null != LayoutEvents) {
			
				foreach (Transactions.TransactionSubmission subs in LayoutEvents) {
			

					//	filter only those with NO REPLY
					if (SubmissionMaster.ThisSubmissionNotResolved (subs) == true) {
						AddTransactionRowDetailsToLittleSub(CurrentItem, subs);
						// added the break because we only care if we find one unanswered sub, right?? (March 17 2013)
						break;

					}
				}
			}


			return CurrentItem;
			                    


		}

		public void RefreshMe ()
		{
			if (true == SupressRefresh) {
			//	NewMessage.Show ("Skipping refresh");
				return;
			}
		//	NewMessage.Show ("Refreshing! " + SupressRefresh.ToString());
			this.Cursor = Cursors.WaitCursor;
			listView1.BeginUpdate();
			listView1.Items.Clear ();
			;


			// the two checkboxes override The Filter
			if (checkBoxSent.Checked == false && checkBoxReadyToSend.Checked == false) {
				AddItemsFromCurrentFilter(true);
			
			}


			if (checkBoxReadyToSend.Checked)
				Refresh_ReadyToSend ();
			if (checkBoxSent.Checked)
				Refresh_CurrentSubs ();
			//  if (checkBoxStatus.Checked) Refresh_ByStatus();


			lvwColumnSorter.SortColumn = 0;
			lvwColumnSorter.Order = SortOrder.Ascending;
			listView1.Sort ();
			this.Cursor = Cursors.Default;

			// we clear the existing project guids and such
			UpdateOtherForms (Constants.BLANK, Constants.BLANK);
			listView1.EndUpdate ();
			this.columnHeader1.Text = Loc.Instance.GetStringFmt("Story ({0})", listView1.Items.Count);
			//   (Parent as CoreUtilities.RollUp).TextLabel = String.Format("Submissions ({0} found)", listView1.Items.Count);
		}

		private void buttonRefresh_Click (object sender, EventArgs e)
		{
			RefreshMe ();
		}

		private void Refresh_CurrentSubs ()
		{
			ArrayList submissions = new ArrayList ();

			List<Transactions.TransactionBase> AllSubs = SubmissionMaster.GetListOfSubmissionsAll();
			//NewMessage.Show (AllSubs.Count.ToString());
			foreach (Transactions.TransactionSubmission TransactionSub in AllSubs)
			{
				if (SubmissionMaster.ThisSubmissionNotResolved(TransactionSub) == true)
				{
					LittleSubmission sub = new LittleSubmission ();
					sub.Story = MasterOfLayouts.GetNameFromGuid(TransactionSub.ProjectGUID);
					sub.GUID = TransactionSub.ProjectGUID;
					sub.Market = "-";
					sub.Words =MasterOfLayouts.GetWordsFromGuid(TransactionSub.ProjectGUID);
					sub = AddDestinationInformation (sub);
					
					
					// grab Current Submission information for those on this filter
					
					//sub = AddSubmissionInformationToCurrentFilterItem (sub);
					AddTransactionRowDetailsToLittleSub(sub, TransactionSub);
					submissions.Add (sub);
				}
			}
			AddItemToListView (submissions);

			// code taken from fLinkPopup.cs
//            foreach (DataRow dr in Program.AppMainForm.data.SubmissionIndex.Rows)
//            {
//                if (dr != null)
//                {
//
//                    if (dr[Data.SubmissionIndexFields.REPLYTYPE].ToString().ToLower()
//                        == English.Invalid.ToLower() &&
//                        dr[Data.SubmissionIndexFields.SUBMISSIONTYPE].ToString().ToLower()
//                        != classSubmission.DESTINATION_SUBTYPE.ToLower())
//                    {
//                        LittleSubmission sub = new LittleSubmission();
//                        sub.Story = dr[Data.SubmissionIndexFields.PROJECT_NAME].ToString();
//                        sub.GUID = dr[Data.SubmissionIndexFields.PROJECTGUID].ToString();
//                       
//                        sub = AddDestinationInformation(sub);
//
//
//
//                        DateTime subDate = ((DateTime)(dr[Data.SubmissionIndexFields.SUBMISSIONDATE]));
//                        DateTime today = DateTime.Today;
//                        int nDays = ((TimeSpan)(today - subDate)).Days;
//                        sub.Days = nDays;
//                        string days = nDays.ToString();
//                        // pad with zeroes for proper sorting
//                        while (days.Length < 3)
//                        {
//                            days = "0" + days;
//                        }
//                        sub.Market = days+" DAYS: " + dr[Data.SubmissionIndexFields.MARKET_NAME].ToString() ;
//                        submissions.Add(sub);
//
//                    }
//
//
//                   
//
//                }
			//           }
			//AddItemToListView (submissions);
			/*
            foreach (LittleSubmission sub in submissions)
            {


                ListViewItem item = new ListViewItem(sub.Story);
                item.Font = new Font("Courier New",12, FontStyle.Regular);
                item.SubItems.Add(sub.Market);
                item.SubItems.Add(sub.NextMarket);
                if (sub.Days > 120)
                {
                    item.BackColor = Color.Red;
                }
                listView1.Items.Add(item);
            }*/
		}
        

		/// <summary>
		/// takes the little submission object and adds Market (if at a market) and NextMarket (destinatino)
		/// information. 
		/// </summary>
		/// <param name="sub"></param>
		/// <returns></returns>
		private LittleSubmission AddDestinationInformation (LittleSubmission sub)
		{

			try {

				ArrayList destinations = new ArrayList ();

				List<TransactionBase> Destinations = SubmissionMaster.GetListOfDestinationsForProject(sub.GUID);
				foreach (TransactionBase transaction in Destinations)
				{
					LittleDestination destination = new LittleDestination();
					destination.Market = ((TransactionSubmissionDestination)transaction).MarketName;// dr[Data.SubmissionIndexFields.MARKET_NAME].ToString();
					destination.Priority = 0.0f;
					float.TryParse(((TransactionSubmissionDestination)transaction).Priority, out destination.Priority);//(float)dr[Data.SubmissionIndexFields.DESTINATION_PRIORITY];
					destinations.Add(destination);
				}

//                foreach (DataRow dr in Program.AppMainForm.data.SubmissionIndex.Rows)
//                {
//                    if (dr != null)
//                    {
//
//                        // destinations with this GUID
//                        if (dr[Data.SubmissionIndexFields.PROJECTGUID].ToString() == sub.GUID
//				                            && dr[Data.SubmissionIndexFields.SUBMISSIONTYPE].ToString().ToLower() == classSubmission.DESTINATION_SUBTYPE.ToLower())
//                        {
//                            // this matches the project
//                            // now we grab all destinations
//                            LittleDestination destination = new LittleDestination();
//                            destination.Market = dr[Data.SubmissionIndexFields.MARKET_NAME].ToString();
//                            destination.Priority = (float)dr[Data.SubmissionIndexFields.DESTINATION_PRIORITY];
//                            destinations.Add(destination);
//                        }
//
//
//                    }
//
//                }

				sub.NextMarket = "";

				// TO DO : Sort destinations

				destinations.Sort ();
				destinations.Reverse ();
				int count = 1;
				foreach (LittleDestination destination in destinations) {
					sub.NextMarket = sub.NextMarket + "(" + count.ToString () + ")" + destination.Market + " ";
					count++;
				}


			} catch (Exception ex) {
				MessageBox.Show (ex.ToString ());
			}

			/* Removed, handled by NameAndGUID
            // jan 2012
            // adding some info for words
            try
            {
//                classPageProject page = (classPageProject)Data.GoToPage(sub.Story);
//                if (page.IsProject())
//                {
//                    sub.Words = page.Words; //HACK for words? 
//                    // its own column?
//                    //sub.Story = sub.Story + " " + sub.Days.ToString();
//                }
            }
            catch (Exception)
            {
            }
            */
			return sub;
		}

		private void Refresh_ReadyToSend ()
		{

			try {
				ArrayList submissions = new ArrayList ();

				listView1.BeginUpdate();
				//Step 1: Use current Filter and REMOVE those entities that
				//        are currently at Markets

			//	AddItemsFromCurrentFilter(false); *

				List<MasterOfLayouts.NameAndGuid> ItemsToKeep = new List<MasterOfLayouts.NameAndGuid>();
		

				List<MasterOfLayouts.NameAndGuid> LayoutsFound = MasterOfLayouts.GetListOfLayouts (CurrentFilter);


				// THIS IS the SLOW PART.
				// 10 seconds + 

				// really we want the list of Transactions that contain ReplyType=Acceptance or ReplyType=Invalid
				// if this list is greater than 0 than we have a match
				List<string> OutstandingReply = LayoutDetails.Instance.CurrentLayout.GetListOfStringsFromSystemTable (SubmissionMaster.TABLE_ReplyTypes, 1, String.Format ("2|{0}", SubmissionMaster.CODE_NO_REPLY_YET));
				List<string> Acceptances = LayoutDetails.Instance.CurrentLayout.GetListOfStringsFromSystemTable (SubmissionMaster.TABLE_ReplyTypes, 1, String.Format ("2|{0}", SubmissionMaster.CODE_ACCEPTANCE));
				string query = "";
				
				foreach (string s in OutstandingReply) {
					
					if (query != "") query = query + " or ";
					query = query + String.Format (" {1}='{0}' ", s, TransactionsTable.DATA7);
				}
				
				
				foreach (string s in Acceptances) {
					
					if (query != "") query = query + " or ";
					query = query + String.Format (" {1}='{0}' ", s, TransactionsTable.DATA7);
				}
				
				string querywrapper = String.Format ("and {1}='{2}' and ({0})", query, TransactionsTable.TYPE, TransactionsTable.T_SUBMISSION);

			//	foreach (ListViewItem item in listView1.Items)
				foreach (MasterOfLayouts.NameAndGuid item in LayoutsFound)
				{
				//	LittleSubmission Sub = (LittleSubmission) item.Tag;
				//	if (Sub != null)
					{
						if (OutstandingReply == null || OutstandingReply.Count <= 0 || Acceptances == null || Acceptances.Count <=0)
						{
							throw new Exception("Default lists not defined.");
						}




						List<Transactions.TransactionBase> ListOfSubs = SubmissionMaster.GetListOfOutstandingSubmissionsOrAcceptances(item.Guid, querywrapper);

						// we found no Acceptances and no invalid replies which mean we are free to SEND
						if (ListOfSubs.Count == 0)
						{
							ItemsToKeep.Add (item);
						}
						/*

						List<Transactions.TransactionBase> ListOfSubs = SubmissionMaster.GetListOfSubmissionsForProject(item.Guid);
						if (ListOfSubs != null && ListOfSubs.Count > 0)
						{bool FoundAtLeastOneSub = false;
							foreach(Transactions.TransactionBase transaction in ListOfSubs)
							{
								if (transaction is Transactions.TransactionSubmission)
								{

									if (SubmissionMaster.IsValidReply( ((Transactions.TransactionSubmission)transaction).ReplyType) == false)
								{
									//	NewMessage.Show ("Removing " + item.Text);
									// if there are oustanding submissions
									// for this project
									// remove it from the list of Ready To send
									//	ItemsToRemove.Add (item); *
										FoundAtLeastOneSub = true;
								}
									if (SubmissionMaster.IsAcceptance( ((Transactions.TransactionSubmission)transaction).ReplyType) == true)
									{
										FoundAtLeastOneSub = true;
									}


									if (true == FoundAtLeastOneSub)
									{
										break;
									}

							}
							}

							if (false == FoundAtLeastOneSub)
							{
								ItemsToKeep.Add (item);
							}
						}
						*/
					}
				}
//				foreach (ListViewItem DeleteMe in ItemsToRemove)
//				{
//					listView1.Items.Remove (DeleteMe);
//				}


				// I flipped the logic to make this fstaer (march 17 2013)
				//   so: we now add them
				AddItemsSubMethod(ItemsToKeep, false);
				listView1.EndUpdate();
				// STOP: March 2013. Maybe I do not need to make this mor ecomplicated.
				// Can I solve it witht he right query?
				// i.e., notebook='Writing' and status='4 Complete' and section='Projects'

				// Step 2: Come up with Advanced Criteria to UNDERSTAND when a complete market is ready to send out

//                DataView dv = new DataView(Program.AppMainForm.data.pageIndex);
//                Program.AppMainForm.data.CreateTableRelationship();
//                dv.RowFilter = "(Retired=false) AND  (StatusType <>'Rewriting') AND (StatusType <>'rewriting') AND(((Sum(Child.CalcBusy) <= 0) AND Finished = true ) OR ( (IsNull(Sum(Child.CalcBusy),-1) = -1) AND (Finished = true) ) )";
//                foreach (DataRow r in dv.ToTable().Rows)
//                {
//                    string sPageName = r[Data.PageIndexFields.NAME].ToString();
//                    // AddSubmissionPanelLabel(sPageName, sPageName, linkLabel1.LinkColor);
//
//                    LittleSubmission sub = new LittleSubmission();
//                    sub.Story = sPageName;
//                    sub.GUID = r[Data.PageIndexFields.GUID].ToString();
//                    sub.Market = "-";
//                    sub = AddDestinationInformation(sub);
//
//                    submissions.Add(sub);
//
//                }
//                dv = null;
			//	AddItemToListView (submissions);
				/*
                foreach (LittleSubmission sub in submissions)
                {


                    ListViewItem item = new ListViewItem(sub.Story);
                    item.Font = new Font("Courier New", 12, FontStyle.Bold);
                    item.SubItems.Add(sub.Market);
                    item.SubItems.Add(sub.NextMarket);
                    listView1.Items.Add(item);
                }*/
			} catch (Exception ex) {
				MessageBox.Show (ex.ToString ());
			}
		}

		private void checkBoxReadyToSend_CheckedChanged (object sender, EventArgs e)
		{
			RefreshMe ();
		}

		private void checkBoxSent_CheckedChanged (object sender, EventArgs e)
		{
			RefreshMe ();
		}

		/// <summary>
		///  double click and go to that page in a new window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_MouseDoubleClick (object sender, MouseEventArgs e)
		{
			if (listView1.SelectedItems != null) {
				//     Program.AppMainForm.OpenNewWindow( listView1.SelectedItems[0].Text, 0, false);
				if (listView1.SelectedItems[0].Tag != null)
				{
					LittleSubmission sub = (LittleSubmission)listView1.SelectedItems[0].Tag;
					LayoutDetails.Instance.LoadLayout(sub.GUID);
				}
				//	NewMessage.Show (listView1.SelectedItems[0].Text);
					//LayoutDetails.Instance.LoadLayout(


			}
		}

		private void listView1_SelectedIndexChanged (object sender, EventArgs e)
		{
			if ((sender as ListView).SelectedItems != null && (sender as ListView).SelectedItems.Count > 0) {
				if (UpdateOtherForms != null) {
					LittleSubmission sub = (LittleSubmission)(sender as ListView).SelectedItems [0].Tag;
					UpdateOtherForms ((sender as ListView).SelectedItems [0].Text, sub.GUID);
				}
			}
		}

		private void comboBoxStatus_DropDown (object sender, EventArgs e)
		{
			// draw list of status
			//   comboBoxStatus.Items.Clear();
//            ClassSubtypes subs = new ClassSubtypes();
//            subs = ClassSubtypes.LoadUserInfo("statustypes");
//            comboBoxStatus.Items.AddRange(subs.getSubtypes());

		}

		/// <summary>
		/// can only use the dropdown if this is checked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBoxStatus_CheckedChanged (object sender, EventArgs e)
		{
            

			// disable the other two checks because they are no longer relevant.
//            if (true == checkBoxStatus.Checked)
//            {
//                checkBoxReadyToSend.Checked = false;
//                checkBoxSent.Checked = false;
//            }
//
//            comboBoxStatus.Enabled = checkBoxStatus.Checked;
//            checkBoxReadyToSend.Enabled = !checkBoxStatus.Checked;
//            checkBoxSent.Enabled = !checkBoxStatus.Checked;
		}

		private void Refresh_ByStatus ()
		{
			// now redraw the list according to the status set
			try {
				ArrayList submissions = new ArrayList ();

//                DataView dv = new DataView(Program.AppMainForm.data.pageIndex);
//                Program.AppMainForm.data.CreateTableRelationship();
//
//                string sValue = "";
//                try
//                {
//                    sValue = comboBoxStatus.Items[comboBoxStatus.SelectedIndex].ToString();
//                }
//                catch
//                {
//                    // 
//                    sValue = "";
//                }
//
//
//                dv.RowFilter = String.Format("((StatusType ='{0}') or (StatusType ='{1}'))", sValue,
//                    sValue.ToLower());
//                foreach (DataRow r in dv.ToTable().Rows)
//                {
//                    string sPageName = r[Data.PageIndexFields.NAME].ToString();
//                    // AddSubmissionPanelLabel(sPageName, sPageName, linkLabel1.LinkColor);
//
//                    LittleSubmission sub = new LittleSubmission();
//                    sub.Story = sPageName;
//                    sub.GUID = r[Data.PageIndexFields.GUID].ToString();
//                    sub.Market = "-";
//           
//                    
//                    
//                    sub = AddDestinationInformation(sub);
//
//                    submissions.Add(sub);
//
//                }
//                dv = null;
				AddItemToListView (submissions);
               
			} catch (Exception ex) {
				MessageBox.Show (ex.ToString ());
			}
		}

		/// <summary>
		/// wrapper to add the array to the items
		/// </summary>
		/// <param name="subs"></param>
		private void AddItemToListView (ArrayList subs)
		{
			foreach (LittleSubmission sub in subs) {


				ListViewItem item = new ListViewItem (sub.Story);
				item.Tag = sub;
				item.Font = new Font ("Courier New", 12, FontStyle.Bold);
				item.SubItems.Add (sub.Words.ToString ());
				item.SubItems.Add (sub.Market);
				item.SubItems.Add (sub.NextMarket);
				if (sub.Days > 120) {
					item.BackColor = Color.Red;
				}
				//  item.ToolTipText = "boo!";
				listView1.Items.Add (item);
			}
		}

		private void comboBoxStatus_SelectedIndexChanged (object sender, EventArgs e)
		{
			RefreshMe (); 
           
		}

		/// <summary>
		/// override tooltip behavior to show current item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_MouseMove (object sender, MouseEventArgs e)
		{/*
            ListViewItem item = listView1.GetItemAt(e.X, e.Y);
            //item.ToolTipText = item.Text;
            

            if (item != null)
            {
                //we care about destination infor here
                string text = item.SubItems[3].Text;
                if (text != "" /*&& this.toolTip1.GetToolTip(listView1) != text)
                {
                    toolTip1.SetToolTip(listView1, item.Text);
                    //toolTip1.Active = true;
                    //toolTip1.Show(text, listView1);
                    //this.toolTip1.SetToolTip(listView1, text);
                    //this.toolTip1.Show(text);
                }
                else
                {
                    this.toolTip1.RemoveAll();
                }
            }
            else
            {
                this.toolTip1.RemoveAll();
            }
          */
		}
	
		/// <summary>
		/// reorder
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_ColumnClick (object sender, ColumnClickEventArgs e)
		{
			lvwColumnSorter.SortColumn = e.Column;
			if (lvwColumnSorter.Order == SortOrder.Ascending) {
				lvwColumnSorter.Order = SortOrder.Descending;
			} else {
				lvwColumnSorter.Order = SortOrder.Ascending;
			}
			listView1.Sort ();
		}
	}

    



}
