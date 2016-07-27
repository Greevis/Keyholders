using System;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.Odbc;
using Resources;

namespace Keyholders
{
	/// <summary>
	/// clsCorrespondenceSendToCustomer deals with everything to do with data about CorrespondenceSendToCustomers.
	/// </summary>
	
	[GuidAttribute("AC135A53-B38E-4cc2-84CF-4DE1F9C7E80E")]
	public class clsCorrespondenceSendToCustomer : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCorrespondenceSendToCustomer
		/// </summary>
		public clsCorrespondenceSendToCustomer() : base("CorrespondenceSendToCustomer")
		{
		}

		/// <summary>
		/// Constructor for clsCorrespondenceSendToCustomer; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsCorrespondenceSendToCustomer(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("CorrespondenceSendToCustomer")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to CorrespondenceSend Information
		/// </summary>
		public clsQueryPart CorrespondenceSendQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Correspondence Information
		/// </summary>
		public clsQueryPart CorrespondenceQ = new clsQueryPart();		
		
		/// <summary>
		/// Part of the Query that Pertains to User Information
		/// </summary>
		public clsQueryPart UserQ = new clsQueryPart();		

		/// <summary>
		/// Part of the Query that Pertains to Customer Information
		/// </summary>
		public clsQueryPart CustomerQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Person Information
		/// </summary>
		public clsQueryPart PersonQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			CustomerQ = CustomerQueryPart();
			PersonQ = PersonQueryPart();
			MainQ = CorrespondenceSendToCustomerQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[3];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = PersonQ;
			baseQueries[2] = CustomerQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By tblCorrespondenceSendToCustomer.CorrespondenceSendToCustomerId" + crLf;

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsCorrespondenceSendToCustomer
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("IncludesAtLeastOneInvoice", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("IncludesStatement", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CustomerId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PersonId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("IncludesAtLeastOneSticker", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CorrespondenceMedium", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CorrespondenceSendToCustomerStatus", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("StickerIncludedList", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CorrespondenceFile", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateGenerated", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateGeneratedUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSent", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSentUtc", System.Type.GetType("System.String"));
			
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
		/// Initialises an internal list of all CorrespondenceSend-In-Categories
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
		/// Initialises an internal list of all CorrespondenceSend-In-Categories
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAllCorrespondenceSendsToCustomer()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a CorrespondenceSend-In-Category by CorrespondenceSendToCustomerId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByCorrespondenceSendToCustomerId(int CorrespondenceSendToCustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ CorrespondenceSendToCustomerId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;
			
			return localRecords.GetRecords(thisSqlQuery);
		}



		/// <summary>
		/// Gets CorrespondenceSend-In-Categories by CustomerId
		/// </summary>
		/// <param name="CustomerId">Id of CorrespondenceSend Category to retrieve CorrespondenceSend-In-Categories for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where " + thisTable + ".CustomerId = " 
				+ CustomerId.ToString() + crLf;

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
		/// Gets CorrespondenceSendToCustomer by CorrespondenceMedium
		/// </summary>
		/// <param name="CorrespondenceMedium">CorrespondenceMedium to retrieve CorrespondenceSendToCustomer for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCorrespondenceMedium(int CorrespondenceMedium)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where " + thisTable + ".CorrespondenceMedium = " 
				+ CorrespondenceMedium.ToString() + crLf;

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
		/// Gets CorrespondenceSendToCustomer by CorrespondenceSendToCustomerStatus
		/// </summary>
		/// <param name="CorrespondenceSendToCustomerStatus">CorrespondenceSendToCustomerStatus to retrieve CorrespondenceSendToCustomer for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCorrespondenceSendToCustomerStatus(int CorrespondenceSendToCustomerStatus)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where " + thisTable + ".CorrespondenceSendToCustomerStatus = " 
				+ CorrespondenceSendToCustomerStatus.ToString() + crLf;

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
		/// Gets CorrespondenceSendToCustomer by CorrespondenceSendToCustomerStatus
		/// </summary>
		/// <param name="CorrespondenceSendToCustomerStatus">CorrespondenceSendToCustomerStatus to retrieve CorrespondenceSendToCustomer for</param>
		/// <param name="CorrespondenceMedium">CorrespondenceMedium to retrieve CorrespondenceSendToCustomer for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCorrespondenceSendToCustomerStatusCorrespondenceMedium(int CorrespondenceSendToCustomerStatus, int CorrespondenceMedium)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where " + thisTable + ".CorrespondenceSendToCustomerStatus = " 
				+ CorrespondenceSendToCustomerStatus.ToString() + crLf
				+ " And "  + thisTable + ".CorrespondenceMedium = " 
				+ CorrespondenceMedium.ToString() + crLf;

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
		/// <param name="IncludesAtLeastOneInvoice">Correspondence Associated with this CorrespondenceSendToCustomer</param>
		/// <param name="IncludesStatement">CorrespondenceSend Associated with this CorrespondenceSendToCustomer</param>
		/// <param name="CustomerId">Customer Associated with this CorrespondenceSendToCustomer</param>
		/// <param name="PersonId">Person Associated with this CorrespondenceSendToCustomer</param>
		/// <param name="IncludesAtLeastOneSticker">Field that this CorrespondenceSendToCustomer was sent using</param>
		/// <param name="CorrespondenceMedium">Medium that this CorrespondenceSendToCustomer was sent using</param>
		/// <param name="CorrespondenceSendToCustomerStatus">Status of this CorrespondenceSendToCustomer</param>
		/// <param name="StickerIncludedList">StickerIncludedList being sent</param>
		/// <param name="CorrespondenceFile">CorrespondenceFile</param>
		public void Add(int CustomerId, 
			int PersonId, 
			int IncludesAtLeastOneInvoice, 
			int IncludesStatement, 
			int IncludesAtLeastOneSticker,
			int CorrespondenceMedium,
			int CorrespondenceSendToCustomerStatus,
			string StickerIncludedList,
			string CorrespondenceFile)
		{

			DateTime thisUTCDateTime = DateTime.UtcNow;
			
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["IncludesAtLeastOneInvoice"] = IncludesAtLeastOneInvoice;
			rowToAdd["IncludesStatement"] = IncludesStatement;
			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["PersonId"] = PersonId;
			rowToAdd["IncludesAtLeastOneSticker"] = IncludesAtLeastOneSticker;
			rowToAdd["CorrespondenceMedium"] = CorrespondenceMedium;
			rowToAdd["CorrespondenceSendToCustomerStatus"] = CorrespondenceSendToCustomerStatus;
			rowToAdd["StickerIncludedList"] = StickerIncludedList;
			rowToAdd["CorrespondenceFile"] = CorrespondenceFile;
			rowToAdd["DateGenerated"] = localRecords.DBDateTime(FromUtcToClientTime(thisUTCDateTime));
			rowToAdd["DateGeneratedUtc"] =  localRecords.DBDateTime(thisUTCDateTime);			
			rowToAdd["DateSent"] = localRecords.DBDateTime(thisUTCDateTime);
			rowToAdd["DateSentUtc"] =  localRecords.DBDateTime(thisUTCDateTime);

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
		/// are available through the ErrorStickerIncludedList and ErrorFieldName methods
		/// If any Warnings are found, WarningFound will return true, and these Warnings 
		/// are available through the WarningStickerIncludedList and WarningFieldName methods
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CorrespondenceSendToCustomerId">CorrespondenceSendToCustomerId (Primary Key of Record)</param>
		/// <param name="IncludesAtLeastOneInvoice">Correspondence Associated with this CorrespondenceSendToCustomer</param>
		/// <param name="IncludesStatement">CorrespondenceSend Associated with this CorrespondenceSendToCustomer</param>
		/// <param name="CustomerId">Customer Associated with this CorrespondenceSendToCustomer</param>
		/// <param name="PersonId">Person Associated with this CorrespondenceSendToCustomer</param>
		/// <param name="IncludesAtLeastOneSticker">Field that this CorrespondenceSendToCustomer was sent using</param>
		/// <param name="CorrespondenceMedium">Medium that this CorrespondenceSendToCustomer was sent using</param>
		/// <param name="CorrespondenceSendToCustomerStatus">Status of this CorrespondenceSendToCustomer</param>
		/// <param name="StickerIncludedList">StickerIncludedList being sent</param>
		/// <param name="DateGenerated">Date this Correspondence was generated</param>
		/// <param name="DateSent">Date this Correspondence was Sent</param>
		/// <param name="CorrespondenceFile">CorrespondenceFile</param>
		public void Modify(int CorrespondenceSendToCustomerId, 
			int CustomerId, 
			int PersonId, 
			int IncludesAtLeastOneInvoice, 
			int IncludesStatement, 
			int IncludesAtLeastOneSticker,
			int CorrespondenceMedium,
			int CorrespondenceSendToCustomerStatus,
			string StickerIncludedList,
			string CorrespondenceFile,
			string DateGenerated,
			string DateSent)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["CorrespondenceSendToCustomerId"] = CorrespondenceSendToCustomerId;
			rowToAdd["IncludesAtLeastOneInvoice"] = IncludesAtLeastOneInvoice;
			rowToAdd["IncludesStatement"] = IncludesStatement;
			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["PersonId"] = PersonId;
			rowToAdd["IncludesAtLeastOneSticker"] = IncludesAtLeastOneSticker;
			rowToAdd["CorrespondenceMedium"] = CorrespondenceMedium;
			rowToAdd["CorrespondenceSendToCustomerStatus"] = CorrespondenceSendToCustomerStatus;
			rowToAdd["StickerIncludedList"] = StickerIncludedList;
			rowToAdd["CorrespondenceFile"] = CorrespondenceFile;

			if (DateGenerated == "")
			{
				rowToAdd["DateGenerated"] = DBNull.Value;
				rowToAdd["DateGeneratedUtc"] =  DBNull.Value;
			}
			else
			{
				rowToAdd["DateGenerated"] = localRecords.DBDateTime(Convert.ToDateTime(DateGenerated));
				rowToAdd["DateGeneratedUtc"] =  localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(Convert.ToDateTime(DateGenerated))));
			}
			
			if (DateSent == "")
			{
				rowToAdd["DateSent"] = DBNull.Value;
				rowToAdd["DateSentUtc"] =  DBNull.Value;
			}
			else
			{
				rowToAdd["DateSent"] = localRecords.DBDateTime(Convert.ToDateTime(DateSent));
				rowToAdd["DateSentUtc"] =  localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(Convert.ToDateTime(DateSent))));
			}
			
