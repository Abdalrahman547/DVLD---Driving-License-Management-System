using DVLD___BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace DVLD___Driving_License_Management.Tests.Controls
{
    public partial class ctrlScheduledTestInfo : UserControl
    {

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
        public int TestAppointmentID
        {
            get { return _TestAppointmentID; }
            set { _TestAppointmentID = value; }
        }

        private int _TestID = -1;
        public int TestID 
        {
            get { return _TestID; }
            set { _TestID = value; }
        }

        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication;
        
        public ctrlScheduledTestInfo()
        {
            InitializeComponent();
        }

        private void _ResetForm()
        {
            lblLDLAppID.Text = "N/A";
            lblDClassName.Text = "[????]";
            lblApplicantName.Text = "[????]";
            lbleTestFees.Text = "0";
            lblTrails.Text = "0";
            lblTestID.Text = "Not Taken Yet";
        }

        public void LoadInfo(int LDLApplicationID, int TestAppointmentID)
        {
            _ResetForm();

            _LDLApplicationID = LDLApplicationID;
            _TestAppointmentID = TestAppointmentID;
            

            _LDLApplication = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID);

            if (_LDLApplication == null)
            {
                MessageBox.Show($"Could not load Local Driving License Application: {LDLApplicationID} information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show($"Could not load Test Appointment: {_LDLApplicationID} information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLDLAppID.Text = _LDLApplication.LDLApplicationID.ToString();
            lblDClassName.Text = clsLicenseClass.Find(_LDLApplication.LicenseClassID).ClassName;
            lblApplicantName.Text = _LDLApplication.ApplicantFullName.ToString();
            lblTrails.Text = _LDLApplication.TotalTrialsPerTest((int)_TestType).ToString();

            lblDate.Text = _TestAppointment.AppointmentDate.ToString();
            lbleTestFees.Text = _TestAppointment.Fees.ToString();


            int TestID = _TestAppointment.GetTestID();
            
            if (TestID == -1)
                lblTestID.Text = "Not Taken Yet";
            else
                lblTestID.Text = TestID.ToString();
        }
    }
}
