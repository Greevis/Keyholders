using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;
using System.Collections;

namespace Keyholders
{
	/// <summary>
	/// clsPersonPremiseRole deals with everything to do with data about PersonPremiseRoles.
	/// </summary>

	
	[GuidAttribute("BE4F7FA3-43C7-4b63-8DC1-1319C162A42A")]
	public class clsPersonPremiseRole : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsPersonPremiseRole
		/// </summary>
		public clsPersonPremiseRole() : base("PersonPremiseRole")
		{
		}

		/// <summary>
		/// Constructor for clsPersonPremiseRole; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsPersonPremiseRole(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("PersonPremiseRole")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to Premise Information
		/// </summary>
		public clsQueryPart PremiseQ = new clsQueryPart();
		
		/// <summary>
		/// Part of the Query that Pertains to Person Information
		/// </summary>
		public clsQueryPart PersonQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Person1Q Information
		/// </summary>
		public clsQueryPart Person1Q = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Customer Information
		/// </summary>
		public clsQueryPart CustomerQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Main1Q Information
		/// </summary>
		public clsQueryPart Main1Q = new clsQueryPart();

		/// <summary>
		/// baseQueriesNoChangeData
		/// </summary>
		public clsQueryPart[] baseQueriesNoChangeData;
		/// <summary>
		/// baseQueriesDistinctPeople
		/// </summary>
		public clsQueryPart[] baseQueriesDistinctPeople;


		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = PersonPremiseRoleQueryPart();
			PersonQ = PersonQueryPart();
			ChangeDataQ = ChangeDataQueryPart();

			CustomerQ = CustomerQueryPart();
			PremiseQ = PremiseQueryPart();
			CustomerQ.Joins.Clear();
			CustomerQ.Joins.Add("tblPerson.CustomerId = tblCustomer.CustomerId");


			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[4];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = PersonQ;
			baseQueries[2] = PremiseQ;
			baseQueries[3] = ChangeDataQ;

			baseQueriesNoChangeData = new clsQueryPart[4]; 

			baseQueriesNoChangeData[0] = MainQ;
			baseQueriesNoChangeData[1] = PersonQ;
			baseQueriesNoChangeData[2] = PremiseQ;
			baseQueriesNoChangeData[3] = CustomerQ;

			baseQueriesDistinctPeople = new clsQueryPart[3]; 

			Main1Q = PersonPremiseRoleQueryPart();
			Main1Q.SelectColumns.Clear();

			Person1Q = PersonQueryPart(true);

