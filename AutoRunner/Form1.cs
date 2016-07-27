using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Diagnostics;
using Resources;
using Keyholders;

namespace AutoRunner
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblOverall;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label lblKdl;
		private System.Windows.Forms.Button button3;
		private System.ComponentModel.IContainer components;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.lblOverall = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.lblKdl = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblOverall
			// 
			this.lblOverall.Location = new System.Drawing.Point(8, 8);
			this.lblOverall.Name = "lblOverall";
			this.lblOverall.Size = new System.Drawing.Size(584, 16);
			this.lblOverall.TabIndex = 11;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(8, 248);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(120, 48);
			this.button2.TabIndex = 13;
			this.button2.Text = "Add Initial Stuff";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(136, 248);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(120, 48);
			this.button1.TabIndex = 12;
			this.button1.Text = "Add Children";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// lblKdl
			// 
			this.lblKdl.Location = new System.Drawing.Point(8, 40);
			this.lblKdl.Name = "lblKdl";
			this.lblKdl.Size = new System.Drawing.Size(584, 16);
			this.lblKdl.TabIndex = 14;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(264, 248);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(120, 48);
			this.button3.TabIndex = 15;
			this.button3.Text = "Orders and Items";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(616, 309);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.lblKdl);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.lblOverall);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		public bool[] Completed = {false};


		public const int MaxAgeBeforeActionMinutes = 4;
		public int TimerIntervalMs = MaxAgeBeforeActionMinutes * 60 * 1000;
		public string executableFolder = Directory.GetCurrentDirectory() + @"\";
		public string TimingFileFolder = Directory.GetCurrentDirectory() + @"\";

		public string timingFileSuffix = "Timing.csv";
		public string lastGoodRecordFileSuffix = "LGRefId.csv";
		public string projectToRunFileSuffix = "Importer";
		public string AlertusDbFileName = "AlertusDb";

//		public const string DevServConnection = "DRIVER={mySql Odbc 3.51 Driver};SERVER=Devserv;port=3308;UID=graham;PASSWORD=1fuckyou;DATABASE=kdl_mod3";
		public const string DevServConnection = "DRIVER={mySql Odbc 3.51 Driver};SERVER=Dev4;port=3306;UID=graham;PASSWORD=1fuckyou;DATABASE=kdl_mod3";
		//		public const string FREEDOMConnection = @"DRIVER={SQL Server};SERVER=CHCWNSV03;DATABASE=Freedom;UID=welman;PWD=!WebFred!1;";
