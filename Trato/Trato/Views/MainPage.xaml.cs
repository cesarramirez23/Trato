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
            perfil.Text = "Perfil " + App.v_perfil.Fn_GetDatos();
            fol.Text = "FOlio " +Application.Current.Properties["log"];// App.v_folio;
            membre.Text = " Membresia " + App.v_membresia;
            string s_tr = "";
            s_tr = "count " + Application.Current.Properties.Count ;
            System.Collections.Generic.List<string> keyList = new System.Collections.Generic.List<string>(Application.Current.Properties.Keys);

            for(int i=0; i< keyList.Count; i++)
            {
                s_tr += "\n key " + keyList[i];
            }
            log.Text = s_tr;

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
            await Navigation.PushAsync(new NavigationPage(new V_Informacion(1)));
        }
    }
}