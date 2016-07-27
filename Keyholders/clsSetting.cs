using System;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.Odbc;
using Resources;
using System.Collections;

namespace Keyholders
{
	/// <summary>
	/// clsSetting deals with everything to do with data about Settings.
	/// </summary>
	

	[GuidAttribute("06767402-8A8B-49a3-9C03-03E8C73771BE")]
	public class clsSetting : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsSetting
		/// </summary>
		public clsSetting() : base("Setting")
		{
		}

		/// <summary>
		/// Constructor for clsSetting; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsSetting(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Setting")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			MainQ.AddSelectColumn("tblSetting.SettingId");
			MainQ.AddSelectColumn("tblSetting.SettingName");
			MainQ.AddSelectColumn("tblSetting.SettingValue");

			MainQ.AddFromTable(thisTable);

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblSetting.SettingName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsSetting
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("SettingName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("SettingValue", System.Type.GetType("System.String"));
			
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
			TimeZone = new clsTimeZone();
			TimeZone.Connect((int) DatabaseType_MySql(), 
				TimeZoneConnectionString());
		}

		/// <summary>
		/// Local Representaion of the class <see cref="clsTimeZone">clsTimeZone</see>
		/// </summary>
		public clsTimeZone TimeZone = new clsTimeZone();



		#endregion

		/// <summary>
		/// Struct for Settings
		/// </summary>
		public struct settingType
		{	
			/// <summary>
			/// Id of Setting
			/// </summary>
			public int SettingId;
			/// <summary>
			/// Name of Setting
			/// </summary>
			public string SettingName;
			/// <summary>
			/// Value of Setting
			/// </summary>
			public string SettingValue;
		}

		/// <summary>
		/// Setting Array Holder
		/// </summary>
		public ArrayList settingsList = new ArrayList();

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Settings
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

			int numResults = localRecords.GetRecords(thisSqlQuery);


			//Allocate all settings to a field
			for(int counter = 0; counter < numResults; counter++)
			{
				settingType thisSetting = new settingType();
				thisSetting.SettingId = my_SettingId(counter);
				thisSetting.SettingName = my_SettingName(counter).Trim().ToLower();
				thisSetting.SettingValue = my_SettingValue(counter);

				switch(thisSetting.SettingName)
				{
					case "assumedcountryid":
						assumedCountryId = Convert.ToInt32(thisSetting.SettingValue);
						break;
					case "assumedshippingzoneid":
						assumedShippingZoneId = Convert.ToInt32(thisSetting.SettingValue);
						break;
					case "clienttimezoneosindex":
						thisClientTimeZoneOsIndex = Convert.ToInt64(thisSetting.SettingValue);
						break;
					case "clienttimezoneregkey":
						thisClientTimeZoneRegKey = thisSetting.SettingValue;
						break;
					case "freightchargebasis":
						freightChargeBasis = (clsKeyBase.freightChargeType) Convert.ToInt32(thisSetting.SettingValue);
						break;
					case "localtaxpercent":
						decimal localTaxPerCent = Convert.ToDecimal(thisSetting.SettingValue);
						localTaxRate = 1 + localTaxPerCent / 100;
						break;
					case "maximumfreightcharge":
						maximumFreightCharge = Convert.ToDecimal(thisSetting.SettingValue);
						break;
					case "minimumbalancerequiringstatement":
						minimumBalanceRequiringStatement = Convert.ToDecimal(thisSetting.SettingValue);
						break;
					case "minimumdetailsupdatefrequency":
						minimumDetailsUpdateFrequency = Convert.ToInt32(thisSetting.SettingValue);
						break;
					case "minimumfreightcharge":
						minimumFreightCharge = Convert.ToDecimal(thisSetting.SettingValue);
						break;
					case "minimumstatementfrequency":
						minimumStatementFrequency = Convert.ToInt32(thisSetting.SettingValue);
						break;
					case "noncitycityid":
						nonCityCityId = Convert.ToInt32(thisSetting.SettingValue);
						break;
					case "priceshownincludeslocaltax":
						if (thisSetting.SettingValue == "1")
							priceShownIncludesLocalTaxRate = true;
						else
							priceShownIncludesLocalTaxRate = false;
						break;
					case "publiccustomergroupid":
						publicCustomerGroupId = Convert.ToInt32(thisSetting.SettingValue);
						break;
					case "systemuserid":
						systemUserId = Convert.ToInt32(thisSetting.SettingValue);
						break;
					case "thisoutputpath":
						thisOutputPath = thisSetting.SettingValue;
						break;
					case "thisreportpath":
						thisReportPath = thisSetting.SettingValue;
						break;
					case "thisrootpath":
						thisRootPath = thisSetting.SettingValue;
						break;
					default:
						break;

				}
				settingsList.Add(thisSetting);

			}



