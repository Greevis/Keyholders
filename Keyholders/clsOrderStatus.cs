using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsOrderStatus deals with everything to do with data about the Status of Orders.
	/// </summary>

	[GuidAttribute("79E2EEC5-39B9-4cfb-971E-362F62A17729")]
	public class clsOrderStatus : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsOrderStatus
		/// </summary>
		public clsOrderStatus() : base("OrderStatus")
		{
		}

		/// <summary>
		/// Constructor for clsOrderStatus; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsOrderStatus(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("OrderStatus")
		{
			Connect(typeOfDb, odbcConnection);
		}


		/// <summary>
		/// Part of the Query that Pertains to Order Summary Information
		/// </summary>
		public clsQueryPart OrderSummaryQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = OrderStatusQueryPart();
			
			OrderSummaryQ.AddSelectColumn("tblOrder.TotalOrders");

			OrderSummaryQ.AddFromTable("(Select tblOrderStatus.OrderStatusId," + crLf
				+ "count(OrderId) as TotalOrders" + crLf
				+ "from tblOrderStatus left outer join tblOrder " + crLf
				+ "on tblOrderStatus.OrderStatusId = tblOrder.OrderStatusId " + crLf
				+ "Group by tblOrderStatus.OrderStatusId) tblOrder");

			OrderSummaryQ.AddJoin("tblOrderStatus.OrderStatusId = tblOrder.OrderStatusId");

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[2];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = OrderSummaryQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);

			orderBySqlQuery = "Order By SupplierFacingName, CustomerFacingDescription, SupplierFacingDescription" + crLf;
		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsOrderStatus
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("SupplierFacingDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerFacingDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("SupplierFacingName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerFacingName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("IsPublic", System.Type.GetType("System.Int64"));
			
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
		/// Initialises an internal list of all Currencies
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = thisTable + ".IsPublic = 1" + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + condition + ") " + thisTable,
				thisTable
				);

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a Supplier Status by OrderStatusId
		/// </summary>
		/// <param name="OrderStatusId">Id of Supplier Status to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByOrderStatusId(int OrderStatusId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
		
			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ OrderStatusId.ToString() + ") " + thisTable,
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
		/// internal OrderStatus table stack; the SaveCurrencies method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="SupplierFacingDescription">OrderStatus's Supplier Facing Description</param>
		/// <param name="CustomerFacingDescription">OrderStatus's Customer Facing Description</param>
		/// <param name="SupplierFacingName">Supplier Facing Name for this Status</param>
		/// <param name="CustomerFacingName">Customer Facing Name for this Status</param>
		public void Add(string SupplierFacingDescription, 
			string CustomerFacingDescription,
			string SupplierFacingName,
			string CustomerFacingName)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["SupplierFacingDescription"] = SupplierFacingDescription;
			rowToAdd["CustomerFacingDescription"] = CustomerFacingDescription;
			rowToAdd["SupplierFacingName"] = SupplierFacingName;
			rowToAdd["CustomerFacingName"] = CustomerFacingName;
			rowToAdd["IsPublic"] = 1;
			
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
		/// internal OrderStatus table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="OrderStatusId">OrderStatusId (Primary Key of Record)</param>
		/// <param name="SupplierFacingDescription">OrderStatus's Supplier Facing Description</param>
		/// <param name="CustomerFacingDescription">OrderStatus's Customer Facing Description</param>
		/// <param name="SupplierFacingName">Supplier Facing Name for this Status</param>
		/// <param name="CustomerFacingName">Customer Facing Name for this Status</param>
		public void Modify(int OrderStatusId, 
			string SupplierFacingDescription, 
			string CustomerFacingDescription,
			string SupplierFacingName,
			string CustomerFacingName)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			clsOrderStatus thisOrderStatus = new clsOrderStatus(thisDbType, localRecords.dbConnection);
			thisOrderStatus.GetByOrderStatusId(OrderStatusId);

			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["OrderStatusId"] = OrderStatusId;
			rowToAdd["SupplierFacingDescription"] = SupplierFacingDescription;
			rowToAdd["CustomerFacingDescription"] = CustomerFacingDescription;
			rowToAdd["SupplierFacingName"] = SupplierFacingName;
			rowToAdd["CustomerFacingName"] = CustomerFacingName;
			rowToAdd["IsPublic"] = 1;

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

		# region My_ Values


		/// <summary>
		/// OrderStatusId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>OrderStatusId for this Row</returns>
		public int my_OrderStatusId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "OrderStatusId"));
		}

		/// <summary>
		/// OrderStatus's Supplier Facing Description
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Supplier Facing Description of Supplier Status for this Row</returns>
		public string my_SupplierFacingDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "SupplierFacingDescription");
		}

		/// <summary>
		/// OrderStatus's Customer Facing Description
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer Facing Description of Supplier Status for this Row</returns>
		public string my_CustomerFacingDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerFacingDescription");
		}

		/// <summary>
		/// Supplier Facing Name for this Status
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>SupplierFacingName of Supplier Status for this Row</returns>
		public string my_SupplierFacingName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "SupplierFacingName");
		}

		/// <summary>
		/// Customer Facing Name for this Status
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CustomerFacingName of Customer Status for this Row</returns>
		public string my_CustomerFacingName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerFacingName");
		}

		/// <summary>Total number of 
		/// <see cref="clsOrder">Orders</see> 
		/// Associated with this Shipping Zone</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total number of 
		/// <see cref="clsOrder">Orders</see> of 
		/// Associated with this Shipping Zone</returns>
		public int my_TotalOrders(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalOrders"));
		}


		#endregion
	}
}
