using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD___BussinessLayer
{
    public class clsDetainedLicense
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public clsUser ReleasedByUserInfo { get; set; }
        public int ReleaseApplicationID { get; set; }

        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;
            this.CreatedByUserInfo = null;
            this.ReleasedByUserInfo = null;
            this.FineFees = 0;

        }

        private clsDetainedLicense(
            int detainID, int licenseID, DateTime detainDate,
            decimal fineFees, int createdByUserID,
            bool isReleased, DateTime releaseDate,
            int releasedByUserID, int releaseApplicationID)
        {
            this.DetainID = detainID;
            this.LicenseID = licenseID;
            this.DetainDate = detainDate;
            this.FineFees = fineFees;
            this.CreatedByUserID = createdByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(createdByUserID);
            this.IsReleased = isReleased;
            this.ReleaseDate = releaseDate;
            this.ReleasedByUserID = releasedByUserID;
            this.ReleasedByUserInfo = clsUser.FindByUserID(releasedByUserID);
            this.ReleaseApplicationID = releaseApplicationID;
        }

        public static clsDetainedLicense FindByDetainID(int detainID)
        {
            int licenseID = 0, createdByUserID = 0;
            DateTime detainDate = DateTime.Now;
            decimal fineFees = 0;
            bool isReleased = false;
            DateTime releaseDate = DateTime.MinValue;
            int releasedByUserID = -1;
            int releaseApplicationID = -1;

            bool found = clsDetainedLicenseData.GetDetainedLicenseInfoByID(
                detainID, ref licenseID, ref detainDate,
                ref fineFees, ref createdByUserID, ref isReleased,
                ref releaseDate, ref releasedByUserID, ref releaseApplicationID);

            if (!found)
                return null;

            return new clsDetainedLicense(detainID, licenseID, detainDate,
                fineFees, createdByUserID, isReleased, (DateTime)releaseDate,
                (int)releasedByUserID, (int)releaseApplicationID);
        }

        public static clsDetainedLicense FindByLicenseID(int licenseID)
        {
            int detainID = 0, createdByUserID = -1;
            DateTime detainDate = DateTime.Now;
            decimal fineFees = 0;
            bool isReleased = false;
            DateTime releaseDate = DateTime.MaxValue;
            int releasedByUserID = -1;
            int releaseApplicationID = -1;

            bool found = clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(
                licenseID, ref detainID, ref detainDate,
                ref fineFees, ref createdByUserID, ref isReleased,
                ref releaseDate, ref releasedByUserID, ref releaseApplicationID);

            if (!found)
                return null;

            return new clsDetainedLicense(detainID, licenseID, detainDate,
                fineFees, createdByUserID, isReleased, (DateTime)releaseDate,
                (int)releasedByUserID, (int)releaseApplicationID);
        }

        public static bool IsLicenseDetained(int licenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(licenseID);
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }

        public bool Save()
        {
            // Only insert (no update for detained record)
            if (this.DetainID == -1)
            {
                this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense(
                    this.LicenseID, this.DetainDate,
                    this.FineFees, this.CreatedByUserID);

                return (this.DetainID != -1);
            }

            return true; // nothing to update
        }

        public bool Release(int releasedByUserID)
        {
            if (this.IsReleased)
                return true;

            // Create release application
            clsApplication releaseApplication = new clsApplication();
            
            int DriverID = clsLicense.Find(this.LicenseID).DriverID;
            releaseApplication.ApplicantPersonID = clsDriver.FindByDriverID(DriverID).PersonID;
            
            releaseApplication.ApplicationDate = DateTime.Now;
            
            releaseApplication.ApplicationTypeID = (int) clsApplication.enApplicationType.ReleaseDetainedLicense;
            
            releaseApplication.Status = clsApplication.enApplicationStatus.Completed;
           
            releaseApplication.LastStatusDate = DateTime.Now;

            releaseApplication.Fees = clsApplicationType.Find(releaseApplication.ApplicationTypeID).ApplicationFees;

            releaseApplication.CreatedByUserID = releasedByUserID;


            if(!releaseApplication.Save())
                return false;
            // End create release application


            this.ReleaseDate = DateTime.Now;
            this.ReleasedByUserID = releasedByUserID;
            this.ReleaseApplicationID = releaseApplication.ApplicationID;
            this.IsReleased = true;

            return clsDetainedLicenseData.ReleaseDetainedLicense(
                this.DetainID,
                this.ReleaseDate,
                (int)this.ReleasedByUserID,
                (int)this.ReleaseApplicationID
            );

        }

    }
}
