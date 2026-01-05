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

namespace DVLD___Driving_License_Management.Licenses.Local_Licenses
{
    public partial class frmShowDriverLicenseHistory : Form
    {
        private int _PersonID = -1;
        public frmShowDriverLicenseHistory()
        {
            InitializeComponent();
        }

        public frmShowDriverLicenseHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        public frmShowDriverLicenseHistory(string NationalNo)
        {
            InitializeComponent();
            _PersonID = clsPerson.Find(NationalNo).PersonID;
        }

        private void frmShowDriverLicenseHostory_Load(object sender, EventArgs e)
        {
            if(_PersonID != -1)
            {
                ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                ctrlDriverLicenses1.LoadInfoByPersonID(_PersonID);
            }
            else
            {
                ctrlPersonCardWithFilter1.FilterEnabled = true;
                ctrlPersonCardWithFilter1.FilterFoucus();
            }
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;

            if (_PersonID != -1)
                ctrlDriverLicenses1.LoadInfoByPersonID(_PersonID);
            else
                ctrlDriverLicenses1.Clear();
        }

    }
}
