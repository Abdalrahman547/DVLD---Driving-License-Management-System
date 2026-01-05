using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.Licenses.Local_Licenses;
using DVLD___Driving_License_Management.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management.Applications.International_Driving_License
{
    public partial class frmListInternationalLicensesApplications : Form
    {

        private static DataTable _dtInternationalLicenses = clsInternationalLicense.GetAllInternationalLicenses();

        private DataTable _dtInternationalLicensesView = _dtInternationalLicenses.DefaultView.ToTable(
            false, "InternationalLicenseID", "ApplicationID", "DriverID",
            "IssuedUsingLocalLicenseID", "IssueDate", "ExpirationDate", "IsActive");
        public frmListInternationalLicensesApplications()
        {
            InitializeComponent();
            cbFilter.SelectedIndex = 0;
            txtSearchBox.Visible = false;
            cbIsActive.Visible = false;
        }
        private void _RefreshList()
        {
            _dtInternationalLicenses = clsInternationalLicense.GetAllInternationalLicenses();

            _dtInternationalLicensesView = _dtInternationalLicenses.DefaultView.ToTable(
            false, "InternationalLicenseID", "ApplicationID", "DriverID",
            "IssuedUsingLocalLicenseID", "IssueDate", "ExpirationDate", "IsActive");

            dgvInternationalLicenses.DataSource = _dtInternationalLicensesView;

            lblNumOfRecords.Text = _dtInternationalLicenses.Rows.Count.ToString();

        }

        private void frmListInternationalLicenses_Load(object sender, EventArgs e)
        {
            _RefreshList();

            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Inter License ID";
                dgvInternationalLicenses.Columns[0].Width = 125;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 125;

                dgvInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[2].Width = 125;

                dgvInternationalLicenses.Columns[3].HeaderText = "Local License ID";
                dgvInternationalLicenses.Columns[3].Width = 125;

                dgvInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].Width = 125;

                dgvInternationalLicenses.Columns[5].HeaderText = "Expiry Date";
                dgvInternationalLicenses.Columns[5].Width = 125;

                dgvInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[6].Width = 120;
            }

        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            Form frm = new frmIssueInternationalLicense();

            frm.ShowDialog();

            _RefreshList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilter.SelectedItem)
            {
                case "International License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;

                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;

                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Local License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;

                case "Is Active":
                    FilterColumn = "IsActive";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (FilterColumn == "None" || txtSearchBox.Text.Trim() == "")
            {
                _RefreshList();

                return;
            }


            if (FilterColumn != "IsActive")
            {
                cbIsActive.Visible = false;
                txtSearchBox.Visible = true;

                _dtInternationalLicensesView.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, txtSearchBox.Text.Trim());
            }
        }

        private void txtSearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _RefreshList();

            if (cbFilter.SelectedItem.ToString() == "None")
            {
                txtSearchBox.Visible = false;
                cbIsActive.Visible = false;
            }
            else if (cbFilter.SelectedItem.ToString() == "Is Active")
            {
                txtSearchBox.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
            }
            else
            {
                cbIsActive.Visible = false;
                txtSearchBox.Visible = true;
                txtSearchBox.Text = "";
                txtSearchBox.Focus();
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            _RefreshList();

            bool isActive = cbIsActive.SelectedItem.ToString() == "Active" ? true : false;

            _dtInternationalLicensesView.DefaultView.RowFilter = string.Format("IsActive = {0}", isActive);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = Convert.ToInt32(dgvInternationalLicenses.CurrentRow.Cells[2].Value);

            int PersonID = clsDriver.FindByDriverID(DriverID).PersonID;

            Form frm = new frmShowPersonInfo(PersonID);

            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ILID = Convert.ToInt32(dgvInternationalLicenses.CurrentRow.Cells[0].Value);

            Form frm = new frmShowInternationalLicenseInfo(ILID);

            frm.ShowDialog();
        }

        private void showPersonLicensesHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = Convert.ToInt32(dgvInternationalLicenses.CurrentRow.Cells[2].Value);

            int PersonID = clsDriver.FindByDriverID(DriverID).PersonID;

            Form frm = new frmShowDriverLicenseHistory(PersonID);

            frm.ShowDialog();
        }
    }
}
