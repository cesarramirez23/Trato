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
    public partial class V_Membresias : CarouselPage
    {
        public V_Membresias()
        {
            InitializeComponent();
        }
        public async void Fn_Comprar(object _sender, EventArgs _args)
        {
            //await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new V_Registro(false)) { Title = "Registro" });
            await Navigation.PushAsync(new V_Registro(false));
        }
    }
}