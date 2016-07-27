using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsCustomerGroup deals with everything to do with data about CustomerGroups.
	/// </summary>
	

	[GuidAttribute("42D7C869-CD6A-4531-BC7C-8DCA8C1737B4")]
	public class clsCustomerGroup : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCustomerGroup
		/// </summary>
		public clsCustomerGroup() : base("CustomerGroup")
		{
		}

		/// <summary>
		/// Constructor for clsCustomerGroup; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsCustomerGroup(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("CustomerGroup")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Customer Summary Information
		/// </summary>
		public clsQueryPart CustomerSummaryQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = CustomerGroupQueryPart();
			ChangeDataQ = ChangeDataQueryPart();

			CustomerSummaryQ.AddSelectColumn("tblCustomer.TotalCustomers");
			CustomerSummaryQ.AddSelectColumn("tblCustomer.TotalUnarchivedCustomers");

			CustomerSummaryQ.AddFromTable("(Select tblCustomerGroup.CustomerGroupId," + crLf
				+ "sum(case tblCustomer.archive when 0 then 1 else 0 end) as TotalUnarchivedCustomers," + crLf
				+ "sum(case tblCustomer.archive when 0 then 1 when -1 then 1 else 0 end)as TotalCustomers" + crLf
				+ "from tblCustomerGroup left outer join tblCustomer " + crLf
				+ "on tblCustomerGroup.CustomerGroupId = tblCustomer.CustomerGroupId " + crLf
				+ "Group by tblCustomerGroup.CustomerGroupId) tblCustomer");

			CustomerSummaryQ.AddJoin("tblCustomerGroup.CustomerGroupId = tblCustomer.CustomerGroupId");

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[3];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = CustomerSummaryQ;
			baseQueries[2] = ChangeDataQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By tblCustomerGroup.CustomerGroupName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsCustomerGroup
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("CustomerGroupName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerGroupDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ChangeDataId", System.Type.GetType("System.Int64"));
			newDataToAdd.Columns.Add("Archive", System.Type.GetType("System.Int64"));
			
			dataToBeModified = new DataTable(thisTable);
			dataToBeModified.Columns.Add(thisPk, System.Type.GetType("System.Int64"));

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


		/// <summary>
		/// Local Representation of the class <see cref="clsCustomerGroup">clsCustomerGroup</see>
		/// </summary>
		public clsCustomerGroup thisCustomerGroup;
		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all CustomerGroups
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
					thisTable);
			
			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}



		/// <summary>
		/// Gets an Customer Group by CustomerGroupId
		/// </summary>
		/// <param name="CustomerGroupId">Id of Customer Group to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerGroupId(int CustomerGroupId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ CustomerGroupId.ToString();

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
		/// Returns CustomerGroups with the specified CustomerGroupName
		/// </summary>
		/// <param name="CustomerGroupName">Name of CustomerGroup</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of CustomerGroups with the specified CustomerGroupName</returns>
		public int GetByCustomerGroupName(string CustomerGroupName, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;	

			string match = MatchCondition(CustomerGroupName, 
				(matchCriteria) MatchCriteria);

			//Additional Condition
			condition += "Where tblCustomerGroup.CustomerGroupName " + match + crLf;

			condition += ArchiveConditionIfNecessary(true);

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
		/// <param name="CustomerGroupName">CustomerGroup's Name</param>
		/// <param name="CustomerGroupDescription">Description of this Customer Group</param>
		/// <param name="CustomerGroupIdToCopyPricingFrom">CustomerGroupId to copy pricing from</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Add(string CustomerGroupName, 
			string CustomerGroupDescription,
			int CustomerGroupIdToCopyPricingFrom,
			int CurrentUser)
		{
			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			AddGeneral(CustomerGroupName,
				CustomerGroupDescription,
				thisChangeData.LastIdAdded(),
				0);

			Save();

			int thisCustomerGroupId = LastIdAdded();

			#region Sort out the pricing
			clsProductCustomerGroupPrice thisProductCustomerGroupPrice = new clsProductCustomerGroupPrice(thisDbType, localRecords.dbConnection);

			int numProductCustomerGroupPrices = thisProductCustomerGroupPrice.GetByCustomerGroupId(CustomerGroupIdToCopyPricingFrom);

			for(int counter = 0; counter < numProductCustomerGroupPrices; counter ++)
			{
				thisProductCustomerGroupPrice.Add(thisProductCustomerGroupPrice.my_ProductId(counter),
					thisCustomerGroupId,
					thisProductCustomerGroupPrice.my_Price(counter));
			}

			thisProductCustomerGroupPrice.Save();

			#endregion
		}
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal CustomerGroup table stack; the SaveCustomerGroups method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CustomerGroupName">CustomerGroupName</param>
		/// <param name="CustomerGroupDescription">CustomerGroupDescription</param>
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		private void AddGeneral(string CustomerGroupName,
			string CustomerGroupDescription,
			int ChangeDataId,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
		    
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["CustomerGroupName"] = CustomerGroupName;
			rowToAdd["CustomerGroupDescription"] = CustomerGroupDescription;

			rowToAdd["ChangeDataId"] = ChangeDataId;
			rowToAdd["Archive"] = Archive;
			
			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				newDataToAdd.Rows.Add(rowToAdd);
			}

		}
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal CustomerGroup table stack; the SaveCustomerGroups method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CurrentCustomerGroup">CurrentCustomerGroup</param>
		/// <param name="thisPkId">thisPkId</param>
		public override int AddArchive(int CurrentCustomerGroup, int thisPkId)
		{
			thisCustomerGroup = new clsCustomerGroup(thisDbType, localRecords.dbConnection);

			thisCustomerGroup.GetByCustomerGroupId(thisPkId);

			AddGeneral(thisCustomerGroup.my_CustomerGroupName(0),
				thisCustomerGroup.my_CustomerGroupDescription(0),
				thisCustomerGroup.my_ChangeDataId(0), 
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisCustomerGroup.my_ChangeData_CreatedByUserId(0),
				thisCustomerGroup.my_ChangeData_CreatedByFirstNameLastName(0),
				thisCustomerGroup.my_ChangeData_DateCreated(0),
				CurrentCustomerGroup,"",dtaNow.ToString());
							
			thisChangeData.Save();

			return thisChangeData.LastIdAdded();
		}
		/// <summary>
		/// Validates the supplied data for Errors and Warnings.
		/// If any Errors are found, ErrorFound will return true, and these Errors 
		/// are available through the ErrorMessage and ErrorFieldName methods
		/// If any Warnings are found, WarningFound will return true, and these Warnings 
		/// are available through the WarningMessage and WarningFieldName methods
		/// If there are no Errors, this method adds a new entry to the 
		/// internal CustomerGroup table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CustomerGroupId">CustomerGroupId (Primary Key of Record)</param>
		/// <param name="CustomerGroupName">CustomerGroupName</param>
		/// <param name="CustomerGroupDescription">CustomerGroupDescription</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Modify(int CustomerGroupId, 
			string CustomerGroupName,
			string CustomerGroupDescription,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, CustomerGroupId);
			rowToAdd["CustomerGroupId"] = CustomerGroupId;

			rowToAdd["CustomerGroupName"] = CustomerGroupName;
			rowToAdd["CustomerGroupDescription"] = CustomerGroupDescription;
			
			rowToAdd["Archive"] = 0;

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

		#region Save

