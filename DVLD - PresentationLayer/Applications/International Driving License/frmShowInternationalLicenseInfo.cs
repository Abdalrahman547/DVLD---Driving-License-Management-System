using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management.Applications.International_Driving_License
{
    public partial class frmShowInternationalLicenseInfo : Form
    {
        private int _ILID = -1;
        public frmShowInternationalLicenseInfo(int ILID)
        {
            InitializeComponent();
            _ILID = ILID;
        }

        private void frmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlInternationalLicenseCard1.LoadInfo(_ILID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
