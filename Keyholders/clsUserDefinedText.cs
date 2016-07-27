using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsUserDefinedText deals with everything to do with data about UserDefinedTexts.
	/// </summary>

	[GuidAttribute("83669604-45AA-4ed1-874A-40FFA6407B7A")]
	public class clsUserDefinedText : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsUserDefinedText
		/// </summary>
		public clsUserDefinedText() : base("UserDefinedText")
		{
		}

		/// <summary>
		/// Constructor for clsUserDefinedText; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsUserDefinedText(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("UserDefinedText")
		{
			Connect(typeOfDb, odbcConnection);
		}


		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			MainQ.AddSelectColumn("tblUserDefinedText.UserDefinedTextId");
			MainQ.AddSelectColumn("tblUserDefinedText.UserDefinedTextName");
			MainQ.AddSelectColumn("tblUserDefinedText.UserDefinedText");

			MainQ.AddFromTable(thisTable);

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblUserDefinedText.UserDefinedTextName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsUserDefinedText
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("UserDefinedTextName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("UserDefinedText", System.Type.GetType("System.String"));
			
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
		/// Initialises an internal list of all UserDefinedTexts
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a User Defined Text by UserDefinedTextId
		/// </summary>
		/// <param name="UserDefinedTextId">Id of User Defined Text to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByUserDefinedTextId(int UserDefinedTextId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
	
			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ UserDefinedTextId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a User Defined Text by Name
		/// </summary>
		/// <param name="UserDefinedTextName">Name of User Defined Text for which to retrieve User Defined Text</param>
		/// <returns>Number of resulting records</returns>
		public int GetByUserDefinedTextName(string UserDefinedTextName)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblUserDefinedText.UserDefinedTextName " + 
				MatchCondition(UserDefinedTextName, matchCriteria.exactMatch) + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition + ") " + thisTable,
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
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="UserDefinedTextName">UserDefinedText's Name</param>
		/// <param name="UserDefinedText">UserDefinedText</param>
		public void Add(string UserDefinedTextName, 
			string UserDefinedText)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["UserDefinedTextName"] = UserDefinedTextName;
			rowToAdd["UserDefinedText"] = UserDefinedText;
			
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
		/// <param name="UserDefinedTextId">UserDefinedTextId (Primary Key of Record)</param>
		/// <param name="UserDefinedTextName">UserDefinedText's Name</param>
		/// <param name="UserDefinedText">UserDefinedText</param>
		public void Modify(int UserDefinedTextId, 
			string UserDefinedTextName, 
			string UserDefinedText)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["UserDefinedTextId"] = UserDefinedTextId;
			rowToAdd["UserDefinedTextName"] = UserDefinedTextName;
			rowToAdd["UserDefinedText"] = UserDefinedText;

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

		}

		#endregion

		# region My_ Values


		/// <summary>
		/// UserDefinedTextId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>UserDefinedTextId for this Row</returns>
		public int my_UserDefinedTextId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "UserDefinedTextId"));
		}

		/// <summary>
		/// UserDefinedText Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name of UserDefinedText for this Row</returns>
		public string my_UserDefinedTextName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "UserDefinedTextName");
		}

		/// <summary>UserDefinedText</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>UserDefinedText</returns>	
		public string my_UserDefinedText(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "UserDefinedText");
		}

		# endregion
	}
}
