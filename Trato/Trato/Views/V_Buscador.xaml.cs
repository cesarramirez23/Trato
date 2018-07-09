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
                v_lista.ItemsSource = App.v_medicos;
            }
            else
            {
                v_medico = false;
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
        void Orden()
        {
            C_Medico _tem = new C_Medico();
            for (int i = 1; i < App.v_medicos.Count; i++)
            {
                for (int j = 0; j < App.v_medicos.Count - i; j++)
                {
                    if (App.v_medicos[j].v_Apellido[0] > App.v_medicos[j + 1].v_Apellido[0])
                    {
                        _tem = App.v_medicos[j + 1];
                        App.v_medicos[j + 1] = App.v_medicos[j];
                        App.v_medicos[j] = _tem;
                    }
                }
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
            C_Medico item = args.SelectedItem as C_Medico;
            if (item == null)
                return;

            await App.Current.MainPage.Navigation.PushAsync(new V_MedicoVista(item) { Title = " Medico " + item.v_Nombre });

            // Manually deselect item.
            v_lista.SelectedItem = null;
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
        void Fn_Refresh(object sender, EventArgs e)
        {

            //hacer una conversion de la lista que esta siendo actualizada
            var list = (ListView)sender;
            //pedir los datos y hacerlos nuevos
            // hacer clear a la lsta que se esta modificando, darle los nuevos valores agregar y listview darle todo la lista creada

            //por ahora esta creando nuevoos
            Random rand = new Random();
            string _val = rand.Next(0, 120).ToString();
            App.v_medicos.Add(new C_Medico { v_Nombre = "Aombre nuevo" + _val+" "+"fpell" +_val, v_Especialidad = "esec" + _val, v_Domicilio = "dom sdsafsdfdf" + _val, v_Info = "infoooooooooo" + _val });
          
            //darle la nueva lista
            list.ItemsSource = App.v_medicos;
            //cancelar la actualizacion
            list.IsRefreshing = false;
        }

    }
}