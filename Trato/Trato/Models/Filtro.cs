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
        /// <summary>
        /// x  i del for ,   y 0 especialidad 1 ciudad
        /// </summary>
        public Vec2 v_Id { get; set; }
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
        /// <param name="_posLista">i del for</param>
        /// <param name="_IdLista">0 especialidad 1 ciudad</param>
        public Filtro(string _texto, bool _Activo, int _posLista, int _IdLista)
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
            v_Id= new Vec2(_posLista, _IdLista);
        }
        public void Fn_SetX(int _x)
        {
            v_Id = new Vec2(_x, v_Id.Y);
        }
    }
    public class GroupFiltro:List<Filtro>
    {
        public string v_titulo { get; set; }
        public List<Filtro> v_item { get; set; }
        /// <summary>
        /// 0 especialidad 1 ciudad
        /// </summary>
        public List<string> Fn_GetSelect()
        {
            List<string> _ret = new List<string>();
            for(int i=0; i<this.Count; i++)
            {
                if(this[i].v_visible)
                {
                    _ret.Add(this[i].v_texto);
                }
            }
            return _ret;
        }
    }
}
