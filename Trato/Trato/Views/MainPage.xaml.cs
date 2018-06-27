using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
        public async void Fn_Log(object sender, EventArgs _args)
        {
            //Page newPage = new V_Login();
            //Application.Current.MainPage =new NavigationPage( new V_Login());
            await Navigation.PushAsync(new NavigationPage(new V_Login()) { Title = "Login" });
        }
        public async void Fn_Reg(object sender, EventArgs _args)
        {
            //Application.Current.MainPage = new NavigationPage(new  V_Registro());
            await Navigation.PushAsync(new NavigationPage(new V_Membresias()) { Title = "Membresias" });
        }
        public async void Fn_Info(object sender, EventArgs _args)
        {

            //Application.Current.MainPage = new NavigationPage(new V_Informacion());
            await Navigation.PushAsync(new NavigationPage(new V_Informacion(1)) { Title = "Informacion" });
        }
    }
}