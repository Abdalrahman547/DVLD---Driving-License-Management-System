using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BussinessLayer
{
    public class clsTestType
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }

        public enTestType TestType { get ; }
        public int TestTypeID { get; set; }

        public string TestTypeTitle { get; set; }

        public string TestTypeDescription { get; set; }

        public double TestTypeFees { get; set; }



        clsTestType()
        {
            TestTypeID = -1;
            TestTypeTitle = string.Empty;
            TestTypeDescription = string.Empty;
            TestTypeFees = 0;
        }

        clsTestType(int ID, string Title, string Description, double Fees)
        {
            TestTypeID = ID;
            TestTypeTitle = Title;
            TestTypeDescription = Description;
            TestTypeFees = Fees;

            TestType = (enTestType)ID;
        }

        public static clsTestType Find(int ID)
        {
            string Title = string.Empty, Description = string.Empty;
            double Fees = 0;

            if (clsTestTypesData.GetTestTypeByID(ID, ref Title, ref Description, ref Fees))
                return new clsTestType(ID, Title, Description, Fees);

            return null;
        }

        public static DataTable GetAllTests()
        {
            return clsTestTypesData.GetAllTestTypes();
        }

        public static bool UpdateTest(int ID, string Title, string Description, double Fees)
        {
            return clsTestTypesData.UpdateTestType(ID, Title, Description, Fees);
        }
    }
}