//		public const string Dev2Connection = @"DRIVER={SQL Server};SERVER=Dev2;DATABASE=Freedom1;UID=sa;PWD=tandori;";
		public string thisConnection = DevServConnection;


		/// <summary>
		/// List of all Folders where Data is stored
		/// </summary>
		public string[] CampFolders = {"KDL"};


		private void Form1_Load(object sender, System.EventArgs e)
		{
			//See if there is a file which dictates our Connection
			if (System.IO.File.Exists(TimingFileFolder + AlertusDbFileName))
			{
				clsCsvReader fileReader = new clsCsvReader(TimingFileFolder + AlertusDbFileName);
				string[] thisLine = fileReader.GetCsvLine();
				thisConnection = thisLine[0];
				fileReader.Dispose();			
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			clsImportData thisImportData = new clsImportData();
			thisImportData.Connect((int) thisImportData.DatabaseType_MySql(), 
				thisConnection);

			if (System.IO.File.Exists(TimingFileFolder + AlertusDbFileName))
			{
				clsCsvReader fileReader = new clsCsvReader(TimingFileFolder + AlertusDbFileName);
				string[] thisLine = fileReader.GetCsvLine();
				thisConnection = thisLine[0];
				fileReader.Dispose();
				
//				thisImportData.thisSetting.Ensure("thisRootPath", @"v:\chc.dev2.welman.co.nz\");
//				thisImportData.thisSetting.Ensure("thisReportPath", @"v:\Resources\ReportTemplates\");
			}

			thisImportData.PreImport();
			
			if (System.IO.File.Exists(TimingFileFolder + AlertusDbFileName))
			{
				clsCsvReader fileReader = new clsCsvReader(TimingFileFolder + AlertusDbFileName);
				string[] thisLine = fileReader.GetCsvLine();
				thisConnection = thisLine[0];
				fileReader.Dispose();

//				thisImportData.thisSetting.Ensure("thisRootPath", @"C:\www\chc.dev2.welman.co.nz\");
//				thisImportData.thisSetting.Ensure("thisReportPath", @"C:\www\Resources\ReportTemplates\");
			}
			else
			{
//				thisImportData.thisSetting.Ensure("thisRootPath", @"D:\FREEDOM\Web\Chc\");
//				thisImportData.thisSetting.Ensure("thisReportPath", @"D:\FREEDOM\Resources\ReportTemplates\");
			}

			MessageBox.Show("Complete");
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			timer1.Stop();
			timer1.Interval = TimerIntervalMs; 

			bool alldone = true;

			//Check if the file has been recently updated
			for(int counter = 0; counter < CampFolders.GetUpperBound(0) + 1; counter++)
			{
				int totReferrals = 0;
				int totReferralsAdded = 0;
				int totIncidents = 0;
				int totIncidentsAdded = 0;

				if (!Completed[counter])
				{
					//Check to see if we have completed
					string thisLastGoodRecordPathFile = TimingFileFolder + CampFolders[counter] + lastGoodRecordFileSuffix;
					int lastGoodRecord = 0;
					int lastCsvFileEntryIndexAdded = 0;
					int lastReferralToAdd = 0;
					int numIncidents = 0;
					int lastIncident_IdAdded = 0;

					if (System.IO.File.Exists(thisLastGoodRecordPathFile))
					{
						clsCsvReader fileReader = new clsCsvReader(thisLastGoodRecordPathFile);

						string[] thisLine = fileReader.GetCsvLine();
						lastGoodRecord = Convert.ToInt32(thisLine[0]);
						lastCsvFileEntryIndexAdded = Convert.ToInt32(thisLine[1]) + 1;
						lastReferralToAdd = Convert.ToInt32(thisLine[2]);
						
						if (thisLine.GetUpperBound(0) > 4)
						{
							lastIncident_IdAdded = Convert.ToInt32(thisLine[4]) + 1;
							numIncidents = Convert.ToInt32(thisLine[5]);
						}
						
						if (numIncidents == 0)
							numIncidents = 1;

						if (lastReferralToAdd == 0)
							lastReferralToAdd = 1;

						fileReader.Dispose();

						totReferrals += lastReferralToAdd;
						totReferralsAdded += lastCsvFileEntryIndexAdded;
						totIncidents += numIncidents;
						totIncidentsAdded += lastIncident_IdAdded;
						
						if(lastCsvFileEntryIndexAdded == lastReferralToAdd && lastIncident_IdAdded == numIncidents && numIncidents > 1)
							Completed[counter] = true;

						string thisDisaplyLine = CampFolders[counter] + ": " + ((lastCsvFileEntryIndexAdded) * 100 / lastReferralToAdd).ToString().Trim() 
							+ "% Done. " + lastCsvFileEntryIndexAdded.ToString().Trim() + " out of " + lastReferralToAdd.ToString().Trim()
							+ " Incidents: " + ((lastIncident_IdAdded) * 100 / numIncidents).ToString().Trim() 
							+ "% Done. "+ lastIncident_IdAdded.ToString().Trim() + " out of " + numIncidents.ToString().Trim();

						switch(counter)
						{

							case 0:
							default:
								lblKdl.Text = thisDisaplyLine;
								break;
						}
					}

					if (!Completed[counter])
					{
						alldone = false;

						bool thisProcFound = false;

						DateTime thisLastAccess = DateTime.Now.AddMinutes(-2 * MaxAgeBeforeActionMinutes);

						if (System.IO.File.Exists(TimingFileFolder + CampFolders[counter] + lastGoodRecordFileSuffix))
							thisLastAccess = System.IO.File.GetLastWriteTime(TimingFileFolder + CampFolders[counter] + lastGoodRecordFileSuffix);
					
						ArrayList al = new ArrayList();
						al.AddRange(Process.GetProcesses());
						foreach(object thisObj in al)
						{
							Process thisProc = (Process) thisObj;
							if (thisProc.ProcessName.IndexOf(projectToRunFileSuffix + CampFolders[counter]) > - 1)
							{
								thisProcFound = true;
								if (thisLastAccess.AddMinutes(MaxAgeBeforeActionMinutes) < DateTime.Now)
								{
									thisProc.Kill();
									Process.Start(executableFolder + projectToRunFileSuffix + CampFolders[counter] + ".exe");
								}
							}
						}
						if (!thisProcFound)
							Process.Start(executableFolder + projectToRunFileSuffix + CampFolders[counter] + ".exe");
					}
				}
			}
			if (!alldone)
				timer1.Start();
			else 
			{
				clsImportData thisImportData = new clsImportData();
				thisImportData.Connect((int) thisImportData.DatabaseType_MySql(), 
					thisConnection);

				if (System.IO.File.Exists(TimingFileFolder + AlertusDbFileName))
				{
					clsCsvReader fileReader = new clsCsvReader(TimingFileFolder + AlertusDbFileName);
					string[] thisLine = fileReader.GetCsvLine();
					thisConnection = thisLine[0];
					fileReader.Dispose();
					
					thisImportData.thisSetting.Ensure("thisRootPath", @"C:\www\chc.dev2.welman.co.nz\");
					thisImportData.thisSetting.Ensure("thisReportPath", @"C:\www\Resources\ReportTemplates\");
				}
				else
				{
					thisImportData.thisSetting.Ensure("thisRootPath", @"D:\FREEDOM\Web\Chc\");
					thisImportData.thisSetting.Ensure("thisReportPath", @"D:\FREEDOM\Resources\ReportTemplates\");
				}
				MessageBox.Show("Complete!");
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (System.IO.File.Exists(TimingFileFolder + AlertusDbFileName))
			{
				clsCsvReader fileReader = new clsCsvReader(TimingFileFolder + AlertusDbFileName);
				string[] thisLine = fileReader.GetCsvLine();
				thisConnection = thisLine[0];
				fileReader.Dispose();

				clsImportData thisImportData = new clsImportData();
				thisImportData.Connect((int) thisImportData.DatabaseType_MySql(), 
					thisConnection);

				thisImportData.thisSetting.Ensure("thisRootPath", @"v:\chc.dev2.welman.co.nz\");
				thisImportData.thisSetting.Ensure("thisReportPath", @"v:\Resources\ReportTemplates\");
			}
			
			timer1.Enabled = true;
			timer1.Interval = MaxAgeBeforeActionMinutes * 60 * 1000; //A bit of grace to start
			timer1.Start();
			for(int counter = 0; counter < CampFolders.GetUpperBound(0) + 1; counter++)
				Process.Start(executableFolder + projectToRunFileSuffix + CampFolders[counter] + ".exe");

		}

		private void button3_Click(object sender, System.EventArgs e)
		{
				clsImportData thisImportData = new clsImportData();
			thisImportData.Connect((int) thisImportData.DatabaseType_MySql(), 
				thisConnection);

			thisImportData.AddOrdersAndItems();
			MessageBox.Show("Complete!");

		}


	}
}
