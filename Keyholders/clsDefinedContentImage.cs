using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsDefinedContentImage deals with everything to do with data about DefinedContentImages.
	/// </summary>
	
	[GuidAttribute("2CF6E050-13A0-4753-9A7B-D106BA936CAB")]
	public class clsDefinedContentImage : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsDefinedContentImage
		/// </summary>
		public clsDefinedContentImage() : base("DefinedContentImage")
		{
		}

		/// <summary>
		/// Constructor for clsDefinedContentImage; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsDefinedContentImage(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("DefinedContentImage")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to DefinedContent Information
		/// </summary>
		public clsQueryPart DefinedContentQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			DefinedContentQ = DefinedContentQueryPart();

			MainQ.AddSelectColumn("tblDefinedContentImage.DefinedContentImageId");
			MainQ.AddSelectColumn("tblDefinedContentImage.DefinedContentId");
			MainQ.AddSelectColumn("tblDefinedContentImage.ImagePosition");
			MainQ.AddSelectColumn("tblDefinedContentImage.ImageRef");
			MainQ.AddSelectColumn("tblDefinedContentImage.ThumbNailRef");
			MainQ.AddSelectColumn("tblDefinedContentImage.Caption");

			MainQ.AddFromTable(thisTable);

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblDefinedContentImage.ImageRef" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsDefinedContentImage
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("DefinedContentId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ImageRef", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ThumbNailRef", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ImagePosition", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("Caption", System.Type.GetType("System.String"));

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
		}

		/// <summary>
		/// Local Representation of the class <see cref="clsDefinedContent">clsDefinedContent</see>
		/// </summary>
		public clsDefinedContent DefinedContent;

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all DefinedContentImages
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets an Defined Content Image by DefinedContentImageId
		/// </summary>
		/// <param name="DefinedContentImageId">Id of Defined Content Image to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByDefinedContentImageId(int DefinedContentImageId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ DefinedContentImageId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Defined Content Images by DefinedContentId
		/// </summary>
		/// <param name="DefinedContentId">Id of Defined Content to retrieve Defined Content Images for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByDefinedContentId(int DefinedContentId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;


			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblDefinedContentImage.DefinedContentId = " 
				+ DefinedContentId.ToString() + crLf;

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
		/// Returns Content with the specified Image Name
		/// </summary>
		/// <param name="ImageName">Name of Image</param>
		/// <returns>Number of DefinedContents with the specified DefinedContentTitle</returns>
		public int GetImagesInDefinedContent(string ImageName)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[1] = DefinedContentQ;

			string match = MatchCondition(ImageName, 
				(matchCriteria.contains));

			//Additional Condition
			string condition = "concat_ws(' ',tblDefinedContent.Description,tblDefinedContentImage.ImageRef,tblDefinedContentImage.ThumbNaiRef) " + match;

			queries[1].AddJoin(condition);

			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				OrderByColumns);

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
		/// <param name="DefinedContentId">DefinedContent Associated with this DefinedContentImage</param>
		/// <param name="ImageRef">Reference to Image File</param>
		/// <param name="ThumbNailRef">Reference to Thumbnail of Image File</param>
		/// <param name="ImagePosition">Position of Image with respect to Other Images for this Defined Content</param>
		/// <param name="Caption">Caption for this Image</param>
		public void Add(int DefinedContentId, 
			string ImageRef, 
			string ThumbNailRef, 
			int ImagePosition,
			string Caption)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["DefinedContentId"] = DefinedContentId;
			rowToAdd["ImageRef"] = ImageRef;
			rowToAdd["ThumbNailRef"] = ThumbNailRef;
			rowToAdd["ImagePosition"] = ImagePosition;
			rowToAdd["Caption"] = Caption;
			
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
		/// <param name="DefinedContentImageId">DefinedContentImageId (Primary Key of Record)</param>
		/// <param name="DefinedContentId">DefinedContent Associated with this DefinedContentImage</param>
		/// <param name="ImageRef">Reference to Image File</param>
		/// <param name="ThumbNailRef">Reference to Thumbnail of Image File</param>
		/// <param name="ImagePosition">Position of Image with respect to Other Images for this Defined Content</param>
		/// <param name="Caption">Caption for this Image</param>
		public void Modify(int DefinedContentImageId, 
			int DefinedContentId, 
			string ImageRef, 
			string ThumbNailRef, 
			int ImagePosition,
			string Caption)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();

			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["DefinedContentImageId"] = DefinedContentImageId;
			rowToAdd["DefinedContentId"] = DefinedContentId;
			rowToAdd["ImageRef"] = ImageRef;
			rowToAdd["ThumbNailRef"] = ThumbNailRef;
			rowToAdd["ImagePosition"] = ImagePosition;
			rowToAdd["Caption"] = Caption;

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

		# region My_ Values DefinedContentImage

		/// <summary>
		/// <see cref="clsDefinedContentImage.my_DefinedContentImageId">DefinedContentImageId</see> of 
		/// <see cref="clsDefinedContentImage">DefinedContentImage</see>
		/// Associated with this DefinedContentImageImage</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContentImage.my_DefinedContentImageId">Id</see> 
		/// of <see cref="clsDefinedContentImage">DefinedContentImage</see> 
		/// for this DefinedContentImageImage</returns>	
		public int my_DefinedContentImageId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DefinedContentImageId"));
		}

		/// <summary>
		/// <see cref="clsDefinedContent.my_DefinedContentId">DefinedContentId</see> of 
		/// <see cref="clsDefinedContent">DefinedContent</see>
		/// Associated with this DefinedContentImage</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContent.my_DefinedContentId">Id</see> 
		/// of <see cref="clsDefinedContent">DefinedContent</see> 
		/// for this DefinedContentImage</returns>	
		public int my_DefinedContentId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DefinedContentId"));
		}

		/// <summary>
		/// <see cref="clsDefinedContentImage.my_ImageRef">Defined Content Image Reference</see> to
		/// <see cref="clsDefinedContentImage">Defined Content Image</see>
		/// Associated with this Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContentImage.my_ImageRef">Reference</see> 
		/// to <see cref="clsDefinedContentImage">Defined Content Image</see> 
		/// for this Product</returns>	
		public string my_ImageRef(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ImageRef");
		}

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

		/// <summary>
		/// <see cref="clsDefinedContentImage.my_Caption">Defined Content Image Caption</see> for
		/// <see cref="clsDefinedContentImage">Defined Content Image</see>
		/// Associated with this Product</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsDefinedContentImage.my_Caption">Caption</see> 
		/// for <see cref="clsDefinedContentImage">Defined Content Image</see> 
		/// for this Product</returns>	
		public string my_Caption(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Caption");
		}


		# endregion

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


	}
}
