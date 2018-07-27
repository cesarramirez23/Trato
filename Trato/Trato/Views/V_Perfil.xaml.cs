using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Perfil : ContentPage
	{

		public V_Perfil ()
		{
			InitializeComponent ();
		}

        public async void Fn_Guardar(object sender, EventArgs _args)
        {

        }
        public void Fn_SwiMedica(object sender, ToggledEventArgs _args)
        {
            v_Medicamentos.IsVisible = _args.Value;
            if (!_args.Value)
                v_Medicamentos.Text = "";
        }
        public void Fn_SwiAlergias(object sender, ToggledEventArgs _args)
        {
            v_Alergias.IsVisible = _args.Value;
            if (!_args.Value)
                v_Alergias.Text = "";
        }
        public void Fn_SwiEnfer(object sender, ToggledEventArgs _args)
        {
            v_Enferme.IsVisible = _args.Value;
            if (!_args.Value)
                v_Enferme.Text = "";
        }

        public void Fn_SwiSexo(object sender, ToggledEventArgs _args)
        {
            if(_args.Value)
            {
                v_sexolbl.Text = "Mujer, ¿Estas Embarazada?, ¿Tienes hijos? ¿Cuantos?";
            }
            else
            {
                v_sexolbl.Text = "Hombre";
                v_sexo.Text = "";
            }
            v_sexo.IsVisible = _args.Value;
        }
	}
}