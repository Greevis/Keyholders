using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsSearchRecord deals with everything to do with data about SearchRecords.
	/// </summary>

	[GuidAttribute("9C780B8B-405B-41a6-88F8-B63BF49C824A")]
	public class clsSearchRecord : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsSearchRecord
		/// </summary>
		public clsSearchRecord() : base("SearchRecord")
		{
		}

		/// <summary>
		/// Constructor for clsSearchRecord; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsSearchRecord(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("SearchRecord")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Search Information
		/// </summary>
		public clsQueryPart SearchQ = new clsQueryPart();


		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			MainQ = SearchRecordQueryPart();
			SearchQ = SearchQueryPart();

			
			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[2];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = SearchQ;
			
			orderBySqlQuery = "Order By " + thisTable + "." + thisPk + crLf;

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsSearchRecord
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("CustomerId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("SearchId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("IdRecord", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("RecordType", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("DateSearchRecord", System.Type.GetType("System.String"));
			
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

		}

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all IdRecord-In-Searchs
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
		/// Gets a IdRecord-In-Search by SearchRecordId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetBySearchRecordId(int SearchRecordId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string Condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ SearchRecordId.ToString() + ") " + thisTable
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
		/// Gets Searchs by CustomerId
		/// </summary>
		/// <param name="CustomerId">Id of CustomerId to retrieve Searchs for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblSearchRecord.CustomerId = " 
				+ CustomerId.ToString() + crLf;

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
		/// Gets Searchs by UserId
		/// </summary>
		/// <param name="UserId">Id of UserId to retrieve Searchs for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByUserId(int UserId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from tblSearch" + crLf;	

			//Additional Condition
			condition += "Where tblSearch.UserId = " 
				+ UserId.ToString() + crLf;

			condition += ") tblSearch";

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition,
				"tblSearch"
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets IdRecord-In-Searchs by IdRecord
		/// </summary>
		/// <param name="IdRecord">Id of IdRecord to retrieve IdRecord-In-Searchs for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByIdRecord(int IdRecord)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblSearchRecord.IdRecord = " 
				+ IdRecord.ToString() + crLf;

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
		/// Gets IdRecord-In-Searchs by SearchId
		/// </summary>
		/// <param name="SearchId">Id of IdRecord Search to retrieve IdRecord-In-Searchs for</param>
		/// <returns>Number of resulting records</returns>
		public int GetBySearchId(int SearchId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where " + thisTable + ".SearchId = " 
				+ SearchId.ToString() + crLf;

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
		/// Gets IdRecord-In-Searchs by IdRecord and SearchId
		/// </summary>
		/// <param name="IdRecord">Id of IdRecord to retrieve IdRecord-In-Searchs for</param>
		/// <param name="SearchId">Id of IdRecord Search to retrieve IdRecord-In-Searchs for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByIdRecordAndSearchId(int IdRecord, int SearchId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblSearchRecord.IdRecord = " 
				+ IdRecord.ToString() + crLf;

			condition += "And tblSearchRecord.SearchId = " 
				+ SearchId.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition,
				thisTable
				);


			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		# region Add/Modify/Validate/Save
		
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal IdRecord table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="SearchId">Id of the Search Associated with this SearchRecord</param>
		/// <param name="CustomerId">Id of the Customer Associated with this SearchRecord</param>
		/// <param name="RecordType">Type of Record Searched for</param>
		/// <param name="IdRecord">Id of the Record Associated with this SearchRecord</param>
		/// <param name="DateSearchRecord">Date this Search was performed</param>
		public void Add(int SearchId,
			int CustomerId,
			int RecordType,
			int IdRecord,
			string DateSearchRecord)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["SearchId"] = SearchId;

			if (CustomerId == 0)
				rowToAdd["CustomerId"] = DBNull.Value;
			else
				rowToAdd["CustomerId"] = CustomerId;

			rowToAdd["RecordType"] = RecordType;
			rowToAdd["IdRecord"] = IdRecord;
			rowToAdd["DateSearchRecord"] = SanitiseDate(DateSearchRecord);
			
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
		/// internal IdRecord table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="SearchRecordId">SearchRecordId (Primary Key of Record)</param>
		/// <param name="SearchId">Id of the Search Associated with this SearchRecord</param>
		/// <param name="CustomerId">Id of the Customer Associated with this SearchRecord</param>
		/// <param name="RecordType">Type of Record Searched for</param>
		/// <param name="IdRecord">Id of the Record Associated with this SearchRecord</param>
		/// <param name="DateSearchRecord">Date this Search was performed</param>
		public void Modify(int SearchRecordId, 
			int SearchId,
			int CustomerId,
			int RecordType,
			int IdRecord,
			string DateSearchRecord)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["SearchRecordId"] = SearchRecordId;
			rowToAdd["SearchId"] = SearchId;

			if (CustomerId == 0)
				rowToAdd["CustomerId"] = DBNull.Value;
			else
				rowToAdd["CustomerId"] = CustomerId;

			rowToAdd["RecordType"] = RecordType;
			rowToAdd["IdRecord"] = IdRecord;
			rowToAdd["DateSearchRecord"] = SanitiseDate(DateSearchRecord);

			//Validate the data supplied
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

		# region My_ Values SearchRecord

		/// <summary>
		/// <see cref="clsSearch.my_SearchId">SearchId</see> of 
		/// <see cref="clsSearch">IdRecord Search</see>
		/// Associated with this SearchRecord</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_SearchId">Id</see> 
		/// of <see cref="clsSearch">IdRecord Search</see> 
		/// for this SearchRecord</returns>
		public int my_SearchId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "SearchId"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">CustomerId</see> of 
		/// <see cref="clsCustomer">IdRecord Customer</see>
		/// Associated with this CustomerRecord</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> 
		/// of <see cref="clsCustomer">IdRecord Customer</see> 
		/// for this CustomerRecord</returns>
		public int my_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerId"));
		}

		/// <summary>Id of Record Associated with this IdRecord Payment Method Type</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Id of Record Associated with this IdRecord Payment Method Type</returns>	
		public int my_IdRecord(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IdRecord"));
		}

		/// <summary>
		/// SearchRecordId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>SearchRecordId for this Row</returns>
		public int my_SearchRecordId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "SearchRecordId"));
		}


		/// <summary>
		/// Whether a IdRecord is permitted to selet this Payment Method Type
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether a IdRecord is permitted to selet this Payment Method Type</returns>
		public int my_RecordType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "RecordType"));
		}


		/// <summary>
		/// Data Search Record
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data Search Record
		/// </returns>
		public string my_DateSearchRecord(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSearchRecord");
		}

		#endregion

		# region My_Search_ Values Search


		/// <summary>
		/// <see cref="clsSearch.my_SearchId">Id</see> of
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_SearchId">Id</see> of 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public int my_Search_SearchId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Search_SearchId"));
		}

		/// <summary>
		/// <see cref="clsUser.my_UserId">UserId</see> of 
		/// <see cref="clsUser">User</see>
		/// Associated with this Search</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_UserId">Id</see> 
		/// of <see cref="clsUser">User</see> 
		/// for this Search</returns>	
		public int my_Search_UserId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Search_UserId"));
		}


		/// <summary>
		/// <see cref="clsSearch.my_SearchType">Whether Local Tax Applies</see> to this
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_SearchType">Whether Local Tax Applies</see> to this 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public int my_Search_SearchType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Search_SearchType"));
		}


		/// <summary>
		/// <see cref="clsSearch.my_SearchQuery">Name</see> of
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_SearchQuery">Name</see> of 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public string my_Search_SearchQuery(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Search_SearchQuery");
		}


		/// <summary>
		/// <see cref="clsSearch.my_SearchReason">Whether we can ship</see> to this
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_SearchReason">Whether we can ship</see> to this 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public string my_Search_SearchReason(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Search_SearchReason");
		}

		/// <summary>
		/// <see cref="clsSearch.my_IPAddress">Whether we can ship</see> to this
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_IPAddress">Whether we can ship</see> to this 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public string my_Search_IPAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Search_IPAddress");
		}

		/// <summary>
		/// <see cref="clsSearch.my_DateSearch">Whether we can ship</see> to this
		/// <see cref="clsSearch">Search</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsSearch.my_DateSearch">Whether we can ship</see> to this 
		/// <see cref="clsSearch">Search</see> 
		/// </returns>
		public string my_Search_DateSearch(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Search_DateSearch");
		}



		#endregion

	}
}
