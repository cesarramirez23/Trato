using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//
using System.Collections.ObjectModel;// para usar las listas   ObservableCollection
using Trato.Personas;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LISTA : ContentPage
	{
        public ObservableCollection<C_Medico> _Medicos { get; set; }            
       // public ObservableCollection<VeggieViewModel> _lista { get; set; }
        public LISTA ()
		{
            InitializeComponent();
            Title = "Titulo";
            _Medicos = new ObservableCollection<C_Medico>();
            _Medicos.Add(new C_Medico { v_Nombre = "nom1",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom2",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom3",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom4",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom5",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom6",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom7",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom8",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom9",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom10",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom11",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom12",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom13",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom14",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            _Medicos.Add(new C_Medico { v_Nombre = "nom15",  v_Especialidad = "esps1",  v_Domicilio = "domicillio sadsadsad", v_Info = "sadsadasfsafdsdsgdgsgdsgdsgd"});
            /*
            _lista = new ObservableCollection<VeggieViewModel>();
            _lista.Add(new VeggieViewModel { Name = "Tomato", Type = "Fruit", Image = "tomato.png" });
            _lista.Add(new VeggieViewModel { Name = "Romaine Lettuce", Type = "Vegetable", Image = "lettuce.png" });
            _lista.Add(new VeggieViewModel { Name = "Zucchini", Type = "Vegetable", Image = "zucchini.png" });
            _lista.Add(new VeggieViewModel { Name = "Tomato", Type = "Fruit", Image = "tomato.png" });
            _lista.Add(new VeggieViewModel { Name = "Romaine Lettuce", Type = "Vegetable", Image = "lettuce.png" });
            _lista.Add(new VeggieViewModel { Name = "Zucchini", Type = "Vegetable", Image = "zucchini.png" }); _lista.Add(new VeggieViewModel { Name = "Tomato", Type = "Fruit", Image = "tomato.png" });
            _lista.Add(new VeggieViewModel { Name = "Romaine Lettuce", Type = "Vegetable", Image = "lettuce.png" });
            _lista.Add(new VeggieViewModel { Name = "Zucchini", Type = "Vegetable", Image = "zucchini.png" }); _lista.Add(new VeggieViewModel { Name = "Tomato", Type = "Fruit", Image = "tomato.png" });
            _lista.Add(new VeggieViewModel { Name = "Romaine Lettuce", Type = "Vegetable", Image = "lettuce.png" });
            _lista.Add(new VeggieViewModel { Name = "Zucchini", Type = "Vegetable", Image = "zucchini.png" }); _lista.Add(new VeggieViewModel { Name = "Tomato", Type = "Fruit", Image = "tomato.png" });
            _lista.Add(new VeggieViewModel { Name = "Romaine Lettuce", Type = "Vegetable", Image = "lettuce.png" });
            _lista.Add(new VeggieViewModel { Name = "Zucchini", Type = "Vegetable", Image = "zucchini.png" }); _lista.Add(new VeggieViewModel { Name = "Tomato", Type = "Fruit", Image = "tomato.png" });
            _lista.Add(new VeggieViewModel { Name = "Romaine Lettuce", Type = "Vegetable", Image = "lettuce.png" });
            _lista.Add(new VeggieViewModel { Name = "Zucchini", Type = "Vegetable", Image = "zucchini.png" });*/
            v_lista.ItemsSource = _Medicos;


        }
        async void  Fn_Select(object sender, SelectedItemChangedEventArgs args)
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
            _Medicos.Add(new C_Medico { v_Nombre = "nombre nuevo" + _val, v_Especialidad = "esec" + _val, v_Domicilio = "dom sdsafsdfdf" + _val, v_Info = "infoooooooooo" + _val });
            //darle la nueva lista
            list.ItemsSource = _Medicos;
            //cancelar la actualizacion
            list.IsRefreshing = false;
        }
	}
}