			baseQueriesDistinctPeople[0] = Main1Q;
			baseQueriesDistinctPeople[1] = Person1Q;
			baseQueriesDistinctPeople[2] = CustomerQ;
			
			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			orderBySqlQuery = "Order By PersonPremiseRoleType, Priority" + crLf;

		}


		/// <summary>
		/// Initialise (or reinitialise) everything for clsPersonPremiseRole
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("PersonPremiseRoleType", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PremiseId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PersonId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("Priority", System.Type.GetType("System.Int32"));
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
		/// Local Representation of the class <see cref="clsPersonPremiseRole">clsPersonPremiseRole</see>
		/// </summary>
		public clsPersonPremiseRole thisPersonPremiseRole;
		
		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all PersonPremiseRoles
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
		/// Gets a PersonPremiseRole by PersonPremiseRoleId
		/// </summary>
		/// <param name="PersonPremiseRoleId">Id of PersonPremiseRole to get</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonPremiseRoleId(int PersonPremiseRoleId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

		
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ PersonPremiseRoleId.ToString() + crLf;

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
		/// Gets PersonPremiseRoles by 
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see>
		/// </summary>
		/// <param name="PremiseId"><see cref="clsPremise.my_PremiseId">PremiseId</see></param>
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
		/// Gets PersonPremiseRoles for a Person
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonId(int PersonId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

		
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".PersonId = "
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
		/// Gets PersonPremiseRoles by 
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see>
		/// and 
		/// <see cref="clsKeyBase.personPremiseRoleType">
		/// PersonPremiseRoleType</see>
		/// </summary>
		/// <param name="PremiseId">
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see></param>
		/// <param name="RoleType">
		/// <see cref="clsKeyBase.personPremiseRoleType">
		/// PersonPremiseRoleType</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetByPremiseIdPersonPremiseRoleType(int PremiseId, int RoleType)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
		
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".PremiseId = "
				+ PremiseId.ToString() + crLf
				+ " And " + thisTable + ".PersonPremiseRoleType = "
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
		/// Gets PersonPremiseRoles by 
		/// <see cref="clsPerson.my_PersonId">PersonId</see>
		/// and 
		/// <see cref="clsKeyBase.personPremiseRoleType">
		/// PersonPremiseRoleType</see>
		/// </summary>
		/// <param name="PersonId">
		/// <see cref="clsPerson.my_PersonId">PersonId</see></param>
		/// <param name="RoleType">
		/// <see cref="clsKeyBase.personPremiseRoleType">
		/// PersonPremiseRoleType</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonIdPersonPremiseRoleType(int PersonId, int RoleType)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
		
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".PersonId = "
				+ PersonId.ToString() + crLf
				+ " And " + thisTable + ".PersonPremiseRoleType = "
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
		/// Gets PersonPremiseRoles by 
		/// <see cref="clsCustomer.my_CustomerId">CustomerId</see>
		/// and 
		/// <see cref="clsKeyBase.personPremiseRoleType">
		/// PersonPremiseRoles</see>
		/// </summary>
		/// <param name="CustomerId">
		/// <see cref="clsCustomer.my_CustomerId">CustomerId</see></param>
		/// <param name="RoleType">
		/// <see cref="clsKeyBase.personPremiseRoleType">
		/// PersonPremiseRoleType</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerIdPersonPremiseRoleType(int CustomerId, int RoleType)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;


		
			string condition1 = "(Select * from tblPerson " + crLf 
				+ " Where tblPerson.CustomerId = "
				+ CustomerId.ToString() + crLf
				+ ") tblPerson";

			string condition2 = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".PersonPremiseRoleType = "
				+ RoleType.ToString() + crLf;

			condition2 += ArchiveConditionIfNecessary(true);

			condition2 += ") " + thisTable;


			clsQueryBuilder.ConditionWithTable[] thisConditions = new Resources.clsQueryBuilder.ConditionWithTable[2];
			thisConditions[0].condition = condition1;
			thisConditions[0].table = "tblPerson";
			thisConditions[1].condition = condition2;
			thisConditions[1].table = thisTable;

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
		/// Checks for the existance PersonPremiseRoles by 
		/// <see cref="clsPerson.my_PersonId">PersonId</see>
		/// at 
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see>
		/// </summary>
		/// <param name="PersonId">
		/// <see cref="clsPerson.my_PersonId">PersonId</see></param>
		/// <param name="PremiseId">
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see></param>		
		/// <returns>Number of resulting records</returns>
		public int GetByPersonIdPremiseId(int PersonId, int PremiseId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
		
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".PersonId = "
				+ PersonId.ToString() + crLf
				+ " And " + thisTable + ".PremiseId = "
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
		/// Checks for the existance PersonPremiseRoles by 
		/// <see cref="clsPerson.my_PersonId">PersonId</see>
		/// at 
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see>
		/// of type 
		/// <see cref="clsKeyBase.personPremiseRoleType">
		/// PersonPremiseRoleType</see>
		/// </summary>
		/// <param name="PersonId">
		/// <see cref="clsPerson.my_PersonId">PersonId</see></param>
		/// <param name="PremiseId"><see cref="clsPremise.my_PremiseId">PremiseId</see></param>
		/// <param name="RoleType">
		/// <see cref="clsKeyBase.personPremiseRoleType">
		/// PersonPremiseRoleType</see></param>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonIdPremiseIdPPRType(int PersonId, int PremiseId, int RoleType)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
		
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".PersonId = "
				+ PersonId.ToString() + crLf
				+ " And " + thisTable + ".PremiseId = "
				+ PremiseId.ToString() + crLf
				+ " And " + thisTable + ".PersonPremiseRoleType = "
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
		/// Checks for the existance PersonPremiseRoles by 
		/// <see cref="clsPerson.my_PersonId">PersonId</see>
		/// at 
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see>
		/// of type 
		/// <see cref="clsKeyBase.personPremiseRoleType">
		/// PersonPremiseRoleType</see>
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetMaxPriorityForPremise(int PremiseId)
		{
			string thisSqlQuery = "Select Max(Priority) as Priority " + crLf
				+ " From " + thisTable 
				+ " Where " + thisTable + ".PremiseId = "
				+ PremiseId.ToString() + crLf
				;

			thisSqlQuery += ArchiveConditionIfNecessary(true);

			int retVal = localRecords.GetRecords(thisSqlQuery);
			if (retVal > 0)
				return my_Priority(0);
			else 
				return 0;
		}

		/// <summary>
		/// Gets all People who are Keyholders for a certain Premise
		/// </summary>
		/// <param name="PremiseId">Premise in Question</param>
		/// <returns>Number of resulting records</returns>
		public int GetKeyholdersForPremiseId(int PremiseId)
		{
			return GetByPremiseIdPersonPremiseRoleType(PremiseId, personPremiseRoleType_keyHolder());
		}


		/// <summary>
		/// GetAllDistintPeopleWhoNeedGetCorrespondence
		/// </summary>
		/// <returns></returns>
		public int GetAllDistintPeopleWhoNeedGetCorrespondence()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueriesDistinctPeople;
		
			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".PersonPremiseRoleType != "
				+  personPremiseRoleType_keyHolder().ToString() + crLf;

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

		#region GetPendingCorrespondence

		/// <summary>
		/// Gets Pending Correpondence for each Person in a Role for a Premise
		/// </summary>
		/// <returns></returns>
		public int GetPendingCorrespondence()
		{
			//Gets the Required Correspondence to send, i.e.
			// Premise Details
			// Customer Details
			// Details of the Person to send to
			// Mechanism of sending

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueriesNoChangeData;

			queries[0].SelectColumns.Add("case when DetailsUpdateRequired = 1 " + crLf
				+ "then tblPerson.PreferredContactMethod"  + crLf
				+ "else 0 " + crLf
				+ "end as CorrespondenceMedium");

			queries[0].SelectColumns.Add("case when (DetailsUpdateRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.detailsManager).ToString() + ") " + crLf
				+ "then " + Convert.ToInt32(correspondenceType.detailsUpdate).ToString() + crLf
				+ "else 0 " + crLf
				+ "end as Correspondence_CorrespondenceTypeId");

			queries[2].AddJoin("(DetailsUpdateRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.detailsManager).ToString() + ") ");


			queries[2].AddJoin("(case when (DetailsUpdateRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.detailsManager).ToString() + ") " + crLf
				+ "then " + Convert.ToInt32(correspondenceType.detailsUpdate).ToString() + crLf
				+ "else 0 " + crLf
				+ "end > 0)");

			//queries[0].SelectColumns.Add("case when StickerRequired = 1" + crLf
			//	+ "then " + Convert.ToInt32(correspondenceMedium.mail).ToString() + crLf
			//	+ "when InvoiceRequired = 1 OR StatementRequired = 1 OR DetailsUpdateRequired = 1 OR CopyOfInvoiceRequired = 1 " + crLf
			//	+ "then tblPerson.PreferredContactMethod" + crLf
			//	+ "else 0 " + crLf
			//	+ "end as CorrespondenceMedium");

			//queries[0].SelectColumns.Add("case when (StickerRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.billingContact).ToString() + ") " + crLf
			//	+ "then " + Convert.ToInt32(correspondenceType.stickerRequired).ToString() + crLf
			//	+ "when (InvoiceRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.billingContact).ToString() + ") " + crLf
			//	+ "then " + Convert.ToInt32(correspondenceType.invoice).ToString()  + crLf
			//	+ "when (CopyOfInvoiceRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.billingContact).ToString() + ") " + crLf
			//	+ "then " + Convert.ToInt32(correspondenceType.copyOfInvoice).ToString()  + crLf
			//	+ "when (StatementRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.billingContact).ToString() + ") " + crLf
			//	+ "then " + Convert.ToInt32(correspondenceType.statement).ToString()  + crLf
			//	+ "when (DetailsUpdateRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.detailsManager).ToString() + ") " + crLf
			//	+ "then " + Convert.ToInt32(correspondenceType.detailsUpdate).ToString()  + crLf
			//	+ "else 0 " + crLf
			//	+ "end as Correspondence_CorrespondenceTypeId");


			//queries[2].AddJoin("((StickerRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.detailsManager).ToString() + ") " + crLf
			//	+ " OR " + "(InvoiceRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.billingContact).ToString() + ") " + crLf
			//	+ " OR " + "(CopyOfInvoiceRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.billingContact).ToString() + ") " + crLf
			//	+ " OR " + "(StatementRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.billingContact).ToString() + ") " + crLf
			//	+ " OR " + "(DetailsUpdateRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.detailsManager).ToString() + ")) ");


			//Adding to Get CorrespondenceId

			//queries[2].AddJoin("(case when (StickerRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.detailsManager).ToString() + ") " + crLf
			//	+ "then " + Convert.ToInt32(correspondenceType.stickerRequired).ToString() + crLf
			//	+ "when (InvoiceRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.billingContact).ToString() + ") " + crLf
			//	+ "then " + Convert.ToInt32(correspondenceType.invoice).ToString()  + crLf
			//	+ "when (CopyOfInvoiceRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.billingContact).ToString() + ") " + crLf
			//	+ "then " + Convert.ToInt32(correspondenceType.copyOfInvoice).ToString()  + crLf
			//	+ "when (StatementRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.billingContact).ToString() + ") " + crLf
			//	+ "then " + Convert.ToInt32(correspondenceType.statement).ToString()  + crLf
			//	+ "when (DetailsUpdateRequired = 1 and PersonPremiseRoleType = " + Convert.ToInt32(personPremiseRoleType.detailsManager).ToString() + ") " + crLf
			//	+ "then " + Convert.ToInt32(correspondenceType.detailsUpdate).ToString()  + crLf
			//	+ "else 0 " + crLf
			//	+ "end > 0)");


			queries[2].Joins.Add("tblPersonPremiseRole.Archive = 0");
			queries[2].Joins.Add("tblCustomer.Archive = 0");
			queries[2].Joins.Add("tblPerson.Archive = 0");
			queries[2].Joins.Add("tblPremise.Archive = 0");

			string condition = "(Select * from " + thisTable + crLf;

			condition += "Where " + ArchiveConditionIfNecessary(false);

			condition += ") " + thisTable;

			OrderByColumns.Clear();

			thisSqlQuery = QB.BuildSqlStatement(
				queries,
				OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
//			if (OrderByColumns.Count == 0)
//				thisSqlQuery += " Order by tblCustomer.CustomerId, tblPerson.PersonId, CorrespondenceMedium";
			thisSqlQuery += " Order by convert(tblCustomer.AccountNumber, signed), tblPerson.PersonId, tblPersonPremiseRole.PersonPremiseRoleType != 2";



			return localRecords.GetRecords(thisSqlQuery);

		}


		#endregion

		#region ResetPremiseCorrepondence

		/// <summary>
		/// Resets Correspondence fields (StickerRequired, InvoiceRequired, DetailsRequired, StatementRequired) for a Premise
		/// </summary>
		/// <param name="PremiseId">Id of Premise to Reset Correspondence Fields for</param>
		public void ResetPremiseCorrepondence(int PremiseId)
		{
			clsPremise thisPremise = new clsPremise(thisDbType, localRecords.dbConnection);
			thisPremise.ResetPremiseCorrepondence(PremiseId);
			thisPremise.Save();
		}

		#endregion
		
		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the SaveCustomers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PersonPremiseRoleType">Type of Person Premise Role</param>
		/// <param name="PremiseId">Premise Associated with this Person Premise Role</param>
		/// <param name="PersonId">Person Associated with this Person Premise Role</param>
		/// <param name="Priority">The Priority in which a Keyholder should be called 
		/// in the event of an emergency. Not relevant for other Role Types</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Add(int PremiseId, 
			int PersonId,
			int PersonPremiseRoleType,
			int Priority,
			int CurrentUser)
		{

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			AddGeneral(PremiseId,
				PersonId,
				PersonPremiseRoleType,
				Priority,
				thisChangeData.LastIdAdded(),
				0);
			
		}
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal PersonPremiseRole table stack; the SavePersonPremiseRoles method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PremiseId">PremiseId</param>
		/// <param name="PersonId">PersonId</param>
		/// <param name="PersonPremiseRoleType">PersonPremiseRoleType</param>
		/// <param name="Priority">Priority</param>
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		private void AddGeneral(int PremiseId,
			int PersonId,
			int PersonPremiseRoleType,
			int Priority,
			int ChangeDataId,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["PersonPremiseRoleType"] = PersonPremiseRoleType;
			rowToAdd["PremiseId"] = PremiseId;
			rowToAdd["PersonId"] = PersonId;

			if (Priority == 0 && PersonPremiseRoleType == personPremiseRoleType_keyHolder())
			{
				//Find the number of Keyholders for this Premise
				clsPersonPremiseRole thisPPR = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);
				int numKeyholders = thisPPR.GetMaxPriorityForPremise(PremiseId);
				rowToAdd["Priority"] = thisPPR.my_Priority(0) + 1;
			}
			else
				rowToAdd["Priority"] = Priority;
			
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
			thisPersonPremiseRole = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);
			thisPersonPremiseRole.GetByPersonPremiseRoleId(thisPkId);

			AddGeneral(thisPersonPremiseRole.my_PremiseId(0),
				thisPersonPremiseRole.my_PersonId(0),
				thisPersonPremiseRole.my_PersonPremiseRoleType(0),
				thisPersonPremiseRole.my_Priority(0),
				thisPersonPremiseRole.my_ChangeDataId(0), 
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisPersonPremiseRole.my_ChangeData_CreatedByUserId(0),
				thisPersonPremiseRole.my_ChangeData_CreatedByFirstNameLastName(0),
				thisPersonPremiseRole.my_ChangeData_DateCreated(0),
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
		/// <param name="PersonPremiseRoleId">PersonPremiseRoleId (Primary Key of Record)</param>
		/// <param name="PersonPremiseRoleType">Type of Person Premise Role</param>
		/// <param name="PremiseId">Premise Associated with this Person Premise Role</param>
		/// <param name="PersonId">Person Associated with this Person Premise Role</param>
		/// <param name="Priority">The Priority in which a Keyholder should be called 
		/// in the event of an emergency. Not relevant for other Role Types</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Modify(int PersonPremiseRoleId, 
			int PremiseId, 
			int PersonId,
			int Priority,
			int PersonPremiseRoleType,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, PersonPremiseRoleId);

			rowToAdd["PersonPremiseRoleId"] = PersonPremiseRoleId;
			rowToAdd["PersonPremiseRoleType"] = PersonPremiseRoleType;
			rowToAdd["PremiseId"] = PremiseId;
			rowToAdd["PersonId"] = PersonId;
			rowToAdd["Priority"] = Priority;
			rowToAdd["Archive"] = 0;

			Validate(rowToAdd, false);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);
			}

		}


		#endregion

		#region Sets, Reindex and Remove

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
		/// <param name="PremiseId">Premise Associated with this PersonPremiseRole</param>
		/// <param name="PersonId">Person Associated with this PersonPremiseRole</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void RemoveKeyholder(int PremiseId, 
			int PersonId,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Find this PersonPremiseRoleId
			clsPersonPremiseRole thisPPR = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);

			int numRecords = thisPPR.GetByPersonIdPremiseIdPPRType(PersonId, PremiseId, personPremiseRoleType_keyHolder());

			for(int counter = 0; counter < numRecords; counter++)
				thisPPR.Remove(thisPPR.my_PersonPremiseRoleId(counter), CurrentUser);

			thisPPR.Save();

			ReindexKeyholders(PremiseId, CurrentUser);
		}

		/// <summary>
		/// Reindexes Keyholders for a Premise
		/// </summary>
		/// <param name="PremiseId">Premise Associated with this PersonPremiseRole</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void ReindexKeyholders(int PremiseId, int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Find this PersonPremiseRoleId
			clsPersonPremiseRole thisPPR = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);

			int numRecords = thisPPR.GetKeyholdersForPremiseId(PremiseId);

			for(int counter = 0; counter < numRecords; counter++)
				if (thisPPR.my_Priority(counter) != counter + 1)
				{
					thisPPR.Modify(thisPPR.my_PersonPremiseRoleId(counter),
						thisPPR.my_PremiseId(counter),
						thisPPR.my_PersonId(counter),
						(counter + 1),
						thisPPR.my_PersonPremiseRoleType(counter),
						CurrentUser);
					thisPPR.Save();
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
		/// <param name="PremiseId">Premise Associated with this PersonPremiseRole</param>
		/// <param name="PersonId">Person Associated with this PersonPremiseRole</param>
		/// <param name="PersonPremiseRoleType">Type of Person Premise Role</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void Set(int PremiseId, 
			int PersonId,
			int PersonPremiseRoleType,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Find this PersonPremiseRoleId
			clsPersonPremiseRole thisPPR = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);

			int numRecords;
			switch((personPremiseRoleType) PersonPremiseRoleType)
			{
					//There can be many Keyholders;
					//If this Person is already a Keyholder for this Premise, do nothing - otherwise add them at the end of the 
					//Keyholder Priority List
				case personPremiseRoleType.keyHolder:
					numRecords = thisPPR.GetByPersonIdPremiseIdPPRType(PersonId, PremiseId, PersonPremiseRoleType);

					if (numRecords == 0)
					{
						int thisMaxPriority = thisPPR.GetMaxPriorityForPremise(PremiseId);
						thisPPR.Add(PremiseId,
							PersonId,
							PersonPremiseRoleType,
							thisPPR.my_Priority(0) + 1,
							CurrentUser);
						thisPPR.Save();
					}

					break;
				case personPremiseRoleType.billingContact:
					//There can be one Billing Contact only per _Customer_
					//If there are other Billing Contacts, Archive them
					clsPerson thisPerson = new clsPerson(thisDbType, localRecords.dbConnection);

					int numPeople = thisPerson.GetByPersonId(PersonId);
					if (numPeople > 0)
					{
						int thisCustomerId = thisPerson.my_CustomerId(0);
			
						//Cycle through all Premises and Ensure this person is the billing Contact for all Premises

						clsPremise thisPremise = new clsPremise(thisDbType, localRecords.dbConnection);
						int numPremise = thisPremise.GetByCustomerId(thisCustomerId);
						for(int premiseCounter = 0; premiseCounter < numPremise; premiseCounter++)
						{
							int thisPremiseId = thisPremise.my_PremiseId(premiseCounter);

							//Get all Billing Contacts for this Premise
							clsPersonPremiseRole theseBillingContacts = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);
							int numbillingContacts = theseBillingContacts.GetByPremiseIdPersonPremiseRoleType(thisPremiseId, personPremiseRoleType_billingContact());
							bool ContactForThisPersonFound = false;

							for(int billingContactCounter = 0; billingContactCounter < numbillingContacts; billingContactCounter++)
							{
								if (theseBillingContacts.my_PersonId(billingContactCounter) == PersonId)
									ContactForThisPersonFound = true;
								else
								{
									thisPPR.Remove(theseBillingContacts.my_PersonPremiseRoleId(billingContactCounter), CurrentUser);
									thisPPR.Save();
								}							
							}

							if (!ContactForThisPersonFound)
							{
								thisPPR.Add(thisPremiseId,
									PersonId,
									personPremiseRoleType_billingContact(), 
									0,
									CurrentUser);
								thisPPR.Save();
							}

						}


					}
					break;			
				
				case personPremiseRoleType.detailsManager:
				case personPremiseRoleType.daytimeContact:
				default:
					//There can be only one Details Manager and Daytime Contact per _Premise_
					//If there are others for this Premise, Archive them
					numRecords = thisPPR.GetByPremiseIdPersonPremiseRoleType(PremiseId, PersonPremiseRoleType);
					bool changeRequired = true;
					for(int counter = 0; counter < numRecords; counter++)
					{
						if (thisPPR.my_PersonId(counter) == PersonId)
							changeRequired = false;
						else
						{
							thisPPR.Remove(thisPPR.my_PersonPremiseRoleId(counter), CurrentUser);
							thisPPR.Save();
						}
					}

					if (changeRequired)
					{
						thisPPR.Add(PremiseId,
							PersonId,
							PersonPremiseRoleType,
							0,
							CurrentUser);

						thisPPR.Save();
					}
					break;
			}

		}

		/// <summary>
		/// Sets a Person's 'Priority' throughout their roles. This is the order their information
		/// is presented in the details updates
		/// </summary>
		/// <param name="PremiseId">Premise Associated with these PersonPremiseRoles</param>
		/// <param name="PersonId">Person Associated with these PersonPremiseRoles</param>
		/// <param name="Priority">Priority</param>
		/// <param name="CurrentUser">CurrentUser</param>
		public void SetPriority(int PremiseId, 
			int PersonId,
			int Priority,
			int CurrentUser)
		{
			//Find this PersonPremiseRoleId
			clsPersonPremiseRole thisPPR = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);

			int numRecords = thisPPR.GetByPersonIdPremiseIdPPRType(PersonId, PremiseId, personPremiseRoleType_keyHolder());

			switch (numRecords)
			{
				case 0:
					thisPPR.Add(PremiseId,
						PersonId,
						personPremiseRoleType_keyHolder(),
						Priority,
						CurrentUser);
					thisPPR.Save();
					break;
				default:
					thisPPR.Modify(thisPPR.my_PersonPremiseRoleId(0),
						thisPPR.my_PremiseId(0),
						thisPPR.my_PersonId(0),
						Priority,
						personPremiseRoleType_keyHolder(),
						CurrentUser);
					thisPPR.Save();

					for (int counter = 1; counter < numRecords; counter++)
					{
						thisPPR.Remove(thisPPR.my_PersonPremiseRoleId(counter), CurrentUser);
							thisPPR.Save();
					}
					break;
			}

