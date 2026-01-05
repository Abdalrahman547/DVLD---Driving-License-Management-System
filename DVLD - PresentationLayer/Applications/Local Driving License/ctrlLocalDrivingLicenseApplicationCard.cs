using DVLD___BussinessLayer;
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

namespace DVLD___Driving_License_Management.Application.Local_Driving_License
{
    public partial class ctrlLocalDrivingLicenseApplicationCard : UserControl
    {
        private int _LDLApplicationID = -1;

        private clsLocalDrivingLicenseApplication _LDLApplication;

        private int _LicenseID = -1;
        public int LDLApplicationID { get { return _LDLApplicationID; } }
        public clsLocalDrivingLicenseApplication LDLApplication { get { return _LDLApplication; } }

        public ctrlLocalDrivingLicenseApplicationCard()
        {
            InitializeComponent();
        }

        private void _FillApplicationInfo()
        {

            _LDLApplicationID = _LDLApplication.LDLApplicationID;

            lnklblViewLicenseInfo.Enabled = (_LicenseID > 0);

            lblLDLAppID.Text = _LDLApplicationID.ToString();
            lblLicenseClassName.Text = clsLicenseClass.Find(LDLApplication.LicenseClassID).ClassName;
            ctrlApplicationCard1.LoadApplicationInfo(_LDLApplication.ApplicationID);

            lblPassedTests.Text = clsLocalDrivingLicenseApplication.GetNumOfPassedTests(LDLApplicationID).ToString() + "/3";

            _LicenseID = _LDLApplication.GetActiveLicenseID();


        }

        private void ResetApplicationInfo()
        {
            _LDLApplicationID        = -1;
            _LDLApplication          = null;
            lblLDLAppID.Text         = "N/A";
            lblLicenseClassName.Text = "[????]";
            lblPassedTests.Text      = "[????]";

        }

        private void _LoadData()
        {
            if(_LDLApplication == null)
            {
                MessageBox.Show("L.D.L Application not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetApplicationInfo();
                return;
            }


            _FillApplicationInfo();
        }

        public void LoadLDLApplicationInfoByLDLApplicationID(int LDLApplicationID)
        {
            _LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID);

            _LoadData();

        }

        public void LoadLDLApplicationInfoByApplicationID(int ApplicationID)
        {
            _LDLApplication = clsLocalDrivingLicenseApplication.FindByApplicationID(ApplicationID);

            _LoadData();

        }
        
        private void ctrlLocalDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            if (!DesignMode && _LDLApplicationID > 0)
            {
                LoadLDLApplicationInfoByLDLApplicationID(_LDLApplicationID);
            }
        }

        private void lnklblViewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //MessageBox.Show($"Active License ID: {_LicenseID}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            Form frm = new frmShowDriverLicenseInfo(_LicenseID);

            frm.ShowDialog();
        }

        private void lnklblVeiwPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowPersonInfo(_LDLApplication.ApplicantPersonID);

            frm.ShowDialog();
        }

        public void ShowLicenseInfo()
        {
            lnklblViewLicenseInfo.Enabled = true;
        }

    }
}