			//Validate the data supplied
			Validate(rowToAdd, false);

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
		/// <param name="CorrespondenceSendToCustomerId">CorrespondenceSendToCustomer (Primary Key of Record)</param>
		public void SetAsSent(int CorrespondenceSendToCustomerId)
		{

			DateTime thisUTCDateTime = DateTime.UtcNow;
			
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			SetAttribute(CorrespondenceSendToCustomerId, "CorrespondenceSendToCustomerStatus", correspondenceSendToCustomerStatus_sent().ToString());
			AddAttributeToSet(CorrespondenceSendToCustomerId, "DateSent", localRecords.DBDateTime(FromUtcToClientTime(thisUTCDateTime)));
			AddAttributeToSet(CorrespondenceSendToCustomerId, "DateSentUtc", localRecords.DBDateTime(thisUTCDateTime));
		}

		/// <summary>
		/// Validates data recieved by the Add or Modify Public Methods.
		/// If any Errors are found, ErrorFound will return true, and these Errors 
		/// are available through the ErrorStickerIncludedList and ErrorFieldName methods
		/// If any Warnings are found, WarningFound will return true, and these Warnings 
		/// are available through the WarningStickerIncludedList and WarningFieldName methods		
		/// </summary>
		/// <param name="valuesToValidate">Values to be Validated.</param>
		/// <param name="newRow">Indicates whether the Row being validated 
		/// is new or already exists in the system</param>
		private void Validate(System.Data.DataRow valuesToValidate, bool newRow)
		{
			//TODO: Add any required Validation here
		}

