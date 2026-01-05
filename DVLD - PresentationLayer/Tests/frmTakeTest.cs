using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.Global_Classes;
using DVLD___Driving_License_Management.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management.Tests
{
    public partial class frmTakeTest : Form
    {
        private int _LDLApplicationID = -1;
        private int _AppointmentID = -1;
        private clsTestAppointment _TestAppointment;
        private clsTestType.enTestType _TestType;

        private int _TestID = -1;
        private clsTest _Test;
        public frmTakeTest(int LDLApplicationID, clsTestType.enTestType TestType, int AppointmentID)
        {
            InitializeComponent();
            
            _LDLApplicationID = LDLApplicationID;
            _TestType = TestType;
            _AppointmentID = AppointmentID;
            
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {

            ctrlScheduledTestInfo1.TestType = _TestType;
            ctrlScheduledTestInfo1.LoadInfo(_LDLApplicationID, _AppointmentID);

            _TestAppointment = clsTestAppointment.Find(_AppointmentID);

            _Test = clsTest.Find(_TestAppointment.GetTestID());

            if(_Test != null)
            {
                if(_Test.TestResult)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;
                
                richTextBox1.Text = _Test.Notes;
                richTextBox1.Enabled = false;
                rbPass.Enabled = false;
                rbFail.Enabled = false;
                btnSave.Enabled = false;
                lblUseMessage.Visible = true;
                return;
            }
            else
                _Test = new clsTest();

            lblUseMessage.Visible = false;
            btnSave.Enabled = false;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_Validate())
                return;


            _Test.TestAppointmentID = _AppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = richTextBox1.Text.Trim();
            _Test.CreatedByUserID = clsGlobal.LoggedInUser.UserID;

            if(MessageBox.Show("Are you sure you want to Save Test Result?, Test Result Cannot changed after saved", "Confirm!",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            if (_Test.Save())
            {
                MessageBox.Show("Test Result Saved Successfully", "Saved Successfully",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Failed To Save the Test Result. Please Try Again Later.", "Save Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        private bool _Validate()
        {
            if (rbPass.Checked == false && rbFail.Checked == false)
            {
                MessageBox.Show("Please select Pass or Fail.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void rbPass_CheckedChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }

        private void rbFail_CheckedChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;

        }
    }
}
