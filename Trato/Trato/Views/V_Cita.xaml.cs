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
using Trato.Models;
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
            for (int i = 0; i < 6; i++)
            {
                v_estados.Add(new Filtro() { v_texto = ((EstadoCita)i).ToString().Replace('_', ' '), v_visible = false });
            }
            List_Fil.ItemsSource = v_estados;
            v_medi = _medic;
            //Fn_GetCitas();
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
            if(v_medi)
            {
                ToolbarItems.Clear();
            }
            Fn_Borrar(null, null);
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
                    if(v_medicamentos.Count<1)
                    {
                        L_Error.IsVisible = true;
                        L_Error.Text = "No se encuentran medicamentos";
                    }
                }
            }
            catch
            {
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
            string _DirEnviar = NombresAux.BASE_URL + "get_citas.php";
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                    v_citas = JsonConvert.DeserializeObject<ObservableCollection<Cita>>(_respuesta);
                    L_Error.IsVisible = false;
                    Ordenar();
                    App.Fn_GuardarCitas(v_citas);
                    ListaCita.ItemsSource = v_citas;
                    if (v_citas.Count < 1 && !v_medi)
                    {
                        L_Error.IsVisible = true;
                        L_Error.Text = "No tiene citas";
                    }
                    else if (v_citas.Count < 1 && v_medi)
                    {
                        L_Error.IsVisible = true;
                        L_Error.Text = "No tiene Medicamentos";
                    }
                    else if(v_citas.Count>0 && v_medi)
                    {
                        Fn_GetMedic();
                        Fn_GetTerminada();
                        for (int i = 0; i < v_citas.Count; i++)
                        {
                            v_citas[i].Fn_SetVisible();
                        }
                    }
                }
            }
            catch(Exception _ex)
            {
                if (App.v_citas.Count > 0)
                {
                    v_citas = App.v_citas;
                    Ordenar();
                    ListaCita.ItemsSource = v_citas;
                    L_Error.IsVisible = false;
                    if (v_medi)
                    {
                        Fn_GetMedic();
                        Fn_GetTerminada();
                        for (int i = 0; i < v_citas.Count; i++)
                        {
                            v_citas[i].Fn_SetVisible();
                        }
                    }
                }
                else
                {
                    L_Error.IsVisible = true;
                    L_Error.Text = "No tiene citas";
                    if (v_medi)
                    {
                        L_Error.Text = "No tiene Medicamentos";
                    }
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
            var _temp = v_citas.OrderByDescending(x => x.v_fechaDate);
            v_citas = new ObservableCollection<Cita>(_temp);
            for (int i = 0; i < v_citas.Count; i++)
            {
                v_citas[i].Fn_CAmbioCol(i);
            }
        }
        private async void ListaCita_Refreshing(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Fn_Borrar(null, null);
            Fn_GetCitas();
            await Task.Delay(100); //cancelar la actualizacion
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
        #region FILTROOO
        /// <summary>
        /// Lista que el usuario elige cada  vez que le pica
        /// </summary>
        List<string> _filTexto = new List<string>();
        /// <summary>
        /// lista de inidices que eliges
        /// </summary>
        List<int> v_indiceTap = new List<int>();
        /// <summary>
        /// textos que se agregan a la lista visible
        /// </summary>
        ObservableCollection<Filtro> v_estados = new ObservableCollection<Filtro>();
        bool v_visible = false;
        private void Fn_ToolFil(object sender, EventArgs e)
        {
            v_visible = !v_visible;
            stackOver.IsVisible = v_visible;
            if (!v_visible)
            {
                Fn_Borrar(null, null);
            }
        }
        private async void Fn_Buscar(object sender, EventArgs e)
        {
            ObservableCollection<Cita> _tempCita = new ObservableCollection<Cita>();
            for (int i = 0; i < v_citas.Count; i++)
            {
                for (int j = 0; j < v_indiceTap.Count; j++)
                {
                    if ((int.Parse(v_citas[i].v_estado) == v_indiceTap[j]) && !_tempCita.Contains(v_citas[i]))
                    {
                        _tempCita.Add(v_citas[i]);
                    }
                }
            }
            v_visible = false;
            stackOver.IsVisible = v_visible;
            if (_tempCita.Count < 1)
            {
                await DisplayAlert("Aviso", "Filtro no encuentra coincidencias", "Aceptar");
                ListaCita.ItemsSource = v_citas;
            }
            else
            {
                ListaCita.ItemsSource = _tempCita;
            }
        }
        private void Fn_Borrar(object sender, EventArgs e)
        {
            List_Fil.ItemsSource = null;
            for (int i = 0; i < v_estados.Count; i++)//prende o apaga la imagen
            {
                v_estados[i].v_visible = false;
            }
            List_Fil.ItemsSource = v_estados;
            _filTexto.Clear();
            v_indiceTap.Clear();
            v_visible = false;
            stackOver.IsVisible = v_visible;
            ListaCita.ItemsSource = v_citas;
        }
        void Fn_TapFiltro(object sender, ItemTappedEventArgs _Args)
        {
            var list = (ListView)sender;
            list.ItemsSource = null;
            var _valor = _Args.Item as Filtro;
            int _valIndice = -1;
            for (int i = 0; i < v_estados.Count; i++)//prende o apaga la imagen
            {
                if (v_estados[i].v_texto == _valor.v_texto)
                {
                    v_estados[i].v_visible = !v_estados[i].v_visible;
                    _valIndice = i;
                }
            }
            if (!_filTexto.Contains(_valor.v_texto))
            {
                _filTexto.Add(_valor.v_texto);
                v_indiceTap.Add(_valIndice);
            }
            else
            {
                _filTexto.Remove(_valor.v_texto);
                v_indiceTap.Remove(_valIndice);
            }
            list.ItemsSource = v_estados;
        }
        #endregion
    }
}