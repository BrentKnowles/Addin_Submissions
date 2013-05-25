// Market.cs
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
using System.ComponentModel;
using System.Xml.Serialization;
using CoreUtilities;
using System.Data;
using System.Reflection;
using Layout;
using MefAddIns;
namespace Submissions
{
	public class Market
	{
		#region constants

		private const string MARKETDETAILS = "Market Details";
		private const string MARKETPAY = "Market Details - Payment";
		#endregion



		private bool retired = false;
		[CategoryAttribute(MARKETDETAILS)]
		public bool Retired {
			get {
				return retired;
			}
			set {
				retired = value;
			}
		}


		private string caption = Loc.Instance.GetString ("New Market");
		[CategoryAttribute(MARKETDETAILS)]
		public string Caption {
			get {
				return caption;
			}
			set {
				caption = value;
			}
		}

		private string guid = Constants.BLANK;

		[ReadOnly(true)]
		[CategoryAttribute("Advanced")]
		public string Guid {
			get {
				return guid;
			}
			set {
				guid = value;
			}
		}

		private string mEditor=Loc.Instance.GetString ("Editor");
		private string mAddress;
		private string mCity;
		private string mProvince;
		private string mPostalCode;
		private string mCountry;
		private string mWorkPhone;
		private string mFaxPhone;
		private string mEmail;
		private string mWeb;
		private bool mUnsolicited;
		private bool mSimultaneous;
		private bool mMultiple;
		private bool mQuery;
		
		private bool mOnHiatus;
		private bool mAcceptEmail;
		private int mAvgRespTime;
		private double mPayment;
		private int mMinimumWord;
		private int mMaximumWord;
		private DateTime mReadingDateStart;
		private DateTime mReadingDateEnd;
		private DateTime mLastUpdate;
		private bool bHasReadingPeriod;
		private bool bIsAnthology;
		private double mMinimumPay;
		private double mMaximumPay;
		bool mGeneralAudienceWarning;
		
		private bool bAcceptReprints;
		
		
		
		
		/// <summary>
		/// returns the amount of a psosible sale, taking into consideration the min and max
		/// </summary>
		/// <param name="words"></param>
		/// <returns></returns>
		public decimal GetPossibleSale(int words)
		{
			double basevalue = this.Payment * words;
			if (mMinimumPay != 0 && (basevalue < mMinimumPay)) basevalue = mMinimumPay;
			if (mMaximumPay != 0 && (basevalue > mMaximumPay)) basevalue = mMaximumPay;
			
			return (decimal)basevalue;
		}
		
		[CategoryAttribute(MARKETPAY)]
		[DescriptionAttribute("If this markets has a minimum payment amount, enter it here.")]
		[DisplayName("Minimum Payment")]
		public double MinPayment
		{
			get { return mMinimumPay; }
			set { mMinimumPay = value; }
		}
		
		[CategoryAttribute(MARKETPAY)]
		[DescriptionAttribute("If this markets has a maximum payment amount, enter it here.")]
		[DisplayName("Maximum Payment")]
		public double MaxPayment
		{
			get { return mMaximumPay; }
			set { mMaximumPay = value; }
		}
		
		[CategoryAttribute(MARKETDETAILS)]
		[TypeConverter(typeof(YesNoTypeConverter))]
		[DescriptionAttribute("Set to true if this market if you want a warning before submitting this market to remind you to reduce profanity and violence.")]
		[DisplayName("Profanity Warning?")]
		public bool ProfanityWarning
		{
			get { return mGeneralAudienceWarning; }
			set { mGeneralAudienceWarning = value; }
		}
		
		[CategoryAttribute(MARKETDETAILS)]
		[TypeConverter(typeof(YesNoTypeConverter))]
		[DescriptionAttribute("Does this market publish reprints?")]
		[DisplayName("Accepts Reprints?")]
		public bool AcceptReprints
		{
			get { return bAcceptReprints; }
			set { bAcceptReprints = value; }
		}
		
