using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Varios;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//para agregar loos eventos al calendario
using Plugin.Calendars;
using Plugin.Calendars.Abstractions;

//para agregar loos eventos al calendario
namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Medicamento : ContentPage
	{
        Cita v_cita;
        ObservableCollection<Medicamentos> v_medicamentos = new ObservableCollection<Medicamentos>();

		public V_Medicamento (Cita _cita)
		{
			InitializeComponent ();
            v_cita = _cita;
            Title = v_cita.v_idCita;
            fecha.Text = v_cita.v_fecha +"      "+ v_cita.v_hora.Hours+":" +v_cita.v_hora.Minutes;
            v_medicamentos = App.Fn_GetMedic(v_cita.v_idCita);
            nombre.Text = v_cita.v_nombreDR + "  " +v_cita.v_especialidad;
            if(v_medicamentos.Count>0)
            {
                for(int i=0;i<v_medicamentos.Count; i++)
                {
                    v_medicamentos[i].Fn_SetTexto();
                }
                ListaMed.ItemsSource = v_medicamentos;
            }
            else
            {
                nota.Text += "\nNo existe medicamentos registrados";
            }
            
		}
        public async void Fn_SetEstado(object sender, EventArgs _args)
        {
            Button button = sender as Button;
            Medicamentos _medi = button.BindingContext as Medicamentos;
            if(_medi.v_estado=="0")
            {
                bool _res =await DisplayAlert("Iniciar Tratamiento", "Seguro de iniciar","Continuar", "Cancelar");
                if(_res)
                {
                    Fn_CalendarioMedi(_medi.v_nombre, _medi.v_extra, _medi.v_dosis, _medi.v_periodo, _medi.v_tiempo);

                    HttpClient _client = new HttpClient();
                    string json = @"{";
                    json += "id:'" + _medi.v_idMedi + "',\n";
                    json += "estado:'" + "1" + "'\n";
                    json += "}";
                    JObject jsonper = JObject.Parse(json);
                    string _DirEnviar = "http://tratoespecial.com/update_medicamentos.php";
                    StringContent _content = new StringContent(jsonper.ToString(), Encoding.UTF8, "application/json");
                   // await DisplayAlert("Exito", jsonper.ToString(), "Aceptar");
                    try
                    {
                        HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                        if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                            if(_respuesta=="1")
                            {
                                await DisplayAlert("Exito", _respuesta, "Aceptar");                               
                                Fn_Cambio(_medi, "1");
                            }
                            else
                            {
                                await DisplayAlert("Falla", _respuesta, "Aceptar");
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        await DisplayAlert("Error", ex.Message.ToString(), "Aceptar");
                    }
                }
            }
            else if(_medi.v_estado=="1")//terminar tratamiento
            {
                bool _res = await DisplayAlert("Terminar Tratamiento", "Seguro de Terminar", "Continuar", "Cancelar");
                if (_res)
                {
                    HttpClient _client = new HttpClient();
                    string json = @"{";
                    json += "id:'" + _medi.v_idMedi + "',\n";
                    json += "estado:'" + "2" + "'\n";
                    json += "}";
                    JObject jsonper = JObject.Parse(json);
                    string _DirEnviar = "http://tratoespecial.com/update_medicamentos.php";
                    StringContent _content = new StringContent(jsonper.ToString(), Encoding.UTF8, "application/json");
                    //await DisplayAlert("Exito", jsonper.ToString(), "Aceptar");
                    try
                    {
                        HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                        if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                            if (_respuesta == "1")
                            {
                                await DisplayAlert("Exito", _respuesta, "Aceptar");
                                Fn_Cambio(_medi, "2");
                            }
                            else
                            {
                                await DisplayAlert("Falla", _respuesta, "Aceptar");
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        await DisplayAlert("Error", ex.Message.ToString(), "Aceptar");
                    }
                }
            }
            else
            {
                await DisplayAlert("Tratamiento terminado","NO PUEDE HACER NADA"+ _medi.v_estado+"  id "+_medi.v_idMedi, "Aceptar");
            }
            await Task.Delay(100);
        }
        /// <summary>
        /// actualizar la lista que ves al estado que le corresponde
        /// </summary>
        async void Fn_Cambio(Medicamentos _temp, string _nuevo)
        {
            for (int i = 0; i < v_medicamentos.Count; i++)
            {
                if (v_medicamentos[i] == _temp)
                {
                    v_medicamentos[i].v_estado = _nuevo;
                    v_medicamentos[i].Fn_SetTexto();
                }
            }
            Fn_ListaRefresh(ListaMed,null);
            await Task.Delay(100);
        }
        /// <summary>
        /// agrega los medicamentos al calendario
        /// </summary>
        public async void Fn_CalendarioMedi(string _nombre, string _extra,string _dosis, float _dias, float _horas)
        {
            int _horaH =(int) Math.Floor(_horas);//horas sin decimal
            float _horaDif = _horas - _horaH;//sacar los decimales
            int _horaMin = (int)(_horaDif * 60);//decimal a minutos
                                                 // horas minutos segundos
            TimeSpan _HoraAgregar = new TimeSpan(_horaH, _horaMin, 0);// cada cuanto le toca el medicamento
            //DateTime.Now.Add(_HoraAgregar);

            int _DiaEntero = (int)Math.Floor(_dias);//dias sin decimal
            float _diaDif = _dias - _DiaEntero;//sacar los decimales del dia
            int _diaHoras = (int)(_diaDif * 24);//decimal a horas del dia
            //todos los dias, ver cuantas horas en total val a ser
            int _HorasTotalesdias = (_DiaEntero * 24) + _diaHoras;
            //DateTime.Now.AddHours(_HorasTotalesdias);
            //numero de eventos para crear un for 
            int _numeroEventos = (int)Math.Floor(_HorasTotalesdias /_horas );


            List<CalendarEvent> _lisEvent = new List<CalendarEvent>();
            //los recordatorios van a ser iguales
            List<CalendarEventReminder> _lisRemain = new List<CalendarEventReminder>();
            _lisRemain.Add(new CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore = new TimeSpan(0, 10, 0)});
            _lisRemain.Add(new CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore = new TimeSpan(0, 5, 0)});
            _lisRemain.Add(new CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore = new TimeSpan(0, 2, 0)});

            for(int i=0; i<_numeroEventos; i++)
            {
                CalendarEvent _Evento = new CalendarEvent() { AllDay = false,
                            Description =_dosis+"\n"+ _extra, Reminders = _lisRemain };
                if (i == 0)
                {//se le da el ahora
                    _Evento.Start = DateTime.Now;
                }
                else
                {
                    _Evento.Start = _lisEvent[i - 1].Start.Add(_HoraAgregar);
                }
                _Evento.Name = "Tomar medicamento " + _nombre ;
                _Evento.End = _Evento.Start.AddHours(1);
                _lisEvent.Add(_Evento);
            }

            //los calendarios en el telefono,   el 0 es el calendario del sistema
            var TodosCalen = await CrossCalendars.Current.GetCalendarsAsync();
            
            for(int i=0; i<_lisEvent.Count; i++)
            {
                //agregarlo
                try
                {

                    await CrossCalendars.Current.AddOrUpdateEventAsync(TodosCalen[0], _lisEvent[i]);
                }
                catch (UnauthorizedAccessException ex)
                {
                    await DisplayAlert("Error calendario", ex.Message, "asa");
                }
            }
        }
        private async void Fn_ListaRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            list.ItemsSource = null;
            v_medicamentos = App.Fn_GetMedic(v_cita.v_idCita);
            if (v_medicamentos.Count > 0)
            {
                for (int i = 0; i < v_medicamentos.Count; i++)
                {
                    v_medicamentos[i].Fn_SetTexto();
                }
                ListaMed.ItemsSource = v_medicamentos;
            }
            else
            {
                nota.Text += "\nNo existe medicamentos registrados";
            }
            await Task.Delay(100);
            //cancelar la actualizacion
            list.IsRefreshing = false;
        }
    }
}