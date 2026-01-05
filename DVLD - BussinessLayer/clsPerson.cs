using DVLD___BussinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BussinessLayer
{
    public class clsPerson
    {
        public enum enMode { AddNew, Update }
        public enMode Mode = enMode.AddNew;
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {SecondName} {ThirdName} {LastName}";
            }
        }
        public DateTime DateOfBirth { get; set; }
        public short Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public clsCountry CountryInfo;
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }
        
        public clsPerson() 
        {
            PersonID = -1;
            FirstName = string.Empty;
            SecondName = string.Empty;
            ThirdName = string.Empty;
            LastName = string.Empty;
            NationalNo = string.Empty;
            DateOfBirth = DateTime.Now;
            Email = string.Empty;
            Phone = string.Empty;
            Gendor = 0;
            Address = string.Empty;
            NationalityCountryID = -1;
            ImagePath = string.Empty;

            Mode = enMode.AddNew;
        }
        public clsPerson(int PersonID, string NationalNo, string FirstName, string SecondName,
                         string ThirdName, string LastName, DateTime DateOfBirth,
                         short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath )
        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;
            this.DateOfBirth = DateOfBirth;
            this.Email = Email;
            this.Phone = Phone;
            this.Gendor = Gendor;
            this.Address = Address;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;

            this.CountryInfo = clsCountry.Find(NationalityCountryID);

            Mode = enMode.Update; 
        }

        private bool _AddNewPerson()
        {
            PersonID = clsPersonData.AddNewPerson(NationalNo, FirstName, SecondName, ThirdName, LastName,
                                                  DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            return (PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                                              DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdatePerson();
                    
            }

            return false;
        }

        public static clsPerson Find(int PersonID)
        {
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "",
                   Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = 0;

            int NationalityCountryID = -1;


            if(clsPersonData.GetPersonInfoByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                                                   ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))

                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            
            else
                return null;
        }

        public static clsPerson Find(string NationalNo)
        {
            int PersonID = -1;
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "",
                  Address = "", Phone = "", Email = "", ImagePath = "";
            short Gendor = 0;
            DateTime DateOfBirth = DateTime.Now;

            int NationalityCountryID = -1;


            if (clsPersonData.GetPersonInfoByNationalNo(ref PersonID, NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                                                   ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))

            return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);


            else
                return null;
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        public static bool IsPersonExists(int PresonID)
        {
            return clsPersonData.IsPersonExsits(PresonID);
        }

        public static bool IsPersonExists(string NationalNo)
        {
            return clsPersonData.IsPersonExsits(NationalNo);
        }


        // Additinal Methods (USELESS)
        //--------------------------------------------
        public static bool IsPhoneExists(string Phone)
        {
            return clsPersonData.IsPhoneExsits(Phone);
        }

        public static bool IsEmailExists(string Email)
        {
            return clsPersonData.IsEmailExsits(Email);
        }

        public static DataTable FilterPeopleByPersonID(string PersonID)
        {
            return clsPersonData.FilterPeopleByPerosnID(PersonID);
        }

        public static DataTable FilterPeopleByNationalNo(string NationalNo)
        {
            return clsPersonData.FilterPeopleByNationalNo(NationalNo);
        }

        public static DataTable FilterPeopleByName(string Name)
        {
            return clsPersonData.FilterPeopleByName(Name);
        }

        public static DataTable FilterPeopleByPhone(string Phone)
        {
            return clsPersonData.FilterPeopleByPhone(Phone);
        }

        public static DataTable FilterPeopleByGendor(short Gendor)
        {
            return clsPersonData.FilterPeopleByGendor(Gendor);
        }

        public static DataTable FilterPeopleByNationality(int Nationality)
        {
            return clsPersonData.FilterPeopleByNationality(Nationality);
        }

    }
}
