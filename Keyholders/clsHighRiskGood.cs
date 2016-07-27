using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;
using System.Collections;

namespace Keyholders
{
	/// <summary>
	/// clsHighRiskGood deals with everything to do with data about High Risk Goods.
	/// </summary>

	[GuidAttribute("D9E46F51-2EF0-4fa9-873D-82D149145CE0")]
	public class clsHighRiskGood : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsHighRiskGood
		/// </summary>
		public clsHighRiskGood() : base("HighRiskGood")
		{
		}

		/// <summary>
		/// Constructor for clsHighRiskGood; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsHighRiskGood(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("HighRiskGood")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Premise Information
		/// </summary>
		public clsQueryPart PremiseQ = new clsQueryPart();


		/// <summary>
		/// Part of the Query that Pertains to Premise Information
		/// </summary>
		public clsQueryPart[] distinctQueries;

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			ChangeDataQ = ChangeDataQueryPart();
			PremiseQ = PremiseQueryPart();
			MainQ = HighRiskGoodQueryPart();


			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[3];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = PremiseQ;
			baseQueries[2] = ChangeDataQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);

			distinctQueries = new clsQueryPart[2];
			
			distinctQueries[0] = HighRiskGoodQueryPart(true);
			distinctQueries[1] = PremiseQ;
			
			orderBySqlQuery = "Order By tblHighRiskGood.HighRiskGoodName" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsHighRiskGood
		/// </summary>
		public override void Initialise()
		{	
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("PremiseId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("HighRiskGoodName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("HighRiskGoodDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("SerialNumber", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("InformationType", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("MakeBrand", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Model", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PurchasePlace", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PurchaseDate", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PricePaid", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Status", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("StatusDate", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("InsuranceCompany", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("InsuranceClaimNumber", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PoliceFileNumber", System.Type.GetType("System.String"));
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
		/// Fields to search for stuff on
		/// </summary>
		public string SearchFields = " concat(tblHighRiskGood.HighRiskGoodName, ' ', tblHighRiskGood.SerialNumber, ' ', tblHighRiskGood.HighRiskGoodDescription) ";

		/// <summary>
		/// Local Representation of the class <see cref="clsHighRiskGood">clsHighRiskGood</see>
		/// </summary>
		public clsHighRiskGood thisHighRiskGood;

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all HighRiskGoods
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
		/// Gets an HighRiskGood by HighRiskGoodId
		/// </summary>
		/// <param name="HighRiskGoodId">Id of HighRiskGood to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByHighRiskGoodId(int HighRiskGoodId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ HighRiskGoodId.ToString() + crLf;

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
		/// Returns HighRiskGoods with the specified Name
		/// </summary>
		/// <param name="Name">Name of HighRiskGood</param>
		/// <returns>Number of HighRiskGoods with the specified Name</returns>
		public int GetByName(string Name)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable 
				+  " Where HighRiskGoodName " 
				+ MatchCondition(Name, matchCriteria.contains) + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

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
		/// Returns HighRiskGoods with the specified Name
		/// </summary>
		/// <param name="searchName">searchName of HighRiskGood</param>
		/// <param name="InformationType">InformationType of HighRiskGood</param>
		/// <returns>Number of HighRiskGoods with the specified Name</returns>
		public int GetByNameInformationType(string searchName, int InformationType)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = distinctQueries;
			
//			queries[0] = MainQ;
//			queries[0].SelectColumns.Clear();
//			queries[0].SelectColumns.Add(" Distinct tblHighRiskGood.PremiseId, tblHighRiskGood.");
//			queries[1] = PremiseQ;

			#region Sort out searchname into distinct words

			int nextSpace = searchName.IndexOf(" ");
			ArrayList SearchTerms = new ArrayList();
			int lastSpace = 0;
			string thisTerm = "";

			while (nextSpace != -1)
			{
				thisTerm = searchName.Substring(lastSpace,nextSpace - lastSpace).Trim();
				SearchTerms.Add(thisTerm);
				lastSpace = nextSpace;
				nextSpace = searchName.IndexOf(" ",lastSpace + 1);
			}

			thisTerm = searchName.Substring(lastSpace).Trim();
			SearchTerms.Add(thisTerm);
	
			string condition = "(";
			bool firstEntry = true;
			foreach(object thisSearchobj in SearchTerms)
			{
				if (firstEntry)
					firstEntry = false;
				else 
					condition += " And ";
				condition += SearchFields
					+ MatchCondition(thisSearchobj.ToString(), matchCriteria.contains) + crLf;
			}
			#endregion

			condition = "(Select * from " + thisTable 
				+  " Where " + crLf
				+ ArchiveConditionIfNecessary(false) + crLf
				+ "And " + condition + ")" + crLf
				;

			if (InformationType > 0)
				condition += "And tblHighRiskGood.InformationType = " + InformationType.ToString() + crLf
					;

			condition += ") " + thisTable;

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
		/// Gets High Risk Goods by PremiseId
		/// </summary>
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
		/// Gets High Risk Goods by PremiseId
		/// </summary>
		/// <param name="PremiseId">PremiseId</param>
		/// <param name="InformationType">InformationType</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPremiseIdInformationType(int PremiseId, int InformationType)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".PremiseId = "
				+ PremiseId.ToString() + crLf
				+ " And " + thisTable + ".InformationType = "
				+ InformationType.ToString() + crLf
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

		
		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the SaveCustomers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PremiseId">Premise Associated with this High Risk Good</param>
		/// <param name="HighRiskGoodName">Name of this High Risk Good</param>
		/// <param name="HighRiskGoodDescription">Description of this High Risk Good</param>
		/// <param name="SerialNumber">High Risk Good's Serial Number</param>
		/// <param name="InformationType">Type of High Risk Good</param>
		/// <param name="MakeBrand">MakeBrand</param>
		/// <param name="Model">Model</param>
		/// <param name="PurchasePlace">PurchasePlace</param>
		/// <param name="PurchaseDate">PurchaseDate</param>
		/// <param name="PricePaid">PricePaid</param>
		/// <param name="Status">Status</param>
		/// <param name="StatusDate">StatusDate</param>
		/// <param name="InsuranceCompany">InsuranceCompany</param>
		/// <param name="InsuranceClaimNumber">InsuranceClaimNumber</param>
		/// <param name="PoliceFileNumber">PoliceFileNumber</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Add(int PremiseId, 
			string HighRiskGoodName,
			string HighRiskGoodDescription,
			string SerialNumber,
			int InformationType,
			string MakeBrand,
			string Model,
			string PurchasePlace,
			string PurchaseDate,
			string PricePaid,
			int Status,
			string StatusDate,
			string InsuranceCompany,
			string InsuranceClaimNumber,
			string PoliceFileNumber,
			int CurrentUser)
		{


			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			AddGeneral(PremiseId,
				HighRiskGoodName,
				HighRiskGoodDescription,
				SerialNumber,
				InformationType,
				MakeBrand,
				Model,
				PurchasePlace,
				PurchaseDate,
				PricePaid,
				Status,
				StatusDate,
				InsuranceCompany,
				InsuranceClaimNumber,
				PoliceFileNumber,
				thisChangeData.LastIdAdded(),
				0);
			
		}
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal HighRiskGood table stack; the SaveHighRiskGoods method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PremiseId">Premise Associated with this High Risk Good</param>
		/// <param name="HighRiskGoodName">Name of this High Risk Good</param>
		/// <param name="HighRiskGoodDescription">Description of this High Risk Good</param>
		/// <param name="SerialNumber">High Risk Good's Serial Number</param>
		/// <param name="InformationType">Type of High Risk Good</param>
		/// <param name="MakeBrand">MakeBrand</param>
		/// <param name="Model">Model</param>
		/// <param name="PurchasePlace">PurchasePlace</param>
		/// <param name="PurchaseDate">PurchaseDate</param>
		/// <param name="PricePaid">PricePaid</param>
		/// <param name="Status">Status</param>
		/// <param name="StatusDate">StatusDate</param>
		/// <param name="InsuranceCompany">InsuranceCompany</param>
		/// <param name="InsuranceClaimNumber">InsuranceClaimNumber</param>
		/// <param name="PoliceFileNumber">PoliceFileNumber</param>		
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		private void AddGeneral(int PremiseId, 
			string HighRiskGoodName,
			string HighRiskGoodDescription,
			string SerialNumber,
			int InformationType,
			string MakeBrand,
			string Model,
			string PurchasePlace,
			string PurchaseDate,
			string PricePaid,
			int Status,
			string StatusDate,
			string InsuranceCompany,
			string InsuranceClaimNumber,
			string PoliceFileNumber,
			int ChangeDataId,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["PremiseId"] = PremiseId;
			rowToAdd["HighRiskGoodName"] = HighRiskGoodName;
			rowToAdd["HighRiskGoodDescription"] = HighRiskGoodDescription;
			rowToAdd["SerialNumber"] = SerialNumber;
			rowToAdd["InformationType"] = InformationType;
			rowToAdd["MakeBrand"] = MakeBrand;
			rowToAdd["Model"] = Model;
			rowToAdd["PurchasePlace"] = PurchasePlace;
			rowToAdd["PurchaseDate"] = SanitiseDate(PurchaseDate);
			rowToAdd["PricePaid"] = PricePaid;
			rowToAdd["Status"] = Status;
			rowToAdd["StatusDate"] = SanitiseDate(StatusDate);
			rowToAdd["InsuranceCompany"] = InsuranceCompany;
			rowToAdd["InsuranceClaimNumber"] = InsuranceClaimNumber;
			rowToAdd["PoliceFileNumber"] = PoliceFileNumber;
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
			thisHighRiskGood = new clsHighRiskGood(thisDbType, localRecords.dbConnection);
			thisHighRiskGood.GetByHighRiskGoodId(thisPkId);

			AddGeneral(thisHighRiskGood.my_PremiseId(0),
				thisHighRiskGood.my_HighRiskGoodName(0),
				thisHighRiskGood.my_HighRiskGoodDescription(0),
				thisHighRiskGood.my_SerialNumber(0),
				thisHighRiskGood.my_InformationType(0),
				thisHighRiskGood.my_MakeBrand(0),
				thisHighRiskGood.my_Model(0),
				thisHighRiskGood.my_PurchasePlace(0),
				thisHighRiskGood.my_PurchaseDate(0),
				thisHighRiskGood.my_PricePaid(0),
				thisHighRiskGood.my_Status(0),
				thisHighRiskGood.my_StatusDate(0),
				thisHighRiskGood.my_InsuranceCompany(0),
				thisHighRiskGood.my_InsuranceClaimNumber(0),
				thisHighRiskGood.my_PoliceFileNumber(0),
				thisHighRiskGood.my_ChangeDataId(0), 
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisHighRiskGood.my_ChangeData_CreatedByUserId(0),
				thisHighRiskGood.my_ChangeData_CreatedByFirstNameLastName(0),
				thisHighRiskGood.my_ChangeData_DateCreated(0),
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
		/// <param name="HighRiskGoodId">HighRiskGoodId; Unique Id for this table</param>
		/// <param name="PremiseId">Premise Associated with this High Risk Good</param>
		/// <param name="HighRiskGoodName">Name of this High Risk Good</param>
		/// <param name="HighRiskGoodDescription">Description of this High Risk Good</param>
		/// <param name="SerialNumber">High Risk Good's Serial Number</param>
		/// <param name="InformationType">Type of High Risk Good</param>
		/// <param name="MakeBrand">MakeBrand</param>
		/// <param name="Model">Model</param>
		/// <param name="PurchasePlace">PurchasePlace</param>
		/// <param name="PurchaseDate">PurchaseDate</param>
		/// <param name="PricePaid">PricePaid</param>
		/// <param name="Status">Status</param>
		/// <param name="StatusDate">StatusDate</param>
		/// <param name="InsuranceCompany">InsuranceCompany</param>
		/// <param name="InsuranceClaimNumber">InsuranceClaimNumber</param>
		/// <param name="PoliceFileNumber">PoliceFileNumber</param>		
		/// <param name="CurrentUser">CurrentUser</param>
		public void Modify(int HighRiskGoodId, 
			int PremiseId, 
			string HighRiskGoodName,
			string HighRiskGoodDescription,
			string SerialNumber,
			int InformationType,
			string MakeBrand,
			string Model,
			string PurchasePlace,
			string PurchaseDate,
			string PricePaid,
			int Status,
			string StatusDate,
			string InsuranceCompany,
			string InsuranceClaimNumber,
			string PoliceFileNumber,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, HighRiskGoodId);

			rowToAdd["HighRiskGoodId"] = HighRiskGoodId;

			rowToAdd["PremiseId"] = PremiseId;
			rowToAdd["HighRiskGoodName"] = HighRiskGoodName;
			rowToAdd["HighRiskGoodDescription"] = HighRiskGoodDescription;
			rowToAdd["SerialNumber"] = SerialNumber;
			rowToAdd["InformationType"] = InformationType;
			rowToAdd["MakeBrand"] = MakeBrand;
			rowToAdd["Model"] = Model;
			rowToAdd["PurchasePlace"] = PurchasePlace;
			rowToAdd["PurchaseDate"] = SanitiseDate(PurchaseDate);
			rowToAdd["PricePaid"] = PricePaid;
			rowToAdd["Status"] = Status;
			rowToAdd["StatusDate"] = SanitiseDate(StatusDate);
			rowToAdd["InsuranceCompany"] = InsuranceCompany;
			rowToAdd["InsuranceClaimNumber"] = InsuranceClaimNumber;
			rowToAdd["PoliceFileNumber"] = PoliceFileNumber;
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

		# region My_ Values HighRiskGood

		/// <summary>
		/// HighRiskGoodId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>HighRiskGoodId for this Row</returns>
		public int my_HighRiskGoodId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "HighRiskGoodId"));
		}


		/// <summary>
		/// <see cref="clsPremise.my_PremiseId">Id</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_PremiseId">Id</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_PremiseId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PremiseId"));
		}

		/// <summary>
		/// High Risk Good Name
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_HighRiskGoodName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGoodName");
		}

		/// <summary>
		/// High Risk Good Description
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Description of HighRiskGood for this Row</returns>
		public string my_HighRiskGoodDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "HighRiskGoodDescription");
		}

		/// <summary>
		/// SerialNumber
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_SerialNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "SerialNumber");
		}

		/// <summary>
		/// Information Type of this HighRiskGood
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Information Type of this HighRiskGood
		/// </returns>
		public int my_InformationType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "InformationType"));
		}



		/// <summary>
		/// MakeBrand
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_MakeBrand(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "MakeBrand");
		}


		/// <summary>
		/// Model
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_Model(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Model");
		}


		/// <summary>
		/// PurchasePlace
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_PurchasePlace(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PurchasePlace");
		}


		/// <summary>
		/// PurchaseDate
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_PurchaseDate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PurchaseDate");
		}


		/// <summary>
		/// PricePaid
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_PricePaid(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PricePaid");
		}

		
		/// <summary>
		/// Status
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Status for this Row</returns>
		public int my_Status(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Status"));
		}

		/// <summary>
		/// StatusDate
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_StatusDate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "StatusDate");
		}


		/// <summary>
		/// InsuranceCompany
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_InsuranceCompany(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "InsuranceCompany");
		}


		/// <summary>
		/// InsuranceClaimNumber
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_InsuranceClaimNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "InsuranceClaimNumber");
		}


		/// <summary>
		/// PoliceFileNumber
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Data  for this Row</returns>
		public string my_PoliceFileNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PoliceFileNumber");
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
	}
}
