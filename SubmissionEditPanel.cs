// SubmissionEditPanel.cs
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
using System.IO;
using CoreUtilities;
using Layout;

namespace Submissions
{
    /// <summary>
    /// Features
    ///  - able to hide the "reply part" if a destination
    ///  - able to "close" when no submission selected (displays a pretty panel)
    /// </summary>
    public partial class SubmissionEditPanel : UserControl
    {
		public int Priority {

			get { 
				int output = 0;
				Int32.TryParse(ePriority.Text, out output);
				return output; 

			}
			set{ ;}
		}

			public string SubmissionTypeType {
			get{ return SubmissionType.Text;}
			set{ ;}
		}

		public string ReplyFeedback {
			get{ return feedback.Text;}
			set{ ;}
		}

		public string ReplyType {
			get { return replytype.Text;}
			set{ ;}
		}

		public string Draft {
			get { return textBoxDraft.Text;}
			set{ ;}
		}

		public string Rights {
			get{ return rights.Text;}
			set{ ;}
		}

		public string Note {
			get { return notes.Text;}
			set{ ;}
		}

		public DateTime DateReplied {
			get { return dateReply.Value;}
			set { ;}
		}

		public DateTime DateSubmitted {
			get { return dateSubmission.Value;}
			set { ;}
		}

		public float Earned {
			get{float output = 0.0f;
				output = (float)this.earned.Value;
				return output;
			}
			set{ ;}
		}

		public float Expenses {
			get {
				float output = 0.0f;
				output = (float)this.postage.Value;
				//float.TryParse (this.postage.Value, out output);
				return output;
			}
			set{ ;}
		}

        public SubmissionEditPanel ( bool ProvideDefaults)
		{
			InitializeComponent ();


			try
			{
				_Loading = true;
				//  labelMarket.Text = English.nomarket; // must be set to this so the add market thing owrks (the market picker)
				SubmissionType.Items.Clear();
				replytype.Items.Clear();
				
				
				List<string> result = LayoutDetails.Instance.CurrentLayout.GetListOfStringsFromSystemTable (SubmissionMaster.TABLE_SubmissionTypes, 1);
				SubmissionType.Items.AddRange(result.ToArray());
				
				
				result = LayoutDetails.Instance.CurrentLayout.GetListOfStringsFromSystemTable (SubmissionMaster.TABLE_ReplyTypes, 1);
				replytype.Items.AddRange(result.ToArray());
				
				
				// reply feedback
				result = LayoutDetails.Instance.CurrentLayout.GetListOfStringsFromSystemTable (SubmissionMaster.TABLE_ReplyFeedback, 1);
				feedback.Items.AddRange(result.ToArray());
				
				
				//      subs = null;
				
				
			}
			catch (Exception)
			{
			}
			_Loading = false;

			//labelMarket.Text = MarketNameAndDetails; 
			if (true == ProvideDefaults) {
				this.dateSubmission.Value = DateTime.Now;
				this.SubmissionType.Text = SubmissionMaster.GetDefaultSubmission ();
				this.textBoxDraft.Text = Constants.BLANK;
				this.postage.Text = "0.0";
				this.ePriority.Text = "1";
				this.textBoxSale.Text = "0.0";
				this.dateReply.Value = DateTime.Now.AddDays (+7);
				this.replytype.Text = SubmissionMaster.GetDefaultReplyType();
				this.feedback.Text = SubmissionMaster.GetDefaultReplyTypeFeedback();
				this.rights.Text = Constants.BLANK;
			}
        }

        public bool ShowPriority
        {
            get { return ePriority.Visible; }
            set { ePriority.Visible = value; prioritylabel.Visible = value;  }
        }

        /// <summary>
        /// performs a pretty wayto empty the submission panel
        /// </summary>
        public void NoSubmissionSelected()
        {
            panel1.Visible = false;
            //submissionObject = null;
			labelMarket.Text = Loc.Instance.GetString ("No Market"); //English.nomarket; // very important
        }

        private string sGUID = "";

        
   public int WordCount; // set when submission changes, defaults to 0. (Feb 2010)

