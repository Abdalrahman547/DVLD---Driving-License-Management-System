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

namespace DVLD___Driving_License_Management.Users
{
    public partial class frmListUsers : Form
    {

        private static DataTable _dtAllUsers = clsUser.GetAllUsers();

        public frmListUsers()
        {
            InitializeComponent();

            cbFilter.SelectedIndex = 0;
        }

        private void _RefreshUsersList()
        {
            _dtAllUsers = clsUser.GetAllUsers();

            dgvUsers.DataSource = _dtAllUsers;

            lblNumOfRecords.Text = dgvUsers.Rows.Count.ToString();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _RefreshUsersList();

            if (dgvUsers.Rows.Count > 0)
            {   
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].Width = 80;

                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].Width = 80;

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].Width = 250;

                dgvUsers.Columns[3].HeaderText = "User Name";
                dgvUsers.Columns[3].Width = 110;

                dgvUsers.Columns[4].HeaderText = "Is Active";
                dgvUsers.Columns[5].HeaderText = "Is Admin";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch(cbFilter.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Is Active":
                    FilterColumn = "IsActive";
                    break;

                case "Is Admin":
                    FilterColumn = "IsAdmin";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if(FilterColumn == "None" || txtSearchBox.Text.Trim() == "")
            {
                _RefreshUsersList();
                return;
            }
           
            if(cbFilter.Text == "User ID" || cbFilter.Text == "Person ID")
                _dtAllUsers.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, txtSearchBox.Text.Trim());
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("{0} LIKE '%{1}%'", FilterColumn, txtSearchBox.Text.Trim());
                    
            lblNumOfRecords.Text = dgvUsers.Rows.Count.ToString();
            
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            _RefreshUsersList();

            txtSearchBox.Text = "";

            txtSearchBox.Visible = (cbFilter.Text != "None");

            if (txtSearchBox.Visible)
                txtSearchBox.Focus();

            cbActiveAdmin.Visible = false;
            

            if(cbFilter.Text == "Is Active" || cbFilter.Text == "Is Admin")
            {
                txtSearchBox.Visible = false;
                cbActiveAdmin.Visible = true;
                cbActiveAdmin.Focus();
                cbActiveAdmin.SelectedIndex = 0;
            }
        }

        private void cbActiveAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilter.Text == "Is Active")
            {
                if(cbActiveAdmin.Text == "All")
                {
                    _RefreshUsersList();
                    return;
                }

                if( cbActiveAdmin.Text == "Yes")
                    _dtAllUsers.DefaultView.RowFilter = string.Format("IsActive = 1");
                else
                    _dtAllUsers.DefaultView.RowFilter = string.Format("IsActive = 0");

            }
            else if(cbFilter.Text == "Is Admin")
            {
                if(cbActiveAdmin.Text == "All")
                {
                    _RefreshUsersList();
                    return;
                }

                if (cbActiveAdmin.Text == "Yes")
                    _dtAllUsers.DefaultView.RowFilter = string.Format("IsAdmin = 1");
                else
                    _dtAllUsers.DefaultView.RowFilter = string.Format("IsAdmin = 0");
            }

            lblNumOfRecords.Text = dgvUsers.Rows.Count.ToString();
        }

        private void txtSearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "User ID" || cbFilter.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = Convert.ToInt32(dgvUsers.CurrentRow.Cells[0].Value);

            frmShowUserInfo frm = new frmShowUserInfo(UserID);

            frm.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsGlobal.LoggedInUser.IsAdmin == false)
            {
                MessageBox.Show("Only Admin Users Can Delete Users.\a", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            int UserID = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);

            string UserFullName = dgvUsers.CurrentRow.Cells["FullName"].Value.ToString();

            if (MessageBox.Show($"Are you sure you want to delete [ {UserFullName} ]?",
                                "Confirm Delete",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) == DialogResult.OK)
            {

                if (clsUser.DeleteUser(UserID))
                {
                    MessageBox.Show("User Deleted Successfully");

                    frmListUsers_Load(null, null); // Refresh
                }

                else
                    MessageBox.Show("Error: Could not delete the User.\a");

            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Comming Soon :)\a");
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Comming Soon :)\a");
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsGlobal.LoggedInUser.IsAdmin == false)
            {
                MessageBox.Show("Only Admin Users Can Edit Users Information.\a", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int UserID = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);

            Form frm = new frmAddEditUserInfo(UserID);
            
            frm.ShowDialog();

            frmListUsers_Load(null, null); // Refresh
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            if(clsGlobal.LoggedInUser.IsAdmin == false)
            {
                MessageBox.Show("Only Admin Users Can Add New Users.\a", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmAddEditUserInfo();

            frm.ShowDialog();

            frmListUsers_Load(null, null); // Refresh
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);

            Form frm = new frmChangePassword(UserID);
            
            frm.ShowDialog();

        }

    }
}
