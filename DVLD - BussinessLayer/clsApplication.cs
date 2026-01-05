using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BussinessLayer
{
    public class clsApplication
    {
        public enum enMode { AddNew, Update }
        public enum enApplicationType
        {
            NewLocalLicense = 1, RenewLicense = 2, ReplaceLostLicense = 3, ReplaceDamagedLicense = 4,
            ReleaseDetainedLicense = 5, NewInternationalLicense = 6, RetakeTest = 7
        }

        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 }


        public enMode Mode = enMode.AddNew;
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public clsPerson ApplicantPersonInfo { get; set; }
        public string ApplicantFullName
        {
            get
            {
                return clsPerson.Find(ApplicantPersonID).FullName;
            }
        }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public clsApplicationType ApplicationTypeInfo { get; set; }
        public enApplicationStatus Status { get; set; }
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }
        }
        public DateTime LastStatusDate { get; set; }
        public double Fees { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { get; set; }

        // Default constructor
        public clsApplication()
        {
            ApplicationID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = -1;
            Status = enApplicationStatus.New;
            LastStatusDate = DateTime.Now;
            Fees = 0;
            CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        // Full constructor
        public clsApplication(int appID, int personID, DateTime appDate, int appTypeID,
                              enApplicationStatus status, DateTime lastStatusDate, double fees, int userID)
        {
            this.ApplicationID = appID;
            this.ApplicantPersonID = personID;
            this.ApplicantPersonInfo = clsPerson.Find(personID);
            this.ApplicationDate = appDate;
            this.ApplicationTypeID = appTypeID;
            this.ApplicationTypeInfo = clsApplicationType.Find(appTypeID);
            this.Status = status;
            this.LastStatusDate = lastStatusDate;
            this.Fees = fees;
            this.CreatedByUserID = userID;
            this.CreatedByUserInfo = clsUser.FindByUserID(userID);

            Mode = enMode.Update;
        }

        private bool _AddNewApplication()
        {
            ApplicationID = clsApplicationData.AddNewApplication(ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                                                                 (short)Status, LastStatusDate, Fees, CreatedByUserID);
            return (ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                                                        (short)Status, LastStatusDate, Fees, CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateApplication();
            }
            return false;
        }

        public static clsApplication Find(int ApplicationID)
        {
            int personID = -1, appTypeID = -1, userID = -1;
            DateTime appDate = DateTime.Now, lastStatusDate = DateTime.Now;
            short status = 0;
            double fees = 0;

            if (clsApplicationData.GetApplicationInfoByID(ApplicationID, ref personID, ref appDate,
                                                          ref appTypeID, ref status,
                                                          ref lastStatusDate, ref fees, ref userID))
            {
                return new clsApplication(ApplicationID, personID, appDate, appTypeID, (enApplicationStatus)status, lastStatusDate, fees, userID);
            }
            else
                return null;
        }

        public static DataTable GetAllApplications()
        {
            return clsApplicationData.GetAllLocalApplications();
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            return clsApplicationData.DeleteApplication(ApplicationID);
        }

        public static bool IsApplicationExists(int AppID)
        {
            return clsApplicationData.IsApplicationExsits(AppID);
        }

        public static int GetActiveApplicationID(int PersonID, int AppTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID, AppTypeID);
        }

        static public int GetActiveApplicationIDForLicenseClass(int PersonID, enApplicationType AppType, int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, (int)AppType, LicenseClassID);
        }
        
        public static bool DoesPersonHaveActiveApplication(int PersonID, int AppTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(PersonID, AppTypeID);
        }

        public static bool UpdateStatus(int ApplicationID, byte Status)
        {
            return clsApplicationData.UpdateStatus(ApplicationID, Status);
        }

        public bool Cancel()
        {
            return clsApplicationData.UpdateStatus(ApplicationID, (short)enApplicationStatus.Cancelled);
        }

        public static bool Cancel(int LDLApplicationID)
        {
            int ApplicationID = clsLocalDrivingLicenseApplication.FindByLDLApplicationID(LDLApplicationID).ApplicationID;

            return clsApplicationData.UpdateStatus(ApplicationID, (short)enApplicationStatus.Cancelled);
        }

        public bool SetComplete()
        {
            return clsApplicationData.UpdateStatus(ApplicationID, (short)enApplicationStatus.Completed);
        }


    }

}
