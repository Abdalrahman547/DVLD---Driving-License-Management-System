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
    public partial class frmShowLocalDrivingLicenseApplicationInfo : Form
    {
        public frmShowLocalDrivingLicenseApplicationInfo(int LDLApplicationID)
        {
            InitializeComponent();
            ctrlLocalDrivingLicenseApplicationCard1.LoadLDLApplicationInfoByLDLApplicationID(LDLApplicationID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
