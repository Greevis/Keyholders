using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsPerson deals with everything to do with data about People.
	/// </summary>

	[GuidAttribute("416F53EA-73C0-4aab-BEDE-40A76662FC12")]
	public class clsPerson : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsPerson
		/// </summary>
		public clsPerson() : base("Person")
		{
		}

		/// <summary>
		/// Constructor for clsPerson; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsPerson(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Person")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Person Information
		/// </summary>
		public clsQueryPart CustomerQ = new clsQueryPart();


		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			ChangeDataQ = ChangeDataQueryPart();
			CustomerQ = CustomerQueryPart();
			MainQ = PersonQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[3];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = CustomerQ;
			baseQueries[2] = ChangeDataQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By tblPerson.LastName, tblPerson.FirstName" + crLf;
			
		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsPerson
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names
			newDataToAdd = new DataTable(thisTable);

			newDataToAdd.Columns.Add("Title", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("FirstName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("LastName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("UserName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Password", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PositionInCompany", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickPostalAddress", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickDaytimePhone", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickDaytimeFax", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickAfterHoursPhone", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickAfterHoursFax", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickMobilePhone", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Email", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PreferredContactMethod", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("KdlComments", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerComments", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("FullName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("IsCustomerAdmin", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("DateLastLoggedIn", System.Type.GetType("System.String"));
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
		}

		/// <summary>
		/// Local Representation of the class <see cref="clsPerson">clsPerson</see>
		/// </summary>
		public clsPerson thisPerson;

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all People
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
		/// Gets a Person by PersonId
		/// </summary>
		/// <param name="PersonId">Id of Person to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonId(int PersonId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ PersonId.ToString() + crLf;

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
		/// Gets a Person by CustomerId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

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
		/// Gets People by any kind of name
		/// </summary>
		/// <param name="Name">Filter for Name or Part of Name of Person</param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctName(string Name)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("DisplayName");

			string condition = "Select Distinct " 
				+ " Ltrim(Rtrim(case" + crLf
				+ "	When Concat(tblPerson.FirstName, tblPerson.LastName) != '' then Concat(tblPerson.FirstName, ' ', tblPerson.LastName)" + crLf
				+ "	else ''" + crLf
				+ "	end)) as DisplayName" + crLf
				+ " from " + thisTable 
				+  " Where concat_ws(' ',tblPerson.FirstName, tblPerson.LastName) " 
				+ MatchCondition(Name, matchCriteria.contains) + crLf;

			condition += ArchiveConditionIfNecessary(true);

			thisSqlQuery = condition;
			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets People by any kind of name
		/// </summary>
		/// <param name="Name">Filter for Name or Part of Name of Person</param>
		/// <returns>Number of resulting records</returns>
		public int GetByName(string Name)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


			string condition = "(Select * from " + thisTable 
				+  " Where concat_ws(' ',tblPerson.FirstName, tblPerson.LastName) " 
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
		/// For Premises which are being moved between Customers, Gets names of people who
		/// are involved in the new Customer and looks for those who match people who are 
		/// involved with the Premise Currently
		/// </summary>
		/// <param name="PremiseId">Id of Premise being moved</param>
		/// <param name="NewCustomerId">Id of Customer Premise is being moved to</param>
		/// <returns>Number of resulting records</returns>
		public int GetMatchingPeopleForPremiseNewCustomer(int PremiseId, int NewCustomerId)
		{

			clsQueryPart MatchPeopleQ = new clsQueryPart();

			MatchPeopleQ.AddSelectColumn("MatchPeople.PersonId as MatchPeople_PersonId");
			MatchPeopleQ.AddSelectColumn("MatchPeople.FirstName as MatchPeople_FirstName");
			MatchPeopleQ.AddSelectColumn("MatchPeople.LastName as MatchPeople_LastName");
			MatchPeopleQ.AddSelectColumn("MatchPeople.PersonPremiseRoleType as MatchPeople_PersonPremiseRoleType");

			MatchPeopleQ.AddFromTable("(Select PersonId, FirstName, LastName, PersonPremiseRoleType from tblPersonPremiseRole, tblPerson  "
				+ " Where tblPersonPremiseRole.PersonId = tblPerson.PersonId And Archive = 0 And PremiseId = " + PremiseId.ToString() + ") MatchPeople");

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];

			queries[0] = MainQ;
			queries[1] = MatchPeopleQ;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".CustomerId = "
				+ NewCustomerId.ToString() + crLf;

			condition += ") " + thisTable;
			
			MatchPeopleQ.AddJoin("(tblPerson.FirstName like '%' + MatchPeople.FirstName + '%' OR MatchPeople.FirstName like '%' + tblPerson.FirstName + '%')");
			MatchPeopleQ.AddJoin("(tblPerson.FirstName like '%' + MatchPeople.FirstName + '%' OR MatchPeople.FirstName like '%' + tblPerson.FirstName + '%')");
			MatchPeopleQ.AddJoin("(tblPerson.LastName like '%' + MatchPeople.LastName + '%' OR MatchPeople.LastName like '%' + tblPerson.LastName + '%')");

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
		/// Returns People with the specified Name
		/// </summary>
		/// <param name="MatchString">Name of Person</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <param name="FieldsToCheck">Fields to check, from the enumeration
		/// <see cref="clsKeyBase.fieldsToMatch">fieldsToMatch</see></param>		
		/// <returns>Number of People with the specified PersonName</returns>
		public int GetByPersonName(string MatchString, int MatchCriteria, int FieldsToCheck)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
		
			string condition = "(Select * from " + thisTable + crLf;
			string matchString = MatchCondition(MatchString, (matchCriteria) MatchCriteria);
			
			switch ((fieldsToMatch) FieldsToCheck)
			{
				case fieldsToMatch.firstNameOnly:
					condition += "	Where tblPerson.FirstName " + matchString + crLf;
					break;
				case fieldsToMatch.lastNameOnly:
					condition += "	Where tblPerson.LastName " + matchString + crLf;
					break;
				case fieldsToMatch.firstAndLastNamesOnly:
					condition += "	Where concat_ws(' ',tblPerson.FirstName, tblPerson.LastName) " + matchString + crLf;
					break;
				default:
					break;
			}


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
		/// Returns People with the specified Name for the specified Person
		/// </summary>
		/// <param name="CustomerId">Person in Question</param>
		/// <param name="MatchString">Name of Person</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <param name="FieldsToCheck">Fields to check, from the enumeration
		/// <see cref="clsKeyBase.fieldsToMatch">fieldsToMatch</see></param>		
		/// <returns>Number of People with the specified Person Name for the Person in Question</returns>
		public int GetByPersonNameForCustomerId(int CustomerId, string MatchString, int MatchCriteria, int FieldsToCheck)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;
			string matchString = MatchCondition(MatchString, (matchCriteria) MatchCriteria);
			
			switch ((fieldsToMatch) FieldsToCheck)
			{
				case fieldsToMatch.firstNameOnly:
					condition += "Where tblPerson.FirstName " + matchString + crLf;
					break;
				case fieldsToMatch.lastNameOnly:
					condition += "Where tblPerson.LastName " + matchString + crLf;
					break;
				case fieldsToMatch.firstAndLastNamesOnly:
					condition += "Where Concat(tblPerson.FirstName, ' ', tblPerson.LastName) " + matchString + crLf;
					break;
				default:
					break;
			}

			condition += " And tblPerson.CustomerId = " + CustomerId.ToString() + crLf;

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
		/// Gets all People who are Keyholders for a certain Premise
		/// </summary>
		/// <param name="PremiseId">Premise in Question</param>
		/// <returns>Number of resulting records</returns>
		public int GetKeyholdersForPremiseId(int PremiseId)
		{
			//New Part of Query Just for PersonPremiseRoles
			clsQueryPart KeyholdersQ = new clsQueryPart();
			
			string KeyholderTableToAdd = "(Select tblPersonPremiseRole.PersonId" + crLf
				+ "From tblPersonPremiseRole" + crLf
				+ "Where tblPersonPremiseRole.personPremiseRoleType = "
				+ personPremiseRoleType_keyHolder().ToString() + crLf
				+ "	And tblPersonPremiseRole.PremiseId = " + PremiseId.ToString()
				+ ") KeyholdersForPremise";

			KeyholdersQ.AddFromTable(KeyholderTableToAdd);

			KeyholdersQ.AddJoin("tblPerson.PersonId = KeyholdersForPremise.PersonId");

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];

			queries[0] = MainQ;
			queries[1] = CustomerQ;
			queries[2] = KeyholdersQ;
		
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
		/// Get a Person by Email and Password combination
		/// </summary>
		/// <param name="Email">Email for which to attempt to retrieve Person</param>
		/// <param name="Password">Password for which to attempt to retrieve Person</param>
		/// <returns>Number of resulting records</returns>
		public int GetByEmailAndPassword(string Email, string Password)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+  " Where " + thisTable + ".Email "
				+ MatchCondition(Email, matchCriteria.exactMatch) + crLf
				+ " And " + thisTable + ".Password "
				+ MatchCondition(Password, matchCriteria.exactMatch) + crLf;

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
		/// Get a Person by UserName and Password combination
		/// </summary>
		/// <param name="UserName">UserName for which to attempt to retrieve Person</param>
		/// <returns>Number of resulting records</returns>
		public int GetByUserName(string UserName)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+  " Where " + thisTable + ".UserName "
				+ MatchCondition(UserName, matchCriteria.exactMatch) + crLf;

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
		/// Get a Person by UserName and Password combination
		/// </summary>
		/// <param name="UserName">UserName for which to attempt to retrieve Person</param>
		/// <param name="Password">Password for which to attempt to retrieve Person</param>
		/// <returns>Number of resulting records</returns>
		public int GetByUserNameAndPassword(string UserName, string Password)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+  " Where " + thisTable + ".UserName "
				+ MatchCondition(UserName, matchCriteria.exactMatch) + crLf
				+ " And " + thisTable + ".Password "
				+ MatchCondition(Password, matchCriteria.exactMatch) + crLf;

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

		#endregion

		#region Get People without either a username or a password

		/// <summary>
		/// Get People without either a username or a password
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByNoUserNameOrPassword()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+  " Where (" + thisTable + ".UserName = ''"
				+ " OR " + thisTable + ".Password = '')"
				+ crLf;

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

		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// Validates the supplied data for Errors and Warnings.
		/// If any Errors are found, ErrorFound will return true, and these Errors 
		/// are available through the ErrorMessage and ErrorFieldName methods
		/// If any Warnings are found, WarningFound will return true, and these Warnings 
		/// are available through the WarningMessage and WarningFieldName methods
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Person table stack; the SavePeople method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="Title">Person's Title</param>
		/// <param name="FirstName">Person's First name</param>
		/// <param name="LastName">Person's Last name</param>
		/// <param name="UserName">Person's User name</param>
		/// <param name="Password">Person's Password</param>
		/// <param name="CustomerId">Person Associated with this Person</param>
		/// <param name="PositionInCompany">Person's Position In Company</param>
		/// <param name="DaytimePhone_InternationalPrefix">Person's Day Time Phone: International Prefix</param>
		/// <param name="DaytimePhone_NationalOrMobilePrefix">Person's Day Time Phone: National Or Mobile Prefix</param>
		/// <param name="DaytimePhone_MainNumber">Person's Day Time Phone: Main Number</param>
		/// <param name="DaytimePhone_Extension">Person's Day Time Phone: Extension</param>
		/// <param name="DaytimeFax_InternationalPrefix">Person's Day Time Fax: International Prefix</param>
		/// <param name="DaytimeFax_NationalOrMobilePrefix">Person's Day Time Fax:National Or Mobile Prefix </param>
		/// <param name="DaytimeFax_MainNumber">Person's Day Time Fax: Main Number</param>
		/// <param name="DaytimeFax_Extension">Person's Day Time Fax: Extension</param>
		/// <param name="MobilePhone_InternationalPrefix">Person's Mobile Phone: International Prefix</param>
		/// <param name="MobilePhone_NationalOrMobilePrefix">Person's Mobile Phone: National Or Mobile Prefix</param>
		/// <param name="MobilePhone_MainNumber">Person's Mobile Phone: Main Number</param>
		/// <param name="MobilePhone_Extension">Person's Mobile Phone: Extension</param>
		/// <param name="AfterHoursPhone_InternationalPrefix">Person's After Hours Phone: International Prefix</param>
		/// <param name="AfterHoursPhone_NationalOrMobilePrefix">Person's After Hours Phone: National Or Mobile Prefix</param>
		/// <param name="AfterHoursPhone_MainNumber">Person's After Hours Phone: Main Number</param>
		/// <param name="AfterHoursPhone_Extension">Person's After Hours Phone: Extension</param>
		/// <param name="AfterHoursFax_InternationalPrefix">Person's After Hours Fax: International Prefix</param>
		/// <param name="AfterHoursFax_NationalOrMobilePrefix">Person's After Hours Fax: National Or Mobile Prefix</param>
		/// <param name="AfterHoursFax_MainNumber">Person's After Hours Fax: Main Number</param>
		/// <param name="AfterHoursFax_Extension">Person's After Hours Fax: Extension</param>
		/// <param name="POBoxType">Person's Postal Address: Whether this is a PO Box or not</param>
		/// <param name="BuildingName">Person's Postal Address: BuildingName e.g. Welman Technologies House</param>
		/// <param name="UnitNumber">Person's Postal Address: UnitNumber e.g. Flat 61</param>
		/// <param name="Number">Person's Postal Address: Number e.g. Flat 61/305</param>
		/// <param name="StreetAddress">Person's Postal Address: Street Address</param>
		/// <param name="Suburb">Person's Postal Address: Suburb</param>
		/// <param name="CityId">Person's Postal Address: Id of City (if known)</param>
		/// <param name="CityName">Person's Postal Address: City</param>
		/// <param name="StateName">Person's Postal Address: State or County</param>
		/// <param name="CountryId">Person's Postal Address: Id of Country (if known)</param>
		/// <param name="CountryName">Person's Postal Address: Country</param>
		/// <param name="PostCode">Person's Postal Address: PostCode</param>
		/// <param name="Email">Person's Email</param>
		/// <param name="PreferredContactMethod">Person's Preferred Contact Method</param>
		/// <param name="KdlComments">KDL's Comments regarding this person</param>
		/// <param name="CustomerComments">Person's own Comments</param>
		/// <param name="IsCustomerAdmin">Whether this person is an Administrator of this Person or not: Not Admin = 0, Admin = 1</param>
		/// <param name="CurrentUser">Current Logged In User</param>
		public void Add(int CustomerId, 
			string Title,
			string FirstName, 
			string LastName, 
			string UserName, 
			string Password, 
			
			string PositionInCompany, 

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
			
			int POBoxType,

			string BuildingName, 
			string UnitNumber, 
			string Number, 
			string StreetAddress, 
			string Suburb, 
			int CityId, 
			string CityName, 
			string StateName, 
			int CountryId, 
			string CountryName, 
			string PostCode,
			
			string Email, 
			int PreferredContactMethod,
			string KdlComments, 
			string CustomerComments,
			int IsCustomerAdmin,
			
			int CurrentUser)
		{

//			bool UserProvided = true;
//			UserProvided = false;

			if (UserName == "" && Password == "")
			{

				UserName = LastName.Trim();

				if (UserName.Length > 5)
					UserName = UserName.Substring(0, 5).Trim();

				int numDigitsToFill = 7 - UserName.Length;

				while (numDigitsToFill > 0)
				{
					UserName += GetRandomNum();
					numDigitsToFill = 7 - UserName.Length;
				}

				Password = GetRandomChar() + GetRandomNum() + GetRandomChar() + GetRandomChar() + GetRandomChar() 
					+ GetRandomNum() + GetRandomNum();
			}

			if (nonCityCityId == 0)
				GetGeneralSettings();

			#region Deal with the address

			if (CountryId == 0)
				CountryId = assumedCountryId;
			
			if (CountryId == assumedCountryId)
				CountryName = "New Zealand";
			else
			{
				if (CountryName == "")
				{
					clsCountry thisCountry = new clsCountry(thisDbType, localRecords.dbConnection);

					int numCountries = thisCountry.GetByCountryId(CountryId);
					if (numCountries > 0)
						CountryName = thisCountry.my_CountryName(0);
				}
			}

			string QuickPostalAddress = GetQuickAddress(POBoxType,
				BuildingName,
				UnitNumber,
				Number,
				StreetAddress,
				Suburb,
				PostCode,
				CityName,
				StateName,
				CountryName);

			#endregion

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

			string FullName = (FirstName + " " + LastName).Trim();

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			AddGeneral(CustomerId,
				Title,
				FirstName,
				LastName,
				UserName,
				Password,
				PositionInCompany,
				QuickPostalAddress,
				QuickDaytimePhone,
				QuickDaytimeFax,
				QuickAfterHoursPhone,
				QuickAfterHoursFax,
				QuickMobilePhone,
				Email,
				IsCustomerAdmin,
				PreferredContactMethod,
				KdlComments,
				CustomerComments,
				FullName,
				"",
				thisChangeData.LastIdAdded(),
				0);

			Save();

			int thisPersonId = LastIdAdded();

			#region Add Address
			clsAddress thisAddress = new clsAddress(thisDbType, localRecords.dbConnection);

			thisAddress.Add(POBoxType,
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
				thisTable, 
				thisPersonId, 
				CurrentUser);

			thisAddress.Save();

			#endregion

			#region Add Phone Numbers
			clsPhoneNumber thisPhoneNumber = new clsPhoneNumber(thisDbType, localRecords.dbConnection);

			thisPhoneNumber.Add(DaytimePhone_InternationalPrefix,
				DaytimePhone_NationalOrMobilePrefix,
				DaytimePhone_MainNumber,
				DaytimePhone_Extension, 
				(int) phoneNumberType.daytimePhone,
				"",
				thisTable,
				thisPersonId,
				systemUserId);

			thisPhoneNumber.Add(DaytimeFax_InternationalPrefix,
				DaytimeFax_NationalOrMobilePrefix,
				DaytimeFax_MainNumber,
				DaytimeFax_Extension, 
				(int) phoneNumberType.daytimeFax,
				"",
				thisTable,
				thisPersonId,
				systemUserId);

			thisPhoneNumber.Add(MobilePhone_InternationalPrefix,
				MobilePhone_NationalOrMobilePrefix,
				MobilePhone_MainNumber,
				MobilePhone_Extension, 
				(int) phoneNumberType.mobilePhone,
				"",
				thisTable,
				thisPersonId,
				systemUserId);

			thisPhoneNumber.Add(AfterHoursPhone_InternationalPrefix,
				AfterHoursPhone_NationalOrMobilePrefix,
				AfterHoursPhone_MainNumber,
				AfterHoursPhone_Extension, 
				(int) phoneNumberType.afterHoursPhone,
				"",
				thisTable,
				thisPersonId,
				systemUserId);

			thisPhoneNumber.Add(AfterHoursFax_InternationalPrefix,
				AfterHoursFax_NationalOrMobilePrefix,
				AfterHoursFax_MainNumber,
				AfterHoursFax_Extension, 
				(int) phoneNumberType.afterHoursFax,
				"",
				thisTable,
				thisPersonId,
				systemUserId);

			thisPhoneNumber.Save();

			#endregion

			#region Add User
//			if (!UserProvided)
//			{

				clsUser thisUser = new clsUser(thisDbType, localRecords.dbConnection);

				thisUser.Add(thisPersonId,
					FirstName,
					LastName,
					UserName,
					Password,
					Email,
					4,
					CurrentUser);

				thisUser.Save();
//			}

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
			thisPerson = new clsPerson(thisDbType, localRecords.dbConnection);
			thisPerson.GetByPersonId(thisPkId);

			AddGeneral(thisPerson.my_CustomerId(0), 
				thisPerson.my_Title(0),
				thisPerson.my_FirstName(0), 
				thisPerson.my_LastName(0), 
				thisPerson.my_UserName(0), 
				thisPerson.my_Password(0),
				thisPerson.my_PositionInCompany(0),
				thisPerson.my_QuickPostalAddress(0), 
				thisPerson.my_QuickDaytimePhone(0), 
				thisPerson.my_QuickDaytimeFax(0), 
				thisPerson.my_QuickAfterHoursPhone(0), 
				thisPerson.my_QuickAfterHoursFax(0), 
				thisPerson.my_QuickMobilePhone(0), 
				thisPerson.my_Email(0), 
				thisPerson.my_IsCustomerAdmin(0), 
				thisPerson.my_PreferredContactMethod(0), 
				thisPerson.my_KdlComments(0), 
				thisPerson.my_CustomerComments(0),
				thisPerson.my_FullName(0),
				thisPerson.my_DateLastLoggedIn(0),
				thisPerson.my_ChangeDataId(0),
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisPerson.my_ChangeData_CreatedByUserId(0),
				thisPerson.my_ChangeData_CreatedByFirstNameLastName(0),
				thisPerson.my_ChangeData_DateCreated(0),
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
		/// <param name="UserName">UserName</param>
		/// <param name="Password">Password</param>
		/// <param name="PositionInCompany">PositionInCompany</param>
		/// <param name="QuickPostalAddress">QuickPostalAddress</param>
		/// <param name="QuickDaytimePhone">QuickDaytimePhone</param>
		/// <param name="QuickDaytimeFax">QuickDaytimeFax</param>
		/// <param name="QuickAfterHoursPhone">QuickAfterHoursPhone</param>
		/// <param name="QuickAfterHoursFax">QuickAfterHoursFax</param>
		/// <param name="QuickMobilePhone">QuickMobilePhone</param>
		/// <param name="Email">Email</param>
		/// <param name="IsCustomerAdmin">IsCustomerAdmin</param>
		/// <param name="PreferredContactMethod">PreferredContactMethod</param>
		/// <param name="KdlComments">KdlComments</param>
		/// <param name="CustomerComments">CustomerComments</param>
		/// <param name="FullName">FullName</param>
		/// <param name="DateLastLoggedIn">DateLastLoggedIn</param>
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		public void AddGeneral(int CustomerId,
			string Title,
			string FirstName,
			string LastName,
			string UserName,
			string Password,
			string PositionInCompany,
			string QuickPostalAddress,
			string QuickDaytimePhone,
			string QuickDaytimeFax,
			string QuickAfterHoursPhone,
			string QuickAfterHoursFax,
			string QuickMobilePhone,
			string Email,
			int IsCustomerAdmin,
			int PreferredContactMethod,
			string KdlComments,
			string CustomerComments,
			string FullName,
			string DateLastLoggedIn,
			int ChangeDataId,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["Title"] = Title;
			rowToAdd["FirstName"] = FirstName;
			rowToAdd["LastName"] = LastName;
			rowToAdd["UserName"] = UserName;
			rowToAdd["Password"] = Password;
			rowToAdd["PositionInCompany"] = PositionInCompany;
			rowToAdd["QuickPostalAddress"] = QuickPostalAddress;
			rowToAdd["QuickDaytimePhone"] = QuickDaytimePhone;
			rowToAdd["QuickDaytimeFax"] = QuickDaytimeFax;
			rowToAdd["QuickAfterHoursPhone"] = QuickAfterHoursPhone;
			rowToAdd["QuickAfterHoursFax"] = QuickAfterHoursFax;
			rowToAdd["QuickMobilePhone"] = QuickMobilePhone;
			rowToAdd["Email"] = Email;
			rowToAdd["IsCustomerAdmin"] = IsCustomerAdmin;
			rowToAdd["PreferredContactMethod"] = PreferredContactMethod;
			rowToAdd["KdlComments"] = KdlComments;
			rowToAdd["CustomerComments"] = CustomerComments;
			rowToAdd["FullName"] = FullName;

			rowToAdd["DateLastLoggedIn"] = SanitiseDate(DateLastLoggedIn);

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
		/// Archives all people associated with this Customer if they have no roles 
		/// </summary>
		/// <param name="CustomerId">Id of Customer</param>
		public void ArchivePeopleWithNoRolesForCustomer(int CustomerId)
		{
			string update = "Update tblPerson, (Select tblPerson.PersonId, count(PersonPremiseRoleId) as NumPPRoles "
				+ "from (select * from tblPerson where Archive = 0 and CustomerId = " + CustomerId.ToString() + ") tblPerson "
				+ "left outer Join "
				+ "(select * from tblPersonPremiseRole where Archive = 0) tblPersonPremiseRole "
				+ "on tblPerson.PersonId = tblPersonPremiseRole.PersonId "
				+ "group by tblPerson.PersonId "
				+ "having NumPPRoles = 0) People set tblPerson.Archive = 1 "
				+ "where tblPerson.PersonId = People.PersonId";

						
			int results = localRecords.GetRecords(update);

		}


		/// <summary>
		/// Validates the supplied data for Errors and Warnings.
		/// If any Errors are found, ErrorFound will return true, and these Errors 
		/// are available through the ErrorMessage and ErrorFieldName methods
		/// If any Warnings are found, WarningFound will return true, and these Warnings 
		/// are available through the WarningMessage and WarningFieldName methods
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Person table stack; the SavePeople method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PersonId">PersonId (Primary Key of Record)</param>
		/// <param name="CustomerId">CustomerId</param>
		/// <param name="Title">Title</param>
		/// <param name="FirstName">FirstName</param>
		/// <param name="LastName">LastName</param>
		/// <param name="UserName">UserName</param>
		/// <param name="Password">Password</param>
		/// <param name="PositionInCompany">PositionInCompany</param>
		/// <param name="QuickPostalAddress">QuickPostalAddress</param>
		/// <param name="QuickDaytimePhone">QuickDaytimePhone</param>
		/// <param name="QuickDaytimeFax">QuickDaytimeFax</param>
		/// <param name="QuickAfterHoursPhone">QuickAfterHoursPhone</param>
		/// <param name="QuickAfterHoursFax">QuickAfterHoursFax</param>
		/// <param name="QuickMobilePhone">QuickMobilePhone</param>
		/// <param name="Email">Email</param>
		/// <param name="IsCustomerAdmin">IsCustomerAdmin</param>
		/// <param name="PreferredContactMethod">PreferredContactMethod</param>
		/// <param name="KdlComments">KdlComments</param>
		/// <param name="CustomerComments">CustomerComments</param>
		/// <param name="DateLastLoggedIn">DateLastLoggedIn</param>
		/// <param name="CurrentUser">Current Logged In User</param>
		public void Modify(int PersonId, 
			int CustomerId,
			string Title,
			string FirstName,
			string LastName,
			string UserName,
			string Password,
			string PositionInCompany,
			string QuickPostalAddress,
			string QuickDaytimePhone,
			string QuickDaytimeFax,
			string QuickAfterHoursPhone,
			string QuickAfterHoursFax,
			string QuickMobilePhone,
			string Email,
			int IsCustomerAdmin,
			int PreferredContactMethod,
			string KdlComments,
			string CustomerComments,
			string DateLastLoggedIn,
			int CurrentUser)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			//Now add everything to the row
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();
		
			string FullName = (FirstName + " " + LastName).Trim();


			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, PersonId);
			
			rowToAdd["PersonId"] = PersonId;
			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["Title"] = Title;
			rowToAdd["FirstName"] = FirstName;
			rowToAdd["LastName"] = LastName;
			rowToAdd["UserName"] = UserName;
			rowToAdd["Password"] = Password;
			rowToAdd["PositionInCompany"] = PositionInCompany;
			rowToAdd["QuickPostalAddress"] = QuickPostalAddress;
			rowToAdd["QuickDaytimePhone"] = QuickDaytimePhone;
			rowToAdd["QuickDaytimeFax"] = QuickDaytimeFax;
			rowToAdd["QuickAfterHoursPhone"] = QuickAfterHoursPhone;
			rowToAdd["QuickAfterHoursFax"] = QuickAfterHoursFax;
			rowToAdd["QuickMobilePhone"] = QuickMobilePhone;
			rowToAdd["Email"] = Email;
			rowToAdd["IsCustomerAdmin"] = IsCustomerAdmin;
			rowToAdd["PreferredContactMethod"] = PreferredContactMethod;
			rowToAdd["KdlComments"] = KdlComments;
			rowToAdd["CustomerComments"] = CustomerComments;
			rowToAdd["FullName"] = FullName;

			rowToAdd["DateLastLoggedIn"] = SanitiseDate(DateLastLoggedIn);

			rowToAdd["Archive"] = 0;

			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);
			}

		}

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Correspondence table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PersonId">Person to set as logged in (Primary Key of Record)</param>
		public void SetAsLoggedIn(int PersonId)
		{
			
			SetAttribute(PersonId, "DateLastLoggedIn", localRecords.DBDateTime(DateTime.Now));
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

		# region My_ Values Person

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">Id</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public int my_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PersonId"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">Id</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public int my_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerId"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_Title">Title</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Title">Title</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Title");
		}
				
		/// <summary>
		/// <see cref="clsPerson.my_FirstName">First Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_FirstName">First Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FirstName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_LastName">Last Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_LastName">Last Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "LastName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_FullName">Full Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_FullName">Full Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FullName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_UserName">User Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_UserName">User Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_UserName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "UserName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_Password">Password</see> for
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Password">Password</see> for 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Password(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Password");
		}

		/// <summary>
		/// <see cref="clsPerson.my_Email">Email thisAddress</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Email">Email thisAddress</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Email");
		}


		/// <summary>
		/// <see cref="clsPerson.my_QuickPostalAddress">Quick Postal thisAddress</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickPostalAddress">Quick Postal thisAddress</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_QuickPostalAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickPostalAddress");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickDaytimePhone">Quick Daytime Phone</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickDaytimePhone">Quick Daytime Phone</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_QuickDaytimePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickDaytimePhone");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickDaytimeFax">Quick Daytime Fax</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickDaytimeFax">Quick Daytime Fax</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_QuickDaytimeFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickDaytimeFax");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_QuickAfterHoursPhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickAfterHoursPhone");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickAfterHoursFax">Quick After Hours Fax</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickAfterHoursFax">Quick After Hours Fax</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_QuickAfterHoursFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickAfterHoursFax");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickMobilePhone">Quick Mobile Phone</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickMobilePhone">Quick Mobile Phone</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_QuickMobilePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickMobilePhone");
		}

	
		/// <summary>
		/// <see cref="clsPerson.my_PreferredContactMethod">
		/// Person's Preferred Contact Method</see> (from enumeration 
		/// <see cref="clsKeyBase.correspondenceMedium">Preferred Contact Method</see>)  of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>
		/// <see cref="clsPerson.my_PreferredContactMethod">
		/// Person's Preferred Contact Method</see> (from enumeration 
		/// <see cref="clsKeyBase.correspondenceMedium">Preferred Contact Method</see>)  of
		/// <see cref="clsPerson">Person</see> 
		/// </returns>		
		public int my_PreferredContactMethod(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PreferredContactMethod"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_IsCustomerAdmin">
		/// Whether this Person is an Administrator for this Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_IsCustomerAdmin">
		/// Whether this Person is an Administrator for this Customer</see>
		/// </returns>
		public int my_IsCustomerAdmin(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsCustomerAdmin"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_PositionInCompany">Position in Company</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PositionInCompany">Position in Company</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_PositionInCompany(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PositionInCompany");
		}

		/// <summary>
		/// <see cref="clsPerson.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "KdlComments");
		}

		/// <summary>
		/// <see cref="clsPerson.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerComments");
		}

		/// <summary>
		/// <see cref="clsPerson.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_DateLastLoggedIn(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateLastLoggedIn");
		}

		#endregion

		# region My_ Values MatchPeople

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">Id</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public int my_MatchPeople_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MatchPeople_PersonId"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_FirstName">First Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_FirstName">First Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_MatchPeople_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "MatchPeople_FirstName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_LastName">Last Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_LastName">Last Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_MatchPeople_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "MatchPeople_LastName");
		}

		/// <summary>
		/// <see cref="clsPersonPremiseRole.my_PersonPremiseRoleType">Person Premise Role Type</see> of
		/// <see cref="clsPersonPremiseRole">Person Premise Role</see>
		/// from the enumeration
		/// <see cref="clsKeyBase.personPremiseRoleType">PersonPremiseRoleType</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPersonPremiseRole.my_PersonPremiseRoleType">Person Premise Role Type</see> of 
		/// <see cref="clsPersonPremiseRole">Person Premise Role</see> 
		/// from the enumeration
		/// <see cref="clsKeyBase.personPremiseRoleType">PersonPremiseRoleType</see>
		/// </returns>
		public int my_MatchPeople_PersonPremiseRoleType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MatchPeople_PersonPremiseRoleType"));
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

		#region My_ Values DisplayName

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

		# region ch_ Values

		/// <summary>
		/// "User Friendly" New Value
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>New Value for this Row</returns>
		public string ch_UfNewValue(int rowNum)
		{
			string baseValue = localRecords.FieldByName(rowNum, "NewValue");
			string thisAttribute = localRecords.FieldByName(rowNum, "AttributeName");
			string result;

			switch(thisAttribute)
			{
				case "CityId":
					clsCity thisCity = new clsCity(thisDbType,
						localRecords.dbConnection);
					thisCity.GetByCityId(Convert.ToInt32(baseValue));
					result = thisCity.my_CityName(0) + ", " + thisCity.my_Country_CountryName(0);
					break;
				default:
					result = baseValue;
					break;

			}
			
			return result;
		}

		# endregion

	
	}
}
