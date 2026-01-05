using DVLD___BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management.ApplicationTypes
{
    public partial class frmUpdateApplicationType : Form
    {
        private int _ApplicationID = -1;

        private clsApplicationType _Application;
        
        public frmUpdateApplicationType(int ApplicationID)
        {
            InitializeComponent();
            _ApplicationID = ApplicationID;
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            _Application  = clsApplicationType.Find(_ApplicationID);

            lblID.Text    = _ApplicationID.ToString();
            txtTitle.Text = _Application.ApplicationTitle;
            txtFees.Text  = _Application.ApplicationFees.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                string Title = txtTitle.Text.Trim();
                double Fees = Convert.ToDouble(txtFees.Text.Trim());

                if (clsApplicationType.UpdateApplication(_ApplicationID, Title, Fees))
                {
                    MessageBox.Show("Application Type updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update Application Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Please correct the errors and try again.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title is required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTitle, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (!decimal.TryParse(txtFees.Text, out decimal fees) || fees < 0)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Please enter a valid non-negative number for fees");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFees, null);
            }
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;

        }
    }
}
