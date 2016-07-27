using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;
using System.Collections;

namespace Keyholders
{
	/// <summary>
	/// clsTransaction deals with everything to do with data about Transactions.
	/// </summary>
	
	[GuidAttribute("061DB1EB-67A7-497f-80E9-A9318C04CF30")]
	public class clsTransaction : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsTransaction
		/// </summary>
		public clsTransaction() : base("Transaction")
		{
		}

		/// <summary>
		/// Constructor for clsTransaction; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsTransaction(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Transaction")
		{
			Connect(typeOfDb, odbcConnection);
		}

//		/// <summary>
//		/// Part of the Query that Pertains to Person Information
//		/// </summary>
//		public clsQueryPart PersonQ = new clsQueryPart();
//
//		/// <summary>
//		/// Part of the Query that Pertains to User Information
//		/// </summary>
//		public clsQueryPart UserQ = new clsQueryPart();
//		
//		/// <summary>
//		/// Part of the Query that Pertains to Order Information
//		/// </summary>
//		public clsQueryPart CCAttemptQ = new clsQueryPart();
//
//		/// <summary>
//		/// Part of the Query that Pertains to Order Information
//		/// </summary>
//		public clsQueryPart OrderQ = new clsQueryPart();
//
		/// <summary>
		/// Part of the Query that Pertains to Customer Information
		/// </summary>
		public clsQueryPart CustomerQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = TransactionQueryPart();
			CustomerQ = CustomerQueryPart();
//			PersonQ = PersonQueryPart();
//
//			OrderQ = OrderQueryPart();
//			OrderQ.FromTables.Clear();
//			OrderQ.Joins.Clear();
//
//			CCAttemptQ = CCAttemptQueryPart();
//			CCAttemptQ.FromTables.Clear();
//			CCAttemptQ.Joins.Clear();
//
//			UserQ = UserQueryPart();
//			UserQ.FromTables.Clear();
//			UserQ.Joins.Clear();
//
//			PersonQ = PersonQueryPart();
//			PersonQ.FromTables.Clear();
//			PersonQ.Joins.Clear();
//
//			CCAttemptQ.FromTables.Clear();
//			OrderQ.FromTables.Clear();

