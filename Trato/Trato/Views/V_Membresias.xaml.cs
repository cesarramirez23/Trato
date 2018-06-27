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
        protected override void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public V_Membresias()
        {
            InitializeComponent();
        }
        public async void Fn_Comprar(object sender, EventArgs _args)
        {
            await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new V_Registro(false)) { Title = "Registro" });
        }
        void Fn_Individual(object sender, EventArgs _args)
        {
            grid.IsVisible = false;
            StackInfo.IsVisible = true;
            info.Text = "Individual" +
                "\ndsasfcdggdf" +
                "\ntytfuytu" +
                "\n6345324" +
                "\nsdxzcxx" +
                "\ngvbh" +
                "\nj" +
                "\nyuo" +
                "\n80";
        }
        void Fn_Familiar(object sender, EventArgs _args)
        {
            info.Text = "familiar!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" +
                "\nsasdsafdsgdsg" +
                "d\nfasfsdafsdaf";
            grid.IsVisible = false;
            StackInfo.IsVisible = true;
        }
        void Fn_Empresarial(object sender, EventArgs _args)
        {
            info.Text = "empresarial!!!!!!!!!!!!!" +
                "\nsadsafsdgdhggjhj" +
                "\niuiopio" +
                "urtyret" +
                "\nqweqwegf" +
                "\nvbvnhj";
            grid.IsVisible = false;
            StackInfo.IsVisible = true;
        }
    }
}