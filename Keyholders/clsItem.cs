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
	/// <summary>clsItem deals with everything to do with data about Items</summary>
	[GuidAttribute("677204AF-4B26-4978-9FFA-04BD007277A8")]
	public class clsItem : clsKeyBase
	{
		#region Initialisation

		#region Constructors

		/// <summary>Constructor for clsItem</summary>
		public clsItem() : base("Item")
		{
		}

		/// <summary>Constructor for clsItem; Allows Calling of 'Connect' Function to be skipped</summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration <see cref="clsRecordHandler.databaseType">databaseType</see></param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsItem(clsRecordHandler.databaseType typeOfDb,
			OdbcConnection odbcConnection) : base("Item")
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

		#region QueryPartSets

		/// <summary>bareQueriesWithOrder</summary>
		public clsQueryPart[] bareQueriesWithOrder;

		#endregion

		#region QueryParts

		/// <summary>Main2Q</summary>
		public clsQueryPart Main2Q = new clsQueryPart();

		/// <summary>ProductQ</summary>
		public clsQueryPart ProductQ = new clsQueryPart();

		/// <summary>PremiseQ</summary>
		public clsQueryPart PremiseQ = new clsQueryPart();

		/// <summary>CustomerQ</summary>
		public clsQueryPart CustomerQ = new clsQueryPart();

		/// <summary>Customer2Q</summary>
		public clsQueryPart Customer2Q = new clsQueryPart();

		/// <summary>OrderQ</summary>
		public clsQueryPart OrderQ = new clsQueryPart();

		#endregion

		#region LoadMainQuery

		/// <summary>Loads the SQL for query types for the GetBys</summary>
		public override void LoadMainQuery()
		{
			MainQ = ItemQueryPart();

			PremiseQ = PremiseQueryPart();
			PremiseQ.FromTables.Clear();
			PremiseQ.Joins.Clear();

			ProductQ = ProductQueryPart();
			ProductQ.FromTables.Clear();
			ProductQ.Joins.Clear();

			OrderQ = OrderQueryPart();

			bareQueriesWithOrder = new clsQueryPart[]
			{
				MainQ,
				OrderQ
			};

			Main2Q = ItemQueryPart();
			Main2Q.FromTables.Clear();
			Main2Q.AddFromTable(thisTable + " left outer join tblProduct on tblItem.ProductId = tblProduct.ProductId"
			 + " left outer join tblPremise on tblItem.PremiseId = tblPremise.PremiseId");



			CustomerQ = CustomerQueryPart();
			CustomerQ.Joins.Clear();
			CustomerQ.AddJoin("tblPremise.CustomerId = tblCustomer.CustomerId");

			//MainQ.AddSelectColumn("tblItem.ItemId");
			//MainQ.AddSelectColumn("tblItem.OrderId");
			//MainQ.AddSelectColumn("tblItem.ProductId");
			//MainQ.AddSelectColumn("tblItem.PremiseId");
			//MainQ.AddSelectColumn("tblItem.Quantity");
			//MainQ.AddSelectColumn("tblItem.ItemName");
			//MainQ.AddSelectColumn("tblItem.ItemCode");
			//MainQ.AddSelectColumn("tblItem.ShortDescription");
			//MainQ.AddSelectColumn("tblItem.LongDescription");
			//MainQ.AddSelectColumn("tblItem.Cost");
			//MainQ.AddSelectColumn("tblItem.Weight");
			//MainQ.AddSelectColumn("tblItem.MaxKeyholdersPerPremise");
			//MainQ.AddSelectColumn("tblItem.MaxAssetRegisterAssets");
			//MainQ.AddSelectColumn("tblItem.MaxAssetRegisterStorage");
			//MainQ.AddSelectColumn("tblItem.MaxDocumentSafeDocuments");
			//MainQ.AddSelectColumn("tblItem.MaxDocumentSafeStorage");
			//MainQ.AddSelectColumn("tblItem.RequiresPremiseForActivation");
			//MainQ.AddSelectColumn("tblItem.DateActivation");
			//MainQ.AddSelectColumn("tblItem.DateActivationUtc");
			//MainQ.AddSelectColumn("tblItem.DateExpiry");
			//MainQ.AddSelectColumn("tblItem.DateExpiryUtc");
			//MainQ.AddSelectColumn("tblItem.DurationNumUnits");
			//MainQ.AddSelectColumn("tblItem.DurationUnitId");
			//MainQ.AddSelectColumn("tblItem.FreightCost");
			//MainQ.AddFromTable(thisTable + " left outer join tblProduct on tblItem.ProductId = tblProduct.ProductId"
			// + " left outer join tblPremise on tblItem.PremiseId = tblPremise.PremiseId");

			clsQueryBuilder QB = new clsQueryBuilder();


			baseQueries = new clsQueryPart[]
			{
				Main2Q,
				PremiseQ,
				ProductQ,
				OrderQ,
				CustomerQ
			};

			//baseQueries = new clsQueryPart[5];

			//baseQueries[0] = MainQ;
			//baseQueries[1] = PremiseQ;
			//baseQueries[2] = ProductQ;
			//baseQueries[3] = OrderQ;
			//baseQueries[4] = CustomerQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By tblItem.ItemId" + crLf;

		}

		#endregion

		#region Initialise

		/// <summary>Initialise (or reinitialise) everything for this class</summary>
		public override void Initialise()
		{

			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);

			newDataToAdd.Columns.Add("OrderId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ProductId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PremiseId", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("Quantity", System.Type.GetType("System.Decimal"));

			newDataToAdd.Columns.Add("ItemName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ItemCode", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("ShortDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("LongDescription", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("Cost", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("Weight", System.Type.GetType("System.Decimal"));

			newDataToAdd.Columns.Add("MaxKeyholdersPerPremise", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("MaxAssetRegisterAssets", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("MaxAssetRegisterStorage", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("MaxDocumentSafeDocuments", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("MaxDocumentSafeStorage", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("RequiresPremiseForActivation", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("DateActivation", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateActivationUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateExpiry", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateExpiryUtc", System.Type.GetType("System.String"));

			newDataToAdd.Columns.Add("DurationNumUnits", System.Type.GetType("System.Double"));

			newDataToAdd.Columns.Add("DurationUnitId", System.Type.GetType("System.Int32"));

			newDataToAdd.Columns.Add("FreightCost", System.Type.GetType("System.Decimal"));


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

		#region GetByItemId

		/// <summary>Get a Item by ItemId</summary>
		/// <param name="ItemId">ItemId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByItemId(int ItemId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable + crLf
				+ " Where " + thisTable + ".ItemId = " + ItemId.ToString() + crLf
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

		#region GetByOrderId

		/// <summary>Gets Items by OrderId</summary>
		/// <param name="OrderId">OrderId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderId(int OrderId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
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

		#region GetByProductId

		/// <summary>Gets Items by ProductId</summary>
		/// <param name="ProductId">ProductId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductId(int ProductId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".ProductId = " + ProductId.ToString() + crLf
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

		#region GetByPremiseId

		/// <summary>Gets Items by PremiseId</summary>
		/// <param name="PremiseId">PremiseId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPremiseId(int PremiseId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".PremiseId = " + PremiseId.ToString() + crLf
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

		#region GetByMaxKeyholdersPerPremise

		/// <summary>Gets Items by MaxKeyholdersPerPremise</summary>
		/// <param name="MaxKeyholdersPerPremise">MaxKeyholdersPerPremise</param>
		/// <returns>Number of resulting records</returns>
		public int GetByMaxKeyholdersPerPremise(int MaxKeyholdersPerPremise)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".MaxKeyholdersPerPremise = " + MaxKeyholdersPerPremise.ToString() + crLf
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

		#region GetByMaxAssetRegisterAssets

		/// <summary>Gets Items by MaxAssetRegisterAssets</summary>
		/// <param name="MaxAssetRegisterAssets">MaxAssetRegisterAssets</param>
		/// <returns>Number of resulting records</returns>
		public int GetByMaxAssetRegisterAssets(int MaxAssetRegisterAssets)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".MaxAssetRegisterAssets = " + MaxAssetRegisterAssets.ToString() + crLf
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

		#region GetByMaxAssetRegisterStorage

		/// <summary>Gets Items by MaxAssetRegisterStorage</summary>
		/// <param name="MaxAssetRegisterStorage">MaxAssetRegisterStorage</param>
		/// <returns>Number of resulting records</returns>
		public int GetByMaxAssetRegisterStorage(int MaxAssetRegisterStorage)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".MaxAssetRegisterStorage = " + MaxAssetRegisterStorage.ToString() + crLf
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

		#region GetByMaxDocumentSafeDocuments

		/// <summary>Gets Items by MaxDocumentSafeDocuments</summary>
		/// <param name="MaxDocumentSafeDocuments">MaxDocumentSafeDocuments</param>
		/// <returns>Number of resulting records</returns>
		public int GetByMaxDocumentSafeDocuments(int MaxDocumentSafeDocuments)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".MaxDocumentSafeDocuments = " + MaxDocumentSafeDocuments.ToString() + crLf
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

		#region GetByMaxDocumentSafeStorage

		/// <summary>Gets Items by MaxDocumentSafeStorage</summary>
		/// <param name="MaxDocumentSafeStorage">MaxDocumentSafeStorage</param>
		/// <returns>Number of resulting records</returns>
		public int GetByMaxDocumentSafeStorage(int MaxDocumentSafeStorage)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".MaxDocumentSafeStorage = " + MaxDocumentSafeStorage.ToString() + crLf
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

		#region GetByRequiresPremiseForActivation

		/// <summary>Gets Items by RequiresPremiseForActivation</summary>
		/// <param name="RequiresPremiseForActivation">RequiresPremiseForActivation</param>
		/// <returns>Number of resulting records</returns>
		public int GetByRequiresPremiseForActivation(int RequiresPremiseForActivation)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".RequiresPremiseForActivation = " + RequiresPremiseForActivation.ToString() + crLf
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

		#region GetByDurationUnitId

		/// <summary>Gets Items by DurationUnitId</summary>
		/// <param name="DurationUnitId">DurationUnitId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByDurationUnitId(int DurationUnitId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable
				+ " Where " + thisTable + ".DurationUnitId = " + DurationUnitId.ToString() + crLf
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



		#region GetByOrderSubmittedOrderCreatedMechanism

		/// <summary>GetByOrderSubmittedOrderCreatedMechanism</summary>
		/// <param name="OrderSubmitted">Filter for orders that have been paid for or not</param>
		/// <param name="OrderCreatedMechanism">Id of Customer</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderSubmittedOrderCreatedMechanism(int OrderSubmitted, int OrderCreatedMechanism)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = bareQueriesWithOrder;

			string thisTable2 = "tblOrder";

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable2
				+ " Where " + thisTable2 + ".OrderSubmitted = " + OrderSubmitted.ToString() + crLf
				+ "		And " + thisTable2 + ".OrderCreatedMechanism = " + OrderCreatedMechanism.ToString() + crLf
				+ ") " + thisTable2
				);

			thisSqlQuery = QB.BuildSqlStatement(
				queries,
				OrderByColumns,
				condition.ToString(),
				thisTable2
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

			string thisTable2 = "tblOrder";

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable2 + crLf
				+ "Where " + thisTable2 + ".OrderSubmitted = 0" + crLf
				+ ") " + thisTable2 + crLf);

			thisSqlQuery = QB.BuildSqlStatement(
				queries,
				new ArrayList(),
				condition.ToString(),
				thisTable2
				);

			//Ordering
			thisSqlQuery += " Order by convert(tblCustomer.AccountNumber, signed)";

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetByQuantity

		/// <summary>GetByQuantity</summary>
		/// <param name="Quantity">Order Quantity to retrieve Items for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByQuantity(decimal Quantity)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf
				+ "Where tblItem.Quantity = "
				+ Quantity.ToString();

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns,
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetProblemItems

		/// <summary>GetProblemItems </summary>
		/// <param name="OrderId">Id of Order to retrieve Items for</param>
		/// <returns>Number of resulting records</returns>
		public int GetProblemItems(int OrderId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf
				+ "Where tblItem.OrderId = " + OrderId.ToString() + crLf
				+ "And (" + MaxAllowable + ") > -1" + crLf
				+ "And tblItem.Quantity > (" + MaxAllowable + ")" + crLf
				;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns,
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetByOrderIdProductNull

		/// <summary>GetByOrderIdProductNull</summary>
		/// <param name="OrderId">Id of Order to retrieve Items for</param>
		/// <param name="ProductNull">Whether the to include ProductIds which are null or those which are not</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderIdProductNull(int OrderId, bool ProductNull)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf
				+ "Where tblItem.OrderId = " + OrderId.ToString()
				;

			if (!ProductNull)
				condition += " And tblItem.ProductId is not NULL";
			else
				condition += " tblItem.ProductId is NULL";

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns,
				condition,
				thisTable
				);


			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetByOrderIdProductId

		/// <summary>GetByOrderIdProductId</summary>
		/// <param name="OrderId">Id of Order to retrieve Items for</param>
		/// <param name="ProductId">Id of Product to find</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderIdProductId(int OrderId, int ProductId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable + crLf
				+ "Where " + thisTable + ".OrderId = " + OrderId.ToString());

			if (ProductId == 0)
				condition.Append(" And " + thisTable + ".ProductId is NULL");
			else
				condition.Append(" And " + thisTable + ".ProductId = " + ProductId.ToString());

			condition.Append(") " + thisTable);

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetByOrderIdPremiseId

		/// <summary>GetByOrderIdPremiseId</summary>
		/// <param name="OrderId">Id of Order to retrieve Items for</param>
		/// <param name="PremiseId">Id of Premise to find</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderIdPremiseId(int OrderId, int PremiseId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			StringBuilder condition = new StringBuilder("(Select * from " + thisTable + crLf
				+ "Where " + thisTable + ".OrderId = " + OrderId.ToString());

			if (PremiseId == 0)
				condition.Append(" And " + thisTable + ".PremiseId is NULL");
			else
				condition.Append(" And " + thisTable + ".PremiseId = " + PremiseId.ToString());

			condition.Append(") " + thisTable);

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns,
				condition.ToString(),
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region GetByPremiseIdOrderDateCreated

		/// <summary>GetByPremiseIdOrderDateCreated</summary>
		/// <param name="PremiseId">Id of Premise to retrieve Items for</param>
		/// <param name="Day">OrderDateCreated Day</param>
		/// <param name="Month">OrderDateCreated Month</param>
		/// <param name="Year"> OrderDateCreatedYear</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPremiseIdOrderDateCreated(int PremiseId,
			int Day, int Month, int Year)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition1 = "(Select * from " + thisTable + crLf
				+ "Where tblItem.PremiseId = " + PremiseId.ToString()
				;
			condition1 += ") " + thisTable;


			string condition2 = "(Select * From tblOrder" + crLf;

			//Additional Condition
			condition2 += "Where Day(tblOrder.DateCreated) = " + Day.ToString()
				;
			condition2 += "	And Month(tblOrder.DateCreated) = " + Month.ToString()
				;
			condition2 += "	And Year(tblOrder.DateCreated) = " + Year.ToString()
				;
			condition2 += ") tblOrder";


			clsQueryBuilder.ConditionWithTable[] thisConditions = new Resources.clsQueryBuilder.ConditionWithTable[2];
			thisConditions[0].condition = condition1;
			thisConditions[0].table = thisTable;
			thisConditions[1].condition = condition2;
			thisConditions[1].table = "tblOrder";

			thisSqlQuery = QB.BuildSqlStatement(queries,
				OrderByColumns,
				thisConditions
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += "Order By tblOrder.OrderId";

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#endregion

		#endregion

		#region AddInvoice

		/// <summary>
		/// Adds an Invoice the the list of items for this customer
		/// </summary>
		/// <param name="OrderId">Order Associated with this Item</param>
		/// <param name="ItemName">Item's Name</param>
		/// <param name="ItemCode">Item's Code</param>
		/// <param name="Cost">Cost of Single Item</param>
		public void AddInvoice(int OrderId,
			string ItemCode,
			string ItemName,
			decimal Cost)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["ProductId"] = DBNull.Value;

			rowToAdd["OrderId"] = OrderId;
			rowToAdd["ItemName"] = ItemName;
			rowToAdd["ItemCode"] = ItemCode;
			rowToAdd["ShortDescription"] = "";
			rowToAdd["LongDescription"] = "";
			rowToAdd["Quantity"] = 1;
			rowToAdd["Cost"] = Cost;

			rowToAdd["Weight"] = DBNull.Value;
			rowToAdd["FreightCost"] = DBNull.Value;

			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				newDataToAdd.Rows.Add(rowToAdd);
			}

		}


		#endregion

		#region CancelRenewalOrder

		/// <summary>
		/// Cancels the renewal for a Premise. This means that the Premise will never have a renewal order created for it
		/// unlees there is manual intervention
		/// </summary>
		/// <param name="ItemId">Id of Item to cancel</param>
		public void CancelRenewalOrder(int ItemId)
		{
			PostponeOrCancel(ItemId, 0);
		}


		#endregion

		#region PostponeRenewalOrder

		/// <summary>
		/// Cancels the renewal for a Premise. This means that the Premise will never have a renewal order created for it
		/// unlees there is manual intervention
		/// </summary>
		/// <param name="ItemId">Id of Item to cancel</param>
		public void PostponeRenewalOrder(int ItemId)
		{
			PostponeOrCancel(ItemId, 1);
		}


		#endregion

		#region PostponeOrCancel

		/// <summary>
		/// Postpones/Cancels Renewal Orders
		/// </summary>
		/// <param name="ItemId">Id of Item to cancel</param>
		/// <param name="Postpone">Whether to Postpone or Cancel</param>
		public void PostponeOrCancel(int ItemId, int Postpone)
		{
			clsItem thisItem = new clsItem(thisDbType, localRecords.dbConnection);
			thisItem.GetByItemId(ItemId);

			int PremiseId = thisItem.my_PremiseId(0);
			int CustomerId = thisItem.my_Order_CustomerId(0);

			Delete(ItemId);
			clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);
			thisOrder.DeleteUnsubmittedOrdersWithNoItems();

			if (PremiseId > 0)
			{
				clsPremise thisPremise = new clsPremise(thisDbType, localRecords.dbConnection);
				thisPremise.SetAttribute(PremiseId, "InvoiceRequired", "0");
				thisPremise.SetAttribute(PremiseId, "CopyOfInvoiceRequired", "0");

				if (Postpone == 0 && CustomerId != 0)
					thisPremise.KillRenewalOrder(PremiseId, CustomerId);

				thisPremise.Save();
			}

		}


		#endregion

		#region SetFromProductId

		/// <summary>
		/// This method adds a quanity of a product to an Order. If the Product already exists 
		/// in this order, it modifies this item and adds Quantity to the existing Quantity
		/// Save must be called to complete this process.
		/// </summary>
		/// <param name="ItemId">Item to Change</param>
		/// <param name="ProductId">Product Associated with this Item</param>
		/// <param name="Quantity">Quantity of Item(s) or part there of</param>
		public void SetFromProductId(int ItemId,
			int ProductId,
			decimal Quantity)
		{

			clsItem thisItem = new clsItem(thisDbType, localRecords.dbConnection);
			thisItem.GetByItemId(ItemId);

			clsProduct thisProduct = new clsProduct(thisDbType, localRecords.dbConnection);
			thisProduct.GetByProductId(ProductId);

			ModifyEasy(ItemId,
				thisItem.my_OrderId(0),
				ProductId,
				thisItem.my_PremiseId(0),
				Quantity,
				thisProduct.my_ProductName(0),
				thisProduct.my_ProductCode(0),
				thisProduct.my_ShortDescription(0),
				thisProduct.my_DefinedContent_Description(0),
				0,
				thisProduct.my_Weight(0),
				thisProduct.my_MaxKeyholdersPerPremise(0),
				thisProduct.my_MaxAssetRegisterAssets(0),
				thisProduct.my_MaxAssetRegisterStorage(0),
				thisProduct.my_MaxDocumentSafeDocuments(0),
				thisProduct.my_MaxDocumentSafeStorage(0),
				thisProduct.my_RequiresPremiseForActivation(0),
				thisItem.my_DateActivation(0),
				thisItem.my_DateExpiry(0),
				thisProduct.my_DurationNumUnits(0),
				thisProduct.my_DurationUnitId(0),
				Convert.ToDecimal(0));

		}


		#endregion

		#region AddFromProductId

		/// <summary>
		/// This method adds a quanity of a product to an Order. If the Product already exists 
		/// in this order, it modifies this item and adds Quantity to the existing Quantity
		/// Save must be called to complete this process.
		/// </summary>
		/// <param name="OrderId">Order Associated with this Item</param>
		/// <param name="ProductId">Product Associated with this Item</param>
		/// <param name="Quantity">Quantity of Item(s) or part there of</param>
		public void AddFromProductId(int OrderId,
			int ProductId,
			decimal Quantity)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			//See if this Product is already in this Order
			clsItem thisItem = new clsItem(thisDbType, localRecords.dbConnection);
			int numInstances = thisItem.GetByOrderIdProductId(OrderId, ProductId);

			clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);
			thisOrder.GetByOrderId(OrderId);

			clsProduct thisProduct = new clsProduct(thisDbType, localRecords.dbConnection);
			thisProduct.GetByProductIdCustomerGroupId(ProductId, thisOrder.my_CustomerGroupId(0));

			if (numInstances == 0 || numInstances > 0 && thisProduct.my_WholeNumbersOnly(0) == productNumberType_oneOnly())
			{

				//Adding a Product to an Order (first instance of this Product in this Order
				System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

				rowToAdd["Cost"] = Convert.ToDecimal(thisProduct.my_ProductCustomerGroupPrice_PriceExcludingTax(0));

				rowToAdd["PremiseId"] = DBNull.Value;
				rowToAdd["DateActivation"] = DBNull.Value;
				rowToAdd["DateActivationUtc"] = DBNull.Value;
				rowToAdd["DateExpiry"] = DBNull.Value;
				rowToAdd["DateExpiryUtc"] = DBNull.Value;

				rowToAdd["ProductId"] = ProductId;
				rowToAdd["OrderId"] = OrderId;
				rowToAdd["Quantity"] = Quantity;

				rowToAdd["ItemName"] = thisProduct.my_ProductName(0);
				rowToAdd["ItemCode"] = thisProduct.my_ProductCode(0);
				rowToAdd["ShortDescription"] = thisProduct.my_ShortDescription(0);
				rowToAdd["LongDescription"] = thisProduct.my_DefinedContent_Description(0);

				rowToAdd["MaxKeyholdersPerPremise"] = thisProduct.my_MaxKeyholdersPerPremise(0);
				rowToAdd["MaxAssetRegisterAssets"] = thisProduct.my_MaxAssetRegisterAssets(0);
				rowToAdd["MaxAssetRegisterStorage"] = thisProduct.my_MaxAssetRegisterStorage(0);
				rowToAdd["MaxDocumentSafeDocuments"] = thisProduct.my_MaxDocumentSafeDocuments(0);
				rowToAdd["MaxDocumentSafeStorage"] = thisProduct.my_MaxDocumentSafeStorage(0);
				rowToAdd["DurationNumUnits"] = thisProduct.my_DurationNumUnits(0);
				rowToAdd["DurationUnitId"] = thisProduct.my_DurationUnitId(0);
				rowToAdd["RequiresPremiseForActivation"] = thisProduct.my_RequiresPremiseForActivation(0);


				decimal Weight = Convert.ToDecimal(thisProduct.my_Weight(0));

				if (Weight != 0)
					rowToAdd["Weight"] = Weight;
				else
					rowToAdd["Weight"] = DBNull.Value;

				rowToAdd["FreightCost"] = DBNull.Value;

				//Validate the data supplied
				Validate(rowToAdd, true);

				if (NumErrors() == 0)
				{
					newDataToAdd.Rows.Add(rowToAdd);
				}
			}
			else
			{
				//Adding an additional Quantity of Product to an Order
				SetQuantity(thisItem.my_ItemId(0), thisItem.my_Quantity(0) + Quantity);
			}


		}

		#endregion

		#region AddFromProductIdPremiseId

		/// <summary>
		/// This method adds a quanity of a product to an Order. If the Product already exists 
		/// in this order, it modifies this item and adds Quantity to the existing Quantity
		/// Save must be called to complete this process.
		/// </summary>
		/// <param name="OrderId">Order Associated with this Item</param>
		/// <param name="ProductId">Product Associated with this Item</param>
		/// <param name="PremiseId">Premise to Add from</param>
		/// <param name="DateActivation">Date of Activation of a subsciption, if there is one</param>
		/// <param name="Quantity">Quantity of Item(s) or part there of</param>
		public void AddFromProductIdPremiseId(int OrderId,
			int ProductId,
			int PremiseId,
			string DateActivation,
			decimal Quantity)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			//See if this Product is already in this Order
			clsItem thisItem = new clsItem(thisDbType, localRecords.dbConnection);
			int numInstances = thisItem.GetByOrderIdProductId(OrderId, ProductId);

			//			clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);
			//			thisOrder.GetByOrderId(OrderId);

			clsProduct thisProduct = new clsProduct(thisDbType, localRecords.dbConnection);
			int numProducts = thisProduct.GetByProductId(ProductId);

			if (numProducts > 0)
			{


				if (numInstances == 0 || numInstances > 0 && thisProduct.my_WholeNumbersOnly(0) == productNumberType_oneOnly())
				{

					//Adding a Product to an Order (first instance of this Product in this Order
					System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

					rowToAdd["PremiseId"] = PremiseId;

					if (DateActivation == "")
					{
						rowToAdd["DateActivation"] = DBNull.Value;
						rowToAdd["DateActivationUtc"] = DBNull.Value;
						rowToAdd["DateExpiry"] = DBNull.Value;
						rowToAdd["DateExpiryUtc"] = DBNull.Value;
					}
					else
					{
						DateTime thisActivation = Convert.ToDateTime(DateActivation);
						DateTime thisExpiry;

						switch ((durationUnitType)thisProduct.my_DurationUnitId(0))
						{
							case durationUnitType.second:
								thisExpiry = thisActivation.AddSeconds(thisProduct.my_DurationNumUnits(0));
								break;
							case durationUnitType.minute:
								thisExpiry = thisActivation.AddMinutes(thisProduct.my_DurationNumUnits(0));
								break;
							case durationUnitType.hour:
								thisExpiry = thisActivation.AddHours(thisProduct.my_DurationNumUnits(0));
								break;
							case durationUnitType.day:
								thisExpiry = thisActivation.AddDays(thisProduct.my_DurationNumUnits(0));
								break;
							case durationUnitType.week:
								thisExpiry = thisActivation.AddDays(7 * thisProduct.my_DurationNumUnits(0));
								break;
							case durationUnitType.month:
								thisExpiry = thisActivation.AddMonths(Convert.ToInt32(thisProduct.my_DurationNumUnits(0)));
								break;
							case durationUnitType.year:
								thisExpiry = thisActivation.AddYears(Convert.ToInt32(thisProduct.my_DurationNumUnits(0)));
								break;
							default:
								thisExpiry = thisActivation.AddYears(Convert.ToInt32(thisProduct.my_DurationNumUnits(0)));
								break;
						}

						rowToAdd["DateActivation"] = localRecords.DBDateTime(thisActivation);
						rowToAdd["DateActivationUtc"] = localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(thisActivation)));
						rowToAdd["DateExpiry"] = localRecords.DBDateTime(thisExpiry);
						rowToAdd["DateActivationUtc"] = localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(thisExpiry)));

					}

					#region Get the Cost here
					int thisCustomerGroupId = 0;

					clsPremise thisPremise = new clsPremise(thisDbType, localRecords.dbConnection);
					int numPremises = thisPremise.GetByPremiseId(PremiseId);

					decimal Cost = 0;

					if (numPremises > 0)
					{
						thisCustomerGroupId = thisPremise.my_Customer_CustomerGroupId(0);

						clsProductCustomerGroupPrice thisProductCustomerGroupPrice = new clsProductCustomerGroupPrice(thisDbType, localRecords.dbConnection);
						int numProductCustomerGroupPrices = thisProductCustomerGroupPrice.GetByProductIdAndCustomerGroupId(ProductId, thisCustomerGroupId);

						if (numProductCustomerGroupPrices > 0)
						{
							Cost = thisProductCustomerGroupPrice.my_Price(0);

						}

					}




					#endregion


					rowToAdd["Cost"] = Cost;

					rowToAdd["ProductId"] = ProductId;
					rowToAdd["OrderId"] = OrderId;
					rowToAdd["Quantity"] = Quantity;

					rowToAdd["ItemName"] = thisProduct.my_ProductName(0);
					rowToAdd["ItemCode"] = thisProduct.my_ProductCode(0);
					rowToAdd["ShortDescription"] = thisProduct.my_ShortDescription(0);
					rowToAdd["LongDescription"] = "";

					rowToAdd["MaxKeyholdersPerPremise"] = thisProduct.my_MaxKeyholdersPerPremise(0);
					rowToAdd["MaxAssetRegisterAssets"] = thisProduct.my_MaxAssetRegisterAssets(0);
					rowToAdd["MaxAssetRegisterStorage"] = thisProduct.my_MaxAssetRegisterStorage(0);
					rowToAdd["MaxDocumentSafeDocuments"] = thisProduct.my_MaxDocumentSafeDocuments(0);
					rowToAdd["MaxDocumentSafeStorage"] = thisProduct.my_MaxDocumentSafeStorage(0);
					rowToAdd["DurationNumUnits"] = thisProduct.my_DurationNumUnits(0);
					rowToAdd["DurationUnitId"] = thisProduct.my_DurationUnitId(0);
					rowToAdd["RequiresPremiseForActivation"] = thisProduct.my_RequiresPremiseForActivation(0);


					decimal Weight = Convert.ToDecimal(thisProduct.my_Weight(0));

					if (Weight != 0)
						rowToAdd["Weight"] = Weight;
					else
						rowToAdd["Weight"] = DBNull.Value;

					rowToAdd["FreightCost"] = DBNull.Value;

					//Validate the data supplied
					Validate(rowToAdd, true);

					if (NumErrors() == 0)
					{
						newDataToAdd.Rows.Add(rowToAdd);
					}
				}
				else
				{
					//Adding an additional Quantity of Product to an Order
					SetQuantity(thisItem.my_ItemId(0), thisItem.my_Quantity(0) + Quantity);
				}
			}

		}


		#endregion

		#region SetFreight

		/// <summary>
		/// When Freight is charged by the Item, there is a freight cost associated with each item.
		/// This Method allows this Freight Cost to be set outside of the Modify Method
		/// </summary>
		/// <param name="ItemId">Id of Item to set Freight Cost for</param>
		/// <param name="FreightCost">Freight Cost to set</param>
		public void SetFreight(int ItemId,
			decimal FreightCost)
		{
			SetAttribute(ItemId, "FreightCost", FreightCost.ToString());
		}

		#endregion

		#region SetCost

		/// <summary>
		/// This Method allows the Item Cost to be set outside of the Modify Method
		/// </summary>
		/// <param name="ItemId">Id of Item to set Cost for</param>
		/// <param name="Cost">Cost to set</param>
		public void SetCost(int ItemId,
			decimal Cost)
		{
			SetAttribute(ItemId, "Cost", Cost.ToString());
		}


		#endregion

		#region SetPremiseId

		/// <summary>
		/// This Method allows the PremiseId to be set for an item outside of the Modify Method
		/// </summary>
		/// <param name="ItemId">Id of Item to set Premise for</param>
		/// <param name="PremiseId">Premise to set</param>
		/// <param name="DateActivation">Date that this subscription is Activated</param>
		/// <param name="DateExpiry">Date that this subscription Expires</param>
		public void SetPremiseId(int ItemId,
			int PremiseId,
			string DateActivation,
			string DateExpiry)
		{
			SetAttribute(ItemId, "PremiseId", PremiseId.ToString());

			if (DateActivation == "" || DateExpiry == "")
			{
				AddAttributeToSet(ItemId, "DateActivation", DBNull.Value.ToString());
				AddAttributeToSet(ItemId, "DateActivationUtc", DBNull.Value.ToString());
				AddAttributeToSet(ItemId, "DateExpiry", DBNull.Value.ToString());
				AddAttributeToSet(ItemId, "DateExpiryUtc", DBNull.Value.ToString());
			}
			else
			{
				DateTime thisActivation = Convert.ToDateTime(DateActivation);
				DateTime thisExpiry = Convert.ToDateTime(DateExpiry); ;

				AddAttributeToSet(ItemId, "DateActivation", localRecords.DBDateTime(thisActivation));
				AddAttributeToSet(ItemId, "DateActivationUtc", localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(thisActivation))));
				AddAttributeToSet(ItemId, "DateExpiry", localRecords.DBDateTime(thisExpiry));
				AddAttributeToSet(ItemId, "DateExpiryUtc", localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(thisExpiry))));

			}
		}


		#endregion

		#region SetQuantity

		/// <summary>
		/// This Method allows this Quantity of an item to be set
		/// </summary>
		/// <param name="ItemId">Id of Item to set Quantity for</param>
		/// <param name="Quantity">Quantity to set</param>
		public void SetQuantity(int ItemId,
			decimal Quantity)
		{
			if (Quantity == 0)
				Delete(ItemId);
			else
			{
				clsItem thisItem = new clsItem(thisDbType, localRecords.dbConnection);
				thisItem.GetByItemId(ItemId);

				System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

				rowToAdd["Quantity"] = Quantity;
				rowToAdd["ItemId"] = ItemId;
				rowToAdd["ProductId"] = thisItem.my_ProductId(0);

				Validate(rowToAdd, false);

				if (NumErrors() == 0)
					SetAttribute(ItemId, "Quantity", Quantity.ToString());

			}
		}

		#endregion

		#region CheckItemsForOrder

		/// <summary>
		/// Checks every item in an order for Stock Control
		/// </summary>
		/// <param name="OrderId">Id Of Order for which to check items</param>
		public void CheckItemsForOrder(int OrderId)
		{
			clsItem theseItems = new clsItem(thisDbType, localRecords.dbConnection);
			int numItems = theseItems.GetByOrderId(OrderId);
			for (int itemCounter = 0; itemCounter < numItems; itemCounter++)
			{
				System.Data.DataRow rowToAdd = dataToBeModified.NewRow();
				rowToAdd["ItemId"] = theseItems.my_ItemId(0);
				rowToAdd["ProductId"] = theseItems.my_ProductId(0);
				rowToAdd["Quantity"] = theseItems.my_Quantity(0);

				Validate(rowToAdd, false);
			}

		}

		#endregion

		#region ModifyEasy

		/// <summary>ModifyEasy</summary>
		/// <param name="ItemId">ItemId (Primary Key of Record)</param>
		/// <param name="OrderId">Order Associated with this Item</param>
		/// <param name="ProductId">Product Associated with this Item</param>
		/// <param name="PremiseId">Id of Premise Associated with this Item</param>
		/// <param name="Quantity">Quantity of Item(s) or part there of</param>
		/// <param name="ItemName">Item's Name</param>
		/// <param name="ItemCode">Item's Code</param>
		/// <param name="ShortDescription">Item's Description</param>
		/// <param name="LongDescription">Item's Description (Long)</param>
		/// <param name="Cost">Cost of Single Item</param>
		/// <param name="Weight">Weight of Item(s) or part there of</param>
		/// <param name="MaxKeyholdersPerPremise">Max Keyholders Per Premise (if this is a Premise Related Item)</param>
		/// <param name="MaxAssetRegisterAssets">Max number of assets in the Asset Register (if this is as Asset Register related Item)</param>
		/// <param name="MaxAssetRegisterStorage">Max Asset Register Storage (if this is as Asset Register related Item)</param>
		/// <param name="MaxDocumentSafeDocuments">Max number of documents in the Document Safe (if this is as Document Safe related Item)</param>
		/// <param name="MaxDocumentSafeStorage">Max Document Safe Storage (if this is as Document Safe related Item)</param>
		/// <param name="RequiresPremiseForActivation">Whether this Item Requires an associated Premise in order to be Activated 
		/// (if this is a Premise/Time Related Item)</param>
		/// <param name="DateActivation">Date that this subscription is Activated</param>
		/// <param name="DateExpiry">Date that this subscription Expires</param>
		/// <param name="DurationNumUnits">Number of Unit's duration (if this is a Service over time)</param>
		/// <param name="DurationUnitId">Time Unit's e.g. year, month, day (if this is a Service over time)</param>
		/// <param name="FreightCost"></param>
		public void ModifyEasy(int ItemId,
			int OrderId,
			int ProductId,
			int PremiseId,
			decimal Quantity,
			string ItemName,
			string ItemCode,
			string ShortDescription,
			string LongDescription,
			decimal Cost,
			decimal Weight,
			int MaxKeyholdersPerPremise,
			int MaxAssetRegisterAssets,
			int MaxAssetRegisterStorage,
			int MaxDocumentSafeDocuments,
			int MaxDocumentSafeStorage,
			int RequiresPremiseForActivation,
			string DateActivation,
			string DateExpiry,
			double DurationNumUnits,
			int DurationUnitId,
			decimal FreightCost)
		{

			if (priceShownIncludesLocalTaxRate)
				Cost = Cost / localTaxRate;

			string DateActivationUtc = "";
			string DateExpiryUtc = "";

			if (DateActivation != "" && DateExpiry != "")
			{
				DateTime thisActivation = Convert.ToDateTime(DateActivation);
				DateTime thisExpiry = Convert.ToDateTime(DateExpiry);

				DateActivation = localRecords.DBDateTime(thisActivation);
				DateActivationUtc = localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(thisActivation)));
				DateExpiry = localRecords.DBDateTime(thisExpiry);
				DateExpiryUtc = localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(thisExpiry)));
			}

			Modify(
			   ItemId,
			   OrderId,

			   ProductId,
			   PremiseId,

			   Quantity,

			   ItemName,
			   ItemCode,

			   ShortDescription,
			   LongDescription,

			   Cost,
			   Weight,

			   MaxKeyholdersPerPremise,
			   MaxAssetRegisterAssets,
			   MaxAssetRegisterStorage,
			   MaxDocumentSafeDocuments,
			   MaxDocumentSafeStorage,
			   RequiresPremiseForActivation,

			   DateActivation,
			   DateActivationUtc,
			   DateExpiry,
			   DateExpiryUtc,

			   DurationNumUnits,

			   DurationUnitId,

			   FreightCost
			   );

			#region Old

			//         //Reinitialise the Error and Warning Count
			//         ResetWarningAndErrorTables();

			////Validate the data supplied
			//System.Data.DataRow rowToAdd = dataToBeModified.NewRow();
			//rowToAdd["ItemId"] = ItemId;

			//rowToAdd["OrderId"] = OrderId;

			//if (ProductId != 0)
			//	rowToAdd["ProductId"] = ProductId;
			//else
			//	rowToAdd["ProductId"] = DBNull.Value;

			//if (PremiseId != 0)
			//	rowToAdd["PremiseId"] = PremiseId;
			//else
			//	rowToAdd["PremiseId"] = DBNull.Value;


			//rowToAdd["Quantity"] = Quantity;
			//rowToAdd["ItemName"] = ItemName;
			//rowToAdd["ItemCode"] = ItemCode;
			//rowToAdd["ShortDescription"] = ShortDescription;
			//rowToAdd["LongDescription"] = LongDescription;

			//if (priceShownIncludesLocalTaxRate)
			//	Cost = Cost / localTaxRate;

			//if (Weight != 0)
			//	rowToAdd["Weight"] = Weight;
			//else
			//	rowToAdd["Weight"] = DBNull.Value;

			//rowToAdd["MaxKeyholdersPerPremise"] = MaxKeyholdersPerPremise;
			//rowToAdd["MaxAssetRegisterAssets"] = MaxAssetRegisterAssets;
			//rowToAdd["MaxAssetRegisterStorage"] = MaxAssetRegisterStorage;
			//rowToAdd["MaxDocumentSafeDocuments"] = MaxDocumentSafeDocuments;
			//rowToAdd["MaxDocumentSafeStorage"] = MaxDocumentSafeStorage;

			//rowToAdd["RequiresPremiseForActivation"] = RequiresPremiseForActivation;

			//if (DateActivation == "" || DateExpiry == "")
			//{
			//	rowToAdd["DateActivation"] = DBNull.Value;
			//	rowToAdd["DateActivationUtc"] = DBNull.Value;
			//	rowToAdd["DateExpiry"] = DBNull.Value;
			//	rowToAdd["DateExpiryUtc"] = DBNull.Value;
			//}
			//else
			//{
			//	DateTime thisActivation = Convert.ToDateTime(DateActivation);
			//	DateTime thisExpiry = Convert.ToDateTime(DateExpiry); ;

			//	rowToAdd["DateActivation"] = localRecords.DBDateTime(thisActivation);
			//	rowToAdd["DateActivationUtc"] = localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(thisActivation)));
			//	rowToAdd["DateExpiry"] = localRecords.DBDateTime(thisExpiry);
			//	rowToAdd["DateActivationUtc"] = localRecords.DBDateTime(Convert.ToDateTime(FromClientToUtcTime(thisExpiry)));
			//}


			//rowToAdd["DurationNumUnits"] = DurationNumUnits;
			//rowToAdd["DurationUnitId"] = DurationUnitId;
			//rowToAdd["FreightCost"] = DBNull.Value;

			//Validate(rowToAdd, false);

			//if (NumErrors() == 0)
			//{
			//	if (UserChanges(rowToAdd))
			//		dataToBeModified.Rows.Add(rowToAdd);
			//}

			#endregion

		}

		#endregion

		#region Add/Modify

		#region Add

		/// <summary>Add</summary>
		/// <param name="OrderId">OrderId</param>
		/// <param name="ProductId">ProductId</param>
		/// <param name="PremiseId">PremiseId</param>
		/// <param name="Quantity">Quantity</param>
		/// <param name="ItemName">ItemName</param>
		/// <param name="ItemCode">ItemCode</param>
		/// <param name="ShortDescription">ShortDescription</param>
		/// <param name="LongDescription">LongDescription</param>
		/// <param name="Cost">Cost</param>
		/// <param name="Weight">Weight</param>
		/// <param name="MaxKeyholdersPerPremise">MaxKeyholdersPerPremise</param>
		/// <param name="MaxAssetRegisterAssets">MaxAssetRegisterAssets</param>
		/// <param name="MaxAssetRegisterStorage">MaxAssetRegisterStorage</param>
		/// <param name="MaxDocumentSafeDocuments">MaxDocumentSafeDocuments</param>
		/// <param name="MaxDocumentSafeStorage">MaxDocumentSafeStorage</param>
		/// <param name="RequiresPremiseForActivation">RequiresPremiseForActivation</param>
		/// <param name="DateActivation">DateActivation</param>
		/// <param name="DateActivationUtc">DateActivationUtc</param>
		/// <param name="DateExpiry">DateExpiry</param>
		/// <param name="DateExpiryUtc">DateExpiryUtc</param>
		/// <param name="DurationNumUnits">DurationNumUnits</param>
		/// <param name="DurationUnitId">DurationUnitId</param>
		/// <param name="FreightCost">FreightCost</param>
		public void Add(
		   int OrderId,

		   int ProductId,
		   int PremiseId,

		   decimal Quantity,

		   string ItemName,
		   string ItemCode,

		   string ShortDescription,
		   string LongDescription,

		   decimal Cost,
		   decimal Weight,

		   int MaxKeyholdersPerPremise,
		   int MaxAssetRegisterAssets,
		   int MaxAssetRegisterStorage,
		   int MaxDocumentSafeDocuments,
		   int MaxDocumentSafeStorage,
		   int RequiresPremiseForActivation,

		   string DateActivation,
		   string DateActivationUtc,
		   string DateExpiry,
		   string DateExpiryUtc,

		   double DurationNumUnits,

		   int DurationUnitId,

		   decimal FreightCost
		   )
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();


			rowToAdd["OrderId"] = OrderId;
			rowToAdd["ProductId"] = ProductId;
			rowToAdd["PremiseId"] = PremiseId;

			rowToAdd["Quantity"] = Quantity;

			rowToAdd["ItemName"] = ItemName;
			rowToAdd["ItemCode"] = ItemCode;

			rowToAdd["ShortDescription"] = ShortDescription;
			rowToAdd["LongDescription"] = LongDescription;

			rowToAdd["Cost"] = Cost;
			rowToAdd["Weight"] = Weight;

			rowToAdd["MaxKeyholdersPerPremise"] = MaxKeyholdersPerPremise;
			rowToAdd["MaxAssetRegisterAssets"] = MaxAssetRegisterAssets;
			rowToAdd["MaxAssetRegisterStorage"] = MaxAssetRegisterStorage;
			rowToAdd["MaxDocumentSafeDocuments"] = MaxDocumentSafeDocuments;
			rowToAdd["MaxDocumentSafeStorage"] = MaxDocumentSafeStorage;
			rowToAdd["RequiresPremiseForActivation"] = RequiresPremiseForActivation;

			rowToAdd["DateActivation"] = SanitiseDate(DateActivation);
			rowToAdd["DateActivationUtc"] = SanitiseDate(DateActivationUtc);
			rowToAdd["DateExpiry"] = SanitiseDate(DateExpiry);
			rowToAdd["DateExpiryUtc"] = SanitiseDate(DateExpiryUtc);

			rowToAdd["DurationNumUnits"] = DurationNumUnits;

			rowToAdd["DurationUnitId"] = DurationUnitId;

			rowToAdd["FreightCost"] = FreightCost;


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
		/// <param name="ItemId">ItemId (Primary Key of Record)</param>
		/// <param name="OrderId">OrderId</param>
		/// <param name="ProductId">ProductId</param>
		/// <param name="PremiseId">PremiseId</param>
		/// <param name="Quantity">Quantity</param>
		/// <param name="ItemName">ItemName</param>
		/// <param name="ItemCode">ItemCode</param>
		/// <param name="ShortDescription">ShortDescription</param>
		/// <param name="LongDescription">LongDescription</param>
		/// <param name="Cost">Cost</param>
		/// <param name="Weight">Weight</param>
		/// <param name="MaxKeyholdersPerPremise">MaxKeyholdersPerPremise</param>
		/// <param name="MaxAssetRegisterAssets">MaxAssetRegisterAssets</param>
		/// <param name="MaxAssetRegisterStorage">MaxAssetRegisterStorage</param>
		/// <param name="MaxDocumentSafeDocuments">MaxDocumentSafeDocuments</param>
		/// <param name="MaxDocumentSafeStorage">MaxDocumentSafeStorage</param>
		/// <param name="RequiresPremiseForActivation">RequiresPremiseForActivation</param>
		/// <param name="DateActivation">DateActivation</param>
		/// <param name="DateActivationUtc">DateActivationUtc</param>
		/// <param name="DateExpiry">DateExpiry</param>
		/// <param name="DateExpiryUtc">DateExpiryUtc</param>
		/// <param name="DurationNumUnits">DurationNumUnits</param>
		/// <param name="DurationUnitId">DurationUnitId</param>
		/// <param name="FreightCost">FreightCost</param>
		public void Modify(
		   int ItemId,
		   int OrderId,

		   int ProductId,
		   int PremiseId,

		   decimal Quantity,

		   string ItemName,
		   string ItemCode,

		   string ShortDescription,
		   string LongDescription,

		   decimal Cost,
		   decimal Weight,

		   int MaxKeyholdersPerPremise,
		   int MaxAssetRegisterAssets,
		   int MaxAssetRegisterStorage,
		   int MaxDocumentSafeDocuments,
		   int MaxDocumentSafeStorage,
		   int RequiresPremiseForActivation,

		   string DateActivation,
		   string DateActivationUtc,
		   string DateExpiry,
		   string DateExpiryUtc,

		   double DurationNumUnits,

		   int DurationUnitId,

		   decimal FreightCost
		   )
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();


			rowToAdd["ItemId"] = ItemId;

			rowToAdd["OrderId"] = OrderId;
			rowToAdd["ProductId"] = ProductId;
			rowToAdd["PremiseId"] = PremiseId;

			rowToAdd["Quantity"] = Quantity;

			rowToAdd["ItemName"] = ItemName;
			rowToAdd["ItemCode"] = ItemCode;

			rowToAdd["ShortDescription"] = ShortDescription;
			rowToAdd["LongDescription"] = LongDescription;

			rowToAdd["Cost"] = Cost;
			rowToAdd["Weight"] = Weight;

			rowToAdd["MaxKeyholdersPerPremise"] = MaxKeyholdersPerPremise;
			rowToAdd["MaxAssetRegisterAssets"] = MaxAssetRegisterAssets;
			rowToAdd["MaxAssetRegisterStorage"] = MaxAssetRegisterStorage;
			rowToAdd["MaxDocumentSafeDocuments"] = MaxDocumentSafeDocuments;
			rowToAdd["MaxDocumentSafeStorage"] = MaxDocumentSafeStorage;
			rowToAdd["RequiresPremiseForActivation"] = RequiresPremiseForActivation;

			rowToAdd["DateActivation"] = SanitiseDate(DateActivation);
			rowToAdd["DateActivationUtc"] = SanitiseDate(DateActivationUtc);
			rowToAdd["DateExpiry"] = SanitiseDate(DateExpiry);
			rowToAdd["DateExpiryUtc"] = SanitiseDate(DateExpiryUtc);

			rowToAdd["DurationNumUnits"] = DurationNumUnits;

			rowToAdd["DurationUnitId"] = DurationUnitId;

			rowToAdd["FreightCost"] = FreightCost;


			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);
			}

		}

		#endregion

		#endregion

		#region Validate

		/// <summary>Validate</summary>
		/// <param name="valuesToValidate">Values to be Validated.</param>
		/// <param name="newRow">Indicates whether the Row being validated 
		/// is new or already exists in the system</param>
		private void Validate(System.Data.DataRow valuesToValidate, bool newRow)
		{
			if (valuesToValidate["ProductId"] != DBNull.Value)
			{
				clsProduct thisProduct = new clsProduct(thisDbType, localRecords.dbConnection);
				thisProduct.GetByProductId(Convert.ToInt32(valuesToValidate["ProductId"]));

				//Check if this product is under stock control, and if so how many are left!
				decimal QuantityAvailable = thisProduct.my_QuantityAvailable(0);
				decimal desiredQuantity = Convert.ToDecimal(valuesToValidate["Quantity"]);
				decimal maxQuantity = thisProduct.my_MaxQuantity(0);
				bool usesStockControl = false;
				string productName = thisProduct.my_ProductName(0);
				string quantityDescription = thisProduct.my_QuantityDescription(0);

				if (quantityDescription.Length != 0)
					quantityDescription = " " + quantityDescription;

				if (productName.Length != 0)
					productName = quote + productName + quote;

				if (thisProduct.my_UseStockControl(0) == 1)
					usesStockControl = true;

				if (usesStockControl && QuantityAvailable < desiredQuantity)
				{
					System.Data.DataRow rowToAdd = errorData.NewRow();
					rowToAdd["FieldName"] = "Quantity";

					rowToAdd["Message"] = "There are only " + QuantityAvailable.ToString()
						+ quantityDescription + " of " + productName
						+ " currently available.";

					errorData.Rows.Add(rowToAdd);
				}

				//Check if the number ordered exceeds the Maximum
				if (maxQuantity > 0 && maxQuantity < desiredQuantity)
				{
					System.Data.DataRow rowToAdd = errorData.NewRow();
					rowToAdd["FieldName"] = "Quantity";
					rowToAdd["Message"] = "The maximum quanitity of " + productName
						+ " you can order is " + maxQuantity.ToString()
						+ quantityDescription + ".";

					errorData.Rows.Add(rowToAdd);
				}

			}
		}


		#endregion

		#region Update Methods

		/// <summary>
		/// Updates Item Costs (and Shipping Costs where appropriate) for an OrderId and CustomerGroupId
		/// </summary>
		/// <param name="OrderId">Id of Order to retrieve Items for</param>
		/// <param name="CustomerGroupId">Id of Customer Group to find</param>
		/// <returns>Number of resulting records</returns>
		public int UpdateItemCosts(int OrderId, int CustomerGroupId)
		{

			string update = "Update " + thisTable + "," + crLf
				+ "(select productId, price as NewCost " + crLf
				+ "	from tblProductCustomerGroupPrice " + crLf
				+ "	where CustomerGroupId = " + CustomerGroupId.ToString() + crLf
				+ ") tblProductCustomerGroupPrice" + crLf
				+ "set tblItem.Cost = NewCost" + crLf
				+ "where orderid = " + OrderId.ToString() + crLf
				+ "and tblItem.ProductId = tblProductCustomerGroupPrice.productId" + crLf;

			return localRecords.GetRecords(update);
		}

		/// <summary>
		/// Updates Item Costs (and Shipping Costs where appropriate) for an OrderId and CustomerGroupId
		/// </summary>
		/// <param name="OrderId">Id of Order to update Items for</param>
		/// <param name="CustomerGroupId">Id of Customer Group to update costs with</param>
		/// <param name="ShippingZoneId">Id of Shipping Zone to update costs with</param>
		/// <returns>Number of Records Affected</returns>
		public int UpdateItemAndFreightCosts(int OrderId, int CustomerGroupId, int ShippingZoneId)
		{

			string update = "Update " + thisTable + "," + crLf
				+ "(select productId, price as NewCost " + crLf
				+ "	from tblProductCustomerGroupPrice " + crLf
				+ "	where CustomerGroupId = " + CustomerGroupId.ToString() + crLf
				+ ") tblProductCustomerGroupPrice" + crLf;

			if (freightChargeBasis == freightChargeType.singleChargePerItem)
			{
				update += "," + crLf
					+ "(select productId, Cost as NewFreightCost " + crLf
					+ "	from tblFrSzCgP, tblFreightRule" + crLf
					+ "	where tblFrSzCgP.FreightRuleId = tblFreightRule.FreightRuleId " + crLf
					//					+ "		And Archive = 0 " + crLf
					+ "		And CustomerGroupId = " + CustomerGroupId.ToString() + crLf
					+ "		And ShippingZoneId = " + ShippingZoneId.ToString() + crLf
					+ ") tblFrSzCgP" + crLf;
			}

			update += "set tblItem.Cost = NewCost" + crLf;

			if (freightChargeBasis == freightChargeType.singleChargePerItem)
				update += "," + "tblItem.FreightCost = NewFreightCost" + crLf;

			update += "where orderid = " + OrderId.ToString() + crLf
				+ "and tblItem.ProductId = tblProductCustomerGroupPrice.productId" + crLf;

			if (freightChargeBasis == freightChargeType.singleChargePerItem)
				update += "and tblItem.ProductId = tblFrSzCgP.productId" + crLf;

			return localRecords.GetRecords(update);
		}

		/// <summary>
		/// Updates 'Problem Items' for an order
		/// </summary>
		/// <param name="OrderId">Id of Order to update Items for</param>
		/// <returns>Number of Records Affected</returns>
		public int UpdateProblemItems(int OrderId)
		{
			string update = "Update " + thisTable + "," + crLf
				+ "(select productId," + crLf
				+ MaxAllowable + " as MaxAllowable" + crLf
				+ "from tblProduct) tblProduct" + crLf;

			update += "set tblItem.Quantity = MaxAllowable" + crLf;

			update += "where orderid = " + OrderId.ToString() + crLf
				+ "and MaxAllowable > -1" + crLf
				+ "and tblItem.Quantity > tblProduct.MaxAllowable" + crLf
				+ "and tblItem.ProductId = tblProduct.productId" + crLf;

			int results = localRecords.GetRecords(update);
			int recordsToDelete = GetByQuantity(0);
			if (recordsToDelete > 0)
				results += localRecords.RemoveRecordById(thisPk, thisTable);

			return results;
		}

		/// <summary>
		/// Updates Stock Levels for completed orders
		/// </summary>
		/// <param name="OrderId">Id of Order to update Items for</param>
		/// <returns>Number of Records Affected</returns>
		public int UpdateStockLevelsOnCompletedOrder(int OrderId)
		{
			string update = "Update tblProduct," + crLf
				+ "(Select tblItem.ProductId," + crLf
				+ "tblProduct.QuantityAvailable - tblItem.Quantity as newQA" + crLf
				+ "From tblProduct, tblItem" + crLf
				+ "Where tblProduct.ProductId = tblItem.ProductId" + crLf
				+ "	And tblProduct.UseStockControl = 1" + crLf
				+ "	And tblItem.OrderId =" + OrderId.ToString() + crLf
				+ ") tblItem" + crLf;

			update += "set tblProduct.QuantityAvailable = tblItem.newQA" + crLf;

			int results = localRecords.GetRecords(update);
			return results;
		}


		#endregion

		#region Delete Methods


		/// <summary>
		/// Deletes 
		/// </summary>
		/// <param name="OrderId"></param>
		/// <returns></returns>
		public int DeleteByOrderId(int OrderId)
		{
			int numItems = GetByOrderId(OrderId);

			localRecords.RemoveRecordById(thisPk, thisTable);

			return numItems;
		}


		#endregion

		#region My_ Values Item

		/// <summary>ItemId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>ItemId</returns>
		public int my_ItemId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ItemId"));
		}


		/// <summary>OrderId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderId</returns>
		public int my_OrderId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "OrderId"));
		}


		/// <summary>ProductId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>ProductId</returns>
		public int my_ProductId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductId"));
		}


		/// <summary>PremiseId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>PremiseId</returns>
		public int my_PremiseId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PremiseId"));
		}


		/// <summary>Quantity</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Quantity</returns>
		public decimal my_Quantity(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Quantity"));
		}


		/// <summary>ItemName</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>ItemName</returns>
		public string my_ItemName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ItemName");
		}


		/// <summary>ItemCode</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>ItemCode</returns>
		public string my_ItemCode(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ItemCode");
		}


		/// <summary>ShortDescription</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>ShortDescription</returns>
		public string my_ShortDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ShortDescription");
		}


		/// <summary>LongDescription</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>LongDescription</returns>
		public string my_LongDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "LongDescription");
		}


		/// <summary>Weight</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Weight</returns>
		public decimal my_Weight(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Weight"));
		}


		/// <summary>MaxKeyholdersPerPremise</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>MaxKeyholdersPerPremise</returns>
		public int my_MaxKeyholdersPerPremise(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MaxKeyholdersPerPremise"));
		}


		/// <summary>MaxAssetRegisterAssets</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>MaxAssetRegisterAssets</returns>
		public int my_MaxAssetRegisterAssets(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MaxAssetRegisterAssets"));
		}


		/// <summary>MaxAssetRegisterStorage</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>MaxAssetRegisterStorage</returns>
		public int my_MaxAssetRegisterStorage(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MaxAssetRegisterStorage"));
		}


		/// <summary>MaxDocumentSafeDocuments</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>MaxDocumentSafeDocuments</returns>
		public int my_MaxDocumentSafeDocuments(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MaxDocumentSafeDocuments"));
		}


		/// <summary>MaxDocumentSafeStorage</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>MaxDocumentSafeStorage</returns>
		public int my_MaxDocumentSafeStorage(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MaxDocumentSafeStorage"));
		}


		/// <summary>RequiresPremiseForActivation</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>RequiresPremiseForActivation</returns>
		public int my_RequiresPremiseForActivation(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "RequiresPremiseForActivation"));
		}


		/// <summary>DateActivation</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateActivation</returns>
		public string my_DateActivation(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateActivation");
		}


		/// <summary>DateActivationUtc</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateActivationUtc</returns>
		public string my_DateActivationUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateActivationUtc");
		}


		/// <summary>DateExpiry</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateExpiry</returns>
		public string my_DateExpiry(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateExpiry");
		}


		/// <summary>DateExpiryUtc</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateExpiryUtc</returns>
		public string my_DateExpiryUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateExpiryUtc");
		}


		/// <summary>DurationNumUnits</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DurationNumUnits</returns>
		public double my_DurationNumUnits(int rowNum)
		{
			return Convert.ToDouble(localRecords.FieldByName(rowNum, "DurationNumUnits"));
		}


		/// <summary>DurationUnitId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DurationUnitId</returns>
		public int my_DurationUnitId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DurationUnitId"));
		}


		/// <summary>FreightCost</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>FreightCost</returns>
		public decimal my_FreightCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightCost"));
		}


		#endregion

		#region Special My_ Values for Item

		/// <summary>Cost</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Cost</returns>
		public decimal my_Cost(int rowNum)
		{

			//If the Cost shown includes the local tax rate, 
			//then the Cost we have been given also includes the local tax rate.
			//But we always store the Cost without tax...
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Cost")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Cost"));
		}


		/// <summary>CostIncludingTax</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CostIncludingTax</returns>
		public decimal my_CostIncludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Cost")) * localTaxRate;
		}

		/// <summary>CostExcludingTax</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CostExcludingTax</returns>
		public decimal my_CostExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Cost"));
		}

		/// <summary>InvoiceCost</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>InvoiceCost</returns>
		public decimal my_InvoiceCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Cost"));
		}

		/// <summary>InvoiceCost</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>InvoiceCost</returns>
		public string my_DurationUnitTypeName(int rowNum)
		{
			return durationUnitTypeName(my_DurationUnitId(rowNum));
		}

		#endregion

		#region My_ Values Product

		/// <summary>
		/// <see cref="clsDefinedContent.my_DefinedContentId">Id</see> of 
		/// <see cref="clsDefinedContent">DefinedContent</see>
		/// Associated with this Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_DefinedContentId">Id</see> 
		/// of <see cref="clsDefinedContent">DefinedContent</see> 
		/// for this Product</returns>	
		public int my_Product_DefinedContentId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_DefinedContentId"));
		}


		/// <summary>
		/// <see cref="clsProduct.my_DisplayTypeId">Product Display Type</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_DisplayTypeId">Product Display Type</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// </returns>
		public int my_Product_DisplayTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_DisplayTypeId"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_ProductName">Product Name</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductName">Product Name</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// </returns>
		public string my_Product_ProductName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Product_ProductName");
		}

		/// <summary>
		/// <see cref="clsProduct.my_ProductCode">Product Code</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductCode">Product Code</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// </returns>
		public string my_Product_ProductCode(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Product_ProductCode");
		}

		/// <summary>
		/// <see cref="clsProduct.my_QuantityDescription">Quantity Description</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_QuantityDescription">Quantity Description</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// </returns>
		public string my_Product_QuantityDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Product_QuantityDescription");
		}

		/// <summary>
		/// Whether this 
		/// <see cref="clsProduct">Product</see> uses
		/// <see cref="clsProduct.my_UseStockControl">Stock Control</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this 
		/// <see cref="clsProduct">Product</see> uses 
		/// <see cref="clsProduct.my_UseStockControl"> Stock Control</see></returns>
		public int my_Product_UseStockControl(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_UseStockControl"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_QuantityAvailable">Quantity Available</see> of 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_QuantityAvailable">Quantity Available</see> of 
		/// <see cref="clsProduct">Product</see></returns>
		public decimal my_Product_QuantityAvailable(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Product_QuantityAvailable"));
		}


		/// <summary>
		/// Whether this 
		/// <see cref="clsProduct">Product</see>  is
		/// <see cref="clsProduct.my_WholeNumbersOnly">discrete or continuous</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this 
		/// <see cref="clsProduct">Product</see>  is 
		/// <see cref="clsProduct.my_WholeNumbersOnly">discrete or continuous</see></returns>
		public int my_Product_WholeNumbersOnly(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_WholeNumbersOnly"));
		}

		/// <summary>
		/// Whether this 
		/// <see cref="clsProduct">Product</see>  is
		/// <see cref="clsProduct.my_ProductOnSpecial">on special or not</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this 
		/// <see cref="clsProduct">Product</see>  is 
		/// <see cref="clsProduct.my_ProductOnSpecial">on special or not</see></returns>
		public int my_Product_ProductOnSpecial(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_ProductOnSpecial"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_MaxKeyholdersPerPremise">Maximum number of Keyholders per Premise</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsProduct.my_MaxKeyholdersPerPremise">Maximum number of Keyholders per Premise</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see></returns>
		public int my_Product_MaxKeyholdersPerPremise(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_MaxKeyholdersPerPremise"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_MaxAssetRegisterAssets">Maximum number of Assets in the Asset Register</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsProduct.my_MaxAssetRegisterAssets">Maximum number of Assets in the Asset Register</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see></returns>	
		public int my_Product_MaxAssetRegisterAssets(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_MaxAssetRegisterAssets"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_MaxAssetRegisterStorage">Maximum Total Storage of Asset Register</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsProduct.my_MaxAssetRegisterStorage">Maximum Total Storage of Asset Register</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see></returns>	
		public int my_Product_MaxAssetRegisterStorage(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_MaxAssetRegisterStorage"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_MaxDocumentSafeDocuments">Maximum number of Documents in the Document Safe</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsProduct.my_MaxDocumentSafeDocuments">Maximum number of Documents in the Document Safe</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see></returns>	
		public int my_Product_MaxDocumentSafeDocuments(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_MaxDocumentSafeDocuments"));
		}


		/// <summary>
		/// <see cref="clsProduct.my_MaxDocumentSafeStorage">Maximum number of Storage in the Document Safe</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsProduct.my_MaxDocumentSafeStorage">Maximum number of Storage in the Document Safe</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see></returns>	
		public int my_Product_MaxDocumentSafeStorage(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_MaxDocumentSafeStorage"));
		}

		/// <summary><see cref="clsProduct.my_DurationUnitId">Time Unit's e.g. year, month, day 
		/// (if this is a Service over time)</see> for this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_DurationUnitId">Time Unit's e.g. year, month, day 
		/// (if this is a Service over time</see>) for this 
		/// <see cref="clsProduct">Product</see>
		/// </returns>	
		public int my_Product_DurationUnitId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_DurationUnitId"));
		}


		/// <summary>
		/// <see cref="clsProduct.my_DurationNumUnits">Number of Unit's duration</see> (if this is a Service over time) for this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_DurationNumUnits">Number of Unit's duration</see> (if this is a Service over time) for this 
		/// <see cref="clsProduct">Product</see>
		/// </returns>
		public decimal my_Product_DurationNumUnits(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Product_DurationNumUnits"));
		}


		/// <summary>Whether this <see cref="clsProduct">Product</see> 
		/// <see cref="clsProduct.my_RequiresPremiseForActivation">Requires an associated Premise in order to be Activated</see>  
		/// (if this is a Premise/Time Related Product)</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this <see cref="clsProduct">Product</see> 
		/// <see cref="clsProduct.my_RequiresPremiseForActivation">Requires an associated Premise in order to be Activated</see>  
		/// (if this is a Premise/Time Related Product)</returns>	
		public int my_Product_RequiresPremiseForActivation(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Product_RequiresPremiseForActivation"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_MaxQuantity">Maximum Quantity of Product Able to be purchased in a single order</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsProduct.my_MaxQuantity">Maximum Quantity of Product Able to be purchased in a single order</see>
		///  allowed for this 
		/// <see cref="clsProduct">Product</see></returns>
		public decimal my_Product_MaxQuantity(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Product_MaxQuantity"));
		}


		/// <summary>
		/// <see cref="clsProduct.my_MaxAllowable">Maximum Quantity/Number of Stock purchasable at this time</see>
		///  for this 
		/// <see cref="clsProduct">Product</see> (uses MaxQuantity, UsesStockControl and QuantityAvailable to calculate this)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsProduct.my_MaxAllowable">Maximum Quantity/Number of Stock purchasable at this time</see>
		///  for this 
		/// <see cref="clsProduct">Product</see> (uses MaxQuantity, UsesStockControl and QuantityAvailable to calculate this)
		/// </returns>	
		public decimal my_Product_MaxAllowable(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Product_MaxAllowable"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_Weight">Weight</see>
		///  of this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsProduct.my_Weight">Weight</see>
		///  of this 
		/// <see cref="clsProduct">Product</see></returns>	
		public decimal my_Product_Weight(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Product_Weight"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_ShortDescription">Short Description</see>
		///  of this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsProduct.my_ShortDescription">Short Description</see>
		///  of this 
		/// <see cref="clsProduct">Product</see></returns>	
		public string my_Product_ShortDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Product_ShortDescription");
		}



		#endregion

		#region My_ Values Premise

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">Id</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public int my_Premise_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_CustomerId"));
		}

		/// <summary>
		/// <see cref="clsItem.my_ItemId">Id</see> of
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_ItemId">Id</see> of 
		/// <see cref="clsItem">Item</see> 
		/// </returns>
		public int my_Premise_ItemId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_ItemId"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_ProductId">Id</see> of
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductId">Id</see> of 
		/// <see cref="clsProduct">Product</see> 
		/// </returns>
		public int my_Premise_ProductId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_ProductId"));
		}


		/// <summary>
		/// <see cref="clsCompanyType.my_CompanyTypeId">Id</see> of
		/// <see cref="clsCompanyType">CompanyType</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCompanyType.my_CompanyTypeId">Id</see> of 
		/// <see cref="clsCompanyType">CompanyType</see> 
		/// </returns>
		public int my_Premise_CompanyTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_CompanyTypeId"));
		}

		/// <summary>
		/// <see cref="clsPremise.my_PremiseNumber">Number</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_PremiseNumber">Number</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_PremiseNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_PremiseNumber");
		}

		/// <summary>
		/// <see cref="clsPremise.my_Url">Url</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_Url">Url</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_Url(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_Url");
		}

		/// <summary>
		/// <see cref="clsPremise.my_QuickPhysicalAddress">Quick Physical Address</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_QuickPhysicalAddress">Quick Physical Address</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>	
		public string my_Premise_QuickPhysicalAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_QuickPhysicalAddress");
		}

		/// <summary>
		/// <see cref="clsPremise.my_CompanyAtPremiseName">Name of Company</see> at
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_CompanyAtPremiseName">Name of Company</see> at 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_CompanyAtPremiseName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_CompanyAtPremiseName");
		}


		/// <summary>
		/// <see cref="clsPremise.my_AlarmDetails">Details of Alarm</see> at
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_AlarmDetails">Details of Alarm</see> at 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_AlarmDetails(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_AlarmDetails");
		}

		/// <summary>
		/// <see cref="clsPremise.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_KdlComments");
		}

		/// <summary>
		/// <see cref="clsPremise.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_CustomerComments");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DateSubscriptionExpires">Date Next Subscription is Due</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateSubscriptionExpires">Date Next Subscription is Due</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_DateSubscriptionExpires(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_DateSubscriptionExpires");
		}


		/// <summary>
		/// <see cref="clsPremise.my_DateNextSubscriptionDueToBeInvoiced">Date Next Subscription is due to be invoiced</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateNextSubscriptionDueToBeInvoiced">Date Next Subscription is due to be invoiced</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_DateNextSubscriptionDueToBeInvoiced(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_DateNextSubscriptionDueToBeInvoiced");
		}


		/// <summary>
		/// <see cref="clsPremise.my_DateLastDetailsUpdate">Date Last Detail Update was sent</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateLastDetailsUpdate">Date Last Detail Update was sent</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_DateLastDetailsUpdate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_DateLastDetailsUpdate");
		}

		/// <summary>
		/// <see cref="clsPremise.my_StickerRequired">Whether a Sticker is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_StickerRequired">Whether a Sticker is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_StickerRequired(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_StickerRequired");
		}

		/// <summary>
		/// <see cref="clsPremise.my_InvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_InvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_InvoiceRequired(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_InvoiceRequired");
		}

		/// <summary>
		/// <see cref="clsPremise.my_CopyOfInvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_CopyOfInvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_CopyOfInvoiceRequired(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_CopyOfInvoiceRequired");
		}


		/// <summary>
		/// <see cref="clsPremise.my_StatementRequired">Whether a Statement is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_StatementRequired">Whether a Statement is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_StatementRequired(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_StatementRequired");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DetailsUpdateRequired">Whether a DetailsUpdate is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DetailsUpdateRequired">Whether a DetailsUpdate is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_DetailsUpdateRequired(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_DetailsUpdateRequired");
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

		#region My_ Values Order

		/// <summary>OrderId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderId</returns>
		public int my_Order_OrderId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_OrderId"));
		}


		/// <summary>CustomerId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CustomerId</returns>
		public int my_Order_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_CustomerId"));
		}


		/// <summary>PersonId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>PersonId</returns>
		public int my_Order_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_PersonId"));
		}


		/// <summary>PaymentMethodTypeId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>PaymentMethodTypeId</returns>
		public int my_Order_PaymentMethodTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_PaymentMethodTypeId"));
		}


		/// <summary>CustomerGroupId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CustomerGroupId</returns>
		public int my_Order_CustomerGroupId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_CustomerGroupId"));
		}


		/// <summary>OrderNum</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderNum</returns>
		public string my_Order_OrderNum(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_OrderNum");
		}


		/// <summary>CustomerType</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CustomerType</returns>
		public int my_Order_CustomerType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_CustomerType"));
		}


		/// <summary>FullName</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>FullName</returns>
		public string my_Order_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_FullName");
		}


		/// <summary>Title</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Title</returns>
		public string my_Order_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_Title");
		}


		/// <summary>FirstName</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>FirstName</returns>
		public string my_Order_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_FirstName");
		}


		/// <summary>LastName</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>LastName</returns>
		public string my_Order_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_LastName");
		}


		/// <summary>QuickPostalAddress</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickPostalAddress</returns>
		public string my_Order_QuickPostalAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickPostalAddress");
		}


		/// <summary>QuickDaytimePhone</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickDaytimePhone</returns>
		public string my_Order_QuickDaytimePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickDaytimePhone");
		}


		/// <summary>QuickDaytimeFax</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickDaytimeFax</returns>
		public string my_Order_QuickDaytimeFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickDaytimeFax");
		}


		/// <summary>QuickAfterHoursPhone</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickAfterHoursPhone</returns>
		public string my_Order_QuickAfterHoursPhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickAfterHoursPhone");
		}


		/// <summary>QuickAfterHoursFax</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickAfterHoursFax</returns>
		public string my_Order_QuickAfterHoursFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickAfterHoursFax");
		}


		/// <summary>QuickMobilePhone</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>QuickMobilePhone</returns>
		public string my_Order_QuickMobilePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_QuickMobilePhone");
		}


		/// <summary>CountryId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CountryId</returns>
		public int my_Order_CountryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_CountryId"));
		}


		/// <summary>Email</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Email</returns>
		public string my_Order_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_Email");
		}


		/// <summary>OrderSubmitted</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderSubmitted</returns>
		public int my_Order_OrderSubmitted(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_OrderSubmitted"));
		}


		/// <summary>OrderPaid</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderPaid</returns>
		public int my_Order_OrderPaid(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_OrderPaid"));
		}


		/// <summary>OrderCreatedMechanism</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderCreatedMechanism</returns>
		public int my_Order_OrderCreatedMechanism(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_OrderCreatedMechanism"));
		}


		/// <summary>OrderStatusId</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderStatusId</returns>
		public int my_Order_OrderStatusId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_OrderStatusId"));
		}


		/// <summary>SupplierComment</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>SupplierComment</returns>
		public string my_Order_SupplierComment(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_SupplierComment");
		}


		/// <summary>DateCreated</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateCreated</returns>
		public string my_Order_DateCreated(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateCreated");
		}


		/// <summary>DateCreatedUtc</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateCreatedUtc</returns>
		public string my_Order_DateCreatedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateCreatedUtc");
		}


		/// <summary>DateSubmitted</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateSubmitted</returns>
		public string my_Order_DateSubmitted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateSubmitted");
		}


		/// <summary>DateSubmittedUtc</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateSubmittedUtc</returns>
		public string my_Order_DateSubmittedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateSubmittedUtc");
		}


		/// <summary>DateProcessed</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateProcessed</returns>
		public string my_Order_DateProcessed(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateProcessed");
		}


		/// <summary>DateProcessedUtc</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateProcessedUtc</returns>
		public string my_Order_DateProcessedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateProcessedUtc");
		}


		/// <summary>DateShipped</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateShipped</returns>
		public string my_Order_DateShipped(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateShipped");
		}


		/// <summary>DateShippedUtc</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateShippedUtc</returns>
		public string my_Order_DateShippedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateShippedUtc");
		}


		/// <summary>DateDue</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateDue</returns>
		public string my_Order_DateDue(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateDue");
		}


		/// <summary>InvoiceRequested</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>InvoiceRequested</returns>
		public int my_Order_InvoiceRequested(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_InvoiceRequested"));
		}


		/// <summary>DateInvoiceLastPrinted</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>DateInvoiceLastPrinted</returns>
		public string my_Order_DateInvoiceLastPrinted(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Order_DateInvoiceLastPrinted");
		}


		/// <summary>TaxAppliedToOrder</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TaxAppliedToOrder</returns>
		public decimal my_Order_TaxAppliedToOrder(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_TaxAppliedToOrder"));
		}


		/// <summary>TaxRateAtTimeOfOrder</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TaxRateAtTimeOfOrder</returns>
		public decimal my_Order_TaxRateAtTimeOfOrder(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_TaxRateAtTimeOfOrder"));
		}


		///// <summary>TaxCost</summary>
		///// <param name="rowNum">Record Index</param>
		///// <returns>TaxCost</returns>
		//public decimal my_Order_TaxCost(int rowNum)
		//{
		//	return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_TaxCost"));
		//}


		///// <summary>FreightCost</summary>
		///// <param name="rowNum">Record Index</param>
		///// <returns>FreightCost</returns>
		//public decimal my_Order_FreightCost(int rowNum)
		//{
		//	return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_FreightCost"));
		//}


		///// <summary>Total</summary>
		///// <param name="rowNum">Record Index</param>
		///// <returns>Total</returns>
		//public decimal my_Order_Total(int rowNum)
		//{
		//	return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_Total"));
		//}


		/// <summary>TotalItemWeight</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TotalItemWeight</returns>
		public decimal my_Order_TotalItemWeight(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_TotalItemWeight"));
		}


		/// <summary>TotalItemCost</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TotalItemCost</returns>
		public decimal my_Order_TotalItemCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_TotalItemCost"));
		}


		/// <summary>TotalItemFreightCost</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TotalItemFreightCost</returns>
		public decimal my_Order_TotalItemFreightCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_TotalItemFreightCost"));
		}


		/// <summary>IsInvoiceOrder</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>IsInvoiceOrder</returns>
		public int my_Order_IsInvoiceOrder(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_IsInvoiceOrder"));
		}


		/// <summary>NumItems</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>NumItems</returns>
		public int my_Order_NumItems(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Order_NumItems"));
		}


		#endregion

		#region My_ Values Order

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



		/// <summary>TotalItemCostExcludingTax</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TotalItemCostExcludingTax</returns>
		public decimal my_Order_TotalItemCostExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Order_TotalItemCost"));
		}


		#endregion
	}
}
