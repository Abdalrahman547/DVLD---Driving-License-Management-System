using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BussinessLayer
{
    public class clsApplicationType
    {

        public int ApplicationID { get; set; }

        public string ApplicationTitle { get; set; }

        public double ApplicationFees { get; set; }

        clsApplicationType()
        {
            ApplicationID = -1;
            ApplicationTitle = string.Empty;
            ApplicationFees = 0;
        }
        
        clsApplicationType(int ID, string Title, double Fees)
        {
            ApplicationID = ID;
            ApplicationTitle = Title;
            ApplicationFees = Fees;
        }

        public static clsApplicationType Find(int ID)
        {
            string Title = string.Empty;
            double Fees = 0;

            if (clsApplocationTypesData.GetApplicationTypeByID(ID, ref Title, ref Fees))
                return new clsApplicationType(ID, Title, Fees);

            return null;
        }
        
        public static DataTable GetAllApplications()
        {
            return clsApplocationTypesData.GetAllApplicationTypes();
        }

        public static bool UpdateApplication(int ID, string Title, double Fees)
        {
            return clsApplocationTypesData.UpdateApplicationType(ID, Title, Fees);
        }

    }
}
