using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;


namespace Trato.Varios
{
    public class C_Notificacion
    {
        public C_Notificacion()
        { }
        public C_Notificacion(string _titulo, string _body)
        {

        }
        public C_Notificacion(string _titulo, string _body,string _)
        {

        }

        [JsonProperty("extra")]
        public string v_titulo { get; set; }
        [JsonProperty("extra2")]
        public string v_cuerpo { set; get; }
    }
    public class Pagar
    {
        /*membre
letra
 consecutivo
 costo
 nombre que membresia*/

       [JsonProperty("membre")]
       public string v_membresia { get; set; }
        [JsonProperty("letra")]
       public string v_letra { get; set; }
       [JsonProperty("consecutivo")]
       public string v_conse { get; set; }
       [JsonProperty("costo")]
       public string v_costo { get; set; }
       [JsonProperty("nombre")]
       public string v_nombre { get; set; }

        public Pagar()
        {

        }
        public Pagar(string _membre, string _letra, string _conse,  string _costo, string _nombre)
        {
            v_membresia = _membre;
            v_letra = _letra;
            v_conse = _conse;
            v_costo = _costo;
            v_nombre = _nombre;
        }

    }
    public class Banner
    {
        [JsonProperty("img")]
        public string v_img { get; set; }
        [JsonProperty("sitio")]
        public string v_sitio { get; set; }

        public Banner()
        {
            v_img = "";
            v_sitio = "";
        }
        public Banner(string _img, string _sitio)
        {
            v_img = _img;
            v_sitio = _sitio;
        }
    }
    public class Medicamentos
    {
        [JsonProperty("nombre")]
        public string v_nombre { get; set; }
        [JsonProperty("periodo")]
        /// <summary>
        /// por cuantos dias 
        /// </summary>
        public int v_periodo { get; set; }
        [JsonProperty("tiempo")]
        /// <summary>
        /// cada cuantas horas
        /// </summary>
        public int v_tiempo { get; set; }

        [JsonProperty("extra")]
        public string v_extra { get; set; }

        public string Fn_Info()
        {
            string _info = "";
            _info = "nombre " + v_nombre + "\n periodo " + v_periodo + "\n tiempo " + v_tiempo + "\n extra " + v_extra;
            return _info;
        }
    }
    public class Perf
    {
        [JsonProperty("idmembre")]
        public string v_membre { get; set; }
        [JsonProperty("idfolio")]
        public string v_fol { get; set; }
        [JsonProperty("letra")]
        public string v_letra { get; set; }
        public string Fn_GetDatos()
        {
            string _sa;
            _sa = "membre " + v_membre + "\n" +
                "folio" + v_fol + "\n" +
                "letra" + v_letra+"\n";
            return _sa;
        }
    }
    public class Filtro
    {
        public string v_texto { get; set; }
        public Color v_color { get; set; }

        public Filtro()
        {
            v_texto = "";
            v_color = new Color(.15686274509, 0.58823529411, 0.81960784313);
        }
        public Filtro(string _texto)
        {
            v_texto = _texto;
            v_color = new Color(.15686274509, 0.58823529411, 0.81960784313);
        }
        public Filtro(string _texto, bool _Activo)
        {
            v_texto = _texto;
            if (_Activo)
            {
                v_color = Color.Red;
            }
            else
            {
                v_color = new Color(.15686274509, 0.58823529411, 0.81960784313);
            }
        }
    }
    public class PrefFil
    {
        public string v_texto { get; set; }
    }
}
