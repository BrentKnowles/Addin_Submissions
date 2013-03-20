using System;
using Layout;
using Transactions;
using System.Collections.Generic;
using CoreUtilities;

namespace Submissions
{
	public class SubmissionMaster
	{
		public SubmissionMaster ()
		{
		}
		public const string TABLE_SubmissionTypes = "submissiontypes";
		public const string TABLE_ReplyTypes = "replytypes";
		public const string TABLE_ReplyFeedback = "replyfeedback";


		public const string CODE_NO_REPLY_YET =  @"none";
		public const string CODE_ACCEPTANCE = "sale";
		public const string CODE_DESTINATION = @"destination";
		public const string CODE_FEEDBACK1 ="1";
		public const string CODE_FEEDBACK2 ="2";
		public const string CODE_FEEDBACK3 ="3";
		public const string CODE_FEEDBACK4 ="4";

		public static void GetMarketDetailsForWarnings (string selectedMarketGuid, string projectGuid, out bool MarketAvailable, out bool ProjectSendHereBefore)
		{
			List<Transactions.TransactionSubmission> submissions = GetListOfSubmissionsForMarket (selectedMarketGuid);
			MarketAvailable = true;
			ProjectSendHereBefore = false;



			foreach (TransactionSubmission submission in submissions) {
				if (ThisSubmissionNotResolved(submission) == true)
				{
					// means we are busy
					MarketAvailable = false;
				}
				if (submission.ProjectGUID == projectGuid)
				{
					ProjectSendHereBefore = true;
				}
			}

		}



		public static List<Transactions.TransactionBase> GetListOfSubmissionsForProject(string ProjectGUID)
		{
			return LayoutDetails.Instance.TransactionsList.GetEventsForLayoutGuid (ProjectGUID,String.Format (" and {1}='{0}' ", TransactionsTable.T_SUBMISSION, TransactionsTable.TYPE));
		}

		public static List<TransactionBase> GetListOfDestinationsForProject (string projectGUID)
		{
			return LayoutDetails.Instance.TransactionsList.GetEventsForLayoutGuid (projectGUID,String.Format (" and {1}='{0}' ", TransactionsTable.T_SUBMISSION_DESTINATION, TransactionsTable.TYPE));
		}

		public static List<Transactions.TransactionSubmission> GetListOfSubmissionsForMarket (string mARKET_GUID)
		{

			List<Transactions.TransactionSubmission> list_new = new List<TransactionSubmission> ();

			List<Transactions.TransactionBase> list = LayoutDetails.Instance.TransactionsList.GetEventsForLayoutGuid ("*", String.Format (" and data2='{0}' and type='{1}' ", mARKET_GUID, TransactionsTable.T_SUBMISSION));
			;
			foreach (Transactions.TransactionBase tran in list) {
				if (tran is Transactions.TransactionSubmission)
				{
					list_new.Add ( (Transactions.TransactionSubmission)tran);
				}
			}

			return list_new;



		}
		public static List<Transactions.TransactionSubmission> GetListOfDestinationsForMarket (string mARKET_GUID)
		{
			List<Transactions.TransactionSubmission> list_new = new List<TransactionSubmission> ();
			
			List<Transactions.TransactionBase> list = LayoutDetails.Instance.TransactionsList.GetEventsForLayoutGuid ("*", String.Format (" and data2='{0}' and type='{1}' ", mARKET_GUID, TransactionsTable.T_SUBMISSION_DESTINATION));
			;
			foreach (Transactions.TransactionBase tran in list) {
				if (tran is Transactions.TransactionSubmissionDestination)
				{
					list_new.Add ( (TransactionSubmission)tran);
				}
			}
			
			return list_new;
			//return LayoutDetails.Instance.TransactionsList.GetEventsForLayoutGuid ("*",String.Format (" and data2='{0}' and type='{1}' ", mARKET_GUID, TransactionsTable.T_SUBMISSION_DESTINATION));
		}

		public static List<Transactions.TransactionBase> GetListOfSubmissionsAll()
		{
			return LayoutDetails.Instance.TransactionsList.GetEventsForLayoutGuid ("*",String.Format (" and {0}='{1}' ",TransactionsTable.TYPE, TransactionsTable.T_SUBMISSION));
		}

		// the first row for populating fields
		public static string GetDefaultSubmission ()
		{
			List<string> result = LayoutDetails.Instance.CurrentLayout.GetListOfStringsFromSystemTable (TABLE_SubmissionTypes, 1,Constants.BLANK, false);
			if (result != null && result.Count > 0) {
				return result [0];
			}
			return Constants.ERROR;
		}
		public static string GetDefaultReplyType ()
		{
			List<string> result = LayoutDetails.Instance.CurrentLayout.GetListOfStringsFromSystemTable (TABLE_ReplyTypes, 1,Constants.BLANK, false);
			if (result != null && result.Count > 0) {
				return result [0];
			}
			return Constants.ERROR;
		}
		public static string GetDefaultReplyTypeFeedback ()
		{
			List<string> result = LayoutDetails.Instance.CurrentLayout.GetListOfStringsFromSystemTable (TABLE_ReplyFeedback, 1,Constants.BLANK, false);
			if (result != null && result.Count > 0) {
				return result [0];
			}
			return Constants.ERROR;
		}

