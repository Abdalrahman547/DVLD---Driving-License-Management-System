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

namespace DVLD___Driving_License_Management.Tests
{
    public partial class frmScheduleTest : Form
    {
        private int _LDLApplicationID = -1;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;

        private int _AppointmentID = -1;
        public frmScheduleTest(int LDLAppID, clsTestType.enTestType TestType, int AppointmentID = -1)
        {
            InitializeComponent();
            _LDLApplicationID = LDLAppID;
            _TestType = TestType;
            _AppointmentID = AppointmentID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestType = _TestType;
            ctrlScheduleTest1.LoadInfo(_LDLApplicationID, _AppointmentID);
        }
    }
}
