using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Trato.Models
{
    public class C_EspeTitu
    {
        [JsonProperty("ID_especialidad")]
        public string v_idespecial { get; set; }
        [JsonProperty("nombre_especialidad")]
        public string v_nombreEspec { get; set; }
        [JsonProperty("ID_titulo")]
        public string v_idtitulo { get; set; }
        [JsonProperty("nombre_titulo")]
        public string v_nombreTitulo { get; set; }
        public bool v_visible { get; set; }
    }
}
