using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsCountry deals with everything to do with data about Countries.
	/// </summary>
	
	[GuidAttribute("4E36E6A0-6E0D-4d50-94C5-BB56D5139BB9")]
	public class clsCountry : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCountry
		/// </summary>
		public clsCountry() : base("Country")
		{
		}

		/// <summary>
		/// Constructor for clsCountry; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsCountry(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Country")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to ShippingZone Information
		/// </summary>
		public clsQueryPart ShippingZoneQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			MainQ.AddSelectColumn("tblCountry.CountryId");
			MainQ.AddSelectColumn("tblCountry.ShippingZoneId");
			MainQ.AddSelectColumn("tblCountry.CountryName");
			MainQ.AddSelectColumn("tblCountry.LocalTaxApplies");
			MainQ.AddSelectColumn("tblCountry.CanShipToCountry");

			MainQ.AddFromTable(thisTable);

			ShippingZoneQ = ShippingZoneQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = ShippingZoneQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblCountry.CountryName" + crLf;

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsCountry
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("ShippingZoneId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CountryName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("LocalTaxApplies", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CanShipToCountry", System.Type.GetType("System.Int32"));
			
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
		}

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Countries
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets an Country by CountryId
		/// </summary>
		/// <param name="CountryId">Id of Country to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCountryId(int CountryId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ CountryId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Returns Countries with the specified CountryName
		/// </summary>
		/// <param name="CountryName">Name of Country</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of Countries with the specified CountryName</returns>
		public int GetByCountryName(string CountryName, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = ShippingZoneQ;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(CountryName, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblCountry.CountryName " + match + crLf;

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
		/// Gets a Distinct List of Country Names only. This is intented for use e.g. as an
		/// auto-completing drop down.
		/// </summary>
		/// <param name="CountryName">Name of Country</param>
		/// <param name="MatchCriteria">Criteria to match Country Name, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctCountryName(string CountryName, int MatchCriteria)
		{
			string fieldRequired = "CountryName";
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct "+ thisTable + "." + fieldRequired);

			string condition = "(Select * from " + thisTable + crLf;
			
			string match = MatchCondition(CountryName, 
				(matchCriteria) MatchCriteria);

			//Additional Conditions
			condition += "Where " + fieldRequired + match + crLf;
			
			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				condition + ") " + thisTable,
				thisTable
				);

			//Ordering
			thisSqlQuery += "Order By tblCountry.CountryName" + crLf;

			return localRecords.GetRecords(thisSqlQuery);

		}

		/// <summary>
		/// Gets Countries by ShippingZoneId
		/// </summary>
		/// <param name="ShippingZoneId">Id of Shipping Zone to retrieve Countries for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByShippingZoneId(int ShippingZoneId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = ShippingZoneQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblCountry.ShippingZoneId = " 
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

		#endregion

		# region Add/Modify/Validate/Save


		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="ShippingZoneId">
		/// <see cref="clsShippingZone.my_ShippingZoneId">ShippingZoneId</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see>
		/// Associated with this Country</param>
		/// <param name="CountryName">Country's Name</param>
		/// <param name="LocalTaxApplies">Whether the Suppliers Local Tax (e.g. VAT, GST) applies in this country</param>
		/// <param name="CanShipToCountry">Whether this Country Can be shipped to.</param>
		public void Add(int ShippingZoneId,
			string CountryName,
			int LocalTaxApplies,
			int CanShipToCountry)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["ShippingZoneId"] = ShippingZoneId;
			rowToAdd["CountryName"] = CountryName;
			rowToAdd["LocalTaxApplies"] = LocalTaxApplies;
			rowToAdd["CanShipToCountry"] = CanShipToCountry;
			
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
		/// <param name="CountryId">CountryId (Primary Key of Record)</param>
		/// <param name="ShippingZoneId">
		/// <see cref="clsShippingZone.my_ShippingZoneId">ShippingZoneId</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see>
		/// Associated with this Country</param>
		/// <param name="CountryName">Country's Name</param>
		/// <param name="LocalTaxApplies">Whether the Suppliers Local Tax (e.g. VAT, GST) applies in this country</param>
		/// <param name="CanShipToCountry">Whether this Country Can be shipped to.</param>		
		public void Modify(int CountryId, 
			int ShippingZoneId,
			string CountryName,
			int LocalTaxApplies,
			int CanShipToCountry)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["CountryId"] = CountryId;
			rowToAdd["ShippingZoneId"] = ShippingZoneId;
			rowToAdd["CountryName"] = CountryName;
			rowToAdd["LocalTaxApplies"] = LocalTaxApplies;
			rowToAdd["CanShipToCountry"] = CanShipToCountry;

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
		/// <param name="CountryId">CountryId (Primary Key of Record)</param>
		/// <param name="ShippingZoneId">
		/// <see cref="clsShippingZone.my_ShippingZoneId">ShippingZoneId</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see>
		/// Associated with this Country</param>
		/// <param name="LocalTaxApplies">Whether the Suppliers Local Tax (e.g. VAT, GST) applies in this country</param>
		/// <param name="CanShipToCountry">Whether this Country Can be shipped to.</param>		
		public void Set(int CountryId, 
			int ShippingZoneId,
			int LocalTaxApplies,
			int CanShipToCountry)
		{
			clsCountry thisCountry = new clsCountry(thisDbType, localRecords.dbConnection);
			thisCountry.GetByCountryId(CountryId);

			Modify(CountryId, ShippingZoneId, thisCountry.my_CountryName(0).ToString(), LocalTaxApplies, CanShipToCountry);
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

		# region My_ Values Country


		/// <summary>
		/// <see cref="clsCountry.my_CountryId">Id</see> of
		/// <see cref="clsCountry">Country</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_CountryId">Id</see> of 
		/// <see cref="clsCountry">Country</see> 
		/// </returns>
		public int my_CountryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CountryId"));
		}

		/// <summary>
		/// <see cref="clsCountry.my_CountryName">Name</see> of
		/// <see cref="clsCountry">Country</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_CountryName">Name</see> of 
		/// <see cref="clsCountry">Country</see> 
		/// </returns>
		public string my_CountryName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CountryName");
		}

		/// <summary>
		/// <see cref="clsCountry.my_LocalTaxApplies">Whether Local Tax Applies</see> to this
		/// <see cref="clsCountry">Country</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_LocalTaxApplies">Whether Local Tax Applies</see> to this 
		/// <see cref="clsCountry">Country</see> 
		/// </returns>
		public int my_LocalTaxApplies(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "LocalTaxApplies"));
		}

		/// <summary>
		/// <see cref="clsCountry.my_CanShipToCountry">Whether we can ship</see> to this
		/// <see cref="clsCountry">Country</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_CanShipToCountry">Whether we can ship</see> to this 
		/// <see cref="clsCountry">Country</see> 
		/// </returns>
		public int my_CanShipToCountry(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CanShipToCountry"));
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
