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
        ObservableCollection<Cita> v_citas = new ObservableCollection<Cita>();
        public V_Cita()
        {
            InitializeComponent();
          //  Fn_GetCitas();
            /*if (App.v_citas.Count > 0)
            {
                v_citas = App.v_citas;
            }
            else
            {
                v_citas.Add(new Cita() { v_nombreDR = "nombre 1", v_fecha = "2018-11-03", v_hora = new TimeSpan(12, 24, 00), v_estado = "0" });
                v_citas.Add(new Cita() { v_nombreDR = "nombre 2", v_fecha = "2019-01-22", v_hora = new TimeSpan(10, 04, 00), v_estado = "1" });
                v_citas.Add(new Cita() { v_nombreDR = "nombre 3", v_fecha = "2018-10-30", v_hora = new TimeSpan(16, 50, 00), v_estado = "2" });
                v_citas.Add(new Cita() { v_nombreDR = "nombre 4", v_fecha = "2018-12-16", v_hora = new TimeSpan(18, 29, 00), v_estado = "3" });
                App.Fn_GuardarCitas(v_citas);
            }
            Ordenar();
            ListaCita.ItemsSource = v_citas;
*/
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Fn_GetCitas();
        }
        private async void Fn_GetCitas()
        {
            HttpClient _client = new HttpClient();
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
                    v_citas=JsonConvert.DeserializeObject<ObservableCollection<  Cita>>(_respuesta);
                    Console.WriteLine("cuantos "+v_citas.Count+"json citaa " + _respuesta);
                    //await DisplayAlert("LLega get citas", _respuesta, "acep");
                    Ordenar();
                    App.Fn_GuardarCitas(v_citas);
                    ListaCita.ItemsSource = v_citas;
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", ex.ToString(), "Aceptar");
                if(App.v_citas.Count>0)
                {
                    v_citas = App.v_citas;
                }
                else
                {
                    v_citas.Add(new Cita() { v_nombreDR = "nombre 1", v_fecha = "2018-11-03", v_hora = new TimeSpan(12, 24, 00), v_estado = "0" });
                    v_citas.Add(new Cita() { v_nombreDR = "nombre 2", v_fecha = "2019-01-22", v_hora = new TimeSpan(10, 04, 00), v_estado = "1" });
                    v_citas.Add(new Cita() { v_nombreDR = "nombre 3", v_fecha = "2018-10-30", v_hora = new TimeSpan(16, 50, 00), v_estado = "2" });
                    v_citas.Add(new Cita() { v_nombreDR = "nombre 4", v_fecha = "2018-12-16", v_hora = new TimeSpan(18, 29, 00), v_estado = "3" });
                Ordenar();
                    App.Fn_GuardarCitas(v_citas);
                }
                Ordenar();
                ListaCita.ItemsSource = v_citas;
            }
        }
        public async void Fn_CitaTap(object sender, ItemTappedEventArgs _args)
        {
            Cita _citaSelec = _args.Item as Cita;
            await  Navigation.PushAsync(new V_NCita(_citaSelec) );
        }
        public void Ordenar()
        {
            for (int i = 0; i < v_citas.Count; i++)
            {
                v_citas[i].Fn_SetValores();
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
    }
}