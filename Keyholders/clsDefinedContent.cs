using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsDefinedContent deals with everything to do with data about DefinedContents.
	/// </summary>
	
	[GuidAttribute("3D603A10-A72A-4af2-9701-F91D07174A79")]
	public class clsDefinedContent : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsDefinedContent
		/// </summary>
		public clsDefinedContent() : base("DefinedContent")
		{
		}

		/// <summary>
		/// Constructor for clsDefinedContent; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsDefinedContent(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("DefinedContent")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			MainQ.AddSelectColumn("tblDefinedContent.DefinedContentId");
			MainQ.AddSelectColumn("tblDefinedContent.DefinedContentTitle");
			MainQ.AddSelectColumn("tblDefinedContent.PopUpWidth");
			MainQ.AddSelectColumn("tblDefinedContent.PopUpHeight");
			MainQ.AddSelectColumn("tblDefinedContent.UsesExternalUrl");
			MainQ.AddSelectColumn("tblDefinedContent.ExternalUrl");
			MainQ.AddSelectColumn("tblDefinedContent.Description");

			MainQ.AddFromTable(thisTable);

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblDefinedContent.DefinedContentTitle" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsDefinedContent
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("DefinedContentTitle", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PopUpWidth", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PopUpHeight", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("UsesExternalUrl", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ExternalUrl", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Description", System.Type.GetType("System.String"));

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
		/// Initialises an internal list of all DefinedContents
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
		/// Gets an Defined Content by DefinedContentId
		/// </summary>
		/// <param name="DefinedContentId">Id of Defined Content to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByDefinedContentId(int DefinedContentId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ DefinedContentId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets DefinedContents by PopUpWidth
		/// </summary>
		/// <param name="PopUpWidth">Width to search for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPopUpWidth(int PopUpWidth)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblDefinedContent.PopUpWidth = " 
				+ PopUpWidth.ToString() + crLf;

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
		/// Returns DefinedContents with the specified DefinedContentTitle
		/// </summary>
		/// <param name="DefinedContentTitle">Name of DefinedContent</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of DefinedContents with the specified DefinedContentTitle</returns>
		public int GetByDefinedContentTitle(string DefinedContentTitle, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(DefinedContentTitle, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblDefinedContent.DefinedContentTitle " + match + crLf;

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
		/// <param name="DefinedContentTitle">DefinedContent's Title</param>
		/// <param name="PopUpWidth">Width of Pop-Up Window for this Content</param>
		/// <param name="PopUpHeight">Height of Pop-Up Window for this Content</param>
		/// <param name="UsesExternalUrl">Whether to use an external Url for this content or not</param>
		/// <param name="ExternalUrl">The External Url to use if one is to be used</param>
		/// <param name="Description">Defined Content</param>
		public void Add(string DefinedContentTitle, 
			int PopUpWidth, 
			int PopUpHeight, 
			int UsesExternalUrl, 
			string ExternalUrl, 
			string Description)
		{
			
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["DefinedContentTitle"] = DefinedContentTitle;
			rowToAdd["PopUpWidth"] = PopUpWidth;
			rowToAdd["PopUpHeight"] = PopUpHeight;
			rowToAdd["UsesExternalUrl"] = UsesExternalUrl;
			rowToAdd["ExternalUrl"] = ExternalUrl;
			rowToAdd["Description"] = Description;
			
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
		/// <param name="DefinedContentId">DefinedContentId (Primary Key of Record)</param>
		/// <param name="DefinedContentTitle">DefinedContent's Title</param>
		/// <param name="PopUpWidth">Width of Pop-Up Window for this Content</param>
		/// <param name="PopUpHeight">Height of Pop-Up Window for this Content</param>
		/// <param name="UsesExternalUrl">Whether to use an external Url for this content or not</param>
		/// <param name="ExternalUrl">The External Url to use if one is to be used</param>
		/// <param name="Description">Defined Content</param>
		public void Modify(int DefinedContentId, 
			string DefinedContentTitle, 
			int PopUpWidth, 
			int PopUpHeight, 
			int UsesExternalUrl, 
			string ExternalUrl, 
			string Description)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["DefinedContentId"] = DefinedContentId;
			rowToAdd["DefinedContentTitle"] = DefinedContentTitle;
			rowToAdd["PopUpWidth"] = PopUpWidth;
			rowToAdd["PopUpHeight"] = PopUpHeight;
			rowToAdd["UsesExternalUrl"] = UsesExternalUrl;
			rowToAdd["ExternalUrl"] = ExternalUrl;
			rowToAdd["Description"] = Description;

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

		# region My_ Values DefinedContent

		/// <summary>
		/// <see cref="clsDefinedContent.my_DefinedContentId">Id</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_DefinedContentId">Id</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public int my_DefinedContentId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DefinedContentId"));
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_DefinedContentTitle">Title</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_DefinedContentTitle">Title</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public string my_DefinedContentTitle(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DefinedContentTitle");
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_PopUpWidth">Pop-Up Width</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_PopUpWidth">Pop-Up Width</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public int my_PopUpWidth(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PopUpWidth"));
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_PopUpHeight">Pop-Up Height</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_PopUpHeight">Pop-Up Height</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public int my_PopUpHeight(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PopUpHeight"));
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_UsesExternalUrl">Whether this Defined Content Uses an External Url</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_UsesExternalUrl">Whether this Defined Content Uses an External Url</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public int my_UsesExternalUrl(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "UsesExternalUrl"));
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_ExternalUrl">The External Url, if this Defined Content has it</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_ExternalUrl">The External Url, if this Defined Content has it</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public string my_ExternalUrl(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ExternalUrl");
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_Description">Description</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_Description">Description</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public string my_Description(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Description");
		}

		# endregion
	}
}
