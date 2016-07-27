using System;
using System.Runtime.InteropServices;
using System.Data;
using System.Collections;
using Resources;
using System.Data.Odbc;
using System.IO;
using System.Web;
using System.Net.Mail;

namespace Keyholders
{
	/// <summary>
	/// clsImportData deals with everything to do with data about ImportDatas.
	/// </summary>
	
	[GuidAttribute("D6604DF1-F859-4ff2-A8DC-190A3A27A146")]
	public class clsImportData : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsImportData
		/// </summary>
		public clsImportData() : base("ImportData")
		{
		}

		/// <summary>
		/// Constructor for clsImportData; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsImportData(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("ImportData")
		{
			Connect(typeOfDb, odbcConnection);
		}


		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsImportData
		/// </summary>
		public override void Initialise()
		{

		}

		
		/// <summary>
		/// List of all Folders where Data is stored
		/// </summary>
		public string[] CampFolders = {"KDL"};

		/// <summary>
		/// File where data about how long each child record takes is stored
		/// </summary>
		public string timingFile = "Timing.csv";

		/// <summary>
		/// File where the last Record completely added is kept
		/// </summary>
		public string lastGoodRecordFile = "LGRefId.csv";

	
		/// <summary>
		/// Connect to Foreign Key classes within this class
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">An already open ODBC database connection</param>
		public override void ConnectToForeignClasses(
			clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection)
		{

			//Instances of Foreign Key Classes
			thisAddress = new clsAddress(thisDbType, localRecords.dbConnection);
			thisCity = new clsCity(thisDbType, localRecords.dbConnection);
			thisCountry = new clsCountry(thisDbType, localRecords.dbConnection);
			thisCustomer = new clsCustomer(thisDbType, localRecords.dbConnection);
			thisCustomerGroup = new clsCustomerGroup(thisDbType, localRecords.dbConnection);
			thisItem = new clsItem(thisDbType, localRecords.dbConnection);
			thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);
			thisOrderStatus = new clsOrderStatus(thisDbType, localRecords.dbConnection);
			thisPhoneNumber = new clsPhoneNumber(thisDbType, localRecords.dbConnection);
			thisPerson = new clsPerson(thisDbType, localRecords.dbConnection);
			thisPersonPremiseRole = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);
			thisPremise = new clsPremise(thisDbType, localRecords.dbConnection);
			thisProduct = new clsProduct(thisDbType, localRecords.dbConnection);
			thisProductCategory = new clsProductCategory(thisDbType, localRecords.dbConnection);
			thisProductInCategory = new clsProductInCategory(thisDbType, localRecords.dbConnection);
			thisProductCustomerGroupPrice = new clsProductCustomerGroupPrice(thisDbType, localRecords.dbConnection);
			thisServiceProvider = new clsServiceProvider(thisDbType, localRecords.dbConnection);
			thisSetting = new clsSetting(thisDbType, localRecords.dbConnection);
			thisServiceProviderPremiseRole = new clsSPPremiseRole(thisDbType, localRecords.dbConnection);
			thisShippingZone = new clsShippingZone(thisDbType, localRecords.dbConnection);
			thisTransaction = new clsTransaction(thisDbType, localRecords.dbConnection);
			thisUser = new clsUser(thisDbType, localRecords.dbConnection);
			thisUserDefinedText = new clsUserDefinedText(thisDbType, localRecords.dbConnection);

		}

		/// <summary>
		/// thisAddress
		/// </summary>
		public clsAddress thisAddress;

		/// <summary>
		/// thisCity
		/// </summary>
		public clsCity thisCity;

		/// <summary>
		/// thisCountry
		/// </summary>
		public clsCountry thisCountry;

		/// <summary>
		/// thisCustomer
		/// </summary>
		public clsCustomer thisCustomer;

		/// <summary>
		/// thisCustomerGroup
		/// </summary>
		public clsCustomerGroup thisCustomerGroup;
	
		/// <summary>
		/// thisItem
		/// </summary>
		public clsItem thisItem;

		/// <summary>
		/// thisOrder
		/// </summary>
		public clsOrder thisOrder;

		/// <summary>
		/// thisTransaction
		/// </summary>
		public clsTransaction thisTransaction;

		/// <summary>
		/// thisOrderStatus
		/// </summary>
		public clsOrderStatus thisOrderStatus;

		/// <summary>
		/// thisPhoneNumber
		/// </summary>
		public clsPhoneNumber thisPhoneNumber;

		/// <summary>
		/// thisPerson
		/// </summary>
		public clsPerson thisPerson;

		/// <summary>
		/// thisProduct
		/// </summary>
		public clsProduct thisProduct;

		/// <summary>
		/// thisProductCategory
		/// </summary>
		public clsProductCategory thisProductCategory;
		
		/// <summary>
		/// thisProductInCategory
		/// </summary>
		public clsProductInCategory thisProductInCategory;

		/// <summary>
		/// thisProductCustomerGroupPrice
		/// </summary>
		public clsProductCustomerGroupPrice thisProductCustomerGroupPrice;
		
		/// <summary>
		/// thisServiceProvider
		/// </summary>
		public clsServiceProvider thisServiceProvider;

		/// <summary>
		/// thisSetting
		/// </summary>
		public clsSetting thisSetting;

		/// <summary>
		/// thisPremise
		/// </summary>
		public clsPremise thisPremise;
		
		/// <summary>
		/// thisPersonPremiseRole
		/// </summary>
		public clsPersonPremiseRole thisPersonPremiseRole;

		/// <summary>
		/// thisServiceProviderPremiseRole
		/// </summary>
		public clsSPPremiseRole thisServiceProviderPremiseRole;

		/// <summary>
		/// thisShippingZone
		/// </summary>
		public clsShippingZone thisShippingZone;

		/// <summary>
		/// thisUser
		/// </summary>
		public clsUser thisUser;

		/// <summary>
		/// thisUserDefinedText
		/// </summary>
		public clsUserDefinedText thisUserDefinedText;


		/// <summary>
		/// Internal, Service Level to Assign Customers being imported
		/// </summary>
		public int thisProductId;

		# endregion

		#region enums

		/// <summary>
		/// This enumeration is used to mark field order in the lutType file
		/// </summary>
		public enum lutTypeFieldIndex : int
		{
			/// <summary>
			/// TypeId Column
			/// </summary>
			typeId = 0,
			/// <summary>
			/// TypeName Column
			/// </summary>
			typeName = 1
		}

		/// <summary>
		/// This enumeration is used to mark field order in the lutTypeAssignment file
		/// </summary>
		public enum lutTypeAssignmentFieldIndex : int
		{
			/// <summary>
			/// TableName Column
			/// </summary>
			tableName = 0,
			/// <summary>
			/// ColumnName Column
			/// </summary>
			columnName = 1,
			/// <summary>
			/// User Friendly ColumnName Column
			/// </summary>
			UFColumnName = 2,
			/// <summary>
			/// TypeId Column
			/// </summary>
			typeId = 3
		}


		/// <summary>
		/// This enumeration is used to mark field order in the lutValue file
		/// </summary>
		public enum lutValueFieldIndex : int
		{
			/// <summary>
			/// TypeId Column
			/// </summary>
			typeId = 0,
			/// <summary>
			/// SysValue Column
			/// </summary>
			SysValue = 1,
			/// <summary>
			/// User Friendly Value Column
			/// </summary>
			uFValue= 2
		}

		/// <summary>
		/// This enumeration is used to mark field order in the old DB file
		/// </summary>
		public enum oldDbFieldIndex : int
		{
			/// <summary>
			/// thisPremise Number.
			/// </summary>
			premiseNumber = 0,
			/// <summary>
			/// Refers to other numbers that are a part of the same customer
			/// </summary>
			kdlRef = 1,
			/// <summary>
			/// Add to clsPremise.KdlComments. Might be some kind of code
			/// </summary>
			subs = 2,
			/// <summary>
			/// When this thisCustomer first subscribed
			/// </summary>
			startDate = 3,
			/// <summary>
			/// When this thisCustomer should Renew their subs (Month) + a space
			/// + if they have or not
			/// </summary>
			renewDate = 4,
			/// <summary>
			/// Note; add to clsPremise.KdlComments
			/// </summary>
			note = 5,
			/// <summary>
			/// Note of an event that happened. Should figure in correspondence at some point
			/// </summary>
			diary = 6,
			/// <summary>
			/// clsCustomer.CompanyName
			/// </summary>
			businessName = 7,
			/// <summary>
			/// BuildingName
			/// </summary>
			buildingName = 8,
			/// <summary>
			/// Address number, Part1; part of clsPremise.physicalAddress_streetAddress
			/// </summary>
			phys_addressNumberP2 = 9,
			/// <summary>
			/// Address number, Part2; part of clsPremise.physicalAddress_streetAddress
			/// </summary>
			phys_addressNumberP1 = 10,
			/// <summary>
			/// Address Street; part of clsPremise.physicalAddress_streetAddress
			/// </summary>
			phys_addressStreet = 11,
			/// <summary>
			/// Address Suburb; part of clsPremise.physicalAddress_streetAddress
			/// </summary>
			phys_addressSuburb = 12,
			/// <summary>
			/// PO BOX Address; part of clsPerson.postalAddress_streetAddress
			/// </summary>
			post_addressPOBoxType = 13,
			/// <summary>
			/// thisCity Address; part of clsPremise.physicalAddress_City
			/// and clsPerson.postalAddress_City
			/// </summary>
			phys_addressCity = 14,
			/// <summary>
			/// thisPerson's DayTime Fax;
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// </summary>
			daytimeFax = 15,
			/// <summary>
			/// clsPerson.FirstName + space + clsPerson.LastName
			/// This thisPerson is the Daytime Contact / Billing Contact and Details Manager
			/// for this premise
			/// </summary>
			personName = 16,
			/// <summary>
			/// thisPerson's DayTime Phone;
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// o No Extension unless space after Main Number (MainNumber at least 6 digits long)
			/// </summary>
			daytimePhone = 17,
			/// <summary>
			/// thisPerson's Email;
			/// </summary>
			email = 18,		
			/// <summary>
			/// clsServiceProvider.CompanyName
			/// This thisServiceProvider is the Alarm Monitoring Company
			/// for this premise
			/// </summary>
			securityCompanyName = 19,
			/// <summary>
			/// Security Company's Daytime Phone; clsServiceProvider.DaytimePhone
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// o No Extension unless space after Main Number (MainNumber at least 6 digits long)
			/// </summary>
			securityCompanydaytimePhone = 20,
			/// <summary>
			/// Date that these thisCustomer Details were last updated; add to clsPremise.KdlComments
			/// </summary>
			dateLastDetailsUpdate = 21,		
			/// <summary>
			/// Date that this thisCustomer needs to be renewed; add to clsPremise.KdlComments
			/// </summary>
			dateOfRenewal= 22,
			/// <summary>
			/// Month that the thisCustomer needs to have these Details verified; add to clsPremise.KdlComments
			/// </summary>
			dateOfDetailVerification= 23,
			/// <summary>
			/// clsPerson.FirstName + space + clsPerson.LastName
			/// This thisPerson is the first Keyholder
			/// for this premise
			/// </summary>
			keyholder1Name = 24,
			/// <summary>
			/// thisPerson's After Hours Phone;
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// o No Extension unless space after Main Number (MainNumber at least 6 digits long)
			/// </summary>
			keyholder1afterHoursPhone = 25,
			/// <summary>
			/// thisPerson's mobilePhone Phone;
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// o No Extension unless space after Main Number (MainNumber at least 6 digits long)
			/// </summary>
			keyholder1MobilePhone = 26,
			/// <summary>
			/// clsPerson.FirstName + space + clsPerson.LastName
			/// This thisPerson is the first Keyholder
			/// for this premise
			/// </summary>
			keyholder2Name = 27,
			/// <summary>
			/// thisPerson's After Hours Phone;
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// o No Extension unless space after Main Number (MainNumber at least 6 digits long)
			/// </summary>
			keyholder2afterHoursPhone = 28,
			/// <summary>
			/// thisPerson's mobilePhone Phone;
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// o No Extension unless space after Main Number (MainNumber at least 6 digits long)
			/// </summary>
			keyholder2MobilePhone = 29,
			/// <summary>
			/// clsPerson.FirstName + space + clsPerson.LastName
			/// This thisPerson is the first Keyholder
			/// for this premise
			/// </summary>
			keyholder3Name = 30,
			/// <summary>
			/// thisPerson's After Hours Phone;
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// o No Extension unless space after Main Number (MainNumber at least 6 digits long)
			/// </summary>
			keyholder3afterHoursPhone = 31,
			/// <summary>
			/// thisPerson's mobilePhone Phone;
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// o No Extension unless space after Main Number (MainNumber at least 6 digits long)
			/// </summary>
			keyholder3MobilePhone = 32,
			/// <summary>
			/// clsPerson.FirstName + space + clsPerson.LastName
			/// This thisPerson is the first Keyholder
			/// for this premise
			/// </summary>
			keyholder4Name = 33,
			/// <summary>
			/// thisPerson's After Hours Phone;
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// o No Extension unless space after Main Number (MainNumber at least 6 digits long)
			/// </summary>
			keyholder4afterHoursPhone = 34,
			/// <summary>
			/// thisPerson's mobilePhone Phone;
			/// o Ignore any number that is less that 6 digits long
			/// o No InternationalPrefix
			/// o Before space is NationalOrMobilePrefix
			/// o Main Number is seperated with dashs (make this space seperated)
			/// o No Extension unless space after Main Number (MainNumber at least 6 digits long)
			/// </summary>
			keyholder4MobilePhone = 35,
			/// <summary>
			/// Further general clsPremise.CustomerComments
			/// </summary>
			furtherDetails = 36		
		}


		#endregion

		#region Structs
		/// <summary>
		/// Temp Strucuture that holds information about keyholders from the Old DB
		/// </summary>
		public struct kdlRefPropertyStruct
		{
			/// <summary>
			/// Sticker Number of Reference
			/// </summary>
			public string StickerNum;
			/// <summary>
			/// Id of Referenced Property
			/// </summary>
			public int PremiseId;
			/// <summary>
			/// Id of Referenced Property's Customer Group
			/// </summary>
			public int CustomerGroupId;
			/// <summary>
			/// Id of Referenced Property's Customer
			/// </summary>
			public int CustomerId;
			/// <summary>
			/// Customer fullname for Premise
			/// </summary>
			public string CustomerFullName;
		}

		/// <summary>
		/// Temp Strucuture that holds information about keyholders from the Old DB
		/// </summary>
		public struct keyHolderStruct
		{
			/// <summary>
			/// Keyholder ID (once Added to the new DB)
			/// </summary>
			public int Id;
			/// <summary>
			/// Keyholder Title
			/// </summary>
			public string Title;
			/// <summary>
			/// Keyholder FirstName
			/// </summary>
			public string FirstName;
			/// <summary>
			/// Keyholder LastName
			/// </summary>
			public string LastName;
			/// <summary>
			/// Comments from the Keyholder's Name
			/// </summary>
			public string NameComments;
			/// <summary>
			/// Keyholder's DaytimePhone
			/// </summary>
			public string DaytimePhone;
			/// <summary>
			/// Keyholder's DaytimeFax
			/// </summary>
			public string DaytimeFax;
			/// <summary>
			/// Keyholder's afterHoursPhone
			/// </summary>
			public string afterHoursPhone;
			/// <summary>
			/// Keyholder's afterHoursFax
			/// </summary>
			public string afterHoursFax;
			/// <summary>
			/// Keyholder's MobilePhone
			/// </summary>
			public string mobilePhone;
			/// <summary>
			/// Keyholder's Email
			/// </summary>
			public string Email;
		}
		
		#endregion

		#region Auxillary String Refining

		/// <summary>
		/// GetIndexLastNumeral
		/// </summary>
		/// <param name="original">original</param>
		/// <returns>Index of Last Numeral</returns>
		public int GetIndexLastNumeral(string original)
		{

			return GetIndexLastNumeral(original, false);

		}

		/// <summary>
		/// Gets the index of the last numeral in a string
		/// </summary>
		/// <param name="original">string form which to elimiate double spaces</param>
		/// <param name="consecutiveSinceStart">consecutiveSinceStart</param>
		/// <returns>string without double spaces</returns>
		public int GetIndexLastNumeral(string original, bool consecutiveSinceStart)
		{
			int lastIndex = -1;

			if (consecutiveSinceStart)
			{
				for(int counter = 0; counter < original.Length; counter++)
				{
					string thisString = original.Substring(counter, 1);
					char thisChar = thisString[0];
					int AsciiValue = (int) thisChar;

					if (AsciiValue < 45 || AsciiValue > 58)
					{
						return lastIndex;

					}

					lastIndex = counter;

				}

			}
			else
			{

				for(int counter = 0; counter < 10; counter++)
				{
					int thisIndex = original.LastIndexOf(counter.ToString());
					if (thisIndex > lastIndex)
						lastIndex = thisIndex;
				}
			}

			return lastIndex;
		}

		/// <summary>
		/// Gets the index of the last numeral in a string
		/// </summary>
		/// <param name="original">string form which to elimiate double spaces</param>
		/// <returns>string without double spaces</returns>
		public int GetIndexFirstNumeral(string original)
		{
			int firstIndex = original.Length + 1;

			for(int counter = 0; counter < 10; counter++)
			{
				int thisIndex = original.IndexOf(counter.ToString());
				if (thisIndex > -1 && thisIndex < firstIndex)
					firstIndex = thisIndex;
			}

			if (firstIndex > original.Length)
				firstIndex = -1;

			
			return firstIndex;
		}



		/// <summary>
		/// Gets rid of Starting or Trailing Hyphens
		/// </summary>
		/// <param name="original">string form which to elimiate double spaces</param>
		/// <param name="CharToGetRidOff">Character to get Rid of from start or end of string</param>
		/// <returns>string without double spaces</returns>
		public string GetRidOfStartingTrailingChars(string original, string CharToGetRidOff)
		{
			string newString = original;

			while (newString.Length > 0 && newString.Substring(0, 1) == CharToGetRidOff)
				newString = newString.Substring(1);
			
			
			int newStringLen = newString.Length;

			while (newString.Length > 0 && newString.Substring(newStringLen-1, 1) == CharToGetRidOff)
			{
				newString = newString.Substring(0, newStringLen-1);
				newStringLen = newString.Length;
			}

			return newString;
		}


		/// <summary>
		/// Gets rid of every instance of a 'Double Space' in a string
		/// </summary>
		/// <param name="original">string form which to elimiate double spaces</param>
		/// <returns>string without double spaces</returns>
		public string GetRidOfDoubleSpaces(string original)
		{
			string newString = original;
			
			int doubleSpace = newString.IndexOf("  ", 0);

			while (doubleSpace > 0)
			{
				newString = (newString.Substring(0, doubleSpace).Trim()
					+ " "
					+ newString.Substring(doubleSpace + 1).Trim()).Trim();

				doubleSpace = newString.IndexOf("  ", 0);
			}

			return newString;
		}

		/// <summary>
		/// 'Sorts out' Brackets and Parenthasese in a string
		/// </summary>
		/// <param name="original">string form which to 'Sort out' Brackets and Parenthasese</param>
		/// <returns>'Sorted out' string</returns>
		public string SortOutBrackets(string original)
		{
			string newString = original;
			
			newString = newString.Replace("[", "(");
			newString = newString.Replace("]", ")");
			newString = newString.Replace("{", "(");
			newString = newString.Replace("}", ")");


			//Ensuring that a space before opening brackets and after closing ones
			int nextBracket = newString.IndexOf("(", 0);
			int nextSpaceBracket = newString.IndexOf(" (", 0);
		
			while (nextBracket > 0)
			{
				if (nextSpaceBracket != nextBracket-1)
				newString = (newString.Substring(0, nextBracket).Trim()
					+ " "
					+ newString.Substring(nextBracket + 1).Trim()).Trim();

				nextBracket = newString.IndexOf("( ", nextBracket+1);
				if (nextBracket > 0)
					nextSpaceBracket = newString.IndexOf("( ", nextBracket-1);
			}


			int BracketSpace = newString.IndexOf("( ", 0);
			
			while (BracketSpace > 0)
			{
				newString = (newString.Substring(0, BracketSpace).Trim()
					+ "("
					+ newString.Substring(BracketSpace + 1).Trim()).Trim();

				BracketSpace = newString.IndexOf("( ", 0);
			}

			BracketSpace = newString.IndexOf(" )", 0);

			while (BracketSpace > 0)
			{
				newString = (newString.Substring(0, BracketSpace).Trim()
					+ ")"
					+ newString.Substring(BracketSpace + 1).Trim()).Trim();

				BracketSpace = newString.IndexOf(" )", 0);
			}

			while (newString.IndexOf("((", 0) > 0)
				newString = newString.Replace("((", "(");

			while (newString.IndexOf("))", 0) > 0)
				newString = newString.Replace("))", ")");


			return newString;
		}
		

		#endregion

		#region DeleteOldData
		
		/// <summary>
		/// Deletes data in preperation for a reimport of Children 
		/// </summary>
		public void DeleteLookupInformation()
		{

			//Cities and Countries
			localRecords.GetRecords(@"Delete from tblCity"); 
			localRecords.GetRecords(@"Delete from tblCountry"); 

			localRecords.GetRecords(@"Delete from tblfrszcgp"); 
			localRecords.GetRecords(@"Delete from tblfreightrule"); 
			localRecords.GetRecords(@"Delete from tblshippingzone"); 

			localRecords.GetRecords(@"Delete from tblcorrespondence"); 
			localRecords.GetRecords(@"Delete from tblcardtype"); 
			localRecords.GetRecords(@"Delete from lutdisplaytype"); 
			localRecords.GetRecords(@"Delete from tblnav"); 
			localRecords.GetRecords(@"Delete from tblpaymentmethodtype"); 
			localRecords.GetRecords(@"Delete from tblsetting"); 
			localRecords.GetRecords(@"Delete from tbluser"); 
			localRecords.GetRecords(@"Delete from tbluserdefinedtext"); 

		}



		/// <summary>
		/// Deletes data in preperation for a reimport of Children
		/// </summary>
		public void DeleteCustomerPremisePersonInformation()
		{

			//Shipping Rules, Orders and Transactions


			localRecords.GetRecords(@"Delete from tblcustomerpaymentmethodtype"); 



			localRecords.GetRecords(@"Delete from tblccattempt"); 
			localRecords.GetRecords(@"Delete from tbltransaction"); 
//			localRecords.GetRecords(@"Delete from tblitem"); 
			localRecords.GetRecords(@"Delete from tblorder"); 


			//Products
//			localRecords.GetRecords(@"Delete from tblproductcustomergroupprice"); 
			localRecords.GetRecords(@"Delete from tblproductincategory"); 
			localRecords.GetRecords(@"Delete from tblproductcategory"); 

			//Premises
			localRecords.GetRecords(@"Delete from tblAddress"); 
			localRecords.GetRecords(@"Delete from tblphonenumber"); 
			localRecords.GetRecords(@"Delete from tblhighriskgood"); 
			localRecords.GetRecords(@"Delete from tblsppremiserole"); 
			localRecords.GetRecords(@"Delete from tblserviceprovider"); 

			//People
			localRecords.GetRecords(@"Delete from tblpersonpremiserole"); 
			localRecords.GetRecords(@"Delete from tblperson"); 
			localRecords.GetRecords(@"Delete from tblpremise"); 


			localRecords.GetRecords(@"Delete from tblhighriskgood"); 

			
			//Customers
			localRecords.GetRecords(@"Delete from tblcustomer"); 
//			localRecords.GetRecords(@"Delete from tblcustomergroup"); 
			localRecords.GetRecords(@"Delete from tblcompanytype"); 
			

		}
		
		/// <summary>
		/// Deletes data in preperation for a reimport of Children
		/// </summary>
		public void DeleteOrdersItems()
		{

			//Correspondence
			localRecords.GetRecords(@"Delete from tblcorrespondencesendtocustomer"); 


		}

		#endregion
		
		#region Pre-Import Auxillary

		#region Insert System User

		/// <summary>
		/// Insert System User
		/// </summary>
		public void InsertSystemUser()
		{
			GetGeneralSettings();

			int totalNumUsers = thisUser.GetAll();

			if (systemUserId == 0 || totalNumUsers == 0)
			{
				localRecords.GetRecords(@"Insert into tblUser (UserName, Password, FirstName, LastName, Email, " + crLf
					+ @" DateLastLoggedIn, AccessLevel, ChangeDataId, Archive )" + crLf
					+ @" VALUES('System User', '', 'System', 'User', 'info@welman.co.nz', null, 0, null, 0)"); 

				thisSetting.Ensure("systemUserId", thisUser.GetSystemUserId().ToString());
			}
		}
		#endregion

		#region Insert Public CustomerGroup

		/// <summary>
		/// Insert System User
		/// </summary>
		public void InsertPublicCustomerGroup()
		{
			GetGeneralSettings();

			int totalNumCustomerGroups = thisCustomerGroup.GetAll();

			if (publicCustomerGroupId == 0 || totalNumCustomerGroups == 0)
			{
			
				localRecords.GetRecords(@"Insert into tblCustomerGroup (CustomerGroupName, CustomerGroupDescription)" + crLf
					+ @" VALUES('Public', 'Public Users', 'System', 'CustomerGroup', 'info@welman.co.nz', null, null, 0, null, 0)"); 

				thisSetting.Ensure("systemCustomerGroupId", localRecords.LastIdAdded.ToString());
			}
		}
		#endregion


		#endregion

		#region Pre-Import

		/// <summary>
		/// PreImport Stuff
		/// </summary>
		public void PreImport()
		{
			GetGeneralSettings();

			#region Deletes
			
			DeleteOrdersItems();
//			DeleteCustomerPremisePersonInformation();
//			DeleteLookupInformation();

			#endregion

			#region Users

			InsertSystemUser();

			GetGeneralSettings();

			thisUser.Add(0, "Mike", "Wells","Mike", "tmch33", "mike@welman.co.nz", 1, systemUserId);
			thisUser.Save();
			thisUser.Add(0, "Graham", "Mann","Greevis", "1fuckyou", "mike@welman.co.nz", 1, systemUserId);
			thisUser.Save();
			thisUser.Add(0, "Wel", "Man","Welman", "1fuckyou", "info@welman.co.nz", 1, systemUserId);
			thisUser.Save();
			thisUser.Add(0, "Jayne ", "Keogh", "jayne", "louis008", "info@kdl.co.nz", 1, systemUserId);
			thisUser.Save();

			#endregion

			#region ShippingZones
			
			thisShippingZone.Add("A", "Within New Zealand");
			thisShippingZone.Add("B", "Rest of World");
			thisShippingZone.Save();

			int thisShippingZoneId = thisShippingZone.LastIdAdded();

			#endregion

			#region Countrys / Citys

			//Insert New Zealand as a Country if not present
			
			int numCountries = thisCountry.GetByCountryId(assumedCountryId);
			
			if (assumedCountryId == 0 || numCountries == 0)
			{
				numCountries = thisCountry.GetByCountryName("New Zealand", matchCriteria_exactMatch());

				if (numCountries > 0)
					assumedCountryId = thisCountry.my_CountryId(0);
				else
				{
					thisCountry.Add(thisShippingZoneId, "New Zealand", 1, 1);
					thisCountry.Save();
					assumedCountryId = thisCountry.LastIdAdded();
				}
			}

			thisSetting.Ensure("assumedCountryId", assumedCountryId.ToString());

			//Insert Non-City City if not present
			if (nonCityCityId == 0)
			{
				thisCity.Add(assumedCountryId, "None","", 0);
				thisCity.Save();
				nonCityCityId = thisCity.LastIdAdded();
			}

			thisSetting.Ensure("nonCityCityId", nonCityCityId.ToString());

			#endregion

			#region Products

//			thisProduct.Add(1,"General Service","KDL1","Premise(s)",2,3,0,0,0,0,5,10,10,10,10,10,1,1,"Includes Stickers\n","KDL Default Product",0,0,0,"","KDL Default Product Description", "", "", "");
//			thisProduct.Save();
//			thisProductId = thisProduct.LastIdAdded();
			thisProductId = 1;

			#endregion

			#region Product Category / Product In Category

			thisProductCategory.Add("Premise Protection","Premise Protection",2,1,1,8,"Title",2,0,0, "", "Description", 0, 0, 
				"", "", "Caption");
			thisProductCategory.Save();

			int thisProductCategoryId = thisProductCategory.LastIdAdded();

			thisProductInCategory.Add(thisProductId, thisProductCategoryId, 1);
			thisProductInCategory.Save();

			#endregion

			#region Customer Groups / ProductCustomerGroupPrice

			thisCustomerGroup.Add("Public",  "Public Users", 0, systemUserId);
			thisCustomerGroup.Save();

			publicCustomerGroupId = thisCustomerGroup.LastIdAdded();

			thisSetting.Ensure("publicCustomerGroupId", publicCustomerGroupId.ToString());

//			thisProductCustomerGroupPrice.Add(thisProductId, publicCustomerGroupId, Convert.ToDecimal(32.00));
//			thisProductCustomerGroupPrice.Save();

			thisCustomerGroup.Add("Half Price Discount", "Half Price Discount for Charities and Friends", 0, systemUserId);
			thisCustomerGroup.Save();

			int thisCustomerGroupId = thisCustomerGroup.LastIdAdded();

//			thisProductCustomerGroupPrice.Add(thisProductId, thisCustomerGroupId, Convert.ToDecimal(16.00));
//			thisProductCustomerGroupPrice.Save();

			thisCustomerGroup.Add("Full Discount", "Full Discount for really nice Charities and really good Friends", 0, systemUserId);
			thisCustomerGroup.Save();

			thisCustomerGroupId = thisCustomerGroup.LastIdAdded();

//			thisProductCustomerGroupPrice.Add(thisProductId, thisCustomerGroupId, Convert.ToDecimal(0.00));
//			thisProductCustomerGroupPrice.Save();


			#endregion

			#region OrderStatuses (Already added by Script)
			
//			thisOrderStatus.Add("Unprocessed", "", "Not yet processed", "");
//			thisOrderStatus.Add("Processed", "", "Your order has been processed.", "");
//			thisOrderStatus.Save();

			#endregion

			#region UserDefinedText

			thisUserDefinedText.Add(@"CMSContent_1", @"\n<h1>Contact Us</h1>\n\n\n");
			thisUserDefinedText.Add(@"CMSContent_10000", @"<h1>BANANA AD HERE<br /></h1>\n\n");
			thisUserDefinedText.Add(@"CMSContent_10001", @"");
			thisUserDefinedText.Add(@"CMSContent_10002", @"");
			thisUserDefinedText.Add(@"CMSContent_10003", @"<h1>New to Keyholders?</h1><br />Create your free user account below to shop online!<br /><br />\n\n");
			thisUserDefinedText.Add(@"CMSContent_10004", @"<h1>Pay Invoices Online</h1><br />Welcome to Keyholders Limited's online credit card payment facility. Please use this service only if you have received a formal invoice from us, and wish to settle it using your credit card.<br /><br />Please read our terms and conditions of use of this service before proceeding.</span><br />\n\n");
			thisUserDefinedText.Save();


			#endregion

			#region Other Settings



			thisSetting.Ensure("assumedShippingZoneId", @"1");
			thisSetting.Ensure("BaseOrderNumber", @"100000");
			thisSetting.Ensure("ClientTimeZoneOsIndex", @"290");
			thisSetting.Ensure("ClientTimeZoneRegKey", @"New Zealand Standard Time");
			thisSetting.Ensure("CompanyNameFooter", @"Keyholders Ltd");
			thisSetting.Ensure("CurrencyName", @"New Zealand Dollars");
			thisSetting.Ensure("CurrencySymbolShort", @"$");
			thisSetting.Ensure("CurrencySymbolLong", @"NZD");
			thisSetting.Ensure("defCreditLimit_NewCustomers", @"0");
			thisSetting.Ensure("defOpeningBal_NewCustomers", @"0");
			thisSetting.Ensure("DefCustGroup_Inv", @"1");
			thisSetting.Ensure("DefCustGroup_Cart", @"1");
			thisSetting.Ensure("DefOrdStatus_Inv", @"1");
			thisSetting.Ensure("DefOrdStatus_Cart", @"1");
			thisSetting.Ensure("EnableGenericPaymentsInterface", @"1");
			thisSetting.Ensure("FreightChargeBasis", ((int) freightChargeType.singleChargePerItem).ToString().Trim());
			thisSetting.Ensure("GenericPaymentsLinkTitle", @"Pay Invoices Online");
			thisSetting.Ensure("GenericPaymentsLineDesc", @"Invoice");
			thisSetting.Ensure("GenericPaymentsLinkTitle", @"0");
			thisSetting.Ensure("GenericPaymentsLineDesc", @"");
			thisSetting.Ensure("HomePageFeatureCategoryId", @"1");
			thisSetting.Ensure("IncludeHeadersOnGenericPayments", @"1");
			thisSetting.Ensure("ItemCodeInvoicePayments", @"INVPMT");
			thisSetting.Ensure("LocalTaxName", @"GST");
			thisSetting.Ensure("MainSiteUrl", @"www.alertus.co.nz");
			thisSetting.Ensure("MinimumFreightCharge", @"0");
			thisSetting.Ensure("MaximumFreightCharge", @"10");
			thisSetting.Ensure("NextOrderNumber", 50000.ToString());
			thisSetting.Ensure("NextStickerNumber", @"30000");
			thisSetting.Ensure("OnpInvoices", @"IP");
			thisSetting.Ensure("OnpOrders", @"SC");
			thisSetting.Ensure("ReceiptHeader", @"Keyholders Ltd");
			thisSetting.Ensure("SiteTitle", @"AlertUs Extranet");
			thisSetting.Ensure("thisOutputPath", @"pdfOutput\");
			thisSetting.Ensure("thisRootPath", @"C:\www\kdl.dev2.welman.co.nz\");
			thisSetting.Ensure("thisReportPath", @"C:\www\Resources\ReportTemplates\");
			thisSetting.Ensure("UseCustomNav", @"0");

//			thisSetting.Ensure("LocalTaxPerCent", @"12.5");


			#endregion
		}

		#endregion

		#region RestartImportFromLastRecord


		/// <summary>
		/// RestartImportFromLastRecord
		/// </summary>
		/// <param name="CsvFileFolder">CsvFileFolder</param>
		/// <param name="fileToImport">fileToImport</param>
		/// <param name="RecordToStartAt">RecordToStartAt</param>
		public void RestartImportFromLastRecord(string CsvFileFolder, string fileToImport, int RecordToStartAt)
		{
			string thisLastGoodRecordPathFile = CsvFileFolder + CampFolders[0] + lastGoodRecordFile;
			
			int lastGoodRecord = 0;
			int nextCsvFileEntryIndexToAdd = 0;
			bool finishedAddingReferrals = false;

			// Get last Good Referral Id, if it exists lastGoodRecordFile
			if (System.IO.File.Exists(thisLastGoodRecordPathFile))
			{
				clsCsvReader fileReader = new clsCsvReader(thisLastGoodRecordPathFile);

				string[] thisLine = fileReader.GetCsvLine();
				lastGoodRecord = Convert.ToInt32(thisLine[0]);
				nextCsvFileEntryIndexToAdd = Convert.ToInt32(thisLine[1]) + 1;
				if (nextCsvFileEntryIndexToAdd == Convert.ToInt32(thisLine[2]))
					finishedAddingReferrals = true;

				fileReader.Dispose();
			}

			if (!finishedAddingReferrals)
				ImportFromCsv(CsvFileFolder, fileToImport, nextCsvFileEntryIndexToAdd);

		}

		#endregion

		#region Main Import
		
		/// <summary>
		/// Imports data from a CSV file and adds this to the database
		/// </summary>
		/// <param name="CsvFileFolder">Folder from which to Import Data</param>
		/// <param name="fileToImport">File from which to Import Data</param>
		/// <param name="RecordToStartAt">Record in file from which to commence Import</param>
		public void ImportFromCsv(string CsvFileFolder, string fileToImport, int RecordToStartAt)
		{

			#region Initialisation
			
			string thisTimingPathFile = CsvFileFolder + CampFolders[0] + timingFile;
			string thisLastGoodRecordPathFile = CsvFileFolder + CampFolders[0] + lastGoodRecordFile;
			string thisDebugFile = CsvFileFolder + CampFolders[0] + "Debug.csv";

			#region debug
			clsCsvWriter debugFileWriter = new clsCsvWriter(thisDebugFile, true);
			debugFileWriter.WriteFields(new
				object[] { 
							 DateTime.Now.ToString(),
							 "ImportFromCsv Start",
							 CsvFileFolder,
							 fileToImport,
							 RecordToStartAt.ToString(), 
						 });
			debugFileWriter.Close();
			#endregion

			if (RecordToStartAt == 0 && System.IO.File.Exists(thisTimingPathFile))
				System.IO.File.Delete(thisTimingPathFile);

			if (RecordToStartAt == 0 && System.IO.File.Exists(thisLastGoodRecordPathFile))
				System.IO.File.Delete(thisLastGoodRecordPathFile);
			
			DateTime startTime = DateTime.Now;
			clsCsvReader fileReader = new clsCsvReader(fileToImport);

			#region Sort out ProperCaser
			clsProperCaser toProper = new clsProperCaser(true);
			string[] FixedCaseWords = toProper.FixedCaseWords;

			int numOldFixedCaseWords = FixedCaseWords.GetUpperBound(0) + 1;

			string[] newFixedCaseWords = new string[numOldFixedCaseWords + 19];
			for(int counter = 0; counter < numOldFixedCaseWords; counter++)
				newFixedCaseWords[counter] = FixedCaseWords[counter];

			newFixedCaseWords[numOldFixedCaseWords] = "OEM";
			newFixedCaseWords[numOldFixedCaseWords + 1] = "TSB";
			newFixedCaseWords[numOldFixedCaseWords + 2] = "PPG";
			newFixedCaseWords[numOldFixedCaseWords + 3] = "SSP";
			newFixedCaseWords[numOldFixedCaseWords + 4] = "WG";
			newFixedCaseWords[numOldFixedCaseWords + 5] = "SIM";
			newFixedCaseWords[numOldFixedCaseWords + 6] = "IRS";
			newFixedCaseWords[numOldFixedCaseWords + 7] = "UFS";
			newFixedCaseWords[numOldFixedCaseWords + 8] = "UFL";
			newFixedCaseWords[numOldFixedCaseWords + 9] = "HRB";
			newFixedCaseWords[numOldFixedCaseWords + 10] = "DH";
			newFixedCaseWords[numOldFixedCaseWords + 11] = "PKF";
			newFixedCaseWords[numOldFixedCaseWords + 12] = "on";
			newFixedCaseWords[numOldFixedCaseWords + 13] = "NRM";
			newFixedCaseWords[numOldFixedCaseWords + 14] = "CLM";
			newFixedCaseWords[numOldFixedCaseWords + 15] = "RMI";
			newFixedCaseWords[numOldFixedCaseWords + 16] = "EMA";
			newFixedCaseWords[numOldFixedCaseWords + 17] = "Machine";
			newFixedCaseWords[numOldFixedCaseWords + 18] = "Machines";

			toProper.FixedCaseWords = newFixedCaseWords;
			#endregion





			#endregion

			int numRecords = 0;
			string[] thisLine = fileReader.GetCsvLine();

			while (thisLine != null && thisLine.Length > 1) //For each Db Record before where we start
			{
				numRecords ++;
				thisLine = fileReader.GetCsvLine();
			}

			fileReader.Dispose();
			
			fileReader = new clsCsvReader(fileToImport);

			for (int counter = 0; counter < RecordToStartAt; counter++)
				fileReader.GetCsvLine();

			thisLine = fileReader.GetCsvLine();

			for(int thisRecord = RecordToStartAt; thisRecord < numRecords; thisRecord++)  //For each Db Record
			{

				if (thisLine[(int) oldDbFieldIndex.businessName].Trim().ToUpper() != "VOID")
//					&& thisLine[(int) oldDbFieldIndex.businessName].Trim() != ""
//					) //Ignore the VOIDs/Empties
				{
				#region Check for one of the phantom not imported records


					string PremiseNumber = thisLine[(int) oldDbFieldIndex.premiseNumber].Trim();

					bool isMaverickRecord = false;

					if (PremiseNumber == "10726"
//						|| PremiseNumber == "10125"
//						|| PremiseNumber == "10300"
//						|| PremiseNumber == "10331"
//						|| PremiseNumber == "10427"
//						|| PremiseNumber == "10610"
//						|| PremiseNumber == "10824"
//						|| PremiseNumber == "10905"
//						|| PremiseNumber == "11005"
//						|| PremiseNumber == "11099"
//						|| PremiseNumber == "11112"
//						|| PremiseNumber == "11136"
//						|| PremiseNumber == "11246"
//						|| PremiseNumber == "11271"
//						|| PremiseNumber == "11343"
//						|| PremiseNumber == "11345"
//						|| PremiseNumber == "11403"
//						|| PremiseNumber == "11420"
//						|| PremiseNumber == "11444"
//						|| PremiseNumber == "11505"
//						|| PremiseNumber == "11511"
//						|| PremiseNumber == "11525"
//						|| PremiseNumber == "11533"
//						|| PremiseNumber == "11536"
//						|| PremiseNumber == "11541"
						)
					{
						isMaverickRecord = true;
						
					}

				#endregion

					DateTime startOfRecordTime = DateTime.Now;

				if (isMaverickRecord)
					startOfRecordTime.AddDays(1);

					#region debug
					debugFileWriter = new clsCsvWriter(thisDebugFile, true);
					debugFileWriter.WriteFields(new
						object[] { 
									 DateTime.Now.ToString(),
									 "ImportFromCsv Progress: Pre-Recieve Data",
									 CsvFileFolder,
									 fileToImport,
									 RecordToStartAt.ToString(),
									 thisRecord.ToString(),
									 numRecords.ToString()
 								 });
					debugFileWriter.Close();
					#endregion

					#region recieve data
					
					//Make each field 'Proper Case' and Trimed
					for (int fieldCounter = 0; fieldCounter < thisLine.GetUpperBound(0) + 1;
						fieldCounter++)
					{
						thisLine[fieldCounter] = toProper.ProperCase(thisLine[fieldCounter].Trim());
						if (thisLine[fieldCounter].ToString() == "-")
							thisLine[fieldCounter] = "";
						thisLine[fieldCounter] = SortOutBrackets(thisLine[fieldCounter]);
						thisLine[fieldCounter] = thisLine[fieldCounter].Replace("'S ", "'s ");
						thisLine[fieldCounter] = thisLine[fieldCounter].Replace("T/a", "T/A");
						thisLine[fieldCounter] = thisLine[fieldCounter].Replace("macHine", "Machine");
					}
					#endregion

					#region Get Comments

					string KdlComments = "";

					if (thisLine[(int) oldDbFieldIndex.note].Length > 0)
						KdlComments += "Note: " + thisLine[(int) oldDbFieldIndex.note] + crLf;

					if (thisLine[(int) oldDbFieldIndex.diary].Length > 0)
						KdlComments += "Diary: " + thisLine[(int) oldDbFieldIndex.diary] + crLf;
					
					if (thisLine[(int) oldDbFieldIndex.dateLastDetailsUpdate].Length > 0)
						KdlComments += "Date of last Actual Details Updated: " + thisLine[(int) oldDbFieldIndex.dateLastDetailsUpdate] + crLf;

					if (thisLine[(int) oldDbFieldIndex.dateOfRenewal].Length > 0)
						KdlComments += "Date of Renewal: " + thisLine[(int) oldDbFieldIndex.dateOfRenewal] + crLf;
					
					if (thisLine[(int) oldDbFieldIndex.subs].Length > 0)
						KdlComments += "Subs: " + thisLine[(int) oldDbFieldIndex.subs] + crLf;

					if (thisLine[(int) oldDbFieldIndex.dateOfDetailVerification].Length > 0)
						KdlComments += "Last Date Detail verification was sent out: " + thisLine[(int) oldDbFieldIndex.dateOfDetailVerification] + crLf;
					

					string CustomerComments = "";

					if (thisLine[(int) oldDbFieldIndex.furtherDetails].Length > 0)
						CustomerComments += "Further Details: " + thisLine[(int) oldDbFieldIndex.furtherDetails] + crLf;

					#endregion

					#region DateSubscriptionExpires

					string DateNextSubscriptionDueToBeInvoiced = thisLine[(int) oldDbFieldIndex.dateOfRenewal].Trim();
					string DateSubscriptionExpires = "";
					string DateLastInvoice = "";
					DateTime thisNextSub;

					try
					{
						thisNextSub = Convert.ToDateTime(DateNextSubscriptionDueToBeInvoiced);

//						thisNextSub = thisNextSub.AddMonths(2).AddDays(-1);
					}
					catch (Exception)
					{
						thisNextSub = DateTime.Now.AddYears(100);
//						DateNextSubscriptionDueToBeInvoiced = localRecords.DBDateTime(thisNextSub);
//						thisNextSub = thisNextSub.AddMonths(2).AddDays(-1);
//						DateSubscriptionExpires = localRecords.DBDateTime(thisNextSub);
					}


					//Set Day to first
					int thisDay = thisNextSub.Date.Day;
					if (thisDay != 1)
						thisNextSub = thisNextSub.AddDays(thisDay * -1 + 1);

					// Set Hour to 0
					int thisHour = thisNextSub.Date.Hour;
					if (thisHour != 0)
						thisNextSub = thisNextSub.AddHours(thisHour * -1);

					// Set Minutes to 0
					int thisMinute = thisNextSub.Date.Minute;
					if (thisMinute != 0)
						thisNextSub = thisNextSub.AddMinutes(thisMinute * -1);

					// Set Seconds to 0
					int thisSecond= thisNextSub.Date.Second;
					if (thisSecond != 0)
						thisNextSub = thisNextSub.AddSeconds(thisSecond * -1);

					//Set the DateSubscriptionExpires
					DateSubscriptionExpires = localRecords.DBDateTime(thisNextSub);

					DateLastInvoice = GetSensibleDate(localRecords.DBDateTime(thisNextSub.AddYears(-1)),1);

					//Set the DateNextSubscriptionDueToBeInvoiced
					thisNextSub = thisNextSub.AddMonths(-1);
					DateNextSubscriptionDueToBeInvoiced = localRecords.DBDateTime(thisNextSub);



					#endregion

					#region DateLastDetailsUpdate / DateStart

//					string DateNextDetailsUpdate = GetSensibleDate(thisLine[(int) oldDbFieldIndex.dateOfDetailVerification].ToLower().Trim(), -1);
//
//					DateTime dDateOfLastDetailsUpdate = Convert.ToDateTime(DateNextDetailsUpdate);

					string DateOfLastDetailsUpdate = GetSensibleDate(thisLine[(int) oldDbFieldIndex.dateOfDetailVerification].ToLower().Trim(), 1);
					//DateOfLastDetailsUpdate is definitely in the last 12 months
					
					DateTime dDateOfLastDetailsUpdate = Convert.ToDateTime(DateOfLastDetailsUpdate);

					if(dDateOfLastDetailsUpdate < Convert.ToDateTime(DateLastInvoice))
					{
						dDateOfLastDetailsUpdate = Convert.ToDateTime(DateLastInvoice);
						DateOfLastDetailsUpdate = localRecords.DBDateTime(dDateOfLastDetailsUpdate);

					}
					//Should definitely now be in the last 6 months.

					while(dDateOfLastDetailsUpdate.AddMonths(6) < DateTime.Now)
					{
						dDateOfLastDetailsUpdate = dDateOfLastDetailsUpdate.AddMonths(6);
						DateOfLastDetailsUpdate = localRecords.DBDateTime(dDateOfLastDetailsUpdate);
					}



					string DateStart = GetSensibleDate(thisLine[(int) oldDbFieldIndex.startDate].ToLower().Trim());

					#endregion

					#region Premise/Person Address

					string PremiseBuildingName = "";
					string PremiseStreetUnitNumber = "";
					string PremiseStreetNumber = "";
					string PremiseStreetAddress = "";
					string PremiseSuburb = "";
					string POBoxTypeAddress = "";
					poBoxType thisPOBoxType = poBoxType.normal;
					string POBoxNumber = "";
					string PersonBuildingName = "";
					string PersonStreetUnitNumber = "";
					string PersonStreetNumber = "";
					string PersonStreetAddress = "";
					string PersonSuburb = "";
					string POBoxSuburb = "";
					string POBoxCityName = "";
					string PersonCityName = "";
					string PremiseCityName = "";
					string PremisePostCode = "";
					string PersonPostCode = "";
					int PremiseCityId = 0;
					string StateName = "";

					GetGeneralSettings();
					int CountryId = assumedCountryId;
					int DefaultCustomerGroupId = 1;

					PremiseBuildingName = thisLine[(int) oldDbFieldIndex.buildingName].Trim() + crLf;

					//Remove starting and trailing dashes
					PremiseStreetUnitNumber = GetRidOfStartingTrailingChars(thisLine[(int) oldDbFieldIndex.phys_addressNumberP1], "-");
					PremiseStreetNumber = GetRidOfStartingTrailingChars(thisLine[(int) oldDbFieldIndex.phys_addressNumberP2], "-");

					PremiseStreetAddress = thisLine[(int) oldDbFieldIndex.phys_addressStreet];
					PremiseSuburb = thisLine[(int) oldDbFieldIndex.phys_addressSuburb];

					#region City and Post Code
					//Let us see if we can find the PremiseCityName
					PremiseCityName = thisLine[(int) oldDbFieldIndex.phys_addressCity];

					int earliestNumber = PremiseCityName.Length + 1;
					//Firstly, if there are any numbers, these are the PremisePostCode
					for (int iCounter = 0; iCounter < 10; iCounter++)
					{
						int thisIndex = PremiseCityName.IndexOf(iCounter.ToString());
						if (thisIndex > -1 && thisIndex < earliestNumber)
							earliestNumber = thisIndex;
					}

					if (earliestNumber < PremiseCityName.Length + 1)
					{
						PersonPostCode = PremiseCityName.Substring(earliestNumber).Trim();
						if (earliestNumber != 0)
							PremiseCityName = PremiseCityName.Substring(0,earliestNumber - 1).Trim();
					}

					//Now deal with trailing 'PremiseCityName'
					int thisTrailingCity = PremiseCityName.IndexOf("City");

					if (thisTrailingCity > -1)
						PremiseCityName = PremiseCityName.Substring(0, thisTrailingCity - 1).Trim();

					//Now deal with space before comments
					int thisSpaces = PremiseCityName.IndexOf("  ");

					if (thisSpaces > -1)
					{
						KdlComments += PremiseCityName.Substring(thisSpaces).Trim(); 
						PremiseCityName = PremiseCityName.Substring(0, thisSpaces - 1).Trim();
					}

					if (PremiseCityName.EndsWith("."))
						PremiseCityName = PremiseCityName.Substring(0,PremiseCityName.Length - 1).Trim();


					//Try to find a match for this PremiseCityName

					int thisMatch = thisCity.GetByCityNameCountryId(PremiseCityName,matchCriteria_contains(), CountryId);
					
					if (thisMatch > 0)
					{
						PremiseCityName = thisCity.my_CityName(0);
						PremiseCityId = thisCity.my_CityId(0);
					}

					#endregion

					#region POBox / Private Bag
					if (thisLine[(int) oldDbFieldIndex.post_addressPOBoxType].Length > 0)
						POBoxTypeAddress += thisLine[(int) oldDbFieldIndex.post_addressPOBoxType].Trim() + crLf;


					int POBoxIndex = POBoxTypeAddress.ToLower().IndexOf("po box"); 
					if (POBoxIndex > -1)
					{
						thisPOBoxType = poBoxType.poBox;
						//POBoxTypeAddress = POBoxTypeAddress.Substring(POBoxIndex + 6).Trim();
//						string POBoxTypeAddressSuburb = POBoxTypeAddress.Substring(POBoxIndex + 6).Trim();
						//If there is anything written after a number, that is a suburb

						POBoxTypeAddress = POBoxTypeAddress.Substring(POBoxIndex + ("po box").Length).Trim();
						int lastNumericalDigit = GetIndexLastNumeral(POBoxTypeAddress, true);
						if (lastNumericalDigit > - 1)
						{
							POBoxNumber = POBoxTypeAddress.Substring(0, lastNumericalDigit + 1).Trim();
							POBoxSuburb = POBoxTypeAddress.Substring(lastNumericalDigit + 1).Trim();
						}
						else
						{
							POBoxNumber = "";
							POBoxSuburb = POBoxTypeAddress.Trim();
						}

						//Let us see if there is a PostCode in the POBOX Suburb
						int firstNumber = GetIndexFirstNumeral(POBoxSuburb);

						if (firstNumber > -1)
						{
							int lastNumber = GetIndexLastNumeral(POBoxSuburb);
							if (lastNumber > -1)
							{
								string newPostCode = POBoxSuburb.Substring(firstNumber, lastNumber - firstNumber + 1).Trim();
								if (newPostCode != "")
								{
									PersonPostCode = newPostCode;
									PremisePostCode = PersonPostCode;
									if (firstNumber == 0)
										firstNumber = 1;
									POBoxSuburb = POBoxSuburb.Substring(0, firstNumber - 1).Trim();
								}

							}
						}


						POBoxCityName = PremiseCityName;
					}

					int PrivateBagIndex = POBoxTypeAddress.ToLower().IndexOf("private bag"); 
					if (PrivateBagIndex > 0)
					{
						thisPOBoxType = poBoxType.privateBag;
						POBoxTypeAddress = POBoxTypeAddress.Substring(PrivateBagIndex + ("private bag").Length).Trim();
						//If there is anything written after a number, that is a suburb
						int lastNumericalDigit = GetIndexLastNumeral(POBoxTypeAddress, true);
						if (lastNumericalDigit > - 1)
						{
							POBoxNumber = POBoxTypeAddress.Substring(0, lastNumericalDigit + 1).Trim();
							POBoxSuburb = POBoxTypeAddress.Substring(lastNumericalDigit + 1).Trim();
						}
						else
						{
							POBoxNumber = "";
							POBoxSuburb = POBoxTypeAddress.Trim();
						}

						//Let us see if there is a PostCode in the POBOX Suburb
						int firstNumber = GetIndexFirstNumeral(POBoxSuburb);

						if (firstNumber > -1)
						{
							int lastNumber = GetIndexLastNumeral(POBoxSuburb);
							if (lastNumber > -1)
							{
								string newPostCode = POBoxSuburb.Substring(firstNumber, lastNumber - firstNumber + 1).Trim();
								if (newPostCode != "")
								{
									PersonPostCode = newPostCode;
									PremisePostCode = PersonPostCode;
									POBoxSuburb = POBoxSuburb.Substring(0, firstNumber - 1).Trim();
								}

							}
						}

						POBoxCityName = PremiseCityName;
					}

					POBoxNumber = POBoxNumber.Replace("o", "0");
					POBoxNumber = GetRidOfDoubleSpaces(POBoxNumber).Trim();
					POBoxNumber = POBoxNumber.Replace(" ", "-");
					#endregion

					#region Non PO Box/ Private Bag
					if (thisPOBoxType == poBoxType.normal)
					{
						PersonBuildingName = PremiseBuildingName;
						PersonStreetUnitNumber = PremiseStreetUnitNumber;
						PersonStreetNumber = PremiseStreetNumber;
						PersonStreetAddress = PremiseStreetAddress;
						PersonSuburb = PremiseSuburb;
						PersonCityName = PremiseCityName;
						PersonCityName = PremiseCityName;
					}
					else
					{
						PersonBuildingName = "";
						PersonStreetUnitNumber = PremiseStreetUnitNumber;
						PersonStreetNumber = PremiseStreetNumber;
						PersonStreetAddress = PremiseStreetAddress;
						PersonSuburb = PremiseSuburb;
						PersonCityName = PremiseCityName;
						PersonCityName = PremiseCityName;
					}


					#endregion

					#endregion

					#region Add Customer and Premise

					int thisCustomerId = 0;
					int thisPremiseId = 0;
					int StickerRequired = 0;

					string BusinessName = GetRidOfDoubleSpaces(thisLine[(int) oldDbFieldIndex.businessName].Trim());
					string KDLRef = thisLine[(int) oldDbFieldIndex.kdlRef];
					string thisUrl = "";
					string thisAlarmDetails = "";
					string thisCustomerFirstName = "";
					string thisCustomerLastName = "";
					string thisTypeOfCompanyAtPremise = "";

					int thisCustomerGroupId = DefaultCustomerGroupId;

					#region Customer Group Price
					if (thisLine[(int) oldDbFieldIndex.subs].Length > 0)
					{
						string ThisSubs = thisLine[(int) oldDbFieldIndex.subs].Trim().ToUpper();

						if (ThisSubs == "G")
							DefaultCustomerGroupId = 2;
					
						if (ThisSubs == "NC")
							DefaultCustomerGroupId = 3;

					}
					#endregion

					#region Customer Group / Customer
					//See if the premise 'refered to' is in the DB
					ArrayList RefNumsThisPremise = new ArrayList();
//					ArrayList RefNumsThisPremise = GetReferencedProperties(KDLRef);
					string thisCustomerFullName = "";
					

					//See if the thisCustomer already exists
					string thisName = BusinessName;

					if (thisName == "")
						thisName = (thisCustomerFirstName + " " + thisCustomerLastName).Trim();
					
					int numResults = thisCustomer.GetByName(thisName);
					thisCustomerGroupId = 0;

					for(int counter = 0; counter < numResults; counter++)
					{
						if (thisCustomer.my_FirstName(counter) == thisCustomerFirstName &&
							thisCustomer.my_LastName(counter) == thisCustomerLastName &&
							thisCustomer.my_CompanyName(counter) == BusinessName)
						{
							thisCustomerId = thisCustomer.my_CustomerId(counter);
							thisCustomerGroupId = thisCustomer.my_CustomerGroupId(counter);
							thisCustomerFullName = thisCustomer.my_CompanyName(counter);  

							int numPremise = thisPremise.GetByCustomerId(thisCustomerId);
							for(int premiseCounter = 0; premiseCounter < numPremise; premiseCounter++)
							{
								kdlRefPropertyStruct newAddition = new kdlRefPropertyStruct();

								newAddition.CustomerId = thisCustomerId;
								newAddition.CustomerGroupId = thisCustomerGroupId;
								newAddition.CustomerFullName = thisCustomer.my_FullName(counter);
								newAddition.PremiseId = thisPremise.my_PremiseId(premiseCounter);
								RefNumsThisPremise.Add(newAddition);

							}

						}
					}


					//Add to this Arraylist any customers with exactly the same name previously found


					//There could be multiple properties being referred too; let us create a list of them
					#region Previously Used for Refered Properties

//					if (RefNumsThisPremise.Count > 0)
//					{
//						//Lets see if we can find a valid Customergroup among the references 
//						foreach(kdlRefPropertyStruct thisReferral in RefNumsThisPremise)
//						{
//							if (thisReferral.CustomerGroupId > 3)
//							{
//								thisCustomerId = thisReferral.CustomerId;
//								thisCustomerGroupId = thisReferral.CustomerGroupId;
//								thisCustomerFullName = thisReferral.CustomerFullName; 
//							}
//						}
//						
//						if (thisCustomerGroupId < 4)
//						{
//							thisCustomerGroup.Add(
//								thisCustomerFullName, 
//								"Customer Group for " + thisCustomerFullName,
//								DefaultCustomerGroupId,
//								systemUserId);
//						
//							thisCustomerGroup.Save();
//						
//							thisCustomerGroupId = thisCustomerGroup.LastIdAdded();
//
//						}
//
//
//					}

//					if (KDLRef != "" && numRefferedPermise > 0)
//					{
//						//This Customer is already in the DB, lets ensure there is a dedicated Customer Group
//						if (thisPremise.my_Customer_CustomerGroupId(0) > 3)
//						{
//							thisCustomerGroupId = thisPremise.my_Customer_CustomerGroupId(0);
//						}
//						else
//						{
//							//Create a Customer Group, and add the original Customer and this one to this group
//							thisCustomerGroup.Add(
//								thisPremise.my_Customer_FullName(0), 
//								"Customer Group for " + thisPremise.my_Customer_FullName(0),
//								DefaultCustomerGroupId,
//								systemUserId);
//						
//							thisCustomerGroup.Save();
//						
//							thisCustomerGroupId = thisCustomerGroup.LastIdAdded();
//	
//							thisCustomer.SetAttribute(thisPremise.my_CustomerId(0), "CustomerGroupId", thisCustomerGroupId.ToString());
//							thisCustomer.Save();
//
//						}
//					}
					#endregion

					#endregion

					#region Add Customer

					if (thisCustomerId == 0)
					{
						//New Customer, put them in the existing group and create them
						thisCustomerGroupId = DefaultCustomerGroupId;

						//Lets Create a thisCustomer Account
						thisCustomer.Add(thisCustomerGroupId, 
							customerType_business(),
							"",
							thisCustomerFirstName,
							thisCustomerLastName,
							BusinessName,
							PremiseNumber,
							assumedCountryId,
							0,
							0,
							"",
							"",
							DateStart,
							localRecords.DBDateTime(DateTime.Now),
							"",
							0,
							systemUserId);

						thisCustomer.Save();

						thisCustomerId = thisCustomer.LastIdAdded();
					}
					else 
					{
						//This Customer is already in the DB, lets ensure there is a dedicated Customer Group
						thisCustomer.GetByCustomerId(thisCustomerId);
						if (thisCustomer.my_CustomerGroupId(0) > 3)
						{
							thisCustomerGroupId = thisCustomer.my_CustomerGroupId(0);
						}
						else
						{
							thisCustomerGroupId =  DefaultCustomerGroupId;
							//Getting rid of customised customer groups

							//Create a Customer Group, and add the original Customer and this one to this group
//							thisCustomerGroup.Add(
//								thisCustomer.my_FullName(0), 
//								"Customer Group for " + thisCustomer.my_FullName(0),
//								DefaultCustomerGroupId,
//								systemUserId);
//							thisCustomerGroup.Save();
//						
//							thisCustomerGroupId = thisCustomerGroup.LastIdAdded();
//	
//							thisCustomer.SetAttribute(thisCustomer.my_CustomerId(0), "CustomerGroupId", thisCustomerGroupId.ToString());
//							thisCustomer.Save();

						}
					}

					//Update referred Properties
					//If there is more than one property in a group, theyare all the same customer and they are all the same
					//customer group
					foreach(kdlRefPropertyStruct thisReferral in RefNumsThisPremise)
					{
						if (thisReferral.PremiseId > 0)
						{
							thisPremise.SetAttribute(thisReferral.PremiseId, "CustomerId", thisCustomerId.ToString());
							thisPremise.Save();
							thisCustomer.SetAttribute(thisCustomerId, "CustomerGroupId", thisCustomerGroupId.ToString());
							thisCustomer.Save();
						}
					}					



					#region Check / Update DateStart for Customer

					thisCustomer.GetByCustomerId(thisCustomerId);
					if (DateStart != "")
					{
						string thisExistingDateStart = thisCustomer.my_DateStart(0);
						if (thisExistingDateStart == "" || 
							SanitiseDateAsDate(thisExistingDateStart) > SanitiseDateAsDate(DateStart))
						{
							thisCustomer.SetAttribute(thisCustomerId, "DateStart", SanitiseDate(DateStart).ToString());
							thisCustomer.Save();
						}

					}


					#endregion

					#endregion

					#region Add Premise itself

					//See if this thisPremise already exists in the Database
					int numPremises = thisPremise.GetByNumber(PremiseNumber, 
						matchCriteria_exactMatch());


					if (numPremises > 0 && PremiseNumber != "")
					{
						//If thisPremise already exists: 
						//o Use the customer account associated with this existing thisPremise
						//o Overwrite this Premise with this this new Premise's data
						thisPremiseId = thisPremise.my_PremiseId(0);

						thisPremise.Modify(thisPremiseId,
							thisCustomerId,
							0,
							0,
							PremiseNumber,
							BusinessName,
							thisPremise.my_QuickPhysicalAddress(0),
							thisTypeOfCompanyAtPremise,
							thisUrl,
							thisAlarmDetails,
							KdlComments,
							CustomerComments,
							DateStart,
							DateSubscriptionExpires,
							DateNextSubscriptionDueToBeInvoiced,
							DateOfLastDetailsUpdate,
							DateLastInvoice,
							"",
							"",
							0,
							0,
							0,
							0,
							0,
							systemUserId);

						thisPremise.Save();

						//Set the address for this Premise
						int numAddress = thisAddress.GetByPremiseId(thisPremiseId);
						if (numAddress > 0)
						{
							thisAddress.Modify(thisAddress.my_AddressId(0),
								poBoxType_normal(),
								PremiseBuildingName,
								PremiseStreetUnitNumber,
								PremiseStreetNumber,
								PremiseStreetAddress,
								PremiseSuburb,
								PremiseCityId,
								PremiseCityName,
								StateName,
								CountryId,
								"",
								PremisePostCode,
								addressType_physical(),
								"",
								"tblPremise",
								thisPremiseId,
								systemUserId);
						}
						else
						{
							thisAddress.Add(poBoxType_normal(),
								PremiseBuildingName,
								PremiseStreetUnitNumber,
								PremiseStreetNumber,
								PremiseStreetAddress,
								PremiseSuburb,
								PremiseCityId,
								PremiseCityName,
								StateName,
								CountryId,
								"",
								PremisePostCode,
								addressType_physical(),
								"",
								"tblPremise",
								thisPremiseId,
								systemUserId);
						}
						thisAddress.Save();

					}
					else
					{
						//Add this premise
						thisPremise.Add(
							thisCustomerId,
							0,
							0,
							PremiseNumber,
							BusinessName,
							thisTypeOfCompanyAtPremise,
							thisUrl,
							thisAlarmDetails,
							KdlComments,
							CustomerComments,
					
							PremiseBuildingName,
							PremiseStreetUnitNumber,
							PremiseStreetNumber,
							PremiseStreetAddress,
							PremiseSuburb,
							PremiseCityId,
							PremiseCityName,
							StateName,
							CountryId,
							"",
							PremisePostCode,
					
							DateStart,
							DateSubscriptionExpires,
							DateNextSubscriptionDueToBeInvoiced,
							DateOfLastDetailsUpdate,
							DateLastInvoice,
							"",
							"",

							StickerRequired,

							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							systemUserId);
				
				
						thisPremise.Save();
						thisPremiseId = thisPremise.LastIdAdded();
					}

					#endregion

					#region Deal with Referenced Properties

//					int ReferencedCustomerId = 0;

					//Did we find a reference thisPremise earlier? If so, add this now
					if (RefNumsThisPremise.Count > 0)
					{
						//Lets see if we can find a valid Customergroup among the references 
						foreach(kdlRefPropertyStruct thisReferral in RefNumsThisPremise)
						{
							if (thisReferral.PremiseId == 0)
							{
								thisPremise.Add(
									thisCustomerId,
									0,
									0,
									thisReferral.StickerNum,
									"",
									"",
									"",
									"",
									"",
									"Reference Found to this number in old database",
									"",
									"",
									"",
									"",
									"",
									0,
									"",
									"",
									CountryId,
									"",
									"",

									"",
									DateSubscriptionExpires,
									DateNextSubscriptionDueToBeInvoiced,
									DateOfLastDetailsUpdate,
									DateLastInvoice,
									"",
									"",

									StickerRequired,
									0,
									0,
									0,
									0,
									0,
									0,
									0,
									0,
									0,
									0,
									systemUserId);

								thisPremise.Save();
							}
						}
					}

//					if (KDLRef != "" && numRefferedPermise == 0)
//					{
//						thisCustomer.Add(thisCustomerGroupId, 
//							customerType_business(),
//							"",
//							thisCustomerFirstName,
//							thisCustomerLastName,
//							BusinessName,
//							PremiseNumber,
//							assumedCountryId,
//							0,
//							0,
//							DateStart,
//							"",
//							"",
//							systemUserId);
//
//						thisCustomer.Save();
//
//						ReferencedCustomerId = thisCustomer.LastIdAdded();					
//					
//						thisPremise.Add(
//							KDLRef,
//							ReferencedCustomerId,
//							"",
//							"",
//							0,
//							0,
//							"",
//							"",
//							"",
//							"Reference Found to this number in old database",
//							"",
//							"",
//							"",
//							"",
//							"",
//							0,
//							"",
//							"",
//							CountryId,
//							"",
//							"",
//
//							"",
//							DateSubscriptionExpires,
//							DateNextSubscriptionDueToBeInvoiced,
//							DateLastDetailsUpdate,
//
//							StickerRequired,
//							0,
//							0,
//							0,
//							0,
//							0,
//							0,
//							0,
//							0,
//							0,
//							systemUserId);
//
//						thisPremise.Save();
//
//					}
					#endregion

					#endregion

					if (thisPremiseId == 521)
						thisPremiseId = 521;

					DateTime afterAddCustomerAndPremise = DateTime.Now;

					//At this stage we have added both a thisCustomer and a thisPremise
					//Now need to add People

					#region Add Contacts

					//Main Contact


					int mainContactId = 0;
					keyHolderStruct mainContact = new keyHolderStruct();
					keyHolderStruct[] keyholder = new keyHolderStruct[4]; 
					
					//Initialise these keyholders
					for(int counter = 0; counter < keyholder.GetUpperBound(0) + 1; counter++)
					{
						keyholder[counter].Id = 0;
						keyholder[counter].Title = "";
						keyholder[counter].FirstName = "";
						keyholder[counter].LastName = "";
						keyholder[counter].NameComments = "";
						keyholder[counter].DaytimePhone = "";
						keyholder[counter].DaytimeFax = "";
						keyholder[counter].afterHoursPhone = "";
						keyholder[counter].mobilePhone = "";
						keyholder[counter].Email = "";
					}

					mainContact.Id = 0;
					mainContact.Title = "";
					mainContact.FirstName = "";
					mainContact.LastName = "";
					mainContact.NameComments = "";
					mainContact.DaytimePhone = "";
					mainContact.DaytimeFax = "";
					mainContact.afterHoursPhone = "";
					mainContact.mobilePhone = "";
					mainContact.Email = "";

					clsNameSectionsFromName thisMainContactName = new clsNameSectionsFromName(thisLine[(int) oldDbFieldIndex.personName].Trim());

//					GetNames(thisLine[(int) oldDbFieldIndex.personName].Trim(),
//						out Title, out FirstName, out LastName, out NameComments);
					
					mainContact.Title = thisMainContactName.Title;
					mainContact.FirstName = thisMainContactName.FirstName;
					mainContact.LastName = thisMainContactName.LastName;
					mainContact.NameComments = thisMainContactName.Comments;
					mainContact.DaytimePhone = thisLine[(int) oldDbFieldIndex.daytimePhone];
					mainContact.DaytimeFax = thisLine[(int) oldDbFieldIndex.daytimeFax];
					mainContact.Email = thisLine[(int) oldDbFieldIndex.email].ToLower();
					
					clsNameSectionsFromName thisKeyholder1Name = new clsNameSectionsFromName(thisLine[(int) oldDbFieldIndex.keyholder1Name].Trim());

					
//					GetNames(thisLine[(int) oldDbFieldIndex.keyholder1Name].Trim(),
//						out Title, out FirstName, out LastName, out NameComments);
					
					keyholder[0].Title = thisKeyholder1Name.Title;
					keyholder[0].FirstName = thisKeyholder1Name.FirstName;
					keyholder[0].LastName = thisKeyholder1Name.LastName;
					keyholder[0].NameComments = thisKeyholder1Name.Comments;
					keyholder[0].afterHoursPhone = thisLine[(int) oldDbFieldIndex.keyholder1afterHoursPhone];
					keyholder[0].mobilePhone = thisLine[(int) oldDbFieldIndex.keyholder1MobilePhone];

//					GetNames(thisLine[(int) oldDbFieldIndex.keyholder2Name].Trim(),
//						out Title, out FirstName, out LastName, out NameComments);

					clsNameSectionsFromName thisKeyholder2Name = new clsNameSectionsFromName(thisLine[(int) oldDbFieldIndex.keyholder2Name].Trim());
					
					keyholder[1].Title = thisKeyholder2Name.Title;
					keyholder[1].FirstName = thisKeyholder2Name.FirstName;
					keyholder[1].LastName = thisKeyholder2Name.LastName;
					keyholder[1].NameComments = thisKeyholder2Name.Comments;
					keyholder[1].afterHoursPhone = thisLine[(int) oldDbFieldIndex.keyholder2afterHoursPhone];
					keyholder[1].mobilePhone = thisLine[(int) oldDbFieldIndex.keyholder2MobilePhone];

//					GetNames(thisLine[(int) oldDbFieldIndex.keyholder3Name].Trim(),
//						out Title, out FirstName, out LastName, out NameComments);

					clsNameSectionsFromName thisKeyholder3Name = new clsNameSectionsFromName(thisLine[(int) oldDbFieldIndex.keyholder3Name].Trim());
					
					keyholder[2].Title = thisKeyholder3Name.Title;
					keyholder[2].FirstName = thisKeyholder3Name.FirstName;
					keyholder[2].LastName = thisKeyholder3Name.LastName;
					keyholder[2].NameComments = thisKeyholder3Name.Comments;
					keyholder[2].afterHoursPhone = thisLine[(int) oldDbFieldIndex.keyholder3afterHoursPhone];
					keyholder[2].mobilePhone = thisLine[(int) oldDbFieldIndex.keyholder3MobilePhone];

//					GetNames(thisLine[(int) oldDbFieldIndex.keyholder4Name].Trim(),
//						out Title, out FirstName, out LastName, out NameComments);

					clsNameSectionsFromName thisKeyholder4Name = new clsNameSectionsFromName(thisLine[(int) oldDbFieldIndex.keyholder4Name].Trim());

					keyholder[3].Title = thisKeyholder4Name.Title;
					keyholder[3].FirstName = thisKeyholder4Name.FirstName;
					keyholder[3].LastName = thisKeyholder4Name.LastName;
					keyholder[3].NameComments = thisKeyholder4Name.Comments;
					keyholder[3].afterHoursPhone = thisLine[(int) oldDbFieldIndex.keyholder4afterHoursPhone];
					keyholder[3].mobilePhone = thisLine[(int) oldDbFieldIndex.keyholder4MobilePhone];

					int dontAddKeyholder = -1;

					bool mainContactisNextLegitimateKeyholder = false;

					if (mainContact.FirstName == "" && mainContact.LastName == "")
						mainContactisNextLegitimateKeyholder = true;


					for (int counter = 0; counter < 4; counter++)
					{
						//If the Keyholder has a name and either that name is the same as the Main Contacts or the Main Contact has no name,
						//then this keyholder is assumed to be the same person as the main contact.
						if ((keyholder[counter].FirstName + keyholder[counter].LastName).Length > 0
							&& 
							(mainContactisNextLegitimateKeyholder 
							||
							((mainContact.FirstName.ToLower() == keyholder[counter].FirstName.ToLower()) &&
							(mainContact.LastName.ToLower() == keyholder[counter].LastName.ToLower()))))
						{
							dontAddKeyholder = counter;
							mainContact.Title = keyholder[counter].Title;
							mainContact.FirstName = keyholder[counter].FirstName;
							mainContact.LastName = keyholder[counter].LastName;
							mainContact.afterHoursPhone = keyholder[counter].afterHoursPhone;
							mainContact.mobilePhone = keyholder[counter].mobilePhone;
							mainContact.NameComments += crLf + keyholder[counter].NameComments;
							mainContactisNextLegitimateKeyholder = false;
						}
					}
					
					//Add Main Contact
					int NumPeopleAdded = 0;
					bool foundMainPerson = false;

					if ((mainContact.FirstName + mainContact.LastName + mainContact.NameComments).Length > 0)
					{
						switch (thisPOBoxType)
						{
							case poBoxType.normal:
								mainContactId = AddAPerson(
									thisCustomerId,
									mainContact.Title,
									mainContact.FirstName,
									mainContact.LastName, 
									mainContact.NameComments,
									mainContact.DaytimePhone, 
									mainContact.DaytimeFax,
									mainContact.afterHoursPhone,
									"",
									mainContact.mobilePhone,
									mainContact.Email, 
									thisPremiseId,
									thisPOBoxType,
									PersonBuildingName,
									PersonStreetUnitNumber,
									PersonStreetNumber,
									PersonStreetAddress,
									PersonPostCode,
									PersonSuburb,
									PersonCityName,
									StateName,
									CountryId);
								break;
							case poBoxType.poBox:
							case poBoxType.privateBag:
							default:
								mainContactId = AddAPerson(
									thisCustomerId,
									mainContact.Title,
									mainContact.FirstName,
									mainContact.LastName, 
									mainContact.NameComments,
									mainContact.DaytimePhone, 
									mainContact.DaytimeFax,
									mainContact.afterHoursPhone,
									"",
									mainContact.mobilePhone,
									mainContact.Email, 
									thisPremiseId,
									thisPOBoxType,
									"",
									"",
									POBoxNumber,
									"",
									"",
									POBoxSuburb,
									POBoxCityName,
									StateName,
									CountryId);
								break;
						}

						
						if (mainContactId != 0)
						{
							NumPeopleAdded++;
							foundMainPerson = true;
						}
					}


					//Now add each of the Keyholders
					for (int counter = 0; counter < 4; counter++)
					{
						if((dontAddKeyholder != counter) &&
							((keyholder[counter].FirstName 
							+ keyholder[counter].LastName).Length > 0))
						{
							keyholder[counter].Id = AddAPerson(
								thisCustomerId,
								keyholder[counter].Title,
								keyholder[counter].FirstName,
								keyholder[counter].LastName, 
								keyholder[counter].NameComments,
								keyholder[counter].DaytimePhone, 
								keyholder[counter].DaytimeFax,
								keyholder[counter].afterHoursPhone,
								"",
								keyholder[counter].mobilePhone,
								keyholder[counter].Email, 
								thisPremiseId,
								poBoxType.normal,
								"",
								"",
								"",
								"",
								"",
								"",
								"",
								"",
								assumedCountryId);

							NumPeopleAdded++;
		
							if (!foundMainPerson && keyholder[counter].Id != 0)
							{
								mainContactId = keyholder[counter].Id;
								foundMainPerson = true;
							}
						}
					}
					
					#endregion

					DateTime afterGetKeyholdersForPremiseId = DateTime.Now;
					
					#region Add Roles for Contacts

					for(int counter = 0; counter < 4; counter++)
					{
						if (keyholder[counter].Id > 0 || dontAddKeyholder == counter)
						{
							if (dontAddKeyholder == counter)
							{
								if (mainContactId != 0)
								{
									thisPersonPremiseRole.Add(thisPremiseId, 
										mainContactId, 
										personPremiseRoleType_keyHolder(), 
										counter + 1,
										systemUserId);
									thisPersonPremiseRole.Save();
								}
							}							
							else							
							{
								thisPersonPremiseRole.Add(thisPremiseId, 
									keyholder[counter].Id, 
									personPremiseRoleType_keyHolder(), 
									counter + 1,
									systemUserId);
								thisPersonPremiseRole.Save();
							}
						}
					}
					
					DateTime afterKeyHolders = DateTime.Now;
	
					//Set the other PPRs

					if (mainContactId != 0)
					{
						thisPersonPremiseRole.Set(thisPremiseId, mainContactId, personPremiseRoleType_billingContact(), systemUserId);
						thisPersonPremiseRole.Save();
						thisPersonPremiseRole.Set(thisPremiseId, mainContactId, personPremiseRoleType_daytimeContact(), systemUserId);
						thisPersonPremiseRole.Save();
						thisPersonPremiseRole.Set(thisPremiseId, mainContactId, personPremiseRoleType_detailsManager(), systemUserId);
						thisPersonPremiseRole.Save();
					}
					//Save them
//					thisPersonPremiseRole.Save();

					#endregion

					DateTime afterPremiseSave = DateTime.Now;

					#region debug
					debugFileWriter = new clsCsvWriter(thisDebugFile, true);
					debugFileWriter.WriteFields(new
						object[] { 
									 DateTime.Now.ToString(),
									 "ImportFromCsv Progress: Pre-Add Service Provider",
									 CsvFileFolder,
									 fileToImport,
									 RecordToStartAt.ToString(),
									 thisRecord.ToString(),
									 numRecords.ToString()
								 });
					debugFileWriter.Close();
					#endregion

					#region Add Service Provider

					string SPName = thisLine[(int) oldDbFieldIndex.securityCompanyName].Trim();
					
					int thisServiceProviderId = 0;
					
					DateTime afterAddServiceProvider;
					DateTime afterGetByPremiseId;

					if (SPName.Length > 0)

					{
						thisServiceProviderId = AddServiceProvider(thisPremiseId, 
							thisLine[(int) oldDbFieldIndex.securityCompanyName].Trim(),
							thisLine[(int) oldDbFieldIndex.securityCompanydaytimePhone].Trim(),
							thisCustomerId);
					
						afterAddServiceProvider = DateTime.Now;

						thisPremise.GetByPremiseId(thisPremiseId);

						afterGetByPremiseId = DateTime.Now;

						thisServiceProviderPremiseRole.Set(thisPremiseId,
							thisServiceProviderId,
							sPPremiseRoleType_alarmMonitor(),
							systemUserId);
						thisServiceProviderPremiseRole.Set(thisPremiseId,
							thisServiceProviderId,
							sPPremiseRoleType_alarmResponse(),
							systemUserId);
						thisServiceProviderPremiseRole.Set(thisPremiseId,
							thisServiceProviderId,
							sPPremiseRoleType_patrol(),
							systemUserId);
						thisServiceProviderPremiseRole.Save();
					}
					else
					{
						afterAddServiceProvider = DateTime.Now;

						thisPremise.GetByPremiseId(thisPremiseId);

						afterGetByPremiseId = DateTime.Now;
					}
					
					#endregion

					#region debug
					debugFileWriter = new clsCsvWriter(thisDebugFile, true);
					debugFileWriter.WriteFields(new
						object[] { 
									 DateTime.Now.ToString(),
									 "ImportFromCsv Progress: Pre-Add To Timing File",
									 CsvFileFolder,
									 fileToImport,
									 RecordToStartAt.ToString(),
									 thisRecord.ToString(),
									 numRecords.ToString()
								 });

					debugFileWriter.Close();
					#endregion

					#region Add Account and Transaction Information

					thisPersonPremiseRole = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);
				
					//Default Due Date is the 20th of next month
					DateTime DateDue = new DateTime(DateTime.Now.Year,
						DateTime.Now.Month,
						20);
					DateDue = DateDue.AddMonths(1);


					//Need to find the main contact; this can be the first billing contact we find
				
					//For each customer, create a single thisOrder
					thisOrder.Add(mainContactId, 
						thisCustomerId, 
						PremiseNumber, 
						orderCreatedMechanism_byVendorAutomatically(), 
						1, 
						"Imported From KDL");
					thisOrder.Save();

					int thisOrderId = thisOrder.LastIdAdded();

					thisOrder.UpdateOrderCosts(thisOrderId);

					numPremises = thisPremise.GetByCustomerId(thisCustomerId);

					DateTime thisRenewalDate = thisNextSub;

//					for(int PremiseCounter = 0; PremiseCounter < numPremises; PremiseCounter++)
//					{
						//For each thisPremise, add an thisItem.
						int thisUnderlingPremiseId = thisPremise.my_PremiseId(0);

//						bool gotDate = true;
//
//						try
//						{
//							thisRenewalDate = Convert.ToDateTime(thisPremise.my_DateSubscriptionExpires(PremiseCounter).Trim());
//						}
//						catch (Exception)
//						{
//							gotDate = false;
//						}



//						if (gotDate)
//						{
							thisItem.AddFromProductIdPremiseId(thisOrderId,1, thisUnderlingPremiseId, thisRenewalDate.ToLongDateString(),1);
							DateDue = new DateTime(thisRenewalDate.Year,
								thisRenewalDate.Month,
								20);
							DateDue = DateDue.AddMonths(1);
//						}
//						else
//							thisItem.AddFromProductIdPremiseId(thisOrderId,1, thisUnderlingPremiseId, "", 1);



						thisItem.Save();
						int thisItemId = thisItem.LastIdAdded();

						//Update the order
						thisOrder.Save();

						//Update the thisPremise
						thisPremise.SetItemId(thisUnderlingPremiseId, thisItemId);
						thisPremise.Save();
					
//					} // Added each thisPremise for a thisCustomer

					thisOrder.SetOrderStatus(thisOrderId, 2);
					thisOrder.Save();


					thisOrder.UpdateOrderCosts(thisOrderId);
					thisOrder.Save();

					string DateLastInvoiceUniversal =  thisOrder.localRecords.DBDateTime(Convert.ToDateTime(DateLastInvoice).ToUniversalTime());

					//Submit Order without creating a transaction
					thisOrder.SetAttribute(thisOrderId, "DateDue", thisOrder.localRecords.DBDateTime(DateDue));
					thisOrder.AddAttributeToSet(thisOrderId, "OrderSubmitted", "1");
					thisOrder.AddAttributeToSet(thisOrderId, "DateSubmitted", DateLastInvoice);
					thisOrder.AddAttributeToSet(thisOrderId, "DateSubmittedUtc", DateLastInvoiceUniversal);
					thisOrder.AddAttributeToSet(thisOrderId, "DateCreated", DateLastInvoice);
					thisOrder.AddAttributeToSet(thisOrderId, "DateCreatedUtc", DateLastInvoiceUniversal);
					thisOrder.AddAttributeToSet(thisOrderId, "DateShipped",DateLastInvoice);
					thisOrder.AddAttributeToSet(thisOrderId, "DateShippedUtc",DateLastInvoiceUniversal);

					thisOrder.Save();

					string renewDate = thisLine[(int) oldDbFieldIndex.renewDate].Trim().ToUpper();
					bool unpaid = false;

					if (renewDate.EndsWith(" N"))
						unpaid = true;

					int numOrders = thisOrder.GetByOrderId(thisOrderId);
 

					#region Add the debit transaction if it is overdue
					if (unpaid && numOrders > 0)
					{

						thisTransaction.ImportedByVendor(thisCustomerId, 
							1, 
							thisCustomer.my_FullName(0),
							"",
							thisOrderId,
							paymentMethodType_orderDebit(),
							0,
							0,
							0,
							(thisOrder.my_Total(0)),
							localRecords.DBDateTime(thisNextSub.AddYears(-1)),
							"Imported from KDL Invoice #" + PremiseNumber,
							"");
						
						thisTransaction.Save();
					}
					#endregion

					#endregion

					#region Add to timing file

					DateTime afterPremiseModify = DateTime.Now;

					clsCsvWriter fileWriter = new clsCsvWriter(thisTimingPathFile, true);

					if (NumPeopleAdded == 0)
						NumPeopleAdded = 1;

					fileWriter.WriteFields(new
						object[] { 
									 startOfRecordTime.ToString(),
									 afterAddCustomerAndPremise.Subtract(startOfRecordTime).TotalMilliseconds.ToString(),
									 afterKeyHolders.Subtract(afterAddCustomerAndPremise).TotalMilliseconds.ToString(),
									 (afterKeyHolders.Subtract(afterAddCustomerAndPremise).TotalMilliseconds / NumPeopleAdded).ToString(),
									 afterAddServiceProvider.Subtract(afterKeyHolders).TotalMilliseconds.ToString(),
									 afterGetByPremiseId.Subtract(afterAddServiceProvider).TotalMilliseconds.ToString(),
									 afterPremiseModify.Subtract(afterGetByPremiseId).TotalMilliseconds.ToString(),
									 afterGetKeyholdersForPremiseId.Subtract(afterPremiseModify).TotalMilliseconds.ToString(),
									 afterPremiseSave.Subtract(afterGetKeyholdersForPremiseId).TotalMilliseconds.ToString(),
									 afterPremiseSave.Subtract(startOfRecordTime).TotalMilliseconds.ToString()			
								 }
						);

					fileWriter.Close();

					#endregion

					#region write last sucessfuly added referral


					clsCsvWriter thhisFileWriter = new clsCsvWriter(thisLastGoodRecordPathFile, false);

					thhisFileWriter.WriteFields(new
						object[] { 
									 thisCustomerId.ToString(), 
									 thisRecord.ToString(),
									 numRecords.ToString(),
									 0,
									 0,
									 0
								 });
					thhisFileWriter.Close();

					#endregion

				}
				thisLine = fileReader.GetCsvLine();
			}
			fileReader.Dispose();
		}


		#endregion

		#region Get Sensible Date

		/// <summary>
		/// Returns a sensible date from the string provided. This may be 1st January, 2001...
		/// </summary>
		/// <param name="thisDate">thisDate</param>
		/// <returns>A sensible date from the string provided. This may be 1st January, 2001...</returns>
		public string GetSensibleDate(string thisDate)
		{
			return GetSensibleDate(thisDate, 1);
		}

		/// <summary>
		/// Returns a sensible date from the string provided. This may be 1st January, 2001...
		/// </summary>
		/// <param name="thisDate">thisDate</param>
		/// <param name="mustBeBeforeNow">-1 = must be after now, 0 = not checked, 1 = must be before now</param>
		/// <returns>A sensible date from the string provided. This may be 1st January, 2001...</returns>
		public string GetSensibleDate(string thisDate, int mustBeBeforeNow)
		{
			DateTime newDate = new DateTime(2001, 1, 1);
			//This might just be a month name; if so add the details
			switch (thisDate.ToLower())
			{
				case "jan":
				case "january":
					newDate = new DateTime(DateTime.Now.Year, 1, 1);
					break;
				case "feb":
				case "february":
					newDate = new DateTime(DateTime.Now.Year, 2, 1);
					break;
				case "mar":
				case "march":
					newDate = new DateTime(DateTime.Now.Year, 3, 1);
					break;
				case "apr":
				case "april":
					newDate = new DateTime(DateTime.Now.Year, 4, 1);
					break;
				case "may":
					newDate = new DateTime(DateTime.Now.Year, 5, 1);
					break;
				case "jun":
				case "june":
					newDate = new DateTime(DateTime.Now.Year, 6, 1);
					break;
				case "jul":
				case "july":
					newDate = new DateTime(DateTime.Now.Year, 7, 1);
					break;
				case "aug":
				case "august":
					newDate = new DateTime(DateTime.Now.Year, 8, 1);
					break;
				case "sep":
				case "sept":
				case "september":
					newDate = new DateTime(DateTime.Now.Year, 9, 1);
					break;
				case "oct":
				case "october":
					newDate = new DateTime(DateTime.Now.Year, 10, 1);
					break;
				case "nov":
				case "november":
					newDate = new DateTime(DateTime.Now.Year, 11, 1);
					break;
				case "dec":
				case "december":
					newDate = new DateTime(DateTime.Now.Year, 12, 1);
					break;
				default:
					try
					{
						newDate = Convert.ToDateTime(thisDate);
					}
					catch (Exception)
					{
					}
					break;
			}

			switch (mustBeBeforeNow)
			{
				case -1:

					while (newDate < DateTime.Now)
						newDate = newDate.AddYears(1);
					break;
				case 1:
					while (newDate > DateTime.Now)
						newDate = newDate.AddYears(-1);
					break;
				default:
					break;
			}

			return localRecords.DBDateTime(newDate);

		}

		#endregion

		#region AddOrdersAndItems but not Tranactions

		/// <summary>
		/// Adds Orders and Items to the Data
		/// </summary>
		public void AddOrdersAndItems()
		{

			//Add Orders and Pricing
			#region No longer used

//			int numCustomers = thisCustomer.GetAll();
//			for(int customerCounter = 0; customerCounter < numCustomers; customerCounter++)
//			{
//				int thisCustomerId = thisCustomer.my_CustomerId(customerCounter);
//
//				thisPersonPremiseRole = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);
//				int numBillingContacts = thisPersonPremiseRole.GetByCustomerIdPersonPremiseRoleType(thisCustomerId, personPremiseRoleType_billingContact());
//				
//				//Default Due Date is the 20th of next month
//				DateTime DateDue = new DateTime(DateTime.Now.Year,
//					DateTime.Now.Month,
//					20);
//				DateDue = DateDue.AddMonths(1);
//
//				if (numBillingContacts != 0)
//				{
//					int mainContactId = thisPersonPremiseRole.my_PersonId(0);
//
//					//Need to find the main contact; this can be the first billing contact we find
//				
//					//For each customer, create a single thisOrder
//					thisOrder.Add(mainContactId, 
//						thisCustomerId, 
//						thisOrder.GetNextOrderNumber(), 
//						orderCreatedMechanism_byVendorAutomatically(), 
//						1, 
//						"Imported From Old System");
//					thisOrder.Save();
//
//					int thisOrderId = thisOrder.LastIdAdded();
//
//					thisOrder.UpdateOrderCosts(thisOrderId);
//
//					int numPremises = thisPremise.GetByCustomerId(thisCustomerId);
//
//					DateTime thisRenewalDate = DateTime.Now;
//
//					for(int PremiseCounter = 0; PremiseCounter < numPremises; PremiseCounter++)
//					{
//						//For each thisPremise, add an thisItem.
//						int thisPremiseId = thisPremise.my_PremiseId(PremiseCounter);
//
//						bool gotDate = true;
//
//						try
//						{
//							thisRenewalDate = Convert.ToDateTime(thisPremise.my_DateSubscriptionExpires(PremiseCounter).Trim());
//						}
//						catch (Exception)
//						{
//							gotDate = false;
//						}
//
//
//
//						if (gotDate)
//						{
//							thisItem.AddFromProductIdPremiseId(thisOrderId,1, thisPremiseId, thisRenewalDate.ToLongDateString(),1);
//							DateDue = new DateTime(thisRenewalDate.Year,
//								thisRenewalDate.Month,
//								20);
//							DateDue = DateDue.AddMonths(1);
//						}
//						else
//							thisItem.AddFromProductIdPremiseId(thisOrderId,1, thisPremiseId, "", 1);
//
//
//
//						thisItem.Save();
//						int thisItemId = thisItem.LastIdAdded();
//
//						//Update the order
//						thisOrder.Save();
//
//						//Update the thisPremise
//						thisPremise.SetItemId(thisPremiseId, thisItemId);
//						thisPremise.Save();
//					
//					} // Added each thisPremise for a thisCustomer
//
//					thisOrder.SetOrderStatus(thisOrderId, 2);
//                    thisOrder.Save();
//
//
//					thisOrder.UpdateOrderCosts(thisOrderId);
//					thisOrder.Save();
//
//					//Submit Order without creating a transaction
//					thisOrder.SetAttribute(thisOrderId, "DateDue", thisOrder.localRecords.DBDateTime(DateDue));
//					thisOrder.AddAttributeToSet(thisOrderId, "OrderSubmitted", "1");
//					thisOrder.AddAttributeToSet(thisOrderId, "DateSubmitted", localRecords.DBDateTime(thisRenewalDate));
//					thisOrder.AddAttributeToSet(thisOrderId, "DateShippedUtc",localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(thisRenewalDate))));
//
//
//					thisOrder.Save();
//
//					#region Add the debit transaction (Actually Don't)
////					int numOrders = thisOrder.GetByOrderId(thisOrderId);
////					if (numOrders > 0)
////					{
////
////						thisTransaction.ImportedByVendor(thisCustomerId, 
////							1, 
////							thisCustomer.my_FullName(customerCounter),
////							"Auto Added on Import",
////							thisOrderId,
////							paymentMethodType_orderDebit(),
////							0,
////							0,
////							0,
////							(thisOrder.my_Total(0) + thisOrder.my_TaxCost(0)) * -1,
////							localRecords.DBDateTime(DateTime.Now),
////							"Auto Added on Import",
////							"");
////
////						thisTransaction.Save();
////
////					}
//					#endregion
//				}
//			}
			#endregion
			
			//Set Invoice Needed to false
			localRecords.GetRecords(@"Update tblPremise set InvoiceRequired = 0;");

			//Update GST
			thisSetting.Ensure("LocalTaxPerCent", "15");

			//Update Pricing
			clsProductCustomerGroupPrice thisProductCustomerGroupPrice = new clsProductCustomerGroupPrice(thisDbType, localRecords.dbConnection);
			thisProductCustomerGroupPrice.Modify(1,1,1,39);
			thisProductCustomerGroupPrice.Modify(2,1,2,19.50M);
			thisProductCustomerGroupPrice.Modify(3,1,3,0);
			thisProductCustomerGroupPrice.Modify(4,2,1,10);
			thisProductCustomerGroupPrice.Modify(5,2,2,5);
			thisProductCustomerGroupPrice.Modify(6,2,3,0);
			thisProductCustomerGroupPrice.Save();


		}


		#endregion

		#region Add Users for all people with credentials

		/// <summary>
		/// AddUsersForPeople
		/// </summary>
		public void AddUsersForPeople()
		{
			int numPeople = thisPerson.GetAll();

			for(int counter = 0; counter < numPeople; counter++)
			{
				thisUser.Add(
					thisPerson.my_PersonId(counter),
					thisPerson.my_FirstName(counter),
					thisPerson.my_LastName(counter),
					thisPerson.my_UserName(counter),
					thisPerson.my_Password(counter),
					thisPerson.my_Email(counter),
					4,
					1);

				thisUser.Save();

			}

		}


		#endregion

		#region DoCorrespondence

		/// <summary>
		/// DoCorrespondence
		/// </summary>
		/// <param name="thisDate">thisDate</param>
		public void DoCorrespondence(DateTime thisDate)
		{
			#region Create a list of Unsubmitted Orders
			clsPremise thisPremise = new clsPremise(thisDbType, localRecords.dbConnection);

			thisPremise.CreateRenewals(
				thisPremise.localRecords.DBDateTime(thisDate)
				);

			#endregion

			#region Get list of Unsubmitted Orders (and submit them)

			clsItem thisItem = new clsItem(thisDbType, localRecords.dbConnection);

			clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);

			int numImminentRenewals = thisItem.GetUnsubmittedOrders();

			thisOrder.SubmitAllUnsubmittedOrders(
				thisOrder.localRecords.DBDateTime(DateTime.Now));
			#endregion

			#region Mark Statement Required and Details Update Required where necessary

			thisPremise.SetStatementRequired(thisPremise.localRecords.DBDateTime(thisDate));
			thisPremise.SetDetailsUpdateRequired(thisPremise.localRecords.DBDateTime(thisDate));

			#endregion

			#region Generate Correspondence

			clsPersonPremiseRole thisPersonPremiseRole = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);

			clsCorrespondenceSendToCustomer thisCorrespondenceSendToCustomer = new clsCorrespondenceSendToCustomer(thisDbType, localRecords.dbConnection);

			clsReport thisReport = new clsReport(thisDbType, localRecords.dbConnection);

			thisReport.thisReportPath = @"C:\Inetpub\wwwroot\WebApplicationTestReport\";
			thisReport.thisRootPath = @"C:\Work\TestReport\";
			thisReport.thisOutputPath = @"test";

			thisPersonPremiseRole.AddOrderByColumns("Customer_FullName");
			thisPersonPremiseRole.AddOrderByColumns("Person_CustomerId");
			thisPersonPremiseRole.AddOrderByColumns("PersonId");
			thisPersonPremiseRole.AddOrderByColumns("PremiseId");
//			thisPersonPremiseRole.AddOrderByColumns("Premise_IncludesStickers");
//			thisPersonPremiseRole.AddOrderByColumns("PersonId");
//			thisPersonPremiseRole.AddOrderByColumns("CorrespondenceMedium");
			int numPendingCorrespondence = thisPersonPremiseRole.GetPendingCorrespondence();

			int oldPremiseId = 0;
			int thisPremiseId = 0;

			int oldPersonId = 0;
			int oldCustomerId = 0;
			int thisPersonId = 0;
			int thisCustomerId = 0;

			for(int counter = 0; counter < numPendingCorrespondence; counter++)
			{
				oldPremiseId = thisPremiseId;
				oldPersonId = thisPersonId;
				oldCustomerId = thisCustomerId;

				thisPremiseId = thisPersonPremiseRole.my_PremiseId(counter);
				thisPersonId = thisPersonPremiseRole.my_PersonId(counter);
				thisCustomerId = thisPersonPremiseRole.my_Person_CustomerId(counter);

				bool InvoiceSent = false;
				bool InvoiceRequired = false;
				bool StatementRequired = false;
				bool StatementSent = false;

				bool CoverPageSent = false;
				bool PremisesSent = false;
				bool PeopleSent = false;
				bool ServiceProvidersSent = false;
				
				int isEmail = 0;
				string EmailSnail = "SnailMail";

				bool isRepeat = true;

				if (oldPersonId != thisPersonId || oldCustomerId != thisCustomerId || oldPremiseId != thisPremiseId)
				{
					isRepeat = false;
					oldPremiseId = thisPremiseId;
					oldPersonId = thisPersonId;
					oldCustomerId = thisCustomerId;


					if (thisPersonPremiseRole.my_Person_PreferredContactMethod(counter) == thisPersonPremiseRole.correspondenceMedium_email())
					{
						isEmail = 1;
						EmailSnail = "";
					}

					ArrayList theseSubReports = new ArrayList();

					if (thisPersonPremiseRole.my_Premise_InvoiceRequired(counter) == 1)
					{
						InvoiceRequired = true;
					}

					if (thisPersonPremiseRole.my_Premise_StatementRequired(counter) == 1)
					{
						StatementRequired = true;
					}

//					if (0 == 1)
//					{

						#region CoverPage

						string thisFileName = @"Temp" + thisReport.WelmanDateTime(DateTime.Now) 
							+ "_" + thisCustomerId.ToString()
							+ "_" + thisPersonId.ToString() 
							+ "Coversheet.pdf";

						int result = thisReport.CoverLetter(thisCustomerId,
							thisPersonId,
							isEmail,
							thisFileName);

						if (result > 0)
						{
							theseSubReports.Add(thisReport.thisRootPath + thisFileName);
							CoverPageSent= true;

						}
						#endregion

						#region Invoice

						if (thisPersonPremiseRole.my_Premise_InvoiceRequired(counter) == 1)
						{
							InvoiceRequired = true;

							thisFileName = @"Temp" + thisReport.WelmanDateTime(DateTime.Now) 
								+ "_" + thisCustomerId.ToString()
								+ "_" + thisPersonId.ToString() 
								+ "Invoice.pdf";

							result = thisReport.Invoices(thisCustomerId,
								isEmail,
								thisFileName);

							if (result > 0)
							{
								theseSubReports.Add(thisReport.thisRootPath + thisFileName);
								InvoiceSent = true;
							}
						}
						#endregion

						#region Statement

						if (thisPersonPremiseRole.my_Premise_StatementRequired(counter) == 1)
						{
							StatementRequired = true;

							thisFileName = @"Temp" + thisReport.WelmanDateTime(DateTime.Now) 
								+ "_" + thisCustomerId.ToString()
								+ "_" + thisPersonId.ToString() 
								+ "Statement.pdf";

							result = thisReport.Statements(thisCustomerId,
								isEmail,
								thisFileName);

							if (result > 0)
							{
								theseSubReports.Add(thisReport.thisRootPath + thisFileName);
								StatementSent = true;
							}
						}
						#endregion

						#region Premises

						thisFileName = @"Temp" + thisReport.WelmanDateTime(DateTime.Now) 
							+ "_" + thisCustomerId.ToString()
							+ "_" + thisPersonId.ToString() 
							+ "Premises.pdf";

						result = thisReport.Premises(thisCustomerId,
							isEmail,
							thisFileName);

						if (result > 0)
						{
							theseSubReports.Add(thisReport.thisRootPath + thisFileName);
							PremisesSent = true;
						}

						#endregion

						#region People

						thisFileName = @"Temp" + thisReport.WelmanDateTime(DateTime.Now) 
							+ "_" + thisCustomerId.ToString()
							+ "_" + thisPersonId.ToString() 
							+ "People.pdf";

						result = thisReport.People(thisCustomerId,
							isEmail,
							thisFileName);

						if (result > 0)
						{
							theseSubReports.Add(thisReport.thisRootPath + thisFileName);
							PeopleSent = true;

						}
						#endregion

						#region ServiceProviders

						thisFileName = @"Temp" + thisReport.WelmanDateTime(DateTime.Now) 
							+ "_" + thisCustomerId.ToString()
							+ "_" + thisPersonId.ToString() 
							+ "ServiceProviders.pdf";

						result = thisReport.ServiceProviders(thisCustomerId,
							isEmail,
							thisFileName);

						if (result > 0)
						{
							theseSubReports.Add(thisReport.thisRootPath + thisFileName);
							ServiceProvidersSent = true;
						}
						#endregion

						#region Write the Correspondence

						//					string retVal = thisReport.Merge_File(theseSubReports, thisReport.thisRootPath + RequiresSticker
						//						+ "_" + thisCustomerId.ToString()
						//						+ "_" + thisPersonPremiseRole.my_Person_FullName(counter)
						//						+ "_" + thisPersonId.ToString() 
						//						+ EmailSnail + ".pdf");

						string retVal = "";

						if (retVal != "")
						{
							//						MessageBox.Show(retVal);
						}
						else
						{

//							foreach(object thisFile in theseSubReports)
//							{
//								try
//								{
//									File.Delete(thisFile.ToString());
//								}
//								catch 
//								{
//								}
//							}
						}

						#endregion

//					}
				}

				#region Write the Output File and Delete the initial files

				string RequiresSticker = "NoStickerRequired";

				if (thisPersonPremiseRole.my_Premise_StickerRequired(counter) > 0)
					RequiresSticker = "!StickerRequired";

				#region Output what correspondence went to whoom
				if (!isRepeat)
				{

					string thisOutputFileName = @"C:\CorrespondenceOut" + WelmanDateTime(thisDate) + ".csv";

					clsCsvWriter thisFileWriter = new clsCsvWriter(thisOutputFileName, true);

					thisFileWriter.WriteFields(new
						object[] { 
									 thisCustomerId.ToString(), 
									 thisPersonId.ToString(),
									 isEmail.ToString(),
									 RequiresSticker,
									 thisPersonPremiseRole.my_Premise_PremiseNumber(counter),
									 thisPersonPremiseRole.my_Premise_CompanyAtPremiseName(counter),
									 thisPersonPremiseRole.my_Person_FullName(counter),
									 thisPersonPremiseRole.my_Premise_DateNextSubscriptionDueToBeInvoiced(counter),
									 thisPersonPremiseRole.my_Premise_DateSubscriptionExpires(counter),
									 thisPersonPremiseRole.my_Premise_DateLastDetailsUpdate(counter),
									 thisPersonPremiseRole.my_Premise_DateLastInvoice(counter),
									 thisPersonPremiseRole.my_Customer_CustomerGroupId(counter),

//									 isRepeat.ToString(),

									 InvoiceRequired.ToString(),
//									 InvoiceSent.ToString(),

									 StatementRequired.ToString(),
//									 StatementSent.ToString(),

//									 CoverPageSent.ToString(),
//									 PremisesSent.ToString(),
//									 PeopleSent.ToString(),
//									 ServiceProvidersSent.ToString()
								 });


					thisFileWriter.Close();
				}

				#endregion


				#endregion

			}


			#endregion

			#region Mark Corrsepondence as Done

			thisPremise.MarkCorrespondenceAsSent(
				thisPremise.localRecords.DBDateTime(thisDate)
				);

			#endregion
		}
	
		#endregion

		#region Export Spreadsheet of Contacts for Mail Merge

		/// <summary>
		/// ExportContactsForMailMerge
		/// </summary>
		/// <returns>Number of Contacts</returns>
		public int ExportContactsForMailMerge()
		{
			return ExportContactsForMailMerge("");
		}

		/// <summary>
		/// ExportContactsForMailMerge
		/// </summary>
		/// <returns>Number of Contacts</returns>
		public int ExportContactsForMailMerge(string thisOutputFileName)
		{

			if (thisOutputFileName == "")
				thisOutputFileName = @"C:\FullCustomerExport" + WelmanDateTime(DateTime.Now) + ".csv";

			clsCsvWriter thisFileWriter = new clsCsvWriter(thisOutputFileName, true);

			thisFileWriter.WriteFields(new
				object[] { 
							 "CustomerFullName",
							 "Title", 
							 "FirstName", 
							 "LastName", 
							 "PostalAddress", 
							 "CustomerAccountNumber", 
							 "NumberOfPremisesThisCustomer", 
							 "NumberOfPropertiesBillingContactFor", 
							 "NumberOfPropertiesDetailsManagerFor"
						 });


			thisFileWriter.Close();

			clsPersonPremiseRole thisPersonPremiseRole = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);

			thisPersonPremiseRole.AddOrderByColumns("Customer_AccountNumber");
			thisPersonPremiseRole.AddOrderByColumns("Customer_FullName");
			thisPersonPremiseRole.AddOrderByColumns("Person_LastName");
			thisPersonPremiseRole.AddOrderByColumns("Person_FirstName");
			int numPeople = thisPersonPremiseRole.GetAllDistintPeopleWhoNeedGetCorrespondence();

			for(int counter = 0; counter < numPeople; counter ++)
			{
				//Get number of premises for this customer

				int numPremises = 0;

				int CustomerId = thisPersonPremiseRole.my_Person_CustomerId(counter);
				int PersonId = thisPersonPremiseRole.my_Person_PersonId(counter);

				clsPremise thisPremise = new clsPremise(thisDbType, localRecords.dbConnection);

				numPremises = thisPremise.GetByCustomerId(CustomerId);

				clsPersonPremiseRole thisPPR = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);

				int numBilling = thisPPR.GetByPersonIdPersonPremiseRoleType(PersonId, personPremiseRoleType_billingContact());
				int numDetailsManager = thisPPR.GetByPersonIdPersonPremiseRoleType(PersonId, personPremiseRoleType_detailsManager());


				#region Output this record

				thisFileWriter = new clsCsvWriter(thisOutputFileName, true);

				thisFileWriter.WriteFields(new
					object[] { 
								 thisPersonPremiseRole.my_Customer_FullName(counter), 
								 thisPersonPremiseRole.my_Person_Title(counter), 
								 thisPersonPremiseRole.my_Person_FirstName(counter), 
								 thisPersonPremiseRole.my_Person_LastName(counter), 
								 quote + thisPersonPremiseRole.my_Person_QuickPostalAddress(counter) + quote, 
								 thisPersonPremiseRole.my_Customer_AccountNumber(counter), 
								 numPremises.ToString(), 
								 numBilling.ToString(), 
								 numDetailsManager.ToString()
							 });


				thisFileWriter.Close();

				#endregion


			}
			return numPeople;

		}


		#endregion

		#region Add People / Service Providers

		#region AddAPerson



		/// <summary>
		/// Takes thisPerson information and attempts to match this to data in the 
		/// database. If an existing person can not be matched up to this data,
		/// a new entry is created.
		/// </summary>
		/// <param name="CustomerId">CustomerId</param>
		/// <param name="Title">thisPerson's Title</param>
		/// <param name="FirstName">thisPerson's First Name</param>
		/// <param name="LastName">thisPerson's Last Name</param>
		/// <param name="CommentsFromName">Comments from this thisPerson's Name in the DB</param>
		/// <param name="DaytimePhone">DaytimePhone</param>
		/// <param name="DaytimeFax">DaytimeFax</param>
		/// <param name="AfterHoursPhone">afterHoursPhone</param>
		/// <param name="AfterHoursFax">AfterHoursFax</param>
		/// <param name="MobilePhone">MobilePhone</param>
		/// <param name="Email">Email</param>
		/// <param name="thisPremiseId">thisPremiseId</param>
		/// <param name="BuildingName">BuildingName</param>
		/// <param name="UnitNumber">UnitNumber</param>
		/// <param name="Number">Number</param>
		/// <param name="StreetAddress">StreetAddress</param>
		/// <param name="POBoxType">POBoxType</param>
		/// <param name="Suburb">Suburb</param>
		/// <param name="PostCode">Post Code</param>
		/// <param name="CityName">CityName</param>
		/// <param name="StateName">StateName</param>
		/// <param name="CountryId">CountryId</param>
		/// <returns>Id of thisPerson</returns>
		public int AddAPerson(
			int CustomerId,
			string Title,
			string FirstName,
			string LastName,
			string CommentsFromName,
			string DaytimePhone,
			string DaytimeFax,
			string AfterHoursPhone,
			string AfterHoursFax,
			string MobilePhone,
			string Email,
			int thisPremiseId,
			poBoxType POBoxType,
			string BuildingName,
			string UnitNumber,
			string Number,
			string StreetAddress,
			string PostCode,
			string Suburb,
			string CityName,
			string StateName,
			int CountryId)
		{
			Random randomNum = new Random();

			//Is this thisPerson already in the DB?
			string thisPositionInCompany = "";
			string thisUserName = LastName.Trim();

			if (thisUserName.Length > 5)
				thisUserName = thisUserName.Substring(0, 5).Trim();

			int numDigitsToFill = 7 - thisUserName.Length;

			while (numDigitsToFill > 0)
			{
				thisUserName += GetRandomNum();
				numDigitsToFill = 7 - thisUserName.Length;
			}

			string thisPassword = GetRandomChar() + GetRandomNum() + GetRandomChar() + GetRandomChar() + GetRandomChar() 
				+ GetRandomNum() + GetRandomNum();

			int CityId = 0;
			string CountryName = "New Zealand";

			int thisPreferredContactMethod = correspondenceMedium_mail();

			if (Email != "")
				thisPreferredContactMethod = correspondenceMedium_email();

			string thisComment = "";
			int thisIsCustomerAdmin = 0;
						
			//Get Id, if any present
			int thisPersonId = GetPersonsId(FirstName, LastName, CustomerId);

			//Update this thisPerson with any information we have
			string[,] thisPhone = new string[phoneNumberType_other(), 
				phoneNumberPart_description()];

			string this_Int = "";
			string this_NatMob = "";
			string this_Main = "";
			string this_Ext = "";

			#region Phone Numbers

			//Add Daytime Phone Number
			GetPhoneNumber(
				DaytimePhone,
				out this_Int,
				out this_NatMob,
				out this_Main,
				out this_Ext);

			if (thisPersonId > 0)
			{
				int numPhoneNumbers = thisPhoneNumber.GetByPersonIdPhoneNumberType(thisPersonId, 
					phoneNumberType_daytimePhone());

				if (numPhoneNumbers > 0)
				{
					thisPhoneNumber.Modify(
						thisPhoneNumber.my_PhoneNumberId(0),
						this_Int,
						this_NatMob,
						this_Main,
						this_Ext,
						phoneNumberType_daytimePhone(),
						"",
						"tblPerson",
						thisPersonId,
						systemUserId);
				}
				else
				{
					thisPhoneNumber.Add(
						this_Int,
						this_NatMob,
						this_Main,
						this_Ext,
						phoneNumberType_daytimePhone(),
						"",
						"tblPerson",
						thisPersonId,
						systemUserId);
				}
			}
			else 
			{
				thisPhone[phoneNumberType_daytimePhone(),
					phoneNumberPart_internationalPrefix()] = this_Int;
				thisPhone[phoneNumberType_daytimePhone(),
					phoneNumberPart_nationalOrMobilePrefix()] = this_NatMob;
				thisPhone[phoneNumberType_daytimePhone(),
					phoneNumberPart_mainNumber()] = this_Main;
				thisPhone[phoneNumberType_daytimePhone(),
					phoneNumberPart_extension()] = this_Ext;
			}

	
			GetPhoneNumber(
				DaytimeFax,
				out this_Int,
				out this_NatMob,
				out this_Main,
				out this_Ext);

			if (thisPersonId > 0)
			{
				int numPhoneNumbers = thisPhoneNumber.GetByPersonIdPhoneNumberType(thisPersonId, 
					phoneNumberType_daytimeFax());

				if (numPhoneNumbers > 0)
				{
				thisPhoneNumber.Modify(
					thisPhoneNumber.my_PhoneNumberId(0),
					this_Int,
					this_NatMob,
					this_Main,
					this_Ext,
					phoneNumberType_daytimeFax(),
					"",
					"tblPerson",
					thisPersonId,
					systemUserId);
				}
				else
				{
					thisPhoneNumber.Add(
						this_Int,
						this_NatMob,
						this_Main,
						this_Ext,
						phoneNumberType_daytimeFax(),
						"",
						"tblPerson",
						thisPersonId,
						systemUserId);
				}
			}
			else 
			{
				thisPhone[phoneNumberType_daytimeFax(),
					phoneNumberPart_internationalPrefix()] = this_Int;
				thisPhone[phoneNumberType_daytimeFax(),
					phoneNumberPart_nationalOrMobilePrefix()] = this_NatMob;
				thisPhone[phoneNumberType_daytimeFax(),
					phoneNumberPart_mainNumber()] = this_Main;
				thisPhone[phoneNumberType_daytimeFax(),
					phoneNumberPart_extension()] = this_Ext;
			}


			//Add afterHours Phone Number
			GetPhoneNumber(
				AfterHoursPhone,
				out this_Int,
				out this_NatMob,
				out this_Main,
				out this_Ext);

			if (thisPersonId > 0)
			{
				int numPhoneNumbers = thisPhoneNumber.GetByPersonIdPhoneNumberType(thisPersonId, 
					phoneNumberType_afterHoursPhone());

				if (numPhoneNumbers > 0)
				{
					thisPhoneNumber.Modify(
						thisPhoneNumber.my_PhoneNumberId(0),
						this_Int,
						this_NatMob,
						this_Main,
						this_Ext,
						phoneNumberType_afterHoursPhone(),
						"",
						"tblPerson",
						thisPersonId,
						systemUserId);
				}
				else
				{
					thisPhoneNumber.Add(
						this_Int,
						this_NatMob,
						this_Main,
						this_Ext,
						phoneNumberType_afterHoursPhone(),
						"",
						"tblPerson",
						thisPersonId,
						systemUserId);
				}
			}
			else 
			{
				thisPhone[phoneNumberType_afterHoursPhone(),
					phoneNumberPart_internationalPrefix()] = this_Int;
				thisPhone[phoneNumberType_afterHoursPhone(),
					phoneNumberPart_nationalOrMobilePrefix()] = this_NatMob;
				thisPhone[phoneNumberType_afterHoursPhone(),
					phoneNumberPart_mainNumber()] = this_Main;
				thisPhone[phoneNumberType_afterHoursPhone(),
					phoneNumberPart_extension()] = this_Ext;
			}

	
			GetPhoneNumber(
				AfterHoursFax,
				out this_Int,
				out this_NatMob,
				out this_Main,
				out this_Ext);

				thisPhone[phoneNumberType_afterHoursFax(),
					phoneNumberPart_internationalPrefix()] = this_Int;
				thisPhone[phoneNumberType_afterHoursFax(),
					phoneNumberPart_nationalOrMobilePrefix()] = this_NatMob;
				thisPhone[phoneNumberType_afterHoursFax(),
					phoneNumberPart_mainNumber()] = this_Main;
				thisPhone[phoneNumberType_afterHoursFax(),
					phoneNumberPart_extension()] = this_Ext;



	
			GetPhoneNumber(
				MobilePhone,
				out this_Int,
				out this_NatMob,
				out this_Main,
				out this_Ext);

			if (thisPersonId > 0)
			{
				int numPhoneNumbers = thisPhoneNumber.GetByPersonIdPhoneNumberType(thisPersonId, 
					phoneNumberType_mobilePhone());

				if (numPhoneNumbers > 0)
				{
					thisPhoneNumber.Modify(
						thisPhoneNumber.my_PhoneNumberId(0),
						this_Int,
						this_NatMob,
						this_Main,
						this_Ext,
						phoneNumberType_mobilePhone(),
						"",
						"tblPerson",
						thisPersonId,
						systemUserId);
				}
				else
				{
					thisPhoneNumber.Add(
						this_Int,
						this_NatMob,
						this_Main,
						this_Ext,
						phoneNumberType_mobilePhone(),
						"",
						"tblPerson",
						thisPersonId,
						systemUserId);
				}

			}
			else 
			{
				thisPhone[phoneNumberType_mobilePhone(),
					phoneNumberPart_internationalPrefix()] = this_Int;
				thisPhone[phoneNumberType_mobilePhone(),
					phoneNumberPart_nationalOrMobilePrefix()] = this_NatMob;
				thisPhone[phoneNumberType_mobilePhone(),
					phoneNumberPart_mainNumber()] = this_Main;
				thisPhone[phoneNumberType_mobilePhone(),
					phoneNumberPart_extension()] = this_Ext;
			}

			#endregion

			if (thisPersonId == 0)
			{

				thisPerson.Add(CustomerId,
					Title,
					FirstName,
					LastName,
					thisUserName,
					thisPassword,
					thisPositionInCompany,

					thisPhone[phoneNumberType_daytimePhone(),phoneNumberPart_internationalPrefix()],
					thisPhone[phoneNumberType_daytimePhone(),phoneNumberPart_nationalOrMobilePrefix()],
					thisPhone[phoneNumberType_daytimePhone(),phoneNumberPart_mainNumber()],
					thisPhone[phoneNumberType_daytimePhone(),phoneNumberPart_extension()],

					thisPhone[phoneNumberType_daytimeFax(),phoneNumberPart_internationalPrefix()],
					thisPhone[phoneNumberType_daytimeFax(),phoneNumberPart_nationalOrMobilePrefix()],
					thisPhone[phoneNumberType_daytimeFax(),phoneNumberPart_mainNumber()],
					thisPhone[phoneNumberType_daytimeFax(),phoneNumberPart_extension()],

					thisPhone[phoneNumberType_afterHoursPhone(),phoneNumberPart_internationalPrefix()],
					thisPhone[phoneNumberType_afterHoursPhone(),phoneNumberPart_nationalOrMobilePrefix()],
					thisPhone[phoneNumberType_afterHoursPhone(),phoneNumberPart_mainNumber()],
					thisPhone[phoneNumberType_afterHoursPhone(),phoneNumberPart_extension()],

					thisPhone[phoneNumberType_afterHoursFax(),phoneNumberPart_internationalPrefix()],
					thisPhone[phoneNumberType_afterHoursFax(),phoneNumberPart_nationalOrMobilePrefix()],
					thisPhone[phoneNumberType_afterHoursFax(),phoneNumberPart_mainNumber()],
					thisPhone[phoneNumberType_afterHoursFax(),phoneNumberPart_extension()],

					thisPhone[phoneNumberType_mobilePhone(),phoneNumberPart_internationalPrefix()],
					thisPhone[phoneNumberType_mobilePhone(),phoneNumberPart_nationalOrMobilePrefix()],
					thisPhone[phoneNumberType_mobilePhone(),phoneNumberPart_mainNumber()],
					thisPhone[phoneNumberType_mobilePhone(),phoneNumberPart_extension()],
					
					(int) POBoxType,
					BuildingName,
					UnitNumber,
					Number,
					StreetAddress,
					Suburb,
					CityId,
					CityName,
					StateName,
					CountryId,
					CountryName,
					PostCode,

					Email,
					thisPreferredContactMethod,
					thisComment,
					CommentsFromName,
					thisIsCustomerAdmin,
					
					systemUserId);

				thisPerson.Save();
				thisPersonId = thisPerson.LastIdAdded();

			}
			else 
			{
				thisPhoneNumber.Save();
				thisPerson.Save();

				int numAddresses = thisAddress.GetByPersonId(thisPersonId);
				if (numAddresses > 0)
				{
					thisAddress.Modify(thisAddress.my_AddressId(0),
						(int) POBoxType,
						BuildingName,
						UnitNumber,
						Number,
						StreetAddress,
						Suburb,
						CityId,
						CityName,
						StateName,
						CountryId,
						CountryName,
						PostCode,
						addressType_postal(),
						"",
						"tblPerson",
						thisPersonId,
						systemUserId);

					thisAddress.Save();
				}
				else
				{
					thisAddress.Add((int) POBoxType,
						BuildingName,
						UnitNumber,
						Number,
						StreetAddress,
						Suburb,
						CityId,
						CityName,
						StateName,
						CountryId,
						CountryName,
						PostCode,
						addressType_postal(),
						"",
						"tblPerson",
						thisPersonId,
						systemUserId);

					thisAddress.Save();
				}
			}

			//Add this Persons Roles with the thisPremise
			return thisPersonId;
		}

		#endregion

		#region AddServiceProvider
		
	
		/// <summary>
		/// Takes Service Provider information and attempts to match this to data in the 
		/// database. If an existing person can not be matched up to this data,
		/// a new entry is created.
		/// </summary>
		/// <param name="thisPremiseId">thisPremiseId</param>
		/// <param name="ServiceProviderName">SecurityCompanyName</param>
		/// <param name="DaytimePhone">DaytimePhone</param>
		/// <param name="thisCustomerId">thisCustomerId</param>
		public int AddServiceProvider(int thisPremiseId,
			string ServiceProviderName,
			string DaytimePhone,
			int thisCustomerId)
		{
			string this_Int = "";
			string this_NatMob = "";
			string this_Main = "";
			string this_Ext = "";			
			
			string[,] thisPhone = new string[phoneNumberType_other(), 
				phoneNumberPart_description()];	

			//Check for numbers after the name
			int firstNumericalDigit = GetIndexFirstNumeral(ServiceProviderName);
			int lastNumericalDigit = GetIndexLastNumeral(ServiceProviderName);

			string AfterHoursPhone = "";

			//If there is a number, this is a phone number
			if (lastNumericalDigit > - 1)
			{
				AfterHoursPhone = ServiceProviderName.Substring(firstNumericalDigit, lastNumericalDigit - firstNumericalDigit + 1).Trim();
				ServiceProviderName = ServiceProviderName.Substring(0, firstNumericalDigit - 1).Trim();
			}

			if (DaytimePhone == "")
				DaytimePhone = AfterHoursPhone;

			if (AfterHoursPhone == "")
				AfterHoursPhone = DaytimePhone;
		
			//Is this thisServiceProvider already in the DB?
			string thisComment = "";
			int thisIsSecurityCompany = 1;
			string thisServiceProviderFirstName = "";
			string thisServiceProviderLastName = "";
			int thisServiceProviderId = 0;
			string commentsFromName = "";		
			string thisNoData = "";

			thisServiceProviderId = GetServiceProviderId(ServiceProviderName);
					
			if (thisServiceProviderId != 0)
			{
				//Set the CustomerId for this Service Provider to be 0
//				int numSps = thisServiceProvider.GetByServiceProviderId(thisServiceProviderId);
//
//				for(int spCounter = 0; spCounter < numSps; spCounter++)
//					thisServiceProvider.Modify(thisServiceProviderId,
//						0,
//						thisServiceProvider.my_Title(spCounter),
//						thisServiceProvider.my_FirstName(spCounter),
//						thisServiceProvider.my_LastName(spCounter),
//						thisServiceProvider.my_CompanyName(spCounter),
//						thisServiceProvider.my_IsSecurityCompany(spCounter),
//						thisServiceProvider.my_QuickDaytimePhone(spCounter), 
//						thisServiceProvider.my_QuickDaytimeFax(spCounter), 
//						thisServiceProvider.my_QuickAfterHoursPhone(spCounter), 
//						thisServiceProvider.my_QuickAfterHoursFax(spCounter), 
//						thisServiceProvider.my_QuickMobilePhone(spCounter), 
//						thisServiceProvider.my_Email(spCounter),
//						thisServiceProvider.my_KdlComments(spCounter),
//						thisServiceProvider.my_CustomerComments(spCounter),
//						systemUserId);
//				thisServiceProvider.Save();
				
			}
			else
			{
				
				#region Add Daytime Phone Number
				GetPhoneNumber(
					DaytimePhone,
					out this_Int,
					out this_NatMob,
					out this_Main,
					out this_Ext);

				if ((this_Int + this_NatMob + this_Main + this_Ext).Length == 0)
				{
					//Nothing supplied, use existing data if it exists
					thisPhone[phoneNumberType_daytimePhone(),
						phoneNumberPart_internationalPrefix()] = 
						thisNoData;
					thisPhone[phoneNumberType_daytimePhone(),
						phoneNumberPart_nationalOrMobilePrefix()] = 
						thisNoData;
					thisPhone[phoneNumberType_daytimePhone(),
						phoneNumberPart_mainNumber()] = 
						thisNoData;
					thisPhone[phoneNumberType_daytimePhone(),
						phoneNumberPart_extension()] = 
						thisNoData;
				}
				else
				{
					thisPhone[phoneNumberType_daytimePhone(),
						phoneNumberPart_internationalPrefix()] = this_Int;
					thisPhone[phoneNumberType_daytimePhone(),
						phoneNumberPart_nationalOrMobilePrefix()] = this_NatMob;
					thisPhone[phoneNumberType_daytimePhone(),
						phoneNumberPart_mainNumber()] = this_Main;
					thisPhone[phoneNumberType_daytimePhone(),
						phoneNumberPart_extension()] = this_Ext;

				}
				#endregion

				#region Add After Hours Phone Number
				GetPhoneNumber(
					AfterHoursPhone,
					out this_Int,
					out this_NatMob,
					out this_Main,
					out this_Ext);

				if ((this_Int + this_NatMob + this_Main + this_Ext).Length == 0)
				{
					//Nothing supplied, use existing data if it exists
					thisPhone[phoneNumberType_afterHoursPhone(),
						phoneNumberPart_internationalPrefix()] = 
						thisNoData;
					thisPhone[phoneNumberType_afterHoursPhone(),
						phoneNumberPart_nationalOrMobilePrefix()] = 
						thisNoData;
					thisPhone[phoneNumberType_afterHoursPhone(),
						phoneNumberPart_mainNumber()] = 
						thisNoData;
					thisPhone[phoneNumberType_afterHoursPhone(),
						phoneNumberPart_extension()] = 
						thisNoData;
				}
				else
				{
					thisPhone[phoneNumberType_afterHoursPhone(),
						phoneNumberPart_internationalPrefix()] = this_Int;
					thisPhone[phoneNumberType_afterHoursPhone(),
						phoneNumberPart_nationalOrMobilePrefix()] = this_NatMob;
					thisPhone[phoneNumberType_afterHoursPhone(),
						phoneNumberPart_mainNumber()] = this_Main;
					thisPhone[phoneNumberType_afterHoursPhone(),
						phoneNumberPart_extension()] = this_Ext;

				}
				#endregion


				thisServiceProvider.Add(0,
					"",
					thisServiceProviderFirstName,
					thisServiceProviderLastName,
					ServiceProviderName,
					
					thisIsSecurityCompany,
					"",
					thisComment,
					commentsFromName,

					thisPhone[phoneNumberType_daytimePhone(),phoneNumberPart_internationalPrefix()],
					thisPhone[phoneNumberType_daytimePhone(),phoneNumberPart_nationalOrMobilePrefix()],
					thisPhone[phoneNumberType_daytimePhone(),phoneNumberPart_mainNumber()],
					thisPhone[phoneNumberType_daytimePhone(),phoneNumberPart_extension()],
					
					thisNoData,
					thisNoData,
					thisNoData,
					thisNoData,

					thisPhone[phoneNumberType_afterHoursPhone(),phoneNumberPart_internationalPrefix()],
					thisPhone[phoneNumberType_afterHoursPhone(),phoneNumberPart_nationalOrMobilePrefix()],
					thisPhone[phoneNumberType_afterHoursPhone(),phoneNumberPart_mainNumber()],
					thisPhone[phoneNumberType_afterHoursPhone(),phoneNumberPart_extension()],

					thisNoData,
					thisNoData,
					thisNoData,
					thisNoData,

					thisNoData,
					thisNoData,
					thisNoData,
					thisNoData,
					systemUserId);

				thisServiceProvider.Save();
				thisServiceProviderId = thisServiceProvider.LastIdAdded();
			}
			return thisServiceProviderId;
		}

		#endregion

		#endregion

		#region Get meaningful information form Data

		#region Get Ref'ed Properties

		/// <summary>
		/// GetReferencedProperties
		/// </summary>
		/// <param name="kdlRef">kdlRef</param>
		/// <returns>ArrayList</returns>
		public ArrayList GetReferencedProperties(string kdlRef)
		{
			ArrayList thisList = new ArrayList();
//			int thisSemiColonPos = kdlRef.IndexOf(";");
//			string thisWorking = kdlRef;
//			while (thisSemiColonPos > -1 && thisWorking.Length > 1)
//			{
//				//Add first number to reference stack
//				kdlRefPropertyStruct thisRef = new kdlRefPropertyStruct();
//				thisRef.StickerNum = thisWorking.Substring(0, thisSemiColonPos).Trim();
//				int numMatches = thisPremise.GetByNumber(
//					thisRef.StickerNum , 
//					matchCriteria_exactMatch());
//				if (numMatches > 0)
//				{
//					thisRef.PremiseId = thisPremise.my_PremiseId(0);
//					thisRef.CustomerGroupId = thisPremise.my_Customer_CustomerGroupId(0);
//					thisRef.CustomerId = thisPremise.my_CustomerId(0);
//					thisRef.CustomerFullName = thisPremise.my_Customer_FullName(0);
//				}
//				else
//				{
//					thisRef.PremiseId = 0;
//					thisRef.CustomerGroupId = 0;
//					thisRef.CustomerId = 0;
//					thisRef.CustomerFullName = "";
//				}
//
//				thisList.Add(thisRef);
//				thisWorking = thisWorking.Substring(thisSemiColonPos + 1).Trim();
//				thisSemiColonPos = thisWorking.IndexOf(";");
//			}
//
//			//Might be one left after the last semi colon
//			if (thisWorking != "")
//			{
//				kdlRefPropertyStruct thisRef2 = new kdlRefPropertyStruct();
//				thisRef2.StickerNum = thisWorking.Trim();
//				int numMatches = thisPremise.GetByNumber(
//					thisRef2.StickerNum , 
//					matchCriteria_exactMatch());
//				if (numMatches > 0)
//				{
//					thisRef2.PremiseId = thisPremise.my_PremiseId(0);
//					thisRef2.CustomerGroupId = thisPremise.my_Customer_CustomerGroupId(0);
//					thisRef2.CustomerId = thisPremise.my_CustomerId(0);
//					thisRef2.CustomerFullName = thisPremise.my_Customer_FullName(0);
//				}
//				else
//				{
//					thisRef2.PremiseId = 0;
//					thisRef2.CustomerGroupId = 0;
//					thisRef2.CustomerId = 0;
//					thisRef2.CustomerFullName = "";
//				}
//				thisList.Add(thisRef2);
//			}

			return thisList;

		}

		#endregion

		#region GetNames


		/// <summary>
		/// Get FirstNames, LastNames and Comments from Names from the Old Name in DB
		/// </summary>
		/// <param name="personName">Supplied name from Old Db</param>
		/// <param name="personTitle">thisPerson's Title</param>
		/// <param name="personFirstName">thisPerson's First Name</param>
		/// <param name="personLastName">thisPerson's Last Name</param>
		/// <param name="commentsFromName">Comments from the Name</param>
		public void GetNames(string personName, 
			out string personTitle,
			out string personFirstName,
			out string personLastName,
			out string commentsFromName)
		{
			personName = personName.Replace("-", " ");
			personName = personName.Replace("[", "(");
			personName = personName.Replace("]", ")");
			personName = personName.Replace("0", "o");

			string oldPersonName;
			personTitle = "";
			personFirstName = "";
			personLastName = "";
			commentsFromName = "";

			string[] theseTitles = {"Mr", "Miss", "Ms", "Mrs", "Dr", "Prof", "Rev"};
			
			bool TitleFound = false;

			foreach(string thisTitle in theseTitles)
			{
				//Check for Titles
				if (!TitleFound && personName.StartsWith(thisTitle + " "))
				{
					personTitle = thisTitle;
					personName = personName.Substring((thisTitle + " ").Length).Trim();
					TitleFound = true;
				}
			}



			if (personName.Length < 3)
				return;

			int instanceOfOpeningBracket = personName.IndexOf("(");
			int instanceOfMoreThanTwoSpaces = personName.IndexOf("  ");

			int earliestSeparator = 0;

			if (instanceOfOpeningBracket > 0) 
			{
				earliestSeparator = instanceOfOpeningBracket;
				if ((instanceOfMoreThanTwoSpaces > 0) && (instanceOfMoreThanTwoSpaces < earliestSeparator))
					earliestSeparator = instanceOfMoreThanTwoSpaces;
			}
			else
			{
				if (instanceOfMoreThanTwoSpaces > 0)
					earliestSeparator = instanceOfMoreThanTwoSpaces;
			}

			
			//There are often comments after someone's name, we can tell this
			//if there are two spaces or brackets
			oldPersonName = personName;

			if (earliestSeparator > 0)
			{
				commentsFromName = oldPersonName.Substring(earliestSeparator).Trim();
				personName = oldPersonName.Substring(0, earliestSeparator).Trim();
			}

			int instanceOfLastSpace = personName.LastIndexOf(" ");
			if (instanceOfLastSpace > 0)
			{
				personFirstName = personName.Substring(0, instanceOfLastSpace).Trim();
				personLastName = personName.Substring(instanceOfLastSpace).Trim();
			}
			else
				personLastName = personName.Trim();

			personFirstName = GetRidOfDoubleSpaces(personFirstName);
			personLastName = GetRidOfDoubleSpaces(personLastName);
			commentsFromName = GetRidOfDoubleSpaces(commentsFromName);

		}

		#endregion

		#region GetServiceProviderId
		
		/// <summary>
		/// Given a ServiceProviderName from the Old Database, this function tries to
		/// identify if this thisServiceProvider already associated with this customer.
		/// If it is, then the ID of the already existing thisServiceProvider is returned
		/// Otherwise a thisServiceProvider is created, and this is added, and the
		/// Id of this entry is added.
		/// </summary>
		/// <param name="ServiceProviderName">Name of thisServiceProvider from old DB</param>
		/// <returns>Id of thisServiceProvider</returns>
		public int GetServiceProviderId(string ServiceProviderName)
		{
			//Is this thisServiceProvider already in the DB?

			int thisServiceProviderId = 0;
			string commentsFromName = "";

			if (ServiceProviderName.Replace("-", " ").Trim().Length < 5)
				return 0;			
			
			int instanceOfMoreThanTwoSpaces = ServiceProviderName.IndexOf("   ");
						
			if (instanceOfMoreThanTwoSpaces > 0)
			{
				//There are often comments after someone's name, we can tell this
				//if there are two spaces
				ServiceProviderName = ServiceProviderName.Substring(0, instanceOfMoreThanTwoSpaces).Trim();
				commentsFromName = ServiceProviderName.Substring(instanceOfMoreThanTwoSpaces).Trim();
			}
	
			int numDuplicateServiceProviders = thisServiceProvider.GetByName(
				ServiceProviderName);
			
			for(int counter = 0; counter < numDuplicateServiceProviders; counter++)
			{
				if (thisServiceProvider.my_CompanyName(counter) == ServiceProviderName)
					thisServiceProviderId = thisServiceProvider.my_ServiceProviderId(counter);
			}

			return thisServiceProviderId;
		}

		#endregion

		#region GetPersonsId

		/// <summary>
		/// Given a PersonName from the Old Database, this function tries to
		/// identify if this person already associated with this customer.
		/// If it is, then the ID of the already existing thisPerson is returned
		/// Otherwise a thisPerson is created, and this is added, and the
		/// Id of this entry is added.
		/// </summary>
		/// <param name="personFirstName">thisPerson's First Name</param>
		/// <param name="personLastName">thisPerson's Last Name</param>
		/// <param name="CustomerId">Id of Associated thisCustomer</param>
		/// <returns>Id of thisPerson</returns>
		public int GetPersonsId(string personFirstName, 
			string personLastName,
			int CustomerId)
		{

			int numDuplicatePeople = thisPerson.GetByPersonNameForCustomerId(CustomerId,
				personFirstName + " " + personLastName, 
				matchCriteria_exactMatch(),
				thisPerson.fieldsToMatch_firstAndLastNamesOnly());
			
			if (numDuplicatePeople > 0)
			{
				thisPerson.GetByPersonId(thisPerson.my_PersonId(0)); //Have to get all attributes of this thisPerson
				return thisPerson.my_PersonId(0);
			}
			else
			{
				return 0;
	
			}

		}

		#endregion

		#region GetPhoneNumber

		/// <summary>
		/// Returns Phone Number information in a format from which it can be added to the Database
		/// </summary>
		/// <param name="containingPhoneNumber"></param>
		/// <param name="this_Int">International Prefix</param>
		/// <param name="this_NatMob">National/mobilePhone Prefix</param>
		/// <param name="this_Main">Main Number</param>
		/// <param name="this_Ext">Extention</param>
		public void GetPhoneNumber(
			string containingPhoneNumber,
			out string this_Int,
			out string this_NatMob,
			out string this_Main,
			out string this_Ext)
		{
			this_Int = "";
			this_NatMob = "";
			this_Main = "";
			this_Ext = "";
			
			ArrayList spaceList = new ArrayList();

			containingPhoneNumber = containingPhoneNumber.Replace("-", " ");

			int nextSpace = containingPhoneNumber.IndexOf(" ");
			while (nextSpace != -1)
			{
				spaceList.Add(nextSpace);
				nextSpace = containingPhoneNumber.IndexOf(" ", nextSpace + 1);
			}

			//Look at total number of characters
			if (containingPhoneNumber.Length > 8)
			{
				//Look for first space
				switch (spaceList.Count)
				{
					case 0: //No spaces: All in Main Number
						this_Main = containingPhoneNumber.Trim();
						break;
					case 1: //1 Space: Nat/Mob prefix + Main Number
					case 2: //2 Spaces: Nat/Mob prefix  + Main Number (includes space)
						this_NatMob = 
							containingPhoneNumber.Substring(0, 
							Convert.ToInt32(spaceList[0])).Trim();

						this_Main = 
							containingPhoneNumber.Substring(
							Convert.ToInt32(spaceList[0])).Trim();
						break;
					case 3: //3 Spaces: Nat/Mob prefix  + Main Number (includes space) + Extension
						this_NatMob = 
							containingPhoneNumber.Substring(0, 
							Convert.ToInt32(spaceList[0])).Trim();

						this_Main = 
							containingPhoneNumber.Substring(Convert.ToInt32(spaceList[0]),
							Convert.ToInt32(spaceList[2]) - Convert.ToInt32(spaceList[0])).Trim();
						
						this_Ext = 
							containingPhoneNumber.Substring(
							Convert.ToInt32(spaceList[2])).Trim();						
						break;
					default:
						this_NatMob = 
							containingPhoneNumber.Substring(0, 
							Convert.ToInt32(spaceList[0])).Trim();

						this_Main = 
							containingPhoneNumber.Substring(Convert.ToInt32(spaceList[0]),
							Convert.ToInt32(spaceList[spaceList.Count - 1]) - Convert.ToInt32(spaceList[1])).Trim();
						
						this_Ext = 
							containingPhoneNumber.Substring(
							Convert.ToInt32(spaceList[spaceList.Count - 1])).Trim();	
						break;

				}
				
			}

		}

		#endregion

		#region ProductIdToUse

		/// <summary>
		/// Service Level to Assign Customers being imported
		/// </summary>
		public int ProductIdToUse
		{
			get
			{	
				return thisProductId;
			}
			set
			{
				thisProductId = value;
			}
		}

		#endregion 

		#endregion

 
		#region Old Method for Sending Email

