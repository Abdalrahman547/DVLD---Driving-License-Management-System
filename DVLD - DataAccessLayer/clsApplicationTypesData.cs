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
    public class clsApplocationTypesData
    {

        static public bool GetApplicationTypeByID(int AppTypeID, ref string Title, ref double Fees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            
            string query = @"SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @AppTypeID";
            
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);

            bool isFound = false;

            try
            {
                connection.Open();
                
                SqlDataReader reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    Title = reader["ApplicationTypeTitle"].ToString();
                    Fees = Convert.ToDouble(reader["ApplicationFees"]);
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

        static public DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM ApplicationTypes";

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

        static public bool UpdateApplicationType(int AppTypeID, string Title, double Fees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Update ApplicationTypes SET " +
                           "ApplicationTypeTitle = @Title, " +
                           "ApplicationFees = @Fees " +
                           "WHERE ApplicationTypeID = @AppTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);
            command.Parameters.AddWithValue("@Title", Title);
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
