using Resources;
using System;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.Odbc;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace Keyholders
{
	/// <summary>clsOrder deals with everything to do with data about Orders</summary>
	[GuidAttribute("46A8389B-A0B3-4d0e-8DEC-BCB2E157478B")]
	public class clsOrder : clsKeyBase
	{
		#region Initialisation

		#region Constructors

		/// <summary>Constructor for clsOrder</summary>
		public clsOrder() : base("Order")
		{
		}

		/// <summary>Constructor for clsOrder; Allows Calling of 'Connect' Function to be skipped</summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration <see cref="clsRecordHandler.databaseType">databaseType</see></param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsOrder(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Order")
		{
			Connect(typeOfDb, odbcConnection);
		}

		#endregion

		#region ConnectToForeignClasses

		/// <summary>Connect to Foreign Key classes within this class</summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration <see cref="clsRecordHandler.databaseType">databaseType</see></param>
		/// <param name="odbcConnection">An already open ODBC database connection</param>
		public override void ConnectToForeignClasses(
			clsRecordHandler.databaseType typeOfDb,
			OdbcConnection odbcConnection)
		{
			GetGeneralSettings();
		}

        #endregion

        #region LoadMainQuery

        #region QueryParts

        /// <summary>CustomerQ</summary>
        public clsQueryPart CustomerQ = new clsQueryPart();

		/// <summary>PersonQ</summary>
		public clsQueryPart PersonQ = new clsQueryPart();

		/// <summary>CountryQ</summary>
		public clsQueryPart CountryQ = new clsQueryPart();

		/// <summary>OrderStatusQ</summary>
		public clsQueryPart OrderStatusQ = new clsQueryPart();

		/// <summary>ItemSummaryQ</summary>
		public clsQueryPart ItemSummaryQ = new clsQueryPart();

		#endregion

		#region LoadMainQuery

		/// <summary>Loads the SQL for query types for the GetBys</summary>
		public override void LoadMainQuery()
		{
			MainQ = OrderQueryPart();
			MainQ.FromTables.Clear();
			MainQ.AddFromTable(thisTable + " left outer join tblPerson on tblOrder.PersonId = tblPerson.PersonId");

			PersonQ = PersonQueryPart();
			PersonQ.Joins.Clear();
			PersonQ.FromTables.Clear();

			CountryQ = CountryQueryPart();
			CustomerQ = CustomerQueryPart();
			OrderStatusQ = OrderStatusQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();

			baseQueries = new clsQueryPart[]
			{
				MainQ,
				CustomerQ,
				PersonQ,
				CountryQ,
				OrderStatusQ
			};

			//baseQueries = new clsQueryPart[5];
			
			//baseQueries[0] = MainQ;
			//baseQueries[1] = CustomerQ;
			//baseQueries[2] = PersonQ;
			//baseQueries[3] = CountryQ;
			//baseQueries[4] = OrderStatusQ;
//			queries[5] = ItemSummaryQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By tblOrder.OrderId" + crLf;
		}

		#endregion

		#endregion

		#region Initialise

		/// <summary>Initialise (or reinitialise) everything for this class</summary>
		public override void Initialise()
		{

			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);

			newDataToAdd.Columns.Add("CustomerId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PersonId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PaymentMethodTypeId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CustomerGroupId", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("OrderNum", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("CustomerType", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("FullName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Title", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("FirstName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("LastName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickPostalAddress", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("QuickDaytimePhone", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickDaytimeFax", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("QuickAfterHoursPhone", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickAfterHoursFax", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickMobilePhone", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("CountryId", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("Email", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("OrderSubmitted", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("OrderPaid", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("OrderCreatedMechanism", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("OrderStatusId", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("SupplierComment", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("DateCreated", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateCreatedUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSubmitted", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSubmittedUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateProcessed", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateProcessedUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateShipped", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateShippedUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateDue", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("InvoiceRequested", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("DateInvoiceLastPrinted", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("TaxAppliedToOrder", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("TaxRateAtTimeOfOrder", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("TaxCost", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("FreightCost", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("Total", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("TotalItemWeight", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("TotalItemCost", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("TotalItemFreightCost", System.Type.GetType("System.Decimal"));

			newDataToAdd.Columns.Add("IsInvoiceOrder", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("NumItems", System.Type.GetType("System.Int32"));


			dataToBeModified = new DataTable(thisTable);
			dataToBeModified.Columns.Add(thisPk, System.Type.GetType("System.Int64"));

			dataToBeModified.PrimaryKey = new System.Data.DataColumn[]
				{dataToBeModified.Columns[thisPk]};

			for (int colCounter = 0; colCounter < newDataToAdd.Columns.Count; colCounter++)
				dataToBeModified.Columns.Add(newDataToAdd.Columns[colCounter].ColumnName, newDataToAdd.Columns[colCounter].DataType);

			InitialiseWarningAndErrorTables();
			InitialiseAttributeChangeDataTable();
		}

		#endregion

		#endregion

		#region All GetBy Methods

		#region General GetBy Methods

		#region GetAll

		/// <summary>GetAll for this class</summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			thisSqlQuery = QB.BuildSqlStatement(queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByOrderId

		/// <summary>Get a Order by OrderId</summary>
		/// <param name="OrderId">OrderId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderId(int OrderId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable + crLf
				+ " Where " + thisTable + ".OrderId = " + OrderId.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#endregion

		#region GetBy SingleId and Type Methods

		#region GetByCustomerId

		/// <summary>Gets Orders by CustomerId</summary>
		/// <param name="CustomerId">CustomerId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".CustomerId = " + CustomerId.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByPersonId

		/// <summary>Gets Orders by PersonId</summary>
		/// <param name="PersonId">PersonId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonId(int PersonId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".PersonId = " + PersonId.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByPaymentMethodTypeId

		/// <summary>Gets Orders by PaymentMethodTypeId</summary>
		/// <param name="PaymentMethodTypeId">PaymentMethodTypeId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPaymentMethodTypeId(int PaymentMethodTypeId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".PaymentMethodTypeId = " + PaymentMethodTypeId.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByCustomerGroupId

		/// <summary>Gets Orders by CustomerGroupId</summary>
		/// <param name="CustomerGroupId">CustomerGroupId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerGroupId(int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".CustomerGroupId = " + CustomerGroupId.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByCustomerType

		/// <summary>Gets Orders by CustomerType</summary>
		/// <param name="CustomerType">CustomerType</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerType(int CustomerType)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".CustomerType = " + CustomerType.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByCountryId

		/// <summary>Gets Orders by CountryId</summary>
		/// <param name="CountryId">CountryId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCountryId(int CountryId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".CountryId = " + CountryId.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByOrderSubmitted

		/// <summary>Gets Orders by OrderSubmitted</summary>
		/// <param name="OrderSubmitted">OrderSubmitted</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderSubmitted(int OrderSubmitted)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".OrderSubmitted = " + OrderSubmitted.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByOrderPaid

		/// <summary>Gets Orders by OrderPaid</summary>
		/// <param name="OrderPaid">OrderPaid</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderPaid(int OrderPaid)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".OrderPaid = " + OrderPaid.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByOrderCreatedMechanism

		/// <summary>Gets Orders by OrderCreatedMechanism</summary>
		/// <param name="OrderCreatedMechanism">OrderCreatedMechanism</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderCreatedMechanism(int OrderCreatedMechanism)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".OrderCreatedMechanism = " + OrderCreatedMechanism.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByOrderStatusId

		/// <summary>Gets Orders by OrderStatusId</summary>
		/// <param name="OrderStatusId">OrderStatusId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderStatusId(int OrderStatusId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".OrderStatusId = " + OrderStatusId.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByInvoiceRequested

		/// <summary>Gets Orders by InvoiceRequested</summary>
		/// <param name="InvoiceRequested">InvoiceRequested</param>
		/// <returns>Number of resulting records</returns>
		public int GetByInvoiceRequested(int InvoiceRequested)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".InvoiceRequested = " + InvoiceRequested.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByIsInvoiceOrder

		/// <summary>Gets Orders by IsInvoiceOrder</summary>
		/// <param name="IsInvoiceOrder">IsInvoiceOrder</param>
		/// <returns>Number of resulting records</returns>
		public int GetByIsInvoiceOrder(int IsInvoiceOrder)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".IsInvoiceOrder = " + IsInvoiceOrder.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetByNumItems

		/// <summary>Gets Orders by NumItems</summary>
		/// <param name="NumItems">NumItems</param>
		/// <returns>Number of resulting records</returns>
		public int GetByNumItems(int NumItems)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".NumItems = " + NumItems.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#endregion

		#region Other GetBy Methods

		#region GetByNameOrNumber

		/// <summary>GetByNameOrNumber</summary>
		/// <param name="NameOrOrderNum">Filter for Name or Part of Name of Customer or Order Number or Part of Order Number</param>
		/// <returns>Number of resulting records</returns>
		public int GetByNameOrNumber(string NameOrOrderNum)
		{
			thisSqlQuery = mainSqlQuery;		
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


			thisSqlQuery = "Select distinct tblOrder.* from tblOrder, tblItem, tblPremise " + crLf
				+ "where tblOrder.OrderId = tblItem.OrderId " + crLf
				+ "and tblPremise.PremiseId = tblItem.PremiseId " + crLf
				+ "and concat_ws(' ',FirstName,LastName,FullName,QuickPostalAddress,OrderNum, PremiseNumber) " + MatchCondition(NameOrOrderNum, matchCriteria.contains) + crLf
				;
			

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetByOrderPaidNameOrOrderNumOrderStatus

		/// <summary>GetByOrderPaidNameOrOrderNumOrderStatus</summary>
		/// <param name="OrderPaid">Filter for orders that have been paid for or not</param>
		/// <param name="NameOrOrderNum">Filter for Name or Part of Name of Customer or Order Number or Part of Order Number</param>
		/// <param name="OrderStatus">Filter for Supplier Status of the order</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderPaidNameOrOrderNumOrderStatus(int OrderPaid, string NameOrOrderNum, int OrderStatus)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			//Condition
			string condition = "(Select * from " + thisTable + crLf
				+ "	Where concat_ws(' ',FirstName,LastName,CompanyName,OrderNum)   " + MatchCondition(NameOrOrderNum, matchCriteria.contains) + crLf;
			
			condition += "	And OrderPaid = " + OrderPaid.ToString() + crLf;

			if (OrderStatus != 0)
				condition += "	And OrderStatusId = " + OrderStatus.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
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

		#region GetByOrderPaidCustomerId

		/// <summary>GetByOrderPaidCustomerId</summary>
		/// <param name="OrderPaid">Filter for orders that have been paid for or not</param>
		/// <param name="CustomerId">Id of Customer</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderPaidCustomerId(int OrderPaid, int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".OrderPaid = " + OrderPaid.ToString() + crLf
				+ "		And " + thisTable + ".CustomerId = " + CustomerId.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetByOrderSubmittedOrderCreatedMechanism

		/// <summary>GetByOrderSubmittedOrderCreatedMechanism</summary>
		/// <param name="OrderSubmitted">Filter for orders that have been paid for or not</param>
		/// <param name="OrderCreatedMechanism">Id of Customer</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderSubmittedOrderCreatedMechanism(int OrderSubmitted, int OrderCreatedMechanism)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".OrderSubmitted = " + OrderSubmitted.ToString() + crLf
				+ "		And " + thisTable + ".OrderCreatedMechanism = " + OrderCreatedMechanism.ToString() + crLf
				+ ") " + thisTable
				);

			thisSqlQuery = QB.BuildSqlStatement(
				queries,
				OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetByOrderSubmittedNameOrOrderNumOrderStatus

		/// <summary>GetByOrderSubmittedNameOrOrderNumOrderStatus</summary>
		/// <param name="OrderSubmitted">Filter for orders that have been Submitted for or not</param>
		/// <param name="NameOrOrderNum">Filter for Name or Part of Name of Customer or Order Number or Part of Order Number</param>
		/// <param name="OrderStatus">Filter for Supplier Status of the order</param>
		/// <param name="IsInvoiceOrder">Whether to return orders that are invoices only or those which are not.</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderSubmittedNameOrOrderNumOrderStatus(
			int OrderSubmitted, 
			string NameOrOrderNum, 
			int OrderStatus,
			int IsInvoiceOrder)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			//Condition

			string condition1 = "(Select * from " + thisTable + crLf
				+ "	Where concat_ws(' ',FirstName,LastName,CompanyName,OrderNum)   " + MatchCondition(NameOrOrderNum, matchCriteria.contains) + crLf;
			
			condition1 += "	And OrderSubmitted = " + OrderSubmitted.ToString() + crLf;

			if (OrderStatus != 0)
				condition1 += "	And OrderStatusId = " + OrderStatus.ToString() + crLf;

			condition1 += ") " + thisTable;

			string condition2 = "(Select * from tblItem" 
				+ " Where (case tblItem.IsInvoiceOrder when 0 then 0 else 1 end) = "
				+ IsInvoiceOrder.ToString();

			condition2 += ") tblItem";

			clsQueryBuilder.ConditionWithTable[] thisConditions = new Resources.clsQueryBuilder.ConditionWithTable[2];
			thisConditions[0].condition = condition1;
			thisConditions[0].table = thisTable;
			thisConditions[1].condition = condition2;
			thisConditions[1].table = "tblItem";

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns,
				thisConditions
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetByOrderSubmittedCustomerId

		/// <summary>GetByOrderSubmittedCustomerId</summary>
		/// <param name="OrderSubmitted">Filter for orders that have been Submitted for or not</param>
		/// <param name="CustomerId">Id of Customer</param>
		/// <param name="IsInvoiceOrder">Whether to return orders that are invoices only or those which are not.</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderSubmittedCustomerId(
			int OrderSubmitted, 
			int CustomerId,
			int IsInvoiceOrder)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			//Condition
			
			string condition1 = "(Select * from " + thisTable + crLf
				+ "	Where OrderSubmitted = " + OrderSubmitted.ToString() + crLf;


			if (CustomerId != 0)
				condition1 += "	And CustomerId = " + CustomerId.ToString() + crLf;

			condition1 += ") " + thisTable;

			string condition2 = "(Select * from tblItem" 
				+ " Where (case tblItem.IsInvoiceOrder when 0 then 0 else 1 end) = "
				+ IsInvoiceOrder.ToString();

			condition2 += ") tblItem";

			clsQueryBuilder.ConditionWithTable[] thisConditions = new Resources.clsQueryBuilder.ConditionWithTable[2];
			thisConditions[0].condition = condition1;
			thisConditions[0].table = thisTable;
			thisConditions[1].condition = condition2;
			thisConditions[1].table = "tblItem";

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns,
				thisConditions
				);
			
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetUnsubmittedOrders

		/// <summary>GetUnsubmittedOrders</summary>
		/// <returns>Number of resulting records</returns>
		public int GetUnsubmittedOrders()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			//Condition
			string condition = "(Select * from " + thisTable + crLf
				+ "	Where OrderSubmitted = 0" + crLf
				;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
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

		#region GetUnsubmittedOrdersWithNoItems

		/// <summary>GetUnsubmittedOrdersWithNoItems</summary>
		/// <returns>Number of resulting records</returns>
		public int GetUnsubmittedOrdersWithNoItems()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			//Condition
			string condition = "(Select * from " + thisTable + crLf
				+ "	Where OrderSubmitted = 0" + crLf
				+ "	And (NumItems is null or NumItems = 0)" + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
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

		#region GetByDateCreated

		/// <summary>GetByDateCreated</summary>
		/// <param name="DateCreated">Filter for Name or Part of Name of Customer or Order Number or Part of Order Number</param>
		/// <returns>Number of resulting records</returns>
		public int GetByDateCreated(string DateCreated)
		{
		
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			thisSqlQuery = "Select * from tblOrder " + crLf
				+ "where tblOrder.DateCreated " + MatchCondition(DateCreated, matchCriteria.contains) + crLf
				;
	
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region GetDuplicatesForCustomerOnDateCreated

		/// <summary>GetDuplicatesForCustomerOnDateCreated</summary>
		/// <param name="Day">Day</param>
		/// <param name="Month">Month</param>
		/// <param name="Year">Year</param>
		/// <returns>Number of Duplicate Invoices on this day</returns>
		public int GetDuplicatesForCustomerOnDateCreated(int Day, int Month, int Year)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string thisSql = "Select CustomerId, count(OrderId), min(orderId), max(orderId) from tblOrder" + crLf
				+ "Where Day(tblOrder.DateCreated) = " + Day.ToString() + crLf
				+ "	And Month(tblOrder.DateCreated) = " + Month.ToString() + crLf
				+ "	And Year(tblOrder.DateCreated) = " + Year.ToString()+ crLf
				+ "Group by CustomerId " + crLf
				+ "Having count(OrderId) > 1; " + crLf
				;
	
			return localRecords.GetRecords(thisSql);
		}

		#endregion

		#region GetByCustomerIdOrderDateCreated

		/// <summary>GetByCustomerIdOrderDateCreated</summary>
		/// <param name="CustomerId">Id of Customer to retrieve Items for</param>
		/// <param name="Day">OrderDateCreated Day</param>
		/// <param name="Month">OrderDateCreated Month</param>
		/// <param name="Year"> OrderDateCreatedYear</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerIdOrderDateCreated(int CustomerId,
			int Day, int Month, int Year)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


			string condition = "(Select * From tblOrder" + crLf;	

			//Additional Condition
			condition += "Where CustomerId = " + CustomerId.ToString()
				;
			condition += "	And Day(tblOrder.DateCreated) = " + Day.ToString()
				;
			condition += "	And Month(tblOrder.DateCreated) = " + Month.ToString()
				;
			condition += "	And Year(tblOrder.DateCreated) = " + Year.ToString()
				;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				OrderByColumns,
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += "Order By tblOrder.OrderId";

			return localRecords.GetRecords(thisSqlQuery);
		}



		#endregion

		#endregion
		
		#endregion

		#region Delete methods

		/// <summary>
		/// Deletes Unsubmitted Orders With No Items
		/// </summary>
		public void DeleteUnsubmittedOrdersWithNoItems()
		{
			GetUnsubmittedOrdersWithNoItems();
			DeleteCurrentGetData();
		}

		/// <summary>
		/// Deletes Unsubmitted Orders With No Items
		/// </summary>
		public void DeleteUnsubmittedOrders()
		{
			clsItem thisItem = new clsItem(thisDbType, localRecords.dbConnection);

			string thisSql = "Delete tblItem.* from tblItem, tblOrder " + crLf
				+ " Where tblItem.OrderId = tblOrder.OrderId" + crLf
				+ " And tblOrder.OrderSubmitted = 0" + crLf
				;

			localRecords.GetRecords(thisSql);

//			thisItem.GetUnsubmittedOrders();
//			thisItem.DeleteCurrentGetData();

			thisSql = "Delete from tblOrder " + crLf
				+ " Where tblOrder.OrderSubmitted = 0" + crLf
				;

			localRecords.GetRecords(thisSql);

//			GetUnsubmittedOrders();
//			DeleteCurrentGetData();

		}


		#endregion

		#region CompleteOrder

		/// <summary>
		/// This Method indicates that an order is complete
		/// e.g. A Successful Credit Card Transaction
		/// </summary>
		/// <param name="OrderId">Id of Order to complete</param>
		/// <param name="CompletionTime">Date and Time of Successful Transaction 
		/// or otherwise Completion of this Order</param>
		public void CompleteOrder(int OrderId, string CompletionTime)
		{
			GetByOrderId(OrderId);

			SetAttribute(OrderId, "OrderPaid", "1");
			AddAttributeToSet(OrderId, "DateProcessed", CompletionTime);

			Save();

			clsItem Items = new clsItem(thisDbType, localRecords.dbConnection);

			//Reduce Stock Levels of Items in Order
			Items.UpdateStockLevelsOnCompletedOrder(OrderId);
		}
		

		#endregion

		#region UpdateOrderCosts

		/// <summary>
		/// Updates the 'Total', 'Cost', and 'Freight' for an Order based on Items in that order
		/// </summary>
		/// <param name="OrderId"></param>
		public void UpdateOrderCosts(int OrderId)
		{
			//Get Items in this Order
			GetByOrderId(OrderId);

			//Need Customer Group Id and CountryId/Shipping Zone to Update Item and Freight Costs
			int thisOrderCustomerGroupId = my_CustomerGroupId(0);
			if (thisOrderCustomerGroupId == 0)
				thisOrderCustomerGroupId = publicCustomerGroupId;

			int thisOrderCountryId = my_CountryId(0);
			int thisOrderShippingZoneId = my_Country_ShippingZoneId(0);
			int thisOrderTaxApplied = my_Country_LocalTaxApplies(0);

			if (thisOrderCountryId == 0)
			{
				thisOrderCountryId = assumedCountryId;
				//Get Country for this Order
				clsCountry Country = new clsCountry(thisDbType, localRecords.dbConnection);
				Country.GetByCountryId(thisOrderCountryId);
				thisOrderTaxApplied = Country.my_LocalTaxApplies(0);
				thisOrderShippingZoneId = Country.my_ShippingZoneId(0);
			}


			//Update Items within the Order; ensure Items have the correct cost given the Customer's Group
			clsItem Items = new clsItem(thisDbType, localRecords.dbConnection);
			Items.UpdateItemAndFreightCosts(OrderId, thisOrderCustomerGroupId, thisOrderShippingZoneId);
			int numItems = Items.GetByOrderId(OrderId);
			decimal TotalItemWeight = 0;
			decimal TotalItemCost = 0;
			decimal TotalItemFreightCost = 0;
			decimal IsInvoiceOrder = 0;
			for(int counter = 0; counter < numItems; counter++)
			{
				TotalItemWeight += Items.my_Weight(counter);
				TotalItemCost += Items.my_CostExcludingTax(counter);
				TotalItemFreightCost += Items.my_FreightCost(counter);
//				if (Items.my_InvoiceCost(counter) > 0)
//					IsInvoiceOrder= 1;

			}
//			//Reload order (Now with new Item Summary Costs)
//			GetByOrderId(OrderId);
//			
			decimal TaxCost = 0;
//			decimal SubTotal = my_TotalItemCostExcludingTax(0);
//			decimal TotalWeight = my_TotalItemWeight(0);
//			decimal TotalItemFreightCost = my_TotalItemFreightCostExcludingTax(0);
//			
//			decimal FreightCost = CalculateFreightForOrder(OrderId, thisOrderCustomerGroupId, 
//				thisOrderShippingZoneId, SubTotal, TotalWeight, TotalItemFreightCost);

			decimal FreightCost = CalculateFreightForOrder(OrderId, thisOrderCustomerGroupId, 
				thisOrderShippingZoneId, TotalItemCost, TotalItemWeight, TotalItemFreightCost);


			if (IsInvoiceOrder == 1)
			{
				
				SetAttribute(OrderId, "FreightCost", "0");
				AddAttributeToSet(OrderId, "TaxCost", "0");
				AddAttributeToSet(OrderId, "TaxRateAtTimeOfOrder", "0");
				AddAttributeToSet(OrderId, "TaxAppliedToOrder", "0");
				
			}
			else
			{
				if  (thisOrderTaxApplied == 1)
					TaxCost = CalculateTaxForOrder(TotalItemCost, FreightCost);
 
				SetAttribute(OrderId, "Total", TotalItemCost.ToString());
				AddAttributeToSet(OrderId, "FreightCost", FreightCost.ToString());
				AddAttributeToSet(OrderId, "TaxCost", TaxCost.ToString());
				AddAttributeToSet(OrderId, "TaxRateAtTimeOfOrder", (localTaxRate - 1).ToString());
				AddAttributeToSet(OrderId, "TaxAppliedToOrder", thisOrderTaxApplied.ToString());
			}

			#region Added

			AddAttributeToSet(OrderId, "TotalItemWeight", TotalItemWeight.ToString());
			AddAttributeToSet(OrderId, "TotalItemCost", TotalItemCost.ToString());
			AddAttributeToSet(OrderId, "TotalItemFreightCost", TotalItemFreightCost.ToString());
			AddAttributeToSet(OrderId, "IsInvoiceOrder", IsInvoiceOrder.ToString());
			AddAttributeToSet(OrderId, "NumItems", numItems.ToString());
			
			#endregion
		}



		#endregion

		#region UpdateUnpaidAutomaticallyGeneratedOrderCosts

		/// <summary>
		/// Updates the 'Total', 'Cost', and 'Freight' for an Order based on Items in that order
		/// </summary>
		public void UpdateUnpaidAutomaticallyGeneratedOrderCosts()
		{
			string update = "Update "  + crLf
				+ "(select OrderId, sum(Cost * Quantity) as RawCost from tblItem group by OrderId) ItemSummary, " + crLf
				+ thisTable + ", tblCustomer, tblCountry, tblShippingZone" + crLf
				+ " Set " + thisTable + ".TaxRateAtTimeOfOrder = " + (localTaxRate - 1).ToString() + "," + crLf
				+ thisTable + ".TaxCost = (case when tblCountry.LocalTaxApplies then RawCost * 0.125 else 0 end)" + crLf
				+ "Where tblOrder.CustomerId = tblCustomer.CustomerId " + crLf
				+ " and tblCustomer.CountryId = tblCountry.CountryId " + crLf
				+ " and tblCountry.ShippingZoneId = tblShippingZone.ShippingZoneId " + crLf
				+ " and tblOrder.OrderId = ItemSummary.OrderId " + crLf;
		}



		#endregion

		#region CalculateFreightForOrder

		/// <summary>
		/// Calulates the Total Freight Cost (not including tax) for an order
		/// </summary>
		/// <param name="OrderId">Id of Order To Calculate Freight For</param>
		/// <param name="thisOrderCustomerGroupId">Id of Customer Group for Order</param>
		/// <param name="thisOrderShippingZoneId">Id of Shipping Zone for Order</param>
		/// <param name="SubTotal">Sub Total not including Tax or Freight</param>
		/// <param name="TotalWeight">Total Weight of the Order</param>
		/// <param name="TotalItemFreightCost">Freight Cost (for item by item basis)</param>
		/// <returns>Freight Cost for Order</returns>
		private decimal CalculateFreightForOrder(int OrderId, 
			int thisOrderCustomerGroupId,  
			int thisOrderShippingZoneId,
			decimal SubTotal, 
			decimal TotalWeight,
			decimal TotalItemFreightCost)
		{
			decimal totalFreightCost = 0;

			clsFrSzCgP thisFrSzCgP = new clsFrSzCgP(thisDbType, localRecords.dbConnection);

			thisFrSzCgP.AddOrderByColumns("Cost", true);

			int	numApplyingRules;
			switch (freightChargeBasis)
			{
				case freightChargeType.singleChargePerItem:
					//For each item get the minimum cost for each
					totalFreightCost = TotalItemFreightCost;
					break;
				case freightChargeType.singleChargePerValueRange:
					numApplyingRules = thisFrSzCgP.GetByShippingZoneIdCustomerGroupIdValue(
						thisOrderShippingZoneId, thisOrderCustomerGroupId, SubTotal);
					if (numApplyingRules > 0)
						totalFreightCost = thisFrSzCgP.my_FreightRule_FRCostExcludingTax(0);
					break;
				case freightChargeType.singleChargePerWeightRange:
					numApplyingRules = thisFrSzCgP.GetByShippingZoneIdCustomerGroupIdWeight(
						thisOrderShippingZoneId, thisOrderCustomerGroupId, TotalWeight);
					if (numApplyingRules > 0)
						totalFreightCost = thisFrSzCgP.my_FreightRule_FRCostExcludingTax(0);
					break;
				default:
					break;
			}

			if (totalFreightCost < minimumFreightCharge)
				totalFreightCost = minimumFreightCharge;

			if (maximumFreightCharge != 0 && totalFreightCost > maximumFreightCharge)
				totalFreightCost = maximumFreightCharge;

			return totalFreightCost;
		}

		/// <summary>
		/// Calulates the Total Tax for an order
		/// </summary>
		/// <param name="SubTotal">Sub Total not including Tax or Freight</param>
		/// <param name="FreightCost">Cost of Freight for the order</param>
		/// <returns>Total Tax</returns>
		private decimal CalculateTaxForOrder(decimal SubTotal, decimal FreightCost)
		{
			// Tax charge = Sum of Tax on All Items + Tax on Freight
			if (priceShownIncludesLocalTaxRate)
				return (SubTotal + FreightCost) * (localTaxRate - 1) / (localTaxRate);
			else
				return (SubTotal + FreightCost) * (localTaxRate - 1);

		}


		#endregion

		#region GetNextOrderNumber / GetBaseOrderNumber / UpdateOrderNumbers

		/// <summary>
		/// Returns the next order number (and increments this)
		/// </summary>
		/// <returns>Next Order Number</returns>
		public string GetNextOrderNumber()
		{	
			return Setting.GetNextOrderNumber();
		}

		/// <summary>
		/// Returns the Base order number (and increments this)
		/// </summary>
		/// <returns>Base Order Number</returns>
		public string GetBaseOrderNumber()
		{	
			return Setting.GetBaseOrderNumber();
		}



		/// <summary>
		/// Returns the next order number (and increments this)
		/// </summary>
		/// <returns>Next Order Number</returns>
		public void UpdateOrderNumbers()
		{	
			string update = "Update tblOrder set OrderNum = OrderId + " + Convert.ToInt32(GetBaseOrderNumber())
				+ " Where OrderNum = 0";

			localRecords.GetRecords(update);
		}

		#endregion

		#region Set from PersonId

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Person table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="OrderId">OrderId (Primary Key of Record)</param>
		/// <param name="PersonId">Person Associated with this Order</param>
		public void SetFromPersonId(int OrderId,
			int PersonId)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			clsPerson Person = new clsPerson(thisDbType, localRecords.dbConnection);

			int numPeople = Person.GetByPersonId(PersonId);

			if (numPeople != 0)
			{
				rowToAdd["CustomerId"] = Person.my_CustomerId(0);
				rowToAdd["CustomerGroupId"] = Person.my_Customer_CustomerGroupId(0);
				rowToAdd["CustomerType"] = Person.my_Customer_CustomerType(0);
				rowToAdd["FullName"] = Person.my_Customer_FullName(0);
				rowToAdd["Title"] = Person.my_Title(0);
				rowToAdd["FirstName"] = Person.my_FirstName(0);
				rowToAdd["LastName"] = Person.my_LastName(0);
				rowToAdd["QuickPostalAddress"] = Person.my_QuickPostalAddress(0);
				rowToAdd["QuickDaytimePhone"] = Person.my_QuickDaytimePhone(0);
				rowToAdd["QuickDaytimeFax"] = Person.my_QuickDaytimeFax(0);
				rowToAdd["QuickAfterHoursPhone"] = Person.my_QuickAfterHoursPhone(0);
				rowToAdd["QuickAfterHoursFax"] = Person.my_QuickAfterHoursFax(0);
				rowToAdd["QuickMobilePhone"] = Person.my_QuickMobilePhone(0);
				rowToAdd["CountryId"] = Person.my_Customer_CountryId(0);
				rowToAdd["Email"] = Person.my_Email(0);

				clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);
				thisOrder.GetByOrderId(OrderId);

				rowToAdd["OrderId"] = OrderId;
				rowToAdd["PersonId"] = PersonId;

				//Rest is copied form the old order

				if (thisOrder.my_DateCreated(0) == "")
				{
					rowToAdd["DateCreated"] = DBNull.Value;
					rowToAdd["DateCreatedUtc"] =  DBNull.Value;
				}
				else
				{
					rowToAdd["DateCreated"] = thisOrder.my_DateCreated(0);
					rowToAdd["DateCreatedUtc"] =  thisOrder.my_DateCreatedUtc(0);
				}

				if (thisOrder.my_DateProcessed(0) == "")
				{
					rowToAdd["DateProcessed"] = DBNull.Value;
					rowToAdd["DateProcessedUtc"] =  DBNull.Value;
				}
				else
				{
					rowToAdd["DateProcessed"] = thisOrder.my_DateProcessed(0);
					rowToAdd["DateProcessedUtc"] =  thisOrder.my_DateProcessedUtc(0);
				}

				if (thisOrder.my_DateShipped(0) == "")
				{
					rowToAdd["DateShipped"] = DBNull.Value;
					rowToAdd["DateShippedUtc"] =  DBNull.Value;
				}
				else
				{
					rowToAdd["DateShipped"] = thisOrder.my_DateShipped(0);
					rowToAdd["DateShippedUtc"] = thisOrder.my_DateShippedUtc(0);
				}

				if (thisOrder.my_DateSubmitted(0) == "")
				{
					rowToAdd["DateSubmitted"] = DBNull.Value;
					rowToAdd["DateSubmittedUtc"] =  DBNull.Value;
				}
				else
				{
					rowToAdd["DateSubmitted"] = thisOrder.my_DateSubmitted(0);
					rowToAdd["DateSubmittedUtc"] = thisOrder.my_DateSubmittedUtc(0);
				}

				if (thisOrder.my_DateDue(0) == "")
				{
					rowToAdd["DateDue"] = DBNull.Value;
				}
				else
				{
					rowToAdd["DateDue"] = thisOrder.my_DateDue(0);
				}

				rowToAdd["OrderSubmitted"] = 0;
				rowToAdd["PaymentMethodTypeId"] = thisOrder.my_PaymentMethodTypeId(0);
				rowToAdd["OrderPaid"] = thisOrder.my_OrderPaid(0);
				rowToAdd["OrderStatusId"] = thisOrder.my_OrderStatusId(0);
				rowToAdd["SupplierComment"] = thisOrder.my_SupplierComment(0);
				rowToAdd["OrderNum"] = thisOrder.my_OrderNum(0);
				rowToAdd["TaxRateAtTimeOfOrder"] = thisOrder.my_TaxRateAtTimeOfOrder(0);
				rowToAdd["TaxAppliedToOrder"] = thisOrder.my_TaxAppliedToOrder(0);			
			
				//Reset Costs
				rowToAdd["Total"] = DBNull.Value;
				rowToAdd["FreightCost"] = DBNull.Value;
				rowToAdd["TaxCost"] = DBNull.Value;

				//Validate the data supplied
				Validate(rowToAdd, true);
			
				if (NumErrors() == 0)
				{
					if (UserChanges(rowToAdd))
						dataToBeModified.Rows.Add(rowToAdd);
				}
			
			}

		}

		#endregion

		#region SubmitOrder

		/// <summary>
		/// SubmitOrder
		/// </summary>
		/// <param name="OrderId">OrderId</param>
		/// <param name="DateSubmitted">DateSubmitted</param>
		/// <param name="UserId">UserId</param>
		/// <param name="UserName">UserName</param>
		/// <param name="UserIPAddress">UserIPAddress</param>
		public void SubmitOrder(int OrderId, string DateSubmitted, int UserId, string UserName, string UserIPAddress)
		{
			clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);

			int numOrders = thisOrder.GetByOrderId(OrderId);

			if (numOrders > 0)
			{
				int OrderSubmitted = 1;

				SetAttribute(OrderId, "OrderSubmitted", OrderSubmitted.ToString());
				AddAttributeToSet(OrderId, "DateSubmitted", localRecords.DBDateTime(Convert.ToDateTime(DateSubmitted)));
				AddAttributeToSet(OrderId, "DateShippedUtc",localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(Convert.ToDateTime(DateSubmitted)))));

				clsTransaction thisTransaction = new clsTransaction(thisDbType, localRecords.dbConnection);

				thisTransaction.AddByVendor(thisOrder.my_CustomerId(0),
					UserId,
					thisOrder.my_Customer_FullName(0),
					UserName,
					OrderId,
					paymentMethodType_asYetUndetermined(),
					0,
					0,
					1,
					(thisOrder.my_TotalItemCostExcludingTax(0) + thisOrder.my_TaxCost(0)) * -1,
					DateSubmitted,
					"Invoice #" + thisOrder.my_OrderNum(0),
					UserIPAddress);

				thisTransaction.Save();

			}

		}

		#endregion

		#region SubmitAllUnsubmittedOrders

		/// <summary>SubmitAllUnsubmittedOrders</summary>
		/// <param name="DateSubmitted">Submittion Date</param>
		/// <param name="SeedInvoiceNumber">SeedInvoiceNumber</param>
		/// <param name="LogFolder">LogFolder</param>
		/// <param name="fileToExportTo">fileToExportTo</param>
		public int SubmitAllUnsubmittedOrders(string DateSubmitted, int SeedInvoiceNumber, string LogFolder, string fileToExportTo)
		{
			DateTime thisDateTime = Convert.ToDateTime(DateSubmitted);
			DateTime thisUtcDateTime = thisDateTime.ToUniversalTime();

			if (localTaxRate == 0)
				GetGeneralSettings();

		
			string insert = "Insert into tblTransaction(CustomerId, PersonId, UserId, OrderId, PaymentMethodTypeId, " + crLf
				+  "CCAttemptId, Amount, Pending, IsInvoicePayment, DateSubmitted, DateSubmittedUtc, DateCompleted, DateCompletedUtc, " + crLf
				+  "VendorMemo, CustomerIPAddress, UserIPAddress, CustomerName, PersonName, UserName)" + crLf
				
				+  "Select tblOrder.CustomerId, tblOrder.PersonId, 1 as UserId, tblOrder.OrderId," + crLf
				+  "CustomerPaymentMethod as PaymenthMethodTypeId, null as CCAttemptId, "
			;

			if (priceShownIncludesLocalTaxRate)
				insert += "RawCost as Amount," + crLf;
			else
				insert += "RawCost + TaxCost as Amount," + crLf;

			insert += "0 as Pending, 0 as IsInvoicePayment," + crLf
				+  "Convert('" + localRecords.DBDateTime(thisDateTime) + "', datetime) as DateSubmitted," + crLf
				+  "Convert('" + localRecords.DBDateTime(thisUtcDateTime) + "', datetime) as DateSubmittedUtc," + crLf
				+  "null as DateCompleted, null as DateCompletedUtc, concat('Invoice #', tblOrder.OrderNum) as VendorMemo," + crLf
				+  "'' as CustomerIPAddress, '' as UserIPAddress, FullName as CustomerName, concat_ws(' ',FirstName,LastName) as PersonName, '' as UserName" + crLf
				
				+  "from (Select OrderId, sum(Cost * Quantity) as RawCost from tblItem group by OrderId) ItemSummary, tblOrder," + crLf
				+ "(Select CustomerId, case IsDirectDebitCustomer when 1 then " + paymentMethodType_directDebit() 
				+ " else " + paymentMethodType_asYetUndetermined() + " end as CustomerPaymentMethod from tblCustomer) tblCustomer" + crLf
				+  "where ItemSummary.OrderId = tblOrder.OrderId" + crLf
				+  "	and tblOrder.CustomerId = tblCustomer.CustomerId" + crLf
				+  "	and OrderSubmitted = 0" + crLf
				+  "	and OrderCreatedMechanism = " + orderCreatedMechanism_byVendorAutomatically().ToString();

			localRecords.GetRecords(insert);


			#region Get Customers about to be affected, so their Customer records can be updated
			string CustomersWithNewOrders = "Select distinct(CustomerId) as CustomerId from tblOrder" + crLf
				+ " where OrderSubmitted = 0 and OrderCreatedMechanism = " + orderCreatedMechanism_byVendorAutomatically().ToString();

			clsCustomer thisCustomer = new clsCustomer(thisDbType, localRecords.dbConnection);

			int numCustomer = thisCustomer.localRecords.GetRecords(CustomersWithNewOrders);

			#endregion


			#region Export the Orders to be submitted

			int numOrders = ExportToCsvForMonth(SeedInvoiceNumber, LogFolder, fileToExportTo);

			#endregion

			#region Submit the orders

			string update = "Update tblOrder set OrderSubmitted = 1, InvoiceRequested = 1," + crLf
				+ "DateSubmitted = '" + localRecords.DBDateTime(thisDateTime) + "'," + crLf
				+ "DateSubmittedUtc = '" + localRecords.DBDateTime(thisUtcDateTime) + "'" + crLf
				+ " where OrderSubmitted = 0 and OrderCreatedMechanism = " + orderCreatedMechanism_byVendorAutomatically().ToString();

			localRecords.GetRecords(update);

			#endregion

			#region Ensure the transactions are appropriatly added
			clsTransaction thisTransaction = new clsTransaction(thisDbType, localRecords.dbConnection);

			int numTransactions = thisTransaction.GetByNullPostBalance();

			for(int counter = 0; counter < numTransactions; counter++)
			{
				clsTransaction editTran = new clsTransaction(thisDbType, localRecords.dbConnection);

				editTran.UpdateCustomerTransactions(thisTransaction.my_CustomerId(counter));

			}
			#endregion

			#region Update the Customer Record

			clsOrder thisCustUpdate = new clsOrder(thisDbType, localRecords.dbConnection);
			for(int counter = 0; counter < numCustomer; counter++)
			{
				thisCustUpdate.UpdateCustomerOrders(thisCustomer.my_CustomerId(counter));
			}


			#endregion

			return numOrders;
		}

        #endregion

        #region ExportToCsvForMonth

        /// <summary>ExportToCsvForMonth</summary>
        /// <param name="SeedInvoiceNumber">SeedInvoiceNumber</param>
        /// <param name="LogFolder">LogFolder</param>
        /// <param name="fileToExportTo">fileToExportTo</param>
        /// <returns>Number of Orders</returns>
        public int ExportToCsvForMonth(int SeedInvoiceNumber, string LogFolder, string fileToExportTo)
		{

			#region Define Xero file

			string[] XeroHeader = {"ContactName",
									  "EmailAddress",
									  "POAddressLine1",
									  "POAddressLine2",
									  "POAddressLine3",
									  "POAddressLine4",
									  "POCity",
									  "PORegion",
									  "POPostalCode",
									  "POCountry",
									  "InvoiceNumber",
									  "Reference",
									  "InvoiceDate",
									  "DueDate",
									  "SubTotal",
									  "TotalTax",
									  "Total",
									  "Description",
									  "Quantity",
									  "UnitAmount",
									  "AccountCode",
									  "TaxType",
									  "TaxAmount",
									  "TrackingName1",
									  "TrackingOption1",
									  "TrackingName2",
									  "TrackingOption2",
									  "Currency",
									  "BrandingTheme"
								  };

			#endregion

			#region Get Items associated with Unsubmitted Orders

			clsItem thisItem = new clsItem(thisDbType, localRecords.dbConnection);

            int numOrderItems = thisItem.GetByOrderSubmittedOrderCreatedMechanism(0, orderCreatedMechanism_byVendorAutomatically());

            #endregion

			#region Create Export File

			string FileNameForExport = LogFolder + fileToExportTo;

            clsCsvWriter thisCsvWriter = new clsCsvWriter(FileNameForExport, true);
            thisCsvWriter.WriteFields(XeroHeader);

            #endregion

            #region Get Default Data

            int LastInvoiceNumber = 0;

            int rollingInvoiceIncrement = -1;


            string ContactName = "";
            string EmailAddress = "";

            string BillingAddressLine1 = "";
            string BillingAddressLine2 = "";
            string BillingAddressLine3 = "";
            string BillingAddressLine4 = "";
            string POCity = "";
            string PORegion = "";
            string POPostalCode = "";
            string POCountry = "";

            string AccountOrderNumber = "";
            string BatchLineItemExportNumber = "";

            string Reference = "";

            string InvoiceDate = "";
            string DueDate = "";
            string SubTotal = "";
            string TotalTax = "";
            string Total = "";

            string Description = "";
            //int thisPropertyId = 0;

            int thisProdcutId = 0;

            decimal dQuantity = 0;

            string Quantity = String.Format("{0:0.00}", dQuantity);

            //int NumPurchaseInstances = 0;

            //int ProductChargingFrequencyType = 0;

            string UnitAmount = "";
            string AccountCode = "";
            string TaxType = "";

            //productChargingMechanismType thisChargeType = productChargingMechanismType.automaticCreditCardCharge;

            string TaxAmount = "";

            string TrackingName1 = "";
            string TrackingOption1 = "";
            string TrackingName2 = "";
            string TrackingOption2 = "";
            string Currency = "";
            string BrandingTheme = "";
            string DDAccountNumber = "";

            #endregion

            for (int counter = 0; counter < numOrderItems; counter++)
			{

				#region Deal with Invoice Numbers

				int thisInvoiceNumber = Convert.ToInt32(thisItem.my_Order_OrderNum(counter));

				string InvoiceNumber = "";

				if (thisInvoiceNumber != LastInvoiceNumber)
				{
					#region New invoice; increment invoice number

					rollingInvoiceIncrement++;
					LastInvoiceNumber = thisInvoiceNumber;

					#endregion

				}

				InvoiceNumber = (SeedInvoiceNumber + rollingInvoiceIncrement).ToString();

				#endregion

				//DDAccountNumber = thisItem.my_Order_DDAccountNumber(counter);

				ContactName = thisItem.my_Order_FullName(counter);
				EmailAddress = thisItem.my_Order_Email(counter);

                #region Deal with Address

                string FullAddress = thisItem.my_Order_QuickPostalAddress(counter);
				if (!FullAddress.EndsWith(crLf))
					FullAddress += crLf;

				ArrayList AddressLines = DelimitedListToArrayList(FullAddress, crLf);

				int numLines = AddressLines.Count;

				POCity = "";
				PORegion = "";
				POPostalCode = "";
				POCountry = "";

				#region Country is the last line of the address

				if (numLines > 0)
                {
					POCountry = ((string)AddressLines[numLines - 1]).Trim();
					numLines--;
				}

				#endregion

				#region City is the second to last line of the address

				if (numLines > 0)
				{
					POCity = ((string)AddressLines[numLines - 1]).Trim();
					numLines--;

					int lastSpaceInCity = POCity.LastIndexOf(" ");

					if (lastSpaceInCity > -1)
                    {
						#region Check if last word is PostCode

						string lastWord = POCity.Substring(lastSpaceInCity).Trim();

						if (isNumerical(lastWord))
                        {
							POPostalCode = lastWord;
						}

						#endregion
					}
                }

				#endregion

				#region Rest of Address

				if (numLines > 0)
				{
					BillingAddressLine1 = ((string)AddressLines[0]).Trim();
				}
				else
					BillingAddressLine1 = "";


				if (numLines > 1)
					BillingAddressLine2 = ((string)AddressLines[1]).Trim();
				else
					BillingAddressLine2 = "";

				if (numLines > 2)
					BillingAddressLine3 = ((string)AddressLines[2]).Trim();
				else
					BillingAddressLine3 = "";

				if (numLines > 3)
                {
					BillingAddressLine4 = ((string)AddressLines[3]).Trim();

					for (int adCounter = 4; adCounter < numLines; adCounter++)
						BillingAddressLine4 += crLf + ((string)AddressLines[adCounter]).Trim();

				}
				else
					BillingAddressLine4 = "";

				#endregion

				#endregion

				if (Setting == null)
					Setting = new clsSetting(thisDbType, localRecords.dbConnection);

				AccountOrderNumber = thisItem.my_Order_OrderNum(counter);

				Reference =
					"~" + AccountOrderNumber;

				InvoiceDate = DateTime.Now.ToShortDateString();
				DueDate = Convert.ToDateTime(thisItem.my_Order_DateDue(counter)).ToShortDateString();

				SubTotal = "";
				TotalTax = "";
				Total = "";

				Description = thisItem.my_ShortDescription(counter);

				//thisPropertyId = thisItem.my_Order_PropertyId(counter);

				thisProdcutId = thisItem.my_ProductId(counter);

				dQuantity = thisItem.my_Quantity(counter);
				Quantity = String.Format("{0:0.00}", dQuantity);

				//NumPurchaseInstances = thisItem.my_NumPurchaseInstances(counter);

				//if (thisProdcutId == 1)
				//{
				//	if (NumPurchaseInstances == 1)
				//		Description += "; 1 Transaction";
				//	else
				//		Description += "; " + NumPurchaseInstances.ToString() + " Transactions";

				//}

				//ProductChargingFrequencyType = thisItem.my_Order_ProductChargingFrequencyType(counter);

				UnitAmount = String.Format("{0:0.00}", thisItem.my_Order_TotalItemCostExcludingTax(counter) / dQuantity);

				AccountCode = "";
				TaxType = @"15% GST on Income";


				//thisChargeType = (productChargingMechanismType)thisItem.my_Order_ProductChargingMechanismType(counter);

				bool DDAccountNumberPresent = false;
				bool CCAccountNumberPresent = false;

				Currency = "NZD";
				BrandingTheme = "";

				//					thisCsvWriter[(int) thisChargeType].WriteFields(

				thisCsvWriter.WriteFields(
					ContactName,
					EmailAddress,
					BillingAddressLine1,
					BillingAddressLine2,
					BillingAddressLine3,
					BillingAddressLine4,
					POCity,
					PORegion,
					POPostalCode,
					POCountry,
					InvoiceNumber,
					Reference,
					InvoiceDate,
					DueDate,
					SubTotal,
					TotalTax,
					Total,
					Description,
					Quantity,
					UnitAmount,
					AccountCode,
					TaxType,
					TaxAmount,
					TrackingName1,
					TrackingOption1,
					TrackingName2,
					TrackingOption2,
					Currency,
					BrandingTheme
					);

			}


            thisCsvWriter.Close();


            return numOrderItems;

        }

        #endregion

        #region ProcessOrder

        /// <summary>
        /// Function that allows processing of an order
        /// </summary>
        /// <param name="OrderId">Id of Order</param>
        /// <param name="DateProcessed">Date Processed</param>
        public void ProcessOrder(int OrderId, string DateProcessed)
		{
			int OrderPaid = 1;


			SetAttribute(OrderId, "OrderPaid", OrderPaid.ToString());
			AddAttributeToSet(OrderId, "DateProcessed", localRecords.DBDateTime(Convert.ToDateTime(DateProcessed)));
			AddAttributeToSet(OrderId, "DateProcessedUtc",localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(Convert.ToDateTime(DateProcessed)))));
		}
		
		#endregion

		#region SetOrderStatus

		/// <summary>
		/// Function that allows processing of an order
		/// </summary>
		/// <param name="OrderId">Id of Order</param>
		/// <param name="OrderStatusId">New Status of Order</param>
		public void SetOrderStatus(int OrderId, int OrderStatusId)
		{
			SetAttribute(OrderId, "OrderStatusId", OrderStatusId.ToString());
		}

		#endregion

		#region AddEasy

		/// <summary>AddEasy</summary>
		/// <param name="PersonId">Person Associated with this Order (if any)</param>
		/// <param name="CustomerId">Customer Associated with this Order</param>
		/// <param name="OrderNum">Customer's Order Number</param>
		/// <param name="OrderStatusId">Order Status</param>
		/// <param name="OrderCreatedMechanism">Mechanism by which this Order was created</param>
		/// <param name="SupplierComment">Supplier Comment</param>
		public void AddEasy(int PersonId,
			int CustomerId,
			string OrderNum,
			int OrderCreatedMechanism,
			int OrderStatusId,
			string SupplierComment)
		{

			DateTime thisUTCDateTime = DateTime.UtcNow;

			clsPerson Person = new clsPerson(thisDbType, localRecords.dbConnection);

			int numPeople = Person.GetByPersonId(PersonId);

            #region Default Data

            int CustomerGroupId = -1;
			int CustomerType = -1;
			string FullName = "";
			string Title = "";
			string FirstName = "";
			string LastName = "";
			string QuickPostalAddress = "";
			string QuickDaytimePhone = "";
			string QuickDaytimeFax = "";
			string QuickAfterHoursPhone = "";
			string QuickAfterHoursFax = "";
			string QuickMobilePhone = "";
			int CountryId = -1;
			string Email = "";

			int PaymentMethodTypeId = paymentMethodType_asYetUndetermined();
			int OrderPaid = 0;
			int OrderSubmitted = 0;

			string DateCreated = localRecords.DBDateTime(FromUtcToClientTime(thisUTCDateTime));
			string DateCreatedUtc = localRecords.DBDateTime(thisUTCDateTime);
			string DateSubmitted = "";
			string DateSubmittedUtc = "";
			string DateProcessed = "";
			string DateProcessedUtc = "";
			string DateShipped = "";
			string DateShippedUtc = "";
			string DateDue = "";
			int InvoiceRequested = 0;
			string DateInvoiceLastPrinted = "";
			decimal TaxRateAtTimeOfOrder = localTaxRate - 1;
			decimal TaxAppliedToOrder = 0;

			decimal Total = 0;
			decimal FreightCost = 0;
			decimal TaxCost = 0;
			decimal TotalItemWeight = 0;
			decimal TotalItemCost = 0;
			decimal TotalItemFreightCost = 0;
			int IsInvoiceOrder = 0;
			int NumItems = 0;

			#endregion

			if (numPeople != 0)
			{
				CustomerId = Person.my_CustomerId(0);
				CustomerGroupId = Person.my_Customer_CustomerGroupId(0);
				CustomerType = Person.my_Customer_CustomerType(0);
				FullName = Person.my_Customer_FullName(0);
				Title = Person.my_Title(0);
				FirstName = Person.my_FirstName(0);
				LastName = Person.my_LastName(0);
				QuickPostalAddress = Person.my_QuickPostalAddress(0);
				QuickDaytimePhone = Person.my_QuickDaytimePhone(0);
				QuickDaytimeFax = Person.my_QuickDaytimeFax(0);
				QuickAfterHoursPhone = Person.my_QuickAfterHoursPhone(0);
				QuickAfterHoursFax = Person.my_QuickAfterHoursFax(0);
				QuickMobilePhone = Person.my_QuickMobilePhone(0);
				CountryId = Person.my_Customer_CountryId(0);
				Email = Person.my_Email(0);
			}
			else
			{
				PersonId = 0;
				CustomerGroupId = publicCustomerGroupId;
				CustomerType = 0;
				FullName = "";
				Title = "";
				FirstName = "";
				LastName = "";
				QuickPostalAddress = "";
				QuickDaytimePhone = "";
				QuickDaytimeFax = "";
				QuickAfterHoursPhone = "";
				QuickAfterHoursFax = "";
				QuickMobilePhone = "";
				CountryId = assumedCountryId;
				Email = "";
			}

			Add(
		        CustomerId,

		        PersonId,
		        PaymentMethodTypeId,
		        CustomerGroupId,

		        OrderNum,

		        CustomerType,

		        FullName,
		        Title,
		        FirstName,
		        LastName,
		        QuickPostalAddress,

		        QuickDaytimePhone,
		        QuickDaytimeFax,

		        QuickAfterHoursPhone,
		        QuickAfterHoursFax,
		        QuickMobilePhone,

		        CountryId,

		        Email,

		        OrderSubmitted,

		        OrderPaid,

		        OrderCreatedMechanism,

		        OrderStatusId,

		        SupplierComment,

		        DateCreated,
		        DateCreatedUtc,
		        DateSubmitted,
		        DateSubmittedUtc,
		        DateProcessed,
		        DateProcessedUtc,
		        DateShipped,
		        DateShippedUtc,
		        DateDue,

		        InvoiceRequested,

		        DateInvoiceLastPrinted,

		        TaxAppliedToOrder,
		        TaxRateAtTimeOfOrder,
		        TaxCost,
		        FreightCost,
		        Total,
		        TotalItemWeight,
		        TotalItemCost,
		        TotalItemFreightCost,

		        IsInvoiceOrder,
		        NumItems
		   );

			//rowToAdd["PaymentMethodTypeId"] = paymentMethodType_asYetUndetermined();
			//rowToAdd["OrderPaid"] = 0;
			//rowToAdd["OrderCreatedMechanism"] = OrderCreatedMechanism;
			//rowToAdd["OrderSubmitted"] = 0;
			//rowToAdd["OrderStatusId"] = OrderStatusId;
			//rowToAdd["SupplierComment"] = SupplierComment;
			//rowToAdd["DateCreated"] = localRecords.DBDateTime(FromUtcToClientTime(thisUTCDateTime));
			//rowToAdd["DateCreatedUtc"] = localRecords.DBDateTime(thisUTCDateTime);
			//rowToAdd["DateSubmitted"] = DBNull.Value;
			//rowToAdd["DateSubmittedUtc"] = DBNull.Value;
			//rowToAdd["DateProcessed"] = DBNull.Value;
			//rowToAdd["DateProcessedUtc"] = DBNull.Value;
			//rowToAdd["DateShipped"] = DBNull.Value;
			//rowToAdd["DateShippedUtc"] = DBNull.Value;
			//rowToAdd["DateDue"] = DBNull.Value;
			//rowToAdd["InvoiceRequested"] = 0;
			//rowToAdd["DateInvoiceLastPrinted"] = DBNull.Value;
			//rowToAdd["TaxRateAtTimeOfOrder"] = localTaxRate - 1;
			//rowToAdd["TaxAppliedToOrder"] = DBNull.Value;

			//rowToAdd["Total"] = 0;
			//rowToAdd["FreightCost"] = DBNull.Value;
			//rowToAdd["TaxCost"] = DBNull.Value;

			////Validate the data supplied
			//Validate(rowToAdd, true);

			//if (NumErrors() == 0)
			//{
			//	newDataToAdd.Rows.Add(rowToAdd);
			//}

		}

		#endregion

		#region ModifyEasy

		/// <summary>ModifyEasy</summary>
		/// <param name="OrderId">OrderId (Primary Key of Record)</param>
		/// <param name="PersonId">Person Associated with this Order</param>
		/// <param name="OrderNum">Customer's Order Number</param>
		/// <param name="OrderStatusId">Supplier Status</param>
		/// <param name="SupplierComment">Supplier Comment</param>
		public void ModifyEasy(int OrderId,
			int PersonId,
			string OrderNum,
			int OrderStatusId,
			string SupplierComment)
		{

			DateTime thisUTCDateTime = DateTime.UtcNow;
			////Reinitialise the Error and Warning Count
			//ResetWarningAndErrorTables();

			////Validate the data supplied
			//System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			clsPerson Person = new clsPerson(thisDbType, localRecords.dbConnection);

			int numPeople = Person.GetByPersonId(PersonId);

			clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);
			thisOrder.GetByOrderId(OrderId);

			#region Default Data

			int CustomerId = -1;
			int CustomerGroupId = -1;
			int CustomerType = -1;
			string FullName = "";
			string Title = "";
			string FirstName = "";
			string LastName = "";
			string QuickPostalAddress = "";
			string QuickDaytimePhone = "";
			string QuickDaytimeFax = "";
			string QuickAfterHoursPhone = "";
			string QuickAfterHoursFax = "";
			string QuickMobilePhone = "";
			int CountryId = -1;
			string Email = "";

			int PaymentMethodTypeId = paymentMethodType_asYetUndetermined();
			int OrderPaid = 0;
			int OrderCreatedMechanism = orderCreatedMechanism_byVendorAutomatically();
			int OrderSubmitted = 0;

			string DateCreated = localRecords.DBDateTime(FromUtcToClientTime(thisUTCDateTime));
			string DateCreatedUtc = localRecords.DBDateTime(thisUTCDateTime);
			string DateSubmitted = "";
			string DateSubmittedUtc = "";
			string DateProcessed = "";
			string DateProcessedUtc = "";
			string DateShipped = "";
			string DateShippedUtc = "";
			string DateDue = "";
			int InvoiceRequested = 0;
			string DateInvoiceLastPrinted = "";
			decimal TaxRateAtTimeOfOrder = localTaxRate - 1;
			decimal TaxAppliedToOrder = 0;

			decimal Total = 0;
			decimal FreightCost = 0;
			decimal TaxCost = 0;
			decimal TotalItemWeight = 0;
			decimal TotalItemCost = 0;
			decimal TotalItemFreightCost = 0;
			int IsInvoiceOrder = 0;
			int NumItems = 0;

			#endregion


			if (numPeople != 0)
			{
				if (PersonId != thisOrder.my_PersonId(0))
				{
					CustomerId = Person.my_CustomerId(0);
					CustomerGroupId = Person.my_Customer_CustomerGroupId(0);
					CustomerType = Person.my_Customer_CustomerType(0);
					FullName = Person.my_Customer_FullName(0);
					Title = Person.my_Title(0);
					FirstName = Person.my_FirstName(0);
					LastName = Person.my_LastName(0);
					QuickPostalAddress = Person.my_QuickPostalAddress(0);
					QuickDaytimePhone = Person.my_QuickDaytimePhone(0);
					QuickDaytimeFax = Person.my_QuickDaytimeFax(0);
					QuickAfterHoursPhone = Person.my_QuickAfterHoursPhone(0);
					QuickAfterHoursFax = Person.my_QuickAfterHoursFax(0);
					QuickMobilePhone = Person.my_QuickMobilePhone(0);
					CountryId = Person.my_Customer_CountryId(0);
					Email = Person.my_Email(0);
				}
				else
				{
					CustomerId = thisOrder.my_CustomerId(0);
					CustomerGroupId = thisOrder.my_Customer_CustomerGroupId(0);
					CustomerType = thisOrder.my_Customer_CustomerType(0);
					FullName = thisOrder.my_FullName(0);
					Title = thisOrder.my_Title(0);
					FirstName = thisOrder.my_FirstName(0);
					LastName = thisOrder.my_LastName(0);
					QuickPostalAddress = thisOrder.my_QuickPostalAddress(0);
					QuickDaytimePhone = thisOrder.my_QuickDaytimePhone(0);
					QuickDaytimeFax = thisOrder.my_QuickDaytimeFax(0);
					QuickAfterHoursPhone = thisOrder.my_QuickAfterHoursPhone(0);
					QuickAfterHoursFax = thisOrder.my_QuickAfterHoursFax(0);
					QuickMobilePhone = thisOrder.my_QuickMobilePhone(0);
					CountryId = thisOrder.my_Customer_CountryId(0);
					Email = thisOrder.my_Email(0);
				}
			}
			else
			{
				CustomerId = 0;
				CustomerGroupId = publicCustomerGroupId;
				CustomerType = 0;
				FullName = "";
				Title = "";
				FirstName = "";
				LastName = "";
				QuickPostalAddress = "";
				QuickDaytimePhone = "";
				QuickDaytimeFax = "";
				QuickAfterHoursPhone = "";
				QuickAfterHoursFax = "";
				QuickMobilePhone = "";
				CountryId = assumedCountryId;
				Email = "";
			}


			//Rest is copied form the old order
			DateCreated = thisOrder.my_DateCreated(0);
			DateCreatedUtc = thisOrder.my_DateCreatedUtc(0);

			DateProcessed = thisOrder.my_DateProcessed(0);
			DateProcessedUtc = thisOrder.my_DateProcessedUtc(0);

			DateShipped = thisOrder.my_DateProcessed(0);
			DateShippedUtc = thisOrder.my_DateProcessed(0);

			DateSubmitted = thisOrder.my_DateSubmitted(0);
			DateSubmittedUtc = thisOrder.my_DateSubmittedUtc(0);

			DateDue = thisOrder.my_DateDue(0);

			InvoiceRequested = thisOrder.my_InvoiceRequested(0);
			DateInvoiceLastPrinted = thisOrder.my_DateInvoiceLastPrinted(0);

			OrderSubmitted = 0;
			PaymentMethodTypeId = thisOrder.my_PaymentMethodTypeId(0);
			OrderPaid = thisOrder.my_OrderPaid(0);
			OrderCreatedMechanism = thisOrder.my_OrderCreatedMechanism(0);
			OrderStatusId = thisOrder.my_OrderStatusId(0);
			SupplierComment = thisOrder.my_SupplierComment(0);
			OrderNum = thisOrder.my_OrderNum(0);
			TaxRateAtTimeOfOrder = thisOrder.my_TaxRateAtTimeOfOrder(0);
			TaxAppliedToOrder = thisOrder.my_TaxAppliedToOrder(0);

			//Reset Costs
			Total = 0;
			FreightCost = 0;
			TaxCost = 0;

			Modify(
			   OrderId,
			   CustomerId,

			   PersonId,
			   PaymentMethodTypeId,
			   CustomerGroupId,

			   OrderNum,

			   CustomerType,

			   FullName,
			   Title,
			   FirstName,
			   LastName,
			   QuickPostalAddress,

			   QuickDaytimePhone,
			   QuickDaytimeFax,

			   QuickAfterHoursPhone,
			   QuickAfterHoursFax,
			   QuickMobilePhone,

			   CountryId,

			   Email,

			   OrderSubmitted,

			   OrderPaid,

			   OrderCreatedMechanism,

			   OrderStatusId,

			   SupplierComment,

			   DateCreated,
			   DateCreatedUtc,
			   DateSubmitted,
			   DateSubmittedUtc,
			   DateProcessed,
			   DateProcessedUtc,
			   DateShipped,
			   DateShippedUtc,
			   DateDue,

			   InvoiceRequested,

			   DateInvoiceLastPrinted,

			   TaxAppliedToOrder,
			   TaxRateAtTimeOfOrder,
			   TaxCost,
			   FreightCost,
			   Total,
			   TotalItemWeight,
			   TotalItemCost,
			   TotalItemFreightCost,

			   IsInvoiceOrder,
			   NumItems
			   );

			//if (numPeople != 0)
			//{
			//	if (PersonId != thisOrder.my_PersonId(0))
			//	{
			//		rowToAdd["CustomerId"] = Person.my_CustomerId(0);
			//		rowToAdd["CustomerGroupId"] = Person.my_Customer_CustomerGroupId(0);
			//		rowToAdd["CustomerType"] = Person.my_Customer_CustomerType(0);
			//		rowToAdd["FullName"] = Person.my_Customer_FullName(0);
			//		rowToAdd["Title"] = Person.my_Title(0);
			//		rowToAdd["FirstName"] = Person.my_FirstName(0);
			//		rowToAdd["LastName"] = Person.my_LastName(0);
			//		rowToAdd["QuickPostalAddress"] = Person.my_QuickPostalAddress(0);
			//		rowToAdd["QuickDaytimePhone"] = Person.my_QuickDaytimePhone(0);
			//		rowToAdd["QuickDaytimeFax"] = Person.my_QuickDaytimeFax(0);
			//		rowToAdd["QuickAfterHoursPhone"] = Person.my_QuickAfterHoursPhone(0);
			//		rowToAdd["QuickAfterHoursFax"] = Person.my_QuickAfterHoursFax(0);
			//		rowToAdd["QuickMobilePhone"] = Person.my_QuickMobilePhone(0);
			//		rowToAdd["CountryId"] = Person.my_Customer_CountryId(0);
			//		rowToAdd["Email"] = Person.my_Email(0);
			//	}
			//	else
			//	{
			//		rowToAdd["CustomerId"] = thisOrder.my_CustomerId(0);
			//		rowToAdd["CustomerGroupId"] = thisOrder.my_Customer_CustomerGroupId(0);
			//		rowToAdd["CustomerType"] = thisOrder.my_Customer_CustomerType(0);
			//		rowToAdd["FullName"] = thisOrder.my_FullName(0);
			//		rowToAdd["Title"] = thisOrder.my_Title(0);
			//		rowToAdd["FirstName"] = thisOrder.my_FirstName(0);
			//		rowToAdd["LastName"] = thisOrder.my_LastName(0);
			//		rowToAdd["QuickPostalAddress"] = thisOrder.my_QuickPostalAddress(0);
			//		rowToAdd["QuickDaytimePhone"] = thisOrder.my_QuickDaytimePhone(0);
			//		rowToAdd["QuickDaytimeFax"] = thisOrder.my_QuickDaytimeFax(0);
			//		rowToAdd["QuickAfterHoursPhone"] = thisOrder.my_QuickAfterHoursPhone(0);
			//		rowToAdd["QuickAfterHoursFax"] = thisOrder.my_QuickAfterHoursFax(0);
			//		rowToAdd["QuickMobilePhone"] = thisOrder.my_QuickMobilePhone(0);
			//		rowToAdd["CountryId"] = thisOrder.my_Customer_CountryId(0);
			//		rowToAdd["Email"] = thisOrder.my_Email(0);
			//	}
			//}
			//else
			//{
			//	rowToAdd["CustomerId"] = DBNull.Value;
			//	rowToAdd["CustomerGroupId"] = publicCustomerGroupId;
			//	rowToAdd["CustomerType"] = 0;
			//	rowToAdd["FullName"] = "";
			//	rowToAdd["Title"] = "";
			//	rowToAdd["FirstName"] = "";
			//	rowToAdd["LastName"] = "";
			//	rowToAdd["QuickPostalAddress"] = "";
			//	rowToAdd["QuickDaytimePhone"] = "";
			//	rowToAdd["QuickDaytimeFax"] = "";
			//	rowToAdd["QuickAfterHoursPhone"] = "";
			//	rowToAdd["QuickAfterHoursFax"] = "";
			//	rowToAdd["QuickMobilePhone"] = "";
			//	rowToAdd["CountryId"] = assumedCountryId;
			//	rowToAdd["Email"] = "";
			//}



			//rowToAdd["PersonId"] = PersonId;
			//rowToAdd["OrderNum"] = OrderNum;
			//rowToAdd["OrderStatusId"] = OrderStatusId;
			//rowToAdd["SupplierComment"] = SupplierComment;

			////Rest is copied form the old order
			//rowToAdd["DateCreated"] = SanitiseDate(thisOrder.my_DateCreated(0));
			//rowToAdd["DateCreatedUtc"] =  SanitiseDate(thisOrder.my_DateCreatedUtc(0));

			//rowToAdd["DateProcessed"] = SanitiseDate(thisOrder.my_DateProcessed(0));
			//rowToAdd["DateProcessedUtc"] =  SanitiseDate(thisOrder.my_DateProcessedUtc(0));

			//rowToAdd["DateShipped"] = SanitiseDate(thisOrder.my_DateProcessed(0));
			//rowToAdd["DateShippedUtc"] =  SanitiseDate(thisOrder.my_DateProcessed(0));

			//rowToAdd["DateSubmitted"] = SanitiseDate(thisOrder.my_DateSubmitted(0));
			//rowToAdd["DateSubmittedUtc"] =  SanitiseDate(thisOrder.my_DateSubmittedUtc(0));

			//rowToAdd["DateDue"] = SanitiseDate(thisOrder.my_DateDue(0));

			//rowToAdd["InvoiceRequested"] = thisOrder.my_InvoiceRequested(0);
			//rowToAdd["DateInvoiceLastPrinted"] = SanitiseDate(thisOrder.my_DateInvoiceLastPrinted(0));

			//rowToAdd["OrderSubmitted"] = 0;
			//rowToAdd["PaymentMethodTypeId"] = thisOrder.my_PaymentMethodTypeId(0);
			//rowToAdd["OrderPaid"] = thisOrder.my_OrderPaid(0);
			//rowToAdd["OrderCreatedMechanism"] = thisOrder.my_OrderCreatedMechanism(0);
			//rowToAdd["OrderStatusId"] = thisOrder.my_OrderStatusId(0);
			//rowToAdd["SupplierComment"] = thisOrder.my_SupplierComment(0);
			//rowToAdd["OrderNum"] = thisOrder.my_OrderNum(0);
			//rowToAdd["TaxRateAtTimeOfOrder"] = thisOrder.my_TaxRateAtTimeOfOrder(0);
			//rowToAdd["TaxAppliedToOrder"] = thisOrder.my_TaxAppliedToOrder(0);			

			////Reset Costs
			//rowToAdd["Total"] = DBNull.Value;
			//rowToAdd["FreightCost"] = DBNull.Value;
			//rowToAdd["TaxCost"] = DBNull.Value;

			//Validate(rowToAdd, false);

			//if (NumErrors() == 0)
			//{
			//	if (UserChanges(rowToAdd))
			//		dataToBeModified.Rows.Add(rowToAdd);
			//}

		}

		#endregion

		#region Add/Modify/Validate

		#region Add

		/// <summary>Add</summary>
		/// <param name="CustomerId">CustomerId</param>
		/// <param name="PersonId">PersonId</param>
		/// <param name="PaymentMethodTypeId">PaymentMethodTypeId</param>
		/// <param name="CustomerGroupId">CustomerGroupId</param>
		/// <param name="OrderNum">OrderNum</param>
		/// <param name="CustomerType">CustomerType</param>
		/// <param name="FullName">FullName</param>
		/// <param name="Title">Title</param>
		/// <param name="FirstName">FirstName</param>
		/// <param name="LastName">LastName</param>
		/// <param name="QuickPostalAddress">QuickPostalAddress</param>
		/// <param name="QuickDaytimePhone">QuickDaytimePhone</param>
		/// <param name="QuickDaytimeFax">QuickDaytimeFax</param>
		/// <param name="QuickAfterHoursPhone">QuickAfterHoursPhone</param>
		/// <param name="QuickAfterHoursFax">QuickAfterHoursFax</param>
		/// <param name="QuickMobilePhone">QuickMobilePhone</param>
		/// <param name="CountryId">CountryId</param>
		/// <param name="Email">Email</param>
		/// <param name="OrderSubmitted">OrderSubmitted</param>
		/// <param name="OrderPaid">OrderPaid</param>
		/// <param name="OrderCreatedMechanism">OrderCreatedMechanism</param>
		/// <param name="OrderStatusId">OrderStatusId</param>
		/// <param name="SupplierComment">SupplierComment</param>
		/// <param name="DateCreated">DateCreated</param>
		/// <param name="DateCreatedUtc">DateCreatedUtc</param>
		/// <param name="DateSubmitted">DateSubmitted</param>
		/// <param name="DateSubmittedUtc">DateSubmittedUtc</param>
		/// <param name="DateProcessed">DateProcessed</param>
		/// <param name="DateProcessedUtc">DateProcessedUtc</param>
		/// <param name="DateShipped">DateShipped</param>
		/// <param name="DateShippedUtc">DateShippedUtc</param>
		/// <param name="DateDue">DateDue</param>
		/// <param name="InvoiceRequested">InvoiceRequested</param>
		/// <param name="DateInvoiceLastPrinted">DateInvoiceLastPrinted</param>
		/// <param name="TaxAppliedToOrder">TaxAppliedToOrder</param>
		/// <param name="TaxRateAtTimeOfOrder">TaxRateAtTimeOfOrder</param>
		/// <param name="TaxCost">TaxCost</param>
		/// <param name="FreightCost">FreightCost</param>
		/// <param name="Total">Total</param>
		/// <param name="TotalItemWeight">TotalItemWeight</param>
		/// <param name="TotalItemCost">TotalItemCost</param>
		/// <param name="TotalItemFreightCost">TotalItemFreightCost</param>
		/// <param name="IsInvoiceOrder">IsInvoiceOrder</param>
		/// <param name="NumItems">NumItems</param>
		public void Add(
		   int CustomerId,

		   int PersonId,
		   int PaymentMethodTypeId,
		   int CustomerGroupId,

		   string OrderNum,

		   int CustomerType,

		   string FullName,
		   string Title,
		   string FirstName,
		   string LastName,
		   string QuickPostalAddress,

		   string QuickDaytimePhone,
		   string QuickDaytimeFax,

		   string QuickAfterHoursPhone,
		   string QuickAfterHoursFax,
		   string QuickMobilePhone,

		   int CountryId,

		   string Email,

		   int OrderSubmitted,

		   int OrderPaid,

		   int OrderCreatedMechanism,

		   int OrderStatusId,

		   string SupplierComment,

		   string DateCreated,
		   string DateCreatedUtc,
		   string DateSubmitted,
		   string DateSubmittedUtc,
		   string DateProcessed,
		   string DateProcessedUtc,
		   string DateShipped,
		   string DateShippedUtc,
		   string DateDue,

		   int InvoiceRequested,

		   string DateInvoiceLastPrinted,

		   decimal TaxAppliedToOrder,
		   decimal TaxRateAtTimeOfOrder,
		   decimal TaxCost,
		   decimal FreightCost,
		   decimal Total,
		   decimal TotalItemWeight,
		   decimal TotalItemCost,
		   decimal TotalItemFreightCost,

		   int IsInvoiceOrder,
		   int NumItems
		   )
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();


			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["PersonId"] = PersonId;
			rowToAdd["PaymentMethodTypeId"] = PaymentMethodTypeId;
			rowToAdd["CustomerGroupId"] = CustomerGroupId;

			rowToAdd["OrderNum"] = OrderNum;

			rowToAdd["CustomerType"] = CustomerType;

			rowToAdd["FullName"] = FullName;
			rowToAdd["Title"] = Title;
			rowToAdd["FirstName"] = FirstName;
			rowToAdd["LastName"] = LastName;
			rowToAdd["QuickPostalAddress"] = QuickPostalAddress;

			rowToAdd["QuickDaytimePhone"] = SanitiseDate(QuickDaytimePhone);
			rowToAdd["QuickDaytimeFax"] = SanitiseDate(QuickDaytimeFax);

			rowToAdd["QuickAfterHoursPhone"] = QuickAfterHoursPhone;
			rowToAdd["QuickAfterHoursFax"] = QuickAfterHoursFax;
			rowToAdd["QuickMobilePhone"] = QuickMobilePhone;

			rowToAdd["CountryId"] = CountryId;

			rowToAdd["Email"] = Email;

			rowToAdd["OrderSubmitted"] = OrderSubmitted;

			rowToAdd["OrderPaid"] = OrderPaid;

			rowToAdd["OrderCreatedMechanism"] = OrderCreatedMechanism;

			rowToAdd["OrderStatusId"] = OrderStatusId;

			rowToAdd["SupplierComment"] = SupplierComment;

			rowToAdd["DateCreated"] = SanitiseDate(DateCreated);
			rowToAdd["DateCreatedUtc"] = SanitiseDate(DateCreatedUtc);
			rowToAdd["DateSubmitted"] = SanitiseDate(DateSubmitted);
			rowToAdd["DateSubmittedUtc"] = SanitiseDate(DateSubmittedUtc);
			rowToAdd["DateProcessed"] = SanitiseDate(DateProcessed);
			rowToAdd["DateProcessedUtc"] = SanitiseDate(DateProcessedUtc);
			rowToAdd["DateShipped"] = SanitiseDate(DateShipped);
			rowToAdd["DateShippedUtc"] = SanitiseDate(DateShippedUtc);
			rowToAdd["DateDue"] = SanitiseDate(DateDue);

			rowToAdd["InvoiceRequested"] = InvoiceRequested;

			rowToAdd["DateInvoiceLastPrinted"] = SanitiseDate(DateInvoiceLastPrinted);

			rowToAdd["TaxAppliedToOrder"] = TaxAppliedToOrder;
			rowToAdd["TaxRateAtTimeOfOrder"] = TaxRateAtTimeOfOrder;
			rowToAdd["TaxCost"] = TaxCost;
			rowToAdd["FreightCost"] = FreightCost;
			rowToAdd["Total"] = Total;
			rowToAdd["TotalItemWeight"] = TotalItemWeight;
			rowToAdd["TotalItemCost"] = TotalItemCost;
			rowToAdd["TotalItemFreightCost"] = TotalItemFreightCost;

			rowToAdd["IsInvoiceOrder"] = IsInvoiceOrder;
			rowToAdd["NumItems"] = NumItems;


			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				newDataToAdd.Rows.Add(rowToAdd);
			}

		}

		#endregion

		#region Modify

		/// <summary>Modify</summary>
		/// <param name="OrderId">OrderId (Primary Key of Record)</param>
		/// <param name="CustomerId">CustomerId</param>
		/// <param name="PersonId">PersonId</param>
		/// <param name="PaymentMethodTypeId">PaymentMethodTypeId</param>
		/// <param name="CustomerGroupId">CustomerGroupId</param>
		/// <param name="OrderNum">OrderNum</param>
		/// <param name="CustomerType">CustomerType</param>
		/// <param name="FullName">FullName</param>
		/// <param name="Title">Title</param>
		/// <param name="FirstName">FirstName</param>
		/// <param name="LastName">LastName</param>
		/// <param name="QuickPostalAddress">QuickPostalAddress</param>
		/// <param name="QuickDaytimePhone">QuickDaytimePhone</param>
		/// <param name="QuickDaytimeFax">QuickDaytimeFax</param>
		/// <param name="QuickAfterHoursPhone">QuickAfterHoursPhone</param>
		/// <param name="QuickAfterHoursFax">QuickAfterHoursFax</param>
		/// <param name="QuickMobilePhone">QuickMobilePhone</param>
		/// <param name="CountryId">CountryId</param>
		/// <param name="Email">Email</param>
		/// <param name="OrderSubmitted">OrderSubmitted</param>
		/// <param name="OrderPaid">OrderPaid</param>
		/// <param name="OrderCreatedMechanism">OrderCreatedMechanism</param>
		/// <param name="OrderStatusId">OrderStatusId</param>
		/// <param name="SupplierComment">SupplierComment</param>
		/// <param name="DateCreated">DateCreated</param>
		/// <param name="DateCreatedUtc">DateCreatedUtc</param>
		/// <param name="DateSubmitted">DateSubmitted</param>
		/// <param name="DateSubmittedUtc">DateSubmittedUtc</param>
		/// <param name="DateProcessed">DateProcessed</param>
		/// <param name="DateProcessedUtc">DateProcessedUtc</param>
		/// <param name="DateShipped">DateShipped</param>
		/// <param name="DateShippedUtc">DateShippedUtc</param>
		/// <param name="DateDue">DateDue</param>
		/// <param name="InvoiceRequested">InvoiceRequested</param>
		/// <param name="DateInvoiceLastPrinted">DateInvoiceLastPrinted</param>
		/// <param name="TaxAppliedToOrder">TaxAppliedToOrder</param>
		/// <param name="TaxRateAtTimeOfOrder">TaxRateAtTimeOfOrder</param>
		/// <param name="TaxCost">TaxCost</param>
		/// <param name="FreightCost">FreightCost</param>
		/// <param name="Total">Total</param>
		/// <param name="TotalItemWeight">TotalItemWeight</param>
		/// <param name="TotalItemCost">TotalItemCost</param>
		/// <param name="TotalItemFreightCost">TotalItemFreightCost</param>
		/// <param name="IsInvoiceOrder">IsInvoiceOrder</param>
		/// <param name="NumItems">NumItems</param>
		public void Modify(
		   int OrderId,
		   int CustomerId,

		   int PersonId,
		   int PaymentMethodTypeId,
		   int CustomerGroupId,

		   string OrderNum,

		   int CustomerType,

		   string FullName,
		   string Title,
		   string FirstName,
		   string LastName,
		   string QuickPostalAddress,

		   string QuickDaytimePhone,
		   string QuickDaytimeFax,

		   string QuickAfterHoursPhone,
		   string QuickAfterHoursFax,
		   string QuickMobilePhone,

		   int CountryId,

		   string Email,

		   int OrderSubmitted,

		   int OrderPaid,

		   int OrderCreatedMechanism,

		   int OrderStatusId,

		   string SupplierComment,

		   string DateCreated,
		   string DateCreatedUtc,
		   string DateSubmitted,
		   string DateSubmittedUtc,
		   string DateProcessed,
		   string DateProcessedUtc,
		   string DateShipped,
		   string DateShippedUtc,
		   string DateDue,

		   int InvoiceRequested,

		   string DateInvoiceLastPrinted,

		   decimal TaxAppliedToOrder,
		   decimal TaxRateAtTimeOfOrder,
		   decimal TaxCost,
		   decimal FreightCost,
		   decimal Total,
		   decimal TotalItemWeight,
		   decimal TotalItemCost,
		   decimal TotalItemFreightCost,

		   int IsInvoiceOrder,
		   int NumItems
		   )
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();


			rowToAdd["OrderId"] = OrderId;

			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["PersonId"] = PersonId;
			rowToAdd["PaymentMethodTypeId"] = PaymentMethodTypeId;
			rowToAdd["CustomerGroupId"] = CustomerGroupId;

			rowToAdd["OrderNum"] = OrderNum;

			rowToAdd["CustomerType"] = CustomerType;

			rowToAdd["FullName"] = FullName;
			rowToAdd["Title"] = Title;
			rowToAdd["FirstName"] = FirstName;
			rowToAdd["LastName"] = LastName;
			rowToAdd["QuickPostalAddress"] = QuickPostalAddress;

			rowToAdd["QuickDaytimePhone"] = SanitiseDate(QuickDaytimePhone);
			rowToAdd["QuickDaytimeFax"] = SanitiseDate(QuickDaytimeFax);

			rowToAdd["QuickAfterHoursPhone"] = QuickAfterHoursPhone;
			rowToAdd["QuickAfterHoursFax"] = QuickAfterHoursFax;
			rowToAdd["QuickMobilePhone"] = QuickMobilePhone;

			rowToAdd["CountryId"] = CountryId;

			rowToAdd["Email"] = Email;

			rowToAdd["OrderSubmitted"] = OrderSubmitted;

			rowToAdd["OrderPaid"] = OrderPaid;

			rowToAdd["OrderCreatedMechanism"] = OrderCreatedMechanism;

			rowToAdd["OrderStatusId"] = OrderStatusId;

			rowToAdd["SupplierComment"] = SupplierComment;

			rowToAdd["DateCreated"] = SanitiseDate(DateCreated);
			rowToAdd["DateCreatedUtc"] = SanitiseDate(DateCreatedUtc);
			rowToAdd["DateSubmitted"] = SanitiseDate(DateSubmitted);
			rowToAdd["DateSubmittedUtc"] = SanitiseDate(DateSubmittedUtc);
			rowToAdd["DateProcessed"] = SanitiseDate(DateProcessed);
			rowToAdd["DateProcessedUtc"] = SanitiseDate(DateProcessedUtc);
			rowToAdd["DateShipped"] = SanitiseDate(DateShipped);
			rowToAdd["DateShippedUtc"] = SanitiseDate(DateShippedUtc);
			rowToAdd["DateDue"] = SanitiseDate(DateDue);

			rowToAdd["InvoiceRequested"] = InvoiceRequested;

			rowToAdd["DateInvoiceLastPrinted"] = SanitiseDate(DateInvoiceLastPrinted);

			rowToAdd["TaxAppliedToOrder"] = TaxAppliedToOrder;
			rowToAdd["TaxRateAtTimeOfOrder"] = TaxRateAtTimeOfOrder;
			rowToAdd["TaxCost"] = TaxCost;
			rowToAdd["FreightCost"] = FreightCost;
			rowToAdd["Total"] = Total;
			rowToAdd["TotalItemWeight"] = TotalItemWeight;
			rowToAdd["TotalItemCost"] = TotalItemCost;
			rowToAdd["TotalItemFreightCost"] = TotalItemFreightCost;

			rowToAdd["IsInvoiceOrder"] = IsInvoiceOrder;
			rowToAdd["NumItems"] = NumItems;


			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);
			}

		}

		#endregion

		#region Validate

		/// <summary>Validate</summary>
		/// <param name="valuesToValidate">Values to be Validated</param>
		/// <param name="newRow">Indicates whether the Row being validated is new or already exists in the system</param>
		public void Validate(System.Data.DataRow valuesToValidate, bool newRow)
		{
		}

		#endregion

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
				int thisCustomer = 0;
				if(thisRow["CustomerId"] == DBNull.Value)
				{
					clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);
					thisOrder.GetByOrderId(Convert.ToInt32(thisRow["OrderId"]));
					thisCustomer = thisOrder.my_CustomerId(0);
				}
				else
					thisCustomer = Convert.ToInt32(thisRow["CustomerId"]);
				
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
				UpdateCustomerOrders(Convert.ToInt32(CustomerId));

			return retVal;
		}

		/// <summary>
		///Update Orders For a Customer
		/// </summary>
		/// <param name="CustomerId">CustomerId</param>
		public void UpdateCustomerOrders(int CustomerId)
		{
			clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);
			thisOrder.AddOrderByColumns("tblOrder.DateSubmitted");
			thisOrder.AddOrderByColumns("tblOrder.OrderId");
			int numOrders = thisOrder.GetByCustomerId(CustomerId);

			if (numOrders > 0)
			{
				DateTime DateFirstOrder = new DateTime(3000, 1, 1);
				DateTime DateFirstOrderUtc = new DateTime(3000, 1, 1);
				DateTime DateLastOrder = new DateTime(1, 1, 1);
				DateTime DateLastOrderUtc = new DateTime(1, 1, 1);
				decimal TotalItemCost = 0;
				decimal TotalTaxCost = 0;
				decimal TotalFreightCost = 0;

				for(int counter = 0; counter < numOrders; counter++)
				{

					#region Date Created - First and Last Orders
					if (thisOrder.my_DateCreated(counter) != "")
					{
						DateTime thisDateCreated = Convert.ToDateTime(thisOrder.my_DateCreated(counter));
						if (thisDateCreated < DateFirstOrder)
						{
							DateFirstOrder = thisDateCreated;
							DateFirstOrderUtc = Convert.ToDateTime(thisOrder.my_DateCreatedUtc(counter));
						}

						if (thisDateCreated > DateLastOrder)
						{
							DateLastOrder = thisDateCreated;
							DateLastOrderUtc = Convert.ToDateTime(thisOrder.my_DateCreatedUtc(counter));
						}
					}
					#endregion

					TotalItemCost += thisOrder.my_Total(counter);
					TotalTaxCost += thisOrder.my_TaxCost(counter);
					TotalFreightCost += thisOrder.my_FreightCostExcludingTax(counter);

				}

				#region Alter the Customer details, if we need to 

				bool ChangeInFirstOrderDate = false;
				bool ChangeInLastOrderDate = false;

				if (thisOrder.my_Customer_DateFirstOrder(0) == "")
				{
					if (DateFirstOrder != new DateTime(3000, 1, 1))
						ChangeInFirstOrderDate = true;
				}
				else
				{
					if (Convert.ToDateTime(thisOrder.my_Customer_DateFirstOrder(0)) != DateFirstOrder)
						ChangeInFirstOrderDate = true;
				}

				if (thisOrder.my_Customer_DateLastOrder(0) == "")
				{
					if (DateLastOrder != new DateTime(3000, 1, 1))
						ChangeInLastOrderDate = true;
				}
				else
				{
					if (Convert.ToDateTime(thisOrder.my_Customer_DateLastOrder(0)) != DateLastOrder)
						ChangeInLastOrderDate = true;
				}


				if (ChangeInFirstOrderDate
					|| ChangeInLastOrderDate
					|| thisOrder.my_Customer_TotalItemCost(0) != TotalItemCost
					|| thisOrder.my_Customer_TotalTaxCost(0) != TotalTaxCost
					|| thisOrder.my_Customer_TotalFreightCost(0) != TotalFreightCost
					|| thisOrder.my_Customer_NumOrders(0) != numOrders
					)
				{

					thisSqlQuery = "Update tblCustomer Set "
						+ "tblCustomer.TotalItemCost = " + TotalItemCost.ToString() + ", " + crLf
						+ "tblCustomer.TotalTaxCost = " + TotalTaxCost.ToString() + ", " + crLf
						+ "tblCustomer.TotalFreightCost = " + TotalFreightCost.ToString() + ", " + crLf
						+ "tblCustomer.NumOrders = " + numOrders.ToString() + crLf
						;

					if (DateFirstOrder != new DateTime(3000, 1, 1))
						thisSqlQuery += ", tblCustomer.DateFirstOrder = '" + localRecords.DBDateTime(DateFirstOrder) + "'" + crLf;

					if (DateFirstOrderUtc != new DateTime(3000, 1, 1))
						thisSqlQuery += ", tblCustomer.DateFirstOrderUtc = '" + localRecords.DBDateTime(DateFirstOrderUtc) + "'" + crLf;

					if (DateLastOrder != new DateTime(1, 1, 1))
						thisSqlQuery += ", tblCustomer.DateLastOrder = '" + localRecords.DBDateTime(DateLastOrder) + "'" + crLf;

					if (DateLastOrderUtc != new DateTime(1, 1, 1))
						thisSqlQuery += ", tblCustomer.DateLastOrderUtc = '" + localRecords.DBDateTime(DateLastOrderUtc) + "'" + crLf;

					thisSqlQuery += " Where CustomerId = " + CustomerId.ToString();

					localRecords.GetRecords(thisSqlQuery);
				}
	
				#endregion

			}
		}
		#endregion

		#region My_ Values Order

		/// <summary>OrderId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderId</returns>
		public int my_OrderId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "OrderId"));
		}


		/// <summary>CustomerId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CustomerId</returns>
		public int my_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerId"));
		}


		/// <summary>PersonId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>PersonId</returns>
		public int my_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PersonId"));
		}


		/// <summary>PaymentMethodTypeId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>PaymentMethodTypeId</returns>
		public int my_PaymentMethodTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PaymentMethodTypeId"));
		}


		/// <summary>CustomerGroupId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CustomerGroupId</returns>
		public int my_CustomerGroupId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerGroupId"));
		}


		/// <summary>OrderNum</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderNum</returns>
		public string my_OrderNum(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "OrderNum");
		}


		/// <summary>CustomerType</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CustomerType</returns>
		public int my_CustomerType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerType"));
		}


		/// <summary>FullName</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>FullName</returns>
		public string my_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FullName");
		}


		/// <summary>Title</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Title</returns>
		public string my_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Title");
		}


		/// <summary>FirstName</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>FirstName</returns>
		public string my_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FirstName");
		}


		/// <summary>LastName</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>LastName</returns>
		public string my_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "LastName");
		}


		/// <summary>QuickPostalAddress</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickPostalAddress</returns>
		public string my_QuickPostalAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickPostalAddress");
		}


		/// <summary>QuickDaytimePhone</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickDaytimePhone</returns>
		public string my_QuickDaytimePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickDaytimePhone");
		}


		/// <summary>QuickDaytimeFax</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickDaytimeFax</returns>
		public string my_QuickDaytimeFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickDaytimeFax");
		}


		/// <summary>QuickAfterHoursPhone</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickAfterHoursPhone</returns>
		public string my_QuickAfterHoursPhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickAfterHoursPhone");
		}


		/// <summary>QuickAfterHoursFax</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickAfterHoursFax</returns>
		public string my_QuickAfterHoursFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickAfterHoursFax");
		}


		/// <summary>QuickMobilePhone</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickMobilePhone</returns>
		public string my_QuickMobilePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickMobilePhone");
		}


		/// <summary>CountryId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CountryId</returns>
		public int my_CountryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CountryId"));
		}


		/// <summary>Email</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Email</returns>
		public string my_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Email");
		}


		/// <summary>OrderSubmitted</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderSubmitted</returns>
		public int my_OrderSubmitted(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "OrderSubmitted"));
		}


		/// <summary>OrderPaid</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderPaid</returns>
		public int my_OrderPaid(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "OrderPaid"));
		}


		/// <summary>OrderCreatedMechanism</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderCreatedMechanism</returns>
		public int my_OrderCreatedMechanism(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "OrderCreatedMechanism"));
		}


		/// <summary>OrderStatusId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderStatusId</returns>
		public int my_OrderStatusId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "OrderStatusId"));
		}


		/// <summary>SupplierComment</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>SupplierComment</returns>
		public string my_SupplierComment(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "SupplierComment");
		}


		/// <summary>DateCreated</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateCreated</returns>
		public string my_DateCreated(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateCreated");
		}


		/// <summary>DateCreatedUtc</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateCreatedUtc</returns>
		public string my_DateCreatedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateCreatedUtc");
		}


		/// <summary>DateSubmitted</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateSubmitted</returns>
		public string my_DateSubmitted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSubmitted");
		}


		/// <summary>DateSubmittedUtc</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateSubmittedUtc</returns>
		public string my_DateSubmittedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSubmittedUtc");
		}


		/// <summary>DateProcessed</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateProcessed</returns>
		public string my_DateProcessed(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateProcessed");
		}


		/// <summary>DateProcessedUtc</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateProcessedUtc</returns>
		public string my_DateProcessedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateProcessedUtc");
		}


		/// <summary>DateShipped</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateShipped</returns>
		public string my_DateShipped(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateShipped");
		}


		/// <summary>DateShippedUtc</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateShippedUtc</returns>
		public string my_DateShippedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateShippedUtc");
		}


		/// <summary>DateDue</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateDue</returns>
		public string my_DateDue(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateDue");
		}


		/// <summary>InvoiceRequested</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>InvoiceRequested</returns>
		public int my_InvoiceRequested(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "InvoiceRequested"));
		}


		/// <summary>DateInvoiceLastPrinted</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateInvoiceLastPrinted</returns>
		public string my_DateInvoiceLastPrinted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateInvoiceLastPrinted");
		}


		/// <summary>TaxAppliedToOrder</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TaxAppliedToOrder</returns>
		public decimal my_TaxAppliedToOrder(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TaxAppliedToOrder"));
		}


		/// <summary>TaxRateAtTimeOfOrder</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TaxRateAtTimeOfOrder</returns>
		public decimal my_TaxRateAtTimeOfOrder(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TaxRateAtTimeOfOrder"));
		}


		/// <summary>Total</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total</returns>
		public decimal my_Total(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Total"));
		}


		/// <summary>TotalItemWeight</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TotalItemWeight</returns>
		public decimal my_TotalItemWeight(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemWeight"));
		}



		/// <summary>TotalItemFreightCost</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TotalItemFreightCost</returns>
		public decimal my_TotalItemFreightCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemFreightCost"));
		}


		/// <summary>IsInvoiceOrder</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>IsInvoiceOrder</returns>
		public int my_IsInvoiceOrder(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsInvoiceOrder"));
		}


		/// <summary>NumItems</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>NumItems</returns>
		public int my_NumItems(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "NumItems"));
		}


		#endregion

		#region Calculated My_ Values Order

		/// <summary>
		/// Tax Cost of Order. Note this is negative if the system setting is to show
		/// Prices including tax, but the customer is from a country for which 
		/// the local Tax does not apply.
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Tax Cost of this Order</returns>
		public decimal my_TaxCost(int rowNum)
		{
			decimal Tax = Convert.ToDecimal(localRecords.FieldByName(rowNum, "TaxCost"));

			if (priceShownIncludesLocalTaxRate && Tax == Convert.ToDecimal(0))
				return (Convert.ToDecimal(localRecords.FieldByName(rowNum, "Total")) 
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightCost")))
					* Convert.ToDecimal(Convert.ToDecimal(1) - (Convert.ToDecimal(localTaxRate)));
			else
				return Tax;
		}
				
		/// <summary>
		/// Freight Cost of Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>FreightCost of this Order</returns>
		public decimal my_FreightCost(int rowNum)
		{
			decimal baseCost = Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightCost"));
			
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
		public decimal my_FreightCostExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightCost"));
		}

		/// <summary>
		/// Total Freight Cost of all Items in this Order Assuming Freight Charged per Item (Excluding Tax)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Freight Cost of all Items in this Order (Excluding Tax)</returns>
		public decimal my_TotalItemFreightCostExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemFreightCost"));
		}


		/// <summary>
		/// Total Cost of all Items in this Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Cost of all Items in this Order</returns>
		public decimal my_TotalItemCost(int rowNum)
		{
			decimal baseCost = Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemCost"));
			
			if (priceShownIncludesLocalTaxRate)
				return baseCost * localTaxRate;
			else
				return baseCost;
		}


		/// <summary>
		/// Total Cost of all Invoices in this Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Cost of all Invoices in this Order</returns>
		public decimal my_TotalInvoiceCost(int rowNum)
		{
			decimal baseCost = Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemCost"));
			return baseCost;
		}

		/// <summary>TotalItemCostExcludingTax</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TotalItemCostExcludingTax</returns>
		public decimal my_TotalItemCostExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemCost"));
		}

		/// <summary>
		/// Grand Total Cost of Cost for Items, Freight and Tax
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Grand Total Cost of Cost for Items, Freight and Tax for this Order</returns>
		public decimal my_GrandTotal(int rowNum)
		{
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightCost"))
					;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "TotalItemCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "TaxCost"));

		}


		#endregion

		#region My_ Values OrderStatus

		/// <summary>
		/// <see cref="clsOrderStatus.my_SupplierFacingName">SupplierFacingName</see> of 
		/// <see cref="clsOrderStatus">OrderStatus</see></summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsOrderStatus.my_SupplierFacingName">SupplierFacingName</see> 
		/// of Associated <see cref="clsOrderStatus">OrderStatus</see> 
		/// for this Order</returns>
		public string my_OrderStatus_SupplierFacingName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "OrderStatus_SupplierFacingName");
		}


		/// <summary>
		/// <see cref="clsOrderStatus.my_CustomerFacingName">CustomerFacingName</see> of 
		/// <see cref="clsOrderStatus">OrderStatus</see></summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsOrderStatus.my_CustomerFacingName">CustomerFacingName</see> 
		/// of Associated <see cref="clsOrderStatus">OrderStatus</see> 
		/// for this Order</returns>
		public string my_OrderStatus_CustomerFacingName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "OrderStatus_CustomerFacingName");
		}

		/// <summary>
		/// <see cref="clsOrderStatus.my_SupplierFacingDescription">SupplierFacingDescription</see> of 
		/// <see cref="clsOrderStatus">OrderStatus</see></summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsOrderStatus.my_SupplierFacingDescription">SupplierFacingDescription</see> 
		/// of Associated <see cref="clsOrderStatus">OrderStatus</see> 
		/// for this Order</returns>
		public string my_OrderStatus_SupplierFacingDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "OrderStatus_SupplierFacingDescription");
		}

		/// <summary>
		/// <see cref="clsOrderStatus.my_CustomerFacingDescription">CustomerFacingDescription</see> of 
		/// <see cref="clsOrderStatus">OrderStatus</see></summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsOrderStatus.my_CustomerFacingDescription">CustomerFacingDescription</see> 
		/// of Associated <see cref="clsOrderStatus">OrderStatus</see> 
		/// for this Order</returns>
		public string my_OrderStatus_CustomerFacingDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "OrderStatus_CustomerFacingDescription");
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
		public int my_Customer_CustomerType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Customer_CustomerType"));
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

		# region My_ Values Country

		/// <summary>
		/// <see cref="clsCountry.my_CountryName">Name</see> of
		/// <see cref="clsCountry">Country</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_CountryName">Name</see> of 
		/// <see cref="clsCountry">Country</see> 
		/// </returns>
		public string my_Country_CountryName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Country_CountryName");
		}

		/// <summary>
		/// <see cref="clsCountry.my_LocalTaxApplies">Whether Local Tax Applies</see> to this
		/// <see cref="clsCountry">Country</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_LocalTaxApplies">Whether Local Tax Applies</see> to this 
		/// <see cref="clsCountry">Country</see> 
		/// </returns>
		public int my_Country_LocalTaxApplies(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Country_LocalTaxApplies"));
		}

		/// <summary>
		/// <see cref="clsCountry.my_CanShipToCountry">Whether we can ship</see> to this
		/// <see cref="clsCountry">Country</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_CanShipToCountry">Whether we can ship</see> to this 
		/// <see cref="clsCountry">Country</see> 
		/// </returns>
		public int my_Country_CanShipToCountry(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Country_CanShipToCountry"));
		}


		/// <summary>
		/// <see cref="clsShippingZone.my_ShippingZoneId">ShippingZoneId</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see>
		/// Associated with this Country</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsShippingZone.my_ShippingZoneId">Id</see> 
		/// of <see cref="clsShippingZone">ShippingZone</see> 
		/// for this Country</returns>	
		public int my_Country_ShippingZoneId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Country_ShippingZoneId"));
		}

		#endregion

	}
}
