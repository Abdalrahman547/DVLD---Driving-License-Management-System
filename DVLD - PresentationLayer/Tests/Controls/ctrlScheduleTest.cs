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

namespace DVLD___Driving_License_Management.Tests.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        public enum enCreationMode { FirstTimeSchedule = 0, RetokenTest = 1 };
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;

        
        public clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;

        [Category("Misc")]
        [Description("Set the type of test (Vision, Written, or Street Test)")]
        [Browsable(true)]
        [DefaultValue(clsTestType.enTestType.VisionTest)]
        [RefreshProperties(RefreshProperties.Repaint)]  // Refreshes other properties when this changes
        [EditorBrowsable(EditorBrowsableState.Always)]  // Ensures visibility in IntelliSense
        public clsTestType.enTestType TestType
        {
            get { return _TestType; }
            set
            {
                _TestType = value;

                switch (_TestType)
                {
                    case clsTestType.enTestType.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pbTestTypeImage.Image = Properties.Resources.Vision_512;
                        break;

                    case clsTestType.enTestType.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pbTestTypeImage.Image = Properties.Resources.Written_Test_512;
                        break;

                    case clsTestType.enTestType.StreetTest:
                        gbTestType.Text = "Practical Test";
                        pbTestTypeImage.Image = Properties.Resources.Street_Test_32;
                        break;

                    default:
                        gbTestType.Text = "Vision Test";
                        pbTestTypeImage.Image = Properties.Resources.Vision_512;
                        break;
                }
                this.Invalidate();

            }
        }

       
        private int _LDLApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LDLApplication;

       
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;



        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication;

        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        private void _ResetForm()
        {

            lblLDLAppID.Text = "N/A";
            lblDClassName.Text = "[????]";
            lblApplicantName.Text = "[????]";
            lblRetokenTestID.Text = "N/A";
            lbleTestFees.Text = "0";
            lblRetokenAppFees.Text = "0";
            lblTotalFees.Text = "0";
            lblTrails.Text = "0";
            gbRetokenTest.Enabled = false;
        }

        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show($"Could not load Test Appointment: {_LDLApplicationID} information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }
            lbleTestFees.Text = _TestAppointment.Fees.ToString();

            // Compare current date with appointment date to set the min date
            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtDate.MinDate = DateTime.Now;
            else
                dtDate.MinDate = _TestAppointment.AppointmentDate;

            dtDate.Value = _TestAppointment.AppointmentDate;

            if(_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRetokenTestID.Text = "N/A";
                lblRetokenAppFees.Text = "0";
            }
            else
            {
                gbRetokenTest.Enabled = true;
                lblRetokenTestID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
                gbRetokenTest.Text = "Schedule Retake Test";
                lbleTestFees.Text = _TestAppointment.RetakeTestAppInfo.Fees.ToString();
            }

            return true;
        }

        private bool _HandleActiveAppointmentConstraint()
        {
            if(_Mode == enMode.AddNew &&
                _LDLApplication.IsThereAnActiveScheduledTest((int)_TestType))
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "This Person Already have an active appointment for this Test Type";
                btnSave.Enabled = false;
                dtDate.Enabled = false;

                return false;
            }

            return true;
        }

        private bool _HandleIsLockedAppointmentConstraint()
        {
            // If Locked, CANNOT Edit
            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Can not Edit this Appointment, As it Attended Already";
                btnSave.Enabled = false;
                dtDate.Enabled = false;

                return false;
            }
            else
                lblUserMessage.Visible = false;

            return true;
    }
        
        private bool _HandlePreviousTestConstraint()
        {
            switch (TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    lblUserMessage.Visible = false;
                    return true;

                case clsTestType.enTestType.WrittenTest:
                    if(_LDLApplication.DoesPassedTestType((int) clsTestType.enTestType.VisionTest))
                    {
                        btnSave.Enabled        = true;
                        dtDate.Enabled         = true;
                        lblUserMessage.Visible = false;
                        
                        return true;
                    }
                    else
                    {
                        lblUserMessage.Text = "Can't Sechedule, You Should Pass Vesion Test First";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled        = false;
                        dtDate.Enabled         = false;
                        
                        return false;
                    }


                case clsTestType.enTestType.StreetTest:

                    if (_LDLApplication.DoesPassedTestType((int)clsTestType.enTestType.WrittenTest))
                    {
                        btnSave.Enabled        = true;
                        dtDate.Enabled         = true;
                        lblUserMessage.Visible = false;
                        
                        return true;
                    }
                    else
                    {
                        lblUserMessage.Text = "Can't Sechedule, You Should Pass Written Test First";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled        = false;
                        dtDate.Enabled         = false;
                        
                        return false;
                    }

                default:
                    return false;
            }
        }

        private bool _HandleRetakeTestApplication()
        {
            if(_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetokenTest)
            {
                // Create an New Application First then Link it with appointment
                clsApplication NewApp = new clsApplication();

                
                NewApp.ApplicantPersonID = _LDLApplication.ApplicantPersonID;
                NewApp.ApplicationDate = DateTime.Now;
                NewApp.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;

                // Set Status to Completed Directly
                NewApp.Status = clsApplication.enApplicationStatus.Completed;
                
                NewApp.LastStatusDate = DateTime.Now;
                NewApp.Fees = clsApplicationType.Find(NewApp.ApplicationTypeID).ApplicationFees;
                NewApp.CreatedByUserID = clsGlobal.LoggedInUser.UserID;

                if(!NewApp.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Failed to Create Application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = NewApp.ApplicationID;
                return true;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!_HandleRetakeTestApplication())
                return;

            _TestAppointment.LDLAppID = _LDLApplicationID;
            _TestAppointment.AppointmentDate = dtDate.Value;
            _TestAppointment.Fees = Convert.ToDouble(lblTotalFees.Text);
            _TestAppointment.TestTypeID = (int)_TestType;
            _TestAppointment.CreatedByUserID = clsGlobal.LoggedInUser.UserID;


            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Appointment Scheduled Succssfully", "Secheduled Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Cant Scheduled Appointment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        
        public void LoadInfo(int LDLApplicationID, int TestAppointmentID = -1)
        {
            _ResetForm();

            // Determine Mode
            if (TestAppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _LDLApplicationID = LDLApplicationID;
            _TestAppointmentID = TestAppointmentID;
            _LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID);

            if(_LDLApplication == null)
            {
                MessageBox.Show($"Could not load Local Driving License Application: {LDLApplicationID} information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            // Determine Creation Type Mode
            if (_LDLApplication.DoesAttendTestType((int)_TestType))
                _CreationMode = enCreationMode.RetokenTest;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;

            double TestTypeFees = clsTestType.Find((int)_TestType).TestTypeFees;

            double RetokenTestFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees;
            
            if (_CreationMode == enCreationMode.RetokenTest)
            {
                lblRetokenAppFees.Text = RetokenTestFees.ToString();
                gbTestType.Text = "Schedule Retake Test";
                lblHeader.Text = "Schedule Retake Test";
                gbRetokenTest.Enabled = true;

            }
            else
            {
                gbRetokenTest.Enabled = false;
                gbTestType.Text = "Schedule Test";
                lblRetokenAppFees.Text = "0";
                lblTotalFees.Text = "0";
                lblRetokenTestID.Text = "N/A";
            }

            lblLDLAppID.Text = _LDLApplication.LDLApplicationID.ToString();
            lblDClassName.Text = clsLicenseClass.Find(_LDLApplication.LicenseClassID).ClassName;
            lblApplicantName.Text = _LDLApplication.ApplicantFullName.ToString();

            // Number of Trails for this Test Type before
            lblTrails.Text = _LDLApplication.TotalTrialsPerTest((int)_TestType).ToString();

            if (_Mode == enMode.AddNew)
            {
                lblRetokenTestID.Text = "N/A";
                lbleTestFees.Text = TestTypeFees.ToString();
                dtDate.MinDate = DateTime.Now;

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }



            if (_CreationMode == enCreationMode.FirstTimeSchedule)
                lblTotalFees.Text = (TestTypeFees).ToString();
            else
                lblTotalFees.Text = (TestTypeFees + RetokenTestFees).ToString();


            // Handle Constraints
            if (!_HandleActiveAppointmentConstraint())
                return;

            if(!_HandleIsLockedAppointmentConstraint())
                return;

            if (!_HandlePreviousTestConstraint())
                return;
            



        }

    }
}
