using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Trato.Models
{
    public class C_Notificacion
    {
        public C_Notificacion()
        { }
        public C_Notificacion(string _titulo, string _body)
        {
            v_titulo = _titulo;
            string temp = _body.Replace("\n", Environment.NewLine);
            v_cuerpo = temp;
        }
        [JsonProperty("estado")]
        public string v_estado { get; set; }
        //Lo que se vaa a mostrar
        [JsonProperty("title")]
        public string v_titulo { get; set; }
        [JsonProperty("message")]
        public string v_cuerpo { set; get; }
    }
}
