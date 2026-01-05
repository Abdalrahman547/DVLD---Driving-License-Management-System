using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BussinessLayer
{
    public class clsTest
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode;

        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public clsTestAppointment TestAppointmentInfo { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }


        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = string.Empty;
            this.CreatedByUserID = -1;
            TestAppointmentInfo = null;
            Mode = enMode.AddNew;
        }

        public clsTest(int testID, int testAppointmentID, bool testResult, string notes, int createdByUserID) 
        {
            this.TestID = testID;
            this.TestAppointmentID = testAppointmentID;
            TestAppointmentInfo = clsTestAppointment.Find(testAppointmentID);
            this.TestResult = testResult;
            this.Notes = notes;
            this.CreatedByUserID = createdByUserID;
           
            Mode = enMode.Update;
        }

        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = -1;

            if (clsTestData.GetByTestInfoID(TestID, ref TestAppointmentID,
                                           ref TestResult, ref Notes,
                                           ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            else
                return null;
        }

        // Find Last Test for Person + TestType + LicenseClass
        public static clsTest GetLastTest(int PersonID, int TestTypeID, int LicenseClassID)
        {
            int TestID = -1;
            int TestAppointmentID = -1;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = -1;

            if (clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass(
                PersonID, TestTypeID, LicenseClassID,
                ref TestID, ref TestAppointmentID, ref TestResult,
                ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateTest();

                default:
                    return false;
            }
        }

        private bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID, this.TestResult,
                                                this.Notes, this.CreatedByUserID);

            return (this.TestID != -1);
        }

        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(this.TestID, this.TestResult,
                                          this.Notes, this.CreatedByUserID);
        }
  
        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();
        }

        public static byte GetNumOfPassedTests(int LDLAppID)
        {
            return clsTestData.GetNumOfPassedTests(LDLAppID);
        }

        public static bool IsPassedAllTests(int LDLAppID)
        {
            return (GetNumOfPassedTests(LDLAppID) == 3);
        }

    }
}
