using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;
using System.Collections;

namespace Keyholders
{
	/// <summary>
	/// clsPremise deals with everything to do with data about Premises.
	/// </summary>

	[GuidAttribute("E299D1B7-D49A-4bea-8776-22BEB1360992")]
	public class clsPremise : clsKeyBase
	{

		# region Initialisation
		/// <summary>
		/// Constructor for clsPremise
		/// </summary>
		public clsPremise() : base("Premise")
		{
		}

		/// <summary>
		/// Constructor for clsPremise; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsPremise(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Premise")
		{
			Connect(typeOfDb, odbcConnection);
		}


		/// <summary>
		/// Part of the Query that Pertains to Customer Group Information
		/// </summary>
		public clsQueryPart CustomerGroupQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Customer Information
		/// </summary>
		public clsQueryPart CustomerQ = new clsQueryPart();
		
		/// <summary>
		/// Part of the Query that Pertains to Company Type Information
		/// </summary>
		public clsQueryPart CompanyTypeQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to Item Information
		/// </summary>
		public clsQueryPart ItemQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to PersonPremiseRoles Information
		/// </summary>
		public clsQueryPart PersonPremiseRoleSummaryQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to PersonPremiseRoles Information
		/// </summary>
		public clsQueryPart SPPremiseRoleSummaryQ = new clsQueryPart();

		
		/// <summary>
		/// Part of the Query that Pertains to High Risk Goods Information
		/// </summary>
		public clsQueryPart HighRiskGoodSummaryQ = new clsQueryPart();

		
		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			CustomerQ = CustomerQueryPart();
			CustomerGroupQ = CustomerGroupQueryPart();
			CustomerGroupQ.Joins.Clear();
			CustomerGroupQ.AddJoin("tblCustomer.CustomerGroupId = tblCustomerGroup.CustomerGroupId");

			CompanyTypeQ = CompanyTypeQueryPart();
			ItemQ = ItemQueryPart();

			MainQ = PremiseQueryPart();
			ChangeDataQ = ChangeDataQueryPart();

			MainQ.FromTables.Clear();

			MainQ.AddFromTable(thisTable + " left outer join tblItem on tblPremise.ItemId = tblItem.ItemId");

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[3];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = CustomerQ;
			baseQueries[2] = ChangeDataQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);
			
			orderBySqlQuery = "Order By QuickPhysicalAddress" + crLf;

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsPremise
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Numbers

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("CustomerId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ItemId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("ProductId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PremiseNumber", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CompanyAtPremiseName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("QuickPhysicalAddress", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CompanyTypeId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CompanyTypeName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Url", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AlarmDetails", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("KdlComments", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerComments", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateStart", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateNextSubscriptionDueToBeInvoiced", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSubscriptionExpires", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateLastDetailsUpdate", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateLastInvoice", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateLastCopyOfInvoice", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateLastStatement", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("StickerRequired", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("InvoiceRequired", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CopyOfInvoiceRequired", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("StatementRequired", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("DetailsUpdateRequired", System.Type.GetType("System.Int32"));
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
		/// Local Representation of the class <see cref="clsPremise">clsPremise</see>
		/// </summary>
		public clsPremise thisPremise;

		/// <summary>
		/// Exceptions that are not to be Renewed
		/// </summary>
		public ArrayList RenewalExceptions = new ArrayList();

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Premises
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
		/// Gets an Premise by PremiseId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByPremiseId(int PremiseId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[4];
			
			queries[0] = MainQ;
			queries[1] = CustomerQ;
			queries[2] = CustomerGroupQ;
			queries[3] = ChangeDataQ;


			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ PremiseId.ToString() + crLf;

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
		/// Gets an Premise by PremiseId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetImminentRenewals(string RenewalDate)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[4];
			
			queries[0] = MainQ;
			queries[1] = CustomerQ;
			queries[2] = CustomerGroupQ;
			queries[3] = ItemQ;

			string condition = "(Select * from " + thisTable + crLf
				+ " Where " + thisTable + ".DateNextSubscriptionDueToBeInvoiced is not null" + crLf
				+ " And " + thisTable + ".DateNextSubscriptionDueToBeInvoiced < '" 
				+ localRecords.safeSql(localRecords.DBDateTime(Convert.ToDateTime(RenewalDate))) 
				+ "'" + crLf
				+ " And " + thisTable + ".InvoiceRequired = 0" 
				+ crLf;

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
		/// Gets Premises by any kind of name
		/// </summary>
		/// <param name="Name">Filter for Name or Part of Name of Premise</param>
		/// <returns>Number of resulting records</returns>
		public int GetDistinctName(string Name)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].AddSelectColumn("Distinct CustomerId, DisplayName, PremiseNumber");
			queries[0].FromTables.Clear();
			queries[0].AddFromTable(thisTable);

			string condition = "(Select CustomerId, PremiseNumber, " 
				+ "Ltrim(Rtrim(Replace(concat_ws(', ',PremiseNumber,CompanyAtPremiseName, QuickPhysicalAddress), '\r\n', ', ')))" + crLf
				+ " as DisplayName" + crLf
				+ "from " + thisTable + crLf
				+ "Where concat_ws(' ',PremiseNumber,CompanyAtPremiseName, QuickPhysicalAddress) " 
				+ MatchCondition(Name, matchCriteria.contains) + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				OrderByColumns, 
				condition,
				thisTable
				);


			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Premises by any kind of name
		/// </summary>
		/// <param name="Name">Filter for Name or Part of Name of Premise</param>
		/// <returns>Number of resulting records</returns>
		public int GetByName(string Name)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			baseQueries[0].AddJoin("concat_ws(' ',PremiseNumber,CompanyAtPremiseName, QuickPhysicalAddress, tblCustomer.FirstName, tblCustomer.LastName, tblCustomer.CompanyName) " 
				+ MatchCondition(Name, matchCriteria.contains));

//			string condition = "(Select * from " + thisTable + crLf
//				+ "Where concat_ws(' ',PremiseNumber,CompanyAtPremiseName, QuickPhysicalAddress, tblCustomer.FirstName, tblCustomer.LastName, tblCustomer.CompanyName) " 
//				+ MatchCondition(Name, matchCriteria.contains) + crLf;
			string condition = "(Select * from " + thisTable + crLf
				+ "Where "  + crLf;

