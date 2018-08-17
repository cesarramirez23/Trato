using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using Trato.Personas;

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
            Detail = new NavigationPage( new V_Buscador(true) { Title = "RED MEDICA" });
        }
        public void Fn_Servicios(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage( new V_Buscador(false) { Title = "SERVICIOS MEDICOS" });//new V_Buscador() { Title = "Buscador" };
        }
        public void Fn_Perfil(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Perfil() { Title = "Perfil" });//new V_Buscador() { Title = "Buscador" };
        }
        public void Fn_Opciones(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Opciones() { Title = "Opciones" });//new V_Buscador() { Title = "Buscador" };
        }
        public void Fn_Salir(object sender, EventArgs _args)
        {
            IsPresented = false;
            App.v_log = "0";
            Application.Current.Properties["log"] = App.v_log;
            Application.Current.SavePropertiesAsync();
            App.Current.MainPage =new V_Master(false,"Bienvenido");
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
            IsPresented = false;
            Detail = new NavigationPage(new MainPage()); 
        }
    }
}