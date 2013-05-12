// mef_Addin_Submissions.cs
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
namespace MefAddIns
{
	using MefAddIns.Extensibility;
	using System.ComponentModel.Composition;
	using System;
	using System.Data;


	using CoreUtilities;
	using System.Collections;
	using System.IO;
	using System.Collections.Generic;
	using System.Windows.Forms;
	using Layout;
	using HotKeys;
	using LayoutPanels;
	using Submissions;

	using System.Reflection;

	[Export(typeof(mef_IBase))]
	public class Addin_Submissions : PlugInBase, mef_IBase
	{
		#region variables
		
		
#endregion
		public Addin_Submissions()
		{
			guid = "submissionsystem";
		}
		
		public string Author
		{
			get { return @"Brent Knowles"; }
		}
		public string Version
		{
			// version history
			// 1.2 - remove destination from default submission type table becaues end users to do not need it
			get { return @"1.3.0.0"; }
		}
		public string Description
		{
			get { return Loc.Instance.GetString ("Track where stories and novels are submitted and financial details."); }
		}
		public string Name
		{
			get { return @"YOMSubs"; }
		}
		
		
		public override bool DeregisterType ()
		{
			
		//	LayoutDetails.Instance.RemoveMarkupFromList(typeof(iMarkupYourOtherMind));
			return true;
			//Layout.LayoutDetails.Instance.AddToList(typeof(NoteDataXML_Picture.NoteDataXML_Pictures), "Picture");
		}
		
		//		public override int TypeOfInformationNeeded {
		//			get {
		//				return (int)GetInformationADDINS.GET_CURRENT_LAYOUT_PANEL;
		//			}
		//		}
		//		public override void SetBeforeRespondInformation (object neededInfo)
		//		{
		//			CurrentPanel = (LayoutPanel)neededInfo;
		//		}
		
		//		mef_IBase myAddInOnMainFormForHotKeys = null;
		//		Action<mef_IBase> myRunnForHotKeys=null;
		//		public override void AssignHotkeys (ref List<HotKeys.KeyData> Hotkeys, ref mef_IBase addin, Action<mef_IBase> Runner)
		//		{
		//			
		//			base.AssignHotkeys (ref Hotkeys, ref addin, Runner);
		//			myAddInOnMainFormForHotKeys = addin;
		//			myRunnForHotKeys=Runner;
		//			Hotkeys.Add (new KeyData (Loc.Instance.GetString ("Picture Capture"), HotkeyAction, Keys.Control, Keys.P, Constants.BLANK, true, "pictureguid"));
		//			
		//		}
		
		//		public void HotkeyAction(bool b)
		//		{
		//			if (myRunnForHotKeys != null && myAddInOnMainFormForHotKeys != null)
		//				myRunnForHotKeys(myAddInOnMainFormForHotKeys);
		//			
		//		}

