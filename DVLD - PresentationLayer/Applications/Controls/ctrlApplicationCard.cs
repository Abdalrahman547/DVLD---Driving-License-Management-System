using DVLD___Driving_License_Management.People;
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

namespace DVLD___Driving_License_Management.Application
{
    public partial class ctrlApplicationCard : UserControl
    {
        private int _ApplicationID = -1;

        private clsApplication _Application;

        public int ApplicationID { get { return _ApplicationID; } }
        public clsApplication Application { get { return _Application; } }
        
        public ctrlApplicationCard()
        {
            InitializeComponent();
        }

        private void _FillApplicationInfo()
        {

            _ApplicationID = _Application.ApplicationID;

            lblAppID.Text =     _Application.ApplicationID.ToString();
            lblApplicant.Text = _Application.ApplicantFullName.ToString();
            lblFees.Text =      _Application.Fees.ToString();

            lblStatus.Text =   _Application.StatusText;
            lblType.Text = clsApplicationType.Find(_Application.ApplicationTypeID).ApplicationTitle;
            lblDate.Text = _Application.ApplicationDate.ToString("d");
            lblStatusDate.Text = _Application.LastStatusDate.ToString("d");
            lblCreatedByUserName.Text = clsUser.FindByUserID(_Application.CreatedByUserID).UserName;
        }

        private void ResetApplicationInfo()
        {
            _ApplicationID = -1;
            _Application = null;
            lblAppID.Text = "N/A";
            lblFees.Text = "0";
            lblApplicant.Text = "[????]";
            lblStatus.Text = "[????]";
            lblType.Text = "[????]";
            lblDate.Text = "[??/??/????]";
            lblStatusDate.Text = "[??/??/????]";

        }

        private void _LoadData()
        {
            if (_Application == null)
            {
                MessageBox.Show("Application not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetApplicationInfo();
                return;
            }


            _FillApplicationInfo();
        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            _ApplicationID = ApplicationID;

            _Application = clsApplication.Find(ApplicationID);
            
            _LoadData();
        }

        private void lnklblVeiwPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowPersonInfo(_Application.ApplicantPersonID);

            frm.ShowDialog();
        }
    }
}