			#region old
//			MainQ.AddSelectColumn("tblTransaction.TransactionId");
//			MainQ.AddSelectColumn("tblTransaction.OrderId");
//			MainQ.AddSelectColumn("tblTransaction.CustomerId");
//			MainQ.AddSelectColumn("tblTransaction.PersonId");
//			MainQ.AddSelectColumn("tblTransaction.UserId");
//			MainQ.AddSelectColumn("tblTransaction.PaymentMethodTypeId");
//			MainQ.AddSelectColumn("tblTransaction.CCAttemptId");
//			MainQ.AddSelectColumn("tblTransaction.Amount");
//			MainQ.AddSelectColumn("tblTransaction.Pending");
//			MainQ.AddSelectColumn("tblTransaction.IsInvoicePayment");
//			MainQ.AddSelectColumn("tblTransaction.DateSubmitted");
//			MainQ.AddSelectColumn("tblTransaction.DateSubmittedUtc");
//			MainQ.AddSelectColumn("tblTransaction.DateCompleted");
//			MainQ.AddSelectColumn("tblTransaction.DateCompletedUtc");
//			MainQ.AddSelectColumn("tblTransaction.VendorMemo");
//			MainQ.AddSelectColumn("tblTransaction.CustomerIPAddress");
//			MainQ.AddSelectColumn("tblTransaction.UserIPAddress");
//
//			MainQ.AddSelectColumn("Tran.InvoiceBaseBalance");
//			MainQ.AddSelectColumn("Tran.InvoicePostBalance");
//			MainQ.AddSelectColumn("Tran.BaseBalance");
//			MainQ.AddSelectColumn("Tran.PriorBalance");
//			MainQ.AddSelectColumn("Tran.PostBalance");
//			
//			MainQ.AddFromTable(thisTable + " left outer join tblOrder on tblTransaction.OrderId = tblOrder.OrderId" + crLf
//				+ "left outer join tblCCAttempt on tblTransaction.CCAttemptId = tblCCAttempt.CCAttemptId" + crLf
//				+ "left outer join tblUser on tblTransaction.UserId = tblUser.UserId left outer join" + crLf
//				+ "(select tblTransaction.TransactionId," + crLf
//				+ "sum(case when Tran.Pending = 0 And Tran.IsInvoicePayment = 0 And Tran.CustomerId = tblTransaction.CustomerId And Tran.TransactionId < tblTransaction.TransactionId then Tran.Amount else 0 end) as BaseBalance," + crLf
//				+ "sum(case when Tran.Pending = 0 And Tran.IsInvoicePayment = 0 And Tran.CustomerId = tblTransaction.CustomerId And Tran.TransactionId < tblTransaction.TransactionId then Tran.Amount else 0 end) + tblCustomer.OpeningBalance  as PriorBalance," + crLf
//				+ "sum(case when Tran.Pending = 0 And Tran.IsInvoicePayment = 0 And Tran.CustomerId = tblTransaction.CustomerId And Tran.TransactionId <= tblTransaction.TransactionId then Tran.Amount else 0 end) + tblCustomer.OpeningBalance  as PostBalance," + crLf
//				+ "sum(case when Tran.Pending = 0 And Tran.IsInvoicePayment = 1 And Tran.CustomerId = tblTransaction.CustomerId And Tran.TransactionId < tblTransaction.TransactionId then Tran.Amount else 0 end) as InvoiceBaseBalance," + crLf
//				+ "sum(case when Tran.Pending = 0 And Tran.IsInvoicePayment = 1 And Tran.CustomerId = tblTransaction.CustomerId And Tran.TransactionId <= tblTransaction.TransactionId then Tran.Amount else 0 end) as InvoicePostBalance" + crLf
//				+ "from tblCustomer, tblTransaction, tblTransaction Tran " + crLf
//				+ "Where tblCustomer.CustomerId = tblTransaction.CustomerId" + crLf
//				+ "Group by tblTransaction.TransactionId " + crLf
//				+ ") Tran on tblTransaction.TransactionId = Tran.TransactionId");
			#endregion

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[2];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = CustomerQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By tblTransaction.TransactionId" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsTransaction
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("OrderId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CustomerId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PersonId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PaymentMethodTypeId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("UserId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CCAttemptId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("Amount", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("InvoiceBaseBalance", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("InvoicePriorBalance", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("InvoicePostBalance", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("BaseBalance", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("PriorBalance", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("PostBalance", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("Pending", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("IsInvoicePayment", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("DateSubmitted", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSubmittedUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateCompleted", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateCompletedUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("VendorMemo", System.Type.GetType("System.String"));
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

			GetGeneralSettings();

		}


		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Transactions
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
		/// Gets a Transaction by TransactionId
		/// </summary>
		/// <param name="TransactionId">Id of Transaction to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByTransactionId(int TransactionId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + "." + thisPk +" = " + TransactionId.ToString() + crLf;

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
		/// Gets Transactions by whether they are Pending or not
		/// </summary>
		/// <param name="Pending">Pending Status of Transactions to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPending(int Pending)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".Pending = " + Pending.ToString() + crLf;

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
		/// Gets Transactions by CustomerId
		/// </summary>
		/// <param name="CustomerId">Id of Customer to retrieve Transactions for</param>
		/// <param name="IsInvoicePayment">Whether to retrieve Invoice Payments or non-Invoice Payments</param>
		/// <param name="Pending">Whether to retrieve Pending Payments or non-Pending Payments</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerIdIsInvoicePaymentPending(int CustomerId, int IsInvoicePayment, int Pending)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".Pending = " + Pending.ToString() + crLf;
			condition += " And " + thisTable + ".CustomerId = " + CustomerId.ToString() + crLf;
			condition += " And " + thisTable + ".IsInvoicePayment = " + IsInvoicePayment.ToString() + crLf;

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
		/// Gets Transactions by CustomerId, and wether they suceeded or failed
		/// </summary>
		/// <param name="CustomerId">Id of Customer to retrieve Transactions for</param>
		/// <param name="Pending">State to return; Pendinge or failures</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerIdPending(int CustomerId, int Pending)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".Pending = " + Pending.ToString() + crLf;
			condition += " And " + thisTable + ".CustomerId = " + CustomerId.ToString() + crLf;

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
		/// Gets Transactions for a Customer in a given month
		/// </summary>
		/// <param name="CustomerId">Id of Customer to retrieve Transactions for</param>
		/// <param name="MonthOfInterest">Month of Interest</param>
		/// <param name="YearOfInterest">Year of Interest</param>
		/// <param name="IsInvoicePayment">Whether to return invoice payments or non-invoice payments</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerIdForMonth(int CustomerId, int MonthOfInterest, int YearOfInterest, int IsInvoicePayment)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			DateTime firstDayOfMonthOfInterest = new DateTime(YearOfInterest, MonthOfInterest, 1, 0, 0, 0);

			string startDate = localRecords.DBDateTime(firstDayOfMonthOfInterest);
			string endDate = localRecords.DBDateTime(firstDayOfMonthOfInterest.AddMonths(1));

			
			string condition = "(Select * from " + thisTable + crLf;			

			//Condition
			condition += "Where " + thisTable + ".DateCompleted > '" + startDate + "'" + crLf;
			condition += " And " + thisTable + ".DateCompleted < '" + endDate + "'" + crLf;
			condition += " And " + thisTable + ".IsInvoicePayment = " + IsInvoicePayment.ToString() + crLf;

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
		/// Gets Transactions for a Customer in a given month
		/// </summary>
		/// <param name="CustomerId">Id of Customer to retrieve Transactions for</param>
		/// <param name="YearOfInterest">Year of Interest</param>
		/// <param name="IsInvoicePayment">Whether to return invoice payments or non-invoice payments</param>
		/// <param name="Pending">State to return; Pendinge or failures</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerIdForYearIsInvoicePaymentPending(int CustomerId, int YearOfInterest, int IsInvoicePayment, int Pending)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			DateTime firstDayOfMonthOfInterest = new DateTime(YearOfInterest, 1, 1, 0, 0, 0);

			string startDate = localRecords.DBDateTime(firstDayOfMonthOfInterest);
			string endDate = localRecords.DBDateTime(firstDayOfMonthOfInterest.AddMonths(1));

			
			string condition = "(Select * from " + thisTable + crLf;			

			//Condition
			condition += "Where " + thisTable + ".DateCompleted > '" + startDate + "'" + crLf;
			condition += " And " + thisTable + ".DateCompleted < '" + endDate + "'" + crLf;
			condition += " And " + thisTable + ".IsInvoicePayment = " + IsInvoicePayment.ToString() + crLf;
			condition += " And " + thisTable + ".Pending = " + Pending.ToString() + crLf;

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
		/// Gets Transactions by OrderId
		/// </summary>
		/// <param name="OrderId">Id of Order to retrieve Transactions for</param>
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
		/// Gets Transactions by OrderId, and wether they suceeded or failed
		/// </summary>
		/// <param name="OrderId">Id of Order to retrieve Transactions for</param>
		/// <param name="Pending">State to return; Pendinges or failures</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderIdPending(int OrderId, int Pending)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".OrderId = " + OrderId.ToString() + crLf;
			condition += " And " + thisTable + ".Pending = " + Pending.ToString() + crLf;

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
		/// Gets Transactions with a null Post Balance
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByNullPostBalance()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".PostBalance is null " + crLf;

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

		# region Add/Modify/Validate

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PaymentMethodTypeId">Payment Method Type for this Transaction</param>
		/// <param name="IsInvoicePayment">Whether this Transaction is for an invoice payment or not</param>
		/// <param name="CustomerId">Customer for this Transaction</param>
		/// <param name="CustomerName">Name of Customer Associated with this Attempted Credit Card Transaction</param>
		/// <param name="PersonName">Name of Person Associated with this Attempted Credit Card Transaction</param>
		/// <param name="OrderId">Order for this Transaction (if there is one)</param>
		/// <param name="Amount">Credit Card Attempt for this Transaction (if there is one)</param>
		/// <param name="VendorMemoDebit">Vendor Memo for the Debit Transaction</param>
		/// <param name="VendorMemoCredit">Vendor Memo for the Credit Transaction</param>
		/// <param name="CustomerIPAddress">IP Address of Customer for this Transaction</param>
		public void AddByCustomer(int PaymentMethodTypeId,
			int IsInvoicePayment,
			int CustomerId,
			string CustomerName,
			string PersonName,
			int OrderId,
			decimal Amount,
			string VendorMemoDebit,
			string VendorMemoCredit,
			string CustomerIPAddress)
			{

			DateTime thisTransactionUTCDateTime = DateTime.UtcNow;

			//Sort out Order 
			if (OrderId != 0)
			{
				clsOrder Order = new clsOrder(thisDbType, localRecords.dbConnection);

				Order.SubmitOrder(OrderId, localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime)), 0, CustomerName, CustomerIPAddress);
				Order.Save();
			}


			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow creditRow = newDataToAdd.NewRow(); 
			System.Data.DataRow debitRow = newDataToAdd.NewRow(); 

			creditRow["CustomerId"] = CustomerId;
			debitRow["CustomerId"] = CustomerId;

			creditRow["CustomerName"] = CustomerName;
			debitRow["CustomerName"] = CustomerName;

			creditRow["PersonName"] = PersonName;
			debitRow["PersonName"] = PersonName;

			creditRow["OrderId"] = OrderId;
			debitRow["OrderId"] = OrderId;
			
			creditRow["CCAttemptId"] = DBNull.Value;
			debitRow["CCAttemptId"] = DBNull.Value;

			creditRow["PaymentMethodTypeId"] = PaymentMethodTypeId;
			debitRow["PaymentMethodTypeId"] = paymentMethodType_orderDebit();

			creditRow["UserId"] = DBNull.Value;
			debitRow["UserId"] = DBNull.Value;

			creditRow["IsInvoicePayment"] = IsInvoicePayment;
			debitRow["IsInvoicePayment"] = IsInvoicePayment;

			creditRow["Pending"] = 1;
			

			if (IsInvoicePayment == 1)
			{
				debitRow["Pending"] = 1;

			}
			else
			{
				debitRow["Pending"] = 0;

			}

			creditRow["Amount"] = Amount;
			debitRow["Amount"] = Amount * -1;

			creditRow["VendorMemo"] = VendorMemoCredit;
			debitRow["VendorMemo"] = VendorMemoDebit;

			creditRow["DateSubmitted"] = localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime));
			creditRow["DateSubmittedUtc"] =  localRecords.DBDateTime(thisTransactionUTCDateTime);
			debitRow["DateSubmitted"] = localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime));
			debitRow["DateSubmittedUtc"] =  localRecords.DBDateTime(thisTransactionUTCDateTime);

			debitRow["DateCompleted"] = debitRow["DateSubmitted"];
			debitRow["DateCompletedUtc"] = debitRow["DateSubmittedUtc"];
			creditRow["DateCompleted"] = DBNull.Value;
			creditRow["DateCompleted"] = DBNull.Value;
			
			creditRow["CustomerIPAddress"] = CustomerIPAddress;
			creditRow["UserIPAddress"] = "";
			debitRow["CustomerIPAddress"] = CustomerIPAddress;
			debitRow["UserIPAddress"] = "";

			//Validate the data supplied
			Validate(creditRow, true);
			
			if((paymentMethodType) PaymentMethodTypeId == paymentMethodType.chargeAccount)
				Validate(debitRow, true);

			if (NumErrors() == 0)
			{
				if( OrderId != 0)
					newDataToAdd.Rows.Add(debitRow);

				newDataToAdd.Rows.Add(creditRow);
			}

		}

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CCAttemptId">CCAttemptId for this Transaction, if there is one</param>
		/// <param name="IsInvoicePayment">Whether this Transaction is for an invoice payment or not</param>
		/// <param name="VendorMemoDebit">Vendor Memo for the Debit Transaction</param>
		/// <param name="VendorMemoCredit">Vendor Memo for the Credit Transaction</param>
		public void AddFromCCAttemptId(int CCAttemptId,
			int IsInvoicePayment,
			string VendorMemoDebit,
			string VendorMemoCredit)
		{

			DateTime thisTransactionUTCDateTime = DateTime.UtcNow;

			string ClientDateTimeOfTransaction = localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime));
			
			
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow creditRow = newDataToAdd.NewRow(); 
			System.Data.DataRow debitRow = newDataToAdd.NewRow(); 

			clsCCAttempt CCAttempt = new clsCCAttempt(thisDbType, localRecords.dbConnection);
			clsOrder Order = new clsOrder(thisDbType, localRecords.dbConnection);

			int numCCAttempts = CCAttempt.GetByCCAttemptId(CCAttemptId);

			int OrderId = CCAttempt.my_OrderId(0);
			

			//Sort out Order 
			if (OrderId != 0)
			{
				Order.SubmitOrder(OrderId, 
					localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime)), 
					CCAttempt.my_UserId(0),
					CCAttempt.my_UserName(0),
					CCAttempt.my_UserIPAddress(0)
					);
				Order.Save();

				creditRow["OrderId"] = OrderId;
				debitRow["OrderId"] = OrderId;
			}
			else
			{
				creditRow["OrderId"] = DBNull.Value;
				debitRow["OrderId"] = DBNull.Value;

			}

			creditRow["CustomerId"] = CCAttempt.my_CustomerId(0);
			debitRow["CustomerId"] = CCAttempt.my_CustomerId(0);

			creditRow["CustomerName"] = CCAttempt.my_CustomerName(0);
			debitRow["CustomerName"] = CCAttempt.my_CustomerName(0);

			creditRow["PersonName"] = CCAttempt.my_PersonName(0);
			debitRow["PersonName"] = CCAttempt.my_PersonName(0);

			creditRow["UserName"] = CCAttempt.my_UserName(0);
			debitRow["UserName"] = CCAttempt.my_UserName(0);

			creditRow["IsInvoicePayment"] = IsInvoicePayment;
			debitRow["IsInvoicePayment"] = IsInvoicePayment;

			creditRow["CCAttemptId"] = CCAttemptId;
			debitRow["CCAttemptId"] = DBNull.Value;

			debitRow["Pending"] = 0;

			object thisDateAttempted = CCAttempt.my_DateAttempted(0);
			object thisDateAttemptedUtc = CCAttempt.my_DateAttemptedUtc(0);

			if (thisDateAttempted.ToString() != "")
			{
				thisDateAttempted = localRecords.DBDateTime(Convert.ToDateTime(thisDateAttempted));
				thisDateAttemptedUtc = localRecords.DBDateTime(Convert.ToDateTime(thisDateAttemptedUtc));
			}
			else
			{
				thisDateAttempted = DBNull.Value;
				thisDateAttemptedUtc = DBNull.Value;
			}
			

			if (CCAttempt.my_IsManual(0) == 1)
			{
				creditRow["PaymentMethodTypeId"] = paymentMethodType_creditCardManual();
				creditRow["Pending"] = 1;
				creditRow["DateCompleted"] = DBNull.Value;
				creditRow["DateCompletedUtc"] = DBNull.Value;

			}
			else
			{
				creditRow["PaymentMethodTypeId"] = paymentMethodType_creditCardAuto();
				creditRow["Pending"] = 0;

				creditRow["DateCompleted"] = thisDateAttempted;
				creditRow["DateCompletedUtc"] = thisDateAttemptedUtc;


				//Sort out Order 
				if (OrderId != 0)
				{
					Order.ProcessOrder(OrderId, thisDateAttemptedUtc.ToString());
					Order.Save();
				}
			}


			debitRow["PaymentMethodTypeId"] = paymentMethodType_orderDebit();

			creditRow["UserId"] = DBNull.Value;
			debitRow["UserId"] = DBNull.Value;

			creditRow["Amount"] = CCAttempt.my_Amount(0);
			debitRow["Amount"] = CCAttempt.my_Amount(0) * -1;

			creditRow["VendorMemo"] = VendorMemoCredit;
			debitRow["VendorMemo"] = VendorMemoDebit;

			object thisDateSubmitted = CCAttempt.my_DateSubmitted(0);
			object thisDateSubmittedUtc = CCAttempt.my_DateSubmittedUtc(0);

			if (thisDateSubmitted.ToString() != "")
			{
				thisDateSubmitted = localRecords.DBDateTime(Convert.ToDateTime(thisDateSubmitted));
				thisDateSubmittedUtc = localRecords.DBDateTime(Convert.ToDateTime(thisDateSubmittedUtc));
			}
			else
			{
				thisDateSubmitted = DBNull.Value;
				thisDateSubmittedUtc = DBNull.Value;
			}

			debitRow["DateCompleted"] = thisDateSubmitted;
			debitRow["DateCompletedUtc"] = thisDateSubmittedUtc;
			
			creditRow["DateSubmitted"] = thisDateSubmitted;
			creditRow["DateSubmittedUtc"] = thisDateSubmittedUtc;
			debitRow["DateSubmitted"] = creditRow["DateSubmitted"];
			debitRow["DateSubmittedUtc"] = creditRow["DateSubmittedUtc"];

			creditRow["CustomerIPAddress"] = CCAttempt.my_CustomerIPAddress(0);
			creditRow["UserIPAddress"] = CCAttempt.my_UserIPAddress(0);
			debitRow["CustomerIPAddress"] = creditRow["CustomerIPAddress"];
			debitRow["UserIPAddress"] = creditRow["UserIPAddress"];

			//Validate the data supplied
			Validate(creditRow, true);
			Validate(debitRow, true);

			if (NumErrors() == 0)
			{
				if (OrderId != 0)
					newDataToAdd.Rows.Add(debitRow);
				newDataToAdd.Rows.Add(creditRow);
			}

		}


		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CustomerId">Customer for this Transaction</param>
		/// <param name="UserId">User for this Transaction</param>
		/// <param name="CustomerName">Name of Customer Associated with this Attempted Credit Card Transaction</param>
		/// <param name="UserName">Name of User Associated with this Attempted Credit Card Transaction</param>
		/// <param name="OrderId">Order for this Transaction (if there is one)</param>
		/// <param name="PaymentMethodTypeId">Payment Method Type for this Transaction</param>
		/// <param name="IsInvoicePayment">Whether this Transaction is for an invoice payment or not</param>
		/// <param name="CCAttemptId">Credit Card Attempt for this Transaction (if there is one)</param>
		/// <param name="Pending">Whether this Transaction is pending or not</param>
		/// <param name="Amount">Credit Card Attempt for this Transaction (if there is one)</param>
		/// <param name="DateSubmitted">Date the Transaction was submitted</param>
		/// <param name="VendorMemo">Vendor Memo for the Transaction</param>
		/// <param name="UserIPAddress">IP Address of User for this Transaction</param>
		public void AddByVendor(int CustomerId,
			int UserId,
			string CustomerName,
			string UserName,
			int OrderId,
			int PaymentMethodTypeId,
			int CCAttemptId,
			int IsInvoicePayment,
			int Pending,
			decimal Amount,
			string DateSubmitted,
			string VendorMemo,
			string UserIPAddress)
		{

			DateTime thisTransactionUTCDateTime = DateTime.UtcNow;

			if (OrderId != 0)
			{
				clsOrder Order = new clsOrder(thisDbType, localRecords.dbConnection);

				Order.ProcessOrder(OrderId, localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime)));
				Order.Save();
			}			

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["UserId"] = UserId;
			rowToAdd["CustomerName"] = CustomerName;
			rowToAdd["UserName"] = UserName;

			rowToAdd["IsInvoicePayment"] = IsInvoicePayment;

			if (OrderId == 0)
				rowToAdd["OrderId"] = DBNull.Value;
			else
				rowToAdd["OrderId"] = OrderId;
			
			rowToAdd["PaymentMethodTypeId"] = PaymentMethodTypeId;
			
			if (CCAttemptId == 0)
				rowToAdd["CCAttemptId"] = DBNull.Value;
			else
				rowToAdd["CCAttemptId"] = CCAttemptId;
			
			rowToAdd["Pending"] = Pending;
			rowToAdd["Amount"] = Amount;

			if (DateSubmitted == "")
			{
				rowToAdd["DateSubmitted"] = localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime));
				rowToAdd["DateSubmittedUtc"] =  localRecords.DBDateTime(thisTransactionUTCDateTime);
			}
			else
			{
				rowToAdd["DateSubmitted"] = localRecords.DBDateTime(Convert.ToDateTime(DateSubmitted));
				rowToAdd["DateSubmittedUtc"] =  localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(Convert.ToDateTime(DateSubmitted))));
			}

			if (Pending == 0)
			{
				rowToAdd["DateCompleted"] = localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime));
				rowToAdd["DateCompletedUtc"] = localRecords.DBDateTime(thisTransactionUTCDateTime);
			}
			else
			{
				rowToAdd["DateCompleted"] = DBNull.Value;
				rowToAdd["DateCompletedUtc"] = DBNull.Value;
			}

			rowToAdd["VendorMemo"] = VendorMemo;

			rowToAdd["CustomerIPAddress"] = "";
			rowToAdd["UserIPAddress"] = UserIPAddress;
	
			rowToAdd["PersonName"] = "";

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
		/// <param name="CustomerId">Customer for this Transaction</param>
		/// <param name="UserId">User for this Transaction</param>
		/// <param name="CustomerName">Name of Customer Associated with this Attempted Credit Card Transaction</param>
		/// <param name="UserName">Name of User Associated with this Attempted Credit Card Transaction</param>
		/// <param name="OrderId">Order for this Transaction (if there is one)</param>
		/// <param name="PaymentMethodTypeId">Payment Method Type for this Transaction</param>
		/// <param name="IsInvoicePayment">Whether this Transaction is for an invoice payment or not</param>
		/// <param name="CCAttemptId">Credit Card Attempt for this Transaction (if there is one)</param>
		/// <param name="Pending">Whether this Transaction is pending or not</param>
		/// <param name="Amount">Credit Card Attempt for this Transaction (if there is one)</param>
		/// <param name="DateSubmitted">Date the Transaction was submitted</param>
		/// <param name="VendorMemo">Vendor Memo for the Transaction</param>
		/// <param name="UserIPAddress">IP Address of User for this Transaction</param>
		public void ImportedByVendor(int CustomerId,
			int UserId,
			string CustomerName,
			string UserName,
			int OrderId,
			int PaymentMethodTypeId,
			int CCAttemptId,
			int IsInvoicePayment,
			int Pending,
			decimal Amount,
			string DateSubmitted,
			string VendorMemo,
			string UserIPAddress)
		{

			DateTime thisTransactionUTCDateTime = DateTime.UtcNow;

			if (OrderId != 0)
			{
				clsOrder Order = new clsOrder(thisDbType, localRecords.dbConnection);

				Order.ProcessOrder(OrderId, localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime)));
				Order.Save();
			}			

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["UserId"] = UserId;
			rowToAdd["CustomerName"] = CustomerName;
			rowToAdd["UserName"] = UserName;

			rowToAdd["IsInvoicePayment"] = IsInvoicePayment;

			if (OrderId == 0)
				rowToAdd["OrderId"] = DBNull.Value;
			else
				rowToAdd["OrderId"] = OrderId;
			
			rowToAdd["PaymentMethodTypeId"] = PaymentMethodTypeId;
			
			if (CCAttemptId == 0)
				rowToAdd["CCAttemptId"] = DBNull.Value;
			else
				rowToAdd["CCAttemptId"] = CCAttemptId;
			
			rowToAdd["Pending"] = Pending;
			rowToAdd["Amount"] = Amount;

			if (DateSubmitted == "")
			{
				rowToAdd["DateSubmitted"] = localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime));
				rowToAdd["DateSubmittedUtc"] =  localRecords.DBDateTime(thisTransactionUTCDateTime);
			}
			else
			{
				rowToAdd["DateSubmitted"] = localRecords.DBDateTime(Convert.ToDateTime(DateSubmitted));
				rowToAdd["DateSubmittedUtc"] =  localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(Convert.ToDateTime(DateSubmitted))));
			}

			if (Pending == 0)
			{
				rowToAdd["DateCompleted"] = localRecords.DBDateTime(Convert.ToDateTime(DateSubmitted));
				rowToAdd["DateCompletedUtc"] =  localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(Convert.ToDateTime(DateSubmitted))));
			}
			else
			{
				rowToAdd["DateCompleted"] = DBNull.Value;
				rowToAdd["DateCompletedUtc"] = DBNull.Value;
			}

			rowToAdd["VendorMemo"] = VendorMemo;

			rowToAdd["CustomerIPAddress"] = "";
			rowToAdd["UserIPAddress"] = UserIPAddress;
	
			rowToAdd["PersonName"] = "";

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
		/// <param name="TransactionId">Transaction to Finalise</param>
		/// <param name="UserId">Id of User for this Transaction</param>
		/// <param name="UserName">Name of User for this Transaction</param>
		/// <param name="DateSubmitted">Date the Transaction was submitted</param>
		/// <param name="VendorMemo">Vendor Memo for the Transaction</param>
		/// <param name="TransactionReversed">Whether this Transaction is to be reversed</param>
		/// <param name="UserIPAddress">IP Address of User for this Transaction</param>
		public void FinaliseTransaction(int TransactionId,
			int UserId,
			string UserName,
			string DateSubmitted,
			string VendorMemo,
			int TransactionReversed,
			string UserIPAddress)
		{

			DateTime thisTransactionUTCDateTime = DateTime.UtcNow;
			
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["TransactionId"] = TransactionId;
			rowToAdd["UserId"] = UserId;
			rowToAdd["UserName"] = UserName;
			rowToAdd["Pending"] = 0;
			rowToAdd["VendorMemo"] = VendorMemo;
			rowToAdd["UserIPAddress"] = UserIPAddress;


			if (DateSubmitted == "")
			{
				rowToAdd["DateSubmitted"] = localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime));
				rowToAdd["DateSubmittedUtc"] =  localRecords.DBDateTime(thisTransactionUTCDateTime);
			}
			else
			{
				rowToAdd["DateSubmitted"] = localRecords.DBDateTime(Convert.ToDateTime(DateSubmitted));
				rowToAdd["DateSubmittedUtc"] =  localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(Convert.ToDateTime(DateSubmitted))));
			}

			rowToAdd["DateCompleted"] = localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime));
			rowToAdd["DateCompletedUtc"] = localRecords.DBDateTime(thisTransactionUTCDateTime);

			
			clsTransaction thisTransaction = new clsTransaction(thisDbType, localRecords.dbConnection);
			thisTransaction.GetByTransactionId(TransactionId);

			int OrderId = thisTransaction.my_OrderId(0);

			if (TransactionReversed == 1)
			{
				rowToAdd["PaymentMethodTypeId"] =  paymentMethodType_reversal();
			}
			else
			{
				rowToAdd["PaymentMethodTypeId"] =  thisTransaction.my_PaymentMethodTypeId(0);
				if (OrderId != 0)
				{
					clsOrder Order = new clsOrder(thisDbType, localRecords.dbConnection);

					Order.ProcessOrder(OrderId, localRecords.DBDateTime(FromUtcToClientTime(thisTransactionUTCDateTime)));
					Order.Save();
				}
			}

			rowToAdd["CustomerId"] = thisTransaction.my_CustomerId(0);

			if (OrderId == 0)
				rowToAdd["OrderId"] = DBNull.Value;
			else
				rowToAdd["OrderId"] = OrderId;
			
			
			int CCAttemptId = thisTransaction.my_CCAttemptId(0);

			if (CCAttemptId == 0)
				rowToAdd["CCAttemptId"] = DBNull.Value;
			else
				rowToAdd["CCAttemptId"] = CCAttemptId;
			

			rowToAdd["IsInvoicePayment"] = thisTransaction.my_IsInvoicePayment(0);
			rowToAdd["Amount"] = thisTransaction.my_Amount(0);
			rowToAdd["CustomerIPAddress"] = thisTransaction.my_CustomerIPAddress(0);

			//Validate the data supplied
			Validate(rowToAdd, true);

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

		#region DeletePendingTransactions

		/// <summary>
		/// Deletes Unsubmitted Orders With No Items
		/// </summary>
		public void DeletePendingTransactions()
		{
			string thisSql = "Delete from " + thisTable + crLf
				+ " Where " + thisTable + ".Pending = 1" + crLf
				;

			localRecords.GetRecords(thisSql);

//			GetByPending(1);
//			DeleteCurrentGetData();
		}

		#endregion

		#region Save

		/// <summary>
		/// Saves these records
		/// </summary>
		/// <returns>Number of Records affected</returns>
		public override int Save()
		{
			ArrayList CustomerIdsAffect = new ArrayList();
			foreach(DataRow thisRow in newDataToAdd.Rows)
			{
				int thisCustomer = Convert.ToInt32(thisRow["CustomerId"]);
				bool foundThisCustomer = false;

				foreach(object Customer in CustomerIdsAffect)
					if (Convert.ToInt32(Customer) == thisCustomer)
						foundThisCustomer = true;
				if (!foundThisCustomer)
					CustomerIdsAffect.Add(thisCustomer);
			}

			foreach(DataRow thisRow in dataToBeModified.Rows)
			{
				int thisCustomer = Convert.ToInt32(thisRow["CustomerId"]);
				bool foundThisCustomer = false;

				foreach(object Customer in CustomerIdsAffect)
					if (Convert.ToInt32(Customer) == thisCustomer)
						foundThisCustomer = true;
				if (!foundThisCustomer)
					CustomerIdsAffect.Add(thisCustomer);
			}

			int retVal = base.Save();
			//Ensure that every CustomerId gets their 'CustomerInPositionAtCamps Sorted out
			foreach(object CustomerId in CustomerIdsAffect)
				UpdateCustomerTransactions(Convert.ToInt32(CustomerId));

			return retVal;
		}

		/// <summary>
		///Update Transactions For a Customer
		/// </summary>
		/// <param name="CustomerId">CustomerId</param>
		public void UpdateCustomerTransactions(int CustomerId)
		{
			clsTransaction thisTransaction = new clsTransaction(thisDbType, localRecords.dbConnection);
			thisTransaction.AddOrderByColumns("tblTransaction.DateSubmitted");
			thisTransaction.AddOrderByColumns("tblTransaction.TransactionId");
			int numTransactions = thisTransaction.GetByCustomerId(CustomerId);

			if (numTransactions > 0)
			{
				decimal InvoiceBaseBalance = 0;
				decimal InvoicePriorBalance = 0;
				decimal InvoicePostBalance = 0;
				decimal BaseBalance = 0;
				decimal PriorBalance = thisTransaction.my_Customer_OpeningBalance(0);
				decimal PostBalance = thisTransaction.my_Customer_OpeningBalance(0);
				decimal totalPurchases = 0;
				decimal totalPaid = 0;
				decimal totalUncleared = 0;
				decimal invoiceTotalUncleared = 0;
				decimal invoiceTotalPurchases = 0;
				decimal invoiceTotalPaid = 0;

				for(int counter = 0; counter < numTransactions; counter++)
				{
					decimal amountThisTransaction = thisTransaction.my_Amount(counter);
					//Update Balance Fields
					if (thisTransaction.my_Pending(0) == 0)
					{
						if(thisTransaction.my_IsInvoicePayment(counter) == 1)
						{
							InvoicePostBalance += amountThisTransaction;

							if (amountThisTransaction < 0)
								invoiceTotalPurchases += amountThisTransaction * -1;
							else
								invoiceTotalPaid += amountThisTransaction;

						}
						else
						{
							PostBalance += amountThisTransaction;
							if (amountThisTransaction < 0)
								totalPurchases += amountThisTransaction * -1;
							else
								totalPaid += amountThisTransaction;
						}
					}
					else
					{
						if (amountThisTransaction > 0)
							if(thisTransaction.my_IsInvoicePayment(counter) == 1)
								invoiceTotalUncleared += amountThisTransaction;
							else
								totalUncleared += amountThisTransaction;
					}

					#region Alter the transaction details, if we need to 

					if (thisTransaction.my_InvoiceBaseBalance(counter) != InvoiceBaseBalance
						|| thisTransaction.my_InvoicePriorBalance(counter) != InvoicePriorBalance
						|| thisTransaction.my_InvoicePostBalance(counter) != InvoicePostBalance
						|| thisTransaction.my_BaseBalance(counter) != BaseBalance
						|| thisTransaction.my_PriorBalance(counter) != PriorBalance
						|| thisTransaction.my_PostBalance(counter) != PostBalance)
					{

						thisSqlQuery = "Update tblTransaction Set "
							+ thisTable + ".InvoiceBaseBalance = " + InvoiceBaseBalance.ToString() + ", " + crLf
							+ thisTable + ".InvoicePriorBalance = " + InvoicePriorBalance.ToString() + ", " + crLf
							+ thisTable + ".InvoicePostBalance = " + InvoicePostBalance.ToString() + ", " + crLf
							+ thisTable + ".BaseBalance = " + BaseBalance.ToString() + ", " + crLf
							+ thisTable + ".PriorBalance = " + PriorBalance.ToString() + ", " + crLf
							+ thisTable + ".PostBalance = " + PostBalance.ToString() + crLf
							+ " Where TransactionId = " + thisTransaction.my_TransactionId(counter);

						localRecords.GetRecords(thisSqlQuery);
					}
	
					#endregion
					
					//Make Alterations
					if (thisTransaction.my_Pending(0) == 0)
					{

						if(thisTransaction.my_IsInvoicePayment(counter) == 1)
						{
							InvoiceBaseBalance += amountThisTransaction;
							InvoicePriorBalance += amountThisTransaction;
						}
						else
						{
							BaseBalance += amountThisTransaction;
							PriorBalance += amountThisTransaction;
						}
					}
				}

				#region Alter the Customer details, if we need to 

				if (thisTransaction.my_Customer_BaseBalance(0) != BaseBalance					
					|| thisTransaction.my_Customer_CurrentBalance(0) != PostBalance
					|| thisTransaction.my_Customer_AvailableCredit(0) != PostBalance + thisTransaction.my_Customer_CreditLimit(0)
					|| thisTransaction.my_Customer_TotalPurchases(0) != totalPurchases
					|| thisTransaction.my_Customer_TotalPaid(0) != totalPaid
					|| thisTransaction.my_Customer_TotalUncleared(0) != totalUncleared
					|| thisTransaction.my_Customer_InvoiceBaseBalance(0) != InvoiceBaseBalance
					|| thisTransaction.my_Customer_InvoiceTotalPurchases(0) != invoiceTotalPurchases
					|| thisTransaction.my_Customer_InvoiceTotalPaid(0) != invoiceTotalPaid
					|| thisTransaction.my_Customer_InvoiceTotalUncleared(0) != invoiceTotalUncleared
					)
				{

					thisSqlQuery = "Update tblCustomer Set "
						+ "tblCustomer.BaseBalance = " + BaseBalance.ToString() + ", " + crLf
						+ "tblCustomer.CurrentBalance = " + PostBalance.ToString() + ", " + crLf
						+ "tblCustomer.AvailableCredit = " + (PostBalance + thisTransaction.my_Customer_CreditLimit(0)).ToString() + ", " + crLf
						+ "tblCustomer.TotalPurchases = " + totalPurchases.ToString() + ", " + crLf
						+ "tblCustomer.TotalPaid = " + totalPaid.ToString() + ", " + crLf
						+ "tblCustomer.TotalUncleared = " + totalUncleared.ToString() + ", " + crLf
						+ "tblCustomer.InvoiceBaseBalance = " + InvoiceBaseBalance.ToString() + ", " + crLf
						+ "tblCustomer.InvoiceTotalPurchases = " + invoiceTotalPurchases.ToString() + ", " + crLf
						+ "tblCustomer.InvoiceTotalUncleared = " + invoiceTotalUncleared.ToString() + ", " + crLf
						+ "tblCustomer.TotalUncleared = " + totalUncleared.ToString() + crLf
						+ " Where CustomerId = " + CustomerId.ToString();

					localRecords.GetRecords(thisSqlQuery);
				}
	
				#endregion

			}
		}
		#endregion

		# region My_ Values Transaction


		/// <summary>
		/// TransactionId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TransactionId for this Row</returns>
		public int my_TransactionId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TransactionId"));
		}

		/// <summary>
		/// <see cref="clsOrder.my_OrderId">OrderId</see> of 
		/// <see cref="clsOrder">Order</see>
		/// Associated with this Transaction</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_OrderId">Id</see> 
		/// of <see cref="clsOrder">Order</see> 
		/// for this Transaction</returns>	
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
		/// <see cref="clsCCAttempt.my_CCAttemptId">CCAttemptId</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// Associated with this Transaction</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_CCAttemptId">Id</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// for this Transaction</returns>	
		public int my_CCAttemptId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CCAttemptId"));
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
		/// <see cref="clsPaymentMethodType.my_PaymentMethodTypeId">PaymentMethodTypeId</see> of 
		/// <see cref="clsPaymentMethodType">Customer PaymentMethodType</see>
		/// Associated with this CustomerPaymentMethodType</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_PaymentMethodTypeId">Id</see> 
		/// of <see cref="clsPaymentMethodType">Customer PaymentMethodType</see> 
		/// for this CustomerPaymentMethodType</returns>
		public int my_PaymentMethodTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PaymentMethodTypeId"));
		}


		/// <summary>
		/// <see cref="clsTransaction.my_Amount">Amount</see> of 
		/// <see cref="clsTransaction">Transaction</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsTransaction.my_Amount">Amount</see> 
		/// of <see cref="clsTransaction">Transaction</see> 
		/// </returns>	
		public decimal my_Amount(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Amount"));
		}


		/// <summary>
		/// <see cref="clsTransaction.my_Pending">Pending Status</see> of 
		/// <see cref="clsTransaction">Transaction</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsTransaction.my_Pending">Pending Status</see> 
		/// of <see cref="clsTransaction">Transaction</see> 
		/// </returns>	
		public int my_Pending(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Pending"));
		}

		/// <summary>
		/// <see cref="clsTransaction.my_IsInvoicePayment">Invoice Payment Status</see> of 
		/// <see cref="clsTransaction">Transaction</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsTransaction.my_IsInvoicePayment">Invoice Payment Status</see> 
		/// of <see cref="clsTransaction">Transaction</see> 
		/// </returns>	
		public int my_IsInvoicePayment(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsInvoicePayment"));
		}

		/// <summary>
		/// <see cref="clsTransaction.my_CustomerIPAddress">Customer's IP Address</see> for 
		/// <see cref="clsTransaction">Transaction</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsTransaction.my_CustomerIPAddress">Customer's IP Address</see> 
		/// for <see cref="clsTransaction">Transaction</see> 
		/// </returns>	
		public string my_CustomerIPAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerIPAddress");
		}

		/// <summary>
		/// <see cref="clsTransaction.my_UserIPAddress">User's IP Address</see> for 
		/// <see cref="clsTransaction">Transaction</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsTransaction.my_UserIPAddress">User's IP Address</see> 
		/// for <see cref="clsTransaction">Transaction</see> 
		/// </returns>	
		public string my_UserIPAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "UserIPAddress");
		}

		/// <summary>
		/// <see cref="clsTransaction.my_VendorMemo">Memo from the Vendor</see> for 
		/// <see cref="clsTransaction">Transaction</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsTransaction.my_VendorMemo">Memo from the Vendor</see> 
		/// for <see cref="clsTransaction">Transaction</see> 
		/// </returns>	
		public string my_VendorMemo(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "VendorMemo");
		}


		/// <summary>
		/// <see cref="clsTransaction.my_DateSubmitted">Date Transaction was Submitted (In Client Time)</see> of 
		/// <see cref="clsTransaction">Transaction</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsTransaction.my_DateSubmitted">Date Transaction was Submitted (In Client Time)</see> 
		/// of <see cref="clsTransaction">Transaction</see> 
		/// </returns>	
		public string my_DateSubmitted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSubmitted");
		}


		/// <summary>
		/// <see cref="clsTransaction.my_DateSubmittedUtc">Date Transaction was Submitted (In Utc Time)</see> of 
		/// <see cref="clsTransaction">Transaction</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsTransaction.my_DateSubmittedUtc">Date Transaction was Submitted (In Utc Time)</see> 
		/// of <see cref="clsTransaction">Transaction</see> 
		/// </returns>
		public string my_DateSubmittedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSubmittedUtc");
		}

		/// <summary>
		/// <see cref="clsTransaction.my_DateCompleted">Date Transaction was Completed (In Client Time)</see> of 
		/// <see cref="clsTransaction">Transaction</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsTransaction.my_DateCompleted">Date Transaction was Completed (In Client Time)</see> 
		/// of <see cref="clsTransaction">Transaction</see> 
		/// </returns>	
		public string my_DateCompleted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateCompleted");
		}


		/// <summary>
		/// <see cref="clsTransaction.my_DateCompletedUtc">Date Transaction was Completed (In Utc Time)</see> of 
		/// <see cref="clsTransaction">Transaction</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsTransaction.my_DateCompletedUtc">Date Transaction was Completed (In Utc Time)</see> 
		/// of <see cref="clsTransaction">Transaction</see> 
		/// </returns>
		public string my_DateCompletedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateCompletedUtc");
		}

		/// <summary>
		/// Balance (not including Opening Balance) immediately after this Transaction
		/// </summary>
		/// <param name="rowNum">Row number for Data</param>
		/// <returns>Balance (not including Opening Balance) immediately after this Transaction</returns>
		public decimal my_BaseBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "BaseBalance"));
		}

		/// <summary>
		/// Balance (including Opening Balance) immediately before this Transaction
		/// </summary>
		/// <param name="rowNum">Row number for Data</param>
		/// <returns>Balance (including Opening Balance) immediately before this Transaction</returns>
		public decimal my_PriorBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "PriorBalance"));
		}

		/// <summary>
		/// Balance (including Opening Balance) immediately after this Transaction
		/// </summary>
		/// <param name="rowNum">Row number for Data</param>
		/// <returns>Balance (including Opening Balance) immediately after this Transaction</returns>
		public decimal my_PostBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "PostBalance"));
		}

		/// <summary>
		/// Invoice Balance immediately after this Transaction
		/// </summary>
		/// <param name="rowNum">Row number for Data</param>
		/// <returns>Balance (not including Opening Balance) immediately after this Transaction</returns>
		public decimal my_InvoiceBaseBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "InvoiceBaseBalance"));
		}

		/// <summary>
		/// Balance (including Opening Balance) immediately after this Transaction
		/// </summary>
		/// <param name="rowNum">Row number for Data</param>
		/// <returns>Balance (including Opening Balance) immediately after this Transaction</returns>
		public decimal my_InvoicePostBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "InvoicePostBalance"));
		}

		/// <summary>
		/// Balance (including Opening Balance) immediately after this Transaction
		/// </summary>
		/// <param name="rowNum">Row number for Data</param>
		/// <returns>Balance (including Opening Balance) immediately after this Transaction</returns>
		public decimal my_InvoicePriorBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "InvoicePriorBalance"));
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
		
		# region My_ Values CCAttempt

		/// <summary>
		/// <see cref="clsOrder.my_OrderId">OrderId</see> of 
		/// <see cref="clsOrder">Order</see>
		/// Associated with this Attempted Credit Card Transaction</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_OrderId">Id</see> 
		/// of <see cref="clsOrder">Order</see> 
		/// for this Attempted Credit Card Transaction</returns>	
		public int my_CCAttempt_OrderId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CCAttempt_OrderId"));
		}


		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">CustomerId</see> of 
		/// <see cref="clsCustomer">Customer</see>
		/// Associated with this Customer Payment Method Type</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> 
		/// of <see cref="clsCustomer">Customer</see> 
		/// for this Customer Payment Method Type</returns>	
		public int my_CCAttempt_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CCAttempt_CustomerId"));
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_CardNumber">Credit Card Number (may be obfuscated)</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_CardNumber">Credit Card Number (may be obfuscated)</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_CardNumber(int rowNum)
		{
			return UnObfuscateCardNumber(localRecords.FieldByName(rowNum, "CCAttempt_CardNumber"));
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_NameOnCard">Name On Credit Card</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_NameOnCard">Name On Credit Card</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_NameOnCard(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_NameOnCard");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_StartDate">Start Date on Card</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_StartDate">Start Date on Card</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_StartDate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_StartDate");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_ExpiryDate">Expiry Date on Card</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_ExpiryDate">Expiry Date on Card</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_ExpiryDate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_ExpiryDate");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_IssueNumber">Issue Number</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_IssueNumber">Issue Number</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_IssueNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_IssueNumber");
		}


		/// <summary>
		/// <see cref="clsCCAttempt.my_Success">Success Status</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_Success">Success Status</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public int my_CCAttempt_Success(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CCAttempt_Success"));
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_IsManual">Manual Status</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_IsManual">Manual Status</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public int my_CCAttempt_IsManual(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CCAttempt_IsManual"));
		}


		/// <summary>
		/// <see cref="clsCCAttempt.my_Amount">Amount</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_Amount">Amount</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public decimal my_CCAttempt_Amount(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "CCAttempt_Amount"));
		}


		/// <summary>
		/// <see cref="clsCCAttempt.my_DateSubmitted">Date Transaction was Submitted (In Client Time)</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_DateSubmitted">Date Transaction was Submitted (In Client Time)</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_DateSubmitted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_DateSubmitted");
		}


		/// <summary>
		/// <see cref="clsCCAttempt.my_DateSubmittedUtc">Date Transaction was Submitted (In Utc Time)</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_DateSubmittedUtc">Date Transaction was Submitted (In Utc Time)</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>
		public string my_CCAttempt_DateSubmittedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_DateSubmittedUtc");
		}
		
		
		/// <summary>
		/// <see cref="clsCCAttempt.my_DateAttempted">Date Transaction was Attempted (In Client Time)</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_DateAttempted">Date Transaction was Attempted (In Client Time)</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_DateAttempted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_DateAttempted");
		}


		/// <summary>
		/// <see cref="clsCCAttempt.my_DateAttemptedUtc">Date Transaction was Attempted (In Utc Time)</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_DateAttemptedUtc">Date Transaction was Attempted (In Utc Time)</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>
		public string my_CCAttempt_DateAttemptedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_DateAttemptedUtc");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_DeclineReason">Reason (if any) the transaction was Declined</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_DeclineReason">Reason (if any) the transaction was Declined</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_DeclineReason(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_DeclineReason");
		}
		
		/// <summary>
		/// <see cref="clsCCAttempt.my_Receipt">Receipt</see> of 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_Receipt">Receipt</see> 
		/// of <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_Receipt(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_Receipt");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_CustomerIPAddress">Customer's IP Address</see> for 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_CustomerIPAddress">Customer's IP Address</see> 
		/// for <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_CustomerIPAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_CustomerIPAddress");
		}

		/// <summary>
		/// <see cref="clsCCAttempt.my_UserIPAddress">User's IP Address</see> for 
		/// <see cref="clsCCAttempt">CCAttempt</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCCAttempt.my_UserIPAddress">User's IP Address</see> 
		/// for <see cref="clsCCAttempt">CCAttempt</see> 
		/// </returns>	
		public string my_CCAttempt_UserIPAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CCAttempt_UserIPAddress");
		}

		/// <summary>
		/// <see cref="clsUser.my_UserId">UserId</see> of 
		/// <see cref="clsUser">User</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_UserId">UserId</see> 
		/// of Associated <see cref="clsUser">User</see> 
		/// for this Transaction</returns>	
		public int my_CCAttempt_UserId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CCAttempt_UserId"));
		}

		/// <summary>
		/// <see cref="clsCardType.my_CardTypeId">CardTypeId</see> of 
		/// <see cref="clsCardType">CardType</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCardType.my_CardTypeId">CardTypeId</see> 
		/// of Associated <see cref="clsCardType">CardType</see> 
		/// for this Transaction</returns>	
		public int my_CCAttempt_CardTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CCAttempt_CardTypeId"));
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
		
		# region My_ Values Order


		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">CustomerId</see> of 
		/// <see cref="clsCustomer">Customer</see>
		/// Associated with this Order</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> 
		/// of <see cref="clsCustomer">Customer</see> 
		/// for this Order</returns>	
		public int my_Order_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_CustomerId"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">PersonId</see> of 
		/// <see cref="clsPerson">Person</see>
		/// Associated with this Order</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> 
		/// of <see cref="clsPerson">Person</see> 
		/// for this Order</returns>	
		public int my_Order_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_PersonId"));
		}
		
		/// <summary>
		/// <see cref="clsPaymentMethodType.my_PaymentMethodTypeId">PaymentMethodTypeId</see> of 
		/// <see cref="clsPaymentMethodType">PaymentMethodType</see>
		/// Associated with this Order</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_PaymentMethodTypeId">Id</see> 
		/// of <see cref="clsPaymentMethodType">PaymentMethodType</see> 
		/// for this Order</returns>	
		public int my_Order_PaymentMethodTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_PaymentMethodTypeId"));
		}

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupId">CustomerGroupId</see> of 
		/// <see cref="clsCustomerGroup">CustomerGroup</see>
		/// Associated with this Order</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupId">Id</see> 
		/// of <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// for this Order</returns>	
		public int my_Order_CustomerGroupId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_CustomerGroupId"));
		}
		
		/// <summary>
		/// <see cref="clsOrder.my_OrderNum">Customer's Order Number</see> for this 
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_OrderNum">Customer's Order Number</see> for this
		/// <see cref="clsOrder">Order</see> 
		/// </returns>	
		public string my_Order_OrderNum(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_OrderNum");
		}
		
		/// <summary>
		/// <see cref="clsOrder.my_CustomerType">Type</see> of Customer for this
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_CustomerType">Type</see> of Customer for this
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_CustomerType(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_CustomerType");
		}

		/// <summary>
		/// <see cref="clsOrder.my_FullName">Full Name</see> of Customer for this
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_FullName">Full Name</see> of Customer for this
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_FullName");
		}


		/// <summary>
		/// <see cref="clsOrder.my_Title">Person's Title</see> for this 
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_Title">Person's Title</see> for this
		/// <see cref="clsOrder">Order</see> 
		/// </returns>	
		public string my_Order_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_Title");
		}

		/// <summary>
		/// <see cref="clsOrder.my_FirstName">Person's First Name</see> for this 
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_FirstName">Person's First Name</see> for this
		/// <see cref="clsOrder">Order</see> 
		/// </returns>	
		public string my_Order_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_FirstName");
		}

		/// <summary>
		/// <see cref="clsOrder.my_LastName">Person's Last Name</see> for this 
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_LastName">Person's Last Name</see> for this
		/// <see cref="clsOrder">Order</see> 
		/// </returns>	
		public string my_Order_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_LastName");
		}


		/// <summary>
		/// <see cref="clsOrder.my_QuickPostalAddress">Quick Postal Address</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_QuickPostalAddress">Quick Postal Address</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_QuickPostalAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickPostalAddress");
		}

		/// <summary>
		/// <see cref="clsOrder.my_QuickDaytimePhone">Quick Daytime Phone</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_QuickDaytimePhone">Quick Daytime Phone</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_QuickDaytimePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickDaytimePhone");
		}

		/// <summary>
		/// <see cref="clsOrder.my_QuickDaytimeFax">Quick Daytime Fax</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_QuickDaytimeFax">Quick Daytime Fax</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_QuickDaytimeFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickDaytimeFax");
		}

		/// <summary>
		/// <see cref="clsOrder.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_QuickAfterHoursPhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickAfterHoursPhone");
		}

		/// <summary>
		/// <see cref="clsOrder.my_QuickAfterHoursFax">Quick After Hours Fax</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_QuickAfterHoursFax">Quick After Hours Fax</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_QuickAfterHoursFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickAfterHoursFax");
		}

		/// <summary>
		/// <see cref="clsOrder.my_QuickMobilePhone">Quick Mobile Phone</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_QuickMobilePhone">Quick Mobile Phone</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_QuickMobilePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickMobilePhone");
		}

		/// <summary>
		/// <see cref="clsCountry.my_CountryId">CountryId</see> of 
		/// <see cref="clsCountry">Country</see></summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsCountry.my_CountryId">CountryId</see> 
		/// of Associated <see cref="clsCountry">Country</see> 
		/// for this Order</returns>
		public int my_Order_CountryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_CountryId"));
		}

		/// <summary>
		/// <see cref="clsOrder.my_Email">Email Address</see> for
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_Email">Email Address</see> for 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_Email");
		}


		/// <summary>
		/// <see cref="clsOrder.my_OrderSubmitted">Submission Status</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_OrderSubmitted">Submission Status</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public int my_Order_OrderSubmitted(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_OrderSubmitted"));
		}


		/// <summary>
		/// <see cref="clsOrder.my_OrderPaid">Paid Status</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_OrderPaid">Paid Status</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public int my_Order_OrderPaid(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_OrderPaid"));
		}

		/// <summary>
		/// <see cref="clsOrder.my_OrderCreatedMechanism">Mechanism</see> by which this 
		/// <see cref="clsOrder">Order</see> was Created
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>
		/// <see cref="clsOrder.my_OrderCreatedMechanism">Mechanism</see> by which this 
		/// <see cref="clsOrder">Order</see> was Created
		/// </returns>
		public int my_Order_OrderCreatedMechanism(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_OrderCreatedMechanism"));
		}

		/// <summary>
		/// <see cref="clsOrderStatus.my_OrderStatusId">OrderStatusId</see> of 
		/// <see cref="clsOrderStatus">OrderStatus</see></summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsOrderStatus.my_OrderStatusId">OrderStatusId</see> 
		/// of Associated <see cref="clsOrderStatus">OrderStatus</see> 
		/// for this Order</returns>
		public int my_Order_OrderStatusId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_OrderStatusId"));
		}

		/// <summary>
		/// <see cref="clsOrder.my_SupplierComment">Supplier's Comments</see> about
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_SupplierComment">Supplier's Comments</see> about 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_SupplierComment(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_SupplierComment");
		}


		/// <summary>
		/// <see cref="clsOrder.my_DateCreated">Date of Creation (Client Time)</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_DateCreated">Date of Creation (Client Time)</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_DateCreated(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateCreated");
		}

		/// <summary>
		/// <see cref="clsOrder.my_DateCreatedUtc">Date of Creation (UTC Time)</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_DateCreatedUtc">Date of Creation (UTC Time)</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_DateCreatedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateCreatedUtc");
		}

		/// <summary>
		/// <see cref="clsOrder.my_DateSubmitted">Date of Submission (Client Time)</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_DateSubmitted">Date of Submission (Client Time)</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_DateSubmitted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateSubmitted");
		}

		/// <summary>
		/// <see cref="clsOrder.my_DateSubmittedUtc">Date of Submission (UTC Time)</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_DateSubmittedUtc">Date of Submission (UTC Time)</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_DateSubmittedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateSubmittedUtc");
		}


		/// <summary>
		/// <see cref="clsOrder.my_DateProcessed">Date of Processing (Client Time)</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_DateProcessed">Date of Processing (Client Time)</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_DateProcessed(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateProcessed");
		}

		/// <summary>
		/// <see cref="clsOrder.my_DateProcessedUtc">Date of Processing (UTC Time)</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_DateProcessedUtc">Date of Processing (UTC Time)</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_DateProcessedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateProcessedUtc");
		}

		/// <summary>
		/// <see cref="clsOrder.my_DateShipped">Date of Shipping (Client Time)</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_DateShipped">Date of Shipping (Client Time)</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_DateShipped(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateShipped");
		}

		/// <summary>
		/// <see cref="clsOrder.my_DateShippedUtc">Date of Shipping (UTC Time)</see> of
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_DateShippedUtc">Date of Shipping (UTC Time)</see> of 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public string my_Order_DateShippedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateShippedUtc");
		}


		/// <summary>
		/// <see cref="clsOrder.my_TaxAppliedToOrder">Whether Tax was applied</see> to this
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_TaxAppliedToOrder">Whether Tax was applied</see> to this 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public int my_Order_TaxAppliedToOrder(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_TaxAppliedToOrder"));
		}

		/// <summary>
		/// <see cref="clsOrder.my_TaxRateAtTimeOfOrder">Tax Rate at time</see> of this
		/// <see cref="clsOrder">Order</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_TaxRateAtTimeOfOrder">Tax Rate at time</see> of this 
		/// <see cref="clsOrder">Order</see> 
		/// </returns>
		public decimal my_Order_TaxRateAtTimeOfOrder(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_TaxRateAtTimeOfOrder"));
		}


		/// <summary>
		/// Tax Cost of Order. Note this is negative if the system setting is to show
		/// Prices including tax, but the customer is from a country for which 
		/// the local Tax does not apply.
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Tax Cost of this Order</returns>
		public decimal my_Order_TaxCost(int rowNum)
		{
			decimal Tax = Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_TaxCost"));

			if (priceShownIncludesLocalTaxRate && Tax == Convert.ToDecimal(0))
				return (Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_Total")) 
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_FreightCost")))
					* Convert.ToDecimal(Convert.ToDecimal(1) - (Convert.ToDecimal(localTaxRate)));
			else
				return Tax;
		}
				
		/// <summary>
		/// Freight Cost of Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>FreightCost of this Order</returns>
		public decimal my_Order_FreightCost(int rowNum)
		{
			decimal baseCost = Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_FreightCost"));
			
			if (priceShownIncludesLocalTaxRate)
				return baseCost * localTaxRate;
			else
				return baseCost;
		}		

		/// <summary>
		/// Freight Cost of Order (Excluding Tax)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>FreightCost of this Order (Excluding Tax)</returns>
		public decimal my_Order_FreightCostExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_FreightCost"));
		}


		/// <summary>
		/// Total Cost of Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Cost of this Order</returns>
		public decimal my_Order_Total(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_Total"));
		}

		
		#endregion

	}
}