		// will
		public void LoadFromExisting (Transactions.TransactionSubmission Sub)
			{
		//	Priority = Sub.Priority;

//			AddForm.SubEditPanel.Expenses, 
//			AddForm.SubEditPanel.Earned, 
//			AddForm.SubEditPanel.DateReplied,
//			AddForm.SubEditPanel.Note,
//			AddForm.SubEditPanel.Rights, 
//			AddForm.SubEditPanel.Draft, 
//			AddForm.SubEditPanel.ReplyType, 
//			AddForm.SubEditPanel.ReplyFeedback,
//			AddForm.SubEditPanel.SubmissionTypeType
		//	NewMessage.Show ("Load defaults");
			dateSubmission.Value = Sub.SubmissionDate;//(DateTime)row[Data.SubmissionIndexFields.SUBMISSIONDATE];
			SubmissionType.Text = Sub.SubmissionType;//row[Data.SubmissionIndexFields.SUBMISSIONTYPE].ToString();
			postage.Value = Sub.Expenses;
			earned.Value = Sub.Earned;
		
		
			replytype.Text = Sub.ReplyType;
			                if (!SubmissionMaster.IsValidReply(Sub.ReplyType))
			                {
			                    dateReply.Value = DateTime.Today;
			                }
			                else
			                    dateReply.Value =Sub.ReplyDate;

			                feedback.Text = Sub.Replyfeedback;
			                rights.Text = Sub.Rights;

			textBoxDraft.Text = Sub.Draft; 

			ePriority.Text = Sub.Priority; //row[Data.SubmissionIndexFields.DESTINATION_PRIORITY].ToString();
			try
				                    {
				                        notes.Rtf = Sub.Notes;
				                    }
				                    catch (Exception)
				                    {
				                        notes.Text = Sub.Notes;
				                    }


		
			}

     //   public classSubmission submissionObject; // when LOADED or NEW this is the submission object. We overwrite only changed fields
        /// <summary>
        /// nice way of showing submission
        /// </summary>
        public void SubmissionSelected (Market market, int words)
		{
			if (market != null) {
				_Loading = true;
				WordCount = words;

				// reset font
				notes.Font = new Font ("Times", 12);
				decimal AmountOfPossibleSale = 0;

				string name = market.Caption;//row[Data.SubmissionIndexFields.MARKET_NAME].ToString();
				sGUID = market.Guid; //row[Data.SubmissionIndexFields.MARKETGUID].ToString();
				//  string sPage = Data.GetPageNameByGUID(sGUID, false);
				try {
					// classPageMarket market = (classPageMarket)Data.GoToPage(sPage);
					string markettype = "";
					string marketprint = "";
					if (market != null) {
						if (market.MarketType != null && market.MarketType != "") {
							markettype = " | " + market.MarketType;
						}
						if (market.PublishType != null && market.PublishType != "") {
							marketprint = " | " + market.PublishType;
						}
						AmountOfPossibleSale = market.GetPossibleSale (words);
						market = null;
					}
             

					if (0 == words) {
						// if we have 0 words our price will be zero so we hide the fields
						// in this way we also get away with hiding these fiels on a Market page
						// without tracking extra data
						// march 2010

						label10.Visible = false;
						textBoxSale.Visible = false;
					} else {
						label10.Visible = true;
						textBoxSale.Visible = true;
					}
                
                
					textBoxSale.Text = String.Format ("{0:0.##}", AmountOfPossibleSale);


					labelMarket.Text = String.Format ("{0} {1} {2}", name, markettype, marketprint);

//
//
//               
//
//
					panel1.Visible = true;


					// load submission details
					_NeedToSave = false;
					_Loading = false;
				} catch (Exception ex) {
					NewMessage.Show (Loc.Instance.GetString ("The market object was null ") + ex.ToString ());
				}
			} else {
				NewMessage.Show (Loc.Instance.GetString ("Market was null"));
			}
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// fill in combo boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmissionEditPanel_Load(object sender, EventArgs e)
        {
          

        }

        /// <summary>
        /// FAILED  Ideally this might be an embedded location (one place) to force a save
        /// DID NOT FIRE
        /// 
        /*
 - mdi closed
 - switching
 - closing

DID fire
 - tab*/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmissionEditPanel_Leave(object sender, EventArgs e)
        {
         //   Mess ageBox.Show("testing when this fires");
        }

        private bool _Loading=false; // mutex;
        public bool _NeedToSave = false; // very important

        /// <summary>
        /// Once I added more header info I needed to change the purpose of the label
        /// </summary>
        public string GetMarketName
        {
            get
            {
                // november 2009
                if (sGUID == "")
                {
                    //throw new Exception("GUID not defined when attempting to save submission entry.");
                    MessageBox.Show("GUID not defined when attempting to save submission entry. Consider shutting down.");
                    // feb 15 2012 - softening this error message
                }

				string sName = "GET ME A PAGE NAME";// Data.GetPageNameByGUID(sGUID, false);

                return sName;

            }
        }
        /// <summary>
        /// saves the contents of the current panel if necssary
        /// </summary>
        public bool Save()
        {
            //return false; // temp

//            if (submissionObject == null) return false;
//
//           
//            if (submissionObject != null && _NeedToSave == true)
//            {
//                FileInfo f = new FileInfo(submissionObject.FileName);
//               
//                string sNewFile = f.Name;
//                string sFileToSave = Program.AppMainForm.paths.SubmissionPath + "\\" + sNewFile;
//
//                //////////////////////////////////////////////////
//                // Set fields based on user changes
//                ///////////////////////////////////////////////////
//
//
//                submissionObject.Notes = notes.Rtf;
//
//                
//
//                submissionObject.MarketName = GetMarketName;
//                submissionObject.DateSent = dateSubmission.Value;
//                submissionObject.DateReplied = dateReply.Value;
//
//
//                submissionObject.SubmissionType = SubmissionType.Text;
//                submissionObject.ReplyType = replytype.Text;
//                submissionObject.ReplyFeedback = (classSubmission.ReplyFeedbackEnum)Enum.Parse(typeof(classSubmission.ReplyFeedbackEnum), feedback.Text);
//                submissionObject.Rights = rights.Text;
//                submissionObject.Draft = textBoxDraft.Text;
//                
//                submissionObject.Expenses = (double)postage.Value;
//                submissionObject.AmountOfSale = (double)earned.Value;
//
//
//                submissionObject.DestinationPriority = (float)Double.Parse( ePriority.Text);
//
//
//
//                //////////////////////////////////////////////////
//                // Do the actual save and database update
//                ///////////////////////////////////////////////////
//
//                General.Serialize(submissionObject, sFileToSave);
//                
//
//                Program.AppMainForm.data.UpdateSubmissionTableImproved(submissionObject);
//                _NeedToSave = false;
//
//            }
             
                    


            return true;
        }

        /// <summary>
        /// set reply date to today
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void replytype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Loading == false) 
                dateReply.Value = DateTime.Today;

