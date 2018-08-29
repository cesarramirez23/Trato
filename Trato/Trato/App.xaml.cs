using System;
using Xamarin.Forms;
using Trato.Views;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;// para las listas
using Trato.Personas;//cargar las clases
using System.Threading.Tasks; // delay 
using Newtonsoft.Json;
using System.Net.Http;//para el las cosas que necesitan web

///https://blog.wilsonvargas.com/generando-y-leyendo-codigos-qr-con-xamarin-form/
//////https://www.youtube.com/watch?v=y7rZbwOqrUk
[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Trato
{
    public partial class App : Application
    {
        public static ObservableCollection<C_Medico> v_medicos = new ObservableCollection<C_Medico>();
        public static ObservableCollection<C_Servicios> v_servicios = new ObservableCollection<C_Servicios>();
        public static ObservableCollection<C_ServGenerales> v_generales = new ObservableCollection<C_ServGenerales>();
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
            
           // App.Current.MainPage = new V_Master(false,"no properties");
        }
        async  void Fn_CrearKey()
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
                v_perfil = new C_PerfilGen();
                string _json = JsonConvert.SerializeObject(v_perfil, Formatting.Indented);
                Current.Properties.Add("perfGen", "");
                Current.Properties["perfGen"] = _json;
            }
            if (!Properties.ContainsKey("perfMed"))
            {
                v_perfMed = new C_PerfilMed();
                string _json = JsonConvert.SerializeObject(v_perfMed, Formatting.Indented);
                Current.Properties.Add("perfMed", "");
                Current.Properties["perfMed"] = _json;
            }
            if (!Properties.ContainsKey("medicos"))
            {
                v_medicos = new ObservableCollection<C_Medico>();
                string _json = JsonConvert.SerializeObject(v_medicos, Formatting.Indented);
                Current.Properties.Add("medicos", "");
                Current.Properties["medicos"] = _json;
            }
            if (!Properties.ContainsKey("servicios"))
            {
                v_servicios = new ObservableCollection<C_Servicios>();
                string _json = JsonConvert.SerializeObject(v_servicios, Formatting.Indented);
                Current.Properties.Add("servicios", "");
                Current.Properties["servicios"] = _json;
            }
            await Current.SavePropertiesAsync();
        }
        /// <summary>
        /// se cargan las listas
        /// </summary>
        async void Fn_CargarListas()
        {
            if (!Current.Properties.ContainsKey("medicos"))
            {
                v_medicos = new ObservableCollection<C_Medico>();
                string _json = JsonConvert.SerializeObject(v_medicos);
                Current.Properties.Add("medicos", "");
                Current.Properties["medicos"] = _json;
            }
            else
            {
                string _jsonMed = Current.Properties["medicos"] as string;
                v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);
            }

            if (!Current.Properties.ContainsKey("servicios"))
            {
                v_servicios = new ObservableCollection<C_Servicios>();
                string _json = JsonConvert.SerializeObject(v_servicios);
                Current.Properties.Add("servicios", "");
                Current.Properties["servicios"] = _json;
            }
            else
            {
                string _jsonServ = Current.Properties["servicios"] as string;
                v_servicios = JsonConvert.DeserializeObject<ObservableCollection<C_Servicios>>(_jsonServ);
            }
            //servicios generales no medicos
            if (!Current.Properties.ContainsKey("generales"))
            {
                v_generales = new ObservableCollection<C_ServGenerales>();
                string _json = JsonConvert.SerializeObject(v_servicios);
                Current.Properties.Add("generales", "");
                Current.Properties["generales"] = _json;
            }
            else
            {
                string _jsonServ = Current.Properties["generales"] as string;
                v_generales = JsonConvert.DeserializeObject<ObservableCollection<C_ServGenerales>>(_jsonServ);
            }
            await Current.SavePropertiesAsync();
            await Task.Delay(100);
            //aca se hace el set de los doctores
        }
        public static async void Fn_CargarDatos()
        {
            if (!Current.Properties.ContainsKey("membre"))
            {
                v_membresia = "";
                Current.Properties.Add("membre", v_membresia);
            }
            else
            {
                v_membresia = Current.Properties["membre"] as string;
            }

            if (!Current.Properties.ContainsKey("folio"))
            {
                v_folio = " cargar";
                Current.Properties.Add("folio", v_folio);
            }
            else
            {
                v_folio = Current.Properties["folio"] as string;
            }
            if (!Current.Properties.ContainsKey("perfGen"))
            {
                v_perfil = new C_PerfilGen();
                string _json = JsonConvert.SerializeObject(v_perfil, Formatting.Indented);
                Current.Properties.Add("perfGen", "");
                Current.Properties["perfGen"] = _json;
            }
            else
            {
                string _jsonGen =Current.Properties["perfGen"] as string;
                v_perfil = JsonConvert.DeserializeObject<C_PerfilGen>(_jsonGen);
            }
            if (!Current.Properties.ContainsKey("perfMed"))
            {
                v_perfMed = new C_PerfilMed();
                string _json = JsonConvert.SerializeObject(v_perfMed, Formatting.Indented);
                Current.Properties.Add("perfMed", "");
                Current.Properties["perfMed"] = _json;
            }
            else
            {
                string _jsonMed = Current.Properties["perfMed"] as string;
                v_perfMed = JsonConvert.DeserializeObject<C_PerfilMed>(_jsonMed);
            }
            

            if (!Current.Properties.ContainsKey("medicos"))
            {
                v_medicos = new ObservableCollection<C_Medico>();
                string _json = JsonConvert.SerializeObject(v_medicos, Formatting.Indented);
                Current.Properties.Add("medicos", "");
                Current.Properties["medicos"] = _json;
            }
            else
            {
                string _jsonMed = Current.Properties["medicos"] as string;
                v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);
            }

            if (!Current.Properties.ContainsKey("servicios"))
            {
                v_servicios = new ObservableCollection<C_Servicios>();
                string _json = JsonConvert.SerializeObject(v_servicios, Formatting.Indented);
                Current.Properties.Add("servicios","");
                Current.Properties["servicios"] = _json;
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
            string _jsonGen = JsonConvert.SerializeObject(v_perfil, Formatting.Indented);
            Current.Properties["log"] = v_log;
            Current.Properties["perfGen"] = _jsonGen;
            Current.Properties["membre"] =v_membresia ;
            Current.Properties["folio"] =v_folio ;
            string _jsoServ = JsonConvert.SerializeObject(v_servicios, Formatting.Indented);
            Current.Properties["servicios"] = _jsoServ;
            string _jsonMed = JsonConvert.SerializeObject(v_medicos, Formatting.Indented);
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
            string _jsonPerMed = JsonConvert.SerializeObject(v_perfMed, Formatting.Indented);
            Current.Properties["log"] = v_log;
            Current.Properties["perfMed"] = _jsonPerMed;
            Current.Properties["membre"] = v_membresia;
            Current.Properties["folio"] = v_folio;
            string _jsoServ = JsonConvert.SerializeObject(v_servicios, Formatting.Indented);
            Current.Properties["servicios"] = _jsoServ;
            string _jsonMed = JsonConvert.SerializeObject(v_medicos, Formatting.Indented);
            Current.Properties["medicos"] = _jsonMed;

            await Current.SavePropertiesAsync();
            Fn_CargarDatos();
            await Task.Delay(100);
        }

        public static async void Fn_GuardarRed(ObservableCollection<C_Medico> _medicos)
        {
            string _json = JsonConvert.SerializeObject(_medicos, Formatting.Indented);
            if(Current.Properties.ContainsKey("medicos"))
            {
                Current.Properties["medicos"] = _json;
            }
            else
            {
                Current.Properties.Add("medicos", "");
                Current.Properties["medicos"] = _json;
            }
            await Current.SavePropertiesAsync();
        }
        public static async void Fn_GuardarServcios(ObservableCollection<C_Servicios> _servicios)
        {
            string _json = JsonConvert.SerializeObject(_servicios, Formatting.Indented);
            if (Current.Properties.ContainsKey("servicios"))
            {
                Current.Properties["servicios"] = _json;
            }
            else
            {
                Current.Properties.Add("servicios", "");
                Current.Properties["servicios"] = _json;
            }
            await Current.SavePropertiesAsync();
        }
        public static async void Fn_GuardarGenerales(ObservableCollection<C_ServGenerales> _general)
        {
            string _json = JsonConvert.SerializeObject(_general, Formatting.Indented);
            if (Current.Properties.ContainsKey("generales"))
            {
                Current.Properties["generales"] = _json;
            }
            else
            {
                Current.Properties.Add("generales", "");
                Current.Properties["generales"] = _json;
            }
            await Current.SavePropertiesAsync();
        }

        public static async void Fn_CerrarSesion()
        {
            v_perfil = new C_PerfilGen();
            v_perfMed = new C_PerfilMed();
            v_folio = "";
            v_membresia = "";
            string _json = JsonConvert.SerializeObject(v_perfil, Formatting.Indented);
            Current.Properties["perfGen"] = _json;
            _json = JsonConvert.SerializeObject(v_perfMed, Formatting.Indented);
            Current.Properties["perfMed"] = _json;
            Current.Properties["membre"] = v_membresia;
            Current.Properties["folio"] = v_folio;
            await Current.SavePropertiesAsync();
            await Task.Delay(100);
        }
        public static void Fn_ImgSexo()
        {
            for (int i = 0; i<v_medicos.Count;i++)
            {
                if (int.Parse(v_medicos[i].v_id) < 1)
                {
                    if (v_medicos[i].v_idsexo == 0)
                    {
                        v_medicos[i].v_img = "doctor.png";
                    }
                    else
                    {
                        v_medicos[i].v_img = "doctora.png";
                    }
                }
                else
                {
                    //v_medicos[i].v_img = "" + v_medicos[i].v_id;
                    v_medicos[i].v_img= "https://useller.com.mx/product_img/Doux%20Moda.jpg";
                }
                //if (v_medicos[i].v_idsexo == 0)
                //{
                //    v_medicos[i].v_img = "doctor.png";
                //}
                //else
                //{
                //    v_medicos[i].v_img = "doctora.png";
                //}
                v_medicos[i].v_completo = v_medicos[i].v_titulo + " " + v_medicos[i].v_Nombre + " " + v_medicos[i].v_Apellido;
            }
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
            //existe la variable guardada
            //Properties.Clear();
            if (Properties.ContainsKey("log"))
            {
                //lee el valor guardado
                v_log = Current.Properties["log"] as string;
                if (v_log == "0")
                {//no esta logeado
                    v_perfil = new C_PerfilGen();
                    v_perfMed = new C_PerfilMed();
                    v_log = "0";
                    v_membresia = "";
                    v_folio = "";
                    
                    string _json = JsonConvert.SerializeObject(v_perfil, Formatting.Indented);
                    Properties["perfGen"] = _json;
                    _json = JsonConvert.SerializeObject(v_perfMed, Formatting.Indented);
                    Properties["perfMed"] = _json;


                    Properties["membre"] = v_membresia;
                    Properties["folio"] = v_folio;
                    Fn_CargarListas();
                    MainPage = new V_Master(false, "Bienvenido a Trato Especial");
                }//si esta logeado
                else if (v_log == "1")
                {
                    string _jsonGen = Properties["perfGen"] as string;
                    v_perfil = JsonConvert.DeserializeObject<C_PerfilGen>(_jsonGen);
                    string _jsonPerfMed = Properties["perfMed"] as string;
                    v_perfMed = JsonConvert.DeserializeObject<C_PerfilMed>(_jsonPerfMed);

                    v_folio = Properties["folio"] as string;
                    v_membresia = Properties["membre"] as string;
                    string _jsonServ = Current.Properties["servicios"] as string;
                    v_servicios = JsonConvert.DeserializeObject<ObservableCollection<C_Servicios>>(_jsonServ);
                    string _jsonMed = Current.Properties["medicos"] as string;
                    v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);
                    string _jsonGenerales = Current.Properties["generales"] as string;
                    v_generales = JsonConvert.DeserializeObject<ObservableCollection<C_ServGenerales>>(_jsonGenerales);

                    MainPage = new V_Master(true, "Bienvenido "+v_perfil.v_Nombre);
                }
                else
                {
                    MainPage = new V_Master(false, v_log);
                }
            }
            else
            {
                v_log = "0";
                v_perfil = new C_PerfilGen();
                v_perfMed = new C_PerfilMed();
                v_folio = "";
                v_membresia = "";
                Fn_CrearKey();
                Fn_CargarListas();
                App.Current.MainPage = new V_Master(false, "no properties");
            }

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
