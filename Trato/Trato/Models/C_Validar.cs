using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Trato.Models
{
    public class C_Validar
    {
        [JsonProperty("activado")]
        public string v_activado { get; set; }//1   0
        /// <summary>
        /// 1 ya se puede pagar
        /// </summary>
        [JsonProperty("renovacion")]
        public string v_renovacion { get; set; }//
        public C_Validar()
        {
            v_activado = "0";
            v_renovacion = "0";
        }
    }
}
