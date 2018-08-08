using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms; 
using Xamarin.Forms.Xaml;

using Trato.Personas;
using Newtonsoft.Json;//json normal
using Newtonsoft.Json.Linq;// parse de string a jobject
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
            CargarGen();
        }
        public async void CargarGen()
        {
            Fn_NullEntry( G_Nombre, App.v_perfil.v_Nombre);
            Fn_NullEntry(G_rfc,App.v_perfil.v_Rfc);
            if (string.IsNullOrEmpty(App.v_perfil.v_FecNaci))
            {
                G_fecha.Date = DateTime.Now;
                G_fecha.IsEnabled = true;
            }
            else
            {
                string[] fecha = App.v_perfil.v_FecNaci.Split('-');
                G_fecha.Date = new DateTime(int.Parse(fecha[0]), int.Parse(fecha[1]), int.Parse(fecha[2]));
                G_fecha.IsEnabled = false;
            }
            if ((App.v_perfil.v_idsexo<=0)&& (App.v_perfil.v_idsexo>=1) )
            {
                G_sexoPick.IsEnabled = true;
            }
            else
            {
                G_sexoPick.SelectedIndex = App.v_perfil.v_idsexo;
                G_sexoPick.Title = G_sexoPick.SelectedIndex.ToString();
                G_sexoPick.IsEnabled = false;
            }

            Fn_NullEntry(G_lugar, App.v_perfil.v_LugNac);
            Fn_NullEntry(G_Ocu, App.v_perfil.v_Ocup);
            Fn_NullEntry(G_Tel, App.v_perfil.v_Tel);
            Fn_NullEntry(G_Cel, App.v_perfil.v_Cel);
            Fn_NullEntry(G_dom, App.v_perfil.v_Calle);
            Fn_NullEntry(G_ext,App.v_perfil.v_NumExt);
            Fn_NullEntry(G_inte,App.v_perfil.v_NumInt);
            Fn_NullEntry(G_col, App.v_perfil.v_Colonia);
            Fn_NullEntry(G_ciu,App.v_perfil.v_Ciudad);
            Fn_NullEntry(G_mun, App.v_perfil.v_municipio);
            Fn_NullEntry(G_est,App.v_perfil.v_Estado);
            Fn_NullEntry(G_cp,App.v_perfil.v_Cp);
            Fn_NullEntry(G_Correo,App.v_perfil.v_Correo);

            await Task.Delay(100);
        }
        public void Fn_NullEntry(Entry _entry, string _textos)
        {
            if (string.IsNullOrEmpty(_textos))
            {
                _entry.Text = "";
            }
            else
            {
                _entry.Text = _textos;
            }
        }

        public async void Fn_Guardar(object sender, EventArgs _args)
        {
            Button _buton = (Button)sender;
            _buton.IsEnabled = false;

            //            string json = @"{
            //  CPU: 'Intel',
            //  Drives: [
            //    'DVD read/writer',
            //    '500 gigabyte hard drive'
            //  ]
            //}";

            string json = @"{";

            json += "idmembre:'" + App.v_membresia+ "',\n";
            json += "idfolio:'" + App.v_folio + "',\n";

            json += "nombre:'" + App.Fn_Vacio(G_Nombre.Text) + "',\n";
            json += "rfc:'" + App.Fn_Vacio(G_rfc.Text) + "',\n";
            json += "fechanac:'" + Fn_GetFecha() + "',\n";
            json += "lugnac:'" + App.Fn_Vacio(G_lugar.Text) + "',\n";
            json += "ocu:'" + App.Fn_Vacio(G_Ocu.Text) + "',\n";
            json += "idsexo:'" + G_sexoPick.SelectedIndex + "',\n";
            json += "tel:'" + App.Fn_Vacio(G_Tel.Text) + "',\n";
            json += "cel:'" + App.Fn_Vacio(G_Cel.Text) + "',\n";
            json += "calle:'" + App.Fn_Vacio(G_dom.Text) + "',\n";
            json += "numext:'" + App.Fn_Vacio(G_ext.Text) + "',\n";
            json += "numint:'" + App.Fn_Vacio(G_inte.Text) + "',\n";
            json += "colonia:'" + App.Fn_Vacio(G_col.Text) + "',\n";
            json += "ciudad:'" + App.Fn_Vacio(G_ciu.Text) + "',\n";
            json += "municipio:'" + App.Fn_Vacio(G_mun.Text) + "',\n";
            json += "estado:'" + App.Fn_Vacio(G_est.Text) + "',\n";
            json += "cp:'" + App.Fn_Vacio(G_cp.Text) + "',\n";
            json += "correo:'" + App.Fn_Vacio(G_Correo.Text) + "',\n";
            json += "}";




            //C_PerfilGen _perfil = new C_PerfilGen();
            //_perfil = new C_PerfilGen(App.Fn_Vacio( G_Nombre.Text), App.Fn_Vacio(G_rfc.Text), 
            //G_fecha.Date, App.Fn_Vacio(G_lugar.Text), 
            //App.Fn_Vacio(G_Ocu.Text),G_sexoPick.SelectedIndex,  App.Fn_Vacio(G_Tel.Text),
            //App.Fn_Vacio(G_Cel.Text), App.Fn_Vacio(G_dom.Text), App.Fn_Vacio(G_ext.Text),
            //App.Fn_Vacio(G_inte.Text), App.Fn_Vacio(G_col.Text), App.Fn_Vacio(G_ciu.Text), 
            //App.Fn_Vacio(G_mun.Text), App.Fn_Vacio(G_est.Text),
            //App.Fn_Vacio(G_cp.Text), App.Fn_Vacio(G_Correo.Text));
            //string _jsonPerf = JsonConvert.SerializeObject(_perfil);
            //StringContent _content = new StringContent(_jsonPerf, Encoding.UTF8, "application/json");

            JObject jsonPer = JObject.Parse(json);

            await DisplayAlert("Json ", jsonPer.ToString(), "Aceptar");

            StringContent _content = new StringContent(jsonPer.ToString(), Encoding.UTF8, "application/json");
            HttpClient _client = new HttpClient();
            string _url = "https://useller.com.mx/trato_especial/update_perfil.php";
            HttpResponseMessage _respuestphp=  await _client.PostAsync(_url, _content);
            string _result = _respuestphp.Content.ReadAsStringAsync().Result;
            await DisplayAlert(_respuestphp.StatusCode.ToString(),  _result , "Aceptar");




            _buton.IsEnabled = true;
        }

        public string Fn_GetFecha()
        {
            string v_FecNaci = "";
            string _month = "";
            if (G_fecha.Date.Month < 10)
            {
                _month = "0" + G_fecha.Date.Month.ToString();
            }
            else
            {
                _month = G_fecha.Date.Month.ToString();
            }
            string _day = "";
            if (G_fecha.Date.Day < 10)
            {
                _day = "0" + G_fecha.Date.Day.ToString();
            }
            else
            {
                _day = G_fecha.Date.Day.ToString();
            }
            v_FecNaci = G_fecha.Date.Year.ToString() + "-" + _month + "-" + _day;
            return v_FecNaci;
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