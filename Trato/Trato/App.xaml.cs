using System;
using Xamarin.Forms;
using Trato.Views;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;// para las listas
using Trato.Personas;//cargar las clases
using System.Threading.Tasks; // delay 
using Newtonsoft.Json;


[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Trato
{
    public partial class App : Application
    {
        public static ObservableCollection<C_Medico> v_medicos = new ObservableCollection<C_Medico>();
        public static ObservableCollection<C_Servicios> v_servicios = new ObservableCollection<C_Servicios>();
        /// <summary>
        /// Perfil General
        /// </summary>
        public static C_PerfilGen v_perfil= new C_PerfilGen();
        public static C_PerfilMed v_perfMed = new C_PerfilMed();
        public static string v_membresia="";
        public static string v_folio="";
        /// <summary>
        /// 0 no esta logeado 1 logeado
        /// </summary>
        public static string v_log;
        public App()
        {
            InitializeComponent();
            //existe la variable guardada
            //Properties.Clear();
            if (Properties.ContainsKey("log"))
            {
                //lee el valor guardado
                v_log = Current.Properties["log"] as string;
                if(v_log=="0")
                {//no esta logeado
                    v_perfil = new C_PerfilGen();
                    v_perfMed = new C_PerfilMed();
                    v_log = "0";
                    v_membresia = "";
                    v_folio = "";
                    Properties["perfGen"] = "";
                    Properties["perfMed"] = "";
                    Properties["membre"] = v_membresia;
                    Properties["folio"] = v_folio;
                    Fn_Cargar();
                    MainPage = new V_Master(false,"si log 0");
                }//si esta logeado
                else if(v_log=="1")
                {
                    string _jsonGen = Properties["perfGen"] as string;
                    v_perfil= JsonConvert.DeserializeObject<C_PerfilGen>(_jsonGen);
                    string _jsonPerfMed = Properties["perfMed"] as string;
                    v_perfMed = JsonConvert.DeserializeObject<C_PerfilMed>(_jsonPerfMed);

                    v_membresia =Properties["folio"] as string;
                    v_folio = Properties["membre"] as string;
                    string _jsonServ = Current.Properties["servicios"] as string;
                    v_servicios = JsonConvert.DeserializeObject<ObservableCollection<C_Servicios>>(_jsonServ);
                    string _jsonMed = Current.Properties["medicos"] as string;
                    v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);

                    MainPage = new V_Master(true, "si log 1" ) ;
                }
            }
            else
            {
                v_log = "-1";
                v_perfil = new C_PerfilGen();
                v_perfMed = new C_PerfilMed();
                v_folio = "";
                v_membresia = "";
                Fn_CrearKey();
                Fn_Cargar();
                App.Current.MainPage = new V_Master(false,"no properties");
            }
        }
        void Fn_CrearKey()
        {
            if (!Properties.ContainsKey("log"))
            {
                Properties.Add("log", v_log);
            }
            if (!Properties.ContainsKey("membre"))
            {
                Properties.Add("membre", v_membresia);
            }
            if (!Properties.ContainsKey("folio"))
            {
                Properties.Add("folio", v_folio);
            }
            if (!Properties.ContainsKey("perfGen"))
            {
                Properties.Add("perfGen", "");
            }
            if (!Properties.ContainsKey("perfMed"))
            {
                Properties.Add("perfMed", "");
            }
            if (!Application.Current.Properties.ContainsKey("medicos"))
            {            //aca cargar los datos de los medicos
                v_medicos = new ObservableCollection<C_Medico>();
                v_medicos.Add(new C_Medico { v_Nombre = "Nombre", v_Apellido = "Ariza", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "Río Purificación 1603,Las  Águilas, 45080,Zapopan,Jal", v_descripcion = "creado al inicio 1", v_img = "ICONOAPP.png" });
                v_medicos.Add(new C_Medico { v_Nombre = "Medico 1", v_Apellido = "Morantes", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "domicillio inicio2", v_descripcion = "creado al inicio 2", v_img = "ICONOAPP.png" });
                v_medicos.Add(new C_Medico { v_Nombre = "otro nombre Cape", v_Apellido = "Presas", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "domicillio inicio3", v_descripcion = "creado al inicio 3", v_img = "ICONOAPP.png" });

                string _jsonMed = JsonConvert.SerializeObject(v_medicos);
                Application.Current.Properties.Add("medicos", _jsonMed);
            }
            //if (!Properties.ContainsKey("servicios"))
            //{
            //    Properties.Add("servicios", "");
            //}
            if (!Application.Current.Properties.ContainsKey("servicios"))
            {
                string _jsoServ = "";
                v_servicios.Add(new C_Servicios { v_Nombre = "inicio", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio1", v_descripcion = "creado al inicio 1", v_img = "ICONOAPP.png" });//v_="descuentos %" ,
                v_servicios.Add(new C_Servicios { v_Nombre = "inicio2", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio2", v_descripcion = "creado al inicio 2", v_img = "ICONOAPP.png" });
                v_servicios.Add(new C_Servicios { v_Nombre = "inici3", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio3", v_descripcion = "creado al inicio 3", v_img = "ICONOAPP.png" });
                _jsoServ = JsonConvert.SerializeObject(v_servicios);
                Application.Current.Properties.Add("servicios", _jsoServ);
            }
            Current.SavePropertiesAsync();
        }
        /// <summary>
        /// se cargan las listas
        /// </summary>
        async void Fn_Cargar()
        {
            if (!Application.Current.Properties.ContainsKey("medicos"))
            {            //aca cargar los datos de los medicos
                v_medicos = new ObservableCollection<C_Medico>();
                v_medicos.Add(new C_Medico { v_Nombre = "Nombre", v_Apellido = "Ariza", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "Río Purificación 1603,Las  Águilas, 45080,Zapopan,Jal", v_descripcion = "creado al inicio 1", v_img = "ICONOAPP.png" });
                v_medicos.Add(new C_Medico { v_Nombre = "Medico 1", v_Apellido = "Morantes", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "domicillio inicio2", v_descripcion = "creado al inicio 2", v_img = "ICONOAPP.png" });
                v_medicos.Add(new C_Medico { v_Nombre = "otro nombre Cape", v_Apellido = "Presas", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "domicillio inicio3", v_descripcion = "creado al inicio 3", v_img = "ICONOAPP.png" });

                string _jsonMed = JsonConvert.SerializeObject(v_medicos);
                Application.Current.Properties.Add("medicos", _jsonMed);
            }
            else
            {
                string _jsonMed = Current.Properties["medicos"] as string;
                v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);
            }

            if (!Application.Current.Properties.ContainsKey("servicios"))
            {
                string _jsoServ = "";
                v_servicios.Add(new C_Servicios { v_Nombre = "inicio", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio1", v_descripcion = "creado al inicio 1", v_img = "ICONOAPP.png" });//v_="descuentos %" ,
                v_servicios.Add(new C_Servicios { v_Nombre = "inicio2", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio2", v_descripcion = "creado al inicio 2", v_img = "ICONOAPP.png" });
                v_servicios.Add(new C_Servicios { v_Nombre = "inici3", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio3", v_descripcion = "creado al inicio 3", v_img = "ICONOAPP.png" });
                _jsoServ = JsonConvert.SerializeObject(v_servicios);
                Application.Current.Properties.Add("servicios", _jsoServ);
            }
            else
            {
                string _jsonServ = Current.Properties["servicios"] as string;
                v_servicios = JsonConvert.DeserializeObject<ObservableCollection<C_Servicios>>(_jsonServ);
            }
            await Current.SavePropertiesAsync();
            await Task.Delay(100);
            //aca se hace el set de los doctores
        }
        public static async void Fn_CargarDatos()
        {
            if (!Application.Current.Properties.ContainsKey("membre"))
            {
                v_membresia = "";
                Application.Current.Properties.Add("membre", v_membresia);
            }
            else
            {
                v_membresia = Application.Current.Properties["membre"] as string;
            }

            if (!Application.Current.Properties.ContainsKey("folio"))
            {
                v_folio = "";
                Application.Current.Properties.Add("folio", v_folio);
            }
            else
            {
                v_folio = Application.Current.Properties["folio"] as string;
            }
            if (!Application.Current.Properties.ContainsKey("perfGen"))
            {
                v_perfil = new C_PerfilGen();
                Application.Current.Properties.Add("perfGen", "");
            }
            else
            {
                string _jsonGen =Current.Properties["perfGen"] as string;
                v_perfil = JsonConvert.DeserializeObject<C_PerfilGen>(_jsonGen);
            }
            if (!Application.Current.Properties.ContainsKey("perfMed"))
            {
                v_perfMed = new C_PerfilMed();
                Application.Current.Properties.Add("perfMed", "");
            }
            else
            {
                string _jsonMed = Current.Properties["perfMed"] as string;
                v_perfMed = JsonConvert.DeserializeObject<C_PerfilMed>(_jsonMed);
            }
            

            if (!Application.Current.Properties.ContainsKey("medicos"))
            {            //aca cargar los datos de los medicos
                v_medicos = new ObservableCollection<C_Medico>();
                v_medicos.Add(new C_Medico { v_Nombre = "Nombre", v_Apellido = "Ariza", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "Río Purificación 1603,Las  Águilas, 45080,Zapopan,Jal", v_descripcion = "creado al inicio 1", v_img = "ICONOAPP.png" });
                v_medicos.Add(new C_Medico { v_Nombre = "Medico 1", v_Apellido = "Morantes", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "domicillio inicio2", v_descripcion = "creado al inicio 2", v_img = "ICONOAPP.png" });
                v_medicos.Add(new C_Medico { v_Nombre = "otro nombre Cape", v_Apellido = "Presas", v_Especialidad = "Espec", v_Ciudad = "ciud1", v_Domicilio = "domicillio inicio3", v_descripcion = "creado al inicio 3", v_img = "ICONOAPP.png" });

                string _jsonMed = JsonConvert.SerializeObject(v_medicos);
                Application.Current.Properties.Add("medicos", _jsonMed);
            }
            else
            {
                string _jsonMed = Current.Properties["medicos"] as string;
                v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);
            }

            if (!Application.Current.Properties.ContainsKey("servicios"))
            {
                string _jsoServ = "";
                v_servicios.Add(new C_Servicios { v_Nombre = "inicio", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio1", v_descripcion = "creado al inicio 1", v_img = "ICONOAPP.png" });//v_="descuentos %" ,
                v_servicios.Add(new C_Servicios { v_Nombre = "inicio2", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio2", v_descripcion = "creado al inicio 2", v_img = "ICONOAPP.png" });
                v_servicios.Add(new C_Servicios { v_Nombre = "inici3", v_Especialidad = "esps1", v_Domicilio = "domicillio inicio3", v_descripcion = "creado al inicio 3", v_img = "ICONOAPP.png" });
                _jsoServ = JsonConvert.SerializeObject(v_servicios);
                Application.Current.Properties.Add("servicios", _jsoServ);
            }
            else
            {
                string _jsonServ = Current.Properties["servicios"] as string;                
                v_servicios = JsonConvert.DeserializeObject<ObservableCollection<C_Servicios>>(_jsonServ);
            }
            await Task.Delay(100);
        }
        public  static async void Fn_GuardarDatos(C_PerfilGen _gen, string _membre, string _folio )
        {
            v_perfil = _gen;
            v_folio = _folio;
            v_membresia = _membre;
            string _jsonGen = JsonConvert.SerializeObject(v_perfil);
            Current.Properties["log"] = v_log;
            Current.Properties["perfGen"] = _jsonGen;
            Current.Properties["membre"] =v_membresia;
            Current.Properties["folio"] =v_folio;
            string _jsoServ = JsonConvert.SerializeObject(v_servicios);
            Current.Properties["servicios"] = _jsoServ;
            string _jsonMed = JsonConvert.SerializeObject(v_medicos);
            Current.Properties["medicos"] = _jsonMed;

            await Current.SavePropertiesAsync();
            Fn_CargarDatos();
            await Task.Delay(100);
        }
        public static async void Fn_GuardarDatos(C_PerfilMed _med, string _membre, string _folio)
        {
            v_perfMed = _med;
            v_folio = _folio;
            v_membresia = _membre;
            string _jsonPerMed = JsonConvert.SerializeObject(v_perfMed);
            Current.Properties["log"] = v_log;
            Current.Properties["perfMed"] = _jsonPerMed;
            Current.Properties["membre"] = v_membresia;
            Current.Properties["folio"] = v_folio;
            string _jsoServ = JsonConvert.SerializeObject(v_servicios);
            Current.Properties["servicios"] = _jsoServ;
            string _jsonMed = JsonConvert.SerializeObject(v_medicos);
            Current.Properties["medicos"] = _jsonMed;

            await Current.SavePropertiesAsync();
            Fn_CargarDatos();
            await Task.Delay(100);
        }
        public static async void Fn_CerrarSesion()
        {
            v_perfil = new C_PerfilGen();
            v_perfMed = new C_PerfilMed();
            v_folio = "";
            v_membresia = "";
            Current.Properties["perfGen"] = "";
            Current.Properties["perfMed"] = "";
            Current.Properties["membre"] = v_membresia;
            Current.Properties["folio"] = v_folio;
            await Task.Delay(100);
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
            // Handle when your app starts
        }
        protected override void OnSleep()
        {
            if(v_log=="1")
            {
                Fn_GuardarDatos(v_perfil,v_membresia,v_folio);
                Fn_GuardarDatos(v_perfMed, v_membresia, v_folio);
            }
            // Handle when your app sleeps       
        }
        protected override void OnResume()
        {
            // Handle when your app resumes
        }        
    }
}
