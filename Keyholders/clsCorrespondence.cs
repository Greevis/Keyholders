using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using Microsoft.Data.Odbc;


namespace Keyholders
{
	/// <summary>
	/// clsCorrespondence deals with everything to do with data about Correspondences.
	/// </summary>
	
	[GuidAttribute("F68F1F31-1B50-43ec-868F-0AA5BA330B41")]
	public class clsCorrespondence : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCorrespondence
		/// </summary>
		public clsCorrespondence() : base("Correspondence")
		{
		}

		/// <summary>
		/// Constructor for clsCorrespondence; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsCorrespondence(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Correspondence")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to CorrespondenceSendToCustomer Information
		/// </summary>
		public clsQueryPart CorrespondenceSendToCustomerSummaryQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			
			MainQ.AddSelectColumn("tblCorrespondence.CorrespondenceId");
			MainQ.AddSelectColumn("tblCorrespondence.CorrespondenceTypeId");
			MainQ.AddSelectColumn("tblCorrespondence.CorrespondenceName");
			MainQ.AddSelectColumn("tblCorrespondence.CorrespondenceDescription");
			MainQ.AddSelectColumn("tblCorrespondence.AddressFrom");
			MainQ.AddSelectColumn("tblCorrespondence.AddressTo");
			MainQ.AddSelectColumn("tblCorrespondence.AddressCC");
			MainQ.AddSelectColumn("tblCorrespondence.AddressBCC");
			MainQ.AddSelectColumn("tblCorrespondence.Subject");
			MainQ.AddSelectColumn("tblCorrespondence.PlainTextMessage");
			MainQ.AddSelectColumn("tblCorrespondence.RichTextMessage");
			MainQ.AddSelectColumn("tblCorrespondence.AttachmentMessage");
			MainQ.AddSelectColumn("tblCorrespondence.FaxMessage");
			MainQ.AddSelectColumn("tblCorrespondence.SnailMessage");
			MainQ.AddSelectColumn("tblCorrespondence.IsAbleToBeEmailed");
			MainQ.AddSelectColumn("tblCorrespondence.IsAbleToBeFaxed");
			MainQ.AddSelectColumn("tblCorrespondence.IsAbleToBePosted");
			MainQ.AddSelectColumn("tblCorrespondence.Archive");

			MainQ.AddFromTable(thisTable);

			CorrespondenceSendToCustomerSummaryQ.AddSelectColumn("tblCorrespondenceSendToCustomer.NumCorrespondenceSendToCustomers");
			CorrespondenceSendToCustomerSummaryQ.AddSelectColumn("tblCorrespondenceSendToCustomer.TotalSentToCustomersInToField");
			CorrespondenceSendToCustomerSummaryQ.AddSelectColumn("tblCorrespondenceSendToCustomer.TotalSentToCustomersInFromField");
			CorrespondenceSendToCustomerSummaryQ.AddSelectColumn("tblCorrespondenceSendToCustomer.TotalSentToCustomersInCCField");
			CorrespondenceSendToCustomerSummaryQ.AddSelectColumn("tblCorrespondenceSendToCustomer.TotalSentToCustomersInBCCField");
			CorrespondenceSendToCustomerSummaryQ.AddSelectColumn("tblCorrespondenceSendToCustomer.LastDateSent");

