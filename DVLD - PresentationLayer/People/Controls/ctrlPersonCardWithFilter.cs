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

namespace DVLD___Driving_License_Management.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        // Define a custome event handler delegate with parameter
        public event Action<int> OnPersonSelected;

        // Create a protected method to raise the event with parameter
        protected virtual void PersonSelected(int personID)
        {
            Action<int> handler = OnPersonSelected;

            if (handler != null)
                handler(personID);
        }

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get { return _ShowAddPerson; }
            set
            {
                _ShowAddPerson = value;
                btnAddNewPerson.Visible = _ShowAddPerson;
            }
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

        
        private int _PersonID = -1;

        public int PersonID { get { return ctrlPersonCard2.PersonID; } }
        
        public clsPerson SelectedPersonInfo { get { return ctrlPersonCard2.SelectedPersonInfo; } }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int PersonID)
        {
            txtFilterValue.Text = PersonID.ToString();
            _PersonID = PersonID;
            _FindNow();
        }

        private void _FindNow()
        {
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    ctrlPersonCard2.LoadPersonInfo(int.Parse(txtFilterValue.Text));
                    break;

                case "National No":
                    ctrlPersonCard2.LoadPersonInfo(txtFilterValue.Text);
                    break;

                default:
                    break;
            }
            
            // Raise the event with the parameter
            if (OnPersonSelected != null && FilterEnabled)
                OnPersonSelected(ctrlPersonCard2.PersonID); // The event Fired on
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = string.Empty;
            txtFilterValue.Focus();
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;

            txtFilterValue.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditiPersonInfo frm = new frmAddEditiPersonInfo();

            frm.DataBack += DatabackEvent; 

            frm.ShowDialog();
        }

        private void DatabackEvent(object sender, int PersonID)
        {
            txtFilterValue.Text = PersonID.ToString();

            ctrlPersonCard2.LoadPersonInfo(PersonID);
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // check if the pressed key is Enter (characker code 13)
            if(e.KeyChar == (char)13)
                btnFind.PerformClick();

            // Allow only digits if PersonID is Selected
            if (cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);  
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFilterValue.Text) && _FilterEnabled)
                _FindNow();
            else
            {
                MessageBox.Show("Please enter a valid value to search.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFilterValue.Focus();
            }
        }

        public void FilterFoucus()
        {
            txtFilterValue.Focus();
        }

    }
}
