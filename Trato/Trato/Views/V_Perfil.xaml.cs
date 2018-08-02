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
namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Perfil : ContentPage
	{
        string _validar = "";

		public V_Perfil ()
		{
			InitializeComponent ();
		}
        public V_Perfil(int _valor)
        {
            InitializeComponent();
            switch (_valor)
            {
                case 0:
                    StackGeneral.IsVisible = true;
                    break;
                case 1:
                    StackMedi.IsVisible = true;
                    break;
                case 2:
                    StackPass.IsVisible = true;
                    break;
                default:
                    break;
            }
        }

        public async void Fn_Guardar(object sender, EventArgs _args)
        {

            C_Perfil _perfil = new C_Perfil();
            _perfil = new C_Perfil(App.Fn_Vacio(G_Nombre.Text), App.Fn_Vacio(G_Correo.Text), App.Fn_Vacio(G_Domi.Text), App.Fn_Vacio(G_Tel.Text), App.Fn_Vacio(G_Cel.Text),
            App.Fn_Vacio(M_Sangre.Text),App.Fn_Vacio(M_sexo.Text), App.Fn_Vacio(M_Alergias.Text), App.Fn_Vacio(M_Operaciones.Text),
            App.Fn_Vacio(M_Enferme.Text), App.Fn_Vacio(M_Medicamentos.Text));

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

        public void FN_passCambio(object sender, TextChangedEventArgs args)
        {
            if(string.IsNullOrEmpty( P_Nueva.Text) || string.IsNullOrWhiteSpace(P_Nueva.Text))
            {
                P_but.IsEnabled = false;
                P_mensaje.IsVisible = true;
                P_mensaje.Text = "Este campo no puede estar vacio o con espacios";
            }
            else
            {
                if(P_Nueva.Text != P_Nueva2.Text)
                {
                    P_but.IsEnabled = false;
                    P_mensaje.Text = "Contraseña no coincide";
                }
                else
                {
                    P_but.IsEnabled = true;
                    P_mensaje.Text = "Contraseña correcta";

                }
            }
        }
        public async void Fn_CambioPass(object sender, EventArgs _args)
        {
            if(Fn_validar(P_actual.Text, P_Nueva.Text))
            {
                await DisplayAlert("bien", "bien", "bien");
            }
            else
            {
                await DisplayAlert("Error", _validar, "Aceptar");

            }
        }
        public bool Fn_validar(string _actual, string _nueva)
        {
            if(_actual == _nueva)
            {
                _validar = "La nueva contraseña no puede ser la misma que la actual";
                return false;
            }
            else
            {
                Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\w)[A-Za-z\w]{8,}$");
                if(!regex.IsMatch(_nueva))
                {
                    _validar = "DEbe contener al menos una mayuscula,una minuscula y un numero";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        /*PARA CREAR LA CONSTRASEÑA SIN NECESIDAD DE CLASES
         * 
         * string json = @"{
          CPU: 'Intel',
          Drives: [
            'DVD read/writer',
            '500 gigabyte hard drive'
          ]
        }";

        JObject o = JObject.Parse(json);
         
         
         */
    }
}