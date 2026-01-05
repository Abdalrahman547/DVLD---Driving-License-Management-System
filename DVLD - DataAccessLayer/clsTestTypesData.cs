using DVLD___BussinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___DataAccessLayer
{
    public class clsTestTypesData
    {

        static public bool GetTestTypeByID(int TestTypeID, ref string Title, ref string Description, ref double Fees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Title = reader["TestTypeTitle"].ToString();
                    Description = reader["TestTypeDescription"].ToString();
                    Fees = Convert.ToDouble(reader["TestTypeFees"]);
                    isFound = true;
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
            return isFound;
        }

        static public DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM TestTypes";

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

        static public bool UpdateTestType(int TestTypeID, string Title, string Description, double Fees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Update TestTypes SET " +
                           "TestTypeTitle = @Title, " +
                           "TestTypeFees = @Fees, " +
                           "TestTypeDescription = @Description " +
                           "WHERE TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Description", Description);
            command.Parameters.AddWithValue("@Fees", Fees);

            int rowsAffected;

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
    }
}