//        /// <summary>
//        /// Sends an email
//        /// </summary>
//        /// <param name="ToAddress">Destination Email Address</param>
//        /// <param name="FromAddress">Who the email appears from</param>
//        /// <param name="ReplyToAddress">Who gets the email if the recipient presses reply</param>
//        /// <param name="Subject">Subject Line</param>
//        /// <param name="Priority">Pirority: 0 = Normal; 1 = High; 2 = Low</param>
//        /// <param name="Format">Formal: 0 = Text; 1 = HTML</param>
//        /// <param name="Message">Message</param>
//        /// <param name="SmtpServer">SmtpServer</param>
//        /// <param name="Attachments">Attachments</param>
//        public void SendEmail(
//            string ToAddress,
//            string FromAddress, 
//            string ReplyToAddress, 
//            string Subject, 
//            int Priority,
//            int Format,
//            string Message,
//            string SmtpServer,
//            System.Collections.ArrayList Attachments)
//        {

//            SmtpMail.SmtpServer = SmtpServer;

//            MailMessage mail = new MailMessage();
//            mail.To = ToAddress;
//            mail.Cc = "";
//            mail.Bcc = "";
//            mail.From = FromAddress;
//            //			mail.Headers.Add("Reply-To", ReplyToAddress);
//            mail.Subject = Subject;
//            mail.Priority = (System.Net.Mail.MailPriority) Priority;
//            mail.BodyFormat = (System.Net.Mail.MailFormat) Format;
//            mail.Body = Message;

