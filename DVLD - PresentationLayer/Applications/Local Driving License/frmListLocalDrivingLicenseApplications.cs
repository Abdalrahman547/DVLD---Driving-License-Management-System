using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.Application.Local_Driving_License;
using DVLD___Driving_License_Management.Test;
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

namespace DVLD___Driving_License_Management.LocalDrivingLicenseApplications
{
    public partial class frmListLocalDrivingLicenseApplications : Form
    {
        private DataTable _dtLocalApplications = clsLocalDrivingLicenseApplication.GetAllLDLApplications();
        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
            cbFilter.SelectedIndex = 0;
        }

        private void _RefreshList()
        {
            _dtLocalApplications = clsLocalDrivingLicenseApplication.GetAllLDLApplications();
            dgvLocalApplications.DataSource = _dtLocalApplications;
            lblNumOfRecords.Text = dgvLocalApplications.Rows.Count.ToString();
        }

        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _RefreshList();

            if(dgvLocalApplications.Rows.Count > 0)
            {
                dgvLocalApplications.Columns[0].HeaderText = "L.D.L App ID";
                dgvLocalApplications.Columns[0].Width = 100;

                dgvLocalApplications.Columns[1].HeaderText = "Class Name";
                dgvLocalApplications.Columns[1].Width = 200;

                dgvLocalApplications.Columns[2].HeaderText = "National No.";
                dgvLocalApplications.Columns[2].Width = 100;

                dgvLocalApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalApplications.Columns[3].Width = 220;

                dgvLocalApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalApplications.Columns[4].Width = 120;
                
                dgvLocalApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLocalApplications.Columns[5].Width = 100;
                
                dgvLocalApplications.Columns[6].HeaderText = "Status";
                dgvLocalApplications.Columns[6].Width = 100;
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingLicenseApplication frm = new frmAddEditLocalDrivingLicenseApplication();

            frm.ShowDialog();

            _RefreshList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilter.SelectedItem)
            {
            
                case "L.D.L.App ID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case "National No.":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Status":
                    FilterColumn = "Status";
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


            if (FilterColumn == "LocalDrivingLicenseApplicationID" )
                _dtLocalApplications.DefaultView.RowFilter = string.Format("{0} = {1}", FilterColumn, txtSearchBox.Text.Trim());
            else
                _dtLocalApplications.DefaultView.RowFilter = string.Format("{0} LIKE '%{1}%'", FilterColumn, txtSearchBox.Text.Trim());

            //dgvLocalApplications.DataSource = _dtLocalApplications;
            lblNumOfRecords.Text = dgvLocalApplications.Rows.Count.ToString();


        }

