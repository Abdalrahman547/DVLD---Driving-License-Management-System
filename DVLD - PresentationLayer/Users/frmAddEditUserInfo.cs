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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD___Driving_License_Management.Users
{
    public partial class frmAddEditUserInfo : Form
    {
        private enum enMode {AddNew, Update}

        private enMode _Mode;

        private int _UserID = -1;
        private clsUser _User;

        public frmAddEditUserInfo()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;

        }

        public frmAddEditUserInfo(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
            _Mode = enMode.Update;

        }

        private void _ResetDefaultValues()
        {
            if(_Mode == enMode.AddNew)
            {
                _User = new clsUser();

                lblHeader.Text = "Add New User";
                this.Text = "Add New User";

                tpLoginInfo.Enabled = false;

                ctrlPersonCardWithFilter1.FilterFoucus();
            }
            else
            {
                lblHeader.Text = "Edit User Information";
                this.Text = "Edit User Information";
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;
            chkIsAdmin.Checked = false;

        }
        
        private void _LoadData()
        {
           
            _User = clsUser.FindByUserID(_UserID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_User == null)
            {   
                MessageBox.Show($"Error loading User: {_UserID} information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);

            lblUserID.Text = _UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = txtPassword.Text;

            chkIsActive.Checked = _User.IsActive;
            chkIsAdmin.Checked = _User.IsAdmin;
           
        }

        private void frmAddEditUserInfo_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }
        
        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;

                tabControl1.SelectedIndex = 1; // Move to Login Info tab
                return;
            }


            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                if (clsUser.IsUserExistsByPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("This User exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFoucus();
                }
                else
                {
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tabControl1.SelectedIndex = 1;
                }
            }
            else
            {
                MessageBox.Show("Please select a person first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_FinalValition())
            {
                _User.UserName = txtUserName.Text.Trim();
                _User.Password = txtPassword.Text.Trim();
                _User.IsActive = chkIsActive.Checked;
                _User.IsAdmin = chkIsAdmin.Checked;

                if (_Mode == enMode.AddNew)
                {
                    _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
                }

                if (_User.Save())
                {
                    MessageBox.Show("User information saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _Mode = enMode.Update;

                    lblUserID.Text = _User.UserID.ToString();

                    lblHeader.Text = "Edit User Information";
                    this.Text = "Edit User Information";

                }
                else
                {
                    MessageBox.Show("Error saving User information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ValidateRequiredField(System.Windows.Forms.TextBox txtBox)
        {
            if (string.IsNullOrWhiteSpace(txtBox.Text))
            {
                errorProvider1.SetError(txtBox, "This field is required.");
                txtBox.Focus();
            }
            else
            {
                errorProvider1.SetError(txtBox, "");
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            ValidateRequiredField((System.Windows.Forms.TextBox)sender);
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            ValidateRequiredField((System.Windows.Forms.TextBox)sender);
        }

        private void txtConfirmPassword_Leave(object sender, EventArgs e)
        {
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Confirmation Failed, Reconfirm it");
                //txtConfirmPassword.Focus();
            }
            else
                errorProvider1.SetError(txtConfirmPassword, "");
        }

        private bool _FinalValition()
        {

            if (txtUserName.Text == string.Empty)
            {
                MessageBox.Show("Password is Requiered", "ReEnter it", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return false;
            }

            if(txtPassword.Text == string.Empty)
            {
                MessageBox.Show("Password is Requiered", "ReEnter it", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return false;
            }

            if (txtConfirmPassword.Text == string.Empty || txtConfirmPassword.Text != txtPassword.Text)
            {
                MessageBox.Show("Confirm Password Again", "Confirmation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtConfirmPassword.Text = "";
                txtPassword.Focus();
                return false;
            }

            return true;
        }

    }
}
