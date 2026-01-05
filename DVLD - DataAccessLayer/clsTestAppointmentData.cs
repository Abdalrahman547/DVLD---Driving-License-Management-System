using DVLD___BussinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___DataAccessLayer
{
    public class clsTestAppointmentData
    {

        static public bool GetTestAppointmentByID(int TestAppointmentID, ref int TestTypeID,
                                                ref int LDLAppID, ref DateTime AppointmentDate,
                                                ref double Fees, ref int CreatedByUserID,
                                                ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;

                    TestTypeID = Convert.ToInt32(reader["TestTypeID"]);
                    LDLAppID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                    Fees = Convert.ToDouble(reader["PaidFees"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    IsLocked = Convert.ToBoolean(reader["IsLocked"]);
                    
                    if(reader["RetakeTestApplicationID"] == DBNull.Value)
                        RetakeTestApplicationID = -1;
                    else
                        RetakeTestApplicationID = Convert.ToInt32(reader["RetakeTestApplicationID"]);
                }

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
            return IsFound;
        }


        static public bool GetLastTestAppointment(int LDLApplication, int TestTypeID,
                                                ref int TestAppointmentID, ref DateTime AppointmentDate,
                                                ref double Fees, ref int CreatedByUserID,
                                                ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TOP 1 * FROM TestAppointments
                            WHERE LocalDrivingLicenseApplicationID = @LDLApplication
                            AND TestTypeID = @TestTypeID
                            ORDER BY TestAppointmentID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplication", LDLApplication);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;

                    TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                    Fees = Convert.ToDouble(reader["PaidFees"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    IsLocked = Convert.ToBoolean(reader["IsLocked"]);
                    RetakeTestApplicationID = Convert.ToInt32(reader["RetakeTestApplicationID"]);
                }

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
            return IsFound;
        }


        static public int AddTestAppointment(int TestTypeID, int LDLAppID,
                                                DateTime AppointmentDate, double Fees,
                                                int CreatedByUserID, bool IsLocked,
                                                int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate,
                                                            PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID) 
                            VALUES (@TestTypeID, @LDLAppID, @AppointmentDate, @Fees, @CreatedByUserID, @IsLocked, NULLIF(@RetakeTestApplicationID, -1));
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@Fees", Fees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);
            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            try
            {
                connection.Open();
                Object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    TestAppointmentID = InsertedID;
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
            return TestAppointmentID;
        }

        static public bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LDLAppID,
                                                DateTime AppointmentDate, double Fees,
                                                int CreatedByUserID, bool IsLocked,
                                                int RetakeTestApplicationID)
                {
                    int rowsAffected = 0;

                    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                    string query = @"UPDATE TestAppointments
                             SET TestTypeID = @TestTypeID,
                                 LocalDrivingLicenseApplicationID = @LDLAppID,
                                 AppointmentDate = @AppointmentDate,
                                 PaidFees = @Fees,
                                 CreatedByUserID = @CreatedByUserID,
                                 IsLocked = @IsLocked,
                                 RetakeTestApplicationID = NULLIF(@RetakeTestApplicationID, -1)
                             WHERE TestAppointmentID = @TestAppointmentID";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    command.Parameters.AddWithValue("@Fees", Fees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@IsLocked", IsLocked);
                    command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

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



        static public DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM TestAppointments_View
                            ORDER BY AppointmentDate DESC";
                                

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


        static public DataTable GetApplicationTestAppointmentPerTestID(int LDLApplication, int TestTypeID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT [TestAppointmentID]
                    ,TestType = (SELECT TestTypeTitle FROM TestTypes WHERE TestTypeID = @TestTypeID)
                    ,[AppointmentDate]
                    ,[PaidFees]
                    ,[IsLocked]
                    FROM [TestAppointments]
                    WHERE [TestTypeID] = @TestTypeID
                    AND LocalDrivingLicenseApplicationID = @LDLApplication
                    ORDER BY [TestAppointmentID] DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplication", LDLApplication);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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


        static public int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TestID FROM Tests
                     WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    TestID = id;
                }
            }
            catch (SqlException ex)
            {
                // Handle exception (مثلاً logging)
            }
            finally
            {
                connection.Close();
            }

            return TestID;
        }


    }
}
