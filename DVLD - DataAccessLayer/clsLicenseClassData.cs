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
    public class clsLicenseClassData
    {
        static public DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM LicenseClasses;";

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

        public static bool GetLicenseClassInfoByID(int LicenseClassID,
             ref string ClassName, ref string ClassDescription,
             ref byte MinimumAllowedAge, ref byte DefaultValidityLength,
             ref double ClassFees)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT *
                                 FROM LicenseClasses
                                 WHERE LicenseClassID = @LicenseClassID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;
                        ClassName = (string)reader["ClassName"];
                        ClassDescription = (string)reader["ClassDescription"];
                        MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                        DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                        ClassFees = Convert.ToDouble(reader["ClassFees"]); // 👈 هنا double
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetLicenseClassInfoByClassName (string ClassName, 
            ref int LicenseClassID, ref string ClassDescription,
            ref byte MinimumAllowedAge, ref byte DefaultValidityLength,
            ref double ClassFees)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT *
                                 FROM LicenseClasses
                                 WHERE ClassName = @ClassName";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ClassName", ClassName);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;
                        LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
                        ClassDescription = (string)reader["ClassDescription"];
                        MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                        DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                        ClassFees = Convert.ToDouble(reader["ClassFees"]); // 👈 هنا double
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static int AddNewLicenseClass(string ClassName, string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, decimal ClassFees)
        {
            int LicenseClassID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO LicenseClasses
                                (ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees)
                                VALUES (@ClassName, @ClassDescription, @MinimumAllowedAge, @DefaultValidityLength, @ClassFees);
                                SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ClassName", ClassName);
                command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                command.Parameters.AddWithValue("@ClassFees", ClassFees);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        LicenseClassID = insertedID;
                }
                catch (SqlException ex)
                {
                    // handle exception
                }
            }

            return LicenseClassID;
        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName, string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, decimal ClassFees)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE LicenseClasses SET
                                ClassName = @ClassName,
                                ClassDescription = @ClassDescription,
                                MinimumAllowedAge = @MinimumAllowedAge,
                                DefaultValidityLength = @DefaultValidityLength,
                                ClassFees = @ClassFees
                                WHERE LicenseClassID = @LicenseClassID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                command.Parameters.AddWithValue("@ClassName", ClassName);
                command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                command.Parameters.AddWithValue("@ClassFees", ClassFees);

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

        public static bool DeleteLicenseClass(int LicenseClassID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static bool IsLicenseClassExists(int LicenseClassID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT FOUND = 1 FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

                SqlCommand command = new SqlCommand(query, connection);
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

    }
}
