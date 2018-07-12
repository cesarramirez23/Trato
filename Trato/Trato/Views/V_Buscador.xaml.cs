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

        List<Label> _filEsp = new List<Label>();
        
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

        void Fn_Fil()
        {
            //if(v_medico)
            //{
            //    ObservableCollection<C_Medico> _temp = new ObservableCollection<C_Medico>(); 
            //    for(int i=0; i<App.v_medicos.Count; i++)
            //    {
            //        string _ciud = App.v_medicos[i].v_Domicilio.Split(' ')[5];
            //        if (App.v_medicos[i].v_Nombre == v_nom.Text ||
            //            _ciud == v_ciu.SelectedItem.ToString() ||//5
            //            App.v_medicos[i].v_Nombre == v_espe.Text)
            //        {
            //            _temp.Add(App.v_medicos[i]);
            //        }
            //    }
            //}

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

                await Navigation.PushAsync(new V_MedicoVista(item) { Title = " Medico " + item.v_Nombre });

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
            for(int i=0; i< _filEsp.Count; i++)
            {
                if(_filEsp[i].Text == _temp.v_Especialidad)
                {
                    _ret = true;
                }
            }
            return _ret;
        }
        void Fn_Tap(object sender, EventArgs _Args)
        {
            var _valor=(Label)sender;
           
            if(!_filEsp.Contains(_valor))
            {
                _valor.TextColor = Color.Red;
                _filEsp.Add(_valor);
            }
            else
            {
                _valor.TextColor = Color.White;
                _filEsp.Remove(_valor);
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
                v_Domicilio = "dom sdsafsdfdf" + _val, v_Info = "infoooooooooo" + _val , v_img="menu_icon.png"});

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
                App.v_servicios.Add(new C_Servicios { v_Nombre = "Aombre nuevo" + _val + " " + "fpell" + _val, v_Servicios = "esec" + _val,
                    v_Domicilio = "dom sdsafsdfdf" + _val, v_Info = "infoooooooooo" + _val, v_Descuento ="descuento "+ _val+"%",
                    v_img = "menu_icon.png"
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