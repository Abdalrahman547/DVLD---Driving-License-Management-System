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
    public class clsUserData
    {
        static public bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string UserName ,ref string Password,
                                               ref bool IsActive, ref bool IsAdmin)
        {
            bool isFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Users WHERE UserID = @UserID ";

            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    PersonID = Convert.ToInt32(reader["PersonID"]);
                    UserName = reader["UserName"].ToString();
                    Password = reader["Password"].ToString();
                    IsActive = (bool)reader["IsActive"];
                    IsAdmin = (bool)reader["IsAdmin"];
                   

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
                Connection.Close();
            }
            return isFound;
        }


        static public bool GetUserInfoByPersonID(ref int UserID, int PersonID, ref string UserName, ref string Password,
                                               ref bool IsActive, ref bool IsAdmin)
        {
            bool isFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Users WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserID = Convert.ToInt32(reader["UserID"]);
                    UserName = reader["UserName"].ToString();
                    Password = reader["Password"].ToString();
                    IsActive = (bool)reader["IsActive"];
                    IsAdmin = (bool)reader["IsAdmin"];


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
                Connection.Close();
            }
            return isFound;
        }


        static public bool GetUserInfoByUserNameAndPassword(ref int UserID, ref int PersonID, string UserName, string Password,
                                       ref bool IsActive, ref bool IsAdmin)
        {
            bool isFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password";

            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

            try
            {
                Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserID = Convert.ToInt32(reader["UserID"]);
                    PersonID = Convert.ToInt32(reader["PersonID"]);
                    IsActive = (bool)reader["IsActive"];
                    IsAdmin = (bool)reader["IsAdmin"];


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
                Connection.Close();
            }
            return isFound;
        }


        static public int AddNewUser(int PersonID, string UserName, string Password,
                                       bool IsActive, bool IsAdmin)
        {
            int UserID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "INSERT INTO Users (PersonID, UserName, Password, IsActive, IsAdmin) " +
                           "VALUES (@PersonID, @UserName, @Password, @IsActive, @IsAdmin); " +
                           "SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, Connection);


            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IsAdmin", IsAdmin);


            try
            {
                Connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return UserID;
        }


        static public bool UpdateUser(int UserID, string UserName, string Password,
                                       bool IsActive, bool IsAdmin)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Update Users SET " +
                           "UserName = @UserName, " +
                           "Password = @Password, " +
                           "IsActive = @IsActive, " +
                           "IsAdmin = @IsAdmin WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IsAdmin", IsAdmin);

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

        static public bool DeleteUser(int UserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "DELETE FROM Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

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

        static public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT UserID, Users.PersonID
	                        ,FirstName + ' ' + SecondName + ' ' +
                             ThirdName + ' ' + LastName AS FullName
                            ,UserName
                            ,IsActive
	                        ,IsAdmin FROM Users
                             INNER JOIN People
                             ON Users.PersonID = People.PersonID";

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

        static public bool IsUserExists(int UserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT FOUND=1 FROM Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

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

        static public bool IsUserExists(string UserName)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT FOUND=1 FROM Users WHERE UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

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
        
        static public bool IsUserExistsByPersonID(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT FOUND=1 FROM Users WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        static public bool ChangePassword(int UserID, string Password)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Update Users SET " +
                           "Password = @Password " +
                           "WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@Password", Password);

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
