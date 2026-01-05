using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management
{
    public partial class frmLogin : Form
    {
        private clsUser _User;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            // Do not validate when form being closed
            if (this.CausesValidation == false)
                return;


            if (txtUserName.Text == "")
            {
                errorProvider1.SetError(txtUserName, "Please enter Username.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtUserName, "");
                e.Cancel = false;
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            // Do not validate when form being closed
            if (this.CausesValidation == false)
                return;

            if (txtPassword.Text == "")
            {
                errorProvider1.SetError(txtPassword, "Please enter Password.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
                e.Cancel = false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Reqired.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            string UserName = txtUserName.Text.Trim();
            string Password = txtPassword.Text.Trim();

            _User = clsUser.FindByUserNameAndPassword(UserName, Password);

            if (_User == null)
            {
                txtUserName.Focus();
               
                MessageBox.Show("Invalid Username Or Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!_User.IsActive)
            {
                txtUserName.Focus();

                MessageBox.Show("Your account is not Active, Contact Admin.\a", "InActive Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(chkRememberMe.Checked)
            {
                Properties.Settings.Default.lastUserName = txtUserName.Text.Trim();
                Properties.Settings.Default.LastPassword = txtPassword.Text.Trim();
                Properties.Settings.Default.RememberMe = true;
            }
            else
            {
                Properties.Settings.Default.lastUserName = "";
                Properties.Settings.Default.LastPassword = "";
                Properties.Settings.Default.RememberMe = false;
            }

            // Save settings
            Properties.Settings.Default.Save();


            clsGlobal.LoggedInUser = _User;

            Form frm = new frmMain(this);

            this.Hide();

            frm.ShowDialog();

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUserName.Text = Properties.Settings.Default.lastUserName;
            txtPassword.Text = Properties.Settings.Default.LastPassword;
            chkRememberMe.Checked = Properties.Settings.Default.RememberMe;
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // To avoid Validating event to be fired when form is being closed
            this.CausesValidation = false;
        }


    }
}
