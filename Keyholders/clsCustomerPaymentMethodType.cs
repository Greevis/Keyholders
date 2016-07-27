using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsCustomerPaymentMethodType deals with everything to do with data about CustomerPaymentMethodTypes.
	/// </summary>
	
	[GuidAttribute("39D939A5-22B1-4686-95F4-5E0F5A0C1C0E")]
	public class clsCustomerPaymentMethodType : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCustomerPaymentMethodType
		/// </summary>
		public clsCustomerPaymentMethodType() : base("CustomerPaymentMethodType")
		{
		}

		/// <summary>
		/// Constructor for clsCustomerPaymentMethodType; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsCustomerPaymentMethodType(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("CustomerPaymentMethodType")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Part of the Query that Pertains to PaymentMethodType Information
		/// </summary>
		public clsQueryPart PaymentMethodTypeQ = new clsQueryPart();

		/// <summary>
		/// Part of the Query that Pertains to "All PaymentMethodTypes And Customers" Information
		/// </summary>
		public clsQueryPart AllPaymentMethodTypesAndCustomers = new clsQueryPart();		
		
		/// <summary>
		/// Part of the Query that Pertains to Customer Information
		/// </summary>
		public clsQueryPart CustomerQ = new clsQueryPart();

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{

			CustomerQ = CustomerQueryPart();
			PaymentMethodTypeQ = PaymentMethodTypeQueryPart();

			MainQ.AddSelectColumn("tblCustomerPaymentMethodType.CustomerPaymentMethodTypeId");
			MainQ.AddSelectColumn("tblCustomerPaymentMethodType.CustomerId");
			MainQ.AddSelectColumn("tblCustomerPaymentMethodType.PaymentMethodTypeId");

			MainQ.AddFromTable(thisTable);

			AllPaymentMethodTypesAndCustomers.AddSelectColumn("tblCustomer.FirstName as Customer_FirstName");
			AllPaymentMethodTypesAndCustomers.AddSelectColumn("tblCustomer.LastName as Customer_LastName");
			AllPaymentMethodTypesAndCustomers.AddSelectColumn("tblCustomerPaymentMethodType.PaymentMethodTypeId");
			AllPaymentMethodTypesAndCustomers.AddSelectColumn("tblCustomerPaymentMethodType.CustomerId");
			AllPaymentMethodTypesAndCustomers.AddSelectColumn("tblCustomerPaymentMethodType.PaymentMethodTypeName as PaymentMethodType_PaymentMethodTypeName");
			AllPaymentMethodTypesAndCustomers.AddSelectColumn("tblCustomerPaymentMethodType.CustomerDisplayOrder as PaymentMethodType_CustomerDisplayOrder");
			AllPaymentMethodTypesAndCustomers.AddSelectColumn("tblCustomerPaymentMethodType.CustomerCanNotChoose as PaymentMethodType_CustomerCanNotChoose");

			AllPaymentMethodTypesAndCustomers.AddFromTable("(Select  tblPaymentMethodType.PaymentMethodTypeId," + crLf
				+ "	tblCustomerPaymentMethodType.CustomerId," + crLf
				+ " tblCustomerPaymentMethodType.CustomerDisplayOrder," + crLf
				+ " tblPaymentMethodType.PaymentMethodTypeName," + crLf
				+ " tblPaymentMethodType.CustomerCanNotChoose" + crLf
				+ "from tblPaymentMethodType left outer join tblCustomerPaymentMethodType " + crLf
				+ "	on tblPaymentMethodType.PaymentMethodTypeId = tblCustomerPaymentMethodType.PaymentMethodTypeId" + crLf
				+ ") tblCustomerPaymentMethodType" + crLf
				+ "left outer join tblCustomer on tblCustomer.CustomerId = tblCustomerPaymentMethodType.CustomerId");			
			
			
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = CustomerQ;
			queries[2] = PaymentMethodTypeQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblPaymentMethodType.PaymentMethodTypeName" + crLf;

		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsCustomerPaymentMethodType
		/// </summary>
		public override void Initialise()
		{
			
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("CustomerId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("PaymentMethodTypeId", System.Type.GetType("System.Int32"));
			
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
		/// Initialises an internal list of all Customer-In-PaymentMethodTypes
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = CustomerQ;
			queries[2] = PaymentMethodTypeQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Initialises an internal list of all Customer-In-PaymentMethodTypes
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAllCustomersInPaymentMethodType()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
	
			queries[0] = AllPaymentMethodTypesAndCustomers;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a Customer-In-PaymentMethodType by CustomerPaymentMethodTypeId
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerPaymentMethodTypeId(int CustomerPaymentMethodTypeId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = CustomerQ;
			queries[2] = PaymentMethodTypeQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ CustomerPaymentMethodTypeId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;
			
			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets Customer-In-PaymentMethodTypes by CustomerId
		/// </summary>
		/// <param name="CustomerId">Id of Customer to retrieve Customer-In-PaymentMethodTypes for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = CustomerQ;
			queries[2] = PaymentMethodTypeQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblCustomerPaymentMethodType.CustomerId = " 
				+ CustomerId.ToString() + crLf;

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
		/// Gets All Payment Method Types where CustomerCanNotChoose is 0, and whether a Customer can choose them or not
		/// </summary>
		/// <param name="CustomerId">Id of Customer to retrieve Customer-In-PaymentMethodTypes for</param>
		/// <returns>Number of resulting records</returns>
		public int GetPaymentMethodTypesForCustomerId(int CustomerId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = PaymentMethodTypeQ;
			queries[0].AddSelectColumn("tblPaymentMethodType.PaymentMethodTypeId");
			queries[0].AddSelectColumn("case when count(tblCustomerPaymentMethodType.CustomerPaymentMethodTypeId) > 0 then 1 else 0 end as Allowed");
			queries[0].FromTables.Clear();
			queries[0].AddFromTable("(select * from tblPaymentMethodType where CustomerCanNotChoose = 0) tblPaymentMethodType" + crLf
				+ "left outer join (select * from tblCustomerPaymentMethodType where CustomerId = "
				+ CustomerId.ToString() + ") tblCustomerPaymentMethodType " + crLf
				+ "on tblPaymentMethodType.PaymentMethodTypeId = tblCustomerPaymentMethodType.PaymentMethodTypeId" + crLf);

			queries[0].WhereConditions.Clear();
			queries[0].Joins.Clear();

			queries[0].AddGroupBy("tblPaymentMethodType.PaymentMethodTypeId");
			queries[0].AddGroupBy("tblPaymentMethodType.PaymentMethodTypeName");
			queries[0].AddGroupBy("tblPaymentMethodType.PaymentMethodTypeDescription");
			queries[0].AddGroupBy("tblPaymentMethodType.CustomerNotePreSale");
			queries[0].AddGroupBy("tblPaymentMethodType.CustomerNotePostSale");
			queries[0].AddGroupBy("tblPaymentMethodType.CustomerNoteEmailHtml");
			queries[0].AddGroupBy("tblPaymentMethodType.CustomerNoteEmailPlainText");
			queries[0].AddGroupBy("tblPaymentMethodType.CustomerCanNotChoose");
			queries[0].AddGroupBy("tblPaymentMethodType.AllowCustomerToUseByDefault");

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Customer-In-PaymentMethodTypes by PaymentMethodTypeId
		/// </summary>
		/// <param name="PaymentMethodTypeId">Id of Customer PaymentMethodType to retrieve Customer-In-PaymentMethodTypes for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPaymentMethodTypeId(int PaymentMethodTypeId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[3];
			
			queries[0] = MainQ;
			queries[1] = PaymentMethodTypeQ;
			queries[2] = CustomerQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where " + thisTable + ".PaymentMethodTypeId = " 
				+ PaymentMethodTypeId.ToString() + crLf;

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
		/// Gets Customer-In-PaymentMethodTypes by CustomerId and PaymentMethodTypeId
		/// </summary>
		/// <param name="CustomerId">Id of Customer to retrieve Customer-In-PaymentMethodTypes for</param>
		/// <param name="PaymentMethodTypeId">Id of Customer PaymentMethodType to retrieve Customer-In-PaymentMethodTypes for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCustomerIdAndPaymentMethodTypeId(int CustomerId, int PaymentMethodTypeId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			string condition = "(Select * from " + thisTable + crLf;	

			//Additional Condition
			condition += "Where tblCustomerPaymentMethodType.CustomerId = " 
				+ CustomerId.ToString() + crLf;

			condition += "And tblCustomerPaymentMethodType.PaymentMethodTypeId = " 
				+ PaymentMethodTypeId.ToString() + crLf;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				condition + ") " + thisTable,
				thisTable
				);


			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal customer table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CustomerId">Customer Associated with this CustomerPaymentMethodType</param>
		/// <param name="PaymentMethodTypeId">Customer PaymentMethodType Associated with this CustomerPaymentMethodType</param>
		public void Add(int CustomerId, 
			int PaymentMethodTypeId)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["PaymentMethodTypeId"] = PaymentMethodTypeId;
			
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
		/// <param name="CustomerPaymentMethodTypeId">CustomerPaymentMethodTypeId (Primary Key of Record)</param>
		/// <param name="CustomerId">Customer Associated with this CustomerPaymentMethodType</param>
		/// <param name="PaymentMethodTypeId">PaymentMethodType Associated with this CustomerPaymentMethodType</param>
		public void Modify(int CustomerPaymentMethodTypeId, 
			int CustomerId, 
			int PaymentMethodTypeId)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["CustomerPaymentMethodTypeId"] = CustomerPaymentMethodTypeId;
			rowToAdd["CustomerId"] = CustomerId;
			rowToAdd["PaymentMethodTypeId"] = PaymentMethodTypeId;

			//Validate the data supplied
			Validate(rowToAdd, false);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);
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
		/// <param name="CustomerId">Customer Associated with this CustomerPaymentMethodType</param>
		/// <param name="PaymentMethodTypeId">Customer PaymentMethodType Associated with this CustomerPaymentMethodType</param>
		public override void Remove(int CustomerId, 
			int PaymentMethodTypeId)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Find this CustomerPaymentMethodTypeId
			clsCustomerPaymentMethodType thisCPM = new clsCustomerPaymentMethodType(thisDbType, localRecords.dbConnection);

			int numRecords = thisCPM.GetByCustomerIdAndPaymentMethodTypeId(CustomerId, PaymentMethodTypeId);

			if (numRecords == 1)
			{
				thisCPM.Delete(thisCPM.my_CustomerPaymentMethodTypeId(0));
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
		/// <param name="CustomerId">Customer Associated with this CustomerPaymentMethodType</param>
		public void RemoveAllForCustomer(int CustomerId)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Find this CustomerPaymentMethodTypeId
			clsCustomerPaymentMethodType thisCPM = new clsCustomerPaymentMethodType(thisDbType, localRecords.dbConnection);

			int numRecords = thisCPM.GetByCustomerId(CustomerId);

			if (numRecords > 0)
				thisCPM.localRecords.RemoveRecordById(thisCPM.thisPk, thisCPM.thisTable);

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

		# region My_ Values CustomerPaymentMethodType

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_PaymentMethodTypeId">PaymentMethodTypeId</see> of 
		/// <see cref="clsPaymentMethodType">Customer PaymentMethodType</see>
		/// Associated with this CustomerPaymentMethodType</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_PaymentMethodTypeId">Id</see> 
		/// of <see cref="clsPaymentMethodType">Customer PaymentMethodType</see> 
		/// for this CustomerPaymentMethodType</returns>
		public int my_PaymentMethodTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PaymentMethodTypeId"));
		}

		/// <summary>
		/// <see cref="clsCustomer.my_CustomerId">CustomerId</see> of 
		/// <see cref="clsCustomer">Customer</see>
		/// Associated with this Customer Payment Method Type</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCustomer.my_CustomerId">Id</see> 
		/// of <see cref="clsCustomer">Customer</see> 
		/// for this Customer Payment Method Type</returns>	
		public int my_CustomerId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerId"));
		}

		/// <summary>
		/// CustomerPaymentMethodTypeId
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>CustomerPaymentMethodTypeId for this Row</returns>
		public int my_CustomerPaymentMethodTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerPaymentMethodTypeId"));
		}


		/// <summary>
		/// Whether a Customer is permitted to selet this Payment Method Type
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>Whether a Customer is permitted to selet this Payment Method Type</returns>
		public int my_Allowed(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Allowed"));
		}




		#endregion

		#region My_ Values PaymentMethodType

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_PaymentMethodTypeName">Payment Method Type Name</see> of 
		/// <see cref="clsPaymentMethodType">Payment Method Type</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_PaymentMethodTypeName">Id</see> 
		/// of <see cref="clsPaymentMethodType">Payment Method Type</see> 
		/// </returns>
		public string my_PaymentMethodType_PaymentMethodTypeName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PaymentMethodType_PaymentMethodTypeName");
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_PaymentMethodTypeDescription">Payment Method Type Description</see> of 
		/// <see cref="clsPaymentMethodType">Payment Method Type</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_PaymentMethodTypeDescription">Id</see> 
		/// of <see cref="clsPaymentMethodType">Payment Method Type</see> 
		/// </returns>
		public string my_PaymentMethodType_PaymentMethodTypeDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PaymentMethodType_PaymentMethodTypeDescription");
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_CustomerCanNotChoose">CustomerCanNotChoose</see> of 
		/// <see cref="clsPaymentMethodType">Payment Method Type</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_CustomerCanNotChoose">Id</see> 
		/// of <see cref="clsPaymentMethodType">Payment Method Type</see> 
		/// </returns>
		public int my_PaymentMethodType_CustomerCanNotChoose(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PaymentMethodType_CustomerCanNotChoose"));
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_CustomerNotePreSale">Pre-Sale Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_CustomerNotePreSale">Pre-Sale Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </returns>
		public string my_PaymentMethodType_CustomerNotePreSale(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PaymentMethodType_CustomerNotePreSale");
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_CustomerNotePostSale">Post-Sale Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_CustomerNotePostSale">Post-Sale Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </returns>
		public string my_PaymentMethodType_CustomerNotePostSale(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PaymentMethodType_CustomerNotePostSale");
		}


		/// <summary>
		/// <see cref="clsPaymentMethodType.my_CustomerNoteEmailHtml">Html Email Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_CustomerNoteEmailHtml">Html Email Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </returns>
		public string my_PaymentMethodType_CustomerNoteEmailHtml(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PaymentMethodType_CustomerNoteEmailHtml");
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_CustomerNoteEmailPlainText">Plain Text Email Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_CustomerNoteEmailPlainText">Plain Text Email Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </returns>
		public string my_PaymentMethodType_CustomerNoteEmailPlainText(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PaymentMethodType_CustomerNoteEmailPlainText");
		}


		/// <summary>
		/// <see cref="clsPaymentMethodType.my_AllowCustomerToUseByDefault">AllowCustomerToUseByDefault</see> of 
		/// <see cref="clsPaymentMethodType">Payment Method Type</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_AllowCustomerToUseByDefault">Id</see> 
		/// of <see cref="clsPaymentMethodType">Payment Method Type</see> 
		/// </returns>
		public int my_PaymentMethodType_AllowCustomerToUseByDefault(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PaymentMethodType_AllowCustomerToUseByDefault"));
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
