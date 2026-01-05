using DVLD___BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___DataAccessLayer
{
    public class clsDetainedLicenseData
    {

        public static bool GetDetainedLicenseInfoByID(int DetainID,
            ref int licenseID, ref DateTime detainDate,
            ref decimal fineFees, ref int createdByUserID,
            ref bool isReleased, ref DateTime releaseDate,
            ref int releasedByUserID, ref int releaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";
            
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    licenseID = (int)reader["LicenseID"];
                    detainDate = (DateTime)reader["DetainDate"];
                    fineFees = (decimal)reader["FineFees"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                    isReleased = (bool)reader["IsReleased"];

                    releaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["ReleaseDate"];
                    releasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)reader["ReleasedByUserID"];
                    releaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)reader["ReleaseApplicationID"];
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID,
            ref int detainID, ref DateTime detainDate,
            ref decimal fineFees, ref int createdByUserID,
            ref bool isReleased, ref DateTime releaseDate,
            ref int releasedByUserID, ref int releaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM DetainedLicenses WHERE LicenseID = @LicenseID ORDER BY DetainID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    detainID = (int)reader["DetainID"];
                    detainDate = (DateTime)reader["DetainDate"];
                    fineFees = (decimal)reader["FineFees"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                    isReleased = (bool)reader["IsReleased"];

                    releaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["ReleaseDate"];
                    releasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)reader["ReleasedByUserID"];
                    releaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)reader["ReleaseApplicationID"];
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dataTable = new DataTable();
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * FROM DetainedLicenses_View ORDER BY DetainDate DESC";
                
                SqlCommand command = new SqlCommand(query, connection);
                
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                
                adapter.Fill(dataTable);
            }
            return dataTable;
        }

        public static int AddNewDetainedLicense(int licenseID, DateTime detainDate,
            decimal fineFees, int createdByUserID)
        {
            int NewID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased)
                             VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0);
                             SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@LicenseID", licenseID);
                command.Parameters.AddWithValue("@DetainDate", detainDate);
                command.Parameters.AddWithValue("@FineFees", fineFees);
                command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                connection.Open();
                
                var result = command.ExecuteScalar();
                
                if(result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    NewID = insertedID;
                }
            }

            return NewID;
        }

        public static bool ReleaseDetainedLicense(int detainID, DateTime ReleaseDate, int releasedByUserID, int releaseApplicationID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE DetainedLicenses 
                             SET IsReleased = 1,
                                 ReleaseDate = @ReleaseDate,
                                 ReleasedByUserID = @ReleasedByUserID,
                                 ReleaseApplicationID = @ReleaseApplicationID
                             WHERE DetainID = @DetainID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@DetainID", detainID);
                command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
                command.Parameters.AddWithValue("@ReleasedByUserID", releasedByUserID);
                command.Parameters.AddWithValue("@ReleaseApplicationID", releaseApplicationID);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }

        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool isDetained = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT IsDetained = 1 FROM DetainedLicenses 
                             WHERE LicenseID = @LicenseID AND IsReleased = 0";
                
                SqlCommand command = new SqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isDetained = reader.HasRows;

            }
                
            return isDetained;

        }
    }
}
