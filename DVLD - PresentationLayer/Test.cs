using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management
{
    public partial class TestForm
        : Form
    {
        public TestForm()
        {
            InitializeComponent();
            //ctrlPersonCard1.LoadPersonInfo(1033);
            //ctrlUserCard1.LoadUserInfo(28);
            //ctrlLocalDrivingLicenseApplicationCard1.LoadLDLApplicationInfoByLDLApplicationID(36);
            //ctrlApplicationCard1.LoadApplicationInfo(110);

            ctrlScheduleTest1.LoadInfo(36);

        }


        private void Test_Load(object sender, EventArgs e)
        {

        }
    }
}
