using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using Trato.Varios;
using System.Threading.Tasks;
using Trato.Personas;
namespace Trato.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public List<Banner> v_mostrar = new List<Banner>();
        int v_actual = 0;
        bool v_cambioban = false;
        public MainPage()
        {
            InitializeComponent();
            if (App.v_log == "1")
            {
                Fn_Fecha();
            }
            else
            {
                M_mensaje.IsVisible = false;
            }
            M_mensaje.IsVisible = true;
            M_mensaje.Text += "token " + App.Fn_GEtToken();
            Console.WriteLine("Refreshed token: " + App.Fn_GEtToken());
            //FN_Red();
        }
        protected override  void OnAppearing()
        {
            base.OnAppearing();
            v_cambioban = true;
        }
        protected override void OnDisappearing()
        {
            v_cambioban = false;
            base.OnDisappearing();
        }
        /// <summary>
        /// hace la consulta a la fecha de vigencia para descativarla o no
        /// </summary>
        async void Fn_Fecha()
        {
            Perf _perf = new Perf();
            _perf.v_fol = App.v_folio ;
            _perf.v_membre = App.v_membresia;
            _perf.v_letra = App.v_letra;
            //crear el json
            string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);
            //crear el cliente
            HttpClient _client = new HttpClient();
            string _DirEnviar="" ;
            _DirEnviar = "http://tratoespecial.com/validacion.php";
            StringContent _content = new StringContent(_jsonper, System.Text.Encoding.UTF8, "application/json");
            try
            {
                //mandar el json con el post
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                try
                {
                    _DirEnviar = "http://tratoespecial.com/query_perfil.php";
                    //mandar el json con el post
                    _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                     _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                    C_PerfilGen _nuePer = JsonConvert.DeserializeObject<C_PerfilGen>(_respuesta);
                    //await DisplayAlert("Info del perfil", _nuePer.Fn_GetDatos(), "Aceptar");
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
                        if (App.v_perfil.v_activo != "1")
                        {
                            M_mensaje.IsVisible = true;
                            M_mensaje.Text = "Aviso \n Cuenta no activada, ve a la seccion de perfil para mas información  ";
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
        public void Fn_AbrirSitio(object sender, EventArgs _args)
        {
            Uri _url= new Uri(v_mostrar[v_actual].v_sitio);
            Device.OpenUri(_url);
        }
    }
}