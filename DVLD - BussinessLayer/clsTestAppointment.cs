using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BussinessLayer
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; } = enMode.AddNew;


        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LDLAppID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public double Fees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int RetakeTestApplicationID { get; set; }
        public clsApplication RetakeTestAppInfo { get; set; }


        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = -1;
            this.LDLAppID = -1;
            this.AppointmentDate = DateTime.Now;
            this.Fees = 0;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.RetakeTestApplicationID = -1;

            Mode = enMode.AddNew;
        }

        private clsTestAppointment(int testAppointmentID, int testTypeID, int ldlAppID,
                                  DateTime appointmentDate, double fees, int createdByUserID,
                                  bool isLocked, int retakeTestApplicationID)
        {
            TestAppointmentID = testAppointmentID;
            TestTypeID = testTypeID;
            LDLAppID = ldlAppID;
            AppointmentDate = appointmentDate;
            Fees = fees;
            CreatedByUserID = createdByUserID;
            IsLocked = isLocked;
            RetakeTestApplicationID = retakeTestApplicationID;
            RetakeTestAppInfo = clsApplication.Find(retakeTestApplicationID);

            Mode = enMode.Update;
        }

        public int GetTestID()
        {
            return clsTestAppointmentData.GetTestID(TestAppointmentID);
        }

       
        public static clsTestAppointment Find(int testAppointmentID)
        {
            int testTypeID = 0, ldlAppID = 0, createdByUserID = 0, retakeTestApplicationID = 0;
            DateTime appointmentDate = DateTime.MinValue;
            double fees = 0;
            bool isLocked = false;

            bool isFound = clsTestAppointmentData.GetTestAppointmentByID(
                testAppointmentID,
                ref testTypeID,
                ref ldlAppID,
                ref appointmentDate,
                ref fees,
                ref createdByUserID,
                ref isLocked,
                ref retakeTestApplicationID
            );

            if (isFound)
            {
                return new clsTestAppointment(
                    testAppointmentID, testTypeID, ldlAppID, appointmentDate,
                    fees, createdByUserID, isLocked, retakeTestApplicationID
                );
            }
            else
            {
                return null;
            }
        }

        public static clsTestAppointment GetLastAppointment(int ldlApplication, int testTypeID)
        {
            int testAppointmentID = 0, createdByUserID = 0, retakeTestApplicationID = 0;
            DateTime appointmentDate = DateTime.MinValue;
            double fees = 0;
            bool isLocked = false;

            bool isFound = clsTestAppointmentData.GetLastTestAppointment(
                ldlApplication, testTypeID,
                ref testAppointmentID,
                ref appointmentDate,
                ref fees,
                ref createdByUserID,
                ref isLocked,
                ref retakeTestApplicationID
            );

            if (isFound)
            {
                return new clsTestAppointment(
                    testAppointmentID, testTypeID, ldlApplication,
                    appointmentDate, fees, createdByUserID, isLocked,
                    retakeTestApplicationID
                );
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();
        }

        public static DataTable GetApplicationTestAppointmentPerTestID(int ldlApplication, int testTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentPerTestID(ldlApplication, testTypeID);
        }


        private bool _AddNewTestAppointment()
        {
            TestAppointmentID = clsTestAppointmentData.AddTestAppointment(
                                this.TestTypeID,
                                this.LDLAppID,
                                this.AppointmentDate,
                                this.Fees,
                                this.CreatedByUserID,
                                this.IsLocked,
                                this.RetakeTestApplicationID );

            return TestAppointmentID != -1;

        }

        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentData.UpdateTestAppointment(
                this.TestAppointmentID,
                this.TestTypeID,
                this.LDLAppID,
                this.AppointmentDate,
                this.Fees,
                this.CreatedByUserID,
                this.IsLocked,
                this.RetakeTestApplicationID
            );
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateTestAppointment();

                default:
                    return false;
            }
        }

    }

}
