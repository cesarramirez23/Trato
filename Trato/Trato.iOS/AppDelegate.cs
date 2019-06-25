using System.Collections.Generic;
using System.Linq;

using System;
using Foundation;
using UIKit;

using Firebase.CloudMessaging;
using UserNotifications;

using PayPal.Forms;
using PayPal.Forms.Abstractions;

using Trato.Varios;
using Newtonsoft.Json;
using Trato.Models;
//xamarin.firebase.ios.cloudMessaging
namespace Trato.iOS
{

    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {

        protected string v_deviceToken= String.Empty;
        public string V_Token { get { return v_deviceToken; } } 
        public void DidRefreshRegistrationToken(Messaging messaging, string fcmtoken)
        {
            System.Diagnostics.Debug.WriteLine($"fcm token   did refre: {fcmtoken}");
            App.Fn_SetToken(fcmtoken);
        }

        //resturar paquetes,   actualizar    limpiar compilar   revisar que el infop.list este en 8
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
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
            CrossPayPalManager.Init(config);
            LoadApplication(new App());



            if(options != null)
            {
                //check for remote notification
                if(options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
                {
                    NSDictionary _remoteNotif = options[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
                    if(_remoteNotif != null)
                    {
                       // new UIAlertView(_remoteNotif.a)
                    }


                }
            }

            // pedir permiso para enviar notificaciones
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // iOS 8 or later
                var notifSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);

                // For iOS 8 display notification (sent via APNS)
                app.RegisterUserNotificationSettings(notifSettings);
                app.RegisterForRemoteNotifications();

              /*  UNUserNotificationCenter.Current.Delegate = this;
                System.Diagnostics.Debug.WriteLine($"antes 11: {app.IsRegisteredForRemoteNotifications.ToString()}");
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
                System.Diagnostics.Debug.WriteLine($"despues 11: {app.IsRegisteredForRemoteNotifications.ToString()}");*/
            }
            else
            {
                //register for remote notifications and get the device token
                //set what kind of notifiaction type we want
                UIRemoteNotificationType _notifTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge;
                //register for remtote notif
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(_notifTypes);

                /*var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                System.Diagnostics.Debug.WriteLine($"antes 9: {app.IsRegisteredForRemoteNotifications.ToString()}");
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
                System.Diagnostics.Debug.WriteLine($"despues 9: {app.IsRegisteredForRemoteNotifications.ToString()}");*/
            }
            Firebase.Core.App.Configure();

            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
            {
                var newtoken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                System.Diagnostics.Debug.WriteLine($"FCM Token observe: {newtoken}");
                App.Fn_SetToken(newtoken);
            });
            //Firebase.InstanceID.InstanceId.TokenRefreshNotification;
            //debugAlert(UIApplication.SharedApplication.IsRegisteredForRemoteNotifications.ToString(),V_Token);
           // Console.WriteLine(" token device " + V_Token);
            //https://github.com/codercampos/FirebaseXF-XamarinLatino/blob/master/src/FirebaseXL/FirebaseXL.iOS/AppDelegate.cs        // blog.xamarians.com/blog/2017/9/18/firebase-cloud-messaging
            return base.FinishedLaunching(app, options);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {//esta es la que se llama con la consola de firebase,  falta hacerlo desde el php
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
                var _alert = aps_d["alert"] as NSDictionary;
                var body = _alert["body"] as NSString;//keys
                var title = _alert["title"] as NSString;
                Console.Write("infooo" + title + "   " + body);
                if (_alert.ContainsKey(new NSString("data")))
                {
                    var _data = _alert.ValueForKey(new NSString("data")) as NSDictionary;//jala la cita
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
                    debugAlert(_titulo, _mensaje,"Revisar");
                }
                else//es una nootif normal, solo mensaje y titulo
                {
                    debugAlert(title, body,"Aceptar");
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
                debugAlert(_titulo, _mensaje,"Revisar");
            }
            else//es una nootif normal, solo mensaje y titulo
            {
                debugAlert(title, body,"Aceptar");
            }
        }
        public void ApplicationReceiveRemoteMessage(RemoteMessage message)
        {
            Console.Write("remote message "+message.AppData);
        }
        //mostrar la alerta
        private void debugAlert(string title, string message, string Txtbtn)
        {
            UIAlertView alert = new UIAlertView()
            {
                Title = title,
                Message = message
            };
            alert.AddButton(Txtbtn);
            alert.Show();

        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            var aps_d = userInfo["aps"] as NSDictionary;//titulo que trae el json
            var body = aps_d["message"] as NSString;//keys
            var title = aps_d["title"] as NSString;
            Console.Write("infooo" + title + "   " + body);
            if (aps_d.ContainsKey(new NSString("data")))
            {
                var _data = aps_d.ValueForKey(new NSString("data")) as NSDictionary;//jala la cita
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
                debugAlert(title, body,"Revisar");
            }
            else//es una nootif normal, solo mensaje y titulo
            {
                debugAlert(title, body,"Aceptar");
            }
        }
        /// <summary>
        /// the ios call the apns in the background and issue a device token to the device when thats accomplished, this method will be called
        /// 
        /// note: the device can change, so this needs to register with your server application everytime
        /// this method is onvoked, or a minimum , cache the last token check for a change
        /// </summary>
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var _devTemp = deviceToken.Description.Replace("<", "").Replace(">", "").Replace(" ", "");
            if(!string.IsNullOrEmpty(_devTemp))
            {
                v_deviceToken = _devTemp;
                //Console.Write("token device " + v_deviceToken);
                //App.Fn_SetToken(v_deviceToken);
            }
        }
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            debugAlert("Error registering push notification", error.LocalizedDescription,"Aceptar");
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
