using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Trato.Varios;
using Newtonsoft.Json;
using System.Net.Http;



namespace Trato.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Cita : ContentPage
    {
        /// <summary>
        /// el bool
        /// </summary>
        bool v_medi = false;
        Cita v_CitaNotif = new Cita();
        /// <summary>
        /// es la nota medica
        /// </summary>
        ObservableCollection<C_NotaMed> v_medicamentos = new ObservableCollection<C_NotaMed>();
        ObservableCollection<Cita> v_citas = new ObservableCollection<Cita>();
        public V_Cita(bool _medic, bool _tiene, Cita _nuevaCita)
        {
            InitializeComponent();
            v_medi = _medic;
            Fn_GetCitas();
            if (_tiene)
            {
                v_CitaNotif = _nuevaCita;
                Fn_Notif(_nuevaCita);
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (v_CitaNotif.v_estado == "-1")
            {
                Fn_GetCitas();
            }
        }
        protected override void OnDisappearing()
        {
            if (v_CitaNotif.v_estado != "-1")
            {
                v_CitaNotif = new Cita();
            }
            base.OnDisappearing();
        }
        public async void Fn_Notif(Cita _nuevacita)
        {
            await Navigation.PushAsync(new V_NCita(_nuevacita, false));
        }
        private async void Fn_GetMedic()
        {
            HttpClient _client = new HttpClient();
            Cita _cita = new Cita(App.v_membresia, App.v_folio, "0");
            string _json = JsonConvert.SerializeObject(_cita);
            string _DirEnviar = "http://tratoespecial.com/get_medicamentos.php";
           // await DisplayAlert("ENVIA PARA medicamentos", _json, "acep");
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                   // await DisplayAlert("LLega get medicamentos", _respuesta, "acep");
                    v_medicamentos = JsonConvert.DeserializeObject<ObservableCollection<C_NotaMed>>(_respuesta);
                    App.Fn_GuardarMedicamentos(v_medicamentos);
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "Aceptar");
                if (App.v_NotasMedic.Count > 0)
                {
                    v_medicamentos = App.v_NotasMedic;
                }
            }
        }
        private async void Fn_GetCitas()
        {
            HttpClient _client = new HttpClient();
            L_Error.IsVisible = true;
            L_Error.Text = "Procesando Informacion";
            Cita _cita = new Cita(App.v_membresia, App.v_folio, "0");
            string _json = JsonConvert.SerializeObject(_cita);
            string _DirEnviar = "http://tratoespecial.com/get_citas.php";
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                   // await DisplayAlert("LLega get citas", _respuesta, "acep");
                    v_citas = JsonConvert.DeserializeObject<ObservableCollection<Cita>>(_respuesta);
                    L_Error.IsVisible = false;
                    //Console.WriteLine("cuantos "+v_citas.Count+"json citaa " + _respuesta);
                    Ordenar();
                 
                    App.Fn_GuardarCitas(v_citas);
                    ListaCita.ItemsSource = v_citas;
                    if (v_medi)
                    {
                        Fn_GetMedic();
                        Fn_GetTerminada();
                        for (int i = 0; i < v_citas.Count; i++)
                        {
                            v_citas[i].Fn_SetVisible();
                        }
                    }
                    if (v_citas.Count < 1)
                    {
                        L_Error.IsVisible = true;
                        L_Error.Text = "No tiene citas";
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "Aceptar");
                if (App.v_citas.Count > 0)
                {
                    v_citas = App.v_citas;
                    L_Error.IsVisible = false;
                }
                else
                {
                    L_Error.IsVisible = true;
                    L_Error.Text = "No tiene citas";
                }
                Ordenar();
                ListaCita.ItemsSource = v_citas;
                if (v_medi)
                {
                    for (int i = 0; i <v_citas.Count; i++)
                    {
                        v_citas[i].Fn_SetVisible();
                    }
                    Fn_GetTerminada();
                }
            }
        }
        public async void Fn_CitaTap(object sender, ItemTappedEventArgs _args)
        {
            Cita _citaSelec = _args.Item as Cita;
            if (v_medi)
            {
                if (_citaSelec.v_estado == "0")
                {
                    await Navigation.PushAsync(new V_Medicamento(_citaSelec));
                }
                else
                {
                    await DisplayAlert("Error", "Esta cita no ha sido terminada por el medico", "Aceptar");
                }
            }
            else
            {
                await Navigation.PushAsync(new V_NCita(_citaSelec));
            }
        }
        public void Ordenar()
        {
            for (int i = 0; i < v_citas.Count; i++)
            {
                v_citas[i].Fn_SetValores();
                v_citas[i].v_visible = true;
            }
            var _temp = v_citas.OrderBy(x => x.v_fechaDate);
            v_citas = new ObservableCollection<Cita>(_temp);

            for (int i = 0; i < v_citas.Count; i++)
            {
                v_citas[i].Fn_CAmbioCol(i);
            }
        }
        private async void ListaCita_Refreshing(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Fn_GetCitas();
            await Task.Delay(100);
            //cancelar la actualizacion
            list.IsRefreshing = false;
        }
        private  void Fn_GetTerminada()
        {
            ListaCita.ItemsSource = null;
            ObservableCollection<Cita> _Temp = new ObservableCollection<Cita>();
            for (int i=0; i<App.v_citas.Count;i++)
            {
                if(App.v_citas[i].v_estado=="0")
                {
                    _Temp.Add(App.v_citas[i]);
                }
            }
            ListaCita.ItemsSource = _Temp;
        }
    }
}