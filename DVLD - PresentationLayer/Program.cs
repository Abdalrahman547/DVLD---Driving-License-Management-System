using DVLD___Driving_License_Management.Application.Local_Driving_License;
using DVLD___Driving_License_Management.Applications.International_Driving_License;
using DVLD___Driving_License_Management.Applications.Release_Detained_License;
using DVLD___Driving_License_Management.ApplicationTypes;
using DVLD___Driving_License_Management.Licenses.DetainedLicense;
using DVLD___Driving_License_Management.LocalDrivingLicenseApplications;
using DVLD___Driving_License_Management.People;
using DVLD___Driving_License_Management.TestTypes;
using DVLD___Driving_License_Management.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinApp = System.Windows.Forms.Application;

namespace DVLD___Driving_License_Management
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            WinApp.EnableVisualStyles();
            WinApp.SetCompatibleTextRenderingDefault(false);

            // Main Forms
            WinApp.Run(new frmLogin());
            //WinApp.Run(new TestForm());
            //WinApp.Run(new frmMain());
            //------------------------------------------------------


            // People
            //WinApp.Run(new frmListPeople());
            //WinApp.Run(new frmFindPerson());
            //WinApp.Run(new frmAddEditiPersonInfo(1028));
            //WinApp.Run(new frmAddEditiPersonInfo(1031));
            //WinApp.Run(new People.frmFindPerson());
            //------------------------------------------------------


            // Users
            //WinApp.Run(new frmListUsers());
            //WinApp.Run(new frmAddEditUserInfo());
            //------------------------------------------------------


            // Drivers
            //WinApp.Run(new Drivers.frmListDrivers());
            //------------------------------------------------------


            // ApplicationTypes
            //WinApp.Run(new frmListApplicationTypes());
            //------------------------------------------------------


            // TestTypes
            //WinApp.Run(new frmListTestTypes());
            //------------------------------------------------------


            // Local Driving License Applications
            //WinApp.Run(new frmListLocalDrivingLicenseApplications());
            //WinApp.Run(new frmAddEditLocalDrivingLicenseApplication());
            //------------------------------------------------------


            // Licenses
            //WinApp.Run(new frmDetainedLicense());
            //WinApp.Run(new frmReleaseDetainedLicense());
            //WinApp.Run(new frmListDetainedLicenses());
            //------------------------------------------------------

            // International Driving License Applications
            //WinApp.Run(new frmIssueInternationalLicense());
            //WinApp.Run(new frmShowInternationalLicenseInfo(20));
            //WinApp.Run(new frmListInternationalLicensesApplications());
            //------------------------------------------------------


        }
    }
}
