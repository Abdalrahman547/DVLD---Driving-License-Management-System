using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.Applications.Release_Detained_License;
using DVLD___Driving_License_Management.Global_Classes;
using DVLD___Driving_License_Management.Licenses.Local_Licenses;
using DVLD___Driving_License_Management.Licenses.Renew_Local_Driving_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// International

namespace DVLD___Driving_License_Management.Applications.International_Driving_License
{    public partial class frmIssueInternationalLicense : Form
     {
        
        private int _SelectedLicenseID = -1;
        public frmIssueInternationalLicense()
        {
            InitializeComponent();
        }

        public frmIssueInternationalLicense(int LicnseID)
        {
            InitializeComponent();
            _SelectedLicenseID = LicnseID;

            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_SelectedLicenseID);
            btnIssueLicense.Enabled = true;
            lnklblShowLicenseInfo.Enabled = false;

        }

        private void _ResetApplicationInfo()
        {
            lblInternationalAppID.Text = "[????]";
            lblApplicationDate.Text = "[??/??/????]";
            lblInternationalLID.Text = "[????]";
            lblLocalLID.Text = "[????]";
            
            lblIssueDate.Text = DateTime.Now.ToString("d");
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToString("d");
            lblCreatedBy.Text = clsGlobal.LoggedInUser.UserName;
            lblFees.Text = "[????]";

            btnIssueLicense.Enabled = false;
            lnklblShowLicenseInfo.Enabled = false;
            lnklblShowLicensesHistory.Enabled = false;
        }

        private void _FillApplicationInfo()
        {
            // 1- Check is International License already issued
            if (clsInternationalLicense.IsActiveInternationalLicenseExists(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID))
            {
                MessageBox.Show("Person already have an Active International License.", "International License Already Issued", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctrlDriverLicenseInfoWithFilter1.ClearFilter();
                ctrlDriverLicenseInfoWithFilter1.FilterFoucus();
                _ResetApplicationInfo();
                return;
            }

            // 2- Check Class 3 license
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClass != 3) // 3 => Class 3 LicenseClassID
            {
                MessageBox.Show("International Driving License can only be issued to Class 3 License holders.", "Invalid License Class", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctrlDriverLicenseInfoWithFilter1.ClearFilter();
                ctrlDriverLicenseInfoWithFilter1.FilterFoucus();
                _ResetApplicationInfo();
                return;
            }

            // 3- Check Is Local License Detained
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("International Driving License cannot be issued to Detained Local License holders.", "Detained Local License", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                _ResetApplicationInfo();

                // Relsease Detained License
                if (MessageBox.Show("Do you want to open the Release Detained License application now?", "Release Detained License", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Form frm = new frmReleaseDetainedLicense(_SelectedLicenseID);

                    frm.ShowDialog();
                }

                return;
            }


            // 4- Check is Local License Expired
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show("your Local License is Expired, Please Renew it first.", "Expired Local License", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _ResetApplicationInfo();

                // Renew License
                if(MessageBox.Show("Do you want to open the Renew License application now?", "Renew License", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // To do: Make an Overload Constructor in frmRenewLDLicenseApplication to accept LicenseID
                    // Form frm = new frmRenewLDLicenseApplication(_SelectedLicenseID);
                    
                    Form frm = new frmRenewLDLicenseApplication();
                    
                    frm.ShowDialog();
                }

                return;
            }

            lblApplicationDate.Text = DateTime.Now.ToString("d");
            lblLocalLID.Text = _SelectedLicenseID.ToString();

            lblCreatedBy.Text = clsGlobal.LoggedInUser.UserName;

            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees.ToString("N2");
        }
        
        private void frmIssueInternationalLicense_Load(object sender, EventArgs e)
        {
            if (_SelectedLicenseID != -1)
            {
                btnIssueLicense.Enabled = true;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                lnklblShowLicenseInfo.Enabled = false;
                lnklblShowLicensesHistory.Enabled = true;
                return;
            }


            _ResetApplicationInfo();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID = obj;

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo == null)
                return;

            btnIssueLicense.Enabled = true;

            lnklblShowLicensesHistory.Enabled = true;

            _FillApplicationInfo();

        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to issue the International Driving License?", "Confirm Issue", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsInternationalLicense NewInternationalLicense = new clsInternationalLicense()
            {
                ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID
                ,
                ApplicationDate = DateTime.Now
                ,
                ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense
                ,
                Status = clsApplication.enApplicationStatus.Completed
                ,
                LastStatusDate = DateTime.Now
                ,
                Fees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees
                ,
                CreatedByUserID = clsGlobal.LoggedInUser.UserID,


                DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID,
                IssuedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID,
                IssueDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddYears(1), // could be inhanced later
                Mode = clsInternationalLicense.enMode.AddNew
            
            };

            //MessageBox.Show("Derived CreatedByUserID = " + NewInternationalLicense.CreatedByUserID);
            //MessageBox.Show("Base CreatedByUserID = " + ((clsApplication)NewInternationalLicense).CreatedByUserID);

            //return;


            if (!NewInternationalLicense.Save())
            {
                MessageBox.Show("An error occurred while issuing the International Driving License. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            lblInternationalAppID.Text = NewInternationalLicense.ApplicationID.ToString();
            lblInternationalLID.Text = NewInternationalLicense.InternationalLicenseID.ToString();
            lblIssueDate.Text = NewInternationalLicense.IssueDate.ToString("d");
            lblExpirationDate.Text = NewInternationalLicense.ExpirationDate.ToString("d");

            lnklblShowLicenseInfo.Enabled = true;
            btnIssueLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.Enabled = false;
            MessageBox.Show("International Driving License issued successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lnklblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowDriverLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);

            frm.ShowDialog();
        }

        private void lnklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowInternationalLicenseInfo(Convert.ToInt16(lblInternationalLID.Text));

            frm.ShowDialog();
        }

    }
}