			condition += ArchiveConditionIfNecessary(false);

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
		/// Gets all Premises who are associated with a certain Customer
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".CustomerId = " + CustomerId.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition + ") " + thisTable,
				thisTable
				);

			//Ordering
			thisSqlQuery += orderBySqlQuery;
			


			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Returns Premises with the specified Premise Number
		/// </summary>
		/// <param name="PremiseNumber">Number of Premise</param>
		/// <param name="MatchCriteria">Criteria to match, from the enumeration 	
		/// <see cref="clsGeneralBase.matchCriteria">matchCriteria</see></param>
		/// <returns>Number of Premises with the specified Premise Number</returns>
		public int GetByNumber(string PremiseNumber, int MatchCriteria)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
		
			string condition = "(Select * from " + thisTable + crLf
				+  "Where tblPremise.PremiseNumber " 
				+ MatchCondition(PremiseNumber, (matchCriteria) MatchCriteria) + crLf;

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
		/// Returns Premises which require a sticker
		/// </summary>
		/// <returns>Number of Premises Premises which require a sticker</returns>
		public int GetByStickerRequired()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
		
			string condition = "(Select * from " + thisTable + crLf
				+  "Where tblPremise.StickerRequired = 1 " + crLf;

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

		#endregion

		#region Mark Correspondence as sent

		/// <summary>
		/// Mark all pending correspondence as sent
		/// </summary>
		public void MarkCorrespondenceAsSentForCustomerId(string RenewalDate, int CustomerId)
		{

			DateTime RenewalDateTime = Convert.ToDateTime(RenewalDate);
			RenewalDate = localRecords.DBDateTime(RenewalDateTime);
			string CurrentDate = localRecords.DBDateTime(DateTime.Now);
			//			string DateNextSubscriptionDueToBeInvoiced = localRecords.DBDateTime(RenewalDateTime.AddYears(1));

			string thisSql = "Update tblPremise set DateLastInvoice = '" + RenewalDate + "'," + crLf
				+ "	InvoiceRequired = 0," + crLf
				//				+ "	DateSubscriptionExpires = case when DateSubscriptionExpires < RenewalDate then Date_Add(DateSubscriptionExpires, INTERVAL 12 MONTH ) else DateSubscriptionExpires end, " + crLf
				//				+ "	DateNextSubscriptionDueToBeInvoiced = case when DateSubscriptionExpires < RenewalDate then Date_Add(DateNextSubscriptionDueToBeInvoiced, INTERVAL 12 MONTH) else DateNextSubscriptionDueToBeInvoiced end " + crLf
				+ "	DateSubscriptionExpires = Date_Add(DateSubscriptionExpires, INTERVAL 12 MONTH ), " + crLf
				+ "	DateNextSubscriptionDueToBeInvoiced = Date_Add(DateNextSubscriptionDueToBeInvoiced, INTERVAL 12 MONTH) " + crLf
				+ "where Archive = 0 and InvoiceRequired = 1" + crLf
				+ "	and CustomerId = " + CustomerId.ToString()
				;

			localRecords.GetRecords(thisSql);

			thisSql = "Update tblPremise set DateLastCopyOfInvoice = '" + CurrentDate + "', " + crLf
				+ " CopyOfInvoiceRequired = 0 " + crLf
				+ "where Archive = 0 and CopyOfInvoiceRequired = 1" + crLf
				+ "	and CustomerId = " + CustomerId.ToString()
				;

			localRecords.GetRecords(thisSql);

			thisSql = "Update tblOrder set DateInvoiceLastPrinted = '" + CurrentDate + "', " + crLf
				+ " InvoiceRequested = 0 " + crLf
				+ "where InvoiceRequested = 1" + crLf
				+ "	and CustomerId = " + CustomerId.ToString()
				;

			localRecords.GetRecords(thisSql);

			thisSql = "Update tblPremise set DateLastStatement = '" + RenewalDate + "', StatementRequired = 0" + crLf
				+ "where Archive = 0 and StatementRequired = 1" + crLf
				+ "	and CustomerId = " + CustomerId.ToString()
				;

			localRecords.GetRecords(thisSql);

			thisSql = "Update tblPremise set DateLastDetailsUpdate = '" + RenewalDate + "', DetailsUpdateRequired = 0" + crLf
				+ "where Archive = 0 and DetailsUpdateRequired = 1" + crLf
				+ "	and CustomerId = " + CustomerId.ToString()
				;

			localRecords.GetRecords(thisSql);

			thisSql = "Update tblPremise set StickerRequired = 0" + crLf
				+ "where Archive = 0 and StickerRequired = 1" + crLf
				+ "	and CustomerId = " + CustomerId.ToString()
				;

			localRecords.GetRecords(thisSql);

		}

		/// <summary>
		/// Mark all pending correspondence as sent
		/// </summary>
		public void MarkCorrespondenceAsSent(string RenewalDate)
		{

			DateTime RenewalDateTime = Convert.ToDateTime(RenewalDate);
			RenewalDate = localRecords.DBDateTime(RenewalDateTime);
			string CurrentDate = localRecords.DBDateTime(DateTime.Now);
//			string DateNextSubscriptionDueToBeInvoiced = localRecords.DBDateTime(RenewalDateTime.AddYears(1));

			string thisSql = "Update tblPremise set DateLastInvoice = '" + RenewalDate + "'," + crLf
				+ "	InvoiceRequired = 0," + crLf
//				+ "	DateSubscriptionExpires = case when DateSubscriptionExpires < RenewalDate then Date_Add(DateSubscriptionExpires, INTERVAL 12 MONTH ) else DateSubscriptionExpires end, " + crLf
//				+ "	DateNextSubscriptionDueToBeInvoiced = case when DateSubscriptionExpires < RenewalDate then Date_Add(DateNextSubscriptionDueToBeInvoiced, INTERVAL 12 MONTH) else DateNextSubscriptionDueToBeInvoiced end " + crLf
				+ "	DateSubscriptionExpires = Date_Add(DateSubscriptionExpires, INTERVAL 12 MONTH ), " + crLf
				+ "	DateNextSubscriptionDueToBeInvoiced = Date_Add(DateNextSubscriptionDueToBeInvoiced, INTERVAL 12 MONTH) " + crLf
				+ "where Archive = 0 and InvoiceRequired = 1";

			localRecords.GetRecords(thisSql);

			thisSql = "Update tblPremise set DateLastCopyOfInvoice = '" + CurrentDate + "', " + crLf
				+ " CopyOfInvoiceRequired = 0 " + crLf
				+ "where Archive = 0 and CopyOfInvoiceRequired = 1";

			localRecords.GetRecords(thisSql);

			thisSql = "Update tblOrder set DateInvoiceLastPrinted = '" + CurrentDate + "', " + crLf
				+ " InvoiceRequested = 0 " + crLf
				+ "where InvoiceRequested = 1";

			localRecords.GetRecords(thisSql);

			thisSql = "Update tblPremise set DateLastStatement = '" + RenewalDate + "', StatementRequired = 0" + crLf
				+ "where Archive = 0 and StatementRequired = 1";

			localRecords.GetRecords(thisSql);

			thisSql = "Update tblPremise set DateLastDetailsUpdate = '" + RenewalDate + "', DetailsUpdateRequired = 0" + crLf
				+ "where Archive = 0 and DetailsUpdateRequired = 1";

			localRecords.GetRecords(thisSql);

			thisSql = "Update tblPremise set StickerRequired = 0" + crLf
				+ "where Archive = 0 and StickerRequired = 1";

			localRecords.GetRecords(thisSql);

		}



		#endregion

		#region ResetPremiseCorrepondence

		/// <summary>
		/// Resets Correspondence fields (StickerRequired, InvoiceRequired, DetailsRequired) for a Premise
		/// </summary>
		/// <param name="PremiseId">Id of Premise to Reset Correspondence Fields for</param>
		public void ResetPremiseCorrepondence(int PremiseId)
		{

			SetAttribute(PremiseId, "StickerRequired", "0");
			AddAttributeToSet(PremiseId, "InvoiceRequired", "0");
			AddAttributeToSet(PremiseId, "CopyOfInvoiceRequired", "0");
			AddAttributeToSet(PremiseId, "StatementRequired", "0");
			AddAttributeToSet(PremiseId, "DetailsUpdateRequired", "0");
		}

		#endregion

		#region CreateRenewals

		/// <summary>
		/// Creates Renewal Order and Items
		/// </summary>
		/// <param name="RenewalDate">Date for Orders and Items</param>
		public void CreateRenewals(string RenewalDate)
		{
			clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);
			clsTransaction thisTransaction = new clsTransaction(thisDbType, localRecords.dbConnection);

			GetGeneralSettings();

			thisOrder.DeleteUnsubmittedOrders();
			thisTransaction.DeletePendingTransactions();

			CreateRenewalOrdersWithExceptions(RenewalDate);
			CreateRenewalItemsWithExceptions(RenewalDate);
			UpdateOrderCostsUnsubmittedOrders();
			

			thisOrder.DeleteUnsubmittedOrdersWithNoItems();
			thisOrder.UpdateOrderNumbers();
			SetInvoiceRequiredForUnsubmittedOrders();
		}

		#endregion

		#region UpdateNumItemsInOrders

		/// <summary>
		/// UpdateNumItemsInOrders
		/// </summary>
		public void UpdateOrderCostsUnsubmittedOrders()
		{

			GetGeneralSettings();

			string TaxCost = "";

			if (priceShownIncludesLocalTaxRate)
				TaxCost = "(sum(Cost) + sum(FreightCost)) * (" 
					+ (localTaxRate - 1).ToString() 
					+ " / " + (localTaxRate).ToString()
					+ ")";
			else
				TaxCost = "(sum(Cost) + sum(FreightCost)) * ("
					+ (localTaxRate - 1).ToString() ;


			string thisSql = "Update tblOrder, " + crLf
				+ "	(Select OrderId, " + crLf
				+ " count(ItemId) as NumItems, " + crLf
				+ " sum(Weight) as TotalItemWeight, " + crLf
				+ " sum(Cost) as Total, " + crLf
				+ " sum(Cost) as TotalItemCost, " + crLf
				+ " sum(FreightCost) as TotalItemFreightCost, " + crLf
				+ TaxCost + " as TaxCost," + crLf
				+ (localTaxRate - 1).ToString() + " as TaxRateAtTimeOfOrder, " + crLf
				+ "1 as TaxAppliedToOrder, " + crLf
				+ "0 as IsInvoiceOrder " + crLf

				+ " from tblItem group by OrderId) tblItem" + crLf

				+ "Set tblOrder.NumItems = tblItem.NumItems," + crLf
				+ " tblOrder.TotalItemWeight = tblItem.TotalItemWeight," + crLf
				+ " tblOrder.Total = tblItem.Total," + crLf
				+ " tblOrder.TotalItemCost = tblItem.TotalItemCost," + crLf
				+ " tblOrder.TotalItemFreightCost = tblItem.TotalItemFreightCost," + crLf
				+ " tblOrder.TaxCost = tblItem.TaxCost," + crLf
				+ " tblOrder.TaxRateAtTimeOfOrder = tblItem.TaxRateAtTimeOfOrder," + crLf
				+ " tblOrder.TaxAppliedToOrder = tblItem.TaxAppliedToOrder," + crLf
				+ " tblOrder.IsInvoiceOrder = tblItem.IsInvoiceOrder" + crLf

				+ " where tblOrder.OrderId = tblItem.OrderId" + crLf
				+ " and tblOrder.OrderSubmitted = 0" + crLf
	
				;




			localRecords.GetRecords(thisSql);
		}

		#endregion

		#region CreateRenewalOrdersWithExceptions

		/// <summary>
		/// Creates Renewal Orders as Required
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int CreateRenewalOrdersWithExceptions(string RenewalDate)
		{
			DateTime thisUTCDateTime = DateTime.UtcNow;

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[6];

			clsQueryPart PersonPremiseRoleQ = PersonPremiseRoleQueryPart();
			PersonPremiseRoleQ.Joins.Clear();
			PersonPremiseRoleQ.Joins.Add("tblPersonPremiseRole.PremiseId = tblPremise.PremiseId");
			PersonPremiseRoleQ.Joins.Add("tblPersonPremiseRole.PersonId = tblPerson.PersonId");
			PersonPremiseRoleQ.Joins.Add("tblPersonPremiseRole.PersonPremiseRoleType = " + personPremiseRoleType_billingContact().ToString());
			PersonPremiseRoleQ.Joins.Add("tblPersonPremiseRole.Archive = 0");
			PersonPremiseRoleQ.Joins.Add("tblCustomer.Archive = 0");
			PersonPremiseRoleQ.Joins.Add("tblPerson.Archive = 0");
			PersonPremiseRoleQ.Joins.Add("tblPremise.Archive = 0");

			clsQueryPart PersonQ = PersonQueryPart();
			PersonQ.Joins.Clear();
			
			queries[0] = MainQ;
			queries[1] = CustomerQ;
			queries[2] = CustomerGroupQ;
			queries[3] = ItemQ;
			queries[4] = PersonPremiseRoleQ;
			queries[5] = PersonQ;

			if (localTaxRate == 0)
				GetGeneralSettings();

			for (int counter = 0; counter < queries.GetUpperBound(0) + 1; counter ++)
				queries[counter].SelectColumns.Clear();

			int numExceptions = RenewalExceptions.Count;
			
			string update = "Insert into tblOrder (CustomerId, PersonId, PaymentMethodTypeId, CustomerGroupId, OrderNum, CustomerType, "
				+ " FullName, Title, FirstName, LastName, QuickPostalAddress, QuickDaytimePhone, QuickDaytimeFax, QuickAfterHoursPhone, "
				+ " QuickAfterHoursFax, QuickMobilePhone, CountryId, Email, OrderSubmitted, OrderPaid, OrderStatusId, SupplierComment, "
				+ " DateCreated, DateCreatedUtc, DateSubmitted, DateSubmittedUtc, DateProcessed, DateProcessedUtc, DateShipped, DateShippedUtc, "
				+ " DateDue, InvoiceRequested, DateInvoiceLastPrinted, TaxAppliedToOrder, TaxRateAtTimeOfOrder, TaxCost, FreightCost, Total, OrderCreatedMechanism)" + crLf;

			queries[0].AddSelectColumn("tblCustomer.CustomerId");
			queries[0].AddSelectColumn("tblPerson.PersonId as PersonId");
			queries[0].AddSelectColumn(paymentMethodType_asYetUndetermined().ToString() + " as PaymentMethodTypeId");
			queries[0].AddSelectColumn("tblCustomer.CustomerGroupId");
			queries[0].AddSelectColumn("0 as OrderNum");
			queries[0].AddSelectColumn("tblCustomer.CustomerType as CustomerType");
			queries[0].AddSelectColumn("tblCustomer.FullName as FullName");
			queries[0].AddSelectColumn("tblPerson.Title as Title");
			queries[0].AddSelectColumn("tblPerson.FirstName as FirstName");
			queries[0].AddSelectColumn("tblPerson.LastName as LastName");
			queries[0].AddSelectColumn("tblPerson.QuickPostalAddress as QuickPostalAddress");
			queries[0].AddSelectColumn("tblPerson.QuickDaytimePhone as QuickDaytimePhone");
			queries[0].AddSelectColumn("tblPerson.QuickDaytimeFax as QuickDaytimeFax");
			queries[0].AddSelectColumn("tblPerson.QuickAfterHoursPhone as QuickAfterHoursPhone");
			queries[0].AddSelectColumn("tblPerson.QuickAfterHoursFax as QuickAfterHoursFax");
			queries[0].AddSelectColumn("tblPerson.QuickMobilePhone as QuickMobilePhone");
			queries[0].AddSelectColumn("tblCustomer.CountryId");
			queries[0].AddSelectColumn("tblPerson.Email as Email");
			queries[0].AddSelectColumn("0 as OrderSubmitted");
			queries[0].AddSelectColumn("0 as OrderPaid");
			queries[0].AddSelectColumn("1 as OrderStatusId");
			queries[0].AddSelectColumn("'Automatically generated for renewal' as SupplierComment");
			queries[0].AddSelectColumn("'" + localRecords.DBDateTime(FromUtcToClientTime(thisUTCDateTime)) + "' as DateCreated");
			queries[0].AddSelectColumn("'" + localRecords.DBDateTime(thisUTCDateTime) + "' as DateCreatedUtc");
			queries[0].AddSelectColumn("Null as DateProcessed");
			queries[0].AddSelectColumn("Null as DateProcessedUtc");
			queries[0].AddSelectColumn("Null as DateProcessed");
			queries[0].AddSelectColumn("Null as DateProcessedUtc");
			queries[0].AddSelectColumn("Null as DateShipped");
			queries[0].AddSelectColumn("Null as DateShippedUtc");
			queries[0].AddSelectColumn("AddDate(AddDate(DateNextSubscriptionDueToBeInvoiced, INTERVAL 1 Month), Interval 20 - DayOfMonth(DateNextSubscriptionDueToBeInvoiced) Day) as DateDue");
			queries[0].AddSelectColumn("1 as InvoiceRequested");
			queries[0].AddSelectColumn("Null as DateInvoiceLastPrinted");
			queries[0].AddSelectColumn("Null as TaxAppliedToOrder");
			queries[0].AddSelectColumn((localTaxRate - 1).ToString() + " as TaxRateAtTimeOfOrder");
			queries[0].AddSelectColumn("Null as TaxCost");
			queries[0].AddSelectColumn("Null as FreightCost");
			queries[0].AddSelectColumn("Null as Total");
			queries[0].AddSelectColumn(orderCreatedMechanism_byVendorAutomatically() + " as OrderCreatedMechanism");

		
			for(int counter = 0; counter <  numExceptions; counter++)
			{
				int thisRenewalException = (int) RenewalExceptions[counter];
				queries[0].AddJoin("PremiseId != " + thisRenewalException.ToString());
			}
			
			
			string condition = "(Select * from " + thisTable + crLf
				+ " Where " + thisTable + ".DateNextSubscriptionDueToBeInvoiced is not null" + crLf
				+ " And " + thisTable + ".DateNextSubscriptionDueToBeInvoiced < '" 
				+ localRecords.safeSql(localRecords.DBDateTime(Convert.ToDateTime(RenewalDate))) 
				+ "'" + crLf
//				+ " And " + thisTable + ".InvoiceRequired = 0" 
				+ crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(update + thisSqlQuery);
		}

		#endregion

		#region CreateRenewalItemsWithExceptions

		/// <summary>
		/// Creates Renewal Items as Required
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int CreateRenewalItemsWithExceptions(string RenewalDate)
		{

			if (localTaxRate == 0)
				GetGeneralSettings();

			DateTime thisUTCDateTime = DateTime.UtcNow;

			clsQueryBuilder QB = new clsQueryBuilder();

			clsQueryPart OrderQ = new clsQueryPart();
			OrderQ.AddFromTable("(Select CustomerId, min(orderId) as OrderId from tblOrder where OrderSubmitted = 0 group by CustomerId) tblOrder");
			OrderQ.AddJoin("tblOrder.CustomerId = tblPremise.CustomerId");
			
			clsQueryPart ProductQ = new clsQueryPart();
			ProductQ.AddFromTable("tblProduct");
			ProductQ.AddJoin("(case when tblPremise.ProductId is null then 1 else tblPremise.ProductId end) = tblProduct.ProductId ");
			
			clsQueryPart ProductCustomerGroupPriceQ = new clsQueryPart();
			ProductCustomerGroupPriceQ.AddFromTable("tblProductCustomerGroupPrice");
			ProductCustomerGroupPriceQ.AddJoin("tblProductCustomerGroupPrice.CustomerGroupId = tblCustomerGroup.CustomerGroupId ");
			ProductCustomerGroupPriceQ.AddJoin("tblProduct.ProductId = tblProductCustomerGroupPrice.ProductId");
			ProductCustomerGroupPriceQ.Joins.Add("tblCustomer.Archive = 0");
			ProductCustomerGroupPriceQ.Joins.Add("tblPremise.Archive = 0");

			clsQueryPart[] queries = new clsQueryPart[6];
			
			queries[0] = MainQ;
			queries[1] = CustomerQ;
			queries[2] = CustomerGroupQ;
			queries[3] = OrderQ;
			queries[4] = ProductQ;
			queries[5] = ProductCustomerGroupPriceQ;
			
			for (int counter = 0; counter < queries.GetUpperBound(0) + 1; counter ++)
				queries[counter].SelectColumns.Clear();

			int numExceptions = RenewalExceptions.Count;
			
			string update = "Insert into tblItem (OrderId, ProductId, PremiseId, Quantity, ItemName, ItemCode, "
				+ " ShortDescription, LongDescription, Cost, Weight, MaxKeyholdersPerPremise, MaxAssetRegisterAssets, MaxAssetRegisterStorage,  "
				+ " MaxDocumentSafeDocuments, MaxDocumentSafeStorage, RequiresPremiseForActivation, DateActivation, DateActivationUtc, "
				+ " DateExpiry, DateExpiryUtc, DurationNumUnits, DurationUnitId, FreightCost)" + crLf;

			queries[0].AddSelectColumn("tblOrder.OrderId");
			queries[0].AddSelectColumn("tblProduct.ProductId");
			queries[0].AddSelectColumn("tblPremise.PremiseId");
			queries[0].AddSelectColumn("1 as Quantity");
			queries[0].AddSelectColumn("tblProduct.ProductName as ItemName");
			queries[0].AddSelectColumn("tblProduct.ProductCode as ItemCode");
			queries[0].AddSelectColumn("tblProduct.ShortDescription");
			queries[0].AddSelectColumn("'' as LongDescription");

			if (priceShownIncludesLocalTaxRate)
				queries[0].AddSelectColumn("tblProductCustomerGroupPrice.Price * " + localTaxRate.ToString() + " as Cost");
			else
				queries[0].AddSelectColumn("tblProductCustomerGroupPrice.Price as Cost");

			queries[0].AddSelectColumn("tblProduct.Weight");
			queries[0].AddSelectColumn("tblProduct.MaxKeyholdersPerPremise");
			queries[0].AddSelectColumn("tblProduct.MaxAssetRegisterAssets");
			queries[0].AddSelectColumn("tblProduct.MaxAssetRegisterStorage");
			queries[0].AddSelectColumn("tblProduct.MaxDocumentSafeDocuments");
			queries[0].AddSelectColumn("tblProduct.MaxDocumentSafeStorage");
			queries[0].AddSelectColumn("tblProduct.RequiresPremiseForActivation");
			queries[0].AddSelectColumn("tblPremise.DateSubscriptionExpires as DateActivation");
			queries[0].AddSelectColumn("tblPremise.DateSubscriptionExpires as DateActivationUtc");
			queries[0].AddSelectColumn("Date_Add(tblPremise.DateSubscriptionExpires, INTERVAL 12 MONTH) as DateExpiry");
			queries[0].AddSelectColumn("Date_Add(tblPremise.DateSubscriptionExpires, INTERVAL 12 MONTH) as DateExpiryUtc");
			queries[0].AddSelectColumn("12 as DurationNumUnits");
			queries[0].AddSelectColumn("5 as DurationUnitId");
			queries[0].AddSelectColumn("0 as FreightCost");
		
			for(int counter = 0; counter <  numExceptions; counter++)
			{
				int thisRenewalException = (int) RenewalExceptions[counter];
				queries[0].AddJoin("PremiseId != " + thisRenewalException.ToString());
			}
			
			
			string condition = "(Select * from " + thisTable + crLf
				+ " Where " + thisTable + ".DateNextSubscriptionDueToBeInvoiced is not null" + crLf
				+ " And " + thisTable + ".DateNextSubscriptionDueToBeInvoiced < '" 
				+ localRecords.safeSql(localRecords.DBDateTime(Convert.ToDateTime(RenewalDate))) 
				+ "'" + crLf
//				+ " And " + thisTable + ".InvoiceRequired = 1" 
				+ crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, OrderByColumns, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(update + thisSqlQuery);
		}

		#endregion

		#region AddRenewalException

		/// <summary>
		/// Adds a Renewal Order, complete with Items, for a Premise
		/// </summary>
		/// <param name="PremiseId">Id of Premise for this Exception</param>
		public void AddRenewalException(int PremiseId)
		{


			int thisRenewalException = PremiseId;
			
			RenewalExceptions.Add(thisRenewalException);
		}

		#endregion 

		#region SetInvoiceRequiredForUnsubmittedOrders

		/// <summary>
		/// Sets Invoice as Required For All Unsubmitted Orders
		/// </summary>
		public void SetInvoiceRequiredForUnsubmittedOrders()
		{
			string update = "Update tblPremise, (select * from tblOrder where OrderSubmitted = 0) tblOrder, tblItem set tblPremise.InvoiceRequired = 1 where "
				+ "tblOrder.OrderId = tblItem.OrderId and tblItem.PremiseId = tblPremise.PremiseId";

			localRecords.GetRecords(update);

		}

		#endregion

		#region SetDetailsUpdateRequired

		/// <summary>
		/// Sets Details Update Required when Correspondence is due in any case.
		/// </summary>
		/// <param name="DateForCorrespondence">DateForCorrespondence</param>
		public void SetDetailsUpdateRequired(string DateForCorrespondence)
		{
			GetGeneralSettings();

			string update = "Update tblPremise set tblPremise.DetailsUpdateRequired = 1 " + crLf
				+ "	where tblPremise.Archive = 0  " + crLf
				+ "  and (StickerRequired = 1" + crLf
				+ "	OR InvoiceRequired = 1 " + crLf
				+ "	OR StatementRequired = 1" + crLf
				+ "	OR DateLastDetailsUpdate is null " + crLf
				+ "	OR DateLastDetailsUpdate < '"  
				+ localRecords.DBDateTime(Convert.ToDateTime(DateForCorrespondence).AddDays(1)) + "' - INTERVAL " 
				+ minimumDetailsUpdateFrequency.ToString() + " MONTH)" + crLf;

			localRecords.GetRecords(update);
		}

		#endregion

		#region SetStatementRequired

		/// <summary>
		/// Sets Details Update Required when Correspondence is due in any case.
		/// </summary>
		/// <param name="DateForCorrespondence">DateForCorrespondence</param>
		public void SetStatementRequired(string DateForCorrespondence)
		{
			GetGeneralSettings();

			string update = "Update tblPremise, tblCustomer set tblPremise.StatementRequired = 1" + crLf
				+ "where tblCustomer.CurrentBalance > " + (minimumBalanceRequiringStatement).ToString() + crLf
				+ "  and tblPremise.CustomerId = tblCustomer.CustomerId" + crLf
				+ "  and tblPremise.Archive = 0" + crLf
				+ "  and tblCustomer.Archive = 0" + crLf
//				+ "  and tblPremise.InvoiceRequired = 0" + crLf
//				+ "  and (tblPremise.DateLastInvoice is null " + crLf
//				+ "		or tblPremise.DateLastInvoice < '" 
//				+ localRecords.DBDateTime(Convert.ToDateTime(DateForCorrespondence)) + "'  - INTERVAL " 
//				+ minimumStatementFrequency.ToString() + " DAY)" + crLf
				+ "  and (tblPremise.DateLastStatement is null or tblPremise.DateLastStatement < '" 
				+ localRecords.DBDateTime(Convert.ToDateTime(DateForCorrespondence)) + "' - INTERVAL " 
				+ minimumStatementFrequency.ToString() + " DAY)" + crLf
				;

			//It no longer matters if people receive a statement and an invoice at the same time.
				

			localRecords.GetRecords(update);
		}

		#endregion

		#region KillRenewalOrder

		/// <summary>
		/// Kill a Renewal Order, complete with Items, for a Premise
		/// </summary>
		/// <param name="CustomerId">Customer Associated with this Order</param>
		/// <param name="PremiseId">Id of Premise for this Order</param>
		public void KillRenewalOrder(int PremiseId, int CustomerId)
		{

			SetAttribute(PremiseId, "InvoiceRequired", "0".ToString());
			AddAttributeToSet(PremiseId, "DateNextSubscriptionDueToBeInvoiced");

			//Determine if this Customer already has an order; if so delete any Items attached to this Premise
			int OrderId = 0;

			clsOrder thisOrder = new clsOrder(thisDbType, localRecords.dbConnection);

			int pendingOrder = thisOrder.GetByOrderSubmittedCustomerId(0, CustomerId, 0);

			if (pendingOrder != 0)
			{
				OrderId = thisOrder.my_CustomerId(0);

				clsItem thisItem = new clsItem(thisDbType, localRecords.dbConnection);
				thisItem.GetByOrderIdPremiseId(OrderId, PremiseId);
				thisItem.DeleteCurrentGetData();
				if (thisItem.GetByOrderId(OrderId) == 0)
					thisOrder.Delete(OrderId);
			}

		}

		#endregion

		#region SetItemId

		/// <summary>
		/// This Method allows the ItemId to be set for a Premise outside of the Modify Method
		/// </summary>
		/// <param name="PremiseId">Id of Premise to set Item for</param>
		/// <param name="ItemId">Item to set</param>
		public void SetItemId(int PremiseId, 
			int ItemId)
		{
			SetAttribute(PremiseId, "ItemId", ItemId.ToString());
		}

		#endregion 

		#region SetCustomerId

		/// <summary>
		/// This Method allows the CustomerId to be set for a Premise. This method also
		/// - Copies each person associated with this Premise to a Person for the selected Customer,
		/// if that Person does not already exist for the selected Customer
		/// - Deletes any people for the original Customer who were only associated with this Premise
		/// </summary>
		/// <param name="PremiseId">Id of Premise to set Customer for</param>
		/// <param name="CustomerId">Customer to set this Premise to</param>
		public void SetCustomerId(int PremiseId, 
			int CustomerId)
		{

			//TODO: rethink this method
//			clsPremise thisPremise = new clsPremise(thisDbType, localRecords.dbConnection);
//			thisPremise.GetByPremiseId(PremiseId);			
//			
//			int oldCustomerId = thisPremise.my_CustomerId(0);
//
//			//Check if any of the People associated with this Premise are in the new Customer's profie; if so change the associations
//
//			clsPerson thisMatchingPeople = new clsPerson(thisDbType, localRecords.dbConnection);
//			int numMatchingPeople = thisMatchingPeople.GetMatchingPeopleForPremiseNewCustomer(PremiseId, CustomerId);			
//			
//
//			for(int counter = 0; counter < numMatchingPeople; counter++)
//			{
//				PersonPremiseRole.Set(PremiseId, thisMatchingPeople.my_PersonId(counter), thisMatchingPeople.my_MatchPeople_PersonPremiseRoleType(counter));
//				if(thisMatchingPeople.my_MatchPeople_PersonPremiseRoleType(counter) == personPremiseRoleType_keyHolder())
//					PersonPremiseRole.RemoveKeyholder(PremiseId, thisMatchingPeople.my_MatchPeople_PersonId(counter));
//			}
//
//			PersonPremiseRole.Save();
//	
//			//If there are any people left associated with this Premise and the Old CustomerId, copy these accross
//			int PPRs = PersonPremiseRole.GetByPremiseId(PremiseId);
//			
//			clsPerson thisPerson = new clsPerson(thisDbType, localRecords.dbConnection);
//
//			for(int counter = 0; counter < PPRs; counter++)
//			{
//				if (PersonPremiseRole.my_Person_CustomerId(counter) == oldCustomerId)
//				{
//					clsPhoneNumber thisPhoneNumber = new clsPhoneNumber(thisDbType, localRecords.dbConnection);
//					thisPhoneNumber.GetByPersonId(PersonPremiseRole.my_Person_PersonId(counter));
//					clsAddress thisAddress = new clsAddress(thisDbType, localRecords.dbConnection);
//					thisAddress.GetByPersonId(PersonPremiseRole.my_Person_PersonId(counter));
//
//					thisPerson.Add(CustomerId,
//						PersonPremiseRole.my_Person_Title(counter),
//						PersonPremiseRole.my_Person_FirstName(counter),
//						PersonPremiseRole.my_Person_LastName(counter),
//						PersonPremiseRole.my_Person_UserName(counter),
//						PersonPremiseRole.my_Person_Password(counter),
//						
//						PersonPremiseRole.my_Person_PositionInCompany(counter),
//						thisPhoneNumber.my_InternationalPrefix(0),
//						thisPhoneNumber.my_NationalOrMobilePrefix(0),
//						thisPhoneNumber.my_MainNumber(0),
//						thisPhoneNumber.my_Extension(0),
//						thisPhoneNumber.my_InternationalPrefix(1),
//						thisPhoneNumber.my_NationalOrMobilePrefix(1),
//						thisPhoneNumber.my_MainNumber(1),
//						thisPhoneNumber.my_Extension(1),						
//						thisPhoneNumber.my_InternationalPrefix(2),
//						thisPhoneNumber.my_NationalOrMobilePrefix(2),
//						thisPhoneNumber.my_MainNumber(2),
//						thisPhoneNumber.my_Extension(2),						
//						thisPhoneNumber.my_InternationalPrefix(3),
//						thisPhoneNumber.my_NationalOrMobilePrefix(3),
//						thisPhoneNumber.my_MainNumber(3),
//						thisPhoneNumber.my_Extension(3),						
//						thisPhoneNumber.my_InternationalPrefix(4),
//						thisPhoneNumber.my_NationalOrMobilePrefix(4),
//						thisPhoneNumber.my_MainNumber(4),
//						thisPhoneNumber.my_Extension(4),
//						thisAddress.my_POBoxType(0),
//						
//						thisAddress.my_StreetAddress(0),
//						thisAddress.my_PostCode(0),
//						thisAddress.my_CityName(0),
//						thisAddress.my_StateName(0),						
//						thisAddress.my_CountryId(0),					
//						PersonPremiseRole.my_Person_Email(counter),
//						PersonPremiseRole.my_Person_PreferredContactMethod(counter),
//						PersonPremiseRole.my_Person_KdlComments(counter),
//						PersonPremiseRole.my_Person_CustomerComments(counter),
//						PersonPremiseRole.my_Person_IsCustomerAdmin(counter));
//					thisPerson.Save();
//					PersonPremiseRole.Set(PremiseId, thisMatchingPeople.my_PersonId(counter), thisMatchingPeople.my_MatchPeople_PersonPremiseRoleType(counter));
//					if(thisMatchingPeople.my_MatchPeople_PersonPremiseRoleType(counter) == personPremiseRoleType_keyHolder())
//						PersonPremiseRole.RemoveKeyholder(PremiseId, thisMatchingPeople.my_MatchPeople_PersonId(counter));
//				}
//			}
//
//			//Archive anybody with no roles for this Customer
//
//			SetAttribute(PremiseId, "CustomerId", CustomerId.ToString());
//			thisPerson.ArchivePeopleWithNoRolesForCustomer(CustomerId);


		}

		#endregion

		#region UpdatePremiseItemAssociations

		/// <summary>
		/// Update Premise-Item Associations
		/// </summary>
		public void UpdatePremiseItemAssociations()
		{
			string update = "Update tblPremise, "
				+ "(select tblPremise.PremiseId, tblItem.ItemId, tblItem.ProductId, tblItem.DateExpiry " + crLf
				+ "from tblPremise left outer join (select * from tblItem " + crLf
				+ "where DateActivation is not null " + crLf
				+ "and DateActivation < Now() " + crLf
				+ "and DateExpiry is not null " + crLf
				+ "and DateExpiry > Now()) tblItem " + crLf
				+ "on tblItem.PremiseId = tblPremise.PremiseId) tblItem " + crLf
				+ "set tblPremise.ItemId = tblItem.ItemId, " + crLf
				+ "tblPremise.ProductId = tblItem.ProductId, " + crLf
				+ "tblPremise.DateSubscriptionExpires = tblItem.DateExpiry, " + crLf
				+ "tblPremise.DateNextSubscriptionDueToBeInvoiced = Date_Sub(tblItem.DateExpiry, Interval 2 month) " + crLf
				+ "where tblPremise.ItemId = tblItem.ItemId";

			localRecords.GetRecords(update);
		}

		#endregion

		# region Add/Modify/Validate/Save
		
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the SaveCustomers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PremiseNumber">Premise's Number</param>
		/// <param name="CustomerId">Associated Customer</param>
		/// <param name="CompanyAtPremiseName">Company Located at the Premise</param>
		/// <param name="CompanyTypeName">Type of Company Located at the Premise</param>
		/// <param name="ItemId">Associated Item</param>
		/// <param name="ProductId">Associated Product</param>
		/// <param name="Url">Url</param>
		/// <param name="AlarmDetails">Details of the Alarm</param>
		/// <param name="KdlComments">Comments made by KDL about this Premise</param>
		/// <param name="CustomerComments">Comments made by the Customer about this Premise</param>
		/// <param name="BuildingName">Premise's Physical Address: Building Name</param>
		/// <param name="UnitNumber">Premise's Physical Address: Unit Number</param>
		/// <param name="Number">Premise's Physical Address: Number</param>
		/// <param name="StreetAddress">Premise's Physical Address: Street Address</param>
		/// <param name="Suburb">Premise's Physical Address: Suburb</param>
		/// <param name="CityId">Premise's Physical Address: Id of City</param>
		/// <param name="CountryId">Premise's Physical Address: Id of Country</param>
		/// <param name="PostCode">Premise's Physical Address: PostCode</param>
		/// <param name="CityName">Premise's Physical Address: City</param>
		/// <param name="StateName">Premise's Physical Address: State or County</param>
		/// <param name="CountryName">Premise's Physical Address: Country</param>
		/// <param name="DateStart">DateStart</param>
		/// <param name="DateSubscriptionExpires">DateSubscriptionExpires</param>
		/// <param name="DateNextSubscriptionDueToBeInvoiced">Date the Next Subscription is Due for this Premise</param>
		/// <param name="DateLastDetailsUpdate">Date the Last Details Update was sent</param>
		/// <param name="DateLastInvoice">Date the Last Invoice was sent</param>
		/// <param name="DateLastCopyOfInvoice">Date the Last Copy of Invoice was sent</param>
		/// <param name="DateLastStatement">Date the Last Statement was sent</param>
		/// <param name="StickerRequired">Whether a Sticker is required in the next batch of Correspondence for this Premise</param>
		/// <param name="InvoiceRequired">Whether an Invoice is required in the next batch of Correspondence for this Premise</param>
		/// <param name="CopyOfInvoiceRequired">Whether a copy of an Invoice is required in the next batch of Correspondence for this Premise</param>
		/// <param name="StatementRequired">Whether a Statement is required in the next batch of Correspondence for this Premise</param>
		/// <param name="DetailsUpdateRequired">Whether a Details Update is required in the next batch of Correspondence for this Premise</param>
		/// <param name="DaytimeContactId">Id of <see cref="clsPerson">Daytime Contact</see></param>
		/// <param name="BillingContactId">Id of <see cref="clsPerson">Billing Contact</see></param>
		/// <param name="DetailsManagerId">Id of <see cref="clsPerson">Details Manager</see></param>
		/// <param name="AlarmMonitorId">Id of <see cref="clsServiceProvider">Alarm Monitoring Company</see></param>
		/// <param name="AlarmResponseId">Id of <see cref="clsServiceProvider">Alarm Response Company</see></param>
		/// <param name="PatrolId">Id of <see cref="clsServiceProvider">Patrol Company</see></param>
		/// <param name="CurrentUser">Current Logged In User</param>
		public void Add(int CustomerId,
			int ItemId,
			int ProductId,
			string PremiseNumber, 
			string CompanyAtPremiseName,
			string CompanyTypeName,
			string Url,
			string AlarmDetails,
			string KdlComments,
			string CustomerComments,
			
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

			string DateStart,
			string DateSubscriptionExpires,
			string DateNextSubscriptionDueToBeInvoiced,
			string DateLastDetailsUpdate,
			string DateLastInvoice,
			string DateLastCopyOfInvoice,
			string DateLastStatement,

			int StickerRequired,
			int InvoiceRequired,
			int CopyOfInvoiceRequired,
			int StatementRequired,
			int DetailsUpdateRequired,

			int DaytimeContactId,
			int BillingContactId,
			int DetailsManagerId,
			int AlarmMonitorId,
			int AlarmResponseId,
			int PatrolId,
			int CurrentUser)
		{

			if (nonCityCityId == 0)
				GetGeneralSettings();

			#region Deal with the address

			if (CountryId == 0)
				CountryId = assumedCountryId;
			
			if (CountryId == assumedCountryId)
				CountryName = "New Zealand";
			else
			{
				if (CountryName == "")
				{
					clsCountry thisCountry = new clsCountry(thisDbType, localRecords.dbConnection);

					int numCountries = thisCountry.GetByCountryId(CountryId);
					if (numCountries > 0)
						CountryName = thisCountry.my_CountryName(0);
				}
			}

			string QuickPhysicalAddress = GetQuickAddress(0,
				BuildingName,
				UnitNumber,
				Number,
				StreetAddress,
				Suburb,
				PostCode,
				CityName,
				StateName,
				CountryName);

			#endregion

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			AddGeneral(CustomerId,
				ItemId,
				ProductId,
				PremiseNumber,
				CompanyAtPremiseName,
				QuickPhysicalAddress,
				CompanyTypeName,
				Url,
				AlarmDetails,
				KdlComments,
				CustomerComments,
				DateStart,
				DateSubscriptionExpires,
				DateNextSubscriptionDueToBeInvoiced,
				DateLastDetailsUpdate,
				DateLastInvoice,
				DateLastCopyOfInvoice,
				DateLastStatement,
				StickerRequired,
				InvoiceRequired,
				CopyOfInvoiceRequired,
				StatementRequired,
				DetailsUpdateRequired,
				thisChangeData.LastIdAdded(),
				0);

			Save();

			int thisPremiseId = LastIdAdded();

			#region Add Address
			clsAddress thisAddress = new clsAddress(thisDbType, localRecords.dbConnection);

			thisAddress.Add(0,
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
				addressType_physical(), 
				"",
				thisTable, 
				thisPremiseId, 
				CurrentUser);

			thisAddress.Save();

			#endregion

			#region Add PPRs
			clsPersonPremiseRole thisPersonPremiseRole = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);

			if (BillingContactId > 0)
				thisPersonPremiseRole.Add(
					thisPremiseId,
					BillingContactId,
					(int) personPremiseRoleType.billingContact,
					0,
					systemUserId);

			if (DaytimeContactId > 0)
				thisPersonPremiseRole.Add(
					thisPremiseId,
					DaytimeContactId,
					(int) personPremiseRoleType.daytimeContact,
					0,
					systemUserId);

			if (DetailsManagerId > 0)
				thisPersonPremiseRole.Add(
					thisPremiseId,
					DetailsManagerId,
					(int) personPremiseRoleType.detailsManager,
					0,
					systemUserId);

			thisPersonPremiseRole.Save();

			#endregion

			#region Add SPPRs

			clsSPPremiseRole thisSPPremiseRole = new clsSPPremiseRole(thisDbType, localRecords.dbConnection);

			if (AlarmMonitorId > 0)
				thisSPPremiseRole.Add(
					thisPremiseId,
					AlarmMonitorId,
					(int) sPPremiseRoleType.alarmMonitor,
					systemUserId);

			if (AlarmResponseId > 0)
				thisSPPremiseRole.Add(
					thisPremiseId,
					AlarmResponseId,
					(int) sPPremiseRoleType.alarmResponse,
					systemUserId);

			if (PatrolId > 0)
				thisSPPremiseRole.Add(
					thisPremiseId,
					PatrolId,
					(int) sPPremiseRoleType.patrol,
					systemUserId);

			thisSPPremiseRole.Save();

			#endregion
			
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
			thisPremise = new clsPremise(thisDbType, localRecords.dbConnection);
			thisPremise.GetByPremiseId(thisPkId);

			AddGeneral(thisPremise.my_CustomerId(0),
				thisPremise.my_ItemId(0), 
				thisPremise.my_ProductId(0), 
				thisPremise.my_PremiseNumber(0), 
				thisPremise.my_CompanyAtPremiseName(0),
				thisPremise.my_QuickPhysicalAddress(0),
				thisPremise.my_CompanyTypeName(0), 
				thisPremise.my_Url(0), 
				thisPremise.my_AlarmDetails(0), 
				thisPremise.my_KdlComments(0), 
				thisPremise.my_CustomerComments(0), 
				thisPremise.my_DateStart(0), 
				thisPremise.my_DateSubscriptionExpires(0), 
				thisPremise.my_DateNextSubscriptionDueToBeInvoiced(0), 
				thisPremise.my_DateLastDetailsUpdate(0), 
				thisPremise.my_DateLastInvoice(0), 
				thisPremise.my_DateLastCopyOfInvoice(0), 
				thisPremise.my_DateLastStatement(0), 
				thisPremise.my_StickerRequired(0), 
				thisPremise.my_InvoiceRequired(0),
				thisPremise.my_CopyOfInvoiceRequired(0),
				thisPremise.my_StatementRequired(0),
				thisPremise.my_DetailsUpdateRequired(0), 
				thisPremise.my_ChangeDataId(0),
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisPremise.my_ChangeData_CreatedByUserId(0),
				thisPremise.my_ChangeData_CreatedByFirstNameLastName(0),
				thisPremise.my_ChangeData_DateCreated(0),
				CurrentUser,"",dtaNow.ToString());
							
			thisChangeData.Save();

			return thisChangeData.LastIdAdded();
	
		}

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Incident table stack; the SaveIncidents method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CustomerId">CustomerId</param>
		/// <param name="ItemId">ItemId</param>
		/// <param name="ProductId">ProductId</param>
		/// <param name="PremiseNumber">PremiseNumber</param>
		/// <param name="CompanyAtPremiseName">CompanyAtPremiseName</param>
		/// <param name="QuickPhysicalAddress">QuickPhysicalAddress</param>
		/// <param name="CompanyTypeName">CompanyTypeName</param>
		/// <param name="Url">Url</param>
		/// <param name="AlarmDetails">AlarmDetails</param>
		/// <param name="KdlComments">Comments made by KDL about this Premise</param>
		/// <param name="CustomerComments">Comments made by the Customer about this Premise</param>
		/// <param name="DateStart">DateStart</param>
		/// <param name="DateSubscriptionExpires">DateSubscriptionExpires</param>
		/// <param name="DateNextSubscriptionDueToBeInvoiced">DateNextSubscriptionDueToBeInvoiced</param>
		/// <param name="DateLastDetailsUpdate">DateLastDetailsUpdate</param>
		/// <param name="DateLastInvoice">Date the Last Invoice was sent</param>
		/// <param name="DateLastCopyOfInvoice">Date the Last Copy of Invoice was sent</param>
		/// <param name="DateLastStatement">Date the Last Statement was sent</param>
		/// <param name="StickerRequired">Whether a Sticker is required in the next batch of Correspondence for this Premise</param>
		/// <param name="InvoiceRequired">Whether an Invoice is required in the next batch of Correspondence for this Premise</param>
		/// <param name="CopyOfInvoiceRequired">Whether a copy of an Invoice is required in the next batch of Correspondence for this Premise</param>
		/// <param name="StatementRequired">Whether a Statement is required in the next batch of Correspondence for this Premise</param>
		/// <param name="DetailsUpdateRequired">Whether a Details Update is required in the next batch of Correspondence for this Premise</param>
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		public void AddGeneral(int CustomerId,
			int ItemId,
			int ProductId,
			string PremiseNumber, 
			string CompanyAtPremiseName,
			string QuickPhysicalAddress,
			string CompanyTypeName,
			string Url,
			string AlarmDetails,
			string KdlComments,
			string CustomerComments,
			
			string DateStart,
			string DateSubscriptionExpires,
			string DateNextSubscriptionDueToBeInvoiced,
			string DateLastDetailsUpdate,
			string DateLastInvoice,
			string DateLastCopyOfInvoice,
			string DateLastStatement,

			int StickerRequired,
			int InvoiceRequired,
			int CopyOfInvoiceRequired,
			int StatementRequired,
			int DetailsUpdateRequired,
			int ChangeDataId,
			int Archive)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			#region Deal with CompanyTypeName

			int CompanyTypeId = 0;

			clsCompanyType thisCompanyType = new clsCompanyType(thisDbType, localRecords.dbConnection);

			if (thisCompanyType.GetByCompanyTypeName(CompanyTypeName, 
				(int) matchCriteria.exactMatch) != 0)
				CompanyTypeId = thisCompanyType.my_CompanyTypeId(0);
			else
			{
				thisCompanyType.Add(CompanyTypeName, 1);
				thisCompanyType.Save();
				CompanyTypeId = thisCompanyType.LastIdAdded();
			}


			rowToAdd["CompanyTypeId"] = CompanyTypeId;
			rowToAdd["CompanyTypeName"] = CompanyTypeName;

			#endregion

			rowToAdd["PremiseNumber"] = PremiseNumber;
			rowToAdd["CustomerId"] = CustomerId;

			if (ItemId == 0)
				rowToAdd["ItemId"] = DBNull.Value;
			else
				rowToAdd["ItemId"] = ItemId;

			if (ProductId == 0)
				rowToAdd["ProductId"] = DBNull.Value;
			else
				rowToAdd["ProductId"] = ProductId;

			rowToAdd["PremiseNumber"] = PremiseNumber;
			rowToAdd["CompanyAtPremiseName"] = CompanyAtPremiseName;
			rowToAdd["QuickPhysicalAddress"] = QuickPhysicalAddress;
			rowToAdd["CompanyTypeId"] = CompanyTypeId;
			rowToAdd["Url"] = Url;
			rowToAdd["AlarmDetails"] = AlarmDetails;
			rowToAdd["CustomerComments"] = CustomerComments;
			rowToAdd["DateStart"] = SanitiseDate(DateStart);
			rowToAdd["DateSubscriptionExpires"] = SanitiseDate(DateSubscriptionExpires);
			rowToAdd["DateNextSubscriptionDueToBeInvoiced"] = SanitiseDate(DateNextSubscriptionDueToBeInvoiced);
			rowToAdd["DateLastDetailsUpdate"] = SanitiseDate(DateLastDetailsUpdate);
			rowToAdd["DateLastInvoice"] = SanitiseDate(DateLastInvoice);
			rowToAdd["DateLastCopyOfInvoice"] = SanitiseDate(DateLastCopyOfInvoice);
			rowToAdd["DateLastStatement"] = SanitiseDate(DateLastStatement);
			rowToAdd["KdlComments"] = KdlComments;
			rowToAdd["StickerRequired"] = StickerRequired;
			rowToAdd["InvoiceRequired"] = InvoiceRequired;
			rowToAdd["CopyOfInvoiceRequired"] = CopyOfInvoiceRequired;
			rowToAdd["StatementRequired"] = StatementRequired;
			rowToAdd["DetailsUpdateRequired"] = DetailsUpdateRequired;

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
		/// are available through the ErrorMessage and ErrorFieldNumber methods
		/// If any Warnings are found, WarningFound will return true, and these Warnings 
		/// are available through the WarningMessage and WarningFieldNumber methods
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PremiseId">PremiseId (Primary Key of Record)</param>
		/// <param name="PremiseNumber">Premise's Number</param>
		/// <param name="CustomerId">Associated Customer</param>
		/// <param name="CompanyAtPremiseName">Company Located at the Premise</param>
		/// <param name="CompanyTypeName">Type of Company Located at the Premise</param>
		/// <param name="ItemId">Associated Item</param>
		/// <param name="ProductId">Associated Product</param>
		/// <param name="Url">Url</param>
		/// <param name="AlarmDetails">Details of the Alarm</param>
		/// <param name="KdlComments">Comments made by KDL about this Premise</param>
		/// <param name="CustomerComments">Comments made by the Customer about this Premise</param>
		/// <param name="QuickPhysicalAddress">QuickPhysicalAddress of this Premise</param>
		/// <param name="DateStart">DateStart</param>
		/// <param name="DateSubscriptionExpires">DateSubscriptionExpires</param>
		/// <param name="DateNextSubscriptionDueToBeInvoiced">Date the Next Subscription is Due for this Premise</param>
		/// <param name="DateLastDetailsUpdate">Date the Last Details Update was sent</param>
		/// <param name="DateLastInvoice">Date the Last Invoice was sent</param>
		/// <param name="DateLastCopyOfInvoice">Date the Last Copy of Invoice was sent</param>
		/// <param name="DateLastStatement">Date the Last Statement was sent</param>
		/// <param name="StickerRequired">Whether a Sticker is required in the next batch of Correspondence for this Premise</param>
		/// <param name="InvoiceRequired">Whether an Invoice is required in the next batch of Correspondence for this Premise</param>
		/// <param name="CopyOfInvoiceRequired">Whether a copy of an Invoice is required in the next batch of Correspondence for this Premise</param>
		/// <param name="StatementRequired">Whether a Statement is required in the next batch of Correspondence for this Premise</param>
		/// <param name="DetailsUpdateRequired">Whether a Details Update is required in the next batch of Correspondence for this Premise</param>
		/// <param name="CurrentUser">Current Logged In User</param>
		public void Modify(int PremiseId, 
			int CustomerId,
			int ItemId,
			int ProductId,
			string PremiseNumber, 
			string CompanyAtPremiseName,
			string QuickPhysicalAddress,
			string CompanyTypeName,
			string Url,
			string AlarmDetails,
			string KdlComments,
			string CustomerComments,
			
			string DateStart,
			string DateSubscriptionExpires,
			string DateNextSubscriptionDueToBeInvoiced,
			string DateLastDetailsUpdate,
			string DateLastInvoice,
			string DateLastCopyOfInvoice,
			string DateLastStatement,

			int StickerRequired,
			int InvoiceRequired,
			int CopyOfInvoiceRequired,
			int StatementRequired,
			int DetailsUpdateRequired,
			int CurrentUser)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, PremiseId);

			#region Deal with CompanyTypeName

			int CompanyTypeId = 0;

			clsCompanyType thisCompanyType = new clsCompanyType(thisDbType, localRecords.dbConnection);

			if (thisCompanyType.GetByCompanyTypeName(CompanyTypeName, 
				(int) matchCriteria.exactMatch) != 0)
				CompanyTypeId = thisCompanyType.my_CompanyTypeId(0);
			else
			{
				thisCompanyType.Add(CompanyTypeName, 1);
				thisCompanyType.Save();
				CompanyTypeId = thisCompanyType.LastIdAdded();
			}


			rowToAdd["CompanyTypeId"] = CompanyTypeId;
			rowToAdd["CompanyTypeName"] = CompanyTypeName;

			#endregion

			rowToAdd["PremiseId"] = PremiseId;
			rowToAdd["CustomerId"] = CustomerId;

			if (ItemId == 0)
				rowToAdd["ItemId"] = DBNull.Value;
			else
				rowToAdd["ItemId"] = ItemId;

			if (ProductId == 0)
				rowToAdd["ProductId"] = DBNull.Value;
			else
				rowToAdd["ProductId"] = ProductId;

			rowToAdd["PremiseNumber"] = PremiseNumber;
			rowToAdd["CompanyAtPremiseName"] = CompanyAtPremiseName;
			rowToAdd["QuickPhysicalAddress"] = QuickPhysicalAddress;
			rowToAdd["CompanyTypeId"] = CompanyTypeId;
			rowToAdd["Url"] = Url;
			rowToAdd["AlarmDetails"] = AlarmDetails;
			
			rowToAdd["DateStart"] = SanitiseDate(DateStart);
			rowToAdd["DateSubscriptionExpires"] = SanitiseDate(DateSubscriptionExpires);
			rowToAdd["DateNextSubscriptionDueToBeInvoiced"] = SanitiseDate(DateNextSubscriptionDueToBeInvoiced);
			rowToAdd["DateLastDetailsUpdate"] = SanitiseDate(DateLastDetailsUpdate);
			rowToAdd["DateLastInvoice"] = SanitiseDate(DateLastInvoice);
			rowToAdd["DateLastCopyOfInvoice"] = SanitiseDate(DateLastCopyOfInvoice);
			rowToAdd["DateLastStatement"] = SanitiseDate(DateLastStatement);


			rowToAdd["KdlComments"] = KdlComments;
			rowToAdd["CustomerComments"] = CustomerComments;

			rowToAdd["StickerRequired"] = StickerRequired;
			rowToAdd["InvoiceRequired"] = InvoiceRequired;
			rowToAdd["CopyOfInvoiceRequired"] = CopyOfInvoiceRequired;
			rowToAdd["StatementRequired"] = StatementRequired;
			rowToAdd["DetailsUpdateRequired"] = DetailsUpdateRequired;
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
		/// are available through the ErrorMessage and ErrorFieldNumber methods
		/// If any Warnings are found, WarningFound will return true, and these Warnings 
		/// are available through the WarningMessage and WarningFieldNumber methods		
		/// </summary>
		/// <param name="valuesToValidate">Values to be Validated.</param>
		/// <param name="newRow">Indicates whether the Row being validated 
		/// is new or already exists in the system</param>
		private void Validate(System.Data.DataRow valuesToValidate, bool newRow)
		{
			//TODO: Add any required Validation here
		}


		#endregion

		#region Remove

		/// <summary>
		/// Remove a Record
		/// </summary>
		/// <param name="thisPkId">PK of Record to Remove</param>
		/// <param name="CurrentUser">Id of User doing the Removing</param>
		public override void Remove(int thisPkId, int CurrentUser)
		{
			base.Remove(thisPkId, CurrentUser);

			clsPersonPremiseRole thisPersonPremiseRole = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);
			clsPersonPremiseRole thisRemovePPR = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);

			int numRoles = thisPersonPremiseRole.GetByPremiseId(thisPkId);

			for(int counter = 0; counter < numRoles; counter++)
				thisRemovePPR.Remove(thisPersonPremiseRole.my_PersonPremiseRoleId(counter), CurrentUser);
			thisRemovePPR.Save();

		}

		#endregion

		# region My_ Values Premise

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
		/// <see cref="clsCustomer.my_CustomerId">Id</see> of
		/// <see cref="clsCustomer">Customer</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> of 
		/// <see cref="clsCustomer">Customer</see> 
		/// </returns>
		public int my_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerId"));
		}

		/// <summary>
		/// <see cref="clsItem.my_ItemId">Id</see> of
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_ItemId">Id</see> of 
		/// <see cref="clsItem">Item</see> 
		/// </returns>
		public int my_ItemId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ItemId"));
		}


		/// <summary>
		/// <see cref="clsProduct.my_ProductId">Id</see> of
		/// <see cref="clsProduct">Product</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductId">Id</see> of 
		/// <see cref="clsProduct">Product</see> 
		/// </returns>
		public int my_ProductId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ProductId"));
		}
		
		/// <summary>
		/// <see cref="clsCompanyType.my_CompanyTypeId">Id</see> of
		/// <see cref="clsCompanyType">CompanyType</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCompanyType.my_CompanyTypeId">Id</see> of 
		/// <see cref="clsCompanyType">CompanyType</see> 
		/// </returns>
		public int my_CompanyTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CompanyTypeId"));
		}		

		/// <summary>
		/// <see cref="clsCompanyType.my_CompanyTypeName">Name</see> of
		/// <see cref="clsCompanyType">CompanyType</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCompanyType.my_CompanyTypeName">Name</see> of 
		/// <see cref="clsCompanyType">CompanyType</see> 
		/// </returns>
		public string my_CompanyTypeName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CompanyTypeName");
		}

		/// <summary>
		/// <see cref="clsPremise.my_PremiseNumber">Number</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_PremiseNumber">Number</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_PremiseNumber(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PremiseNumber");
		}

		/// <summary>
		/// <see cref="clsPremise.my_Url">Url</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_Url">Url</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_Url(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Url");
		}

		/// <summary>
		/// <see cref="clsPremise.my_QuickPhysicalAddress">Quick Physical thisAddress</see> of
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_QuickPhysicalAddress">Quick Physical thisAddress</see> of 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>	
		public string my_QuickPhysicalAddress(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "QuickPhysicalAddress");
		}

		/// <summary>
		/// <see cref="clsPremise.my_CompanyAtPremiseName">Name of Company</see> at
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_CompanyAtPremiseName">Name of Company</see> at 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_CompanyAtPremiseName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CompanyAtPremiseName");
		}


		/// <summary>
		/// <see cref="clsPremise.my_AlarmDetails">Details of Alarm</see> at
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_AlarmDetails">Details of Alarm</see> at 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_AlarmDetails(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "AlarmDetails");
		}

		/// <summary>
		/// <see cref="clsPremise.my_KdlComments">Comments by KDL</see> about
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_KdlComments">Comments by KDL</see> about 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_KdlComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "KdlComments");
		}

		/// <summary>
		/// <see cref="clsPremise.my_CustomerComments">Comments by Customer</see> about
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_CustomerComments">Comments by Customer</see> about 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_CustomerComments(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerComments");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DateStart">Date Next Subscription is Due</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateStart">Date Next Subscription is Due</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_DateStart(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateStart");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DateSubscriptionExpires">Date Next Subscription is Due</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateSubscriptionExpires">Date Next Subscription is Due</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_DateSubscriptionExpires(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSubscriptionExpires");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DateNextSubscriptionDueToBeInvoiced">Date Next Subscription is due to be invoiced</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateNextSubscriptionDueToBeInvoiced">Date Next Subscription is due to be invoiced</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_DateNextSubscriptionDueToBeInvoiced(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateNextSubscriptionDueToBeInvoiced");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DateLastDetailsUpdate">Date Last Detail Update was sent</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateLastDetailsUpdate">Date Last Detail Update was sent</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_DateLastDetailsUpdate(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateLastDetailsUpdate");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DateLastInvoice">Date Last Detail Update was sent</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateLastInvoice">Date Last Detail Update was sent</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_DateLastInvoice(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateLastInvoice");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DateLastCopyOfInvoice">Date Last Detail Update was sent</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateLastCopyOfInvoice">Date Last Detail Update was sent</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_DateLastCopyOfInvoice(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateLastCopyOfInvoice");
		}

		/// <summary>
		/// <see cref="clsPremise.my_DateLastStatement">Date Last Detail Update was sent</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DateLastStatement">Date Last Detail Update was sent</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public string my_DateLastStatement(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateLastStatement");
		}


		
		/// <summary>
		/// <see cref="clsPremise.my_StickerRequired">Whether a Sticker is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_StickerRequired">Whether a Sticker is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_StickerRequired(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "StickerRequired"));
		}

		/// <summary>
		/// <see cref="clsPremise.my_InvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_InvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_InvoiceRequired(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "InvoiceRequired"));
		}

		/// <summary>
		/// <see cref="clsPremise.my_CopyOfInvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_CopyOfInvoiceRequired">Whether a Invoice is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_CopyOfInvoiceRequired(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CopyOfInvoiceRequired"));
		}


		/// <summary>
		/// <see cref="clsPremise.my_StatementRequired">Whether a Statement is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_StatementRequired">Whether a Statement is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_StatementRequired(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "StatementRequired"));
		}


		/// <summary>
		/// <see cref="clsPremise.my_DetailsUpdateRequired">Whether a Details Update is Required in the next batch of correspondence for this Premise</see> for
		/// <see cref="clsPremise">Premise</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_DetailsUpdateRequired">Whether a Details Update is Required in the next batch of correspondence for this Premise</see> for 
		/// <see cref="clsPremise">Premise</see> 
		/// </returns>
		public int my_DetailsUpdateRequired(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "DetailsUpdateRequired"));
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

		# region My_ Values CustomerGroup

		/// <summary>
		/// <see cref="clsCustomerGroup.my_CustomerGroupName">Name</see> of
		/// <see cref="clsCustomerGroup">CustomerGroup</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomerGroup.my_CustomerGroupName">Name</see> of 
		/// <see cref="clsCustomerGroup">CustomerGroup</see> 
		/// </returns>
		public string my_CustomerGroup_CustomerGroupName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerGroup_CustomerGroupName");
		}

		#endregion

		#region My_ Values Derrived

		/// <summary>
		/// Number of
		/// <see cref="clsPersonPremiseRole">Person Premise Roles</see>
		/// Associated with this Premise
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of 
		/// <see cref="clsPersonPremiseRole">Person Premise Role</see> 
		/// for this Premise</returns>
		public string my_NumPersonPremiseRoles(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "_NumPersonPremiseRoles");
		}

		/// <summary>
		/// Number of
		/// <see cref="clsSPPremiseRole">Service Provider Premise Roles</see>
		/// Associated with this Premise
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of 
		/// <see cref="clsSPPremiseRole">Service Provider Premise Roles</see> 
		/// for this Premise</returns>
		public string my_NumSPPremiseRoles(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "NumSPPremiseRoles");
		}

		/// <summary>
		/// Number of
		/// <see cref="clsPersonPremiseRole">PersonPremiseRoles</see>
		/// Associated with this Premise that are not related to security
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of 
		/// <see cref="clsPersonPremiseRole">PersonPremiseRole</see> 
		/// for this Premise</returns>
		public string my_NumNonSecurityRoles(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "NumNonSecurityRoles");
		}



		/// <summary>
		/// Number of People with at least one
		/// <see cref="clsPersonPremiseRole">PersonPremiseRole</see>
		/// Associated with this Premise
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of People with at least one of 
		/// <see cref="clsPersonPremiseRole">PersonPremiseRole</see> 
		/// for this Premise</returns>
		public string my_NumPeopleWithAtLeastOneRoleAtPremise(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "NumPeopleWithAtLeastOneRoleAtPremise");
		}

		#endregion

		#region My_ Values HighRiskGoods

		/// <summary>
		/// Number of
		/// <see cref="clsHighRiskGood">HighRiskGoods</see>
		/// Associated with this Premise
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Number of 
		/// <see cref="clsHighRiskGood">HighRiskGood</see> 
		/// for this Premise</returns>
		public string my_NumHighRiskGoods(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "NumHighRiskGoods");
		}


		#endregion

		#region My_ Values Display

		/// <summary>
		/// Name for Auto-completing Drop Down Display
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Name for Auto-completing Drop Down Display</returns>
		public string my_DisplayName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DisplayName");
		}

		# endregion

		# region My_ Values Item

		/// <summary>
		/// <see cref="clsOrder.my_OrderId">Id</see> of 
		/// <see cref="clsOrder">Order</see>
		/// Associated with this Item</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsOrder.my_OrderId">Id</see> 
		/// of <see cref="clsOrder">Order</see> 
		/// for this Item</returns>	
		public int my_Item_OrderId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_OrderId"));
		}

		/// <summary>
		/// <see cref="clsProduct.my_ProductId">Id</see> of 
		/// <see cref="clsProduct">Product</see>
		/// Associated with this Item</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsProduct.my_ProductId">Id</see> 
		/// of <see cref="clsProduct">Product</see> 
		/// for this Item</returns>	
		public int my_Item_ProductId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_ProductId"));
		}


		/// <summary>
		/// <see cref="clsPremise.my_PremiseId">Id</see> of 
		/// <see cref="clsPremise">Premise</see>
		/// Associated with this Item</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPremise.my_PremiseId">Id</see> 
		/// of <see cref="clsPremise">Premise</see> 
		/// for this Item</returns>	
		public int my_Item_PremiseId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_PremiseId"));
		}

		/// <summary>
		/// <see cref="clsItem.my_Quantity">Quantity of Item in Order</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_Quantity">Quantity of Item in Order</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>	
		public decimal my_Item_Quantity(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_Quantity"));
		}

		/// <summary>
		/// <see cref="clsItem.my_ItemName">Name</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_ItemName">Name</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>
		public string my_Item_ItemName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Item_ItemName");
		}

		/// <summary>
		/// <see cref="clsItem.my_ItemCode">Code</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_ItemCode">Code</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>
		public string my_Item_ItemCode(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Item_ItemCode");
		}

		/// <summary>
		/// <see cref="clsItem.my_ShortDescription">Short Description</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_ShortDescription">Short Description</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>
		public string my_Item_ShortDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Item_ShortDescription");
		}

		/// <summary>
		/// <see cref="clsItem.my_LongDescription">Long Description</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_LongDescription">Long Description</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>
		public string my_Item_LongDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Item_LongDescription");
		}

		/// <summary>
		/// <see cref="clsItem.my_Cost">Cost (for display)</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_Cost">Cost (for display)</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>
		public decimal my_Item_Cost(int rowNum)
		{
			
			//If the Cost shown includes the local tax rate, 
			//then the Cost we have been given also includes the local tax rate.
			//But we always store the Cost without tax...
			if (priceShownIncludesLocalTaxRate)
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_Cost")) * localTaxRate;
			else
				return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_Cost"));
		}

		/// <summary>
		/// <see cref="clsItem.my_CostIncludingTax">Cost (Including Tax)</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_CostIncludingTax">Cost (Including Tax)</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>
		public decimal my_Item_CostIncludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_Cost")) * localTaxRate;
		}

		/// <summary>
		/// <see cref="clsItem.my_CostExcludingTax">Cost (Excluding Tax)</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_CostExcludingTax">Cost (Excluding Tax)</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>
		public decimal my_Item_CostExcludingTax(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_Cost"));
		}
		
		/// <summary>
		/// <see cref="clsItem.my_InvoiceCost">Cost (For an Invoice)</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_InvoiceCost">Cost (For an Invoice)</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>
		public decimal my_Item_InvoiceCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_Cost"));
		}

		/// <summary>
		/// <see cref="clsItem.my_Weight">Weight</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_Weight">Weight</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>
		public decimal my_Item_Weight(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_Weight"));
		}


		/// <summary>
		/// <see cref="clsItem.my_FreightCost">Freight Cost</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_FreightCost">Freight Cost</see> 
		/// of <see cref="clsItem">Item</see> 
		/// </returns>
		public decimal my_Item_FreightCost(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_FreightCost"));
		}

		/// <summary>
		/// Whether this 
		/// <see cref="clsItem">Item</see> uses
		/// <see cref="clsItem.my_UseStockControl"> Stock Control</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this 
		/// <see cref="clsItem">Item</see> uses 
		/// <see cref="clsItem.my_UseStockControl"> Stock Control</see></returns>
		public int my_Item_UseStockControl(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_UseStockControl"));
		}

		/// <summary>
		/// <see cref="clsItem.my_QuantityAvailable">Quantity Available</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_QuantityAvailable">Quantity Available</see> of 
		/// <see cref="clsItem">Item</see></returns>
		public decimal my_Item_QuantityAvailable(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_QuantityAvailable"));
		}

		/// <summary>
		/// <see cref="clsItem.my_MaxQuantity">Maximum Quantity</see> of 
		/// <see cref="clsItem">Item</see> purchasable at one time
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_MaxQuantity">Maximum Quantity</see> of 
		/// <see cref="clsItem">Item</see>  purchasable at one time</returns>
		public decimal my_Item_MaxQuantity(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_MaxQuantity"));
		}

		/// <summary>
		/// <see cref="clsItem.my_MaxAllowable">Maximum Allowable</see> Quantity of 
		/// <see cref="clsItem">Item</see> purchasable at this time 
		/// (uses MaxQuantity, UsesStockControl and QuantityAvailable to calculate this)
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_MaxAllowable">Maximum Allowable</see> of 
		/// <see cref="clsItem">Item</see>  purchasable at this time</returns>
		public decimal my_Item_MaxAllowable(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_MaxAllowable"));
		}

		/// <summary>
		/// Whether this 
		/// <see cref="clsItem">Item</see>  is
		/// <see cref="clsItem.my_WholeNumbersOnly">discrete or continuous</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this 
		/// <see cref="clsItem">Item</see>  is 
		/// <see cref="clsItem.my_WholeNumbersOnly">discrete or continuous</see></returns>
		public int my_Item_WholeNumbersOnly(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_WholeNumbersOnly"));
		}

		/// <summary>
		/// <see cref="clsItem.my_QuantityDescription">Quantity Description</see> of 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_QuantityDescription">Quantity Description</see> of 
		/// <see cref="clsItem">Item</see></returns>
		public string my_Item_QuantityDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Item_QuantityDescription");
		}

		/// <summary>
		/// <see cref="clsItem.my_MaxKeyholdersPerPremise">Maximum number of Keyholders per Premise</see>
		///  allowed for this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsItem.my_MaxKeyholdersPerPremise">Maximum number of Keyholders per Premise</see>
		///  allowed for this 
		/// <see cref="clsItem">Item</see></returns>
		public int my_Item_MaxKeyholdersPerPremise(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_MaxKeyholdersPerPremise"));
		}

		/// <summary>
		/// <see cref="clsItem.my_MaxAssetRegisterAssets">Maximum number of Assets in the Asset Register</see>
		///  allowed for this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsItem.my_MaxAssetRegisterAssets">Maximum number of Assets in the Asset Register</see>
		///  allowed for this 
		/// <see cref="clsItem">Item</see></returns>	
		public int my_Item_MaxAssetRegisterAssets(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_MaxAssetRegisterAssets"));
		}

		/// <summary>
		/// <see cref="clsItem.my_MaxAssetRegisterStorage">Maximum Total Storage of Asset Register</see>
		///  allowed for this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsItem.my_MaxAssetRegisterStorage">Maximum Total Storage of Asset Register</see>
		///  allowed for this 
		/// <see cref="clsItem">Item</see></returns>	
		public int my_Item_MaxAssetRegisterStorage(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_MaxAssetRegisterStorage"));
		}

		/// <summary>
		/// <see cref="clsItem.my_MaxDocumentSafeDocuments">Maximum number of Documents in the Document Safe</see>
		///  allowed for this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsItem.my_MaxDocumentSafeDocuments">Maximum number of Documents in the Document Safe</see>
		///  allowed for this 
		/// <see cref="clsItem">Item</see></returns>	
		public int my_Item_MaxDocumentSafeDocuments(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_MaxDocumentSafeDocuments"));
		}


		/// <summary>
		/// <see cref="clsItem.my_MaxDocumentSafeStorage">Maximum number of Storage in the Document Safe</see>
		///  allowed for this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>		
		/// <see cref="clsItem.my_MaxDocumentSafeStorage">Maximum number of Storage in the Document Safe</see>
		///  allowed for this 
		/// <see cref="clsItem">Item</see></returns>	
		public int my_Item_MaxDocumentSafeStorage(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_MaxDocumentSafeStorage"));
		}

		/// <summary><see cref="clsItem.my_DurationUnitId">Time Unit's e.g. year, month, day 
		/// (if this is a Service over time)</see> for this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_DurationUnitId">Time Unit's e.g. year, month, day 
		/// (if this is a Service over time</see>) for this 
		/// <see cref="clsItem">Item</see>
		/// </returns>	
		public int my_Item_DurationUnitId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_DurationUnitId"));
		}


		/// <summary>
		/// <see cref="clsItem.my_DurationNumUnits">Number of Unit's duration</see> (if this is a Service over time) for this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsItem.my_DurationNumUnits">Number of Unit's duration</see> (if this is a Service over time) for this 
		/// <see cref="clsItem">Item</see>
		/// </returns>
		public decimal my_Item_DurationNumUnits(int rowNum)
		{
			return Convert.ToDecimal(localRecords.FieldByName(rowNum, "Item_DurationNumUnits"));
		}


		/// <summary>Whether this <see cref="clsItem">Item</see> 
		/// <see cref="clsItem.my_RequiresPremiseForActivation">Requires an associated Premise in order to be Activated</see>  
		/// (if this is a Premise/Time Related Item)</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether this <see cref="clsItem">Item</see> 
		/// <see cref="clsItem.my_RequiresPremiseForActivation">Requires an associated Premise in order to be Activated</see>  
		/// (if this is a Premise/Time Related Item)</returns>	
		public int my_Item_RequiresPremiseForActivation(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Item_RequiresPremiseForActivation"));
		}

		/// <summary>
		/// <see cref="clsItem.my_DateActivation">Date of Activation (Client Time) of</see> this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>	
		/// <see cref="clsItem.my_DateActivation">Date of Activation (Client Time) of</see> this 
		/// <see cref="clsItem">Item</see>
		/// </returns>	
		public string my_Item_DateActivation(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Item_DateActivation");
		}

		/// <summary>
		/// <see cref="clsItem.my_DateActivation">Date of Activation (UTC Time) of</see> this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>	
		/// <see cref="clsItem.my_DateActivation">Date of Activation (UTC Time) of</see> this 
		/// <see cref="clsItem">Item</see>
		/// </returns>
		public string my_Item_DateActivationUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Item_DateActivationUtc");
		}

		/// <summary>
		/// <see cref="clsItem.my_DateExpiry">Date of Expiry (Client Time) of</see> this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>	
		/// <see cref="clsItem.my_DateExpiry">Date of Expiry (Client Time) of</see> this 
		/// <see cref="clsItem">Item</see>
		/// </returns>	
		public string my_Item_DateExpiry(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Item_DateExpiry");
		}

		/// <summary>
		/// <see cref="clsItem.my_DateExpiry">Date of Expiry (UTC Time) of</see> this 
		/// <see cref="clsItem">Item</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>	
		/// <see cref="clsItem.my_DateExpiry">Date of Expiry (UTC Time) of</see> this 
		/// <see cref="clsItem">Item</see>
		/// </returns>
		public string my_Item_DateExpiryUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Item_DateExpiryUtc");
		}


		# endregion

	}
}
