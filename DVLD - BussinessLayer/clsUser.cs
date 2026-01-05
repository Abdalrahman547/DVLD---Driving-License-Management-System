using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD___DataAccessLayer;

namespace DVLD___BussinessLayer
{
    public class clsUser
    {
        public enum enMode { AddNew, Update}

        public enMode Mode = enMode.AddNew;
        public int UserID { get; set; }
        public int PersonID { get; set; }

        // Composition => [User.PersonInfo.(Whatever)] 
        public clsPerson PersonInfo;
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public clsUser()
        {
            UserID = -1;
            PersonID = -1;
            UserName = string.Empty;
            Password = string.Empty;
            IsActive = true;
            IsAdmin = false;

            Mode = enMode.AddNew;
        }

        public clsUser(int UserID, int PersonID, string UserName, string Password, bool IsActive, bool IsAdmin)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;
            this.IsAdmin = IsAdmin;

            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            UserID = clsUserData.AddNewUser(PersonID, UserName, Password, IsActive, IsAdmin);

            return (UserID != -1);
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(UserID, UserName, Password, IsActive, IsAdmin);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                {
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                }
                case enMode.Update:
                    return _UpdateUser();

                default:
                    return false;
            }
            
        }

        public static clsUser FindByUserID(int UserID)
        {
            int PersonID = -1;
            string UserName = string.Empty;
            string Password = string.Empty;
            bool IsActive = false;
            bool IsAdmin = false;


            if (clsUserData.GetUserInfoByUserID(UserID, ref PersonID, ref UserName,
                                            ref Password, ref IsActive, ref IsAdmin))
                return new clsUser(UserID, PersonID, UserName, Password, IsActive, IsAdmin);
            else
                return null;

        }

        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1;
            string UserName = string.Empty;
            string Password = string.Empty;
            bool IsActive = false;
            bool IsAdmin = false;


            if (clsUserData.GetUserInfoByPersonID(ref UserID, PersonID, ref UserName,
                                            ref Password, ref IsActive, ref IsAdmin))
                return new clsUser(UserID, PersonID, UserName, Password, IsActive, IsAdmin);
            else
                return null;

        }

        public static clsUser FindByUserNameAndPassword(string UserName, string Password)
        {
            int UserID = -1;
            int PersonID = -1;
            bool IsActive = false;
            bool IsAdmin = false;

            if (clsUserData.GetUserInfoByUserNameAndPassword(ref UserID, ref PersonID, UserName,
                                             Password, ref IsActive, ref IsAdmin))
                return new clsUser(UserID, PersonID, UserName, Password, IsActive, IsAdmin);
            else
                return null;

        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public static bool IsUserExists(int UserID)
        {
            return clsUserData.IsUserExists(UserID);
        }

        public static bool IsUserExists(string UserName)
        {
            return clsUserData.IsUserExists(UserName);
        }

        public static bool IsUserExistsByPersonID(int PersonID)
        {
            return clsUserData.IsUserExistsByPersonID(PersonID);
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }   

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            return clsUserData.ChangePassword(UserID, NewPassword);
        }

    }
}