		public const  string SYSTEM_PUBLISHTYPES ="list_publishtypes";
		public const  string SYSTEM_MARKETTYPES ="list_markettypes";
		public override void RegisterType ()
		{
//			NewMessage.Show ("adding transactions");
//			LayoutDetails.Instance.AddTo_TransactionsLIST(typeof(Transactions.TransactionSubmission));
//			LayoutDetails.Instance.AddTo_TransactionsLIST(typeof(Transactions.TransactionSubmissionDestination));

			//LayoutDetails.Instance.AddMarkupToList(new iMarkupYourOtherMind());
			//NewMessage.Show ("Registering Picture");
			//	Layout.LayoutDetails.Instance.AddToList(typeof(NoteDataXML_Market), Loc.Instance.GetString ("Market (ADDIN- YomSub)"));
			Layout.LayoutDetails.Instance.AddToList (typeof(NoteDataXML_Submissions), Loc.Instance.GetString ("Submissions"), PlugInBase.AddInFolderName);



			// Build default table of grammar
			
			string TableName = SYSTEM_PUBLISHTYPES;
			LayoutPanels.NoteDataXML_Panel PanelContainingTables = LayoutPanel.GetPanelToAddTableTo (TableName);
			//	BringToFrontAndShow ();
			// can't use TableLayout because its not the actual tablelayout (its a copy)
			if (PanelContainingTables != null) {
				
				// create the note
				NoteDataXML_Table randomTables = new NoteDataXML_Table (100, 100, new ColumnDetails[2]{new ColumnDetails ("id", 100), 
					new ColumnDetails ("category", 100)});
				
				randomTables.Caption = TableName;
				
				
				PanelContainingTables.AddNote (randomTables);
				randomTables.CreateParent (PanelContainingTables.GetPanelsLayout ());
				
				randomTables.AddRow (new object[2]{"1", Loc.Instance.GetString ("Both")});
				randomTables.AddRow (new object[2]{"2", Loc.Instance.GetString ("Electronic")});
				randomTables.AddRow (new object[2]{"3", Loc.Instance.GetString ("None")});
				randomTables.AddRow (new object[2]{"4", Loc.Instance.GetString ("Print")});
				
				//		LayoutDetails.Instance.TableLayout.SaveLayout();
				PanelContainingTables.GetPanelsLayout ().SaveLayout ();


				// now add the next table
				TableName = SYSTEM_MARKETTYPES;
				randomTables = new NoteDataXML_Table (100, 100, new ColumnDetails[2]{new ColumnDetails ("id", 100), 
					new ColumnDetails ("category", 100)});
				randomTables.Caption = TableName;
				PanelContainingTables.AddNote (randomTables);
				randomTables.CreateParent (PanelContainingTables.GetPanelsLayout ());

				randomTables.AddRow (new object[2]{"1", Loc.Instance.GetString ("Non Paying")});
				randomTables.AddRow (new object[2]{"2", Loc.Instance.GetString ("None")});
				randomTables.AddRow (new object[2]{"3", Loc.Instance.GetString ("Semi-Pro")});
				randomTables.AddRow (new object[2]{"4", Loc.Instance.GetString ("Small Press (Token)")});
				randomTables.AddRow (new object[2]{"5", Loc.Instance.GetString ("Pro Market")});

				//NewMessage.Show("Making new");
				// now we reload the system version
				PanelContainingTables.GetPanelsLayout ().SaveLayout ();
				LayoutDetails.Instance.TableLayout.LoadLayout (LayoutDetails.TABLEGUID, true, null);
				//BringToFrontAndShow ();
			} else {
				//NewMessage.Show ("Panel not found");
			}

		}

		void TMP_BuildTestingPage ()
		{
//			if (LayoutDetails.Instance.CurrentLayout != null) {
//				LayoutPanels.NoteDataXML_Panel NewPanel = new NoteDataXML_Panel();
//			
//				NewPanel.GuidForNote = "NewPanelB";
//				NewPanel.Caption = "Market Panel  BBBB";
//
//				LayoutDetails.Instance.CurrentLayout.AddNote (NewPanel);
//				NewPanel.GetPanelsLayout().ShowTabs = false;
//				for (int i = 0; i < 200; i++)
//				{
//				//	NoteDataXML_Market Market = new NoteDataXML_Market(300,300);
//					Market.Caption = "Market " + i.ToString();
//					Market.GuidForNote = "market " + i.ToString();
//					NewPanel.AddNote(Market);
//					//Market.CreateParent(NewPanel.GetPanelsLayout());
//				}
//				NewPanel.Save ();
//				LayoutDetails.Instance.CurrentLayout.SaveLayout ();
//			}

		}
		public void TMP_FindAllMarketsWithCriteria ()
		{



//			ArrayList notes = LayoutDetails.Instance.CurrentLayout.GetAllNotes ();
//			int Found = 0;
//			List<NoteDataXML_Market> FoundM = new List<NoteDataXML_Market>();
//			foreach (NoteDataInterface note in notes) {
//				if (note is NoteDataXML_Market)
//				{
//					if ((note as NoteDataXML_Market).MinimumWord > 0 && (note as NoteDataXML_Market).MaximumWord < 7000 )
//					{
//						Found++;
//						FoundM.Add ((NoteDataXML_Market)note);
//					}
//				}
//			}


			//NewMessage.Show (Found.ToString ());
		}





