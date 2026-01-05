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
    public class clsApplicationData
    {
        static public bool GetApplicationInfoByID(int AppID, ref int PersonID, ref DateTime AppDate, ref int AppTypeID,
                                                  ref short Status, ref DateTime LastStatusDate, ref double Fees, ref int UserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Applications WHERE ApplicationID = @AppID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppID", AppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PersonID = (int)reader["ApplicantPersonID"];
                    AppDate = (DateTime)reader["ApplicationDate"];
                    AppTypeID = (int)reader["ApplicationTypeID"];
                    Status = (byte)reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    Fees = Convert.ToDouble(reader["PaidFees"]);
                    UserID = (int)reader["CreatedByUserID"];
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return isFound;

        }


        static public int AddNewApplication(int PersonID, DateTime AppDate, int AppTypeID,
                                            short Status, DateTime LastStatusDate, double Fees, int UserID)
        {
            int NewAppID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Applications
                                    (
                                        ApplicantPersonID,
                                        ApplicationDate,
                                        ApplicationTypeID,
                                        ApplicationStatus,
                                        LastStatusDate,
                                        PaidFees,
                                        CreatedByUserID
                                    )
                                    VALUES
                                    (
                                        @PersonID, 
                                        @AppDate,  
                                        @AppTypeID,
                                        @Status,   
                                        @LastStatusDate,  
                                        @Fees,     
                                        @UserID  
                                    );
                                    SELECT SCOPE_IDENTITY();";



            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@AppDate", AppDate);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);
            command.Parameters.AddWithValue("@Status", Status);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@Fees", Fees);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    NewAppID = insertedID;
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
            return NewAppID;
        }


        static public bool UpdateApplication(int AppID, int PersonID, DateTime AppDate, int AppTypeID,
                                            short Status, DateTime LastStatusDate, double Fees, int UserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update Applications SET
                                        ApplicantPersonID = @PersonID,
                                        ApplicationDate = @AppDate,
                                        ApplicationTypeID = @AppTypeID,
                                        ApplicationStatus = @Status,
                                        LastStatusDate = @LastStatusDate,
                                        PaidFees = @Fees,
                                        CreatedByUserID =  @UserID
                                     WHERE ApplicationID = @AppID";



            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppID", AppID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@AppDate", AppDate);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);
            command.Parameters.AddWithValue("@Status", Status);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@Fees", Fees);
            command.Parameters.AddWithValue("@UserID", UserID);

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


        static public DataTable GetAllLocalApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Applications;";

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

        static public bool DeleteApplication(int AppID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "DELETE FROM Applications WHERE ApplicationID = @AppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppID", AppID);

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

        static public bool IsApplicationExsits(int AppID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT FOUND=1 FROM Applications WHERE ApplicationID = @AppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppID", AppID);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();

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
            return isFound;

        }

        static public int GetActiveApplicationID(int PersonID, int AppTypeID)
        {
            int ActiveAppID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT ApplicationID FROM Applications
                                    WHERE ApplicantPersonID = @PersonID AND
                                          ApplicationTypeID = @AppTypeID AND
                                          ApplicationStatus = 1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ActiveAppID = (int)reader[0];
                }

                reader.Close();

            }
            catch (SqlException ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return ActiveAppID;

        }

        static public bool DoesPersonHaveActiveApplication(int PersonID, int AppTypeID)
        {
            return GetActiveApplicationID(PersonID, AppTypeID) != -1;
        }

        static public bool UpdateStatus(int AppID, short Status)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update Applications SET
                                     ApplicationStatus = @Status,
                                     LastStatusDate = GETDATE()  
                                     WHERE ApplicationID = @AppID";



            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppID", AppID);
            command.Parameters.AddWithValue("@Status", Status);

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

        static public int GetActiveApplicationIDForLicenseClass(int PersonID, int AppTypeID, int LicenseClassID)
        {
            int ActiveAppID = -1;
            
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            
            string query = @"SELECT A.ApplicationID FROM Applications A
                             JOIN LocalDrivingLicenseApplications L ON A.ApplicationID = L.ApplicationID
                             WHERE A.ApplicantPersonID = @PersonID AND
                                   A.ApplicationTypeID = @AppTypeID AND
                                   A.ApplicationStatus = 1 AND
                                   L.LicenseClassID = @LicenseClassID;";

            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ActiveAppID = (int)reader[0];
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return ActiveAppID;
        }

        

    }
}
