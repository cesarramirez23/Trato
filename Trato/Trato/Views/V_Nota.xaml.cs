using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//para agregar loos eventos al calendario
using Plugin.Calendars;
using Plugin.Calendars.Abstractions;
using Trato.Models;
//para agregar loos eventos al calendario
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Trato.Personas;
using System.Collections.ObjectModel;
using Trato.Varios;
using System.Threading.Tasks;
namespace Trato.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Nota : ContentPage
    {
        Cita v_cita;
        ObservableCollection<Medicamentos> v_medicamentos = new ObservableCollection<Medicamentos>();
        public V_Nota(Cita _cita)
        {
            InitializeComponent();
            v_cita = _cita;
            Fn_GetInfoDr();
        }
        async void Fn_GetInfoDr()
        {
            HttpClient _client = new HttpClient();
            C_Login _login = new C_Login(v_cita.v_doctorId, "");
            string _jsonLog = JsonConvert.SerializeObject(_login, Formatting.Indented);
            string _url = NombresAux.BASE_URL + "query_r_one_service.php";
            StringContent _content = new StringContent(_jsonLog, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage _respuestphp = await _client.PostAsync(_url, _content);
                string _result = await _respuestphp.Content.ReadAsStringAsync();
                C_Medico _nuePer = JsonConvert.DeserializeObject<C_Medico>(_result);
                nombre.Text = v_cita.v_completo+"\n";
                especialidad.Text = v_cita.v_Especialidad;
                descripcion.Text = Fn_GEtDire(v_cita.v_doctorId) + "\n" + _nuePer.v_Tel + "\n";
                if (!string.IsNullOrEmpty(_nuePer.v_horario))
                {
                    string[] _split = _nuePer.v_horario.Split('/');
                    descripcion.Text  += "De " + _split[0] + " a " + _split[1];
                }
                fecha.Text = v_cita.v_fechaDate.ToString("dddd, dd MMMM yyyy hh:mm tt");
                await Fn_CreaLista();
            }
            catch (Exception ex)
            {
                Fn_GetInfoDr();
            }
        }
        string Fn_GEtDire(string _id)
        {
            string _ret = string.Empty;
            for(int i=0; i< App.v_medicos.Count; i++)
            {
                if (App.v_medicos[i].v_membre == _id)
                {
                    _ret = App.v_medicos[i].v_Domicilio;
                }
            }
            return _ret;
        }
        private async Task Fn_CreaLista()
        {
            StackLista.Children.Clear();
            v_medicamentos = App.Fn_GetMedic(v_cita.v_idCita);
            nombre.Text = v_cita.v_nombreDR;
            especialidad.Text = v_cita.v_Especialidad;
            if (v_medicamentos.Count > 0)
            {
                Color _azulfondo = (Color)App.Current.Resources["AzulFondo"];
                Color _azulprin = (Color)App.Current.Resources["AzulPrincipal"];
                Color _grisbase = (Color)App.Current.Resources["GrisBase"];
                Style _stylebut = (Style)App.Current.Resources["Buton"];
                for (int i = 0; i < v_medicamentos.Count; i++)
                {
                    StackLayout _stack = new StackLayout() { BackgroundColor = Color.White, Padding = new Thickness(0) };
                    if(i%2==1)
                    {
                        _stack.BackgroundColor = _azulfondo;
                    }
                    _stack.BindingContext = v_medicamentos[i];
                    v_medicamentos[i].Fn_SetTexto();
                    StackLayout _hori = new StackLayout() { Orientation = StackOrientation.Horizontal, Padding = new Thickness(0)};
                    Image _img = new Image() { Source = "Circulo.png",HorizontalOptions= LayoutOptions.Start,
                        HeightRequest =15, Aspect= Aspect.AspectFit, WidthRequest=15 };
                    Label _nombre = new Label() { Text = v_medicamentos[i].v_nombre, TextColor= _azulprin };
                    _hori.Children.Add(_img);
                    _hori.Children.Add(_nombre);
                    _stack.Children.Add(_hori);
                    Label _dosis = new Label() { Text = "Dosis: " + v_medicamentos[i].v_dosis, Margin = new Thickness(15,0,0,0) };
                    Label _dias = new Label() { Text = "Días: "+v_medicamentos[i].v_periodo, Margin = new Thickness(15, 0, 0, 0) };
                    Label _horas = new Label() { Text = "Horas: "+ v_medicamentos[i].v_tiempo, Margin = new Thickness(15, 0, 0, 0) };
                    Label _info = new Label() { Text = "Información Extra: "+ v_medicamentos[i].v_extra, Margin = new Thickness(15, 0, 0, 0) };
                    Button _btn = new Button()
                    {
                        Style = _stylebut,
                        Text = v_medicamentos[i].v_texto,
                        FontSize= Device.GetNamedSize(NamedSize.Small,typeof(Label)),
                        Margin= new Thickness(0,0,0,5)
                    };
                    if(v_medicamentos[i].v_estado!= "1"  && v_medicamentos[i].v_estado!="0")
                    {
                        _btn.IsEnabled = false;
                    }
                    _btn.Clicked += Fn_SetEstado;
                    ContentView _linea = new ContentView() {
                        HeightRequest = 2,
                        BackgroundColor = _azulprin,
                        Margin =new Thickness(0),
                        Padding = new Thickness(0)
                     };
                    _stack.Children.Add(_dosis);
                    _stack.Children.Add(_dias);
                    _stack.Children.Add(_horas);
                    _stack.Children.Add(_info);
                    _stack.Children.Add(_btn);
                    _stack.Children.Add(_linea);
                    StackLista.Children.Add(_stack);
                }
            }
            else
            {
                Label _men = new Label() {
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    TextColor = (Color)App.Current.Resources["GrisBold"],
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Text= "No existe medicamentos registrados"
                };
                StackLista.BackgroundColor = Color.White;
                StackLista.Children.Add(_men);
            }
            await Task.Delay(100);
        }
        public async void Fn_SetEstado(object sender, EventArgs _args)
        {
            Button button = sender as Button;
            Medicamentos _medi = button.BindingContext as Medicamentos;
            if (_medi.v_estado == "0")
            {
                bool _res = await DisplayAlert("Iniciar Tratamiento", "Seguro de iniciar", "Continuar", "Cancelar");
                if (_res)
                {
                    Fn_CalendarioMedi(_medi.v_nombre, _medi.v_extra, _medi.v_dosis, _medi.v_periodo, _medi.v_tiempo);

                    HttpClient _client = new HttpClient();
                    string json = @"{";
                    json += "id:'" + _medi.v_idMedi + "',\n";
                    json += "estado:'" + "1" + "'\n";
                    json += "}";
                    JObject jsonper = JObject.Parse(json);
                    string _DirEnviar = NombresAux.BASE_URL + "update_medicamentos.php";
                    StringContent _content = new StringContent(jsonper.ToString(), Encoding.UTF8, "application/json");
                    // await DisplayAlert("Exito", jsonper.ToString(), "Aceptar");
                    try
                    {
                        HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                        if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                            if (_respuesta == "1")
                            {
                                await DisplayAlert("Exito", "Cambios generados correctamente", "Aceptar");
                                Fn_Cambio(_medi, "1");
                            }
                            else
                            {
                                await DisplayAlert("Falla", _respuesta, "Aceptar");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", "Error de Conexión", "Aceptar");
                    }
                }
            }
            else if (_medi.v_estado == "1")//terminar tratamiento
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
                    string _DirEnviar = NombresAux.BASE_URL + "update_medicamentos.php";
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
                                await DisplayAlert("Exito", "Cambios generados correctamente", "Aceptar");
                                Fn_Cambio(_medi, "2");
                            }
                            else
                            {
                                await DisplayAlert("Falla", _respuesta, "Aceptar");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", "Error de Conexión", "Aceptar");
                    }
                }
            }
            else
            {
                await DisplayAlert("Tratamiento terminado", "", "Aceptar");
                button.IsEnabled = false;
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
            await Fn_CreaLista();
        }
        /// <summary>
        /// agrega los medicamentos al calendario
        /// </summary>
        public async void Fn_CalendarioMedi(string _nombre, string _extra, string _dosis, float _dias, float _horas)
        {
            int _horaH = (int)Math.Floor(_horas);//horas sin decimal
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
            int _numeroEventos = (int)Math.Floor(_HorasTotalesdias / _horas);


            List<CalendarEvent> _lisEvent = new List<CalendarEvent>();
            //los recordatorios van a ser iguales
            List<CalendarEventReminder> _lisRemain = new List<CalendarEventReminder>();
            _lisRemain.Add(new CalendarEventReminder() { Method = CalendarReminderMethod.Alert, TimeBefore = new TimeSpan(0, 2, 0) });

            for (int i = 0; i < _numeroEventos; i++)
            {
                CalendarEvent _Evento = new CalendarEvent()
                {
                    AllDay = false,
                    Description = _dosis + "\n" + _extra,
                    Reminders = _lisRemain
                };
                if (i == 0)
                {//se le da el ahora
                    _Evento.Start = DateTime.Now;
                }
                else
                {
                    _Evento.Start = _lisEvent[i - 1].Start.Add(_HoraAgregar);
                }
                _Evento.Name = "Tomar medicamento " + _nombre;
                _Evento.End = _Evento.Start.AddHours(1);
                _lisEvent.Add(_Evento);
            }

            //los calendarios en el telefono,   el 0 es el calendario del sistema
            var TodosCalen = await CrossCalendars.Current.GetCalendarsAsync();
            if (TodosCalen.Count > 1)
            {
                for (int i = 0; i < _lisEvent.Count; i++)
                {
                    //agregarlo
                    try
                    {

                        await CrossCalendars.Current.AddOrUpdateEventAsync(TodosCalen[0], _lisEvent[i]);
                    }
                    catch
                    {
                        await DisplayAlert("Error", "No es posible agregar el evento", "Aceptar");
                    }
                }
            }
            else
            {
                await DisplayAlert("Error calendario", "No se encontraron calendarios", "asa");
            }
        }
    }
}