		void TMP_FillTableWithData ()
		{

			// RESULTS: This loads in less than a second compared to 16-20 if each market is a note.

			// RERUN: after we add all columns

			// NEW RESULTS:

			ArrayList FoundM = LayoutDetails.Instance.CurrentLayout.GetAllNotes ();
			foreach (NoteDataInterface note in FoundM) {
				if (note is NoteDataXML_Submissions) {
					for (int i = 0; i < 500; i++) {
						(note as NoteDataXML_Submissions).AddRow (new string[4] {
							"lajdsfklasjflkasjdflkasdjf",
							"aesdfasdfasdfasdfasdfasdfas",
							"asdfaewaefads'vd",
							"asdfadsfewe333333"
						});
					}
				}
			}
		}



		public void TMP_BuildSampleTable ()
		{
//
//			PropertyInfo[] properties = typeof(Market).GetProperties ();
//			//	NewMessage.Show ("Property Count " + properties.Length.ToString ());
//			ArrayList FoundM = LayoutDetails.Instance.CurrentLayout.GetAllNotes ();
//			foreach (NoteDataInterface note in FoundM) {
//				if (note is NoteDataXML_Submissions) {
//
//
//					// first build the data table and ovewrite the old
//					DataTable Table = CreateDataTable (properties);
//				//	NewMessage.Show (Table.Columns.Count.ToString ());
//					(note as NoteDataXML_Submissions).ForceTableUpdate (Table);
//					//(note as NoteDataXML_Submissions).dataSource =Table;
//					LayoutDetails.Instance.CurrentLayout.SaveLayout ();
//					//(note as NoteDataXML_Submissions).Update(LayoutDetails.Instance.CurrentLayout);
//
//					// then populate it with example data
//
//
//					for (int i = 0; i < 500; i++) {
//						Market newMarket = Market.DefaultMarket ();
//						newMarket.Caption = "Market " + i.ToString ();
//						FillData (properties, Table, newMarket);
//
////					}
//					}
//				}
//			}
		}


		public void RespondToMenuOrHotkey<T>(T form) where T: System.Windows.Forms.Form, MEF_Interfaces.iAccess 
		{
			//TMP_BuildSampleTable ();
		//	TMP_FindAllMarketsWithCriteria_USING_A_TABLE_INSTEAD();

			//TMP_FillTableWithData();

			//TMP_FindAllMarketsWithCriteria();

		//	TMP_BuildTestingPage();


			//NewMessage.Show ("Fact or Search! This would only appear if a menu item was hooked up");
			// do nothing. This is not called for mef_Inotes
			return;
		}
	
		

		// the param is the filename to the temp file
		public void ActionWithParamForNoteTextActions (object param)
		{

		}
		public override string dependencymainapplicationversion { get { return "1.0.0.0"; }}
		
		//override string GUID{ get { return  "notedataxml_picture"; };
		public PlugInAction CalledFrom { 
			get
			{
				PlugInAction action = new PlugInAction();
				//	action.HotkeyNumber = -1;
				action.MyMenuName = "*error";//Loc.Instance.GetString ("Screen Capture");
				action.ParentMenuName = "NotesMenu";//"NotesMenu";
				action.IsOnContextStrip = false;
				action.IsANote = true;
				action.IsOnAMenu = false;
				action.IsNoteAction = false;
				action.QuickLinkShows = false;
				action.ToolTip ="";//Loc.Instance.GetString("Allows images to be added to Layouts, as well as a Screen Capture tool.");
				//action.Image = FileUtils.GetImage_ForDLL("camera_add.png");
				action.GUID = GUID;
				return action;
			} 
		}
		
		
		
	}
}