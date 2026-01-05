using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD___BussinessLayer;
using System.IO;

namespace DVLD___Driving_License_Management.People.Controls
{
    public partial class ctrlPersonCard: UserControl
    {
        int _PersonID = -1;
        private clsPerson _Person;

        public int PersonID
        {
            get { return _PersonID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public ctrlPersonCard(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        public  void LoadPersonInfo(int PersonID)
        {
            _PersonID = PersonID;
            _Person = clsPerson.Find(_PersonID);

            _LoadData();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);

            _LoadData();
        }
        
        private void _FillPersonInfo()
        {
            llSetImage.Visible = true;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblName.Text = _Person.FullName;
            lblPhone.Text = _Person.Phone;
            lblAddress.Text = _Person.Address;
            lblBirthdate.Text = _Person.DateOfBirth.ToShortDateString();
            lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;
            lblGendor.Text = (_Person.Gendor == 0 ? "Male" : "Female");
            lblEmail.Text = _Person.Email != string.Empty ? _Person.Email : "";

            _LoadPerosnImage();
        }

        private void _ResetPersonInfo()
        {
            llSetImage.Visible = false;
            lblPersonID.Text = string.Empty;
            lblNationalNo.Text = string.Empty;
            lblName.Text = string.Empty;
            lblPhone.Text = string.Empty;
            lblAddress.Text = string.Empty;
            lblBirthdate.Text = string.Empty;
            lblCountry.Text = string.Empty;
            lblGendor.Text = string.Empty;
            pictureBox1.Image = Properties.Resources.Male_512;
            lblEmail.Text = string.Empty;
        }

        private void _LoadPerosnImage()
        {
            if (_Person.Gendor == 0)
                pictureBox1.Image = Properties.Resources.Male_512;
            else
                pictureBox1.Image = Properties.Resources.Female_512;

            string ImagePath = _Person.ImagePath;

            if(ImagePath != string.Empty)
            {
                if (File.Exists(ImagePath))
                    pictureBox1.ImageLocation = ImagePath;
                else
                    MessageBox.Show($"Could not find this image: {ImagePath}", "Error", MessageBoxButtons.OK);
            }
        }

        private void _LoadData()
        {

            if( _Person != null )
            {
                _FillPersonInfo();

                _PersonID = _Person.PersonID;
            }
            else
            {
                _ResetPersonInfo();
                MessageBox.Show("Person not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ctrlShowPersonInfo_Load(object sender, EventArgs e)
        {
            if(!DesignMode && _PersonID > 0)
            {
                LoadPersonInfo(_PersonID);
            }
           
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditiPersonInfo frm = new frmAddEditiPersonInfo(_PersonID);
            frm.ShowDialog();

            // Refresh
            LoadPersonInfo(_PersonID);
        }

    }
}
