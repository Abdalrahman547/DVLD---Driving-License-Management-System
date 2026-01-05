using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD___BussinessLayer.clsLicense;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD___BussinessLayer
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;
        
        public int LDLApplicationID {get; set; }
        public int LicenseClassID { get; set; }

        clsLicenseClass LicenseClassInfo { get; set; }

        public string FullName
        {
            get
            {
                return base.ApplicantPersonInfo.FullName; // From base class
                
                //return clsPerson.Find(ApplicantPersonID).FullName;
            }
        }
        public clsLocalDrivingLicenseApplication()
        {
            this.LDLApplicationID = -1;
            this.LicenseClassID = -1;
            
            Mode = enMode.AddNew;
        }

        public clsLocalDrivingLicenseApplication(int LDLApplicationID, int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate,
            int ApplicationTypeID, enApplicationStatus Status, DateTime LastStatusDate,
            double Fees, int CreatedByUserID, int LicenseClassID)
        {
            this.LDLApplicationID = LDLApplicationID;
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicantPersonInfo = clsPerson.Find(ApplicantPersonID);
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.Status = Status;
            this.LastStatusDate = LastStatusDate;
            this.Fees = Fees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);

            Mode = enMode.Update;
        }


        static public DataTable GetAllLDLApplications()
        {
            return clsLocalDrivingLicenseApplicationsData.GetAllLDLApplications();
        }

        private bool _AddNewLDLApplication()
        {
            
            this.LDLApplicationID = clsLocalDrivingLicenseApplicationsData.AddNewLDLApplication(this.ApplicationID, this.LicenseClassID);

            return (this.LDLApplicationID > 0);

        }

        private bool _UpdateLDLApplication()
        {
            return clsLocalDrivingLicenseApplicationsData.UpdateLDLApplication(this.LDLApplicationID, this.ApplicationID, this.LicenseClassID);
        }

        public bool Save()
        {
            // First we call Save from Base Class, to adding all information to the Application Table

            // Make Base class Mode = this.Mode
            base.Mode = (clsApplication.enMode) this.Mode;


            if (!base.Save())
                return false;

            // After We inserted all information to the Application Table Successfully
            // Insert Information to LDLAppApplication Table
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLDLApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return (_UpdateLDLApplication());
            
            }
            
            return false;
        }


        public bool Delete()
        {
            if (!clsApplication.DeleteApplication(ApplicationID))
                return false;

            return clsLocalDrivingLicenseApplicationsData.DeleteLDLApplication(LDLApplicationID);
        }

        public static clsLocalDrivingLicenseApplication FindByLDLApplicationID(int LDLApplicationID)
        {
            int ApplicationID = -1;
            int LicenceClassID = -1;

            bool IsFound = clsLocalDrivingLicenseApplicationsData.GetLDLApplicationInfoByID(LDLApplicationID, ref ApplicationID, ref LicenceClassID);
                
            if(IsFound)
            {
                // Find Base Class Info
                clsApplication Application = clsApplication.Find(ApplicationID);
                if (Application == null)
                    return null;

                // Now we have all data to Create clsLocalDrivingLicenseApplication object
                return new clsLocalDrivingLicenseApplication(LDLApplicationID, ApplicationID, Application.ApplicantPersonID,
                    Application.ApplicationDate, Application.ApplicationTypeID, Application.Status, Application.LastStatusDate,
                    Application.Fees, Application.CreatedByUserID, LicenceClassID);
            }
            return null;
        }

        public static clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {
            int LDLApplicatonID = -1;
            int LicenceClassID = -1;

            bool IsFound = clsLocalDrivingLicenseApplicationsData.GetLDLApplicationInfoByApplicationID(ApplicationID, ref LDLApplicatonID, ref LicenceClassID);


            if (IsFound)
            {
                // Find Base Class Info
                clsApplication Application = clsApplication.Find(ApplicationID);

                // Now we have all data to Create clsLocalDrivingLicenseApplication object
                return new clsLocalDrivingLicenseApplication(LDLApplicatonID, ApplicationID, Application.ApplicantPersonID,
                    Application.ApplicationDate, Application.ApplicationTypeID, Application.Status, Application.LastStatusDate,
                    Application.Fees, Application.CreatedByUserID, LicenceClassID);
            }
            return null;
        }

        public static byte GetNumOfPassedTests(int LDLAppID)
        {
            return clsLocalDrivingLicenseApplicationsData.GetNumOfPassedTests(LDLAppID);
        }

        public int GetActiveLicenseID()
        {
            return clsLocalDrivingLicenseApplicationsData.GetActiveLicenseID(ApplicantPersonID, LicenseClassID);
        }
       
        public short TotalTrialsPerTest(int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsData.TotalTrialsPerTest(LDLApplicationID, TestTypeID);
        }

        public bool DoesAttendTestType(int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsData.DoesAttendTestType(LDLApplicationID, TestTypeID);
        }

        public bool IsThereAnActiveScheduledTest(int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsData.IsThereAnActiveScheduledTest(LDLApplicationID, TestTypeID);
        }

        public bool DoesPassedTestType(int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsData.DoesPassTestType(LDLApplicationID, TestTypeID);
        }

        public clsTest GetLastTestperTestType(int TestTypeID)
        {
            int TestID = -1, CreatedByUserID = -1, TestAppointmentID = -1;
            bool TestResult = false;
            string Notes = "";

            if (clsLocalDrivingLicenseApplicationsData.GetLastTestperTestType(
                LDLApplicationID, TestTypeID,
                ref TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
                
            }
            return null;
        }

        public int IssueLicenseForFirstTime(string Notes, int CreatedByUserID)
        {
            int DriverID = clsDriver.IsPersonADriver(ApplicantPersonID);

            if (DriverID == -1)
            {
                clsDriver NewDriver = new clsDriver();

                NewDriver.PersonID = ApplicantPersonID;
                NewDriver.CreatedByUserID = CreatedByUserID;

                if (!NewDriver.Save())
                {
                    MessageBox.Show("Failed to create driver record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                DriverID = NewDriver.DriverID;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = ApplicationID;
            NewLicense.DriverID = DriverID;
            NewLicense.LicenseClass = LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(LicenseClassInfo.DefaultValidityLength);

            NewLicense.Notes = Notes;
            NewLicense.PaidFees = LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = enIssueReason.FirstTime;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (NewLicense.Save())
            {
                Status = clsApplication.enApplicationStatus.Completed;
                LastStatusDate = DateTime.Now;

                this.SetComplete(); // Update Application Status

                return NewLicense.LicenseID;
            }
            return -1;
        }

        public bool DoesPassedAllTests()
        {
            return clsTest.IsPassedAllTests(LDLApplicationID);
        }

    }
}
