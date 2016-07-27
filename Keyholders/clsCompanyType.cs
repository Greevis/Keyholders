using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsCompanyType deals with everything to do with data about Company Types.
	/// </summary>

	[GuidAttribute("DBF49C3E-3BF3-4210-949D-CF078AAC6781")]
	public class clsCompanyType : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCompanyType
		/// </summary>
		public clsCompanyType() : base("CompanyType")
		{
		}

		/// <summary>
		/// Constructor for clsCompanyType; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsCompanyType(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("CompanyType")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
		
			MainQ = CompanyTypeQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[1];
			
			baseQueries[0] = MainQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);

			orderBySqlQuery = "Order By tblCompanyType.CompanyTypeName" + crLf;

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsCompanyType
		/// </summary>
		public override void Initialise()
		{
		
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("CompanyTypeName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("IsPublic", System.Type.GetType("System.Int32"));
			
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
		/// Initialises an internal list of all CompanyTypes
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = thisTable + ".IsPublic = 1" + crLf;

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
		/// Gets an CompanyType by CompanyTypeId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByCompanyTypeId(int CompanyTypeId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];

			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ CompanyTypeId.ToString() + crLf;

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
		/// Returns Company Types with the specified Company TypeN ame
		/// </summary>
		/// <param name="CompanyTypeName">Name of Company Type</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of Company Types with the specified Company Type Name</returns>
		public int GetByCompanyTypeName(string CompanyTypeName, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(CompanyTypeName, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where " + thisTable + ".CompanyTypeName " + match + crLf;
			condition += " And " + thisTable + ".IsPublic = 1" + crLf;


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
		/// Gets a Distinct List of Company Type Names only. This is intented for use e.g. as an
		/// auto-completing drop down.
		/// </summary>
		/// <param name="CompanyTypeName">Name of Company Type</param>
		/// <param name="MatchCriteria">Criteria to match Company Type Name, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctCompanyTypeName(string CompanyTypeName, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;	
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct DisplayName");

			string condition = "(Select CompanyTypeName as DisplayName" + crLf
				+ "From " + thisTable + crLf
				+ "Where CompanyTypeName " 
				+ MatchCondition(CompanyTypeName, matchCriteria.contains) + crLf;

			condition += " And " + thisTable + ".IsPublic = 1" + crLf;


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

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the SaveCustomers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CompanyTypeName">CompanyType's Name</param>
		/// <param name="IsPublic">Whether this Type of Comapny is Public</param>
		public void Add(string CompanyTypeName,
			int IsPublic)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["CompanyTypeName"] = CompanyTypeName;
			rowToAdd["IsPublic"] = IsPublic;
			
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
		/// <param name="CompanyTypeId">CompanyTypeId (Primary Key of Record)</param>
		/// <param name="CompanyTypeName">CompanyType's Name</param>
		/// <param name="IsPublic">Whether this Type of Comapny is Public</param>
		public void Modify(int CompanyTypeId, 
			string CompanyTypeName,
			int IsPublic)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["CompanyTypeId"] = CompanyTypeId;
			rowToAdd["CompanyTypeName"] = CompanyTypeName;
			rowToAdd["IsPublic"] = IsPublic;

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

		# region My_ Values CompanyType

		/// <summary>
		/// <see cref="clsCompanyType.my_CompanyTypeId">Id</see> of
		/// <see cref="clsCompanyType">CompanyType</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCompanyType.my_CompanyTypeId">Id</see> of 
		/// <see cref="clsCompanyType">CompanyType</see> 
		/// </returns>
		public int my_CompanyTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CompanyTypeId"));
		}

		/// <summary>
		/// <see cref="clsCompanyType.my_CompanyTypeName">Name</see> of
		/// <see cref="clsCompanyType">CompanyType</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCompanyType.my_CompanyTypeName">Name</see> of 
		/// <see cref="clsCompanyType">CompanyType</see> 
		/// </returns>
		public string my_CompanyTypeName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CompanyTypeName");
		}

		/// <summary>
		/// Whether this <see cref="clsCompanyType">CompanyType</see>
		/// <see cref="clsCompanyType.my_IsPublic">is Public</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>
		/// Whether this <see cref="clsCompanyType">CompanyType</see>
		/// <see cref="clsCompanyType.my_IsPublic">is Public</see>
		/// </returns>
		public int my_IsPublic(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsPublic"));
		}

		# endregion

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



		#endregion
	}
}