//            if (Attachments != null && Attachments.Count > 0)
//            {
//                for(int counter = 0; counter < Attachments.Count; counter++)
//                {
//                    string thisFile = Attachments[counter].ToString();
//                    if(System.IO.File.Exists(thisFile))
//                    {
//                        MailAttachment attach = new MailAttachment(thisFile);
//                        mail.Attachments.Add(attach);
//                    }				
//                }
//            }

//            try
//            {
//                SmtpMail.Send(mail);
//            }
//            catch(System.SystemException e)
//            {
//                string Response = e.ToString();
//                int RaisedError = 901;

//                string Error = "Error Message: " + e.Message + crLf
//                    + "Stack Trace: " + e.StackTrace + crLf
//                    + "ThisSmtpServer: " + thisSmtpServer + crLf
//                    + "Mail: " + mail.ToString() + crLf
//                    + "Whole exception: " + e.ToString()
//                    ;

////				LogError(SmtpServer, Error, RaisedError);
//            }


//        }


//        /// <summary>
//        /// Sends an email
//        /// </summary>
//        /// <param name="ToAddress">Destination Email Address</param>
//        /// <param name="FromAddress">Who the email appears from</param>
//        /// <param name="ReplyToAddress">Who gets the email if the recipient presses reply</param>
//        /// <param name="Subject">Subject Line</param>
//        /// <param name="Priority">Pirority: 0 = Normal; 1 = High; 2 = Low</param>
//        /// <param name="Format">Formal: 0 = Text; 1 = HTML</param>
//        /// <param name="Message">Message</param>
//        /// <param name="Attachments">Attachments</param>
//        public void SendEmail(
//            string ToAddress,
//            string FromAddress, 
//            string ReplyToAddress, 
//            string Subject, 
//            int Priority,
//            int Format,
//            string Message,
//            System.Collections.ArrayList Attachments)
//        {
//            GetGeneralSettings();

