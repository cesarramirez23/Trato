using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.App;


   //android.support.v4.app.NotificationCompat.Builder
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
            /* 
             * USUARIO ELIGE EL DIA Y LA HORA, ESO SE LE ENVIA AL MEDICO,
             * MEDICO RECIBE LA CITA, SI ACEPTA
             * 
             * en firebase copnsole,   al enviar el mensaje, hasta abajo en adavanced option-> custom data->key de message   
             es para recibir esta info de abajo   se envia como data   en el archivo /firebase/index.php */

            /*DESDE CONSOLE FIREBASE   SE MANDA COMO EXTRA->TU LE PONES EL KEY Y SU VALOR
            ESA LLEGA DIRECTO A REMOTEMESSAGE.GETNOTIFICACION()   AHI LLEGAN LOS VALORES DEL FIREBASE 
            MESSAGETEXT  ES EL VALOR DEL MENSAJE COMPLETO
            IR A LAS OPCIONALES
             */

            /*ttl  timetolive,  se manda en segundos es el tiempo de expiracion 
             * 
             * https://firebase.google.com/docs/cloud-messaging/http-server-ref    
             * https://documentation.onesignal.com/reference#create-notification
             * 
             * JSON QUE LLEGA DESDE EL PHP
             {
    "to": "fsVPtHKu-f4:APA91bFFG_IM97AK9TYcaTHcWnWe-
    ZDLNZWWtCZBI9YJhImOt6dt4Pr910BifNaunJoKCHAsMZjh
    5Go7kZr4SJgNr31x52f8e9Q1WMWQxhzI3Rw22S9Wp47DZAXemRmbNFH5lN5fWq4c",
    "data": {
        "title": "titulo",
        "message": "fsdfds",
        "image": "",
        "action": "",
        "action_destination": "",
        "estadoo":"1"
        "data": {
            "extra": "info extra 1\nngvjhfgujhg\nngvjhfgujh\nngvjhfgujhg\nngvjhfgujhg",
            "extra2": "info extra 22222ojbgjkhv jkhv jkhvbjkhvjkhvbkuhjkvjkvjk"
        }
    }
}
             */

            // Notification string:          Titulo opcional que llega como el verdadero titulo  Mensaje principal a mostrar
            // Log.Debug(TAG, "Notification string: " + message.GetNotification().Title+"  "+message.GetNotification().Body+"  "+
            //954999819863          1538664774942      2419200
            //   message.From+"   "+ message.SentTime+"   " +message.To+"   "+message.Ttl);



            C_Notificacion _minotif = new C_Notificacion();
            if (message.GetNotification() == null)//LLEGA DESDE EL PHP
            {
                _minotif = new C_Notificacion(message.Data["title"], message.Data["message"]);
            }
            else
            {
                _minotif = new C_Notificacion(message.GetNotification().Title, message.GetNotification().Body);
            }

            if (message.Data.ContainsKey("data"))//tiene la info para la cita
            {
                //Cita _citaActual = new Cita(message.Data["estado"]);
                Cita _citaActual = JsonConvert.DeserializeObject<Cita>(message.Data["data"]);
                Console.Write("info cita" + _citaActual.Fn_GetInfo());
                _citaActual.Fn_SetValores();
                App.Fn_SetCita(_citaActual);
                string _titulo="";
                string _mensaje = "";
                if (_citaActual.v_estado == "0")
                {
                    _titulo = "Aviso de cita";
                    _mensaje = "Se ha Terminado una cita";
                }
                else if (_citaActual.v_estado == "2")
                {
                    _titulo = "Aviso de cita";
                    _mensaje = "Se ha reagendado una cita";
                }
                else if (_citaActual.v_estado == "3")
                {
                    _titulo = "Aviso de cita";
                    _mensaje = "Se ha aceptado una cita";
                }
                else if (_citaActual.v_estado == "4")
                {
                    _titulo = "Aviso de cita";
                    _mensaje = "Se ha cancelado una cita";
                }
                SendNotification(_mensaje, _titulo);
            }
            else//es una nootif normal, solo mensaje y titulo
            {
                SendNotification(_minotif.v_cuerpo, _minotif.v_titulo);
            }
        }
        //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/navigation/hierarchical
        //https://documentation.onesignal.com/docs/customize-notification-icons
        void SendNotification(string messageBody, string _titulo)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            //intent.PutExtra
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            //Bitmap largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.ICONOAPP);
            var notificationManager = NotificationManager.FromContext(this);
            var notificationBuilder = new Android.Support.V4.App.NotificationCompat.Builder(this)
             .SetSmallIcon(Resource.Drawable.Logo_Redondo_512x512_Blanco)//color
                                                                         // .SetLargeIcon(largeIcon)
             .SetContentTitle(_titulo)
             .SetContentText(messageBody)
             .SetContentIntent(pendingIntent)
             .SetColor(40150209)
             .SetStyle(new NotificationCompat.BigTextStyle().BigText(messageBody))
             .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
             .SetPriority(1)
             .SetAutoCancel(true);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                                                                // The id of the channel.    ----The user-visible name of the channel.
                NotificationChannel mChannel = new NotificationChannel("channelTratoEspecial", "Canal Visible", NotificationImportance.High);
                // Configure the notification channel.
                mChannel.Description="Description channel";               
                mChannel.EnableLights(true);
                // Sets the notification light color for notifications posted to this
                // channel, if the device supports this feature.
                mChannel.LightColor=Color.Red;
                mChannel.SetShowBadge(true);
                notificationBuilder.SetChannelId("channelTratoEspecial");
                notificationManager.CreateNotificationChannel(mChannel);
            }
                notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}
/*

            //    if (message.Data.ContainsKey("data"))//este data es un arreglo extra de loos keys que se manda desde el php que hice
            //    {
            //        string json = message.Data["data"];
            //        if(string.IsNullOrEmpty( json))
            //        {
            //            string _title = "";
            //            string _mess = "";

            //            if (string.IsNullOrEmpty(message.GetNotification().Title))
            //            {
            //                _title = "Titulo vacio";
            //            }
            //            else
            //            {
            //                _title = message.GetNotification().Title;
            //            }
            //            if (string.IsNullOrEmpty(message.GetNotification().Body))
            //            {
            //                _mess = "Mensaje vacio";
            //            }
            //            else
            //            {
            //                _mess = message.GetNotification().Body;
            //            }

            //            SendNotification(_title, _mess);
            //        }
            //        else
            //        {
            //            C_Notificacion _notif = JsonConvert.DeserializeObject<C_Notificacion>(json);
            //            SendNotification(_notif.v_titulo, _notif.v_cuerpo);
            //        }
            //    }
            //    else
            //    {//siempre trae esto
            //        string _title = "";
            //        string _mess = "";

            //        if (string.IsNullOrEmpty( message.GetNotification().Title))
            //        {
            //            _title = "";
            //        }
            //        else
            //        {
            //            _title = message.GetNotification().Title;
            //        }
            //        if (string.IsNullOrEmpty(message.GetNotification().Body))
            //        {
            //            _mess = "Mensaje vacio";
            //        }
            //        else
            //        {
            //            _mess = message.GetNotification().Body;
            //        }

            //        SendNotification(_title,_mess);
            //}
            
             */
