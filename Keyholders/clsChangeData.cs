using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsChangeData deals with everything to do with data about ChangeData.
	/// </summary>

	[GuidAttribute("284B7D2E-5FB6-4d48-B5F1-1B793F4B7D70")]
	public class clsChangeData : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsChangeData
		/// </summary>
		public clsChangeData() : base("ChangeData")
		{
		}

		/// <summary>
		/// Constructor for clsChangeData; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsChangeData(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("ChangeData")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = ChangeDataQueryPart();
			
			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[1];
			
			baseQueries[0] = MainQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);

			orderBySqlQuery = "Order By ChangeDataId" + crLf;
		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsChangeData
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("CreatedByUserId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CreatedByFirstNameLastName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateCreated", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ChangedByUserId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ChangedByFirstNameLastName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateChanged", System.Type.GetType("System.String"));
			
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

		/// <summary>
		/// assumedSystemUserId; saves having to load settings all the time.
		/// </summary>
		public int assumedSystemUserId = 1;

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all ChangeDatas
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a ChangeData by ChangeDataId.
		/// </summary>
		/// <param name="ChangeDataId">Id of ChangeData to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByChangeDataId(int ChangeDataId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ ChangeDataId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		
		/// <summary>
		/// Gets all changes to the Care Plan for a Referral
		/// </summary>
		/// <param name="ReferralId">ReferralId for Care Plan</param>
		/// <returns>Number of resulting records</returns>
		public int GetAllChangesToCarePlan(int ReferralId)
		{
			thisSqlQuery = "Select tblChangeData.*" + crLf
				+ "from tblCpdNeed, tblChangeData" + crLf
				+ "where tblCpdNeed.ReferralId = " + ReferralId.ToString() + crLf
				+ "	and tblCpdNeed.ChangeDataId = tblChangeData.ChangeDataId" + crLf
				+ " union " + crLf
				+ "Select tblChangeData.*" + crLf
				+ "from tblCpdObjective, tblCpdNeed, tblChangeData	" + crLf
				+ "where tblCpdNeed.ReferralId = " + ReferralId.ToString() + crLf
				+ "	and tblCpdObjective.CpdNeedId = tblCpdNeed.CpdNeedId" + crLf
				+ "	and tblCpdObjective.ChangeDataId = tblChangeData.ChangeDataId" + crLf
				+ "	union " + crLf
				+ "Select tblChangeData.*" + crLf
				+ "from tblCpdPlanItem, tblCpdObjective, tblCpdNeed, tblChangeData	" + crLf
				+ "where tblCpdNeed.ReferralId = " + ReferralId.ToString() + crLf
				+ "	and tblCpdPlanItem.CpdObjectiveId = tblCpdObjective.CpdObjectiveId" + crLf
				+ "	and tblCpdObjective.CpdNeedId = tblCpdNeed.CpdNeedId" + crLf
				+ "	and tblCpdPlanItem.ChangeDataId = tblChangeData.ChangeDataId" + crLf;

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += "Order By tblChangeData.DateChanged desc";

			return localRecords.GetRecords(thisSqlQuery);
		}



		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal ChangeData table stack; the SaveChangeDatas method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CreatedByUserId">CreatedByUserId</param>
		/// <param name="CreatedByFirstNameLastName">CreatedByFirstNameLastName</param>
		/// <param name="DateCreated">DateCreated</param>
		/// <param name="ChangedByUserId">ChangedByUserId</param>
		/// <param name="ChangedByFirstNameLastName">ChangedByFirstNameLastName</param>
		/// <param name="DateChanged">DateChanged</param>
		public void Add(int CreatedByUserId,
			string CreatedByFirstNameLastName,
			string DateCreated,
			int ChangedByUserId,
			string ChangedByFirstNameLastName,
			string DateChanged)
		{

			string dtaNow = localRecords.DBDateTime(DateTime.Now);
			
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			#region CreatedByUserId and CreatedByFirstNameLastName

			if (CreatedByUserId == 0)
				rowToAdd["CreatedByUserId"] = assumedSystemUserId;
			else
				rowToAdd["CreatedByUserId"] = CreatedByUserId;

			if (CreatedByFirstNameLastName == "")
				if (CreatedByUserId != assumedSystemUserId)
				{
					clsUser thisUser = new clsUser(thisDbType, localRecords.dbConnection);

					int numResults = thisUser.GetFirstNameLastNameByUserId(CreatedByUserId);
					if (numResults != 0)
						rowToAdd["CreatedByFirstNameLastName"] = (thisUser.my_FirstName(0) + " " + thisUser.my_LastName(0)).Trim();
					else
						rowToAdd["CreatedByFirstNameLastName"] = "";
				}
				else
					rowToAdd["CreatedByFirstNameLastName"] = "System User";
			else
				rowToAdd["CreatedByFirstNameLastName"] = CreatedByFirstNameLastName;

			#endregion

			#region DateCreated

			if (DateCreated == "")
				rowToAdd["DateCreated"] = localRecords.DBDateTime(Convert.ToDateTime(dtaNow));
			else
				rowToAdd["DateCreated"] = localRecords.DBDateTime(Convert.ToDateTime(DateCreated));

			#endregion

			#region ChangedByUserId and ChangedByFirstNameLastName

			if (ChangedByUserId == 0)
				rowToAdd["ChangedByUserId"] = assumedSystemUserId;
			else
				rowToAdd["ChangedByUserId"] = ChangedByUserId;

			if(rowToAdd["ChangedByUserId"] == rowToAdd["CreatedByUserId"])
			{
				rowToAdd["ChangedByFirstNameLastName"] = rowToAdd["CreatedByFirstNameLastName"] ;
			}
			else 
			{

				if (ChangedByFirstNameLastName == "")
					if (ChangedByUserId != assumedSystemUserId)
					{
						clsUser thisUser = new clsUser(thisDbType, localRecords.dbConnection);
						int numResults = thisUser.GetFirstNameLastNameByUserId(ChangedByUserId);
						if (numResults != 0)
							rowToAdd["ChangedByFirstNameLastName"] = (thisUser.my_FirstName(0) + " " + thisUser.my_LastName(0)).Trim();
						else
							rowToAdd["ChangedByFirstNameLastName"] = "";
					}
					else
						rowToAdd["ChangedByFirstNameLastName"] = "System User";
				else
					rowToAdd["ChangedByFirstNameLastName"] = ChangedByFirstNameLastName;
			}
			#endregion

			#region DateChanged

			if (DateChanged == "")
				rowToAdd["DateChanged"] = localRecords.DBDateTime(Convert.ToDateTime(dtaNow));
			else
				rowToAdd["DateChanged"] = localRecords.DBDateTime(Convert.ToDateTime(DateChanged));
						
			#endregion

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
		/// internal ChangeData table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="ChangeDataId">ChangeDataId (Primary Key of Record)</param>
		/// <param name="CreatedByUserId">CreatedByUserId</param>
		/// <param name="CreatedByFirstNameLastName">CreatedByFirstNameLastName</param>
		/// <param name="DateCreated">DateCreated</param>
		/// <param name="ChangedByUserId">ChangedByUserId</param>
		/// <param name="ChangedByFirstNameLastName">ChangedByFirstNameLastName</param>
		/// <param name="DateChanged">DateChanged</param>
		public void Modify(int ChangeDataId, 
			int CreatedByUserId,
			string CreatedByFirstNameLastName,
			string DateCreated,
			int ChangedByUserId,
			string ChangedByFirstNameLastName,
			string DateChanged)
		{

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = ChangeDataId;

			clsUser thisUser = new clsUser(thisDbType, localRecords.dbConnection);

			if (assumedSystemUserId == 0)
				GetGeneralSettings();

			if (CreatedByUserId == 0)
				rowToAdd["CreatedByUserId"] = assumedSystemUserId;
			else
				rowToAdd["CreatedByUserId"] = CreatedByUserId;

			if (CreatedByFirstNameLastName == "")
				if (CreatedByUserId != assumedSystemUserId)
				{
					int numResults = thisUser.GetFirstNameLastNameByUserId(CreatedByUserId);
					if (numResults != 0)
						rowToAdd["CreatedByFirstNameLastName"] = (thisUser.my_FirstName(0) + " " + thisUser.my_LastName(0)).Trim();
					else
						rowToAdd["CreatedByFirstNameLastName"] = "";
				}
				else
					rowToAdd["CreatedByFirstNameLastName"] = "System User";
			else
				rowToAdd["CreatedByFirstNameLastName"] = CreatedByFirstNameLastName;

			if (DateCreated == "")
				rowToAdd["DateCreated"] = localRecords.DBDateTime(Convert.ToDateTime(dtaNow));
			else
				rowToAdd["DateCreated"] = localRecords.DBDateTime(Convert.ToDateTime(DateCreated));

			if (ChangedByUserId == 0)
				rowToAdd["ChangedByUserId"] = assumedSystemUserId;
			else
				rowToAdd["ChangedByUserId"] = ChangedByUserId;

			if (ChangedByFirstNameLastName == "")
				if (ChangedByUserId != assumedSystemUserId)
				{
					int numResults = thisUser.GetFirstNameLastNameByUserId(ChangedByUserId);
					if (numResults != 0)
						rowToAdd["ChangedByFirstNameLastName"] = (thisUser.my_FirstName(0) + " " + thisUser.my_LastName(0)).Trim();
					else
						rowToAdd["ChangedByFirstNameLastName"] = "";
				}
				else
					rowToAdd["ChangedByFirstNameLastName"] = "System User";
			else
				rowToAdd["ChangedByFirstNameLastName"] = ChangedByFirstNameLastName;


			if (DateChanged == "")
				rowToAdd["DateChanged"] = localRecords.DBDateTime(Convert.ToDateTime(dtaNow));
			else
				rowToAdd["DateChanged"] = localRecords.DBDateTime(Convert.ToDateTime(DateChanged));

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
		
		}
		
		#endregion

		# region My_ Values ChangeData

		/// <summary>
		/// <see cref="clsChangeData.my_CreatedByUserId">CreatedByUserId</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_CreatedByUserId">CreatedByUserId</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public int my_CreatedByUserId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CreatedByUserId"));
		}

		/// <summary>
		/// <see cref="clsChangeData.my_CreatedByFirstNameLastName">CreatedByFirstNameLastName</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_CreatedByFirstNameLastName">CreatedByFirstNameLastName</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public string my_CreatedByFirstNameLastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CreatedByFirstNameLastName");
		}

		/// <summary>
		/// <see cref="clsChangeData.my_DateCreated">DateCreated</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_DateCreated">DateCreated</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public string my_DateCreated(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateCreated");
		}

		/// <summary>
		/// <see cref="clsChangeData.my_ChangedByUserId">ChangedByUserId</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_ChangedByUserId">ChangedByUserId</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public int my_ChangedByUserId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ChangedByUserId"));
		}

		/// <summary>
		/// <see cref="clsChangeData.my_ChangedByFirstNameLastName">ChangedByFirstNameLastName</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_ChangedByFirstNameLastName">ChangedByFirstNameLastName</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public string my_ChangedByFirstNameLastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ChangedByFirstNameLastName");
		}

		/// <summary>
		/// <see cref="clsChangeData.my_DateChanged">DateChanged</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_DateChanged">DateChanged</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public string my_DateChanged(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateChanged");
		}
		#endregion
	}
}
