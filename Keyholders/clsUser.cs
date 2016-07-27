using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;

namespace Keyholders
{
	/// <summary>
	/// clsUser deals with everything to do with data about Users.
	/// </summary>


	[GuidAttribute("1041CEA8-BBEE-44f4-8989-A3E1A1E0B812")]
	public class clsUser : clsKeyBase
	{
		# region Initialisation
		/// <summary>
		/// Constructor for clsUser
		/// </summary>
		public clsUser() : base("User")
		{
		}

		/// <summary>
		/// Constructor for clsUser; Allows Calling of 'Connect' Function to be skipped
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsUser(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("User")
		{
			Connect(typeOfDb, odbcConnection);
		}

		/// <summary>
		/// Loads the SQL for the 'main' query for the Get Functions
		/// </summary>
		public override void LoadMainQuery()
		{
			MainQ = UserQueryPart();
			ChangeDataQ = ChangeDataQueryPart();

			clsQueryBuilder QB = new clsQueryBuilder();
			baseQueries = new clsQueryPart[2];
			
			baseQueries[0] = MainQ;
			baseQueries[1] = ChangeDataQ;

			mainSqlQuery = QB.BuildSqlStatement(baseQueries);

			orderBySqlQuery = "Order By LastName, FirstName, UserName" + crLf;
		}

		/// <summary>
		/// Initialise (or reinitialise) everything for clsUser
		/// </summary>
		public override void Initialise()
		{
			//Initialise the data tables with Column Names

			newDataToAdd = new DataTable(thisTable);
			newDataToAdd.Columns.Add("PersonId", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("FirstName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("LastName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("UserName", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Password", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("Email", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("AccessLevel", System.Type.GetType("System.Int32"));
			newDataToAdd.Columns.Add("DateLastLoggedIn", System.Type.GetType("System.String"));
			newDataToAdd.Columns.Add("ChangeDataId", System.Type.GetType("System.Int64"));
			newDataToAdd.Columns.Add("Archive", System.Type.GetType("System.Int64"));

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
		/// Local Representation of the class <see cref="clsUser">clsUser</see>
		/// </summary>
		public clsUser thisUser;

		#endregion

		# region Get Methods

		/// <summary>
		/// Initialises an internal list of all Users
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
		/// Gets a User by UserId.
		/// </summary>
		/// <param name="UserId">Id of User to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByUserId(int UserId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ UserId.ToString() + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets all Premises who are associated with a certain Person
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetByPersonId(int PersonId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;
			
			string condition = "(Select * from " + thisTable + crLf;			
			
			//Condition
			condition += "Where " + thisTable + ".PersonId = " + PersonId.ToString() + crLf;

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
		/// Gets a User by AccessLevel.
		/// </summary>
		/// <param name="AccessLevel">Id of User to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByAccessLevel(int AccessLevel)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;



			string condition = "(Select * from " + thisTable + crLf;			
			
			condition += "Where " + thisTable + ".AccessLevel = " + AccessLevel.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition +=  ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition,
				thisTable
				);


			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a User by AccessLevel.
		/// </summary>
		/// <param name="AccessLevel">Id of User to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetByNotAccessLevel(int AccessLevel)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf;			
			
			condition += "Where " + thisTable + ".AccessLevel != " + AccessLevel.ToString() + crLf;

			condition += ArchiveConditionIfNecessary(true);

			condition +=  ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition,
				thisTable
				);


			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		/// <summary>
		/// Gets a User by UserId.
		/// </summary>
		/// <param name="UserId">Id of User to retrieve</param>
		/// <returns>Number of resulting records</returns>
		public int GetFirstNameLastNameByUserId(int UserId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];
			

			queries[0] = MainQ;
			queries[0].SelectColumns.Clear();
			queries[0].SelectColumns.Add(thisTable + ".FirstName");
			queries[0].SelectColumns.Add(thisTable + ".LastName");

			string condition = "";

			condition += ArchiveConditionIfNecessary(true);

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = "
				+ UserId.ToString() + crLf 
				+ condition + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets a User by UserId.
		/// </summary>
		/// <returns>Number of resulting records</returns>
		public int GetSystemUserId()
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = new clsQueryPart[1];

			queries[0] = MainQ;
			
			string condition = "";

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				"(Select * from " + thisTable 
				+ " Where " + thisTable + ".UserName = 'System User'"
				+ crLf 
				+ condition + ") " + thisTable,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			int numRecords = localRecords.GetRecords(thisSqlQuery);
			if (numRecords == 0)
				return 0;
			else
				return my_UserId(0);

		}

		/// <summary>
		/// Get a User by User Name
		/// </summary>
		/// <param name="UserName">User Name for which to attempt to retrieve User</param>
		/// <returns>Number of resulting records</returns>
		public int GetByUserName(string UserName)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + ".UserName "
				+ MatchCondition(UserName, matchCriteria.exactMatch) + crLf 
				;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Get a User by User Name and Password combination
		/// </summary>
		/// <param name="UserName">User Name for which to attempt to retrieve User</param>
		/// <param name="Password">Password for which to attempt to retrieve User</param>
		/// <returns>Number of resulting records</returns>
		public int GetByUserNameAndPassword(string UserName, string Password)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			clsQueryPart[] queries = baseQueries;

			string condition = "(Select * from " + thisTable + crLf 
				+ " Where " + thisTable + ".UserName "
				+ MatchCondition(UserName, matchCriteria.exactMatch) + crLf 
				+ " And " + thisTable + ".Password "
				+ MatchCondition(Password, matchCriteria.exactMatch) + crLf 
				;

			condition += ArchiveConditionIfNecessary(true);

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(queries, 
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += orderBySqlQuery;

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region Ensure all people have users and creds

		/// <summary>
		/// EnsureAllPeopleHaveUsersAndCreds
		/// </summary>
		/// <returns>Number wihtout creds</returns>
		public int EnsureAllPeopleHaveUsersAndCreds()
		{
			clsPerson thisPerson = new clsPerson(thisDbType, localRecords.dbConnection);

			int numPersons = thisPerson.GetByNoUserNameOrPassword();

			for(int counter = 0; counter < numPersons; counter++)
			{
				#region Create Random Usernames and Passwords
				string thisUserName = thisPerson.my_LastName(counter).Trim();

				if (thisUserName.Length > 5)
					thisUserName = thisUserName.Substring(0, 5).Trim();

				int numDigitsToFill = 7 - thisUserName.Length;

				while (numDigitsToFill > 0)
				{
					thisUserName += GetRandomNum();
					numDigitsToFill = 7 - thisUserName.Length;
				}

				string thisPassword = GetRandomChar() + GetRandomNum() + GetRandomChar() + GetRandomChar() + GetRandomChar() 
					+ GetRandomNum() + GetRandomNum();


				int thisPersonId = thisPerson.my_PersonId(counter);

				#endregion

				clsUser thisUser = new clsUser(thisDbType, localRecords.dbConnection);

				int numUser = thisUser.GetByPersonId(thisPersonId);

				#region Add or Update the User

				if (numUser == 0)
				{
					thisUser.Add(
						thisPersonId,
						thisPerson.my_FirstName(counter),
						thisPerson.my_LastName(counter),
						thisPerson.my_UserName(counter),
						thisPerson.my_Password(counter),
						thisPerson.my_Email(counter),
						4,
						1);

					thisUser.Save();
				}
				else
				{
					if (thisUser.my_UserName(0) != "")
						thisUserName = thisUser.my_UserName(0);

					if (thisUser.my_Password(0) != "")
						thisPassword = thisUser.my_Password(0);

					thisUser.Modify(thisUser.my_UserId(0),
						thisPersonId,
						thisPerson.my_FirstName(counter),
						thisPerson.my_LastName(counter),
						thisUserName,
						thisPassword,
						thisPerson.my_Email(counter),
						4,
						"",
						1);

					thisUser.Save();
				}

				#endregion

				#region Update Person Details

				clsPerson thisModPerson = new clsPerson(thisDbType, localRecords.dbConnection);

				thisModPerson.Modify(thisPersonId,
					thisPerson.my_CustomerId(counter),
					thisPerson.my_Title(counter),
					thisPerson.my_FirstName(counter),
					thisPerson.my_LastName(counter),
					thisUserName,
					thisPassword,
					thisPerson.my_PositionInCompany(counter),
					thisPerson.my_QuickPostalAddress(counter),
					thisPerson.my_QuickDaytimePhone(counter),
					thisPerson.my_QuickDaytimeFax(counter),
					thisPerson.my_QuickAfterHoursPhone(counter),
					thisPerson.my_QuickAfterHoursFax(counter),
					thisPerson.my_QuickMobilePhone(counter),
					thisPerson.my_Email(counter),
					thisPerson.my_IsCustomerAdmin(counter),
					thisPerson.my_PreferredContactMethod(counter),
					thisPerson.my_KdlComments(counter),
					thisPerson.my_CustomerComments(counter),
					thisPerson.my_DateLastLoggedIn(counter),
					1
					);

				thisModPerson.Save();

				#endregion

			}

			return numPersons;
		}


		#endregion

		# region Add/Modify/Validate/Save

		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal User table stack; the SaveUsers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PersonId">Person Associated with this User (if any)</param>
		/// <param name="FirstName">FirstName</param>
		/// <param name="LastName">LastName</param>
		/// <param name="UserName">UserName</param>
		/// <param name="Password">User's Password</param>
		/// <param name="Email">User's Email</param>
		/// <param name="AccessLevel">User's Access Level</param>
		/// <param name="CurrentUser">Current Logged In User</param>
		public void Add(int PersonId,
			string FirstName,
			string LastName,
			string UserName,
			string Password, 
			string Email,
			int AccessLevel,
			int CurrentUser)
		{
			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(CurrentUser,"",dtaNow,CurrentUser,"",dtaNow);
			thisChangeData.Save();

			AddGeneral(PersonId,
				FirstName,
				LastName,
				UserName,
				Password,
				Email,
				AccessLevel,
				"",
				thisChangeData.LastIdAdded(),
				0);

			if(PersonId != 0)
			{
				clsPerson thisPerson = new clsPerson(thisDbType, localRecords.dbConnection);
				int numPeople = thisPerson.GetByPersonId(PersonId);
				if (numPeople > 0)
				{
					thisPerson.Modify(thisPerson.my_PersonId(0),
						thisPerson.my_CustomerId(0),
						thisPerson.my_Title(0),
						thisPerson.my_FirstName(0),
						thisPerson.my_LastName(0),
						UserName,
						Password,
						thisPerson.my_PositionInCompany(0),
						thisPerson.my_QuickPostalAddress(0),

						thisPerson.my_QuickDaytimePhone(0),
						thisPerson.my_QuickDaytimeFax(0),
						thisPerson.my_QuickAfterHoursPhone(0),
						thisPerson.my_QuickAfterHoursFax(0),
						thisPerson.my_QuickMobilePhone(0),
						thisPerson.my_Email(0),
						1,
						thisPerson.my_PreferredContactMethod(0),
						thisPerson.my_KdlComments(0),
						thisPerson.my_CustomerComments(0),
						thisPerson.my_DateLastLoggedIn(0),
						CurrentUser);

					thisPerson.Save();
				}
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
			thisUser = new clsUser(thisDbType, localRecords.dbConnection);

			thisUser.GetByUserId(thisPkId);

			AddGeneral(thisUser.my_PersonId(0),
				thisUser.my_FirstName(0),
				thisUser.my_LastName(0),
				thisUser.my_UserName(0),
				thisUser.my_Password(0),
				thisUser.my_Email(0),
				thisUser.my_AccessLevel(0),
				thisUser.my_DateLastLoggedIn(0),
				thisUser.my_ChangeDataId(0), 
				thisPkId);

			clsChangeData thisChangeData = new clsChangeData(thisDbType, localRecords.dbConnection);

			string dtaNow = localRecords.DBDateTime(DateTime.Now);

			thisChangeData.Add(thisUser.my_ChangeData_CreatedByUserId(0),
				thisUser.my_ChangeData_CreatedByFirstNameLastName(0),
				thisUser.my_ChangeData_DateCreated(0),
				CurrentUser,"",dtaNow.ToString());
							
			thisChangeData.Save();

			return thisChangeData.LastIdAdded();
		}


		/// <summary>
		/// If there are no Errors, this method adds a new entry to the 
		/// internal User table stack; the SaveUsers method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="PersonId">PersonId</param>
		/// <param name="FirstName">FirstName</param>
		/// <param name="LastName">LastName</param>
		/// <param name="UserName">UserName</param>
		/// <param name="Password">Password</param>
		/// <param name="Email">Email</param>
		/// <param name="AccessLevel">AccessLevel</param>
		/// <param name="DateLastLoggedIn">DateLastLoggedIn</param>
		/// <param name="ChangeDataId">ChangeDataId</param>
		/// <param name="Archive">Archive</param>
		private void AddGeneral(int PersonId,
			string FirstName,
			string LastName,
			string UserName,
			string Password,
			string Email,
			int AccessLevel,
			string DateLastLoggedIn,
			int ChangeDataId,
			int Archive)
		{

			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			System.Data.DataRow rowToAdd = newDataToAdd.NewRow();

			if (PersonId == 0)
				rowToAdd["PersonId"] = DBNull.Value;
			else
				rowToAdd["PersonId"] = PersonId;

			rowToAdd["FirstName"] = FirstName;
			rowToAdd["LastName"] = LastName;
			rowToAdd["UserName"] = UserName;
			rowToAdd["Password"] = Password;
			rowToAdd["Email"] = Email;
			rowToAdd["AccessLevel"] = AccessLevel;

			rowToAdd["DateLastLoggedIn"] = SanitiseDate(DateLastLoggedIn);

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
		/// internal User table stack; the Save method will save these
		/// to the database when it is subsequently called
		/// </summary>
		/// <param name="UserId">UserId (Primary Key of Record)</param>
		/// <param name="PersonId">Person Associated with this User (if any)</param>
		/// <param name="FirstName">FirstName</param>
		/// <param name="LastName">LastName</param>
		/// <param name="UserName">UserName</param>
		/// <param name="Password">User's Password</param>
		/// <param name="Email">User's Email</param>
		/// <param name="AccessLevel">User's Access Level</param>
		/// <param name="DateLastLoggedIn">DateLastLoggedIn</param>
		/// <param name="CurrentUser">Current Logged In User</param>
		public void Modify(int UserId,
			int PersonId,
			string FirstName,
			string LastName,
			string UserName,
			string Password,
			string Email,
			int AccessLevel,
			string DateLastLoggedIn,
			int CurrentUser)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["ChangeDataId"] = AddArchive(CurrentUser, UserId);

			rowToAdd["UserId"] = UserId;
			if (PersonId == 0)
				rowToAdd["PersonId"] = DBNull.Value;
			else
				rowToAdd["PersonId"] = PersonId;

			rowToAdd["FirstName"] = FirstName;
			rowToAdd["LastName"] = LastName;
			rowToAdd["UserName"] = UserName;
			rowToAdd["Password"] = Password;
			rowToAdd["Email"] = Email;
			rowToAdd["AccessLevel"] = AccessLevel;
			rowToAdd["Archive"] = 0;

			rowToAdd["DateLastLoggedIn"] = SanitiseDate(DateLastLoggedIn);
			Validate(rowToAdd, false);

			if (NumErrors() == 0)
			{
				if (UserChanges(rowToAdd))
					dataToBeModified.Rows.Add(rowToAdd);

			}

			if(PersonId != 0)
			{
				clsPerson thisPerson = new clsPerson(thisDbType, localRecords.dbConnection);
				int numPeople = thisPerson.GetByPersonId(PersonId);
				if (numPeople > 0)
				{
					thisPerson.Modify(thisPerson.my_PersonId(0),
						thisPerson.my_CustomerId(0),
						thisPerson.my_Title(0),
						thisPerson.my_FirstName(0),
						thisPerson.my_LastName(0),
						UserName,
						Password,
						thisPerson.my_PositionInCompany(0),
						thisPerson.my_QuickPostalAddress(0),

						thisPerson.my_QuickDaytimePhone(0),
						thisPerson.my_QuickDaytimeFax(0),
						thisPerson.my_QuickAfterHoursPhone(0),
						thisPerson.my_QuickAfterHoursFax(0),
						thisPerson.my_QuickMobilePhone(0),
						thisPerson.my_Email(0),
						1,
						thisPerson.my_PreferredContactMethod(0),
						thisPerson.my_KdlComments(0),
						thisPerson.my_CustomerComments(0),
						thisPerson.my_DateLastLoggedIn(0),
						CurrentUser);

					thisPerson.Save();
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
//			int pk = 0;
//
//			if (!newRow)
//				pk = Convert.ToInt32(valuesToValidate[thisPk]);
//
//			//UserName must be unique
//			CheckUnique(pk, "UserName",valuesToValidate["UserName"].ToString(),true);
//
//			//Email must be unique
//			CheckUnique(pk, "Email",valuesToValidate["Email"].ToString(),true);

		}

	


		/// <summary>
		/// Allows Modification of Password
		/// </summary>
		/// <param name="UserId"></param>
		/// <param name="OldPassword">User's old Password</param>
		/// <param name="NewPassword">User's new Password</param>
		/// <returns>0 if old password is incorrect for supplied UserId, 1 if Password sucessfully changed</returns>
		public int ModifyPassword(int UserId,
			string OldPassword, 
			string NewPassword)
		{
			//Reinitialise the Error and Warning Count
			ResetWarningAndErrorTables();
			
			clsUser thisUser = new clsUser(thisDbType, localRecords.dbConnection);
			int numRecords = thisUser.GetByUserId(UserId);

			//Validate the data supplied
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();

			rowToAdd["UserId"] = UserId;

			if (numRecords == 1 && OldPassword == thisUser.localRecords.FieldByName(0, "Password"))
			{

				string DateLastLoggedIn = thisUser.my_DateLastLoggedIn(0);
				
				if (DateLastLoggedIn == "")
				{
					rowToAdd["DateLastLoggedIn"] = DBNull.Value;
				}
				else
				{
					rowToAdd["DateLastLoggedIn"] = localRecords.DBDateTime(Convert.ToDateTime(DateLastLoggedIn));
				}
				
				rowToAdd["Password"] = NewPassword;

				rowToAdd["UserId"] = UserId;
				rowToAdd["UserName"] = thisUser.my_UserName(0);
				rowToAdd["Email"] = thisUser.my_Email(0);
				rowToAdd["Archive"] = thisUser.my_Archive(0);

				Validate(rowToAdd, false);

				if (NumErrors() == 0)
				{
					if (UserChanges(rowToAdd))
						dataToBeModified.Rows.Add(rowToAdd);
					return 1;
				}
				else return 0;
			}
			else return 0;

		}
		
		#endregion

		# region My_ Values User


		/// <summary>
		/// <see cref="clsUser.my_UserId">Id</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_UserId">Id</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public int my_UserId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "UserId"));
		}

		/// <summary>
		/// <see cref="clsPerson.my_PersonId">Id</see> of
		/// <see cref="clsPerson">Person</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsPerson.my_PersonId">Id</see> of 
		/// <see cref="clsPerson">Person</see> 
		/// </returns>
		public int my_PersonId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "PersonId"));
		}

		/// <summary>
		/// <see cref="clsUser.my_AccessLevel">AccessLevel</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_AccessLevel">AccessLevel</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public int my_AccessLevel(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "AccessLevel"));
		}

		/// <summary>
		/// <see cref="clsUser.my_FirstName">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_FirstName">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_FirstName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "FirstName");
		}

		/// <summary>
		/// <see cref="clsUser.my_LastName">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_LastName">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_LastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "LastName");
		}

		/// <summary>
		/// <see cref="clsUser.my_UserName">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_UserName">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_UserName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "UserName");
		}

		/// <summary>
		/// <see cref="clsUser.my_Password">Name</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_Password">Name</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_Password(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Password");
		}


		/// <summary>
		/// <see cref="clsUser.my_Email">Email</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_Email">Email</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_Email(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "Email");
		}

		/// <summary>
		/// <see cref="clsUser.my_DateLastLoggedIn">Date Last Logged In (Client Time)</see> of
		/// <see cref="clsUser">User</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsUser.my_DateLastLoggedIn">Date Last Logged In (Client Time)</see> of 
		/// <see cref="clsUser">User</see> 
		/// </returns>
		public string my_DateLastLoggedIn(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "DateLastLoggedIn");
		}


		#endregion

	}
}
