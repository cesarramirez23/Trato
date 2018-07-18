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

namespace Trato.Views
{
    public class Filtro
    {
       public string v_texto { get; set; }
       public Color v_color { get; set; }

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

            _especialidades.Add(new Filtro{ v_texto="Espec", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espec2", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe3", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe4", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe6", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe5", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe7", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe8", v_color = Color.Blue });
            filEspc.ItemsSource = _especialidades;
            
            _ciudades.Add(new Filtro { v_texto = "ciud1", v_color = Color.Pink });
            _ciudades.Add(new Filtro { v_texto = "ciud2", v_color = Color.Pink });
            _ciudades.Add(new Filtro { v_texto = "ciud3", v_color = Color.Pink });
            _ciudades.Add(new Filtro { v_texto = "ciud4", v_color = Color.Pink });
            _ciudades.Add(new Filtro { v_texto = "ciud5", v_color = Color.Pink });
            _ciudades.Add(new Filtro { v_texto = "ciud6", v_color = Color.Pink });
            filCiudad.ItemsSource = _ciudades;

            overlay.IsVisible = v_filtro;
            if(_valor)
            {
                v_medico = true;
              //  Orden();
                v_lista.ItemsSource = App.v_medicos;
                
            }
            else
            {
                v_medico = false;
               // Orden();
                v_lista.ItemsSource = App.v_servicios;
            }
            Orden();
        }


        public void Fn_Cancelar(object sender, EventArgs _Args)
        {
            _filCiud.Clear();
            _filEspec.Clear();

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
        public void Fn_Aceptar(object sender, EventArgs _args)
        {
            if(v_medico)
            {
                List<C_Medico> _filtrada = new List<C_Medico>();
                //recorre toda la lista de medicos
                for(int i=0; i<App.v_medicos.Count; i++)
                {
                    //recorre lista de ciudad a filtrar
                    for(int j=0; j<_filCiud.Count; j++)
                    {
                        if(App.v_medicos[i].v_Ciudad==_filCiud[j] && !_filtrada.Contains(App.v_medicos[i]))
                        {
                            _filtrada.Add(App.v_medicos[i]);
                        }
                    }
                    //recorre lista de especialidad a filtrar
                    for (int j = 0; j < _filEspec.Count; j++)
                    {
                        if (App.v_medicos[i].v_Especialidad == _filEspec[j] && !_filtrada.Contains(App.v_medicos[i]))
                        {
                            _filtrada.Add(App.v_medicos[i]);
                        }
                    }
                }
                v_lista.ItemsSource = _filtrada;
            }
            else
            {
                List<C_Servicios> _filtrada = new List<C_Servicios>();
                //recorre toda la lista de medicos
                for (int i = 0; i < App.v_medicos.Count; i++)
                {
                    //recorre lista de ciudad a filtrar
                    for (int j = 0; j < _filCiud.Count; j++)
                    {
                        if (App.v_medicos[i].v_Ciudad == _filCiud[j] && !_filtrada.Contains(App.v_servicios[i]))
                        {
                            _filtrada.Add(App.v_servicios[i]);
                        }
                    }
                    //recorre lista de especialidad a filtrar
                    for (int j = 0; j < _filEspec.Count; j++)
                    {
                        if (App.v_medicos[i].v_Especialidad == _filEspec[j] && !_filtrada.Contains(App.v_servicios[i]))
                        {
                            _filtrada.Add(App.v_servicios[i]);
                        }
                    }
                }
                v_lista.ItemsSource = _filtrada;
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
            
           // var id = (int)((Switch)sender).BindingContext;
            v_filtro = !v_filtro;

            overlay.IsVisible = v_filtro;
        }
        async Task Orden()
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
        async void Fn_TapEspec(object sender, ItemTappedEventArgs _Args)
        {
            var _valor =  _Args.Item as Filtro;// este es el texto que trae escrito
            if (!_filEspec.Contains(_valor.v_texto))
            {
                for(int i=0; i<_especialidades.Count; i++)
                {
                    if(_especialidades[i].v_texto==_valor.v_texto)
                    {
                        _especialidades[i].v_color = Color.Red;
                        await DisplayAlert("titu", "enconttrado en " + i, "nada");
                    }
                }
               _filEspec.Add(_valor.v_texto);
                filEspc.ItemsSource = _especialidades;
            }
            else
            {
                //_especialidades.Find(x => x.v_texto.Contains(_valor.v_texto)).v_color = Color.Blue;
                _filEspec.Remove(_valor.v_texto);
                filEspc.ItemsSource = _especialidades;
            }
        }
        /// <summary>
        /// agregar o quitar del filtro de especialidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_Args"></param>
        void Fn_TapCiu(object sender, SelectedItemChangedEventArgs _Args)
        {
            var _valor = (TextCell)sender;

            if (!_filCiud.Contains(_valor.Text))
            {
                _valor.TextColor = Color.Red;
                _filCiud.Add(_valor.Text);
            }
            else
            {
                _valor.TextColor = Color.White;
                _filCiud.Remove(_valor.Text);
            }
        }
        async void Fn_Refresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            if(v_medico)
            {
            //hacer una conversion de la lista que esta siendo actualizada
            //pedir los datos y hacerlos nuevos
            // hacer clear a la lsta que se esta modificando, darle los nuevos valores agregar y listview darle todo la lista creada

            //por ahora esta creando nuevoos
            Random rand = new Random();
            string _val = rand.Next(0, 120).ToString();
            App.v_medicos.Add(new C_Medico { v_Nombre = "Aombre nuevo" + _val,v_Ciudad= "ciud4", v_Apellido =" apellido "+_val, v_Especialidad = "Espec2" + _val,
                v_Domicilio = "dom sdsafsdfdf" + _val, v_descripcion = "infoooooooooo" + _val , v_img="ICONOAPP.png"});

                await Orden();
            //darle la nueva lista
            list.ItemsSource = App.v_medicos;
            }
            else
            {
                //hacer una conversion de la lista que esta siendo actualizada
                //pedir los datos y hacerlos nuevos
                // hacer clear a la lsta que se esta modificando, darle los nuevos valores agregar y listview darle todo la lista creada

                //por ahora esta creando nuevoos
                Random rand = new Random();
                string _val = rand.Next(0, 120).ToString();
                App.v_servicios.Add(new C_Servicios { v_Nombre = "Nombre lugar  nuevo" + _val ,
                    v_Especialidad = "esec" + _val, v_Domicilio = "dom sdsafsdfdf" + _val, v_descripcion = "infoooooooooo" + _val,
                    v_img = "ICONOAPP.png"
                });// v_Descuento ="descuento "+ _val+"%",

                await Orden();
                //darle la nueva lista
                list.ItemsSource = App.v_servicios;
            }

            

            //cancelar la actualizacion
            list.IsRefreshing = false;
        }

    }
}