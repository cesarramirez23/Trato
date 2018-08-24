using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace Trato.Varios
{
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
    public class Perf
    {
        [JsonProperty("idmembre")]
        public string v_membre { get; set; }
        [JsonProperty("idfolio")]
        public string v_fol { get; set; }
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
