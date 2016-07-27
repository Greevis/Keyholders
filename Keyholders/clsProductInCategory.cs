using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsProductInCategory deals with everything to do with data about ProductInCategorys.
	/// </summary>
	
	[GuidAttribute("65AF9B0D-0445-4aa6-8571-14A0EE44185D")]
	public class clsProductInCategory : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsProductInCategory
		/// </summary>
		public clsProductInCategory() : base("ProductInCategory")
		{
		}

		/// <summary>
		/// Constructor for clsProductInCategory; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsProductInCategory(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("ProductInCategory")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Product Information
		/// </summary>
		public clsQueryPart ProductQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Product Category Information
		/// </summary>
		public clsQueryPart ProductCategoryQ = new clsQueryPart();

		
		/// <summary>
		/// Part of the Query that Pertains to "All Categories And Products" Information
		/// </summary>
		public clsQueryPart AllCategoriesAndProducts = new clsQueryPart();		
		
		/// <summary>
		/// Part of the Query that Pertains to Price Matrix Information
		/// </summary>
		public clsQueryPart ThisMatrixQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			ProductQ = ProductQueryPart();
			ProductCategoryQ = ProductCategoryQueryPart();
			
			MainQ.AddSelectColumn("tblProductInCategory.ProductInCategoryId");
			MainQ.AddSelectColumn("tblProductInCategory.ProductId");
			MainQ.AddSelectColumn("tblProductInCategory.ProductCategoryId");
			MainQ.AddSelectColumn("tblProductInCategory.ProductDisplayOrder");

			MainQ.AddFromTable(thisTable);

			AllCategoriesAndProducts.AddSelectColumn("tblProduct.ProductName as Product_ProductName");
			AllCategoriesAndProducts.AddSelectColumn("tblProduct.ProductCode as Product_ProductCode");
			AllCategoriesAndProducts.AddSelectColumn("tblProductInCategory.ProductCategoryId");
			AllCategoriesAndProducts.AddSelectColumn("tblProductInCategory.ProductId");
			AllCategoriesAndProducts.AddSelectColumn("tblProductInCategory.ProductCategoryName as ProductCategory_ProductCategoryName");
			AllCategoriesAndProducts.AddSelectColumn("tblProductInCategory.ProductDisplayOrder as ProductCategory_ProductDisplayOrder");
			AllCategoriesAndProducts.AddSelectColumn("tblProductInCategory.CategoryDisplayOrder as ProductCategory_CategoryDisplayOrder");

			AllCategoriesAndProducts.AddFromTable("(Select  tblProductCategory.ProductCategoryId," + crLf
				+ "	tblProductInCategory.ProductId," + crLf
				+ " tblProductInCategory.ProductDisplayOrder," + crLf
				+ " tblProductCategory.ProductCategoryName," + crLf
				+ " tblProductCategory.CategoryDisplayOrder" + crLf
				+ "from tblProductCategory left outer join tblProductInCategory " + crLf
				+ "	on tblProductCategory.ProductCategoryId = tblProductInCategory.ProductCategoryId" + crLf
				+ ") tblProductInCategory" + crLf
				+ "left outer join tblProduct on tblProduct.ProductId = tblProductInCategory.ProductId");			
			
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ProductQ;
			queries[2] = ProductCategoryQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblProductCategory.CategoryDisplayOrder, tblProductInCategory.ProductDisplayOrder" + crLf;

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsProductInCategory
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("ProductId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ProductCategoryId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ProductDisplayOrder", System.Type.GetType("System.String"));
			
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
			Product = new clsProduct(thisDbType, localRecords.dbConnection);
			ProductCategory = new clsProductCategory(thisDbType, localRecords.dbConnection);
		}

		/// <summary>
		/// Local Representation of the class <see cref="clsProductCategory">clsProductCategory</see>
		/// </summary>
		public clsProductCategory ProductCategory;

		/// <summary>
		/// Local Representation of the class <see cref="clsProduct">clsProduct</see>
		/// </summary>
		public clsProduct Product;

		/// <summary>
		/// Row Prefix for ProductCategories
		/// </summary>
		public string RowPrefix = "PC";
		
		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Product-In-Categories
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ProductQ;
			queries[2] = ProductCategoryQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Returns all Products, and Categories they are in or not in.
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetMatrixAll()
		{
			return GetByMatrixProductCodeProductNameProductCategoryProduct("", matchCriteria_contains(), 0, 0);
		}

		/// <summary>
		/// Returns a Product and the Categories it is in
		/// </summary>
		/// <param name="ProductId">Specified Product Id. 0 Returns all Products regardless of Category</param>
		/// <returns>Number of Records that match the criteria specified</returns>
		public int GetByMatrixProductId(int ProductId)
		{
			return GetByMatrixProductCodeProductNameProductCategoryProduct("", matchCriteria_contains(), 0, ProductId);
		}


		/// <summary>
		/// Returns Products with the specified Product Code/Product Name in the specified Product Category and which Categories each are in 
		/// </summary>
		/// <param name="Name">Name/Part Name of Product Code/Product Name</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <param name="ProductCategoryId">Specified Product Category. 0 Returns all Products regardless of Category</param>
		/// <returns>Number of Records that match the criteria specified</returns>
		public int GetByMatrixProductCodeProductNameProductCategory(string Name, int MatchCriteria, int ProductCategoryId)
		{
			
			return GetByMatrixProductCodeProductNameProductCategoryProduct(Name, MatchCriteria, ProductCategoryId, 0);
		}


		/// <summary>
		/// Returns Products with the specified Product Code/Product Name in the specified Product Category or of the specified
		/// ProductId and which Categories each are in 
		/// </summary>
		/// <param name="Name">Name/Part Name of Product Code/Product Name</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <param name="ProductCategoryId">Specified Product Category. 0 Returns all Products regardless of Category</param>
		/// <param name="ProductId">Specified Product Id. 0 Returns all Products</param>
		/// <returns>Number of Records that match the criteria specified</returns>
		public int GetByMatrixProductCodeProductNameProductCategoryProduct(string Name, int MatchCriteria, int ProductCategoryId, int ProductId)
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
			ThisMatrixQ.AddJoin("tblDefinedContentImage.DefinedContentId = tblProduct.Product_DefinedContentId");

			if (ProductCategoryId != 0)
			{
				ThisMatrixQ.AddFromTable("(Select ProductId from tblProductInCategory where ProductCategoryId = " 
					+ ProductCategoryId.ToString() + ") tblProductInCategory");
			}

			int numSubCategories = ProductCategory.GetByTotalSubCategories(0);

			string tableToAdd = "(Select tblProduct.ProductId," + crLf;
			
			for(int counter = 0; counter < numSubCategories; counter++)
			{
				
				int thisId = ProductCategory.my_ProductCategoryId(counter);
				string colName = RowPrefix + thisId.ToString().Trim();

				ThisMatrixQ.AddSelectColumn("Categories." + colName);
				
				tableToAdd += "Max(case when tblProductInCategory.ProductCategoryId = " + thisId.ToString()
					+ " then 1 else 0 end) as " + colName + "," + crLf;
				
			}

			//Lose last cooma
			if (tableToAdd.EndsWith("," + crLf))
			{
				tableToAdd = tableToAdd.Substring(0, tableToAdd.Length - ("," + crLf).Length ) + crLf;
			}

			tableToAdd += "from tblProduct left outer join tblProductInCategory" + crLf;
			tableToAdd += "on tblProduct.ProductId = tblProductInCategory.ProductId" + crLf;
			tableToAdd += "Group By tblProduct.ProductId) Categories" + crLf;

			ThisMatrixQ.AddFromTable(tableToAdd);

			ThisMatrixQ.AddJoin("tblProduct.ProductId = Categories.ProductId");
			
			if (ProductCategoryId != 0)
				ThisMatrixQ.AddJoin("tblProductInCategory.ProductId = tblProduct.ProductId");

			ThisMatrixQ.AddJoin("concat_ws(' ',tblProduct.ProductName,tblProduct.ProductCode) " + MatchCondition(Name, matchCriteria.contains));
			ThisMatrixQ.AddJoin("tblProduct.Archive = 0");

			queries[0] = ThisMatrixQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Initialises an internal list of all Product-In-Categories
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAllProductsInCategory()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
	
			queries[0] = AllCategoriesAndProducts;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a Product-In-Category by ProductInCategoryId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByProductInCategoryId(int ProductInCategoryId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ProductQ;
			queries[2] = ProductCategoryQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ ProductInCategoryId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;
			
			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Product-In-Categories by ProductId
		/// </summary>
		/// <param name="ProductId">Id of Product to retrieve Product-In-Categories for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductId(int ProductId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ProductQ;
			queries[2] = ProductCategoryQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblProductInCategory.ProductId = " 
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
		/// Gets Product-In-Categories by ProductCategoryId
		/// </summary>
		/// <param name="ProductCategoryId">Id of Product Category to retrieve Product-In-Categories for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductCategoryId(int ProductCategoryId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = ProductCategoryQ;
			queries[2] = ProductQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where " + thisTable + ".ProductCategoryId = " 
				+ ProductCategoryId.ToString() + crLf;

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
		/// Gets Product-In-Categories by ProductId and ProductCategoryId
		/// </summary>
		/// <param name="ProductId">Id of Product to retrieve Product-In-Categories for</param>
		/// <param name="ProductCategoryId">Id of Product Category to retrieve Product-In-Categories for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductIdAndProductCategoryId(int ProductId, int ProductCategoryId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblProductInCategory.ProductId = " 
				+ ProductId.ToString() + crLf;

			condition += "And tblProductInCategory.ProductCategoryId = " 
				+ ProductCategoryId.ToString() + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition + ") " + thisTable,
				thisTable
				);


			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="ProductId">Product Associated with this ProductInCategory</param>
		/// <param name="ProductCategoryId">Product Category Associated with this ProductInCategory</param>
		/// <param name="ProductDisplayOrder">Position of this Product in the list for this 
		/// Product Category. If this is set to zero, this method will set the Product's Position 
		/// to be the last in the Category</param>
		public void Add(int ProductId, 
			int ProductCategoryId, 
			int ProductDisplayOrder)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			if (ProductDisplayOrder == 0)
			{
				//Find the number of Products in this Category
				clsProductInCategory thisPIC = new clsProductInCategory(thisDbType, localRecords.dbConnection);
				int numProductsInCategory = thisPIC.GetByProductCategoryId(ProductCategoryId);
				rowToAdd["ProductDisplayOrder"] = numProductsInCategory + 1;
			}
			else
				rowToAdd["ProductDisplayOrder"] = ProductDisplayOrder;

			
			rowToAdd["ProductId"] = ProductId;
			rowToAdd["ProductCategoryId"] = ProductCategoryId;
			
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
		/// <param name="ProductInCategoryId">ProductInCategoryId (Primary Key of Record)</param>
		/// <param name="ProductDisplayOrder">Position of this Product in the list for this Product Category</param>
		/// <param name="ProductId">Product Associated with this ProductInCategory</param>
		/// <param name="ProductCategoryId">Product Category Associated with this ProductInCategory</param>
		public void Modify(int ProductInCategoryId, 
			int ProductId, 
			int ProductCategoryId, 
			int ProductDisplayOrder)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			if (ProductDisplayOrder == 0)
			{
				//Find the number of Products in this Category
				clsProductInCategory thisPIC = new clsProductInCategory(thisDbType, localRecords.dbConnection);
				int numProductsInCategory = thisPIC.GetByProductCategoryId(ProductCategoryId);
				rowToAdd["ProductDisplayOrder"] = numProductsInCategory + 1;
			}
			else
				rowToAdd["ProductDisplayOrder"] = ProductDisplayOrder;
			
			rowToAdd["ProductInCategoryId"] = ProductInCategoryId;
			rowToAdd["ProductId"] = ProductId;
			rowToAdd["ProductCategoryId"] = ProductCategoryId;

			//Validate the data supplied
			Validate(rowToAdd, false);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);
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
		/// <param name="ProductId">Product Associated with this ProductInCategory</param>
		/// <param name="ProductCategoryId">Product Category Associated with this ProductInCategory</param>
		public override void Remove(int ProductId, 
			int ProductCategoryId)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Find this ProductInCategoryId
			clsProductInCategory thisPIC = new clsProductInCategory(thisDbType, localRecords.dbConnection);

			int numRecords = thisPIC.GetByProductIdAndProductCategoryId(ProductId, ProductCategoryId);

			if (numRecords == 1)
			{
				thisPIC.Delete(thisPIC.my_ProductInCategoryId(0));
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
		/// <param name="ProductId">Product Associated with this ProductInCategory</param>
		/// <param name="ProductCategoryId">Product Category Associated with this ProductInCategory</param>
		public void Set(int ProductId, 
			int ProductCategoryId)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Find this ProductInCategoryId
			clsProductInCategory thisPIC = new clsProductInCategory(thisDbType, localRecords.dbConnection);

			int numRecords = thisPIC.GetByProductIdAndProductCategoryId(ProductId, ProductCategoryId);
			switch(numRecords)
			{
				case 0:
					Add(ProductId,ProductCategoryId,0);
					break;
				case 1:
				default:
					break;
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

		# region My_ Values ProductInCategory


		/// <summary>
		/// ProductInCategoryId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>ProductInCategoryId for this Row</returns>
		public int my_ProductInCategoryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductInCategoryId"));
		}

		/// <summary>
		/// ProductInCategory Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name of ProductInCategory for this Row</returns>
		public int my_ProductDisplayOrder(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductDisplayOrder"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_ProductId">ProductId</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this ProductInCategory</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductId">Id</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// for this ProductInCategory</returns>
		public int my_ProductId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductId"));
		}

		/// <summary>
		/// <see cref="clsProductCategory.my_ProductCategoryId">ProductCategoryId</see> of 
		/// <see cref="clsProductCategory">Product Category</see>
		/// Associated with this ProductInCategory</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCategory.my_ProductCategoryId">Id</see> 
		/// of <see cref="clsProductCategory">Product Category</see> 
		/// for this ProductInCategory</returns>
		public int my_ProductCategoryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCategoryId"));
		}

		#endregion

		#region My_Values DefinedContentImage

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

		#endregion

		#region My_ Values ProductCustomerGroupPrice Matrix
		/// <summary>
		/// Price for this ProductCustomerGroupPrice
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <param name="ProductCategoryId">Id of ProductCategory to check if this Product is in</param>
		/// <returns>Whether this Product is in this Product Category or not</returns>
		public int my_ProductInCategory(int rowNum, int ProductCategoryId)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, RowPrefix + ProductCategoryId.ToString().Trim()));
		}

		# endregion

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


	}
}