		#endregion
 
		# region My_ Values CorrespondenceSendToCustomer


		/// <summary>
		/// CorrespondenceSendToCustomerId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CorrespondenceSendToCustomerId for this Row</returns>
		public int my_CorrespondenceSendToCustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CorrespondenceSendToCustomerId"));
		}

		/// <summary>
		/// <see cref="clsCorrespondenceSendToCustomer.my_IncludesAtLeastOneInvoice">IncludesAtLeastOneInvoice</see> of 
		/// <see cref="clsCorrespondenceSendToCustomer">Correspondence</see>
		/// Associated with this CorrespondenceSend</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSendToCustomer.my_IncludesAtLeastOneInvoice">IncludesAtLeastOneInvoice</see> 
		/// of <see cref="clsCorrespondenceSendToCustomer">Correspondence</see> 
		/// for this CorrespondenceSend</returns>	
		public int my_IncludesAtLeastOneInvoice(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IncludesAtLeastOneInvoice"));
		}

		/// <summary>
		/// <see cref="clsCorrespondenceSendToCustomer.my_IncludesStatement">IncludesStatement</see> of 
		/// <see cref="clsCorrespondenceSendToCustomer">CorrespondenceSend</see>
		/// Associated with this CorrespondenceSendToCustomer</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSendToCustomer.my_IncludesStatement">Id</see> 
		/// of <see cref="clsCorrespondenceSendToCustomer">CorrespondenceSend</see> 
		/// for this CorrespondenceSendToCustomer</returns>
		public int my_IncludesStatement(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IncludesStatement"));
		}


		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">CustomerId</see> of 
		/// <see cref="clsCustomer">CorrespondenceSend Category</see>
		/// Associated with this CorrespondenceSendToCustomer</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> 
		/// of <see cref="clsCustomer">CorrespondenceSend Category</see> 
		/// for this CorrespondenceSendToCustomer</returns>
		public int my_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerId"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">PersonId</see> of 
		/// <see cref="clsPerson">CorrespondenceSend Category</see>
		/// Associated with this CorrespondenceSendToPerson</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> 
		/// of <see cref="clsPerson">CorrespondenceSend Category</see> 
		/// for this CorrespondenceSendToPerson</returns>
		public int my_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PersonId"));
		}
		
		/// <summary>
		/// Correspondence Field Type
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Correspondence Field Type</returns>
		public int my_IncludesAtLeastOneSticker(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IncludesAtLeastOneSticker"));
		}

		/// <summary>
		/// Correspondence Medium
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Correspondence Medium</returns>
		public int my_CorrespondenceMedium(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CorrespondenceMedium"));
		}

		/// <summary>
		/// Correspondence SendToCustomerStatus
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Correspondence SendToCustomerStatus</returns>
		public int my_CorrespondenceSendToCustomerStatus(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CorrespondenceSendToCustomerStatus"));
		}

		/// <summary>
		/// StickerIncludedList To Send To Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>StickerIncludedList To Send To Customer</returns>
		public string my_StickerIncludedList(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "StickerIncludedList");
		}

		/// <summary>
		/// CorrespondenceFile To Send To Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CorrespondenceFile To Send To Customer</returns>
		public string my_CorrespondenceFile(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CorrespondenceFile");
		}

		/// <summary>
		/// <see cref="clsCorrespondenceSendToCustomer.my_DateGenerated">Date</see> this 
		/// <see cref="clsCorrespondenceSendToCustomer">Correspondence Send was Generated (Client Time)</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSendToCustomer.my_DateGenerated">Date</see> this 
		/// <see cref="clsCorrespondenceSendToCustomer">Correspondence Send was Generated (Client Time)</see>
		/// </returns>	
		public string my_DateGenerated(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateGenerated");
		}

		/// <summary>
		/// <see cref="clsCorrespondenceSendToCustomer.my_DateGeneratedUtc">Date</see> this 
		/// <see cref="clsCorrespondenceSendToCustomer">Correspondence Send To Customer was Generated (UTC Time)</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSendToCustomer.my_DateGeneratedUtc">Date</see> this 
		/// <see cref="clsCorrespondenceSendToCustomer">Correspondence Send To Customer was Generated (UTC Time)</see>
		/// </returns>
		public string my_DateGeneratedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateGeneratedUtc");
		}

		/// <summary>
		/// <see cref="clsCorrespondenceSendToCustomer.my_DateSent">Date</see> this 
		/// <see cref="clsCorrespondenceSendToCustomer">Correspondence Send To Customer was Sent (Client Time)</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSendToCustomer.my_DateSent">Date</see> this 
		/// <see cref="clsCorrespondenceSendToCustomer">Correspondence Send To Customer was Sent (Client Time)</see>
		/// </returns>	
		public string my_DateSent(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSent");
		}

		/// <summary>
		/// <see cref="clsCorrespondenceSendToCustomer.my_DateSentUtc">Date</see> this 
		/// <see cref="clsCorrespondenceSendToCustomer">Correspondence Send To Customer was Sent (UTC Time)</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSendToCustomer.my_DateSentUtc">Date</see> this 
		/// <see cref="clsCorrespondenceSendToCustomer">Correspondence Send To Customer was Sent (UTC Time)</see>
		/// </returns>
		public string my_DateSentUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSentUtc");
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

		# region My_ Values Person

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">Id</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public int my_Person_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Person_PersonId"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">Id</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public int my_Person_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Person_CustomerId"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_Title">Title</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Title">Title</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_Title");
		}
				
		/// <summary>
		/// <see cref="clsPerson.my_FirstName">First Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_FirstName">First Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_FirstName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_LastName">Last Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_LastName">Last Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_LastName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_FullName">Full Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_FullName">Full Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_FullName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_UserName">User Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_UserName">User Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_UserName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_UserName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_Password">Password</see> for
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Password">Password</see> for 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_Password(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_Password");
		}

		/// <summary>
		/// <see cref="clsPerson.my_Email">Email Address</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Email">Email Address</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_Email");
		}


		/// <summary>
		/// <see cref="clsPerson.my_QuickPostalAddress">Quick Postal Address</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickPostalAddress">Quick Postal Address</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickPostalAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickPostalAddress");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickDaytimePhone">Quick Daytime Phone</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickDaytimePhone">Quick Daytime Phone</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickDaytimePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickDaytimePhone");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickDaytimeFax">Quick Daytime Fax</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickDaytimeFax">Quick Daytime Fax</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickDaytimeFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickDaytimeFax");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickAfterHoursPhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickAfterHoursPhone");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickAfterHoursFax">Quick After Hours Fax</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickAfterHoursFax">Quick After Hours Fax</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickAfterHoursFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickAfterHoursFax");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickMobilePhone">Quick Mobile Phone</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickMobilePhone">Quick Mobile Phone</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickMobilePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickMobilePhone");
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
		public int my_Person_PreferredContactMethod(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Person_PreferredContactMethod"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_IsCustomerAdmin">
		/// Whether this Person is an Administrator for this Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_IsCustomerAdmin">
		/// Whether this Person is an Administrator for this Customer</see>
		/// </returns>
		public int my_Person_IsCustomerAdmin(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Person_IsCustomerAdmin"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_PositionInCompany">Position in Company</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PositionInCompany">Position in Company</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_PositionInCompany(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_PositionInCompany");
		}

		/// <summary>
		/// <see cref="clsPerson.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_KdlComments");
		}

		/// <summary>
		/// <see cref="clsPerson.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_CustomerComments");
		}

		/// <summary>
		/// <see cref="clsPerson.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_DateLastLoggedIn(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_DateLastLoggedIn");
		}


		#endregion

	}
}