		/// <summary>
		/// returns true if the market is able to be submitted to
		/// (not on hiatus or a reading period)
		/// 
		/// June 2009 - Logic error
		///   I was returning false if we were IN a reading period
		/// </summary>
		/// <returns></returns>
		public bool Reading()
		{
			if (OnHiatus == true)
			{
				return false;
			}
			if ( (HasReadingPeriod == true) && (ReadingPeriodStart <= DateTime.Today) && (ReadingPeriodEnd>=DateTime.Today))
			{
				return true;
			}
			else //outside reading period
				if ((HasReadingPeriod == true) && ((ReadingPeriodStart > DateTime.Today) || (ReadingPeriodEnd < DateTime.Today)))
			{
				return false;
			}
			return true;
		}
		
		
		
		
		
		
		const string ADDRESS = "Market Address";
		const string CONTACT = "Market Contact Information";
		
		
		///////////////////////////////////////////////
		// Property Grid View
		///////////////////////////////////////////////
		const string ADRESSS = "Market Address";
		const string DATES = "Market Dates";
		
		[CategoryAttribute(DATES)]
		[DescriptionAttribute("When does the reading period for this market open?")]
		[DisplayName("Reading Period, Start")]
		public DateTime ReadingPeriodStart
		{
			get { return mReadingDateStart; }
			set { mReadingDateStart = value; }
		}
		
		[CategoryAttribute(DATES)]
		[DescriptionAttribute("Does this market have a reading period? If so, specify the dates when it accepts submissions.")]
		[DisplayName("Reading Period?")]
		[TypeConverter(typeof(YesNoTypeConverter))]
		public bool HasReadingPeriod
		{
			get { return bHasReadingPeriod; }
			set { bHasReadingPeriod = value; }
		}
		[CategoryAttribute(DATES)]
		[DescriptionAttribute("When does the reading period for this market end?")]
		[DisplayName("Reading Period, End")]
		public DateTime ReadingPeriodEnd
		{
			get { return mReadingDateEnd; }
			set { mReadingDateEnd = value; }
		}
		
		[CategoryAttribute(CONTACT)]
		[DescriptionAttribute("Enter the name of the editor, if known.")]
		[DisplayName("Editor")]
		public string Editor
		{
			get { return mEditor; }
			set { mEditor = value; }
		}
		[CategoryAttribute(ADDRESS)]
		[DescriptionAttribute("Enter the street address or box number.")]
		[DisplayName("Address")]
		public string Address
		{
			get { return mAddress; }
			set { mAddress = value; }
		}
		
		[CategoryAttribute(ADDRESS)]
		[DescriptionAttribute("Enter the city portion of the market's address.")]
		[DisplayName("City")]
		public string City
		{
			get { return mCity; }
			set { mCity = value; }
		}
		[CategoryAttribute(ADDRESS)]
		[DescriptionAttribute("Enter the state or province for the market.")]
		[DisplayName("State or Province")]
		public string Province
		{
			get { return mProvince; }
			set { mProvince = value; }
		}
		[CategoryAttribute(ADDRESS)]
		[DescriptionAttribute("Enter the ZIP or Postal Code for the market.")]
		[DisplayName("ZIP or Postal Code")]
		public string PostalCode
		{
			get { return mPostalCode; }
			set { mPostalCode = value; }
		}
		[CategoryAttribute(ADDRESS)]
		[DescriptionAttribute("Enter the country portion of the market's address.")]
		[DisplayName("Country")]
		public string Country
		{
			get { return mCountry; }
			set { mCountry = value; }
		}
		[CategoryAttribute(CONTACT)]
		[DescriptionAttribute("Enter the phone number.")]
		[DisplayName("Phone#, Work")]
		public string WorkPhone
		{
			get { return mWorkPhone; }
			set { mWorkPhone = value; }
		}
		[CategoryAttribute(CONTACT)]
		[DescriptionAttribute("Enter the fax number.")]
		[DisplayName("Fax#")]
		public string FaxPhone
		{
			get { return mFaxPhone; }
			set { mFaxPhone = value; }
		}
		[CategoryAttribute(CONTACT)]
		[DescriptionAttribute("The e-mail address for this market.")]
		[DisplayName("E-mail")]
		public string Email
		{
			get { return mEmail; }
			set { mEmail = value; }
		}
		[CategoryAttribute(CONTACT)]
		[DescriptionAttribute("Enter the URL for the market's web-site.")]
		[DisplayName("URL")]
		public string Web
		{
			get { return mWeb; }
			set { mWeb = value; }
		}
		
