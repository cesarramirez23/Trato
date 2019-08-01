using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Trato.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Trato.Varios;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Medicamento : ContentPage
	{
        ObservableCollection<C_NotaMed> v_medicamentos = new ObservableCollection<C_NotaMed>();
        private ObservableCollection<Cita> v_citas;
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
        public V_Medicamento ()
		{
			InitializeComponent ();
            Fn_GetCitas();
		}
        private async Task Fn_GetMedic()
        {
            HttpClient _client = new HttpClient();
            Cita _cita = new Cita(App.v_membresia, App.v_folio, "0");
            string _json = JsonConvert.SerializeObject(_cita);
            string _DirEnviar = NombresAux.BASE_URL + "get_medicamentos.php";
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                    v_medicamentos = JsonConvert.DeserializeObject<ObservableCollection<C_NotaMed>>(_respuesta);
                    App.Fn_GuardarMedicamentos(v_medicamentos);
                    if (v_medicamentos.Count < 1)
                    {
                        L_Error.IsVisible = true;
                        L_Error.Text = "No se encuentran medicamentos";
                    }
                }
            }
            catch(Exception _ex)
            {
                Fn_GetMedic();
            }
        }
        public ObservableCollection<Cita> Ordenar(ObservableCollection<Cita> _args)
        {
            for (int i = 0; i < _args.Count; i++)
            {
                _args[i].Fn_SetValores();
                _args[i].v_visible = true;
            }
            var _temp = _args.OrderByDescending(x => x.v_fechaDate);
            _args = new ObservableCollection<Cita>(_temp);
            for (int i = 0; i < _args.Count; i++)
            {
                _args[i].Fn_CAmbioCol(i);
            }
            return _args;
        }
        private async Task Fn_GetCitas()
        {
            HttpClient _client = new HttpClient();
            L_Error.IsVisible = true;
            L_Error.Text = "Procesando Informacion";
            Cita _cita = new Cita(App.v_membresia, App.v_folio, "0");
            string _json = JsonConvert.SerializeObject(_cita);
            string _DirEnviar = NombresAux.BASE_URL + "get_citas.php";
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                    v_citas = JsonConvert.DeserializeObject<ObservableCollection<Cita>>(_respuesta);
                    v_citas = Ordenar(v_citas);
                    App.Fn_GuardarCitas(v_citas);
                    if (v_citas.Count ==0 )
                    {
                        L_Error.IsVisible = true;
                        L_Error.Text = "No tiene Medicamentos";
                    }
                    else 
                    {
                        await Fn_GetMedic();
                        Fn_GetTerminada();
                        L_Error.IsVisible = false;
                        //v_citas=Ordenar(v_citas);
                        //for (int i = 0; i < v_citas.Count; i++)
                        //{
                        //    v_citas[i].Fn_SetVisible();
                        //}
                    }
                }
            }
            catch (Exception _ex)
            {
                Fn_GetCitas();
            }
        }
        private void Fn_GetTerminada()
        {
            List_Fil.ItemsSource = null;
            ObservableCollection<Cita> _Temp = new ObservableCollection<Cita>();
            for (int i = 0; i < App.v_citas.Count; i++)
            {
                if (App.v_citas[i].v_estado == "0")
                {
                    _Temp.Add(App.v_citas[i]);
                }
            }
            _Temp = Ordenar(_Temp);
            L_Error.IsVisible = false;
            List_Fil.ItemsSource = _Temp;
        }

        private async void List_Fil_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Cita _citaSelec = e.Item as Cita;
            if (_citaSelec != null)
            {
                await Navigation.PushAsync(new V_Nota(_citaSelec) );
            }
        }
    }
}