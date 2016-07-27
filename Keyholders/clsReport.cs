using System;
using System.Runtime.InteropServices;
using System.Data;
using Resources;
using System.Data.Odbc;
//using MySql.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Keyholders
{
	/// <summary>
	/// Summary description for clsReport.
	/// </summary>
	[GuidAttribute("0F8CE3AD-9EA4-461b-9159-094D4E67C76C")]
	public class clsReport : clsKeyBase
	{	
		#region Initialization
		private ReportDocument reportDocument = new ReportDocument();

		private TableLogOnInfo crTableLogOnInfo = new TableLogOnInfo();
		private ConnectionInfo crConnectionInfo = new ConnectionInfo(); 
		private CrystalDecisions.CrystalReports.Engine.Database crDatabase;
		
		private CrystalDecisions.CrystalReports.Engine.Tables crTables;
		private CrystalDecisions.CrystalReports.Engine.Section crSection;
		private CrystalDecisions.CrystalReports.Engine.SubreportObject crSubreportObject;
		private CrystalDecisions.CrystalReports.Engine.ReportDocument SubReport;

//		MySql.Data.MySqlClient.MySqlConnection conn;
//		MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
//		MySql.Data.MySqlClient.MySqlDataAdapter adap;


//		OdbcConnection conn;
		OdbcCommand cmd = new OdbcCommand();
//		OdbcDataAdapter adap;

		private DataSet ds = new DataSet();
		
		private string strUser="";
		private string strPass="";
		private string strServer = "";
		private string strDB="";
	
		private int ReportData=0;
		
		
		/// <summary>
		/// clsReport
		/// </summary>
		public clsReport() : base("Report")
		{		
//			conn = new OdbcConnection("SERVER=Dev1;port=3306;UID=graham;PASSWORD=1fuckyou;DATABASE=kdl1"); 
			
		}   
		
		/// <summary>
		/// Constructor for clsReport
		/// </summary>
		/// <param name="typeOfDb">Type of Database, form the enumeration
		/// <see cref="clsRecordHandler.databaseType">databaseType</see>
		/// </param>
		/// <param name="odbcConnection">Open Database Connection</param>
		public clsReport(clsRecordHandler.databaseType typeOfDb, 
			OdbcConnection odbcConnection) : base("Report")
		{
			Connect(typeOfDb, odbcConnection);	

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

		#region Reports

		#region Test

		/// <summary>
		/// Cover Letter Report
		/// </summary>
		/// <param name="FileName">FileName</param>
		public int Test(string FileName)
		{

			VerifySettings();
			ReportData = 0;

			string sqlQuery = "select distinct tblCustomer.CustomerId "
				+ "from tblPerson, "
				+ "  tblPersonPremiseRole, "
				+ "  tblPremise, "
				+ "  tblCustomer "
				+ "where tblPerson.Archive = 0 "
				+ "  and tblPersonPremiseRole.Archive = 0 "
				+ "  and tblPremise.Archive = 0 "
				+ "  and tblCustomer.Archive = 0 "
				+ "  and tblPerson.PersonId = tblPersonPremiseRole.PersonId "
				+ "  and tblPremise.PremiseId = tblPersonPremiseRole.PremiseId "
				+ "  and tblPerson.CustomerId = tblCustomer.CustomerId "
				+ "  and tblPersonPremiseRole.PersonPremiseRoleType = 2 "
//				+ "  and (sum(StickerRequired) > 0  "
//				+ "		or sum(InvoiceRequired) > 0  "
//				+ "		or sum(StatementRequired) > 0  "
//				+ "		or sum(DetailsUpdateRequired) > 0)  "
				+ "group by tblPerson.PersonId,tblPerson.CustomerId,tblPersonPremiseRole.PersonPremiseRoleId;";
			
			if (localRecords.GetRecords(sqlQuery) > 0)
			{
				reportDocument.Load(thisReportPath + "rptCustomerCorrespondence" +".rpt");

				//set the report with the dataset
				reportDocument.SetDataSource(localRecords.dataTable);
				reportDocument.SetParameterValue("IsEmail", "Yes");

				SetUpDataSource();
										
				GenerateReport(FileName);
			
				ReportData = 1;
			}

			return ReportData;

////		    conn.Open();
//			clsPersonPremiseRole thisPersonPremiseRole = new clsPersonPremiseRole(thisDbType, localRecords.dbConnection);
//			thisPersonPremiseRole.GetPendingCorrespondence();
//
////			cmd.CommandText = "SELECT tblUser.UserId, tblUser.Password,tblUser.Email FROM tblUser";
////			cmd.Connection = conn;
////
////			adap = new OdbcDataAdapter();
////			adap.SelectCommand = cmd;
////			adap.Fill(ds);
//
////			if (ds.Tables[0].Rows.Count >0 )
////			{
//				reportDocument.Load(@"C:\Documents and Settings\Graham\My Documents\Visual Studio Projects\Keyholders\Keyholders\" + "rptCorrespondence.rpt");
//
//				reportDocument.SetDataSource(thisPersonPremiseRole.localRecords.dataTable);
//
//				GenerateReport(FileName);
//
//				ReportData = 1;
////			}
//
////			conn.Close();
//
//			return ReportData;
		}

		#endregion

		#region Cover Letter
		/// <summary>
		/// Cover Letter Report
		/// </summary>
		/// <param name="CustomerId">CustomerId</param>
		/// <param name="PersonId">PersonId</param>
		/// <param name="isEmail">isEmail</param>
		/// <param name="FileName">FileName</param>
		/// <returns>Sucess or Failure</returns>
		public int CoverLetter(int CustomerId, int PersonId, int isEmail, string FileName)
		{

			VerifySettings();
			ReportData = 0;

			string sqlQuery = " select tblPerson.QuickPostalAddress, tblPerson.Title, tblPerson.FirstName, tblPerson.LastName, "
				+ " tblPerson.UserName, tblPerson.Password, SettingValue, tblCustomer.CompanyName,"
				+ " tblUser.DateLastLoggedIn, "
				+ "(sum(StickerRequired) > 0) * (case when MaxPPR = " + personPremiseRoleType_detailsManager().ToString() + " then 1 else 0 end) AS Stickers, " + crLf
				+ "(sum(InvoiceRequired + CopyOfInvoiceRequired) > 0) * (case when MinPPR = " + personPremiseRoleType_billingContact().ToString() + " then 1 else 0 end) AS Invoices, " + crLf
				+ "(sum(StatementRequired) > 0) * (case when MinPPR = " + personPremiseRoleType_billingContact().ToString() + " then 1 else 0 end) AS Statements, "+ crLf
				+ "(sum(DetailsUpdateRequired) > 0) AS DetailsUpdates  " + crLf
				+ "from tblPerson, "+ crLf
				+ "  tblUser, "+ crLf
//				+ "  tblPersonPremiseRole, "+ crLf
				+ "  tblPremise, "+ crLf
				+ "  tblCustomer, "+ crLf
				+ "  tblsetting, "+ crLf
				+ "	(Select PersonId, PremiseId, max(PersonPremiseRoleType) as MaxPPR, min(PersonPremiseRoleType) as MinPPR " + crLf
				+ "		from tblPersonPremiseRole "+ crLf
				+ "		where Archive = 0 "+ crLf
				+ "			and PersonPremiseRoleType < " + personPremiseRoleType_keyHolder().ToString() 
				+ "			and PersonPremiseRoleType > " + personPremiseRoleType_daytimeContact().ToString() 
				+ "		Group by PersonId, PremiseId) tblPersonPremiseRole " + crLf
				+ "where tblPerson.Archive = 0 "+ crLf
				+ "	and tblPersonPremiseRole.PersonId = tblPerson.PersonId " + crLf
				+ "	and tblPersonPremiseRole.PremiseId = tblPremise.PremiseId " + crLf
				+ "  and tblPremise.Archive = 0 "+ crLf
				+ "  and tblUser.Archive = 0 "+ crLf
				+ "  and tblCustomer.Archive = 0 "+ crLf
				+ "  and tblsetting.SettingName = 'CorrespondenceLetterText' "+ crLf
				+ "  and tblPerson.CustomerId = tblCustomer.CustomerId "+ crLf
				+ "  and tblPerson.PersonId = tblUser.PersonId "+ crLf
				+ "  and tblPerson.PersonId = " + PersonId.ToString() + crLf 
				+ "  and tblPremise.CustomerId = " + CustomerId.ToString() + crLf 
				+ "group by QuickPostalAddress,tblPerson.Title, tblPerson.FirstName,tblPerson.LastName,tblPerson.UserName,tblPerson.Password, SettingValue, tblCustomer.CompanyName, tblUser.DateLastLoggedIn;"; 
			
			if (localRecords.GetRecords(sqlQuery) > 0)
			{
				reportDocument.Load(thisReportPath + "rptCustomerCorrespondenceSubCoverLetter" +".rpt");

				//set the report with the dataset
				reportDocument.SetDataSource(localRecords.dataTable);

				if (isEmail == 1)
					reportDocument.SetParameterValue("IsEmail", "Yes");
				else
					reportDocument.SetParameterValue("IsEmail", "No");

				SetUpDataSource();
										
				GenerateReport(FileName);
			
				ReportData = 1;
			}

			return ReportData;

		}
		#endregion

		#region Premises
		/// <summary>
		/// Cover Letter Report
		/// </summary>
		/// <param name="isEmail">isEmail</param>
		/// <param name="FileName">FileName</param>
		/// <param name="CustomerId">CustomerId</param>
		/// <returns>Sucess or Failure</returns>
		public int Premises(int CustomerId, int isEmail, string FileName)
		{

			VerifySettings();
			ReportData = 0;

			string sqlQuery = "select tblPremise.CustomerId, tblPremise.PremiseId, PremiseNumber, CompanyAtPremiseName, QuickPhysicalAddress, "+ crLf
				+ "	AlarmDetails, tblPremise.CustomerComments, PersonPremiseRoleType as RoleType, Priority, FullName"+ crLf
				+ "from tblPerson, "+ crLf
				+ "  tblPersonPremiseRole, "+ crLf
				+ "  tblPremise "+ crLf
				+ "where tblPerson.Archive = 0 "+ crLf
				+ "  and tblPersonPremiseRole.Archive = 0 "+ crLf
				+ "  and tblPremise.Archive = 0 "+ crLf
				+ "  and tblPerson.PersonId = tblPersonPremiseRole.PersonId "+ crLf
				+ "  and tblPremise.PremiseId = tblPersonPremiseRole.PremiseId "+ crLf
				+ "  and tblPremise.CustomerId = " + CustomerId.ToString() + crLf
				+ " Union " + crLf
				+  "select tblPremise.CustomerId, tblPremise.PremiseId, PremiseNumber, CompanyAtPremiseName, QuickPhysicalAddress, "+ crLf
				+ "	AlarmDetails, tblPremise.CustomerComments, SPPremiseRoleType + 10 as RoleType, 0 as Priority, FullName"+ crLf
				+ "from tblServiceProvider, "+ crLf
				+ "  tblSPPremiseRole, "+ crLf
				+ "  tblPremise "+ crLf
				+ "where tblServiceProvider.Archive = 0 "+ crLf
				+ "  and tblSPPremiseRole.Archive = 0 "+ crLf
				+ "  and tblPremise.Archive = 0 "+ crLf
				+ "  and tblServiceProvider.ServiceProviderId = tblSPPremiseRole.ServiceProviderId "+ crLf
				+ "  and tblPremise.PremiseId = tblSPPremiseRole.PremiseId "+ crLf
				+ "  and tblPremise.CustomerId = " + CustomerId.ToString() + crLf
				;
			
			if (localRecords.GetRecords(sqlQuery) > 0)
			{
				reportDocument.Load(thisReportPath + "rptCustomerCorrespondenceSubPremises" +".rpt");

				//set the report with the dataset
				reportDocument.SetDataSource(localRecords.dataTable);
				
				if (isEmail == 1)
					reportDocument.SetParameterValue("IsEmail", "Yes");
				else
					reportDocument.SetParameterValue("IsEmail", "No");

				SetUpDataSource();
										
				GenerateReport(FileName);
			
				ReportData = 1;
			}

			return ReportData;

		}
		#endregion

		#region People
		/// <summary>
		/// Cover Letter Report
		/// </summary>
		/// <param name="isEmail">isEmail</param>
		/// <param name="FileName">FileName</param>
		/// <param name="CustomerId">CustomerId</param>
		/// <returns>Sucess or Failure</returns>
		public int People(int CustomerId, int isEmail, string FileName)
		{

			VerifySettings();
			ReportData = 0;

			string sqlQuery = "select tblperson.PersonId, Title, FirstName, LastName, PositionInCompany, " + crLf
				+ "	QuickPostalAddress, QuickDaytimePhone, QuickDaytimeFax, QuickAfterHoursPhone, "+ crLf
				+ "	QuickMobilePhone, Email, tblperson. CustomerComments "+ crLf
				+ "from tblPerson, "+ crLf
				+ "  tblPersonPremiseRole, "+ crLf
				+ "  tblPremise "+ crLf
				+ "where tblPerson.Archive = 0 "+ crLf
				+ "  and tblPersonPremiseRole.Archive = 0 "+ crLf
				+ "  and tblPremise.Archive = 0 "+ crLf
				+ "  and tblPerson.PersonId = tblPersonPremiseRole.PersonId "+ crLf
				+ "  and tblPremise.PremiseId = tblPersonPremiseRole.PremiseId "+ crLf
				+ "  and tblPerson.CustomerId = " + CustomerId.ToString() + crLf
				;
			
			if (localRecords.GetRecords(sqlQuery) > 0)
			{
				reportDocument.Load(thisReportPath + "rptCustomerCorrespondenceSubPeople" +".rpt");

				//set the report with the dataset
				reportDocument.SetDataSource(localRecords.dataTable);
				
				if (isEmail == 1)
					reportDocument.SetParameterValue("IsEmail", "Yes");
				else
					reportDocument.SetParameterValue("IsEmail", "No");

				SetUpDataSource();
										
				GenerateReport(FileName);
			
				ReportData = 1;
			}

			return ReportData;

		}
		#endregion

		#region ServiceProviders
		/// <summary>
		/// Cover Letter Report
		/// </summary>
		/// <param name="isEmail">isEmail</param>
		/// <param name="FileName">FileName</param>
		/// <param name="CustomerId">CustomerId</param>
		/// <returns>Sucess or Failure</returns>
		public int ServiceProviders(int CustomerId, int isEmail, string FileName)
		{

			VerifySettings();
			ReportData = 0;

			string sqlQuery = "select tblPremise.PremiseNumber, tblPremise.CompanyAtPremiseName, tblPremise.QuickPhysicalAddress, tblPremise.CustomerId, "+ crLf
				+ "	tblServiceProvider.FullName, tblServiceProvider.QuickDaytimePhone, tblServiceProvider.QuickDaytimeFax, "+ crLf
				+ "	tblServiceProvider.QuickAfterHoursPhone, tblServiceProvider.QuickAfterHoursFax,  tblServiceProvider.QuickMobilePhone, "+ crLf
				+ "	tblSPPremiseRole.SPPremiseRoleType "+ crLf
				+ "from tblPremise, "+ crLf
				+ "  tblSPPremiseRole, "+ crLf
				+ "  tblServiceProvider "+ crLf
				+ "where tblpremise.Archive = 0 "+ crLf
				+ "  and tblSPPremiseRole.Archive = 0 "+ crLf
				+ "  and tblServiceProvider.Archive = 0 "+ crLf
				+ "  and tblPremise.PremiseId = tblSPPremiseRole.PremiseId "+ crLf
				+ "  and tblServiceProvider.ServiceProviderId = tblSPPremiseRole.ServiceProviderId "+ crLf
				+ "  and tblPremise.CustomerId = " + CustomerId.ToString() + crLf
				;
			
			if (localRecords.GetRecords(sqlQuery) > 0)
			{
				reportDocument.Load(thisReportPath + "rptCustomerCorrespondenceSubServiceProviders" +".rpt");

				//set the report with the dataset
				reportDocument.SetDataSource(localRecords.dataTable);
				
				if (isEmail == 1)
					reportDocument.SetParameterValue("IsEmail", "Yes");
				else
					reportDocument.SetParameterValue("IsEmail", "No");

				SetUpDataSource();
										
				GenerateReport(FileName);
			
				ReportData = 1;
			}

			return ReportData;

		}
		#endregion

		#region Invoices
		/// <summary>
		/// Invoice Report
		/// </summary>
		/// <param name="isEmail">isEmail</param>
		/// <param name="FileName">FileName</param>
		/// <param name="CustomerId">CustomerId</param>
		/// <returns>Sucess or Failure</returns>
		public int Invoices(int CustomerId, int isEmail, string FileName)
		{

			VerifySettings();
			ReportData = 0;

			string sqlQuery = "select tblPremise.PremiseId, tblItem.ItemId, tblOrder.OrderId, tblOrder.CustomerGroupId, tblOrder.OrderStatusId, " + crLf
				+ "	tblPremise.PremiseNumber, tblPremise.CompanyAtPremiseName, tblPremise.QuickPhysicalAddress, tblPremise.CompanyTypeId, "
				+ " tblPremise.CompanyTypeName, tblPremise.URL, tblPremise.DateStart, tblPremise.DateSubscriptionExpires, "+ crLf
				+ "	tblPremise.DateNextSubscriptionDueToBeInvoiced, tblPremise.DateLastDetailsUpdate, tblPremise.StickerRequired, "+ crLf
				+ "	tblPremise.InvoiceRequired, tblPremise.StatementRequired, tblPremise.DetailsUpdateRequired, "+ crLf
				+ "	tblItem.Quantity, tblItem.ItemName, tblItem.ItemCode, tblItem.ShortDescription, tblItem.LongDescription, tblItem.Cost,  "+ crLf
				+ "	tblItem.Weight, tblItem.MaxKeyholdersPerPremise, tblItem.MaxAssetRegisterAssets, tblItem.MaxAssetRegisterStorage, "+ crLf
				+ "	tblItem.MaxDocumentSafeDocuments, tblItem.MaxDocumentSafeStorage, tblItem.RequiresPremiseForActivation, " + crLf
				+ "	tblItem.DateActivation, " + crLf
				+ "	tblItem.DateExpiry, " + crLf
				+ "	tblItem.DurationNumUnits, tblItem.DurationUnitId, "+ crLf
				+ "	tblOrder.PaymentMethodTypeId, tblOrder.OrderNum, tblOrder.CustomerType, tblOrder.FullName, tblOrder.Title, "+ crLf
				+ "	tblOrder.FirstName, tblOrder.LastName, tblOrder.QuickPostalAddress, tblOrder.QuickDaytimePhone, " + crLf
				+ "	tblOrder.OrderSubmitted, tblOrder.OrderPaid, "+ crLf
				+ " tblOrder.OrderCreatedMechanism,  tblOrder.SupplierComment, tblOrder.DateCreated, tblOrder.DateSubmitted, " + crLf
				+ "	tblOrder.DateProcessed, tblOrder.DateShipped, tblOrder.DateDue, tblOrder.TaxAppliedToOrder, " + crLf
				+ "	tblOrder.TaxRateAtTimeOfOrder, tblOrder.TaxCost, tblOrder.TotalItemCost, tblOrder.NumItems, "+ crLf
				+ "	tblCustomer.IsDirectDebitCustomer "+ crLf
				+ "from tblPremise, tblItem, tblOrder, tblCustomer " + crLf
				+ "where tblPremise.PremiseId = tblItem.PremiseId " + crLf
				+ "	and tblOrder.OrderId = tblItem.OrderId " + crLf
				+ "	and tblCustomer.CustomerId = tblPremise.CustomerId " + crLf
				+ "	and tblPremise.Archive = 0 " + crLf
				+ "	and tblCustomer.Archive = 0 " + crLf
				+ "	and tblOrder.InvoiceRequested = 1 "+ crLf
				+ "	and tblPremise.CustomerId = " + CustomerId.ToString() + crLf
//				+ "	order by OrderId desc limit 1 "+ crLf
				;
			
			if (localRecords.GetRecords(sqlQuery) > 0)
			{
				reportDocument.Load(thisReportPath + "rptCustomerCorrespondenceSubInvoices" +".rpt");

				//set the report with the dataset
				reportDocument.SetDataSource(localRecords.dataTable);
				
				if (isEmail == 1)
					reportDocument.SetParameterValue("IsEmail", "Yes");
				else
					reportDocument.SetParameterValue("IsEmail", "No");

				SetUpDataSource();
										
				GenerateReport(FileName);
			
				ReportData = 1;
			}

			return ReportData;

		}
		#endregion

		#region Statements
		/// <summary>
		/// Statement Report
		/// </summary>
		/// <param name="isEmail">isEmail</param>
		/// <param name="FileName">FileName</param>
		/// <param name="CustomerId">CustomerId</param>
		/// <returns>Success or Failure</returns>
		public int Statements(int CustomerId, int isEmail, string FileName)
		{

			VerifySettings();
			ReportData = 0;

			string sqlQuery = "select tblTransaction.CustomerId, tblOrder.OrderId, tblOrder.CustomerGroupId, tblOrder.OrderStatusId, tblTransaction.TransactionId, " + crLf
				+ "	tblTransaction.DateSubmitted as Transaction_DateSubmitted, tblTransaction.Amount, tblTransaction.PostBalance, tblTransaction.VendorMemo,"
				+ " tblOrder.PaymentMethodTypeId, tblOrder.OrderNum, tblOrder.CustomerType, tblOrder.FullName, tblOrder.Title, "+ crLf
				+ "	tblOrder.FirstName, tblOrder.LastName, tblOrder.QuickPostalAddress, tblOrder.QuickDaytimePhone, "+ crLf
				+ "	tblOrder.OrderSubmitted, tblOrder.OrderPaid, "+ crLf
				+ "	tblOrder.OrderCreatedMechanism,  tblOrder.SupplierComment, tblOrder.DateCreated, tblOrder.DateSubmitted as Order_DateSubmitted,  "+ crLf
				+ "	tblOrder.DateProcessed, tblOrder.DateShipped, tblOrder.DateDue, tblOrder.TaxAppliedToOrder, "+ crLf
				+ "	tblOrder.TaxRateAtTimeOfOrder, tblOrder.TaxCost, tblOrder.TotalItemCost, tblOrder.NumItems, "+ crLf
				+ "	tblCustomer.IsDirectDebitCustomer" + crLf
				+ "from tblCustomer, tblTransaction left outer join (Select * from tblOrder where tblOrder.OrderSubmitted = 1) tblOrder on tblTransaction.OrderId = tblOrder.OrderId "+ crLf
				+ "where tblCustomer.CustomerId = tblTransaction.CustomerId "+ crLf
				+ "	and tblTransaction.Pending = 0" + crLf
				+ "	and tblTransaction.CustomerId = " + CustomerId.ToString() + crLf
				;
			
			if (localRecords.GetRecords(sqlQuery) > 0)
			{
				reportDocument.Load(thisReportPath + "rptCustomerCorrespondenceSubStatements" +".rpt");

				//set the report with the dataset
				reportDocument.SetDataSource(localRecords.dataTable);
				
				if (isEmail == 1)
					reportDocument.SetParameterValue("IsEmail", "Yes");
				else
					reportDocument.SetParameterValue("IsEmail", "No");

				SetUpDataSource();
										
				GenerateReport(FileName);
			
				ReportData = 1;
			}

			return ReportData;

		}
		#endregion

		#endregion

		#region Generate Report
		
		private void GenerateReport(string FileName)
		{
			reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
			PageMargins thisPageMargin = new PageMargins(0,0,0,0);
//			PageMargins thisPageMargin = new PageMargins(254,254,254,254);
			reportDocument.PrintOptions.ApplyPageMargins(thisPageMargin);

			//			reportDocument.PrintOptions.PageMargins.rightMargin = 0;
//			reportDocument.PrintOptions.PageMargins.topMargin = 0;
//			reportDocument.PrintOptions.PageMargins.bottomMargin = 0;
      									
			if (thisOutputPath == "test")
			{
				#region used just for decarlos
										
				CrystalDecisions.Shared.ExportFormatType fileFormat = new CrystalDecisions.Shared.ExportFormatType();
				switch(FileName.Substring((FileName.Length - 3),3))
				{
					case "pdf":
						reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, thisRootPath + FileName);
						break;
					case "xls":
						reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, thisRootPath  + FileName);
						break;
					case "doc":
						reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, thisRootPath + FileName);
						break;
					case "htm":
						reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.HTML40, thisRootPath + FileName);
						break;
					case "rtf":
						reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.RichText, thisRootPath + FileName);
						break;
				}
				#endregion
			}
			else
			{					
				switch(FileName.Substring((FileName.Length - 3),3))
				{
					case "pdf":
						reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, thisRootPath + thisOutputPath + FileName);
						break;
					case "xls":
						reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, thisRootPath + thisOutputPath + FileName);
						break;
					case "doc":
						reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, thisRootPath + thisOutputPath + FileName);
						break;
					case "htm":
						reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.HTML40, thisRootPath + thisOutputPath + FileName);
						break;
					case "rtf":
						reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.RichText, thisRootPath + thisOutputPath + FileName);
						break;
				}
			}
			reportDocument.Close();
			reportDocument.Dispose();
		}

		/// <summary>
		/// VerifySettings
		/// </summary>
		public void VerifySettings()
		{
			if ((thisOutputPath	== "") || (thisReportPath	== "")) 
				GetGeneralSettings();
		}

		/// <summary>
		/// SetUpDataSource
		/// </summary>
		public void SetUpDataSource()
		{
			GetDbSettings();

			crConnectionInfo.ServerName = strServer;
			crConnectionInfo.DatabaseName = strDB;
			crConnectionInfo.UserID = strUser;
			crConnectionInfo.Password = strPass;

			crDatabase = reportDocument.Database;
			crTables = crDatabase.Tables;
				
			foreach(CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
			{
				crTableLogOnInfo = crTable.LogOnInfo;
				crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
				crTable.ApplyLogOnInfo(crTableLogOnInfo);
			}
	
	
			for(int i=0; reportDocument.ReportDefinition.Sections.Count > i; i++)
			{
				crSection = reportDocument.ReportDefinition.Sections[i];
	
				for (int j=0; crSection.ReportObjects.Count > j ; j++)
				{
					if (crSection.ReportObjects[j].Kind == ReportObjectKind.SubreportObject)
					{
						crSubreportObject = (SubreportObject) crSection.ReportObjects[j];
						SubReport = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);
						crTables = SubReport.Database.Tables;
	
						foreach(CrystalDecisions.CrystalReports.Engine.Table crT in crTables)
						{					
							crTableLogOnInfo.ConnectionInfo.ServerName = strServer;
							crTableLogOnInfo.ConnectionInfo.DatabaseName = strDB;
							crTableLogOnInfo.ConnectionInfo.UserID = strUser;
							crTableLogOnInfo.ConnectionInfo.Password = strPass;
	
							crT.ApplyLogOnInfo(crTableLogOnInfo);
						}
					}
				}
			}
		}

		private void GetDbSettings()
		{
			string[] strConn = this.ConnectionString.Split('=');
			string[] strAux = null;

			for(int i=0; i <= strConn.Length - 1; i++)
			{
				strAux = strConn[i].Split(';');
				switch(i)
				{
					case 2:
						strServer = strAux[0].ToString();			
						break;
					case 6:
						strDB = strAux[0].ToString();			
						break;
					case 4:
						strUser = strAux[0].ToString();			
						break;
					case 5:
						strPass = strAux[0].ToString();			
						break;
				}
			}
		}


		#endregion
	}
}