        private void cbActiveAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbStatus.SelectedItem.ToString() == "All")
            {
                _RefreshList();
                return;
            }

            _dtLocalApplications.DefaultView.RowFilter = string.Format("Status Like '{0}'", cbStatus.SelectedItem.ToString());

            lblNumOfRecords.Text = dgvLocalApplications.Rows.Count.ToString();

        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _RefreshList();

            txtSearchBox.Text = "";
            
            txtSearchBox.Visible = cbFilter.SelectedItem.ToString() != "None";
            
            if(txtSearchBox.Visible)
                txtSearchBox.Focus();
            
            cbStatus.Visible = false;

            if (cbFilter.SelectedItem.ToString() == "Status")
            {
                txtSearchBox.Visible = false;
                cbStatus.Visible = true;
                cbStatus.SelectedIndex = 0;
                cbStatus.Focus();
            }
        }

        private void txtSearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "L.D.L.App ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);

            Form frm = new frmShowLocalDrivingLicenseApplicationInfo(LDLApplicationID);

            frm.ShowDialog();

        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);

            Form frm = new frmAddEditLocalDrivingLicenseApplication(LDLApplicationID);

            frm.ShowDialog();

            _RefreshList();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Implementation of deleting application is disabled Currently.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            /*int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);


            if (MessageBox.Show("Are you sure you want to delete this application?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID);

                
                if(localDrivingLicenseApplication != null) 
                {
                    if (localDrivingLicenseApplication.Delete())
                    {
                        MessageBox.Show("Application not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Error occurred while deleting the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Application deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _RefreshList();
            }*/
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);

            if (MessageBox.Show("Are you sure you want to Cancel this application?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                clsLocalDrivingLicenseApplication.Cancel(LDLApplicationID);

                MessageBox.Show("Application Cancelled successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _RefreshList();
            }
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Implementation of scheduling Vision Test is disabled Currently.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);

            Form frm = new frmListTestAppointments(LDLApplicationID, clsTestType.enTestType.VisionTest);

            frm.ShowDialog();

            _RefreshList();
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Implementation of scheduling Written Test is disabled Currently.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);

            Form frm = new frmListTestAppointments(LDLApplicationID, clsTestType.enTestType.WrittenTest);

            frm.ShowDialog();
            _RefreshList();
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Implementation of scheduling Street Test is disabled Currently.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);

            Form frm = new frmListTestAppointments(LDLApplicationID, clsTestType.enTestType.StreetTest);

            frm.ShowDialog();

            _RefreshList();

        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Implementation of Issuing Driving License is disabled Currently.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);

            Form frm = new frmIssueLicenseForFirstTime(LDLApplicationID);

            frm.ShowDialog();

            _RefreshList();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Implementation of Showing Driving License is disabled Currently.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);

            int LicenseID = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID).GetActiveLicenseID();
            
            if(LicenseID == -1)
            {
                MessageBox.Show("No Active License Found for this Application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowDriverLicenseInfo(LicenseID);

            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);

            int PersonID = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID).ApplicantPersonID;

            Form frm = new frmShowDriverLicenseHistory(PersonID);

            frm.ShowDialog();
        }

        // TODO: Implement Some Logic to Enable/Disable some menu items based on the Application Status
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            int LDLApplicationID = Convert.ToInt32(dgvLocalApplications.CurrentRow.Cells[0].Value);

            clsLocalDrivingLicenseApplication LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID);

            // Enable/Disable (Edit, Delete, Cancel, Schedule Tests) if Status = New
            editApplicationToolStripMenuItem.Enabled = (LDLApplication.Status == clsApplication.enApplicationStatus.New);
            deleteApplicationToolStripMenuItem.Enabled = (LDLApplication.Status == clsApplication.enApplicationStatus.New);
            cancelApplicationToolStripMenuItem.Enabled = (LDLApplication.Status == clsApplication.enApplicationStatus.New);
            schedualTestToolStripMenuItem.Enabled = (LDLApplication.Status == clsApplication.enApplicationStatus.New);

            // Enable/Disable (Issue License - Schedule Tests) if Status = New && All Tests Passed
            bool PassedAllTests = clsTest.IsPassedAllTests(LDLApplicationID);

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled =
                (LDLApplication.Status == clsApplication.enApplicationStatus.New && PassedAllTests);


            if (PassedAllTests)
                schedualTestToolStripMenuItem.Enabled = false;

            // Enable/Disable (Show License, Show Person License History) if License Issued
            showLicenseToolStripMenuItem.Enabled =
                (LDLApplication.GetActiveLicenseID() != -1);

            showPersonLicenseHistoryToolStripMenuItem.Enabled = showLicenseToolStripMenuItem.Enabled;
            // Check Order of Tests 
            scheduleVisionTestToolStripMenuItem.Enabled = true;
            scheduleWrittenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = false;

            // 1- Passed vision test
            if (LDLApplication.DoesPassedTestType((int)clsTestType.enTestType.VisionTest))
            {
                scheduleVisionTestToolStripMenuItem.Enabled = false;

                scheduleWrittenTestToolStripMenuItem.Enabled = true;
            }

            // 2- Passed Written Test
            if (LDLApplication.DoesPassedTestType((int)clsTestType.enTestType.WrittenTest))
            {
                scheduleWrittenTestToolStripMenuItem.Enabled = false;

                scheduleStreetTestToolStripMenuItem.Enabled = true;
            }

            // 5- passed Street Test
            if (LDLApplication.DoesPassedTestType((int)clsTestType.enTestType.StreetTest))
            {
                scheduleStreetTestToolStripMenuItem.Enabled = false;
            }

        }

    }
}
