using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsCCAttempt deals with everything to do with data about Attempted Credit Card Transactions.
	/// </summary>
	
	[GuidAttribute("E746145E-4298-4f19-B2C9-10AA331FA339")]
	public class clsCCAttempt : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCCAttempt
		/// </summary>
		public clsCCAttempt() : base("CCAttempt")
		{
		}

		/// <summary>
		/// Constructor for clsCCAttempt; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsCCAttempt(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("CCAttempt")
		{
			Connect(typeOfDb, odbcConnection);
		}

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

			MainQ = CCAttemptQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[1];
			
			baseQueries[0] = MainQ;
	
			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By tblCCAttempt.CCAttemptId" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsCCAttempt
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("OrderId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CustomerId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("UserId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CardTypeId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("NameOnCard", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("StartDate", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ExpiryDate", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("IssueNumber", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CardNumber", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Amount", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSubmitted", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSubmittedUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateAttempted", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateAttemptedUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Success", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("IsManual", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("DeclineReason", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Receipt", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerIPAddress", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("UserIPAddress", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PersonName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("UserName", System.Type.GetType("System.String"));

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
		/// Initialises an internal list of all Attempted Credit Card Transactions
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
		/// Gets a CCAttempt by CCAttemptId
		/// </summary>
		/// <param name="CCAttemptId">Id of CCAttempt to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCCAttemptId(int CCAttemptId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + "." + thisPk +" = " + CCAttemptId.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets all Premises who are associated with a certain Customer
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".CustomerId = " + CustomerId.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition,
				thisTable
				);

			//Ordering
			thisSqlQuery += orderBySqlQuery;
			


			return localRecords.GetRecords(thisSqlQuery);
		}




		/// <summary>
		/// Gets Attempted Credit Card Transactions by CustomerId, and wether they suceeded or failed
		/// </summary>
		/// <param name="CustomerId">Id of Customer to retrieve Attempted Credit Card Transactions for</param>
		/// <param name="Success">State to return; Successes or failures</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerIdSuccess(int CustomerId, int Success)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".CustomerId = " + CustomerId.ToString() + crLf;
			condition += " And " + thisTable + ".Success = " + Success.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition,
				thisTable
				);
            
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Attempted Credit Card Transactions by PersonId
		/// </summary>
		/// <param name="PersonId">Id of Person to retrieve Attempted Credit Card Transactions for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonId(int PersonId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".PersonId = " + PersonId.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Attempted Credit Card Transactions by PersonId, and wether they suceeded or failed
		/// </summary>
		/// <param name="PersonId">Id of Person to retrieve Attempted Credit Card Transactions for</param>
		/// <param name="Success">State to return; Successes or failures</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonIdSuccess(int PersonId, int Success)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".PersonId = " + PersonId.ToString() + crLf;
			condition += " And " + thisTable + ".Success = " + Success.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Attempted Credit Card Transactions by OrderId
		/// </summary>
		/// <param name="OrderId">Id of Order to retrieve Attempted Credit Card Transactions for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderId(int OrderId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".OrderId = " + OrderId.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Attempted Credit Card Transactions by OrderId, and wether they suceeded or failed
		/// </summary>
		/// <param name="OrderId">Id of Order to retrieve Attempted Credit Card Transactions for</param>
		/// <param name="Success">State to return; Successes or failures</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderIdSuccess(int OrderId, int Success)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".OrderId = " + OrderId.ToString() + crLf;
			condition += " And " + thisTable + ".Success = " + Success.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
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
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="OrderId">Order Associated with this Attempted Credit Card Transaction, if any</param>
		/// <param name="CustomerId">Customer Associated with this Attempted Credit Card Transaction, if any</param>
		/// <param name="CustomerName">Name of Customer Associated with this Attempted Credit Card Transaction</param>
		/// <param name="PersonName">Name of Person Associated with this Attempted Credit Card Transaction</param>
		/// <param name="CardTypeId">Type of Card this was</param>
		/// <param name="NameOnCard">Name provided on the Card</param>
		/// <param name="StartDate">Start Date on the Card (if required)</param>
		/// <param name="ExpiryDate">Expiry Date of the Card</param>
		/// <param name="IssueNumber">Issue Number of the Card (if required)</param>
		/// <param name="CardNumber">Card Number</param>
		/// <param name="Amount">Ammoun of the Attempted Credit Card Transaction</param>
		/// <param name="Success">Whether this Attempted Credit Card Transaction was Successful or not</param>
		/// <param name="DeclineReason">Reason Attempted Credit Card Transaction was declined (if any)</param>
		/// <param name="Receipt">Receipt for this Attempted Credit Card Transaction</param>
		/// <param name="CustomerIPAddress">IP Address for this Attempted Credit Card Transaction</param>
		public void AddAuto(int OrderId,
			int CustomerId,
			string CustomerName,
			string PersonName,
			int CardTypeId,
			string NameOnCard, 
			string StartDate, 
			string ExpiryDate, 
			string IssueNumber, 
			string CardNumber, 
			decimal Amount, 
			int Success,
			string DeclineReason, 
			string Receipt, 
			string CustomerIPAddress)
		{
			
			DateTime thisTranUTCDateTime = DateTime.UtcNow;

			string ClientDateTimeOfTran = localRecords.DBDateTime(FromUtcToClientTime(thisTranUTCDateTime));
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();
			
			if (OrderId == 0)
				rowToAdd["OrderId"] = DBNull.Value;
			else
				rowToAdd["OrderId"] = OrderId;

			rowToAdd["UserId"] = DBNull.Value;
			
			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["CustomerName"] = CustomerName;
			rowToAdd["PersonName"] = PersonName;
			rowToAdd["UserName"] = "";

			rowToAdd["CardTypeId"] = CardTypeId;
			rowToAdd["NameOnCard"] = NameOnCard;
			rowToAdd["StartDate"] = StartDate;
			rowToAdd["ExpiryDate"] = ExpiryDate;
			rowToAdd["IssueNumber"] = IssueNumber;
			rowToAdd["CardNumber"] = ObfuscateCardNumber(CardNumber, true);
			rowToAdd["Amount"] = Amount;

			rowToAdd["DateAttempted"] = ClientDateTimeOfTran;
			rowToAdd["DateAttemptedUtc"] = localRecords.DBDateTime(thisTranUTCDateTime);
			rowToAdd["DateSubmitted"] = ClientDateTimeOfTran;
			rowToAdd["DateSubmittedUtc"] = localRecords.DBDateTime(thisTranUTCDateTime);

			rowToAdd["Success"] = Success;
			rowToAdd["IsManual"] = 0;
			rowToAdd["DeclineReason"] = DeclineReason;
			rowToAdd["Receipt"] = Receipt;
			rowToAdd["CustomerIPAddress"] = CustomerIPAddress;
			rowToAdd["UserIPAddress"] = "";
			

			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				newDataToAdd.Rows.Add(rowToAdd);
			}

		}


		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="OrderId">Order Associated with this Attempted Credit Card Transaction, if any</param>
		/// <param name="CustomerId">Customer Associated with this Attempted Credit Card Transaction</param>
		/// <param name="CustomerName">Name of Customer Associated with this Attempted Credit Card Transaction</param>
		/// <param name="PersonName">Name of Person Associated with this Attempted Credit Card Transaction</param>
		/// <param name="CardTypeId">Type of Card this was</param>
		/// <param name="NameOnCard">Name provided on the Card</param>
		/// <param name="StartDate">Start Date on the Card (if required)</param>
		/// <param name="ExpiryDate">Expiry Date of the Card</param>
		/// <param name="IssueNumber">Issue Number of the Card (if required)</param>
		/// <param name="CardNumber">Card Number</param>
		/// <param name="Amount">Ammoun of the Attempted Credit Card Transaction</param>
		/// <param name="CustomerIPAddress">Customer IP Address for this Attempted Credit Card Transaction</param>
		public void AddManual(int OrderId,
			int CustomerId,
			string CustomerName,
			string PersonName,
			int CardTypeId,
			string NameOnCard, 
			string StartDate, 
			string ExpiryDate, 
			string IssueNumber, 
			string CardNumber, 
			decimal Amount, 
			string CustomerIPAddress)
		{
			
			DateTime thisTranUTCDateTime = DateTime.UtcNow;

			string ClientDateTimeOfTran = localRecords.DBDateTime(FromUtcToClientTime(thisTranUTCDateTime));
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			if (OrderId == 0)
				rowToAdd["OrderId"] = DBNull.Value;
			else
				rowToAdd["OrderId"] = OrderId;

			rowToAdd["CustomerId"] = CustomerId;

			rowToAdd["CustomerName"] = CustomerName;
			rowToAdd["PersonName"] = PersonName;
			rowToAdd["UserName"] = "";

			rowToAdd["UserId"] = DBNull.Value;
			rowToAdd["CardTypeId"] = CardTypeId;
			rowToAdd["NameOnCard"] = NameOnCard;
			rowToAdd["StartDate"] = StartDate;
			rowToAdd["ExpiryDate"] = ExpiryDate;
			rowToAdd["IssueNumber"] = IssueNumber;
			rowToAdd["CardNumber"] = ObfuscateCardNumber(CardNumber, false);
			rowToAdd["Amount"] = Amount;

			rowToAdd["DateAttempted"] = DBNull.Value;
			rowToAdd["DateAttemptedUtc"] = DBNull.Value;
			rowToAdd["DateSubmitted"] = ClientDateTimeOfTran;
			rowToAdd["DateSubmittedUtc"] = localRecords.DBDateTime(thisTranUTCDateTime);

			rowToAdd["Success"] = DBNull.Value;
			rowToAdd["IsManual"] = 1;
			rowToAdd["DeclineReason"] = "";
			rowToAdd["Receipt"] = "";
			rowToAdd["CustomerIPAddress"] = CustomerIPAddress;
			rowToAdd["UserIPAddress"] = "";
			
			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				newDataToAdd.Rows.Add(rowToAdd);
			}

		}


		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CCAttemptId">Id of this Attempted Credit Card Transaction</param>
		/// <param name="UserId">Id of User who Processed this Manual Attempted Credit Card Transaction</param>
		/// <param name="CustomerName">Name of Customer Associated with this Attempted Credit Card Transaction</param>
		/// <param name="PersonName">Name of Person Associated with this Attempted Credit Card Transaction</param>
		/// <param name="UserName">Name of User Associated with this Attempted Credit Card Transaction</param>
		/// <param name="Success">Whether this Attempted Credit Card Transaction was Successful or not</param>
		/// <param name="DeclineReason">Reason Attempted Credit Card Transaction was declined (if any)</param>
		/// <param name="Receipt">Receipt for this Attempted Credit Card Transaction</param>
		/// <param name="UserIPAddress">User IP Address for this Attempted Credit Card Transaction</param>
		public void ProcessManual(int CCAttemptId,
			int UserId,
			string CustomerName,
			string PersonName,
			string UserName,
			int Success,
			string DeclineReason,
			string Receipt, 
			string UserIPAddress)
		{
			
			DateTime thisTranUTCDateTime = DateTime.UtcNow;

			string ClientDateTimeOfTran = localRecords.DBDateTime(FromUtcToClientTime(thisTranUTCDateTime));
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			SetAttribute(CCAttemptId, "UserId", UserID.ToString());
			AddAttributeToSet(CCAttemptId, "Success", Success.ToString());
			AddAttributeToSet(CCAttemptId, "DeclineReason", DeclineReason);
			AddAttributeToSet(CCAttemptId, "Receipt", Receipt);
			AddAttributeToSet(CCAttemptId, "UserIPAddress", UserIPAddress);
			AddAttributeToSet(CCAttemptId, "CustomerName", CustomerName);
			AddAttributeToSet(CCAttemptId, "PersonName", PersonName);
			AddAttributeToSet(CCAttemptId, "UserName", UserName);

			clsCCAttempt thisAttempt = new clsCCAttempt(thisDbType, localRecords.dbConnection);

			thisAttempt.GetByCCAttemptId(CCAttemptId);
			
			AddAttributeToSet(CCAttemptId, "CardNumber", ObfuscateCardNumber(thisAttempt.my_CardNumber(0), true));



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

		# region My_ Values CCAttempt

		/// <summary>
		/// <see cref="clsCCAttempt.my_CCAttemptId">Id</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_CCAttemptId">Id</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public int my_CCAttemptId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CCAttemptId"));
		}

		/// <summary>
		/// <see cref="clsOrder.my_OrderId">OrderId</see> of 
		/// <see cref="clsOrder">Order</see>
		/// Associated with this Attempted Credit Card Transaction</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_OrderId">Id</see> 
		/// of <see cref="clsOrder">Order</see> 
		/// for this Attempted Credit Card Transaction</returns>	
		public int my_OrderId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "OrderId"));
		}


		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">CustomerId</see> of 
		/// <see cref="clsCustomer">Customer</see>
		/// Associated with this Customer Payment Method Type</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> 
		/// of <see cref="clsCustomer">Customer</see> 
		/// for this Customer Payment Method Type</returns>	
		public int my_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerId"));
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_CardNumber">Credit Card Number (may be obfuscated)</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_CardNumber">Credit Card Number (may be obfuscated)</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CardNumber(int rowNum)
		{
			return UnObfuscateCardNumber(localRecords.FieldByName(rowNum, "CardNumber"));
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_NameOnCard">Name On Credit Card</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_NameOnCard">Name On Credit Card</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_NameOnCard(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "NameOnCard");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_StartDate">Start Date on Card</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_StartDate">Start Date on Card</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_StartDate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "StartDate");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_ExpiryDate">Expiry Date on Card</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_ExpiryDate">Expiry Date on Card</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_ExpiryDate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ExpiryDate");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_IssueNumber">Issue Number</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_IssueNumber">Issue Number</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_IssueNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "IssueNumber");
		}


		/// <summary>
		/// <see cref="clsCCAttempt.my_Success">Success Status</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_Success">Success Status</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public int my_Success(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Success"));
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_IsManual">Manual Status</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_IsManual">Manual Status</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public int my_IsManual(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsManual"));
		}


		/// <summary>
		/// <see cref="clsCCAttempt.my_Amount">Amount</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_Amount">Amount</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public decimal my_Amount(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Amount"));
		}


		/// <summary>
		/// <see cref="clsCCAttempt.my_DateSubmitted">Date Transaction was Submitted (In Client Time)</see> of 
		/// <see cref="clsCCAttempt">CCSubmit</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_DateSubmitted">Date Transaction was Submitted (In Client Time)</see> 
		/// of <see cref="clsCCAttempt">CCSubmit</see> 
		/// </returns>	
		public string my_DateSubmitted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSubmitted");
		}


		/// <summary>
		/// <see cref="clsCCAttempt.my_DateSubmittedUtc">Date Transaction was Submitted (In Utc Time)</see> of 
		/// <see cref="clsCCAttempt">CCSubmit</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_DateSubmittedUtc">Date Transaction was Submitted (In Utc Time)</see> 
		/// of <see cref="clsCCAttempt">CCSubmit</see> 
		/// </returns>
		public string my_DateSubmittedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSubmittedUtc");
		}
		
		
		/// <summary>
		/// <see cref="clsCCAttempt.my_DateAttempted">Date Transaction was Attempted (In Client Time)</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_DateAttempted">Date Transaction was Attempted (In Client Time)</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_DateAttempted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateAttempted");
		}


		/// <summary>
		/// <see cref="clsCCAttempt.my_DateAttemptedUtc">Date Transaction was Attempted (In Utc Time)</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_DateAttemptedUtc">Date Transaction was Attempted (In Utc Time)</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>
		public string my_DateAttemptedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateAttemptedUtc");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_DeclineReason">Reason (if any) the transaction was Declined</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_DeclineReason">Reason (if any) the transaction was Declined</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_DeclineReason(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DeclineReason");
		}
		
		/// <summary>
		/// <see cref="clsCCAttempt.my_Receipt">Receipt</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_Receipt">Receipt</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_Receipt(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Receipt");
		}



		/// <summary>
		/// <see cref="clsCCAttempt.my_CustomerIPAddress">Customer's IP Address</see> for 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_CustomerIPAddress">Customer's IP Address</see> 
		/// for <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CustomerIPAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerIPAddress");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_UserIPAddress">User's IP Address</see> for 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_UserIPAddress">User's IP Address</see> 
		/// for <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_UserIPAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "UserIPAddress");
		}

		/// <summary>
		/// <see cref="clsUser.my_UserId">UserId</see> of 
		/// <see cref="clsUser">User</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_UserId">UserId</see> 
		/// of Associated <see cref="clsUser">User</see> 
		/// for this Transaction</returns>	
		public int my_UserId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "UserId"));
		}

		/// <summary>
		/// <see cref="clsCardType.my_CardTypeId">CardTypeId</see> of 
		/// <see cref="clsCardType">CardType</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCardType.my_CardTypeId">CardTypeId</see> 
		/// of Associated <see cref="clsCardType">CardType</see> 
		/// for this Transaction</returns>	
		public int my_CardTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CardTypeId"));
		}


		/// <summary>
		/// <see cref="clsPerson.my_FullName">Full Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_FullName">Full Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_PersonName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PersonName");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_FullName">Full Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_FullName">Full Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_CustomerName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerName");
		}

		/// <summary>
		/// <see cref="clsUser.my_UserName">Full Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_UserName">Full Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_UserName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "UserName");
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
