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

namespace DVLD___Driving_License_Management.Users.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        private int _UserID = -1;
        clsUser _User;

        public int UserID
        {
            get { return _UserID; }
        }

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int UserID)
        {
            _UserID = UserID;
           
            _User = clsUser.FindByUserID(UserID);

            _LoadData();

        }

        private void _FillUserInfo()
        {
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName;
            lblIsActive.Text = _User.IsActive ? "Yes" : "No";
            lblIsAdmin.Text = _User.IsAdmin ? "Yes" : "No";
        }

        private void _LoadData()
        {
            if (_User != null)
            {
                _FillUserInfo();
                ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
            }
            else
            {
                _ResetInfo();
                MessageBox.Show($"Could not load User: {_UserID} information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void _ResetInfo()
        {
            lblUserID.Text = string.Empty;
            lblUserName.Text = string.Empty;
            lblIsActive.Text = string.Empty;
            lblIsAdmin.Text = string.Empty;
            ctrlPersonCard1.LoadPersonInfo(-1);
        }

    }
}
