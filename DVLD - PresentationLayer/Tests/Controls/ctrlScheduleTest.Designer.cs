namespace DVLD___Driving_License_Management.Tests.Controls
{
    partial class ctrlScheduleTest
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbTestType = new System.Windows.Forms.GroupBox();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.pbTestTypeImage = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblUserMessage = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.gbRetokenTest = new System.Windows.Forms.GroupBox();
            this.dadwwd = new System.Windows.Forms.Label();
            this.lblTotalFees = new System.Windows.Forms.Label();
            this.dwa = new System.Windows.Forms.Label();
            this.sasd = new System.Windows.Forms.Label();
            this.lblRetokenTestID = new System.Windows.Forms.Label();
            this.lblRetokenAppFees = new System.Windows.Forms.Label();
            this.fd = new System.Windows.Forms.Label();
            this.lbleTestFees = new System.Windows.Forms.Label();
            this.sda = new System.Windows.Forms.Label();
            this.sasa = new System.Windows.Forms.Label();
            this.lblLDLAppID = new System.Windows.Forms.Label();
            this.dsa = new System.Windows.Forms.Label();
            this.dfds = new System.Windows.Forms.Label();
            this.lblDClassName = new System.Windows.Forms.Label();
            this.sas = new System.Windows.Forms.Label();
            this.lblApplicantName = new System.Windows.Forms.Label();
            this.lblTrails = new System.Windows.Forms.Label();
            this.gbTestType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTestTypeImage)).BeginInit();
            this.gbRetokenTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTestType
            // 
            this.gbTestType.Controls.Add(this.dtDate);
            this.gbTestType.Controls.Add(this.pbTestTypeImage);
            this.gbTestType.Controls.Add(this.btnSave);
            this.gbTestType.Controls.Add(this.lblUserMessage);
            this.gbTestType.Controls.Add(this.lblHeader);
            this.gbTestType.Controls.Add(this.gbRetokenTest);
            this.gbTestType.Controls.Add(this.fd);
            this.gbTestType.Controls.Add(this.lbleTestFees);
            this.gbTestType.Controls.Add(this.sda);
            this.gbTestType.Controls.Add(this.sasa);
            this.gbTestType.Controls.Add(this.lblLDLAppID);
            this.gbTestType.Controls.Add(this.dsa);
            this.gbTestType.Controls.Add(this.dfds);
            this.gbTestType.Controls.Add(this.lblDClassName);
            this.gbTestType.Controls.Add(this.sas);
            this.gbTestType.Controls.Add(this.lblApplicantName);
            this.gbTestType.Controls.Add(this.lblTrails);
            this.gbTestType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTestType.Location = new System.Drawing.Point(3, 3);
            this.gbTestType.Name = "gbTestType";
            this.gbTestType.Size = new System.Drawing.Size(455, 647);
            this.gbTestType.TabIndex = 21;
            this.gbTestType.TabStop = false;
            this.gbTestType.Text = "Vision Test";
            // 
            // dtDate
            // 
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDate.Location = new System.Drawing.Point(167, 383);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(123, 24);
            this.dtDate.TabIndex = 20;
            // 
            // pbTestTypeImage
            // 
            this.pbTestTypeImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbTestTypeImage.Image = global::DVLD___Driving_License_Management.Properties.Resources.Vision_512;
            this.pbTestTypeImage.Location = new System.Drawing.Point(162, 23);
            this.pbTestTypeImage.Name = "pbTestTypeImage";
            this.pbTestTypeImage.Size = new System.Drawing.Size(128, 116);
            this.pbTestTypeImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbTestTypeImage.TabIndex = 14;
            this.pbTestTypeImage.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSave.Location = new System.Drawing.Point(331, 585);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(111, 44);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblUserMessage
            // 
            this.lblUserMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserMessage.ForeColor = System.Drawing.Color.Brown;
            this.lblUserMessage.Location = new System.Drawing.Point(20, 175);
            this.lblUserMessage.Name = "lblUserMessage";
            this.lblUserMessage.Size = new System.Drawing.Size(416, 18);
            this.lblUserMessage.TabIndex = 13;
            this.lblUserMessage.Text = "Can\'t Schedule, Vision Test Should be Passed First.";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.Brown;
            this.lblHeader.Location = new System.Drawing.Point(117, 142);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(214, 33);
            this.lblHeader.TabIndex = 13;
            this.lblHeader.Text = "Schedule Test";
            // 
            // gbRetokenTest
            // 
            this.gbRetokenTest.Controls.Add(this.dadwwd);
            this.gbRetokenTest.Controls.Add(this.lblTotalFees);
            this.gbRetokenTest.Controls.Add(this.dwa);
            this.gbRetokenTest.Controls.Add(this.sasd);
            this.gbRetokenTest.Controls.Add(this.lblRetokenTestID);
            this.gbRetokenTest.Controls.Add(this.lblRetokenAppFees);
            this.gbRetokenTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbRetokenTest.Location = new System.Drawing.Point(9, 468);
            this.gbRetokenTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbRetokenTest.Name = "gbRetokenTest";
            this.gbRetokenTest.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbRetokenTest.Size = new System.Drawing.Size(428, 98);
            this.gbRetokenTest.TabIndex = 16;
            this.gbRetokenTest.TabStop = false;
            this.gbRetokenTest.Text = "Retake Test Info:";
            // 
            // dadwwd
            // 
            this.dadwwd.AutoSize = true;
            this.dadwwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dadwwd.Location = new System.Drawing.Point(28, 67);
            this.dadwwd.Name = "dadwwd";
            this.dadwwd.Size = new System.Drawing.Size(100, 18);
            this.dadwwd.TabIndex = 15;
            this.dadwwd.Text = "R.App.Fees:";
            // 
            // lblTotalFees
            // 
            this.lblTotalFees.AutoSize = true;
            this.lblTotalFees.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalFees.Location = new System.Drawing.Point(382, 67);
            this.lblTotalFees.Name = "lblTotalFees";
            this.lblTotalFees.Size = new System.Drawing.Size(45, 18);
            this.lblTotalFees.TabIndex = 15;
            this.lblTotalFees.Text = "[$$$]";
            // 
            // dwa
            // 
            this.dwa.AutoSize = true;
            this.dwa.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dwa.Location = new System.Drawing.Point(11, 26);
            this.dwa.Name = "dwa";
            this.dwa.Size = new System.Drawing.Size(117, 18);
            this.dwa.TabIndex = 15;
            this.dwa.Text = "R.Test.App ID:";
            // 
            // sasd
            // 
            this.sasd.AutoSize = true;
            this.sasd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sasd.Location = new System.Drawing.Point(283, 67);
            this.sasd.Name = "sasd";
            this.sasd.Size = new System.Drawing.Size(93, 18);
            this.sasd.TabIndex = 15;
            this.sasd.Text = "Total Fees:";
            // 
            // lblRetokenTestID
            // 
            this.lblRetokenTestID.AutoSize = true;
            this.lblRetokenTestID.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRetokenTestID.Location = new System.Drawing.Point(150, 26);
            this.lblRetokenTestID.Name = "lblRetokenTestID";
            this.lblRetokenTestID.Size = new System.Drawing.Size(35, 18);
            this.lblRetokenTestID.TabIndex = 15;
            this.lblRetokenTestID.Text = "N/A";
            // 
            // lblRetokenAppFees
            // 
            this.lblRetokenAppFees.AutoSize = true;
            this.lblRetokenAppFees.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRetokenAppFees.Location = new System.Drawing.Point(150, 67);
            this.lblRetokenAppFees.Name = "lblRetokenAppFees";
            this.lblRetokenAppFees.Size = new System.Drawing.Size(45, 18);
            this.lblRetokenAppFees.TabIndex = 15;
            this.lblRetokenAppFees.Text = "[$$$]";
            // 
            // fd
            // 
            this.fd.AutoSize = true;
            this.fd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fd.Location = new System.Drawing.Point(50, 217);
            this.fd.Name = "fd";
            this.fd.Size = new System.Drawing.Size(93, 18);
            this.fd.TabIndex = 15;
            this.fd.Text = "D.L.App ID:";
            // 
            // lbleTestFees
            // 
            this.lbleTestFees.AutoSize = true;
            this.lbleTestFees.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbleTestFees.Location = new System.Drawing.Point(164, 424);
            this.lbleTestFees.Name = "lbleTestFees";
            this.lbleTestFees.Size = new System.Drawing.Size(45, 18);
            this.lbleTestFees.TabIndex = 15;
            this.lbleTestFees.Text = "[$$$]";
            // 
            // sda
            // 
            this.sda.AutoSize = true;
            this.sda.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sda.Location = new System.Drawing.Point(65, 257);
            this.sda.Name = "sda";
            this.sda.Size = new System.Drawing.Size(78, 18);
            this.sda.TabIndex = 15;
            this.sda.Text = "D. Class:";
            // 
            // sasa
            // 
            this.sasa.AutoSize = true;
            this.sasa.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sasa.Location = new System.Drawing.Point(93, 424);
            this.sasa.Name = "sasa";
            this.sasa.Size = new System.Drawing.Size(50, 18);
            this.sasa.TabIndex = 15;
            this.sasa.Text = "Fees:";
            // 
            // lblLDLAppID
            // 
            this.lblLDLAppID.AutoSize = true;
            this.lblLDLAppID.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLDLAppID.Location = new System.Drawing.Point(164, 217);
            this.lblLDLAppID.Name = "lblLDLAppID";
            this.lblLDLAppID.Size = new System.Drawing.Size(35, 18);
            this.lblLDLAppID.TabIndex = 15;
            this.lblLDLAppID.Text = "N/A";
            // 
            // dsa
            // 
            this.dsa.AutoSize = true;
            this.dsa.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dsa.Location = new System.Drawing.Point(81, 300);
            this.dsa.Name = "dsa";
            this.dsa.Size = new System.Drawing.Size(62, 18);
            this.dsa.TabIndex = 15;
            this.dsa.Text = "Name: ";
            // 
            // dfds
            // 
            this.dfds.AutoSize = true;
            this.dfds.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfds.Location = new System.Drawing.Point(94, 383);
            this.dfds.Name = "dfds";
            this.dfds.Size = new System.Drawing.Size(48, 18);
            this.dfds.TabIndex = 15;
            this.dfds.Text = "Date:";
            // 
            // lblDClassName
            // 
            this.lblDClassName.AutoSize = true;
            this.lblDClassName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDClassName.Location = new System.Drawing.Point(164, 257);
            this.lblDClassName.Name = "lblDClassName";
            this.lblDClassName.Size = new System.Drawing.Size(54, 18);
            this.lblDClassName.TabIndex = 15;
            this.lblDClassName.Text = "[????]";
            // 
            // sas
            // 
            this.sas.AutoSize = true;
            this.sas.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sas.Location = new System.Drawing.Point(88, 344);
            this.sas.Name = "sas";
            this.sas.Size = new System.Drawing.Size(55, 18);
            this.sas.TabIndex = 15;
            this.sas.Text = "Trials:";
            // 
            // lblApplicantName
            // 
            this.lblApplicantName.AutoSize = true;
            this.lblApplicantName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicantName.Location = new System.Drawing.Point(164, 300);
            this.lblApplicantName.Name = "lblApplicantName";
            this.lblApplicantName.Size = new System.Drawing.Size(54, 18);
            this.lblApplicantName.TabIndex = 15;
            this.lblApplicantName.Text = "[????]";
            // 
            // lblTrails
            // 
            this.lblTrails.AutoSize = true;
            this.lblTrails.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrails.Location = new System.Drawing.Point(164, 344);
            this.lblTrails.Name = "lblTrails";
            this.lblTrails.Size = new System.Drawing.Size(17, 18);
            this.lblTrails.TabIndex = 15;
            this.lblTrails.Text = "0";
            // 
            // ctrlScheduleTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTestType);
            this.Name = "ctrlScheduleTest";
            this.Size = new System.Drawing.Size(464, 658);
            this.gbTestType.ResumeLayout(false);
            this.gbTestType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTestTypeImage)).EndInit();
            this.gbRetokenTest.ResumeLayout(false);
            this.gbRetokenTest.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTestType;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.PictureBox pbTestTypeImage;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.GroupBox gbRetokenTest;
        private System.Windows.Forms.Label dadwwd;
        private System.Windows.Forms.Label lblTotalFees;
        private System.Windows.Forms.Label dwa;
        private System.Windows.Forms.Label sasd;
        private System.Windows.Forms.Label lblRetokenTestID;
        private System.Windows.Forms.Label lblRetokenAppFees;
        private System.Windows.Forms.Label fd;
        private System.Windows.Forms.Label lbleTestFees;
        private System.Windows.Forms.Label sda;
        private System.Windows.Forms.Label sasa;
        private System.Windows.Forms.Label lblLDLAppID;
        private System.Windows.Forms.Label dsa;
        private System.Windows.Forms.Label dfds;
        private System.Windows.Forms.Label lblDClassName;
        private System.Windows.Forms.Label sas;
        private System.Windows.Forms.Label lblApplicantName;
        private System.Windows.Forms.Label lblTrails;
        private System.Windows.Forms.Label lblUserMessage;
    }
}
