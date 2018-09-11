using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//para las notificaciones
using Microsoft.WindowsAzure.MobileServices;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;


namespace Trato.Droid
{
    //label es el nombre que se va a ver en pantalla de aplicaciones 
    [Activity(Label = "Trato Especial", Icon = "@drawable/Logo_Redondo_512", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MobileServiceClient MobileService = new MobileServiceClient( "https://tratoespecial2.azurewebsites.net" );
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

          
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            LoadApplication(new App());
            CurrentPlatform.Init();
            TodoItem item = new TodoItem { Name= "Awesome item" };
             MobileService.GetTable<TodoItem>().InsertAsync(item);
        }
       
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
    }
}

