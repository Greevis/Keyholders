using System;
using System.Runtime.InteropServices;
using System.Data;
using Microsoft.Data.Odbc;
using Resources;
using System.IO;

namespace Keyholders
{
	/// <summary>
	/// clsCorrespondenceSend deals with everything to do with data about Correspondences Sent.
	/// </summary>

	[GuidAttribute("4C46854A-FFB7-4e5c-9AF8-1E9DF4F60DD4")]
	public class clsCorrespondenceSend : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsCorrespondenceSend
		/// </summary>
		public clsCorrespondenceSend() : base("CorrespondenceSend")
		{
		}

		/// <summary>
		/// Constructor for clsCorrespondenceSend; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsCorrespondenceSend(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("CorrespondenceSend")
		{
			Connect(typeOfDb, odbcConnection);
		}


		/// <summary>
		/// Part of the Query that Pertains to User Information
		/// </summary>
		public clsQueryPart UserQ = new clsQueryPart();
		
		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			UserQ = UserQueryPart();
			
			MainQ.AddSelectColumn("tblCorrespondenceSend.CorrespondenceSendId");
			MainQ.AddSelectColumn("tblCorrespondenceSend.UserId");
			MainQ.AddSelectColumn("tblCorrespondenceSend.CorrespondenceStatus");
			MainQ.AddSelectColumn("tblCorrespondenceSend.FaxFile");
			MainQ.AddSelectColumn("tblCorrespondenceSend.SnailFile");
			MainQ.AddSelectColumn("tblCorrespondenceSend.DateGenerated");
			MainQ.AddSelectColumn("tblCorrespondenceSend.DateGeneratedUtc");
			MainQ.AddSelectColumn("tblCorrespondenceSend.DateSent");
			MainQ.AddSelectColumn("tblCorrespondenceSend.DateSentUtc");

			MainQ.AddFromTable(thisTable);

			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = UserQ;

			mainSqlQuery = QB.BuildSqlStatement(queries);
			orderBySqlQuery = "Order By tblCorrespondenceSend.CorrespondenceSendId" + crLf;
		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsCorrespondenceSend
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("UserId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("CorrespondenceStatus", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("FaxFile", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("SnailFile", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateGenerated", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateGeneratedUtc", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSent", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("DateSentUtc", System.Type.GetType("System.String"));

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
		/// Initialises an internal list of all Correspondences Sent
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetAll()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = UserQ;


			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns);

			//CorrespondenceSending
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets an Order By CorrespondenceSendId
		/// </summary>
		/// <param name="CorrespondenceSendId">Id of CorrespondenceSend to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByCorrespondenceSendId(int CorrespondenceSendId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[2];
			
			queries[0] = MainQ;
			queries[1] = UserQ;

			thisSqlQuery = QB.BuildSqlStatement(
				queries, OrderByColumns, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ CorrespondenceSendId.ToString() + ") " + thisTable,
				thisTable
				);

