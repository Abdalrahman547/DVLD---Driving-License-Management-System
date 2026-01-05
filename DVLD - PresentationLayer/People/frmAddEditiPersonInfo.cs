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
using System.Xml.Linq;

namespace DVLD___Driving_License_Management
{
    public partial class frmAddEditiPersonInfo : Form
    {

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int PersonID);

        // Declare an event using this delegate
        public event DataBackEventHandler DataBack;


        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        int _PersonID;
        clsPerson _Person;
        public static DataTable GetAllCountries()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection("Server=.;Database=DVLD;User Id=sa;Password=123456;");

            string query = "SELECT * FROM Countries order by CountryName";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }

        private void _FillCountriesInComoboBox()
        {
            DataTable dtCountries = GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountries.Items.Add(row["CountryName"]);
            }

        }

        public frmAddEditiPersonInfo()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;

            lblHeader.Text = "Add New Person";
            _Person = new clsPerson();
        }

        public frmAddEditiPersonInfo(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
            _Mode = enMode.Update;

            lblHeader.Text = "Edit Person Information";
        }

        private void _ResetDefaultValues()
        {
            _FillCountriesInComoboBox();

            // Hide/Show Remove Link incase there is an image or not
            LnklblRemoveImage.Visible = (pbPersonImage.ImageLocation != null);

            cbCountries.SelectedIndex = cbCountries.FindString("Egypt");

            // Set MaxDate to 18 years from today as LESS than 18 doesnt allowed
            dtBirthdate.MaxDate = DateTime.Now.AddYears(-18);
            dtBirthdate.Value = dtBirthdate.MaxDate;

            // Set MinDate to 100 years before today as More than 100 years ages people doesnt allowed to Drive 
            dtBirthdate.MinDate = DateTime.Now.AddYears(-100);


            txtNationalNo.Text = "";
            txtFirstName.Text = "";
            txtThirdName.Text = "";
            txtSecondName.Text = "";
            txtLastName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            rdMale.Checked = true;
            pbPersonImage.Image = Properties.Resources.Male_512;
            LnklblRemoveImage.Visible = false;
        }

        private void _LoadData()
        {

            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show($"No Person with ID: {_PersonID}" , "Person not found.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblPersonID.Text = _Person.PersonID.ToString();
            txtNationalNo.Text = _Person.NationalNo;
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtLastName.Text = _Person.LastName;
            dtBirthdate.Value = _Person.DateOfBirth;
            txtPhone.Text = _Person.Phone;
            txtAddress.Text = _Person.Address;
            cbCountries.SelectedIndex = _Person.NationalityCountryID;


            if (_Person.Gendor == 0)
            {
                rdMale.Checked = true;
            }
            else
            {
                rdFemale.Checked = true;
            }

            // Disable Gendor buttons
            rdMale.Enabled = false;
            rdFemale.Enabled = false;

            //Handle Null values
            if (!string.IsNullOrEmpty(_Person.ImagePath))
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;
                LnklblRemoveImage.Visible = true;
            }
            else
            {
                if (rdMale.Checked)
                    pbPersonImage.Image = Properties.Resources.Male_512;
                else
                    pbPersonImage.Image = Properties.Resources.Female_512;
            }
            //------------------
            if (_Person.ThirdName != null)
                txtThirdName.Text = _Person.ThirdName;
            else
                txtThirdName.Text = string.Empty;

            //------------------
            if (_Person.Email != null)
                txtEmail.Text = _Person.Email;
            else
                txtEmail.Text = string.Empty;

        }

        private void frmAddEditiPersonInfo_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private bool _FinalInputValidationCheck()
        {

            if (txtFirstName.Text == string.Empty)
            {
                MessageBox.Show("First Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtSecondName.Text == string.Empty)
            {
                MessageBox.Show("Second Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtLastName.Text == string.Empty)
            {
                MessageBox.Show("Last Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtNationalNo.Text == string.Empty)
            {
                MessageBox.Show("National No is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtPhone.Text == string.Empty)
            {
                MessageBox.Show("Phone number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtAddress.Text == string.Empty)
            {
                MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Handle Email
            if(txtEmail.Text != string.Empty && txtEmail.Text != _Person.Email)
            {
                if (!clsValidation.ValidateEmail(txtEmail.Text))
                {
                    MessageBox.Show(
                        "Invalid email address. Please enter a valid email.",
                        "Validation Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }

                if (clsPerson.IsEmailExists(_Person.Email))
                {
                    MessageBox.Show("Email is exists, Please Enter anohter one.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Text = string.Empty;
                    txtEmail.Focus();
                    return false;
                }
            }



            // handle Unique Values


            // Handle NationalNo
            if (txtNationalNo.Text != _Person.NationalNo && clsPerson.IsPersonExists(_Person.NationalNo))
            {
                MessageBox.Show("NationalNo is repeated, Please Enter it correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNationalNo.Text = string.Empty;
                txtNationalNo.Focus();
                return false;
            }

            if (_Person.Phone.Length < 11)
            {
                MessageBox.Show("Phone number Not Valid, Please Enter anohter one.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Text = string.Empty;
                txtPhone.Focus();
                return false;
            }

            // Handle Phone Number
            if (txtPhone.Text != _Person.Phone && clsPerson.IsPhoneExists(_Person.Phone))
            {
                MessageBox.Show("Phone number is exists, Please Enter anohter one.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Text = string.Empty;
                txtPhone.Focus();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandlePersonImage())
            {
                MessageBox.Show("Error handling person image. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.DateOfBirth = dtBirthdate.Value;
            _Person.Gendor = (short)(rdMale.Checked ? 0 : 1);
            _Person.Address = txtAddress.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.NationalityCountryID = cbCountries.SelectedIndex;

            _Person.ImagePath = pbPersonImage.ImageLocation != null ? pbPersonImage.ImageLocation.ToString() : string.Empty;

           

            if (!_FinalInputValidationCheck())
                return;

            if (_Person.Save())
            {
                MessageBox.Show("Person information saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Mode = enMode.Update;

                lblHeader.Text = "Edit Person Information";

                lblPersonID.Text = _Person.PersonID.ToString();

                // Trigger the event to send data back to the caller form
                DataBack?.Invoke(this, _Person.PersonID);

            }
            else
                MessageBox.Show("Failed to save person information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ValidateRequiredField(TextBox txtBox)
        {
            if (string.IsNullOrWhiteSpace(txtBox.Text))
            {
                errorProvider1.SetError(txtBox, "This field is required.");
            }
            else
            {
                errorProvider1.SetError(txtBox, "");
            }
        }

        private void txtbox_TextChanged_InputValidation(object sender, EventArgs e)
        {
            ValidateRequiredField((TextBox)sender);
        }

        private void LnklblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Select a photo";
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                pbPersonImage.Image = Image.FromFile(openFileDialog.FileName);

                pbPersonImage.ImageLocation = openFileDialog.FileName;

                LnklblRemoveImage.Visible = true;
            }

        }

        private void LnklblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            pbPersonImage.ImageLocation = null;

            if (rdMale.Checked)
                pbPersonImage.Image = Properties.Resources.Male_512;
            else
                pbPersonImage.Image = Properties.Resources.Female_512;

            LnklblRemoveImage.Visible = false;
        }

        private void rdMale_CheckedChanged(object sender, EventArgs e)
        {
            pbPersonImage.Image = Properties.Resources.Male_512;
        }

        private void rdFemale_CheckedChanged(object sender, EventArgs e)
        {
            pbPersonImage.Image = Properties.Resources.Female_512;
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        // -----------------------------
        // Handle Images Location

        private bool _HandlePersonImage()
        {
            // if there is any changes
            if (_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                // first we delete the old image from folder in case there is any
                if (!string.IsNullOrWhiteSpace(_Person.ImagePath) && File.Exists(_Person.ImagePath))
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting file: " + ex.Message);
                    }
                }

                if (pbPersonImage.ImageLocation != null)
                {
                    // Copy the new image to the folder

                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile, _PersonID))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK);
                        return false;
                    }
                }
            }

            return true; // No changes in image, so we return true
        }

        
    }
}
