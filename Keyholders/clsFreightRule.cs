using System;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.Odbc;
using Resources;
using System.Collections;

namespace Keyholders
{
	/// <summary>
	/// clsFreightRule deals with everything to do with data about FreightRules.
	/// </summary>
	
	[GuidAttribute("44A26F27-B5E5-4b3f-A5DA-751E6266202E")]
	public class clsFreightRule : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsFreightRule
		/// </summary>
		public clsFreightRule() : base("FreightRule")
		{
		}

		/// <summary>
		/// Constructor for clsFreightRule; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsFreightRule(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("FreightRule")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Customer Group Information
		/// </summary>
		public clsQueryPart CustomerGroupQ = new clsQueryPart();


		/// <summary>
		/// Part of the Query that Pertains to Shipping Zone Information
		/// </summary>
		public clsQueryPart ShippingZoneQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Product Information
		/// </summary>
		public clsQueryPart ProductQ = new clsQueryPart();

		
		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			ShippingZoneQ = ShippingZoneQueryPart();
			CustomerGroupQ = CustomerGroupQueryPart();
			ProductQ = ProductQueryPart();

			MainQ.AddSelectColumn("tblFreightRule.FreightRuleId");
			MainQ.AddSelectColumn("tblFreightRule.FreightRuleName");
			MainQ.AddSelectColumn("tblFreightRule.FreightRuleDescription");
			MainQ.AddSelectColumn("tblFreightRule.MinTotalWeight");
			MainQ.AddSelectColumn("tblFreightRule.MaxTotalWeight");
			MainQ.AddSelectColumn("tblFreightRule.MinTotalValue");
			MainQ.AddSelectColumn("tblFreightRule.MaxTotalValue");
			MainQ.AddSelectColumn("tblFreightRule.FRCost");
			MainQ.AddSelectColumn("tblFreightRule.Archive");