//            if (thisSmtpServer == "")
//            {
//                Setting.Ensure("thisSmtpServer", "smtp.unleash.co.nz");
//                GetGeneralSettings();

//            }

//            SendEmail(
//                ToAddress,
//                FromAddress, 
//                ReplyToAddress, 
//                Subject, 
//                Priority,
//                Format,
//                Message,
//                thisSmtpServer,
//                Attachments);

//        }

//        /// <summary>
//        /// Sends an email
//        /// </summary>
//        /// <param name="ToAddress">Destination Email Address</param>
//        /// <param name="FromAddress">Who the email appears from</param>
//        /// <param name="ReplyToAddress">Who gets the email if the recipient presses reply</param>
//        /// <param name="Subject">Subject Line</param>
//        /// <param name="Priority">Pirority: 0 = Normal; 1 = High; 2 = Low</param>
//        /// <param name="Format">Formal: 0 = Text; 1 = HTML</param>
//        /// <param name="Message">Message</param>
//        public void SendEmail(
//            string ToAddress,
//            string FromAddress, 
//            string ReplyToAddress, 
//            string Subject, 
//            int Priority,
//            int Format,
//            string Message)
//        {
//            SendEmail(ToAddress,
//                FromAddress, 
//                ReplyToAddress, 
//                Subject, 
//                Priority,
//                Format,
//                Message,
//                null);

