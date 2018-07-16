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
        /// filtro de especialidad
        /// </summary>
        List<string> _filEspec = new List<string>();
        List<string> _filCiud = new List<string>();

        List<string> _especialidades = new List<string>();
        List<string> _ciudades = new List<string>();

        public V_Buscador()
        {
            InitializeComponent();
            overlay.IsVisible = v_filtro;

        }
        public V_Buscador(bool _valor)
        {
            InitializeComponent();

            _especialidades.Add("sadsad");
            _especialidades.Add("poiu");
            _especialidades.Add("retret");
            _especialidades.Add("piutrrr");
            _especialidades.Add("asdt");
            _especialidades.Add("yutyuytet");
            _especialidades.Add("piutrrr");
            _especialidades.Add("poiu");
            filEspc.ItemsSource = _especialidades;
            
            _ciudades.Add("sadsad");
            _ciudades.Add("yutyuytet");
            _ciudades.Add("dfgfdgfd");
            _ciudades.Add("cxvcxv");
            _ciudades.Add("ewrwe");
            _ciudades.Add("fgdfgfdg");
            _ciudades.Add("yutyuytet");
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
        }

        /// <summary>
        /// activar/Desactivar la pantalla de los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_Args"></param>
        public void Fn_Filtro(object sender, EventArgs _Args)
        {
            //sacar el id para saber si es aceptar o cancelar
            var _id =  ((Button)sender).Id;
            
           // var id = (int)((Switch)sender).BindingContext;
            v_filtro = !v_filtro;

            overlay.IsVisible = v_filtro;
        }
        async Task Orden()
        {
            if(v_medico)
            {
                //  List<C_Medico> temp = App.v_medicos.OrderBy(x => x.v_Nombre).ToList(); ;
                IEnumerable<C_Medico> _temp=      App.v_medicos.OrderBy(x => x.v_Nombre);
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
        bool Fn_Ciudad(C_Medico _temp)
        {
            return true;
        }


        bool Fn_Espec(C_Medico _temp)
        {
            bool _ret=false;
            for(int i=0; i< _filEspec.Count; i++)
            {
                if(_filEspec[i] == _temp.v_Especialidad)
                {
                    _ret = true;
                }
            }
            return _ret;
        }
        /// <summary>
        /// agregar o quitar del filtro de especialidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_Args"></param>
        void Fn_TapEspec(object sender, SelectedItemChangedEventArgs _Args)
        {
            var _valor =(ListView)sender;
           
            //if(!_filEspec.Contains(_valor.Text))
            //{
            //    _valor.TextColor = Color.Red;
            //    _filEspec.Add(_valor.Text);
            //}
            //else
            //{
            //    _valor.TextColor = Color.White;
            //    _filEspec.Remove(_valor.Text);
            //}
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
            App.v_medicos.Add(new C_Medico { v_Nombre = "Aombre nuevo" + _val+" "+"fpell" +_val, v_Especialidad = "esec" + _val,
                v_Domicilio = "dom sdsafsdfdf" + _val, v_Info = "infoooooooooo" + _val , v_img="ICONOAPP.png"});

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
                App.v_servicios.Add(new C_Servicios { v_Nombre = "Aombre nuevo" + _val + " " + "fpell" + _val,
                    v_Especialidad = "esec" + _val, v_Domicilio = "dom sdsafsdfdf" + _val, v_Info = "infoooooooooo" + _val,
                    v_Descuento ="descuento "+ _val+"%", v_img = "ICONOAPP.png"
                });

                await Orden();
                //darle la nueva lista
                list.ItemsSource = App.v_servicios;
            }

            

            //cancelar la actualizacion
            list.IsRefreshing = false;
        }

    }
}