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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD___Driving_License_Management.TestTypes
{
    public partial class frmUpdateTestType : Form
    {
        private int _TestID = -1;

        private clsTestType _Test;
        public frmUpdateTestType(int TestID)
        {
            InitializeComponent();
            _TestID = TestID;

        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            _Test = clsTestType.Find(_TestID);

            lblID.Text = _TestID.ToString();
            txtTitle.Text = _Test.TestTypeTitle;
            txtDescription.Text = _Test.TestTypeDescription;
            txtFees.Text = _Test.TestTypeFees.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                string Title = txtTitle.Text.Trim();
                string Description = txtDescription.Text.Trim();
                double Fees = Convert.ToDouble(txtFees.Text.Trim());

                if (clsTestType.UpdateTest(_TestID, Title, Description, Fees))
                {
                    MessageBox.Show("Test Type updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update Test Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Please correct the errors and try again.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
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

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Description is required");
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

    
    }
}
