using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;
using System.Web.Services;
using System.Xml;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Text;


namespace Keyholders
{
	/// <summary>
	/// clsImage deals with everything to do with data about Image.
	/// </summary>

	[GuidAttribute("943A3925-2DE5-42c8-A21C-6F0DCE4B82AE")]
	public class clsImage : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsImage
		/// </summary>
		public clsImage() : base("Image")
		{
		}

		/// <summary>
		/// Constructor for clsImage; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsImage(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Image")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to HighRiskGood Information
		/// </summary>
		public clsQueryPart HighRiskGoodQ = new clsQueryPart();


		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = ImageQueryPart();
			HighRiskGoodQ = HighRiskGoodQueryPart();
			
			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[2];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = HighRiskGoodQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);

			orderBySqlQuery = "Order By " + thisTable + ".SortOrder" + crLf;
		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsImage
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("HighRiskGoodId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("Caption", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ThumbFilename", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("FeatureFilename", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("FullFullname", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("SortOrder", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("Archive", System.Type.GetType("System.Int32"));

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
		/// Initialises an internal list of all Images
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

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
		/// Gets a Image by ImageId.
		/// </summary>
		/// <param name="ImageId">Id of Image to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByImageId(int ImageId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ ImageId.ToString();

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
		/// Gets Images by ThirdPartyId and HighRiskGoodId.
		/// </summary>
		/// <param name="HighRiskGoodId">HighRiskGoodId to retrieve Images for</param>
		/// <returns></returns>
		public int GetByHighRiskGoodId(int HighRiskGoodId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".HighRiskGoodId = "
				+ HighRiskGoodId.ToString() +  crLf;

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


		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Image table stack; the SaveImages method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="HighRiskGoodId">HighRiskGoodId</param>
		/// <param name="Caption">Caption</param>
		/// <param name="ThumbFilename">ThumbFilename</param>
		/// <param name="FeatureFilename">FeatureFilename</param>
		/// <param name="FullFullname">FullFullname</param>
		/// <param name="SortOrder">SortOrder</param>
		public void Add(int HighRiskGoodId,
			string Caption,
			string ThumbFilename,
			string FeatureFilename,
			string FullFullname,
			int SortOrder)
		{		
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["HighRiskGoodId"] = HighRiskGoodId;
			rowToAdd["Caption"] = Caption;
			rowToAdd["ThumbFilename"] = ThumbFilename;
			rowToAdd["FeatureFilename"] = FeatureFilename;
			rowToAdd["FullFullname"] = FullFullname;
			rowToAdd["SortOrder"] = SortOrder;
			rowToAdd["Archive"] = 0;
			
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
		/// internal Image table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="ImageId">ImageId (Primary Key of Record)</param>
		/// <param name="HighRiskGoodId">HighRiskGoodId</param>
		/// <param name="Caption">Caption</param>
		/// <param name="ThumbFilename">ThumbFilename</param>
		/// <param name="FeatureFilename">FeatureFilename</param>
		/// <param name="FullFullname">FullFullname</param>
		/// <param name="SortOrder">SortOrder</param>
		/// <param name="Archive">Archive</param>
		public void Modify(int ImageId, 
			int HighRiskGoodId,
			string Caption,
			string ThumbFilename,
			string FeatureFilename,
			string FullFullname,
			int SortOrder,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ImageId"] = ImageId;
			rowToAdd["HighRiskGoodId"] = HighRiskGoodId;
			rowToAdd["Caption"] = Caption;
			rowToAdd["ThumbFilename"] = ThumbFilename;
			rowToAdd["FeatureFilename"] = FeatureFilename;
			rowToAdd["FullFullname"] = FullFullname;
			rowToAdd["SortOrder"] = SortOrder;
			rowToAdd["Archive"] = Archive;
			
			//Validate the data supplied
			Validate(rowToAdd, true);

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
		
		}
		
		#endregion

		# region My_ Values Image

		/// <summary>
		/// <see cref="clsImage.my_ImageId">Id</see> of
		/// <see cref="clsImage">Image</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsImage.my_ImageId">Id</see> of 
		/// <see cref="clsImage">Image</see> 
		/// </returns>
		public int my_ImageId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ImageId"));
		}

	
		/// <summary>
		/// <see cref="clsImage.my_Caption">Caption</see> of
		/// <see cref="clsImage">Image</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsImage.my_Caption">Caption</see> of 
		/// <see cref="clsImage">Image</see> 
		/// </returns>
		public string my_Caption(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Caption");
		}
				
		/// <summary>
		/// <see cref="clsImage.my_ThumbFilename">ThumbFilename</see> of
		/// <see cref="clsImage">Image</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsImage.my_ThumbFilename">ThumbFilename</see> of 
		/// <see cref="clsImage">Image</see> 
		/// </returns>
		public string my_ThumbFilename(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ThumbFilename");
		}

		/// <summary>
		/// <see cref="clsImage.my_FeatureFilename">FeatureFilename</see> of
		/// <see cref="clsImage">Image</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsImage.my_FeatureFilename">FeatureFilename</see> of 
		/// <see cref="clsImage">Image</see> 
		/// </returns>
		public string my_FeatureFilename(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FeatureFilename");
		}

		/// <summary>
		/// <see cref="clsImage.my_FullFullname">FullFullname</see> of
		/// <see cref="clsImage">Image</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsImage.my_FullFullname">FullFullname</see> of 
		/// <see cref="clsImage">Image</see> 
		/// </returns>
		public string my_FullFullname(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FullFullname");
		}

		#endregion

		# region my_HighRiskGood_ Values HighRiskGood

		/// <summary>
		/// HighRiskGoodId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>HighRiskGoodId for this Row</returns>
		public int my_HighRiskGood_HighRiskGoodId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "HighRiskGood_HighRiskGoodId"));
		}


		/// <summary>
		/// <see cref="clsPremise.my_PremiseId">Id</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_PremiseId">Id</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_HighRiskGood_PremiseId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "HighRiskGood_PremiseId"));
		}

		/// <summary>
		/// High Risk Good Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_HighRiskGoodName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_HighRiskGoodName");
		}

		/// <summary>
		/// High Risk Good Description
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Description of HighRiskGood for this Row</returns>
		public string my_HighRiskGood_HighRiskGoodDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_HighRiskGoodDescription");
		}

		/// <summary>
		/// SerialNumber
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_SerialNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_SerialNumber");
		}

		/// <summary>
		/// Information Type of this HighRiskGood
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Information Type of this HighRiskGood
		/// </returns>
		public int my_HighRiskGood_InformationType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "HighRiskGood_InformationType"));
		}



		/// <summary>
		/// MakeBrand
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_MakeBrand(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_MakeBrand");
		}


		/// <summary>
		/// Model
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_Model(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_Model");
		}


		/// <summary>
		/// PurchasePlace
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_PurchasePlace(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_PurchasePlace");
		}


		/// <summary>
		/// PurchaseDate
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_PurchaseDate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_PurchaseDate");
		}


		/// <summary>
		/// PricePaid
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_PricePaid(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_PricePaid");
		}

		
		/// <summary>
		/// Status
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Status for this Row</returns>
		public int my_HighRiskGood_Status(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "HighRiskGood_Status"));
		}

		/// <summary>
		/// StatusDate
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_StatusDate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_StatusDate");
		}


		/// <summary>
		/// InsuranceCompany
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_InsuranceCompany(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_InsuranceCompany");
		}


		/// <summary>
		/// InsuranceClaimNumber
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_InsuranceClaimNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_InsuranceClaimNumber");
		}


		/// <summary>
		/// PoliceFileNumber
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGood_PoliceFileNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGood_PoliceFileNumber");
		}


		#endregion

	}
}
