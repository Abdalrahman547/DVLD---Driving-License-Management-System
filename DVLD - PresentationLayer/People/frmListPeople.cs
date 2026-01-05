using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management
{
    public partial class frmListPeople: Form
    {
        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();

        // Only Select the columns that i want to show in the DataGridView
        private DataTable _dtPeopleView = _dtAllPeople.DefaultView.ToTable(
            false, "PersonID", "NationalNo", "FirstName", "SecondName",
            "ThirdName", "LastName", "GendorCaption", "DateOfBirth",
            "Address", "CountryName", "Phone", "Email");
        public frmListPeople()
        {
            InitializeComponent();
            cbFilter.SelectedIndex = 0; // Default to "All People"
        }

        private void _RefreshPeopleList()
        {
            _dtAllPeople = clsPerson.GetAllPeople();

            _dtPeopleView = _dtAllPeople.DefaultView.ToTable(
            false, "PersonID", "NationalNo", "FirstName", "SecondName",
            "ThirdName", "LastName", "GendorCaption", "DateOfBirth",
            "Address", "CountryName", "Phone", "Email");

            dgvPeople.DataSource = _dtPeopleView;

            lblNumOfRecords.Text = (dgvPeople.Rows.Count).ToString();

            cbFilter.SelectedIndex = 0;
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditiPersonInfo();

            frm.ShowDialog();

            _RefreshPeopleList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList();

            if(dgvPeople.Rows.Count > 0)
            {
                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[1].HeaderText = "National No";
                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[7].HeaderText = "Date of Birth";
                dgvPeople.Columns[8].HeaderText = "Address";
                dgvPeople.Columns[9].HeaderText = "Country";


            }

        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {

            string FilterColumn = "";

            switch (cbFilter.Text)
            {
                case "Person ID":
                FilterColumn = "PersonID";
                break;

                case "National No":
                FilterColumn = "NationalNo";
                break;

                case "First Name":
                FilterColumn = "FirstName";
                break;

                case "Second Name":
                FilterColumn = "SecondName";
                break;

                case "Third Name":
                FilterColumn = "ThirdName";
                break;

                case "Last Name":
                FilterColumn = "LastName";
                break;

                case "Country":
                FilterColumn = "CountryName";
                break;

                case "Gendor":
                FilterColumn = "GendorCaption";
                break;

                case "Date of Birth":
                FilterColumn = "DateOfBirth";
                break;

                default:
                FilterColumn = "None";
                break;
            }

            // Reset Filter to defualt view
            if (txtSearchBox.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtPeopleView.DefaultView.RowFilter = "";

                lblNumOfRecords.Text = (dgvPeople.Rows.Count).ToString();

                return;
            }

            if (FilterColumn == "PersonID")
                // Dealing with intger
                _dtPeopleView.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtSearchBox.Text.Trim());
            else
                // Dealing with string
                _dtPeopleView.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtSearchBox.Text.Trim());

            lblNumOfRecords.Text = (dgvPeople.Rows.Count).ToString();
        }

        private int _GetNationalCountryIDFromName(string CountryName)
        {
            int CountryID = -1;

            SqlConnection connection = new SqlConnection("Server=.;Database=DVLD;User Id=sa;Password=123456;");

            string query = "SELECT CountryID FROM Countries WHERE CountryName = @CountryName";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                    CountryID = reader.GetInt32(0); // CountryID is the first column in the result set

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

            return CountryID;
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Comming Soon :)\a");
        }

        private void callPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Comming Soon :)\a");
        }

        private void showInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = Convert.ToInt32(dgvPeople.CurrentRow.Cells["PersonID"].Value);
            //MessageBox.Show(PersonID.ToString());

            Form frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();

            _RefreshPeopleList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = Convert.ToInt32(dgvPeople.CurrentRow.Cells["PersonID"].Value);

            Form frm = new frmAddEditiPersonInfo(PersonID);

            frm.ShowDialog();

            _RefreshPeopleList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = Convert.ToInt32(dgvPeople.CurrentRow.Cells["PersonID"].Value);

            string PersonName = dgvPeople.CurrentRow.Cells["FirstName"].Value.ToString() + " " +
                                dgvPeople.CurrentRow.Cells["LastName"].Value.ToString();

            if (MessageBox.Show($"Are you sure you want to delete [ {PersonName} ]?",
                                "Confirm Delete",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) == DialogResult.OK)
            {

                 if (clsPerson.DeletePerson(PersonID))
                 {
                    MessageBox.Show("Person Deleted Successfully");

                    _RefreshPeopleList();
                 }
                
                else
                    MessageBox.Show("Error: Could not delete the person.\a");

            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearchBox.Visible = (cbFilter.Text != "None");

            if (txtSearchBox.Visible)
            {
                txtSearchBox.Text = "";
                txtSearchBox.Focus();
            }
        }

        private void txtSearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Person ID" || cbFilter.Text == "Phone")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
