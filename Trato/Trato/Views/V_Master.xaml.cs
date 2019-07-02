using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Trato.Personas;
///las cosas del push
///

namespace Trato.Views
{
    //public class Menu
    //{
    //    public string v_icon { get; set; }
    //    public string v_titulo { get; set; }
    //}
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Master : MasterDetailPage
	{/*
        las funciones se le agregan en lugar de mandar un view se lo agregas a master.detail
         */
       

        public V_Master()
        {
            InitializeComponent ();
        }
        public V_Master(bool _logeado,string _title)
        {
            InitializeComponent ();
            if(_logeado)
            {
                StackLog.IsVisible = true;
                App.Fn_CargarDatos();
                //Detail.Title = _title;
                StackPrin.IsVisible = false;
                if (App.Fn_GetCita())
                {
                    string _json = App.Current.Properties[NombresAux.v_citaNot] as string;
                    App.v_nueva = JsonConvert.DeserializeObject<Models.Cita>(_json);
                    IsPresented = false;
                    Detail = new NavigationPage(new V_Cita(false,true, App.v_nueva) { Title = "Citas" });
                }
                else
                {
                    IsPresented = false;
                    Detail = new NavigationPage(new MainPage() { Title = _title });
                }
            }
            else
            {
                StackPrin.IsVisible = true;
                StackLog.IsVisible = false;
                IsPresented = false;
                Detail = new NavigationPage(new MainPage() { Title = _title });
            }
           
        }

        public async void A()
        {
            await Task.Delay(100);
        } 
        public void Fn_Contacto(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Contacto("1") { Title = "COONTACTO" });
        }
        public void Fn_Citas(object sender, EventArgs _args)
        {
            IsPresented = false;
            Console.Write("medicam   ---- true");
            Detail = new NavigationPage(new V_Cita(false,false,null) { Title = "CITAS" });
        }
        public void Fn_Medicamentos(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Cita(true,false,null) { Title = "MEDICAMENTOS" });
        }
        public void Fn_Medicos(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage( new V_Buscador(0) { Title = "RED MÉDICA" });
        }
        public void Fn_Servicios(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage( new V_Buscador(1) { Title = "SERVICIOS DE SALUD" });
        }
        public void Fn_ServGen(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Buscador(2) { Title = "SERVICIOS GENERALES" });
        }
        public void Fn_Perfil(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Perfil() { Title = "PERFIL" });
        }
        public void Fn_Opciones(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Opciones() { Title = "CUENTA" });
        }
        public async void Fn_Salir(object sender, EventArgs _args)
        {
            string prime = App.v_membresia.Split('-')[0];
            string _membre = "";///los 4 numeros de la mebresia sin laletra
            for (int i = 0; i < prime.Length - 1; i++)
            {
                _membre += prime[i];
            }
            string _conse = App.v_membresia.Split('-')[1];
            C_Login _login = new C_Login( _membre, App.v_letra, _conse, "");
            string _jsonLog = JsonConvert.SerializeObject(_login, Formatting.Indented);
            string _DirEnviar = NombresAux.BASE_URL + "token_notification.php";
            StringContent _content = new StringContent(_jsonLog, Encoding.UTF8, "application/json");
            Console.WriteLine(" infosss " + _jsonLog);

            //crear el cliente
            HttpClient _client = new HttpClient();

            try
            {
                //mandar el json con el post
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                if (_respuesta == "1")
                {
                    IsPresented = false;
                    App.Fn_CerrarSesion();
                    App.Current.MainPage = new V_Master(false, "Bienvenido a Trato Especial");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo cerrar sesion", "Aceptar");
                    IsPresented = false;
                }

            }
            catch 
            {
                bool _elige= await DisplayAlert("Error", "No se pudo cerrar sesion Correctamente,\n ¿Cerrar sesión de forma local?", "Si", "No");
                if(_elige)
                {
                    IsPresented = false;
                    App.Fn_CerrarSesion();
                    App.Current.MainPage = new V_Master(false, "Bienvenido a Trato Especial");
                }
                else
                {
                    IsPresented = false;
                }
            }
        }
        public void Fn_Info(object sender, EventArgs _Args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Informacion());

        }
        public void Fn_Tarjeta(object sender, EventArgs _Args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Contacto("1") { Title = "Tarjeta Virtual" });
        }
        public void Fn_Membre(object sender, EventArgs _Args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Membresias());
        }
        public void Fn_Log(object sender, EventArgs _Args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Login());
        }
        public void Fn_Inicio(object sender, EventArgs _args)
        {
            if (App.v_log=="0")
            {
                Detail = new NavigationPage(new MainPage() {Title ="Bienvenido a Trato Especial" }); 
            }
            else if(App.v_log=="1")
            {
                Detail = new NavigationPage(new MainPage() {Title="Bienvenido "+App.v_perfil.v_Nombre  }); 
            }
            IsPresented = false;
        }
    }
}