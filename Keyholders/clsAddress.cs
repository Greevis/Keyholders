using System;
using System.Data;
using System.Runtime.InteropServices;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsAddress deals with everything to do with data about Addresses.
	/// </summary>
	
	[GuidAttribute("0C529B26-5AD2-4069-9EDB-68D1430B28A7")]
	public class clsAddress : clsKeyBase
	{
		# region Initialisation
		
		/// <summary>
		/// Constructor for clsAddress
		/// </summary>
		public clsAddress() : base("Address")
		{
		}

		/// <summary>
		/// Constructor for clsAddress; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsAddress(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Address")
		{
			Connect(typeOfDb, odbcConnection);
		}
		
		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = AddressQueryPart(false);
			ChangeDataQ = ChangeDataQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[2];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = ChangeDataQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);

			orderBySqlQuery = "Order By CountryName, CityName, StateName, Suburb, StreetAddress" + crLf;

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsAddress
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("POBoxType", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("BuildingName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("UnitNumber", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Number", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("StreetAddress", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Suburb", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CityId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CityName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("StateName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PostCode", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CountryId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CountryName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickAddress", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AddressType", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("AddressTypeDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AssocTableName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AssocRowId", System.Type.GetType("System.Int32"));
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
			//Instances of Foreign Key Classes		
		}

		/// <summary>
		/// Local Representation of the class <see cref="clsAddress">clsAddress</see>
		/// </summary>
		public clsAddress thisAddress;

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Addresss
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
		/// Gets an Address by AddressId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByAddressId(int AddressId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ AddressId.ToString() + crLf;

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
		/// Gets Addresses by PersonId
		/// </summary>
		/// <param name="PersonId">Id of Person who's Persones to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonId(int PersonId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".AssocTableName = "
				+ "'tblPerson'" + crLf
				+ "And " + thisTable + ".AssocRowId = "
				+ PersonId.ToString() + crLf;

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
		/// Gets Addresss for a given Address of a given type
		/// </summary>
		/// <param name="AddressId"></param>
		/// <param name="addressTypeRequired">Type of Address Required
		/// from the enumeration 
		/// <see cref="clsKeyBase.addressType">addressType</see> 
		/// </param>
		/// <returns>Number of resulting records</returns>
		public int GetByAddressIdaddressType(int AddressId, int addressTypeRequired)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".AssocTableName = "
				+ "'tblAddress'" + crLf
				+ "And " + thisTable + ".AssocRowId = "
				+ AddressId.ToString() + crLf
				+ "And " + thisTable + ".AddressType = " 
				+ addressTypeRequired.ToString() + crLf;

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
		/// Gets Addresses by PremiseId
		/// </summary>
		/// <param name="PremiseId">Id of Premise who's Addresses to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPremiseId(int PremiseId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".AssocTableName = "
				+ "'tblPremise'" + crLf
				+ "And " + thisTable + ".AssocRowId = "
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
		/// Gets Addresss for a given Premise of a given type
		/// </summary>
		/// <param name="PremiseId"></param>
		/// <param name="AddressType">Type of Address Required
		/// from the enumeration 
		/// <see cref="clsKeyBase.addressType">addressType</see> 
		/// </param>
		/// <returns>Number of resulting records</returns>
		public int GetByPremiseIdAddressType(int PremiseId, int AddressType)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".AssocTableName = "
				+ "'tblPremise'" + crLf
				+ "And " + thisTable + ".AssocRowId = "
				+ PremiseId.ToString() + crLf
				+ "And " + thisTable + ".AddressType = " 
				+ AddressType.ToString() + crLf;

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
		/// <param name="POBoxType">Whether this Address includes a PO Box or not</param>
		/// <param name="BuildingName">BuildingName</param>
		/// <param name="UnitNumber">UnitNumber</param>
		/// <param name="Number">Number</param>
		/// <param name="StreetAddress">Street Address</param>
		/// <param name="Suburb">Suburb</param>
		/// <param name="CityId">City associated with this Address</param>
		/// <param name="CityName">Name of City associated with this Address</param>
		/// <param name="PostCode">Post Code associated with this Address</param>
		/// <param name="StateName">Name of State associated with this Address</param>
		/// <param name="CountryId">Country associated with this Address</param>
		/// <param name="CountryName">Name of Country associated with this Address</param>
		/// <param name="AddressType">Type of Address, from <see cref="clsKeyBase.addressType">addressType</see></param>
		/// <param name="AddressTypeDescription">Description of Address Type. 
		/// Only required if the 
		/// <see cref="clsKeyBase.addressType">addressType</see>is
		/// <see cref="clsKeyBase.addressType_other">addressType_other</see></param>
		/// <param name="AssocTableName">Name of the Table Associated with this Address</param>
		/// <param name="AssocRowId">Row in AssocTableName Associated with this Address</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Add(int POBoxType, 
			string BuildingName,
			string UnitNumber,
			string Number,
			string StreetAddress,
			string Suburb,
			int CityId,
			string CityName, 
			string StateName, 
			int CountryId,
			string CountryName, 
			string PostCode, 
			int AddressType,
			string AddressTypeDescription,
			string AssocTableName,
			int AssocRowId,
			int CurrentUser)
		{
		
			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			AddGeneral(POBoxType,
				BuildingName,
				UnitNumber,
				Number,
				StreetAddress,
				Suburb,
				CityId,
				CityName,
				StateName,
				CountryId,
				CountryName,
				PostCode,
				AddressType,
				AddressTypeDescription,
				AssocTableName,
				AssocRowId,
				thisChangeData.LastIdAdded(),
				0);
			
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
			thisAddress = new clsAddress(thisDbType, localRecords.dbConnection);
			thisAddress.GetByAddressId(thisPkId);

			AddGeneral(thisAddress.my_POBoxType(0), 
				thisAddress.my_BuildingName(0),
				thisAddress.my_UnitNumber(0),
				thisAddress.my_Number(0),
				thisAddress.my_StreetAddress(0), 
				thisAddress.my_Suburb(0), 
				thisAddress.my_CityId(0), 
				thisAddress.my_CityName(0),
				thisAddress.my_StateName(0),
				thisAddress.my_CountryId(0), 
				thisAddress.my_CountryName(0), 
				thisAddress.my_PostCode(0), 
				thisAddress.my_AddressType(0), 
				thisAddress.my_AddressTypeDescription(0), 
				thisAddress.my_AssocTableName(0), 
				thisAddress.my_AssocRowId(0), 
				thisAddress.my_ChangeDataId(0), 
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisAddress.my_ChangeData_CreatedByUserId(0),
				thisAddress.my_ChangeData_CreatedByFirstNameLastName(0),
				thisAddress.my_ChangeData_DateCreated(0),
				CurrentUser,"",dtaNow.ToString());
							
			thisChangeData.Save();

			return thisChangeData.LastIdAdded();
	
		}

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Incident table stack; the SaveIncidents method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="POBoxType">POBoxType</param>
		/// <param name="BuildingName">BuildingName</param>
		/// <param name="UnitNumber">UnitNumber</param>
		/// <param name="Number">Number</param>
		/// <param name="StreetAddress">StreetAddress</param>
		/// <param name="Suburb">Suburb</param>
		/// <param name="CityId">CityId</param>
		/// <param name="CityName">CityName</param>
		/// <param name="StateName">StateName</param>
		/// <param name="CountryId">CountryId</param>
		/// <param name="CountryName">CountryName</param>
		/// <param name="PostCode">PostCode</param>
		/// <param name="AddressType">AddressType</param>
		/// <param name="AddressTypeDescription">AddressTypeDescription</param>
		/// <param name="AssocTableName">AssocTableName</param>
		/// <param name="AssocRowId">AssocRowId</param>
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		public void AddGeneral(int POBoxType,
			string BuildingName,
			string UnitNumber,
			string Number,
			string StreetAddress,
			string Suburb,
			int CityId,
			string CityName,
			string StateName,
			int CountryId,
			string CountryName,
			string PostCode,
			int AddressType,
			string AddressTypeDescription,
			string AssocTableName,
			int AssocRowId,
			int ChangeDataId,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			clsCity thisCity = new clsCity(thisDbType, localRecords.dbConnection);
			clsCountry thisCountry = new clsCountry(thisDbType, localRecords.dbConnection);
			
			if (nonCityCityId == 0)
				GetGeneralSettings();

			if (CityId == 0)
			{
				if (CityName == "")
				{
					CityId = nonCityCityId;
				}
				else
				{
					thisCity.LoadMainQuery();
					int numCities = thisCity.GetByCityName(CityName,matchCriteria_exactMatch());
					if (numCities == 0)
					{

						//Get the country
						if (CountryId == 0)
						{

							if (CountryName == "")
							{
								CountryId = assumedCountryId;
								int numCountrys = thisCountry.GetByCountryId(CountryId);
								if (numCountrys > 0)
									CountryName = thisCountry.my_CountryName(0);
							}
							else
							{

								int numCountrys = thisCountry.GetByCountryName(CountryName, matchCriteria_exactMatch());
								if (numCountrys == 0)
								{
									thisCountry.Add(assumedShippingZoneId, CountryName, 0, 0);
									thisCountry.Save();
									CountryId = thisCountry.LastIdAdded();
								}
								else
									CountryId = thisCountry.my_CountryId(0);

							}
						}
						else
						{
							int numCountrys = thisCountry.GetByCountryId(CountryId);
							if (numCountrys > 0)
								CountryName = thisCountry.my_CountryName(0);
							else
							{
								CountryId = assumedCountryId;
								numCountrys = thisCountry.GetByCountryId(CountryId);
								if (numCountrys > 0)
									CountryName = thisCountry.my_CountryName(0);
							}

						}

						thisCity.Add(assumedCountryId, CityName, StateName, 1);
						thisCity.Save();
						CityId = thisCity.LastIdAdded();
					}
					else
						CityId = thisCity.my_CityId(0);
				}
			}
			else
			{
				thisCity.Initialise();
				int numCities = thisCity.GetByCityId(CityId);
				CityName = thisCity.my_CityName(0);
			}

			if (CountryId == 0)
			{
				CountryId = assumedCountryId;
				int numCountrys = thisCountry.GetByCountryId(CountryId);
				if (numCountrys > 0)
					CountryName = thisCountry.my_CountryName(0);
			}

			rowToAdd["POBoxType"] = POBoxType;
			rowToAdd["BuildingName"] = BuildingName;
			rowToAdd["UnitNumber"] = UnitNumber;
			rowToAdd["Number"] = Number;
			rowToAdd["StreetAddress"] = StreetAddress;
			rowToAdd["Suburb"] = Suburb;
			rowToAdd["CityId"] = CityId;
			rowToAdd["CityName"] = CityName;
			rowToAdd["StateName"] = StateName;
			rowToAdd["CountryId"] = CountryId;
			rowToAdd["CountryName"] = CountryName;
			rowToAdd["PostCode"] = PostCode;
			rowToAdd["QuickAddress"] = GetQuickAddress(POBoxType,
				BuildingName,
				UnitNumber,
				Number,
				StreetAddress,
				Suburb,
				PostCode,
				CityName,
				StateName,
				CountryName);


			rowToAdd["AddressType"] = AddressType;
			rowToAdd["AddressTypeDescription"] = AddressTypeDescription;
			rowToAdd["AssocTableName"] = AssocTableName;
			rowToAdd["AssocRowId"] = AssocRowId;
		
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
		/// Validates the supplied data for Errors and Warnings.
		/// If any Errors are found, ErrorFound will return true, and these Errors 
		/// are available through the ErrorMessage and ErrorFieldName methods
		/// If any Warnings are found, WarningFound will return true, and these Warnings 
		/// are available through the WarningMessage and WarningFieldName methods
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="AddressId">AddressId (Primary Key of Record)</param>
		/// <param name="POBoxType">Whether this Address includes a PO Box or not</param>
		/// <param name="BuildingName">BuildingName</param>
		/// <param name="UnitNumber">UnitNumber</param>
		/// <param name="Number">Number</param>
		/// <param name="StreetAddress">Street Address</param>
		/// <param name="Suburb">Suburb</param>
		/// <param name="CityId">City associated with this Address</param>
		/// <param name="CityName">Name of City associated with this Address</param>
		/// <param name="PostCode">Post Code associated with this Address</param>
		/// <param name="StateName">Name of State associated with this Address</param>
		/// <param name="CountryId">Country associated with this Address</param>
		/// <param name="CountryName">Name of Country associated with this Address</param>
		/// <param name="AddressType">Type of Address, from <see cref="clsKeyBase.addressType">addressType</see></param>
		/// <param name="AddressTypeDescription">Description of Address Type. 
		/// Only required if the 
		/// <see cref="clsKeyBase.addressType">addressType</see>is
		/// <see cref="clsKeyBase.addressType_other">addressType_other</see></param>
		/// <param name="AssocTableName">Name of the Table Associated with this Address</param>
		/// <param name="AssocRowId">Row in AssocTableName Associated with this Address</param>
		/// <param name="CurrentUser">CurrentUser</param>

		public void Modify(int AddressId, 
			int POBoxType, 
			string BuildingName,
			string UnitNumber,
			string Number,
			string StreetAddress,
			string Suburb,
			int CityId,
			string CityName, 
			string StateName, 
			int CountryId,
			string CountryName, 
			string PostCode, 
			int AddressType,
			string AddressTypeDescription,
			string AssocTableName,
			int AssocRowId,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, AddressId);
			rowToAdd["AddressId"] = AddressId;

			clsCity thisCity = new clsCity(thisDbType, localRecords.dbConnection);
			clsCountry thisCountry = new clsCountry(thisDbType, localRecords.dbConnection);

			if (nonCityCityId == 0)
				GetGeneralSettings();

			if (CityId == 0)
			{
				if (CityName == "")
				{
					CityId = nonCityCityId;
					CountryId = assumedCountryId;
					int numCountrys = thisCountry.GetByCountryId(CountryId);
					if (numCountrys > 0)
						CountryName = thisCountry.my_CountryName(0);
				}
				else
				{
					thisCity.LoadMainQuery();
					int numCities = thisCity.GetByCityName(CityName,matchCriteria_exactMatch());
					if (numCities == 0)
					{
						//Get the country
						if (CountryId == 0)
						{

							if (CountryName == "")
							{
								CountryId = assumedCountryId;
								int numCountrys = thisCountry.GetByCountryId(CountryId);
								if (numCountrys > 0)
									CountryName = thisCountry.my_CountryName(0);
							}
							else
							{

								int numCountrys = thisCountry.GetByCountryName(CountryName, matchCriteria_exactMatch());
								if (numCountrys == 0)
								{
									thisCountry.Add(assumedShippingZoneId, CountryName, 0, 0);
									thisCountry.Save();
									CountryId = thisCountry.LastIdAdded();
								}
								else
									CountryId = thisCountry.my_CountryId(0);

							}
						}
						else
						{
							int numCountrys = thisCountry.GetByCountryId(CountryId);
							if (numCountrys > 0)
								CountryName = thisCountry.my_CountryName(0);
							else
							{
								CountryId = assumedCountryId;
								numCountrys = thisCountry.GetByCountryId(CountryId);
								if (numCountrys > 0)
									CountryName = thisCountry.my_CountryName(0);
							}

						}

						thisCity.Add(assumedCountryId, CityName, StateName, 1);
						thisCity.Save();
						CityId = thisCity.LastIdAdded();
					}
					else
						CityId = thisCity.my_CityId(0);
				}
			}
			else
			{
				thisCity.Initialise();
				int numCities = thisCity.GetByCityId(CityId);
				CityName = thisCity.my_CityName(0);
				CountryId = thisCity.my_CountryId(0);
				CountryName = thisCity.my_Country_CountryName(0);
			}


			if (CountryId == 0)
			{
				CountryId = assumedCountryId;
				int numCountrys = thisCountry.GetByCountryId(CountryId);
				if (numCountrys > 0)
					CountryName = thisCountry.my_CountryName(0);
			}

			rowToAdd["POBoxType"] = POBoxType;
			rowToAdd["BuildingName"] = BuildingName;
			rowToAdd["UnitNumber"] = UnitNumber;
			rowToAdd["Number"] = Number;
			rowToAdd["StreetAddress"] = StreetAddress;
			rowToAdd["Suburb"] = Suburb;
			rowToAdd["CityId"] = CityId;
			rowToAdd["CountryId"] = CountryId;
			rowToAdd["CityName"] = CityName;
			rowToAdd["StateName"] = StateName;
			rowToAdd["CountryName"] = CountryName;
			rowToAdd["PostCode"] = PostCode;
			
			string thisQuickAddress = GetQuickAddress(POBoxType,
				BuildingName,
				UnitNumber,
				Number,
				StreetAddress,
				Suburb,
				PostCode,
				CityName,
				StateName,
				CountryName);

			rowToAdd["QuickAddress"] = thisQuickAddress;


			rowToAdd["AddressType"] = AddressType;
			rowToAdd["AddressTypeDescription"] = AddressTypeDescription;
			rowToAdd["AssocTableName"] = AssocTableName;
			rowToAdd["AssocRowId"] = AssocRowId;


			rowToAdd["Archive"] = 0;

			Validate(rowToAdd, false);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
				{
					dataToBeModified.Rows.Add(rowToAdd);
					
					string attribute = "Quick";

					switch ((addressType) AddressType)
					{
						case addressType.physical:
							attribute += "PhysicalAddress";
							break;
						case addressType.postal:
							attribute += "PostalAddress";
							break;
						default:
							attribute = "";
							break;
					}
					
					if (attribute != "")
					{
						
						//Update parent 'quick numbers'
						switch (AssocTableName)
						{
							case "tblPerson":
								clsPerson Person = new clsPerson(thisDbType, localRecords.dbConnection);
								Person.SetAttribute(AssocRowId, attribute, thisQuickAddress);
								Person.Save();
								break;
							case "tblPremise":
								clsPremise Premise = new clsPremise(thisDbType, localRecords.dbConnection);
								Premise.SetAttribute(AssocRowId, attribute, thisQuickAddress);
								Premise.Save();
								break;
							default:
								break;
						}
					}				
				}
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

		# region My_ Values Address

		/// <summary>
		/// AddressId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>AddressId for this Row</returns>
		public int my_AddressId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "AddressId"));
		}

		/// <summary>
		/// <see cref="clsCity.my_CityId">CityId</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCity.my_CityId">CityId for this row</see></returns>
		public int my_CityId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CityId"));
		}

		/// <summary>
		/// <see cref="clsCountry.my_CountryId">CountryId</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCountry.my_CountryId">CountryId for this row</see></returns>
		public int my_CountryId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CountryId"));
		}

		/// <summary>
		/// Whether this address is a POBoxType or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>POBoxType for this Row</returns>
		public int my_POBoxType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "POBoxType"));
		}

		/// <summary>
		/// UnitNumber
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>UnitNumber for this Row</returns>
		public string my_UnitNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "UnitNumber");
		}

		/// <summary>
		/// Number
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number for this Row</returns>
		public string my_Number(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Number");
		}

		/// <summary>
		/// BuildingName
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>BuildingName for this Row</returns>
		public string my_BuildingName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "BuildingName");
		}

		/// <summary>
		/// Street Address
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Street Address for this Row</returns>
		public string my_StreetAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "StreetAddress");
		}

		/// <summary>
		/// Suburb
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Suburb for this Row</returns>
		public string my_Suburb(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Suburb");
		}


		/// <summary>
		/// CityName of this Address
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CityName of Address for this Row</returns>
		public string my_CityName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CityName");
		}


		/// <summary>
		/// StateName of this Address
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>StateName of Address for this Row</returns>
		public string my_StateName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "StateName");
		}

		/// <summary>
		/// CountryName of this Address
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CountryName of Address for this Row</returns>
		public string my_CountryName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CountryName");
		}

		/// <summary>
		/// Post Code
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Post Code for this Row</returns>
		public string my_PostCode(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PostCode");
		}

		/// <summary>
		/// Quick Address
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Quick Address for this Row</returns>
		public string my_QuickAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickAddress");
		}

		/// <summary>
		/// AssocTableName of this Address
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>AssocTableName of Address for this Row</returns>
		public string my_AssocTableName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "AssocTableName");
		}

		/// <summary>
		/// AssocRowId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>AssocRowId for this Row</returns>
		public int my_AssocRowId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "AssocRowId"));
		}

		/// <summary>
		/// Address Type
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Type of Address for this Row</returns>
		public int my_AddressType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "AddressType"));
		}
		
		/// <summary>
		/// Address Type Description
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Description of Address Type for this Row</returns>
		public string my_AddressTypeDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "AddressTypeDescription");
		}

		#endregion

	}
}
