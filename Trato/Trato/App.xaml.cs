using System;
using Xamarin.Forms;
using Trato.Views;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Collections.ObjectModel;// para las listas
using Trato.Personas;//cargar las clases
using System.Threading.Tasks; // delay 
using Newtonsoft.Json;
using Trato.Varios;
using Trato.Models;
//para agregar loos eventos al calendario
using Plugin.Calendars;
using Plugin.Calendars.Abstractions;
//para agregar loos eventos al calendario
[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Trato
{
    public partial class App : Application
    {
        #region Listas de servicios y citas
        public static ObservableCollection<C_Medico> v_medicos = new ObservableCollection<C_Medico>();
        public static ObservableCollection<C_Servicios> v_servicios = new ObservableCollection<C_Servicios>();
        public static ObservableCollection<C_ServGenerales> v_generales = new ObservableCollection<C_ServGenerales>();
        public static ObservableCollection<Cita> v_citas = new ObservableCollection<Cita>();
        public static ObservableCollection<C_NotaMed> v_NotasMedic = new ObservableCollection<C_NotaMed>();
        #endregion
        #region PErfil y cosas propias para el login
        public static C_Validar v_valida = new C_Validar();
        /// <summary>
        /// Perfil General
        /// </summary>
        public static C_PerfilGen v_perfil = new C_PerfilGen();
        public static C_PerfilMed v_perfMed = new C_PerfilMed();
        /// <summary>
        /// viene todo completo(1810I-0558) ya trae la letra, hacer el split con -
        /// </summary>
        public static string v_membresia = "";
        /// <summary>
        /// la letra de la membresia( I F E)
        /// </summary>
        public static string v_letra = "";
        /// <summary>
        /// folio, 0 I, 1-5 F,
        /// </summary>
        public static string v_folio = "";
        /// <summary>
        /// 0 no esta logeado 1 logeado
        /// </summary>
        public static string v_log;
        //public static string v_IdCalendar = "";
        #endregion

        #region Propias de la app
        public App()
        {
            InitializeComponent();
        }
        protected override void OnStart()
        {    //existe la variable guardada
            Properties.Clear();
            if (Properties.ContainsKey(NombresAux.v_log))
            {
                //lee el valor guardado
                v_log = Current.Properties[NombresAux.v_log] as string;
                if (v_log == "0")
                {//no esta logeado
                    Fn_CargarDatos();
                    v_perfil = new C_PerfilGen();
                    v_perfMed = new C_PerfilMed();
                    v_membresia = "0000D-0000";
                    v_folio = "";
                    v_letra = "";
                    string _json = JsonConvert.SerializeObject(v_perfil);
                    Properties[NombresAux.v_perfGen] = _json;
                    _json = JsonConvert.SerializeObject(v_perfMed);
                    Properties[NombresAux.v_perMed] = _json;
                    Properties[NombresAux.v_letra] = v_letra;
                    Properties[NombresAux.v_membre] = v_membresia;
                    Properties[NombresAux.v_folio] = v_folio;
                    //v_IdCalendar = Current.Properties[NombresAux.v_IdCalendar] as string;
                    //Fn_CargarListas();

                    MainPage = new V_Master(false, "Bienvenido a Trato Especial");
                    //MainPage = new Page1();
                }//si esta logeado
                else if (v_log == "1")
                {
                    /*if(v_log=="1" && Fn_GEtToken()=="a")
                    {
                        Fn_CerrarSesion();
                        MainPage = new V_Master(false, "Bienvenido a Trato Especial");
                    }*/
                    Fn_CargarDatos();
                    if (!Current.Properties.ContainsKey(NombresAux.v_perfGen))
                    {
                        v_perfil = new C_PerfilGen();
                        string _json = JsonConvert.SerializeObject(v_perfil);
                        Current.Properties.Add(NombresAux.v_perfGen, "");
                        Current.Properties[NombresAux.v_perfGen] = _json;
                    }
                    else
                    {
                        string _jsonGen = Current.Properties[NombresAux.v_perfGen] as string;
                        v_perfil = JsonConvert.DeserializeObject<C_PerfilGen>(_jsonGen);
                        Console.Write("cargca carga " + v_perfil.Fn_GetDatos() + "\n");
                    }
                    // v_IdCalendar = Current.Properties[NombresAux.v_IdCalendar] as string;
                    //MainPage = new Page1();
                    MainPage = new V_Master(true, "Bienvenido " + v_perfil.v_Nombre);
                }
                else
                {
                    //MainPage = new Page1();
                    MainPage = new V_Master(false, "Bienvenido a Trato Especial");
                }
            }
            else//es la primera ve que abre la app
            {
                v_log = "0";
                v_perfil = new C_PerfilGen();
                v_perfMed = new C_PerfilMed();
                v_folio = "";
                v_letra = "";
                v_membresia = "0000D-0000";
                Fn_CrearKey();
                Fn_CargarListas();
                App.Current.MainPage = new Inicio();
                //App.Current.MainPage = new V_Master(false, "Bienvenido a Trato Especial");
            }
        }
        protected override void OnSleep() { }
        protected override void OnResume() { }
        #endregion
        #region CCARGA DE DATOS DESDE  EL PROPERTIES
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
                string _json = JsonConvert.SerializeObject(v_perfil);
                Current.Properties.Add(NombresAux.v_perfGen, "");
                Current.Properties[NombresAux.v_perfGen] = _json;
            }
            if (!Properties.ContainsKey(NombresAux.v_perMed))
            {
                v_perfMed = new C_PerfilMed();
                string _json = JsonConvert.SerializeObject(v_perfMed);
                Current.Properties.Add(NombresAux.v_perMed, "");
                Current.Properties[NombresAux.v_perMed] = _json;
            }
            if (!Properties.ContainsKey(NombresAux.v_redmedica2))
            {
                v_medicos = new ObservableCollection<C_Medico>();
                string _json = JsonConvert.SerializeObject(v_medicos);
                Current.Properties.Add(NombresAux.v_redmedica2, "");
                Current.Properties[NombresAux.v_redmedica2] = _json;
                if (Current.Properties.ContainsKey(NombresAux.v_redmedica))//eliminar el valor anterior cuando la red medica
                {
                    Current.Properties.Remove(NombresAux.v_redmedica);
                }
            }
            if (!Properties.ContainsKey(NombresAux.v_serviciosmedicos))
            {
                v_servicios = new ObservableCollection<C_Servicios>();
                string _json = JsonConvert.SerializeObject(v_servicios);
                Current.Properties.Add(NombresAux.v_serviciosmedicos, "");
                Current.Properties[NombresAux.v_serviciosmedicos] = _json;
            }
            if (!Properties.ContainsKey(NombresAux.v_serviciosgenereales))
            {
                v_servicios = new ObservableCollection<C_Servicios>();
                string _json = JsonConvert.SerializeObject(v_servicios);
                Current.Properties.Add(NombresAux.v_serviciosgenereales, "");
                Current.Properties[NombresAux.v_serviciosgenereales] = _json;
            }
            if (!Properties.ContainsKey(NombresAux.v_citas))
            {
                v_citas = new ObservableCollection<Cita>();
                string _json = JsonConvert.SerializeObject(v_citas);
                Current.Properties.Add(NombresAux.v_citas, "");
                Current.Properties[NombresAux.v_citas] = _json;
            }
            if (!Properties.ContainsKey(NombresAux.v_Nota))
            {
                v_NotasMedic = new ObservableCollection<C_NotaMed>();
                string _json = JsonConvert.SerializeObject(v_NotasMedic);
                Current.Properties.Add(NombresAux.v_Nota, "");
                Current.Properties[NombresAux.v_Nota] = _json;
            }
            //CIta para la notif
            if (!Properties.ContainsKey(NombresAux.v_citaNot))
            {
                v_nueva = new Cita();
                string _json = JsonConvert.SerializeObject(v_nueva);
                Properties.Add(NombresAux.v_citaNot, _json);
            }
            if (!Properties.ContainsKey(NombresAux.v_filtro))
            {
                List<string>[] _arr =new List<string>[]
                    {
                        new List<string>(),
                        new List<string>(),
                        new List<string>()
                    };
                string _json = JsonConvert.SerializeObject(_arr);
                Properties.Add(NombresAux.v_filtro, _json);
            }
            if (!Properties.ContainsKey(NombresAux.v_filCita))
            {
                List<string> _arr = new List<string>();
                string _json = JsonConvert.SerializeObject(_arr);
                Properties.Add(NombresAux.v_filCita, _json);
            }
            //if (!Properties.ContainsKey(NombresAux.v_IdCalendar))
            //{
            //    v_IdCalendar = "";
            //    var TodosCalen = await CrossCalendars.Current.GetCalendarsAsync();
            //    Calendar _nuevoCal = new Calendar()
            //    {
            //        AccountName = "Trato Especial",
            //        Name = "Trato Especial",
            //        Color = "2896D1"
            //    };
            //    if (!TodosCalen.Contains(_nuevoCal))
            //    {
            //        await CrossCalendars.Current.AddOrUpdateCalendarAsync(_nuevoCal);
            //        v_IdCalendar = _nuevoCal.ExternalID;
            //        Current.Properties.Add(NombresAux.v_IdCalendar, v_IdCalendar);
            //    }
            //}
            if (!Properties.ContainsKey(NombresAux.v_validar))
            {
                v_valida = new C_Validar();
                string _json = JsonConvert.SerializeObject(v_valida);
                Properties.Add(NombresAux.v_validar, _json);
            }
            await Current.SavePropertiesAsync();
        }
        /// <summary>
        /// se cargan las listas
        /// </summary>
        async void Fn_CargarListas()
        {
            if (!Current.Properties.ContainsKey(NombresAux.v_redmedica2))
            {
                v_medicos = new ObservableCollection<C_Medico>();
                string _json = JsonConvert.SerializeObject(v_medicos);
                Current.Properties.Add(NombresAux.v_redmedica2, "");
                Current.Properties[NombresAux.v_redmedica2] = _json;
                if (Current.Properties.ContainsKey(NombresAux.v_redmedica))//eliminar el valor anterior cuando la red medica
                {
                    Current.Properties.Remove(NombresAux.v_redmedica);
                }
            }
            else
            {
                string _jsonMed = Current.Properties[NombresAux.v_redmedica2] as string;
                v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);
                if (Current.Properties.ContainsKey(NombresAux.v_redmedica))//eliminar el valor anterior cuando la red medica
                {
                    Current.Properties.Remove(NombresAux.v_redmedica);
                }
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
            if (!Current.Properties.ContainsKey(NombresAux.v_citas))
            {
                v_citas = new ObservableCollection<Cita>();
                string _json = JsonConvert.SerializeObject(v_citas);
                Current.Properties.Add(NombresAux.v_citas, "");
                Current.Properties[NombresAux.v_citas] = _json;
            }
            else
            {
                string _jsonCita = Current.Properties[NombresAux.v_citas] as string;
                v_citas = JsonConvert.DeserializeObject<ObservableCollection<Cita>>(_jsonCita);
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_Nota))
            {
                v_NotasMedic = new ObservableCollection<C_NotaMed>();
                string _json = JsonConvert.SerializeObject(v_NotasMedic);
                Current.Properties.Add(NombresAux.v_Nota, "");
                Current.Properties[NombresAux.v_Nota] = _json;
            }
            else
            {
                string _jsonCita = Current.Properties[NombresAux.v_Nota] as string;
                v_NotasMedic = JsonConvert.DeserializeObject<ObservableCollection<C_NotaMed>>(_jsonCita);
            }
            await Current.SavePropertiesAsync();
            await Task.Delay(100);
            //aca se hace el set de los doctores
        }
        public static async void Fn_CargarDatos()
        {
            if (!Current.Properties.ContainsKey(NombresAux.v_membre))
            {
                v_membresia = "0000D-0000";
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
                string _json = JsonConvert.SerializeObject(v_perfil);
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
                string _json = JsonConvert.SerializeObject(v_perfMed);
                Current.Properties.Add(NombresAux.v_perMed, "");
                Current.Properties[NombresAux.v_perMed] = _json;
            }
            else
            {
                string _jsonMed = Current.Properties[NombresAux.v_perMed] as string;
                v_perfMed = JsonConvert.DeserializeObject<C_PerfilMed>(_jsonMed);
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_redmedica2))
            {
                v_medicos = new ObservableCollection<C_Medico>();
                string _json = JsonConvert.SerializeObject(v_medicos);
                Current.Properties.Add(NombresAux.v_redmedica2, "");
                Current.Properties[NombresAux.v_redmedica2] = _json;
                if (Current.Properties.ContainsKey(NombresAux.v_redmedica))//eliminar el valor anterior cuando la red medica
                {
                    Current.Properties.Remove(NombresAux.v_redmedica);
                }
            }
            else
            {
                string _jsonMed = Current.Properties[NombresAux.v_redmedica2] as string;
                v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_jsonMed);
                if (Current.Properties.ContainsKey(NombresAux.v_redmedica))//eliminar el valor anterior cuando la red medica
                {
                    Current.Properties.Remove(NombresAux.v_redmedica);
                }
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
            if (!Current.Properties.ContainsKey(NombresAux.v_serviciosgenereales))
            {
                v_generales = new ObservableCollection<C_ServGenerales>();
                string _json = JsonConvert.SerializeObject(v_generales);
                Current.Properties.Add(NombresAux.v_serviciosgenereales, "");
                Current.Properties[NombresAux.v_serviciosgenereales] = _json;
            }
            else
            {
                string _jsonServ = Current.Properties[NombresAux.v_serviciosgenereales] as string;
                v_generales = JsonConvert.DeserializeObject<ObservableCollection<C_ServGenerales>>(_jsonServ);
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_citas))
            {
                v_citas = new ObservableCollection<Cita>();
                string _json = JsonConvert.SerializeObject(v_citas);
                Current.Properties.Add(NombresAux.v_citas, "");
                Current.Properties[NombresAux.v_citas] = _json;
            }
            else
            {
                string _jsonCitas = Current.Properties[NombresAux.v_citas] as string;
                v_citas = JsonConvert.DeserializeObject<ObservableCollection<Cita>>(_jsonCitas);
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_Nota))
            {
                v_NotasMedic = new ObservableCollection<C_NotaMed>();
                string _json = JsonConvert.SerializeObject(v_NotasMedic);
                Current.Properties.Add(NombresAux.v_Nota, "");
                Current.Properties[NombresAux.v_Nota] = _json;
            }
            else
            {
                string _jsonCitas = Current.Properties[NombresAux.v_Nota] as string;
                v_NotasMedic = JsonConvert.DeserializeObject<ObservableCollection<C_NotaMed>>(_jsonCitas);
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_filtro))
            {
                List<string>[] _arr = new List<string>[]
                    {
                        new List<string>(),
                        new List<string>(),
                        new List<string>()
                    };
                string _json = JsonConvert.SerializeObject(_arr);
                Current.Properties.Add(NombresAux.v_filtro, _json);
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_filCita))
            {
                List<string> _arr = new List<string>();
                string _json = JsonConvert.SerializeObject(_arr);
                Current.Properties.Add(NombresAux.v_filCita, _json);
            }
            if (!Current.Properties.ContainsKey(NombresAux.v_validar))
            {
                v_valida = new C_Validar();
                string _json = JsonConvert.SerializeObject(v_valida);
                Current.Properties.Add(NombresAux.v_validar, "");
                Current.Properties[NombresAux.v_validar] = _json;
            }
            else
            {
                string _json = Current.Properties[NombresAux.v_validar] as string;
                v_valida = JsonConvert.DeserializeObject<C_Validar>(_json);
            }
            //ID DEL CALENDARIO
            //if (!Current.Properties.ContainsKey(NombresAux.v_IdCalendar))
            //{
            //    v_IdCalendar = "";
            //    var TodosCalen = await CrossCalendars.Current.GetCalendarsAsync();
            //    Calendar _nuevoCal = new Calendar()
            //    {
            //        AccountName = "Trato Especial",
            //        Name = "Trato Especial",
            //        Color = "2896D1"
            //    };
            //    if (!TodosCalen.Contains(_nuevoCal))
            //    {
            //        await CrossCalendars.Current.AddOrUpdateCalendarAsync(_nuevoCal);
            //        v_IdCalendar = _nuevoCal.ExternalID;
            //        Current.Properties.Add(NombresAux.v_IdCalendar, v_IdCalendar);
            //    }
            //}
            //else
            //{
            //    v_IdCalendar = Current.Properties[NombresAux.v_IdCalendar] as string;
            //}
            await Task.Delay(100);
        }
        #endregion      
        #region GUARDAR LOS DATOS
        public static async void Fn_GuardarDatos(C_PerfilGen _gen, string _membre, string _folio, string _letra)
        {
            v_perfil = _gen;
            v_folio = _folio;
            v_membresia = _membre;
            v_letra = _letra;
            string _jsonGen = JsonConvert.SerializeObject(v_perfil);
            Current.Properties[NombresAux.v_log] = v_log;
            Current.Properties[NombresAux.v_perfGen] = _jsonGen;
            Current.Properties[NombresAux.v_membre] = v_membresia;
            Current.Properties[NombresAux.v_folio] = v_folio;
            Current.Properties[NombresAux.v_letra] = v_letra;
            string _jsoServ = JsonConvert.SerializeObject(v_servicios);
            Current.Properties[NombresAux.v_serviciosmedicos] = _jsoServ;
            string _jsonMed = JsonConvert.SerializeObject(v_medicos);
            Current.Properties[NombresAux.v_redmedica2] = _jsonMed;
            await Current.SavePropertiesAsync();
            Fn_CargarDatos();
            await Task.Delay(100);
        }
        public static async void Fn_GuardarDatos(string _gen, string _membre, string _folio, string _letra)
        {
            v_perfil = JsonConvert.DeserializeObject<C_PerfilGen>(_gen);
            v_folio = _folio;
            v_membresia = _membre;
            v_letra = _letra;
            //string _jsonGen = JsonConvert.SerializeObject(v_perfil);
            Current.Properties[NombresAux.v_log] = v_log;
            Current.Properties[NombresAux.v_perfGen] = _gen;
            Current.Properties[NombresAux.v_membre] = v_membresia;
            Current.Properties[NombresAux.v_folio] = v_folio;
            Current.Properties[NombresAux.v_letra] = v_letra;
            string _jsoServ = JsonConvert.SerializeObject(v_servicios);
            Current.Properties[NombresAux.v_serviciosmedicos] = _jsoServ;
            string _jsonMed = JsonConvert.SerializeObject(v_medicos);
            Current.Properties[NombresAux.v_redmedica2] = _jsonMed;
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
            string _jsonPerMed = JsonConvert.SerializeObject(v_perfMed);
            Current.Properties[NombresAux.v_log] = v_log;
            Current.Properties[NombresAux.v_perMed] = _jsonPerMed;
            Current.Properties[NombresAux.v_membre] = v_membresia;
            Current.Properties[NombresAux.v_folio] = v_folio;
            Current.Properties[NombresAux.v_letra] = v_letra;
            string _jsoServ = JsonConvert.SerializeObject(v_servicios);
            Current.Properties[NombresAux.v_serviciosmedicos] = _jsoServ;
            string _jsonMed = JsonConvert.SerializeObject(v_medicos);
            Current.Properties[NombresAux.v_redmedica2] = _jsonMed;

            await Current.SavePropertiesAsync();
            Fn_CargarDatos();
            await Task.Delay(100);
        }
        public static async void Fn_GuardarRed(ObservableCollection<C_Medico> _medicos)
        {
            string _json = JsonConvert.SerializeObject(_medicos);
            if (Current.Properties.ContainsKey(NombresAux.v_redmedica2))
            {
                Current.Properties[NombresAux.v_redmedica2] = "";
                Current.Properties[NombresAux.v_redmedica2] = _json;
            }
            else
            {
                Current.Properties.Add(NombresAux.v_redmedica2, "");
                Current.Properties[NombresAux.v_redmedica2] = _json;
            }
            //if(Current.Properties.ContainsKey(NombresAux.v_redmedica))//eliminar el valor anterior cuando la red medica
            //{
            //    Current.Properties.Remove(NombresAux.v_redmedica);
            //}
            await Current.SavePropertiesAsync();
        }
        public static async void Fn_GuardarServcios(ObservableCollection<C_Servicios> _servicios)
        {
            string _json = JsonConvert.SerializeObject(_servicios);
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
            string _json = JsonConvert.SerializeObject(_general);
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
        public static async void Fn_GuardarCitas(ObservableCollection<Cita> _citas)
        {
            v_citas = _citas;
            string _json = JsonConvert.SerializeObject(v_citas);
            if (Current.Properties.ContainsKey(NombresAux.v_citas))
            {
                Current.Properties[NombresAux.v_citas] = "";
                Current.Properties[NombresAux.v_citas] = _json;
            }
            else
            {
                Current.Properties.Add(NombresAux.v_citas, "");
                Current.Properties[NombresAux.v_citas] = _json;
            }
            await Current.SavePropertiesAsync();
        }
        /// <summary>
        /// guarda la nota medica
        /// </summary>
        public static async void Fn_GuardarMedicamentos(ObservableCollection<C_NotaMed> _medica)
        {
            v_NotasMedic = _medica;
            string _json = JsonConvert.SerializeObject(v_NotasMedic);
            if (Current.Properties.ContainsKey(NombresAux.v_Nota))
            {
                Current.Properties[NombresAux.v_Nota] = "";
                Current.Properties[NombresAux.v_Nota] = _json;
            }
            else
            {
                Current.Properties.Add(NombresAux.v_Nota, "");
                Current.Properties[NombresAux.v_Nota] = _json;
            }
            await Current.SavePropertiesAsync();
        }
        public static async void Fn_GuardaFiltro(List<string>[] _filtro)
        {
            string _json = JsonConvert.SerializeObject(_filtro);
            Current.Properties[NombresAux.v_filtro] = _json;
            await Current.SavePropertiesAsync();
        }
        public static async void Fn_GuardaFiltro(List<string> _filtro)
        {
            string _json = JsonConvert.SerializeObject(_filtro);
            Current.Properties[NombresAux.v_filCita] = _json;
            await Current.SavePropertiesAsync();
        }
        public static async void Fn_GuardaValidacion(C_Validar _val)
        {
            string _json = JsonConvert.SerializeObject(_val);
            Current.Properties[NombresAux.v_validar] = _json;
            await Current.SavePropertiesAsync();
        }
        #endregion
        #region Varios
        public static async void Fn_CerrarSesion()
        {
            v_perfil = new C_PerfilGen();
            v_perfMed = new C_PerfilMed();
            v_NotasMedic = new ObservableCollection<C_NotaMed>();
            v_citas = new ObservableCollection<Cita>();
            v_membresia = "0000D-0000";
            v_folio = "";
            v_letra = "";
            v_log = "0";
            string _json = JsonConvert.SerializeObject(v_perfil);
            Current.Properties[NombresAux.v_perfGen] = _json;
            _json = JsonConvert.SerializeObject(v_perfMed);
            Current.Properties[NombresAux.v_perMed] = _json;
            _json = JsonConvert.SerializeObject(v_NotasMedic);
            Current.Properties[NombresAux.v_Nota] = _json;
            _json = JsonConvert.SerializeObject(v_citas);
            Current.Properties[NombresAux.v_Nota] = _json;

            Current.Properties[NombresAux.v_membre] = v_membresia;
            Current.Properties[NombresAux.v_folio] = v_folio;
            Current.Properties[NombresAux.v_letra] = v_letra;
            Current.Properties[NombresAux.v_log] = v_log;
            await Current.SavePropertiesAsync();
            await Task.Delay(100);
        }
        public static void Fn_ImgSexo()
        {/// 0 MEDICOS,   1 SERVICIOS MEDICOS,    2 SERVICIOS GENERALES

            for (int i = 0; i < v_medicos.Count; i++)
            {
                if (v_medicos[i].v_idsexo == 0)
                {
                    v_medicos[i].v_img = NombresAux.BASE_URL + "imgs/dr_app.jpeg";
                }
                else
                {
                    v_medicos[i].v_img = NombresAux.BASE_URL + "imgs/dra_app.jpeg";
                }
                v_medicos[i].v_completo = v_medicos[i].v_titulo + " " + v_medicos[i].v_Nombre + " " +
                    v_medicos[i].v_Apellido;
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
        public static List<string>[] Fn_Getfiltro()
        {
            List<string>[] _ret ;
            string  _sa=Current.Properties[NombresAux.v_filtro] as string;
            _ret = JsonConvert.DeserializeObject<List<string>[]>(_sa);
            return _ret;
        }
        public static List<string> Fn_GetfiltroCita()
        {
            List<string> _ret;
            string _sa = Current.Properties[NombresAux.v_filCita] as string;
            _ret = JsonConvert.DeserializeObject<List<string>>(_sa);
            return _ret;
        }
        #endregion
        #region Coosas de la cita
        /// <summary>
        /// se manda un id y busca en todos los medicamentos
        /// </summary>
        public static ObservableCollection<Medicamentos> Fn_GetMedic(string _idcita)
        {
            bool _re = false;
            ObservableCollection<Medicamentos> _ret = new ObservableCollection<Medicamentos>();
            for (int i = 0; i < v_NotasMedic.Count; i++)
            {
                if (v_NotasMedic[i].v_idCita == _idcita && !_re)
                {
                    _ret=v_NotasMedic[i].v_medic;
                    _re = true;
                }
            }
            return _ret;
        }
        public static string Fn_GetNombreCita(string _idcita)
        {
            string _nombre = "";
            for (int i = 0; i < v_NotasMedic.Count; i++)
            {
                if (v_NotasMedic[i].v_idCita == _idcita )
                {
                    _nombre = v_NotasMedic[i].v_titulo + " " + v_NotasMedic[i].v_nombreDr;
                }
            }
            return _nombre;
        }
        #endregion
        #region Notificaciones
        /// <summary>
        /// la cita desde la notif
        /// </summary>
        public static Cita v_nueva;
        /// <summary>
        /// cuando lo pones desde la notif
        /// </summary>
        public static async void Fn_SetCita(Cita _nueva)
        {
            v_nueva = _nueva;
            string _json = JsonConvert.SerializeObject(v_nueva);
            if (Current.Properties.ContainsKey(NombresAux.v_citaNot))
            {
                Current.Properties[NombresAux.v_citaNot] = _json;
            }
            else
            {
                Current.Properties.Add(NombresAux.v_citaNot, "");
                Current.Properties[NombresAux.v_citaNot] = _json;
            }
            await Current.SavePropertiesAsync();
            await Task.Delay(100);
        }
        /// <summary>
        /// get cita de la notif
        /// </summary>
        public static bool Fn_GetCita()
        {
            if (Current.Properties.ContainsKey(NombresAux.v_citaNot))
            {
                string _json = Current.Properties[NombresAux.v_citaNot] as string;
                v_nueva = JsonConvert.DeserializeObject<Cita>(_json);
                if (v_nueva.v_estado != "-1")//tiene valores
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public async static void Fn_Borra()
        {
            v_nueva = new Cita();
            string _json = JsonConvert.SerializeObject(v_nueva);
            Current.Properties[NombresAux.v_citaNot] = _json;
            await Current.SavePropertiesAsync();
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
        #endregion
    }
}
