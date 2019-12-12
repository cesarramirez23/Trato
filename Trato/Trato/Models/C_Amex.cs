using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Trato.Models
{
    public class C_Amex
    {
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
        [JsonProperty("amexCustEmailAddr")]
        public string v_correo { get; set; }
        /// <summary>
        /// Cardholder Phone Number (Max length: 10)
        /// </summary>
        [JsonProperty("amexCustIdPhoneNbr")]
        public string v_phone { get; set; }   
    }
}
