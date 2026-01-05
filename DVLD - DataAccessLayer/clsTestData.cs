using DVLD___BussinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___DataAccessLayer
{
    public class clsTestData
    {
        public static bool GetByTestInfoID(int TestID,
                            ref int TestAppointmentID,
                            ref bool TestResult,
                            ref string Notes,
                            ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT * FROM Tests
                             WHERE TestID = @TestID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestID", TestID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            isFound = true;

                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)(reader["TestResult"]);
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "";
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {

                        isFound = false;
                    }
                }
            }

            return isFound;
        }


        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass
                            (int PersonID,
                            int TestTypeID,
                            int LicenseClassID,
                            ref int TestID,
                            ref int TestAppointmentID,
                            ref bool TestResult,
                            ref string Notes,
                            ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT TOP 1 Tests.*      
                            FROM  TestAppointments 
                            INNER JOIN Tests
                                ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            INNER JOIN LocalDrivingLicenseApplications LDL
                                ON TestAppointments.LocalDrivingLicenseApplicationID = LDL.LocalDrivingLicenseApplicationID
                            INNER JOIN Applications
                                ON LDL.ApplicationID = Applications.ApplicationID
	                            AND ApplicantPersonID = @PersonID
	                            AND LicenseClassID = @LicenseClassID
	                            AND TestAppointments.TestTypeID = @TestTypeID
                            ORDER BY Tests.TestAppointmentID DESC";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            isFound = true;

                            TestID = (int)reader["TestID"];
                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)(reader["TestResult"]);
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "";
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {

                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static int AddNewTest(int TestAppointmentID,
                                    bool TestResult, string Notes,
                                    int CreatedByUserID)
        {
            int TestID = -1;

            string query = @"
                    INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID)
                    VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);
                    
                    -- Update TestSatus Appointment after adding the test 
                    UPDATE TestAppointments
                    SET IsLocked = 1 
                    WHERE TestAppointmentID = @TestAppointmentID;

                    SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(Notes) ? (object)DBNull.Value : Notes);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                            TestID = id;
                    }
                    catch (Exception ex)
                    {
                        TestID = -1;
                    }
                }
            }

            return TestID;
        }

        // For Wasta And Ma7sobia
        public static bool UpdateTest(int TestID, bool TestResult,
                                    string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"
            UPDATE Tests
            SET TestResult = @TestResult,
                Notes = @Notes,
                CreatedByUserID = @CreatedByUserID
            WHERE TestID = @TestID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestID", TestID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(Notes) ? (object)DBNull.Value : Notes);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        rowsAffected = 0;
                    }
                }
            }
            return rowsAffected > 0;

        }

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT * FROM Tests";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        dt.Load(reader);
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        dt = null;
                    }
                }
            }

            return dt;
        }

        static public byte GetNumOfPassedTests(int LDLAppID)
        {
            byte PassedTests = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT   COUNT(T.TestID) AS PassedTests     
                                FROM TestAppointments TA
                                INNER JOIN Tests T
                                ON TA.TestAppointmentID = T.TestAppointmentID 
                                AND LocalDrivingLicenseApplicationID = @LDLAppID
                                AND T.TestResult = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int NumOfPassedTests))
                    PassedTests = (byte)NumOfPassedTests;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return PassedTests;
        }


    }
}
