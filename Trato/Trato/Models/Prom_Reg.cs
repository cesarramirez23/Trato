using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Trato.Models
{
    public class Prom_Reg
    {
        [JsonProperty("membre")]
        public string v_membre { get; set; }
        [JsonProperty("folio")]
        public string v_folio { get; set; }
        [JsonProperty("pass")]
        public string v_pass { get; set; }
        [JsonProperty("parent")]
        public string v_recom { get; set; }
    }
}
