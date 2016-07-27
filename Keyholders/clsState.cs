using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsState deals with everything to do with data about States.
	/// </summary>

	[GuidAttribute("8A65A2E8-7723-452c-8506-44D0CE3B9DC9")]
	public class clsState : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsState
		/// </summary>
		public clsState() : base("City")
		{
		}

		/// <summary>
		/// Constructor for clsState; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsState(clsRecordHandler.databaseType typeOfDb, 
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

			MainQ = CityQueryPart();
			CountryQ = CountryQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = CountryQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblCity.StateName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsState
		/// </summary>
		public override void Initialise()
		{

			//Initialise the data tables with Column Names

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
			Country = new clsCountry(thisDbType, localRecords.dbConnection);
		}

		/// <summary>
		/// Local Representation of the class <see cref="clsCountry">clsCountry</see>
		/// </summary>
		public clsCountry Country;

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all States
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = CountryQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets States by CountryId
		/// </summary>
		/// <param name="CountryId">Id of Country to retrieve States for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCountryId(int CountryId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = CountryQ;


			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblCity.CountryId = " 
				+ CountryId.ToString() + crLf;

			condition += "And tblCity.StateName is not Null" + crLf;

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
		/// Returns States with the specified StateName
		/// </summary>
		/// <param name="StateName">Name of State</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of States with the specified StateName</returns>
		public int GetByStateName(string StateName, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = CountryQ;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(StateName, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblCity.StateName " + match + crLf;

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
		/// Gets a Distinct List of State Names only. This is intented for use e.g. as an
		/// auto-completing drop down.
		/// </summary>
		/// <param name="StateName">Name of State</param>
		/// <param name="MatchCriteria">Criteria to match State Name, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctStateName(string StateName, int MatchCriteria)
		{
			string fieldRequired = "StateName";
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct "+ thisTable + "." + fieldRequired);

			string condition = "(Select * from " + thisTable + crLf;
			
			string match = MatchCondition(StateName, 
				(matchCriteria) MatchCriteria);

			//Additional Conditions
			condition += "Where " + fieldRequired + match + crLf;
			
			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				condition + ") " + thisTable,
				thisTable
				);

			//Ordering
			thisSqlQuery += "Order By tblCity.StateName" + crLf;

			return localRecords.GetRecords(thisSqlQuery);

		}

		#endregion

		# region My_ Values City
		
		/// <summary>
		/// <see cref="clsCity.my_CityName">CityName</see> of this
		/// <see cref="clsCity">City</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCity.my_CityName">CityName</see> of this
		/// <see cref="clsCity">City</see></returns>
		public string my_City_CityName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "City_CityName");
		}

		/// <summary>
		/// <see cref="clsCity.my_StateName">State Name</see> for this
		/// <see cref="clsCity">City</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCity.my_StateName">State Name</see> for this
		/// <see cref="clsCity">City</see></returns>
		public string my_City_StateName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "City_StateName");
		}

		/// <summary>
		/// <see cref="clsCountry.my_CountryId">CountryId</see> of 
		/// <see cref="clsCountry">Country</see>
		/// Associated with this City</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_CountryId">Id</see> 
		/// of <see cref="clsCountry">Country</see> 
		/// for this City</returns>	
		public int my_City_CountryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "City_CountryId"));
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
