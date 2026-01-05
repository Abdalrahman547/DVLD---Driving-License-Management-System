using DVLD___Driving_License_Management.Application;
using DVLD___Driving_License_Management.Application.Local_Driving_License;
using DVLD___Driving_License_Management.Applications.Replace_Lost_Or_Damaged_License;
using DVLD___Driving_License_Management.Applications.International_Driving_License;
using DVLD___Driving_License_Management.ApplicationTypes;
using DVLD___Driving_License_Management.Global_Classes;
using DVLD___Driving_License_Management.Licenses.Renew_Local_Driving_License;
using DVLD___Driving_License_Management.LocalDrivingLicenseApplications;
using DVLD___Driving_License_Management.TestTypes;
using DVLD___Driving_License_Management.Users;
using DVLD___Driving_License_Management.Tests;
using DVLD___Driving_License_Management.Licenses.DetainedLicense;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD___Driving_License_Management.Applications.Release_Detained_License;
using DVLD___Driving_License_Management.Drivers;

namespace DVLD___Driving_License_Management
{
    public partial class frmMain : Form
    {
        private frmLogin _frmLogin;
        
        public frmMain(frmLogin frmLogin)
        {
            InitializeComponent();

            _frmLogin = frmLogin;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListPeople();
            
            frm.ShowDialog();
        }

        private void currentUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowUserInfo(clsGlobal.LoggedInUser.UserID);

            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmChangePassword(clsGlobal.LoggedInUser.UserID);

            frm.ShowDialog();
        }

        private void signToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.LoggedInUser = null;
            this.Close();

            _frmLogin.Show();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListUsers();

            frm.ShowDialog();
        }

        private void manageApToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListApplicationTypes();

            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListTestTypes();

            frm.ShowDialog();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _frmLogin.Close();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditLocalDrivingLicenseApplication();

            frm.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListLocalDrivingLicenseApplications();

            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListDrivers();

            frm.ShowDialog();
        }

        private void internationalToolStripMenuItem_Click(object sender, EventArgs e)
        {     
            Form frm = new frmListInternationalLicensesApplications();

            frm.ShowDialog();
        }

        private void renewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmRenewLDLicenseApplication();

            frm.ShowDialog();
        }

        private void replaceForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReplaceLostOrDamagedLicense();

            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmDetainedLicense();

            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmReleaseDetainedLicense();

            form.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListDetainedLicenses();

            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm  = new frmIssueInternationalLicense();

            frm.ShowDialog();
        }

        private void releseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetainedLicense();

            frm.ShowDialog();
        }

    }
}
