using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsCustomer deals with everything to do with data about Customers.
	/// </summary>

	[GuidAttribute("B0A6A0B6-ED35-4133-8D0A-EBD9948B7A77")]
	public class clsCustomer : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCustomer
		/// </summary>
		public clsCustomer() : base("Customer")
		{
		}

		/// <summary>
		/// Constructor for clsCustomer; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsCustomer(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Customer")
		{
			Connect(typeOfDb, odbcConnection);
		}


		/// <summary>
		/// Part of the Query that Pertains to Customer Group Information
		/// </summary>
		public clsQueryPart CustomerGroupQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Country Information
		/// </summary>
		public clsQueryPart CountryQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Order Information
		/// </summary>
		public clsQueryPart OrderSummaryQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Service Provider Premise Roles Summary Information
		/// </summary>
		public Resources.clsQueryPart SPPremiseRolesSummaryQ = new Resources.clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = CustomerQueryPart();
			CustomerGroupQ = CustomerGroupQueryPart();
			CountryQ = CountryQueryPart();
			ChangeDataQ = ChangeDataQueryPart();

			#region Old
//			OrderSummaryQ.AddSelectColumn("tblOrder.DateFirstOrder");
//			OrderSummaryQ.AddSelectColumn("tblOrder.DateFirstOrderUtc");
//			OrderSummaryQ.AddSelectColumn("tblOrder.DateLastOrder");
//			OrderSummaryQ.AddSelectColumn("tblOrder.DateLastOrderUtc");
//			OrderSummaryQ.AddSelectColumn("tblOrder.NumOrders");
//			OrderSummaryQ.AddSelectColumn("tblOrder.TotalItemCost");
//			OrderSummaryQ.AddSelectColumn("tblOrder.TotalTaxCost");
//			OrderSummaryQ.AddSelectColumn("tblOrder.TotalFreightCost");
//
//			OrderSummaryQ.AddFromTable("(Select tblCustomer.CustomerId," + crLf
//				+ "min(tblOrder.DateCreated) as DateFirstOrder,"  + crLf
//				+ "min(tblOrder.DateCreatedUtc) as DateFirstOrderUtc,"  + crLf
//				+ "max(tblOrder.DateCreated) as DateLastOrder,"  + crLf
//				+ "max(tblOrder.DateCreatedUtc) as DateLastOrderUtc,"  + crLf
//				+ "sum(tblOrder.Total) as TotalItemCost,"  + crLf
//				+ "sum(tblOrder.TaxCost) as TotalTaxCost,"  + crLf
//				+ "sum(tblOrder.FreightCost) as TotalFreightCost,"  + crLf
//				+ "count(tblOrder.OrderId) as NumOrders"  + crLf
//				+ "from tblCustomer left outer join "  + crLf
//				+ "(select tblOrder.CustomerId, tblOrder.DateCreated, tblOrder.DateCreatedUtc, "
//				+ "		tblOrder.Total, tblOrder.TaxCost, tblOrder.FreightCost, tblOrder.OrderId "
//				+ "	from tblOrder where tblOrder.OrderPaid = 1) tblOrder"  + crLf
//				+ "on tblCustomer.CustomerId = tblOrder.CustomerId"  + crLf
//				+ "group by tblcustomer.customerid) tblOrder");
//
//			OrderSummaryQ.AddJoin("tblCustomer.CustomerId = tblOrder.CustomerId");
			#endregion

			
            clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[4];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = CustomerGroupQ;
			baseQueries[2] = CountryQ;
			baseQueries[3] = ChangeDataQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);

			orderBySqlQuery = "Order By CompanyName, LastName, FirstName" + crLf;
		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsCustomer
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("CustomerGroupId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CustomerType", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("Title", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("FirstName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("LastName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CompanyName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AccountNumber", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CountryId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("OpeningBalance", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("CreditLimit", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("KdlComments", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerComments", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("StartDateForStatement", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("FullName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateLastLoggedIn", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateLastLoggedInUtc", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("DateFirstOrder", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateFirstOrderUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateLastOrder", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateLastOrderUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("NumOrders", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("TotalItemCost", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("TotalTaxCost", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("TotalFreightCost", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("BaseBalance", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("CurrentBalance", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("AvailableCredit", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("TotalPurchases", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("TotalPaid", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("TotalUncleared", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("InvoiceCurrentBalance", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("InvoiceTotalPurchases", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("InvoiceTotalPaid", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("InvoiceTotalUncleared", System.Type.GetType("System.Decimal"));

			newDataToAdd.Columns.Add("DateStart", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("StartDateForInvoices", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("IsDirectDebitCustomer", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ChangeDataId", System.Type.GetType("System.Int64"));
			newDataToAdd.Columns.Add("Archive", System.Type.GetType("System.Int64"));

			dataToBeModified = new DataTable(thisTable);
			dataToBeModified.Columns.Add(thisPk, System.Type.GetType("System.Int64"));

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


		/// <summary>
		/// Local Representation of the class <see cref="clsCustomer">clsCustomer</see>
		/// </summary>
		public clsCustomer thisCustomer;

		#endregion

		#region ResetPremiseCorrepondence


		/// <summary>
		/// Resets Correspondence fields (StickerRequired, InvoiceRequired, DetailsRequired) for a Premise
		/// </summary>
		/// <param name="CustomerId">Id of Custoemr to Reset Correspondence Fields for</param>
		public void ResetPremiseCorrepondence(int CustomerId)
		{
			clsCustomer thisCustomer = new clsCustomer(thisDbType, localRecords.dbConnection);
			clsPremise thisOutsidePremise = new clsPremise(thisDbType, localRecords.dbConnection);

			int numPresmises = thisOutsidePremise.GetByCustomerId(CustomerId);
			for(int counter = 0; counter < numPresmises; counter++)
			{
				clsPremise thisPremise = new clsPremise(thisDbType, localRecords.dbConnection);
				int PremiseId = thisOutsidePremise.my_PremiseId(counter);

				thisPremise.SetAttribute(PremiseId, "StickerRequired", "0");
				thisPremise.AddAttributeToSet(PremiseId, "InvoiceRequired", "0");
				thisPremise.AddAttributeToSet(PremiseId, "CopyOfInvoiceRequired", "0");
				thisPremise.AddAttributeToSet(PremiseId, "StatementRequired", "0");
				thisPremise.AddAttributeToSet(PremiseId, "DetailsUpdateRequired", "0");

				thisPremise.Save();
			}
		}
		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Customers
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
//			queries[0] = MainQ;
//			queries[1] = CountryQ;
//			queries[2] = OrderSummaryQ;
//			queries[3] = CustomerGroupQ;
//			queries[4] = TransactionSummaryQ;

			string condition = "";

			condition += ArchiveConditionIfNecessary(false);

			if (condition == "")
				thisSqlQuery = QB.BuildSqlStatement(
					queries, OrderByColumns);
			else
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
		/// Gets a Customer by CustomerId
		/// </summary>
		/// <param name="CustomerId">Id of Customer to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ CustomerId.ToString() + crLf;

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
		/// Part of the Query that Pertains to Transaction Summary Information
		/// </summary>
		public clsQueryPart TransactionSummaryQ(int StartMonth, int StartYear, int NumMonths)
		{
			clsQueryPart thisQueryPart = new clsQueryPart();

			thisQueryPart.AddSelectColumn("tblTransaction.BaseBalance");
			thisQueryPart.AddSelectColumn("tblTransaction.CurrentBalance");
			thisQueryPart.AddSelectColumn("tblTransaction.AvailableCredit");
			thisQueryPart.AddSelectColumn("tblTransaction.TotalPurchases");
			thisQueryPart.AddSelectColumn("tblTransaction.TotalPaid");
			thisQueryPart.AddSelectColumn("tblTransaction.TotalUncleared");

			string PreviousMonthsDataToAdd = "";

			DateTime startDate = new DateTime(StartYear, StartMonth, 1);

			for(int counter = 0; counter < NumMonths; counter++)
			{				
				DateTime thisDate = startDate.AddMonths(counter); 
				string dateCondition =  " And Month(tblTransaction.DateCompleted) = " + thisDate.Month.ToString().Trim() 
					+ " And Year(tblTransaction.DateCompleted) = " + thisDate.Year.ToString().Trim();
				string PurchasesMonthFieldName = "PurchasesMonth" + counter.ToString().Trim();
				string PaidMonthFieldName = "PaidMonth" + counter.ToString().Trim();
				string InvoicePurchasesMonthFieldName = "InvoicePurchasesMonth" + counter.ToString().Trim();
				string InvoicePaidMonthFieldName = "InvoicePaidMonth" + counter.ToString().Trim();
				
				PreviousMonthsDataToAdd += "sum(case when tblTransaction.Pending = 0 "
					+ "And tblTransaction.IsInvoicePayment = 0 "
					+ "And tblTransaction.Amount < 0 " 
					+ dateCondition 
					+ " then tblTransaction.Amount * -1 else 0 end) as " 
					+ PurchasesMonthFieldName + ","  + crLf;
				PreviousMonthsDataToAdd += "sum(case when tblTransaction.Pending = 0 "
					+ "And tblTransaction.IsInvoicePayment = 0 "
					+ "And tblTransaction.Amount > 0 " 
					+ dateCondition 
					+ " then tblTransaction.Amount else 0 end) as " 
					+ PaidMonthFieldName + ","  + crLf;
				PreviousMonthsDataToAdd += "sum(case when tblTransaction.Pending = 0 "
					+ "And tblTransaction.IsInvoicePayment = 1 "
					+ "And tblTransaction.Amount < 0 " 
					+ dateCondition 
					+ " then tblTransaction.Amount * -1 else 0 end) as " 
					+ InvoicePurchasesMonthFieldName + ","  + crLf;
				PreviousMonthsDataToAdd += "sum(case when tblTransaction.Pending = 0 "
					+ "And tblTransaction.IsInvoicePayment = 1 "
					+ "And tblTransaction.Amount > 0 " 
					+ dateCondition 
					+ " then tblTransaction.Amount else 0 end) as " 
					+ InvoicePaidMonthFieldName + ","  + crLf;

				thisQueryPart.AddSelectColumn(PurchasesMonthFieldName);
				thisQueryPart.AddSelectColumn(PaidMonthFieldName);
				thisQueryPart.AddSelectColumn(InvoicePurchasesMonthFieldName);
				thisQueryPart.AddSelectColumn(InvoicePaidMonthFieldName);
			}

			thisQueryPart.AddFromTable("(Select tblCustomer.CustomerId," + crLf
				+ PreviousMonthsDataToAdd
				+ "sum(case when tblTransaction.IsInvoicePayment = 0 And tblTransaction.Pending = 0 then tblTransaction.Amount else 0 end) as BaseBalance,"  + crLf
				+ "sum(case when tblTransaction.IsInvoicePayment = 0 And tblTransaction.Pending = 0 then tblTransaction.Amount else 0 end) + tblCustomer.OpeningBalance as CurrentBalance,"  + crLf
				+ "sum(case when tblTransaction.IsInvoicePayment = 0 And tblTransaction.Pending = 0 then tblTransaction.Amount else 0 end) + tblCustomer.OpeningBalance + tblCustomer.CreditLimit as AvailableCredit,"  + crLf
				+ "sum(case when tblTransaction.IsInvoicePayment = 0 And tblTransaction.Pending = 0 And tblTransaction.Amount < 0 then tblTransaction.Amount * -1 else 0 end) as TotalPurchases,"  + crLf
				+ "sum(case when tblTransaction.IsInvoicePayment = 0 And tblTransaction.Pending = 0 And tblTransaction.Amount > 0 then tblTransaction.Amount else 0 end) as TotalPaid,"  + crLf
				+ "sum(case when tblTransaction.IsInvoicePayment = 0 And tblTransaction.Pending = 1 And tblTransaction.Amount > 0 then tblTransaction.Amount else 0 end) as TotalUncleared,"  + crLf
				+ "sum(case when tblTransaction.IsInvoicePayment = 1 And tblTransaction.Pending = 0 then tblTransaction.Amount else 0 end) as InvoiceBaseBalance,"  + crLf
				+ "sum(case when tblTransaction.IsInvoicePayment = 1 And tblTransaction.Pending = 0 then tblTransaction.Amount else 0 end) + tblCustomer.OpeningBalance as InvoiceCurrentBalance,"  + crLf
				+ "sum(case when tblTransaction.IsInvoicePayment = 1 And tblTransaction.Pending = 0 And tblTransaction.Amount < 0 then tblTransaction.Amount * -1 else 0 end) as InvoiceTotalPurchases,"  + crLf
				+ "sum(case when tblTransaction.IsInvoicePayment = 1 And tblTransaction.Pending = 0 And tblTransaction.Amount > 0 then tblTransaction.Amount else 0 end) as InvoiceTotalPaid,"  + crLf
				+ "sum(case when tblTransaction.IsInvoicePayment = 1 And tblTransaction.Pending = 1 And tblTransaction.Amount > 0 then tblTransaction.Amount else 0 end) as InvoiceTotalUncleared"  + crLf
				+ "from tblCustomer left outer join "  + crLf
				+ "(select tblTransaction.CustomerId, tblTransaction.Pending, tblTransaction.IsInvoicePayment, tblTransaction.DateCompleted, tblTransaction.Amount from tblTransaction) tblTransaction"  + crLf
				+ "on tblCustomer.CustomerId = tblTransaction.CustomerId"  + crLf
				+ "group by tblcustomer.customerid) tblTransaction");

			return thisQueryPart;
		}
			

		/// <summary>
		/// Gets a Transaction Summary for a CustomerId
		/// </summary>
		/// <param name="CustomerId">Id of Customer to retrieve</param>
		/// <param name="StartMonth">First Month of interest e.g. 3 = March</param>
		/// <param name="StartYear">First Year of interest e.g. 2009</param>
		/// <param name="NumMonths">Number of Months</param>
		/// <returns>Number of resulting records</returns>
		public int GetTransactionSummaryByCustomerId(int CustomerId, int StartMonth, int StartYear, int NumMonths)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[0];

			queries[0] = TransactionSummaryQ(StartMonth, StartYear, NumMonths);

			string condition = "(Select * from tblTransaction" + crLf 
				+ " Where tblTransaction.CustomerId = "
				+ CustomerId.ToString() + crLf;

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
		/// Gets Customers by any kind of name
		/// </summary>
		/// <param name="Name">Filter for Name or Part of Name of Customer</param>
		/// <returns>Number of resulting records</returns>
		public int GetByName(string Name)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+  " Where concat_ws(' ',FirstName,LastName,CompanyName) " 
				+ MatchCondition(Name, matchCriteria.contains) + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
				condition,
				thisTable
				);


			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Customers by any kind of name
		/// </summary>
		/// <param name="Name">Filter for Name or Part of Name of Customer</param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctName(string Name)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;	
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct DisplayName");

			string condition = "(Select  "
				+ "	Rtrim(case" + crLf
				+ "When tblCustomer.CustomerType = 1 then" + crLf
				+ "	case" + crLf
				+ "	When tblCustomer.CompanyName != '' then" + crLf
				+ "		Concat(tblCustomer.CompanyName, ' '," + crLf
				+ "			case" + crLf
				+ "			When Concat(tblCustomer.FirstName, tblCustomer.LastName) != '' then" + crLf
				+ "				Concat('('," + crLf
				+ "				Ltrim(Rtrim(Concat(tblCustomer.FirstName, ' ', tblCustomer.LastName)))," + crLf
				+ "				')')" + crLf
				+ "			else ''" + crLf
				+ "			end" + crLf
				+ "			)" + crLf
				+ "	else Ltrim(Rtrim(Concat(tblCustomer.FirstName, ' ', tblCustomer.LastName)))" + crLf
				+ "	end" + crLf
				+ "else" + crLf
				+ "	case" + crLf
				+ "	When Concat(tblCustomer.FirstName, tblCustomer.LastName) != '' then" + crLf
				+ "		Concat(Ltrim(Rtrim(Concat(tblCustomer.FirstName, ' ', tblCustomer.LastName)))," + crLf
				+ "			case" + crLf
				+ "			When tblCustomer.CompanyName != '' then" + crLf
				+ "				Concat(' (', tblCustomer.CompanyName, ')')" + crLf
				+ "			else ''" + crLf
				+ "			end" + crLf
				+ "			)" + crLf
				+ "	else tblCustomer.CompanyName" + crLf
				+ "	end" + crLf
				+ "end) as DisplayName" + crLf
				+ "from " + thisTable 
				+  " Where concat_ws(' ',FirstName,LastName,CompanyName) " 
				+ MatchCondition(Name, matchCriteria.contains) + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
				condition,
				thisTable
				);

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Customers by Account Number
		/// </summary>
		/// <param name="AccountNumber">Filter for Account Number of Customer</param>
		/// <returns>Number of resulting records</returns>
		public int GetByAccountNumber(string AccountNumber)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
	

			string condition = "(Select * from " + thisTable 
				+  " Where AccountNumber " 
				+ MatchCondition(AccountNumber, matchCriteria.exactMatch) + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
				condition,
				thisTable
				);


			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Get a Customer by Email Address
		/// </summary>
		/// <param name="Email">Email for which to attempt to retrieve Customer</param>
		/// <returns>Number of resulting records</returns>
		public int GetByEmail(string Email)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+  " Where " + thisTable + ".Email "
				+ MatchCondition(Email, matchCriteria.exactMatch) + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
				condition,
				thisTable
				);


			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Customers by Customer Group
		/// </summary>
		/// <param name="CustomerGroup">Filter for CustomerGroup</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerGroupId(int CustomerGroup)
		{
		
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable;

			if (CustomerGroup != 0)
				condition += "	Where CustomerGroupId = " 
					+ CustomerGroup.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
				condition,
				thisTable
				);
			
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Customers by any kind of name and Customer Group
		/// </summary>
		/// <param name="Name">Filter for Name or Part of Name of Customer</param>
		/// <param name="CustomerGroup">Filter for CustomerGroup</param>
		/// <returns>Number of resulting records</returns>
		public int GetByNameCustomerGroup(string Name, int CustomerGroup)
		{
		
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
	
			string condition = "(Select * from " + thisTable 
				+  "	Where concat_ws(' ',FirstName,LastName,CompanyName)  " 
				+ MatchCondition(Name, matchCriteria.contains) + crLf;

			if (CustomerGroup != 0)
				condition += "	And CustomerGroupId = " 
					+ CustomerGroup.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
				condition,
				thisTable
				);
			
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetByOverdue

		/// <summary>
		/// Gets Customers with overdue accounts
		/// </summary>
		/// <param name="MinAmountOverdue">MinAmountOverdue</param>
		/// <returns>Number of resulting records</returns>
		public int GetByDue(double MinAmountOverdue)
		{
		
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
	
			string condition = "(Select * from " + thisTable + crLf
				+  "Where " + thisTable + ".CurrentBalance > " + MinAmountOverdue.ToString() + crLf
				;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
				condition,
				thisTable
				);
			
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Customers with overdue accounts
		/// </summary>
		/// <param name="MinAmountOverdue">MinAmountOverdue</param>
		/// <param name="MinNumberOfDaysOverdue">MinNumberOfDaysOverdue</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOverdue(double MinAmountOverdue, int MinNumberOfDaysOverdue)
		{
		
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
	
			string condition = "(Select * from " + thisTable + crLf
				+  "Where " + thisTable + ".CurrentBalance > " + MinAmountOverdue.ToString() + crLf
				;

			if (MinNumberOfDaysOverdue > 0)
				condition +=  " And " + thisTable + ".DateLastOrder + Interval " + MinNumberOfDaysOverdue.ToString() + " Day < now() " + crLf
					;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
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
	
		#region Get Customer Fullname (auxillary function_
		/// <summary>
		/// GetCustomerFullName
		/// </summary>
		/// <param name="CustomerType">CustomerType</param>
		/// <param name="FirstName">FirstName</param>
		/// <param name="LastName">LastName</param>
		/// <param name="CompanyName">CompanyName</param>
		/// <returns>Customer FullName</returns>
		public string GetCustomerFullName(int CustomerType, string FirstName, string LastName, string CompanyName)
		{
			string Fullname = "";

			switch ((customerType) CustomerType)
			{
				case customerType.residential:
					if ((FirstName + " " + LastName).Trim() == "")
					{
						Fullname = CompanyName;
					}
					else
					{
						Fullname = (FirstName + " " + LastName).Trim() + " ";
						if (CompanyName != "")
							Fullname += "(" + CompanyName + ")";
					}
					break;
				default:
				case customerType.business:
					if (CompanyName.Trim() == "")
					{
						Fullname = (FirstName + " " + LastName).Trim();
					}
					else
					{
						Fullname = CompanyName + " ";
						if ((FirstName + " " + LastName).Trim() != "")
							Fullname += "(" + (FirstName + " " + LastName).Trim() + ")";
					}
					break;
			}

			Fullname = Fullname.Trim();

			return Fullname;

		 }

		#endregion

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CustomerGroupId">Customer's Group</param>
		/// <param name="CustomerType">Customer's Type</param>
		/// <param name="Title">Customer's Title</param>
		/// <param name="FirstName">Customer's First Name</param>
		/// <param name="LastName">Customer's Last Name</param>
		/// <param name="CompanyName">Customer's Company Name</param>
		/// <param name="AccountNumber">Customer's Account Number</param>
		/// <param name="CountryId">Customer's Country</param>
		/// <param name="OpeningBalance">Customer's Opening Balance</param>
		/// <param name="CreditLimit">Customer's Credit Limit</param>
		/// <param name="KdlComments">KDL's Comments regarding this Customer</param>
		/// <param name="CustomerComments">Customer's own Comments</param>
		/// <param name="DateStart">Customer's StartDate</param>
		/// <param name="StartDateForStatement">Customer's StartDateForStatement</param>
		/// <param name="StartDateForInvoices">Customer's StartDateForInvoices</param>
		/// <param name="IsDirectDebitCustomer">IsDirectDebitCustomer</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Add(int CustomerGroupId,
			int CustomerType, 
			string Title, 
			string FirstName, 
			string LastName,
			string CompanyName,
			string AccountNumber,
			int CountryId, 
			decimal OpeningBalance,
			decimal CreditLimit,
			string KdlComments,
			string CustomerComments,
			string DateStart,
			string StartDateForStatement,
			string StartDateForInvoices,
			int IsDirectDebitCustomer,
			int CurrentUser)
		{

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			string Fullname = GetCustomerFullName(CustomerType, FirstName, LastName, CompanyName);

			AddGeneral(CustomerGroupId,
				CustomerType,
				Title,
				FirstName,
				LastName,
				CompanyName,
				AccountNumber,
				CountryId,
				OpeningBalance,
				CreditLimit,
				KdlComments,
				CustomerComments,
				DateStart, 
				StartDateForStatement, 
				StartDateForInvoices, 
				Fullname,
				"",
				"",
				"",
				"",
				"",
				"",
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				IsDirectDebitCustomer,
				thisChangeData.LastIdAdded(),
				0);

			Save();

			int thisPremiseId = LastIdAdded();

		}

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal User table stack; the SaveUsers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CurrentUser">CurrentUser</param>
		/// <param name="thisPkId">thisPkId</param>
		public override int AddArchive(int CurrentUser, int thisPkId)
		{
			thisCustomer = new clsCustomer(thisDbType, localRecords.dbConnection);
			thisCustomer.GetByCustomerId(thisPkId);

			AddGeneral(thisCustomer.my_CustomerGroupId(0),
				thisCustomer.my_CustomerType(0), 
				thisCustomer.my_Title(0), 
				thisCustomer.my_FirstName(0), 
				thisCustomer.my_LastName(0),
				thisCustomer.my_CompanyName(0),
				thisCustomer.my_AccountNumber(0), 
				thisCustomer.my_CountryId(0), 
				thisCustomer.my_OpeningBalance(0), 
				thisCustomer.my_CreditLimit(0), 
				thisCustomer.my_KdlComments(0), 
				thisCustomer.my_CustomerComments(0), 
				thisCustomer.my_DateStart(0), 
				thisCustomer.my_StartDateForStatement(0), 
				thisCustomer.my_StartDateForInvoices(0), 
				thisCustomer.my_FullName(0),
				thisCustomer.my_DateLastLoggedIn(0),
				thisCustomer.my_DateLastLoggedInUtc(0),
				thisCustomer.my_DateFirstOrder(0),
				thisCustomer.my_DateFirstOrderUtc(0),
				thisCustomer.my_DateLastOrder(0),
				thisCustomer.my_DateLastOrderUtc(0),
				thisCustomer.my_NumOrders(0),
				thisCustomer.my_TotalItemCost(0),
				thisCustomer.my_TotalTaxCost(0),
				thisCustomer.my_TotalFreightCost(0),
				thisCustomer.my_BaseBalance(0),
				thisCustomer.my_CurrentBalance(0),
				thisCustomer.my_AvailableCredit(0),
				thisCustomer.my_TotalPurchases(0),
				thisCustomer.my_TotalPaid(0),
				thisCustomer.my_TotalUncleared(0),
				thisCustomer.my_InvoiceCurrentBalance(0),
				thisCustomer.my_InvoiceTotalPurchases(0),
				thisCustomer.my_InvoiceTotalPaid(0),
				thisCustomer.my_InvoiceTotalUncleared(0),
				thisCustomer.my_IsDirectDebitCustomer(0),
				thisCustomer.my_ChangeDataId(0),
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisCustomer.my_ChangeData_CreatedByUserId(0),
				thisCustomer.my_ChangeData_CreatedByFirstNameLastName(0),
				thisCustomer.my_ChangeData_DateCreated(0),
				CurrentUser,"",dtaNow.ToString());
							
			thisChangeData.Save();

			return thisChangeData.LastIdAdded();
	
		}

	
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Incident table stack; the SaveIncidents method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CustomerGroupId">CustomerGroupId</param>
		/// <param name="CustomerType">CustomerType</param>
		/// <param name="Title">Title</param>
		/// <param name="FirstName">FirstName</param>
		/// <param name="LastName">LastName</param>
		/// <param name="CompanyName">CompanyName</param>
		/// <param name="AccountNumber">AccountNumber</param>
		/// <param name="CountryId">CountryId</param>
		/// <param name="OpeningBalance">OpeningBalance</param>
		/// <param name="CreditLimit">CreditLimit</param>
		/// <param name="CustomerComments">CustomerComments</param>
		/// <param name="KdlComments">KdlComments</param>
		/// <param name="DateStart">DateStart</param>
		/// <param name="StartDateForStatement">Customer's StartDateForStatement</param>
		/// <param name="StartDateForInvoices">Customer's StartDateForInvoices</param>
		/// <param name="FullName">FullName</param>
		/// <param name="DateLastLoggedIn">DateLastLoggedIn</param>
		/// <param name="DateLastLoggedInUtc">DateLastLoggedInUtc</param>
		/// <param name="DateFirstOrder">DateFirstOrder</param>
		/// <param name="DateFirstOrderUtc">DateFirstOrderUtc</param>
		/// <param name="DateLastOrder">DateLastOrder</param>
		/// <param name="DateLastOrderUtc">DateLastOrderUtc</param>
		/// <param name="NumOrders">NumOrders</param>
		/// <param name="TotalItemCost">TotalItemCost</param>
		/// <param name="TotalTaxCost">TotalTaxCost</param>
		/// <param name="TotalFreightCost">TotalFreightCost</param>
		/// <param name="BaseBalance">BaseBalance</param>
		/// <param name="CurrentBalance">CurrentBalance</param>
		/// <param name="AvailableCredit">AvailableCredit</param>
		/// <param name="TotalPurchases">TotalPurchases</param>
		/// <param name="TotalPaid">TotalPaid</param>
		/// <param name="TotalUncleared">TotalUncleared</param>
		/// <param name="InvoiceCurrentBalance">InvoiceCurrentBalance</param>
		/// <param name="InvoiceTotalPurchases">InvoiceTotalPurchases</param>
		/// <param name="InvoiceTotalPaid">InvoiceTotalPaid</param>
		/// <param name="InvoiceTotalUncleared">InvoiceTotalUncleared</param>
		/// <param name="IsDirectDebitCustomer">IsDirectDebitCustomer</param>
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		public void AddGeneral(int CustomerGroupId,
			int CustomerType,
			string Title,
			string FirstName,
			string LastName,
			string CompanyName,
			string AccountNumber,
			int CountryId,
			decimal OpeningBalance,
			decimal CreditLimit,
			string KdlComments,
			string CustomerComments,
			string DateStart,
			string StartDateForStatement,
			string StartDateForInvoices,
			string FullName,
			string DateLastLoggedIn,
			string DateLastLoggedInUtc,
			string DateFirstOrder,
			string DateFirstOrderUtc,
			string DateLastOrder,
			string DateLastOrderUtc,
			int NumOrders,
			decimal TotalItemCost,
			decimal TotalTaxCost,
			decimal TotalFreightCost,
			decimal BaseBalance,
			decimal CurrentBalance,
			decimal AvailableCredit,
			decimal TotalPurchases,
			decimal TotalPaid,
			decimal TotalUncleared,
			decimal InvoiceCurrentBalance,
			decimal InvoiceTotalPurchases,
			decimal InvoiceTotalPaid,
			decimal InvoiceTotalUncleared,
			int IsDirectDebitCustomer,
			int ChangeDataId,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			if (CountryId == 0)
			{
				if (assumedCountryId == 0)
					GetGeneralSettings();
				CountryId = assumedCountryId;
			}

			rowToAdd["CountryId"] = CountryId;
			rowToAdd["CustomerType"] = CustomerType;
			rowToAdd["CustomerGroupId"] = CustomerGroupId;
			rowToAdd["Title"] = Title;
			rowToAdd["FirstName"] = FirstName;
			rowToAdd["LastName"] = LastName;
			rowToAdd["CompanyName"] = CompanyName;
			rowToAdd["AccountNumber"] = AccountNumber;

			rowToAdd["KdlComments"] = KdlComments;
			rowToAdd["CustomerComments"] = CustomerComments;

			rowToAdd["DateStart"] = SanitiseDate(DateStart);
			rowToAdd["StartDateForStatement"] = SanitiseDate(StartDateForStatement);
			rowToAdd["StartDateForInvoices"] = SanitiseDate(StartDateForInvoices);

			rowToAdd["FullName"] = FullName;

			rowToAdd["DateLastLoggedIn"] = SanitiseDate(DateLastLoggedIn);
			rowToAdd["DateLastLoggedInUtc"] = SanitiseDate(DateLastLoggedInUtc);
			rowToAdd["DateFirstOrder"] = SanitiseDate(DateFirstOrder);
			rowToAdd["DateFirstOrderUtc"] = SanitiseDate(DateFirstOrderUtc);
			rowToAdd["DateLastOrder"] = SanitiseDate(DateLastOrder);
			rowToAdd["DateLastOrderUtc"] = SanitiseDate(DateLastOrderUtc);

			rowToAdd["NumOrders"] = NumOrders;
			rowToAdd["TotalItemCost"] = TotalItemCost;
			rowToAdd["TotalTaxCost"] = TotalTaxCost;
			rowToAdd["TotalFreightCost"] = TotalFreightCost;
			rowToAdd["BaseBalance"] = BaseBalance;
			rowToAdd["CurrentBalance"] = CurrentBalance;
			rowToAdd["AvailableCredit"] = AvailableCredit;
			rowToAdd["TotalPurchases"] = TotalPurchases;
			rowToAdd["TotalPaid"] = TotalPaid;
			rowToAdd["TotalUncleared"] = TotalUncleared;
			rowToAdd["InvoiceCurrentBalance"] = InvoiceCurrentBalance;
			rowToAdd["InvoiceTotalPurchases"] = InvoiceTotalPurchases;
			rowToAdd["InvoiceTotalPaid"] = InvoiceTotalPaid;
			rowToAdd["InvoiceTotalUncleared"] = InvoiceTotalUncleared;
			rowToAdd["OpeningBalance"] = OpeningBalance;
			rowToAdd["CreditLimit"] = CreditLimit;
			rowToAdd["IsDirectDebitCustomer"] = IsDirectDebitCustomer;
			rowToAdd["ChangeDataId"] = ChangeDataId;
			rowToAdd["Archive"] = Archive;
			
			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				newDataToAdd.Rows.Add(rowToAdd);
			}


		}

		
		/// <summary>
		/// Allows Modification of a record in this Table
		/// </summary>
		/// <param name="CustomerId">CustomerId (Primary Key of Record)</param>
		/// <param name="CustomerType">Customer's Type</param>
		/// <param name="CustomerGroupId">Customer's Group</param>
		/// <param name="Title">Customer's Title</param>
		/// <param name="FirstName">Customer's First Name</param>
		/// <param name="LastName">Customer's Last Name</param>
		/// <param name="CompanyName">Customer's Company Name</param>
		/// <param name="AccountNumber">Customer's Account Number</param>
		/// <param name="CountryId">Customer's Country</param>
		/// <param name="OpeningBalance">Customer's Opening Balance</param>
		/// <param name="CreditLimit">Customer's Credit Limit</param>
		/// <param name="KdlComments">KDL's Comments regarding this Customer</param>
		/// <param name="CustomerComments">Customer's own Comments</param>
		/// <param name="DateStart">DateStart</param>
		/// <param name="StartDateForStatement">Customer's StartDateForStatement</param>
		/// <param name="StartDateForInvoices">Customer's StartDateForInvoices</param>
		/// <param name="DateLastLoggedIn">DateLastLoggedIn</param>
		/// <param name="IsDirectDebitCustomer">IsDirectDebitCustomer</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Modify(int CustomerId, 
			int CustomerGroupId,
			int CustomerType, 
			string Title, 
			string FirstName, 
			string LastName,
			string CompanyName,
			string AccountNumber,
			int CountryId, 
			decimal OpeningBalance,
			decimal CreditLimit,
			string KdlComments,
			string CustomerComments,
			string DateStart,
			string StartDateForStatement,
			string StartDateForInvoices,
			string DateLastLoggedIn,
			int IsDirectDebitCustomer,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, CustomerId);

			if (CountryId == 0)
			{
				if (assumedCountryId == 0)
					GetGeneralSettings();
				CountryId = assumedCountryId;
			}

			string Fullname = GetCustomerFullName(CustomerType, FirstName, LastName, CompanyName);

			rowToAdd["CustomerId"] = CustomerId;


			thisCustomer = new clsCustomer(thisDbType, localRecords.dbConnection);
			thisCustomer.GetByCustomerId(CustomerId);


			rowToAdd["DateFirstOrder"] = SanitiseDate(thisCustomer.my_DateFirstOrder(0));

			rowToAdd["DateFirstOrderUtc"] = SanitiseDate(thisCustomer.my_DateFirstOrderUtc(0));
			rowToAdd["DateLastOrder"] = SanitiseDate(thisCustomer.my_DateLastOrder(0));
			rowToAdd["DateLastOrderUtc"] = SanitiseDate(thisCustomer.my_DateLastOrderUtc(0));

			rowToAdd["NumOrders"] = thisCustomer.my_NumOrders(0);
			rowToAdd["TotalItemCost"] = thisCustomer.my_TotalItemCost(0);
			rowToAdd["TotalTaxCost"] = thisCustomer.my_TotalTaxCost(0);
			rowToAdd["TotalFreightCost"] = thisCustomer.my_TotalFreightCost(0);
			rowToAdd["BaseBalance"] = thisCustomer.my_BaseBalance(0);
			rowToAdd["CurrentBalance"] = thisCustomer.my_CurrentBalance(0);
			rowToAdd["AvailableCredit"] = thisCustomer.my_AvailableCredit(0);
			rowToAdd["TotalPurchases"] = thisCustomer.my_TotalPurchases(0);
			rowToAdd["TotalPaid"] = thisCustomer.my_TotalPaid(0);
			rowToAdd["TotalUncleared"] = thisCustomer.my_TotalUncleared(0);
			rowToAdd["InvoiceCurrentBalance"] = thisCustomer.my_InvoiceCurrentBalance(0);
			rowToAdd["InvoiceTotalPurchases"] = thisCustomer.my_InvoiceTotalPurchases(0);
			rowToAdd["InvoiceTotalPaid"] = thisCustomer.my_InvoiceTotalPaid(0);
			rowToAdd["InvoiceTotalUncleared"] = thisCustomer.my_InvoiceTotalUncleared(0);
			rowToAdd["OpeningBalance"] = thisCustomer.my_OpeningBalance(0);
			rowToAdd["CreditLimit"] = thisCustomer.my_CreditLimit(0);

			rowToAdd["CountryId"] = CountryId;
			rowToAdd["CustomerType"] = CustomerType;
			rowToAdd["CustomerGroupId"] = CustomerGroupId;
			rowToAdd["Title"] = Title;
			rowToAdd["FirstName"] = FirstName;
			rowToAdd["LastName"] = LastName;
			rowToAdd["CompanyName"] = CompanyName;
			rowToAdd["AccountNumber"] = AccountNumber;

			rowToAdd["KdlComments"] = KdlComments;
			rowToAdd["CustomerComments"] = CustomerComments;

			rowToAdd["DateStart"] = SanitiseDate(DateStart);
			rowToAdd["StartDateForStatement"] = SanitiseDate(StartDateForStatement);
			rowToAdd["StartDateForInvoices"] = SanitiseDate(StartDateForInvoices);
			rowToAdd["DateLastLoggedIn"] = SanitiseDate(DateLastLoggedIn);
			
			if (rowToAdd["DateLastLoggedIn"] == DBNull.Value)
				rowToAdd["DateLastLoggedInUtc"] = DBNull.Value;
			else
				rowToAdd["DateLastLoggedInUtc"] = SanitiseDate(FromClientToUtcTime(Convert.ToDateTime(DateLastLoggedIn)).ToString());

			rowToAdd["Fullname"] = Fullname;
			rowToAdd["OpeningBalance"] = OpeningBalance;
			rowToAdd["CreditLimit"] = CreditLimit;
			rowToAdd["IsDirectDebitCustomer"] = IsDirectDebitCustomer;
			rowToAdd["Archive"] = 0;



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
		/// <param name="CustomerId">Customer to set as logged in (Primary Key of Record)</param>
		public void SetAsLoggedIn(int CustomerId)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			SetAttribute(CustomerId, "DateLastLoggedIn", localRecords.DBDateTime(DateTime.Now));
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

		#region Update Methods
		/// <summary>
		/// Updates those elements of the table Customers which are summary data.
		/// </summary>
		/// <returns>Number of Records affected</returns>
		public int UpdateCustomer(int CustomerId)
		{
			string update = "Update tblCustomer," + crLf
				+ "(select tblCustomer.CustomerId, Count(PremiseId) as NumPremises " + crLf
				+ "	From tblCustomer left outer join " + crLf
				+ "(Select PremiseId, CustomerId from tblPremise where Archive = 0) Premises" + crLf
				+ "On Premises.CustomerId = tblCustomer.CustomerId" + crLf
				+ "Group By tblCustomer.CustomerId) Premises" + crLf
				+ "set tblCustomer.NumPremises = Premises.NumPremises" + crLf
				+ "where tblCustomer.CustomerId = Premises.CustomerId" + crLf
				+ "and CustomerId = " + CustomerId.ToString() +  crLf;

			
			int retval = localRecords.GetRecords(update);
			
			update = "Update tblCustomer," + crLf
				+ "(select tblCustomer.CustomerId, Count(PersonId) as NumPeople " + crLf
				+ "	From tblCustomer left outer join " + crLf
				+ "(Select PersonId, CustomerId from tblPerson where Archive = 0) People" + crLf
				+ "On People.CustomerId = tblCustomer.CustomerId" + crLf
				+ "Group By tblCustomer.CustomerId) People" + crLf
				+ "set tblCustomer.NumPeople = People.NumPeople" + crLf
				+ "where tblCustomer.CustomerId = People.CustomerId" + crLf
				+ "and CustomerId = " + CustomerId.ToString() +  crLf;

			retval += localRecords.GetRecords(update);

			update = "Update tblCustomer," + crLf
				+ "(select tblCustomer.CustomerId, Count(SPPremiseRoleId) as NumSPPremiseRoles, " + crLf
				+ "	Count(Distinct ServiceProviderId) as NumServiceProviders " + crLf
				+ "	From tblCustomer left outer join " + crLf
				+ "(Select tblSPPremiseRole.SPPremiseRoleId, tblSPPremiseRole.ServiceProviderId, tblSPPremiseRole.PremiseId, " + crLf
				+ "tblPremise.CustomerId" + crLf
				+ "	From tblSPPremiseRole, tblPremise where tblSPPremiseRole.Archive = 0 And tblPremise.Archive = 0 " + crLf
				+ " And tblPremise.PremiseId = tblSPPremiseRole.PremiseId) SPPremiseRoles" + crLf
				+ "On SPPremiseRoles.CustomerId = tblCustomer.CustomerId" + crLf
				+ "Group By tblCustomer.CustomerId) SPPremiseRoles" + crLf
				+ "set tblCustomer.NumSPPremiseRoles = SPPremiseRoles.NumSPPremiseRoles," + crLf
				+ " tblCustomer.NumServiceProviders = SPPremiseRoles.NumServiceProviders" + crLf
				+ "where tblCustomer.CustomerId = SPPremiseRoles.CustomerId" + crLf
				+ "and CustomerId = " + CustomerId.ToString() +  crLf;

			retval += localRecords.GetRecords(update);

			return retval;

			//			SPPremiseRolesSummaryQ.AddFromTable("(Select tblCustomer.CustomerId,"  + crLf
			//				+ "	Count(SPPremiseRoleId) as NumSPPremiseRoles," + crLf
			//				+ "	Count(Distinct ServiceProviderId) as NumDistinctServiceProviders" + crLf
			//				+ "From tblCustomer left outer join" + crLf
			//				+ "(Select tblSPPremiseRole.SPPremiseRoleId, tblSPPremiseRole.ServiceProviderId, tblSPPremiseRole.PremiseId, tblPremise.CustomerId " + crLf
			//				+ "	From tblSPPremiseRole, tblPremise where tblSPPremiseRole.Archive = 0 And tblPremise.Archive = 0 And tblPremise.PremiseId = tblSPPremiseRole.PremiseId) SPPremiseRoles" + crLf
			//				+ "On SPPremiseRoles.CustomerId = tblCustomer.CustomerId" + crLf
			//				+ "Group By tblCustomer.CustomerId) SPPremiseRoles");

			//			PersonSummaryQ.AddFromTable("(Select tblCustomer.CustomerId,"  + crLf
			//				+ "	Count(PersonId) as NumPeople" + crLf
			//				+ "From tblCustomer left outer join" + crLf
			//				+ "(Select PersonId, CustomerId from tblPerson where Archive = 0) People" + crLf
			//				+ "On People.CustomerId = tblCustomer.CustomerId" + crLf
			//				+ "Group By tblCustomer.CustomerId) People");

			//			PremiseSummaryQ.AddFromTable("(Select tblCustomer.CustomerId,"  + crLf
			//				+ "	Count(PremiseId) as NumPremises" + crLf
			//				+ "From tblCustomer left outer join" + crLf
			//				+ "(Select PremiseId, CustomerId from tblPremise where Archive = 0) Premises" + crLf
			//				+ "On Premises.CustomerId = tblCustomer.CustomerId" + crLf
			//				+ "Group By tblCustomer.CustomerId) Premises");

		}

		#endregion

		# region My_ Values Customer

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">Id</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public int my_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerId"));
		}

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupId">CustomerGroupId</see> of 
		/// <see cref="clsCustomerGroup">CustomerGroup</see> for this Customer</summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupId">CustomerGroupId</see> 
		/// of Associated <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// for this Customer</returns>
		public int my_CustomerGroupId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerGroupId"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerType">Type</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerType">Type</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public int my_CustomerType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerType"));
		}



		/// <summary>
		/// <see cref="clsCustomer.my_Title">Title</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_Title">Title</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Title");
		}

				
		/// <summary>
		/// <see cref="clsCustomer.my_FirstName">First Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_FirstName">First Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FirstName");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_LastName">Last Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_LastName">Last Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "LastName");
		}
		
		/// <summary>
		/// <see cref="clsCustomer.my_CompanyName">Company Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CompanyName">Company Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_CompanyName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CompanyName");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_AccountNumber">Account Number</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_AccountNumber">Account Number</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_AccountNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "AccountNumber");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_OpeningBalance">Opening Balance</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_OpeningBalance">Opening Balance</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public decimal my_OpeningBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "OpeningBalance"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CreditLimit">Credit Limit</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CreditLimit">Credit Limit</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public decimal my_CreditLimit(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "CreditLimit"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_FullName">Full Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_FullName">Full Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FullName");
		}

		/// <summary>
		/// <see cref="clsCountry.my_CountryId">CountryId</see> of 
		/// <see cref="clsCountry">Country</see></summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsCountry.my_CountryId">CountryId</see> 
		/// of Associated <see cref="clsCountry">Country</see> 
		/// for this Order</returns>
		public int my_CountryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CountryId"));
		}


		/// <summary>
		/// <see cref="clsCustomer.my_IsDirectDebitCustomer">Type</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_IsDirectDebitCustomer">Type</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public int my_IsDirectDebitCustomer(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsDirectDebitCustomer"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_DateStart">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_DateStart">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_DateStart(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateStart");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_DateLastLoggedIn(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateLastLoggedIn");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_DateLastLoggedInUtc">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_DateLastLoggedInUtc">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_DateLastLoggedInUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateLastLoggedInUtc");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_StartDateForStatement">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_StartDateForStatement">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_StartDateForStatement(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "StartDateForStatement");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_StartDateForInvoices">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_StartDateForInvoices">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_StartDateForInvoices(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "StartDateForInvoices");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "KdlComments");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerComments");
		}

		/// <summary>
		/// Total Spend by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Spend by this Customer</returns>
		public decimal my_TotalSpend(int rowNum)
		{
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalFreightCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalTaxCost"));
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalFreightCost"));
		}

		/// <summary>
		/// Total Item Cost of all Items bought by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Item Cost of all Items bought by this Customer</returns>
		public decimal my_TotalItemCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemCost"));
		}
		
		/// <summary>
		/// Total Tax Cost of all Taxs bought by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Tax Cost of all Taxs bought by this Customer</returns>
		public decimal my_TotalTaxCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalTaxCost"));
		}

		/// <summary>
		/// Total Freight Cost of all Freights bought by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Freight Cost of all Freights bought by this Customer</returns>
		public decimal my_TotalFreightCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalFreightCost"));
		}

		/// <summary>
		/// Number of Orders by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of Orders by this Customer</returns>
		public int my_NumOrders(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "NumOrders"));
		}

		/// <summary>
		/// The Date/Time that this Customer first Completed an Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer first Completed an Order</returns>
		public string my_DateFirstOrder(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateFirstOrder");
		}

		/// <summary>
		/// The Date/Time that this Customer First Completed an Order (Utc)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer First Completed an Order (Utc)</returns>
		public string my_DateFirstOrderUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateFirstOrderUtc");
		}

		/// <summary>
		/// The Date/Time that this Customer last Completed an Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer last Completed an Order</returns>
		public string my_DateLastOrder(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateLastOrder");
		}

		/// <summary>
		/// The Date/Time that this Customer last Completed an Order (Utc)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer last Completed an Order (Utc)</returns>
		public string my_DateLastOrderUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateLastOrderUtc");
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
		public decimal my_BaseBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "BaseBalance"));
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
		public decimal my_CurrentBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "CurrentBalance"));
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
		public decimal my_AvailableCredit(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "AvailableCredit"));
		}

		/// <summary>
		/// Customer's Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s. 
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s.</returns>
		public decimal my_TotalPurchases(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalPurchases"));
		}

		/// <summary>
		/// Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments</returns>
		public decimal my_TotalPaid(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalPaid"));
		}

		/// <summary>
		/// Customer's Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.</returns>
		public decimal my_TotalUncleared(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalUncleared"));
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
		public decimal my_InvoiceBaseBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "InvoiceBaseBalance"));
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
		public decimal my_InvoiceCurrentBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "InvoiceCurrentBalance"));
		}
		
		/// <summary>
		/// Customer's Invoice Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s. 
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Invoice Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s.</returns>
		public decimal my_InvoiceTotalPurchases(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "InvoiceTotalPurchases"));
		}

		/// <summary>
		/// Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments</returns>
		public decimal my_InvoiceTotalPaid(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "InvoiceTotalPaid"));
		}

		/// <summary>
		/// Customer's Invoice Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Invoice Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.</returns>
		public decimal my_InvoiceTotalUncleared(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "InvoiceTotalUncleared"));
		}

		# endregion

		# region My_ Values CustomerGroup

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupName">Name</see> of
		/// <see cref="clsCustomerGroup">CustomerGroup</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupName">Name</see> of 
		/// <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// </returns>
		public string my_CustomerGroup_CustomerGroupName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerGroup_CustomerGroupName");
		}

		#endregion

		#region My_ Values Country

		/// <summary>
		/// <see cref="clsCountry.my_CountryName">CountryName</see> of 
		/// <see cref="clsCountry">Country</see></summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsCountry.my_CountryName">CountryName</see> 
		/// of Associated <see cref="clsCountry">Country</see> 
		/// for this Order</returns>
		public string my_CountryName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CountryName");
		}

		#endregion

		#region My_ Values TransactionSummary
		
		/// <summary>
		/// Customer's Total Purchases in a given month
		/// </summary>
		/// <param name="MonthNum">Month Number for data (counting backwards from 0, which is current month)</param>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Purchases in a given month</returns>
		public decimal my_PurchasesMonth(int MonthNum, int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "PurchasesMonth" + MonthNum.ToString().Trim()));
		}

		/// <summary>
		/// Customer's Total Payments in a given month
		/// </summary>
		/// <param name="MonthNum">Month Number for data (counting backwards from 0, which is current month)</param>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Payments in a given month</returns>
		public decimal my_PaidMonth(int MonthNum, int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "PaidMonth" + MonthNum.ToString().Trim()));
		}
		
		/// <summary>
		/// Customer's Total Invoice Purchases in a given month
		/// </summary>
		/// <param name="MonthNum">Month Number for data (counting backwards from 0, which is current month)</param>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Invoice Purchases in a given month</returns>
		public decimal my_InvoicePurchasesMonth(int MonthNum, int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "InvoicePurchasesMonth" + MonthNum.ToString().Trim()));
		}

		/// <summary>
		/// Customer's Total Invoice Payments in a given month
		/// </summary>
		/// <param name="MonthNum">Month Number for data (counting backwards from 0, which is current month)</param>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Invoice Payments in a given month</returns>
		public decimal my_InvoicePaidMonth(int MonthNum, int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "InvoicePaidMonth" + MonthNum.ToString().Trim()));
		}
		#endregion

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
