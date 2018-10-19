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
                Detail.Title = _title;
                StackPrin.IsVisible = false;
            }
            else
            {
                StackPrin.IsVisible = true;
                StackLog.IsVisible = false;
            }
            IsPresented = false;
            Detail = new NavigationPage(new MainPage() {Title=_title });
        }

        public async void A()
        {
            await Task.Delay(100);
        } 
        public void Fn_Contacto(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Contacto() { Title = "Contacto" });
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
            Detail = new NavigationPage(new V_Perfil() { Title = "Perfil" });
        }
        public void Fn_Opciones(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Opciones() { Title = "Opciones" });
        }
        public async void Fn_Salir(object sender, EventArgs _args)
        {

            //C_Login _login = new C_Login(App.v_membresia _membre, letra, _conse, pass.Text, fol.Text);
            ////crear el json
            //string _jsonLog = JsonConvert.SerializeObject(_login, Formatting.Indented);
            ////mostrar la pantalla con mensajes
            //// Mensajes_over.Text +=_jsonLog ;
            ////crear el cliente
            //HttpClient _client = new HttpClient();
            //string _DirEnviar = "http://tratoespecial.com/login.php";
            //StringContent _content = new StringContent(_jsonLog, Encoding.UTF8, "application/json");
            ////mandar el json con el post
            //try
            //{ }
            //catch(HttpRequestException ex)
            //{

            //}



            IsPresented = false;
            App.Fn_CerrarSesion();
            App.Current.MainPage =new V_Master(false, "Bienvenido a Trato Especial");
        }
        public void Fn_Info(object sender, EventArgs _Args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Informacion());

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