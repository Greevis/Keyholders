using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsNav deals with everything to do with data about Navigation Pages.
	/// </summary>
	
	[GuidAttribute("73D8976F-ED12-4599-915E-548D91EF05AD")]
	public class clsNav : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsNav
		/// </summary>
		public clsNav() : base("Nav")
		{
		}

		/// <summary>
		/// Constructor for clsNav; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsNav(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Nav")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			MainQ.AddSelectColumn("tblNav.NavId");
			MainQ.AddSelectColumn("tblNav.LinkTarget");
			MainQ.AddSelectColumn("tblNav.LinkLabel");
			MainQ.AddSelectColumn("tblNav.OpenInNewWindow");
			MainQ.AddSelectColumn("tblNav.NavDisplayOrder");
			MainQ.AddSelectColumn("tblNav.Archive");

			MainQ.AddFromTable(thisTable);

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblNav.LinkTarget" + crLf;

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsNav
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("LinkTarget", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("LinkLabel", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("OpenInNewWindow", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("NavDisplayOrder", System.Type.GetType("System.Int32"));
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
		/// Initialises an internal list of all Navigation Pages
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
		/// Gets an Nav by NavId
		/// </summary>
		/// <param name="NavId">Id of Nav to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByNavId(int NavId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ NavId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Returns Navigation Pages with the specified LinkLabel
		/// </summary>
		/// <param name="LinkLabel">Name of Nav</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of Navigation Pages with the specified LinkLabel</returns>
		public int GetByLinkLabel(string LinkLabel, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(LinkLabel, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblNav.LinkLabel " + match + crLf;

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


		/// <summary>
		/// Gets a Distinct List of Nav Names only. This is intented for use e.g. as an
		/// auto-completing drop down.
		/// </summary>
		/// <param name="LinkLabel">Name of Nav</param>
		/// <param name="MatchCriteria">Criteria to match Nav Name, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctLinkLabel(string LinkLabel, int MatchCriteria)
		{
			string fieldRequired = "LinkLabel";
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct "+ thisTable + "." + fieldRequired);

			string condition = "(Select * from " + thisTable + crLf;
			
			string match = MatchCondition(LinkLabel, 
				(matchCriteria) MatchCriteria);

			//Additional Conditions
			condition += "Where " + fieldRequired + match + crLf;
			
			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				condition + ") " + thisTable,
				thisTable
				);

			//Ordering
			thisSqlQuery += "Order By tblNav.LinkLabel" + crLf;

			return localRecords.GetRecords(thisSqlQuery);

		}

		/// <summary>
		/// Gets Navigation Pages by LinkTarget
		/// </summary>
		/// <param name="LinkTarget">Target of Link</param>
		/// <param name="MatchCriteria">Criteria to match Link Target, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetByLinkTarget(string LinkTarget, int MatchCriteria)
		{
			string fieldRequired = "LinkLabel";
			
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;

			string match = MatchCondition(LinkTarget, 
				(matchCriteria) MatchCriteria);

			//Additional Conditions
			condition += "Where " + fieldRequired + match + crLf;

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
		/// <param name="LinkTarget">Target of Link </param>
		/// <param name="LinkLabel">Link's Label</param>
		/// <param name="OpenInNewWindow">Whether this link opens in a new window or not</param>
		/// <param name="NavDisplayOrder">Display order for this link</param>
		public void Add(string LinkTarget,
			string LinkLabel,
			int OpenInNewWindow,
			int NavDisplayOrder)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["LinkTarget"] = LinkTarget;
			rowToAdd["LinkLabel"] = LinkLabel;
			rowToAdd["OpenInNewWindow"] = OpenInNewWindow;
			rowToAdd["NavDisplayOrder"] = NavDisplayOrder;
			rowToAdd["Archive"] = 0;
			
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
		/// <param name="NavId">NavId (Primary Key of Record)</param>
		/// <param name="LinkTarget">Target of Link </param>
		/// <param name="LinkLabel">Link's Label</param>
		/// <param name="OpenInNewWindow">Whether this link opens in a new window or not</param>
		/// <param name="NavDisplayOrder">Display order for this link</param>		
		public void Modify(int NavId, 
			string LinkTarget,
			string LinkLabel,
			int OpenInNewWindow,
			int NavDisplayOrder)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["NavId"] = NavId;
			rowToAdd["LinkTarget"] = LinkTarget;
			rowToAdd["LinkLabel"] = LinkLabel;
			rowToAdd["OpenInNewWindow"] = OpenInNewWindow;
			rowToAdd["NavDisplayOrder"] = NavDisplayOrder;
			rowToAdd["Archive"] = 0;

			Validate(rowToAdd, false);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);
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
		/// <param name="LinkTarget">Target of Link </param>
		/// <param name="LinkLabel">Link's Label</param>
		/// <param name="OpenInNewWindow">Whether this link opens in a new window or not</param>
		/// <param name="NavDisplayOrder">Display order for this link</param>		
		public void Set(string LinkTarget,
			string LinkLabel,
			int OpenInNewWindow,
			int NavDisplayOrder)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Find this LinkTarget
			clsNav thisNav = new clsNav(thisDbType, localRecords.dbConnection);

			int numRecords = thisNav.GetByLinkTarget(LinkTarget, matchCriteria_exactMatch());
			switch(numRecords)
			{
				case 0:
					Add(LinkTarget, LinkLabel, OpenInNewWindow, NavDisplayOrder);
					break;
				case 1:
					if(thisNav.my_Archive(0) == 1 
						|| thisNav.my_LinkLabel(0) != LinkLabel
						|| thisNav.my_OpenInNewWindow(0) != OpenInNewWindow
						|| thisNav.my_NavDisplayOrder(0) != NavDisplayOrder)
						Modify(thisNav.my_NavId(0), LinkTarget, LinkLabel, OpenInNewWindow, NavDisplayOrder);
					break;
				default:
					break;
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

		# region My_ Values Nav


		/// <summary>
		/// NavId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>NavId for this Row</returns>
		public int my_NavId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "NavId"));
		}

		/// <summary>
		/// Link's Label
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name of Nav for this Row</returns>
		public string my_LinkLabel(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "LinkLabel");
		}

		/// <summary>
		/// Whether this link opens in a new window or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this link opens in a new window or not</returns>
		public int my_OpenInNewWindow(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "OpenInNewWindow"));
		}

		/// <summary>
		/// Display order for this link
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Display order for this link</returns>
		public int my_NavDisplayOrder(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "NavDisplayOrder"));
		}


		/// <summary>Target for this Link</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Target for this Link</returns>	
		public string my_LinkTarget(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "LinkTarget");
		}

		# endregion
	}
}
