﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubyTouch.Utils.Location.FrenchZipCode
{

    public class Model
    {
        #region Cctor
        internal Model(ReadOnlyCollection<region> regions, Departments departments, ReadOnlyCollection<CityFileRecord> cityFileRecords, bool createLinks)
        {
            Regions = regions;
            Departments = departments;
            CityFileRecords = cityFileRecords;

            CreateCitiesAndMailOfficies(createLinks);
        }

        private void CreateCitiesAndMailOfficies(bool createLinks)
        {

            var x = CityFileRecords.Where(r => r.insee_code == "97617").Count();


            Cities = new Cities();
            MailOffices = new MailOffices();

            int officeCreationNb = 0;
            int nullPostalCodeNb = 0;

            City city = null;
            PostOffice mailOffice;

            foreach (var rec in CityFileRecords)
            {

                if (!string.IsNullOrEmpty(rec.insee_code))
                {
                    if (Cities.Contains(rec.insee_code))
                        city = Cities[rec.insee_code];
                    else
                    {
                        city = new City() { name = rec.name, insee_code = rec.insee_code, department_code=rec.department_code};
                        Cities.Add(city);
                    }
                }
                else city = null;

                if (!string.IsNullOrEmpty(rec.zip_code))
                {
                    if (MailOffices.Contains(rec.zip_code))
                        mailOffice = MailOffices[rec.zip_code];
                    else
                    {
                        mailOffice = new PostOffice() { zip_code = rec.zip_code, slug = rec.slug, gps_lat = rec.gps_lat, gps_lng = rec.gps_lng };
                        MailOffices.Add(mailOffice);
                        officeCreationNb += 1;
                    }

                    if (city != null && mailOffice != null)
                    {
                        city.innerMaillOffices.Add(mailOffice);
                        mailOffice.innerCities.Add(city);
                    }
                }
                else nullPostalCodeNb += 1;
            };
        }

        #endregion

        public ReadOnlyCollection<region> Regions { get; private set; }
        public ReadOnlyCollection<CityFileRecord> CityFileRecords { get; private set; }
        public MailOffices MailOffices { get; private set; }
        public Departments Departments { get; private set; }
        public Cities Cities { get; private set; }

        public department GetDepartment(string postalCode)
        {
            var dptCode = GetDptCode(postalCode);
            var departement = (Departments.Contains(dptCode)) ? Departments[dptCode] : null;
            return departement;
        }
        internal static string GetDptCode(string postalCode)
        {
            var domTom = (postalCode.StartsWith("97") || postalCode.StartsWith("98"));
            string r = (domTom) ? postalCode.Substring(0, 3) : postalCode.Substring(0, 2);
            return r;
        }
    }
}
