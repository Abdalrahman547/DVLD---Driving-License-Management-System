using DVLD___BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management.Licenses.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private int _LicenseID = -1;
        
        private clsLicense _LicenseInfo;
        
        public int LicenseID
        {
            get { return _LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        {
            get { return _LicenseInfo; }
        }

        public ctrlDriverLicenseInfo()
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

        private void _FillDriverLicenseInfo()
        {
            lblClassName.Text = clsLicenseClass.Find(_LicenseInfo.LicenseClass).ClassName;
            lblbLicenseID.Text = _LicenseInfo.LicenseID.ToString();
            lblFullName.Text = _LicenseInfo.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text = _LicenseInfo.DriverInfo.PersonInfo.NationalNo;
            lblDateOfBirth.Text = _LicenseInfo.DriverInfo.PersonInfo.DateOfBirth.ToString("d");
            lblGendor.Text = _LicenseInfo.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblIssueDate.Text = _LicenseInfo.IssueDate.ToString("d");
            lblExpirationDate.Text = _LicenseInfo.ExpirationDate.ToString("d");
            lblIssueReason.Text = _LicenseInfo.IssueReasonText;
            lblNotes.Text = _LicenseInfo.Notes == "" ? "No Notes" : _LicenseInfo.Notes;
            lblIsActive.Text = _LicenseInfo.IsActive ? "Yes" : "No";
            lblDriverID.Text = _LicenseInfo.DriverID.ToString();
            lblIsDetained.Text = clsDetainedLicense.IsLicenseDetained(_LicenseInfo.LicenseID) ? "Yes" : "No";

            _LoadPerosnImage();
        }

        private void _LoadData()
        {
            if (_LicenseInfo == null)
            {
                MessageBox.Show($"No License With ID: {_LicenseID}", "Not Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillDriverLicenseInfo();
        }
       
        public void LoadInfo(int licenseID)
        {
            this._LicenseID = licenseID;

            _LicenseInfo = clsLicense.Find(_LicenseID);

            _LoadData();
        }

    }
}
