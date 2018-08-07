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
        /// <summary>
        /// 
        /// </summary>
        public static C_PerfilGen v_perfil= new C_PerfilGen();
        /// <summary>
        /// id membresia
        /// </summary>
        public static string v_membresia="";
        /// <summary>
        /// folio de la membresia
        /// </summary>
        public static string v_folio="";
        /// <summary>
        /// 0 no esta logeado 1 logeado
        /// </summary>
        public static string v_log;



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
            //App.Current.MainPage = new V_Master();// new NavigationPage(new MainPage());//  new NavigationPage(new V_Registro(true));//  new NavigationPage(new V_Registro(false));//       
                                                  //MainPage =new NavigationPage( new V_Master());// new NavigationPage(new pruebas());//// new NavigationPage(new V_Master());



            if(!Application.Current.Properties.ContainsKey("membre"))
            {
                Application.Current.Properties.Add("membre", v_membresia);
            }
            else
            {
                v_membresia = Application.Current.Properties["membre"] as string;
            }
            if (!Application.Current.Properties.ContainsKey("folio"))
            {
                Application.Current.Properties.Add("folio", v_folio);
            }
            else
            {
                v_folio = Application.Current.Properties["folio"] as string;
            }

            //existe la variable guardada
            if (Application.Current.Properties.ContainsKey("log"))
            {
                //lee el valor guardado
                v_log = Application.Current.Properties["log"] as string;
                if(v_log=="0")
                {//no esta logeado
                    App.Current.MainPage = new V_Master(false);
                }//si esta logeado
                else if(v_log=="1")
                {
                    App.Current.MainPage = new V_Master(true);
                }
            }
            else
            {
                v_log = "0";
                Application.Current.Properties.Add("log", v_log);
                App.Current.MainPage = new V_Master(false);

            }

        }
        async void Fn_Cargar()
        {
            await Task.Delay(100);
            //aca se hace el set de los doctores
        }

        public static string Fn_Vacio(string _valor)
        {

            if (string.IsNullOrEmpty(_valor) || string.IsNullOrWhiteSpace(_valor))
            {
                return "";
            }
            else
            {
                return _valor;
            }

        }

        protected override void OnStart()
        {
           // v_logeado = false;
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
