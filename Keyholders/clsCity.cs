using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsCity deals with everything to do with data about Citys.
	/// </summary>
	
	[GuidAttribute("649006EE-B97A-4a76-AA69-60EABD60B7E9")]
	public class clsCity : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCity
		/// </summary>
		public clsCity() : base("City")
		{
		}

		/// <summary>
		/// Constructor for clsCity; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsCity(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("City")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Country Information
		/// </summary>
		public clsQueryPart CountryQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			CountryQ = CountryQueryPart();
			MainQ = CityQueryPart();


			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[2];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = CountryQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);

			orderBySqlQuery = "Order By tblCity.CityName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsCity
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("CityName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("StateName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CountryId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("IncludeAsPublic", System.Type.GetType("System.Int32"));
			
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
		/// Initialises an internal list of all Citys
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets an City by CityId
		/// </summary>
		/// <param name="CityId">Id of City to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCityId(int CityId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ CityId.ToString() + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets an City by CityId and StateName
		/// </summary>
		/// <param name="CityId">Id of City</param>
		/// <param name="StateName">Name of State</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCityIdStateName(int CityId, string StateName)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			//Condition
			condition += "Where " + thisTable + "." + thisPk + " = " + CityId.ToString() + crLf;
			condition += "And " + thisTable + ".StateName " + MatchCondition(StateName, matchCriteria.exactMatch) 
				+ ") " + thisTable + crLf;

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
		/// Gets an City by CityId and StateName and CountryId
		/// </summary>
		/// <param name="CityName">Name of City</param>
		/// <param name="StateName">Name of State</param>
		/// <param name="CountryId">Id of Country</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCityNameStateNameCountryId(string CityName, string StateName, int CountryId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "";

			if (StateName == "")
				condition = " tblCity.StateName is null " + crLf;
			else
				condition = " tblCity.StateName " + MatchCondition(StateName, matchCriteria.exactMatch) + crLf;

			if (CityName == "")
				condition += " and tblCity.CityName is null " + crLf;
			else
				condition += " tblCity.CityName " + MatchCondition(CityName, matchCriteria.exactMatch) + crLf;

			condition += " and tblCity.CountryId = " + CountryId.ToString() + crLf;

			condition = "(Select * from " + thisTable + crLf
				+  "Where " + condition
				+ ") " + thisTable;

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
		/// Gets Citys by CountryId
		/// </summary>
		/// <param name="CountryId">Id of Country to retrieve Cities for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCountryId(int CountryId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			condition += "Where tblCity.CountryId = " + CountryId.ToString() + crLf
				+ " and tblCity.IncludeAsPublic = 1 " + crLf
				+ ") " + thisTable + crLf;
			

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
		/// Returns Citys with the specified CityName
		/// </summary>
		/// <param name="CityName">Name of City</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of Citys with the specified CityName</returns>
		public int GetByCityName(string CityName, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf	
				+ "Where tblCity.CityName " 
				+ MatchCondition(CityName, (matchCriteria) MatchCriteria) + crLf
				+ " and tblCity.IncludeAsPublic = 1 " + crLf
				+ ") " + thisTable + crLf;

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
		/// Returns Citys with the specified CityName
		/// </summary>
		/// <param name="CityName">Name of City</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <param name="CountryId">Id of Country to match</param>
		/// <returns>Number of Citys with the specified City Name and CountryId</returns>
		public int GetByCityNameCountryId(string CityName, int MatchCriteria, int CountryId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf
				+ "Where tblCity.CityName " 
				+ MatchCondition(CityName,(matchCriteria) MatchCriteria) + crLf
				+ "And tblCity.CountryId = " + CountryId.ToString() + crLf
				+ "And tblCity.IncludeAsPublic = 1 "+ crLf
				+ ") " + thisTable + crLf;

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
		/// Gets a Distinct List of City Names only. This is intented for use e.g. as an
		/// auto-completing drop down.
		/// </summary>
		/// <param name="CityName">Name of City</param>
		/// <param name="MatchCriteria">Criteria to match City Name, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctCityName(string CityName, int MatchCriteria)
		{
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct "+ thisTable + ".CityName");

			string condition = "(Select * from " + thisTable + crLf
				+ "Where CityName " + MatchCondition(CityName, (matchCriteria) MatchCriteria) + crLf
				+ "And tblCity.IncludeAsPublic = 1 "+ crLf
				+ ") " + thisTable + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				condition,
				thisTable
				);

			//Ordering
			thisSqlQuery += "Order By tblCity.CityName";

			return localRecords.GetRecords(thisSqlQuery);

		}

		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CountryId">Country Associated with this City</param>
		/// <param name="CityName">City's Name</param>
		/// <param name="StateName">State for this city (if it has one)</param>
		/// <param name="IncludeAsPublic">Whether to include this City as Public or not</param>
		public void Add(int CountryId,
			string CityName, 
			string StateName,
			int IncludeAsPublic
			)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			if (StateName != "")
				rowToAdd["StateName"] = StateName;
			else
				rowToAdd["StateName"] = DBNull.Value;


			rowToAdd["CityName"] = CityName;
			rowToAdd["CountryId"] = CountryId;
			rowToAdd["IncludeAsPublic"] = 0;
			
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
		/// <param name="CityId">CityId (Primary Key of Record)</param>
		/// <param name="CountryId">Country Associated with this City</param>
		/// <param name="CityName">City's Name</param>
		/// <param name="StateName">State for this city (if it has one)</param>
		/// <param name="IncludeAsPublic">Whether to include this City as Public or not</param>
		public void Modify(int CityId,
			int CountryId,
			string CityName, 
			string StateName, 
			int IncludeAsPublic)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			if (StateName != "")
				rowToAdd["StateName"] = StateName;
			else
				rowToAdd["StateName"] = DBNull.Value;


			rowToAdd["CityId"] = CityId;
			rowToAdd["CityName"] = CityName;
			rowToAdd["CountryId"] = CountryId;
			rowToAdd["IncludeAsPublic"] = IncludeAsPublic;

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

		# region My_ Values City
		
		/// <summary>
		/// <see cref="clsCity.my_CityId">Id</see> of this
		/// <see cref="clsCity">City</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCity.my_CityId">Id</see> of this
		/// <see cref="clsCity">City</see></returns>
		public int my_CityId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CityId"));
		}
		
		/// <summary>
		/// <see cref="clsCity.my_CityName">CityName</see> of this
		/// <see cref="clsCity">City</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCity.my_CityName">CityName</see> of this
		/// <see cref="clsCity">City</see></returns>
		public string my_CityName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CityName");
		}

		/// <summary>
		/// <see cref="clsCity.my_StateName">State Name</see> for this
		/// <see cref="clsCity">City</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCity.my_StateName">State Name</see> for this
		/// <see cref="clsCity">City</see></returns>
		public string my_StateName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "StateName");
		}

		/// <summary>
		/// <see cref="clsCountry.my_CountryId">CountryId</see> of 
		/// <see cref="clsCountry">Country</see>
		/// Associated with this City</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_CountryId">Id</see> 
		/// of <see cref="clsCountry">Country</see> 
		/// for this City</returns>	
		public int my_CountryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CountryId"));
		}

		#endregion

		# region My_ Values Country

		/// <summary>
		/// <see cref="clsCountry.my_CountryName">Name</see> of
		/// <see cref="clsCountry">Country</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_CountryName">Name</see> of 
		/// <see cref="clsCountry">Country</see> 
		/// </returns>
		public string my_Country_CountryName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Country_CountryName");
		}

		/// <summary>
		/// <see cref="clsCountry.my_LocalTaxApplies">Whether Local Tax Applies</see> to this
		/// <see cref="clsCountry">Country</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_LocalTaxApplies">Whether Local Tax Applies</see> to this 
		/// <see cref="clsCountry">Country</see> 
		/// </returns>
		public int my_Country_LocalTaxApplies(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Country_LocalTaxApplies"));
		}

		/// <summary>
		/// <see cref="clsCountry.my_CanShipToCountry">Whether we can ship</see> to this
		/// <see cref="clsCountry">Country</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_CanShipToCountry">Whether we can ship</see> to this 
		/// <see cref="clsCountry">Country</see> 
		/// </returns>
		public int my_Country_CanShipToCountry(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Country_CanShipToCountry"));
		}


		/// <summary>
		/// <see cref="clsShippingZone.my_ShippingZoneId">ShippingZoneId</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see>
		/// Associated with this Country</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsShippingZone.my_ShippingZoneId">Id</see> 
		/// of <see cref="clsShippingZone">ShippingZone</see> 
		/// for this Country</returns>	
		public int my_Country_ShippingZoneId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Country_ShippingZoneId"));
		}

		#endregion

	}
}
