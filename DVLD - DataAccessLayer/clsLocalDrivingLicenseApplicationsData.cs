using DVLD___BussinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___DataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationsData
    {

        static public bool GetLDLApplicationInfoByID(int LDLAppID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LDLID;";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLID", LDLAppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return isFound;

        }

        static public bool GetLDLApplicationInfoByApplicationID(int ApplicationID, ref int LDLID, ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM LocalDrivingLicenseApplications WHERE ApplicationID = @ApplicationID;";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    LDLID = (int)reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return isFound;

        }

        static public int AddNewLDLApplication(int ApplicationID, int LicenseClassID)
        {
            int LDLAppID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO LocalDrivingLicenseApplications
                            ( ApplicationID, LicenseClassID )
                            VALUES
                            ( @ApplicationID, @LicenseClassID );
                            SELECT SCOPE_IDENTITY();";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);



            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LDLAppID = insertedID;
                }

            }

            catch (SqlException ex)
            {
                // Handle exception
            }
            finally
            {
                connection.Close();
            }
            return LDLAppID;
        }

        static public DataTable GetAllLDLApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM LocalDrivingLicenseApplications_View
                             ORDER BY ApplicationDate DESC";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    dt.Load(reader);

                reader.Close();
            }
            catch (SqlException ex)
            {
                // Handle exception
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        static public bool DeleteLDLApplication(int LDLAppID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE LocalDrivingLicenseApplications 
                             WHERE LocalDrivingLicenseApplicationID = @LDLAppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);

            int RowsAffected = 0;

            try
            {
                connection.Open();

                RowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return RowsAffected > 0;
        }

        static public bool UpdateLDLApplication(int LDLAppID, int ApplicationID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update LocalDrivingLicenseApplications SET
                            ApplicationID = @ApplicationID,
                            LicenseClassID = @LicenseClassID
                            WHERE LocalDrivingLicenseApplicationID = @LDLAppID";



            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }

            catch (SqlException ex)
            {
                // Handle exception
                return false;

            }
            finally
            {
                connection.Close();
            }
            return rowsAffected > 0;
        }

        static public bool DoesPassTestType(int LDLAppID, int TestTypeID)
        {
            bool doesPass = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT TOP 1 T.TestResult
                                 FROM LocalDrivingLicenseApplications L
                                 INNER JOIN TestAppointments TA 
                                     ON L.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID
                                 INNER JOIN Tests T 
                                     ON T.TestAppointmentID = TA.TestAppointmentID
                                 WHERE L.LocalDrivingLicenseApplicationID = @LDLAppID
                                   AND TA.TestTypeID = @TestTypeID
                                 ORDER BY TA.TestAppointmentID DESC; --Last Appointment";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // TestResult is stored as int (1 = pass, 0 = fail)
                        doesPass = Convert.ToInt32(reader["TestResult"]) == 1;
                    }

                    reader.Close();
                }
                catch (SqlException ex)
                {
                    // Handle exception (log it, rethrow, etc.)
                }
            }

            return doesPass;
        }

        static public int GetActiveLicenseID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT L.LicenseID
                            FROM Licenses L
                            JOIN LicenseClasses LC
                                ON L.LicenseClass = LC.LicenseClassID
                            JOIN Drivers D 
                                ON D.DriverID = L.DriverID
                            WHERE 
                                D.PersonID = @PersonID
                                AND LC.LicenseClassID = @LicenseClassID
                                AND L.IsActive = 1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    LicenseID = ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return LicenseID;
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

        static public bool DoesAttendTestType(int LDLAppID, int TestTypeID)
        {
            int TestAppointmentID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TOP (1) TestAppointmentID
                            FROM TestAppointments
                            WHERE LocalDrivingLicenseApplicationID = @LDLAppID
                            AND TestTypeID = @TestTypeID 
                            AND IsLocked = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    TestAppointmentID = ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return TestAppointmentID != -1;

        }

        static public short TotalTrialsPerTest(int LDLAppID, int TestTypeID)
        {
            short Trails = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT count(TestAppointmentID) AS Trails
                              FROM [DVLD].[dbo].[TestAppointments_View]
                              WHERE LocalDrivingLicenseApplicationID = @LDLAppID 
                              AND TestTypeTitle = (SELECT TestTypes.TestTypeTitle FROM TestTypes WHERE TestTypeID = @TestTypeID);";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int NumOfTrails))
                    Trails = (short)NumOfTrails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return Trails;

        }

        static public bool IsThereAnActiveScheduledTest(int LDLAppID, int TestTypeID)
        {
            int TestAppointmentID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TOP (1) TestAppointmentID
                            FROM [TestAppointments]
                            WHERE LocalDrivingLicenseApplicationID = @LDLAppID
                            AND TestTypeID = @TestTypeID
                            AND IsLocked = 0
                            ORDER BY TestAppointmentID DESC;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    TestAppointmentID = ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return TestAppointmentID != -1;
        }

        static public bool GetLastTestperTestType(int LDLAppID, int TestTypeID, ref int TestID,
                                                ref int TestAppointment, ref bool TestResult,
                                                ref string Notes, ref int CreatedByUserID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TOP 1 Tests.*
                            FROM LocalDrivingLicenseApplications LDLA
                            INNER JOIN TestAppointments 
                            ON LDLA.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                            INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE LDLA.LocalDrivingLicenseApplicationID = @LDLAppID
                            AND TestTypeID = @TestTypeID
                            ORDER BY TestAppointmentID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;

                    TestID = Convert.ToInt32(reader["TestID"]);
                    TestAppointment = Convert.ToInt32(reader["TestAppointmentID"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    
                    TestResult = (bool)reader["TestResult"];
                    
                    if (reader["Notes"] == null)
                        Notes = "";
                    else
                        Notes = reader["Notes"].ToString();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return IsFound;

        }
    }
}
