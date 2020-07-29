using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace EbayCSVParser
{
    class CountryCodeDataSet
    {
        public string CountryCode { get; set; } = "";
        public string CountryName { get; set; } = "";
        public string Label_Required { get; set; } = "";
        
    }

    class CountryCodes 
    {
        private static CountryCodes _instance = null;
        public List<CountryCodeDataSet> DataSets = new List<CountryCodeDataSet>();


        public string GetCountryCode(string country)
        {
            CountryCodeDataSet ds = DataSets.FirstOrDefault(r => r.CountryName == country);

            if (ds != null) return ds.CountryCode;

            return "";
        }

        public bool LabelRequired (string countryCode)
        {
            return DataSets.Any(r => r.CountryCode == countryCode && r.Label_Required.ToUpper().Trim() == "YES");
        }

        public static CountryCodes Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new CountryCodes();
                    using (TextReader tr = new StreamReader("CountryCodes.csv"))
                    {
                        CsvHelper.Configuration.CsvConfiguration config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            HeaderValidated = null,
                            MissingFieldFound = null,
                            HasHeaderRecord = false
                        };
                        CsvReader csvReader = new CsvReader(tr, config);

                        csvReader.Read();

                        _instance.DataSets = csvReader.GetRecords<CountryCodeDataSet>().ToList();
                    }
                }

                return _instance;
            }
        }
    }
}
