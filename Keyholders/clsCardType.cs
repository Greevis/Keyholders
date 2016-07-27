using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsCardType deals with everything to do with data about CardTypes.
	/// </summary>


	[GuidAttribute("D0D5C435-E89C-4508-A82F-6A762F624482")]
	public class clsCardType : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCardType
		/// </summary>
		public clsCardType() : base("CardType")
		{
		}

		/// <summary>
		/// Constructor for clsCardType; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param CardTypeName="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param CardTypeName="odbcConnection">Open Database Connection</param>
		public clsCardType(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("CardType")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ.AddSelectColumn("tblCardType.CardTypeId");
			MainQ.AddSelectColumn("tblCardType.Accepted");
			MainQ.AddSelectColumn("tblCardType.CardTypeName");
			MainQ.AddSelectColumn("tblCardType.ImageFileName");
			
			MainQ.AddFromTable(thisTable);

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);

			orderBySqlQuery = "Order By Accepted desc, CardTypeName" + crLf;
		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsCardType
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("Accepted", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CardTypeName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ImageFileName", System.Type.GetType("System.String"));

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
		/// <param CardTypeName="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param CardTypeName="odbcConnection">An already open ODBC database connection</param>
		public override void ConnectToForeignClasses(
			clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection)
		{
		}


		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all CardTypes
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
		/// Gets a CardType by CardTypeId. The value of this maps to the Enumeration 
		/// <see cref="clsKeyBase.creditCardType">creditCardType</see> 
		/// </summary>
		/// <param name="CardTypeId">Id of CardType to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCardTypeId(int CardTypeId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
		
			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns,  
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ CardTypeId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a CardType by whether it is accepted by the Vendor or not
		/// </summary>
		/// <param name="Accepted">whether to get accepted or not accepted cards</param>
		/// <returns>Number of resulting records</returns>
		public int GetByAccepted(int Accepted)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			
			queries[0] = MainQ;
		
			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns,  
				"(Select * from " + thisTable 
				+ " Where " + thisTable + ".Accepted = "
				+ Accepted.ToString() + ") " + thisTable,
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
		/// internal CardType table stack; the SaveCardTypes method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param CardTypeName="Accepted">Whether CardType is Accepted By Vendor or not</param>
		/// <param CardTypeName="CardTypeName">Card Type Name</param>
		/// <param CardTypeName="ImageFileName">Image File Name for Card</param>
		public void Add(int Accepted, 
			string CardTypeName,
			string ImageFileName)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["Accepted"] = Accepted;
			rowToAdd["CardTypeName"] = CardTypeName;
			rowToAdd["ImageFileName"] = ImageFileName;
			
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
		/// internal CardType table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param CardTypeName="CardTypeId">CardTypeId (Primary Key of Record)</param>
		/// <param CardTypeName="Accepted">Whether CardType is Accepted By Vendor or not</param>
		/// <param CardTypeName="SymbolLong">CardType's Long Symbol</param>
		/// <param CardTypeName="CardTypeName">Card Type Name</param>
		/// <param CardTypeName="ImageFileName">Image File Name for Card</param>
		public void Modify(int CardTypeId, 
			int Accepted,
			string CardTypeName,
			string ImageFileName)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			clsCardType thisCardType = new clsCardType(thisDbType, localRecords.dbConnection);
			thisCardType.GetByCardTypeId(CardTypeId);

			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["CardTypeId"] = CardTypeId;
			rowToAdd["Accepted"] = Accepted;
			rowToAdd["CardTypeName"] = CardTypeName;
			rowToAdd["ImageFileName"] = ImageFileName;

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
		/// <param CardTypeName="valuesToValidate">Values to be Validated.</param>
		/// <param CardTypeName="newRow">Indicates whether the Row being validated 
		/// is new or already exists in the system</param>
		private void Validate(System.Data.DataRow valuesToValidate, bool newRow)
		{
			//TODO: Add any required Validation here
		}

		#endregion

		# region My_ Values


		/// <summary>
		/// CardTypeId. The value of this maps to the Enumeration 
		/// </summary>
		/// <param CardTypeName="rowNum">Row number for Data</param>
		/// <returns><see cref="clsKeyBase.creditCardType">creditCardType</see> for this Row</returns>
		public int my_CardTypeId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CardTypeId"));
		}

		/// <summary>
		/// Whether CardType is Accepted By Vendor or not
		/// </summary>
		/// <param CardTypeName="rowNum">Row number for Data</param>
		/// <returns>Whether this CardType is Accepted By Vendor or not</returns>
		public int my_Accepted(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Accepted"));
		}

		/// <summary>
		/// Card Type Name
		/// </summary>
		/// <param CardTypeName="rowNum">Row number for Data</param>
		/// <returns>Card Type Name for this Row</returns>
		public string my_CardTypeName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "CardTypeName");
		}

		/// <summary>
		/// Image File Name
		/// </summary>
		/// <param ImageFileName="rowNum">Row number for Data</param>
		/// <returns>Image File Name for this Row</returns>
		public string my_ImageFileName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ImageFileName");
		}

		#endregion
	}
}
