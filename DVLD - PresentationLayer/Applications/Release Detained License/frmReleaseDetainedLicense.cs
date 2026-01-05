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

namespace DVLD___Driving_License_Management.Applications.Release_Detained_License
{
    public partial class frmReleaseDetainedLicense : Form
    {
        private int _SelectedLicenseID = -1;
        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }
        public frmReleaseDetainedLicense(int LicnseID)
        {
            InitializeComponent();
            _SelectedLicenseID = LicnseID;

            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_SelectedLicenseID);
            btnRelease.Enabled = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained;
            

        }
        
        private void _ResetDetainedLicenseInfo()
        {
            lblLicenseID.Text = "[????]";
            lblDetainedLicenseID.Text = "[????]";
            lblFineFees.Text = "[????]";
            btnRelease.Enabled = false;
            lnklblShowLicensesHistory.Enabled = false;
            lnklblShowLicenseInfo.Enabled = false;
            lblApplicationFees.Text = "[????]";
            lblApplicationID.Text = "[????]";
            lblDetainedDate.Text = DateTime.Now.ToString("d");
            lblCreatedBy.Text = clsGlobal.LoggedInUser.UserName;

        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            if(_SelectedLicenseID != -1)
            {
                btnRelease.Enabled = true;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                lnklblShowLicenseInfo.Enabled = true;
                lnklblShowLicensesHistory.Enabled = true;
                return;
            }
            

            _ResetDetainedLicenseInfo();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
           _SelectedLicenseID = obj;

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo == null)
                return;

            _FillDetainedLicenseInfo();
        }
        
        private void _FillDetainedLicenseInfo()
        {

            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("This license is not Detained.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }


            lblDetainedLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainID.ToString();

            lblLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID.ToString();

            lblFineFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.FineFees.ToString("N2");

            lblApplicationFees.Text = clsApplicationType.Find((int) clsApplication.enApplicationType.ReleaseDetainedLicense).ApplicationFees.ToString("N2");

            decimal TotalFees = Convert.ToDecimal(lblFineFees.Text) + Convert.ToDecimal(lblApplicationFees.Text);
            
            lblTotalFees.Text = TotalFees.ToString("N2");

            btnRelease.Enabled = true;

            lnklblShowLicensesHistory.Enabled = true;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to release this detained license?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            btnRelease.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            lnklblShowLicenseInfo.Enabled = true;

            ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ReleaseFromDetain(clsGlobal.LoggedInUser.UserID);
      
            lblApplicationID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.ReleaseApplicationID.ToString();
            
            MessageBox.Show("The detained license has been released successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            frmReleaseDetainedLicense_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lnklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowDriverLicenseInfo(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID);

            frm.ShowDialog();

            frmReleaseDetainedLicense_Load(null, null);
        }

        private void lnklblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowDriverLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);

            frm.ShowDialog();

            frmReleaseDetainedLicense_Load(null, null);
        }
    }
}
