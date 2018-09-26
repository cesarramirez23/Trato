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
using Firebase;
using Android.Util;
using Android.Gms.Common;
using PayPal.Forms.Abstractions;
using PayPal.Forms;
using Android.Content;

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
            //paypal AQrrFs8D-iSCYiAEEmO9ni3CcQ7GjgjPqSBBVhxNTmRKOnLR_Ol_qRcy2Pr4yxhwcQ2BK1BoZbzl0Hka
            var config = new PayPalConfiguration(PayPalEnvironment.Sandbox, "AVART2W6j2cnNhmWej6EcQjx_ytsVpl1hmnArzHtVWSsZFRVAWOlZq6y3EjPFM0FHUhG_yrvkftXAAtN")
            {
                //If you want to accept credit cards
                AcceptCreditCards = false,
                //Your business name
                MerchantName = "Tienda",
                //Your privacy policy Url
                MerchantPrivacyPolicyUri = "https://www.useller.com.mx/aviso_privacidad",
                //Your user agreement Url
                MerchantUserAgreementUri = "https://www.useller.com.mx/terminos",
                // OPTIONAL - ShippingAddressOption (Both, None, PayPal, Provided)
                ShippingAddressOption = ShippingAddressOption.Both,
                // OPTIONAL - Language: Default languege for PayPal Plug-In
                Language = "es",
                // OPTIONAL - PhoneCountryCode: Default phone country code for PayPal Plug-In
                PhoneCountryCode = "52",
            };
            CrossPayPalManager.Init(config, this);


            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            LoadApplication(new App());
            CurrentPlatform.Init();
            TodoItem item = new TodoItem { Name= "Awesome item" };
            MobileService.GetTable<TodoItem>().InsertAsync(item);
            CheckForGoogleServices();

            //FirebaseApp.InitializeApp(this.ApplicationContext);
            //Java.Lang.IllegalStateException: Default FirebaseApp 
            //is not initialized in this process com.alsain.trato4.
            //Make sure to call FirebaseApp.initializeApp(Context) first.
            //App.Fn_SetToken(FirebaseInstanceId.Instance.Token);
        }
       
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool CheckForGoogleServices()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Toast.MakeText(this, GoogleApiAvailability.Instance.GetErrorString(resultCode), ToastLength.Long);
                }
                else
                {
                    Toast.MakeText(this, "This device does not support Google Play Services", ToastLength.Long);
                }
                return false;
            }
            return true;
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            PayPalManagerImplementation.Manager.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnDestroy()
        { 
            //PayPalManagerImplementation.Manager.Destroy();
            base.OnDestroy();
        }
    }
}

