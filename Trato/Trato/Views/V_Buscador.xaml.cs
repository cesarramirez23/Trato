using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Trato.Personas;
using System.Collections.ObjectModel;//listas observa
using System.Collections;//para el sorte list
using System.Net.Http;
using Newtonsoft.Json;



namespace Trato.Views
{
    public class Filtro
    {
       public string v_texto { get; set; }
       public Color v_color { get; set; }

        public Filtro()
        {
            v_texto = "";
            v_color = Color.Blue;
        }
        public Filtro(string _texto)
        {
            v_texto = _texto;
            v_color = Color.Blue;
        }
        public Filtro(string _texto, bool _Activo)
        {
            v_texto = _texto;
            if (_Activo)
            {
                v_color = Color.Red;
            }
            else
            {
                v_color = Color.Blue;
            }
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Buscador : ContentPage
    {
        /// <summary>
        /// para saber si es medico o servicios lo que se va a mostrar
        /// </summary>
        bool v_medico=false;
        /// <summary>
        /// la pantalla con los filtro esta visible
        /// </summary>
        bool v_filtro = false;
        /// <summary>
        /// Lista que el usuario elige
        /// </summary>
        List<string> _filEspec = new List<string>();
        /// <summary>
        /// Lista que el usuario elige
        /// </summary>
        List<string> _filCiud = new List<string>();
        /// <summary>
        /// textos que se agregan a la lista visible
        /// </summary>
        ObservableCollection<Filtro> _especialidades = new ObservableCollection<Filtro>();
        /// <summary>
        /// textos que se agregan a la lista visible
        /// </summary>
        ObservableCollection<Filtro> _ciudades = new ObservableCollection<Filtro>();

        public V_Buscador()
        {
            InitializeComponent();
            overlay.IsVisible = v_filtro;
        }
        public V_Buscador(bool _valor)
        {
            InitializeComponent();
            overlay.IsVisible = v_filtro;
            if(_valor)
            {
                v_medico = true;
                Orden();
                v_lista.Header = "Medico,Desliza hacia arriba o abajo para ver mas sugerencias";
                v_lista.ItemsSource = App.v_medicos;
            }
            else
            {
                v_medico = false;
                v_lista.Header = "Servicios,Desliza hacia arriba o abajo para ver mas sugerencias";
                Orden();
                v_lista.ItemsSource = App.v_servicios;
            }
            Buscador.IsVisible = false;
            Fn_CargarDAtos();
        }
        public async void Fn_CrearCiud()
        {
            if(v_medico)
            {
                _ciudades.Clear();
                List<string> _tempCont = new List<string>();
                for (int i=0;i<App.v_medicos.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_medicos[i].v_Ciudad))
                    {
                        _tempCont.Add(App.v_medicos[i].v_Ciudad);
                        if (_filCiud.Contains(App.v_medicos[i].v_Ciudad))
                        {
                            _ciudades.Add(new Filtro(App.v_medicos[i].v_Ciudad, true));
                        }
                        else
                        {
                            _ciudades.Add(new Filtro(App.v_medicos[i].v_Ciudad, false));
                        }
                    }
                }
            }
            else
            {
                _ciudades.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_servicios.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_servicios[i].v_Ciudad))
                    {
                        _tempCont.Add(App.v_servicios[i].v_Ciudad);
                        if (_filCiud.Contains(App.v_servicios[i].v_Ciudad))
                        {
                            _ciudades.Add(new Filtro(App.v_servicios[i].v_Ciudad, true));
                        }
                        else
                        {
                            _ciudades.Add(new Filtro(App.v_servicios[i].v_Ciudad, false));
                        }
                    }
                }
            }
            filCiudad.ItemsSource = _ciudades;
           await  Task.Delay(100);
        }
        public async void Fn_CrearEspec()
        {
            if (v_medico)
            {
                _especialidades.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_medicos.Count; i++)
                {
               // _fil.v_texto= App.v_medicos[i].v_Especialidad ;
                    if (!_tempCont.Contains( App.v_medicos[i].v_Especialidad))
                    {
                        _tempCont.Add(App.v_medicos[i].v_Especialidad);
                        if(_filEspec.Contains(App.v_medicos[i].v_Especialidad))
                        {
                            _especialidades.Add(new Filtro(App.v_medicos[i].v_Especialidad,true));
                        }
                        else
                        {
                            _especialidades.Add(new Filtro(App.v_medicos[i].v_Especialidad,false));
                        }
                    }
                }
            }
            else
            {
                _especialidades.Clear();
                List<string> _tempCont = new List<string>();
                for (int i = 0; i < App.v_servicios.Count; i++)
                {
                    if (!_tempCont.Contains(App.v_servicios[i].v_Especialidad))
                    {
                        _tempCont.Add(App.v_servicios[i].v_Especialidad);
                        if (_filEspec.Contains(App.v_servicios[i].v_Especialidad))
                        {
                            _especialidades.Add(new Filtro(App.v_servicios[i].v_Especialidad, true));
                        }
                        else
                        {
                            _especialidades.Add(new Filtro(App.v_servicios[i].v_Especialidad, false));
                        }
                    }
                }
            }
            filEspc.ItemsSource = _especialidades;
            await Task.Delay(100);
        }
        public void Fn_Cancelar(object sender, EventArgs _Args)
        {
            _filCiud.Clear();
            _filEspec.Clear();
            filEspc.ItemsSource = null;
            for(int i=0; i<_especialidades.Count;i++)
            {
                _especialidades[i].v_color = Color.Blue;
            }
            filEspc.ItemsSource = _especialidades;

            filCiudad.ItemsSource = null;
            for (int i = 0; i < _ciudades.Count; i++)
            {
                _ciudades[i].v_color = Color.Blue;
            }
            filCiudad.ItemsSource = _ciudades;

            if (v_medico)
            {
                v_lista.ItemsSource = App.v_medicos;
            }
            else
            {
                v_lista.ItemsSource = App.v_servicios;
            }
            Fn_Filtro(sender, _Args);
        }
        public void Fn_BorrarFiltro(object sender, EventArgs _args)
        {
            if (v_medico)
            {
                _filEspec.Clear();
                _filCiud.Clear();
                v_lista.ItemsSource = App.v_medicos;
            }
            else
            {
                _filEspec.Clear();
                _filCiud.Clear();
                v_lista.ItemsSource = App.v_servicios;
            }
            Fn_Filtro(sender, _args);

        }
        public async void Fn_Aceptar(object sender, EventArgs _args)
        {
            if(v_medico)
            {
                ObservableCollection<C_Medico> _filtrada = new ObservableCollection<C_Medico>();


                if(_filCiud.Count>0 && _filEspec.Count>0 )
                {
                    //recorre toda la lista de medicos
                    for (int i = 0; i < App.v_medicos.Count; i++)
                    {
                        //recorre lista de ciudad a filtrar
                        for (int j = 0; j < _filCiud.Count; j++)
                        {//recorre lista de especialidad a filtrar
                            for (int k=0;k<_filEspec.Count; k++)
                            {
                                if ((App.v_medicos[i].v_Ciudad == _filCiud[j]  && App.v_medicos[i].v_Especialidad== _filEspec[k] )&& !_filtrada.Contains(App.v_medicos[i]))
                                {
                                    _filtrada.Add(App.v_medicos[i]);
                                }
                            }
                        }
                    }
                }
                else
                {
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
                            if (App.v_medicos[i].v_Especialidad == _filEspec[j] && !_filtrada.Contains(App.v_medicos[i]))
                            {
                                _filtrada.Add(App.v_medicos[i]);
                            }
                        }
                    }
                }
                IEnumerable<C_Medico> _temp = _filtrada.OrderBy(x => x.v_Apellido);
                _filtrada = new ObservableCollection<C_Medico>(_temp);

                await Task.Delay(100);
                if (_filtrada.Count>0)
                {
                    v_lista.ItemsSource = _filtrada;
                }
                else
                {
                    await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
                    v_lista.ItemsSource = App.v_medicos;
                }
            }
            else
            {
                ObservableCollection<C_Servicios> _filtrada = new ObservableCollection<C_Servicios>();
                //recorre toda la lista de medicos
                for (int i = 0; i < App.v_servicios.Count; i++)
                {
                    //recorre lista de ciudad a filtrar
                    for (int j = 0; j < _filCiud.Count; j++)
                    {
                        if (App.v_servicios[i].v_Ciudad == _filCiud[j] && !_filtrada.Contains(App.v_servicios[i]))
                        {
                            _filtrada.Add(App.v_servicios[i]);
                        }
                    }
                    //recorre lista de especialidad a filtrar
                    for (int j = 0; j < _filEspec.Count; j++)
                    {
                        if (App.v_servicios[i].v_Especialidad == _filEspec[j] && !_filtrada.Contains(App.v_servicios[i]))
                        {
                            _filtrada.Add(App.v_servicios[i]);
                        }
                    }
                }
                if (_filtrada.Count > 0)
                {
                    v_lista.ItemsSource = _filtrada;
                }
                else
                {
                    await DisplayAlert("Filtro vacio", "No se encontraron resultados", "Aceptar");
                    v_lista.ItemsSource = App.v_servicios;
                }
                IEnumerable<C_Servicios> _temp = _filtrada.OrderBy(x => x.v_Nombre);
                _filtrada = new ObservableCollection<C_Servicios>(_temp);
            }
            Fn_Filtro(sender, _args);
        }
        /// <summary>
        /// activar/Desactivar la pantalla de los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_Args"></param>
        public void Fn_Filtro(object sender, EventArgs _Args)
        {
            v_filtro = !v_filtro;
            if(v_filtro)
            {
                Fn_CrearCiud();
                Fn_CrearEspec();
            }
            overlay.IsVisible = v_filtro;
        }
        async void Orden()
        {
            if(v_medico)
            {
                //  List<C_Medico> temp = App.v_medicos.OrderBy(x => x.v_Nombre).ToList(); ;
                IEnumerable<C_Medico> _temp=      App.v_medicos.OrderBy(x => x.v_Apellido);
                App.v_medicos = new ObservableCollection<C_Medico>(_temp);
               await Task.Delay(100);
            }
            else
            {
                IEnumerable<C_Servicios> _temp = App.v_servicios.OrderBy(x => x.v_Nombre);
                App.v_servicios =new ObservableCollection<C_Servicios>( _temp);
                await Task.Delay(100);                
            }

        }
        /// <summary>
        /// seleccionas un elemento de la lista para expandir su informacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void Fn_Select(object sender, SelectedItemChangedEventArgs args)
        {
            if(v_medico)
            {
                C_Medico item = args.SelectedItem as C_Medico;
                if (item == null)
                    return;

                await Navigation.PushAsync(new V_MedicoVista(item) { Title = " Medico " + item.v_Nombre });

                // Manually deselect item.
                v_lista.SelectedItem = null;
            }
            else
            {
                C_Servicios item = args.SelectedItem as C_Servicios;
                if (item == null)
                    return;

                await Navigation.PushAsync(new V_MedicoVista(item) { Title = "Lugar " + item.v_Nombre });

                // Manually deselect item.
                v_lista.SelectedItem = null;
            }
            
        }        
        /// <summary>
        /// agregar o quitar del filtro de especialidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_Args"></param>
         void Fn_TapEspec(object sender, ItemTappedEventArgs _Args)
        {
            //para mostrar un cambio en la lista la estoy haciendo null y despues volviendo a llenar
            var list = (ListView)sender;
            list.ItemsSource = null;
            var _valor =  _Args.Item as Filtro;//cast al template que ya esta preparado 
            if (!_filEspec.Contains(_valor.v_texto))
            {
                for(int i=0; i<_especialidades.Count; i++)
                {
                    if(_especialidades[i].v_texto==_valor.v_texto)
                    {
                        _especialidades[i].v_color=Color.Red;
                    }
                }
               _filEspec.Add(_valor.v_texto);
                list.ItemsSource = _especialidades;
            }
            else
            {
                for (int i = 0; i < _especialidades.Count; i++)
                {
                    if (_especialidades[i].v_texto == _valor.v_texto)
                    {
                        _especialidades[i].v_color = Color.Blue;
                    }
                }
                _filEspec.Remove(_valor.v_texto);
                list.ItemsSource = _especialidades;
            }
        }
        /// <summary>
        /// agregar o quitar del filtro de especialidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_Args"></param>
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
                        _ciudades[i].v_color = Color.Red;
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
                        _ciudades[i].v_color = Color.Blue;
                    }
                }
                _filCiud.Remove(_valor.v_texto);
                list.ItemsSource = _ciudades;
            }
        }
        public async void Fn_CargarDAtos()
        {
            if (v_medico)
            {

                //hacer una conversion de la lista que esta siendo actualizada
                //pedir los datos y hacerlos nuevos
                // hacer clear a la lsta que se esta modificando, darle los nuevos valores agregar y listview darle todo la lista creada
                HttpClient _cliente = new HttpClient();
                string url = "https://useller.com.mx/trato_especial/prueba_json.php";
                L_Error.IsVisible = true;
                L_Error.Text = "Procesando Informacion";
                try
                {
                    HttpResponseMessage _respuestphp = await _cliente.PostAsync(url, null);
                    string _respu = await _respuestphp.Content.ReadAsStringAsync();
                    ObservableCollection<C_Medico> _med = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_respu);
                    L_Error.IsVisible = false;
                    B_Filtrar.IsEnabled = true;
                    App.v_medicos = _med;
                    App.Fn_GuardarRed(App.v_medicos);
                    Orden();

                    App.Fn_ImgSexo();
                    v_lista.ItemsSource = App.v_medicos;
                }
                catch (HttpRequestException _ex)
                {
                    if (Application.Current.Properties.ContainsKey("medicos"))
                    {
                        string _json = App.Current.Properties["medicos"] as string;
                        App.v_medicos = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_json);
                        Orden();
                        App.Fn_ImgSexo();
                        if(App.v_medicos.Count>0)
                        {
                            L_Error.IsVisible = false;
                            B_Filtrar.IsEnabled = true;
                        }
                        else
                        {
                            L_Error.Text = "Error de Conexion";
                            L_Error.IsVisible = true;
                            B_Filtrar.IsEnabled = false;
                        }
                        v_lista.ItemsSource = App.v_medicos;
                    }
                    else
                    {
                        L_Error.Text = "Error de Conexion";
                        L_Error.IsVisible = true;
                        B_Filtrar.IsEnabled = false;
                        v_lista.ItemsSource = null;
                    }
                }

                //darle la nueva lista
            }
            else
            {
                //hacer una conversion de la lista que esta siendo actualizada
                //pedir los datos y hacerlos nuevos
                // hacer clear a la lsta que se esta modificando, darle los nuevos valores agregar y listview darle todo la lista creada

                //por ahora esta creando nuevoos
                Random rand = new Random();
                string _val = rand.Next(0, 120).ToString();
                App.v_servicios.Add(new C_Servicios
                {
                    v_Nombre = "Nombre lugar  nuevo" + _val,
                    v_Especialidad = "esec" + _val,
                    v_Domicilio = "dom sdsafsdfdf" + _val,
                    v_descripcion = "infoooooooooo" + _val,
                    v_img = "ICONOAPP.png"
                });// v_Descuento ="descuento "+ _val+"%",
                Orden();
                //darle la nueva lista
                v_lista.ItemsSource = App.v_servicios;
            }
            Fn_CrearCiud();
            Fn_CrearEspec();
        }
        async void Fn_Refresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Fn_CargarDAtos();
            await Task.Delay(100);
            //cancelar la actualizacion
            list.IsRefreshing = false;
            List<string> _da = new List<string>();
        }

        public void Fn_Buscar(object sender, TextChangedEventArgs e)
        {
            SearchBar _search = (SearchBar)sender;
            string _abuscar = _search.Text.ToLower(); //.StartsWith()

            if (string.IsNullOrEmpty(_abuscar) ||  string.IsNullOrWhiteSpace(_abuscar))
            {
                Buscador.ItemsSource = null;
                Buscador.IsVisible = false;
            }
            else
            {
                Buscador.ItemsSource = null;
                Buscador.IsVisible = true;
                ObservableCollection<string> _lista = new ObservableCollection<string>();
                if (v_medico)
                {
                    for(int i=0; i<App.v_medicos.Count; i++)
                    {
                        if (App.v_medicos[i].v_Nombre.ToLower().StartsWith(_abuscar) && !_lista.Contains(App.v_medicos[i].v_Nombre) )
                        {
                            _lista.Add(App.v_medicos[i].v_Nombre);
                        }
                        else if( App.v_medicos[i].v_Apellido.ToLower().StartsWith(_abuscar) && !_lista.Contains(App.v_medicos[i].v_Apellido))
                        {
                            _lista.Add(App.v_medicos[i].v_Apellido);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < App.v_servicios.Count; i++)
                    {

                    }
                }
                for (int i = 0; i <_especialidades.Count; i++)
                {
                    if (_especialidades[i].v_texto.ToLower().StartsWith(_abuscar))
                    {
                        _lista.Add(_especialidades[i].v_texto);
                    }
                }
                if(_lista.Count>0)
                {
                    Buscador.ItemsSource = _lista;
                }
                else
                {
                    Buscador.IsVisible = false;
                }
            }
        }
    }
}