		// If list is EMPTY then this Project is AVAILABLE
		public static List<TransactionBase> GetListOfOutstandingSubmissionsOrAcceptances (string PROJECT_GUID, string querywrapper)
		{
			// grab list of valid "no replies"
		//	List<string> result = LayoutDetails.Instance.CurrentLayout.GetListOfStringsFromSystemTable (TABLE_ReplyTypes, 1, String.Format ("1|{0}", CODE_NO_REPLY_YET));

			// build query list




			//lg.Instance.Line ("SubmissionMaster->GetListOfOUtstandingSubmissions", ProblemType.MESSAGE, querywrapper);
			List<Transactions.TransactionBase> transactions = LayoutDetails.Instance.TransactionsList.GetEventsForLayoutGuid (PROJECT_GUID, querywrapper);
			return transactions;
		}

		public static void ChangeToSubmission (TransactionSubmissionDestination Current)
		{
			// delete existing Transaction
			LayoutDetails.Instance.TransactionsList.DeleteEvent(TransactionsTable.ID, Current.ID);
			//add as destination
			TransactionSubmission NewVersion = new TransactionSubmission(Current.GetRowData ());
			//NewVersion.SetID(DBNull.Value);
			NewVersion.RefreshType(TransactionsTable.T_SUBMISSION);
			LayoutDetails.Instance.TransactionsList.AddEvent(NewVersion);
		}

		public static void ChangeToDestination(TransactionSubmission Current)
		{
			// make a copy of transaction (which we have coming in)
			
			
			// delete existing Transaction
			LayoutDetails.Instance.TransactionsList.DeleteEvent(TransactionsTable.ID, Current.ID);
			//add as destination
			TransactionSubmissionDestination NewVersion = new TransactionSubmissionDestination(Current.GetRowData ());
			//NewVersion.SetID(DBNull.Value);
			NewVersion.RefreshType(TransactionsTable.T_SUBMISSION_DESTINATION);
			LayoutDetails.Instance.TransactionsList.AddEvent(NewVersion);
		}

		static string LookupInTable (string Tablename, string ToSearchFor)
		{
			// don't like doing it this way
			// but if we assume we are on a Layout (which is necssary given that's where Submission Notes exist
			// then we also assume tht able has to be on this Layout (because it does)
			// Which means the current Layout must have the table on it.

			// worried this might be slow
			string returnvalue = Constants.BLANK;

		


			if (LayoutDetails.Instance.CurrentLayout == null) {
				// There is no CurrentLayout while note is first loading
				NewMessage.Show (Loc.Instance.GetString ("Really bad. You should never see this. It means that a call from a SubmissionNote on a Layout cannot find the Layout it is on."));
				return returnvalue;
			}
			//NoteDataXML_Table table = LayoutDetails.Instance.CurrentLayout.FindNoteByGuid(Tablename);
			List<string> result = LayoutDetails.Instance.CurrentLayout.GetListOfStringsFromSystemTable (Tablename, 2, String.Format ("1|{0}", ToSearchFor));




		//	NewMessage.Show (result.Count.ToString ());
			if (result != null && result.Count > 0) {
				returnvalue = result[0];
			}
			return returnvalue;
		}
		/// <summary>
		/// Determines if is valid reply the specified Sub.
		/// 
		/// Returns true if this is a valid reply (ie.., you have received a response)
		/// </summary>
		/// <returns>
		/// <c>true</c> if is valid reply the specified Sub; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='Sub'>
		/// Sub.
		/// </param>
		public static bool IsValidReply (string replytype)
		{
			// grab submission type
		//	string replytype = replytype;
			string replytype_code = LookupInTable (TABLE_ReplyTypes, replytype);
			if (Constants.BLANK != replytype_code) {
				//NewMessage.Show (String.Format ("ReplyTypeCode {0} Comparing to Code {1}", replytype_code, CODE_NO_REPLY_YET));
				if (replytype_code.ToLower () == CODE_NO_REPLY_YET.ToLower ()) {
					return false;
				}
			}

			return true;
		}

		public static bool IsAcceptance (string replytype)
		{
			// grab submission type
			//	string replytype = replytype;
			string replytype_code = LookupInTable (TABLE_ReplyTypes, replytype);
			if (Constants.BLANK != replytype_code) {
				//NewMessage.Show (String.Format ("ReplyTypeCode {0} Comparing to Code {1}", replytype_code, CODE_NO_REPLY_YET));
				if (replytype_code.ToLower () == CODE_ACCEPTANCE.ToLower ()) {
					return true;
				}
			}
			
			return false;
		}

		private static bool IsDestination (TransactionSubmission Sub)
		{
			if (Sub.GetTypeCode == TransactionsTable.T_SUBMISSION_DESTINATION.ToString ()) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// Determines if has no reply_ submission the specified Sub.
		/// </summary>
		/// <returns>
		/// <c>true</c> if has no reply_ submission the specified Sub; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='Sub'>
		/// Sub.
		/// </param>
		public static bool ThisSubmissionNotResolved (TransactionSubmission Sub)
		{    
			//if (dr[Data.SubmissionIndexFields.REPLYTYPE].ToString().ToLower()
			//                        == English.Invalid.ToLower() &&
			//                        dr[Data.SubmissionIndexFields.SUBMISSIONTYPE].ToString().ToLower()
			//                        != classSubmission.DESTINATION_SUBTYPE.ToLower())
			if (!IsValidReply (Sub.ReplyType) && !IsDestination (Sub)) {
				return true;
			}
			return false;
		}
	}
}

