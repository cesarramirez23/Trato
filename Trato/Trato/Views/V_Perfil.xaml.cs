using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Trato.Personas;
using Newtonsoft.Json;
using System.Net.Http;

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

            C_Perfil _perfil = new C_Perfil();
            _perfil = new C_Perfil(App.Fn_Vacio(v_Nombre.Text), App.Fn_Vacio(v_Correo.Text), App.Fn_Vacio(v_Domi.Text), App.Fn_Vacio(v_Tel.Text), App.Fn_Vacio(v_Cel.Text),
            App.Fn_Vacio(v_sexo.Text),App.Fn_Vacio(v_sexo.Text), App.Fn_Vacio(v_Alergias.Text), App.Fn_Vacio(v_Operaciones.Text),
            App.Fn_Vacio(v_Enferme.Text), App.Fn_Vacio(v_Medicamentos.Text));
                
                string _jsonPerf = JsonConvert.SerializeObject(_perfil);
                StringContent _content = new StringContent(_jsonPerf, Encoding.UTF8, "application/json");
                HttpClient _client = new HttpClient();
                string _url = "";

                await _client.PostAsync(_url, _content);

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
               
            }
            else
            {
               
                
            }
            
        }
	}
}