			return numResults;

		}

		/// <summary>
		/// Gets general Settings
		/// </summary>
		public override void GetGeneralSettings()
		{
			if (settingsList.Count == 0)
				GetAll();
		}


		/// <summary>
		/// Gets an Setting by SettingId
		/// </summary>
		/// <param name="SettingId">Id of Setting to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetBySettingId(int SettingId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ SettingId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Returns Requested Setting
		/// </summary>
		/// <param name="SettingName">Name of Setting to retrieve</param>
		/// <returns>Requested Setting</returns>
		public string GetSetting(string SettingName)
		{
			
			int numSettings;
			
			if (settingsList.Count == 0)
				numSettings = GetAll();
			else
				numSettings = NumRecords();

			bool foundSetting = false;
			int counter = 0;
			string retval = "";

			string settingToFind = SettingName.Trim().ToLower();

			while(!foundSetting && counter < numSettings)
			{
				settingType thisSetting = (settingType) settingsList[counter];
				if (thisSetting.SettingName == settingToFind)
				{
					retval = thisSetting.SettingValue;
					foundSetting = true;
					if(priceShownIncludesLocalTaxRate && 
						(thisSetting.SettingName == "minimumFreightCharge"  ||
						thisSetting.SettingName == "maximumFreightCharge"))

						retval = (Convert.ToDecimal(thisSetting.SettingValue) * localTaxRate).ToString();
				}
				counter++;
			}

			return retval;

		}

	
		/// <summary>
		/// Gets a setting by Name
		/// </summary>
		/// <param name="SettingName">Name of Setting to retrieve</param>
		/// <returns>Number of Resulting Records</returns>
		public int GetBySettingName(string SettingName)
		{
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblSetting.SettingName " + MatchCondition(SettingName, matchCriteria.exactMatch) + crLf;

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
		/// Returns the next order number and increments this value in the Settings Table
		/// </summary>
		/// <returns>Next Order Number</returns>
		public string GetNextOrderNumber()
		{
			string thisSettingName = "NextOrderNumber";
 
			int numSettings = GetBySettingName(thisSettingName);

			if (numSettings == 0)
				numSettings = GetBySettingName("BaseOrderNumber");

			long nextOrder = 100000;


			if (numSettings == 0)
				Add(thisSettingName, Convert.ToString(nextOrder + 1));
			else
			{
				if (my_SettingValue(0) != "")
					nextOrder = Convert.ToInt64(my_SettingValue(0));

				Modify(my_SettingId(0), thisSettingName, Convert.ToString(nextOrder + 1));
			}	

			Save();
			
			return Convert.ToString(nextOrder);
		}

		/// <summary>
		/// Returns the next Sticker number and increments this value in the Settings Table
		/// </summary>
		/// <returns>Next Sticker Number</returns>
		public string GetNextStickerNumber()
		{
			string thisSettingName = "NextStickerNumber";
 
			GetBySettingName(thisSettingName);

			long nextSticker = Convert.ToInt64(my_SettingValue(0));
			
			Modify(my_SettingId(0), thisSettingName, Convert.ToString(nextSticker + 1));

			Save();
			
			return Convert.ToString(nextSticker);
		}

		/// <summary>
		/// Returns the Base order number
		/// </summary>
		/// <returns>Base Order Number</returns>
		public string GetBaseOrderNumber()
		{
			string thisSettingName = "BaseOrderNumber";
 
			GetBySettingName(thisSettingName);

			long BaseOrder = Convert.ToInt64(my_SettingValue(0));
			
			Modify(my_SettingId(0), thisSettingName, Convert.ToString(BaseOrder + 1));

			Save();
			
			return Convert.ToString(BaseOrder);
		}


		/// <summary>
		/// Gets a Distinct List of Setting Names only. This is intented for use e.g. as an
		/// auto-completing drop down.
		/// </summary>
		/// <param name="SettingName">Name of Setting</param>
		/// <param name="MatchCriteria">Criteria to match Setting Name, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctSettingName(string SettingName, int MatchCriteria)
		{
			string fieldRequired = "SettingName";
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct "+ thisTable + "." + fieldRequired);

			string condition = "(Select * from " + thisTable + crLf;
			
			string match = MatchCondition(SettingName, 
				(matchCriteria) MatchCriteria);

			//Additional Conditions
			condition += "Where " + fieldRequired + match + crLf;
			

			thisSqlQuery = QB.BuildSqlStatement(
				queries,
				condition + ") " + thisTable,
				thisTable
				);

			//Ordering
			thisSqlQuery += "Order By tblSetting.SettingName" + crLf;

			return localRecords.GetRecords(thisSqlQuery);

		}

		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="SettingName">Setting's Name</param>
		/// <param name="SettingValue">Value of this Setting</param>
		public void Add(string SettingName, 
			string SettingValue)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["SettingName"] = SettingName;
			rowToAdd["SettingValue"] = SettingValue;
			
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
		/// <param name="SettingId">SettingId (Primary Key of Record)</param>
		/// <param name="SettingName">Setting's Name</param>
		/// <param name="SettingValue">Value of this Setting</param>
		public void Modify(int SettingId,
			string SettingName, 
			string SettingValue)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["SettingId"] = SettingId;
			rowToAdd["SettingName"] = SettingName;

			if (settingsList.Count == 0)
				GetAll();

			if (priceShownIncludesLocalTaxRate)
				switch (SettingName)
				{
					case "MinimumFreightCharge":
					case "MaximumFreightCharge":
						SettingValue = (Convert.ToDecimal(SettingValue) / localTaxRate).ToString();
						break;
					default:
						break;
				}

			rowToAdd["SettingValue"] = SettingValue;

			Validate(rowToAdd, false);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);
			}

		}


		/// <summary>
		/// Checks for the existance of the Setting with the supplied name.
		/// If it exists, it is modified to this Setting Value
		/// If it does not exist, it is created and set with this valie
		/// </summary>
		/// <param name="SettingName">Setting's Name</param>
		/// <param name="SettingValue">Value of this Setting</param>
		public void Ensure(string SettingName, 
			string SettingValue)
		{
			int settingExists = GetBySettingName(SettingName);

			if (settingExists > 0)
				Modify(my_SettingId(0), SettingName, SettingValue);
			else
				Add(SettingName, SettingValue);

			Save();
		}

		/// <summary>
		/// Sets Requested Setting
		/// </summary>
		/// <param name="SettingName">Name of Setting to retrieve</param>
		/// <param name="SettingValue">Value of Setting</param>
		public void SetSetting(string SettingName, string SettingValue)
		{
			int numSettings;
			
			if (settingsList.Count == 0)
				numSettings = GetAll();
			else
				numSettings = NumRecords();

			bool foundSetting = false;
			int counter = -1;
			settingType thisSetting = new settingType();

			string settingToFind = SettingName.Trim().ToLower();

			while(!foundSetting && counter < numSettings)
			{
				counter++;
				thisSetting = (settingType) settingsList[counter];
				if (thisSetting.SettingName == settingToFind)
					foundSetting = true;
			}

			if (foundSetting)
			{
				if (thisSetting.SettingValue != SettingValue)
					Modify(thisSetting.SettingId, SettingName, SettingValue);
			}
			else
				Add(SettingName, SettingValue);
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
		/// SettingId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>SettingId for this Row</returns>
		public int my_SettingId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "SettingId"));
		}

		/// <summary>
		/// Setting Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name of Setting for this Row</returns>
		public string my_SettingName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "SettingName");
		}

		/// <summary>Value of this Setting</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Value of this Setting</returns>	
		public string my_SettingValue(int rowNum)
		{
			string SettingValue = localRecords.FieldByName(rowNum, "SettingValue");

			if (priceShownIncludesLocalTaxRate)
				switch (my_SettingName(rowNum))
				{
					case "MinimumFreightCharge":
					case "MaximumFreightCharge":
						SettingValue = (Convert.ToDecimal(SettingValue) * localTaxRate).ToString();
						break;
					default:
						break;
				}

			return SettingValue;
		}

		# endregion
	}
}
