using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsProductCategory deals with everything to do with data about ProductCategorys.
	/// </summary>

	[GuidAttribute("984F1A3E-90F0-415d-9272-9108CE1FA96F")]
	public class clsProductCategory : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsProductCategory
		/// </summary>
		public clsProductCategory() : base("ProductCategory")
		{
		}

		/// <summary>
		/// Constructor for clsProductCategory; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsProductCategory(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("ProductCategory")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Defined Content Information
		/// </summary>
		public clsQueryPart DefinedContentQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Defined Content Image Information
		/// </summary>
		public clsQueryPart DefinedContentImageQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to SubCategorySummary Information
		/// </summary>
		public clsQueryPart SubCategorySummaryQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Product Summary Information
		/// </summary>
		public clsQueryPart ProductSummaryQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			DefinedContentQ = DefinedContentQueryPart();
			DefinedContentImageQ = DefinedContentImageQueryPart();

			MainQ.AddSelectColumn("tblProductCategory.ProductCategoryId");
			MainQ.AddSelectColumn("tblProductCategory.ProductCategoryName");
			MainQ.AddSelectColumn("tblProductCategory.ProductCategoryShortDescription");
			MainQ.AddSelectColumn("tblProductCategory.DefinedContentId");
			MainQ.AddSelectColumn("tblProductCategory.ShowInCategoryBar");
			MainQ.AddSelectColumn("tblProductCategory.ShowOnHomePage");
			MainQ.AddSelectColumn("tblProductCategory.CategoryDisplayOrder");
			MainQ.AddSelectColumn("tblProductCategory.CategoryDisplayContent");
			MainQ.AddSelectColumn("tblProductCategory.IsSubCategoryOf");
			MainQ.AddSelectColumn("tblProductCategory.DisplayStyle");
			MainQ.AddSelectColumn("tblProductCategory.IsPublic");
			MainQ.AddSelectColumn("Parent.ProductCategoryName as ParentProductCategoryName");

			MainQ.AddFromTable(thisTable + " left outer join tblProductCategory Parent on tblProductCategory.IsSubCategoryOf = Parent.ProductCategoryId");

			ProductSummaryQ.AddSelectColumn("ProductSummary.TotalProducts");

			ProductSummaryQ.AddFromTable("(Select tblProductCategory.ProductCategoryId," + crLf
				+ "count(ProductId) as TotalProducts" + crLf
				+ "from tblProductCategory left outer join tblProductInCategory " + crLf
				+ "on tblProductCategory.ProductCategoryId = tblProductInCategory.ProductCategoryId " + crLf
				+ "Group by tblProductCategory.ProductCategoryId) ProductSummary");

			ProductSummaryQ.AddJoin("tblProductCategory.ProductCategoryId = ProductSummary.ProductCategoryId");

			SubCategorySummaryQ.AddSelectColumn("SubCategorySummary.TotalSubCategories");

			SubCategorySummaryQ.AddFromTable("(Select tblProductCategory.ProductCategoryId," + crLf
				+ "count(InsideTable.ProductCategoryId) as TotalSubCategories" + crLf
				+ "from tblProductCategory left outer join tblProductCategory InsideTable " + crLf
				+ "on tblProductCategory.ProductCategoryId = InsideTable.IsSubCategoryOf " + crLf
				+ "Group by tblProductCategory.ProductCategoryId) SubCategorySummary");

			SubCategorySummaryQ.AddJoin("tblProductCategory.ProductCategoryId = SubCategorySummary.ProductCategoryId");


			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[5];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductSummaryQ;
			queries[4] = SubCategorySummaryQ;
			
			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblProductCategory.ProductCategoryName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsProductCategory
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("DefinedContentId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ProductCategoryName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ProductCategoryShortDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ShowOnHomePage", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ShowInCategoryBar", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CategoryDisplayOrder", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CategoryDisplayContent", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("IsSubCategoryOf", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("DisplayStyle", System.Type.GetType("System.Int32"));
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
		/// Initialises an internal list of all ProductCategorys
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[5];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductSummaryQ;
			queries[4] = SubCategorySummaryQ;

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
		/// Gets an Product Category by ProductCategoryId
		/// </summary>
		/// <param name="ProductCategoryId">Id of Product Category to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductCategoryId(int ProductCategoryId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[5];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductSummaryQ;
			queries[4] = SubCategorySummaryQ;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ ProductCategoryId.ToString() + crLf;

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
		/// Gets all Categories a Product is in
		/// </summary>
		/// <param name="ProductId">Id of Product to retrieve Categories for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductId(int ProductId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[5];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductSummaryQ;
			queries[4] = SubCategorySummaryQ;

			string condition = "(Select * from tblProductInCategory" + crLf;	

			//Additional Condition
			condition += "Where tblProductInCategory.ProductId = " 
				+ ProductId.ToString() + crLf;

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
		/// Gets Product Categories by IsSubCategoryOfId
		/// </summary>
		/// <param name="IsSubCategoryOf">Status of 'IsSubCategoriesOf' for which to retrieve Product Categories</param>
		/// <returns>Number of resulting records</returns>
		public int GetByIsSubCategoryOf(int IsSubCategoryOf)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[5];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductSummaryQ;
			queries[4] = SubCategorySummaryQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition

			condition += "Where tblProductCategory.IsSubCategoryOf ";
 
			if (IsSubCategoryOf == 0)
				condition +=  "is NULL" + crLf;
			else
				condition +=  "= " + IsSubCategoryOf.ToString() + crLf;

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
		/// Gets Product Categories by TotalSubCategories
		/// </summary>
		/// <param name="TotalSubCategories">Number of 'TotalSubCategories' for which to retrieve Product Categories</param>
		/// <returns>Number of resulting records</returns>
		public int GetByTotalSubCategories(int TotalSubCategories)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[5];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductSummaryQ;
			queries[4] = SubCategorySummaryQ;

			queries[4].AddJoin("SubCategorySummary.TotalSubCategories = " + TotalSubCategories.ToString());

			string condition = "(Select * from " + thisTable + crLf;

			condition += " Where " + thisTable + ".IsPublic = 1" + crLf;
			
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
		/// Gets Product Categories by ShowOnHomePage
		/// </summary>
		/// <param name="ShowOnHomePage">Status of 'ShowOnHomePage' for which to retrieve Product Categories</param>
		/// <returns>Number of resulting records</returns>
		public int GetByShowOnHomePage(int ShowOnHomePage)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[5];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductSummaryQ;
			queries[4] = SubCategorySummaryQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition

			condition += "Where tblProductCategory.ShowOnHomePage = ";

			condition +=  ShowOnHomePage.ToString() + crLf;

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
		/// Gets Product Categories by ShowInCategoryBar
		/// </summary>
		/// <param name="ShowInCategoryBar">Status of 'ShowInCategoryBar' for which to retrieve Product Categories</param>
		/// <returns>Number of resulting records</returns>
		public int GetByShowInCategoryBar(int ShowInCategoryBar)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[5];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductSummaryQ;
			queries[4] = SubCategorySummaryQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition

			condition += "Where tblProductCategory.ShowInCategoryBar = ";

			condition +=  ShowInCategoryBar.ToString() + crLf;

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
		/// Returns Product Categories with the specified Product Category Name
		/// </summary>
		/// <param name="ProductCategoryName">Name of Product Category</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductCategoryName(string ProductCategoryName, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[5];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;
			queries[2] = DefinedContentImageQ;
			queries[3] = ProductSummaryQ;
			queries[4] = SubCategorySummaryQ;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(ProductCategoryName, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblProductCategory.ProductCategoryName " + match + crLf;

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



		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="ProductCategoryName">Product Category's Name</param>
		/// <param name="ProductCategoryShortDescription">Product Category's Short Description</param>
		/// <param name="ShowOnHomePage">Whether to show this Category on the Homepage or not</param>
		/// <param name="ShowInCategoryBar">Whether to show this Category in the "Category Bar" or not</param>
		/// <param name="CategoryDisplayOrder">Display Order of this Category</param>
		/// <param name="CategoryDisplayContent">Content Type to display for this Category</param>
		/// <param name="DefinedContentTitle">DefinedContent's Title</param>
		/// <param name="PopUpWidth">Width of Pop-Up Window for this Content</param>
		/// <param name="PopUpHeight">Height of Pop-Up Window for this Content</param>
		/// <param name="UsesExternalUrl">Whether to use an external Url for this content or not</param>
		/// <param name="ExternalUrl">The External Url to use if one is to be used</param>
		/// <param name="Description">Defined Content</param>
		/// <param name="IsSubCategoryOf">If this Product Category is a Sub Category Of another Category,
		/// This is the value of it's parent</param>
		/// <param name="DisplayStyle">Display Style of this Category</param>
		/// <param name="ImageRef">Reference to Image File</param>
		/// <param name="ThumbNailRef">Reference to Thumbnail of Image File</param>
		/// <param name="Caption">Caption for this Image</param>
		public void Add(string ProductCategoryName, 
			string ProductCategoryShortDescription,
			int ShowOnHomePage, 
			int ShowInCategoryBar,
			int CategoryDisplayOrder,
			int CategoryDisplayContent,
			string DefinedContentTitle, 
			int PopUpWidth,
			int PopUpHeight, 
			int UsesExternalUrl, 
			string ExternalUrl, 
			string Description,
			int IsSubCategoryOf,
			int DisplayStyle,
			string ImageRef,
			string ThumbNailRef,
			string Caption)
		{
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
			
			
			if (IsSubCategoryOf == 0)
				rowToAdd["IsSubCategoryOf"] = DBNull.Value;
			else
				rowToAdd["IsSubCategoryOf"] = IsSubCategoryOf;

			
			rowToAdd["DisplayStyle"] = DisplayStyle;
			rowToAdd["DefinedContentId"] = DefinedContentId;
			rowToAdd["ProductCategoryName"] = ProductCategoryName;
			rowToAdd["ProductCategoryShortDescription"] = ProductCategoryShortDescription;
			rowToAdd["ShowOnHomePage"] = ShowOnHomePage;
			rowToAdd["ShowInCategoryBar"] = ShowInCategoryBar;
			rowToAdd["IsPublic"] = 1;

			if (CategoryDisplayOrder == 0)
			{
				clsProductCategory thisPC = new clsProductCategory(thisDbType, localRecords.dbConnection);
				int numCategories = thisPC.GetAll();
				rowToAdd["CategoryDisplayOrder"] = numCategories + 1;
			} 
			else
				rowToAdd["CategoryDisplayOrder"] = CategoryDisplayOrder;
			
			rowToAdd["CategoryDisplayContent"] = CategoryDisplayContent;

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
		/// <param name="ProductCategoryId">ProductCategoryId (Primary Key of Record)</param>
		/// <param name="DefinedContentId">DefinedContent Associated with this Product Category</param>
		/// <param name="ProductCategoryName">Product Category's Name</param>
		/// <param name="ProductCategoryShortDescription">Product Category's Short Description</param>
		/// <param name="ShowOnHomePage">Whether to show this Category on the Homepage or not</param>
		/// <param name="ShowInCategoryBar">Whether to show this Category in the "Category Bar" or not</param>
		/// <param name="CategoryDisplayOrder">Display Order of this Category</param>
		/// <param name="CategoryDisplayContent">Content Type to display for this Category</param>
		/// <param name="DefinedContentTitle">DefinedContent's Title</param>
		/// <param name="PopUpWidth">Width of Pop-Up Window for this Content</param>
		/// <param name="PopUpHeight">Height of Pop-Up Window for this Content</param>
		/// <param name="UsesExternalUrl">Whether to use an external Url for this content or not</param>
		/// <param name="ExternalUrl">The External Url to use if one is to be used</param>
		/// <param name="Description">Defined Content</param>
		/// <param name="IsSubCategoryOf">If this Product Category is a Sub Category Of another Category,
		/// This is the value of it's parent</param>
		/// <param name="DisplayStyle">Display Style of this Category</param>
		/// <param name="DefinedContentImageId">Id of Defined Content Image Associated with this Product</param>
		/// <param name="ImageRef">Reference to Image File</param>
		/// <param name="ThumbNailRef">Reference to Thumbnail of Image File</param>
		/// <param name="Caption">Caption for this Image</param>
		public void Modify(int ProductCategoryId, 
			string ProductCategoryShortDescription, 
			int DefinedContentId, 
			string ProductCategoryName, 
			int ShowOnHomePage, 
			int ShowInCategoryBar,
			int CategoryDisplayOrder,
			int CategoryDisplayContent,
			string DefinedContentTitle, 
			int PopUpWidth,
			int PopUpHeight, 
			int UsesExternalUrl, 
			string ExternalUrl, 
			string Description,
			int IsSubCategoryOf,
			int DisplayStyle, 
			int DefinedContentImageId,
			string ImageRef,
			string ThumbNailRef,
			string Caption)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Firstly, Add the Defined Content
			DefinedContent.Modify(DefinedContentId,DefinedContentTitle, PopUpWidth, PopUpHeight, UsesExternalUrl, ExternalUrl, Description);
			DefinedContent.Save();

			//Then, Modify the Defined Content Image
			DefinedContentImage.Modify(DefinedContentImageId, DefinedContentId, ImageRef, ThumbNailRef, 1, Caption);
			DefinedContentImage.Save();

			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();
			
			if (IsSubCategoryOf == 0)
				rowToAdd["IsSubCategoryOf"] = DBNull.Value;
			else
				rowToAdd["IsSubCategoryOf"] = IsSubCategoryOf;

			
			rowToAdd["DisplayStyle"] = DisplayStyle;
			rowToAdd["DefinedContentId"] = DefinedContentId;
			rowToAdd["ProductCategoryId"] = ProductCategoryId;
			rowToAdd["ProductCategoryName"] = ProductCategoryName;
			rowToAdd["ShowOnHomePage"] = ShowOnHomePage;
			rowToAdd["ShowInCategoryBar"] = ShowInCategoryBar;
			rowToAdd["ProductCategoryShortDescription"] = ProductCategoryShortDescription;
			rowToAdd["IsPublic"] = 1;

			if (CategoryDisplayOrder == 0)
			{
				clsProductCategory thisPC = new clsProductCategory(thisDbType, localRecords.dbConnection);
				int numCategories = thisPC.GetAll();
				rowToAdd["CategoryDisplayOrder"] = numCategories + 1;
			} 
			else
				rowToAdd["CategoryDisplayOrder"] = CategoryDisplayOrder;

			rowToAdd["CategoryDisplayContent"] = CategoryDisplayContent;

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

		# region My_ Values ProductCategory


		/// <summary>
		/// ProductCategoryId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>ProductCategoryId for this Row</returns>
		public int my_ProductCategoryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductCategoryId"));
		}

		/// <summary>
		/// Product Category Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name of Product Category for this Row</returns>
		public string my_ProductCategoryName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ProductCategoryName");
		}


		/// <summary>
		/// Parent Product Category Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name of Product Category for the Parent of this Product Category</returns>
		public string my_ParentProductCategoryName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ParentProductCategoryName");
		}

		/// <summary>
		/// Product Category Short Description
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Description of Product Category for this Row</returns>
		public string my_ProductCategoryShortDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ProductCategoryShortDescription");
		}
		
		/// <summary>
		/// <see cref="clsDefinedContent.my_DefinedContentId">Defined ContentI d</see> of 
		/// <see cref="clsDefinedContent">Defined Content</see>
		/// Associated with this Product Category</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_DefinedContentId">Id</see> 
		/// of <see cref="clsDefinedContent">Defined Content</see> 
		/// for this Product Category</returns>	
		public int my_DefinedContentId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DefinedContentId"));
		}

		/// <summary>
		/// <see cref="clsProductCategory.my_ProductCategoryId">ProductCategoryId</see> of 
		/// <see cref="clsProductCategory">Parent Product Category</see>
		/// Associated with this ProductCategory</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProductCategory.my_ProductCategoryId">Id</see> 
		/// of <see cref="clsProductCategory">ProductCategory</see> 
		/// for this ProductCategory</returns>	
		public int my_IsSubCategoryOf(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "IsSubCategoryOf"));
		}

		/// <summary>
		/// Display Style for this Product Category
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Display Style for this Product Category</returns>
		public int my_DisplayStyle(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DisplayStyle"));
		}


		/// <summary>
		/// Whether this Product Category is shown on the Home Page or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this Product Category is shown on the Home Page or not</returns>
		public int my_ShowOnHomePage(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ShowOnHomePage"));
		}

		/// <summary>
		/// Whether this Product Category is shown in the Category Bar or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this Product Category is shown in the Category Bar or not</returns>
		public int my_ShowInCategoryBar(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ShowInCategoryBar"));
		}

		/// <summary>
		/// Category Display Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Display Order for this Category</returns>
		public int my_CategoryDisplayOrder(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CategoryDisplayOrder"));
		}

		/// <summary>
		/// Content type to Display for this Category
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Content type to Display for this Category</returns>
		public int my_CategoryDisplayContent(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CategoryDisplayContent"));
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

		#region My_ Values ProductSummary

		/// <summary>Total number of 
		/// <see cref="clsProduct">Products</see> 
		/// Associated with this Product Category</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total number of 
		/// <see cref="clsProduct">Products</see>
		/// Associated with this Product Category</returns>
		public int my_TotalProducts(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalProducts"));
		}

		/// <summary>Total number of SubCategories 
		/// Associated with this Product Category</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total number of SubCategories 
		/// Associated with this Product Category</returns>
		public int my_TotalSubCategories(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalSubCategories"));
		}

		# endregion
	}
}
