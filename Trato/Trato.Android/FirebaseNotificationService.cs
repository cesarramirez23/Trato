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



namespace Trato.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseNotificationService : FirebaseMessagingService
    {
      

        const string TAG = "FirebaseNotificationService";

        public override void OnMessageReceived(RemoteMessage message)
        {
           // Log.Debug(TAG, "From: " + message.From);


            /*
       en firebase copnsole,   al enviar el mensaje, hasta abajo en adavanced option->
       custom data->key de message   es para recibir esta info de abajo
       */
            // Pull message body out of the template
            //var messageBody = message.Data["message"];
            //if (string.IsNullOrWhiteSpace(messageBody))
            //    return;

            //var messageTitle = message.Data["titulo"];
            //if (string.IsNullOrWhiteSpace(messageBody))
            //    return;

            var extra1 = message.Data["extra"];
            if (string.IsNullOrWhiteSpace(extra1))
                return;

            var extra2 = message.Data["extra2"];
            if (string.IsNullOrWhiteSpace(extra1))
                return;

            // Log.Debug(TAG, "Notification message body: " + messageBody);
            //SendNotification(messageBody, extra1);
            SendNotification(extra1, extra2);
        }

        void SendNotification(string messageBody,string _titulo)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new Android.Support.V4.App.NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.Ambulancia)
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