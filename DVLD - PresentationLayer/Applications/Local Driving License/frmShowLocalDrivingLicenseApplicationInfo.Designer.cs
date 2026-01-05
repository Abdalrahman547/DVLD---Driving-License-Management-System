namespace DVLD___Driving_License_Management.Application.Local_Driving_License
{
    partial class frmShowLocalDrivingLicenseApplicationInfo
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.ctrlLocalDrivingLicenseApplicationCard1 = new DVLD___Driving_License_Management.Application.Local_Driving_License.ctrlLocalDrivingLicenseApplicationCard();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.Firebrick;
            this.lblHeader.Location = new System.Drawing.Point(116, 54);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(587, 37);
            this.lblHeader.TabIndex = 6;
            this.lblHeader.Text = "Local Driving License Application Info";
            // 
            // ctrlLocalDrivingLicenseApplicationCard1
            // 
            this.ctrlLocalDrivingLicenseApplicationCard1.Location = new System.Drawing.Point(12, 110);
            this.ctrlLocalDrivingLicenseApplicationCard1.Name = "ctrlLocalDrivingLicenseApplicationCard1";
            this.ctrlLocalDrivingLicenseApplicationCard1.Size = new System.Drawing.Size(806, 369);
            this.ctrlLocalDrivingLicenseApplicationCard1.TabIndex = 7;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(698, 485);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 39);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmShowLocalDrivingLicenseApplicationInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 534);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ctrlLocalDrivingLicenseApplicationCard1);
            this.Controls.Add(this.lblHeader);
            this.Name = "frmShowLocalDrivingLicenseApplicationInfo";
            this.Text = "Local Driving License Application Info";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private ctrlLocalDrivingLicenseApplicationCard ctrlLocalDrivingLicenseApplicationCard1;
        private System.Windows.Forms.Button btnClose;
    }
}