			//CorrespondenceSending
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		# region Add/Modify/Validate/Save

		
		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Correspondence table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="UserId">User associated with this CorrespondenceSend</param>
		/// <param name="CorrespondenceStatus">Status of this Correspondence</param>
		/// <param name="FaxFile">File where all the Faxes pertaining to this Correspondnce are kept</param>
		/// <param name="SnailFile">File where all the Snail Messages pertaining to this Correspondnce are kept</param>
		public void Add(int UserId,
			int CorrespondenceStatus,
			string FaxFile,
			string SnailFile)
		{

			DateTime thisUTCDateTime = DateTime.UtcNow;
			
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			rowToAdd["UserId"] = UserId;
			rowToAdd["CorrespondenceStatus"] = CorrespondenceStatus;
			rowToAdd["FaxFile"] = FaxFile;
			rowToAdd["SnailFile"] = SnailFile;
			rowToAdd["DateGenerated"] = localRecords.DBDateTime(FromUtcToClientTime(thisUTCDateTime));
			rowToAdd["DateGeneratedUtc"] =  localRecords.DBDateTime(thisUTCDateTime);
			rowToAdd["DateSent"] = DBNull.Value;
			rowToAdd["DateSentUtc"] =  DBNull.Value;

			//Validate the data supplied
			Validate(rowToAdd, true);

			if (NumErrors() == 0)
			{
				newDataToAdd.Rows.Add(rowToAdd);
			}

		}

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal Correspondence table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="CorrespondenceSendId">CorrespondenceSend Associated with this CorrespondenceSend</param>
		public void SetAsSent(int CorrespondenceSendId)
		{

			DateTime thisUTCDateTime = DateTime.UtcNow;
			
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			SetAttribute(CorrespondenceSendId, "DateSent", localRecords.DBDateTime(FromUtcToClientTime(thisUTCDateTime)));
			AddAttributeToSet(CorrespondenceSendId, "DateSentUtc", localRecords.DBDateTime(thisUTCDateTime));
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

		# region My_ Values CorrespondenceSend


		/// <summary>
		/// <see cref="clsCorrespondenceSend.my_CorrespondenceSendId">CorrespondenceSendId</see> of 
		/// <see cref="clsCorrespondenceSend">CorrespondenceSend</see>
		/// Associated with this CorrespondenceSendSend</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSend.my_CorrespondenceSendId">CorrespondenceSendId</see> 
		/// of <see cref="clsCorrespondenceSend">CorrespondenceSend</see> 
		/// for this CorrespondenceSendSend</returns>	
		public int my_CorrespondenceSendId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "CorrespondenceSendId"));
		}


		/// <summary>
		/// <see cref="clsUser.my_UserId">UserId</see> of 
		/// <see cref="clsUser">User</see>
		/// Associated with this CorrespondenceSend</summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_UserId">Id</see> 
		/// of <see cref="clsUser">User</see> 
		/// for this CorrespondenceSend</returns>	
		public int my_UserId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "UserId"));
		}

		/// <summary>
		/// <see cref="clsCorrespondenceSend.my_DateGenerated">Date</see> this 
		/// <see cref="clsCorrespondenceSend">Correspondence Send was Generated (Client Time)</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSend.my_DateGenerated">Date</see> this 
		/// <see cref="clsCorrespondenceSend">Correspondence Send was Generated (Client Time)</see>
		/// </returns>	
		public string my_DateGenerated(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateGenerated");
		}

		/// <summary>
		/// <see cref="clsCorrespondenceSend.my_DateGeneratedUtc">Date</see> this 
		/// <see cref="clsCorrespondenceSend">Correspondence Send was Generated (UTC Time)</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSend.my_DateGeneratedUtc">Date</see> this 
		/// <see cref="clsCorrespondenceSend">Correspondence Send was Generated (UTC Time)</see>
		/// </returns>
		public string my_DateGeneratedUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateGeneratedUtc");
		}

		/// <summary>
		/// <see cref="clsCorrespondenceSend.my_DateSent">Date</see> this 
		/// <see cref="clsCorrespondenceSend">Correspondence Send was Sent (Client Time)</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSend.my_DateSent">Date</see> this 
		/// <see cref="clsCorrespondenceSend">Correspondence Send was Sent (Client Time)</see>
		/// </returns>	
		public string my_DateSent(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSent");
		}

		/// <summary>
		/// <see cref="clsCorrespondenceSend.my_DateSentUtc">Date</see> this 
		/// <see cref="clsCorrespondenceSend">Correspondence Send was Sent (UTC Time)</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsCorrespondenceSend.my_DateSentUtc">Date</see> this 
		/// <see cref="clsCorrespondenceSend">Correspondence Send was Sent (UTC Time)</see>
		/// </returns>
		public string my_DateSentUtc(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateSentUtc");
		}

		#endregion

		# region My_ Values User

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">Id</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public int my_User_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "User_PersonId"));
		}

		/// <summary>
		/// <see cref="clsUser.my_AccessLevel">AccessLevel</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_AccessLevel">AccessLevel</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public int my_User_AccessLevel(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "User_AccessLevel"));
		}

		/// <summary>
		/// <see cref="clsUser.my_FirstName">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_FirstName">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_FirstName");
		}

		/// <summary>
		/// <see cref="clsUser.my_LastName">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_LastName">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_LastName");
		}

		/// <summary>
		/// <see cref="clsUser.my_UserName">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_UserName">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_UserName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_UserName");
		}

		/// <summary>
		/// <see cref="clsUser.my_Password">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_Password">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_Password(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_Password");
		}


		/// <summary>
		/// <see cref="clsUser.my_Email">Email</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_Email">Email</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_Email");
		}

		/// <summary>
		/// <see cref="clsUser.my_DateLastLoggedIn">Date Last Logged In (Client Time)</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_DateLastLoggedIn">Date Last Logged In (Client Time)</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_User_DateLastLoggedIn(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "User_DateLastLoggedIn");
		}


		#endregion

	}
}
