using System;
using System.Data;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD___BussinessLayer
{
    public class clsPersonData
    {

        static public bool GetPersonInfoByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName,
                                                ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
                                                ref short Gendor, ref string Address, ref string Phone, ref string Email,
                                                ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    NationalNo = reader["NationalNo"].ToString();
                    FirstName = reader["FirstName"].ToString();
                    SecondName = reader["SecondName"].ToString();
                    LastName = reader["LastName"].ToString();
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    Gendor = (byte)reader["Gendor"];
                    Address = reader["Address"].ToString();
                    Phone = reader["Phone"].ToString();
                    NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);

                    // Handle ThirdName and Address as optional parameters
                    //----------------------------
                    if (reader["ThirdName"] == DBNull.Value)
                        ThirdName = "";
                    else
                        ThirdName = reader["ThirdName"].ToString();

                    //-----------------------------
                    if (reader["Email"] == DBNull.Value)
                        Email = "";
                    else
                        Email = reader["Email"].ToString();

                    //----------------------------
                    if (reader["ImagePath"] == DBNull.Value)
                        ImagePath = "";
                    else
                        ImagePath = reader["ImagePath"].ToString();

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

        static public bool GetPersonInfoByNationalNo(ref int PersonID, string NationalNo, ref string FirstName, ref string SecondName,
                                               ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
                                               ref short Gendor, ref string Address, ref string Phone, ref string Email,
                                               ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    PersonID = Convert.ToInt32(reader["PersonID"]);
                    FirstName = reader["FirstName"].ToString();
                    SecondName = reader["SecondName"].ToString();
                    LastName = reader["LastName"].ToString();
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    Gendor = (byte)reader["Gendor"];
                    Address = reader["Address"].ToString();
                    Phone = reader["Phone"].ToString();
                    NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);

                    // Handle ThirdName and Address as optional parameters
                    //----------------------------
                    if (reader["ThirdName"] != DBNull.Value)
                        ThirdName = "";
                    else
                        ThirdName = reader["ThirdName"].ToString();

                    //-----------------------------
                    if (reader["Email"] == DBNull.Value)
                        Email = "";
                    else
                        Email = reader["Email"].ToString();

                    //----------------------------
                    if (reader["ImagePath"] == DBNull.Value)
                        ImagePath = "";
                    else
                        ImagePath = reader["ImagePath"].ToString();

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

        static public int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
                                     DateTime DateOfBirth, short Gendor, string Address,
                                    string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "INSERT INTO People (FirstName, SecondName, ThirdName, LastName, NationalNo, DateOfBirth, Email, Phone, Gendor, Address, NationalityCountryID, ImagePath) " +
                           "VALUES (@FirstName, @SecondName, @ThirdName, @LastName, @NationalNo, @DateOfBirth, @Email, @Phone, @Gendor, @Address, @NationalityCountryID, @ImagePath); " +
                           "SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, Connection);

            // Handle Date Fomat
            DateOfBirth = DateOfBirth.Date;

            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@Address", Address);

            // Handle ThirdName and Address as optional parameters
            if (string.IsNullOrEmpty(ThirdName))
                command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
            else
                command.Parameters.AddWithValue("@ThirdName", ThirdName);

            //------------------------
            if (string.IsNullOrEmpty(ImagePath))
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            else
            command.Parameters.AddWithValue("@ImagePath", ImagePath);

            //------------------------
            if (string.IsNullOrEmpty(Email))
                command.Parameters.AddWithValue("@Email", DBNull.Value);
            else
                command.Parameters.AddWithValue("@Email", Email);

            try
            {
                Connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
                }
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return PersonID;
        }

        static public bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName,
                                        string LastName, DateTime DateOfBirth, short Gendor, string Address,
                                        string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Update People SET " +
                           "FirstName = @FirstName, " +
                           "SecondName = @SecondName, " +
                           "ThirdName = @ThirdName, " +
                           "LastName = @LastName, " +
                           "NationalNo = @NationalNo, " +
                           "DateOfBirth = @DateOfBirth, " +
                           "Email = @Email, " +
                           "Phone = @Phone, " +
                           "Gendor = @Gendor, " +
                           "Address = @Address, " +
                           "NationalityCountryID = @NationalityCountryID, " +
                           "ImagePath = @ImagePath WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@Address", Address);

            // Handle ThirdName and Address as optional parameters
            if (string.IsNullOrEmpty(ThirdName))
                command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
            else
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            //------------------------
            
            if (string.IsNullOrEmpty(Email))
                command.Parameters.AddWithValue("@Email", DBNull.Value);
            else
                command.Parameters.AddWithValue("@Email", Email);
            //------------------------

            if(string.IsNullOrEmpty(ImagePath))
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            else
                command.Parameters.AddWithValue("@ImagePath", ImagePath);

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

        static public bool DeletePerson(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "DELETE FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        static public DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT [PersonID], [NationalNo],
                            [FirstName], [SecondName], [ThirdName], [LastName],
                            [DateOfBirth],
                            CASE 
                                WHEN [Gendor] = 0 THEN 'Male'
                                WHEN [Gendor] = 1 THEN 'Female'
                            END AS GendorCaption, [Address] ,[Phone] ,[Email],
                            [Countries].[CountryName], [ImagePath] 
                            FROM [People] INNER JOIN [Countries]
                            ON People.NationalityCountryID = Countries.CountryID;";

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

        static public bool IsPersonExsits(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT FOUND=1 FROM People WHERE PersonID = @PersonID";

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

        static public bool IsPersonExsits(string NationalNo)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT FOUND=1 FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

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

        // Advanced
        // ------------------------------------------------------------
        static public bool IsPhoneExsits(string Phone)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT FOUND=1 FROM People WHERE Phone = @Phone";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Phone", Phone);

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

        static public bool IsEmailExsits(string Email)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT FOUND=1 FROM People WHERE Email = @Email";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Email", Email);

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


        // USELESS Methods
        static public DataTable FilterPeopleByPerosnID(string PersonID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE PersonID LIKE @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID + "%");

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

        static public DataTable FilterPeopleByNationalNo(string NationalNo)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE NationalNo LIKE @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo + "%");

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

        static public DataTable FilterPeopleByName(string Name)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM People 
                            WHERE FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName
                            LIKE @Name";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", Name + "%");

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

        static public DataTable FilterPeopleByPhone(string Phone)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE Phone LIKE @Phone";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Phone", Phone + "%");

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

        // Implement this!
        static public DataTable FilterPeopleByNationality(int CountryID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE CountryID LIKE @CountryID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID + "%");

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

        static public DataTable FilterPeopleByGendor(short Gendor)
        {

            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE Gendor = @Gendor";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Gendor", Gendor);

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

    }
}
