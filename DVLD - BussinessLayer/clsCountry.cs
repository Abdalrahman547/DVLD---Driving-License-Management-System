using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BussinessLayer
{
    public class clsCountry
    {

        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public clsCountry()
        {
            CountryID = -1;
            CountryName = string.Empty;
        }
        public clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }

        public static clsCountry Find(int CountryID)
        {
            string CountryName = string.Empty;

            bool isFound = clsCountryData.GetCountryByID(CountryID, ref CountryName);

            if(isFound)
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null; // or throw an exception if you prefer
            }


        }

        public static clsCountry Find(string CountryName)
        {
            int CountryID = -1;

            bool isFound = clsCountryData.GetCountryByCountryName(CountryName, ref CountryID);

            if (isFound)
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null; // or throw an exception if you prefer
            }


        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }

    }
}
