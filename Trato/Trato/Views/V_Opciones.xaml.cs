using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trato.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Trato.Personas;
using Trato.Varios;

namespace Trato.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Opciones : ContentPage
	{
        Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\w)[A-Za-z\w]{8,}$");
        public V_Opciones ()
		{
            InitializeComponent();
		}
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarGen();
            App.Fn_CargarDatos();
            C_Membre.Text = App.v_membresia + "  folio: " + App.v_folio;// + "  token " + App.Fn_GEtToken()+"   --aaaaa" ;
            if (App.v_letra == "I")
            { C_Tipo.Text = "Membresia Individual"; }
            else if (App.v_letra == "F")
            { C_Tipo.Text = "Membresia Familiar"; }
            else if (App.v_letra == "E")
            {
                C_Tipo.Text = "Membresia Empresarial";
                //P_actual.Text = "c1e501";
                //P_Nueva.Text = "Cesar1234";
                if (int.Parse(App.v_folio) == 0)
                {
                    C_T_usu.IsVisible = true;
                    C_T_usu.Text = "Total de usuarios: " + App.v_perfil.v_numEmple;
                }
            }
            string[] _Arr = App.v_perfil.v_vig.Split('-');
            C_fecha.Text = _Arr[2] + " - " + _Arr[1] + " - " + _Arr[0];

            Pro_Membre.Text = App.v_membresia;
            Pro_Fol.Text = App.v_folio;
            if(App.v_perfil.v_activo=="1")
            {
                StackTodoPromo.IsVisible = true;
                if (App.v_perfil.v_promotor == "0")
                {
                    Pro_Pass.IsEnabled = true;
                    Pro_Promo.IsEnabled = true;
                    Pro_Mensaje.IsVisible = false;
                    StackApp.IsVisible = false;
                    btnReg.IsEnabled = true;
                }
                else
                {
                    Pro_Pass.IsEnabled = false;
                    Pro_Promo.IsEnabled = false;
                    Pro_Mensaje.IsVisible = true;
                    Pro_Mensaje.Text = "Este usuario ya es promotor";
                    StackApp.IsVisible = true;
                    btnReg.IsEnabled = false;
                }
            }
            else
            {
                StackTodoPromo.IsVisible = false;
            }
        }
        public async Task CargarGen()
        {
            string _noespacios = "";
            string _usutexto = App.v_membresia;
            for (int i = 0; i < _usutexto.Length; i++)
            {
                string _temp = _usutexto[i].ToString();
                if (_temp != " ")
                {
                    _noespacios += _usutexto[i];
                }
            }
            Perf _perf = new Perf();
            _perf.v_fol = App.v_folio;
            _perf.v_membre = _noespacios;
            _perf.v_letra = App.v_letra;
            //crear el json
            string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);
            Console.Write("json para perfil" + _jsonper);
            //await DisplayAlert("enviar datos", _jsonper, "sdfds");
            HttpClient _client = new HttpClient();
            string _DirEnviar = NombresAux.BASE_URL + "query_perfil.php";
            StringContent _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
            try
            {
                //mandar el json con el post
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                //await DisplayAlert("REspuesta", _respuesta, "sdasd");
                C_PerfilGen _nuePer;
                if (string.IsNullOrEmpty(_respuesta))
                {
                    _nuePer = new C_PerfilGen();
                }
                else
                {
                    _nuePer = JsonConvert.DeserializeObject<C_PerfilGen>(_respuesta);
                }
                App.Fn_GuardarDatos(_nuePer, _noespacios, App.v_folio, App.v_letra);
            }
            catch
            {
                await CargarGen();
            }
        }
        public void FN_passCambio(object sender, TextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(P_Nueva.Text) || string.IsNullOrWhiteSpace(P_Nueva.Text))
            {
                P_mensaje.IsVisible = true;
                P_mensaje.Text = "Este campo no puede estar vacio o con espacios";
                P_but.IsEnabled = false;
            }
            else//1804F-0442
            {
                if (!regex.IsMatch(P_Nueva.Text))
                {
                    P_mensaje.IsVisible = true;
                    P_mensaje.Text = "Debe contener al menos una mayuscula,una minuscula y un numero";                                   
                    P_but.IsEnabled = false;
                }
                else
                {
                    P_mensaje.IsVisible = false;                    
                    P_but.IsEnabled = true;
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
                        //await DisplayAlert("respuesta", json, "Aceptar");
                        JObject jsonPer = JObject.Parse(json);
                        StringContent _content = new StringContent(jsonPer.ToString(), Encoding.UTF8, "application/json");
                        HttpClient _client = new HttpClient();
                        string _url = NombresAux.BASE_URL + "password_change.php";
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
                            else
                            {
                                await DisplayAlert("respuesta", _result, "Aceptar");
                            }
                        }
                        catch (Exception exception)
                        {
                            await DisplayAlert("Error", "Error de Conexión", "Aceptar");
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
        /// <summary>
        /// prende apaga el al stak para cambio de contraseña
        /// </summary>
        public void MostrarPass(object sender, EventArgs _Args)
        {
            StackPass.IsVisible = !StackPass.IsVisible;
        }
        private void Fn_VisibleProm(object sender, EventArgs e)
        {
            StackPromotor.IsVisible = !StackPromotor.IsVisible;
        }
        private async void FN_registra(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            _btn.IsEnabled = false;
            if(!Fn_Inputs())//error
            {
                await DisplayAlert("Error", "Uno o mas campos esta incompleto", "Aceptar");
            }
            else
            {
                HttpClient _client = new HttpClient();
                Prom_Reg _reg = new Prom_Reg() { v_membre = Pro_Membre.Text, v_pass = Pro_Pass.Text, v_recom = Pro_Promo.Text ,
                 v_folio = Pro_Fol.Text};
                string _json = JsonConvert.SerializeObject(_reg);
                StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage _respuestaphp = await _client.PostAsync(NombresAux.BASE_URL+"registro_promotor.php", _content);
                    string _respuesta =await _respuestaphp.Content.ReadAsStringAsync();
                    C_Mensaje _men = JsonConvert.DeserializeObject<C_Mensaje>(_respuesta);
                    if(_men.v_code=="1")
                    {
                        await DisplayAlert("Éxito", "Registrado correctamente", "Aceptar");
                        Pro_Mensaje.Text = "Ahora eres promotor, descarga la aplicacion para iniciar sesión";
                        StackApp.IsVisible = true;
                        Pro_Promo.IsEnabled = false;
                        Pro_Pass.IsEnabled = false;
                    }
                    else if(_men.v_code=="0")
                    {
                        _btn.IsEnabled = true;
                    }
                }
                catch
                {
                    await DisplayAlert("Error", "Error, reintentarlo más tarde", "Aceptar");
                }
            }
        }
        bool Fn_Inputs()
        {
            int _cont = 0;
            if(string.IsNullOrEmpty( Pro_Membre.Text) || string.IsNullOrWhiteSpace(Pro_Membre.Text))
            {
                Pro_Membre.BackgroundColor = Color.Red;
                _cont++;
            }
            else
            {
                Pro_Membre.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(Pro_Pass.Text) || string.IsNullOrWhiteSpace(Pro_Pass.Text))
            {
                Pro_Pass.BackgroundColor = Color.Red;
                _cont++;
            }
            else
            {
                Pro_Pass.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(Pro_Fol.Text) || string.IsNullOrWhiteSpace(Pro_Fol.Text))
            {
                Pro_Fol.BackgroundColor = Color.Red;
                _cont++;
            }
            else
            {
                Pro_Fol.BackgroundColor = Color.Transparent;
            }
            if (_cont > 0)
                return false;
            else
                return true;
        }
        private void Fn_AppIos(object sender, EventArgs e)
        {
           Device.OpenUri(new Uri( "https://apps.apple.com/mx/app/te-servicios/id1450966914"));
        }
        private void Fn_AppAndroid(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=com.alsain.teservicios"));
        }
    }
}