using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Personas;
using System.Collections.ObjectModel;//listas observa
using System.Net.Http;
using Newtonsoft.Json;
using Trato.Varios;
using Trato.Models;
namespace Trato.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Buscador : ContentPage
    {
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
        bool v_descarga = true;
        /// <summary>
        /// la pantalla con los filtro esta visible
        /// </summary>
        bool v_filtro = false; 
        /// <summary>
        /// textos que se agregan a la lista visible
        /// </summary>
        ObservableCollection<Filtro> _especialidades = new ObservableCollection<Filtro>();
        /// <summary>
        /// textos que se agregan a la lista visible
        /// </summary>
        ObservableCollection<Filtro> _ciudades = new ObservableCollection<Filtro>();
        /// <summary>
        /// textos que se agregan a la lista visible
        /// </summary>
        ObservableCollection<Filtro> _estados = new ObservableCollection<Filtro>();
        /// <summary>
        /// 0 MEDICOS,   1 SERVICIOS MEDICOS,    2 SERVICIOS GENERALES
        /// </summary>
        int v_tipo = -1;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(v_filtro)
            {
                Fn_GEtFiltro();
            }
        }
        /// <summary>
        /// con el nuevo metodo delk filtro en otro lado
        /// </summary>
        private async void Fn_GEtFiltro()
        {
            List<string>[] _filtro =  App.Fn_Getfiltro();
            if (v_tipo == 0)
            {
                ObservableCollection<C_Medico> _filtrada = new ObservableCollection<C_Medico>();
                for (int _idmed = 0; _idmed < App.v_medicos.Count; _idmed++)
                {
                    int _cont = 0;
                    //especialidades
                    if (_filtro[0].Count > 0)
                    {
                        for (int _idfil = 0; _idfil < _filtro[0].Count; _idfil++)
                        {
                            if (App.v_medicos[_idmed].v_Especialidad.Contains(_filtro[0][_idfil]))
                            {
                                _cont++;
                            }
                        }
                    }
                    else
                    {
                        _cont++;
                    }
                    if (_filtro[1].Count > 0)
                    {
                        for (int _idfil = 0; _idfil < _filtro[1].Count; _idfil++)
                        {
                            if ((App.v_medicos[_idmed].v_estado == _filtro[1][_idfil]))
                            {
                                _cont++;
                            }
                        }
                    }
                    else
                    {
                        _cont++;
                    }
                    if (_filtro[2].Count > 0)
                    {
                        for (int _idfil = 0; _idfil < _filtro[2].Count; _idfil++)
                        {
                            if ((App.v_medicos[_idmed].v_Ciudad == _filtro[2][_idfil]))
                            {
                                _cont++;
                            }
                        }
                    }
                    else
                    {
                        _cont++;
                    }
                    if (_cont == 3 && !_filtrada.Contains(App.v_medicos[_idmed]))
                    {
                        _filtrada.Add(App.v_medicos[_idmed]);
                    }
                }
                IEnumerable<C_Medico> _temp = _filtrada.OrderBy(x => x.v_Apellido);
                _filtrada = new ObservableCollection<C_Medico>(_temp);
                if (_filtrada.Count > 0)
                {
                    v_lista.ItemsSource = _filtrada;
                }
                else
                {
                    await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
                    v_lista.ItemsSource = App.v_medicos;
                }
            }
            else if (v_tipo == 1)
            {
                ObservableCollection<C_Servicios> _filtrada = new ObservableCollection<C_Servicios>();
                for (int _idmed = 0; _idmed < App.v_servicios.Count; _idmed++)
                {
                    int _cont = 0;
                    //especialidades
                    if (_filtro[0].Count > 0)
                    {
                        for (int _idfil = 0; _idfil < _filtro[0].Count; _idfil++)
                        {
                            if (App.v_servicios[_idmed].v_Especialidad== _filtro[0][_idfil])
                            {
                                _cont++;
                            }
                        }
                    }
                    else
                    {
                        _cont++;
                    }
                    if (_filtro[1].Count > 0)
                    {
                        for (int _idfil = 0; _idfil < _filtro[1].Count; _idfil++)
                        {
                            if ((App.v_servicios[_idmed].v_estado == _filtro[1][_idfil]))
                            {
                                _cont++;
                            }
                        }
                    }
                    else
                    {
                        _cont++;
                    }
                    if (_filtro[2].Count > 0)
                    {
                        for (int _idfil = 0; _idfil < _filtro[2].Count; _idfil++)
                        {
                            if ((App.v_servicios[_idmed].v_Ciudad == _filtro[2][_idfil]))
                            {
                                _cont++;
                            }
                        }
                    }
                    else
                    {
                        _cont++;
                    }
                    if (_cont == 3 && !_filtrada.Contains(App.v_servicios[_idmed]))
                    {
                        _filtrada.Add(App.v_servicios[_idmed]);
                    }
                }
                IEnumerable<C_Servicios> _temp = _filtrada.OrderBy(x => x.v_completo);
                _filtrada = new ObservableCollection<C_Servicios>(_temp);
                if (_filtrada.Count > 0)
                {
                    v_lista.ItemsSource = _filtrada;
                }
                else
                {
                    await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
                    v_lista.ItemsSource = App.v_servicios;
                }
            }
            else if (v_tipo == 2)
            {
                ObservableCollection<C_ServGenerales> _filtrada = new ObservableCollection<C_ServGenerales>();
                for (int _idmed = 0; _idmed < App.v_generales.Count; _idmed++)
                {
                    int _cont = 0;
                    //especialidades
                    if (_filtro[0].Count > 0)
                    {
                        for (int _idfil = 0; _idfil < _filtro[0].Count; _idfil++)
                        {
                            if (App.v_generales[_idmed].v_Especialidad==_filtro[0][_idfil])
                            {
                                _cont++;
                            }
                        }
                    }
                    else
                    {
                        _cont++;
                    }
                    if (_filtro[1].Count > 0)
                    {
                        for (int _idfil = 0; _idfil < _filtro[1].Count; _idfil++)
                        {
                            if ((App.v_generales[_idmed].v_estado == _filtro[1][_idfil]))
                            {
                                _cont++;
                            }
                        }
                    }
                    else
                    {
                        _cont++;
                    }
                    if (_filtro[2].Count > 0)
                    {
                        for (int _idfil = 0; _idfil < _filtro[2].Count; _idfil++)
                        {
                            if ((App.v_generales[_idmed].v_Ciudad == _filtro[2][_idfil]))
                            {
                                _cont++;
                            }
                        }
                    }
                    else
                    {
                        _cont++;
                    }
                    if (_cont == 3 && !_filtrada.Contains(App.v_generales[_idmed]))
                    {
                        _filtrada.Add(App.v_generales[_idmed]);
                    }
                }
                IEnumerable<C_ServGenerales> _temp = _filtrada.OrderBy(x => x.v_completo);
                _filtrada = new ObservableCollection<C_ServGenerales>(_temp);
                if (_filtrada.Count > 0)
                {
                    v_lista.ItemsSource = _filtrada;
                }
                else
                {
                    await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
                    v_lista.ItemsSource = App.v_generales;
                }
            }
        }
        public V_Buscador(int _valor)
        {
            InitializeComponent();
            //StackOver.IsVisible = v_filtro;
            /*
            if (Device.RuntimePlatform == Device.Android)//para que solo en ios se vea la barra de arriba y el buscador completo
            {
                ToolbarItems.Clear();
                Grid_Busqueda.BackgroundColor = Color.Transparent;
            }//en prueba
            else if(Device.RuntimePlatform== Device.iOS)
            {
                Grid.SetColumnSpan(v_Search, 4);
                B_Filtrar.IsVisible = false;
                v_Search.BackgroundColor = Color.Transparent;                
                Grid_Busqueda.BackgroundColor = Color.Transparent;
            }
            */
            v_tipo = _valor;
            App.Fn_GuardaFiltro(new List<string>[]
            {
                new List<string>(),
                new List<string>(),
                new List<string>()
            });
            Buscador.IsVisible = false;
            v_descarga = true;
            Fn_CargarDAtos();
        }
        public void Fn_CrearCiud()
        {
            if (v_tipo==0)
            {
                _ciudades.Clear();
                List<string> _tempCont = new List<string>();
                for (int i=0;i<App.v_medicos.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_medicos[i].v_Ciudad))
                    {
                        _tempCont.Add(App.v_medicos[i].v_Ciudad);
                        _ciudades.Add(new Filtro(App.v_medicos[i].v_Ciudad, true));
                        //if (_filCiud.Contains(App.v_medicos[i].v_Ciudad))
                        //{
                        //    _ciudades.Add(new Filtro(App.v_medicos[i].v_Ciudad, true));
                        //}
                        //else
                        //{
                        //    _ciudades.Add(new Filtro(App.v_medicos[i].v_Ciudad, false));
                        //}
                    }
                }
            }
            else if(v_tipo==1)
            {
                _ciudades.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_servicios.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_servicios[i].v_Ciudad))
                    {
                        _tempCont.Add(App.v_servicios[i].v_Ciudad);
                        _ciudades.Add(new Filtro(App.v_servicios[i].v_Ciudad, true));
                        //if (_filCiud.Contains(App.v_servicios[i].v_Ciudad))
                        //{
                        //    _ciudades.Add(new Filtro(App.v_servicios[i].v_Ciudad, true));
                        //}
                        //else
                        //{
                        //    _ciudades.Add(new Filtro(App.v_servicios[i].v_Ciudad, false));
                        //}
                    }
                }
            }
            else if(v_tipo==2)
            {
                _ciudades.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_generales.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_generales[i].v_Ciudad))
                    {
                        _tempCont.Add(App.v_generales[i].v_Ciudad);
                        _ciudades.Add(new Filtro(App.v_generales[i].v_Ciudad, true));
                        //if (_filCiud.Contains(App.v_generales[i].v_Ciudad))
                        //{
                        //    _ciudades.Add(new Filtro(App.v_generales[i].v_Ciudad, true));
                        //}
                        //else
                        //{
                        //    _ciudades.Add(new Filtro(App.v_generales[i].v_Ciudad, false));
                        //}
                    }
                }
            }
            IEnumerable<Filtro> _temp = _ciudades.OrderBy(x => x.v_texto);
            _ciudades = new ObservableCollection<Filtro>(_temp);
            Fn_CrearEst();
        }
        public void Fn_CrearEspec()
        {
            if(v_tipo==0)
            {
                _especialidades.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_medicos.Count; i++)//recorre todos los medicos
                {
                    for (int j = 0; j < App.v_medicos[i].v_ListaEsp.Count; j++)
                    {
                        if ((!string.IsNullOrEmpty(App.v_medicos[i].v_ListaEsp[j].v_nombreEspec) && !string.IsNullOrWhiteSpace(App.v_medicos[i].v_ListaEsp[j].v_nombreEspec))
                            && (!_tempCont.Contains(App.v_medicos[i].v_ListaEsp[j].v_nombreEspec)))
                        {
                            _tempCont.Add(App.v_medicos[i].v_ListaEsp[j].v_nombreEspec);
                            _especialidades.Add(new Filtro(App.v_medicos[i].v_ListaEsp[j].v_nombreEspec, true));
                            //if (_filEspec.Contains(App.v_medicos[i].v_ListaEsp[j].v_nombreEspec))
                            //{
                            //    _especialidades.Add(new Filtro(App.v_medicos[i].v_ListaEsp[j].v_nombreEspec, true));
                            //}
                            //else
                            //{
                            //    _especialidades.Add(new Filtro(App.v_medicos[i].v_ListaEsp[j].v_nombreEspec, false));
                            //}
                        }
                    }
                }
            }
            else if(v_tipo==1)
            {
                _especialidades.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_servicios.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_servicios[i].v_Especialidad))
                    {
                        _tempCont.Add(App.v_servicios[i].v_Especialidad);
                        _especialidades.Add(new Filtro(App.v_servicios[i].v_Especialidad, true));
                        //if (_filEspec.Contains(App.v_servicios[i].v_Especialidad))
                        //{
                        //    _especialidades.Add(new Filtro(App.v_servicios[i].v_Especialidad, true));
                        //}
                        //else
                        //{
                        //    _especialidades.Add(new Filtro(App.v_servicios[i].v_Especialidad, false));
                        //}
                    }
                }
            }
            else if(v_tipo==2)
            {
                _especialidades.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_generales.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_generales[i].v_Especialidad))
                    {
                        _tempCont.Add(App.v_generales[i].v_Especialidad);
                        _especialidades.Add(new Filtro(App.v_generales[i].v_Especialidad, true));
                        //if (_filEspec.Contains(App.v_generales[i].v_Especialidad))
                        //{
                        //    _especialidades.Add(new Filtro(App.v_generales[i].v_Especialidad, true));
                        //}
                        //else
                        //{
                        //    _especialidades.Add(new Filtro(App.v_generales[i].v_Especialidad, false));
                        //}
                    }
                }
            }
            IEnumerable<Filtro> _temp = _especialidades.OrderBy(x => x.v_texto);
            _especialidades = new ObservableCollection<Filtro>(_temp);
            Fn_CrearCiud();
        }
        public void Fn_CrearEst()
        {
            if (v_tipo == 0)
            {
                _estados.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_medicos.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_medicos[i].v_estado))
                    {
                        _tempCont.Add(App.v_medicos[i].v_estado);
                        _estados.Add(new Filtro(App.v_medicos[i].v_estado, true));
                        //if (_filCiud.Contains(App.v_medicos[i].v_estado))
                        //{
                        //    _estados.Add(new Filtro(App.v_medicos[i].v_estado, true));
                        //}
                        //else
                        //{
                        //    _estados.Add(new Filtro(App.v_medicos[i].v_estado, false));
                        //}
                    }
                }
            }
            else if (v_tipo == 1)
            {
                _estados.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_servicios.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_servicios[i].v_estado))
                    {
                        _tempCont.Add(App.v_servicios[i].v_estado);
                        _estados.Add(new Filtro(App.v_servicios[i].v_estado, true));
                        //if (_filCiud.Contains(App.v_servicios[i].v_estado))
                        //{
                        //    _estados.Add(new Filtro(App.v_servicios[i].v_estado, true));
                        //}
                        //else
                        //{
                        //    _estados.Add(new Filtro(App.v_servicios[i].v_estado, false));
                        //}
                    }
                }
            }
            else if (v_tipo == 2)
            {
                _estados.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_generales.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_generales[i].v_estado))
                    {
                        _tempCont.Add(App.v_generales[i].v_estado);
                        _estados.Add(new Filtro(App.v_generales[i].v_estado, true));
                        //if (_filCiud.Contains(App.v_generales[i].v_estado))
                        //{
                        //    _estados.Add(new Filtro(App.v_generales[i].v_estado, true));
                        //}
                        //else
                        //{
                        //    _estados.Add(new Filtro(App.v_generales[i].v_estado, false));
                        //}
                    }
                }
            }
            IEnumerable<Filtro> _temp = _estados.OrderBy(x => x.v_texto);
            _estados = new ObservableCollection<Filtro>(_temp);
            v_descarga = false;
        }
        public async  void Fn_Filtro(object sender, EventArgs _Args)
        {
            if (v_descarga || v_lista.IsRefreshing)
                return;
            string[] _tit = new string[] {
                "Especialidades", "Estado","Ciudad"
            };
            ObservableCollection<Filtro>[] _filtr = new ObservableCollection<Filtro>[]
            {
                _especialidades,  _estados,_ciudades
            };
            await Navigation.PushAsync(new V_Filtro(_tit, _filtr));
            v_filtro = true;
        }
        async Task Orden()
        {
            if(v_tipo==0)
            {
                for (int i = 0; i < App.v_medicos.Count; i++)
                {
                    App.v_medicos[i].Fn_SetEspec();
                }
                IEnumerable<C_Medico> _temp=      App.v_medicos.OrderBy(x => x.v_Apellido);
                App.v_medicos = new ObservableCollection<C_Medico>(_temp);  
                await Task.Delay(100);
            }
            else if(v_tipo==1)
            {
                IEnumerable<C_Servicios> _temp = App.v_servicios.OrderBy(x => x.v_completo);
                App.v_servicios =new ObservableCollection<C_Servicios>( _temp);
                await Task.Delay(100);                
            }
            else if(v_tipo==2)
            {
                IEnumerable<C_ServGenerales> _temp = App.v_generales.OrderBy(x => x.v_completo);
                App.v_generales = new ObservableCollection<C_ServGenerales>(_temp);
                await Task.Delay(100);
            }
        }
        /// <summary>
        /// seleccionas un elemento de la lista para expandir su informacion
        /// </summary>
        async void Fn_Select(object sender, ItemTappedEventArgs args)
        {
            if (v_tipo==0)
            {
                if (!(args.Item is C_Medico item))
                    return;

                v_filtro = false;
                await Navigation.PushAsync(new V_MedicoVista(item) { Title = item.v_titulo+" " +item.v_Nombre });
                v_lista.SelectedItem = null;
            }
            else if(v_tipo==1)
            {
                if (!(args.Item is C_Servicios item))
                    return;

                v_filtro = false;
                await Navigation.PushAsync(new V_MedicoVista(item) { Title =  item.v_completo });
                v_lista.SelectedItem = null;
            }
            else if(v_tipo==2)
            {
                if (!(args.Item is C_ServGenerales item))
                    return;

                v_filtro = false;
                await Navigation.PushAsync(new V_MedicoVista(item) { Title =  item.v_completo });
                v_lista.SelectedItem = null;
            }
        }        
        public async void Fn_CargarDAtos()
        {
            string url = "";
            if(v_tipo==0)
            {
                HttpClient _cliente = new HttpClient();
                L_Error.IsVisible = true;
                L_Error.Text = "Procesando Información";
                try
                {
                    url = NombresAux.BASE_URL + "prueba_json.php";
                    HttpResponseMessage _respuestphp = await _cliente.PostAsync(url, null);
                    string _respu = await _respuestphp.Content.ReadAsStringAsync();
                    ObservableCollection<C_Medico> _med = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_respu);
                    L_Error.IsVisible = false;
                    //B_Filtrar.IsEnabled = true;
                    App.v_medicos = _med;
                    await Orden();
                    App.Fn_ImgSexo();
                    v_lista.ItemsSource = App.v_medicos;
                    App.Fn_GuardarRed(App.v_medicos);
                }
                catch (Exception _ex)
                {
                    if (Application.Current.Properties.ContainsKey(NombresAux.v_redmedica2))
                    {
                        string _json = App.Current.Properties[NombresAux.v_redmedica2] as string;
                        App.v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_json);
                        await Orden();
                        App.Fn_ImgSexo();
                        if(App.v_medicos.Count>0)
                        {
                            L_Error.IsVisible = false;
                            //B_Filtrar.IsEnabled = true;
                        }
                        else
                        {
                            L_Error.Text = "Error de Conexion";
                            L_Error.IsVisible = true;
                            //B_Filtrar.IsEnabled = false;
                        }
                        v_lista.ItemsSource = App.v_medicos;
                    }
                    else
                    {
                        L_Error.Text = "Error de Conexion";
                        L_Error.IsVisible = true;
                        //B_Filtrar.IsEnabled = false;
                        v_lista.ItemsSource = null;
                    }
                }
            }
            else if(v_tipo==1)
            {
                url = NombresAux.BASE_URL + "query_servicios_medicos.php";
                HttpClient _cliente = new HttpClient();
                L_Error.IsVisible = true;
                L_Error.Text = "Procesando Información";
                try
                {
                    HttpResponseMessage _respuestphp = await _cliente.PostAsync(url, null);
                    string _respu = await _respuestphp.Content.ReadAsStringAsync();
                    ObservableCollection<C_Servicios> _med = JsonConvert.DeserializeObject<ObservableCollection<C_Servicios>>(_respu);
                    if(_med.Count<1)
                    {
                        L_Error.Text = "Llega vacio";
                    }
                    else
                    {
                        L_Error.IsVisible = false;
                        //B_Filtrar.IsEnabled = true;
                    }
                    App.v_servicios = _med;
                    App.Fn_GuardarServcios(App.v_servicios);
                    await Orden();
                    v_lista.ItemsSource = App.v_servicios;
                }
                catch (Exception _ex)
                {
                    if (Application.Current.Properties.ContainsKey("servicios"))
                    {
                        string _json = App.Current.Properties["servicios"] as string;
                        App.v_servicios = JsonConvert.DeserializeObject<ObservableCollection<C_Servicios>>(_json);
                        await Orden();
                        //App.Fn_ImgSexo(v_tipo);
                        if (App.v_servicios.Count > 0)
                        {
                            L_Error.IsVisible = false;
                         //   B_Filtrar.IsEnabled = true;
                        }
                        else
                        {
                            L_Error.Text = "Error de Conexion";
                            L_Error.IsVisible = true;
                            //B_Filtrar.IsEnabled = false;
                        }
                        v_lista.ItemsSource = App.v_servicios;
                    }
                    else
                    {
                        L_Error.Text = "Error de Conexion";
                        L_Error.IsVisible = true;
                        //B_Filtrar.IsEnabled = false;
                        v_lista.ItemsSource = null;
                    }
                }
            }
            else if(v_tipo==2)
            { 
                HttpClient _cliente = new HttpClient();
                url = NombresAux.BASE_URL + "query_servicios_generales.php";
                L_Error.IsVisible = true;
                L_Error.Text = "Procesando Información";
                try
                {
                    HttpResponseMessage _respuestphp = await _cliente.PostAsync(url, null);
                    string _respu = await _respuestphp.Content.ReadAsStringAsync();
                    ObservableCollection<C_ServGenerales> _med = JsonConvert.DeserializeObject<ObservableCollection<C_ServGenerales>>(_respu);
                    if (_med.Count < 1)
                    {
                        L_Error.Text = "Error de Conexion";
                        L_Error.IsVisible = true;
                       // B_Filtrar.IsEnabled = false;
                    }
                    else
                    {
                        L_Error.IsVisible = false;
                        //B_Filtrar.IsEnabled = true;
                    }
                    App.v_generales = _med;
                    App.Fn_GuardarGenerales(App.v_generales);
                    await Orden();
                    v_lista.ItemsSource = App.v_generales;
                }
                catch (Exception _ex)
                {
                    if (Application.Current.Properties.ContainsKey("generales"))
                    {
                        string _json = App.Current.Properties["generales"] as string;
                        App.v_generales = JsonConvert.DeserializeObject<ObservableCollection<C_ServGenerales>>(_json);
                        await Orden();
                        if (App.v_generales.Count > 0)
                        {
                            L_Error.IsVisible = false;
                            //B_Filtrar.IsEnabled = true;
                        }
                        else
                        {
                            L_Error.Text = "Error de Conexion";
                            L_Error.IsVisible = true;
                          //  B_Filtrar.IsEnabled = false;
                        }
                        v_lista.ItemsSource = App.v_generales;
                    }
                    else
                    {
                        L_Error.Text = "Error de Conexion";
                        L_Error.IsVisible = true;
                        //B_Filtrar.IsEnabled = false;
                        v_lista.ItemsSource = null;
                    }
                }
            }
            Fn_CrearEspec();
        }
        /// <summary>
        /// refresh de la lista principal
        /// </summary>
        async void Fn_Refresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Fn_CargarDAtos();
            v_descarga = true;
            v_Search.Text = "";
            await Task.Delay(100);
            v_descarga = false;
            list.IsRefreshing = false;//cancelar la actualizacion
        }
        /// <summary>
        /// la barra de busqueda mientras escribes
        /// </summary>
        public void Fn_Buscar(object sender, TextChangedEventArgs e)
        {
            SearchBar _search = (SearchBar)sender;
            if (_search != null)
            {
                v_lista.ItemsSource = null;
                string _abuscar = _search.Text;//.StartsWith()
                if (string.IsNullOrEmpty(_abuscar) || string.IsNullOrWhiteSpace(_abuscar))
                {
                    //v_lista.IsVisible = false;
                    //Buscador.ItemsSource = null;
                    Buscador.IsVisible = false;
                    //v_lista.IsVisible = true;
                    if (v_tipo == 0)
                    {
                        v_lista.ItemsSource = App.v_medicos;
                    }
                    else if (v_tipo == 1)
                    {
                        v_lista.ItemsSource = App.v_servicios;
                    }
                    else if (v_tipo == 2)
                    {
                        v_lista.ItemsSource = App.v_generales;
                    }
                }
                else
                {
                    _abuscar = _abuscar.ToLower();
                    //v_lista.IsVisible = false;
                    //Buscador.ItemsSource = null;
                    Buscador.IsVisible = true;
                    ObservableCollection<string> _lista = new ObservableCollection<string>();
                    if (v_tipo == 0)//recorre solo los  medicos, para ver su nombre y apellido
                    {
                        for (int i = 0; i < App.v_medicos.Count; i++)//hacer cambios para cuando tienen  2 nombre apellidoos
                        {
                            if (App.v_medicos[i].v_Nombre.ToLower().StartsWith(_abuscar) && !_lista.Contains(App.v_medicos[i].v_Nombre))
                            {
                                _lista.Add(App.v_medicos[i].v_Nombre);
                            }
                            else if (App.v_medicos[i].v_Apellido.ToLower().StartsWith(_abuscar) && !_lista.Contains(App.v_medicos[i].v_Apellido))
                            {
                                _lista.Add(App.v_medicos[i].v_Apellido);
                            }
                        }
                    }
                    else if (v_tipo == 1)
                    {
                        for (int i = 0; i < App.v_servicios.Count; i++)
                        {
                            if (App.v_servicios[i].v_completo.ToLower().StartsWith(_abuscar) && !_lista.Contains(App.v_servicios[i].v_completo))
                            {
                                _lista.Add(App.v_servicios[i].v_completo);
                            }
                        }
                    }
                    else if (v_tipo == 2)
                    {
                        for (int i = 0; i < App.v_generales.Count; i++)
                        {
                            if (App.v_generales[i].v_completo.ToLower().StartsWith(_abuscar) && !_lista.Contains(App.v_generales[i].v_completo))
                            {
                                _lista.Add(App.v_generales[i].v_completo);
                            }
                        }
                    }
                    //recorre solamente las especialidades que ya se guardaron antes
                    for (int i = 0; i < _especialidades.Count; i++)
                    {
                        if (_especialidades[i].v_texto.ToLower().StartsWith(_abuscar))
                        {
                            _lista.Add(_especialidades[i].v_texto);
                        }
                    }
                    if (_lista.Count > 0)
                    {
                        Fn_BuscaStack(_lista);
                        ObservableCollection<PrefFil> _pref = new ObservableCollection<PrefFil>();
                        for (int i = 0; i < _lista.Count; i++)
                        {
                            _pref.Add(new PrefFil { v_texto = _lista[i] });
                        }
                        //Buscador.ItemsSource = _pref;
                        Buscador.IsVisible = true;
                    }
                    else
                    {
                        Buscador.IsVisible = false;
                    }
                }
            }
        }
        public void Fn_BuscaStack(ObservableCollection<string> _lista)
        {
            Buscador.Children.Clear();
            for(int i=0; i<_lista.Count; i++)
            {
                StackLayout _stack = new StackLayout() { BackgroundColor = Color.White };
                Label _nombre = new Label() {
                    Text = _lista[i],
                    BackgroundColor = Color.White,
                    TextColor =(Color)Application.Current.Resources["AzulPrincipal"],
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                TapGestureRecognizer _tapGest = new TapGestureRecognizer();
                _tapGest.Tapped += (s,e)=> {
                    var _sta = s as StackLayout;
                    _sta.BackgroundColor = (Color)App.Current.Resources["AzulFondo"];
                    Label _filtro = _sta.Children[0] as Label;
                    v_Search.Text = _filtro.Text;
                    if (v_tipo == 0)
                    {
                        ObservableCollection<C_Medico> _medicosTemp = new ObservableCollection<C_Medico>();
                        for (int _id = 0; _id < App.v_medicos.Count; _id++)
                        {
                            for (int _e = 0; _e < App.v_medicos[_id].v_ListaEsp.Count; _e++)
                            {
                                if (((App.v_medicos[_id].v_Nombre == _filtro.Text) ||
                                    (App.v_medicos[_id].v_Apellido == _filtro.Text) ||
                                    (App.v_medicos[_id].v_ListaEsp[_e].v_nombreEspec == _filtro.Text)
                                    ) && !_medicosTemp.Contains(App.v_medicos[_id]))
                                {
                                    _medicosTemp.Add(App.v_medicos[_id]);
                                }
                            }
                        }
                        v_lista.ItemsSource = _medicosTemp;
                    }
                    else if (v_tipo == 1)
                    {
                        ObservableCollection<C_Servicios> _serTemp = new ObservableCollection<C_Servicios>();
                        for (int _id = 0; _id < App.v_servicios.Count; _id++)
                        {
                            if (((App.v_servicios[_id].v_completo == _filtro.Text) ||
                                (App.v_servicios[_id].v_Especialidad == _filtro.Text)) && !_serTemp.Contains(App.v_servicios[_id]))
                            {
                                _serTemp.Add(App.v_servicios[_id]);
                            }
                        }
                        v_lista.ItemsSource = _serTemp;
                    }
                    else if (v_tipo == 2)
                    {
                        ObservableCollection<C_ServGenerales> _serTemp = new ObservableCollection<C_ServGenerales>();
                        for (int _id = 0; _id < App.v_generales.Count; _id++)
                        {
                            if (((App.v_generales[_id].v_completo == _filtro.Text) ||
                                (App.v_generales[_id].v_Especialidad == _filtro.Text)) && !_serTemp.Contains(App.v_generales[_id]))
                            {
                                _serTemp.Add(App.v_generales[_id]);
                            }
                        }
                        v_lista.ItemsSource = _serTemp;
                    }
                    v_lista.IsVisible = true;
                    v_lista.VerticalOptions = LayoutOptions.FillAndExpand;
                    Buscador.IsVisible = false;
                };
                _stack.Children.Add(_nombre);
                _stack.GestureRecognizers.Add(_tapGest);
                Buscador.Children.Add(_stack);
            }
        }
    }
}
/// <summary>
/// Lista que el usuario elige cada  vez que le pica
/// </summary>
//List<string> _filEspec = new List<string>();
/// <summary>
/// Lista que el usuario elige cada  vez que le pica
/// </summary>
//List<string> _filCiud = new List<string>();
/// <summary>
/// Lista que el usuario elige cada  vez que le pica
/// </summary>
//List<string> _filEstado = new List<string>();

