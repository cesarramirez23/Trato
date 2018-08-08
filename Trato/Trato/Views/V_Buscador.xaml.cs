﻿using System;
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

            _especialidades.Add(new Filtro("Espec"));//, v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espec2", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe3", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe4", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe6", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe5", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe7", v_color = Color.Blue });
            _especialidades.Add(new Filtro{ v_texto="Espe8", v_color = Color.Blue });
            filEspc.ItemsSource = _especialidades;
            
            _ciudades.Add(new Filtro { v_texto = "ciud1", v_color = Color.Blue });
            _ciudades.Add(new Filtro { v_texto = "ciud2", v_color = Color.Blue });
            _ciudades.Add(new Filtro { v_texto = "ciud3", v_color = Color.Blue });
            _ciudades.Add(new Filtro { v_texto = "ciud4", v_color = Color.Blue });
            _ciudades.Add(new Filtro { v_texto = "ciud5", v_color = Color.Blue });
            _ciudades.Add(new Filtro { v_texto = "ciud6", v_color = Color.Blue });
            filCiudad.ItemsSource = _ciudades;

            overlay.IsVisible = v_filtro;
            if(_valor)
            {
                v_medico = true;
                Orden();
                v_lista.ItemsSource = App.v_medicos;
                
            }
            else
            {
                v_medico = false;
                Orden();
                v_lista.ItemsSource = App.v_servicios;
            }
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

        }
        public async void Fn_Aceptar(object sender, EventArgs _args)
        {
            if(v_medico)
            {
                ObservableCollection<C_Medico> _filtrada = new ObservableCollection<C_Medico>();
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
            Orden();
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
                Orden();
                //darle la nueva lista
                list.ItemsSource = App.v_servicios;
            }
            await Task.Delay(100);
            //cancelar la actualizacion
            list.IsRefreshing = false;
        }
    }
}