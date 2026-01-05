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

namespace DVLD___Driving_License_Management.Applications.Replace_Lost_Or_Damaged_License
{
    public partial class frmReplaceLostOrDamagedLicense : Form
    {
        private int _ReplacementLicenseID = -1;
        
        private clsLicense _SelectedLicense;

        private clsLicense.enIssueReason _IssuedReason;

        public frmReplaceLostOrDamagedLicense()
        {
            InitializeComponent();
        }

        private void frmReplaceLostOrDamagedLicense_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.Focus();

            lblAppDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            lblCreatedByUserID.Text = clsGlobal.LoggedInUser.UserName.ToString();

            btnIssueReplacement.Enabled = false;

            lnklblShowLicenseHistory.Enabled = false;
            
            lnklblShowNewLicenseInfo.Enabled = false;

            rbDamagedLicense.Checked = true;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            _SelectedLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;

            if (_SelectedLicense == null)
            {
                ctrlDriverLicenseInfoWithFilter1.Focus();
                return;
            }

            lnklblShowLicenseHistory.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)
                return;

            // Check License is Active
            if (!_SelectedLicense.IsActive)
            {
                MessageBox.Show($"The selected license is not active.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblOldLicenseID.Text = _SelectedLicense.LicenseID.ToString();
            btnIssueReplacement.Enabled = true;

        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Issue Replacement License?", "Confirm Issued?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsLicense ReplacementLicense = _SelectedLicense.Replace(_IssuedReason, clsGlobal.LoggedInUser.UserID);

            if (ReplacementLicense == null)
            {
                MessageBox.Show("An error occurred while Issued this License.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _ReplacementLicenseID = ReplacementLicense.LicenseID;

            lblReplacementLicenseID.Text = ReplacementLicense.LicenseID.ToString();

            lblRLAppID.Text = ReplacementLicense.ApplicationID.ToString();

            btnIssueReplacement.Enabled = false;
            
            gbReplacementReason.Enabled = false;

            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

            MessageBox.Show($"The license has been Replacement successfully with ID: [{_ReplacementLicenseID}].", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            lnklblShowNewLicenseInfo.Enabled = true;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            _IssuedReason = clsLicense.enIssueReason.DamagedReplacement;

            lblAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedLicense).ApplicationFees.ToString();

            lblHeader.Text = "Replace Damaged License";
            
            this.Text = lblHeader.Text;

        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            _IssuedReason = clsLicense.enIssueReason.LostReplacement;

            lblAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceLostLicense).ApplicationFees.ToString();

            lblHeader.Text = "Replace Lost License";
            this.Text = lblHeader.Text;

        }

        private void lnklblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Todo: Show License History Form

            MessageBox.Show("Show License History Form - To Be Implemented.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lnklblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowDriverLicenseInfo(_ReplacementLicenseID);

            frm.ShowDialog();
        }
    }
}