            /// enable the replydate button only if a reply type has been chosen
			if (SubmissionMaster.IsValidReply(replytype.Text) == true)
            {
                dateReply.Enabled = true;
            }
            else
            	dateReply.Enabled = false;

		//	NewMessage.Show (SubmissionMaster.IsAcceptance (replytype.Text).ToString () );
            RefreshRightsSold();
        

            _NeedToSave = true;
        }
        /// <summary>
        /// will turn the rightssold text box blue if it has no Text
        /// and replyType = acceptance
        /// </summary>
        private void RefreshRightsSold ()
		{
			rights.BackColor = textDaysOut.BackColor;
			if (SubmissionMaster.IsAcceptance (replytype.Text) == true) {
				if (rights.Text == Constants.BLANK) {
					rights.BackColor = Color.Blue;
				}
			} else {

			}
        }
        private void SubmissionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Loading == false)
            {
                _NeedToSave = true;
                dateSubmission.Value = DateTime.Today;
//                if (SubmissionType.Text == classSubmission.DESTINATION_SUBTYPE)
//                {
//                    ToDestination();
//                }
//                else if (SubmissionType.Text == classSubmission.DEFAULT_SUBTYPE)
//                {
//                    ToSubmission();
//                   
//                }

            }

           
        }

        private void notes_TextChanged(object sender, EventArgs e)
        {
            _NeedToSave = true; // remember: Must setup all fields still
        }

        private void dateSubmission_ValueChanged(object sender, EventArgs e)
        {
            _NeedToSave = true;
            RecalculateDaysOut();
        }

        private void dateReply_ValueChanged(object sender, EventArgs e)
        {
            _NeedToSave = true;
            RecalculateDaysOut();
          
        }

        private void postage_ValueChanged(object sender, EventArgs e)
        {
            _NeedToSave = true;
        }

        private void earned_ValueChanged(object sender, EventArgs e)
        {
            _NeedToSave = true;
        }

        private void feedback_SelectedIndexChanged(object sender, EventArgs e)
        {
            _NeedToSave = true;
        }

        private void rights_TextChanged(object sender, EventArgs e)
        {
            _NeedToSave = true;
            RefreshRightsSold();
        }
        //recalcs and updates the days out field -- done here so its more verasilte
        private void RecalculateDaysOut()
        {
            DateTime datetouse = DateTime.Today;
            if (SubmissionMaster.IsValidReply(replytype.Text) == false)
            {
                // if not returned, use todays date
            }
            else
            {
                // if returned, use reply date'
                datetouse = dateReply.Value;
            }

            textDaysOut.Text = ( (datetouse - dateSubmission.Value).Days).ToString();
        }

        /// <summary>
        /// Need to think about the logic here. 
        /// 
        /// This is basically the same thing as editing any other field. But name is slightly mor eimportant.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editMarketName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EditMarket();
        }

        /// <summary>
        /// shows the edit market screen called from double click and clicking the link
        /// </summary>
        public void EditMarket()
        {
            // attempt #1 - easy - just invoke the function
            // don't even save, just set needs save to true


            // this call is only valid IF submission form open (i.e., can't be called in isolation from that)
//            submissionObject = ((SubmissionMainControl)this.Parent.Parent.Parent).AddSubmissionMarketPicker(submissionObject);
//
//            if (submissionObject == null)
//            {
//                //June 2010 - changed this to a LOG message. Wasn't sure what causes this to happen. Only seen it once.
//                namespaceLogs.Logs.Line("EditMarket", "Submission Object was null", "");
//                return;
//                //throw new Exception("Submission Object was null");
//            }
//
//            // nov 2009
//            sGUID = submissionObject.MarketGUID;
//            _NeedToSave = true;
//
//
//            if (submissionObject != null)
//            {
//                labelMarket.Text = submissionObject.MarketName;
//                
//                Save();
//                ProfanityCheck(labelMarket.Text);
//            }
//            else
//            {
//                NewMessage.Show("Error: submissionObject null after invoking market picker");
//            }
        }

        /// <summary>
        /// takes existing submission and converts it to a destination
        /// 
        /// called from SubmissionMainControl and user editing field directly
        /// </summary>
        public void ToDestination()
        {



            // assume object valid
//            if (submissionObject != null)
//            {
//                _Loading = true;
//                if (SubmissionType.Text != classSubmission.DESTINATION_SUBTYPE)
//                {
//                    SubmissionType.Text = classSubmission.DESTINATION_SUBTYPE;
//                }
//                _Loading = false;
//                //submissionObject.SubmissionType = classSubmission.DESTINATION_SUBTYPE;
//                _NeedToSave = true;
//                Save();
//                NoSubmissionSelected();
//            }
        }
 /// <summary>
        /// takes existing destination and converts it to a submission
        /// 
        /// called from SubmissionMainControl and user editing field directly
        /// </summary>
        public void ToSubmission()
        {

            

            // assume object valid
//            if (submissionObject != null)
//            {
//                _Loading = true;
//                if (SubmissionType.Text != classSubmission.DEFAULT_SUBTYPE)
//                {
//                    SubmissionType.Text = classSubmission.DEFAULT_SUBTYPE;
//                    dateSubmission.Value = dateSubmission.Value = DateTime.Today;
//                }
//                _Loading = false;
//                //submissionObject.SubmissionType = classSubmission.DESTINATION_SUBTYPE;
//                _NeedToSave = true;
//                Save();
//
//
//                ProfanityCheck(labelMarket.Text);
//
//                NoSubmissionSelected();
//            }
            

        }
        /// <summary>
        /// does a test to see if we need to show a profanity warning
        /// </summary>
        public void ProfanityCheck(string marketname)
        {
//            classPageMarket market = (classPageMarket)Data.GoToPage(marketname.Trim());
//            if (market != null)
//            {
//                if (market.ProfanityWarning == true)
//                {
//                    NewMessage.Show("REMINDER! Check this story submission for profanity, violence and sexuality as per the market's guidelines.");
//                }
//            }
        }

        private void ePriority_TextChanged(object sender, EventArgs e)
        {
            _NeedToSave = true;

        }

        /// <summary>
        /// opens in new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelMarket_Click(object sender, EventArgs e)
        {

            string sPage = GetMarketName;
            if (sPage != null && sPage != "")
            {
				NewMessage.Show ("Go TO this layout");
            //Program.AppMainForm.OpenNewWindow(sPage, 0, false);
            }
        }

        private void textBoxDraft_TextChanged(object sender, EventArgs e)
        {
            _NeedToSave = true;
        }
    } // class


}
