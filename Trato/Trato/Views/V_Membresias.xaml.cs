using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Membresias : ContentPage
    {
        VisualElement v_objMedio;
        Rectangle v_boundMedio = new Rectangle();
        string[] textosInfo = { "sadsadsadsadsadsa", "fgfhgjhgjhgjhgjhgj", "popipoipoiopipoipoiop" };
        protected override void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public V_Membresias()
        {
            InitializeComponent();
            v_objMedio = Familiar;
            v_boundMedio = Familiar.Bounds ;
        }
        void Fn_CambioText(int _valor )
        {
            label.Text = textosInfo[_valor];
        }
        public async void Fn_Cambio(object sender, EventArgs _args)
        {
            VisualElement _tocado = (VisualElement)sender;
            //si es el mismo
            if(_tocado == v_objMedio)
            {
                return;
            }
            else
            {
                Rectangle _copiaMed = v_objMedio.Bounds;//copio del que esta en medio
                Rectangle _copianuevo = _tocado.Bounds;//copio del qu esta tocando
                AbsoluteLayout.SetLayoutBounds(v_objMedio, _copianuevo);
                AbsoluteLayout.SetLayoutBounds(_tocado, _copiaMed);
                v_objMedio = _tocado;
            }
        }
        public async void Fn_Comprar(object _sender, EventArgs _args)
        {
            await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new V_Registro(false)) { Title = "Registro" });
        }
    }
}