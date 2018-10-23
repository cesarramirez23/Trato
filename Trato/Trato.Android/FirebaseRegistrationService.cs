using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
//lo que dice el tutorial   https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-xamarin-forms-get-started-push
using System.Threading.Tasks;
using Android.Util;
using Firebase.Iid;


namespace Trato.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseRegistrationService : FirebaseInstanceIdService
    {
        const string TAG = "FirebaseRegistrationService";

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            Console.WriteLine("Refreshed token: " + refreshedToken);
            App.Fn_SetToken(refreshedToken);
            /*TOKEN 22/10/2018
             * SXz8GAZVcU:APA91bEc1eHJUmWRiWMahRYUmObMl-8PM_dVoyXqxBb3rx9IdAbM8nCJXupKp4QQuT_PhHwMLHkJHYplyMwjK065Ra1kvzz0h3LvQA5ObcX8ix0ITAG73cP_JploOHsQTaVlhkpk6lv0
             * 
             * HACE EL REFRESH CUANDO SE INSTALA Y TAMBIEN CUANDO BORRAS LOS DATOS DESDE EL TELEFONO,
             * NOOOOO CUANDO SE HACE DESDE ACA EL CLEAR PROPERTIES
             * 
             * 
             * 
            guardar este token, al hacer la cita mandar a base mi propio toke y los datos del doctor,
            el doctor debe tenr su propio token , entonces mandarle la notif al doctor junto con el json del paciente y su token
            aceptar o no se reenvia el mensaje al topke delpaciente
            */
        }

    }
}