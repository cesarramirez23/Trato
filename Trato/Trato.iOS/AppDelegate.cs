using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using UIKit;

using Firebase.CloudMessaging;
using UserNotifications;
using Firebase.Core;
/*
Objective-C exception thrown.  Name: com.firebase.core Reason: Configuration fails. 
It may be caused by an invalid GOOGLE_APP_ID in GoogleService-Info.plist or set in the customized options.
*/
namespace Trato.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            // Firebase component initialize
            Firebase.Core.App.Configure();
            LoadApplication(new App());

            var config = new PayPalConfiguration(PayPalEnvironment.Sandbox, "AVART2W6j2cnNhmWej6EcQjx_ytsVpl1hmnArzHtVWSsZFRVAWOlZq6y3EjPFM0FHUhG_yrvkftXAAtN")
            {
                //If you want to accept credit cards
                AcceptCreditCards = false,
                //Your business name
                MerchantName = "Test Store",
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
            CrossPayPalManager.Init(config);
            //https://github.com/codercampos/FirebaseXF-XamarinLatino/blob/master/src/FirebaseXL/FirebaseXL.iOS/AppDelegate.cs
    





            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    Console.WriteLine(granted);
                });

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                // For iOS 10 data message (sent via FCM)
                Messaging.SharedInstance.Delegate=this;
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
            
            UIApplication.SharedApplication.RegisterForRemoteNotifications();



            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender,e)=>{
                var newtoken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                //if you want to send notification per user, use this token
                App.Fn_SetToken(newtoken);
                System.Diagnostics.Debug.WriteLine(newtoken);
            });

// blog.xamarians.com/blog/2017/9/18/firebase-cloud-messaging


            return base.FinishedLaunching(app, options);
        }
        /*
2018-10-05 10:55:26.157 Trato.iOS[1073:622909] <Firebase/Network/ERROR> Encounter network error. Code, error: -1200, 
Error Domain=NSURLErrorDomain Code=-1200 "An SSL error has occurred and a secure connection to the server cannot be made." 
UserInfo={NSURLErrorFailingURLPeerTrustErrorKey=<SecTrustRef: 0x109570a00>, NSLocalizedRecoverySuggestion=Would you like to connect to the server anyway?, 
_kCFStreamErrorDomainKey=3, _kCFStreamErrorCodeKey=-9802, NSErrorPeerCertificateChainKey=(


"<cert(0x103911800) s: *.googleapis.com i: Google Internet Authority G3>",
    "<cert(0x103912000) s: Google Internet Authority G3 i: GlobalSign>"
), NSUnderlyingError=0x10959be60 {Error Domain=kCFErrorDomainCFNetwork Code=-1200 "(null)" 
UserInfo={_kCFStreamPropertySSLClientCertificateState=0, kCFStreamPropertySSLPeerTrust=<SecTrustRef: 0x109570a00>, 
_kCFNetworkCFStreamSSLErrorOriginalValue=-9802, _kCFStreamErrorDomainKey=3, _kCFStreamErrorCodeKey=-9802, kCFStreamPropertySSLPeerCertificates=(
    "<cert(0x103911800) s: *.googleapis.com i: Google Internet Authority G3>",
    "<cert(0x103912000) s: Google Internet Authority G3 i: GlobalSign>"


)}}, NSLocalizedDescription=An SSL error has occurred and a secure connection to the server cannot be made.,
NSErrorFailingURLKey=https://play.googleapis.com/log, NSErrorFailingURLStringKey=https://play.googleapis.com/log, NSErrorClientCertificateStateKey=0}



NSURLSession/NSURLConnection HTTP load failed (kCFStreamErrorDomainSSL, -9802)


2018-10-05 11:10:21.737 Trato.iOS[1088:630745] <Firebase/Network/ERROR> Encounter network error. Code, error: -1001, Error Domain=NSURLErrorDomain Code=-1001 "The request timed out." 
UserInfo={NSErrorFailingURLStringKey=https://play.googleapis.com/log, NSErrorFailingURLKey=https://play.googleapis.com/log, _kCFStreamErrorDomainKey=4,
_kCFStreamErrorCodeKey=-2103, NSLocalizedDescription=The request timed out
        */


        // iOS 9 <=, fire when recieve notification foreground
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);

            // Generate custom event
            NSString[] keys = { new NSString("Event_type") };
            NSObject[] values = { new NSString("Recieve_Notification") };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);

            // Send custom event
           // Firebase.Analytics.Analytics.LogEvent("CustomEvent", parameters);

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
        [Export("messaging:didRefreshRegistrationToken:")]
        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            System.Diagnostics.Debug.WriteLine($"FCM Token: {fcmToken}");
            App.Fn_SetToken(fcmToken);
        }
    }
}
