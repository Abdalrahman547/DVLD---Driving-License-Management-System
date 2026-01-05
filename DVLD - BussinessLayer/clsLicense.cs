using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___BussinessLayer
{
    public class clsLicense
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;

        public enum enIssueReason
        {
            FirstTime = 1,
            Renew = 2,
            DamagedReplacement = 3,
            LostReplacement = 4
        }
        public enIssueReason IssueReason {get; set; }

        public clsDriver DriverInfo;
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClass { get; set; }

        public clsLicenseClass LicenseClassInfo;
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public double PaidFees { get; set; }
        public bool IsActive { get; set; }

        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(IssueReason);
            }
        }
        public int CreatedByUserID { get; set; }
        public bool IsDetained {get; set; }
        public clsDetainedLicense DetainedInfo {get; set; }


        public clsLicense()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClass = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = string.Empty;
            PaidFees = 0;
            IsActive = true;
            CreatedByUserID = -1;
            IssueReason = enIssueReason.FirstTime;
            DriverInfo = null;
            IsDetained = false;

            Mode = enMode.AddNew;
        }

        private clsLicense(int licenseID, int applicationID, int driverID, int licenseClass,
            DateTime issueDate, DateTime expirationDate, string notes, double paidFees,
            bool isActive, enIssueReason issueReason, int createdByUserID)
        {
            LicenseID = licenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            LicenseClass = licenseClass;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = issueReason;
            CreatedByUserID = createdByUserID;

            DriverInfo = clsDriver.FindByDriverID(DriverID);
            LicenseClassInfo = clsLicenseClass.Find(LicenseClass);
            IsDetained = clsDetainedLicense.IsLicenseDetained(licenseID);
            DetainedInfo = IsDetained ? clsDetainedLicense.FindByLicenseID(LicenseID) : null;


            Mode = enMode.Update;
        }

        private bool _AddNewLicense()
        {
            LicenseID = clsLicenseData.AddNewLicense(ApplicationID, DriverID, LicenseClass,
                IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (byte) IssueReason, CreatedByUserID);

            return (LicenseID != -1);
        }

        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(LicenseID, ApplicationID, DriverID, LicenseClass,
                IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (byte) IssueReason, CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateLicense();

                default:
                    return false;
            }
        }

        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClass = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = string.Empty;
            double PaidFees = 0;
            bool IsActive = false;
            byte IssueReason = 0;

            if (clsLicenseData.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID,
                ref LicenseClass, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees,
                ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass,
                    IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (enIssueReason) IssueReason, CreatedByUserID);
            }
            else
                return null;
        }

        public static bool Delete(int LicenseID)
        {
            return clsLicenseData.DeleteLicense(LicenseID);
        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }

        public static bool IsLicenseExists(int LicenseID)
        {
            return clsLicenseData.IsLicenseExists(LicenseID);
        }

        public static bool IsLicenseExistsByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.IsLicenseExistsByPersonID(PersonID, LicenseClassID);
        }

        public static DataTable GetLicensesByDriverID(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }

        public static int GetActiveLicensesByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }

        public  bool DeactivateCurrentLicense()
        {
            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }   

        private static string GetIssueReasonText(enIssueReason IssueReason)
        {
            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";

                case enIssueReason.Renew:
                    return "Renew";

                case enIssueReason.DamagedReplacement:
                    return "Damaged Replacement";

                case enIssueReason.LostReplacement:
                    return "Lost Replacement";

                default:
                    return "Unknown";
            }
        }

        public bool IsLicenceExpired()
        {
            return (DateTime.Now > ExpirationDate);
        }


        public int Detain(decimal FineFees, int CreatedByUserID)
        {
            clsDetainedLicense NewDetain = new clsDetainedLicense();
            NewDetain.LicenseID = this.LicenseID;
            NewDetain.DetainDate = DateTime.Now;
            NewDetain.FineFees = FineFees;
            NewDetain.CreatedByUserID = CreatedByUserID;

            
            if (!NewDetain.Save())
            {
                return -1;
            }
            this.IsDetained = true;
            this.DetainedInfo = NewDetain;
            
            return NewDetain.DetainID;
        }

        public bool ReleaseFromDetain(int CreatedByUserID)
        {
            if (this.DetainedInfo == null || !this.IsDetained)
            {
                MessageBox.Show("This License is not detained.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!this.DetainedInfo.Release(CreatedByUserID))
            {
                MessageBox.Show("Failed to release this License from detain.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            this.IsDetained = false;
            
            return true;
        }

        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {
            clsApplication NewApp = new clsApplication();

            NewApp.ApplicantPersonID = this.DriverInfo.PersonID;
            NewApp.ApplicationTypeID = (int) clsApplication.enApplicationType.RenewLicense;
            NewApp.ApplicationDate = DateTime.Now;
            NewApp.Status = clsApplication.enApplicationStatus.Completed;
            NewApp.LastStatusDate = DateTime.Now;
            NewApp.CreatedByUserID = CreatedByUserID;
            NewApp.Fees = clsApplicationType.Find(NewApp.ApplicationTypeID).ApplicationFees;

            if(!NewApp.Save())
            {
                MessageBox.Show("Failed to create new application for license renewal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = NewApp.ApplicationID; // Update with new Application ID
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now; // Set Issue Date to Today

            int DefaultValidityYears = this.LicenseClassInfo.DefaultValidityLength;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityYears); // Set Expiration Date based on License Class Default Validity Length

            NewLicense.Notes = Notes; // Set Notes
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees; // Set Paid Fees based on License Class Issue Fees
            NewLicense.IsActive = true;
            NewLicense.IssueReason = enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;
            
            if (!NewLicense.Save())
            {
                MessageBox.Show("Failed to Renew this License.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            DeactivateCurrentLicense(); // Deactivate Current License

            return NewLicense;

        }

        public clsLicense Replace(enIssueReason issueReason, int CreatedByUserID)
        {
            clsApplication NewApp = new clsApplication();

            NewApp.ApplicantPersonID = this.DriverInfo.PersonID;

            NewApp.ApplicationTypeID = (issueReason == enIssueReason.DamagedReplacement)
                ? (int) clsApplication.enApplicationType.ReplaceDamagedLicense
                : (int)clsApplication.enApplicationType.ReplaceLostLicense;

            NewApp.ApplicationDate = DateTime.Now;
            NewApp.Status = clsApplication.enApplicationStatus.Completed;
            NewApp.LastStatusDate = DateTime.Now;
            NewApp.CreatedByUserID = CreatedByUserID;
            NewApp.Fees = clsApplicationType.Find(NewApp.ApplicationTypeID).ApplicationFees;

            if (!NewApp.Save())
            {
                MessageBox.Show("Failed to create new application for license renewal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = NewApp.ApplicationID; // Update with new Application ID
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now; // Set Issue Date to Today

            int DefaultValidityYears = this.LicenseClassInfo.DefaultValidityLength;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityYears); // Set Expiration Date based on License Class Default Validity Length

            NewLicense.Notes = Notes; // Set Notes
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees; // Set Paid Fees based on License Class Issue Fees
            NewLicense.IsActive = true;
            NewLicense.IssueReason = issueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save())
            {
                MessageBox.Show("Failed to Renew this License.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            DeactivateCurrentLicense(); // Deactivate Old License

            return NewLicense;
        }

    }
}
