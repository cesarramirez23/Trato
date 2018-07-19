using System;
using Xamarin.Forms;
using Trato.Views;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;// pa las listas
using Trato.Personas;//cargar las clases
using System.Threading.Tasks; // delay 
[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Trato
{
    public partial class App : Application
    {
        public static ObservableCollection<C_Medico> v_medicos = new ObservableCollection<C_Medico>();
        public static ObservableCollection<C_Servicios> v_servicios = new ObservableCollection<C_Servicios>();
        public static bool v_logeado = false;
        public App()
        {
            InitializeComponent();
            //aca cargar los datos de los medicos
            v_medicos.Add(new C_Medico { v_Nombre = "Nombre", v_Apellido= "Ariza", v_Especialidad = "Espec", v_Ciudad= "ciud1", v_Domicilio = "Río Purificación 1603,Las  Águilas, 45080,Zapopan,Jal", v_descripcion = "creado al inicio 1", v_img = "ICONOAPP.png" });
            v_medicos.Add(new C_Medico { v_Nombre = "Medico 1",v_Apellido= "Morantes", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "domicillio inicio2", v_descripcion = "creado al inicio 2", v_img = "ICONOAPP.png" });
            v_medicos.Add(new C_Medico { v_Nombre = "otro nombre Cape",v_Apellido= "Presas", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "domicillio inicio3", v_descripcion = "creado al inicio 3", v_img = "ICONOAPP.png" });

            v_servicios.Add(new C_Servicios { v_Nombre = "inicio", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio1", v_descripcion = "creado al inicio 1",  v_img = "ICONOAPP.png" });//v_="descuentos %" ,
            v_servicios.Add(new C_Servicios { v_Nombre = "inicio2", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio2", v_descripcion = "creado al inicio 2",  v_img = "ICONOAPP.png" });
            v_servicios.Add(new C_Servicios { v_Nombre = "inici3", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio3", v_descripcion = "creado al inicio 3",v_img = "ICONOAPP.png" });
            //v_logeado = false;
            App.Current.MainPage =   new V_Master();// new NavigationPage(new MainPage()); //
            //MainPage =new NavigationPage( new V_Master());// new NavigationPage(new pruebas());//// new NavigationPage(new V_Master());

        }
        async void Fn_Cargar()
        {
            await Task.Delay(100);
            //aca se hace el set de los doctores
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
