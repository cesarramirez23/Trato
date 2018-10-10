using System;
using Xamarin.Forms;
using Trato.Views;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;// para las listas
using Trato.Personas;//cargar las clases
using System.Threading.Tasks; // delay 
using Newtonsoft.Json;
using System.Net.Http;//para el las cosas que necesitan web
using System.Text;

/*https://xamarinhelp.com/visual-studio-2017-net-standard-xamarin/ 
 * El archivo de recursos no ha cambiado, así que se omitirá su escritura. Ruta de acceso: C:\Users\AlsainVR\Documents\GitHub\Trato\Trato\Trato.Android\obj\project.assets.json
Ejecutando restauración...
Error de restauración en 40.16 ms para C:\Users\AlsainVR\Documents\GitHub\Trato\Trato\Trato.Android\Trato.Android.csproj.
El archivo de recursos no ha cambiado, así que se omitirá su escritura. Ruta de acceso: C:\Users\AlsainVR\Documents\GitHub\Trato\Trato\Trato.iOS\obj\project.assets.json
Error de restauración en 47.57 ms para C:\Users\AlsainVR\Documents\GitHub\Trato\Trato\Trato.iOS\Trato.iOS.csproj.
 * https://firebase.google.com/docs/cloud-messaging/send-with-console
 * http://www.tothenew.com/blog/push-notifications-using-firebase-cloud-messaging/
 * 
 * https://www.microsoft.com/net/download/thank-you/net452-developer-pack
 * 
 membre     
 letra 
 consecutivo
 costo
 nombre   que membresia

 */
//legacy server key    AIzaSyBJShtMkJ22N0ksY4KGivhAwelFDFwFWfQ

