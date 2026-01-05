using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD___BussinessLayer.clsTestType;

namespace DVLD___Driving_License_Management.Test
{
    public partial class frmListTestAppointments : Form
    {
        private int _LDLApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LDLApplication = null;
        private int _AppointmentID = -1;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;


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
                        lblheader.Text = "Vision Test Appointments";
                        pbTestTypeImage.Image = Properties.Resources.Vision_512;
                        break;

                    case clsTestType.enTestType.WrittenTest:
                        lblheader.Text = "Written Test Appointments";
                        pbTestTypeImage.Image = Properties.Resources.Written_Test_512;
                        break;

                    case clsTestType.enTestType.StreetTest:
                        lblheader.Text = "Practical Test Appointments";
                        pbTestTypeImage.Image = Properties.Resources.Street_Test_32;
                        break;

                    default:
                        lblheader.Text = "Vision Test Appointments";
                        pbTestTypeImage.Image = Properties.Resources.Street_Test_32;
                        break;
                }
                this.Invalidate();

            }
        }

        public frmListTestAppointments(int LDLApplicationID, clsTestType.enTestType TestType, int AppointmentID = -1)
        {
            InitializeComponent();
            _LDLApplicationID = LDLApplicationID;
            _TestType = TestType;
            _AppointmentID = AppointmentID;

        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            
            if (_LDLApplication.IsThereAnActiveScheduledTest((int)_TestType))
            {
                MessageBox.Show("The person already has an active Appointment for this test type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTest LastTest = _LDLApplication.GetLastTestperTestType((int)_TestType);

            if (LastTest == null)
            {
                Form frm1 = new frmScheduleTest(_LDLApplicationID, _TestType);

                frm1.ShowDialog();
                frmListTestAppointments_Load(null, null);

                return;
            }

            if(LastTest.TestResult)
            {               
                MessageBox.Show("The person has already passed this test type.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           
            Form frm2 = new frmScheduleTest(
                LastTest.TestAppointmentInfo.LDLAppID,
                _TestType);
                
            frm2.ShowDialog();
                
            frmListTestAppointments_Load(null, null);
                
        }

        private void _RefreshList()
        {
            dgvVisionAppontments.DataSource = clsTestAppointment.GetApplicationTestAppointmentPerTestID(_LDLApplicationID, (int)_TestType);
            lblNumOfRecords.Text = dgvVisionAppontments.Rows.Count.ToString();
        }
        
        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            TestType = _TestType;
            
            ctrlLocalDrivingLicenseApplicationCard1.LoadLDLApplicationInfoByLDLApplicationID(_LDLApplicationID);

            //MessageBox.Show(_LDLApplicationID.ToString());

            _LDLApplication = ctrlLocalDrivingLicenseApplicationCard1.LDLApplication;
            _RefreshList();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = Convert.ToInt32(dgvVisionAppontments.CurrentRow.Cells[0].Value);

            Form frm = new frmScheduleTest(_LDLApplicationID, TestType, TestAppointmentID);

            frm.ShowDialog();

            _RefreshList();

        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = Convert.ToInt32(dgvVisionAppontments.CurrentRow.Cells[0].Value);

            Form frm = new frmTakeTest(_LDLApplicationID, _TestType, TestAppointmentID);

            frm.ShowDialog();

            frmListTestAppointments_Load(null, null);
        }
    }
}
