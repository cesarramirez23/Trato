using System;
using Xamarin.Forms;
using Trato.Views;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Collections.ObjectModel;// para las listas
[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Trato
{
    public partial class App : Application
    {
       

        #region Propias de la app
        public App()
        {
            InitializeComponent();
        }
        protected override void OnStart()
        {    //existe la variable guardada
            Properties.Clear();
            Current.MainPage = new Inicio();
        }
        protected override void OnSleep() { }
        protected override void OnResume() { }
        #endregion

    }
}