//        }

		#endregion


		#region Issue Credit Note for those that need it

		/// <summary>
		/// Returns Apologetic Email
		/// </summary>
		/// <param name="PersonName">PersonName</param>
		/// <returns></returns>
		public string GetEmail(string PersonName)
		{
			string retVal = @"<html><head>" + crLf
				+  @"<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" /><style>" + crLf
				+  @"body" + crLf
				+  @"{" + crLf
				+  @"	font-family:verdana;" + crLf
				+  @"	font-size:11px;	" + crLf
				+  @"}" + crLf
				+  @"</style></head><body>Dear " + PersonName + ",<br/>	" + crLf
				+  @"<br/>" + crLf
				+  @"Please accept our sincerest apologies for the error in sending you duplicate invoices, in our last correspondence.<br/>" + crLf
				+  @"<br/>" + crLf
				+  @"You will see from the attached statement we have credited you with the amount of the duplicate, to correct this error.<br/>" + crLf
				+  @"<br/>" + crLf
				+  @"If you have any questions, please feel free to contact us on 0800 201 201, or email us at <a href=""mailto:admin@alertus.co.nz"">admin@alertus.co.nz</a>.<br/>" + crLf
				+  @"<br/>" + crLf
				+  @"With Alertus, safer property starts here.<br/>" + crLf
				+  @"<br/>" + crLf
				+  @"Kind Regards<br/>" + crLf
				+  @"<br/>" + crLf
				+  @"The Alertus Team" + crLf
				+  @"<br/><br/>" + crLf
				+  @"" + crLf
				+  @"<b>AlertUs Ltd</b><br/>" + crLf
				+  @"<span style=""font-family:verdana;font-style:italic;font-size:10px;color:#666;font-weight:500;"">When no-one knows how to find you, we do!</span>" + crLf
				+  @"<br/><br/>" + crLf
				+  @"<b>0800 201 201</b><br/>" + crLf
				+  @"<a style=""color:navy;"" href=""http://www.alertus.co.nz"">www.alertus.co.nz</a>" + crLf
				+  @"</body>" + crLf
				+  @"</html>" + crLf
				;

			return retVal;

		}

		/// <summary>
		/// CreditNote
		/// </summary>
		/// <returns>Number of Emails sent</returns>
		public int CreditNote()
		{
			int numEmails = 0;

			#region Build arraylist of Email addresses of people who've recieved correct invoices

			ArrayList theseEmails = new ArrayList();
		
			clsCsvReader thisReader = new clsCsvReader(@"c:\_EmailLog.txt");

			int numRecords = 0;
			string[] thisLine = thisReader.GetCsvLine();
			string oldEmail = "";

			while (thisLine != null && thisLine.Length == 2) //For each entry in the file
			{
				string thisEmail = thisLine[1].ToString();
				if (oldEmail != thisEmail)
				{
					theseEmails.Add(thisEmail);

					oldEmail = thisEmail;
				}

				thisLine = thisReader.GetCsvLine();

			}

			thisReader.Dispose();

			#endregion

			#region Iterate throught the Job file looking for new entries
			thisReader = new clsCsvReader(@"c:\_Job.txt", Convert.ToChar(";"));


			numRecords = 0;
			thisLine = thisReader.GetCsvLine();
			int oldPersonId = 0;
			int oldCustomerId = 0;

			while (thisLine != null && thisLine.Length> 6) //For each entry in the file
			{
				#region Get Pertinent Data from the file

				#region Get CustomerId
				int CustomerId = 0;
				string thisTemp = thisLine[0].ToString();
				int thisSpace = thisTemp.IndexOf(" ");
				if (thisSpace > - 1)
					CustomerId = Convert.ToInt32(thisTemp.Substring(thisSpace).Trim());
				#endregion

				#region Get PersonId
				int PersonId = 0;
				thisTemp = thisLine[1].ToString();
				thisSpace = thisTemp.IndexOf(" ");
				if (thisSpace > - 1)
					PersonId = Convert.ToInt32(thisTemp.Substring(thisSpace).Trim());
				#endregion

				#region Get CorrespondenceMedium
				int CorrespondenceMedium = 0;
				thisTemp = thisLine[3].ToString();
				thisSpace = thisTemp.IndexOf(" ");
				if (thisSpace > - 1)
					CorrespondenceMedium = Convert.ToInt32(thisTemp.Substring(thisSpace).Trim());
				#endregion
				
				#region Get InvoicePresence
				int InvoicePresence = 0;
				thisTemp = thisLine[4].ToString();
				thisSpace = thisTemp.IndexOf(" ");
				if (thisSpace > - 1)
					InvoicePresence = Convert.ToInt32(thisTemp.Substring(thisSpace).Trim());
				#endregion


				#endregion

				string thisEmailAddress = "";
				string thisFullName = "";
				string thisCustomerName = "";

				#region Only worry if this is a new person

				string thisFileName = @"Statement" 
					+ CustomerId.ToString().Trim() + ".pdf"
					;


				if (oldPersonId != PersonId)
				{
					#region New Person; Get their details (email address, full name)
					clsPerson thisPerson = new clsPerson(thisDbType, localRecords.dbConnection);
					int numPeople = thisPerson.GetByPersonId(PersonId);

					if (numPeople != 0)
					{
						thisEmailAddress = thisPerson.my_Email(0);
						thisFullName = thisPerson.my_FullName(0);
						thisCustomerName = thisPerson.my_Customer_FullName(0);
					}
					#endregion

					#region Check if this person was emailled form Dev

					bool EmailledFromDev = false;

					for(int counter = 0; counter < theseEmails.Count && !EmailledFromDev; counter++)
						if (thisEmailAddress.ToLower().Trim() == theseEmails[counter].ToString().ToLower().Trim())
							EmailledFromDev = true;

					#endregion

					int numResults = 0;

					if (InvoicePresence == 1)
					{
						clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);
						int numOrders = thisOrder.GetByCustomerIdOrderDateCreated(CustomerId,
							4, 4, 2012);

						if (EmailledFromDev || CorrespondenceMedium == correspondenceMedium_mail())
						{
							#region Delete the duplicate invoice

							if (numOrders > 1)
							{
								int firstOrderId = thisOrder.my_OrderId(0);
								int thisLastOrderId = firstOrderId;
								for(int counter = 1; counter < numOrders; counter++)
								{
									int thisOrderId = thisOrder.my_OrderId(counter);
									if (thisOrderId != firstOrderId
										&& thisOrderId != thisLastOrderId)
									{
										#region Delete Items for this Order and the Order Itself
										string[] thisSql = {
															   "Delete from tblTransaction where OrderId = " + thisOrderId.ToString(),
															   "Delete from tblItem where OrderId = " + thisOrderId.ToString(),
															   "Delete from tblOrder where OrderId = " + thisOrderId.ToString() 
														   };
										localRecords.RunMultipleCommands(thisSql);

										thisLastOrderId = thisOrderId;

										#endregion
									}

								}

							}


							#endregion

						}
						else
						{
							#region Credit the account, create a statement and email the customer

							if (numOrders > 1)
							{
								if (oldCustomerId != CustomerId)
								{
									#region Add Transaction

									int thisOrderId = thisOrder.my_OrderId(1);
									decimal thisAmount = thisOrder.my_Total(1);

//									clsTransaction thisTransaction = new clsTransaction(thisDbType, localRecords.dbConnection);
//									thisTransaction.AddByVendor(CustomerId, 1, thisCustomerName,
//										"Head Office", thisOrderId, paymentMethodType_reversal(),
//										0, 0, 0, thisAmount * - 1, (new DateTime(2012, 4, 4)).ToString(), "Credit offsetting the duplicate invoice",
//										"");
//									thisTransaction.Save();

									#endregion
								}

								#region Create Statement and send email

								#region Build the Statement if we haven't already

								string OutputPath = @"C:\Work\TestReport\";

								if (oldCustomerId != CustomerId)
								{

									clsReport thisReport = new clsReport(thisDbType, localRecords.dbConnection);

									thisReport.thisReportPath = @"C:\Inetpub\wwwroot\WebApplicationTestReport\";
									thisReport.thisRootPath = OutputPath;
									thisReport.thisOutputPath = @"test";

									numResults = thisReport.Statements(CustomerId, 1, thisFileName);

								}
								#endregion

								if (numResults > 0)
								{
									ArrayList theseAttachments = new ArrayList();

									theseAttachments.Add(OutputPath + thisFileName);

//									SendEmail(thisEmailAddress,
//										"admin@alertus.co.nz",
//										"admin@alertus.co.nz",
//										"Credit Note from Alertus",
//										(int) System.Net.Mail.MailPriority.Low,
//										(int) System.Net.Mail.MailFormat.Html,
//										"",
//										"Dev1",
//										theseAttachments);

									SendEmail(thisEmailAddress,
										"admin@alertus.co.nz",
										"admin@alertus.co.nz",
										"Credit Note from Alertus",
										(int) System.Net.Mail.MailPriority.Low,
										1,
										GetEmail(thisFullName),
										"smtp.dts.net.nz",
										theseAttachments);

									numEmails++;
								}

								#endregion

							}

							#endregion

						}
					}


				}
				oldPersonId = PersonId;
				oldCustomerId = CustomerId;

				#endregion

				thisLine = thisReader.GetCsvLine();

			}
			#endregion

			return numEmails;

