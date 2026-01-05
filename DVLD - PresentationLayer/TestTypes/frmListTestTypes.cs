using DVLD___BussinessLayer;
using DVLD___Driving_License_Management.ApplicationTypes;
using DVLD___Driving_License_Management.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management.TestTypes
{
    public partial class frmListTestTypes : Form
    {
        private DataTable _dtTestsList = clsTestType.GetAllTests();
        public frmListTestTypes()
        {
            InitializeComponent();
        }

        private void _RefreshList()
        {
            _dtTestsList = clsTestType.GetAllTests();

            dgvTestTypes.DataSource = _dtTestsList;

            lblNumOfRecords.Text = dgvTestTypes.Rows.Count.ToString();

        }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _RefreshList();

            dgvTestTypes.Columns[0].HeaderText = "ID";
            dgvTestTypes.Columns[0].Width = 100;

            dgvTestTypes.Columns[1].HeaderText = "Title";
            dgvTestTypes.Columns[1].Width = 150;

            dgvTestTypes.Columns[2].HeaderText = "Description";
            dgvTestTypes.Columns[2].Width = 350;

            dgvTestTypes.Columns[3].HeaderText = "Fees";
            dgvTestTypes.Columns[3].Width = 100;

        }

        private void updateTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!clsGlobal.LoggedInUser.IsAdmin)
            {
                MessageBox.Show("Only Admin users can update Test Types.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ID = Convert.ToInt32(dgvTestTypes.CurrentRow.Cells[0].Value);

            Form frm = new frmUpdateTestType(ID);

            frm.ShowDialog();

            _RefreshList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
