using DVLD___BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management.Applications.International_Driving_License.Controls
{
    public partial class ctrlInternationalLicenseCard : UserControl
    {
        private int _ILID = -1;

        private clsInternationalLicense _LicenseInfo;

        public int InternationalLicenseID
        {
            get { return _ILID; }
        }

        public clsInternationalLicense SelectedLicenseInfo
        {
            get { return _LicenseInfo; }
        }
        public ctrlInternationalLicenseCard()
        {
            InitializeComponent();
        }

        private void _LoadPerosnImage()
        {
            if (_LicenseInfo.DriverInfo.PersonInfo.Gendor == 0)
                pbPersonImage.Image = Properties.Resources.Male_512;
            else
                pbPersonImage.Image = Properties.Resources.Female_512;

            string ImagePath = _LicenseInfo.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != string.Empty)
            {
                if (File.Exists(ImagePath))
                    pbPersonImage.ImageLocation = ImagePath;
                else
                    MessageBox.Show($"Could not find this image: {ImagePath}", "Error", MessageBoxButtons.OK);
            }
        }

        private void _FillInternationalLicenseInfo()
        {
            lblInternationalLicenseID.Text = _LicenseInfo.InternationalLicenseID.ToString();
            lblApplicationID.Text = _LicenseInfo.ApplicationID.ToString();
            lblLicenseID.Text = _LicenseInfo.IssuedUsingLocalLicenseID.ToString();
            lblFullName.Text = _LicenseInfo.ApplicantFullName;
            lblNationalNo.Text = _LicenseInfo.DriverInfo.PersonInfo.NationalNo;
            lblDateOfBirth.Text = _LicenseInfo.DriverInfo.PersonInfo.DateOfBirth.ToString("d");
            lblGendor.Text = _LicenseInfo.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblIssueDate.Text = _LicenseInfo.IssueDate.ToString("d");
            lblExpirationDate.Text = _LicenseInfo.ExpirationDate.ToString("d");
            lblIsActive.Text = _LicenseInfo.IsActive ? "Yes" : "No";
            lblDriverID.Text = _LicenseInfo.DriverID.ToString();

            _LoadPerosnImage();
        }

        private void _LoadData()
        {
            if (_LicenseInfo == null)
            {
                MessageBox.Show($"No International License With ID: {_ILID}", "Not Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillInternationalLicenseInfo();
        }

        public void LoadInfo(int ILID)
        {
            _ILID = ILID;

            _LicenseInfo = clsInternationalLicense.FindByInternationalID(_ILID);

            _LoadData();
        }

    }
}
