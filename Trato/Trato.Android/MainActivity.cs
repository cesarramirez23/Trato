using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//para las notificaciones
using Firebase.Messaging;
using Firebase.Iid;
using Firebase;
using Android.Util;
using Android.Gms.Common;
using PayPal.Forms.Abstractions;
using PayPal.Forms;
using Android.Content;
using Plugin.Permissions;
/*
https://stackoverflow.com/questions/40181654/firebase-fcm-open-activity-and-pass-data-on-notification-click-android/40185654
*/
namespace Trato.Droid
{
    //label es el nombre que se va a ver en pantalla de aplicaciones 
    [Activity(Label = "Trato Especial", Icon = "@drawable/Logo_Redondo_512", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

          
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            //paypal produccion AQrrFs8D-iSCYiAEEmO9ni3CcQ7GjgjPqSBBVhxNTmRKOnLR_Ol_qRcy2Pr4yxhwcQ2BK1BoZbzl0Hka
            //paypal sandbox  AVART2W6j2cnNhmWej6EcQjx_ytsVpl1hmnArzHtVWSsZFRVAWOlZq6y3EjPFM0FHUhG_yrvkftXAAtN
            var config = new PayPalConfiguration(PayPalEnvironment.Production, "AQrrFs8D-iSCYiAEEmO9ni3CcQ7GjgjPqSBBVhxNTmRKOnLR_Ol_qRcy2Pr4yxhwcQ2BK1BoZbzl0Hka")
            //var config = new PayPalConfiguration(PayPalEnvironment.Sandbox, "AVART2W6j2cnNhmWej6EcQjx_ytsVpl1hmnArzHtVWSsZFRVAWOlZq6y3EjPFM0FHUhG_yrvkftXAAtN")
            {
                //If you want to accept credit cards
                AcceptCreditCards = false,
                //Your business name
                MerchantName = "Tienda",
                //Your privacy policy Url
                MerchantPrivacyPolicyUri = "http://tratoespecial.com/politicas-de-privacidad/",
                //Your user agreement Url
                MerchantUserAgreementUri = "http://tratoespecial.com/terminos-y-condiciones/",
                // OPTIONAL - ShippingAddressOption (Both, None, PayPal, Provided)
                ShippingAddressOption = ShippingAddressOption.Both,
                // OPTIONAL - Language: Default languege for PayPal Plug-In
                Language = "es",
                // OPTIONAL - PhoneCountryCode: Default phone country code for PayPal Plug-In
                PhoneCountryCode = "52",
            };
            CrossPayPalManager.Init(config, this);

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);//para pedir los permisos cross plat
            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            LoadApplication(new App());
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
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

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

