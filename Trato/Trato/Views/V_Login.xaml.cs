using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Personas;
using System.Net.Http;
using Trato.Varios;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Trato.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Login : ContentPage
    {
        public V_Login()
        {
            InitializeComponent();
            //usu.Text = "1810I-0558 ";
            //pass.Text = "Cesar1234";
            //fol.Text = "0";
        }
        //el que muestra la pantalla de registro para familiar o empresarial
        public async void Fn_Registro(object sender, EventArgs _Args)
        {
            await Navigation.PushAsync(new V_Registro(true,0));
            //await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new V_Registro(true)) );
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
        public async void Fn_Login(object sender, EventArgs _args)
        {
            if (Fn_Condiciones())
            {
                StackPrincipal.IsVisible = false;
                StackMen.IsVisible = true;
                Mensajes_over.Text = " Comprobando informacion\n";
                Regex MembreRegex = new Regex(@"^([0-9]){4}([A-Z]){1}-([0-9]){4}$");
                if (MembreRegex.IsMatch(usu.Text))
                {
                    string prime = usu.Text.Split('-')[0];
                    string _membre = "";///los 4 numeros de la mebresia sin laletra
                    for(int i=0; i<prime.Length-1; i++)
                    {
                        _membre += prime[i];
                    }
                    string letra = prime[prime.Length - 1].ToString();
                    string _conse = usu.Text.Split('-')[1];
                    C_Login _login = new C_Login(_membre,letra,_conse, pass.Text,fol.Text);
                    string _jsonLog = JsonConvert.SerializeObject(_login, Formatting.Indented);
                    HttpClient _client = new HttpClient();
                    string _DirEnviar = NombresAux.BASE_URL + "login.php";
                    StringContent _content = new StringContent(_jsonLog, Encoding.UTF8, "application/json");
                    try
                    {
                        HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                        if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                            if (_respuesta == "0")
                            {
                                Mensajes_over.Text += "\n Error en los datos";
                                Reinten.IsVisible = true;
                            }
                            else if (_respuesta == "1" || _respuesta == "2")
                            {
                                string _noespacios = "";
                                string _usutexto = usu.Text;
                                for(int i=0; i<_usutexto.Length;i++)
                                {
                                    string _temp= _usutexto[i].ToString();
                                    if(_temp!=" ")
                                    {
                                        _noespacios += _usutexto[i];
                                    }
                                }
                                Perf _perf = new Perf();
                                _perf.v_fol = fol.Text;
                                _perf.v_membre = _noespacios;
                                _perf.v_letra = letra;
                                string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);
                                string _respPerf = await Fn_GetPerfil(new StringContent(_jsonper, Encoding.UTF8, "application/json"),
                                    _noespacios,letra);

                                if (!string.IsNullOrEmpty(_respPerf))//error
                                {
                                    Mensajes_over.Text = "Error al inciar sesión";
                                    Reinten.IsVisible = true;
                                }
                                else
                                {
                                    string _med =await Fn_GetMedico(new StringContent(_jsonper, Encoding.UTF8, "application/json"),
                                         _noespacios, letra);
                                    if (!string.IsNullOrEmpty(_med))//error
                                    {
                                        Mensajes_over.Text = "Error al inciar sesión";
                                        Reinten.IsVisible = true;
                                    }
                                    else
                                    {
                                        string _token = await Fn_SetToken(_membre, letra, _conse);
                                        if (!string.IsNullOrEmpty(_token))//error
                                        {
                                            Mensajes_over.Text = "Error al inciar sesión";
                                            Reinten.IsVisible = true;
                                        }
                                        else
                                        {
                                            App.v_log = "1";
                                            Application.Current.MainPage = new V_Master(true, "Bienvenido " + App.v_perfil.v_Nombre);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                //Mensajes_over.Text = "otra cosa que no entra en el if " + _respuesta;
                                Mensajes_over.Text = "Error";
                                Reinten.IsVisible = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Mensajes_over.Text = "Error al inciar sesión";
                        Reinten.IsVisible = true;
                    }
                }
                else
                {
                    //Mensajes_over.Text = "otra cosa que no entra en el if " + _respuesta;
                    Mensajes_over.Text = "No contiene el formato de membresia\n 0000F-0000";
                    Reinten.IsVisible = true;
                }
            }
            else
            {
                await DisplayAlert("Error", "Uno o mas campos se encuentran vacios", "Aceptar");
            }
        }
        async Task<string> Fn_GetMedico(StringContent _content, string _membre, string _letra)
        {
            string _DirEnviar = NombresAux.BASE_URL + "query_perfil_medico.php";
            HttpClient _client = new HttpClient();
            try
            {
                //mandar el json con el post
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                C_PerfilMed _nuePerMEd = new C_PerfilMed();
                if (string.IsNullOrEmpty(_respuesta))
                {
                    _nuePerMEd = new C_PerfilMed(App.v_perfil.v_idsexo);
                }
                else
                {
                    _nuePerMEd = JsonConvert.DeserializeObject<C_PerfilMed>(_respuesta);
                }
                App.Fn_GuardarDatos(_nuePerMEd, _membre, fol.Text, _letra);
                return string.Empty;

               
            }
            catch (Exception exception)
            {
                return "Error al inciar sesión";
                //await DisplayAlert("Error", "Error al inciar sesión", "Aceptar");
                //Reinten.IsVisible = true;
            }
        }
        async Task<string> Fn_SetToken(string _membre, string _letra, string _conse)
        {
            //cargar la nueva pagina de perfil
            //string _nombre = (_nuePer.v_Nombre.Split(' ')[0]);
            HttpClient _client = new HttpClient();
            C_Login _login = new C_Login(_membre, _letra, _conse, App.Fn_GEtToken());
            //crear el json
            string _jsonLog = JsonConvert.SerializeObject(_login, Formatting.Indented);
            string _DirEnviar = NombresAux.BASE_URL + "token_notification.php";
            StringContent _content = new StringContent(_jsonLog, Encoding.UTF8, "application/json");
            try
            {
                //mandar el json con el post
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                if (_respuesta == "1")
                {
                    return string.Empty;
                    //App.v_log = "1";
                    //Application.Current.MainPage = new V_Master(true, "Bienvenido " + App.v_perfil.v_Nombre);
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception exception)
            {
                return "Error al inciar sesión";
            }
        }
        async Task<string> Fn_GetPerfil(StringContent _content, string _membre, string _letra)
        {
            HttpClient _client = new HttpClient();
            string _DirEnviar = NombresAux.BASE_URL + "query_perfil.php";
            try
            {
                //mandar el json con el post
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                C_PerfilGen _nuePer = JsonConvert.DeserializeObject<C_PerfilGen>(_respuesta);
                App.Fn_GuardarDatos(_nuePer, _membre, fol.Text, _letra);
                return string.Empty;
            }
            catch (Exception exception)
            {
                return "Error al inciar sesión";
                //await DisplayAlert("Error", "Error al inciar sesión", "Aceptar");
                //Reinten.IsVisible = true;
            }
        }
        /// <summary>
        /// apagar el panel de mensajes para reintentar el formulario
        /// </summary>
        public void Fn_Reintento(object sender, EventArgs _args)
        {
            StackMen.IsVisible = false; 
            Mensajes_over.Text="";
            Reinten.IsVisible = false;
            StackPrincipal.IsVisible = true;
        }
        /// <summary>
        /// para no enviar basura solo numeros
        /// </summary>
        void Fn_SoloNumero(object sender, TextChangedEventArgs _args)
        {
            Entry _entry = (Entry)sender;
            if (_entry.Text.Length > 0)
            {
                char _ultimo = _entry.Text[_entry.Text.Length - 1];
                if (_ultimo == '-' || _ultimo == ',' || _ultimo == '_' || _ultimo == '.')
                {
                    if (_entry.Text.Length == 1)
                    {
                        _entry.Text = "";
                    }
                    else
                    {
                        _entry.Text = _entry.Text.Remove(_entry.Text.Length - 1); // remove last char
                    }
                }
            }
        }
        /// <summary>
        /// si es false, hay algun dato mal y no puedes continuar
        /// </summary>
        bool Fn_Condiciones()
        {
            int _conta = 0;
            if (string.IsNullOrEmpty(usu.Text) || string.IsNullOrWhiteSpace(usu.Text))
            {
                usu.BackgroundColor = Color.Red; 
                _conta++;
            }
            else
            {
                usu.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(pass.Text) || string.IsNullOrWhiteSpace(pass.Text))
            {
                pass.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                pass.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(fol.Text) || string.IsNullOrWhiteSpace(fol.Text))
            {
                fol.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                fol.BackgroundColor = Color.Transparent;
            }
            if (_conta > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #region CAMBIO PASS
        /// <summary>
        /// mostrar panel pass
        /// </summary>
        public void Fn_Reenviar(object sender, EventArgs _args)
        {
            StackContra.IsVisible = true;
            StackPrincipal.IsVisible = false;
          //  PassCorreo.Text = "";
           // PassMembre.Text = "";
        }
        /// <summary>
        /// apaga el panel de contraseña
        /// </summary>
        public void Fn_APagaPass(object sender, EventArgs _args)
        {
            StackContra.IsVisible = false;
            StackPrincipal.IsVisible = true;
        }
        private async void Fn_CambioPass(object sender, EventArgs _args)
        {
            Button _buton = (Button)sender;
            _buton.IsEnabled = false;
            Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (string.IsNullOrEmpty(PassCorreo.Text) || string.IsNullOrWhiteSpace(PassCorreo.Text) || !EmailRegex.IsMatch(PassCorreo.Text))
            {
                await DisplayAlert("Error", "Correo vacio o no contiene formato correcto", "Reintentar");
            }
            else if (string.IsNullOrEmpty(PassMembre.Text) || string.IsNullOrWhiteSpace(PassMembre.Text))
            {
                await DisplayAlert("Error", "Error en Membresia", "Reintentar");
            }
            else if (string.IsNullOrEmpty(PassFol.Text) || string.IsNullOrWhiteSpace(PassFol.Text))
            {
                await DisplayAlert("Error", "Error en Folio", "Reintentar");
            }
            else
            {
                                             //4 numeros  1 letra  guion   4 numeros o mas
                Regex MembreRegex = new Regex(@"^([0-9]){4}([A-Z]){1}-([0-9]){4}$");
                if (MembreRegex.IsMatch(PassMembre.Text))
                {
                    string prime = PassMembre.Text.Split('-')[0];
                    string _membre = "";///los 4 numeros de la mebresia sin laletra
                    for (int i = 0; i < prime.Length - 1; i++)
                    {
                        _membre += prime[i];
                    }
                    string letra = prime[prime.Length - 1].ToString();
                    string _conse = PassMembre.Text.Split('-')[1];
                    string json = @"{";
                    json += "correo:'" + App.Fn_Vacio(PassCorreo.Text) + "',\n";
                    json += "membre:'" + App.Fn_Vacio(_membre) + "',\n";
                    json += "letra:'" + App.Fn_Vacio(letra) + "',\n";
                    json += "consecutivo:'" + App.Fn_Vacio(_conse) + "',\n";
                    json += "folio:'" + App.Fn_Vacio(PassFol.Text) + "'\n";
                    json += "}";
                    JObject jsonper = JObject.Parse(json);
                    string _DirEnviar = NombresAux.BASE_URL + "restore_pass.php";
                    StringContent _content = new StringContent(jsonper.ToString(), Encoding.UTF8, "application/json");
                    HttpClient _client = new HttpClient();
                    try
                    {
                        HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                        string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                        if (_respuesta == "0")
                        {
                            await DisplayAlert("Error", "Falló el correo", "Aceptar");
                        }
                        else if (_respuesta == "1")
                        {
                            await DisplayAlert("Exito", "Correo enviado correctamente", "Aceptar");
                            Fn_APagaPass(null, null);
                        }
                        else if (_respuesta == "32")
                        {
                            await DisplayAlert("Error", "membresia no coincide con el correo", "Aceptar");
                        }
                        else
                        {
                            await DisplayAlert("Error", _respuesta, "Aceptar");
                        }
                    }
                    catch (Exception exception)
                    {
                        await DisplayAlert("Error", "Error de Conexión", "Aceptar");
                        Reinten.IsVisible = true;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No contiene el formato de membresia\n0000F-0000", "Aceptar");
                }
            }
            _buton.IsEnabled = true;
        }
        #endregion
    }
}