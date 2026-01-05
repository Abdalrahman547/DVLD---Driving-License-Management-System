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

namespace DVLD___Driving_License_Management.Application.Local_Driving_License
{
    public partial class frmAddEditLocalDrivingLicenseApplication : Form
    {
        private enum enMode { AddNew, Update }

        private enMode _Mode;

        private int _LDLApplicationID = -1;

        private int _SelectedPersonID = -1;

        private clsLocalDrivingLicenseApplication _LDLApplication;
        public frmAddEditLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddEditLocalDrivingLicenseApplication(int LDLApplicationID)
        {
            InitializeComponent();

            _LDLApplicationID = LDLApplicationID;
            _Mode = enMode.Update;
        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable dt = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow row in dt.Rows)
            {
                cbLicenseClassName.Items.Add(row["ClassName"]);
            }
        }

        private void _ResetDefaultValues()
        {
            _FillLicenseClassesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblHeader.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";

                _LDLApplication = new clsLocalDrivingLicenseApplication();
                ctrlPersonCardWithFilter1.FilterFoucus();
                lblDate.Text = DateTime.Now.ToString("d");
                cbLicenseClassName.SelectedIndex = 2;
                //lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewLocalLicense).ApplicationFees.ToString(); // NewLocalDrivingLicense Application TypeID = 1
                lblFees.Text = clsApplicationType.Find(1).ApplicationFees.ToString(); // NewLocalDrivingLicense Application TypeID = 1
                lblCreatedbyUser.Text = clsGlobal.LoggedInUser.UserName;
                tpLoginInfo.Enabled = false;
                btnSave.Enabled = false;

            }
            else
            {
                lblHeader.Text = "Edit Local Driving License Application";
                this.Text = "Edit Local Driving License Application";
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

        }

        private void _LoadData()
        {
            _LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(_LDLApplicationID);

            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_LDLApplication == null)
            {
                MessageBox.Show($"Error loading L.D.L Application: {_LDLApplicationID} information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            ctrlPersonCardWithFilter1.LoadPersonInfo(_LDLApplication.ApplicantPersonID);

            _SelectedPersonID = ctrlPersonCardWithFilter1.PersonID;

            cbLicenseClassName.SelectedIndex = _LDLApplication.LicenseClassID - 1;

            lblLDLApplicationID.Text = _LDLApplicationID.ToString();

            lblDate.Text = _LDLApplication.ApplicationDate.ToString("d");

            lblFees.Text = _LDLApplication.Fees.ToString();

            lblCreatedbyUser.Text = clsUser.FindByUserID(_LDLApplication.CreatedByUserID).UserName;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please select a valid person for the application.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btnSave.Enabled = true;
            tpLoginInfo.Enabled = true;

            tabControl1.SelectedIndex = 1; // Move to Login Info tab
            return;

            /*if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;

                tabControl1.SelectedIndex = 1; // Move to Login Info tab
                return;
            }
            */




        }

        private bool _ValidateData()
        {
            int LicenseClassID = cbLicenseClassName.SelectedIndex + 1;

            int ActiveApplicationID = clsLocalDrivingLicenseApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplication.enApplicationType.NewLocalLicense, LicenseClassID);

            // Check if there is an active application for the same person and license class
            if (ActiveApplicationID != -1)
            {
                MessageBox.Show($"Choose another License Class, The Selected Person Already have an Active Application for the Selected ClassID {LicenseClassID}", "This Person have already Application", MessageBoxButtons.OK, MessageBoxIcon.Error);

                cbLicenseClassName.Focus();

                return false;

            }

            // Check if the person already has an active license for the selected class
            if (clsLicense.IsLicenseExistsByPersonID(_SelectedPersonID, LicenseClassID))
            {
                MessageBox.Show($"Choose another License Class, The Selected Person Already have an Active License for the Selected ClassID {LicenseClassID}", "This Person have already License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClassName.Focus();
                return false;
            }

            // Check Minimum Age Requirment
            int MinimumAllowedYears = clsLicenseClass.Find(LicenseClassID).MinimumAllowedAge;
            DateTime MinAllowAge = DateTime.Now.AddYears(-MinimumAllowedYears);
            DateTime DateOfBirth = clsPerson.Find(_SelectedPersonID).DateOfBirth;

            int PersonAge = DateTime.Now.Year - DateOfBirth.Year;

            if (DateOfBirth > MinAllowAge)
            {
                MessageBox.Show($"The Selected Person Age [{PersonAge}] does not meet the minimum age requirement for the selected license class. Minimum Age: {MinimumAllowedYears} years.", "Age Requirement Not Met", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!_ValidateData())
            {
                return;
            }

            _LDLApplication.ApplicantPersonID = _SelectedPersonID;
            _LDLApplication.ApplicationTypeID = 1; // NewLocalDrivingLicense Application TypeID = 1
            _LDLApplication.ApplicationDate = DateTime.Now;
            _LDLApplication.Status = clsApplication.enApplicationStatus.New;
            _LDLApplication.LastStatusDate = DateTime.Now;
            _LDLApplication.LicenseClassID = cbLicenseClassName.SelectedIndex + 1;
            _LDLApplication.Fees = Convert.ToDouble(lblFees.Text);
            _LDLApplication.CreatedByUserID = clsGlobal.LoggedInUser.UserID;

            if (_LDLApplication.Save())
            {
                MessageBox.Show("L.D.L Application information saved successfully.", "Application Saved Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _Mode = enMode.Update;

                lblLDLApplicationID.Text = _LDLApplication.LDLApplicationID.ToString();

                lblHeader.Text = "Edit Local Driving License Application";
                this.Text = "Edit Local Driving License Application";
            }
            else
            {
                MessageBox.Show("Error saving L.D.L Application information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void frmAddEditLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddEditLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.Focus();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected_1(int obj)
        {
            _SelectedPersonID = obj;
        }

    }
}
