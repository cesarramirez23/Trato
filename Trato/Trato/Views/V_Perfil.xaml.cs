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

using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Perfil : TabbedPage
	{
       

		public V_Perfil ()
		{
			InitializeComponent ();

        }
        public async void Fn_Guardar(object sender, EventArgs _args)
        {

            C_PerfilGen _perfil = new C_PerfilGen();
            _perfil = new C_PerfilGen(App.Fn_Vacio( G_Nombre.Text), App.Fn_Vacio(G_rfc.Text), G_fecha.Date, App.Fn_Vacio(G_lugar.Text), 
                App.Fn_Vacio(G_Ocu.Text),G_sexoPick.SelectedIndex,  App.Fn_Vacio(G_Tel.Text),App.Fn_Vacio(G_Cel.Text), App.Fn_Vacio(G_dom.Text), App.Fn_Vacio(G_ext.Text),
                App.Fn_Vacio(G_inte.Text), App.Fn_Vacio(G_col.Text), App.Fn_Vacio(G_ciu.Text), App.Fn_Vacio(G_mun.Text), App.Fn_Vacio(G_est.Text),
                App.Fn_Vacio(G_cp.Text), App.Fn_Vacio(G_Correo.Text));
            string _jsonPerf = JsonConvert.SerializeObject(_perfil);
            StringContent _content = new StringContent(_jsonPerf, Encoding.UTF8, "application/json");
            HttpClient _client = new HttpClient();
            string _url = "";

            HttpResponseMessage _respuestphp=  await _client.PostAsync(_url, _content);
            string _result = _respuestphp.Content.ReadAsStringAsync().Result;

            await DisplayAlert("Respuesta",_result , "Aceptar");


        }
        public void Fn_PickSexo(object sender, EventArgs _args)
        {
            if(M_sexoPick.SelectedIndex==0)
            {
                M_sexo.IsVisible = false;
                M_sexolbl.IsVisible = false;
                M_sexolbl.Text = "";
                M_sexo.Text = "";
            }
            else if(M_sexoPick.SelectedIndex==1)
            {
                M_sexo.IsVisible = true;
                M_sexolbl.IsVisible = true;
                M_sexolbl.Text = "¿estas embarazada?,\n ¿tienes hijos? ¿cuantos?";

            }
        }

        public void Fn_SwiMedica(object sender, ToggledEventArgs _args)
        {
            M_Medicamentos.IsVisible = _args.Value;
            if (!_args.Value)
                M_Medicamentos.Text = "";
        }
        public void Fn_SwiAlergias(object sender, ToggledEventArgs _args)
        {
            M_Alergias.IsVisible = _args.Value;
            if (!_args.Value)
                M_Alergias.Text = "";
        }
        public void Fn_SwiEnfer(object sender, ToggledEventArgs _args)
        {
            M_Enferme.IsVisible = _args.Value;
            if (!_args.Value)
                M_Enferme.Text = "";
        }
    }
}