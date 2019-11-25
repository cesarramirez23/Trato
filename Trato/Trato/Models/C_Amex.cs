using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Trato.Models
{
    public class C_Amex
    {
        /// <summary>
        /// Unique identiﬁer for the stored card
        /// </summary>
        [JsonProperty("cardToken")]
        public string v_card  { get; set; }
        /// <summary>
        /// Amount to be charged double
        /// </summary>
        [JsonProperty("amount")]
        public double v_amount { get; set; }
        /// <summary>
        /// Optional, see notes String
        /// </summary>
        [JsonProperty("txnType")]
        public string v_txnType { get; set; }
        /// <summary>
        /// Mandatory for Recurring Transactions String
        /// </summary>
        [JsonProperty("contractNumber")]
        public string  v_contractNumber { get; set; }
        /// <summary>
        /// Payment Plan indicator int
        /// </summary>
        [JsonProperty("paymentPlan")]
        public int v_paymentPlan { get; set; }
        /// <summary>
        /// Cardholder Billing Postal Code (Max length: 9)
        /// </summary>
        [JsonProperty("amexCustPostalCode")]
        public string v_postalcode { get; set; }
        /// <summary>
        /// Cardholder Billing Address (Max length: 20
        /// </summary>
        [JsonProperty("amexCustAddress")]
        public string v_direcccion { get; set; }
        /// <summary>
        /// Cardholder First Name (Max length: 15)
        /// </summary>
        [JsonProperty("amexCustFirstName")]
        public string v_nombre { get; set; }
        /// <summary>
        /// Cardholder Last Name (Max length: 30)
        /// </summary>
        [JsonProperty("amexCustLastName")]
        public string v_apellido { get; set; }
        /// <summary>
        /// Cardholder Email Address (Max length: 60)
        /// </summary>
        [JsonProperty("amexCustEmailAddress")]
        public string v_correo { get; set; }
        /// <summary>
        /// Customer Host Server Name (Max length: 60)
        /// </summary>
        [JsonProperty("amexCustHostServerNm")]
        public string v_host { get; set; }
        /// <summary>
        /// Customer HTTP Browser Type (Max length: 60)
        /// </summary>
        [JsonProperty("amexCustBrowserTypDescTxt")]
        public string v_browser { get; set; }
        /// <summary>
        /// Country code (default: MX ’484’, Max length: 3)
        /// </summary>
        [JsonProperty("amexShipToCtryCd")]
        public string v_countrycode { get; set; }
        /// <summary>
        /// Shipment method code (default ’02’, Max length, 2)
        /// </summary>
        [JsonProperty("amexShipMthdCd")]
        public string v_shipment { get; set; }
        /// <summary>
        /// Merchant SKU Number (Max length: 15)
        /// </summary>
        [JsonProperty("amexMerSKUNbr")]
        public string v_sku { get; set; }
        /// <summary>
        /// Customer IP Address (Max length: 15
        /// </summary>
        [JsonProperty("amexCustIPAddr")]
        public string v_ipAdd { get; set; }
        /// <summary>
        /// Cardholder Phone Number (Max length: 10)
        /// </summary>
        [JsonProperty("amexCustIdPhoneNbr")]
        public string v_phone { get; set; }
        /// <summary>
        /// Call Type ID (Max length: 2)
        /// </summary>
        [JsonProperty("amexCallTypId")]
        public string v_typeid{ get; set; }     
    }
}