			CorrespondenceSendToCustomerSummaryQ.AddFromTable("(Select tblCorrespondence.CorrespondenceId," + crLf
				+ "sum(case when tblCorrespondenceSendToCustomer.CorrespondenceFieldId = 1 then 1 else 0 end) as TotalSentToCustomersInToField," + crLf
				+ "sum(case when tblCorrespondenceSendToCustomer.CorrespondenceFieldId = 2 then 1 else 0 end) as TotalSentToCustomersInFromField," + crLf
				+ "sum(case when tblCorrespondenceSendToCustomer.CorrespondenceFieldId = 3 then 1 else 0 end) as TotalSentToCustomersInCCField," + crLf
				+ "sum(case when tblCorrespondenceSendToCustomer.CorrespondenceFieldId = 4 then 1 else 0 end) as TotalSentToCustomersInBCCField,"  + crLf
				+ "max(tblCorrespondenceSendToCustomer.DateSent) as LastDateSent,"  + crLf
				+ "count(tblCorrespondenceSendToCustomer.CorrespondenceSendToCustomerId) as NumCorrespondenceSendToCustomers"  + crLf
				+ "from tblCorrespondence left outer join "  + crLf
				+ "	(select tblCorrespondenceSendToCustomer.CorrespondenceSendToCustomerId,  "
				+ "		tblCorrespondenceSendToCustomer.CorrespondenceSendId, "
				+ "		tblCorrespondenceSendToCustomer.CorrespondenceId, "
				+ "		tblCorrespondenceSendToCustomer.CorrespondenceFieldId, "
				+ "		tblCorrespondenceSendToCustomer.DateSent"  + crLf
				+ "	From tblCorrespondenceSendToCustomer"  + crLf
				+ ") tblCorrespondenceSendToCustomer"  + crLf
				+ "on tblCorrespondence.CorrespondenceId = tblCorrespondenceSendToCustomer.CorrespondenceId"  + crLf
				+ "Group by tblCorrespondence.CorrespondenceId) tblCorrespondenceSendToCustomer");