		[CategoryAttribute(MARKETDETAILS)]
		[TypeConverter(typeof(YesNoTypeConverter))]
		[DescriptionAttribute("Is this market an anthology?")]
		[DisplayName("Is Anthology?")]
		public bool IsAnthology
		{
			get { return bIsAnthology; }
			set { bIsAnthology = value; }
		}
		
		
		[CategoryAttribute(MARKETDETAILS)]
		[TypeConverter(typeof(YesNoTypeConverter))]
		[DescriptionAttribute("Does this market accept unsolicited submissions?")]
		[DisplayName("Accepts Unsolicited")]
		public bool Unsolicited
		{
			get { return mUnsolicited; }
			set { mUnsolicited = value; }
		}
		[CategoryAttribute(MARKETDETAILS)]
		[TypeConverter(typeof(YesNoTypeConverter))]
		[DescriptionAttribute("Does this market allow submissions to be sent to it while the same submission has been submitted to another market?")]
		[DisplayName("Accepts Simultaneous")]
		public bool Simultaneous
		{
			get { return mSimultaneous; }
			set { mSimultaneous = value; }
		}
		[CategoryAttribute(MARKETDETAILS)]
		[TypeConverter(typeof(YesNoTypeConverter))]
		[DescriptionAttribute("Does this market accept multiple submissions at the same time?")]
		[DisplayName("Accepts Multiple")]
		public bool Multiple
		{
			get { return mMultiple; }
			set { mMultiple = value; }
		}
		[CategoryAttribute(MARKETDETAILS)]
		[TypeConverter(typeof(YesNoTypeConverter))]
		[DescriptionAttribute("Does this market accept/encourage queries?")]
		[DisplayName("Accepts Queries")]
		public bool Query
		{
			get { return mQuery; }
			set { mQuery = value; }
		}
		
		[CategoryAttribute(MARKETDETAILS)]
		[TypeConverter(typeof(YesNoTypeConverter))]
		[DescriptionAttribute("Does this market accept electronic submissions (e-mail or form)?")]
		[DisplayName("Accepts electronic submissions")]
		public bool AcceptsEmail
		{
			get { return mAcceptEmail; }
			set { mAcceptEmail = value; }
		}
		[CategoryAttribute(MARKETDETAILS)]
		[TypeConverter(typeof(YesNoTypeConverter))]
		[DescriptionAttribute("Is this market current on hiatus (not accepting submissions)?")]
		[DisplayName("On Hiatus")]
		public bool OnHiatus
		{
			get { return mOnHiatus; }
			set { mOnHiatus = value; }
		}
		[CategoryAttribute(MARKETDETAILS)]
		[DescriptionAttribute("The average number of days to receive a response.")]
		[DisplayName("Average Response Time")]
		public int AverageResponeTime
		{
			get { return mAvgRespTime; }
			set { mAvgRespTime = value; }
		}
		[CategoryAttribute(MARKETPAY)]
		[DescriptionAttribute("How much per word will you be paid when the market accepts your submission.")]
		[DisplayName("Payment per word")]
		public double Payment
		{
			get { return mPayment; }
			set { mPayment = value; }
		}
		[CategoryAttribute(MARKETDETAILS)]
		[DescriptionAttribute("What is the minimum sized submission the market considers?")]
		[DisplayName("Words, Minimum")]
		public int MinimumWord
		{
			get { return mMinimumWord; }
			set { mMinimumWord = value; }
		}
		[CategoryAttribute(MARKETDETAILS)]
		[DescriptionAttribute("What is the maximum sized submission the market considers?")]
		[DisplayName("Words, Maximum")]
		public int MaximumWord
		{
			get { return mMaximumWord; }
			set { mMaximumWord = value; }
		}
		[CategoryAttribute(DATES)]
		[DescriptionAttribute("When was this market last updated by you? This field is set automatically.")]
		[DisplayName("Last Updated On...")]
		public DateTime LastUpdate
		{
			get { return mLastUpdate; }
			set { mLastUpdate = value; }
		}
		private string publishtype;
		[CategoryAttribute(MARKETDETAILS)]
		//  [System.ComponentModel.TypeConverter(typeof(CoreUtilities.EnumDescConverter))]
		[TypeConverter(typeof(PublishTypeListConverter))]
		[DisplayName("Publishing Type")]
		[DescriptionAttribute("Indicate whether this ia print or online magazine")]
		public string PublishType
		{
			get { return publishtype; }
			set { publishtype = value; }
		}
		private string markettype;
		[CategoryAttribute(MARKETDETAILS)]
		//  [System.ComponentModel.TypeConverter(typeof(CoreUtilities.EnumDescConverter))]
		[TypeConverter(typeof(MarketTypeListConverter))]
		[DisplayName("Pay Category/Market Type")]
		[DescriptionAttribute("Indicate the type of market")]
		public string MarketType
		{
			get { return markettype; }
			set { markettype = value; }
		}

