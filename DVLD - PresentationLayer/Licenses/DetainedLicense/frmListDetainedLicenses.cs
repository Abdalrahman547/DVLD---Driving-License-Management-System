using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.Applications.Release_Detained_License;
using DVLD___Driving_License_Management.People;
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
    public partial class frmListDetainedLicenses : Form
    {
        private DataTable _dtDetainedLicenses;
        public frmListDetainedLicenses()
        {
            InitializeComponent();
            cbFilter.SelectedIndex = 0;
            txtSearchBox.Visible = false;
            cbIsReleased.Visible = false;
        }

        private void _RefreshList()
        {
            _dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();
            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;
            lblNumOfRecords.Text = _dtDetainedLicenses.Rows.Count.ToString();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _RefreshList();

            if (dgvDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicenses.Columns[0].Width = 70;

                dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenses.Columns[1].Width = 70;

                dgvDetainedLicenses.Columns[2].HeaderText = "Detain Date";
                dgvDetainedLicenses.Columns[2].Width = 125;

                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 120;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 80;

                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 125;

                dgvDetainedLicenses.Columns[6].HeaderText = "National No.";
                dgvDetainedLicenses.Columns[6].Width = 150;

                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[7].Width = 270;

                dgvDetainedLicenses.Columns[8].HeaderText = "Release App.ID";
                dgvDetainedLicenses.Columns[8].Width = 110;
            }
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            Form frm = new frmDetainedLicense();

            frm.ShowDialog();

            _RefreshList();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetainedLicense();

            frm.ShowDialog();

            _RefreshList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((string)cbFilter.SelectedItem == "Detain ID" || 
                (string)cbFilter.SelectedItem == "License ID"|| 
                (string)cbFilter.SelectedItem == "Release Application ID")
            {
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
            }
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string NationalNo = dgvDetainedLicenses.CurrentRow.Cells[6].Value.ToString();
            
            Form frm = new frmShowPersonInfo(NationalNo);

            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = Convert.ToInt32(dgvDetainedLicenses.CurrentRow.Cells[1].Value);

            Form frm = new frmShowDriverLicenseInfo(LicenseID);

            frm.ShowDialog();
        }

        private void showPersonLicensesHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string NationalNo = dgvDetainedLicenses.CurrentRow.Cells[6].Value.ToString();

            Form frm = new frmShowDriverLicenseHistory(NationalNo);

            frm.ShowDialog();
        }

        private void releaseDetainedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = Convert.ToInt32(dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            
            Form frm = new frmReleaseDetainedLicense(LicenseID);

            frm.ShowDialog();

            _RefreshList();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            bool isReleased = Convert.ToBoolean(dgvDetainedLicenses.CurrentRow.Cells[3].Value);
            
            releaseDetainedToolStripMenuItem.Enabled = !isReleased;
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _RefreshList();

            if(cbFilter.SelectedItem.ToString() == "None")
                {
                txtSearchBox.Visible = false;
                cbIsReleased.Visible = false;
            }
            else if(cbFilter.SelectedItem.ToString() == "Is Released")
            {
                txtSearchBox.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.SelectedIndex = 0;
            }
            else
            {
                cbIsReleased.Visible = false;
                txtSearchBox.Visible = true;
                txtSearchBox.Text = "";
                txtSearchBox.Focus();
            }

        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilter.SelectedItem)
            {
                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;

                case "License ID":
                    FilterColumn = "LicenseID";
                    break;

                case "Is Released":
                    FilterColumn = "IsReleased";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Release App.ID":
                    FilterColumn = "ReleaseApplicationID";
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


            if (FilterColumn != "IsReleased")
            {
                cbIsReleased.Visible = false;
                txtSearchBox.Visible = true;
                

                if (FilterColumn == "DetainID" || FilterColumn == "LicenseID" || FilterColumn == "ReleaseApplicationID")
                {
                    _dtDetainedLicenses.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, txtSearchBox.Text.Trim());
                }
                else
                {
                    _dtDetainedLicenses.DefaultView.RowFilter = string.Format("{0} LIKE '%{1}%'", FilterColumn, txtSearchBox.Text.Trim());
                }
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            _dtDetainedLicenses.DefaultView.RowFilter = string.Format("{0} = {1}", "IsReleased", cbIsReleased.SelectedItem.ToString() == "Released" ? "True" : "False");

        }
    }
}
