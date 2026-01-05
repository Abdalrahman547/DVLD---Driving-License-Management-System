using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.People.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        // Define a custome event handler delegate with parameter
        public event Action<int> OnLicenseSelected;

        // Create a protected method to raise the event with parameter
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
                handler(LicenseID);
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set
            {
                _FilterEnabled = value;
                groupBox1.Enabled = _FilterEnabled;
            }
        }

        private int _LicenseID = -1;

        public int LicenseID { get { return ctrlDriverLicenseInfo1.LicenseID; } }
        
        public clsLicense SelectedLicenseInfo { get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; } }
        
        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }
        
        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);

            if (e.KeyChar == (char)Keys.Enter)
            {
                btnFind.PerformClick();
            }
        }

        private void _FindNow()
        {
            ctrlDriverLicenseInfo1.LoadInfo(Convert.ToInt32(_LicenseID));

            if (OnLicenseSelected != null && FilterEnabled)
            {
                OnLicenseSelected(_LicenseID);
            }
        }
        
        public void LoadLicenseInfo(int LicenseID)
        {
            txtFilterValue.Text = LicenseID.ToString();
            _LicenseID = LicenseID;
            _FindNow();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterValue.Text))
            {
                MessageBox.Show("Please enter a valid License ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _LicenseID = Convert.ToInt32(txtFilterValue.Text);

            _FindNow();
        }

        public void FilterFoucus()
        {
            txtFilterValue.Focus();
        }

        public void ClearFilter()
        {
            txtFilterValue.Text = string.Empty;
        }

        private void DatabackEvent(object sender, int LicenseID)
        {
            txtFilterValue.Text = LicenseID.ToString();

            ctrlDriverLicenseInfo1.LoadInfo(LicenseID);
        }
    }
}
