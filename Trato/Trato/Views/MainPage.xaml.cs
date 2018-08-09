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
    }
}