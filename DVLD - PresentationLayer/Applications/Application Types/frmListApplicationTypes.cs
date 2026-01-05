using DVLD___BussinessLayer;
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

namespace DVLD___Driving_License_Management.ApplicationTypes
{
    public partial class frmListApplicationTypes : Form
    {

        private DataTable _dtApplicationList = clsApplicationType.GetAllApplications();

        public frmListApplicationTypes()
        {
            InitializeComponent();
        }

        private void _RefreshList()
        {
            _dtApplicationList = clsApplicationType.GetAllApplications();

            dgvApplicationTypes.DataSource = _dtApplicationList;

            lblNumOfRecords.Text = dgvApplicationTypes.Rows.Count.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {
            _RefreshList();

            dgvApplicationTypes.Columns[0].HeaderText = "ID";
            dgvApplicationTypes.Columns[0].Width = 100;

            dgvApplicationTypes.Columns[1].HeaderText = "Title";
            dgvApplicationTypes.Columns[1].Width = 350;

            dgvApplicationTypes.Columns[2].HeaderText = "Fees";
            dgvApplicationTypes.Columns[2].Width = 100;

        }

        private void updateApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.LoggedInUser.IsAdmin)
            {
                MessageBox.Show("Only Admin users can update Application Types.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int ID = Convert.ToInt32(dgvApplicationTypes.CurrentRow.Cells[0].Value);

            Form frm = new frmUpdateApplicationType(ID);

            frm.ShowDialog();

            _RefreshList();
        }
    }
}
