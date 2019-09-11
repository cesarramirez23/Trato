using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Trato.Varios;
using System.Threading.Tasks;
using Trato.Personas;
using System.Text;
using Trato.Models;
//https://www.c-sharpcorner.com/blogs/store-data-and-access-from-any-pages-in-xamarinform
namespace Trato.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
        public List<Banner> v_mostrar = new List<Banner>();
        int v_actual = 0;
        bool v_cambioban = false;
        public MainPage()
        {
            InitializeComponent();
        }
        private async void Fn_GetMedic()
        {
            HttpClient _client = new HttpClient();
            Cita _cita = new Cita(App.v_membresia, App.v_folio, "0");
            string _json = JsonConvert.SerializeObject(_cita);
            string _DirEnviar = NombresAux.BASE_URL+"get_medicamentos.php";
            //await DisplayAlert("ENVIA PARA medicamentos", _json, "acep");
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            try 
            {
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                    //await DisplayAlert("LLega get medicamentos", _respuesta, "acep");
                   ObservableCollection<C_NotaMed> v_medicamentos = JsonConvert.DeserializeObject<ObservableCollection<C_NotaMed>>(_respuesta);
                    //Console.WriteLine("cuantos "+v_citas.Count+"json citaa " + _respuesta);
                    App.Fn_GuardarMedicamentos(v_medicamentos);
                }
            }
            catch 
            {
                Console.Write("Error request");
            }
        }
        protected async override  void OnAppearing()
        {
            base.OnAppearing();
            if (App.v_log == "1")
            {
                Fn_Fecha();
                Fn_GetMedic();
                Token();
            }
            else
            {
                M_mensaje.IsVisible = false;
            }
            await Task.Delay(1000);
            //M_mensaje.IsVisible = true;
            //M_mensaje.Text += "token " + App.Fn_GEtToken();

            //Console.WriteLine("Refreshed token: " + App.Fn_GEtToken());

            //FN_Red();
            v_cambioban = true;
        }
        protected override void OnDisappearing()
        {
            v_cambioban = false;
            base.OnDisappearing();
        }
        public async void Token()
        {
            string prime = App.v_membresia.Split('-')[0];
            string _membre = "";///los 4 numeros de la mebresia sin laletra
            for (int i = 0; i < prime.Length - 1; i++)//-1 para no agarrar la letra
            {
                _membre += prime[i];
            }
            string _conse = App.v_membresia.Split('-')[1];
            C_Login  _login = new C_Login(_membre, App.v_letra, _conse, App.Fn_GEtToken());
            //crear el json
           string _jsonLog = JsonConvert.SerializeObject(_login, Formatting.Indented);
           string _DirEnviar = NombresAux.BASE_URL + "token_notification.php";
           StringContent _content = new StringContent(_jsonLog, Encoding.UTF8, "application/json");
            HttpClient _client = new HttpClient();
            //Console.WriteLine(" infosss " + _jsonLog);
            try
            {
                //mandar el json con el post
               HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
               string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                if (_respuesta == "1")
                {
                    Console.Write("token 1");
                }
                else
                {
                    Console.WriteLine("token no 1");
                }

            }
            catch
            {
                Console.WriteLine("error token");
            }
        }
        /// <summary>
        /// hace la consulta a la fecha de vigencia para descativarla o no
        /// </summary>
        async void Fn_Fecha()
        {
            App.Fn_CargarDatos();
            Perf _perf = new Perf();
            _perf.v_fol = App.v_folio ;
            _perf.v_membre = App.v_membresia;
            _perf.v_letra = App.v_letra;
            //crear el json
            string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);
            //await DisplayAlert("envia ", _jsonper, "asda");
            //crear el cliente
            HttpClient _client = new HttpClient();
            string _DirEnviar="" ;
            _DirEnviar = NombresAux.BASE_URL + "validacion.php";
            StringContent _content = new StringContent(_jsonper, System.Text.Encoding.UTF8, "application/json");
            try
            {
                //mandar el json con el post
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                //await DisplayAlert("llega validacion", _respuesta, "sdsad");
                App.v_perfil.v_activo = _respuesta;
                App.Fn_GuardarDatos(App.v_perfil, App.v_membresia, App.v_folio, App.v_letra);
                if(_respuesta!="1")
                {
                    M_mensaje.IsVisible = true;
                    M_mensaje.Text = "Aviso \n Cuenta no activada, ve a la seccion de perfil para mas información  ";
                  
                }
                /*
                try
                {
                    _client = new HttpClient();
                    _DirEnviar = "https://tratoespecial.com/query_perfil.php";
                    //mandar el json con el post
                    _respuesta = "";
                    _respuestaphp = new HttpResponseMessage();
                    _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                    _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                    await DisplayAlert("llega ", _respuesta, "sdsad");
                    C_PerfilGen _nuePer = JsonConvert.DeserializeObject<C_PerfilGen>(_respuesta);
                    await DisplayAlert("Info del perfil", _nuePer.Fn_GetDatos(), "Aceptar");

                    //App.Fn_GuardarDatos(_respuesta,App.v_membresia, App.v_folio, App.v_letra);
                    App.Fn_GuardarDatos(_nuePer, App.v_membresia, App.v_folio, App.v_letra);
                    _DirEnviar = "http://tratoespecial.com/query_perfil_medico.php";
                    _content = new StringContent(_jsonper, System.Text.Encoding.UTF8, "application/json");
                    try
                    {
                        //mandar el json con el post
                        _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                        _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                        C_PerfilMed _nuePerMEd = JsonConvert.DeserializeObject<C_PerfilMed>(_respuesta);
                        App.Fn_GuardarDatos(_nuePerMEd, App.v_membresia, App.v_folio, App.v_letra);
                        //string _as = Application.Current.Properties[NombresAux.v_perfGen] as string;

                        App.Fn_CargarDatos();
                        await DisplayAlert("activacion", "asasas  " + App.v_perfil.Fn_GetDatos()+"\n activ " +App.v_perfil.v_activo, "sads");

                        if (App.v_perfil.v_activo != "1" && App.v_folio=="0")
                        {
                            M_mensaje.IsVisible = true;
                            M_mensaje.Text = "Aviso \n Cuenta no activada, ve a la seccion de perfil para más información  ";
                        }else if(App.v_perfil.v_activo != "1" && App.v_folio != "0")
                        {
                            M_mensaje.IsVisible = true;
                            M_mensaje.Text = "Aviso \n Cuenta no activada, Contacta al propietario de la memebresía para más información";
                        }

                    }
                    catch (HttpRequestException exception)
                    {
                        await DisplayAlert("Error", exception.Message, "Aceptar");
                        App.Fn_CargarDatos();
                        if (App.v_perfil.v_activo != "1")
                        {
                            M_mensaje.IsVisible = true;
                            M_mensaje.Text = "Aviso \n Cuenta no activada, ve a la seccion de perfil para mas información  ";
                        }
                    }
                }
                catch (HttpRequestException exception)
                {
                    await DisplayAlert("Error", exception.Message, "Aceptar");
                    App.Fn_CargarDatos();
                    if (App.v_perfil.v_activo != "1")
                    {
                        M_mensaje.IsVisible = true;
                        M_mensaje.Text = "Aviso \n Cuenta no activada, ve a la seccion de perfil para mas información  ";
                    }
                }
                */
            }
            catch
            {
                App.Fn_CargarDatos();
                if (App.v_perfil.v_activo != "1")
                {
                    M_mensaje.IsVisible = true;
                    M_mensaje.Text = "Aviso \n Cuenta no activada, ve a la seccion de perfil para mas información  ";
                }
            }
        }
        /*
       public async void FN_Red()
        {
            HttpClient _cliente= new HttpClient();
            string url = " s";
            try
            {
                HttpResponseMessage _respuestphp=  await _cliente.PostAsync(url,null);
                string _respu =await  _respuestphp.Content.ReadAsStringAsync();
                List<Banner> _banner = JsonConvert.DeserializeObject<List<Banner>>(_respu);
                v_mostrar = _banner;
                //Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                //{

                //    return true;
                //});
                //while(v_cambioban)
                //{
                //    v_actual++;
                //    if (v_actual == v_mostrar.Count) v_actual = 0;

                //    MainBanner.Source = v_mostrar[v_actual].v_img;
                //    await Task.Delay(10000);
                //}
            }
            catch
            {
                v_mostrar.Clear();
                v_mostrar.Add(new Banner("HOME_icon.png", "http://forums.xamarin.com/discussion/82989/implementation-of-auto-slider-for-carousal-view-xamarin-forms"));
                v_mostrar.Add(new Banner("Medicos.png", "http://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.carouselpage?view=xamarin-forms"));
                v_mostrar.Add(new Banner("Services_icon.png", "http://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.multipage-1.oncurrentpagechanged?view=xamarin-forms#Xamarin_Forms_MultiPage_1_OnCurrentPageChanged"));
                v_mostrar.Add(new Banner("Membresia_Icon.png", "http://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.device.starttimer?view=xamarin-forms"));
                v_mostrar.Add(new Banner("LOGOTRATOESPECIAL.png", "http://tratoespecial.com/"));

                v_actual = 0;
                MainBanner.Source = v_mostrar[v_actual].v_img;

                //Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                //{

                //    return true;
                //});
                while (v_cambioban)
                {
                    v_actual++;
                    if (v_actual == v_mostrar.Count) v_actual = 0;

                    MainBanner.Source = v_mostrar[v_actual].v_img;
                    await Task.Delay(10000);
                }
            }
            
        }
        */

        public void Fn_AbrirSitio(object sender, EventArgs _args)
        {
            //Uri _url= new Uri(v_mostrar[v_actual].v_sitio);
            Uri _url= new Uri("https://server.anaseguros.com.mx/Micrositios/TRATOESPECIAL/Cotizador.html");
            Device.OpenUri(_url);
        }


    }
}