//com.alsain.trato4 en la oficina
//com.alsain.trato5 en casa


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
        public static C_PerfilGen v_perfil = new C_PerfilGen();
        public static C_PerfilMed v_perfMed = new C_PerfilMed();
        /// <summary>
        /// ya trae la letra, hacer el split con -
        /// </summary>
        public static string v_membresia = "";
        /// <summary>
        /// la letra de la membresia
        /// </summary>
        public static string v_letra = "";
        /// <summary>
        /// es solo el numero consecutivo
        /// </summary>
        public static string v_folio = "";
        /// <summary>
        /// 0 no esta logeado 1 logeado
        /// </summary>
        public static string v_log;
        public App()
        {
            InitializeComponent();
            // App.Current.MainPage = new V_Master(false,"no properties");
        }
        async void Fn_CrearKey()
        {
            if (!Properties.ContainsKey(NombresAux.v_log))
            {
                Properties.Add(NombresAux.v_log, v_log);
            }
            if (!Properties.ContainsKey(NombresAux.v_membre))
            {
                Properties.Add(NombresAux.v_membre, v_membresia);
            }
            if (!Properties.ContainsKey(NombresAux.v_letra))
            {
                Properties.Add(NombresAux.v_letra, v_letra);
            }
            if (!Properties.ContainsKey(NombresAux.v_folio))
            {
                Properties.Add(NombresAux.v_folio, v_folio);
            }
            if (!Properties.ContainsKey(NombresAux.v_perfGen))
            {
                v_perfil = new C_PerfilGen();
                string _json = JsonConvert.SerializeObject(v_perfil, Formatting.Indented);
                Current.Properties.Add(NombresAux.v_perfGen, "");
                Current.Properties[NombresAux.v_perfGen] = _json;
            }
            if (!Properties.ContainsKey(NombresAux.v_perMed))
            {
                v_perfMed = new C_PerfilMed();
                string _json = JsonConvert.SerializeObject(v_perfMed, Formatting.Indented);
                Current.Properties.Add(NombresAux.v_perMed, "");
                Current.Properties[NombresAux.v_perMed] = _json;
            }
            if (!Properties.ContainsKey(NombresAux.v_redmedica))
            {
                v_medicos = new ObservableCollection<C_Medico>();
                string _json = JsonConvert.SerializeObject(v_medicos, Formatting.Indented);
                Current.Properties.Add(NombresAux.v_redmedica, "");
                Current.Properties[NombresAux.v_redmedica] = _json;
            }
            if (!Properties.ContainsKey(NombresAux.v_serviciosmedicos))
            {
                v_servicios = new ObservableCollection<C_Servicios>();
                string _json = JsonConvert.SerializeObject(v_servicios, Formatting.Indented);
                Current.Properties.Add(NombresAux.v_serviciosmedicos, "");
                Current.Properties[NombresAux.v_serviciosmedicos] = _json;
            }
            if (!Properties.ContainsKey(NombresAux.v_serviciosgenereales))
            {
                v_servicios = new ObservableCollection<C_Servicios>();
                string _json = JsonConvert.SerializeObject(v_servicios, Formatting.Indented);
                Current.Properties.Add(NombresAux.v_serviciosgenereales, "");
                Current.Properties[NombresAux.v_serviciosgenereales] = _json;
            }
            await Current.SavePropertiesAsync();
        }
        /// <summary>
        /// se cargan las listas
        /// </summary>
        async void Fn_CargarListas()
        {
            if (!Current.Properties.ContainsKey(NombresAux.v_redmedica))
            {
                v_medicos = new ObservableCollection<C_Medico>();
                string _json = JsonConvert.SerializeObject(v_medicos);
                Current.Properties.Add(NombresAux.v_redmedica, "");
                Current.Properties[NombresAux.v_redmedica] = _json;
            }
            else
            {
                string _jsonMed = Current.Properties[NombresAux.v_redmedica] as string;
                v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);
            }

            if (!Current.Properties.ContainsKey(NombresAux.v_serviciosmedicos))
            {
                v_servicios = new ObservableCollection<C_Servicios>();
                string _json = JsonConvert.SerializeObject(v_servicios);
                Current.Properties.Add(NombresAux.v_serviciosmedicos, "");
                Current.Properties[NombresAux.v_serviciosmedicos] = _json;
            }
            else
            {
                string _jsonServ = Current.Properties[NombresAux.v_serviciosmedicos] as string;
                v_servicios = JsonConvert.DeserializeObject<ObservableCollection<C_Servicios>>(_jsonServ);
            }
            //servicios generales no medicos
            if (!Current.Properties.ContainsKey(NombresAux.v_serviciosgenereales))
            {
                v_generales = new ObservableCollection<C_ServGenerales>();
                string _json = JsonConvert.SerializeObject(v_servicios);
                Current.Properties.Add(NombresAux.v_serviciosgenereales, "");
                Current.Properties[NombresAux.v_serviciosgenereales] = _json;
            }
            else
            {
                string _jsonServ = Current.Properties[NombresAux.v_serviciosgenereales] as string;
                v_generales = JsonConvert.DeserializeObject<ObservableCollection<C_ServGenerales>>(_jsonServ);
            }
            await Current.SavePropertiesAsync();
            await Task.Delay(100);
            //aca se hace el set de los doctores
        }
        public static async void Fn_CargarDatos()
        {
            if (!Current.Properties.ContainsKey(NombresAux.v_membre))
            {
                v_membresia = "";
                Current.Properties.Add(NombresAux.v_membre, v_membresia);
            }
            else
            {
                v_membresia = Current.Properties[NombresAux.v_membre] as string;
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_letra))
            {
                v_letra = "";
                Current.Properties.Add(NombresAux.v_letra, v_letra);
            }
            else
            {
                v_letra = Current.Properties[NombresAux.v_letra] as string;
            }

            if (!Current.Properties.ContainsKey(NombresAux.v_folio))
            {
                v_folio = "";
                Current.Properties.Add(NombresAux.v_folio, v_folio);
            }
            else
            {
                v_folio = Current.Properties[NombresAux.v_folio] as string;
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_perfGen))
            {
                v_perfil = new C_PerfilGen();
                string _json = JsonConvert.SerializeObject(v_perfil, Formatting.Indented);
                Current.Properties.Add(NombresAux.v_perfGen, "");
                Current.Properties[NombresAux.v_perfGen] = _json;
            }
            else
            {
                string _jsonGen = Current.Properties[NombresAux.v_perfGen] as string;
                v_perfil = JsonConvert.DeserializeObject<C_PerfilGen>(_jsonGen);
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_perMed))
            {
                v_perfMed = new C_PerfilMed();
                string _json = JsonConvert.SerializeObject(v_perfMed, Formatting.Indented);
                Current.Properties.Add(NombresAux.v_perMed, "");
                Current.Properties[NombresAux.v_perMed] = _json;
            }
            else
            {
                string _jsonMed = Current.Properties[NombresAux.v_perMed] as string;
                v_perfMed = JsonConvert.DeserializeObject<C_PerfilMed>(_jsonMed);
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_redmedica))
            {
                v_medicos = new ObservableCollection<C_Medico>();
                string _json = JsonConvert.SerializeObject(v_medicos, Formatting.Indented);
                Current.Properties.Add(NombresAux.v_redmedica, "");
                Current.Properties[NombresAux.v_redmedica] = _json;
            }
            else
            {
                string _jsonMed = Current.Properties[NombresAux.v_redmedica] as string;
                v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);
            }

            if (!Current.Properties.ContainsKey(NombresAux.v_serviciosmedicos))
            {
                v_servicios = new ObservableCollection<C_Servicios>();
                string _json = JsonConvert.SerializeObject(v_servicios, Formatting.Indented);
                Current.Properties.Add(NombresAux.v_serviciosmedicos, "");
                Current.Properties[NombresAux.v_serviciosmedicos] = _json;
            }
            else
            {
                string _jsonServ = Current.Properties[NombresAux.v_serviciosmedicos] as string;
                v_servicios = JsonConvert.DeserializeObject<ObservableCollection<C_Servicios>>(_jsonServ);
            }
            await Task.Delay(100);
        }
        public static async void Fn_GuardarDatos(C_PerfilGen _gen, string _membre, string _folio, string _letra)
        {
            v_perfil = _gen;
            v_folio = _folio;
            v_membresia = _membre;
            v_letra = _letra;
            string _jsonGen = JsonConvert.SerializeObject(v_perfil, Formatting.Indented);
            Current.Properties[NombresAux.v_log] = v_log;
            Current.Properties[NombresAux.v_perfGen] = _jsonGen;
            Current.Properties[NombresAux.v_membre] = v_membresia;
            Current.Properties[NombresAux.v_folio] = v_folio;
            Current.Properties[NombresAux.v_letra] = v_letra;
            string _jsoServ = JsonConvert.SerializeObject(v_servicios, Formatting.Indented);
            Current.Properties[NombresAux.v_serviciosmedicos] = _jsoServ;
            string _jsonMed = JsonConvert.SerializeObject(v_medicos, Formatting.Indented);
            Current.Properties[NombresAux.v_redmedica] = _jsonMed;

            await Current.SavePropertiesAsync();
            Fn_CargarDatos();
            await Task.Delay(100);
        }
        public static async void Fn_GuardarDatos(C_PerfilMed _med, string _membre, string _folio, string _letra)
        {
            v_perfMed = _med;
            v_folio = _folio;
            v_membresia = _membre;
            v_letra = _letra;
            string _jsonPerMed = JsonConvert.SerializeObject(v_perfMed, Formatting.Indented);
            Current.Properties[NombresAux.v_log] = v_log;
            Current.Properties[NombresAux.v_perMed] = _jsonPerMed;
            Current.Properties[NombresAux.v_membre] = v_membresia;
            Current.Properties[NombresAux.v_folio] = v_folio;
            Current.Properties[NombresAux.v_letra] = v_letra;
            string _jsoServ = JsonConvert.SerializeObject(v_servicios, Formatting.Indented);
            Current.Properties[NombresAux.v_serviciosmedicos] = _jsoServ;
            string _jsonMed = JsonConvert.SerializeObject(v_medicos, Formatting.Indented);
            Current.Properties[NombresAux.v_redmedica] = _jsonMed;

            await Current.SavePropertiesAsync();
            Fn_CargarDatos();
            await Task.Delay(100);
        }

        public static async void Fn_GuardarRed(ObservableCollection<C_Medico> _medicos)
        {
            string _json = JsonConvert.SerializeObject(_medicos, Formatting.Indented);
            if (Current.Properties.ContainsKey(NombresAux.v_redmedica))
            {
                Current.Properties[NombresAux.v_redmedica] = "";
                Current.Properties[NombresAux.v_redmedica] = _json;
            }
            else
            {
                Current.Properties.Add(NombresAux.v_redmedica, "");
                Current.Properties[NombresAux.v_redmedica] = _json;
            }
            await Current.SavePropertiesAsync();
        }
        public static async void Fn_GuardarServcios(ObservableCollection<C_Servicios> _servicios)
        {
            string _json = JsonConvert.SerializeObject(_servicios, Formatting.Indented);
            if (Current.Properties.ContainsKey(NombresAux.v_serviciosmedicos))
            {
                Current.Properties[NombresAux.v_serviciosmedicos] = "";
                Current.Properties[NombresAux.v_serviciosmedicos] = _json;
            }
            else
            {
                Current.Properties.Add(NombresAux.v_serviciosmedicos, "");
                Current.Properties[NombresAux.v_serviciosmedicos] = _json;
            }
            await Current.SavePropertiesAsync();
        }
        public static async void Fn_GuardarGenerales(ObservableCollection<C_ServGenerales> _general)
        {
            string _json = JsonConvert.SerializeObject(_general, Formatting.Indented);
            if (Current.Properties.ContainsKey(NombresAux.v_serviciosgenereales))
            {
                Current.Properties[NombresAux.v_serviciosgenereales] = "";
                Current.Properties[NombresAux.v_serviciosgenereales] = _json;
            }
            else
            {
                Current.Properties.Add(NombresAux.v_serviciosgenereales, "");
                Current.Properties[NombresAux.v_serviciosgenereales] = _json;
            }
            await Current.SavePropertiesAsync();
        }

        public static async void Fn_CerrarSesion()
        {
            v_perfil = new C_PerfilGen();
            v_perfMed = new C_PerfilMed();
            v_folio = "";
            v_membresia = "";
            v_letra = "";
            v_log = "0";
            string _json = JsonConvert.SerializeObject(v_perfil, Formatting.Indented);
            Current.Properties[NombresAux.v_perMed] = _json;
            _json = JsonConvert.SerializeObject(v_perfMed, Formatting.Indented);
            Current.Properties[NombresAux.v_perMed] = _json;
            Current.Properties[NombresAux.v_membre] = v_membresia;
            Current.Properties[NombresAux.v_folio] = v_folio;
            Current.Properties[NombresAux.v_letra] = v_letra;
            await Current.SavePropertiesAsync();
            await Task.Delay(100);
        }
        public static void Fn_ImgSexo(int _valor)
        {/// 0 MEDICOS,   1 SERVICIOS MEDICOS,    2 SERVICIOS GENERALES
            switch (_valor)
            {
                case 0:
                    {
                        for (int i = 0; i < v_medicos.Count; i++)
                        {
                            if (v_medicos[i].v_idsexo == 0)
                            {
                                v_medicos[i].v_img = "doctor.png";
                            }
                            else
                            {
                                v_medicos[i].v_img = "doctora.png";
                            }
                            v_medicos[i].v_completo = v_medicos[i].v_titulo + " " + v_medicos[i].v_Nombre + " " +
                                v_medicos[i].v_Apellido;
                        }
                    }
                    break;
                default:
                    break;
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
           // Properties.Clear();
            if (Properties.ContainsKey(NombresAux.v_log))
            {
                //lee el valor guardado
                v_log = Current.Properties[NombresAux.v_log] as string;
                if (v_log == "0")
                {//no esta logeado
                    v_perfil = new C_PerfilGen();
                    v_perfMed = new C_PerfilMed();
                    v_membresia = "";
                    v_folio = "";
                    v_letra = "";
                    string _json = JsonConvert.SerializeObject(v_perfil, Formatting.Indented);
                    Properties[NombresAux.v_perfGen]= _json;
                    _json = JsonConvert.SerializeObject(v_perfMed, Formatting.Indented);
                    Properties[NombresAux.v_perMed] = _json;

                    Properties[NombresAux.v_letra] = v_letra;
                    Properties[NombresAux.v_membre] = v_membresia;
                    Properties[NombresAux.v_folio] = v_folio;
                    Fn_CargarListas();
                    MainPage = new V_Master(false, "Bienvenido a Trato Especial");
                }//si esta logeado
                else if (v_log == "1")
                {
                    string _jsonGen = Properties[NombresAux.v_perfGen] as string;
                    v_perfil = JsonConvert.DeserializeObject<C_PerfilGen>(_jsonGen);
                    string _jsonPerfMed = Properties[NombresAux.v_perMed] as string;
                    v_perfMed = JsonConvert.DeserializeObject<C_PerfilMed>(_jsonPerfMed);

                    v_folio = Properties[NombresAux.v_folio] as string;
                    v_membresia = Properties[NombresAux.v_membre] as string;
                    v_letra = Properties[NombresAux.v_letra] as string;
                    string _jsonServ = Current.Properties[NombresAux.v_serviciosmedicos] as string;
                    v_servicios = JsonConvert.DeserializeObject<ObservableCollection<C_Servicios>>(_jsonServ);
                    string _jsonMed = Current.Properties[NombresAux.v_redmedica] as string;
                    v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);
                    string _jsonGenerales = Current.Properties[NombresAux.v_serviciosgenereales] as string;
                    v_generales = JsonConvert.DeserializeObject<ObservableCollection<C_ServGenerales>>(_jsonGenerales);
                    MainPage = new V_Master(true, "Bienvenido " + v_perfil.v_Nombre);
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
                v_letra = "";
                v_membresia = "";
                Fn_CrearKey();
                Fn_CargarListas();
                App.Current.MainPage = new V_Master(false, "Bienvenido a Trato Especial");
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
        public static async void Fn_SetToken(string _token)
        {
            if (Current.Properties.ContainsKey(NombresAux.v_token))
            {
                Current.Properties[NombresAux.v_token] = _token;
            }
            else
            {
                Current.Properties.Add(NombresAux.v_token, "");
                Current.Properties[NombresAux.v_token] = _token;
            }
            await Current.SavePropertiesAsync();
        }
        public static string Fn_GEtToken()
        {
            if (Current.Properties.ContainsKey(NombresAux.v_token))
            {
                return Current.Properties[NombresAux.v_token].ToString();
            }
            else
            {
                return "a";
            }
        }
    }

}
