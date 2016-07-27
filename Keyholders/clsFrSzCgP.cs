using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsFrSzCgP deals with everything to do with data about FrSzCgPs.
	/// </summary>

	[GuidAttribute("6632597D-71A0-4eaa-AA70-63984D2A8068")]
	public class clsFrSzCgP : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsFrSzCgP
		/// </summary>
		public clsFrSzCgP() : base("FrSzCgP")
		{
		}

		/// <summary>
		/// Constructor for clsFrSzCgP; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsFrSzCgP(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("FrSzCgP")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to FreightRule Information
		/// </summary>
		public clsQueryPart FreightRuleQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to FreightRule CustomerGroup Information
		/// </summary>
		public clsQueryPart CustomerGroupQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to FreightRule ShippingZone Information
		/// </summary>
		public clsQueryPart ShippingZoneQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Product Information
		/// </summary>
		public clsQueryPart ProductQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			ShippingZoneQ = ShippingZoneQueryPart();
			CustomerGroupQ = CustomerGroupQueryPart();
			ProductQ = ProductQueryPart();
			FreightRuleQ = FreightRuleQueryPart();

			MainQ.AddSelectColumn("tblFrSzCgP.FrSzCgPId");
			MainQ.AddSelectColumn("tblFrSzCgP.FreightRuleId");
			MainQ.AddSelectColumn("tblFrSzCgP.CustomerGroupId");
			MainQ.AddSelectColumn("tblFrSzCgP.ShippingZoneId");
			MainQ.AddSelectColumn("tblFrSzCgP.ProductId");
			MainQ.AddSelectColumn("tblFrSzCgP.Cost");

			MainQ.AddFromTable(thisTable);

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[5];
			
			queries[0] = MainQ;
			queries[1] = FreightRuleQ;
			queries[2] = CustomerGroupQ;
			queries[3] = ShippingZoneQ;
			queries[4] = ProductQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblFrSzCgP.Cost" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsFrSzCgP
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("FreightRuleId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CustomerGroupId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ShippingZoneId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ProductId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("Cost", System.Type.GetType("System.Int32"));

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

			//Need to get the tax rate so that we can return the price with tax
			GetGeneralSettings();

		}

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all FrSzCgPs
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = FreightRuleQ;
			queries[2] = CustomerGroupQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets an Freight Rule-Shipping Zone-Customer Group-Price by FrSzCgPId
		/// </summary>
		/// <param name="FrSzCgPId">Id for Freight Rule-Shipping Zone-Customer Group-Price</param>
		/// <returns>Number of resulting records</returns>
		public int GetByFrSzCgPId(int FrSzCgPId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = FreightRuleQ;
			queries[2] = CustomerGroupQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ FrSzCgPId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Freight Rule-Shipping Zone-Customer Group-Prices by ShippingZoneId, CustomerGroupId and ProductId
		/// </summary>
		/// <param name="ShippingZoneId">Id of Shipping Zone to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <param name="CustomerGroupId">Id of Customer Group to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <param name="ProductId">ProductId for Freight Rule-Shipping Zone-Customer Group-Prices</param>
		/// <returns>Number of resulting records</returns>
		public int GetByShippingZoneIdCustomerGroupIdProductId(int ShippingZoneId, int CustomerGroupId, int ProductId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = FreightRuleQ;

			queries[1].AddJoin("tblFreightRule.Archive = 0");

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblFrSzCgP.ShippingZoneId = " 
				+ ShippingZoneId.ToString() + crLf;

			condition += "And tblFrSzCgP.CustomerGroupId = " 
				+ CustomerGroupId.ToString() + crLf;

			condition += "And tblFrSzCgP.ProductId = "
				+ ProductId.ToString() + ") " + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition + thisTable,
				thisTable
				);


			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Freight Rule-Shipping Zone-Customer Group-Prices by ShippingZoneId, CustomerGroupId and Value
		/// </summary>
		/// <param name="ShippingZoneId">Id of Shipping Zone to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <param name="CustomerGroupId">Id of Customer Group to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <param name="Value">Value for Freight Rule-Shipping Zone-Customer Group-Prices</param>
		/// <returns>Number of resulting records</returns>
		public int GetByShippingZoneIdCustomerGroupIdValue(int ShippingZoneId, int CustomerGroupId, decimal Value)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = FreightRuleQ;

			queries[1].AddJoin("tblFreightRule.Archive = 0");
			queries[1].AddJoin("(MinTotalValue = 0"
				+ " OR MinTotalValue < " + Value.ToString() + ")");
			queries[1].AddJoin("(MaxTotalValue = 0"
				+ " OR MaxTotalValue > " + Value.ToString() + ")");

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblFrSzCgP.ShippingZoneId = " 
				+ ShippingZoneId.ToString() + crLf;

			condition += "And tblFrSzCgP.CustomerGroupId = " 
				+ CustomerGroupId.ToString() + crLf;
			
			condition += ") " + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition + thisTable,
				thisTable
				);
			
			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Freight Rule-Shipping Zone-Customer Group-Prices by ShippingZoneId, CustomerGroupId and Weight
		/// </summary>
		/// <param name="ShippingZoneId">Id of Shipping Zone to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <param name="CustomerGroupId">Id of Customer Group to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <param name="Weight">Weight for Freight Rule-Shipping Zone-Customer Group-Prices</param>
		/// <returns>Number of resulting records</returns>
		public int GetByShippingZoneIdCustomerGroupIdWeight(int ShippingZoneId, int CustomerGroupId, decimal Weight)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = FreightRuleQ;

			queries[1].AddJoin("tblFreightRule.Archive = 0");
			queries[1].AddJoin("(tblFrSzCgP.MinTotalWeight = 0"
				+ " OR tblFrSzCgP.MinTotalWeight < " + Weight.ToString() + ")");
			queries[1].AddJoin("(MaxTotalWeight = 0"
				+ " OR MaxTotalWeight > " + Weight.ToString() + ")");

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblFrSzCgP.ShippingZoneId = " 
				+ ShippingZoneId.ToString() + crLf;

			condition += "And tblFrSzCgP.CustomerGroupId = " 
				+ CustomerGroupId.ToString() + crLf;

			condition += ") " + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition + thisTable,
				thisTable
				);

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Freight Rule-Shipping Zone-Customer Group-Prices by ShippingZoneId, CustomerGroupId
		/// </summary>
		/// <param name="ShippingZoneId">Id of Shipping Zone to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <param name="CustomerGroupId">Id of Customer Group to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByShippingZoneIdCustomerGroupId(int ShippingZoneId, int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblFrSzCgP.ShippingZoneId = " 
				+ ShippingZoneId.ToString() + crLf;

			condition += "And tblFrSzCgP.CustomerGroupId = " 
				+ CustomerGroupId.ToString() + ") " + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition + thisTable,
				thisTable
				);


			return localRecords.GetRecords(thisSqlQuery);

		}

		/// <summary>
		/// Gets Freight Rule-Shipping Zone-Customer Group-Prices by FreightRuleId And CustomerGroupId
		/// </summary>
		/// <param name="FreightRuleId">Id of Freight Rule to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <param name="CustomerGroupId">Id of Customer Group to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByFreightRuleIdCustomerGroupId(int FreightRuleId, int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblFrSzCgP.FreightRuleId = " 
				+ FreightRuleId.ToString() + crLf;

			condition += "And tblFrSzCgP.CustomerGroupId = " 
				+ CustomerGroupId.ToString() + ") " + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition + thisTable,
				thisTable
				);


			return localRecords.GetRecords(thisSqlQuery);
		}


		
		/// <summary>
		/// Gets Freight Rule-Shipping Zone-Customer Group-Prices by FreightRuleId
		/// </summary>
		/// <param name="FreightRuleId">Id of Freight Rule to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByFreightRuleId(int FreightRuleId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = FreightRuleQ;
			queries[2] = CustomerGroupQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblFrSzCgP.FreightRuleId = " 
				+ FreightRuleId.ToString() + crLf;

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
		/// Gets Freight Rule-Shipping Zone-Product-Prices by ProductId
		/// </summary>
		/// <param name="ProductId">Id of Product to retrieve Freight Rule-Shipping Zone-Product-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductId(int ProductId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ProductQ;
			queries[2] = FreightRuleQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblFrSzCgP.ProductId = " 
				+ ProductId.ToString() + crLf;

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
		/// Gets Freight Rule-Shipping Zone-Shipping Zone-Prices by ShippingZoneId
		/// </summary>
		/// <param name="ShippingZoneId">Id of Shipping Zone to retrieve Freight Rule-Shipping Zone-Shipping Zone-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByShippingZoneId(int ShippingZoneId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ShippingZoneQ;
			queries[2] = FreightRuleQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblFrSzCgP.ShippingZoneId = " 
				+ ShippingZoneId.ToString() + crLf;

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
		/// Gets Freight Rule-Shipping Zone-Customer Group-Prices by CustomerGroupId
		/// </summary>
		/// <param name="CustomerGroupId">Id of Customer Group to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerGroupId(int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = CustomerGroupQ;
			queries[2] = FreightRuleQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblFrSzCgP.CustomerGroupId = " 
				+ CustomerGroupId.ToString() + crLf;

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
		/// <param name="FreightRuleId">FreightRule Associated with this Freight Rule-Shipping Zone-Customer Group-Product</param>
		/// <param name="ShippingZoneId">Shipping Zone Associated with this Freight Rule-Shipping Zone-Customer Group-Product</param>
		/// <param name="CustomerGroupId">Customer Group Associated with this Freight Rule-Shipping Zone-Customer Group-Product</param>
		/// <param name="ProductId">Product Associated with this Freight Rule-Shipping Zone-Customer Group-Product</param>
		/// <param name="Cost">Cost for this Freight Rule-Shipping Zone-Customer Group-Product</param>
		public void Add(int FreightRuleId,
			int ShippingZoneId,
			int CustomerGroupId,
			int ProductId,
			decimal Cost)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			if (ShippingZoneId == 0) 
				rowToAdd["ShippingZoneId"] = DBNull.Value;
			else
				rowToAdd["ShippingZoneId"] = ShippingZoneId;

			
			if (CustomerGroupId == 0)
				rowToAdd["CustomerGroupId"] = DBNull.Value;
			else
				rowToAdd["CustomerGroupId"] = CustomerGroupId;

			if (ProductId == 0) 
				rowToAdd["ProductId"] = DBNull.Value;
			else
				rowToAdd["ProductId"] = ProductId;
			
			rowToAdd["FreightRuleId"] = FreightRuleId;

			if (priceShownIncludesLocalTaxRate)
				Cost = Cost / localTaxRate;


			rowToAdd["Cost"] = Cost;
			
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
		/// <param name="FrSzCgPId">FrSzCgPId (Primary Key of Record)</param>
		/// <param name="FreightRuleId">FreightRule Associated with this Freight Rule-Shipping Zone-Customer Group-Product</param>
		/// <param name="ShippingZoneId">Shipping Zone Associated with this Freight Rule-Shipping Zone-Customer Group-Product</param>
		/// <param name="CustomerGroupId">Customer Group Associated with this Freight Rule-Shipping Zone-Customer Group-Product</param>
		/// <param name="ProductId">Product Associated with this Freight Rule-Shipping Zone-Customer Group-Product</param>
		/// <param name="Cost">Cost for this Freight Rule-Shipping Zone-Customer Group-Product</param>
		public void Modify(int FrSzCgPId, 
			int FreightRuleId,
			int ShippingZoneId,
			int CustomerGroupId,
			int ProductId,
			decimal Cost)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["FrSzCgPId"] = FrSzCgPId;
			if (ShippingZoneId == 0) 
				rowToAdd["ShippingZoneId"] = DBNull.Value;
			else
				rowToAdd["ShippingZoneId"] = ShippingZoneId;

			
			if (CustomerGroupId == 0)
				rowToAdd["CustomerGroupId"] = DBNull.Value;
			else
				rowToAdd["CustomerGroupId"] = CustomerGroupId;

			if (ProductId == 0) 
				rowToAdd["ProductId"] = DBNull.Value;
			else
				rowToAdd["ProductId"] = ProductId;
			
			rowToAdd["FreightRuleId"] = FreightRuleId;

			if (priceShownIncludesLocalTaxRate)
				Cost = Cost / localTaxRate;
			
			rowToAdd["Cost"] = Cost;

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

		# region My_ Values FrSzCgP


		/// <summary>
		/// FrSzCgPId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>FrSzCgPId for this Row</returns>
		public int my_FrSzCgPId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "FrSzCgPId"));
		}

		/// <summary>
		/// Cost of this Freight Rule Application
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Cost of this Freight Rule Application</returns>
		public decimal my_Cost(int rowNum)
		{
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Cost")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Cost"));
		}

		/// <summary>
		/// Cost of this Freight Rule Application (Excluding Tax)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Cost of this Freight Rule Application (Excluding Tax)</returns>
		public decimal my_CostExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Cost"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_FreightRuleId">FreightRuleId</see> of 
		/// <see cref="clsFreightRule">FreightRule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_FreightRuleId">Id</see> 
		/// of <see cref="clsFreightRule">FreightRule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product</returns>
		public int my_FreightRuleId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "FreightRuleId"));
		}

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupId">CustomerGroupId</see> of 
		/// <see cref="clsCustomerGroup">CustomerGroup</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupId">Id</see> 
		/// of <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product</returns>
		public int my_CustomerGroupId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerGroupId"));
		}

		/// <summary>
		/// <see cref="clsShippingZone.my_ShippingZoneId">ShippingZoneId</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see>
		/// Associated with this Country</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsShippingZone.my_ShippingZoneId">Id</see> 
		/// of <see cref="clsShippingZone">ShippingZone</see> 
		/// for this Country</returns>	
		public int my_ShippingZoneId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ShippingZoneId"));
		}

		#endregion

		# region My_ Values FreightRule

		/// <summary>
		/// Freight RuleD escription
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Freight Rule Description for this Row</returns>
		public string my_FreightRule_FreightRuleDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FreightRule_FreightRuleDescription");
		}

		/// <summary>
		/// Freight Rule Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name of FreightRule for this Row</returns>
		public string my_FreightRule_FreightRuleName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FreightRule_FreightRuleName");
		}


		/// <summary>
		/// <see cref="clsFreightRule.my_MinTotalWeight">Minimum Total Weight</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MinTotalWeight">Minimum Total Weight</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_FreightRule_MinTotalWeight(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_MinTotalWeight"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_MaxTotalWeight">Maximum Total Weight</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MaxTotalWeight">Maximum Total Weight</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_FreightRule_MaxTotalWeight(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_MaxTotalWeight"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_MinTotalValue">Minimum Total Value</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MinTotalValue">Minimum Total Value</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_FreightRule_MinTotalValue(int rowNum)
		{
			//If the price shown includes the local tax rate, 
			//then the price we have been given also includes the local tax rate.
			//But we always store the price without tax...
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_MinTotalValue")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_MinTotalValue"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_MinTotalValue">Minimum Total Value (Excluding Tax)</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MinTotalValue">Minimum Total Value (Excluding Tax)</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_FreightRule_MinTotalValueExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_MinTotalValue"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_MaxTotalValue">Maximum Total Value</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MaxTotalValue">Maximum Total Value</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_FreightRule_MaxTotalValue(int rowNum)
		{
			//If the price shown includes the local tax rate, 
			//then the price we have been given also includes the local tax rate.
			//But we always store the price without tax...
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_MaxTotalValue")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_MaxTotalValue"));	
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_MaxTotalValue">Maximum Total Value (Excluding Tax)</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MaxTotalValue">Maximum Total Value (Excluding Tax)</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_FreightRule_MaxTotalValueExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_MaxTotalValue"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_FRCost">Cost (For Display Purposes)</see> for this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_FRCost">Cost (For Display Purposes)</see> 
		/// for this <see cref="clsFreightRule">Freight Rule</see> 
		/// </returns>
		public decimal my_FreightRule_FRCost(int rowNum)
		{
			//If the price shown includes the local tax rate, 
			//then the price we have been given also includes the local tax rate.
			//But we always store the price without tax...
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_FRCost")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_FRCost"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_FRCostExcludingTax">Cost (Excluding Tax)</see> for this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_FRCostExcludingTax">Cost (Excluding Tax)</see> 
		/// for this <see cref="clsFreightRule">Freight Rule</see> 
		/// </returns>
		public decimal my_FreightRule_FRCostExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_FRCost"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_FRCostIncludingTax">Cost (Including Tax)</see> for this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_FRCostIncludingTax">Cost (Including Tax)</see> 
		/// for this <see cref="clsFreightRule">Freight Rule</see> 
		/// </returns>
		public decimal my_FreightRule_FRCostIncludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FreightRule_FRCost")) * localTaxRate;
		}


		#endregion

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

		# region My_ Values Product

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

		#region My_ Values ShippingZone

		/// <summary>
		/// <see cref="clsShippingZone.my_ShippingZoneCode">Code</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsShippingZone.my_ShippingZoneCode">Code</see> 
		/// of <see cref="clsShippingZone">ShippingZone</see> 
		/// </returns>	
		public string my_ShippingZone_ShippingZoneCode(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ShippingZone_ShippingZoneCode");
		}


		/// <summary>
		/// <see cref="clsShippingZone.my_ShippingZoneDescription">Description</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsShippingZone.my_ShippingZoneDescription">Description</see> 
		/// of <see cref="clsShippingZone">ShippingZone</see> 
		/// </returns>	
		public string my_ShippingZone_ShippingZoneDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ShippingZone_ShippingZoneDescription");
		}

		# endregion


	}
}