//		/// <summary>
//		/// Saves changes to data added by the 'Add' and 'Modify' functions
//		/// The Virtual Member is overridden so that changes to 
//		/// Product Customer Group Pricing can be performed
//		/// </summary>
//		/// <returns>true</returns>
//		public override int Save()
//		{
//			//Conventional Part of the Function:
//			//Save the records
//
//			int numRecords = base.Save();
//
//			//Get the lastId for the Product Customer Group Pricing
//			int newCustomerGroupId = localRecords.LastIdAdded;
//
//			if (newCustomerGroupId > 0)
//			{
//				string sqlString = "Insert into tblProductCustomerGroupPrice(ProductId, CustomerGroupId, Price, MinimumQuantityForPrice)" + crLf
//					+ " Select ProductId, " + newCustomerGroupId.ToString() + " as CustomerGroupId, Price, MinimumQuantityForPrice " + crLf
//					+ " From tblProductCustomerGroupPrice " + crLf 
//					+ " Where CustomerGroupId = " + CustomerGroupIdToCopyFrom.ToString() + crLf
//					+ " Order by ProductId "
//					;
//
//				localRecords.GetRecords(sqlString);
//
////				clsProductCustomerGroupPrice thisPcgp = new clsProductCustomerGroupPrice(thisDbType, localRecords.dbConnection);
////				int numProducts = thisPcgp.GetByCustomerGroupId(CustomerGroupIdToCopyFrom);
////				for (int counter = 0; counter < numProducts; counter++)
////					thisPcgp.Set(thisPcgp.my_ProductId(counter),newCustomerGroupId, thisPcgp.my_Price(counter));
////
////				thisPcgp.Save();
//			}
//
//			//Reinitialise the class
//			Initialise();
//			return numRecords;
//		}
		
		#endregion

		# region My_ Values CustomerGroup

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupId">Id</see> of
		/// <see cref="clsCustomerGroup">CustomerGroup</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupId">Id</see> of 
		/// <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// </returns>
		public int my_CustomerGroupId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerGroupId"));
		}

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupName">Name</see> of
		/// <see cref="clsCustomerGroup">CustomerGroup</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupName">Name</see> of 
		/// <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// </returns>
		public string my_CustomerGroupName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerGroupName");
		}

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupDescription">Description</see> of
		/// <see cref="clsCustomerGroup">CustomerGroup</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupDescription">Description</see> of 
		/// <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// </returns>
		public string my_CustomerGroupDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerGroupDescription");
		}

		#endregion

		#region My_ Values CustomerSummary

		/// <summary>Total number of 
		/// <see cref="clsCustomer">Customers</see> 
		/// Associated with this Customer Group</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total number of 
		/// <see cref="clsCustomer">Customers</see>
		/// Associated with this Customer Group</returns>
		public int my_TotalCustomers(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalCustomers"));
		}

		/// <summary>Total number of unarchived
		/// <see cref="clsCustomer">Customers</see> of 
		/// Associated with this Customer Group</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total number of unarchived
		/// <see cref="clsCustomer">Customers</see> of 
		/// Associated with this Customer Group</returns>
		public int my_TotalUnarchivedCustomers(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "TotalUnarchivedCustomers"));
		}

		# endregion
	}
}