//			thisPPR.ReindexKeyholders(PremiseId, CurrentUser);
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

		#region My_ Values for Correspondence run

		/// <summary>
		/// CorrespondenceMedium
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CorrespondenceMedium</returns>
		public int my_CorrespondenceMedium(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CorrespondenceMedium"));
		}

		/// <summary>
		/// Correspondence_CorrespondenceTypeId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>TCorrespondence_CorrespondenceTypeId</returns>
		public int my_Correspondence_CorrespondenceTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Correspondence_CorrespondenceTypeId"));
		}


		#endregion

		# region My_ Values PersonPremiseRole

		/// <summary>
		/// Person Premise Role Id
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>PersonPremiseRoleId for this Row</returns>
		public int my_PersonPremiseRoleId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PersonPremiseRoleId"));
		}

		/// <summary>
		/// Person Premise Role Type, from the enumeration
		/// <see cref="clsKeyBase.personPremiseRoleType">PersonPremiseRoleType</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Type of Person Premise Role for this Row</returns>
		public int my_PersonPremiseRoleType(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PersonPremiseRoleType"));
		}

		/// <summary>PersonPremiseRoleTypeName</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>PersonPremiseRoleTypeName</returns>
		public string my_PersonPremiseRoleTypeName(int rowNum)
		{
			return personPremiseRoleTypeName(my_PersonPremiseRoleType(rowNum));
		}

		/// <summary>
		/// Priority (only applies to Keyholders)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Priority (only applies to Keyholders)</returns>
		public int my_Priority(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Priority"));
		}


		/// <summary>
		/// <see cref="clsPremise.my_PremiseId">PremiseId</see> of 
		/// <see cref="clsPremise">Premise</see>
		/// Associated with this Person Premise Role</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_PremiseId">Id</see> 
		/// of <see cref="clsPremise">Premise</see> 
		/// for this Person Premise Role</returns>	
		public int my_PremiseId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PremiseId"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">PersonId</see> of 
		/// <see cref="clsPerson">Person</see>
		/// Associated with this Person Premise Role</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> 
		/// of <see cref="clsPerson">Person</see> 
		/// for this Person Premise Role</returns>	
		public int my_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PersonId"));
		}

		/// <summary>
		/// Number of People in this Role
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of People in this Role</returns>
		public int my_NumPeopleInRole(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "NumPeopleInRole"));
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
		/// <see cref="clsPremise.my_DateLastInvoice">Date Last Detail Update was sent</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateLastInvoice">Date Last Detail Update was sent</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Premise_DateLastInvoice(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Premise_DateLastInvoice");
		}

		
		/// <summary>
		/// <see cref="clsPremise.my_StickerRequired">Whether a Sticker is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_StickerRequired">Whether a Sticker is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_Premise_StickerRequired(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_StickerRequired"));
		}

		/// <summary>
		/// <see cref="clsPremise.my_InvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_InvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_Premise_InvoiceRequired(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_InvoiceRequired"));
		}

		/// <summary>
		/// <see cref="clsPremise.my_CopyOfInvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_CopyOfInvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_Premise_CopyOfInvoiceRequired(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_CopyOfInvoiceRequired"));
		}

		/// <summary>
		/// <see cref="clsPremise.my_StatementRequired">Whether a Statement is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_StatementRequired">Whether a Statement is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_Premise_StatementRequired(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_StatementRequired"));
		}

		/// <summary>
		/// <see cref="clsPremise.my_DetailsUpdateRequired">Whether a DetailsUpdate is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DetailsUpdateRequired">Whether a DetailsUpdate is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_Premise_DetailsUpdateRequired(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Premise_DetailsUpdateRequired"));
		}


		#endregion

		# region My_ Values Person

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">Id</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public int my_Person_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Person_PersonId"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">Id</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public int my_Person_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Person_CustomerId"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_Title">Title</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Title">Title</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_Title(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_Title");
		}

				
		/// <summary>
		/// <see cref="clsPerson.my_FirstName">First Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_FirstName">First Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_FirstName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_LastName">Last Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_LastName">Last Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_LastName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_FullName">Full Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_FullName">Full Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_FullName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_FullName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_UserName">User Name</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_UserName">User Name</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_UserName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_UserName");
		}

		/// <summary>
		/// <see cref="clsPerson.my_Password">Password</see> for
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Password">Password</see> for 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_Password(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_Password");
		}

		/// <summary>
		/// <see cref="clsPerson.my_Email">Email Address</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_Email">Email Address</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_Email");
		}


		/// <summary>
		/// <see cref="clsPerson.my_QuickPostalAddress">Quick Postal Address</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickPostalAddress">Quick Postal Address</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickPostalAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickPostalAddress");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickDaytimePhone">Quick Daytime Phone</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickDaytimePhone">Quick Daytime Phone</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickDaytimePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickDaytimePhone");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickDaytimeFax">Quick Daytime Fax</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickDaytimeFax">Quick Daytime Fax</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickDaytimeFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickDaytimeFax");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickAfterHoursPhone">Quick After Hours Phone</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickAfterHoursPhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickAfterHoursPhone");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickAfterHoursFax">Quick After Hours Fax</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickAfterHoursFax">Quick After Hours Fax</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickAfterHoursFax(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickAfterHoursFax");
		}

		/// <summary>
		/// <see cref="clsPerson.my_QuickMobilePhone">Quick Mobile Phone</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_QuickMobilePhone">Quick Mobile Phone</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_QuickMobilePhone(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_QuickMobilePhone");
		}

	
		/// <summary>
		/// <see cref="clsPerson.my_PreferredContactMethod">
		/// Person's Preferred Contact Method</see> (from enumeration 
		/// <see cref="clsKeyBase.correspondenceMedium">Preferred Contact Method</see>)  of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>
		/// <see cref="clsPerson.my_PreferredContactMethod">
		/// Person's Preferred Contact Method</see> (from enumeration 
		/// <see cref="clsKeyBase.correspondenceMedium">Preferred Contact Method</see>)  of
		/// <see cref="clsPerson">Person</see> 
		/// </returns>		
		public int my_Person_PreferredContactMethod(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Person_PreferredContactMethod"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_IsCustomerAdmin">
		/// Whether this Person is an Administrator for this Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_IsCustomerAdmin">
		/// Whether this Person is an Administrator for this Customer</see>
		/// </returns>
		public int my_Person_IsCustomerAdmin(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Person_IsCustomerAdmin"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_PositionInCompany">Position in Company</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PositionInCompany">Position in Company</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_PositionInCompany(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_PositionInCompany");
		}

		/// <summary>
		/// <see cref="clsPerson.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_KdlComments");
		}

		/// <summary>
		/// <see cref="clsPerson.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_CustomerComments");
		}

		/// <summary>
		/// <see cref="clsPerson.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_DateLastLoggedIn">Date Last Logged In (in Client Time)</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public string my_Person_DateLastLoggedIn(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Person_DateLastLoggedIn");
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
