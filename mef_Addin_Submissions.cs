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
			get { return @"1.0.0.0"; }
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
		public override void RegisterType ()
		{

			//TODO: Create custom tables
			//TODO: need to hook up PublishTyleLIstConverter and MarketTYpeListOnverter
			if (false) {

			}

			//LayoutDetails.Instance.AddMarkupToList(new iMarkupYourOtherMind());
			//NewMessage.Show ("Registering Picture");
			Layout.LayoutDetails.Instance.AddToList(typeof(NoteDataXML_Market), Loc.Instance.GetString ("Market (ADDIN- YomSub)"));
			Layout.LayoutDetails.Instance.AddToList(typeof(NoteDataXML_Submissions), Loc.Instance.GetString ("Submissions (ADDIN- YomSub)"));
		}

		void TMP_BuildTestingPage ()
		{
			if (LayoutDetails.Instance.CurrentLayout != null) {
				LayoutPanels.NoteDataXML_Panel NewPanel = new NoteDataXML_Panel();
			
				NewPanel.GuidForNote = "NewPanelB";
				NewPanel.Caption = "Market Panel  BBBB";

				LayoutDetails.Instance.CurrentLayout.AddNote (NewPanel);
				NewPanel.GetPanelsLayout().ShowTabs = false;
				for (int i = 0; i < 200; i++)
				{
					NoteDataXML_Market Market = new NoteDataXML_Market(300,300);
					Market.Caption = "Market " + i.ToString();
					Market.GuidForNote = "market " + i.ToString();
					NewPanel.AddNote(Market);
					//Market.CreateParent(NewPanel.GetPanelsLayout());
				}
				NewPanel.Save ();
				LayoutDetails.Instance.CurrentLayout.SaveLayout ();
			}

		}
		public void TMP_FindAllMarketsWithCriteria ()
		{



			ArrayList notes = LayoutDetails.Instance.CurrentLayout.GetAllNotes ();
			int Found = 0;
			List<NoteDataXML_Market> FoundM = new List<NoteDataXML_Market>();
			foreach (NoteDataInterface note in notes) {
				if (note is NoteDataXML_Market)
				{
					if ((note as NoteDataXML_Market).MinimumWord > 0 && (note as NoteDataXML_Market).MaximumWord < 7000 )
					{
						Found++;
						FoundM.Add ((NoteDataXML_Market)note);
					}
				}
			}


			NewMessage.Show (Found.ToString ());
		}
		public void TMP_FindAllMarketsWithCriteria_USING_A_TABLE_INSTEAD ()
		{
			// OK: this is super fast too
			ArrayList FoundM = LayoutDetails.Instance.CurrentLayout.GetAllNotes ();
			foreach (NoteDataInterface note in FoundM) {
				if (note is NoteDataXML_Submissions) {
					//List<DataRow> rows = (note as NoteDataXML_Submissions).GetRows();
					DataView View = new DataView((note as NoteDataXML_Submissions).dataSource);
					//TODO: When build default table I need to define types somehow
					View.RowFilter = "Roll > '0' and Roll < '3'";
					NewMessage.Show(View.Count.ToString());
				}
				
			}
			
			


			
			

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
				action.MyMenuName = "TMP - Add testing notes";//Loc.Instance.GetString ("Screen Capture");
				action.ParentMenuName = "NotesMenu";//"NotesMenu";
				action.IsOnContextStrip = false;
				action.IsANote = true;
				action.IsOnAMenu = true;
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