using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsShippingZone deals with everything to do with data about ShippingZones.
	/// </summary>

	[GuidAttribute("CF10A013-AE89-4570-831D-E316CAD15930")]
	public class clsShippingZone : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsShippingZone
		/// </summary>
		public clsShippingZone() : base("ShippingZone")
		{
		}

		/// <summary>
		/// Constructor for clsShippingZone; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsShippingZone(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("ShippingZone")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Country Summary Information
		/// </summary>
		public clsQueryPart CountrySummaryQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = ShippingZoneQueryPart();

			CountrySummaryQ.AddSelectColumn("tblCountry.TotalCountries");

			CountrySummaryQ.AddFromTable("(Select tblShippingZone.ShippingZoneId," + crLf
				+ "count(CountryId) as TotalCountries" + crLf
				+ "from tblShippingZone left outer join tblCountry " + crLf
				+ "on tblShippingZone.ShippingZoneId = tblCountry.ShippingZoneId " + crLf
				+ "Group by tblShippingZone.ShippingZoneId) tblCountry");

			CountrySummaryQ.AddJoin("tblShippingZone.ShippingZoneId = tblCountry.ShippingZoneId");

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[2];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = CountrySummaryQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By tblShippingZone.ShippingZoneCode" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsShippingZone
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("ShippingZoneCode", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ShippingZoneDescription", System.Type.GetType("System.String"));
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
		}

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all ShippingZones
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
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets an ShippingZone by ShippingZoneId
		/// </summary>
		/// <param name="ShippingZoneId">Id of Shipping Zone to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByShippingZoneId(int ShippingZoneId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ ShippingZoneId.ToString() + crLf;

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
		/// Returns ShippingZones with the specified ShippingZoneCode
		/// </summary>
		/// <param name="ShippingZoneCode">Name of ShippingZone</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of ShippingZones with the specified ShippingZoneCode</returns>
		public int GetByShippingZoneCode(string ShippingZoneCode, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(ShippingZoneCode, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblShippingZone.ShippingZoneCode " + match + crLf;

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
		/// <param name="ShippingZoneCode">Shipping Zone's Name</param>
		/// <param name="ShippingZoneDescription">Shipping Zone's Short Description</param>
		public void Add(string ShippingZoneCode, 
			string ShippingZoneDescription)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();
			
			rowToAdd["ShippingZoneCode"] = ShippingZoneCode;
			rowToAdd["ShippingZoneDescription"] = ShippingZoneDescription;
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
		/// <param name="ShippingZoneId">ShippingZoneId (Primary Key of Record)</param>
		/// <param name="ShippingZoneCode">Shipping Zone's Name</param>
		/// <param name="ShippingZoneDescription">Shipping Zone's Short Description</param>
		public void Modify(int ShippingZoneId, 
			string ShippingZoneCode, 
			string ShippingZoneDescription)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();
			
			rowToAdd["ShippingZoneId"] = ShippingZoneId;
			rowToAdd["ShippingZoneCode"] = ShippingZoneCode;
			rowToAdd["ShippingZoneDescription"] = ShippingZoneDescription;
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

		# region My_ Values


		/// <summary>
		/// <see cref="clsShippingZone.my_ShippingZoneId">Id</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsShippingZone.my_ShippingZoneId">Id</see> 
		/// of <see cref="clsShippingZone">ShippingZone</see> 
		/// </returns>	
		public int my_ShippingZoneId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ShippingZoneId"));
		}

		/// <summary>
		/// <see cref="clsShippingZone.my_ShippingZoneCode">Code</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsShippingZone.my_ShippingZoneCode">Code</see> 
		/// of <see cref="clsShippingZone">ShippingZone</see> 
		/// </returns>	
		public string my_ShippingZoneCode(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ShippingZoneCode");
		}


		/// <summary>
		/// <see cref="clsShippingZone.my_ShippingZoneDescription">Description</see> of 
		/// <see cref="clsShippingZone">ShippingZone</see></summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsShippingZone.my_ShippingZoneDescription">Description</see> 
		/// of <see cref="clsShippingZone">ShippingZone</see> 
		/// </returns>	
		public string my_ShippingZoneDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ShippingZoneDescription");
		}

		#endregion

		#region My_ Values CountrySummary

		/// <summary>Total number of 
		/// <see cref="clsCountry">Countries</see> 
		/// Associated with this Shipping Zone</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total number of 
		/// <see cref="clsCountry">Countries</see> of 
		/// Associated with this Shipping Zone</returns>
		public int my_TotalCountries(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalCountries"));
		}

		# endregion
	}
}
