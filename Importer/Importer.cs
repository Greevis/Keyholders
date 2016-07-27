using System;
using Resources;
using Keyholders;
using System.IO;
using System.Runtime.InteropServices;

namespace Importer
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>

	[GuidAttribute("349F9CFE-1299-4676-AD39-2BA9C69663E8")]
	class Importer
	{
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			string Dev1Connection = "DRIVER={mySql ODBC 5.2 Unicode Driver};SERVER=Dev4;port=3306;UID=graham;PASSWORD=1fuckyou;DATABASE=kdl_mod3";
//			string Dev1Connection = "DRIVER={mySql ODBC 5.2 Unicode Driver};SERVER=Dev1;port=3306;UID=graham;PASSWORD=1fuckyou;DATABASE=kdl_mod3";
			//			string Dev2Connection = @"DRIVER={SQL Server};SERVER=Dev2;DATABASE=Freedom1;UID=sa;PWD=tandori;";
			//			string FREEDOMConnection = @"DRIVER={SQL Server};SERVER=localhost;DATABASE=Freedom;UID=welman;PWD=!WebFred!1;";
			string thisConnection = Dev1Connection;
			string AlertusDbFileName = "AlertusDB";

			string thisFolder = Directory.GetCurrentDirectory() + @"\";

			if (System.IO.File.Exists(thisFolder + AlertusDbFileName))
			{
				clsCsvReader fileReader = new clsCsvReader(thisFolder + AlertusDbFileName);
				string[] thisLine = fileReader.GetCsvLine();
				thisConnection = thisLine[0];
				fileReader.Dispose();			
			}

			clsImportData thisImportData = new clsImportData();
			thisImportData.Connect((int) thisImportData.DatabaseType_MySql(), 
				thisConnection);
			try
			{

				thisImportData.PreImport();
				thisImportData.RestartImportFromLastRecord(@"C:\", @"C:\KDL0504.CSV", 0);
				thisImportData.AddOrdersAndItems();
				thisImportData.AddUsersForPeople();
				thisImportData.ExportContactsForMailMerge();

				int currentYear = DateTime.Now.Year;
				int currentMonth = DateTime.Now.Month;


				for(int counter = 0; counter < 13; counter++)
				{
					thisImportData.DoCorrespondence(new DateTime(currentYear, currentMonth, 1));
					currentMonth++;
					if (currentMonth > 12)
					{
						currentYear++;
						currentMonth = currentMonth % 12;
					}
				}


//				thisImportData.DoCorrespondence(new DateTime(2010, 10, 1));
//				thisImportData.DoCorrespondence(new DateTime(2010, 11, 1));
//				thisImportData.DoCorrespondence(new DateTime(2010, 12, 1));
//				thisImportData.DoCorrespondence(new DateTime(2011, 1, 1));
//				thisImportData.DoCorrespondence(new DateTime(2011, 2, 1));
//				thisImportData.DoCorrespondence(new DateTime(2011, 3, 1));
//				thisImportData.DoCorrespondence(new DateTime(2011, 4, 1));
//				thisImportData.DoCorrespondence(new DateTime(2011, 5, 1));
//				thisImportData.DoCorrespondence(new DateTime(2011, 6, 1));
//				thisImportData.DoCorrespondence(new DateTime(2011, 7, 1));
//				thisImportData.DoCorrespondence(new DateTime(2011, 8, 1));
				//				thisImportData.DoCorrespondence(new DateTime(2011, 10, 1));
			}
			catch (System.Exception e)
			{
				clsCsvWriter fileWriter = new clsCsvWriter(thisFolder + "Importererr.txt", true);

				fileWriter.WriteFields(new
					object[] { 
								 e.ToString(), 
								 e.Message,
								 DateTime.Now.ToString()
							 });

				fileWriter.Close();
			}
		}
	}
}