			MainQ.AddFromTable(thisTable);

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblFreightRule.FreightRuleName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsFreightRule
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("FreightRuleName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("FreightRuleDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("MinTotalWeight", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("MaxTotalWeight", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("MinTotalValue", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("MaxTotalValue", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("FRCost", System.Type.GetType("System.Decimal"));
			newDataToAdd.Columns.Add("Archive", System.Type.GetType("System.Int64"));
			
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

			GetGeneralSettings();
		}


		/// <summary>
		/// Struct for Products and Prices Associated with a Freight Rule
		/// </summary>
		public struct ProductType
		{
			/// <summary>
			/// Id of Freight Rule this Product Type Applies to
			/// </summary>
			public int FreightRuleId;
			/// <summary>
			/// Id of Product this Product Type Applies to
			/// </summary>
			public int ProductId;
			/// <summary>
			/// Freight Price of this Product for this Freight Rule
			/// </summary>
			public decimal Price;
		}


		/// <summary>
		/// Struct for Customer Groups Associated with a Freight Rule
		/// </summary>
		public struct CustomerGroupType
		{
			/// <summary>
			/// Id of Freight Rule this Customer Group Type Applies to
			/// </summary>
			public int FreightRuleId;
			/// <summary>
			/// Id of Customer Group this Customer Group Type Applies to
			/// </summary>
			public int CustomerGroupId;
		}

		/// <summary>
		/// Struct for Shipping Zones Associated with a Freight Rule
		/// </summary>
		public struct ShippingZoneType
		{
			/// <summary>
			/// Id of Freight Rule this Shipping Zone Type Applies to
			/// </summary>
			public int FreightRuleId;
			/// <summary>
			/// Id of Shipping Zone this Shipping Zone Type Applies to
			/// </summary>
			public int ShippingZoneId;
		}


		/// <summary>
		/// Customer Groups for which to aply a new freight rule
		/// </summary>
		ArrayList CustomerGroups = new ArrayList();

		/// <summary>
		/// Shipping Zones for which to aply a new freight rule
		/// </summary>
		ArrayList ShippingZones = new ArrayList();

		/// <summary>
		/// Products for which to aply a new freight rule
		/// </summary>
		ArrayList Products = new ArrayList();

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all FreightRules
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "";

			switch (thisArchivedRecordBehaviour)
			{
				case archivedRecordBehaivour.archivedOnly:
					condition = thisTable + ".Archive = 1" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedOnly:
					condition = thisTable + ".Archive = 0" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedAndArchived:
				default:
					break;
			}

			if (condition == "")
				thisSqlQuery = QB.BuildSqlStatement(
					queries, OrderByColumns);
			else
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
		/// Gets an Freight Rule by FreightRuleId
		/// </summary>
		/// <param name="FreightRuleId">Id of Freight Rule to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByFreightRuleId(int FreightRuleId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ FreightRuleId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets an Freight Rule by ProductId
		/// </summary>
		/// <param name="ProductId">Id of Freight Rule to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByProductId(int ProductId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			queries[0].AddSelectColumn("tblFrSzCgP.Cost");

			queries[0].AddFromTable("(select FreightRuleId, count(FrSzCgPId), Cost" + crLf
				+ "from tblFrSzCgP where ProductId = " + crLf
				+ ProductId.ToString() + " group by FreightRuleId) tblFrSzCgP");

			queries[0].AddJoin("tblFrSzCgP.FreightRuleId = " + thisTable + "." + thisPk);

			string condition = "";

			switch (thisArchivedRecordBehaviour)
			{
				case archivedRecordBehaivour.archivedOnly:
					condition = thisTable + ".Archive = 1" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedOnly:
					condition = thisTable + ".Archive = 0" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedAndArchived:
				default:
					break;
			}
			
			if (condition == "")
				thisSqlQuery = QB.BuildSqlStatement(
					queries, OrderByColumns);
			else
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
		/// Gets an Freight Rule by CustomerGroupId
		/// </summary>
		/// <param name="CustomerGroupId">Id of Freight Rule to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerGroupId(int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].AddFromTable("(select FreightRuleId, count(FrSzCgPId) "
				+ "from tblFrSzCgP where CustomerGroupId = "
				+ CustomerGroupId.ToString() + " group by FreightRuleId) tblFrSzCgP");

			queries[0].AddJoin("tblFrSzCgP.FreightRuleId = " + thisTable + "." + thisPk);

			string condition = "";

			switch (thisArchivedRecordBehaviour)
			{
				case archivedRecordBehaivour.archivedOnly:
					condition = thisTable + ".Archive = 1" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedOnly:
					condition = thisTable + ".Archive = 0" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedAndArchived:
				default:
					break;
			}
			
			if (condition == "")
				thisSqlQuery = QB.BuildSqlStatement(
					queries, OrderByColumns);
			else
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
		/// Gets an Freight Rule by ShippingZoneId
		/// </summary>
		/// <param name="ShippingZoneId">Id of Freight Rule to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByShippingZoneId(int ShippingZoneId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].AddFromTable("(select FreightRuleId, count(FrSzCgPId) "
				+ "from tblFrSzCgP where ShippingZoneId = "
				+ ShippingZoneId.ToString() + " group by FreightRuleId) tblFrSzCgP");

			queries[0].AddJoin("tblFrSzCgP.FreightRuleId = " + thisTable + "." + thisPk);

			string condition = "";

			switch (thisArchivedRecordBehaviour)
			{
				case archivedRecordBehaivour.archivedOnly:
					condition = thisTable + ".Archive = 1" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedOnly:
					condition = thisTable + ".Archive = 0" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedAndArchived:
				default:
					break;
			}
			
			if (condition == "")
				thisSqlQuery = QB.BuildSqlStatement(
					queries, OrderByColumns);
			else
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
		/// Returns FreightRules with the specified FreightRuleName
		/// </summary>
		/// <param name="FreightRuleName">Name of FreightRule</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of FreightRules with the specified FreightRuleName</returns>
		public int GetByFreightRuleName(string FreightRuleName, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(FreightRuleName, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblFreightRule.FreightRuleName " + match + crLf;

			switch (thisArchivedRecordBehaviour)
			{
				case archivedRecordBehaivour.archivedOnly:
					condition += " And " + thisTable + ".Archive = 1" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedOnly:
					condition += " And " + thisTable + ".Archive = 0" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedAndArchived:
				default:
					break;
			}

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
		/// Gets all CustomerGroupIds and whether they are applicable to this Freight Rule
		/// </summary>
		/// <param name="FreightRuleId">Id of Freight Rule to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetCustomerGroupsForFreightRuleId(int FreightRuleId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = CustomerGroupQ;

			queries[0].AddSelectColumn("AppliesToThisFreightRule");

			queries[0].AddFromTable("tblcustomerGroup left outer join " + crLf
				+ "(select CustomerGroupId, freightruleid, " + crLf
				+ "	case count(frszcgPid) when 0 then 0 else 1 end as AppliesToThisFreightRule " + crLf
				+ "from tblfrszcgP where freightruleid = " + FreightRuleId.ToString() + crLf
				+ "group by CustomerGroupId) tblFrSzCgP" + crLf
				+ "on tblcustomerGroup.CustomerGroupId = tblFrSzCgP.CustomerGroupId ");


			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);


			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets all ShippingZoneIds and whether they are applicable to this Freight Rule
		/// </summary>
		/// <param name="FreightRuleId">Id of Freight Rule to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetShippingZonesForFreightRuleId(int FreightRuleId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = ShippingZoneQ;

			queries[0].AddSelectColumn("AppliesToThisFreightRule");

			queries[0].AddFromTable("tblShippingZone left outer join " + crLf
				+ "(select ShippingZoneId, freightruleid, " + crLf
				+ "	case count(frszcgPid) when 0 then 0 else 1 end as AppliesToThisFreightRule " + crLf
				+ "from tblfrszcgP where freightruleid = " + FreightRuleId.ToString() + crLf
				+ "group by ShippingZoneId) tblFrSzCgP" + crLf
				+ "on tblShippingZone.ShippingZoneId = tblFrSzCgP.ShippingZoneId ");


			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);


			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets all ProductIds and whether they are applicable to this Freight Rule
		/// </summary>
		/// <param name="FreightRuleId">Id of Freight Rule to retrieve Freight Rule-Shipping Zone-Customer Group-Prices for</param>
		/// <returns>Number of resulting records</returns>
		public int GetProductsForFreightRuleId(int FreightRuleId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = ProductQ;

			queries[0].AddSelectColumn("AppliesToThisFreightRule");
			queries[0].AddSelectColumn("FRCost");

			queries[0].AddFromTable("(select * from tblProduct where Archive = 0) tblProduct left outer join " + crLf
				+ "(select ProductId, freightruleid, " + crLf
				+ "	case count(frszcgPid) when 0 then 0 else 1 end as AppliesToThisFreightRule, " + crLf
				+ "	max(Cost) as FRCost" + crLf
				+ "from tblfrszcgP where freightruleid = " + FreightRuleId.ToString() + crLf
				+ "group by ProductId) tblFrSzCgP" + crLf
				+ "on tblProduct.ProductId = tblFrSzCgP.ProductId ");

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);


			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets a Distinct List of FreightRule Names only. This is intented for use e.g. as an
		/// auto-completing drop down.
		/// </summary>
		/// <param name="FreightRuleName">Name of FreightRule</param>
		/// <param name="MatchCriteria">Criteria to match FreightRule Name, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctFreightRuleName(string FreightRuleName, int MatchCriteria)
		{
			string fieldRequired = "FreightRuleName";
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct "+ thisTable + "." + fieldRequired);

			string condition = "(Select * from " + thisTable + crLf;
			
			string match = MatchCondition(FreightRuleName, 
				(matchCriteria) MatchCriteria);

			//Additional Conditions
			condition += "Where " + fieldRequired + match + crLf;
			
			switch (thisArchivedRecordBehaviour)
			{
				case archivedRecordBehaivour.archivedOnly:
					condition += " And " + thisTable + ".Archive = 1" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedOnly:
					condition += " And " + thisTable + ".Archive = 0" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedAndArchived:
				default:
					break;
			}

			thisSqlQuery = QB.BuildSqlStatement(
				queries, 
				condition + ") " + thisTable,
				thisTable
				);

			//Ordering
			thisSqlQuery += "Order By tblFreightRule.FreightRuleName" + crLf;

			return localRecords.GetRecords(thisSqlQuery);

		}

		#endregion

		# region Add/Modify/Validate/Save
		
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="FreightRuleDescription">Description of Freight Rule</param>
		/// <param name="FreightRuleName">FreightRule's Name</param>
		/// <param name="MinTotalWeight">Minimum Total Weight for which this Freight Rule-Shipping Zone-Customer Group-Product Applies</param>
		/// <param name="MaxTotalWeight">Maximum Total Weight for which this Freight Rule-Shipping Zone-Customer Group-Product Applies</param>
		/// <param name="MinTotalValue">Minimum Total Value for which this Freight Rule-Shipping Zone-Customer Group-Product Applies</param>
		/// <param name="MaxTotalValue">Maximum Total Value for which this Freight Rule-Shipping Zone-Customer Group-Product Applies</param>
		/// <param name="FRCost">FRCost this Freight Rule-Shipping Zone-Customer Group-Product Applies</param>
		public void Add(string FreightRuleName,
			string FreightRuleDescription,
			decimal MinTotalWeight,
			decimal MaxTotalWeight,
			decimal MinTotalValue,
			decimal MaxTotalValue,
			decimal FRCost)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();
			
			rowToAdd["FreightRuleDescription"] = FreightRuleDescription;
			rowToAdd["FreightRuleName"] = FreightRuleName;
			rowToAdd["MinTotalWeight"] = MinTotalWeight;
			rowToAdd["MaxTotalWeight"] = MaxTotalWeight;
			rowToAdd["Archive"] = 0;

			if (priceShownIncludesLocalTaxRate)
			{
				rowToAdd["MinTotalValue"] = MinTotalValue / localTaxRate;
				rowToAdd["MaxTotalValue"] = MaxTotalValue / localTaxRate;
				rowToAdd["FRCost"] = FRCost / localTaxRate;
			}
			else
			{
				rowToAdd["MinTotalValue"] = MinTotalValue;
				rowToAdd["MaxTotalValue"] = MaxTotalValue;
				rowToAdd["FRCost"] = FRCost;
			}

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
		/// <param name="FreightRuleId">FreightRuleId (Primary Key of Record)</param>
		/// <param name="FreightRuleDescription">Description of Freight Rule</param>
		/// <param name="FreightRuleName">FreightRule's Name</param>
		/// <param name="MinTotalWeight">Minimum Total Weight for which this Freight Rule-Shipping Zone-Customer Group-Product Applies</param>
		/// <param name="MaxTotalWeight">Maximum Total Weight for which this Freight Rule-Shipping Zone-Customer Group-Product Applies</param>
		/// <param name="MinTotalValue">Minimum Total Value for which this Freight Rule-Shipping Zone-Customer Group-Product Applies</param>
		/// <param name="MaxTotalValue">Maximum Total Value for which this Freight Rule-Shipping Zone-Customer Group-Product Applies</param>
		/// <param name="FRCost">FRCost this Freight Rule-Shipping Zone-Customer Group-Product Applies</param>
		public void Modify(int FreightRuleId, 
			string FreightRuleName,
			string FreightRuleDescription,
			decimal MinTotalWeight,
			decimal MaxTotalWeight,
			decimal MinTotalValue,
			decimal MaxTotalValue,
			decimal FRCost)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["FreightRuleId"] = FreightRuleId;
			rowToAdd["FreightRuleName"] = FreightRuleName;
			rowToAdd["FreightRuleDescription"] = FreightRuleDescription;
			rowToAdd["MinTotalWeight"] = MinTotalWeight;
			rowToAdd["MaxTotalWeight"] = MaxTotalWeight;
			rowToAdd["Archive"] = 0;

			if (priceShownIncludesLocalTaxRate)
			{
				rowToAdd["MinTotalValue"] = MinTotalValue / localTaxRate;
				rowToAdd["MaxTotalValue"] = MaxTotalValue / localTaxRate;
				rowToAdd["FRCost"] = FRCost / localTaxRate;
			}
			else
			{
				rowToAdd["MinTotalValue"] = MinTotalValue;
				rowToAdd["MaxTotalValue"] = MaxTotalValue;
				rowToAdd["FRCost"] = FRCost;
			}

			Validate(rowToAdd, false);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);
			}

		}

		/// <summary>
		/// Add a Customer Group for which this Freight Rule Will Apply
		/// </summary>
		/// <param name="FreightRuleId">Id of Freight Rule</param>
		/// <param name="CustomerGroupId">Id of Customer Group for which 
		/// this Freight Rule Will Apply</param>
		public void AddCustomerGroupId(int FreightRuleId, int CustomerGroupId)
		{
			CustomerGroupType thisItem = new CustomerGroupType();
			thisItem.FreightRuleId = FreightRuleId;
			thisItem.CustomerGroupId = CustomerGroupId;
			CustomerGroups.Add(thisItem);
		}

		/// <summary>
		/// Add a Shipping Zone for which this Freight Rule Will Apply
		/// </summary>
		/// <param name="FreightRuleId">Id of Freight Rule</param>
		/// <param name="ShippingZoneId">Id of Shipping Zonefor which 
		/// this Freight Rule Will Apply</param>
		public void AddShippingZoneId(int FreightRuleId, int ShippingZoneId)
		{
			ShippingZoneType thisItem = new ShippingZoneType();
			thisItem.FreightRuleId = FreightRuleId;
			thisItem.ShippingZoneId = ShippingZoneId;
			ShippingZones.Add(thisItem);
		}

		/// <summary>
		/// Add a Product for which this Freight Rule Will Apply
		/// </summary>
		/// <param name="FreightRuleId">Id of Freight Rule</param>
		/// <param name="ProductId">Id of Product for which 
		/// this Freight Rule Will Apply</param>
		/// <param name="Price">Price to Freight this Product</param>
		public void AddProductId(int FreightRuleId, int ProductId, decimal Price)
		{
			ProductType thisItem = new ProductType();

			if (priceShownIncludesLocalTaxRate)
				Price = Price / localTaxRate;

			thisItem.FreightRuleId = FreightRuleId;
			thisItem.ProductId = ProductId;
			thisItem.Price = Price;
			Products.Add(thisItem);
		}


		/// <summary>
		/// Deletes any old Applications of this freight rule,
		/// and creates new applications using Customer Groups, Shipping Zones 
		/// and Products
		/// specified using the Add___ methods
		/// </summary>
		/// <param name="FreightRuleId">Id of Freight Rule to Apply</param>
		/// <returns>Number of FrSzCgPs created</returns>
		public int ApplyFreightRule(int FreightRuleId)
		{
			GetByFreightRuleId(FreightRuleId);
			
			clsFrSzCgP thisFrSzCgP = new clsFrSzCgP(thisDbType, localRecords.dbConnection);
			int numApplications = thisFrSzCgP.GetByFreightRuleId(FreightRuleId);

			if (numApplications > 0)
				thisFrSzCgP.localRecords.RemoveRecordById(thisFrSzCgP.thisPk, thisFrSzCgP.thisTable);

			string cgQ = "(select CustomerGroupId from tblCustomerGroup where ";
			string szQ = "(select ShippingZoneId from tblShippingZone where ";
			string pQ = "(select ProductId from tblProduct where ";
			string pCaseQ = "case ProductId " + crLf;

			int numCGs = CustomerGroups.Count;
			int numSZs = ShippingZones.Count;
			int numPs = Products.Count;
			CustomerGroupType thisCG;
			ShippingZoneType thisSZ;
			ProductType thisP;


			for(int counter = 0; counter < numCGs - 1; counter++)
			{
				thisCG = (CustomerGroupType) CustomerGroups[counter];
				cgQ += "CustomerGroupId = " + thisCG.CustomerGroupId.ToString() + crLf + " OR ";
			}

			thisCG = (CustomerGroupType) CustomerGroups[numCGs - 1];
			cgQ += "CustomerGroupId = " + thisCG.CustomerGroupId.ToString();
			cgQ += ") tblCustomerGroup" + crLf;

			for(int counter = 0; counter < numSZs - 1; counter++)
			{
				thisSZ = (ShippingZoneType) ShippingZones[counter];
				szQ += "ShippingZoneId = " + thisSZ.ShippingZoneId.ToString() + crLf + " OR ";
			}

			thisSZ = (ShippingZoneType) ShippingZones[numSZs - 1];
			szQ += "ShippingZoneId = " + thisSZ.ShippingZoneId.ToString();
			szQ += ") tblShippingZone" + crLf;

			if (numPs > 0)
			{
				for(int counter = 0; counter < numPs - 1; counter++)
				{
					thisP = (ProductType) Products[counter];
					pQ += "ProductId = " + thisP.ProductId.ToString() + crLf + " OR ";
					pCaseQ += "When " + thisP.ProductId.ToString() + crLf + " Then " + thisP.Price.ToString() + crLf;
				}

				thisP = (ProductType) Products[numPs - 1];
				pQ += "ProductId = " + thisP.ProductId.ToString();
				pQ += ") tblProduct" + crLf;
				pCaseQ += "When " + thisP.ProductId.ToString() + crLf + " Then " + thisP.Price.ToString() + crLf;
				pCaseQ += "Else 0 End as Cost" + crLf;
			}


			clsQueryPart thisQ = new clsQueryPart();
			
			thisQ.AddSelectColumn(FreightRuleId.ToString() + " as FreightRuleId");
			thisQ.AddSelectColumn("tblCustomerGroup.CustomerGroupId");
			thisQ.AddSelectColumn("tblShippingZone.ShippingZoneId");

			thisQ.AddFromTable(cgQ);
			thisQ.AddFromTable(szQ);
			
			
			string insert = "";

			//Now, create new records
			switch(freightChargeBasis)
			{
				case freightChargeType.singleChargePerItem:
					insert = "Insert Into tblFrSzCgP(FreightRuleId, CustomerGroupId, ShippingZoneId, ProductId, Cost)" + crLf;

					thisQ.AddSelectColumn("tblProduct.ProductId");
					thisQ.AddSelectColumn(pCaseQ);

					thisQ.AddFromTable(pQ);

					break;
				case freightChargeType.singleChargePerValueRange:
				case freightChargeType.singleChargePerWeightRange:
					insert = "insert into tblFrSzCgP(FreightRuleId, CustomerGroupId, ShippingZoneId, Cost)" + crLf;
					
					thisQ.AddSelectColumn(my_FRCostExcludingTax(0).ToString() + " as FRCost");
					
					break;
				default:
					break;
			}
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = thisQ;

			thisSqlQuery = insert + QB.BuildSqlStatement(
				queries);

			
			return localRecords.AddBySelect(thisSqlQuery);

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

		# region My_ Values FreightRule

		/// <summary>
		/// FreightRuleId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>FreightRuleId for this Row</returns>
		public int my_FreightRuleId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "FreightRuleId"));
		}


		/// <summary>
		/// Freight RuleD escription
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Freight Rule Description for this Row</returns>
		public string my_FreightRuleDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FreightRuleDescription");
		}

		/// <summary>
		/// Freight Rule Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name of FreightRule for this Row</returns>
		public string my_FreightRuleName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FreightRuleName");
		}


		/// <summary>
		/// <see cref="clsFreightRule.my_MinTotalWeight">Minimum Total Weight</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MinTotalWeight">Minimum Total Weight</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_MinTotalWeight(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "MinTotalWeight"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_MaxTotalWeight">Maximum Total Weight</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MaxTotalWeight">Maximum Total Weight</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_MaxTotalWeight(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "MaxTotalWeight"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_MinTotalValue">Minimum Total Value</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MinTotalValue">Minimum Total Value</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_MinTotalValue(int rowNum)
		{
			//If the price shown includes the local tax rate, 
			//then the price we have been given also includes the local tax rate.
			//But we always store the price without tax...
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "MinTotalValue")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "MinTotalValue"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_MinTotalValue">Minimum Total Value (Excluding Tax)</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MinTotalValue">Minimum Total Value (Excluding Tax)</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_MinTotalValueExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "MinTotalValue"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_MaxTotalValue">Maximum Total Value</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MaxTotalValue">Maximum Total Value</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_MaxTotalValue(int rowNum)
		{
			//If the price shown includes the local tax rate, 
			//then the price we have been given also includes the local tax rate.
			//But we always store the price without tax...
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "MaxTotalValue")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "MaxTotalValue"));	
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_MaxTotalValue">Maximum Total Value (Excluding Tax)</see> that this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// Associated with this Freight Rule-Shipping Zone-Customer Group-Product Applies to</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_MaxTotalValue">Maximum Total Value (Excluding Tax)</see> 
		/// that this <see cref="clsFreightRule">Freight Rule</see> 
		/// for this Freight Rule-Shipping Zone-Customer Group-Product Applies to</returns>
		public decimal my_MaxTotalValueExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "MaxTotalValue"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_FRCost">Cost (For Display Purposes)</see> for this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_FRCost">Cost (For Display Purposes)</see> 
		/// for this <see cref="clsFreightRule">Freight Rule</see> 
		/// </returns>
		public decimal my_FRCost(int rowNum)
		{
			//If the price shown includes the local tax rate, 
			//then the price we have been given also includes the local tax rate.
			//But we always store the price without tax...
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FRCost")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FRCost"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_FRCostExcludingTax">Cost (Excluding Tax)</see> for this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_FRCostExcludingTax">Cost (Excluding Tax)</see> 
		/// for this <see cref="clsFreightRule">Freight Rule</see> 
		/// </returns>
		public decimal my_FRCostExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FRCost"));
		}

		/// <summary>
		/// <see cref="clsFreightRule.my_FRCostIncludingTax">Cost (Including Tax)</see> for this
		/// <see cref="clsFreightRule">Freight Rule</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsFreightRule.my_FRCostIncludingTax">Cost (Including Tax)</see> 
		/// for this <see cref="clsFreightRule">Freight Rule</see> 
		/// </returns>
		public decimal my_FRCostIncludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "FRCost")) * localTaxRate;
		}

		#endregion

		#region My_ Values CustomerGroup

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupName">Name</see> of 
		/// <see cref="clsCustomerGroup">Customer Group</see> for this Customer</summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupName">Name</see> 
		/// of Associated <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// for this Freight Rule</returns>
		public string my_CustomerGroupName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerGroupName");
		}

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupDescription">Description</see> of 
		/// <see cref="clsCustomerGroup">Customer Group</see> for this Customer</summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupDescription">Description</see> 
		/// of Associated <see cref="clsCustomerGroup">Customer Group</see> 
		/// for this Freight Rule</returns>
		public string my_CustomerGroupDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerGroupDescription");
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

	}
}
