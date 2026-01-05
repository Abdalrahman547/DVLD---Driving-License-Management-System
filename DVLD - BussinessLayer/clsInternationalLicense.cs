using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD___BussinessLayer.clsApplication;

namespace DVLD___BussinessLayer
{
    public class clsInternationalLicense: clsApplication
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;

        public clsDriver DriverInfo;
        public int InternationalLicenseID { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public clsInternationalLicense()
        {
            InternationalLicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            IssuedUsingLocalLicenseID = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            IsActive = true;
            CreatedByUserID = -1;
            DriverInfo = null;

            Mode = enMode.AddNew;
        }

        private clsInternationalLicense(
            int ApplicationID, int ApplicantPersonID ,DateTime ApplicationDate,
            enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
            float PaidFees, int CreatedByUserID,
            int internationalLicenseID, int driverID,
            int issuedUsingLocalLicenseID, DateTime issueDate, DateTime expirationDate,
            bool isActive)
            : base(
                ApplicationID,
                ApplicantPersonID,
                ApplicationDate,
                (int) enApplicationType.NewInternationalLicense,
                ApplicationStatus,
                LastStatusDate,
                PaidFees,
                CreatedByUserID
                 )
        {

            this.InternationalLicenseID = internationalLicenseID;
            this.DriverID = driverID;
            this.IssuedUsingLocalLicenseID = issuedUsingLocalLicenseID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.IsActive = isActive;

            DriverInfo = clsDriver.FindByDriverID(this.DriverID);

            Mode = enMode.Update;
        }

        private bool _AddNewInternationalLicense()
        {
            InternationalLicenseID = clsInternationalLicenseDate.AddNewInternationalLicense(
                ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                IssueDate, ExpirationDate, CreatedByUserID);

            return (InternationalLicenseID != -1);
        }

        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseDate.UpdateInternationalLicense(
                InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                IssueDate, ExpirationDate, IsActive, CreatedByUserID);
        }

        public bool Save()
        {
            // Save the base application first
            base.Mode = (clsApplication.enMode)Mode;
            if(!base.Save())
                return false;


            // Then save the international license details
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateInternationalLicense();

                default:
                    return false;
            }
        }

        public static clsInternationalLicense FindByInternationalID(int InternationalLicenseID)
        {
            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = false;

            if (clsInternationalLicenseDate.GetInternationalLicenseInfoByInternationalID(InternationalLicenseID,
                ref ApplicationID, ref DriverID, ref IssuedUsingLocalLicenseID,
                ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                // Find Base Class Info
                clsApplication Application = clsApplication.Find(ApplicationID);

                if (Application == null) 
                    return null;

                return new clsInternationalLicense( ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                         Application.Status, Application.LastStatusDate,
                         (float)Application.Fees,  Application.CreatedByUserID,
                         InternationalLicenseID,  DriverID,
                         IssuedUsingLocalLicenseID,  IssueDate,  ExpirationDate,
                         IsActive);
            }
            else
                return null;
        }

        public static clsInternationalLicense FindByLDLID(int LDLID)
        {
            int ApplicationID = -1, DriverID = -1, InternationalLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = false;

            if (clsInternationalLicenseDate.GetInternationalLicenseInfoByLDLID(LDLID,
                ref InternationalLicenseID, ref ApplicationID, ref DriverID,
                ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                // Find Base Class Info
                clsApplication Application = clsApplication.Find(ApplicationID);

                if (Application == null)
                    return null;

                return new clsInternationalLicense(ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                         Application.Status, Application.LastStatusDate,
                         (float)Application.Fees, Application.CreatedByUserID,
                         InternationalLicenseID, DriverID,
                         LDLID, IssueDate, ExpirationDate,
                         IsActive);
            }
            else
                return null;
        }
        
        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseDate.GetDriverInternationalLicenses(DriverID);
        }
        
        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseDate.GetAllInternationalLicenses();
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseDate.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        public static bool IsActiveInternationalLicenseExists(int DriverID)
        {
            return GetActiveInternationalLicenseIDByDriverID(DriverID) != -1;
        }

        public static bool DeactivateInternationalLicense(int InternationalLicenseID)
        {
             return clsInternationalLicenseDate.DeactivateInternationalLicense(InternationalLicenseID);
        }

    }
}
