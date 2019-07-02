using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Trato.Models
{
    public class C_Mensaje
    {
        [JsonProperty("code")]
        public string v_code { get; set; }
        [JsonProperty("message")]
        public string v_message { get; set; }
    }
}
