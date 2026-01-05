using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management.Licenses.Local_Licenses
{
    public partial class frmShowDriverLicenseInfo : Form
    {
        private int _LicenseID = -1;
        public frmShowDriverLicenseInfo(int LicenseID)
        {
            InitializeComponent();

            _LicenseID = LicenseID;
        }

        private void frmShowDriverLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfo1.LoadInfo(_LicenseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
