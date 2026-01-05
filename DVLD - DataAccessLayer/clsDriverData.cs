using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DVLD___BussinessLayer;
using System.Data;

namespace DVLD___DataAccessLayer
{
    public class clsDriverData
    {

        public static bool GetDriverInfoByID(int DriverID,
                                ref int PersonID,
                                ref int CreatedByUserID,
                                ref DateTime CreatedDate)
        {
            bool isFound = false;

            string query = @"SELECT * 
                             FROM Drivers WHERE DriverID = @DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DriverID", DriverID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    PersonID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];
                }

                reader.Close();
            }

            return isFound;
        }

        public static bool GetDriverInfoByPersonID(int PersonID,
                               ref int DriverID,
                               ref int CreatedByUserID,
                               ref DateTime CreatedDate)
        {
            bool isFound = false;

            string query = @"SELECT *
                             FROM Drivers WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonID", PersonID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    DriverID = (int)reader["DriverID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];
                }

                reader.Close();
            }

            return isFound;
        }

        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int newID = -1;

            string query = @"INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
                             VALUES (@PersonID, @CreatedByUserID, GETDATE() );
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    newID = Convert.ToInt32(result);
                }
            }

            return newID;
        }

        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Drivers 
                             SET PersonID = @PersonID,
                                 CreatedByUserID = @CreatedByUserID,
                                 CreatedDate = GETDATE()
                             WHERE DriverID = @DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return (rowsAffected > 0);
        }

        public static bool Delete(int DriverID)
        {
            int rowsAffected = 0;

            string query = "DELETE FROM Drivers WHERE DriverID = @DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DriverID", DriverID);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM Drivers_View ORDER BY FullName";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);
            }

            return dt;
        }

        public static int IsDriverExistsByPersonID(int PersonID)
        {
            int DriverID = -1;

            string query = @"SELECT DriverID
                             FROM Drivers
                             WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonID", PersonID);

                connection.Open();
                object result = command.ExecuteScalar();
                if(result != null && int.TryParse(result.ToString(), out int id))
                {
                    DriverID = id;
                }

            }

            return DriverID;
        }

        public static DataTable GetDriverLocalLicensesHistory(int DriverID)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT  
	                            Licenses.LicenseID, Licenses.ApplicationID, LicenseClasses.ClassName, Licenses.IssueDate, Licenses.ExpirationDate, Licenses.IsActive
                            FROM    
	                            Licenses INNER JOIN LicenseClasses 
		                            ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            WHERE DriverID = @DriverID
                            ORDER BY IssueDate DESC";
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@DriverID", DriverID);
                
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                
                adapter.Fill(dt);
            }
            return dt;
        }

        public static DataTable GetDriverInternationalLicensesHistory(int DriverID)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT  
                                InternationalLicenses.InternationalLicenseID, InternationalLicenses.ApplicationID, InternationalLicenseTypes.LicenseTypeName, 
                                InternationalLicenses.IssueDate, InternationalLicenses.ExpirationDate, InternationalLicenses.IsActive
                            FROM    
                                InternationalLicenses INNER JOIN InternationalLicenseTypes 
                                    ON InternationalLicenses.LicenseTypeID = InternationalLicenseTypes.LicenseTypeID
                            WHERE DriverID = @DriverID
                            ORDER BY IssueDate DESC";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }
            return dt;
        }

    }

}
