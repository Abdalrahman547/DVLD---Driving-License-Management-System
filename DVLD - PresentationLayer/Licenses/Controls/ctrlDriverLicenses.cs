using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.Licenses.Local_Licenses;
using DVLD___Driving_License_Management.Applications.International_Driving_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID;
        private clsDriver _Driver;
        private DataTable _dtDriverLocalLicensesHistory;
        private DataTable _dtDriverInternationalLicensesHistory;
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        private void _LoadLocalLicensesInfo()
        {
            _dtDriverLocalLicensesHistory = clsDriver.GetLocalLicenseHistory(_DriverID);
            dgvLocalLicensesHistory.DataSource = _dtDriverLocalLicensesHistory;
            lblLocalRecordsCount.Text = _dtDriverLocalLicensesHistory.Rows.Count.ToString();


            if(dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 110;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 110;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 270;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 170;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiry Date";
                dgvLocalLicensesHistory.Columns[4].Width = 170;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 110;

            }

        }

        private void _LoadInternationalLicensesInfo()
        {
            _dtDriverInternationalLicensesHistory = clsInternationalLicense.GetDriverInternationalLicenses(_DriverID);

            DataTable _dtDriverInternationalLicensesHistory_View = _dtDriverInternationalLicensesHistory.DefaultView.ToTable(
            false, "InternationalLicenseID", "ApplicationID",
            "IssuedUsingLocalLicenseID", "IssueDate", "ExpirationDate", "IsActive");

            dgvInternationalLicensesHistory.DataSource = _dtDriverInternationalLicensesHistory_View;

            lblInternationalRecordsCount.Text = _dtDriverInternationalLicensesHistory.Rows.Count.ToString();

            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Inter License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 150;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 150;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "Local License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 150;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 170;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiry Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 170;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 150;
            }
        }

        public void LoadInfo(int DriverID)
        {
            _DriverID = DriverID;

            _Driver = clsDriver.FindByDriverID(_DriverID);

            if (_Driver == null)
            {
                MessageBox.Show("Driver information could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LoadLocalLicensesInfo();
            _LoadInternationalLicensesInfo();
            
        }

        public void LoadInfoByPersonID(int PersonID)
        {
            _Driver = clsDriver.FindByPersonID(PersonID);

            if (_Driver == null)
            {
                MessageBox.Show("Driver information could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DriverID = _Driver.DriverID;
            _LoadLocalLicensesInfo();
            _LoadInternationalLicensesInfo();
        }

        public void Clear()
        {
            _dtDriverLocalLicensesHistory.Clear();
            _dtDriverInternationalLicensesHistory.Clear();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = Convert.ToInt32(dgvLocalLicensesHistory.CurrentRow.Cells[0].Value);

            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(LicenseID);

            frm.ShowDialog();
        }
        
        private void showLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int ILID = Convert.ToInt32(dgvInternationalLicensesHistory.CurrentRow.Cells[0].Value);

            Form frm = new frmShowInternationalLicenseInfo(ILID);

            frm.ShowDialog();
        }


    }
}
