using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Xamarin.Forms;
namespace Trato.Models
{
    public class Filtro
    {
        public string v_texto { get; set; }
        public Color v_color { get; set; }
        public bool v_visible { get; set; }
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
}
