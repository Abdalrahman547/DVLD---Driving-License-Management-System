using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BussinessLayer
{
    public class clsDriver
    {

        public enum enMode { AddNew, Update }

        public enMode Mode { get; set; }

        public clsPerson PersonInfo;
       
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;

            PersonInfo = null;

            Mode = enMode.AddNew;
        }

        public clsDriver(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            this.DriverID = driverID;
            this.PersonID = personID;
            this.CreatedByUserID = createdByUserID;
            this.CreatedDate = createdDate;

            this.PersonInfo = clsPerson.Find(this.PersonID);

            Mode = enMode.Update;
        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            int personID = -1;
            int createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByID(DriverID, ref personID, ref createdByUserID, ref createdDate))
            {
                return new clsDriver(DriverID, personID, createdByUserID, createdDate);
            }
            else
                return null;
        }

        public static clsDriver FindByPersonID(int PersonID)
        {
            int DriverID = -1;
            int createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByPersonID(PersonID, ref DriverID, ref createdByUserID, ref createdDate))
            {
                return new clsDriver(DriverID, PersonID, createdByUserID, createdDate);
            }
            else
                return null;
        }
       
        private bool _AddNewDriver()
        {
            this.DriverID = clsDriverData.AddNewDriver(this.PersonID, this.CreatedByUserID);
            return (this.DriverID != -1);
        }

        private bool _UpdateDriver()
        {
            return clsDriverData.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateDriver();

                default:
                    return false;
            }
        }

        public static bool Delete(int DriverID)
        {
            return clsDriverData.Delete(DriverID);
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();
        }

        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicense.GetLicensesByDriverID(DriverID);
        }

        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        }

        public static int IsPersonADriver(int personID)
        {
            return clsDriverData.IsDriverExistsByPersonID(personID);
        }

        public static DataTable GetLocalLicenseHistory(int DriverID)
        {
            return clsDriverData.GetDriverLocalLicensesHistory(DriverID);
        }
    }
}
