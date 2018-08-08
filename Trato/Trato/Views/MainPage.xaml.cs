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

            App.Fn_CargarDatos();
            perfil.Text = "Perfil " + (Application.Current.Properties["perfGen"] as Personas.C_PerfilGen).Fn_GetDatos();
            fol.Text = "FOlio " +Application.Current.Properties["folio"];// App.v_folio;
            membre.Text = " Membresia " + Application.Current.Properties["membre"];
            string s_tr = "";
            s_tr = Application.Current.Properties["log"]+ "  count " + Application.Current.Properties.Count ;
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