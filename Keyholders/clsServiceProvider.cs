using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsServiceProvider deals with everything to do with data about Service Providers.
	/// </summary>
	
	[GuidAttribute("E3713B88-EE61-4484-865B-A8C0E0ABE4F0")]
	public class clsServiceProvider : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsServiceProvider
		/// </summary>
		public clsServiceProvider() : base("ServiceProvider")
		{
		}

		/// <summary>
		/// Constructor for clsServiceProvider; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsServiceProvider(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("ServiceProvider")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Customer Information
		/// </summary>
		public clsQueryPart CustomerQ = new clsQueryPart();
		
		/// <summary>
		/// Part of the Query that Pertains to Service Provider Premise Roles Information
		/// </summary>
		public clsQueryPart SPPremiseRolesSummaryQ = new clsQueryPart();


		/// <summary>
		/// Base Queries with a summary of roles added
		/// </summary>
		public clsQueryPart[] baseQueriesWithSummary;

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = ServiceProviderQueryPart();
			MainQ.FromTables.Clear();
			MainQ.AddFromTable(thisTable + " left outer join tblCustomer on " + thisTable + ".CustomerId = tblCustomer.CustomerId");

			
			CustomerQ = CustomerQueryPart();
			CustomerQ.FromTables.Clear();
			CustomerQ.Joins.Clear();
			ChangeDataQ = ChangeDataQueryPart();
			
			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[3];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = CustomerQ;
			baseQueries[2] = ChangeDataQ;

			baseQueriesWithSummary = new clsQueryPart[3];
			baseQueriesWithSummary[0] = MainQ;
			baseQueriesWithSummary[1] = CustomerQ;
			baseQueriesWithSummary[2] = ChangeDataQ;

			orderBySqlQuery = "Order By FullName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsServiceProvider
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("CustomerId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("Title", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("FirstName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("LastName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CompanyName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickDaytimePhone", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickDaytimeFax", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickAfterHoursPhone", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickAfterHoursFax", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickMobilePhone", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Email", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("KdlComments", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerComments", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("IsSecurityCompany", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("FullName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ChangeDataId", System.Type.GetType("System.Int64"));
			newDataToAdd.Columns.Add("Archive", System.Type.GetType("System.Int64"));
			
			dataToBeModified = new DataTable(thisTable);
			dataToBeModified.Columns.Add(thisPk, System.Type.GetType("System.Int32"));

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
			//Instances of Foreign Key Classes		
		}

		/// <summary>
		/// Local Representation of the class <see cref="clsServiceProvider">clsServiceProvider</see>
		/// </summary>
		public clsServiceProvider thisServiceProvider;

		#endregion

		# region Get Methods	

		/// <summary>
		/// Initialises an internal list of all Service Providers
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "";

			condition += ArchiveConditionIfNecessary(true);

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
		/// Gets an Service Provider by ServiceProviderId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByServiceProviderId(int ServiceProviderId)
		{

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueriesWithSummary;
			
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ ServiceProviderId.ToString() + crLf;

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
		/// Gets all Service Providers who are managed by a certain Customer
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueriesWithSummary;
			
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".CustomerId = "
				+ CustomerId.ToString() + crLf;

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
		/// Gets all Service Providers who are managed by a certain Customer OR
		/// by KDL
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetForCustomerId(int CustomerId)
		{
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueriesWithSummary;
			
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".CustomerId = "
				+ CustomerId.ToString() + crLf
				+ "Or "  + thisTable + ".CustomerId = 0)"  + crLf;

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
		/// Gets Service Providers by any kind of name
		/// </summary>
		/// <param name="Name">Filter for Name or Part of Name of Service Provider</param>
		/// <returns>Number of resulting records</returns>
		public int GetByName(string Name)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+  " Where concat_ws(' ',FirstName,LastName,CompanyName) " 
				+ MatchCondition(Name, matchCriteria.contains) + crLf;

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
		/// Gets Service Providers by any kind of name and CustomerId
		/// </summary>
		/// <param name="CustomerId">CustomerId</param>
		/// <param name="Name">Filter for Name or Part of Name of Service Provider</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerIdName(int CustomerId, string Name)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+  " Where concat_ws(' ',FirstName,LastName,CompanyName) " 
				+ MatchCondition(Name, matchCriteria.contains) + crLf
				+ "And " + thisTable + ".CustomerId = " + CustomerId.ToString()  + crLf
				;

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
		/// Gets Service Providers by any kind of name
		/// </summary>
		/// <param name="Name">Filter for Name or Part of Name of Service Provider</param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctName(string Name)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;	
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct DisplayName");

			string condition = "(Select  "
				+ "Ltrim(Rtrim(Concat(tblServiceProvider.CompanyName, ' '," + crLf
				+ "		case" + crLf
				+ "		When Concat(tblServiceProvider.FirstName, tblServiceProvider.LastName) != '' then" + crLf
				+ "			Concat('('," + crLf
				+ "			Ltrim(Rtrim(Concat(tblServiceProvider.FirstName, ' ', tblServiceProvider.LastName)))," + crLf
				+ "			')')" + crLf
				+ "		else ''" + crLf
				+ "		end" + crLf
				+ "))) as DisplayName" + crLf
				+ "from " + thisTable + crLf
				+ "Where concat_ws(' ',FirstName,LastName,CompanyName) " 
				+ MatchCondition(Name, matchCriteria.contains) + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
				condition,
				thisTable
				);

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		# region Add/Modify/Validate/Save

		#region Get ServiceProvider Fullname (auxillary function)
		/// <summary>
		/// GetServiceProviderFullName
		/// </summary>
		/// <param name="FirstName">FirstName</param>
		/// <param name="LastName">LastName</param>
		/// <param name="CompanyName">CompanyName</param>
		/// <returns>Customer FullName</returns>
		public string GetServiceProviderFullName(string FirstName, string LastName, string CompanyName)
		{
			string Fullname = "";

			if (CompanyName.Trim() == "")
			{
				Fullname = (FirstName + " " + LastName).Trim();
			}
			else
			{
				Fullname = CompanyName + " ";
				if ((FirstName + " " + LastName).Trim() != "")
					Fullname += "(" + (FirstName + " " + LastName).Trim() + ")";
			}

			Fullname = Fullname.Trim();

			return Fullname;

		}

		#endregion

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the SaveCustomers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="Title">Service Provider's Title</param>
		/// <param name="FirstName">Service Provider's first name)</param>
		/// <param name="LastName">Service Provider's last name</param>
		/// <param name="CompanyName">Service Provider's Company name</param>
		/// <param name="CustomerId">Customer Associated with this Service Provider</param>
		/// <param name="IsSecurityCompany">Whether this company is a securuty company or not: not a security company = 0, Security Company= 1</param>
		/// <param name="Email">Service Provider's Email</param>
		/// <param name="KdlComments">KDL's Comments regarding this Service Provider</param>
		/// <param name="CustomerComments">Service Provider's Comments regarding Service Provider</param>
		/// <param name="DaytimePhone_InternationalPrefix">Service Provider's Day Time Phone: International Prefix</param>
		/// <param name="DaytimePhone_NationalOrMobilePrefix">Service Provider's Day Time Phone: National Or Mobile Prefix</param>
		/// <param name="DaytimePhone_MainNumber">Service Provider's Day Time Phone: Main Number</param>
		/// <param name="DaytimePhone_Extension">Service Provider's Day Time Phone: Extension</param>
		/// <param name="DaytimeFax_InternationalPrefix">Service Provider's Day Time Fax: International Prefix</param>
		/// <param name="DaytimeFax_NationalOrMobilePrefix">Service Provider's Day Time Fax:National Or Mobile Prefix </param>
		/// <param name="DaytimeFax_MainNumber">Service Provider's Day Time Fax: Main Number</param>
		/// <param name="DaytimeFax_Extension">Service Provider's Day Time Fax: Extension</param>
		/// <param name="MobilePhone_InternationalPrefix">Service Provider's Mobile Phone: International Prefix</param>
		/// <param name="MobilePhone_NationalOrMobilePrefix">Service Provider's Mobile Phone: National Or Mobile Prefix</param>
		/// <param name="MobilePhone_MainNumber">Service Provider's Mobile Phone: Main Number</param>
		/// <param name="MobilePhone_Extension">Service Provider's Mobile Phone: Extension</param>
		/// <param name="AfterHoursPhone_InternationalPrefix">Service Provider's After Hours Phone: International Prefix</param>
		/// <param name="AfterHoursPhone_NationalOrMobilePrefix">Service Provider's After Hours Phone: National Or Mobile Prefix</param>
		/// <param name="AfterHoursPhone_MainNumber">Service Provider's After Hours Phone: Main Number</param>
		/// <param name="AfterHoursPhone_Extension">Service Provider's After Hours Phone: Extension</param>
		/// <param name="AfterHoursFax_InternationalPrefix">Service Provider's After Hours Fax: International Prefix</param>
		/// <param name="AfterHoursFax_NationalOrMobilePrefix">Service Provider's After Hours Fax: National Or Mobile Prefix</param>
		/// <param name="AfterHoursFax_MainNumber">Service Provider's After Hours Fax: Main Number</param>
		/// <param name="AfterHoursFax_Extension">Service Provider's After Hours Fax: Extension</param>
		/// <param name="CurrentUser">Current Logged In User</param>
		public void Add(int CustomerId,
			string Title,
			string FirstName,
			string LastName,
			string CompanyName,
			int IsSecurityCompany,

			string Email,
			string KdlComments,
			string CustomerComments, 

			string DaytimePhone_InternationalPrefix, 
			string DaytimePhone_NationalOrMobilePrefix, 
			string DaytimePhone_MainNumber, 
			string DaytimePhone_Extension, 

			string DaytimeFax_InternationalPrefix, 
			string DaytimeFax_NationalOrMobilePrefix, 
			string DaytimeFax_MainNumber, 
			string DaytimeFax_Extension, 

			string AfterHoursPhone_InternationalPrefix, 
			string AfterHoursPhone_NationalOrMobilePrefix, 
			string AfterHoursPhone_MainNumber, 
			string AfterHoursPhone_Extension, 

			string AfterHoursFax_InternationalPrefix, 
			string AfterHoursFax_NationalOrMobilePrefix, 
			string AfterHoursFax_MainNumber, 
			string AfterHoursFax_Extension, 

			string MobilePhone_InternationalPrefix, 
			string MobilePhone_NationalOrMobilePrefix, 
			string MobilePhone_MainNumber, 
			string MobilePhone_Extension,

			int CurrentUser)
		{

			#region Deal with the Phone Numbers

			string QuickDaytimePhone = GetQuickNumber(
				DaytimePhone_InternationalPrefix,
				DaytimePhone_NationalOrMobilePrefix,
				DaytimePhone_MainNumber,
				DaytimePhone_Extension);

			string QuickDaytimeFax = GetQuickNumber(
				DaytimeFax_InternationalPrefix,
				DaytimeFax_NationalOrMobilePrefix,
				DaytimeFax_MainNumber,
				DaytimeFax_Extension);
			
			string QuickAfterHoursPhone = GetQuickNumber(
				AfterHoursPhone_InternationalPrefix,
				AfterHoursPhone_NationalOrMobilePrefix,
				AfterHoursPhone_MainNumber,
				AfterHoursPhone_Extension);
			
			string QuickAfterHoursFax = GetQuickNumber(
				AfterHoursFax_InternationalPrefix,
				AfterHoursFax_NationalOrMobilePrefix,
				AfterHoursFax_MainNumber,
				AfterHoursFax_Extension);
			
			string QuickMobilePhone = GetQuickNumber(
				MobilePhone_InternationalPrefix,
				MobilePhone_NationalOrMobilePrefix,
				MobilePhone_MainNumber,
				MobilePhone_Extension);
			#endregion			

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			string FullName = GetServiceProviderFullName(FirstName, LastName, CompanyName);

			AddGeneral(CustomerId,
				Title,
				FirstName,
				LastName,
				CompanyName,
				IsSecurityCompany,
				QuickDaytimePhone,
				QuickDaytimeFax,
				QuickAfterHoursPhone,
				QuickAfterHoursFax,
				QuickMobilePhone,
				Email,
				KdlComments,
				CustomerComments,
				FullName,
				thisChangeData.LastIdAdded(),
				0);

			Save();

			int thisServiceProviderId = LastIdAdded();

			#region Add Phone Numbers
			clsPhoneNumber thisPhoneNumber = new clsPhoneNumber(thisDbType, localRecords.dbConnection);

			thisPhoneNumber.Add(DaytimePhone_InternationalPrefix,
				DaytimePhone_NationalOrMobilePrefix,
				DaytimePhone_MainNumber,
				DaytimePhone_Extension, 
				(int) phoneNumberType.daytimePhone,
				"",
				thisTable,
				thisServiceProviderId,
				CurrentUser);

			thisPhoneNumber.Add(DaytimeFax_InternationalPrefix,
				DaytimeFax_NationalOrMobilePrefix,
				DaytimeFax_MainNumber,
				DaytimeFax_Extension, 
				(int) phoneNumberType.daytimeFax,
				"",
				thisTable,
				thisServiceProviderId,
				CurrentUser);

			thisPhoneNumber.Add(MobilePhone_InternationalPrefix,
				MobilePhone_NationalOrMobilePrefix,
				MobilePhone_MainNumber,
				MobilePhone_Extension, 
				(int) phoneNumberType.mobilePhone,
				"",
				thisTable,
				thisServiceProviderId,
				CurrentUser);

			thisPhoneNumber.Add(AfterHoursPhone_InternationalPrefix,
				AfterHoursPhone_NationalOrMobilePrefix,
				AfterHoursPhone_MainNumber,
				AfterHoursPhone_Extension, 
				(int) phoneNumberType.afterHoursPhone,
				"",
				thisTable,
				thisServiceProviderId,
				CurrentUser);

			thisPhoneNumber.Add(AfterHoursFax_InternationalPrefix,
				AfterHoursFax_NationalOrMobilePrefix,
				AfterHoursFax_MainNumber,
				AfterHoursFax_Extension, 
				(int) phoneNumberType.afterHoursFax,
				"",
				thisTable,
				thisServiceProviderId,
				CurrentUser);

			thisPhoneNumber.Save();

			#endregion

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
			thisServiceProvider = new clsServiceProvider(thisDbType, localRecords.dbConnection);
			thisServiceProvider.GetByServiceProviderId(thisPkId);

			AddGeneral(thisServiceProvider.my_CustomerId(0), 
				thisServiceProvider.my_Title(0),
				thisServiceProvider.my_FirstName(0), 
				thisServiceProvider.my_LastName(0), 
				thisServiceProvider.my_CompanyName(0), 
				thisServiceProvider.my_IsSecurityCompany(0),
				thisServiceProvider.my_QuickDaytimePhone(0), 
				thisServiceProvider.my_QuickDaytimeFax(0), 
				thisServiceProvider.my_QuickAfterHoursPhone(0), 
				thisServiceProvider.my_QuickAfterHoursFax(0), 
				thisServiceProvider.my_QuickMobilePhone(0), 
				thisServiceProvider.my_Email(0), 
				thisServiceProvider.my_KdlComments(0), 
				thisServiceProvider.my_CustomerComments(0),
				thisServiceProvider.my_FullName(0),
				thisServiceProvider.my_ChangeDataId(0),
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisServiceProvider.my_ChangeData_CreatedByUserId(0),
				thisServiceProvider.my_ChangeData_CreatedByFirstNameLastName(0),
				thisServiceProvider.my_ChangeData_DateCreated(0),
				CurrentUser,"",dtaNow.ToString());
							
			thisChangeData.Save();

			return thisChangeData.LastIdAdded();
	
		}

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Incident table stack; the SaveIncidents method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CustomerId">CustomerId</param>
		/// <param name="Title">Title</param>
		/// <param name="FirstName">FirstName</param>
		/// <param name="LastName">LastName</param>
		/// <param name="CompanyName">CompanyName</param>
		/// <param name="IsSecurityCompany">IsSecurityCompany</param>
		/// <param name="QuickDaytimePhone">QuickDaytimePhone</param>
		/// <param name="QuickDaytimeFax">QuickDaytimeFax</param>
		/// <param name="QuickAfterHoursPhone">QuickAfterHoursPhone</param>
		/// <param name="QuickAfterHoursFax">QuickAfterHoursFax</param>
		/// <param name="QuickMobilePhone">QuickMobilePhone</param>
		/// <param name="Email">Email</param>
		/// <param name="KdlComments">KdlComments</param>
		/// <param name="CustomerComments">CustomerComments</param>
		/// <param name="FullName">FullName</param>
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		public void AddGeneral(int CustomerId,
			string Title,
			string FirstName,
			string LastName,
			string CompanyName,
			int IsSecurityCompany,
			string QuickDaytimePhone,
			string QuickDaytimeFax,
			string QuickAfterHoursPhone,
			string QuickAfterHoursFax,
			string QuickMobilePhone,
			string Email,
			string KdlComments,
			string CustomerComments,
			string FullName,
			int ChangeDataId,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			if (CustomerId == 0)
				rowToAdd["CustomerId"] = DBNull.Value;
			else
				rowToAdd["CustomerId"] = CustomerId;

			rowToAdd["Title"] = Title;
			rowToAdd["FirstName"] = FirstName;
			rowToAdd["LastName"] = LastName;
			rowToAdd["CompanyName"] = CompanyName;
			rowToAdd["IsSecurityCompany"] = IsSecurityCompany;
			rowToAdd["QuickDaytimePhone"] = QuickDaytimePhone;
			rowToAdd["QuickDaytimeFax"] = QuickDaytimeFax;
			rowToAdd["QuickAfterHoursPhone"] = QuickAfterHoursPhone;
			rowToAdd["QuickAfterHoursFax"] = QuickAfterHoursFax;
			rowToAdd["QuickMobilePhone"] = QuickMobilePhone;
			rowToAdd["Email"] = Email;
			rowToAdd["KdlComments"] = KdlComments;
			rowToAdd["CustomerComments"] = CustomerComments;
			rowToAdd["FullName"] = FullName;

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
		/// <param name="ServiceProviderId">ServiceProviderId (Primary Key of Record)</param>
		/// <param name="Title">Service Provider's Title</param>
		/// <param name="FirstName">Service Provider's first name)</param>
		/// <param name="LastName">Service Provider's last name</param>
		/// <param name="CompanyName">Service Provider's Company name</param>
		/// <param name="CustomerId">Customer Associated with this Service Provider</param>
		/// <param name="IsSecurityCompany">Whether this company is a securuty company or not: not a security company = 0, Security Company= 1</param>
		/// <param name="QuickDaytimePhone">QuickDaytimePhone</param>
		/// <param name="QuickDaytimeFax">QuickDaytimeFax</param>
		/// <param name="QuickAfterHoursPhone">QuickAfterHoursPhone</param>
		/// <param name="QuickAfterHoursFax">QuickAfterHoursFax</param>
		/// <param name="QuickMobilePhone">QuickMobilePhone</param>
		/// <param name="Email">Service Provider's Email</param>
		/// <param name="KdlComments">KDL's Comments regarding this Service Provider</param>
		/// <param name="CustomerComments">Person's Comments regarding Service Provider</param>
		/// <param name="CurrentUser">Current Logged In User</param>
		public void Modify(int ServiceProviderId,
			int CustomerId,
			string Title,
			string FirstName,
			string LastName,
			string CompanyName,
			int IsSecurityCompany,
			string QuickDaytimePhone,
			string QuickDaytimeFax,
			string QuickAfterHoursPhone,
			string QuickAfterHoursFax,
			string QuickMobilePhone,
			string Email,
			string KdlComments,
			string CustomerComments,

			int CurrentUser)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, ServiceProviderId);

			string FullName = GetServiceProviderFullName(FirstName, LastName, CompanyName);

			rowToAdd["ServiceProviderId"] = ServiceProviderId;
			
			if (CustomerId == 0)
				rowToAdd["CustomerId"] = DBNull.Value;
			else
				rowToAdd["CustomerId"] = CustomerId;

			rowToAdd["Title"] = Title;
			rowToAdd["FirstName"] = FirstName;
			rowToAdd["LastName"] = LastName;
			rowToAdd["CompanyName"] = CompanyName;
			rowToAdd["QuickDaytimePhone"] = QuickDaytimePhone;
			rowToAdd["QuickDaytimeFax"] = QuickDaytimeFax;
			rowToAdd["QuickAfterHoursPhone"] = QuickAfterHoursPhone;
			rowToAdd["QuickAfterHoursFax"] = QuickAfterHoursFax;
			rowToAdd["QuickMobilePhone"] = QuickMobilePhone;
			rowToAdd["Email"] = Email;
			rowToAdd["KdlComments"] = KdlComments;
			rowToAdd["CustomerComments"] = CustomerComments;
			rowToAdd["FullName"] = FullName;
			
			if (CustomerId == 0)
				rowToAdd["CustomerId"] = DBNull.Value;
			else
				rowToAdd["CustomerId"] = CustomerId;

			rowToAdd["IsSecurityCompany"] = IsSecurityCompany;
			rowToAdd["Archive"] = 0;

			Validate(rowToAdd, false);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);

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
			//TODO: Add any required Validation here
		}


		#endregion

		# region My_ Values ServiceProvider

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">Id</see> of
		/// <see cref="clsPerson">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> of 
		/// <see cref="clsPerson">ServiceProvider</see> 
		/// </returns>
		public int my_ServiceProviderId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ServiceProviderId"));
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_FullName">Full Name</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_FullName">Full Name</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FullName");
		}


		/// <summary>
		/// <see cref="clsServiceProvider.my_Title">Title</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_Title">Title</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Title");
		}

				
		/// <summary>
		/// <see cref="clsServiceProvider.my_FirstName">First Name</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_FirstName">First Name</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FirstName");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_LastName">Last Name</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_LastName">Last Name</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "LastName");
		}
		
		/// <summary>
		/// <see cref="clsServiceProvider.my_CompanyName">Company Name</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_CompanyName">Company Name</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_CompanyName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CompanyName");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "KdlComments");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerComments");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">CustomerId</see> of 
		/// <see cref="clsCustomer">Customer</see>
		/// Associated with this ServiceProvider</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> 
		/// of <see cref="clsCustomer">Customer</see> 
		/// for this ServiceProvider</returns>	
		public int my_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerId"));
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_QuickDaytimePhone">Quick Daytime Phone</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_QuickDaytimePhone">Quick Daytime Phone</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_QuickDaytimePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickDaytimePhone");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_QuickDaytimeFax">Quick Daytime Fax</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_QuickDaytimeFax">Quick Daytime Fax</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_QuickDaytimeFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickDaytimeFax");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_QuickAfterHoursPhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickAfterHoursPhone");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_QuickAfterHoursFax">Quick After Hours Fax</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_QuickAfterHoursFax">Quick After Hours Fax</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_QuickAfterHoursFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickAfterHoursFax");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_QuickMobilePhone">Quick Mobile Phone</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_QuickMobilePhone">Quick Mobile Phone</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_QuickMobilePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickMobilePhone");
		}

		/// <summary>
		/// <see cref="clsPerson.my_Email">Email Address</see> of
		/// <see cref="clsPerson">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Email">Email Address</see> of 
		/// <see cref="clsPerson">ServiceProvider</see> 
		/// </returns>
		public string my_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Email");
		}


		/// <summary>
		/// If this Service Provider is a Security Company or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>this Service Provider is a Security Company or not</returns>
		public int my_IsSecurityCompany(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsSecurityCompany"));
		}
		
		
		#endregion

		#region My_ Values Customer

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupId">CustomerGroupId</see> of 
		/// <see cref="clsCustomerGroup">CustomerGroup</see> for this Customer</summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupId">CustomerGroupId</see> 
		/// of Associated <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// for this Customer</returns>
		public int my_Customer_CustomerGroupId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Customer_CustomerGroupId"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerType">Type</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerType">Type</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_CustomerType(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_CustomerType");
		}

		
		/// <summary>
		/// <see cref="clsCustomer.my_CompanyName">Company Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CompanyName">Company Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_CompanyName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_CompanyName");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_AccountNumber">Account Number</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_AccountNumber">Account Number</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_AccountNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_AccountNumber");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_Title">Title</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_Title">Title</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_Title");
		}

				
		/// <summary>
		/// <see cref="clsCustomer.my_FirstName">First Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_FirstName">First Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_FirstName");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_LastName">Last Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_LastName">Last Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_LastName");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_FullName">Full Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_FullName">Full Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_FullName");
		}



		/// <summary>
		/// <see cref="clsCustomer.my_DateStart">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_DateStart">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_DateStart(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateStart");
		}




		/// <summary>
		/// <see cref="clsCountry.my_CountryId">CountryId</see> of 
		/// <see cref="clsCountry">Country</see></summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsCountry.my_CountryId">CountryId</see> 
		/// of Associated <see cref="clsCountry">Country</see> 
		/// for this Order</returns>
		public int my_Customer_CountryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Customer_CountryId"));
		}

		
		/// <summary>
		/// <see cref="clsCustomer.my_OpeningBalance">Opening Balance</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_OpeningBalance">Opening Balance</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public decimal my_Customer_OpeningBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_OpeningBalance"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CreditLimit">Credit Limit</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CreditLimit">Credit Limit</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public decimal my_Customer_CreditLimit(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_CreditLimit"));
		}


		/// <summary>
		/// <see cref="clsCustomer.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_DateLastLoggedIn(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateLastLoggedIn");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_DateLastLoggedInUtc">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_DateLastLoggedInUtc">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_DateLastLoggedInUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateLastLoggedInUtc");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_StartDateForStatement">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_StartDateForStatement">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_StartDateForStatement(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_StartDateForStatement");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_StartDateForInvoices">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_StartDateForInvoices">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_StartDateForInvoices(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_StartDateForInvoices");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_KdlComments");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_CustomerComments");
		}

		/// <summary>
		/// Total Spend by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Spend by this Customer</returns>
		public decimal my_Customer_TotalSpend(int rowNum)
		{
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalItemCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalFreightCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalTaxCost"));
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalItemCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalFreightCost"));
		}

		/// <summary>
		/// Total Item Cost of all Items bought by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Item Cost of all Items bought by this Customer</returns>
		public decimal my_Customer_TotalItemCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalItemCost"));
		}
		
		/// <summary>
		/// Total Tax Cost of all Taxs bought by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Tax Cost of all Taxs bought by this Customer</returns>
		public decimal my_Customer_TotalTaxCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalTaxCost"));
		}

		/// <summary>
		/// Total Freight Cost of all Freights bought by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Freight Cost of all Freights bought by this Customer</returns>
		public decimal my_Customer_TotalFreightCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalFreightCost"));
		}

		/// <summary>
		/// Number of Orders by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of Orders by this Customer</returns>
		public int my_Customer_NumOrders(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Customer_NumOrders"));
		}

		/// <summary>
		/// The Date/Time that this Customer first Completed an Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer first Completed an Order</returns>
		public string my_Customer_DateFirstOrder(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateFirstOrder");
		}


		/// <summary>
		/// The Date/Time that this Customer First Completed an Order (Utc)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer First Completed an Order (Utc)</returns>
		public string my_Customer_DateFirstOrderUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateFirstOrderUtc");
		}

		/// <summary>
		/// The Date/Time that this Customer last Completed an Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer last Completed an Order</returns>
		public string my_Customer_DateLastOrder(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateLastOrder");
		}

		/// <summary>
		/// The Date/Time that this Customer last Completed an Order (Utc)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer last Completed an Order (Utc)</returns>
		public string my_Customer_DateLastOrderUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateLastOrderUtc");
		}

		/// <summary>
		/// Customer's Base Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s without 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Base Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s without 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see></returns>
		public decimal my_Customer_BaseBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_BaseBalance"));
		}

		/// <summary>
		/// Customer's Current Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s  
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Current Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s,  
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see></returns>
		public decimal my_Customer_CurrentBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_CurrentBalance"));
		}
		
		/// <summary>
		/// Customer's Available Credit; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s, 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">Opening Balance</see>
		/// and <see cref="clsCustomer.my_CreditLimit">Credit Limit</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns> Customer's Available Credit; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s, 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">Opening Balance</see>
		/// and <see cref="clsCustomer.my_CreditLimit">Credit Limit</see></returns>
		public decimal my_Customer_AvailableCredit(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_AvailableCredit"));
		}

		/// <summary>
		/// Customer's Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s. 
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s.</returns>
		public decimal my_Customer_TotalPurchases(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalPurchases"));
		}

		/// <summary>
		/// Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments</returns>
		public decimal my_Customer_TotalPaid(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalPaid"));
		}

		/// <summary>
		/// Customer's Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.</returns>
		public decimal my_Customer_TotalUncleared(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalUncleared"));
		}

		/// <summary>
		/// Customer's Invoice Base Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s without 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Invoice Base Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s without 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see></returns>
		public decimal my_Customer_InvoiceBaseBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_InvoiceBaseBalance"));
		}

		/// <summary>
		/// Customer's Invoice Current Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s  
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Invoice Current Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s,  
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see></returns>
		public decimal my_Customer_InvoiceCurrentBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_InvoiceCurrentBalance"));
		}
		
		/// <summary>
		/// Customer's Invoice Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s. 
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Invoice Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s.</returns>
		public decimal my_Customer_InvoiceTotalPurchases(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_InvoiceTotalPurchases"));
		}

		/// <summary>
		/// Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments</returns>
		public decimal my_Customer_InvoiceTotalPaid(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_InvoiceTotalPaid"));
		}

		/// <summary>
		/// Customer's Invoice Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Invoice Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.</returns>
		public decimal my_Customer_InvoiceTotalUncleared(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_InvoiceTotalUncleared"));
		}


		#endregion

		#region My_Values SPPRSummary


		/// <summary>
		/// Number of Roles this Service Provider holds
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of Roles this Service Provider holds</returns>
		public int my_NumRoles(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "NumRoles"));
		}
	
		/// <summary>
		/// Number of Premises this Service Provider holds roles with
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of Premises this Service Provider holds at least one role for</returns>
		public int my_NumPremisesWithAtLeastOneRole(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "NumPremisesWithAtLeastOneRole"));
		}

		#endregion

		#region My_ Values Display

		/// <summary>
		/// Name for Auto-completing Drop Down Display
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name for Auto-completing Drop Down Display</returns>
		public string my_DisplayName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DisplayName");
		}

		# endregion
	}
}
