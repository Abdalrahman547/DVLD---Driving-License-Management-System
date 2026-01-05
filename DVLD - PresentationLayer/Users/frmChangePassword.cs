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

namespace DVLD___Driving_License_Management.Users
{
    public partial class frmChangePassword : Form
    {
        private int _UserID = -1;
        
        private clsUser _User;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void _ResetValues()
        {
            txtCurrentPass.Text = "";
            txtNewPass.Text = "";
            txtConfirmedPass.Text = "";
            txtCurrentPass.Focus();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetValues();

            _User = clsUser.FindByUserID(_UserID);

            if(_User == null)
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlUserCard1.LoadUserInfo(_UserID);

        }

        private void txtCurrentPass_Validating(object sender, CancelEventArgs e)
        {
            // Do not validate if Form being closed
            if (this.CausesValidation == false)
                return;

            if (txtCurrentPass.Text.Trim() == "")
            {
                errorProvider1.SetError(txtCurrentPass, "Please enter Current Password.");
                e.Cancel = true;
            }
            else if (txtCurrentPass.Text.Trim() != _User.Password)
            {
                errorProvider1.SetError(txtCurrentPass, "Current Password is incorrect.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPass, "");
            }
        }
        
        private void txtNewPass_Validating(object sender, CancelEventArgs e)
        {
            if(txtNewPass.Text.Trim() == "")
            {
                errorProvider1.SetError(txtNewPass, "Please enter New Password.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtNewPass, "");
            }
        }

        private void txtConfirmedPass_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmedPass.Text.Trim() == "")
            {
                errorProvider1.SetError(txtConfirmedPass, "Please enter Confirmed Password.");
                e.Cancel = true;
            }
            else if (txtConfirmedPass.Text.Trim() != txtNewPass.Text.Trim())
            {
                errorProvider1.SetError(txtConfirmedPass, "Confirmed Password does not match New Password.");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.CausesValidation = false;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fileds are not valide!.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (clsUser.ChangePassword(_UserID, txtNewPass.Text.Trim()))
            {
                MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Could not change the password.\a", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void frmChangePassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.CausesValidation = false;
        }

    }
}
