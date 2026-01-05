using System;
using DVLD___BussinessLayer;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD___Driving_License_Management.Application.Local_Driving_License;
using DVLD___Driving_License_Management.People;
using DVLD___Driving_License_Management.Licenses;
using DVLD___Driving_License_Management.Licenses.Local_Licenses;

namespace DVLD___Driving_License_Management.Drivers
{
    public partial class frmListDrivers : Form
    {
        DataTable _dtDriversList = clsDriver.GetAllDrivers();
        public frmListDrivers()
        {
            InitializeComponent();
            cbFilter.SelectedIndex = 0;
            txtSearchBox.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _RefreshDriversList()
        {
            _dtDriversList = clsDriver.GetAllDrivers();
            dgvDrivers.DataSource = _dtDriversList;
            lblNumOfRecords.Text = dgvDrivers.Rows.Count.ToString();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _RefreshDriversList();

            if (dgvDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 80;
                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 80;
                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[2].Width = 90;
                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 250;
                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 120;
                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].Width = 120;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _RefreshDriversList();

            txtSearchBox.Text = "";

            txtSearchBox.Focus();
        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch(cbFilter.SelectedItem)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                
                case "National No.":
                    FilterColumn = "NationalNo";
                    break;
                
                default:
                    FilterColumn = "None";
                    break;
            }

            if(FilterColumn == "None" || txtSearchBox.Text.Trim() == "")
            {
                _RefreshDriversList();
                return;
            }

            if(FilterColumn == "FullName" || FilterColumn == "NationalNo")
                _dtDriversList.DefaultView.RowFilter = string.Format("{0} LIKE '%{1}%'", FilterColumn, txtSearchBox.Text.Trim());

            else
                _dtDriversList.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, txtSearchBox.Text.Trim());

            lblNumOfRecords.Text = dgvDrivers.Rows.Count.ToString();
        }

        private void txtSearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Driver ID" || cbFilter.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditLocalDrivingLicenseApplication();

            frm.ShowDialog();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = Convert.ToInt32(dgvDrivers.CurrentRow.Cells["PersonID"].Value);

            Form frm = new frmShowPersonInfo(PersonID);

            frm.ShowDialog();
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Todo: Implement this feature

            MessageBox.Show("This feature is not implemented yet.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void zToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = Convert.ToInt32(dgvDrivers.CurrentRow.Cells["PersonID"].Value);

            Form frm = new frmShowDriverLicenseHistory(PersonID);

            frm.ShowDialog();
        }
    }
}
