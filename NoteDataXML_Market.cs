using System;
using CoreUtilities;
using System.Windows.Forms;
using System.Drawing;
using CoreUtilities.Links;
using System.ComponentModel;
using Layout;
using System.Xml.Serialization;
//TODO: Transform this into a MarketRow class (like a Transaction but for Markets, a wrapper) [REMOVE ME]
namespace MefAddIns
{
	public class NoteDataXML_Market  : Layout.NoteDataXML_RichText
	{
		public override int defaultHeight { get { return 500; } }
		public override int defaultWidth { get { return 300; } }
		#region variables
		public override bool IsLinkable { get { return false; }}
		
		private const string MARKETDETAILS = "Market Details";
		private const string MARKETPAY = "Market Details - Payment";
		private string mEditor;
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

#endregion
		
		#region interface
		TableLayoutPanel TablePanel = null;
		
#endregion
		
		
		public override void Dispose ()
		{
			

			base.Dispose();
			
		}
		private void CommonConstructor ()
		{

			Caption = Loc.Instance.GetString("New Market");

			mEditor = mAddress = mCity = mPostalCode = mProvince = mEmail = "";
			mWorkPhone = mWeb = mCountry = mFaxPhone = "";
			mAcceptEmail =  mMultiple = mOnHiatus = false;
			mUnsolicited = true;
			mAvgRespTime = mMinimumWord = mMaximumWord = 0;
			mPayment = 0.0;
			IsAnthology = false;
			bAcceptReprints = false;
			mMinimumPay = 0;
			mMaximumPay = 0;
			mGeneralAudienceWarning = false;
			this.Editor = "DEFAULT EDITOR";

		}
		public NoteDataXML_Market () : base()
		{
			CommonConstructor();
		}
		public NoteDataXML_Market(int height, int width):base(height, width)
		{
			CommonConstructor();
		}
		
		protected override void DoBuildChildren (LayoutPanelBase Layout)
		{
			base.DoBuildChildren (Layout);
			
			
			
			CaptionLabel.Dock = DockStyle.Top;
			
			TablePanel = new TableLayoutPanel ();
			
			
			
			TablePanel.Height = 200;
			TablePanel.RowCount = 4;
			TablePanel.ColumnCount = 2;
			TablePanel.Dock = DockStyle.Top;
			ParentNotePanel.Controls.Add (TablePanel);
			TablePanel.BringToFront ();
			//TablePanel.AutoSize = true;
			
			
			
			
			ToolTip Tipster = new ToolTip ();
			
			
		
			
			//			TablePanel.ColumnStyles[0].SizeType  = SizeType.Percent;;
			//			TablePanel.ColumnStyles[0].Width = 25;
			//
			//			TablePanel.ColumnStyles[1].SizeType  = SizeType.Percent;;
			//			TablePanel.ColumnStyles[1].Width = 75;
			foreach (ColumnStyle style in TablePanel.ColumnStyles) {
				NewMessage.Show (style.ToString());
				style.SizeType = SizeType.Percent;
				style.Width = 50;
			}
			
			
			richBox.BringToFront();
			
		}
		
	
		
		
		protected override void DoChildAppearance (AppearanceClass app)
		{
			base.DoChildAppearance (app);
			
			TablePanel.BackColor = app.mainBackground;
			
		}
		public override void Save ()
		{
			base.Save ();
			//CharacterColorInt = CharacterColor.ToArgb();
		}
		
		
		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{

			return Loc.Instance.GetString("not used in an AddIn");
		}
		
		
		
	}
}

