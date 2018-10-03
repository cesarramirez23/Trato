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

using Android.Media;
using Android.Support.V7.App;
using Android.Util;
using Firebase.Messaging;
using Android.Graphics;

using Newtonsoft.Json;
using Trato.Varios;



namespace Trato.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseNotificationService : FirebaseMessagingService
    {


        const string TAG = "FirebaseNotificationService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            /* en firebase copnsole,   al enviar el mensaje, hasta abajo en adavanced option-> custom data->key de message   
             es para recibir esta info de abajo   se envia como data   en el archivo /firebase/index.php */


            foreach (string key in message.Data.Keys)
            {
                Console.WriteLine("keysessss " + key);
            }
            Console.WriteLine("total keysssss " + message.Data.Keys.Count);
               

            if (message.Data.ContainsKey("data"))
            {
                string json = message.Data["data"];
                if(string.IsNullOrEmpty( json))
                {
                    string _title = "";
                    string _mess = "";

                    if (string.IsNullOrEmpty(message.Data["title"]))
                    {
                        _title = "Titulo vacio";
                    }
                    else
                    {
                        _title = message.Data["title"];
                    }
                    if (string.IsNullOrEmpty(message.Data["message"]))
                    {
                        _mess = "Mensaje vacio";
                    }
                    else
                    {
                        _mess = message.Data["message"];
                    }

                    SendNotification(_title, _mess);
                }
                else
                {
                    Notifi _notif = JsonConvert.DeserializeObject<Notifi>(json);
                    SendNotification(_notif.v_titulo, _notif.v_cuerpo);
                }
            }
            else
            {//siempre trae esto
                string _title = "";
                string _mess = "";

                if (string.IsNullOrEmpty( message.Data["title"]))
                {
                    _title = "Titulo vacio";
                }
                else
                {
                    _title = message.Data["title"];
                }
                if (string.IsNullOrEmpty(message.Data["message"]))
                {
                    _mess = "Mensaje vacio";
                }
                else
                {
                    _mess = message.Data["message"];
                }

                SendNotification(_title,_mess);
            }
        }

        void SendNotification(string messageBody, string _titulo)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            Bitmap largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Ambulancia);

            var notificationBuilder = new Android.Support.V4.App.NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.Ambulancia)
                .SetLargeIcon(largeIcon)
                .SetContentTitle(_titulo)
                .SetContentText(messageBody)
                .SetContentIntent(pendingIntent)
                .SetColor(40150209)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetPriority(1)
                .SetAutoCancel(true);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}