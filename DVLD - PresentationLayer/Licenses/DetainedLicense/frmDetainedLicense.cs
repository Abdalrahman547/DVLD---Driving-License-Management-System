using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.Global_Classes;
using DVLD___Driving_License_Management.Licenses;
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

namespace DVLD___Driving_License_Management.Licenses.DetainedLicense
{
    public partial class frmDetainedLicense : Form
    {

        private int _DetainedLicenseID = -1;

        public frmDetainedLicense()
        {
            InitializeComponent();

        }

        private void _ResetDetainedLicenseInfo()
        {
            lblLicenseID.Text = "[????]";
            lblDetainedLicenseID.Text = "[????]";
            txtFineFees.Text = string.Empty;
            btnDetain.Enabled = false;
            lnklblShowLicensesHistory.Enabled = false;
            lnklblShowLicenseInfo.Enabled = false;
            txtFineFees.Enabled = true;
           
        }
        
        private void frmDetainedLicense_Load(object sender, EventArgs e) 
        {
            btnDetain.Enabled = false;
            lnklblShowLicenseInfo.Enabled = false;
            lnklblShowLicensesHistory.Enabled = false;
            txtFineFees.Enabled = false;
            lblDetainedDate.Text = DateTime.Now.ToString("d");
            lblCreatedBy.Text = clsGlobal.LoggedInUser.UserName;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
          
            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo == null)
                return;

            _ResetDetainedLicenseInfo();

            _FillDetainedLicenseInfo();     

        }

        private void _FillDetainedLicenseInfo()
        {

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("This license is already detained.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtFineFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.FineFees.ToString("F2");
                lblDetainedLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainID.ToString();

                lblLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID.ToString();

                txtFineFees.Enabled = false;
                frmDetainedLicense_Load(null, null);
                return;
            }


            btnDetain.Enabled = true;
            txtFineFees.Focus();
           
            lnklblShowLicensesHistory.Enabled = true;
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to detain this license?", "Confirm Detain", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            _DetainedLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Detain(Convert.ToDecimal(txtFineFees.Text), clsGlobal.LoggedInUser.UserID);
            
            if (_DetainedLicenseID != -1)
            {
                MessageBox.Show("License detained successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblDetainedLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainID.ToString();
                btnDetain.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                txtFineFees.Enabled = false;
                lnklblShowLicenseInfo.Enabled = true;
                
            }
            else
            {
                MessageBox.Show("Error detaining license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnklblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowDriverLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);

            frm.ShowDialog();

            frmDetainedLicense_Load(null, null);
        }

        private void lnklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowDriverLicenseInfo(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID);
            
            frm.ShowDialog();

            frmDetainedLicense_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFineFees.Text))
            {
                errorProvider1.SetError(txtFineFees, "Fine Fees is required.");
                e.Cancel = true;
            }
            else
            {
                decimal fineFees;
                if (!decimal.TryParse(txtFineFees.Text, out fineFees) || fineFees < 0)
                {
                    errorProvider1.SetError(txtFineFees, "Fine Fees must be a valid non-negative number.");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(txtFineFees, string.Empty);
                }
            }
        }
    }
}
