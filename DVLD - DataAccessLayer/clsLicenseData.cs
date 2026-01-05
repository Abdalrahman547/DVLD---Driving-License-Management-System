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
    public class clsLicenseData
    {
        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID,
            ref int LicenseClass, ref DateTime IssueDate, ref DateTime ExpirationDate,
            ref string Notes, ref double PaidFees, ref bool IsActive,
            ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        ApplicationID = (int)reader["ApplicationID"];
                        DriverID = (int)reader["DriverID"];
                        LicenseClass = (int)reader["LicenseClass"];
                        IssueDate = (DateTime)reader["IssueDate"];
                        ExpirationDate = (DateTime)reader["ExpirationDate"];
                        Notes = reader["Notes"].ToString();
                        PaidFees = Convert.ToDouble(reader["PaidFees"]);
                        IsActive = (bool)reader["IsActive"];
                        IssueReason = Convert.ToByte(reader["IssueReason"]);
                        CreatedByUserID = (int)reader["CreatedByUserID"];

                        isFound = true;
                    }
                    reader.Close();
                }
                catch (SqlException ex)
                {
                    // handle exception
                }
            }

            return isFound;
        }

        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes, double PaidFees,
            bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;

            using ( SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO Licenses 
                                (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
                                VALUES (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                                SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                command.Parameters.AddWithValue("@Notes", Notes);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@IssueReason", IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        LicenseID = insertedID;
                }
                catch (SqlException ex)
                {
                    // handle exception
                }
            }

            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes, double PaidFees,
            bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE Licenses SET
                                ApplicationID = @ApplicationID,
                                DriverID = @DriverID,
                                LicenseClass = @LicenseClass,
                                IssueDate = @IssueDate,
                                ExpirationDate = @ExpirationDate,
                                Notes = @Notes,
                                PaidFees = @PaidFees,
                                IsActive = @IsActive,
                                IssueReason = @IssueReason,
                                CreatedByUserID = @CreatedByUserID
                                WHERE LicenseID = @LicenseID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                command.Parameters.AddWithValue("@Notes", Notes);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@IssueReason", IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // handle exception
                }
            }

            return rowsAffected > 0;
        }

        public static bool DeleteLicense(int LicenseID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM Licenses WHERE LicenseID = @LicenseID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // handle exception
                }
            }

            return rowsAffected > 0;
        }

        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Licenses";

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
                    // handle exception
                }
            }

            return dt;
        }

        public static bool IsLicenseExists(int LicenseID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT FOUND = 1 FROM Licenses WHERE LicenseID = @LicenseID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    isFound = reader.HasRows;
                    reader.Close();
                }
                catch (SqlException ex)
                {
                    // handle exception
                }
            }

            return isFound;
        }

        public static bool IsLicenseExistsByPersonID(int PersonID, int LicenseClassID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT FOUND = 1 FROM Licenses WHERE PersonID = @PersonID AND LicenseClass = @LicenseClassID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    isFound = reader.HasRows;
                    reader.Close();
                }
                catch (SqlException ex)
                {
                    // handle exception
                }
            }

            return isFound;
        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable dt = new DataTable();
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT	Licenses.*, LicenseClasses.ClassName
                                FROM    Licenses 
                                INNER JOIN LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                                WHERE DriverID = @DriverID
                                ORDER BY IsActive DESC, ExpirationDate DESC";
                
                SqlCommand command = new SqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@DriverID", DriverID);
                
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
                    // handle exception
                }
            }
            return dt;
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT Top 1 Licenses.LicenseID FROM LicenseClasses
                                INNER JOIN Licenses ON LicenseClasses.LicenseClassID = Licenses.LicenseClass
                                INNER JOIN Drivers ON Licenses.DriverID = Drivers.DriverID 
                                WHERE PersonID = @PersonID AND LicenseClassID = @LicenseClassID AND IsActive = 1 
                                ORDER BY ExpirationDate DESC";
                
                SqlCommand command = new SqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@PersonID", PersonID);
                
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                
                try
                {
                    connection.Open();
                    
                    object result = command.ExecuteScalar();
                    
                    if (result != null && int.TryParse(result.ToString(), out int foundID))
                        LicenseID = foundID;
                }
                catch (SqlException ex)
                {
                    // handle exception
                }
            }
            
            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE Licenses SET IsActive = 0 WHERE LicenseID = @LicenseID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // handle exception
                }
            }
            return rowsAffected > 0;
        }
    }
}
