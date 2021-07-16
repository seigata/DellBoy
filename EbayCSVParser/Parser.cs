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


    class Parser
    {
        public static string ConvertReports(string csvFile, string outputFile, DateTime dispatchedDate)
        {
            // Parse CSV Dataset using CSVHelper
            List<EbayDataSet> ebayReport = ParseEbayReport(csvFile);
            
            // Create empty dataset for DHL CSV
            List<DHLDataSet> dataSets = new List<DHLDataSet>();

            // Loop through every entry in ebay report parsing to a DHL CSV entry
            foreach (EbayDataSet ebayDS in ebayReport)
            {
                bool dateParsed = DateTime.TryParse(ebayDS.Dispatchedondate, out DateTime dsDate);

                // Check that this is an address record
                if (!string.IsNullOrEmpty(ebayDS.Buyeraddress1) && (dateParsed && dsDate.Date >= dispatchedDate.Date))
                {
                    DHLDataSet ds = ebayDS.ConvertToDHL();

                    if (string.IsNullOrEmpty(ds.DESTINATION_COUNTRY)) return "Could not find country code for: " + ebayDS.Buyercountry;

                    // Check that a label is required or this is tracked to outside UK
                    if (CountryCodes.Instance.LabelRequired(ds.DESTINATION_COUNTRY) || ((Properties.Settings.Default.ParseSettings.CreateForAllInternational || ebayDS.Tracked) && ds.DESTINATION_COUNTRY != "GB"))
                    {
                        // Check that we do not allready have a label for this address.
                        DHLDataSet existingDS = dataSets.FirstOrDefault(r => r.ADDRESS_LINE_1 == ds.ADDRESS_LINE_1 &&
                                        r.NAME == ds.NAME &&
                                        r.POSTAL_CODE == ds.POSTAL_CODE &&
                                        r.DESTINATION_COUNTRY == ds.DESTINATION_COUNTRY &&
                                        r.ADDRESS_LINE_2 == ds.ADDRESS_LINE_2 &&
                                        r.CITY == ds.CITY &&
                                        r.CUST_REF == ds.CUST_REF);

                        if (existingDS == null)
                        {
                            dataSets.Add(ds);
                        }
                        else
                        {
                            float totalValueExisitng = float.Parse(existingDS.TOTAL_VALUE);
                            float totalValueNew = float.Parse(ds.TOTAL_VALUE);

                            float newTotalValue = totalValueExisitng + totalValueNew;

                            existingDS.TOTAL_VALUE = newTotalValue.ToString();
                            existingDS.DECLARED_VALUE_1 = existingDS.TOTAL_VALUE;

                        }
                    }
                }
            }

            WriteDHLCSV(outputFile, dataSets);
            
            return "";
        }

        private static List<EbayDataSet> ParseEbayReport(string fileName)
        {
            using (TextReader tr = new StreamReader(fileName))
            {
                tr.ReadLine();
                
                CsvHelper.Configuration.CsvConfiguration config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    HasHeaderRecord = false
                    ,BadDataFound = null
                };

                CsvReader csvReader = new CsvReader(tr, config);

                csvReader.Read();
                
                return csvReader.GetRecords<EbayDataSet>().ToList();
            }
        }

        public static void WriteDHLCSV(string fileName, List<DHLDataSet> dataSet)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            using (CsvWriter cw = new CsvWriter(sw, CultureInfo.InvariantCulture))
            {
                cw.WriteRecords(dataSet);
            }
        }
        
    }

    public enum ShippingFilter
    {
        All,
        Tracked,
        None
    };

    public class ParserSettings
    {
        public bool CreateForAllInternational = false;
        public string Category = string.Empty;
    }
}
