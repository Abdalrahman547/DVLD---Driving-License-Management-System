using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.Global_Classes;
using DVLD___Driving_License_Management.Licenses.Local_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD___BussinessLayer.clsLicense;

namespace DVLD___Driving_License_Management.Licenses
{
    public partial class frmIssueLicenseForFirstTime : Form
    {
        private int _LDLAppID = -1;
        
        private int _NewLicenseID = -1;

        private clsLocalDrivingLicenseApplication _LDLApplication = null;

        public frmIssueLicenseForFirstTime(int LDLAppID)
        {
            InitializeComponent();
            this._LDLAppID = LDLAppID;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int LicenseID = _LDLApplication.IssueLicenseForFirstTime(txtNotes.Text.Trim(), clsGlobal.LoggedInUser.UserID);

            if (LicenseID != -1)
            {
                _NewLicenseID = LicenseID;

                MessageBox.Show($"License issued with ID: {LicenseID} successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                btnIssue.Enabled = false;
                txtNotes.Enabled = false;

                ctrlLocalDrivingLicenseApplicationCard1.LoadLDLApplicationInfoByLDLApplicationID(_LDLAppID); // Refresh

                ctrlLocalDrivingLicenseApplicationCard1.ShowLicenseInfo();
            }
            else
            {
                MessageBox.Show("Failed to issue license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
             
        }

        private void frmIssueLicenseForFirstTime_Load(object sender, EventArgs e)
        {

            _LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(_LDLAppID);

            // Validate Application
            if (_LDLApplication == null)
            {
                MessageBox.Show($"No Application With ID: {_LDLAppID}", "Not Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                this.Close();

                return;
            }

            // Must Passed All Tests
            if (!_LDLApplication.DoesPassedAllTests())
            {
                MessageBox.Show("Must Passed All Tests First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();

                return;
            }

            int LicenseID = _LDLApplication.GetActiveLicenseID();

            // Must Not Have Active License
            if (LicenseID != -1)
                            {
                MessageBox.Show($"License Already Issued With ID: {LicenseID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                this.Close();
                
                return;
            }

            txtNotes.Focus();

            ctrlLocalDrivingLicenseApplicationCard1.LoadLDLApplicationInfoByLDLApplicationID(_LDLAppID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
