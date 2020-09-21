using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Keyholders;
using Resources;

namespace TestApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button11;
		private System.Windows.Forms.Button button12;
		private System.Windows.Forms.Button button13;
		private System.Windows.Forms.Button button14;
		private System.Windows.Forms.Button button15;
		private System.Windows.Forms.Button button16;
		private System.Windows.Forms.Button button17;
		private System.Windows.Forms.Button button18;
		private System.Windows.Forms.Button button19;
		private System.Windows.Forms.Button button20;
		private System.Windows.Forms.Button button21;
		private System.Windows.Forms.Button button22;
		private System.Windows.Forms.Button button23;
		private System.Windows.Forms.Button button24;
		private System.Windows.Forms.Button button25;
		private System.Windows.Forms.Button button26;
		private System.Windows.Forms.Button button27;
		private System.Windows.Forms.Button button28;
		private System.Windows.Forms.Button button29;
		private System.Windows.Forms.Button button30;
		private System.Windows.Forms.Button button31;
		private System.Windows.Forms.Button button32;
		private System.Windows.Forms.Button button33;
		private System.Windows.Forms.Button button34;
		private System.Windows.Forms.Button button35;
		private System.Windows.Forms.Button button36;
		private System.Windows.Forms.TextBox txtYear;
		private System.Windows.Forms.TextBox txtMonth;
		private System.Windows.Forms.TextBox txtDay;
		private System.Windows.Forms.Label lblYear;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button37;
		private System.Windows.Forms.Button button38;
		private System.Windows.Forms.Button button39;
		private System.Windows.Forms.Button button40;
		private System.Windows.Forms.Button button41;
		private System.Windows.Forms.Button button42;
		private System.Windows.Forms.Button button43;
		private System.Windows.Forms.Button button44;
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.Button button46;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button45;
		private System.Windows.Forms.Button button47;
		private System.Windows.Forms.Button button48;
		private System.Windows.Forms.Button button49;
		private System.Windows.Forms.Button button50;
		private System.Windows.Forms.TextBox txtOrderId;
		private System.Windows.Forms.Button button51;
		private System.Windows.Forms.Button button52;
		private System.Windows.Forms.TextBox txtSearch;
        private GroupBox Connection;
        private Button butConUpdateDefaults;
        private Button butConUpdateNow;
        private TextBox txtConDriver;
        private Label label6;
        private TextBox txtConPassword;
        private Label label5;
        private TextBox txtConUid;
        private Label label4;
        private TextBox txtConPort;
        private Label label3;
        private TextBox txtConDatabase;
        private Label label7;
        private Label label8;
        private TextBox txtConServer;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.button27 = new System.Windows.Forms.Button();
            this.button28 = new System.Windows.Forms.Button();
            this.button29 = new System.Windows.Forms.Button();
            this.button30 = new System.Windows.Forms.Button();
            this.button31 = new System.Windows.Forms.Button();
            this.button32 = new System.Windows.Forms.Button();
            this.button33 = new System.Windows.Forms.Button();
            this.button34 = new System.Windows.Forms.Button();
            this.button35 = new System.Windows.Forms.Button();
            this.button36 = new System.Windows.Forms.Button();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.txtMonth = new System.Windows.Forms.TextBox();
            this.txtDay = new System.Windows.Forms.TextBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button37 = new System.Windows.Forms.Button();
            this.button38 = new System.Windows.Forms.Button();
            this.button39 = new System.Windows.Forms.Button();
            this.button40 = new System.Windows.Forms.Button();
            this.button41 = new System.Windows.Forms.Button();
            this.button42 = new System.Windows.Forms.Button();
            this.button43 = new System.Windows.Forms.Button();
            this.button44 = new System.Windows.Forms.Button();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.button46 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button45 = new System.Windows.Forms.Button();
            this.button47 = new System.Windows.Forms.Button();
            this.button48 = new System.Windows.Forms.Button();
            this.button49 = new System.Windows.Forms.Button();
            this.button50 = new System.Windows.Forms.Button();
            this.txtOrderId = new System.Windows.Forms.TextBox();
            this.button51 = new System.Windows.Forms.Button();
            this.button52 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.Connection = new System.Windows.Forms.GroupBox();
            this.butConUpdateDefaults = new System.Windows.Forms.Button();
            this.butConUpdateNow = new System.Windows.Forms.Button();
            this.txtConDriver = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtConUid = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtConPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConDatabase = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtConServer = new System.Windows.Forms.TextBox();
            this.Connection.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(344, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(320, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "GetByProductIdAndCustomerGroupId";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(344, 48);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(320, 24);
            this.button2.TabIndex = 1;
            this.button2.Text = "GetPendingCorrespondence";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(344, 112);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(320, 24);
            this.button3.TabIndex = 2;
            this.button3.Text = "Person";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(344, 80);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(320, 24);
            this.button4.TabIndex = 3;
            this.button4.Text = "Import Orders and Items";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(344, 144);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(320, 24);
            this.button5.TabIndex = 4;
            this.button5.Text = "PersonPremiseRole";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(344, 176);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(320, 24);
            this.button6.TabIndex = 5;
            this.button6.Text = "Premise";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(8, 208);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(320, 24);
            this.button7.TabIndex = 6;
            this.button7.Text = "Create Pending Orders";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(8, 112);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(320, 24);
            this.button8.TabIndex = 7;
            this.button8.Text = "Get Pending Correspondence";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(8, 176);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(320, 24);
            this.button9.TabIndex = 8;
            this.button9.Text = "Create Renewals with Exceptions";
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(8, 144);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(320, 24);
            this.button10.TabIndex = 9;
            this.button10.Text = "Get Submitted Orders";
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(8, 80);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(320, 24);
            this.button11.TabIndex = 10;
            this.button11.Text = "Create PDFs";
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(8, 48);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(320, 24);
            this.button12.TabIndex = 11;
            this.button12.Text = "Update Customer + Premises";
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(8, 240);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(320, 24);
            this.button13.TabIndex = 12;
            this.button13.Text = "Test Report";
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(344, 208);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(320, 24);
            this.button14.TabIndex = 13;
            this.button14.Text = "clsCompanyTypeName.GetDistinctCompanyTypeName";
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(8, 272);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(320, 24);
            this.button15.TabIndex = 14;
            this.button15.Text = "clsPersonPremiseRole.RemoveKeyholder";
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(8, 304);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(320, 24);
            this.button16.TabIndex = 15;
            this.button16.Text = "clsPersonPremiseRole.Set";
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(8, 336);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(320, 24);
            this.button17.TabIndex = 16;
            this.button17.Text = "clsPersonPremiseRole.GetByCustomerId";
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(8, 368);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(320, 24);
            this.button18.TabIndex = 17;
            this.button18.Text = "clsSPPremiseRole.GetByCustomerId";
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(8, 400);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(320, 24);
            this.button19.TabIndex = 18;
            this.button19.Text = "clsSpPremiseRole.GetDistinctServiceProviderByCustomerId";
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(344, 240);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(320, 24);
            this.button20.TabIndex = 19;
            this.button20.Text = "clsServiceProvider.GetByServiceProviderId";
            this.button20.Click += new System.EventHandler(this.button20_Click);
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(8, 432);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(320, 24);
            this.button21.TabIndex = 20;
            this.button21.Text = "clsSpPremiseRole.GetDistinctServiceProviderByPremiseId";
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(8, 464);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(320, 24);
            this.button22.TabIndex = 21;
            this.button22.Text = "clsSPPremiseRole.GetByPremiseId";
            this.button22.Click += new System.EventHandler(this.button22_Click);
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(344, 272);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(320, 24);
            this.button23.TabIndex = 22;
            this.button23.Text = "clsProduct.GetAll";
            this.button23.Click += new System.EventHandler(this.button23_Click);
            // 
            // button24
            // 
            this.button24.Location = new System.Drawing.Point(344, 304);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(320, 24);
            this.button24.TabIndex = 23;
            this.button24.Text = "clsPerson.GetByPersonId";
            this.button24.Click += new System.EventHandler(this.button24_Click);
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(344, 336);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(320, 24);
            this.button25.TabIndex = 24;
            this.button25.Text = "clsPremise.GetByPremiseId";
            this.button25.Click += new System.EventHandler(this.button25_Click);
            // 
            // button26
            // 
            this.button26.Location = new System.Drawing.Point(344, 368);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(320, 24);
            this.button26.TabIndex = 25;
            this.button26.Text = "clsCustomer.GetByCustomerId";
            this.button26.Click += new System.EventHandler(this.button26_Click);
            // 
            // button27
            // 
            this.button27.Location = new System.Drawing.Point(344, 400);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(320, 24);
            this.button27.TabIndex = 26;
            this.button27.Text = "clsServiceProvider.GetByServiceProviderId";
            this.button27.Click += new System.EventHandler(this.button27_Click);
            // 
            // button28
            // 
            this.button28.Location = new System.Drawing.Point(344, 432);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(320, 24);
            this.button28.TabIndex = 27;
            this.button28.Text = "clsCustomerGroup.GetByCustomerGroupId";
            this.button28.Click += new System.EventHandler(this.button28_Click);
            // 
            // button29
            // 
            this.button29.Location = new System.Drawing.Point(344, 464);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(320, 24);
            this.button29.TabIndex = 28;
            this.button29.Text = "clsUser.GetByUserId";
            this.button29.Click += new System.EventHandler(this.button29_Click);
            // 
            // button30
            // 
            this.button30.Location = new System.Drawing.Point(344, 496);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(320, 24);
            this.button30.TabIndex = 29;
            this.button30.Text = "clsUser.GetByUserNameAndPassword";
            this.button30.Click += new System.EventHandler(this.button30_Click);
            // 
            // button31
            // 
            this.button31.Location = new System.Drawing.Point(8, 496);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(320, 24);
            this.button31.TabIndex = 30;
            this.button31.Text = "clsAddress.Modify";
            this.button31.Click += new System.EventHandler(this.button31_Click);
            // 
            // button32
            // 
            this.button32.Location = new System.Drawing.Point(8, 528);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(320, 24);
            this.button32.TabIndex = 31;
            this.button32.Text = "clsUser.Add";
            this.button32.Click += new System.EventHandler(this.button32_Click);
            // 
            // button33
            // 
            this.button33.Location = new System.Drawing.Point(344, 528);
            this.button33.Name = "button33";
            this.button33.Size = new System.Drawing.Size(320, 24);
            this.button33.TabIndex = 32;
            this.button33.Text = "clsPremise.GetDistinctName";
            this.button33.Click += new System.EventHandler(this.button33_Click);
            // 
            // button34
            // 
            this.button34.Location = new System.Drawing.Point(344, 560);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(320, 24);
            this.button34.TabIndex = 33;
            this.button34.Text = "clsTransaction.AddByVendor ";
            this.button34.Click += new System.EventHandler(this.button34_Click);
            // 
            // button35
            // 
            this.button35.Location = new System.Drawing.Point(344, 592);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(320, 24);
            this.button35.TabIndex = 34;
            this.button35.Text = "clsSearch.GetBySearchId";
            this.button35.Click += new System.EventHandler(this.button35_Click);
            // 
            // button36
            // 
            this.button36.Location = new System.Drawing.Point(8, 560);
            this.button36.Name = "button36";
            this.button36.Size = new System.Drawing.Size(320, 24);
            this.button36.TabIndex = 35;
            this.button36.Text = "Generate All Correspondence";
            this.button36.Click += new System.EventHandler(this.button36_Click);
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(64, 592);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(56, 20);
            this.txtYear.TabIndex = 36;
            this.txtYear.Text = "2010";
            // 
            // txtMonth
            // 
            this.txtMonth.Location = new System.Drawing.Point(176, 592);
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(56, 20);
            this.txtMonth.TabIndex = 37;
            this.txtMonth.Text = "12";
            // 
            // txtDay
            // 
            this.txtDay.Location = new System.Drawing.Point(272, 592);
            this.txtDay.Name = "txtDay";
            this.txtDay.Size = new System.Drawing.Size(56, 20);
            this.txtDay.TabIndex = 38;
            this.txtDay.Text = "1";
            // 
            // lblYear
            // 
            this.lblYear.Location = new System.Drawing.Point(16, 592);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(48, 24);
            this.lblYear.TabIndex = 39;
            this.lblYear.Text = "Year";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(120, 592);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(48, 24);
            this.label1.TabIndex = 40;
            this.label1.Text = "Month";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(240, 592);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(24, 24);
            this.label2.TabIndex = 41;
            this.label2.Text = "Day";
            // 
            // button37
            // 
            this.button37.Location = new System.Drawing.Point(8, 624);
            this.button37.Name = "button37";
            this.button37.Size = new System.Drawing.Size(320, 24);
            this.button37.TabIndex = 42;
            this.button37.Text = "thisOrder.UpdateOrderCosts(thisOrderId);";
            this.button37.Click += new System.EventHandler(this.button37_Click);
            // 
            // button38
            // 
            this.button38.Location = new System.Drawing.Point(8, 656);
            this.button38.Name = "button38";
            this.button38.Size = new System.Drawing.Size(320, 24);
            this.button38.TabIndex = 43;
            this.button38.Text = "thisPremise.SetDetailsUpdateRequired";
            this.button38.Click += new System.EventHandler(this.button38_Click);
            // 
            // button39
            // 
            this.button39.Location = new System.Drawing.Point(344, 624);
            this.button39.Name = "button39";
            this.button39.Size = new System.Drawing.Size(320, 24);
            this.button39.TabIndex = 44;
            this.button39.Text = "clsProductCustomerGroupPrice.Set";
            this.button39.Click += new System.EventHandler(this.button39_Click);
            // 
            // button40
            // 
            this.button40.Location = new System.Drawing.Point(8, 688);
            this.button40.Name = "button40";
            this.button40.Size = new System.Drawing.Size(320, 24);
            this.button40.TabIndex = 45;
            this.button40.Text = "thisPremise.MarkCorrespondenceAsSent";
            this.button40.Click += new System.EventHandler(this.button40_Click);
            // 
            // button41
            // 
            this.button41.Location = new System.Drawing.Point(344, 656);
            this.button41.Name = "button41";
            this.button41.Size = new System.Drawing.Size(320, 24);
            this.button41.TabIndex = 46;
            this.button41.Text = "ExportContactsForMailMerge";
            this.button41.Click += new System.EventHandler(this.button41_Click);
            // 
            // button42
            // 
            this.button42.Location = new System.Drawing.Point(344, 688);
            this.button42.Name = "button42";
            this.button42.Size = new System.Drawing.Size(320, 24);
            this.button42.TabIndex = 47;
            this.button42.Text = "AddPeopleUsers";
            this.button42.Click += new System.EventHandler(this.button42_Click);
            // 
            // button43
            // 
            this.button43.Location = new System.Drawing.Point(344, 720);
            this.button43.Name = "button43";
            this.button43.Size = new System.Drawing.Size(320, 24);
            this.button43.TabIndex = 48;
            this.button43.Text = "UpdateCustomerTransactions";
            this.button43.Click += new System.EventHandler(this.button43_Click);
            // 
            // button44
            // 
            this.button44.Location = new System.Drawing.Point(8, 720);
            this.button44.Name = "button44";
            this.button44.Size = new System.Drawing.Size(320, 24);
            this.button44.TabIndex = 49;
            this.button44.Text = "UpdateCustomerAccounts";
            this.button44.Click += new System.EventHandler(this.button44_Click);
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(8, 8);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(128, 20);
            this.txtServer.TabIndex = 158;
            this.txtServer.Text = "Dev64";
            // 
            // button46
            // 
            this.button46.Location = new System.Drawing.Point(240, 8);
            this.button46.Name = "button46";
            this.button46.Size = new System.Drawing.Size(88, 24);
            this.button46.TabIndex = 157;
            this.button46.Text = "Change";
            this.button46.Click += new System.EventHandler(this.button46_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(680, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(128, 20);
            this.textBox1.TabIndex = 159;
            this.textBox1.Text = "Kate.welman.co.nz";
            // 
            // button45
            // 
            this.button45.Location = new System.Drawing.Point(680, 48);
            this.button45.Name = "button45";
            this.button45.Size = new System.Drawing.Size(168, 24);
            this.button45.TabIndex = 160;
            this.button45.Text = "Person";
            // 
            // button47
            // 
            this.button47.Location = new System.Drawing.Point(680, 112);
            this.button47.Name = "button47";
            this.button47.Size = new System.Drawing.Size(232, 24);
            this.button47.TabIndex = 161;
            this.button47.Text = "Ensure People have Users and Creds";
            this.button47.Click += new System.EventHandler(this.button47_Click);
            // 
            // button48
            // 
            this.button48.Location = new System.Drawing.Point(672, 80);
            this.button48.Name = "button48";
            this.button48.Size = new System.Drawing.Size(168, 24);
            this.button48.TabIndex = 162;
            this.button48.Text = "Fix Orders";
            this.button48.Click += new System.EventHandler(this.button48_Click);
            // 
            // button49
            // 
            this.button49.Location = new System.Drawing.Point(680, 144);
            this.button49.Name = "button49";
            this.button49.Size = new System.Drawing.Size(184, 32);
            this.button49.TabIndex = 163;
            this.button49.Text = "clsOrder.SubmitAllUnsubmittedOrder";
            this.button49.Click += new System.EventHandler(this.button49_Click);
            // 
            // button50
            // 
            this.button50.Location = new System.Drawing.Point(680, 192);
            this.button50.Name = "button50";
            this.button50.Size = new System.Drawing.Size(184, 32);
            this.button50.TabIndex = 164;
            this.button50.Text = "CreditNote";
            this.button50.Click += new System.EventHandler(this.button50_Click);
            // 
            // txtOrderId
            // 
            this.txtOrderId.Location = new System.Drawing.Point(680, 240);
            this.txtOrderId.Name = "txtOrderId";
            this.txtOrderId.Size = new System.Drawing.Size(56, 20);
            this.txtOrderId.TabIndex = 165;
            this.txtOrderId.Text = "1";
            // 
            // button51
            // 
            this.button51.Location = new System.Drawing.Point(744, 240);
            this.button51.Name = "button51";
            this.button51.Size = new System.Drawing.Size(168, 24);
            this.button51.TabIndex = 166;
            this.button51.Text = "Get Rid of OrderId";
            this.button51.Click += new System.EventHandler(this.button51_Click);
            // 
            // button52
            // 
            this.button52.Location = new System.Drawing.Point(744, 272);
            this.button52.Name = "button52";
            this.button52.Size = new System.Drawing.Size(168, 24);
            this.button52.TabIndex = 168;
            this.button52.Text = "clsPremise.Search";
            this.button52.Click += new System.EventHandler(this.button52_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(680, 272);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(56, 20);
            this.txtSearch.TabIndex = 167;
            this.txtSearch.Text = "Knigges";
            // 
            // Connection
            // 
            this.Connection.Controls.Add(this.butConUpdateDefaults);
            this.Connection.Controls.Add(this.butConUpdateNow);
            this.Connection.Controls.Add(this.txtConDriver);
            this.Connection.Controls.Add(this.label6);
            this.Connection.Controls.Add(this.txtConPassword);
            this.Connection.Controls.Add(this.label5);
            this.Connection.Controls.Add(this.txtConUid);
            this.Connection.Controls.Add(this.label4);
            this.Connection.Controls.Add(this.txtConPort);
            this.Connection.Controls.Add(this.label3);
            this.Connection.Controls.Add(this.txtConDatabase);
            this.Connection.Controls.Add(this.label7);
            this.Connection.Controls.Add(this.label8);
            this.Connection.Controls.Add(this.txtConServer);
            this.Connection.Location = new System.Drawing.Point(678, 304);
            this.Connection.Name = "Connection";
            this.Connection.Size = new System.Drawing.Size(234, 200);
            this.Connection.TabIndex = 169;
            this.Connection.TabStop = false;
            this.Connection.Text = "Connection";
            // 
            // butConUpdateDefaults
            // 
            this.butConUpdateDefaults.Location = new System.Drawing.Point(128, 170);
            this.butConUpdateDefaults.Name = "butConUpdateDefaults";
            this.butConUpdateDefaults.Size = new System.Drawing.Size(100, 23);
            this.butConUpdateDefaults.TabIndex = 24;
            this.butConUpdateDefaults.Text = "Update Defaults";
            this.butConUpdateDefaults.UseVisualStyleBackColor = true;
            this.butConUpdateDefaults.Click += new System.EventHandler(this.butConUpdateDefaults_Click);
            // 
            // butConUpdateNow
            // 
            this.butConUpdateNow.Location = new System.Drawing.Point(8, 170);
            this.butConUpdateNow.Name = "butConUpdateNow";
            this.butConUpdateNow.Size = new System.Drawing.Size(100, 23);
            this.butConUpdateNow.TabIndex = 24;
            this.butConUpdateNow.Text = "Update Now";
            this.butConUpdateNow.UseVisualStyleBackColor = true;
            this.butConUpdateNow.Click += new System.EventHandler(this.butConUpdateNow_Click);
            // 
            // txtConDriver
            // 
            this.txtConDriver.Location = new System.Drawing.Point(105, 135);
            this.txtConDriver.Name = "txtConDriver";
            this.txtConDriver.Size = new System.Drawing.Size(125, 20);
            this.txtConDriver.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Driver";
            // 
            // txtConPassword
            // 
            this.txtConPassword.Location = new System.Drawing.Point(105, 110);
            this.txtConPassword.Name = "txtConPassword";
            this.txtConPassword.Size = new System.Drawing.Size(125, 20);
            this.txtConPassword.TabIndex = 32;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Password";
            // 
            // txtConUid
            // 
            this.txtConUid.Location = new System.Drawing.Point(105, 85);
            this.txtConUid.Name = "txtConUid";
            this.txtConUid.Size = new System.Drawing.Size(125, 20);
            this.txtConUid.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "UID:";
            // 
            // txtConPort
            // 
            this.txtConPort.Location = new System.Drawing.Point(105, 62);
            this.txtConPort.Name = "txtConPort";
            this.txtConPort.Size = new System.Drawing.Size(125, 20);
            this.txtConPort.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Port:";
            // 
            // txtConDatabase
            // 
            this.txtConDatabase.Location = new System.Drawing.Point(105, 38);
            this.txtConDatabase.Name = "txtConDatabase";
            this.txtConDatabase.Size = new System.Drawing.Size(125, 20);
            this.txtConDatabase.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Database:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Server:";
            // 
            // txtConServer
            // 
            this.txtConServer.Location = new System.Drawing.Point(105, 12);
            this.txtConServer.Name = "txtConServer";
            this.txtConServer.Size = new System.Drawing.Size(125, 20);
            this.txtConServer.TabIndex = 23;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1024, 749);
            this.Controls.Add(this.Connection);
            this.Controls.Add(this.button52);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.button51);
            this.Controls.Add(this.txtOrderId);
            this.Controls.Add(this.button50);
            this.Controls.Add(this.button49);
            this.Controls.Add(this.button48);
            this.Controls.Add(this.button47);
            this.Controls.Add(this.button45);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.button46);
            this.Controls.Add(this.button44);
            this.Controls.Add(this.button43);
            this.Controls.Add(this.button42);
            this.Controls.Add(this.button41);
            this.Controls.Add(this.button40);
            this.Controls.Add(this.button39);
            this.Controls.Add(this.button38);
            this.Controls.Add(this.button37);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.txtDay);
            this.Controls.Add(this.txtMonth);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.button36);
            this.Controls.Add(this.button35);
            this.Controls.Add(this.button34);
            this.Controls.Add(this.button33);
            this.Controls.Add(this.button32);
            this.Controls.Add(this.button31);
            this.Controls.Add(this.button30);
            this.Controls.Add(this.button29);
            this.Controls.Add(this.button28);
            this.Controls.Add(this.button27);
            this.Controls.Add(this.button26);
            this.Controls.Add(this.button25);
            this.Controls.Add(this.button24);
            this.Controls.Add(this.button23);
            this.Controls.Add(this.button22);
            this.Controls.Add(this.button21);
            this.Controls.Add(this.button20);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Connection.ResumeLayout(false);
            this.Connection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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


        public const string delimiter = "~~~";

        public string KateConnection = "DRIVER={mySql ODBC 5.2 Unicode Driver};SERVER=WebroomsDbServer;port=3306;UID=WebroomsDbUser;PASSWORD=pd3Esnsmxke;DATABASE=kdl_mod3";

        public string thisConnectionAndLogFileName = Application.ProductName + "ConnectionString.txt";
        public string thisFileFolder = @"c:\";
        public string thisConnection = "";

        public int thisDbType = (int)Resources.clsRecordHandler.databaseType.mySql;



		private void button1_Click(object sender, System.EventArgs e)
		{

			
			clsProductCustomerGroupPrice thisProductCustomerGroupPrice = new clsProductCustomerGroupPrice();
			thisProductCustomerGroupPrice.Connect((int) thisProductCustomerGroupPrice.DatabaseType_MySql(), 
				thisConnection);
			
			int numResults = thisProductCustomerGroupPrice.GetByProductIdAndCustomerGroupId(2,1);
			MessageBox.Show("Done");


		}

		private void button2_Click(object sender, System.EventArgs e)
		{

			clsPersonPremiseRole PersonPremiseRole = new clsPersonPremiseRole();
			PersonPremiseRole.Connect((int) PersonPremiseRole.DatabaseType_MySql(), 
				thisConnection);

			clsReport thisReport = new clsReport();
			thisReport.Connect((int) thisReport.DatabaseType_MySql(), 
				thisConnection);
	
			int numReturned = PersonPremiseRole.GetPendingCorrespondence();

			MessageBox.Show(numReturned.ToString());

			thisReport.thisReportPath = @"C:\Inetpub\wwwroot\WebApplicationTestReport\";
			thisReport.thisRootPath = @"C:\Work\TestReport\";
			thisReport.thisOutputPath = @"test";


			numReturned = thisReport.Invoices(31,
				1,
				@"thisInvoice.pdf");

			MessageBox.Show(numReturned.ToString());
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			clsPerson Person = new clsPerson();
			Person.Connect((int) Person.DatabaseType_MySql(), 
				thisConnection);
			Person.GetByPersonId(1);
			Person.Save();
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			clsImportData ImportData = new clsImportData();
			ImportData.Connect((int) ImportData.DatabaseType_MySql(), 
				thisConnection);

			ImportData.AddOrdersAndItems();
			MessageBox.Show("Done");
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			clsPersonPremiseRole PersonPremiseRole = new clsPersonPremiseRole();
			PersonPremiseRole.Connect((int) PersonPremiseRole.DatabaseType_MySql(), 
				thisConnection);
			int numPersonPremiseRoles = PersonPremiseRole.GetByPersonIdPremiseId(1,1);
			MessageBox.Show("numPersonPremiseRoles: " + numPersonPremiseRoles.ToString());
			PersonPremiseRole.Save();
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			clsPremise Premise = new clsPremise();
			Premise.Connect((int) Premise.DatabaseType_MySql(), 
				thisConnection);
			int numRecords = Premise.GetByPremiseId(2845);
//			Premise.Save();
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			clsPremise Premise = new clsPremise();
			Premise.Connect((int) Premise.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = Premise.GetImminentRenewals(DateTime.Now.ToString());
			for (int counter = 0; counter < numRecords; counter++)
			{
//				Premise.AddRenewalOrder(Premise.my_CustomerId(counter), 1, Premise.my_PremiseId(counter), DateTime.Now.ToString(),5, 12);
			}
			Premise.Save();

		}

		private void button8_Click(object sender, System.EventArgs e)
		{
			clsPersonPremiseRole PersonPremiseRole = new clsPersonPremiseRole();
			PersonPremiseRole.Connect((int) PersonPremiseRole.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = PersonPremiseRole.GetPendingCorrespondence();

			MessageBox.Show("NumRecords: " + numRecords.ToString());

		}

		private void button9_Click(object sender, System.EventArgs e)
		{
			clsPremise Premise = new clsPremise();
			Premise.Connect((int) Premise.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			Premise.CreateRenewals(DateTime.Now.ToString());
			MessageBox.Show("NumRecords: " + numRecords.ToString());

		}

		private void button10_Click(object sender, System.EventArgs e)
		{
			clsItem Item = new clsItem();
			Item.Connect((int) Item.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = Item.GetUnsubmittedOrders();
			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button11_Click(object sender, System.EventArgs e)
		{
			clsKeyBase thisKeyBase = new clsKeyBase();
			thisKeyBase.Connect((int) thisKeyBase.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			thisKeyBase.MakePDF(@"<html> <head> <style> body { background:#DFE3F2; font-family:verdana; font-size: 12px; padding:30px; } td { font-size: 12px; } .keyholderTable { width:100%; border-top: 1px solid navy; border-left: 1px solid navy; border-right: 1px solid navy; } .keyholderTable tr td { vertical-align:top; border-bottom: 1px solid navy; } .sectionHead { font-weight:bold; font-size:14px; margin: 5px 0px; } .premiseBlock { background:white; border: 1px solid navy; padding:5px; } .premiseBlock table tr td { vertical-align:top; } div.printSection { page-break-after:always; } </style> </head> <body>finalizing New Zealand Window Shades Ltd </body> </html>",
				@"w:\kdlec.dev.welman.co.nz\templates\correspondence\kdl_header.jpg",
				@"w:\kdlec.dev.welman.co.nz\templates\correspondence\kdl_footer.jpg", 
				@"C:\Email_1.pdf",
				0.5, 1.4, 0, 0);

			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button12_Click(object sender, System.EventArgs e)
		{
			clsPremise Premise = new clsPremise();
			Premise.Connect((int) Premise.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
//			numRecords = Premise.UpdateCustomers();
			MessageBox.Show("NumRecords: " + numRecords.ToString());
			Premise.UpdatePremiseItemAssociations();
			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button14_Click(object sender, System.EventArgs e)
		{
			clsCompanyType thisCompanyType = new clsCompanyType();
			thisCompanyType.Connect((int) thisCompanyType.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = thisCompanyType.GetDistinctCompanyTypeName("a", thisCompanyType.matchCriteria_startsWith());
			MessageBox.Show("NumRecords: " + numRecords.ToString());


		}

		private void button15_Click(object sender, System.EventArgs e)
		{
			clsPersonPremiseRole thisPersonPremiseRole = new clsPersonPremiseRole();
			thisPersonPremiseRole.Connect((int) thisPersonPremiseRole.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			thisPersonPremiseRole.RemoveKeyholder(1,1,thisPersonPremiseRole.systemUserId);
			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button16_Click(object sender, System.EventArgs e)
		{
			clsPersonPremiseRole thisPersonPremiseRole = new clsPersonPremiseRole();
			thisPersonPremiseRole.Connect((int) thisPersonPremiseRole.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			thisPersonPremiseRole.Set(979,2702,thisPersonPremiseRole.personPremiseRoleType_billingContact(),1);
			thisPersonPremiseRole.Save();

			numRecords = thisPersonPremiseRole.GetByPremiseIdPersonPremiseRoleType(979, thisPersonPremiseRole.personPremiseRoleType_billingContact());

			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button17_Click(object sender, System.EventArgs e)
		{
			clsPersonPremiseRole thisPersonPremiseRole = new clsPersonPremiseRole();
			thisPersonPremiseRole.Connect((int) thisPersonPremiseRole.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisPersonPremiseRole.GetByCustomerId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString());
			
		}

		private void button18_Click(object sender, System.EventArgs e)
		{
			clsSPPremiseRole thisSPPremiseRole = new clsSPPremiseRole();
			thisSPPremiseRole.Connect((int) thisSPPremiseRole.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisSPPremiseRole.GetByCustomerId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button19_Click(object sender, System.EventArgs e)
		{
			clsSPPremiseRole thisSPPremiseRole = new clsSPPremiseRole();
			thisSPPremiseRole.Connect((int) thisSPPremiseRole.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisSPPremiseRole.GetDistinctServiceProviderByCustomerId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button20_Click(object sender, System.EventArgs e)
		{
			clsServiceProvider thisServiceProvider = new clsServiceProvider();
			thisServiceProvider.Connect((int) thisServiceProvider.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisServiceProvider.GetByServiceProviderId(52);
			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button13_Click(object sender, System.EventArgs e)
		{
			clsReport thisReport = new clsReport();
			thisReport.Connect((int) thisReport.DatabaseType_MySql(), 
				thisConnection);

			thisReport.thisReportPath = @"C:\Inetpub\wwwroot\WebApplicationTestReport\";
			thisReport.thisRootPath = @"C:\Work\TestReport\";
			thisReport.thisOutputPath = @"test";
			int numRecords = thisReport.CoverLetter(390,1017,0, "CoverLetter390-1017.pdf");
			numRecords += thisReport.CoverLetter(842,2379,0, "CoverLetter842-2379.pdf");

//			int numRecords = thisReport.Invoices(1066,1,  @"this.pdf");
			MessageBox.Show("NumRecords: " + numRecords.ToString());

		}

		private void button21_Click(object sender, System.EventArgs e)
		{
			clsSPPremiseRole thisSPPremiseRole = new clsSPPremiseRole();
			thisSPPremiseRole.Connect((int) thisSPPremiseRole.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisSPPremiseRole.GetDistinctServiceProviderByPremiseId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button22_Click(object sender, System.EventArgs e)
		{
			clsSPPremiseRole thisSPPremiseRole = new clsSPPremiseRole();
			thisSPPremiseRole.Connect((int) thisSPPremiseRole.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisSPPremiseRole.GetByPremiseId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button23_Click(object sender, System.EventArgs e)
		{
			clsProduct thisProduct = new clsProduct();
			thisProduct.Connect((int) thisProduct.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisProduct.GetAll();
			MessageBox.Show("NumRecords: " + numRecords.ToString());
		}

		private void button24_Click(object sender, System.EventArgs e)
		{
			clsPerson thisPerson = new clsPerson();
			thisPerson.Connect((int) thisPerson.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisPerson.GetByPersonId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				+ thisPerson.my_LastName(0) + "\r\n"
				+ thisPerson.my_UserName(0) + "\r\n"
				+ thisPerson.my_Password(0) + "\r\n" 
				);
		}

		private void button25_Click(object sender, System.EventArgs e)
		{
			clsPremise thisPremise = new clsPremise();
			thisPremise.Connect((int) thisPremise.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisPremise.GetByPremiseId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				+ thisPremise.my_CompanyTypeName(0) + "\r\n"
				+ thisPremise.my_Customer_FirstName(0) + "\r\n"
				+ thisPremise.my_Customer_DateStart(0) + "\r\n"
				);
		}

		private void button26_Click(object sender, System.EventArgs e)
		{
			clsCustomer thisCustomer = new clsCustomer();
			thisCustomer.Connect((int) thisCustomer.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisCustomer.GetByCustomerId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				+ thisCustomer.my_CompanyName(0) + "\r\n"
				+ thisCustomer.my_FirstName(0) + "\r\n"
				+ thisCustomer.my_DateStart(0) + "\r\n"
				);
		}

		private void button27_Click(object sender, System.EventArgs e)
		{
			clsServiceProvider thisServiceProvider = new clsServiceProvider();
			thisServiceProvider.Connect((int) thisServiceProvider.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisServiceProvider.GetByServiceProviderId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				+ thisServiceProvider.my_CompanyName(0) + "\r\n"
				+ thisServiceProvider.my_FirstName(0) + "\r\n"
				+ thisServiceProvider.my_ChangeDataId(0) + "\r\n"
				);
		}

		private void button28_Click(object sender, System.EventArgs e)
		{
			clsCustomerGroup thisCustomerGroup = new clsCustomerGroup();
			thisCustomerGroup.Connect((int) thisCustomerGroup.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisCustomerGroup.GetByCustomerGroupId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				+ thisCustomerGroup.my_CustomerGroupName(0) + "\r\n"
				+ thisCustomerGroup.my_CustomerGroupId(0).ToString() + "\r\n"
				+ thisCustomerGroup.my_ChangeDataId(0) + "\r\n"
				);
		}

		private void button29_Click(object sender, System.EventArgs e)
		{
			clsUser thisUser = new clsUser();
			thisUser.Connect((int) thisUser.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisUser.GetByUserId(1);
			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				+ thisUser.my_LastName(0) + "\r\n"
				+ thisUser.my_UserName(0) + "\r\n"
				+ thisUser.my_Password(0) + "\r\n" 
				);
		}

		private void button30_Click(object sender, System.EventArgs e)
		{
			

			clsUser thisUser = new clsUser();
			thisUser.Connect((int) thisUser.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = 0;
			numRecords = 0;

			numRecords = thisUser.GetByUserNameAndPassword("Mike", "tmch33");
			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				+ thisUser.my_LastName(0) + "\r\n"
				+ thisUser.my_UserName(0) + "\r\n"
				+ thisUser.my_Password(0) + "\r\n" 
				);
		}

		private void button31_Click(object sender, System.EventArgs e)
		{
		
			clsAddress thisAddress = new clsAddress();
			thisAddress.Connect((int) thisAddress.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = thisAddress.GetByPremiseId(979);

			thisAddress.Modify(thisAddress.my_AddressId(0),
				thisAddress.my_POBoxType(0),
				thisAddress.my_BuildingName(0),
				thisAddress.my_UnitNumber(0),
				thisAddress.my_Number(0),
				thisAddress.my_StreetAddress(0),
				thisAddress.my_Suburb(0),
				thisAddress.my_CityId(0),
				thisAddress.my_CityName(0),
				thisAddress.my_StateName(0),
				thisAddress.my_CountryId(0),
				thisAddress.my_CountryName(0),
				thisAddress.my_PostCode(0),
				thisAddress.my_AddressType(0),
				thisAddress.my_AddressTypeDescription(0),
				thisAddress.my_AssocTableName(0),
				thisAddress.my_AssocRowId(0),
				1);

			thisAddress.Save();
			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				);
		}

		private void button32_Click(object sender, System.EventArgs e)
		{
			clsUser thisUser = new clsUser();
			thisUser.Connect((int) thisUser.DatabaseType_MySql(), 
				thisConnection);

			thisUser.Add(5880,
				"Andrew",
				"Halligan",
				"andrew",
				"andrew",
				"",
				4,
				2);

			int numRecords = thisUser.Save();
			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				);
		}

		private void button33_Click(object sender, System.EventArgs e)
		{
			clsHighRiskGood thisHighRiskGood = new clsHighRiskGood();
			thisHighRiskGood.Connect((int) thisHighRiskGood.DatabaseType_MySql(), 
				thisConnection);

			int numRecords = thisHighRiskGood.GetByNameInformationType("dog", 0);

			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				);
		}

		private void button34_Click(object sender, System.EventArgs e)
		{
			clsTransaction thisTransaction = new clsTransaction();
			thisTransaction.Connect((int) thisTransaction.DatabaseType_MySql(), 
				thisConnection);

			thisTransaction.AddByVendor(1,1,"","",0,1,0,0,0,Convert.ToDecimal(99.99), DateTime.Now.ToString(),
				"VendorMemo", "UserIP");

			thisTransaction.Save();


			int numRecords = thisTransaction.GetByCustomerId (1);

			MessageBox.Show("NumRecords: " + numRecords.ToString() + "\r\n"
				);
		}

		private void button35_Click(object sender, System.EventArgs e)
		{
			clsSearch thisSearch = new clsSearch();
			thisSearch.Connect((int) thisSearch.DatabaseType_MySql(), 
				thisConnection);

			thisSearch.GetBySearchId(1);

			thisSearch.Save();

		}

		private void button36_Click(object sender, System.EventArgs e)
		{
			
			DateTime thisDate = new DateTime(Convert.ToInt32(txtYear.Text),
				Convert.ToInt32(txtMonth.Text),
				Convert.ToInt32(txtDay.Text));

			clsImportData thisImportData = new clsImportData();
			thisImportData.Connect((int) thisImportData.DatabaseType_MySql(), 
				thisConnection);

            int SeedInvoiceNumber = 1;
            string LogFolder = @"W:\logs\";
            string fileToExportTo = @"export.csv";


            thisImportData.DoCorrespondence(thisDate, SeedInvoiceNumber, LogFolder, fileToExportTo);
			

			MessageBox.Show("Done!");
			
		}


		private void button37_Click(object sender, System.EventArgs e)
		{
			clsOrder thisOrder = new clsOrder();
			thisOrder.Connect((int) thisOrder.DatabaseType_MySql(), 
				thisConnection);

			clsOrder thisInternalOrder = new clsOrder();
			thisInternalOrder.Connect((int) thisInternalOrder.DatabaseType_MySql(), 
				thisConnection);

			int numOrders = thisOrder.GetAll();

			for(int counter = 0; counter < numOrders; counter++)
			{

				thisInternalOrder.UpdateOrderCosts(thisOrder.my_OrderId(counter));
				thisInternalOrder.Save();
			}
		}

		private void button38_Click(object sender, System.EventArgs e)
		{
			clsPremise thisPremise = new clsPremise();
			thisPremise.Connect((int) thisPremise.DatabaseType_MySql(), 
				thisConnection);

			DateTime thisDate = DateTime.Now;

			thisPremise.SetDetailsUpdateRequired(thisPremise.localRecords.DBDateTime(thisDate));

			MessageBox.Show("Done");

		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			if (System.IO.File.Exists(thisFileFolder + thisConnectionAndLogFileName))
			{
				clsCsvReader fileReader = new clsCsvReader(thisFileFolder + thisConnectionAndLogFileName);
				string[] thisLine = fileReader.GetCsvLine();
				thisConnection = thisLine[0];
				fileReader.Dispose();			
			}		
			if (thisConnection == "")
				thisConnection = KateConnection;

            SetFieldsFromConnectionString(thisConnection);
        }

		private void button39_Click(object sender, System.EventArgs e)
		{

			clsProductCustomerGroupPrice thisProductCustomerGroupPrice = new clsProductCustomerGroupPrice();
			thisProductCustomerGroupPrice.Connect((int) thisProductCustomerGroupPrice.DatabaseType_MySql(), 
				thisConnection);

			int numProducts = thisProductCustomerGroupPrice.GetByProductId(1);

			for(int counter = 0; counter < numProducts; counter++)
			{
				thisProductCustomerGroupPrice.Set(thisProductCustomerGroupPrice.my_ProductId(counter),
					thisProductCustomerGroupPrice.my_CustomerGroupId(counter),
					thisProductCustomerGroupPrice.my_Price(counter) - 1);
			}
			thisProductCustomerGroupPrice.Save();

			MessageBox.Show("Done");
		}

		private void button40_Click(object sender, System.EventArgs e)
		{
			clsPremise thisPremise = new clsPremise();
			thisPremise.Connect((int) thisPremise.DatabaseType_MySql(), 
				thisConnection);

			DateTime thisDate = DateTime.Now;

			thisPremise.MarkCorrespondenceAsSent(
				thisPremise.localRecords.DBDateTime(thisDate)
				);



			MessageBox.Show("Done");
		}

		private void button41_Click(object sender, System.EventArgs e)
		{

			clsImportData thisImportData = new clsImportData();
			thisImportData.Connect((int) thisImportData.DatabaseType_MySql(), 
				thisConnection);
		
			int retVal = thisImportData.ExportContactsForMailMerge();

			MessageBox.Show("Done: " + retVal.ToString());
		}

		private void button42_Click(object sender, System.EventArgs e)
		{
			clsImportData thisImportData = new clsImportData();
			thisImportData.Connect((int) thisImportData.DatabaseType_MySql(), 
				thisConnection);
		
			int retVal = 0;
			thisImportData.AddUsersForPeople();

			MessageBox.Show("Done: " + retVal.ToString());

		}

		private void button43_Click(object sender, System.EventArgs e)
		{
			clsTransaction thisTransaction = new clsTransaction();
			thisTransaction.Connect((int) thisTransaction.DatabaseType_MySql(), 
				thisConnection);

            clsCustomer thisCustomer = new clsCustomer();
            thisCustomer.Connect((int)thisCustomer.DatabaseType_MySql(),
                thisConnection);

            int numCustomers = thisCustomer.GetAll();
            int retVal = numCustomers;

            //int numTransactions = thisTransaction.GetByNullPostBalance();
            //int retVal = numTransactions;

            //for(int counter = 0; counter < numTransactions; counter++)
            //{
            for (int counter = 0; counter < numCustomers; counter++)
            {
                //int thisCustomerId = thisTransaction.my_CustomerId(counter);
                int thisCustomerId = thisCustomer.my_CustomerId(counter);
                clsTransaction editTran = new clsTransaction();
				editTran.Connect((int) editTran.DatabaseType_MySql(), 
					thisConnection);

                editTran.UpdateCustomerTransactions(thisCustomerId);

			}

			MessageBox.Show("Done: " + retVal.ToString());

		}

		private void button44_Click(object sender, System.EventArgs e)
		{
			clsCustomer thisCustomer = new clsCustomer();
			thisCustomer.Connect((int) thisCustomer.DatabaseType_MySql(), 
				thisConnection);
		

			int numCustomers = thisCustomer.GetAll();

			MessageBox.Show("numCustomers: " + numCustomers.ToString());

			for(int counter = 0; counter < numCustomers; counter++)
			{
				clsOrder thisOrder = new clsOrder();
				thisOrder.Connect((int) thisOrder.DatabaseType_MySql(), 
					thisConnection);

				int thisCustomerId = thisCustomer.my_CustomerId(counter);

				thisOrder.UpdateCustomerOrders(thisCustomerId);

				clsTransaction thisTransaction = new clsTransaction();
				thisTransaction.Connect((int) thisTransaction.DatabaseType_MySql(), 
					thisConnection);

				thisTransaction.UpdateCustomerTransactions(thisCustomerId);

			}

			MessageBox.Show("Done");
		}

		private void button46_Click(object sender, System.EventArgs e)
		{
			KateConnection = "DRIVER={mySql ODBC 5.3 Unicode Driver};SERVER=" + txtServer.Text +";port=3306;UID=graham;PASSWORD=1fuckyou;DATABASE=kdl_mod3";
			thisConnection = KateConnection;
            MessageBox.Show("Connection Updated");
		}

		private void button48_Click(object sender, System.EventArgs e)
		{
			#region Get rid of the triplicate orders
			clsOrder thisOrder = new clsOrder();
			thisOrder.Connect((int) thisOrder.DatabaseType_MySql(), 
				thisConnection);

		
			int numOrders = thisOrder.GetByDateCreated("2011-11-09 12:17:57");

			for(int counter = 0; counter < numOrders; counter++)
			{
				int thisOrderId = thisOrder.my_OrderId(counter);
				int thisCustomerId = thisOrder.my_CustomerId(counter);

				#region Deal with Items

				clsItem thisDelItem = new clsItem();
				thisDelItem.Connect((int) thisDelItem.DatabaseType_MySql(), 
					thisConnection);

				int numItems = thisDelItem.GetByOrderId(thisOrderId);
				for (int tranCounter = 0; tranCounter < numItems; tranCounter++)
				{
					thisDelItem.Delete(thisDelItem.my_ItemId(tranCounter));
				}
				thisDelItem.Save();

				#endregion

				#region Deal with Transactions

				clsTransaction thisDelTransaction = new clsTransaction();
				thisDelTransaction.Connect((int) thisDelTransaction.DatabaseType_MySql(), 
					thisConnection);

				int numTransactions = thisDelTransaction.GetByOrderId(thisOrderId);
				for (int tranCounter = 0; tranCounter < numTransactions; tranCounter++)
				{
					thisDelTransaction.Delete(thisDelTransaction.my_TransactionId(tranCounter));
				}
				thisDelTransaction.Save();

				thisDelTransaction.UpdateCustomerTransactions(thisCustomerId);

				#endregion

				#region Deal with Orders

				clsOrder thisDelOrder = new clsOrder();
				thisDelOrder.Connect((int) thisOrder.DatabaseType_MySql(), 
					thisConnection);

				thisDelOrder.Delete(thisOrderId);
				thisDelOrder.Save();

				thisDelOrder.UpdateCustomerOrders(thisCustomerId);

				#endregion


			}

			MessageBox.Show("Got rid of triplicates. numOrders: " + numOrders.ToString());

			#endregion

			#region Get rid of duplicates
			
			numOrders = thisOrder.GetByDateCreated("2011-11-08 15:40:19");

			for(int counter = 0; counter < numOrders; counter++)
			{
				int thisOrderId = thisOrder.my_OrderId(counter);
				int thisCustomerId = thisOrder.my_CustomerId(counter);


				if (thisCustomerId != 2835
					&& thisCustomerId != 2837)
				{
					if (thisCustomerId == 1313)
					{
						thisOrderId = 22416;

					}

					#region Deal with Items

					clsItem thisDelItem = new clsItem();
					thisDelItem.Connect((int) thisDelItem.DatabaseType_MySql(), 
						thisConnection);

					int numItems = thisDelItem.GetByOrderId(thisOrderId);
					for (int tranCounter = 0; tranCounter < numItems; tranCounter++)
					{
						thisDelItem.Delete(thisDelItem.my_ItemId(tranCounter));
					}
					thisDelItem.Save();


					#endregion

					#region Deal with Transactions

					clsTransaction thisDelTransaction = new clsTransaction();
					thisDelTransaction.Connect((int) thisDelTransaction.DatabaseType_MySql(), 
						thisConnection);

					int numTransactions = thisDelTransaction.GetByOrderId(thisOrderId);
					for (int tranCounter = 0; tranCounter < numTransactions; tranCounter++)
					{
						thisDelTransaction.Delete(thisDelTransaction.my_TransactionId(tranCounter));
					}
					thisDelTransaction.Save();

					thisDelTransaction.UpdateCustomerTransactions(thisCustomerId);

					#endregion

					#region Deal with Orders

					clsOrder thisDelOrder = new clsOrder();
					thisDelOrder.Connect((int) thisOrder.DatabaseType_MySql(), 
						thisConnection);

					thisDelOrder.Delete(thisOrderId);
					thisDelOrder.Save();

					thisDelOrder.UpdateCustomerOrders(thisCustomerId);

					#endregion



				}

			}

			MessageBox.Show("Got rid of duplicates. numOrders: " + numOrders.ToString());
			#endregion

			#region Mark all the rest of the invoices as needing a copy sent
			
			numOrders = thisOrder.GetAll();

			for(int counter = 0; counter < numOrders; counter++)
			{
				int thisOrderId = thisOrder.my_OrderId(counter);
				int thisCustomerId = thisOrder.my_CustomerId(counter);

				if (thisOrderId > 22042)
				{

					clsOrder thisModOrder = new clsOrder();
					thisModOrder.Connect((int) thisOrder.DatabaseType_MySql(), 
						thisConnection);

					thisModOrder.SetAttribute(thisOrderId, "InvoiceRequested", "1");
					thisModOrder.Save();

				}
			}

			MessageBox.Show("Got rid of duplicates. numOrders: " + numOrders.ToString());
			#endregion

		}

		private void button47_Click(object sender, System.EventArgs e)
		{

			clsUser thisUser = new clsUser();
			thisUser.Connect((int) thisUser.DatabaseType_MySql(), 
				thisConnection);

			int numPersons = thisUser.EnsureAllPeopleHaveUsersAndCreds();

			MessageBox.Show(numPersons.ToString());
			
		}

		private void button49_Click(object sender, System.EventArgs e)
		{
			clsOrder thisOrder = new clsOrder();
			thisOrder.Connect((int) thisOrder.DatabaseType_MySql(), 
				thisConnection);

            int SeedInvoiceNumber = 1;
            string LogFolder = @"W:\logs\";
            string fileToExportTo = @"export.csv";


            thisOrder.SubmitAllUnsubmittedOrders(DateTime.Now.ToString(), SeedInvoiceNumber, LogFolder, fileToExportTo);

		
		}

		private void button50_Click(object sender, System.EventArgs e)
		{
			clsImportData thisImportData = new clsImportData();
			thisImportData.Connect((int) thisImportData.DatabaseType_MySql(), 
				thisConnection);

			int numEmails = thisImportData.CreditNote();

			MessageBox.Show(numEmails.ToString());

		}

		private void button51_Click(object sender, System.EventArgs e)
		{
			#region Get rid of the triplicate orders
			clsOrder thisOrder = new clsOrder();
			thisOrder.Connect((int) thisOrder.DatabaseType_MySql(), 
				thisConnection);

		
			int numOrders = thisOrder.GetByOrderId(Convert.ToInt32(txtOrderId.Text));

			for(int counter = 0; counter < numOrders; counter++)
			{
				int thisOrderId = thisOrder.my_OrderId(counter);
				int thisCustomerId = thisOrder.my_CustomerId(counter);

				#region Deal with Items

				clsItem thisDelItem = new clsItem();
				thisDelItem.Connect((int) thisDelItem.DatabaseType_MySql(), 
					thisConnection);

				int numItems = thisDelItem.GetByOrderId(thisOrderId);
				for (int tranCounter = 0; tranCounter < numItems; tranCounter++)
				{
					thisDelItem.Delete(thisDelItem.my_ItemId(tranCounter));
				}
				thisDelItem.Save();

				#endregion

				#region Deal with Transactions

				clsTransaction thisDelTransaction = new clsTransaction();
				thisDelTransaction.Connect((int) thisDelTransaction.DatabaseType_MySql(), 
					thisConnection);

				int numTransactions = thisDelTransaction.GetByOrderId(thisOrderId);
				for (int tranCounter = 0; tranCounter < numTransactions; tranCounter++)
				{
					thisDelTransaction.Delete(thisDelTransaction.my_TransactionId(tranCounter));
				}
				thisDelTransaction.Save();

				thisDelTransaction.UpdateCustomerTransactions(thisCustomerId);

				#endregion

				#region Deal with Orders

				clsOrder thisDelOrder = new clsOrder();
				thisDelOrder.Connect((int) thisOrder.DatabaseType_MySql(), 
					thisConnection);

				thisDelOrder.Delete(thisOrderId);
				thisDelOrder.Save();

				thisDelOrder.UpdateCustomerOrders(thisCustomerId);

				#endregion


			}

			MessageBox.Show("Got rid of Order. numOrders: " + numOrders.ToString());

			#endregion

		}

		private void button52_Click(object sender, System.EventArgs e)
		{
			clsPremise thisPremise = new clsPremise();
			thisPremise.Connect((int) thisPremise.DatabaseType_MySql(), 
				thisConnection);

			int numResults = thisPremise.GetByName(txtSearch.Text);

			MessageBox.Show(numResults.ToString());
		}


        /// <summary>
        /// GetConnectionStringFromFields
        /// </summary>
        /// <returns>ConnectionString</returns>
        public string GetConnectionStringFromFields()
        {

            string thisCon = "DRIVER=" + txtConDriver.Text + ";"
                + "SERVER=" + txtConServer.Text + ";"
                ;

            if (txtConPort.Text.Trim() != "")
                thisCon += "PORT=" + txtConPort.Text + ";"
                ;

            thisCon += "UID=" + txtConUid.Text + ";"
                + "PASSWORD=" + txtConPassword.Text + ";"
                + "DATABASE=" + txtConDatabase.Text
                ;

            return thisCon;

        }

        public void SetFieldsFromConnectionString(string thisConnectionString)
        {

            clsSetting thisSetting = new clsSetting();
            //            thisSetting.Connect(thisDbType, thisConnection);
            thisSetting.Connect((int)thisSetting.DatabaseType_SqlServer(), KateConnection);


            #region Generate the Delimited list

            if (!thisConnectionString.EndsWith(";"))
                thisConnectionString += ";";

            ArrayList thisNewList = thisSetting.DelimitedListToArrayList(thisConnectionString);

            #endregion


            foreach (object thisNewObject in thisNewList)
            {
                #region Find and Update the Variable

                string fullString = thisNewObject.ToString();
                int equals = fullString.IndexOf("=");

                if (equals > 0 && equals < fullString.Length)
                {
                    string variableName = fullString.Substring(0, equals).Trim().ToUpper();
                    string variableValue = fullString.Substring(equals + 1).Trim();

                    switch (variableName)
                    {
                        case "DRIVER":
                            txtConDriver.Text = variableValue;
                            break;
                        case "SERVER":
                            txtConServer.Text = variableValue;
                            break;
                        case "PORT":
                            txtConPort.Text = variableValue;
                            break;
                        case "UID":
                            txtConUid.Text = variableValue;
                            break;
                        case "PASSWORD":
                        case "PWD":
                            txtConPassword.Text = variableValue;
                            break;
                        case "DATABASE":
                            txtConDatabase.Text = variableValue;
                            break;
                        default:
                            break;
                    }

                }

                #endregion
            }

        }

        private void butConUpdateNow_Click(object sender, EventArgs e)
        {
            thisConnection = GetConnectionStringFromFields();

            MessageBox.Show("Connection Updated.");
        }



        public void SaveConnectionDefaults()
        {
            string[] thisLine = new string[1];

            if (System.IO.File.Exists(thisFileFolder + thisConnectionAndLogFileName))
            {
                clsCsvReader fileReader = new clsCsvReader(thisFileFolder + thisConnectionAndLogFileName);
                thisLine = fileReader.GetCsvLine();
                fileReader.Dispose();
            }

            thisLine[0] = GetConnectionStringFromFields();

            clsCsvWriter thisWriter = new clsCsvWriter(thisFileFolder + thisConnectionAndLogFileName, false);

            thisWriter.WriteFields(thisLine);

            thisWriter.Close();

            clsSetting thisSetting = new clsSetting();
            thisSetting.Connect(thisDbType, thisConnection);

            thisSetting.GetGeneralSettings();

        }

        private void butConUpdateDefaults_Click(object sender, EventArgs e)
        {
            SaveConnectionDefaults();
            MessageBox.Show("File: " + thisConnectionAndLogFileName + " Updated.");
        }
    }
}
