using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbayCSVParser
{
    class EbayDataSet
    {
        public string Salesrecordnumber { get; set; } = string.Empty;
        public string Ordernumber { get; set; } = string.Empty;
        public string Buyerusername { get; set; } = string.Empty;
        public string Buyername { get; set; } = string.Empty;
        public string Buyeremail { get; set; } = string.Empty;
        public string Buyernote { get; set; } = string.Empty;
        public string Buyeraddress1 { get; set; } = string.Empty;
        public string Buyeraddress2 { get; set; } = string.Empty;
        public string Buyercity { get; set; } = string.Empty;
        public string Buyercounty { get; set; } = string.Empty;
        public string Buyerpostcode { get; set; } = string.Empty;
        public string Buyercountry { get; set; } = string.Empty;
        public string Posttoname { get; set; } = string.Empty;
        public string Posttophone { get; set; } = string.Empty;
        public string Posttoaddress1 { get; set; } = string.Empty;
        public string Posttoaddress2 { get; set; } = string.Empty;
        public string Posttocity { get; set; } = string.Empty;
        public string Posttocounty { get; set; } = string.Empty;
        public string Posttopostcode { get; set; } = string.Empty;
        public string Posttocountry { get; set; } = string.Empty;
        public string Itemnumber { get; set; } = string.Empty;
        public string Itemtitle { get; set; } = string.Empty;
        public string Customlabel { get; set; } = string.Empty;
        public string SoldviaPromotedlistings { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public string Soldfor { get; set; } = string.Empty;
        public string Postageandpackaging { get; set; } = string.Empty;
        public string IncludedVATrate { get; set; } = string.Empty;
        public string Sellercollectedtax { get; set; } = string.Empty;
        public string eBaycollectedtax { get; set; } = string.Empty;
        public string Electronicwasterecyclingfee { get; set; } = string.Empty;
        public string Mattressrecyclingfee { get; set; } = string.Empty;
        public string Additionalfee { get; set; } = string.Empty;
        public string Totalprice { get; set; } = string.Empty;
        public string eBaycollectedtaxandfeesincludedintotal { get; set; } = string.Empty;
        public string Paymentmethod { get; set; } = string.Empty;
        public string Saledate { get; set; } = string.Empty;
        public string Paidondate { get; set; } = string.Empty;
        public string Postbydate { get; set; } = string.Empty;
        public string Minimumestimateddeliverydate { get; set; } = string.Empty;
        public string Maximumestimateddeliverydate { get; set; } = string.Empty;
        public string Dispatchedondate { get; set; } = string.Empty;
        public string Feedbackleft { get; set; } = string.Empty;
        public string Feedbackreceived { get; set; } = string.Empty;
        public string Myitemnote { get; set; } = string.Empty;
        public string PayPaltransactionID { get; set; } = string.Empty;
        public string Deliveryservice { get; set; } = string.Empty;
        public string Trackingnumber { get; set; } = string.Empty;
        public string TransactionID { get; set; } = string.Empty;
        public string Variationdetails { get; set; } = string.Empty;
        public string GlobalShippingProgramme { get; set; } = string.Empty;
        public string GlobalShippingReferenceID { get; set; } = string.Empty;
        public string ClickandCollect { get; set; } = string.Empty;
        public string ClickandCollectReferenceNumber { get; set; } = string.Empty;
        public string eBayPlus { get; set; } = string.Empty;



        public DHLDataSet ConvertToDHL()
        {
            DHLDataSet dhlDS = new DHLDataSet();

            if (Tracked)
            {
                dhlDS.PRODUCT = PRODUCT_CODE_TRACKED;
            }
            else
            {
                dhlDS.PRODUCT = PRODUCT_CODE_NON_TRACKED;
            }

            dhlDS.SERVICE_LEVEL = SanitiseData(SERVICE_LEVEL);
            dhlDS.CUST_REF = SanitiseData(Buyername);
            dhlDS.NAME = SanitiseData(Posttoname);
            dhlDS.ADDRESS_LINE_1 = SanitiseData(Buyeraddress1);
            dhlDS.ADDRESS_LINE_2 = SanitiseData(Buyeraddress2);
            dhlDS.STATE = SanitiseData(Buyercounty);
            dhlDS.CITY = SanitiseData(Buyercity);
            dhlDS.POSTAL_CODE = SanitiseData(Buyerpostcode);
            dhlDS.DESTINATION_COUNTRY = SanitiseData(CountryCodes.Instance.GetCountryCode(Buyercountry));
            dhlDS.CURRENCY = SanitiseData(CURRENCY);
            dhlDS.CONTENT_TYPE = SanitiseData(CONTENT_TYPE);
            dhlDS.DECLARED_CONTENT_AMOUNT_1 = "1";
            dhlDS.DETAILED_CONTENT_DESCRIPTIONS_1 = "Pack of stamps";
            dhlDS.DECLARED_ORIGIN_COUNTRY_1 = "GB";
            dhlDS.RETURN_LABEL = "FALSE";
            dhlDS.RECIPIENT_PHONE = SanitiseData(Posttophone);
            dhlDS.RECIPIENT_EMAIL = SanitiseData(Buyeremail);

            dhlDS.DECLARED_VALUE_1 = SanitiseData(Soldfor.Replace("£", ""));
            dhlDS.TOTAL_VALUE = SanitiseData(dhlDS.DECLARED_VALUE_1);
            
            return dhlDS;
        }

        public string GetCountryCode(string country)
        {
            return "";
        }

        public string SanitiseData(string entry)
        {
            return entry.Replace("\"", "");
        }

        private const string PRODUCT_CODE_TRACKED = "GPT";
        private const string PRODUCT_CODE_NON_TRACKED = "GMP";
        private const string SERVICE_LEVEL = "PRIORITY";
        private const string CURRENCY = "GBP";
        private const string CONTENT_TYPE = "SALE_GOODS";


        public bool Tracked
        {
            get
            {
                string delService = Deliveryservice.ToUpper();

                return delService.Contains("TRACKED") || delService.Contains("SIGNED");
            }
        }
    }
}
