using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsProduct deals with everything to do with data about Products.
	/// </summary>
	
	[GuidAttribute("893B9F93-7735-44bc-8B9F-DDA1BE86191A")]
	public class clsProduct : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsProduct
		/// </summary>
		public clsProduct() : base("Product")
		{
		}

		/// <summary>
		/// Constructor for clsProduct; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsProduct(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Product")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to ProductInCategory Information
		/// </summary>
		public clsQueryPart ProductInCategoryQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to ProductCategory Information
		/// </summary>
		public clsQueryPart ProductCategoryQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to ProductCustomerGroupPrice Information
		/// </summary>
		public clsQueryPart ProductCustomerGroupPriceQ = new clsQueryPart();
		
		/// <summary>
		/// Part of the Query that Pertains to Defined Content Information
		/// </summary>
		public clsQueryPart DefinedContentQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Defined Content Image Information
		/// </summary>
		public clsQueryPart DefinedContentImageQ = new clsQueryPart();


		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			DefinedContentQ = DefinedContentQueryPart();
			DefinedContentImageQ = DefinedContentImageQueryPart();
			ProductCategoryQ = ProductCategoryQueryPart();
			ProductCustomerGroupPriceQ = ProductCustomerGroupPriceQueryPart();
			MainQ = ProductQueryPart();

			
			ProductInCategoryQ.AddSelectColumn("tblProductInCategory.ProductCategoryId as ProductInCategory_ProductCategoryId");
			ProductInCategoryQ.AddSelectColumn("tblProductInCategory.isInCategory as ProductInCategory_isInCategory");

			ProductInCategoryQ.AddFromTable("(Select ProductAndCategories.ProductId, ProductAndCategories.ProductCategoryId," + crLf
				+ "count(tblProductInCategory.ProductInCategoryId) as isInCategory" + crLf
				+ "from" + crLf
				+ "	(Select tblProduct.ProductId, tblProductCategory.ProductCategoryId" + crLf
				+ "	from tblProduct, tblProductCategory) ProductAndCategories" + crLf
				+ "	left outer join tblProductInCategory" + crLf
				+ "	on ProductAndCategories.ProductId = tblProductInCategory.ProductId " + crLf
				+ "		and ProductAndCategories.ProductCategoryId = tblProductInCategory.ProductCategoryId" + crLf
				+ "	group by ProductAndCategories.ProductId, ProductAndCategories.ProductCategoryId) tblProductInCategory");


			ProductInCategoryQ.AddJoin("tblProduct.ProductId = tblProductInCategory.ProductId");

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[1];
			
			baseQueries[0] = MainQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By tblProduct.ProductName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsProduct
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("DefinedContentId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("DisplayTypeId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ProductName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ProductCode", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuantityDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("WholeNumbersOnly", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ProductOnSpecial", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("UseStockControl", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("QuantityAvailable", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("MaxQuantity", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("ShortDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Weight", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("MaxKeyholdersPerPremise", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("MaxAssetRegisterAssets", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("MaxAssetRegisterStorage", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("MaxDocumentSafeDocuments", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("MaxDocumentSafeStorage", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("DurationNumUnits", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("DurationUnitId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("RequiresPremiseForActivation", System.Type.GetType("System.Int32"));
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

			//Instances of Foreign Key Classes		
			DefinedContent = new clsDefinedContent(thisDbType, localRecords.dbConnection);
			DefinedContentImage = new clsDefinedContentImage(thisDbType, localRecords.dbConnection);

			//Need to get the tax rate so that we can return the price with tax
			GetGeneralSettings();
		}

		/// <summary>
		/// Local Representation of the class <see cref="clsDefinedContent">clsDefinedContent</see>
		/// </summary>
		public clsDefinedContent DefinedContent;

		/// <summary>
		/// Local Representation of the class <see cref="clsDefinedContentImage">clsDefinedContentImage</see>
		/// </summary>
		public clsDefinedContentImage DefinedContentImage;

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Products
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
				thisTable);


			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets an Product by ProductId
		/// </summary>
		/// <param name="ProductId">Id of Product to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductId(int ProductId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ ProductId.ToString();

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


		/// <summary>
		/// Gets information for a Product including which Product Categories a Product is in or not
		/// </summary>
		/// <param name="ProductId">Id of Product to retrieve</param>
		/// <returns>Gets information for a Product including which Product Categories a Product is in or not
		/// </returns>
		public int GetInProductCategory(int ProductId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[4];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;
			queries[2] = ProductInCategoryQ;
			queries[3] = ProductCategoryQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ ProductId.ToString() + ") " + thisTable,
				thisTable
				);
			
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets All Products within a Product Category, including the price of the Product for a given Customer Group
		/// </summary>
		/// <param name="ProductCategoryId">Product Category to search within</param>
		/// <param name="CustomerGroupId">Customer Group whose Price to return</param>
		/// <returns>Number of Resulting Records</returns>
		public int GetByProductCategoryIdCustomerGroupId(int ProductCategoryId, int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[4];
			
			queries[0] = MainQ;
			queries[0].AddJoin("tblProduct.DisplayTypeId != 1");
			
			if (ProductCategoryId > 0)
			{
				queries[0].AddSelectColumn("tblProductInCategory.ProductDisplayOrder");

				queries[0].AddFromTable("(Select ProductId, ProductDisplayOrder "
					+ "From tblProductInCategory"
					+ " Where tblProductInCategory.ProductCategoryId = "
					+ ProductCategoryId.ToString() + ") tblProductInCategory");

				queries[0].AddJoin("tblProduct.ProductId = tblProductInCategory.ProductId");
			}


			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductCustomerGroupPriceQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select ProductId as ProductId, Price, CustomerGroupId from tblProductCustomerGroupPrice"
				+ " Where tblProductCustomerGroupPrice.CustomerGroupId = "
				+ CustomerGroupId.ToString() + ") tblProductCustomerGroupPrice",
				"tblProductCustomerGroupPrice"
				);
			
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets All Products with the specified ProductId, including the price of the Product for a given Customer Group
		/// </summary>
		/// <param name="ProductId">Id of Product</param>
		/// <param name="CustomerGroupId">Customer Group whose Price to return</param>
		/// <returns>Number of Resulting Records</returns>
		public int GetByProductIdCustomerGroupId(int ProductId, int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[4];
			
			queries[0] = MainQ;

			string match = "tblProduct.ProductId = " + ProductId.ToString();

			queries[0].AddJoin(match);
			

			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductCustomerGroupPriceQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select ProductId as ProductId, Price, CustomerGroupId from tblProductCustomerGroupPrice"
				+ " Where tblProductCustomerGroupPrice.CustomerGroupId = "
				+ CustomerGroupId.ToString() + ") tblProductCustomerGroupPrice",
				"tblProductCustomerGroupPrice"
				);
			
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets All Products with the specified ProductName, including the price of the Product for a given Customer Group
		/// </summary>
		/// <param name="ProductName">Name of Product</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <param name="CustomerGroupId">Customer Group whose Price to return</param>
		/// <returns>Number of Resulting Records</returns>
		public int GetByProductNameCustomerGroupId(string ProductName, int MatchCriteria, int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[4];
			
			queries[0] = MainQ;
			queries[0].AddJoin("tblProduct.DisplayTypeId != 1");

			string match = "tblProduct.ProductName " + MatchCondition(ProductName, 
				(matchCriteria) MatchCriteria);


			queries[0].AddJoin(match);
			
			string condition = thisTable + ".IsPublic = 1";

			if (condition != "")
				queries[0].AddJoin(condition);


			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductCustomerGroupPriceQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select ProductId as ProductId, Price, CustomerGroupId from tblProductCustomerGroupPrice"
				+ " Where tblProductCustomerGroupPrice.CustomerGroupId = "
				+ CustomerGroupId.ToString() + ") tblProductCustomerGroupPrice",
				"tblProductCustomerGroupPrice"
				);
			
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Returns Products with the specified ProductName
		/// </summary>
		/// <param name="ProductName">Name of Product</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of Products with the specified ProductName</returns>
		public int GetByProductName(string ProductName, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(ProductName, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblProduct.ProductName " + match + crLf;
			condition += " And " + thisTable + ".IsPublic = 1" + crLf;

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


		/// <summary>
		/// Gets a Distinct List of Product Names only. This is intented for use e.g. as an
		/// auto-completing drop down.
		/// </summary>
		/// <param name="ProductName">Name of Product</param>
		/// <param name="MatchCriteria">Criteria to match Product Name, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctProductName(string ProductName, int MatchCriteria)
		{
			string fieldRequired = "ProductName";
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct "+ thisTable + "." + fieldRequired);

			string condition = "(Select * from " + thisTable + crLf;
			
			string match = MatchCondition(ProductName, 
				(matchCriteria) MatchCriteria);

			//Additional Conditions
			condition += "Where " + fieldRequired + match + crLf;
			
			condition += " And " + thisTable + ".IsPublic = 1" + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			thisSqlQuery += "Order By tblProduct.ProductName" + crLf;

			return localRecords.GetRecords(thisSqlQuery);

		}

		#endregion

		# region Add/Modify/Validate/Save
		
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="DisplayTypeId">Whether this Product is advertised and For Sale</param>
		/// <param name="ProductName">Product's Name</param>
		/// <param name="ProductCode">Product's Code</param>
		/// <param name="QuantityDescription">Item that summaries what the user is buying, e.g "kg of Sausages" or "10 packs of cards"</param>
		/// <param name="WholeNumbersOnly">Whether this product is discrete or continuous</param>
		/// <param name="ProductOnSpecial">Whether this product is on Special</param>
		/// <param name="UseStockControl">Whether this product uses Stock Control</param>
		/// <param name="QuantityAvailable">Number/Quantity of this product in stock</param>
		/// <param name="MaxQuantity">Max Number/Quantity of this product that can be purchased by a user in a single order</param>
		/// <param name="Weight">Weight of this product when packaged (for freight/shipping Purposes)</param>
		/// <param name="MaxKeyholdersPerPremise">Max Keyholders Per Premise (if this is a Premise Related Product)</param>
		/// <param name="MaxAssetRegisterAssets">Max number of assets in the Asset Register (if this is as Asset Register related Product)</param>
		/// <param name="MaxAssetRegisterStorage">Max Asset Register Storage (if this is as Asset Register related Product)</param>
		/// <param name="MaxDocumentSafeDocuments">Max number of documents in the Document Safe (if this is as Document Safe related Product)</param>
		/// <param name="MaxDocumentSafeStorage">Max Document Safe Storage (if this is as Document Safe related Product)</param>
		/// <param name="DurationNumUnits">Number of Unit's duration (if this is a Service over time)</param>
		/// <param name="DurationUnitId">Time Unit's e.g. year, month, day (if this is a Service over time)</param>
		/// <param name="RequiresPremiseForActivation">Whether this Product Requires an associated Premise in order to be Activated 
		/// (if this is a Premise/Time Related Product)</param>
		/// <param name="ShortDescription">Short Description of this product</param>
		/// <param name="DefinedContentTitle">DefinedContent's Title</param>
		/// <param name="PopUpWidth">Width of Pop-Up Window for this Content</param>
		/// <param name="PopUpHeight">Height of Pop-Up Window for this Content</param>
		/// <param name="UsesExternalUrl">Whether to use an external Url for this content or not</param>
		/// <param name="ExternalUrl">The External Url to use if one is to be used</param>
		/// <param name="Description">Defined Content</param>
		/// <param name="ImageRef">Reference to Image File</param>
		/// <param name="ThumbNailRef">Reference to Thumbnail of Image File</param>
		/// <param name="Caption">Caption for this Image</param>
		public void Add(int DisplayTypeId,
			string ProductName,
			string ProductCode,
			string QuantityDescription,
			int WholeNumbersOnly, 
			int ProductOnSpecial, 
			int UseStockControl, 
			decimal QuantityAvailable,
			decimal MaxQuantity, 
			decimal Weight, 
			int MaxKeyholdersPerPremise, 
			int MaxAssetRegisterAssets, 
			int MaxAssetRegisterStorage, 
			int MaxDocumentSafeDocuments, 
			int MaxDocumentSafeStorage, 
			decimal DurationNumUnits, 
			int DurationUnitId, 
			int RequiresPremiseForActivation, 
			string ShortDescription,
			string DefinedContentTitle, 
			int PopUpWidth,
			int PopUpHeight, 
			int UsesExternalUrl, 
			string ExternalUrl, 
			string Description,
			string ImageRef,
			string ThumbNailRef,
			string Caption)
		{
			DefinedContentImageQ.AddSelectColumn("AuxDefinedContentImage.DefinedContentImageId");
	
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			//Firstly, Add the Defined Content
			DefinedContent.Add(DefinedContentTitle, PopUpWidth, PopUpHeight, UsesExternalUrl, ExternalUrl, Description);
			DefinedContent.Save();
			
			int DefinedContentId = DefinedContent.LastIdAdded();

			//Then, Add the Defined Content Image
			DefinedContentImage.Add(DefinedContentId, ImageRef, ThumbNailRef, 1, Caption);
			DefinedContentImage.Save();
			
			rowToAdd["DefinedContentId"] = DefinedContentId;
			rowToAdd["DisplayTypeId"] = DisplayTypeId;
			rowToAdd["ProductName"] = ProductName;
			rowToAdd["ProductCode"] = ProductCode;
			rowToAdd["QuantityDescription"] = QuantityDescription;
			rowToAdd["WholeNumbersOnly"] = WholeNumbersOnly;
			rowToAdd["ProductOnSpecial"] = ProductOnSpecial;
			rowToAdd["UseStockControl"] = UseStockControl;
			rowToAdd["QuantityAvailable"] = QuantityAvailable;
			rowToAdd["MaxQuantity"] = MaxQuantity;
			rowToAdd["Weight"] = Weight;
			rowToAdd["ShortDescription"] = ShortDescription;
			rowToAdd["MaxKeyholdersPerPremise"] = MaxKeyholdersPerPremise;
			rowToAdd["MaxAssetRegisterAssets"] = MaxAssetRegisterAssets;
			rowToAdd["MaxAssetRegisterStorage"] = MaxAssetRegisterStorage;
			rowToAdd["MaxDocumentSafeDocuments"] = MaxDocumentSafeDocuments;
			rowToAdd["MaxDocumentSafeStorage"] = MaxDocumentSafeStorage;
			rowToAdd["DurationNumUnits"] = DurationNumUnits;
			rowToAdd["DurationUnitId"] = DurationUnitId;
			rowToAdd["RequiresPremiseForActivation"] = RequiresPremiseForActivation;
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
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="ProductId">ProductId (Primary Key of Record)</param>
		/// <param name="DisplayTypeId">Whether this Product is advertised and For Sale</param>
		/// <param name="ProductName">Product's Name</param>
		/// <param name="ProductCode">Product's Code</param>
		/// <param name="QuantityDescription">Item that summaries what the user is buying, e.g "kg of Sausages" or "10 packs of cards"</param>
		/// <param name="WholeNumbersOnly">Whether this product is discrete or continuous</param>
		/// <param name="ProductOnSpecial">Whether this product is on Special</param>
		/// <param name="UseStockControl">Whether this product uses Stock Control</param>
		/// <param name="QuantityAvailable">Number/Quantity of this product in stock</param>
		/// <param name="MaxQuantity">Max Number/Quantity of this product that can be purchased by a user in a single order</param>
		/// <param name="Weight">Weight of this product when packaged (for freight/shipping Purposes)</param>
		/// <param name="MaxKeyholdersPerPremise">Max Keyholders Per Premise (if this is a Premise Related Product)</param>
		/// <param name="MaxAssetRegisterAssets">Max number of assets in the Asset Register (if this is as Asset Register related Product)</param>
		/// <param name="MaxAssetRegisterStorage">Max Asset Register Storage (if this is as Asset Register related Product)</param>
		/// <param name="MaxDocumentSafeDocuments">Max number of documents in the Document Safe (if this is as Document Safe related Product)</param>
		/// <param name="MaxDocumentSafeStorage">Max Document Safe Storage (if this is as Document Safe related Product)</param>
		/// <param name="DurationNumUnits">Number of Unit's duration (if this is a Service over time)</param>
		/// <param name="DurationUnitId">Time Unit's e.g. year, month, day (if this is a Service over time)</param>
		/// <param name="RequiresPremiseForActivation">Whether this Product Requires an associated Premise in order to be Activated 
		/// (if this is a Premise/Time Related Product)</param>
		/// <param name="ShortDescription">Short Description of this product</param>
		/// <param name="DefinedContentTitle">DefinedContent's Title</param>
		/// <param name="PopUpWidth">Width of Pop-Up Window for this Content</param>
		/// <param name="PopUpHeight">Height of Pop-Up Window for this Content</param>
		/// <param name="UsesExternalUrl">Whether to use an external Url for this content or not</param>
		/// <param name="ExternalUrl">The External Url to use if one is to be used</param>
		/// <param name="Description">Defined Content</param>
		/// <param name="ImageRef">Reference to Image File</param>
		/// <param name="ThumbNailRef">Reference to Thumbnail of Image File</param>
		/// <param name="Caption">Caption for this Image</param>
		/// <param name="DefinedContentImageId"><see cref="clsDefinedContentImage.my_DefinedContentImageId">Defined Content Image Id</see> of 
		/// <see cref="clsDefinedContentImage">Defined Content Image</see>
		/// Associated with this Product</param>
		/// <param name="DefinedContentId"><see cref="clsDefinedContent.my_DefinedContentId">DefinedContentId</see> of 
		/// <see cref="clsDefinedContent">DefinedContent</see>
		/// Associated with this Product</param>
		public void Modify(int ProductId, 
			int DefinedContentId, 
			int DisplayTypeId,
			string ProductName,
			string ProductCode,
			string QuantityDescription,
			int WholeNumbersOnly, 
			int ProductOnSpecial, 
			int UseStockControl, 
			decimal QuantityAvailable, 
			decimal MaxQuantity, 
			decimal Weight, 
			int MaxKeyholdersPerPremise, 
			int MaxAssetRegisterAssets, 
			int MaxAssetRegisterStorage, 
			int MaxDocumentSafeDocuments, 
			int MaxDocumentSafeStorage, 
			decimal DurationNumUnits, 
			int DurationUnitId, 
			int RequiresPremiseForActivation, 
			string ShortDescription,
			string DefinedContentTitle, 
			int PopUpWidth,
			int PopUpHeight, 
			int UsesExternalUrl, 
			string ExternalUrl,
			string Description, 
			int DefinedContentImageId,
			string ImageRef,
			string ThumbNailRef,
			string Caption)
		{ 
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Firstly, Modify the Defined Content
			DefinedContent.Modify(DefinedContentId,DefinedContentTitle, PopUpWidth, PopUpHeight, UsesExternalUrl, ExternalUrl, Description);
			DefinedContent.Save();

			//Then, Modify the Defined Content Image
			DefinedContentImage.Modify(DefinedContentImageId, DefinedContentId, ImageRef, ThumbNailRef, 1, Caption);
			DefinedContentImage.Save();

			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();
			
			rowToAdd["ProductId"] = ProductId;
			rowToAdd["DefinedContentId"] = DefinedContentId;
			rowToAdd["DisplayTypeId"] = DisplayTypeId;
			rowToAdd["ProductName"] = ProductName;
			rowToAdd["ProductCode"] = ProductCode;
			rowToAdd["QuantityDescription"] = QuantityDescription;
			rowToAdd["WholeNumbersOnly"] = WholeNumbersOnly;
			rowToAdd["ProductOnSpecial"] = ProductOnSpecial;
			rowToAdd["UseStockControl"] = UseStockControl;
			rowToAdd["QuantityAvailable"] = QuantityAvailable;
			rowToAdd["MaxQuantity"] = MaxQuantity;
			rowToAdd["Weight"] = Weight;
			rowToAdd["ShortDescription"] = ShortDescription;
			rowToAdd["MaxKeyholdersPerPremise"] = MaxKeyholdersPerPremise;
			rowToAdd["MaxAssetRegisterAssets"] = MaxAssetRegisterAssets;
			rowToAdd["MaxAssetRegisterStorage"] = MaxAssetRegisterStorage;
			rowToAdd["MaxDocumentSafeDocuments"] = MaxDocumentSafeDocuments;
			rowToAdd["MaxDocumentSafeStorage"] = MaxDocumentSafeStorage;
			rowToAdd["DurationNumUnits"] = DurationNumUnits;
			rowToAdd["DurationUnitId"] = DurationUnitId;
			rowToAdd["RequiresPremiseForActivation"] = RequiresPremiseForActivation;
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

		#region Delete

		/// <summary>
		/// Deletes a product, and references to that product in
		/// tblProductInCateogry, tblFrSzCgP, tblProductCustomerGroupPrice
		/// </summary>
		/// <param name="Id">Id of Product to Delete</param>
		public override void Delete(int Id)
		{
			int numRecordsToDelete;
			//Delete References to the Product from other tables first:
			
			//ProductInCategory
			clsProductInCategory thisProductInCategory = new clsProductInCategory(thisDbType, localRecords.dbConnection);
			numRecordsToDelete = thisProductInCategory.GetByProductId(Id);
			if (numRecordsToDelete > 0)
				thisProductInCategory.localRecords.RemoveRecordById(thisProductInCategory.thisPk, thisProductInCategory.thisTable);

			//FrSzCgP
			clsFrSzCgP thisFrSzCgP = new clsFrSzCgP(thisDbType, localRecords.dbConnection);
			numRecordsToDelete = thisFrSzCgP.GetByProductId(Id);
			if (numRecordsToDelete > 0)
				thisFrSzCgP.localRecords.RemoveRecordById(thisFrSzCgP.thisPk, thisFrSzCgP.thisTable);

			//Product
			MakeNotIsPublic(Id);
			Save();
		
		}

		#endregion

		# region My_ Values Product

		/// <summary>
		/// <see cref="clsProduct.my_ProductId">ProductId</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductId">Id</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// </returns>
		public int my_ProductId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductId"));
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_DefinedContentId">Id</see> of 
		/// <see cref="clsDefinedContent">DefinedContent</see>
		/// Associated with this Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_DefinedContentId">Id</see> 
		/// of <see cref="clsDefinedContent">DefinedContent</see> 
		/// for this Product</returns>	
		public int my_DefinedContentId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DefinedContentId"));
		}


		/// <summary>
		/// <see cref="clsProduct.my_DisplayTypeId">Product Display Type</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_DisplayTypeId">Product Display Type</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// </returns>
		public int my_DisplayTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DisplayTypeId"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_ProductName">Product Name</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductName">Product Name</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// </returns>
		public string my_ProductName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ProductName");
		}

		/// <summary>
		/// <see cref="clsProduct.my_ProductCode">Product Code</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductCode">Product Code</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// </returns>
		public string my_ProductCode(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ProductCode");
		}

		/// <summary>
		/// <see cref="clsProduct.my_QuantityDescription">Quantity Description</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_QuantityDescription">Quantity Description</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// </returns>
		public string my_QuantityDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuantityDescription");
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
		public int my_UseStockControl(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "UseStockControl"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_QuantityAvailable">Quantity Available</see> of 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_QuantityAvailable">Quantity Available</see> of 
		/// <see cref="clsProduct">Product</see></returns>
		public decimal my_QuantityAvailable(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "QuantityAvailable"));
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
		public int my_WholeNumbersOnly(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "WholeNumbersOnly"));
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
		public int my_ProductOnSpecial(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductOnSpecial"));
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
		public int my_MaxKeyholdersPerPremise(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MaxKeyholdersPerPremise"));
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
		public int my_MaxAssetRegisterAssets(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MaxAssetRegisterAssets"));
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
		public int my_MaxAssetRegisterStorage(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MaxAssetRegisterStorage"));
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
		public int my_MaxDocumentSafeDocuments(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MaxDocumentSafeDocuments"));
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
		public int my_MaxDocumentSafeStorage(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "MaxDocumentSafeStorage"));
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
		public int my_DurationUnitId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DurationUnitId"));
		}


		/// <summary>
		/// <see cref="clsProduct.my_DurationNumUnits">Number of Unit's duration</see> (if this is a Service over time) for this 
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_DurationNumUnits">Number of Unit's duration</see> (if this is a Service over time) for this 
		/// <see cref="clsProduct">Product</see>
		/// </returns>
		public double my_DurationNumUnits(int rowNum)
		{
			return Convert.ToDouble(localRecords.FieldByName(rowNum, "DurationNumUnits"));
		}


		/// <summary>Whether this <see cref="clsProduct">Product</see> 
		/// <see cref="clsProduct.my_RequiresPremiseForActivation">Requires an associated Premise in order to be Activated</see>  
		/// (if this is a Premise/Time Related Product)</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this <see cref="clsProduct">Product</see> 
		/// <see cref="clsProduct.my_RequiresPremiseForActivation">Requires an associated Premise in order to be Activated</see>  
		/// (if this is a Premise/Time Related Product)</returns>	
		public int my_RequiresPremiseForActivation(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "RequiresPremiseForActivation"));
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
		public decimal my_MaxQuantity(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "MaxQuantity"));
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
		public decimal my_MaxAllowable(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "MaxAllowable"));
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
		public decimal my_Weight(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Weight"));
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
		public string my_ShortDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ShortDescription");
		}



		#endregion

		# region My_ Values DefinedContent

		/// <summary>
		/// <see cref="clsDefinedContent.my_DefinedContentTitle">Title</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_DefinedContentTitle">Title</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public string my_DefinedContent_DefinedContentTitle(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DefinedContent_DefinedContentTitle");
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_PopUpWidth">Pop-Up Width</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_PopUpWidth">Pop-Up Width</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public int my_DefinedContent_PopUpWidth(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DefinedContent_PopUpWidth"));
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_PopUpHeight">Pop-Up Height</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_PopUpHeight">Pop-Up Height</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public int my_DefinedContent_PopUpHeight(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DefinedContent_PopUpHeight"));
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_UsesExternalUrl">Whether this Defined Content Uses an External Url</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_UsesExternalUrl">Whether this Defined Content Uses an External Url</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public int my_DefinedContent_UsesExternalUrl(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DefinedContent_UsesExternalUrl"));
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_ExternalUrl">The External Url, if this Defined Content has it</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_ExternalUrl">The External Url, if this Defined Content has it</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public string my_DefinedContent_ExternalUrl(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DefinedContent_ExternalUrl");
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_Description">Description</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_Description">Description</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// </returns>
		public string my_DefinedContent_Description(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DefinedContent_Description");
		}

		# endregion

		#region My_ Values DefinedContentImage

		/// <summary>
		/// <see cref="clsDefinedContentImage.my_DefinedContentImageId">DefinedContentImageId of this Defined Content</see> of 
		/// <see cref="clsDefinedContentImage">Defined Content Image</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContentImage.my_DefinedContentImageId">DefinedContentImageId of this Defined Content</see> 
		/// of <see cref="clsDefinedContentImage">Defined Content Image</see> 
		/// </returns>
		public int my_DefinedContentImage_DefinedContentImageId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DefinedContentImage_DefinedContentImageId"));
		}


		/// <summary>
		/// <see cref="clsDefinedContentImage.my_ImageRef">Defined Content Image Reference</see> to
		/// <see cref="clsDefinedContentImage">Defined Content Image</see>
		/// Associated with this Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContentImage.my_ImageRef">Reference</see> 
		/// to <see cref="clsDefinedContentImage">Defined Content Image</see> 
		/// for this Product</returns>	
		public string my_DefinedContentImage_ImageRef(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DefinedContentImage_ImageRef");
		}

		/// <summary>
		/// <see cref="clsDefinedContentImage.my_ThumbNailRef">Defined Content ThumbNail Reference</see> to
		/// <see cref="clsDefinedContentImage">Defined Content Image</see>
		/// Associated with this Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContentImage.my_ThumbNailRef">Reference</see> 
		/// to <see cref="clsDefinedContentImage">Defined Content Image</see> 
		/// for this Product</returns>	
		public string my_DefinedContentImage_ThumbNailRef(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DefinedContentImage_ThumbNailRef");
		}

		/// <summary>
		/// <see cref="clsDefinedContentImage.my_Caption">Defined Content Image Caption</see> for
		/// <see cref="clsDefinedContentImage">Defined Content Image</see>
		/// Associated with this Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContentImage.my_Caption">Caption</see> 
		/// for <see cref="clsDefinedContentImage">Defined Content Image</see> 
		/// for this Product</returns>	
		public string my_DefinedContentImage_Caption(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DefinedContentImage_Caption");
		}

		#endregion

		#region My_ Values ProductInCategory
		
		/// <summary>
		/// <see cref="clsProductCategory.my_ProductCategoryId">ProductCategoryId</see> of 
		/// <see cref="clsProductCategory">ProductCategory</see>
		/// Associated with this Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCategory.my_ProductCategoryId">Id</see> 
		/// of <see cref="clsProductCategory">ProductCategory</see> 
		/// for this Product</returns>	
		public int my_ProductInCategory_ProductCategoryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductInCategory_ProductCategoryId"));
		}
		
		/// <summary>
		/// Whether this product is in this Category or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this product is in this Category or not</returns>
		public int my_ProductInCategory_isInCategory(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductInCategory_isInCategory"));
		}

		/// <summary>
		/// <see cref="clsProductInCategory.my_ProductDisplayOrder">Display Order</see> of 
		/// <see cref="clsProductInCategory">Product Category</see>
		/// Associated with this Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductInCategory.my_ProductDisplayOrder">Display Order</see> 
		/// of <see cref="clsProductInCategory">Product Category</see> 
		/// Associated with this Producty</returns>
		public int my_ProductInCategory_ProductDisplayOrder(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductInCategory_ProductDisplayOrder"));
		}

		#endregion

		# region My_ Values ProductCategory


		/// <summary>
		/// Product Category Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name of Product Category for this Row</returns>
		public string my_ProductCategory_ProductCategoryName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ProductCategory_ProductCategoryName");
		}


		/// <summary>
		/// Parent Product Category Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name of Product Category for the Parent of this Product Category</returns>
		public string my_ProductCategory_ParentProductCategoryName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ProductCategory_ParentProductCategoryName");
		}

		/// <summary>
		/// Product Category Short Description
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Description of Product Category for this Row</returns>
		public string my_ProductCategory_ProductCategoryShortDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ProductCategory_ProductCategoryShortDescription");
		}
		
		/// <summary>
		/// <see cref="clsDefinedContent.my_DefinedContentId">Defined ContentI d</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// Associated with this Product Category</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_DefinedContentId">Id</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// for this Product Category</returns>	
		public int my_ProductCategory_DefinedContentId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCategory_DefinedContentId"));
		}

		/// <summary>
		/// <see cref="clsProductCategory.my_ProductCategoryId">ProductCategoryId</see> of 
		/// <see cref="clsProductCategory">Parent Product Category</see>
		/// Associated with this ProductCategory</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCategory.my_ProductCategoryId">Id</see> 
		/// of <see cref="clsProductCategory">ProductCategory</see> 
		/// for this ProductCategory</returns>	
		public int my_ProductCategory_IsSubCategoryOf(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCategory_IsSubCategoryOf"));
		}

		/// <summary>
		/// Display Style for this Product Category
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Display Style for this Product Category</returns>
		public int my_ProductCategory_DisplayStyle(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCategory_DisplayStyle"));
		}


		/// <summary>
		/// Whether this Product Category is shown on the Home Page or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this Product Category is shown on the Home Page or not</returns>
		public int my_ProductCategory_ShowOnHomePage(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCategory_ShowOnHomePage"));
		}

		/// <summary>
		/// Whether this Product Category is shown in the Category Bar or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this Product Category is shown in the Category Bar or not</returns>
		public int my_ProductCategory_ShowInCategoryBar(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCategory_ShowInCategoryBar"));
		}

		/// <summary>
		/// Category Display Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Display Order for this Category</returns>
		public int my_ProductCategory_CategoryDisplayOrder(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCategory_CategoryDisplayOrder"));
		}

		/// <summary>
		/// Content type to Display for this Category
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Content type to Display for this Category</returns>
		public int my_ProductCategory_CategoryDisplayContent(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCategory_CategoryDisplayContent"));
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
		public int my_ProductCustomerGroupPrice_ProductCustomerGroupPriceId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCustomerGroupPrice_ProductCustomerGroupPriceId"));
		}
		
		/// <summary>
		/// <see cref="clsProduct.my_ProductId">ProductId</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductId">Id</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// for this ProductCustomerGroupPrice</returns>
		public int my_ProductCustomerGroupPrice_ProductId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCustomerGroupPrice_ProductId"));
		}

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupId">CustomerGroupId</see> of 
		/// <see cref="clsCustomerGroup">PCustomer Group</see>
		/// Associated with this ProductCustomerGroupPrice</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupId">Id</see> 
		/// of <see cref="clsCustomerGroup">Customer Group</see> 
		/// for this ProductCustomerGroupPrice</returns>
		public int my_ProductCustomerGroupPrice_CustomerGroupId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCustomerGroupPrice_CustomerGroupId"));
		}

		/// <summary>
		/// <see cref="clsProductCustomerGroupPrice.my_Price">Price (for display)</see> from 
		/// <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCustomerGroupPrice.my_Price">Price (for display)</see> 
		/// from <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see> 
		/// </returns>
		public decimal my_ProductCustomerGroupPrice_Price(int rowNum)
		{
			
			//If the price shown includes the local tax rate, 
			//then the price we have been given also includes the local tax rate.
			//But we always store the price without tax...
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "ProductCustomerGroupPrice_Price")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "ProductCustomerGroupPrice_Price"));
		}

		/// <summary>
		/// <see cref="clsProductCustomerGroupPrice.my_PriceIncludingTax">Price (Including Tax)</see> from 
		/// <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCustomerGroupPrice.my_PriceIncludingTax">Price (Including Tax)</see> 
		/// from <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see> 
		/// </returns>
		public decimal my_ProductCustomerGroupPrice_PriceIncludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "ProductCustomerGroupPrice_Price")) * localTaxRate;
		}

		/// <summary>
		/// <see cref="clsProductCustomerGroupPrice.my_PriceExcludingTax">Price (Excluding Tax)</see> from 
		/// <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCustomerGroupPrice.my_PriceExcludingTax">Price (Excluding Tax)</see> 
		/// from <see cref="clsProductCustomerGroupPrice">ProductCustomerGroupPrice</see> 
		/// </returns>
		public decimal my_ProductCustomerGroupPrice_PriceExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "ProductCustomerGroupPrice_Price"));
		}

		#endregion

	}
}
