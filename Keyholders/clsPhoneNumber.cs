using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsPhoneNumber deals with everything to do with data about Phone Numbers.
	/// </summary>
	
	[GuidAttribute("DA584539-2732-454c-AEB0-2B3911B398F5")]
	public class clsPhoneNumber : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsPhoneNumber
		/// </summary>
		public clsPhoneNumber() : base("PhoneNumber")
		{
		}

		/// <summary>
		/// Constructor for clsPhoneNumber; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsPhoneNumber(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("PhoneNumber")
		{
			Connect(typeOfDb, odbcConnection);
		}


		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			ChangeDataQ = ChangeDataQueryPart();
			MainQ = PhoneNumberQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[2];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = ChangeDataQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By PhoneNumberType" + crLf;
		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsPhoneNumber
		/// </summary>
		public override void Initialise()
		{
		
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("InternationalPrefix", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("NationalOrMobilePrefix", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("MainNumber", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Extension", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PhoneNumberType", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PhoneNumberTypeDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickPhoneNumber", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AssocTableName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AssocRowId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ChangeDataId", System.Type.GetType("System.Int64"));
			newDataToAdd.Columns.Add("Archive", System.Type.GetType("System.Int64"));
			
			dataToBeModified = new DataTable(thisTable);
			dataToBeModified.Columns.Add(thisPk, System.Type.GetType("System.Int64"));

			dataToBeModified.PrimaryKey = new System.Data.DataColumn[] 
				{dataToBeModified.Columns[thisPk]};

			for (int colCounter = 0; colCounter < newDataToAdd.Columns.Count; colCounter++)
				dataToBeModified.Columns.Add(newDataToAdd.Columns[colCounter].ColumnName, newDataToAdd.Columns[colCounter].DataType);

			InitialiseWarningAndErrorTables();
			InitialiseAttributeChangeDataTable();

		}
	
		/// <summary>
		/// Connect to Foreign Key classes within this clasee
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">An already open ODBC database connection</param>
		public override void ConnectToForeignClasses(
			clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection)
		{
		}

		#endregion

		/// <summary>
		/// Local Representation of the class <see cref="clsPhoneNumber">clsPhoneNumber</see>
		/// </summary>
		public clsPhoneNumber thisPhoneNumber;


		# region Get Methods


		//Get Methods

		/// <summary>
		/// Initialises an internal list of all PhoneNumbers
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "";

			condition += ArchiveConditionIfNecessary(false);

			if (condition == "")
				thisSqlQuery = QB.BuildSqlStatement(
					queries, OrderByColumns);
			else
				thisSqlQuery = QB.BuildSqlStatement(
					queries, OrderByColumns, 
					"(Select * from " + thisTable 
					+ " Where " + condition + ") " + thisTable,
					thisTable
					);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets an PhoneNumber by PhoneNumberId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByPhoneNumberId(int PhoneNumberId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ PhoneNumberId.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}



		/// <summary>
		/// Gets Phone Numbers by PersonId
		/// </summary>
		/// <param name="PersonId">Id of Person who's Phone Numbers to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonId(int PersonId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".AssocTableName = "
				+ "'tblPerson'" + crLf
				+ "And " + thisTable + ".AssocRowId = "
				+ PersonId.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Phone Numbers for a given Person of a given type
		/// </summary>
		/// <param name="PersonId"></param>
		/// <param name="phoneNumberTypeRequired">Type of Phone Number Required
		/// from the enumeration 
		/// <see cref="clsKeyBase.phoneNumberType">phoneNumberType</see> 
		/// </param>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonIdPhoneNumberType(int PersonId, int phoneNumberTypeRequired)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".AssocTableName = "
				+ "'tblPerson'" + crLf
				+ "And " + thisTable + ".AssocRowId = "
				+ PersonId.ToString() + crLf
				+ "And " + thisTable + ".PhoneNumberType = " 
				+ phoneNumberTypeRequired.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);

		}

		/// <summary>
		/// Gets Phone Numbers by ServiceProviderId
		/// </summary>
		/// <param name="ServiceProviderId">Id of ServiceProvider who's Phone Numbers to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByServiceProviderId(int ServiceProviderId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".AssocTableName = "
				+ "'tblServiceProvider'" + crLf
				+ "And " + thisTable + ".AssocRowId = "
				+ ServiceProviderId.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Phone Numbers for a given ServiceProvider of a given type
		/// </summary>
		/// <param name="ServiceProviderId"></param>
		/// <param name="phoneNumberTypeRequired">Type of Phone Number Required
		/// from the enumeration 
		/// <see cref="clsKeyBase.phoneNumberType">phoneNumberType</see> 
		/// </param>
		/// <returns>Number of resulting records</returns>
		public int GetByServiceProviderIdphoneNumberType(int ServiceProviderId, int phoneNumberTypeRequired)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".AssocTableName = "
				+ "'tblServiceProvider'" + crLf
				+ "And " + thisTable + ".AssocRowId = "
				+ ServiceProviderId.ToString() + crLf
				+ "And " + thisTable + ".PhoneNumberType = " 
				+ phoneNumberTypeRequired.ToString() + crLf;


			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);

		}

		/// <summary>
		/// Gets an Phone Number from a supplied Number
		/// </summary>
		/// <param name="PhoneNumber">Phone Number or part thereof to match</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetByPhoneNumber(string PhoneNumber, int MatchCriteria)
		{

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string match = MatchCondition(PhoneNumber.Replace(" ", ""), 
				(matchCriteria) MatchCriteria);

			string condition = "(Select * from " + thisTable 
				+  " Where concat_ws(' ',FirstName,LastName,CompanyName) " 
				+ match + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
				condition,
				thisTable
				);


			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Matching Phone Number Parts for Customer Phone Numbers
		/// </summary>
		/// <param name="CustomerId">Customer whose numbers to check</param>
		/// <param name="PhoneNumber">Phone Number string to match</param>
		/// <param name="phoneNumberPartRequired">Part of Number Required
		/// from the enumeration 
		/// <see cref="clsKeyBase.phoneNumberPart">phoneNumberPart</see> 
		/// </param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetPhonePartByCustomerId(int CustomerId, string PhoneNumber, 
			int phoneNumberPartRequired, int MatchCriteria)
		{
			string fieldRequired = PhonePartFieldName((phoneNumberPart) 
				phoneNumberPartRequired);
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct " + fieldRequired);

			thisSqlQuery = QB.BuildSqlStatement(queries);
			

			
			string match = MatchCondition(PhoneNumber, 
				(matchCriteria) MatchCriteria);

			//Additional Conditions
			thisSqlQuery += "Where " + fieldRequired + match + crLf;

			thisSqlQuery += "	And CustomerId = " + CustomerId.ToString() + crLf;

			//Add Archive Filtering, if any
			if (archiveFilterSqlQuery.Length > 0)
				thisSqlQuery += "	And " + thisTable + "." + archiveFilterSqlQuery.Trim() + crLf;

			//Ordering
			thisSqlQuery += "Order By " + fieldRequired + crLf;

			return localRecords.GetRecords(thisSqlQuery);

		}



		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the SaveCustomers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="InternationalPrefix">International Prefix (if any) of this Phone Number</param>
		/// <param name="NationalOrMobilePrefix">National or Mobile Prefix (if any) of this Phone Number</param>
		/// <param name="MainNumber">Main Number (if any) of this Phone Number</param>
		/// <param name="Extension">Extension (if any) of this Phone Number</param>
		/// <param name="PhoneNumberType">Type of Phone Number, from 
		/// <see cref="clsKeyBase.phoneNumberType">phoneNumberType</see></param>
		/// <param name="PhoneNumberTypeDescription">Description of Phone Number. 
		/// Only required if the 
		/// <see cref="clsKeyBase.phoneNumberType">phoneNumberType</see>is
		/// <see cref="clsKeyBase.phoneNumberType_other">phoneNumberType_other</see></param>
		/// <param name="AssocTableName">Name of the Table Associated with this Phone Number</param>
		/// <param name="AssocRowId">Row in AssocTableName Associated with this Phone Number</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Add(string InternationalPrefix,			
			string NationalOrMobilePrefix,
			string MainNumber,
			string Extension,
			int PhoneNumberType,
			string PhoneNumberTypeDescription,
			string AssocTableName,
			int AssocRowId,
			int CurrentUser)
		{

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			AddGeneral(InternationalPrefix,
				NationalOrMobilePrefix,
				MainNumber,
				Extension,
				PhoneNumberType,
				PhoneNumberTypeDescription,
				AssocTableName,
				AssocRowId,
				thisChangeData.LastIdAdded(),
				0);
			
		}


		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal User table stack; the SaveUsers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CurrentUser">CurrentUser</param>
		/// <param name="thisPkId">thisPkId</param>
		public override int AddArchive(int CurrentUser, int thisPkId)
		{
			thisPhoneNumber = new clsPhoneNumber(thisDbType, localRecords.dbConnection);
			thisPhoneNumber.GetByPhoneNumberId(thisPkId);

			AddGeneral(thisPhoneNumber.my_InternationalPrefix(0), 
				thisPhoneNumber.my_NationalOrMobilePrefix(0),
				thisPhoneNumber.my_MainNumber(0), 
				thisPhoneNumber.my_Extension(0), 
				thisPhoneNumber.my_PhoneNumberType(0),
				thisPhoneNumber.my_PhoneNumberTypeDescription(0),
				thisPhoneNumber.my_AssocTableName(0), 
				thisPhoneNumber.my_AssocRowId(0), 
				thisPhoneNumber.my_ChangeDataId(0), 
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisPhoneNumber.my_ChangeData_CreatedByUserId(0),
				thisPhoneNumber.my_ChangeData_CreatedByFirstNameLastName(0),
				thisPhoneNumber.my_ChangeData_DateCreated(0),
				CurrentUser,"",dtaNow.ToString());
							
			thisChangeData.Save();

			return thisChangeData.LastIdAdded();
	
		}

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Incident table stack; the SaveIncidents method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="InternationalPrefix">InternationalPrefix</param>
		/// <param name="NationalOrMobilePrefix">NationalOrMobilePrefix</param>
		/// <param name="MainNumber">MainNumber</param>
		/// <param name="Extension">Extension</param>
		/// <param name="PhoneNumberType">PhoneNumberType</param>
		/// <param name="PhoneNumberTypeDescription">PhoneNumberTypeDescription</param>
		/// <param name="AssocTableName">AssocTableName</param>
		/// <param name="AssocRowId">AssocRowId</param>
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		public void AddGeneral(string InternationalPrefix,
			string NationalOrMobilePrefix,
			string MainNumber,
			string Extension,
			int PhoneNumberType,
			string PhoneNumberTypeDescription,
			string AssocTableName,
			int AssocRowId,
			int ChangeDataId,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			string QuickPhoneNumber = GetQuickNumber(InternationalPrefix,
				NationalOrMobilePrefix,
				MainNumber,
				Extension);


			rowToAdd["InternationalPrefix"] = InternationalPrefix;
			rowToAdd["NationalOrMobilePrefix"] = NationalOrMobilePrefix;
			rowToAdd["MainNumber"] = MainNumber;
			rowToAdd["Extension"] = Extension;
			rowToAdd["QuickPhoneNumber"] = QuickPhoneNumber;
			rowToAdd["PhoneNumberType"] = PhoneNumberType;
			rowToAdd["PhoneNumberTypeDescription"] = PhoneNumberTypeDescription;
			rowToAdd["AssocTableName"] = AssocTableName;
			rowToAdd["AssocRowId"] = AssocRowId;
		
			rowToAdd["ChangeDataId"] = ChangeDataId;
			rowToAdd["Archive"] = Archive;
			
			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				newDataToAdd.Rows.Add(rowToAdd);
			}


		}



		/// <summary>
		/// Validates the supplied data for Errors and Warnings.
		/// If any Errors are found, ErrorFound will return true, and these Errors 
		/// are available through the ErrorMessage and ErrorFieldName methods
		/// If any Warnings are found, WarningFound will return true, and these Warnings 
		/// are available through the WarningMessage and WarningFieldName methods
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PhoneNumberId">Id of this Phone Number</param>
		/// <param name="InternationalPrefix">International Prefix (if any) of this Phone Number</param>
		/// <param name="NationalOrMobilePrefix">National or Mobile Prefix (if any) of this Phone Number</param>
		/// <param name="MainNumber">Main Number (if any) of this Phone Number</param>
		/// <param name="Extension">Extension (if any) of this Phone Number</param>
		/// <param name="PhoneNumberType">Type of Phone Number, from 
		/// <see cref="clsKeyBase.phoneNumberType">phoneNumberType</see></param>
		/// <param name="PhoneNumberTypeDescription">Description of Phone Number. 
		/// Only required if the 
		/// <see cref="clsKeyBase.phoneNumberType">phoneNumberType</see>is
		/// <see cref="clsKeyBase.phoneNumberType_other">phoneNumberType_other</see></param>
		/// <param name="AssocTableName">Name of the Table Associated with this Phone Number</param>
		/// <param name="AssocRowId">Row in AssocTableName Associated with this Phone Number</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Modify(int PhoneNumberId, 
			string InternationalPrefix,			
			string NationalOrMobilePrefix,
			string MainNumber,
			string Extension,
			int PhoneNumberType,
			string PhoneNumberTypeDescription,
			string AssocTableName,
			int AssocRowId,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, PhoneNumberId);

			string QuickPhoneNumber = GetQuickNumber(InternationalPrefix, NationalOrMobilePrefix, MainNumber, Extension);

			rowToAdd["PhoneNumberId"] = PhoneNumberId;
			rowToAdd["InternationalPrefix"] = InternationalPrefix;
			rowToAdd["NationalOrMobilePrefix"] = NationalOrMobilePrefix;
			rowToAdd["MainNumber"] = MainNumber;
			rowToAdd["Extension"] = Extension;
			rowToAdd["QuickPhoneNumber"] = QuickPhoneNumber;
			rowToAdd["PhoneNumberType"] = PhoneNumberType;
			rowToAdd["PhoneNumberTypeDescription"] = PhoneNumberTypeDescription;
			rowToAdd["AssocTableName"] = AssocTableName;
			rowToAdd["AssocRowId"] = AssocRowId;
			rowToAdd["Archive"] = 0;

			Validate(rowToAdd, false);


			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
				{
					dataToBeModified.Rows.Add(rowToAdd);
					
					string attribute = "Quick";

					switch ((phoneNumberType) PhoneNumberType)
					{
						case phoneNumberType.afterHoursFax:
							attribute += "AfterHoursFax";
							break;
						case phoneNumberType.afterHoursPhone:
							attribute += "AfterHoursPhone";
							break;
						case phoneNumberType.daytimeFax:
							attribute += "DaytimeFax";
							break;
						case phoneNumberType.daytimePhone:
							attribute += "DaytimePhone";
							break;
						case phoneNumberType.mobilePhone:
							attribute += "MobilePhone";
							break;
						default:
							attribute = "";
							break;
					}
					
					if (attribute != "")
					{

						//Update parent 'quick numbers'
						switch (AssocTableName)
						{
							case "tblPerson":
								clsPerson Person = new clsPerson(thisDbType, localRecords.dbConnection);
								Person.SetAttribute(AssocRowId, attribute, QuickPhoneNumber);
								Person.Save();
								break;
							case "tblPremise":
								clsPremise Premise = new clsPremise(thisDbType, localRecords.dbConnection);
								Premise.SetAttribute(AssocRowId, attribute, QuickPhoneNumber);
								Premise.Save();
								break;
							case "tblServiceProvider":
								clsServiceProvider ServiceProvider = new clsServiceProvider(thisDbType, localRecords.dbConnection);
								ServiceProvider.SetAttribute(AssocRowId, attribute, QuickPhoneNumber);
								ServiceProvider.Save();
								break;
							default:
								break;
						}
					}
				}			
			}
		}

		/// <summary>
		/// Validates data recieved by the Add or Modify Public Methods.
		/// If any Errors are found, ErrorFound will return true, and these Errors 
		/// are available through the ErrorMessage and ErrorFieldName methods
		/// If any Warnings are found, WarningFound will return true, and these Warnings 
		/// are available through the WarningMessage and WarningFieldName methods		
		/// </summary>
		/// <param name="valuesToValidate">Values to be Validated.</param>
		/// <param name="newRow">Indicates whether the Row being validated 
		/// is new or already exists in the system</param>
		private void Validate(System.Data.DataRow valuesToValidate, bool newRow)
		{

		}

		#endregion

		# region My_ Values PhoneNumber

		/// <summary>
		/// PhoneNumberId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>PhoneNumberId for this Row</returns>
		public int my_PhoneNumberId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PhoneNumberId"));
		}

		/// <summary>
		/// InternationalPrefix
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Description of PhoneNumber Type for this Row</returns>
		public string my_InternationalPrefix(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "InternationalPrefix");
		}

		/// <summary>
		/// NationalOrMobilePrefix
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Description of PhoneNumber Type for this Row</returns>
		public string my_NationalOrMobilePrefix(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "NationalOrMobilePrefix");
		}

		/// <summary>
		/// MainNumber
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Description of PhoneNumber Type for this Row</returns>
		public string my_MainNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "MainNumber");
		}

		/// <summary>
		/// Extension
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Description of PhoneNumber Type for this Row</returns>
		public string my_Extension(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Extension");
		}


		/// <summary>
		/// PhoneNumber Type
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Type of PhoneNumber for this Row</returns>
		public int my_PhoneNumberType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PhoneNumberType"));
		}
		
		/// <summary>
		/// PhoneNumber Type Description
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Description of PhoneNumber Type for this Row</returns>
		public string my_PhoneNumberTypeDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PhoneNumberTypeDescription");
		}


		/// <summary>
		/// AssocTableName of this PhoneNumber
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>AssocTableName of PhoneNumber for this Row</returns>
		public string my_AssocTableName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "AssocTableName");
		}

		/// <summary>
		/// AssocRowId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>AssocRowId for this Row</returns>
		public int my_AssocRowId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "AssocRowId"));
		}

		/// <summary>
		/// Quick PhoneNumber
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Quick PhoneNumber for this Row</returns>
		public string my_QuickPhoneNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickPhoneNumber");
		}




		#endregion


	}
}
