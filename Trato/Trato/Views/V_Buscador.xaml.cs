using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Trato.Personas;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Buscador : ContentPage
	{
        List<VisualElement> _arr = new List<VisualElement>();
        int  num_filtro=-1;
		public V_Buscador ()
		{
			InitializeComponent ();
            _arr.Add(v_nom);
            _arr.Add(v_espe);
            _arr.Add(v_ciu);
            Fn_CambioFiltro();
            v_lista.ItemsSource = App.v_medicos;
        }
        public void Fn_Filtro(object sender, EventArgs _Args)
        {
            num_filtro = v_filtro.SelectedIndex;
            Fn_CambioFiltro();
        }
        void Fn_CambioFiltro()
        {
            text.Text = "" + _arr.Count; 
            

            //v_nom.IsVisible = false;
            //v_espe.IsVisible = false;
            //v_ciu.IsVisible = false;
            for(int i=0; i<_arr.Count; i++)
            {
                _arr[i].IsVisible = false;
            }
            if(num_filtro<3  && num_filtro>-1)
            {
                _arr[num_filtro].IsVisible = true;
            }
            text.Text = "" + _arr.Count;

        }
        async void Fn_Select(object sender, SelectedItemChangedEventArgs args)
        {
            C_Medico item = args.SelectedItem as C_Medico;
            if (item == null)
                return;

            await App.Current.MainPage.Navigation.PushAsync(new V_MedicoVista(item) { Title = " Medico " + item.v_Nombre });

            // Manually deselect item.
            v_lista.SelectedItem = null;
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
            App.v_medicos.Add(new C_Medico { v_Nombre = "nombre nuevo" + _val, v_Especialidad = "esec" + _val, v_Domicilio = "dom sdsafsdfdf" + _val, v_Info = "infoooooooooo" + _val });
            //darle la nueva lista
            list.ItemsSource = App.v_medicos;
            //cancelar la actualizacion
            list.IsRefreshing = false;
        }

    }
}