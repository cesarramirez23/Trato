using System.Collections.Generic;
using System.Linq;

using System;
using Foundation;
using UIKit;

//xamarin.firebase.ios.cloudMessaging
namespace Trato.iOS
{

    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        protected string v_deviceToken= String.Empty;
        public string V_Token { get { return v_deviceToken; } } 


        //resturar paquetes,   actualizar    limpiar compilar   revisar que el infop.list este en 8
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
        /*
         public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
           
            base.RegisteredForRemoteNotifications(application, deviceToken);
            Console.Write(deviceToken.ToString());
        }
        // iOS 9 <=, fire when recieve notification foreground
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);

            // Generate custom event
            NSString[] keys = { new NSString("Event_type") };
            NSObject[] values = { new NSString("Recieve_Notification") };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);

            // Send custom event
            Firebase.Analytics.Analytics.LogEvent("CustomEvent", parameters);

            if (application.ApplicationState == UIApplicationState.Active)
            {
                System.Diagnostics.Debug.WriteLine(userInfo);
                var aps_d = userInfo["aps"] as NSDictionary;//titulo que trae el json
                var body = aps_d["message"] as NSString;//keys
                var title = aps_d["title"] as NSString;
                Console.Write("infooo" + title + "   " + body);
                if (aps_d.ContainsKey(new NSString("data")) )
                {
                    var _data=aps_d.ValueForKey(new NSString("data")) as NSDictionary;//jala la cita
                    Cita _citaActual = JsonConvert.DeserializeObject<Cita>((aps_d["data"]).ToString());
                    Console.Write("info cita" + _citaActual.Fn_GetInfo());
                    _citaActual.Fn_SetValores();
                    App.Fn_SetCita(_citaActual);
                    string _titulo = "";
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
                    debugAlert(_titulo, _mensaje);
                }
                else//es una nootif normal, solo mensaje y titulo
                {
                    debugAlert(title, body);
                }
            }
        }

        // iOS mayor 9, fire when recieve notification foreground
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var title = notification.Request.Content.Title;
            var body = notification.Request.Content.Body;
            Console.Write("infooo" + title + "   " + body);
            var UserINfo = notification.Request.Content.UserInfo;
            if (UserINfo.ContainsKey(new NSString("data")))
            {
                var _data = UserINfo.ValueForKey(new NSString("data")) as NSDictionary;//jala la cita
                Cita _citaActual = JsonConvert.DeserializeObject<Cita>((UserINfo["data"]).ToString());
                Console.Write("info cita" + _citaActual.Fn_GetInfo());
                _citaActual.Fn_SetValores();
                App.Fn_SetCita(_citaActual);
                string _titulo = "";
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
                debugAlert(_titulo, _mensaje);
            }
            else//es una nootif normal, solo mensaje y titulo
            {
                debugAlert(title, body);
            }
        }

                  */
    }
}
/*

 public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            System.Diagnostics.Debug.WriteLine($"FCM Token: {fcmToken}");
            App.Fn_SetToken(fcmToken);
        }
// Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    Console.WriteLine("granted  "+granted);
                });

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                //UIApplication.SharedApplication.RegisterUserNotificationSettings(authOptions);
                // For iOS 10 data message (sent via FCM)
                //Messaging.SharedInstance.Delegate = this;
                //Messaging.SharedInstance.RemoteMessageDelegate = this;
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();


            
            Firebase.Core.App.Configure();
            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
            {
                var newtoken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                System.Diagnostics.Debug.WriteLine($"FCM Token: {newtoken}");
                App.Fn_SetToken(newtoken);
            });

        // iOS 9 <=, fire when recieve notification foreground
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);
            // Generate custom event
            NSString[] keys = { new NSString("Event_type") };
            NSObject[] values = { new NSString("Recieve_Notification") };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);

            // Send custom event
            Firebase.Analytics.Analytics.LogEvent("CustomEvent", parameters);

            if (application.ApplicationState == UIApplicationState.Active)
            {
                System.Diagnostics.Debug.WriteLine(userInfo);
                var aps_d = userInfo["aps"] as NSDictionary;
                var alert_d = aps_d["alert"] as NSDictionary;
                var body = alert_d["body"] as NSString;
                var title = alert_d["title"] as NSString;
                debugAlert(title, body);
            }
        }
        // iOS 10, fire when recieve notification foreground
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var title = notification.Request.Content.Title;
            var body = notification.Request.Content.Body;
            debugAlert(title, body);
        }
        private void debugAlert(string title, string message)
        {
            UIAlertView alert = new UIAlertView()
            {
                Title = title,
                Message = message
            };
            alert.AddButton("Cancel");
            alert.AddButton("OK");
            alert.Show();
        }

*/
