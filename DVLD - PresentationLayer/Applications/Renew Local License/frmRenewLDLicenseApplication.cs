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

namespace DVLD___Driving_License_Management.Licenses.Renew_Local_Driving_License
{
    public partial class frmRenewLDLicenseApplication : Form
    {

        private int _NewLicenseID = -1;
        private clsLicense _SelectedLicense;

        public frmRenewLDLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmRenewLDLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.Focus();

            lblAppDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            lblIssueDate.Text = lblAppDate.Text;

            lblCreatedByUserID.Text = clsGlobal.LoggedInUser.UserName.ToString();

            lblAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewLicense).ApplicationFees.ToString();

            btnRenew.Enabled = false;

            lnklblShowLicenseHistory.Enabled = false;
            lnklblShowNewLicenseInfo.Enabled = false;

            txtNotes.Enabled = false;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            _SelectedLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;
            
            if(_SelectedLicense == null)
            {
                ctrlDriverLicenseInfoWithFilter1.Focus();
                return;
            }

            lnklblShowLicenseHistory.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)
                return;

            // Check if the license is expired
            if (!_SelectedLicense.IsLicenceExpired())
            {
                MessageBox.Show($"The selected license is not expired yet. it will be expired on: {_SelectedLicense.ExpirationDate}",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check License is Active
            if (!_SelectedLicense.IsActive)
            {   
                MessageBox.Show($"The selected license is not active.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            int DefaultValidityLength = _SelectedLicense.LicenseClassInfo.DefaultValidityLength;

            lblExpirationDate.Text = DateTime.Now.AddYears(DefaultValidityLength).ToString("dd/MM/yyyy");

            lblLicenseFees.Text = _SelectedLicense.LicenseClassInfo.ClassFees.ToString();
            
            lblTotalFees.Text = Convert.ToString(Convert.ToDouble(lblAppFees.Text) + Convert.ToDouble(lblLicenseFees.Text));

            txtNotes.Text = _SelectedLicense.Notes.ToString();

            btnRenew.Enabled = true;

            txtNotes.Enabled = true;
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to renew the selected license?", "Confirm Renewal", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsLicense NewLicense = _SelectedLicense.RenewLicense(txtNotes.Text, clsGlobal.LoggedInUser.UserID);

            if(NewLicense == null)
            {
                MessageBox.Show("An error occurred while renewing the license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
    
            _NewLicenseID = NewLicense.LicenseID;
            
            lblNewLicenseID.Text = NewLicense.LicenseID.ToString();
            
            lblRLAppID.Text = NewLicense.ApplicationID.ToString();

            btnRenew.Enabled = false;

            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

            MessageBox.Show($"The license has been renewed successfully with ID: [{_NewLicenseID}].", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            lnklblShowNewLicenseInfo.Enabled = true;

            txtNotes.Enabled = false;

        }

        private void lnklblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Todo: Show License History Form

            MessageBox.Show("This feature is not implemented yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void lnklblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowDriverLicenseInfo(_NewLicenseID);

            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