			CorrespondenceSendToCustomerSummaryQ.AddJoin("tblCorrespondenceSendToCustomer.CorrespondenceId = tblCorrespondence.CorrespondenceId");

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = CorrespondenceSendToCustomerSummaryQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblCorrespondence.CorrespondenceName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsCorrespondence
		/// </summary>
		public override void Initialise()
		{
			
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("CorrespondenceTypeId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CorrespondenceName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CorrespondenceDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AddressTo", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AddressFrom", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AddressCC", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AddressBCC", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Subject", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PlainTextMessage", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("RichTextMessage", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("FaxMessage", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("SnailMessage", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AttachmentMessage", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("IsAbleToBeEmailed", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("IsAbleToBeFaxed", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("IsAbleToBePosted", System.Type.GetType("System.Int32"));
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


		}

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Correspondences
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = CorrespondenceSendToCustomerSummaryQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets an Correspondence by CorrespondenceId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByCorrespondenceId(int CorrespondenceId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = CorrespondenceSendToCustomerSummaryQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				"(Select * From " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ CorrespondenceId.ToString() + ") " + thisTable,
				thisTable
				);

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Correspondences by CorrespondenceTypeId
		/// </summary>
		/// <param name="CorrespondenceTypeId">Type of Correspondence to return</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCorrespondenceTypeId(int CorrespondenceTypeId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = CorrespondenceSendToCustomerSummaryQ;

			string condition = "(Select * From " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblCorrespondence.CorrespondenceTypeId = " 
				+ CorrespondenceTypeId.ToString() + crLf;

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
		/// Gets Correspondences that are not of a certain  CorrespondenceTypeId
		/// </summary>
		/// <param name="CorrespondenceTypeId">Type of Correspondence not to return</param>
		/// <returns>Number of resulting records</returns>
		public int GetByNotCorrespondenceTypeId(int CorrespondenceTypeId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = CorrespondenceSendToCustomerSummaryQ;

			string condition = "(Select * From " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblCorrespondence.CorrespondenceTypeId != " 
				+ CorrespondenceTypeId.ToString() + crLf;

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
		/// Returns Correspondences with the specified CorrespondenceName
		/// </summary>
		/// <param name="CorrespondenceName">Name of Correspondence</param>
		/// <param name="MatchCriteria">Criteria to match, From the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of Correspondences with the specified CorrespondenceName</returns>
		public int GetByCorrespondenceName(string CorrespondenceName, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * From " + thisTable + crLf;	

			string match = MatchCondition(CorrespondenceName, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblCorrespondence.CorrespondenceName " + match + crLf;

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
		/// Gets a Distinct List of Correspondence Names only. This is intented for use e.g. as an
		/// auto-completing drop down.
		/// </summary>
		/// <param name="CorrespondenceName">Name of Correspondence</param>
		/// <param name="MatchCriteria">Criteria to match Correspondence Name, From the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctCorrespondenceName(string CorrespondenceName, int MatchCriteria)
		{
			string fieldRequired = "CorrespondenceName";
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct "+ thisTable + "." + fieldRequired);

			string condition = "(Select * From " + thisTable + crLf;
			
			string match = MatchCondition(CorrespondenceName, 
				(matchCriteria) MatchCriteria);

			//Additional Conditions
			condition += "Where " + fieldRequired + match + crLf;
			
			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				condition + ") " + thisTable,
				thisTable
				);

			//Ordering
			thisSqlQuery += "Order By tblCorrespondence.CorrespondenceName" + crLf;

			return localRecords.GetRecords(thisSqlQuery);

		}

		#endregion

		# region Add/Modify/Validate/Save

		
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Customer table stack; the SaveCustomers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CorrespondenceName">Name of Correspondence</param>
		/// <param name="CorrespondenceDescription">Description of Correspondence</param>
		/// <param name="CorrespondenceTypeId">Type of Correspondence</param>
		/// <param name="AddressTo">Recipient Address of the Correspondence</param>
		/// <param name="AddressFrom">From Address of the Correspondence</param>
		/// <param name="AddressCC">CC Address(s) of the Correspondence</param>
		/// <param name="AddressBCC">AddressBCC Address(s) of the Correspondence</param>
		/// <param name="Subject">Subject of the Correspondence</param>
		/// <param name="PlainTextMessage">Plain Text version of the Correspondence</param>
		/// <param name="RichTextMessage">Rich Text version of the Correspondence</param>
		/// <param name="AttachmentMessage">Email Attachment version of the Correspondence</param>
		/// <param name="FaxMessage">Fax version of the Correspondence</param>
		/// <param name="SnailMessage">Snail Mail version of the Correspondence</param>
		/// <param name="IsAbleToBeEmailed">This Correspondence can be Emailed</param>
		/// <param name="IsAbleToBeFaxed">This Correspondence can be Faxed</param>
		/// <param name="IsAbleToBePosted">This Correspondence can be Posted</param>
		public void Add(string CorrespondenceName, 
			string CorrespondenceDescription,
			int CorrespondenceTypeId, 
			string AddressTo,
			string AddressFrom,
			string AddressCC,
			string AddressBCC,
			string Subject,
			string PlainTextMessage,
			string RichTextMessage,
			string AttachmentMessage,
			string FaxMessage,
			string SnailMessage,
			int IsAbleToBeEmailed,
			int IsAbleToBeFaxed,
			int IsAbleToBePosted)
		{

			
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["CorrespondenceName"] = CorrespondenceName;
			rowToAdd["CorrespondenceDescription"] = CorrespondenceDescription;
			rowToAdd["CorrespondenceTypeId"] = CorrespondenceTypeId;
			rowToAdd["AddressTo"] = AddressTo;
			rowToAdd["AddressFrom"] = AddressFrom;
			rowToAdd["AddressCC"] = AddressCC;
			rowToAdd["AddressBCC"] = AddressBCC;
			rowToAdd["Subject"] = Subject;
			rowToAdd["PlainTextMessage"] = PlainTextMessage;
			rowToAdd["RichTextMessage"] = RichTextMessage;
			rowToAdd["AttachmentMessage"] = AttachmentMessage;
			rowToAdd["FaxMessage"] = FaxMessage;
			rowToAdd["SnailMessage"] = SnailMessage;
			rowToAdd["IsAbleToBeEmailed"] = IsAbleToBeEmailed;
			rowToAdd["IsAbleToBeFaxed"] = IsAbleToBeFaxed;
			rowToAdd["IsAbleToBePosted"] = IsAbleToBePosted;
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
		/// internal Customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CorrespondenceId">CorrespondenceId (Primary Key of Record)</param>
		/// <param name="CorrespondenceName">Name of Correspondence</param>
		/// <param name="CorrespondenceDescription">Description of Correspondence</param>
		/// <param name="CorrespondenceTypeId">Type of Correspondence</param>
		/// <param name="AddressTo">Recipient Address of the Correspondence</param>
		/// <param name="AddressFrom">AddressFrom Address of the Correspondence</param>
		/// <param name="AddressCC">CC Address(s) of the Correspondence</param>
		/// <param name="AddressBCC">BCC Address(s) of the Correspondence</param>
		/// <param name="Subject">Subject of the Correspondence</param>
		/// <param name="PlainTextMessage">Plain Text version of the Correspondence</param>
		/// <param name="RichTextMessage">Rich Text version of the Correspondence</param>
		/// <param name="AttachmentMessage">Email Attachment version of the Correspondence</param>
		/// <param name="FaxMessage">Fax version of the Correspondence</param>
		/// <param name="SnailMessage">Snail Mail version of the Correspondence</param>
		/// <param name="IsAbleToBeEmailed">This Correspondence can be Emailed</param>
		/// <param name="IsAbleToBeFaxed">This Correspondence can be Faxed</param>
		/// <param name="IsAbleToBePosted">This Correspondence can be Posted</param>
		public void Modify(int CorrespondenceId, 
			string CorrespondenceName, 
			string CorrespondenceDescription,
			int CorrespondenceTypeId, 
			string AddressTo,
			string AddressFrom,
			string AddressCC,
			string AddressBCC,
			string Subject,
			string PlainTextMessage,
			string RichTextMessage,
			string AttachmentMessage,
			string FaxMessage,
			string SnailMessage,
			int IsAbleToBeEmailed,
			int IsAbleToBeFaxed,
			int IsAbleToBePosted)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["CorrespondenceId"] = CorrespondenceId;
			rowToAdd["CorrespondenceName"] = CorrespondenceName;
			rowToAdd["CorrespondenceDescription"] = CorrespondenceDescription;
			rowToAdd["CorrespondenceTypeId"] = CorrespondenceTypeId;
			rowToAdd["AddressTo"] = AddressTo;
			rowToAdd["AddressFrom"] = AddressFrom;
			rowToAdd["AddressCC"] = AddressCC;
			rowToAdd["AddressBCC"] = AddressBCC;
			rowToAdd["Subject"] = Subject;
			rowToAdd["PlainTextMessage"] = PlainTextMessage;
			rowToAdd["RichTextMessage"] = RichTextMessage;
			rowToAdd["AttachmentMessage"] = AttachmentMessage;
			rowToAdd["FaxMessage"] = FaxMessage;
			rowToAdd["SnailMessage"] = SnailMessage;
			rowToAdd["IsAbleToBeEmailed"] = IsAbleToBeEmailed;
			rowToAdd["IsAbleToBeFaxed"] = IsAbleToBeFaxed;
			rowToAdd["IsAbleToBePosted"] = IsAbleToBePosted;
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

		# region My_ Values Correspondence


		/// <summary>
		/// <see cref="clsCorrespondence.my_CorrespondenceId">Id</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_CorrespondenceId">Id</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public int my_CorrespondenceId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CorrespondenceId"));
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_CorrespondenceName">Name</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_CorrespondenceName">Name</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_CorrespondenceName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CorrespondenceName");
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_CorrespondenceDescription">Description</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_CorrespondenceDescription">Description</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_CorrespondenceDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CorrespondenceDescription");
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_CorrespondenceTypeId">Type</see> of
		/// <see cref="clsCorrespondence">Correspondence</see> from the enumeration
		/// <see cref="clsKeyBase.correspondenceType">correspondenceType</see> 
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_CorrespondenceTypeId">Type</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> from the enumeration
		/// <see cref="clsKeyBase.correspondenceType">correspondenceType</see> 
		/// </returns>	
		public int my_CorrespondenceTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CorrespondenceTypeId"));
		}


		/// <summary>
		/// <see cref="clsCorrespondence.my_AddressFrom">Addresses in the 'From' Section</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_AddressFrom">Addresses in the 'From' Section</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_AddressFrom(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "AddressFrom");
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_AddressTo">Addresses in the 'To' Section</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_AddressTo">Addresses in the 'To' Section</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_AddressTo(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "AddressTo");
		}

		
		/// <summary>
		/// <see cref="clsCorrespondence.my_AddressCC">Addresses in the 'CC' Section</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_AddressCC">Addresses in the 'CC' Section</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_AddressCC(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "AddressCC");
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_AddressBCC">Addresses in the 'BCC' Section</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_AddressBCC">Addresses in the 'BCC' Section</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_AddressBCC(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "AddressBCC");
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_Subject">Subject</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_Subject">Subject</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_Subject(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Subject");
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_PlainTextMessage">Plain Text Email Version of Message</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_PlainTextMessage">Plain Text Email Version of Message</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_PlainTextMessage(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PlainTextMessage");
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_RichTextMessage">Rich Text Email Version of Message</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_RichTextMessage">Rich Text Email Version of Message</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_RichTextMessage(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "RichTextMessage");
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_AttachmentMessage">Email Attachment Version of Message</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_AttachmentMessage">Email Attachment Version of Message</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_AttachmentMessage(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "AttachmentMessage");
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_FaxMessage">Fax Version of Message</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_FaxMessage">Fax Version of Message</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_FaxMessage(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FaxMessage");
		}

		/// <summary>
		/// <see cref="clsCorrespondence.my_SnailMessage">Snail Version of Message</see> of
		/// <see cref="clsCorrespondence">Correspondence</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondence.my_SnailMessage">Snail Version of Message</see> of 
		/// <see cref="clsCorrespondence">Correspondence</see> 
		/// </returns>
		public string my_SnailMessage(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "SnailMessage");
		}


		/// <summary>
		/// Whether this 
		/// <see cref="clsCorrespondence">Correspondence</see> is 
		/// <see cref="clsCorrespondence.my_IsAbleToBeEmailed">able to be emailed</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// Whether this 
		/// <see cref="clsCorrespondence">Correspondence</see> is 
		/// <see cref="clsCorrespondence.my_IsAbleToBeEmailed">able to be emailed</see>
		/// </returns>	
		public int my_IsAbleToBeEmailed(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsAbleToBeEmailed"));
		}

		/// <summary>
		/// Whether this 
		/// <see cref="clsCorrespondence">Correspondence</see> is 
		/// <see cref="clsCorrespondence.my_IsAbleToBeFaxed">able to be emailed</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// Whether this 
		/// <see cref="clsCorrespondence">Correspondence</see> is 
		/// <see cref="clsCorrespondence.my_IsAbleToBeFaxed">able to be emailed</see>
		/// </returns>	
		public int my_IsAbleToBeFaxed(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsAbleToBeFaxed"));
		}

		/// <summary>
		/// Whether this 
		/// <see cref="clsCorrespondence">Correspondence</see> is 
		/// <see cref="clsCorrespondence.my_IsAbleToBePosted">able to be emailed</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// Whether this 
		/// <see cref="clsCorrespondence">Correspondence</see> is 
		/// <see cref="clsCorrespondence.my_IsAbleToBePosted">able to be emailed</see>
		/// </returns>	
		public int my_IsAbleToBePosted(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsAbleToBePosted"));
		}

		#endregion 

		#region My_ Values derrived values


		/// <summary>
		/// Number of Customers this Correspondence has been sent to
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of Customers this Correspondence has been sent to</returns>
		public int my_NumCorrespondenceSendToCustomers(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "NumCorrespondenceSendToCustomers"));
		}

		/// <summary>
		/// Number of Customers this Correspondence has been sent to in the 'To' Field
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of Customers this Correspondence has been sent to in the 'To' Field</returns>
		public int my_TotalSentToCustomersInToField(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalSentToCustomersInToField"));
		}

		/// <summary>
		/// Number of Customers this Correspondence has been sent to in the 'CC' Field
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of Customers this Correspondence has been sent to in the 'CC' Field</returns>
		public int my_TotalSentToCustomersInCCField(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalSentToCustomersInCCField"));
		}

		/// <summary>
		/// Number of Customers this Correspondence has been sent to in the 'BCC' Field
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of Customers this Correspondence has been sent to in the 'BCC' Field</returns>
		public int my_TotalSentToCustomersInBCCField(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalSentToCustomersInBCCField"));
		}

		/// <summary>
		/// Number of Customers this Correspondence has been sent to in the 'From' Field
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of Customers this Correspondence has been sent to in the 'From' Field</returns>
		public int my_TotalSentToCustomersInFromField(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalSentToCustomersInFromField"));
		}


		/// <summary>
		/// Last Date this Correspondence was sent to anyone
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Last Date this Correspondence was sent to anyo</returns>
		public string my_LastDateSent(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "LastDateSent");
		}


		#endregion 


	}
}