		private string notes = Constants.BLANK;
		[Browsable(false)]
		public string Notes {
			get {
				return notes;
			}
			set {
				notes = value;
			}
		}

		public Market ()
		{

		}

		public Market (DataRow DataBaseRow)
		{
			// creates a Market object based on the contents of a databaserow
			//	this.Caption = DataBaseRow["Caption"].ToString() ;



//			this.GetType ().InvokeMember (field,
//			                           BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
//			                            Type.DefaultBinder, this, DataBaseRow [field]);

			foreach (DataColumn column in (DataBaseRow.Table.Columns )){
			//foreach (DataColumn column in ((DataView)DataBaseRow.DataView).Table.Columns ){
				string field = column.ColumnName; //"Caption";
				Type type = this.GetType ();
				PropertyInfo prop = type.GetProperty (field);
				try
				{
					object setter = DataBaseRow [field];

					if (prop.GetType () == typeof(float))
					{
						float test = 0.0f;
						if (float.TryParse (setter.ToString (), out test) == false)
						{
							// failed parse, default to 0
							setter = 0.0f;
						}
					}

					if (prop.GetType () == typeof(bool))
					{
						bool test = false;
						if (bool.TryParse(setter.ToString (), out test) == false)
						{
							// failed parse, default to false
							setter = false;
						}

					}
				if (null != prop) {
					prop.SetValue (this, DataBaseRow [field], null);
				}
				}
				catch (Exception)
				{
					// the Market Add/Edit form should prevent data issues but if the user
					// modified by hand we might end up with invalid values.
				}
			}
		}


		public static Market DefaultMarket()
		{
			Market defaultMarket = new Market();
			defaultMarket.Guid = System.Guid.NewGuid().ToString ();
			defaultMarket.PublishType = LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable(Addin_Submissions.SYSTEM_PUBLISHTYPES,1)[0];
			defaultMarket.MarketType = LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable(Addin_Submissions.SYSTEM_MARKETTYPES,1)[0];;
			defaultMarket.MinimumWord = 1000;
			defaultMarket.MaximumWord = 5000;
			defaultMarket.AcceptReprints = false;
			defaultMarket.AcceptsEmail = true;
			defaultMarket.LastUpdate = DateTime.Now;
			defaultMarket.ProfanityWarning = false;
			defaultMarket.Web="www.brentknowles.com";
			defaultMarket.Payment = 0.05;
			return defaultMarket;
				 
		}
	}
}

