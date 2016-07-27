using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsProductCustomerGroupPrice deals with everything to do with data about ProductCustomerGroupPrices.
	/// </summary>

	[GuidAttribute("E0C119C8-9F15-4262-9F35-720A75FF21B0")]
	public class clsProductCustomerGroupPrice : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsProductCustomerGroupPrice
		/// </summary>
		public clsProductCustomerGroupPrice() : base("ProductCustomerGroupPrice")
		{
		}

		/// <summary>
		/// Constructor for clsProductCustomerGroupPrice; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsProductCustomerGroupPrice(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("ProductCustomerGroupPrice")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Number of Customer Groups
		/// </summary>
		public int numCustomerGroups = 0;

		/// <summary>
		/// Part of the Query that Pertains to Product Information
		/// </summary>
		public clsQueryPart ProductQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Product Category Information
		/// </summary>
		public clsQueryPart CustomerGroupQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Price Matrix Information
		/// </summary>
		public clsQueryPart ThisMatrixQ = new clsQueryPart();

		/// <summary>
		/// Row Prefix for Customer Groups
		/// </summary>
		public string RowPrefix = "CG";

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			ProductQ = ProductQueryPart();
			CustomerGroupQ = CustomerGroupQueryPart();

			CustomerGroupQ.Joins.Clear();
			CustomerGroupQ.AddJoin("tblProductCustomerGroupPrice.CustomerGroupId = tblCustomerGroup.CustomerGroupId");

			MainQ.AddSelectColumn("tblProductCustomerGroupPrice.ProductCustomerGroupPriceId");
			MainQ.AddSelectColumn("tblProductCustomerGroupPrice.ProductId");
			MainQ.AddSelectColumn("tblProductCustomerGroupPrice.CustomerGroupId");
			MainQ.AddSelectColumn("tblProductCustomerGroupPrice.Price");

			MainQ.AddFromTable("(Select ProductAndCustomerGroups.ProductId," + crLf
				+ "ProductAndCustomerGroups.CustomerGroupId," + crLf
				+ "Max(tblProductCustomerGroupPrice.Price) as Price," + crLf
				+ "Max(tblProductCustomerGroupPrice.ProductCustomerGroupPriceId) as ProductCustomerGroupPriceId" + crLf
				+ "from" + crLf
				+ "	(Select tblProduct.ProductId, tblCustomerGroup.CustomerGroupId" + crLf
				+ "	from tblProduct, tblCustomerGroup) ProductAndCustomerGroups" + crLf
				+ "	left outer join " + thisTable + crLf
				+ "	on ProductAndCustomerGroups.ProductId = tblProductCustomerGroupPrice.ProductId " + crLf
				+ "		and ProductAndCustomerGroups.CustomerGroupId = tblProductCustomerGroupPrice.CustomerGroupId" + crLf
				+ "	group by ProductAndCustomerGroups.ProductId, ProductAndCustomerGroups.CustomerGroupId) tblProductCustomerGroupPrice");

			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ProductQ;
			queries[2] = CustomerGroupQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblProductCustomerGroupPrice.ProductId" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsProductCustomerGroupPrice
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("ProductId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CustomerGroupId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("Price", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("MinimumQuantityForPrice", System.Type.GetType("System.Decimal"));
			
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

			//Instances of Foreign Key Classes		

			CustomerGroup = new clsCustomerGroup(thisDbType, localRecords.dbConnection);
			//Need to get the tax rate so that we can return the price with tax
			GetGeneralSettings();	
		}

		/// <summary>
		/// Local Representation of the class <see cref="clsCustomerGroup">clsCustomerGroup</see>
		/// </summary>
		public clsCustomerGroup CustomerGroup;

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all ProductCustomerGroupPrices
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ProductQ;
			queries[2] = CustomerGroupQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets a Product-Customer Group-Price by ProductCustomerGroupPriceId
		/// </summary>
		/// <param name="ProductCustomerGroupPriceId">Product-Customer Group-Price to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductCustomerGroupPriceId(int ProductCustomerGroupPriceId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ProductQ;
			queries[2] = CustomerGroupQ;

			queries[0].AddJoin("tblProductCustomerGroupPrice.ProductCustomerGroupPriceId = " 
				+ ProductCustomerGroupPriceId.ToString());

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Product-Customer Group-Prices by ProductId And CustomerGroupId
		/// </summary>
		/// <param name="ProductId">Id of Product to retrieve Product-Customer Group-Prices for</param>
		/// <param name="CustomerGroupId">Id of Customer Group to retrieve Product-Customer Group-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductIdAndCustomerGroupId(int ProductId, int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			//Additional Condition
			queries[0].AddJoin("tblProductCustomerGroupPrice.ProductId = " 
				+ ProductId.ToString());

			queries[0].AddJoin("tblProductCustomerGroupPrice.CustomerGroupId = " 
				+ CustomerGroupId.ToString());
			
			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets roduct-Customer Group-Prices by ProductId
		/// </summary>
		/// <param name="ProductId">Id of Product to retrieve Product-Customer Group-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductId(int ProductId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ProductQ;
			queries[2] = CustomerGroupQ;

			string condition = "(Select * from tblProduct" + crLf;	

			//Additional Condition
			condition += "Where tblProduct.ProductId = " 
				+ ProductId.ToString() + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition + ") tblProduct",
				"tblProduct"
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Get Product-Customer Group-Prices by CustomerGroupId
		/// </summary>
		/// <param name="CustomerGroupId">Id of Customer Group to retrieve Product-Customer Group-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerGroupId(int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = CustomerGroupQ;
			queries[2] = ProductQ;

			//Additional Condition
			queries[1].AddJoin("tblProductCustomerGroupPrice.CustomerGroupId = " 
				+ CustomerGroupId.ToString());

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Initialises an internal list of all Product-Customer Group-Prices
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetMatrixAll()
		{
			return GetByMatrixProductCodeProductNameProductCategoryProductId("", matchCriteria_contains(), 0, 0);
		}

		/// <summary>
		/// Returns a Product and the Categories it is in
		/// </summary>
		/// <param name="ProductId">Specified Product Id. 0 Returns all Products regardless of Category</param>
		/// <returns>Number of Records that match the criteria specified</returns>
		public int GetByMatrixProductId(int ProductId)
		{
			return GetByMatrixProductCodeProductNameProductCategoryProductId("", matchCriteria_contains(), 0, ProductId);
		}

		/// <summary>
		/// Returns Product-Customer Group-Prices with the specified Product Code/Product Name
		/// </summary>
		/// <param name="Name">Name/Part Name of Product Code/Product Name</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetByMatrixProductCodeProductName(string Name, int MatchCriteria)
		{
			return GetByMatrixProductCodeProductNameProductCategoryProductId(Name, MatchCriteria, 0, 0);
		}


		/// <summary>
		/// Returns Product-Customer Group-Prices with the specified Product Code/Product Name in the specified Product Category
		/// </summary>
		/// <param name="Name">Name/Part Name of Product Code/Product Name</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <param name="ProductCategory">Specified Product Category. 0 Returns all Products regardless of Category</param>
		/// <returns>Number of Records that match the criteria specified</returns>
		public int GetByMatrixProductCodeProductNameProductCategory(string Name, int MatchCriteria, int ProductCategory)
		{
			return GetByMatrixProductCodeProductNameProductCategoryProductId(Name, MatchCriteria, ProductCategory, 0);
		}


		/// <summary>
		/// Returns Product-Customer Group-Prices with the specified Product Code/Product Name in the specified Product Category
		/// </summary>
		/// <param name="Name">Name/Part Name of Product Code/Product Name</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <param name="ProductCategory">Specified Product Category. 0 Returns all Products regardless of Category</param>
		/// <param name="ProductId">Specified Product Id. 0 Returns all Products</param>
		/// <returns>Number of Records that match the criteria specified</returns>
		public int GetByMatrixProductCodeProductNameProductCategoryProductId(string Name, int MatchCriteria, int ProductCategory, int ProductId)
		{
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			ThisMatrixQ = new clsQueryPart();

			ThisMatrixQ.AddSelectColumn("tblProduct.ProductId");
			ThisMatrixQ.AddSelectColumn("tblProduct.ProductName");
			ThisMatrixQ.AddSelectColumn("tblProduct.ProductCode");
			ThisMatrixQ.AddSelectColumn("tblProduct.UseStockControl");
			ThisMatrixQ.AddSelectColumn("tblProduct.WholeNumbersOnly");
			ThisMatrixQ.AddSelectColumn("tblProduct.QuantityAvailable");

			if (ProductId == 0)
				ThisMatrixQ.AddFromTable("(select * from tblProduct where Archive = 0) tblProduct");
			else
				ThisMatrixQ.AddFromTable("(select * from tblProduct where ProductId = " + ProductId.ToString().Trim()
					+ ") tblProduct");

			//Add the ThumbNail Reference
			ThisMatrixQ.AddSelectColumn("tblDefinedContentImage.ThumbNailRef");
			ThisMatrixQ.AddFromTable("tblDefinedContentImage");
			ThisMatrixQ.AddJoin("tblDefinedContentImage.DefinedContentId = tblProduct.DefinedContentId");

			if (ProductCategory != 0)
			{
				ThisMatrixQ.AddFromTable("(Select ProductId from tblProductInCategory where ProductCategoryId = " 
					+ ProductCategory.ToString() + ") tblProductInCategory");
			}

			numCustomerGroups = CustomerGroup.GetAll();
			
			for(int counter = 0; counter < numCustomerGroups; counter++)
			{
				int thisId = CustomerGroup.my_CustomerGroupId(counter);
				string colName = RowPrefix + thisId.ToString().Trim();

				ThisMatrixQ.AddSelectColumn(colName + ".Price as " + colName);
				
				ThisMatrixQ.AddFromTable("(Select ProductAndCustomerGroups.ProductId," + crLf
					+ "ProductAndCustomerGroups.CustomerGroupId," + crLf
					+ "Max(tblProductCustomerGroupPrice.Price) as Price," + crLf
					+ "Max(tblProductCustomerGroupPrice.ProductCustomerGroupPriceId) as ProductCustomerGroupPriceId" + crLf
					+ "from" + crLf
					+ "	(Select tblProduct.ProductId, tblCustomerGroup.CustomerGroupId" + crLf
					+ "	from tblProduct, tblCustomerGroup where tblCustomerGroup.CustomerGroupId = " + thisId.ToString() + crLf
					+ "	) ProductAndCustomerGroups" + crLf
					+ "	left outer join tblProductCustomerGroupPrice" + crLf
					+ "	on ProductAndCustomerGroups.ProductId = tblProductCustomerGroupPrice.ProductId " + crLf
					+ "		and ProductAndCustomerGroups.CustomerGroupId = tblProductCustomerGroupPrice.CustomerGroupId" + crLf
					+ "	group by ProductAndCustomerGroups.ProductId, ProductAndCustomerGroups.CustomerGroupId) " + colName);
				
				ThisMatrixQ.AddJoin("tblProduct.ProductId = " + colName + ".ProductId");
		
			}

			if (ProductCategory != 0)
				ThisMatrixQ.AddJoin("tblProductInCategory.ProductId = tblProduct.ProductId");


			ThisMatrixQ.AddJoin("concat_ws(' ',tblProduct.ProductName,tblProduct.ProductCode) " + MatchCondition(Name, matchCriteria.contains));
			ThisMatrixQ.AddJoin("tblProduct.Archive = 0");

			queries[0] = ThisMatrixQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			return localRecords.GetRecords(thisSqlQuery);
		}

		#endregion

		#region Sets

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="Price">Price for this Product for this Customer Group</param>
		/// <param name="ProductId">Product Associated with this ProductCustomerGroupPrice</param>
		/// <param name="CustomerGroupId">Customer Group Associated with this ProductCustomerGroupPrice</param>
		public void Set(int ProductId, 
			int CustomerGroupId, 
			decimal Price)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			clsProductCustomerGroupPrice thisPCGP = new clsProductCustomerGroupPrice(thisDbType, localRecords.dbConnection);

			int numRecords = thisPCGP.GetByProductIdAndCustomerGroupId(ProductId, CustomerGroupId);

			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			switch (thisPCGP.my_ProductCustomerGroupPriceId(0))
			{
				case 0:
					Add(ProductId, CustomerGroupId, Price);
					break;
				default:
					//If the price shown includes the local tax rate, 
					//then the price we have been given also includes the local tax rate.
					//But we always store the price without tax...
					if (priceShownIncludesLocalTaxRate)
						Price = Price / localTaxRate;

					SetAttribute(thisPCGP.my_ProductCustomerGroupPriceId(0), "Price", Price.ToString());
					break;
			}
		}
		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="Price">Price for this Product for this Customer Group</param>
		/// <param name="ProductId">Product Associated with this ProductCustomerGroupPrice</param>
		/// <param name="CustomerGroupId">Customer Group Associated with this ProductCustomerGroupPrice</param>
		public void Add(int ProductId, 
			int CustomerGroupId, 
			decimal Price)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			//If the price shown includes the local tax rate, 
			//then the price we have been given also includes the local tax rate.
			//But we always store the price without tax...
			if (priceShownIncludesLocalTaxRate)
				Price = Price / localTaxRate;

			rowToAdd["Price"] = Price;

			rowToAdd["ProductId"] = ProductId;
			rowToAdd["CustomerGroupId"] = CustomerGroupId;
			rowToAdd["MinimumQuantityForPrice"] = 1;
			
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
		/// <param name="ProductCustomerGroupPriceId">ProductCustomerGroupPriceId (Primary Key of Record)</param>
		/// <param name="Price">Price for this Product for this Customer Group</param>
		/// <param name="ProductId">Product Associated with this ProductCustomerGroupPrice</param>
		/// <param name="CustomerGroupId">Customer Group Associated with this ProductCustomerGroupPrice</param>
		public void Modify(int ProductCustomerGroupPriceId, 
			int ProductId, 
			int CustomerGroupId, 
			decimal Price)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			//If the price shown includes the local tax rate, 
			//then the price we have been given also includes the local tax rate.
			//But we always store the price without tax...
			if (priceShownIncludesLocalTaxRate)
				Price = Price / localTaxRate;
			
			rowToAdd["ProductCustomerGroupPriceId"] = ProductCustomerGroupPriceId;
			rowToAdd["Price"] = Price;
			rowToAdd["ProductId"] = ProductId;
			rowToAdd["CustomerGroupId"] = CustomerGroupId;
			rowToAdd["MinimumQuantityForPrice"] = 1;

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

		# region My_ Values ProductCustomerGroupPrice

		/// <summary>
		/// <see cref="clsProductCustomerGroupPrice.my_ProductCustomerGroupPriceId">Id</see> of 
		/// <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCustomerGroupPrice.my_ProductCustomerGroupPriceId">Id</see> 
		/// of <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see> 
		/// </returns>
		public int my_ProductCustomerGroupPriceId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCustomerGroupPriceId"));
		}
		
		/// <summary>
		/// <see cref="clsProduct.my_ProductId">ProductId</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductId">Id</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// for this ProductCustomerGroupPrice</returns>
		public int my_ProductId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductId"));
		}

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupId">CustomerGroupId</see> of 
		/// <see cref="clsCustomerGroup">PCustomer Group</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupId">Id</see> 
		/// of <see cref="clsCustomerGroup">Customer Group</see> 
		/// for this ProductCustomerGroupPrice</returns>
		public int my_CustomerGroupId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerGroupId"));
		}

		/// <summary>
		/// <see cref="clsProductCustomerGroupPrice.my_Price">Price (for display)</see> from 
		/// <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCustomerGroupPrice.my_Price">Price (for display)</see> 
		/// from <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see> 
		/// </returns>
		public decimal my_Price(int rowNum)
		{
			
			//If the price shown includes the local tax rate, 
			//then the price we have been given also includes the local tax rate.
			//But we always store the price without tax...
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Price")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Price"));
		}

		/// <summary>
		/// <see cref="clsProductCustomerGroupPrice.my_PriceIncludingTax">Price (Including Tax)</see> from 
		/// <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCustomerGroupPrice.my_PriceIncludingTax">Price (Including Tax)</see> 
		/// from <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see> 
		/// </returns>
		public decimal my_PriceIncludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Price")) * localTaxRate;
		}

		/// <summary>
		/// <see cref="clsProductCustomerGroupPrice.my_PriceExcludingTax">Price (Excluding Tax)</see> from 
		/// <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCustomerGroupPrice.my_PriceExcludingTax">Price (Excluding Tax)</see> 
		/// from <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see> 
		/// </returns>
		public decimal my_PriceExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Price"));
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

		#region My_ Values ProductCustomerGroupPrice Matrix

		/// <summary>
		/// Number of Customer Groups
		/// </summary>
		/// <returns> Number of Customer Groups</returns>
		public int my_numCustomerGroups()
		{
			return numCustomerGroups;
		}

		/// <summary>
		/// Price for this ProductCustomerGroupPrice
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <param name="CusotmerGroupId">Column number for Data</param>
		/// <returns> Price for this Product/CustomerGroup Combo</returns>
		public decimal my_CGPrice(int rowNum, int CusotmerGroupId)
		{
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, RowPrefix + CusotmerGroupId.ToString().Trim())) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, RowPrefix + CusotmerGroupId.ToString().Trim()));
		}


		/// <summary>
		/// Price Including Tax for this ProductCustomerGroupPrice
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <param name="CusotmerGroupId">Column number for Data</param>
		/// <returns> Price Including Tax for this Product/CustomerGroup Combo</returns>
		public decimal my_CGPriceIncludingTax(int rowNum, int CusotmerGroupId)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, RowPrefix + CusotmerGroupId.ToString().Trim())) * localTaxRate;
		}

		/// <summary>
		/// Price Excluding Tax for this ProductCustomerGroupPrice
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <param name="CusotmerGroupId">Column number for Data</param>
		/// <returns> Price Excluding Tax for this Product/CustomerGroup Combo</returns>
		public decimal my_CGPriceExcludingTax(int rowNum, int CusotmerGroupId)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, RowPrefix + CusotmerGroupId.ToString().Trim()));
		}

		#endregion

		#region My_ Values CustomerGroup

		/// <summary>
		/// Name for this Customer Group
		/// </summary>
		/// <param name="colNum">Column number for Data</param>
		/// <returns> Price for this Product/CustomerGroup Combo</returns>
		public string my_CGName(int colNum)
		{
			return CustomerGroup.localRecords.FieldByName(colNum, "CustomerGroupName");
		}

		#endregion

		#region My_ Values DefinedContentImage

		/// <summary>
		/// <see cref="clsDefinedContentImage.my_ThumbNailRef">Defined Content ThumbNail Reference</see> to
		/// <see cref="clsDefinedContentImage">Defined Content Image</see>
		/// Associated with this Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContentImage.my_ThumbNailRef">Reference</see> 
		/// to <see cref="clsDefinedContentImage">Defined Content Image</see> 
		/// for this Product</returns>	
		public string my_ThumbNailRef(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ThumbNailRef");
		}

		# endregion
	}
}
