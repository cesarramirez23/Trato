using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Personas;
using Trato.Varios;
using Newtonsoft.Json;
using System.Net.Http;

//para agregar loos eventos al calendario
using Plugin.Calendars;
using Plugin.Calendars.Abstractions;
using Newtonsoft.Json.Linq;
//para agregar loos eventos al calendario


namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_NCita : ContentPage
	{
        C_Medico v_medico;
        Cita v_cita;
        bool v_nueva;
        string _esta;


        public V_NCita(Cita _cita, bool _nueva)
        {
            InitializeComponent();
            v_nueva = _nueva;
            v_cita = _cita;
            if(_nueva)
            {
                v_botCrear.IsVisible = true;
            }
            else
            {
                v_botCrear.IsVisible = false;
            }
            v_cita.Fn_SetValores();
            v_fecha.Date = v_cita.v_fechaDate;
            v_fecha.MinimumDate = DateTime.Now;
            v_fecha.MaximumDate = v_fecha.MinimumDate.AddMonths(1);
            v_fecha.IsEnabled = false;
            v_hora.IsEnabled = false;
            v_hora.Time = v_cita.v_hora;
            _esta = v_cita.v_estado;
            int _a = int.Parse(v_cita.v_estado);
            v_estado.Text = ((EstadoCita)_a).ToString();
            v_nombre.Text = v_cita.v_nombreDR;
            Fn_Botones(v_cita.v_estado);
            App.Fn_Borra();
        }

        public V_NCita( C_Medico _medico)
        {
            InitializeComponent();
            v_nueva = true;
            v_medico = _medico;
            v_botCrear.IsVisible = true;
            v_nombre.Text = v_medico.v_completo;
            v_fecha.MinimumDate = DateTime.Now;
            v_fecha.MaximumDate = DateTime.Today.AddMonths(1);
            v_estado.Text = (EstadoCita.Nueva).ToString();
            StackPendiente.IsVisible = false;
            StackTre.IsVisible = false;
        }
        public V_NCita(Cita _cita)
        {
            InitializeComponent();
            v_nueva = false;
            v_botCrear.IsVisible = false;
            v_cita = _cita;
            //v_cita.Fn_SetValores();
            v_fecha.Date = v_cita.v_fechaDate;
            v_fecha.IsEnabled = false;
            v_hora.IsEnabled = false;
            v_hora.Time = v_cita.v_hora;
            _esta = v_cita.v_estado;
            int _a = int.Parse(v_cita.v_estado);
            v_estado.Text = ((EstadoCita)_a).ToString();
            v_nombre.Text = v_cita.v_nombreDR;
            v_fecha.MinimumDate = DateTime.Now;
            v_fecha.MaximumDate = DateTime.Today.AddMonths(1);
            Fn_Botones(v_cita.v_estado);
        }
        public async void Fn_Crear(object sender, EventArgs _args)
        {
            if(v_nueva)
            {
                Cita _cita = new Cita(v_medico.v_membre, App.v_membresia, App.v_folio, "1",v_fecha.Date,
                 v_hora.Time, v_medico.v_tokenDR);
                string _json = JsonConvert.SerializeObject(_cita, Formatting.Indented);
                Console.Write("Info cita " + _json);
               // await DisplayAlert("Enviar", _json, "aceptar");
                HttpClient _client = new HttpClient();
                string _DirEnviar = "http://tratoespecial.com/set_citas.php";
                StringContent _content= new StringContent(_json, Encoding.UTF8, "application/json");
                try
                {  //getting exception in the following line    //HttpResponseMessage upd_now_playing = await cli.PostAsync(new Uri("http://ws.audioscrobbler.com/2.0/", UriKind.RelativeOrAbsolute), tunp);
                    HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                    if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                        if(_respuesta!="0")
                        {
                            await DisplayAlert("Exito", "Cita generada correctamente, espera la respuesta de tu doctor", "Aceptar");
                            await Navigation.PopAsync();
                        }
                        else 
                        {
                            await DisplayAlert("Error", "No se pudo agendar tu cita, intentalo mas tarde", "Aceptar");
                        }
                    }
                }
                catch(HttpRequestException ex)
                {
                    await DisplayAlert("Error",ex.Message, "Aceptar");
                }
            }
            else//aca actualizar el estado de la cita
            {

            }

        }
        /// <summary>
        /// Terminada = 0,  Nueva = 1, Pendiente = 2, Aceptada = 3, Cancelada = 4
        /// </summary>
        /// <param name="_valor"></param>
        private void Fn_Botones(string _valor)
        {
            switch (_valor)
            {
                case "0"://TErminada, ya paso
                    {
                        v_botAcep.IsEnabled = false;
                        v_botCambio.IsEnabled = false;
                        v_botRec.IsEnabled = false;
                    }
                    break;
                case "1"://nueva
                    {
                        v_botAcep.IsEnabled = false;
                        v_botCambio.IsEnabled = false;
                        v_botRec.IsEnabled = true;
                    }
                    break;
                case "2"://pendiente
                    {
                        v_botAcep.IsEnabled = true;
                        v_botCambio.IsEnabled = true;
                        v_botRec.IsEnabled = true;
                    }
                    break;
                case "3"://aceptada
                    {
                        v_botAcep.IsEnabled = false;
                        v_botCambio.IsEnabled = false;
                        v_botRec.IsEnabled = true;
                        Fn_Calendario(v_cita.v_fechaDate, v_cita.v_hora);
                    }
                    break;
                case "4"://cancelada
                    {
                        v_botAcep.IsEnabled = false;
                        v_botCambio.IsEnabled = false;
                        v_botRec.IsEnabled = false;
                    }
                    break;
            }
        }
        /// <summary>
        /// Terminada = 0,  Nueva = 1, Pendiente = 2, Aceptada = 3, Cancelada = 4
        /// </summary>
        /// <param name="_nuevoestado"></param>
        private async void Fn_ActualizarInfo(string _nuevoestado)
        {
            Fn_Botones("4");
            Cita _cita = new Cita(_nuevoestado, v_fecha.Date, v_hora.Time, v_cita.v_idCita,"0");
            string _json = JsonConvert.SerializeObject(_cita, Formatting.Indented);
          //  await DisplayAlert("Enviar", _json, "aceptar");
            HttpClient _client = new HttpClient();
            string _DirEnviar = "http://tratoespecial.com/update_citas.php";
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            try
            {  //getting exception in the following line    //HttpResponseMessage upd_now_playing = await cli.PostAsync(new Uri("http://ws.audioscrobbler.com/2.0/", UriKind.RelativeOrAbsolute), tunp);
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                    if (_respuesta == "1")
                    {
                        await DisplayAlert("Exito", "Cambios generados correctamente", "Aceptar");
                        ///aca tomar el nuevo estado y ver si es 3(aceptadaa), agregar el evento al calaendario, la cita
                        ///recoordatorio de horas antes y de 1 dia antes
                        if(_nuevoestado=="3")
                        {
                            Fn_Calendario(v_fecha.Date, v_hora.Time);
                        }
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo agendar tu cita, intentalo mas tarde", "Aceptar");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
            Fn_Botones("1");
        }
        private void Fn_Acep(object sender, EventArgs _Args)
        {
            Fn_ActualizarInfo("3");
        }
        private void Fn_Recha(object sender, EventArgs _Args)
        {
            Fn_ActualizarInfo("4");
        }
        /// <summary>
        /// Cuando quieres cambiar la fecha/hora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_Args"></param>
        private void Fn_Cambios(object sender, EventArgs _Args)
        {
            v_fecha.IsEnabled = true;
            v_hora.IsEnabled = true;
            StackPendiente.IsVisible = true;
            StackTre.IsVisible = false;
        }
        /// <summary>
        /// hacer los cambios en el calendario y reloj, luego aceptar los cambios
        /// </summary>
        private void Fn_AcepCambio(object sender, EventArgs _args)
        {
            StackPendiente.IsVisible = false;
            StackTre.IsVisible = true;
            Fn_ActualizarInfo("2");
        }
        private void Fn_CancelCambio(object sender, EventArgs _args)
        {
            v_fecha.Date = v_cita.v_fechaDate;
            v_fecha.IsEnabled = false;
            v_hora.Time = v_cita.v_hora;
            v_hora.IsEnabled = false;
            Fn_Botones(_esta);
            StackPendiente.IsVisible = false;
            StackTre.IsVisible = true;
        }      
        /// <summary>
        /// agrega la cita a tu calendario
        /// </summary>
        private async void Fn_Calendario(DateTime _fecha, TimeSpan _hora)
        {
            //los calendarios en el telefono,   el 0 es el calendario del sistema
            var TodosCalen = await CrossCalendars.Current.GetCalendarsAsync();

            //var _cal =await CrossCalendars.Current.GetCalendarByIdAsync(App.v_IdCalendar);


            //if(_cal.AccountName== "Trato Especial")//encuentrab el calendario
            //{
            if (string.IsNullOrEmpty( v_cita.v_idCalendar)|| string.IsNullOrWhiteSpace(v_cita.v_idCalendar) )//la cita no tiene ningun evento
            {
                DateTime _final = new DateTime(_fecha.Year, _fecha.Month, _fecha.Day, _hora.Hours, _hora.Minutes, _hora.Seconds);
                if (DateTime.Compare(DateTime.Now,_final)<0 )//la fecha actual es menor a la cita agregarla al calendario
                {
                    //info a mostrar en la agenda
                    var calendarEvent = new CalendarEvent
                    {
                        Name = "Cita desde Trato Especial",
                        Start = _final,
                        End = _final.AddHours(1),
                        Description = "Tienes agendada una cita con " + v_cita.v_nombreDR,
                        Reminders = new List<CalendarEventReminder>()
                        {
                            new  CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore=new TimeSpan(24, 0, 0) },
                            new  CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore=new TimeSpan(2, 0, 0) },
                            new  CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore=new TimeSpan(0, 10, 0) },
                            new  CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore=new TimeSpan(0, 30, 0) }
                        },
                        AllDay = false
                    };
                    //agregarlo
                    try
                    {

                        await CrossCalendars.Current.AddOrUpdateEventAsync(TodosCalen[0], calendarEvent);
                        v_cita.v_idCalendar = calendarEvent.ExternalID;//guarda el id del evento

                        string json = @"{";
                        json += "ID_cita:'" + v_cita.v_idCita + "',\n";
                        json += "idcalendario:'" + v_cita.v_idCalendar + "'\n";
                        json += "}";
                        JObject _json = JObject.Parse(json);
                        HttpClient _client = new HttpClient();
                        string _DirEnviar = "http://tratoespecial.com/update_calendario.php";
                        StringContent _content = new StringContent(_json.ToString(), Encoding.UTF8, "application/json");
                        try
                        {
                            HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                            if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                                if (_respuesta == "1")
                                {
                                    await DisplayAlert("Exito", "Agregado a tu agenda correctamente", "Aceptar");
                                }
                                else
                                {
                                    await DisplayAlert("Error", "No se pudo agendar tu cita, intentalo mas tarde", "Aceptar");
                                }
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            await DisplayAlert("Error", ex.Message, "Aceptar");
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        await DisplayAlert("Error calendario", ex.Message, "asa");
                    }
                }
                else
                {
                    v_cita.v_idCalendar = "-1";
                    string json = @"{";
                    json += "ID_cita:'" + v_cita.v_idCita + "',\n";
                    json += "idcalendario:'" + v_cita.v_idCalendar + "'\n";
                    json += "}";
                    JObject _json = JObject.Parse(json);
                    HttpClient _client = new HttpClient();
                    string _DirEnviar = "http://tratoespecial.com/update_calendario.php";
                    StringContent _content = new StringContent(_json.ToString(), Encoding.UTF8, "application/json");
                    try
                    {
                        HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                        if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                            if (_respuesta == "1")
                            {
                                await DisplayAlert("Error", "La fecha que se trata de agendar, ya ha pasado", "Continuar");
                            }
                            else
                            {
                                await DisplayAlert("Error", "La fecha que se trata de agendar, ya ha pasado", "Continuar");
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        await DisplayAlert("Error", "La fecha que se trata de agendar, ya ha pasado", "Continuar");
                    }
                }
            }
            //}
            //crear recordatorios
            //List<CalendarEventReminder> _lista = new List<CalendarEventReminder>();
            //CalendarEventReminder _va = new CalendarEventReminder();
            //_va.Method = CalendarReminderMethod.Alert;
            //_va.TimeBefore = new TimeSpan(24, 0, 0);
            //_lista.Add(_va);
            //_va.TimeBefore = new TimeSpan(2, 0, 0);
            //_lista.Add(_va);
            //_va.TimeBefore = new TimeSpan(0, 10, 0);
            //_lista.Add(_va);
            //DateTime _final = new DateTime(_fecha.Year, _fecha.Month, _fecha.Day, _hora.Hours, _hora.Minutes, _hora.Seconds);

            ////info a mostrar en la agenda
            //var calendarEvent = new CalendarEvent
            //{
            //    Name = "Cita desde Trato Especial",
            //    Start = _final,
            //    //End = DateTime.Now.AddHours(1),
            //    End =_final.AddHours(1),
            //    Description = "Tienes agendada una cita con " + v_cita.v_nombreDR,
            //    Reminders = new List<CalendarEventReminder>()
            //    {
            //        new  CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore=new TimeSpan(24, 0, 0) },
            //        new  CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore=new TimeSpan(2, 0, 0) },
            //        new  CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore=new TimeSpan(0, 10, 0) },
            //        new  CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore=new TimeSpan(0, 30, 0) }
            //    },
            //    AllDay = false
            //};

            //if(TodosCalen[0].ExternalID  .Contains(calendarEvent))
            //{

            //}

            //Calendar _nuevoCal = new Calendar()
            //{
            //    AccountName = "Trato Especial", Name = "Trato Especial", Color = "2896D1"

            //};

            //if(!TodosCalen.Contains(_nuevoCal))
            //{
            //await CrossCalendars.Current.AddOrUpdateCalendarAsync(_nuevoCal);

            //}

            //agregarlo
            //await CrossCalendars.Current.AddOrUpdateEventAsync(TodosCalen[0], calendarEvent);
        }
    }
}