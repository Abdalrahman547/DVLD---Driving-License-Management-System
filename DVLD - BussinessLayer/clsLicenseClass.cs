using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BussinessLayer
{
    public class clsLicenseClass
    {

        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public double ClassFees { get; set; } // 👈 double

        public clsLicenseClass()
        {
            LicenseClassID = -1;
            ClassName = string.Empty;
            ClassDescription = string.Empty;
            MinimumAllowedAge = 0;
            DefaultValidityLength = 0;
            ClassFees = 0.0;
        }

        private clsLicenseClass(int LicenseClassID, string ClassName, string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, double ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
        }

        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = string.Empty;
            string ClassDescription = string.Empty;
            byte MinimumAllowedAge = 0;
            byte DefaultValidityLength = 0;
            double ClassFees = 0.0;

            if (clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref ClassName,
                ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }

        public static clsLicenseClass Find(string ClassName)
        {
            int LicenseClassID = -1;
            string ClassDescription = string.Empty;
            byte MinimumAllowedAge = 0;
            byte DefaultValidityLength = 0;
            double ClassFees = 0.0;

            if (clsLicenseClassData.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID, 
                ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }

        static public DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }

    }
}
