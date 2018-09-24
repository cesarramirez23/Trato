using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Net.Http;



namespace Trato.Views
{
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Opciones : TabbedPage
	{
        Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\w)[A-Za-z\w]{8,}$");
        public V_Opciones ()
		{
			InitializeComponent ();
            C_Membre.Text = App.v_membresia+ "  folio: "+ App.v_folio;

            if (App.v_letra == "I")
            { C_Tipo.Text = "Membresia Individual"; }
            else if (App.v_letra == "F")
            { C_Tipo.Text = "Membresia Familiar"; }
            else if (App.v_letra == "E"  )
            { C_Tipo.Text = "Membresia Empresarial";
                if( int.Parse(App.v_folio) == 0)
                {
                    C_T_usu.IsVisible = true;
                    C_T_usu.Text = "Total de usuarios: " + App.v_perfil.v_numEmple;
                }
            }
            string[] _Arr = App.v_perfil.v_vig.Split('-');
            C_fecha.Text = _Arr[2] + " - " + _Arr[1] + " - " + _Arr[0];
           
		}
        public void FN_passCambio(object sender, TextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(P_Nueva.Text) || string.IsNullOrWhiteSpace(P_Nueva.Text))
            {
                P_mensaje.IsVisible = true;
                P_mensaje.Text = "Este campo no puede estar vacio o con espacios";
            }
            else
            {
                if (!regex.IsMatch(P_Nueva.Text))
                {
                    P_mensaje.IsVisible = true;
                    P_mensaje.Text = "Debe contener al menos una mayuscula,una minuscula y un numero";                                   
                }
                else
                {
                    P_mensaje.IsVisible = false;                    
                }
            }
        }
        public async void Fn_CambioPass(object sender, EventArgs _args)
        {
            Button _buton = (Button)sender;
            _buton.IsEnabled = false;
            if (string.IsNullOrEmpty(P_Nueva.Text) || string.IsNullOrWhiteSpace(P_Nueva.Text))
            {
                P_actual.BackgroundColor = Color.Red;
            }
            else
            {
                if(Fn_validar(P_actual.Text, P_Nueva.Text))
                {
                    P_actual.BackgroundColor = Color.Transparent;

                    if (string.IsNullOrEmpty(P_Nueva.Text) || string.IsNullOrWhiteSpace(P_Nueva.Text))
                    {
                        P_mensaje.IsVisible = true;
                        P_mensaje.Text = "Este campo no puede estar vacio o con espacios";
                    }
                    else
                    {
                        P_mensaje.IsVisible = false;
                        string prime = App.v_membresia.Split('-')[0];
                        string _membre = "";
                        for (int i = 0; i < prime.Length - 1; i++)
                        {
                            _membre += prime[i];
                        }
                        string letra = prime[prime.Length - 1].ToString();
                        string _conse = App.v_membresia.Split('-')[1];

                        string json = @"{";
                        json += "membre:'" + _membre + "',\n";
                        json += "folio:'" + App.v_folio + "',\n";
                        json += "letra:'" + letra + "',\n";
                        json += "consecutivo:'" + _conse + "',\n";
                        json += "password:'" + P_actual.Text + "',\n";
                        json += "newpassword:'" + P_Nueva.Text + "',\n";
                        json += "}";

                        JObject jsonPer = JObject.Parse(json);
                        StringContent _content = new StringContent(jsonPer.ToString(), Encoding.UTF8, "application/json");
                        HttpClient _client = new HttpClient();
                        string _url = "https://useller.com.mx/trato_especial/password_change.php";
                        try
                        {
                            HttpResponseMessage _respuestphp = await _client.PostAsync(_url, _content);
                            string _result = _respuestphp.Content.ReadAsStringAsync().Result;
                            if(_result=="1")
                            {
                                await DisplayAlert("Exito", "Cambio de contraseña exitoso", "Aceptar");
                                P_actual.Text = "";
                                P_Nueva.Text = "";
                                P_mensaje.Text = "";
                                P_mensaje.IsVisible = false;
                            }
                            else if(_result=="8")
                            {
                                await DisplayAlert("Error", "No se pudo actualizar, por favor intentalo mas tarde", "Aceptar");
                            }
                            else if(_result=="9")
                            {
                                await DisplayAlert("Error", "La información proporcionada como contraseña actual, no coincide con la información del usuario", 
                                    "Aceptar");
                            }
                            else if (_result=="10")
                            {
                                await DisplayAlert("respuesta", "Usuario no encontrado, por favor intentalo mas tarde ", "Aceptar");
                            }
                        }
                        catch (HttpRequestException exception)
                        {
                            await DisplayAlert("Error", exception.Message, "Aceptar");
                        }
                    }
                }
                else
                {
                    P_mensaje.IsVisible = true;
                }
            }
            _buton.IsEnabled = true;
        }

        public bool Fn_validar(string _actual, string _nueva)
        {
            if (_actual == _nueva)
            {
                P_mensaje.Text = "La nueva contraseña no puede ser la misma que la actual";
                return false;
            }
            else
            {
                if (!regex.IsMatch(_nueva))
                {
                    P_mensaje.Text = "Debe contener al menos una mayuscula,una minuscula y un numero";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}