//
//			#region Step 1: Deal with the duplicate invoice for those who didn't recieve it
//
//				clsCsvReader thisReader = new clsCsvReader(@"c:\_EmailLog.txt");
//
//			int numRecords = 0;
//			string[] thisLine = thisReader.GetCsvLine();
//			int oldPremiseId = 0;
//
//			while (thisLine != null && thisLine.Length == 4) //For each entry in the file
//			{
//				int thisPremiseId = Convert.ToInt32(thisLine[3]);
//				if (oldPremiseId != thisPremiseId)
//				{
//					#region Check these items for duplciate invoices
//					clsItem thisItem = new clsItem(thisDbType, localRecords.dbConnection);
//
//					int numItems = thisItem.GetByPremiseIdOrderDateCreated(thisPremiseId,
//						4,4,2012);
//
//					if (numItems > 1)
//					{
//						int firstOrderId = thisItem.my_OrderId(0);
//						int thisLastOrderId = firstOrderId;
//						for(int counter = 1; counter < numItems; counter++)
//						{
//							int thisOrderId = thisItem.my_OrderId(counter);
//							if (thisOrderId != firstOrderId
//								&& thisOrderId != thisLastOrderId)
//							{
//								#region Delete Items for this Order and the Order Itself
//								string[] thisSql = {
//													   "Delete from tblItem where OrderId = " + thisOrderId.ToString(),
//													   "Delete from tblOrder where OrderId = " + thisOrderId.ToString() 
//												   };
//								localRecords.RunMultipleCommands(thisSql);
//
//								thisLastOrderId = thisOrderId;
//
//								#endregion
//							}
//
//						}
//
//					}
//					#endregion
//
//					oldPremiseId = thisPremiseId;
//				}
//
//				thisLine = thisReader.GetCsvLine();
//
//			}
//
//			#endregion
//
//			#region Step 2: Locate all those with duplicates and issue Credit Notes and Statements
//
//
//
//			#endregion


		}

		#endregion

	}

	#region clsNameSectionsFromName
	/// <summary>
	/// Class build to be able to get the Referral Status from
	/// </summary>
	public class clsNameSectionsFromName : clsKeyBase
	{
		/// <summary>
		/// Constructor for clsImportData
		/// </summary>
		public clsNameSectionsFromName(string oldFullName)
		{
			clsProperCaser thisProperCaser = new clsProperCaser(true);

			oldFullName = oldFullName.Replace("-", " ");
			oldFullName = oldFullName.Replace("[", "(");
			oldFullName = oldFullName.Replace("]", ")");
			oldFullName = oldFullName.Replace("0", "o");

			string newFullName = oldFullName;

			//Check for instances of titles
			string[] theseTitles = {"Mr", "Miss", "Ms", "Mrs", "Dr", "Prof", "Assoc Prof", "Rev", "Rt Hon", "Sir", "Dame"};
			
			bool TitleFound = false;

			foreach(string thisTitle in theseTitles)
			{
				//Check for Titles
				int appearance = oldFullName.IndexOf(thisTitle + " ");
				if (appearance == -1 && oldFullName.EndsWith(thisTitle))
					appearance = oldFullName.Length - thisTitle.Length;

				if (!TitleFound && appearance > -1)
				{
					Title = thisTitle;
					//Remove the Title from the string
					if (appearance == 0)
						newFullName = newFullName.Substring(appearance + thisTitle.Length).Trim();
					else //Reverse the name if the title was not at the beginning
						newFullName = (newFullName.Substring(appearance + thisTitle.Length) + " " + newFullName.Substring(0, appearance - 1)).Trim();

					TitleFound = true;
				}
			}

			LastName = newFullName;

			int FirstSpace = newFullName.IndexOf(" ");
			if (FirstSpace > 0)
			{
				FirstName = newFullName.Substring(0, FirstSpace).Trim();
				LastName = newFullName.Substring(FirstSpace).Trim();
			}


			newFullName = LastName;

			int LastDoubleSpace = newFullName.LastIndexOf("  ");
			if (LastDoubleSpace > 0)
			{
				LastName = newFullName.Substring(0, LastDoubleSpace).Trim();
				Comments = newFullName.Substring(LastDoubleSpace).Trim();
			}


			Title = thisProperCaser.ProperCase(Title);
			FirstName = thisProperCaser.ProperCase(FirstName);
			LastName = thisProperCaser.ProperCase(LastName);
			Comments = thisProperCaser.ProperCase(Comments);
		}


		/// <summary>
		/// Interal representation of Title
		/// </summary>
		public string Title = "";
		
		/// <summary>
		/// Interal representation FirstName
		/// </summary>
		public string FirstName = "";

		/// <summary>
		/// Interal representation LastName
		/// </summary>
		public string LastName = "";

		/// <summary>
		/// Interal representation Comments
		/// </summary>
		public string Comments = "";

	}

	#endregion

}
