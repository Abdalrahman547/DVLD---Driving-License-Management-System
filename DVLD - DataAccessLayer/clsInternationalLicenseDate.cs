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
    public class clsInternationalLicenseDate
    {
        public static bool GetInternationalLicenseInfoByInternationalID(int InternationalLicenseID,
            ref int ApplicationID, ref int DriverID, ref int IssuedLDLID,
            ref DateTime IssueDate, ref DateTime ExpirationDate,
            ref bool IsActive, ref int createdByUserID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                IssuedLDLID = (int)reader["IssuedUsingLocalLicenseID"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];
                                IsActive = (bool)reader["IsActive"];
                                createdByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log it)
                    }

                }
            }

            return isFound;

        }

        public static bool GetInternationalLicenseInfoByLDLID(int IssuedLDLID,
           ref int InternationalLicenseID, ref int ApplicationID, ref int DriverID,
           ref DateTime IssueDate, ref DateTime ExpirationDate,
           ref bool IsActive, ref int createdByUserID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * FROM InternationalLicenses WHERE IssuedUsingLocalLicenseID = @IssuedLDLID 
                                ORDER BY ExpirationDate DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IssuedLDLID", IssuedLDLID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                InternationalLicenseID = (int)reader["InternationalLicenseID"];
                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];
                                IsActive = (bool)reader["IsActive"];
                                createdByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log it)
                    }

                }
            }

            return isFound;

        }

        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * FROM InternationalLicenses ORDER BY IsActive, ExpirationDate DESC";

                SqlCommand command = new SqlCommand(query, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);
            }

            return dt;
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * FROM InternationalLicenses 
                                WHERE DriverID = @DriverID ORDER BY ExpirationDate DESC";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@DriverID", DriverID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);
            }

            return dt;
        }

        public static int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedLDLID,
             DateTime IssueDate, DateTime ExpirationDate, int createdByUserID)
        {
            int NewID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
                             Update InternationalLicenses
                                SET IsActive = 0
                                WHERE DriverID = @DriverID;

                             
                             INSERT INTO InternationalLicenses
                                    (ApplicationID,
                                     DriverID,
                                     IssuedUsingLocalLicenseID,
                                     IssueDate,
                                     ExpirationDate,
                                     IsActive,
                                     CreatedByUserID)
                             VALUES 
                             (@ApplicationID,
                              @DriverID,
                              @IssuedLDLID,
                              @IssueDate,
                              @ExpirationDate,
                              1, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@IssuedLDLID", IssuedLDLID);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                connection.Open();

                var result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    NewID = insertedID;
                }
            }

            return NewID;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT Top 1 InternationalLicenseID 
                                 FROM InternationalLicenses 
                                 WHERE 
                                    DriverID = @DriverID
                                    AND IsActive = 1
                                    AND GetDate() BETWEEN IssueDate AND ExpirationDate
                                    ORDER BY ExpirationDate DESC";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@DriverID", DriverID);

                connection.Open();

                var result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    InternationalLicenseID = id;
                }
            }
            return InternationalLicenseID;
        }

        public static bool DeactivateInternationalLicense(int InternationalLicenseID)
        {
            bool isSuccess = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE InternationalLicenses
                                 SET IsActive = 0
                                 WHERE InternationalLicenseID = @InternationalLicenseID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

                try
                {
                    connection.Open();

                    isSuccess = command.ExecuteNonQuery() > 0;

                }
                catch (Exception ex)
                {
                    // Handle exception (e.g., log it)
                }
            }
            return isSuccess;
        }

        // Incase we need to update any information
        public static bool UpdateInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID,
           int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate,
           bool IsActive, int CreatedByUserID)
        {
            int rowsAffected = 0; 

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE InternationalLicenses SET
                                ApplicationID = @ApplicationID,
                                DriverID = @DriverID,
                                IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                                IssueDate = @IssueDate,
                                ExpirationDate = @ExpirationDate,
                                IsActive = @IsActive,
                                CreatedByUserID = @CreatedByUserID
                                WHERE InternationalLicenseID = @InternationalLicenseID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    // handle exception
                }
            }

            return rowsAffected > 0;
        }

    }
}
