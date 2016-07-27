using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsPaymentMethodType deals with everything to do with data about PaymentMethodTypes.
	/// </summary>

	[GuidAttribute("DA7A4394-5C5C-4a83-86FB-717FF0C36A50")]
	public class clsPaymentMethodType : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsPaymentMethodType
		/// </summary>
		public clsPaymentMethodType() : base("PaymentMethodType")
		{
		}

		/// <summary>
		/// Constructor for clsPaymentMethodType; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsPaymentMethodType(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("PaymentMethodType")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ.AddSelectColumn("tblPaymentMethodType.PaymentMethodTypeId");
			MainQ.AddSelectColumn("tblPaymentMethodType.PaymentMethodTypeName");
			MainQ.AddSelectColumn("tblPaymentMethodType.PaymentMethodTypeDescription");
			MainQ.AddSelectColumn("tblPaymentMethodType.CustomerCanNotChoose");
			MainQ.AddSelectColumn("tblPaymentMethodType.CustomerNotePreSale");
			MainQ.AddSelectColumn("tblPaymentMethodType.CustomerNotePostSale");
			MainQ.AddSelectColumn("tblPaymentMethodType.CustomerNoteEmailHtml");
			MainQ.AddSelectColumn("tblPaymentMethodType.CustomerNoteEmailPlainText");
			MainQ.AddSelectColumn("tblPaymentMethodType.AllowCustomerToUseByDefault");
			
			MainQ.AddFromTable(thisTable);

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);

			orderBySqlQuery = "Order By PaymentMethodTypeDescription" + crLf;
		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsPaymentMethodType
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("PaymentMethodTypeDescription", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("PaymentMethodTypeName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerCanNotChoose", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CustomerNotePreSale", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerNotePostSale", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerNoteEmailHtml", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("CustomerNoteEmailPlainText", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AllowCustomerToUseByDefault", System.Type.GetType("System.Int32"));

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
		/// Initialises an internal list of all PaymentMethodTypes
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

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a PaymentMethodType by PaymentMethodTypeId.
		/// </summary>
		/// <param name="PaymentMethodTypeId">Id of PaymentMethodType to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPaymentMethodTypeId(int PaymentMethodTypeId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ PaymentMethodTypeId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets Payment Method Types by AllowCustomerToUseByDefault.
		/// </summary>
		/// <param name="AllowCustomerToUseByDefault">Id of PaymentMethodType to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByAllowCustomerToUseByDefault(int AllowCustomerToUseByDefault)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + ".AllowCustomerToUseByDefault = "
				+ AllowCustomerToUseByDefault.ToString() + ") " + thisTable,
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
		/// internal PaymentMethodType table stack; the SavePaymentMethodTypes method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PaymentMethodTypeName">Name of Payment Method Type</param>
		/// <param name="PaymentMethodTypeDescription">Description of Payment Method Type</param>
		/// <param name="CustomerCanNotChoose">Whether any Customer could ever be able to choose this method of payment themselves</param>
		/// <param name="CustomerNotePreSale">Pre-Sale Note to Customers regarding this Payment Type</param>
		/// <param name="CustomerNotePostSale">Post-Sale Note to Customers regarding this Payment Type</param>
		/// <param name="CustomerNoteEmailHtml">HTML Email Note to Customers regarding this Payment Type</param>
		/// <param name="CustomerNoteEmailPlainText">Plain Text Email Note to Customers regarding this Payment Type</param>
		/// <param name="AllowCustomerToUseByDefault">Whether customers should be able to use this method of payment by default</param>
		public void Add(string PaymentMethodTypeName, 
			string PaymentMethodTypeDescription,
			int CustomerCanNotChoose,
			string CustomerNotePreSale,
			string CustomerNotePostSale,
			string CustomerNoteEmailHtml,
			string CustomerNoteEmailPlainText,
			int AllowCustomerToUseByDefault)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["PaymentMethodTypeName"] = PaymentMethodTypeName;
			rowToAdd["PaymentMethodTypeDescription"] = PaymentMethodTypeDescription;
			rowToAdd["CustomerCanNotChoose"] = CustomerCanNotChoose;
			rowToAdd["CustomerNotePreSale"] = CustomerNotePreSale;
			rowToAdd["CustomerNotePostSale"] = CustomerNotePostSale;
			rowToAdd["CustomerNoteEmailHtml"] = CustomerNoteEmailHtml;
			rowToAdd["CustomerNoteEmailPlainText"] = CustomerNoteEmailPlainText;
			rowToAdd["AllowCustomerToUseByDefault"] = AllowCustomerToUseByDefault;
			
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
		/// internal PaymentMethodType table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PaymentMethodTypeId">Payment Method Type Id (Primary Key of Record)</param>
		/// <param name="PaymentMethodTypeName">Name of Payment Method Type</param>
		/// <param name="PaymentMethodTypeDescription">Description of Payment Method Type</param>
		/// <param name="CustomerCanNotChoose">Whether any Customer could ever be able to choose this method of payment themselves</param>
		/// <param name="CustomerNotePreSale">Pre-Sale Note to Customers regarding this Payment Type</param>
		/// <param name="CustomerNotePostSale">Post-Sale Note to Customers regarding this Payment Type</param>
		/// <param name="CustomerNoteEmailHtml">HTML Email Note to Customers regarding this Payment Type</param>
		/// <param name="CustomerNoteEmailPlainText">Plain Text Email Note to Customers regarding this Payment Type</param>
		/// <param name="AllowCustomerToUseByDefault">Whether customers should be able to use this method of payment by default</param>
		public void Modify(int PaymentMethodTypeId,
			string PaymentMethodTypeName, 
			string PaymentMethodTypeDescription,
			int CustomerCanNotChoose,
			string CustomerNotePreSale,
			string CustomerNotePostSale,
			string CustomerNoteEmailHtml,
			string CustomerNoteEmailPlainText,
			int AllowCustomerToUseByDefault)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["PaymentMethodTypeId"] = PaymentMethodTypeId;
			rowToAdd["PaymentMethodTypeName"] = PaymentMethodTypeName;
			rowToAdd["PaymentMethodTypeDescription"] = PaymentMethodTypeDescription;
			rowToAdd["CustomerCanNotChoose"] = CustomerCanNotChoose;
			rowToAdd["CustomerNotePreSale"] = CustomerNotePreSale;
			rowToAdd["CustomerNotePostSale"] = CustomerNotePostSale;
			rowToAdd["CustomerNoteEmailHtml"] = CustomerNoteEmailHtml;
			rowToAdd["CustomerNoteEmailPlainText"] = CustomerNoteEmailPlainText;
			rowToAdd["AllowCustomerToUseByDefault"] = AllowCustomerToUseByDefault;

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

		# region My_ Values


		/// <summary>
		/// <see cref="clsPaymentMethodType.my_PaymentMethodTypeId">Id</see> of 
		/// <see cref="clsPaymentMethodType">Payment Method Type</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_PaymentMethodTypeId">Id</see> 
		/// of <see cref="clsPaymentMethodType">Payment Method Type</see> 
		/// </returns>
		public int my_PaymentMethodTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PaymentMethodTypeId"));
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_PaymentMethodTypeName">Payment Method Type Name</see> of 
		/// <see cref="clsPaymentMethodType">Payment Method Type</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_PaymentMethodTypeName">Id</see> 
		/// of <see cref="clsPaymentMethodType">Payment Method Type</see> 
		/// </returns>
		public string my_PaymentMethodTypeName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PaymentMethodTypeName");
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_PaymentMethodTypeDescription">Payment Method Type Description</see> of 
		/// <see cref="clsPaymentMethodType">Payment Method Type</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_PaymentMethodTypeDescription">Id</see> 
		/// of <see cref="clsPaymentMethodType">Payment Method Type</see> 
		/// </returns>
		public string my_PaymentMethodTypeDescription(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "PaymentMethodTypeDescription");
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_CustomerCanNotChoose">CustomerCanNotChoose</see> of 
		/// <see cref="clsPaymentMethodType">Payment Method Type</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_CustomerCanNotChoose">Id</see> 
		/// of <see cref="clsPaymentMethodType">Payment Method Type</see> 
		/// </returns>
		public int my_CustomerCanNotChoose(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CustomerCanNotChoose"));
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_CustomerNotePreSale">Pre-Sale Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_CustomerNotePreSale">Pre-Sale Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </returns>
		public string my_CustomerNotePreSale(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerNotePreSale");
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_CustomerNotePostSale">Post-Sale Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_CustomerNotePostSale">Post-Sale Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </returns>
		public string my_CustomerNotePostSale(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerNotePostSale");
		}


		/// <summary>
		/// <see cref="clsPaymentMethodType.my_CustomerNoteEmailHtml">Html Email Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_CustomerNoteEmailHtml">Html Email Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </returns>
		public string my_CustomerNoteEmailHtml(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerNoteEmailHtml");
		}

		/// <summary>
		/// <see cref="clsPaymentMethodType.my_CustomerNoteEmailPlainText">Plain Text Email Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_CustomerNoteEmailPlainText">Plain Text Email Note to Customer</see> for this
		/// <see cref="clsPaymentMethodType">Payment Method Type </see>
		/// </returns>
		public string my_CustomerNoteEmailPlainText(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CustomerNoteEmailPlainText");
		}


		/// <summary>
		/// <see cref="clsPaymentMethodType.my_AllowCustomerToUseByDefault">AllowCustomerToUseByDefault</see> of 
		/// <see cref="clsPaymentMethodType">Payment Method Type</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPaymentMethodType.my_AllowCustomerToUseByDefault">Id</see> 
		/// of <see cref="clsPaymentMethodType">Payment Method Type</see> 
		/// </returns>
		public int my_AllowCustomerToUseByDefault(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "AllowCustomerToUseByDefault"));
		}


		#endregion
	}
}
