using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsSPPremiseRole deals with everything to do with data about SPPremiseRoles.
	/// </summary>
	
	[GuidAttribute("F21B42A3-EB79-404a-9F4E-54805A03AA31")]
	public class clsSPPremiseRole : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsSPPremiseRole
		/// </summary>
		public clsSPPremiseRole() : base("SPPremiseRole")
		{
		}

		/// <summary>
		/// Constructor for clsSPPremiseRole; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsSPPremiseRole(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("SPPremiseRole")
		{
			Connect(typeOfDb, odbcConnection);
		}


		/// <summary>
		/// Part of the Query that Pertains to Customer Information
		/// </summary>
		public clsQueryPart CustomerQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Premise Information
		/// </summary>
		public clsQueryPart PremiseQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Person Information
		/// </summary>
		public clsQueryPart ServiceProviderQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			ChangeDataQ = ChangeDataQueryPart();
			PremiseQ = PremiseQueryPart();
			ServiceProviderQ = ServiceProviderQueryPart();
			ChangeDataQ = ChangeDataQueryPart();

			CustomerQ = CustomerQueryPart();
			CustomerQ.Joins.Clear();
			CustomerQ.Joins.Add("tblPremise.CustomerId = tblCustomer.CustomerId");

			MainQ = SPPremiseRoleQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[5];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = ServiceProviderQ;
			baseQueries[2] = PremiseQ;
			baseQueries[3] = CustomerQ;
			baseQueries[4] = ChangeDataQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By ServiceProvider_FullName" + crLf;

		}

		
		/// <summary>
		/// Initialise (or reinitialise) everything for clsSPPremiseRole
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("SPPremiseRoleType", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PremiseId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ServiceProviderId", System.Type.GetType("System.Int32"));
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
		/// Connect to Foreign Key classes within this clasee
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
		/// Local Representation of the class <see cref="clsSPPremiseRole">clsSPPremiseRole</see>
		/// </summary>
		public clsSPPremiseRole thisSPPremiseRole;

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all SPPremiseRoles
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = ArchiveConditionIfNecessary(false);

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
		/// Gets an SPPremiseRole by SPPremiseRoleId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetBySPPremiseRoleId(int SPPremiseRoleId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ SPPremiseRoleId.ToString() + crLf;

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
		/// Gets an SPPremiseRole by PremiseId
		/// </summary>
		/// <param name="PremiseId">
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetByPremiseId(int PremiseId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".PremiseId = "
				+ PremiseId.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

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
		/// Gets an SPPremiseRole by ServiceProviderId
		/// </summary>
		/// <param name="ServiceProviderId">ServiceProviderId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByServiceProviderId(int ServiceProviderId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".ServiceProviderId = "
				+ ServiceProviderId.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

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
		/// Gets an SPPremiseRole by ServiceProviderId and PremiseId
		/// </summary>
		/// <param name="ServiceProviderId">ServiceProviderId</param>
		/// <param name="PremiseId">PremiseId</param>
		/// <returns>Number of resulting records</returns>
		public int GetByServiceProviderIdPremiseId(int ServiceProviderId, int PremiseId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".ServiceProviderId = "
				+ ServiceProviderId.ToString() + crLf
				+ " And " + thisTable + ".PremiseId = "
				+ PremiseId.ToString() + crLf
				;

			condition += ArchiveConditionIfNecessary(true);

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
		/// Gets PersonPremiseRoles for a Customer
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
		
			string condition1 = "(Select * from " + thisTable + crLf;
			condition1 += "Where " + crLf;
			condition1 += ArchiveConditionIfNecessary(false) + crLf;
			condition1 += ") " + thisTable;
 

			string condition2 = "(Select * From tblPremise" + crLf;	

			//Additional Condition
			condition2 += "Where tblPremise.CustomerId = " 
				+ CustomerId.ToString() + crLf;

			condition2 += ArchiveConditionIfNecessary(true, "tblPremise");

			condition2 += ") tblPremise";


			clsQueryBuilder.ConditionWithTable[] thisConditions = new Resources.clsQueryBuilder.ConditionWithTable[2];
			thisConditions[0].condition = condition1;
			thisConditions[0].table = thisTable;
			thisConditions[1].condition = condition2;
			thisConditions[1].table = "tblPremise";

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns,
				thisConditions
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets PersonPremiseRoles for a Customer
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctServiceProviderByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];

			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct " + thisTable + ".ServiceProviderId");
			queries[1] = ServiceProviderQueryPart(true);
			queries[2] = PremiseQueryPart();
			queries[2].SelectColumns.Clear();

			string condition1 = "(Select * from " + thisTable + crLf;
			condition1 += "Where " + crLf;
			condition1 += ArchiveConditionIfNecessary(false) + crLf;
			condition1 += ") " + thisTable;


			string condition2 = "(Select * From tblPremise" + crLf;	

			//Additional Condition
			condition2 += "Where tblPremise.CustomerId = " 
				+ CustomerId.ToString() + crLf;

			condition2 += ArchiveConditionIfNecessary(true, "tblPremise");

			condition2 += ") tblPremise";


			clsQueryBuilder.ConditionWithTable[] thisConditions = new Resources.clsQueryBuilder.ConditionWithTable[2];
			thisConditions[0].condition = condition1;
			thisConditions[0].table = thisTable;
			thisConditions[1].condition = condition2;
			thisConditions[1].table = "tblPremise";

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns,
				thisConditions
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Distinct Service Providers for a Premise
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctServiceProviderByPremiseId(int PremiseId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];

			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct " + thisTable + ".ServiceProviderId");
			queries[1] = ServiceProviderQueryPart(true);
			queries[2] = PremiseQueryPart();
			queries[2].SelectColumns.Clear();

			string condition1 = "(Select * from " + thisTable + crLf;
			condition1 += "Where " + crLf;
			condition1 += ArchiveConditionIfNecessary(false) + crLf;
			condition1 += ") " + thisTable;


			string condition2 = "(Select * From tblPremise" + crLf;	

			//Additional Condition
			condition2 += "Where tblPremise.PremiseId = " 
				+ PremiseId.ToString() + crLf;

			condition2 += ArchiveConditionIfNecessary(true, "tblPremise");

			condition2 += ") tblPremise";


			clsQueryBuilder.ConditionWithTable[] thisConditions = new Resources.clsQueryBuilder.ConditionWithTable[2];
			thisConditions[0].condition = condition1;
			thisConditions[0].table = thisTable;
			thisConditions[1].condition = condition2;
			thisConditions[1].table = "tblPremise";

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns,
				thisConditions
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets an SPPremiseRole by 
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see>
		/// and 
		/// <see cref="clsKeyBase.sPPremiseRoleType">
		/// SPPremiseRoleType</see>
		/// </summary>
		/// <param name="PremiseId">
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see></param>
		/// <param name="RoleType">
		/// <see cref="clsKeyBase.sPPremiseRoleType">
		/// SPPremiseRoleType</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetByPremiseIdSPPremiseRoleType(int PremiseId, int RoleType)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".PremiseId = "
				+ PremiseId.ToString() + crLf
				+ " And " + thisTable + ".SPPremiseRoleType = "
				+ RoleType.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

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
		/// Checks for the existance SPPremiseRoles by 
		/// <see cref="clsServiceProvider.my_ServiceProviderId">ServiceProviderId</see>
		/// at 
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see>
		/// of type 
		/// <see cref="clsKeyBase.sPPremiseRoleType">
		/// SPPremiseRoleType</see>
		/// </summary>
		/// <param name="ServiceProviderId">
		/// <see cref="clsServiceProvider.my_ServiceProviderId">ServiceProviderId</see></param>
		/// <param name="PremiseId"><see cref="clsPremise.my_PremiseId">PremiseId</see></param>
		/// <param name="RoleType">
		/// <see cref="clsKeyBase.sPPremiseRoleType">
		/// SPPremiseRoleType</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetBySPIdPremiseIdSPPRType(int ServiceProviderId, int PremiseId, int RoleType)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".ServiceProviderId = "
				+ ServiceProviderId.ToString() + crLf
				+ " And " + thisTable + ".PremiseId = "
				+ PremiseId.ToString() + crLf
				+ " And " + thisTable + ".SPPremiseRoleType = "
				+ RoleType.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

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

		#endregion

		# region Add/Modify/Validate/Save


		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the SaveCustomers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="SPPremiseRoleType">Type of SP Premise Role</param>
		/// <param name="PremiseId">Premise Associated with this SP Premise Role</param>
		/// <param name="ServiceProviderId">SP Associated with this SP Premise Role</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Add(int PremiseId, 
			int ServiceProviderId,
			int SPPremiseRoleType,
			int CurrentUser)
		{

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			AddGeneral(PremiseId,
				ServiceProviderId,
				SPPremiseRoleType,
				thisChangeData.LastIdAdded(),
				0);
			
		}
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal SPPremiseRole table stack; the SaveSPPremiseRoles method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PremiseId">PremiseId</param>
		/// <param name="ServiceProviderId">ServiceProviderId</param>
		/// <param name="SPPremiseRoleType">SPPremiseRoleType</param>
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		private void AddGeneral(int PremiseId,
			int ServiceProviderId,
			int SPPremiseRoleType,
			int ChangeDataId,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["SPPremiseRoleType"] = SPPremiseRoleType;
			rowToAdd["PremiseId"] = PremiseId;
			rowToAdd["ServiceProviderId"] = ServiceProviderId;
			
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
		/// internal User table stack; the SaveUsers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CurrentUser">CurrentUser</param>
		/// <param name="thisPkId">thisPkId</param>
		public override int AddArchive(int CurrentUser, int thisPkId)
		{
			thisSPPremiseRole = new clsSPPremiseRole(thisDbType, localRecords.dbConnection);
			thisSPPremiseRole.GetBySPPremiseRoleId(thisPkId);

			AddGeneral(thisSPPremiseRole.my_PremiseId(0),
				thisSPPremiseRole.my_ServiceProviderId(0),
				thisSPPremiseRole.my_SPPremiseRoleType(0),
				thisSPPremiseRole.my_ChangeDataId(0), 
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisSPPremiseRole.my_ChangeData_CreatedByUserId(0),
				thisSPPremiseRole.my_ChangeData_CreatedByFirstNameLastName(0),
				thisSPPremiseRole.my_ChangeData_DateCreated(0),
				CurrentUser,"",dtaNow.ToString());
			
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
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="SPPremiseRoleId">SPPremiseRoleId (Primary Key of Record)</param>
		/// <param name="ServiceProviderId">SP Associated with this SP Premise Role</param>
		/// <param name="SPPremiseRoleType">Type of SP Premise Role</param>
		/// <param name="PremiseId">Premise Associated with this SP Premise Role</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Modify(int SPPremiseRoleId, 
			int PremiseId, 
			int ServiceProviderId,
			int SPPremiseRoleType,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, SPPremiseRoleId);

			rowToAdd["SPPremiseRoleId"] = SPPremiseRoleId;
			rowToAdd["SPPremiseRoleType"] = SPPremiseRoleType;
			rowToAdd["PremiseId"] = PremiseId;
			rowToAdd["ServiceProviderId"] = ServiceProviderId;
			rowToAdd["Archive"] = 0;

			Validate(rowToAdd, false);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);
			}

		}

		#endregion

		#region Other Sets





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
		/// <param name="PremiseId">Premise Associated with this Service Provider Premise Role</param>
		/// <param name="ServiceProviderId">Service Provider Associated with this ServiceProviderPremiseRole</param>
		/// <param name="SPPremiseRoleType">Type of Service Provider Premise Role</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Set(int PremiseId, 
			int ServiceProviderId,
			int SPPremiseRoleType,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Find this SPPremiseRoleId
			clsSPPremiseRole thisSPPR = new clsSPPremiseRole(thisDbType, localRecords.dbConnection);

			int numRecords = thisSPPR.GetByPremiseIdSPPremiseRoleType(PremiseId, SPPremiseRoleType);
			bool change = true;

			switch((sPPremiseRoleType) SPPremiseRoleType)
			{
					//There can be many Keyholders;
					//If this sP is already a Keyholder for this Premise, do nothing - otherwise add them at the end of the 
					//Keyholder Priority List
				case sPPremiseRoleType.other:
					numRecords = thisSPPR.GetBySPIdPremiseIdSPPRType(ServiceProviderId, PremiseId, SPPremiseRoleType);

				switch (numRecords)
				{
					case 0:
						thisSPPR.Add(PremiseId,
							ServiceProviderId,
							SPPremiseRoleType,
							CurrentUser);
						break;
					case 1:
					default:
						break;
				}
					break;
				case sPPremiseRoleType.alarmMonitor:
				case sPPremiseRoleType.alarmResponse:
				case sPPremiseRoleType.patrol:
				default:
					
					for(int counter = 0; counter < numRecords; counter++)
					{
						if (thisSPPR.my_ServiceProviderId(counter) == ServiceProviderId)
							change = false;
						else
							thisSPPR.Remove(thisSPPR.my_SPPremiseRoleId(counter), CurrentUser);
					}

					if (change)
						thisSPPR.Add(PremiseId,
							ServiceProviderId,
							SPPremiseRoleType,
							CurrentUser);

					break;
			}
			thisSPPR.Save();
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
		/// <param name="PremiseId">Premise Associated with this SPPremiseRole</param>
		/// <param name="ServiceProviderId">Service Provider Associated with this SPPremiseRole</param>
		/// <param name="SPPremiseRoleType">Type of SPPremiseRole</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void RemoveRole(int PremiseId, 
			int ServiceProviderId,
			int SPPremiseRoleType,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Find this SPPremiseRoleId
			clsSPPremiseRole thisSPR = new clsSPPremiseRole(thisDbType, localRecords.dbConnection);

			int numRecords = thisSPR.GetByPremiseIdSPPremiseRoleType(PremiseId, SPPremiseRoleType);

			for(int counter = 0; counter < numRecords; counter++)
				if (thisSPR.my_ServiceProviderId(counter) == ServiceProviderId)
					thisSPR.Remove(thisSPR.my_SPPremiseRoleId(counter), CurrentUser);

			thisSPR.Save();

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

		# region My_ Values SPPremiseRoleType

		/// <summary>
		/// Service Provider Premise Role Id
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>SPPremiseRoleId for this Row</returns>
		public int my_SPPremiseRoleId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "SPPremiseRoleId"));
		}

		/// <summary>
		/// Service Provider Premise Role Type, from the enumeration
		/// <see cref="clsKeyBase.sPPremiseRoleType">SPPremiseRoleType</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Type of Service Provider Premise Role for this Row</returns>
		public int my_SPPremiseRoleType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "SPPremiseRoleType"));
		}

		/// <summary>
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see> of 
		/// <see cref="clsPremise">Premise</see>
		/// Associated with this Service Provider Premise Role</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_PremiseId">Id</see> 
		/// of <see cref="clsPremise">Premise</see> 
		/// for this Service Provider Premise Role</returns>	
		public int my_PremiseId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PremiseId"));
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_ServiceProviderId">ServiceProviderId</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// Associated with this Service Provider Premise Role</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_ServiceProviderId">Id</see> 
		/// of <see cref="clsServiceProvider">ServiceProvider</see> 
		/// for this Service Provider Premise Role</returns>	
		public int my_ServiceProviderId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ServiceProviderId"));
		}

		#endregion

		# region My_ Values Premise

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">Id</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public int my_Premise_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_CustomerId"));
		}

		/// <summary>
		/// <see cref="clsItem.my_ItemId">Id</see> of
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_ItemId">Id</see> of 
		/// <see cref="clsItem">Item</see> 
		/// </returns>
		public int my_Premise_ItemId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_ItemId"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_ProductId">Id</see> of
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductId">Id</see> of 
		/// <see cref="clsProduct">Product</see> 
		/// </returns>
		public int my_Premise_ProductId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_ProductId"));
		}

		
		/// <summary>
		/// <see cref="clsCompanyType.my_CompanyTypeId">Id</see> of
		/// <see cref="clsCompanyType">CompanyType</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCompanyType.my_CompanyTypeId">Id</see> of 
		/// <see cref="clsCompanyType">CompanyType</see> 
		/// </returns>
		public int my_Premise_CompanyTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_CompanyTypeId"));
		}		

		/// <summary>
		/// <see cref="clsPremise.my_PremiseNumber">Number</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_PremiseNumber">Number</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_PremiseNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_PremiseNumber");
		}

		/// <summary>
		/// <see cref="clsPremise.my_Url">Url</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_Url">Url</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_Url(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_Url");
		}

		/// <summary>
		/// <see cref="clsPremise.my_QuickPhysicalAddress">Quick Physical Address</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_QuickPhysicalAddress">Quick Physical Address</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>	
		public string my_Premise_QuickPhysicalAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_QuickPhysicalAddress");
		}

		/// <summary>
		/// <see cref="clsPremise.my_CompanyAtPremiseName">Name of Company</see> at
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_CompanyAtPremiseName">Name of Company</see> at 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_CompanyAtPremiseName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_CompanyAtPremiseName");
		}


		/// <summary>
		/// <see cref="clsPremise.my_AlarmDetails">Details of Alarm</see> at
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_AlarmDetails">Details of Alarm</see> at 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_AlarmDetails(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_AlarmDetails");
		}

		/// <summary>
		/// <see cref="clsPremise.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_KdlComments");
		}

		/// <summary>
		/// <see cref="clsPremise.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_CustomerComments");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DateSubscriptionExpires">Date Next Subscription is Due</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateSubscriptionExpires">Date Next Subscription is Due</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_DateSubscriptionExpires(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_DateSubscriptionExpires");
		}
		
		
		/// <summary>
		/// <see cref="clsPremise.my_DateNextSubscriptionDueToBeInvoiced">Date Next Subscription is due to be invoiced</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateNextSubscriptionDueToBeInvoiced">Date Next Subscription is due to be invoiced</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_DateNextSubscriptionDueToBeInvoiced(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_DateNextSubscriptionDueToBeInvoiced");
		}


		/// <summary>
		/// <see cref="clsPremise.my_DateLastDetailsUpdate">Date Last Detail Update was sent</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateLastDetailsUpdate">Date Last Detail Update was sent</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_DateLastDetailsUpdate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_DateLastDetailsUpdate");
		}

		/// <summary>
		/// <see cref="clsPremise.my_StickerRequired">Whether a Sticker is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_StickerRequired">Whether a Sticker is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_StickerRequired(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_StickerRequired");
		}

		/// <summary>
		/// <see cref="clsPremise.my_InvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_InvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_InvoiceRequired(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_InvoiceRequired");
		}

		/// <summary>
		/// <see cref="clsPremise.my_CopyOfInvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_CopyOfInvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_CopyOfInvoiceRequired(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_CopyOfInvoiceRequired");
		}

		/// <summary>
		/// <see cref="clsPremise.my_StatementRequired">Whether a Statement is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_StatementRequired">Whether a Statement is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_StatementRequired(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_StatementRequired");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DetailsUpdateRequired">Whether a DetailsUpdate is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DetailsUpdateRequired">Whether a DetailsUpdate is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_DetailsUpdateRequired(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_DetailsUpdateRequired");
		}


		#endregion
		
		# region My_ Values ServiceProvider


		/// <summary>
		/// <see cref="clsServiceProvider.my_FullName">Full Name</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_FullName">Full Name</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_FullName");
		}


		/// <summary>
		/// <see cref="clsServiceProvider.my_Title">Title</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_Title">Title</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_Title");
		}

				
		/// <summary>
		/// <see cref="clsServiceProvider.my_FirstName">First Name</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_FirstName">First Name</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_FirstName");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_LastName">Last Name</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_LastName">Last Name</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_LastName");
		}
		
		/// <summary>
		/// <see cref="clsServiceProvider.my_CompanyName">Company Name</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_CompanyName">Company Name</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_CompanyName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_CompanyName");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_KdlComments");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_CustomerComments");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">CustomerId</see> of 
		/// <see cref="clsCustomer">Customer</see>
		/// Associated with this ServiceProvider</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> 
		/// of <see cref="clsCustomer">Customer</see> 
		/// for this ServiceProvider</returns>	
		public int my_ServiceProvider_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ServiceProvider_CustomerId"));
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_QuickDaytimePhone">Quick Daytime Phone</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_QuickDaytimePhone">Quick Daytime Phone</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_QuickDaytimePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_QuickDaytimePhone");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_QuickDaytimeFax">Quick Daytime Fax</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_QuickDaytimeFax">Quick Daytime Fax</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_QuickDaytimeFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_QuickDaytimeFax");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_QuickAfterHoursPhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_QuickAfterHoursPhone");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_QuickAfterHoursFax">Quick After Hours Fax</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_QuickAfterHoursFax">Quick After Hours Fax</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_QuickAfterHoursFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_QuickAfterHoursFax");
		}

		/// <summary>
		/// <see cref="clsServiceProvider.my_QuickMobilePhone">Quick Mobile Phone</see> of
		/// <see cref="clsServiceProvider">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsServiceProvider.my_QuickMobilePhone">Quick Mobile Phone</see> of 
		/// <see cref="clsServiceProvider">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_QuickMobilePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_QuickMobilePhone");
		}

		/// <summary>
		/// <see cref="clsPerson.my_Email">Email Address</see> of
		/// <see cref="clsPerson">ServiceProvider</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Email">Email Address</see> of 
		/// <see cref="clsPerson">ServiceProvider</see> 
		/// </returns>
		public string my_ServiceProvider_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ServiceProvider_Email");
		}

		/// <summary>
		/// If this Service Provider is a Security Company or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>this Service Provider is a Security Company or not</returns>
		public int my_ServiceProvider_IsSecurityCompany(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ServiceProvider_IsSecurityCompany"));
		}
		
		
		#endregion

		#region My_ Values Customer

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupId">CustomerGroupId</see> of 
		/// <see cref="clsCustomerGroup">CustomerGroup</see> for this Customer</summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupId">CustomerGroupId</see> 
		/// of Associated <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// for this Customer</returns>
		public int my_Customer_CustomerGroupId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Customer_CustomerGroupId"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerType">Type</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerType">Type</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_CustomerType(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_CustomerType");
		}

		
		/// <summary>
		/// <see cref="clsCustomer.my_CompanyName">Company Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CompanyName">Company Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_CompanyName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_CompanyName");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_AccountNumber">Account Number</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_AccountNumber">Account Number</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_AccountNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_AccountNumber");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_Title">Title</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_Title">Title</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_Title");
		}

				
		/// <summary>
		/// <see cref="clsCustomer.my_FirstName">First Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_FirstName">First Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_FirstName");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_LastName">Last Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_LastName">Last Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_LastName");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_FullName">Full Name</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_FullName">Full Name</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_FullName");
		}



		/// <summary>
		/// <see cref="clsCustomer.my_DateStart">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_DateStart">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_DateStart(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateStart");
		}




		/// <summary>
		/// <see cref="clsCountry.my_CountryId">CountryId</see> of 
		/// <see cref="clsCountry">Country</see></summary>
		/// <param Long="rowNum">Row number for Data</param>
		/// <returns><see cref="clsCountry.my_CountryId">CountryId</see> 
		/// of Associated <see cref="clsCountry">Country</see> 
		/// for this Order</returns>
		public int my_Customer_CountryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Customer_CountryId"));
		}

		
		/// <summary>
		/// <see cref="clsCustomer.my_OpeningBalance">Opening Balance</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_OpeningBalance">Opening Balance</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public decimal my_Customer_OpeningBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_OpeningBalance"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CreditLimit">Credit Limit</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CreditLimit">Credit Limit</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public decimal my_Customer_CreditLimit(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_CreditLimit"));
		}


		/// <summary>
		/// <see cref="clsCustomer.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_DateLastLoggedIn(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateLastLoggedIn");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_DateLastLoggedInUtc">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_DateLastLoggedInUtc">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_DateLastLoggedInUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateLastLoggedInUtc");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_StartDateForStatement">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_StartDateForStatement">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_StartDateForStatement(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_StartDateForStatement");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_StartDateForInvoices">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_StartDateForInvoices">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_StartDateForInvoices(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_StartDateForInvoices");
		}


		/// <summary>
		/// <see cref="clsCustomer.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_KdlComments");
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public string my_Customer_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_CustomerComments");
		}

		/// <summary>
		/// Total Spend by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Spend by this Customer</returns>
		public decimal my_Customer_TotalSpend(int rowNum)
		{
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalItemCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalFreightCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalTaxCost"));
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalItemCost"))
					+ Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalFreightCost"));
		}

		/// <summary>
		/// Total Item Cost of all Items bought by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Item Cost of all Items bought by this Customer</returns>
		public decimal my_Customer_TotalItemCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalItemCost"));
		}
		
		/// <summary>
		/// Total Tax Cost of all Taxs bought by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Tax Cost of all Taxs bought by this Customer</returns>
		public decimal my_Customer_TotalTaxCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalTaxCost"));
		}

		/// <summary>
		/// Total Freight Cost of all Freights bought by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Total Freight Cost of all Freights bought by this Customer</returns>
		public decimal my_Customer_TotalFreightCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalFreightCost"));
		}

		/// <summary>
		/// Number of Orders by this Customer
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of Orders by this Customer</returns>
		public int my_Customer_NumOrders(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Customer_NumOrders"));
		}

		/// <summary>
		/// The Date/Time that this Customer first Completed an Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer first Completed an Order</returns>
		public string my_Customer_DateFirstOrder(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateFirstOrder");
		}


		/// <summary>
		/// The Date/Time that this Customer First Completed an Order (Utc)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer First Completed an Order (Utc)</returns>
		public string my_Customer_DateFirstOrderUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateFirstOrderUtc");
		}

		/// <summary>
		/// The Date/Time that this Customer last Completed an Order
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer last Completed an Order</returns>
		public string my_Customer_DateLastOrder(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateLastOrder");
		}

		/// <summary>
		/// The Date/Time that this Customer last Completed an Order (Utc)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>The Date/Time that this Customer last Completed an Order (Utc)</returns>
		public string my_Customer_DateLastOrderUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Customer_DateLastOrderUtc");
		}

		/// <summary>
		/// Customer's Base Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s without 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Base Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s without 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see></returns>
		public decimal my_Customer_BaseBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_BaseBalance"));
		}

		/// <summary>
		/// Customer's Current Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s  
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Current Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s,  
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see></returns>
		public decimal my_Customer_CurrentBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_CurrentBalance"));
		}
		
		/// <summary>
		/// Customer's Available Credit; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s, 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">Opening Balance</see>
		/// and <see cref="clsCustomer.my_CreditLimit">Credit Limit</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns> Customer's Available Credit; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s, 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">Opening Balance</see>
		/// and <see cref="clsCustomer.my_CreditLimit">Credit Limit</see></returns>
		public decimal my_Customer_AvailableCredit(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_AvailableCredit"));
		}

		/// <summary>
		/// Customer's Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s. 
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s.</returns>
		public decimal my_Customer_TotalPurchases(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalPurchases"));
		}

		/// <summary>
		/// Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments</returns>
		public decimal my_Customer_TotalPaid(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalPaid"));
		}

		/// <summary>
		/// Customer's Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.</returns>
		public decimal my_Customer_TotalUncleared(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_TotalUncleared"));
		}

		/// <summary>
		/// Customer's Invoice Base Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s without 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Invoice Base Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s without 
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see></returns>
		public decimal my_Customer_InvoiceBaseBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_InvoiceBaseBalance"));
		}

		/// <summary>
		/// Customer's Invoice Current Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s  
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Invoice Current Balance; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s,  
		/// taking into account <see cref="clsCustomer.my_OpeningBalance">OpeningBalance</see></returns>
		public decimal my_Customer_InvoiceCurrentBalance(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_InvoiceCurrentBalance"));
		}
		
		/// <summary>
		/// Customer's Invoice Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s. 
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Invoice Total Purchases; i.e. the sum total of all of this customer's negative
		/// <see cref="clsTransaction">Transaction</see>s.</returns>
		public decimal my_Customer_InvoiceTotalPurchases(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_InvoiceTotalPurchases"));
		}

		/// <summary>
		/// Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Total Paid; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are cleared payments</returns>
		public decimal my_Customer_InvoiceTotalPaid(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_InvoiceTotalPaid"));
		}

		/// <summary>
		/// Customer's Invoice Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Customer's Invoice Total Uncleared Funds; i.e. the sum total of all of this customer's
		/// <see cref="clsTransaction">Transaction</see>s that are uncleared payments.</returns>
		public decimal my_Customer_InvoiceTotalUncleared(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Customer_InvoiceTotalUncleared"));
		}


		#endregion

	}
}