//if(v_tipo==0)
//{
//    ObservableCollection<C_Medico> _filtrada = new ObservableCollection<C_Medico>();
//    for (int _idmed = 0; _idmed < App.v_medicos.Count; _idmed++)
//    {
//        for(int _idesp=0; _idesp<App.v_medicos[_idmed].v_ListaEsp.Count; _idesp++)
//        {
//            for(int _idfil=0; _idfil< _filtro.Count; _idfil++ )
//            {
//                if( (  App.v_medicos[_idmed].v_Ciudad== _filtro[_idfil]  || 
//                    App.v_medicos[_idmed].v_estado == _filtro[_idfil]  ||
//                    App.v_medicos[_idmed].v_ListaEsp[_idesp].v_nombreEspec == _filtro[_idfil] )
//                    && (!_filtrada.Contains(App.v_medicos[_idmed])))
//                {
//                    _filtrada.Add(App.v_medicos[_idmed]);
//                }
//            }
//        }
//    }
//    IEnumerable<C_Medico> _temp = _filtrada.OrderBy(x => x.v_Apellido);
//    _filtrada = new ObservableCollection<C_Medico>(_temp);
//    if (_filtrada.Count > 0)
//    {
//        v_lista.ItemsSource = _filtrada;
//    }
//    else
//    {
//        await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
//        v_lista.ItemsSource = App.v_medicos;
//    }
//}
//else if(v_tipo==1)
//{
//    ObservableCollection<C_Servicios> _filtrada = new ObservableCollection<C_Servicios>();
//    for (int _idmed = 0; _idmed < App.v_servicios.Count; _idmed++)
//    {
//        for (int _idfil = 0; _idfil < _filtro.Count; _idfil++)
//        {
//            if ((App.v_servicios[_idmed].v_Ciudad == _filtro[_idfil] ||
//                App.v_servicios[_idmed].v_estado == _filtro[_idfil] ||
//                 App.v_servicios[_idmed].v_Especialidad == _filtro[_idfil])
//                 && (!_filtrada.Contains(App.v_servicios[_idmed])))
//                {
//                    _filtrada.Add(App.v_servicios[_idmed]);
//                }
//        }
//    }
//    IEnumerable<C_Servicios> _temp = _filtrada.OrderBy(x => x.v_completo);
//    _filtrada = new ObservableCollection<C_Servicios>(_temp);
//    if (_filtrada.Count > 0)
//    {
//        v_lista.ItemsSource = _filtrada;
//    }
//    else
//    {
//        await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
//        v_lista.ItemsSource = App.v_medicos;
//    }
//}
//else if (v_tipo == 2)
//{
//    ObservableCollection<C_ServGenerales> _filtrada = new ObservableCollection<C_ServGenerales>();
//    for (int _idmed = 0; _idmed < App.v_generales.Count; _idmed++)
//    {
//        for (int _idfil = 0; _idfil < _filtro.Count; _idfil++)
//        {
//            if ((App.v_servicios[_idmed].v_Ciudad == _filtro[_idfil] ||
//                App.v_servicios[_idmed].v_estado == _filtro[_idfil] ||
//                 App.v_servicios[_idmed].v_Especialidad == _filtro[_idfil])
//                 && (!_filtrada.Contains(App.v_generales[_idmed])))
//            {
//                _filtrada.Add(App.v_generales[_idmed]);
//            }
//        }
//    }
//    IEnumerable<C_ServGenerales> _temp = _filtrada.OrderBy(x => x.v_completo);
//    _filtrada = new ObservableCollection<C_ServGenerales>(_temp);
//    if (_filtrada.Count > 0)
//    {
//        v_lista.ItemsSource = _filtrada;
//    }
//    else
//    {
//        await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
//        v_lista.ItemsSource = App.v_medicos;
//    }
/*
/// <summary>
/// lista que esta abajop de la barra de busqueda 
/// </summary>
 *public void Fn_TapFiltro(object sender, ItemTappedEventArgs _args)
{
    PrefFil _nuevoFiltro =_args.Item as PrefFil;
    v_Search.Text = _nuevoFiltro.v_texto;
    if(v_tipo==0)
    {
        ObservableCollection<C_Medico> _medicosTemp = new ObservableCollection<C_Medico>();
        for(int i=0; i<App.v_medicos.Count; i++)
        {
            for (int e = 0; e < App.v_medicos[i].v_ListaEsp.Count; e++)
            {
                if (((App.v_medicos[i].v_Nombre == _nuevoFiltro.v_texto) ||
                    (App.v_medicos[i].v_Apellido == _nuevoFiltro.v_texto) || 
                    (App.v_medicos[i].v_ListaEsp[e].v_nombreEspec == _nuevoFiltro.v_texto)
                    )&& !_medicosTemp.Contains(App.v_medicos[i]) )
                {
                    _medicosTemp.Add(App.v_medicos[i]);
                }
            }
        }
        v_lista.ItemsSource = _medicosTemp;
    }
    else if(v_tipo==1)
    {
        ObservableCollection<C_Servicios> _serTemp = new ObservableCollection<C_Servicios>();
        for (int i = 0; i < App.v_servicios.Count; i++)
        {
            if (( (App.v_servicios[i].v_completo == _nuevoFiltro.v_texto) ||
                (App.v_servicios[i].v_Especialidad == _nuevoFiltro.v_texto))&& !_serTemp.Contains(App.v_servicios[i]) )
            {
                _serTemp.Add(App.v_servicios[i]);
            }
        }
        v_lista.ItemsSource = _serTemp;
    }
    else if(v_tipo==2)
    {
        ObservableCollection<C_ServGenerales> _serTemp = new ObservableCollection<C_ServGenerales>();
        for (int i = 0; i < App.v_generales.Count; i++)
        {
            if (((App.v_generales[i].v_completo == _nuevoFiltro.v_texto) ||
                (App.v_generales[i].v_Especialidad == _nuevoFiltro.v_texto) ) && !_serTemp.Contains(App.v_generales[i]))
            {
                _serTemp.Add(App.v_generales[i]);
            }
        }
        v_lista.ItemsSource = _serTemp;
    }
    v_lista.IsVisible = true;
    v_lista.VerticalOptions = LayoutOptions.FillAndExpand;
    Buscador.IsVisible = false;
}        
*/
/*
/// <summary>
/// agregar o quitar del filtro de especialidad
/// </summary>
 void Fn_TapEspec(object sender, ItemTappedEventArgs _Args)
{//para mostrar un cambio en la lista la estoy haciendo null y despues volviendo a llenar
    var list = (ListView)sender;
    list.ItemsSource = null;
    var _valor =  _Args.Item as Filtro;
    for (int i = 0; i < _especialidades.Count; i++)
    {
        if (_especialidades[i].v_texto == _valor.v_texto)
        {
            _especialidades[i].v_visible = !_especialidades[i].v_visible;
        }
    }
    if (!_filEspec.Contains(_valor.v_texto))
    {
        _filEspec.Add(_valor.v_texto);
    }
    else
    {
        _filEspec.Remove(_valor.v_texto);
    }
    list.ItemsSource = _especialidades; 
}
/// <summary>
/// agregar o quitar del filtro de especialidad
/// </summary>
void Fn_TapCiu(object sender, ItemTappedEventArgs _Args)
{
    //para mostrar un cambio en la lista la estoy haciendo null y despues volviendo a llenar
    var list = (ListView)sender;
    list.ItemsSource = null;
    var _valor = _Args.Item as Filtro;//cast al template que ya esta preparado 
    if (!_filCiud.Contains(_valor.v_texto))
    {
        for (int i = 0; i < _ciudades.Count; i++)
        {
            if (_ciudades[i].v_texto == _valor.v_texto)
            {
                _ciudades[i].v_visible = !_ciudades[i].v_visible;
            }
        }
        _filCiud.Add(_valor.v_texto);
        list.ItemsSource = _ciudades;
    }
    else
    {
        for (int i = 0; i < _ciudades.Count; i++)
        {
            if (_ciudades[i].v_texto == _valor.v_texto)
            {
                _ciudades[i].v_visible = !_ciudades[i].v_visible;
            }
        }
        _filCiud.Remove(_valor.v_texto);
        list.ItemsSource = _ciudades;
    }
}
*/
/*
private void FnCreaLista()
{
if(_especialidades.Count>0)
{
StackEspe.Children.Clear();
ContentView _vi = new ContentView() { HeightRequest = 1, BackgroundColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand };
StackEspe.Children.Add(_vi);
for (int i = 0; i < _especialidades.Count; i++)
{
   StackLayout _stack = new StackLayout() { };
   _stack.BindingContext = _especialidades[i];
   Grid _grid = new Grid();
   _grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
   _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
   _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
   Label _espe = new Label() { Text = _especialidades[i].v_texto, VerticalOptions= LayoutOptions.End, TextColor= Color.Black };                  
   Image _img = new Image() { Source = "Palomita.png", IsVisible = _especialidades[i].v_visible,
   HeightRequest=30, WidthRequest=30, Aspect = Aspect.AspectFit
   };
   _grid.Children.Add(_espe, 0, 0);
   _grid.Children.Add(_img, 1, 0);
   ContentView _view = new ContentView() { HeightRequest = 1, BackgroundColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand };
   TapGestureRecognizer _tap = new TapGestureRecognizer();
   _tap.Tapped += (object _sender, EventArgs _args) =>
   {
       StackLayout _st = _sender as StackLayout;
       Filtro _fil = _st.BindingContext as Filtro;
       if (_fil != null)
       {
           for (int _j = 0; _j < _especialidades.Count; _j++)
           {
               if (_especialidades[_j].v_texto == _fil.v_texto)
               {
                   _especialidades[_j].v_visible = !_especialidades[_j].v_visible;
               }
           }
           if (!_filEspec.Contains(_fil.v_texto))
           {
               _filEspec.Add(_fil.v_texto);
           }
           else
           {
               _filEspec.Remove(_fil.v_texto);
           }
           FnCreaLista();
       }
   };
   _stack.GestureRecognizers.Add(_tap);
   _stack.Children.Add(_grid);
   _stack.Children.Add(_view);
   StackEspe.Children.Add(_stack);
}
}
if (_ciudades.Count > 0)
{
StackCiudad.Children.Clear();
ContentView _vi = new ContentView() { HeightRequest = 1, BackgroundColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand };
StackCiudad.Children.Add(_vi);
for (int i = 0; i < _ciudades.Count; i++)
{
   StackLayout _stack = new StackLayout() { };
   _stack.BindingContext = _ciudades[i];
   Grid _grid = new Grid();
   _grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
   _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
   _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
   Label _espe = new Label() { Text = _ciudades[i].v_texto , VerticalOptions = LayoutOptions.End, TextColor = Color.Black };
   Image _img = new Image() { Source = "Palomita.png", IsVisible = _ciudades[i].v_visible,
       HeightRequest = 30,
       WidthRequest = 30,
       Aspect = Aspect.AspectFit
   };
   _grid.Children.Add(_espe, 0, 0);
   _grid.Children.Add(_img, 1, 0);
   ContentView _view = new ContentView() { HeightRequest = 1, BackgroundColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand };
   TapGestureRecognizer _tap = new TapGestureRecognizer();
   _tap.Tapped += (object _sender, EventArgs _args) =>
   {
       StackLayout _st = _sender as StackLayout;
       Filtro _fil = _st.BindingContext as Filtro;
       if (_fil != null)
       {
           if (!_filCiud.Contains(_fil.v_texto))
           {
               for (int _j = 0; _j < _ciudades.Count; _j++)
               {
                   if (_ciudades[_j].v_texto == _fil.v_texto)
                   {
                       _ciudades[_j].v_visible = !_ciudades[_j].v_visible;
                   }
               }
               _filCiud.Add(_fil.v_texto);
           }
           else
           {
               for (int _j = 0; _j < _ciudades.Count; _j++)
               {
                   if (_ciudades[_j].v_texto == _fil.v_texto)
                   {
                       //_ciudades[i].v_color = Color.Blue;
                       _ciudades[_j].v_visible = !_ciudades[_j].v_visible;
                   }
               }
               _filCiud.Remove(_fil.v_texto);
           }
           FnCreaLista();
       }
   };
   _stack.GestureRecognizers.Add(_tap);
   _stack.Children.Add(_grid);
   _stack.Children.Add(_view);
   StackCiudad.Children.Add(_stack);
}
}            
if (_estados.Count > 0)
{
StackEstado.Children.Clear();
ContentView _vi = new ContentView() { HeightRequest = 1, BackgroundColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand };
StackEstado.Children.Add(_vi);
for (int i = 0; i < _estados.Count; i++)
{
   StackLayout _stack = new StackLayout() { };
   _stack.BindingContext = _estados[i];
   Grid _grid = new Grid();
   _grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
   _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
   _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
   Label _espe = new Label() { Text = _estados[i].v_texto, VerticalOptions = LayoutOptions.End, TextColor = Color.Black };
   Image _img = new Image() { Source = "Palomita.png", IsVisible = _estados[i].v_visible,
       HeightRequest = 30,
       WidthRequest = 30,
       Aspect = Aspect.AspectFit
   };
   _grid.Children.Add(_espe, 0, 0);
   _grid.Children.Add(_img, 1, 0);
   ContentView _view = new ContentView() { HeightRequest = 1, BackgroundColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand };
   TapGestureRecognizer _tap = new TapGestureRecognizer();
   _tap.Tapped += (object _sender, EventArgs _args) =>
   {
       StackLayout _st = _sender as StackLayout;
       Filtro _fil = _st.BindingContext as Filtro;
       if (_fil != null)
       {
           for (int _j = 0; _j < _estados.Count; _j++)
           {
               if (_estados[_j].v_texto == _fil.v_texto)
               {
                   _estados[_j].v_visible = !_estados[_j].v_visible;
               }
           }
           if (!_filEstado.Contains(_fil.v_texto))
           {
               _filEstado.Add(_fil.v_texto);
           }
           else
           {
               _filEstado.Remove(_fil.v_texto);
           }
           FnCreaLista();
       }
   };
   _stack.GestureRecognizers.Add(_tap);
   _stack.Children.Add(_grid);
   _stack.Children.Add(_view);
   StackEstado.Children.Add(_stack);
}
}            
}

private async void Fn_StackEspe(object sender, EventArgs e)
{
Image _img = sender as Image;
if(StackEspe.IsVisible)
{
await _img.RotateTo(180, 100, Easing.Linear);
}
else
{
await _img.RotateTo(90, 100, Easing.Linear);
}
StackEspe.IsVisible = !StackEspe.IsVisible;
}
private async void Fn_StackCiudad(object sender, EventArgs e)
{
Image _img = sender as Image;
if (StackCiudad.IsVisible)
{
await _img.RotateTo(180, 100, Easing.Linear);
}
else
{
await _img.RotateTo(90, 100, Easing.Linear);
}
StackCiudad.IsVisible = !StackCiudad.IsVisible;
}
private async void Fn_StackEstado(object sender, EventArgs e)
{
Image _img = sender as Image;
if (StackEstado.IsVisible)
{
await _img.RotateTo(180, 100, Easing.Linear);
}
else
{
await _img.RotateTo(90, 100, Easing.Linear);
}
StackEstado.IsVisible = !StackEstado.IsVisible;

}
*/
/*
public void Fn_BorrarFiltro(object sender, EventArgs _args)
{
    for(int i=0; i<_especialidades.Count; i++)//quita las imagenes del filtro
    {
        if(_especialidades[i].v_visible==true)
        {
        _especialidades[i].v_visible = false;
        }
    }
    for (int i = 0; i < _ciudades.Count; i++)//quita las imagenes del filtro
    {
        if (_ciudades[i].v_visible == true)
        {
            _ciudades[i].v_visible = false;
        }
    }
    for (int i = 0; i < _estados.Count; i++)//quita las imagenes del filtro
    {
        if (_estados[i].v_visible == true)
        {
            _estados[i].v_visible = false;
        }
    }
    //_filEspec.Clear();//limpia lo que el usuario eligio antes
    //_filCiud.Clear();
    //_filEstado.Clear();
    if (v_tipo==0)//regresa todo a lo inicial
    {
        v_lista.ItemsSource = App.v_medicos;
    }
    else if(v_tipo==1)
    {
        v_lista.ItemsSource = App.v_servicios;
    }
    else if(v_tipo==2)
    {
        v_lista.ItemsSource = App.v_generales;
    }
    Fn_Filtro(sender, _args);
}
public async void Fn_Aceptar(object sender, EventArgs _args)
{
    v_lista.ItemsSource = null;
    if (v_tipo == 0)
    {
        ObservableCollection<C_Medico> _filtrada = new ObservableCollection<C_Medico>();
        //if (_filCiud.Count > 0 && _filEspec.Count > 0)
        //{
        //    //recorre toda la lista de medicos
        //    for (int i = 0; i < App.v_medicos.Count; i++)
        //    {
        //        //recorre lista de ciudad a filtrar
        //        for (int j = 0; j < _filCiud.Count; j++)
        //        {//recorre lista de especialidad a filtrar
        //            for (int k = 0; k < _filEspec.Count; k++)
        //            {
        //                //falta recorrer la lista de cada medico por si tiene mas especialidades
        //                for (int e = 0; e < App.v_medicos[i].v_ListaEsp.Count; e++)
        //                {
        //                    if ((App.v_medicos[i].v_Ciudad == _filCiud[j] 
        //                         && App.v_medicos[i].v_ListaEsp[e].v_nombreEspec == _filEspec[k]) 
        //                        && !_filtrada.Contains(App.v_medicos[i]))
        //                    {
        //                        _filtrada.Add(App.v_medicos[i]);
        //                    }
        //                }                                
        //            }
        //        }
        //    }
        //}
        //else
        //{
            for (int j = 0; j < _filEstado.Count; j++)
            {//recorre toda la lista de medicos
                for (int i = 0; i < App.v_medicos.Count; i++)
                { //recorre lista de ciudad a filtrar
                    if (App.v_medicos[i].v_estado == _filEstado[j] && !_filtrada.Contains(App.v_medicos[i]))
                    {
                        _filtrada.Add(App.v_medicos[i]);
                    }
                }
            }
            for (int j = 0; j < _filCiud.Count; j++)
            {//recorre toda la lista de medicos
                for (int i = 0; i < App.v_medicos.Count; i++)
                { //recorre lista de ciudad a filtrar
                    if (App.v_medicos[i].v_Ciudad == _filCiud[j] && !_filtrada.Contains(App.v_medicos[i]))
                    {
                        _filtrada.Add(App.v_medicos[i]);
                    }
                }
            }
            //recorre lista de especialidad a filtrar
            for (int j = 0; j < _filEspec.Count; j++)
            {
                //recorre toda la lista de medicos
                for (int i = 0; i < App.v_medicos.Count; i++)
                {
                    //falta recorrer la lista de cada medico por si tiene mas especialidades
                    for  (int e = 0; e < App.v_medicos[i].v_ListaEsp.Count; e++)
                    {
                        if (App.v_medicos[i].v_ListaEsp[e].v_nombreEspec == _filEspec[j] && !_filtrada.Contains(App.v_medicos[i]))
                        {
                            _filtrada.Add(App.v_medicos[i]);
                        }
                         ////if (App.v_medicos[i].v_Especialidad == _filEspec[j] && !_filtrada.Contains(App.v_medicos[i]))
                         ////{
                         ////    _filtrada.Add(App.v_medicos[i]);
                         ////}
                    }
                }
            }
        //}
        IEnumerable<C_Medico> _temp = _filtrada.OrderBy(x => x.v_Apellido);
        _filtrada = new ObservableCollection<C_Medico>(_temp);
        await Task.Delay(100);
        if (_filtrada.Count > 0)
        {
            v_lista.ItemsSource = _filtrada;
        }
        else
        {
            await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
            v_lista.ItemsSource = App.v_medicos;
        }
    }
    else if (v_tipo == 1)
    {
        ObservableCollection<C_Servicios> _filtrada = new ObservableCollection<C_Servicios>();
        //if (_filCiud.Count > 0 && _filEspec.Count > 0)
        //{
        //    //recorre toda la lista de medicos
        //    for (int i = 0; i < App.v_servicios.Count; i++)
        //    {
        //        //recorre lista de ciudad a filtrar
        //        for (int j = 0; j < _filCiud.Count; j++)
        //        {//recorre lista de especialidad a filtrar
        //            for (int k = 0; k < _filEspec.Count; k++)
        //            {
        //                if ((App.v_servicios[i].v_Ciudad == _filCiud[j] && App.v_servicios[i].v_Especialidad == _filEspec[k]) && !_filtrada.Contains(App.v_servicios[i]))
        //                {
        //                    _filtrada.Add(App.v_servicios[i]);
        //                }
        //            }
        //        }
        //    }
        //}
        //else
        //{
            for (int j = 0; j < _filEstado.Count; j++)
            {//recorre toda la lista de medicos
                for (int i = 0; i < App.v_servicios.Count; i++)
                { //recorre lista de ciudad a filtrar
                    if (App.v_servicios[i].v_estado == _filEstado[j] && !_filtrada.Contains(App.v_servicios[i]))
                    {
                        _filtrada.Add(App.v_servicios[i]);
                    }
                }
            }
            for (int j = 0; j < _filCiud.Count; j++)
            {//recorre toda la lista de medicos
                for (int i = 0; i < App.v_servicios.Count; i++)
                { //recorre lista de ciudad a filtrar
                    if (App.v_servicios[i].v_Ciudad == _filCiud[j] && !_filtrada.Contains(App.v_servicios[i]))
                    {
                        _filtrada.Add(App.v_servicios[i]);
                    }
                }
            }
            //recorre lista de especialidad a filtrar
            for (int j = 0; j < _filEspec.Count; j++)
            {
                //recorre toda la lista de medicos
                for (int i = 0; i < App.v_servicios.Count; i++)
                {
                    if (App.v_servicios[i].v_Especialidad == _filEspec[j] && !_filtrada.Contains(App.v_servicios[i]))
                    {
                        _filtrada.Add(App.v_servicios[i]);
                    }
                }
            }
       // }
        IEnumerable<C_Servicios> _temp = _filtrada.OrderBy(x => x.v_completo);
        _filtrada = new ObservableCollection<C_Servicios>(_temp);
        if (_filtrada.Count > 0)
        {
            v_lista.ItemsSource = _filtrada;
        }
        else
        {
            await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
            v_lista.ItemsSource = App.v_servicios;
        }
    }
    else if (v_tipo == 2)
    {
        ObservableCollection<C_ServGenerales> _filtrada = new ObservableCollection<C_ServGenerales>();
        //if (_filCiud.Count > 0 && _filEspec.Count > 0)
        //{
        //    //recorre toda la lista de medicos
        //    for (int i = 0; i < App.v_generales.Count; i++)
        //    {
        //        //recorre lista de ciudad a filtrar
        //        for (int j = 0; j < _filCiud.Count; j++)
        //        {//recorre lista de especialidad a filtrar
        //            for (int k = 0; k < _filEspec.Count; k++)
        //            {
        //                if ((App.v_generales[i].v_Ciudad == _filCiud[j] && App.v_generales[i].v_Especialidad == _filEspec[k]) && !_filtrada.Contains(App.v_generales[i]))
        //                {
        //                    _filtrada.Add(App.v_generales[i]);
        //                }
        //            }
        //        }
        //    }
        //}
        //else
        //{
            for (int j = 0; j < _filEstado.Count; j++)
            {//recorre toda la lista de medicos
                for (int i = 0; i < App.v_generales.Count; i++)
                { //recorre lista de ciudad a filtrar
                    if (App.v_generales[i].v_estado == _filEstado[j] && !_filtrada.Contains(App.v_generales[i]))
                    {
                        _filtrada.Add(App.v_generales[i]);
                    }
                }
            }
            for (int j = 0; j < _filCiud.Count; j++)
            {//recorre toda la lista de medicos
                for (int i = 0; i < App.v_generales.Count; i++)
                { //recorre lista de ciudad a filtrar
                    if (App.v_generales[i].v_Ciudad == _filCiud[j] && !_filtrada.Contains(App.v_generales[i]))
                    {
                        _filtrada.Add(App.v_generales[i]);
                    }
                }
            }
            //recorre lista de especialidad a filtrar
            for (int j = 0; j < _filEspec.Count; j++)
            {
                //recorre toda la lista de medicos
                for (int i = 0; i < App.v_generales.Count; i++)
                {
                    if (App.v_generales[i].v_Especialidad == _filEspec[j] && !_filtrada.Contains(App.v_generales[i]))
                    {
                        _filtrada.Add(App.v_generales[i]);
                    }
                }
            }
        //}
        IEnumerable < C_ServGenerales> _temp = _filtrada.OrderBy(x => x.v_completo);
        _filtrada = new ObservableCollection<C_ServGenerales>(_temp);
        if (_filtrada.Count > 0)
        {
            v_lista.ItemsSource = _filtrada;
        }
        else
        {
            await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
            v_lista.ItemsSource = App.v_generales;
        }
    }
    Fn_Filtro(sender, _args);
}
*/
