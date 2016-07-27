using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsSearch deals with everything to do with data about Searches.
	/// </summary>

	[GuidAttribute("23D0C942-53A3-4067-A63D-16169D07D60C")]
	public class clsSearch : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsSearch
		/// </summary>
		public clsSearch() : base("Search")
		{
		}

		/// <summary>
		/// Constructor for clsSearch; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsSearch(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Search")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to User Information
		/// </summary>
		public clsQueryPart UserQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			MainQ = SearchQueryPart();

			MainQ.AddSelectColumn("TotalViews");
			MainQ.FromTables.Clear();
			MainQ.AddFromTable(thisTable + " left outer join " + crLf
				+ "(Select SearchId, count(tblSearchRecord.SearchRecordId) as TotalViews from tblSearchRecord group by SearchId) tblSearchRecord" + crLf
				+ "on " + thisTable + "." + thisPk + " = tblSearchRecord." + thisPk);

			UserQ = UserQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[2];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = UserQ;

			orderBySqlQuery = "Order By tblSearch.SearchId" + crLf;

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsSearch
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("UserId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("SearchType", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("SearchQuery", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("SearchReason", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("IPAddress", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSearch", System.Type.GetType("System.String"));
			
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

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Searches
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a Search by SearchId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetBySearchId(int SearchId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string Condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ SearchId.ToString() + ") " + thisTable
				;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				OrderByColumns, 
				Condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;
			
			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Searchs by UserId
		/// </summary>
		/// <param name="UserId">Id of UserId to retrieve Searchs for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByUserId(int UserId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblSearchRecord.UserId = " 
				+ UserId.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Searchers by User's AccessLevel
		/// </summary>
		/// <param name="AccessLevel">AccessLevel of Users to retrieve Searches for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByAccessLevel(int AccessLevel)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from tblUser" + crLf;	

			//Additional Condition
			condition += "Where tblUser.AccessLevel = " 
				+ AccessLevel.ToString() + crLf;

			condition += ") tblUser";

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition,
				"tblUser"
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Returns Searches with the specified SearchQuery
		/// </summary>
		/// <param name="SearchQuery">Name of Search</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of Searches with the specified SearchQuery</returns>
		public int GetBySearchQuery(string SearchQuery, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(SearchQuery, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblSearch.SearchQuery " + match + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets a Distinct List of Search Names only. This is intented for use e.g. as an
		/// auto-completing drop down.
		/// </summary>
		/// <param name="SearchQuery">Name of Search</param>
		/// <param name="MatchCriteria">Criteria to match Search Name, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctSearchQuery(string SearchQuery, int MatchCriteria)
		{
			string fieldRequired = "SearchQuery";
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct "+ thisTable + "." + fieldRequired);

			string condition = "(Select * from " + thisTable + crLf;
			
			string match = MatchCondition(SearchQuery, 
				(matchCriteria) MatchCriteria);

			//Additional Conditions
			condition += "Where " + fieldRequired + match + crLf;
			
			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				condition + ") " + thisTable,
				thisTable
				);

			//Ordering
			thisSqlQuery += "Order By tblSearch.SearchQuery" + crLf;

			return localRecords.GetRecords(thisSqlQuery);

		}

	
		#endregion

		# region Add/Modify/Validate/Save




		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="UserId">Id of Associated User</param>
		/// <param name="SearchType">Type of Search</param>
		/// <param name="SearchQuery">Search Query</param>
		/// <param name="SearchReason">Reason for Search</param>
		/// <param name="IPAddress">IPAddress of Searcher</param>
		/// <param name="DateSearch">Date of the Search</param>
		public void Add(int UserId,
			int SearchType,
			string SearchQuery,
			string SearchReason,
			string IPAddress,
			string DateSearch)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["UserId"] = UserId;
			rowToAdd["SearchType"] = SearchType;
			rowToAdd["SearchQuery"] = SearchQuery;
			rowToAdd["SearchReason"] = SearchReason;
			rowToAdd["IPAddress"] = IPAddress;
			rowToAdd["DateSearch"] = SanitiseDate(DateSearch);
			
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
		/// <param name="SearchId">SearchId (Primary Key of Record)</param>
		/// <param name="UserId">Id of Associated User</param>
		/// <param name="SearchType">Type of Search</param>
		/// <param name="SearchQuery">Search Query</param>
		/// <param name="SearchReason">Reason for Search</param>
		/// <param name="IPAddress">IPAddress of Searcher</param>
		/// <param name="DateSearch">Date of the Search</param>
		public void Modify(int SearchId, 
			int UserId,
			int SearchType,
			string SearchQuery,
			string SearchReason,
			string IPAddress,
			string DateSearch)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["SearchId"] = SearchId;
			rowToAdd["UserId"] = UserId;
			rowToAdd["SearchType"] = SearchType;
			rowToAdd["SearchQuery"] = SearchQuery;
			rowToAdd["SearchReason"] = SearchReason;
			rowToAdd["IPAddress"] = IPAddress;
			rowToAdd["DateSearch"] = SanitiseDate(DateSearch);

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

		# region My_ Values Search


		/// <summary>
		/// <see cref="clsSearch.my_SearchId">Id</see> of
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_SearchId">Id</see> of 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public int my_SearchId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "SearchId"));
		}

		/// <summary>
		/// <see cref="clsUser.my_UserId">UserId</see> of 
		/// <see cref="clsUser">User</see>
		/// Associated with this Search</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_UserId">Id</see> 
		/// of <see cref="clsUser">User</see> 
		/// for this Search</returns>	
		public int my_UserId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "UserId"));
		}


		/// <summary>
		/// <see cref="clsSearch.my_SearchType">Whether Local Tax Applies</see> to this
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_SearchType">Whether Local Tax Applies</see> to this 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public int my_SearchType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "SearchType"));
		}


		/// <summary>
		/// <see cref="clsSearch.my_SearchQuery">Name</see> of
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_SearchQuery">Name</see> of 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public string my_SearchQuery(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "SearchQuery");
		}


		/// <summary>
		/// <see cref="clsSearch.my_SearchReason">Whether we can ship</see> to this
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_SearchReason">Whether we can ship</see> to this 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public string my_SearchReason(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "SearchReason");
		}

		/// <summary>
		/// <see cref="clsSearch.my_IPAddress">Whether we can ship</see> to this
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_IPAddress">Whether we can ship</see> to this 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public string my_IPAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "IPAddress");
		}

		/// <summary>
		/// <see cref="clsSearch.my_DateSearch">Whether we can ship</see> to this
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_DateSearch">Whether we can ship</see> to this 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public string my_DateSearch(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSearch");
		}

		/// <summary>
		/// <see cref="clsSearch.my_TotalViews">Whether Local Tax Applies</see> to this
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_TotalViews">Whether Local Tax Applies</see> to this 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public int my_TotalViews(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalViews"));
		}

		#endregion

		# region My_ Values User

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">Id</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public int my_User_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "User_PersonId"));
		}

		/// <summary>
		/// <see cref="clsUser.my_AccessLevel">AccessLevel</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_AccessLevel">AccessLevel</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public int my_User_AccessLevel(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "User_AccessLevel"));
		}

		/// <summary>
		/// <see cref="clsUser.my_FirstName">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_FirstName">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_FirstName");
		}

		/// <summary>
		/// <see cref="clsUser.my_LastName">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_LastName">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_LastName");
		}

		/// <summary>
		/// <see cref="clsUser.my_UserName">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_UserName">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_UserName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_UserName");
		}

		/// <summary>
		/// <see cref="clsUser.my_Password">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_Password">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_Password(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_Password");
		}


		/// <summary>
		/// <see cref="clsUser.my_Email">Email</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_Email">Email</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_Email");
		}

		/// <summary>
		/// <see cref="clsUser.my_DateLastLoggedIn">Date Last Logged In (Client Time)</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_DateLastLoggedIn">Date Last Logged In (Client Time)</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_DateLastLoggedIn(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_DateLastLoggedIn");
		}


		#endregion


	}
}
