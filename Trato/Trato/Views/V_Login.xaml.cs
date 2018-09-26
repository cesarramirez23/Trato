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

using Microsoft.WindowsAzure.MobileServices;


namespace Trato.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Login : ContentPage
    {
        public V_Login()
        {
            InitializeComponent();
            
        }
        //el que muestra la pantalla de registro para familiar o empresarial
        public async void Fn_Registro(object sender, EventArgs _Args)
        {
            await Navigation.PushAsync(new V_Registro(true));
            //await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new V_Registro(true)) );
        }
        public async void Fn_Login(object sender, EventArgs _args)
        {
            if (Fn_Condiciones())
            {
                StackMen.IsVisible = true;
                Mensajes_over.Text = " Comprobando informacion\n";
                string prime = usu.Text.Split('-')[0];
                string _membre = "";
                for(int i=0; i<prime.Length-1; i++)
                {
                    _membre += prime[i];
                }
                string letra = prime[prime.Length - 1].ToString();
                string _conse = usu.Text.Split('-')[1];

                C_Login _login = new C_Login(_membre,letra,_conse, pass.Text,fol.Text);
                //crear el json
                string _jsonLog = JsonConvert.SerializeObject(_login, Formatting.Indented);
                //mostrar la pantalla con mensajes
               // Mensajes_over.Text +=_jsonLog ;
                //crear el cliente
                HttpClient _client = new HttpClient();
                string _DirEnviar = "http://tratoespecial.com/login.php";
                StringContent _content = new StringContent(_jsonLog, Encoding.UTF8, "application/json");
                //mandar el json con el post

                try
                {  //getting exception in the following line    //HttpResponseMessage upd_now_playing = await cli.PostAsync(new Uri("http://ws.audioscrobbler.com/2.0/", UriKind.RelativeOrAbsolute), tunp);
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
                            //cambiar a logeado
                            //StackMen.IsVisible = false;
                            App.v_log = "1";
                            Perf _perf = new Perf();
                            _perf.v_fol = fol.Text;
                            _perf.v_membre = usu.Text;
                            _perf.v_letra = letra;
                            //crear el json
                            string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);
                            //mostrar la pantalla con mensajes
                            //Mensajes_over.Text += "\n" + _jsonper + "\n  valor llega"+_respuesta+"\n";
                            //crear el cliente
                            _client = new HttpClient();
                            _DirEnviar = "http://tratoespecial.com/query_perfil.php";
                            _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");

                            try
                            {
                                //mandar el json con el post
                                _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                                _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                                C_PerfilGen _nuePer = JsonConvert.DeserializeObject<C_PerfilGen>(_respuesta);
                               // await DisplayAlert("Info del perfil", _nuePer.Fn_GetDatos(), "Aceptar");
                                App.Fn_GuardarDatos(_nuePer, usu.Text, fol.Text, letra);
                                _DirEnviar = "http://tratoespecial.com/query_perfil_medico.php";
                                _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                                try
                                {
                                    //mandar el json con el post
                                    _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                                    _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                                    C_PerfilMed _nuePerMEd = new C_PerfilMed();
                                    if(string.IsNullOrEmpty( _respuesta) )
                                    {
                                        _nuePerMEd = new C_PerfilMed(App.v_perfil.v_idsexo);
                                    }
                                    else
                                    {
                                        _nuePerMEd = JsonConvert.DeserializeObject<C_PerfilMed>(_respuesta);
                                    }
                                  //  Mensajes_over.Text ="info medica\n" + _nuePerMEd.Fn_Info();
                                    App.Fn_GuardarDatos(_nuePerMEd, usu.Text, fol.Text,letra);
                                    //cargar la nueva pagina de perfil
                                    string _nombre = (_nuePer.v_Nombre.Split(' ')[0]);
                                    Application.Current.MainPage = new V_Master(true, "Bienvenido " + App.v_perfil.v_Nombre);
                                }
                                catch (HttpRequestException exception)
                                {
                                    await DisplayAlert("Error", exception.Message, "Aceptar");
                                    Reinten.IsVisible = true;
                                }
                            }
                            catch (HttpRequestException exception)
                            {
                                await DisplayAlert("Error", exception.Message, "Aceptar");
                                Reinten.IsVisible = true;
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
                catch (HttpRequestException ex)
                {
                    Mensajes_over.Text = ex.Message.ToString();
                    Reinten.IsVisible = true;
                }
            }
        }
        /// <summary>
        /// apagar el panel de mensajes para reintentar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_args"></param>
        public void Fn_Reintento(object sender, EventArgs _args)
        {
            StackMen.IsVisible = false; 
            Mensajes_over.Text="";
            Reinten.IsVisible = false;
        }
        /// <summary>
        /// si es false, hay algun dato mal y no puedes continuar
        /// </summary>
        /// <returns></returns>
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

            if (_conta > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

    }
}