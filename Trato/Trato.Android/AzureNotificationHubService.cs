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
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace Trato.Droid
{
    public class AzureNotificationHubService
    {
        const string TAG = "AzureNotificationHubService";

        public static async Task RegisterAsync(Push push, string token)
        {
            try
            {
                const string templateBody = "{\"data\":{\"message\":\"$(messageParam)\"}}";
                JObject templates = new JObject();
                templates["genericMessage"] = new JObject
            {
                {"body", templateBody}
            };

                await push.RegisterAsync(token, templates);
                Log.Info("Push Installation Id: ", push.InstallationId.ToString());
            }
            catch (Exception ex)
            {
                Log.Error(TAG, "Could not register with Notification Hub: " + ex.Message);
            }
        }
    }
}

/*
 D/Mono    (27184): Assembly Ref addref Trato.Android[0x7f8b2e5000] -> Newtonsoft.Json[0x7f8b2e5800]: 4
Resolved pending breakpoint at 'AzureNotificationHubService.cs:28,1' to void Trato.Droid.AzureNotificationHubService.<RegisterAsync>d__1.MoveNext () [0x0001a].
09-12 09:22:50.705 D/Surface (27184): Surface::setBuffersDimensions(this=0x7f6e11cc00,w=720,h=1280)
09-12 09:22:52.649 E/AzureNotificationHubService(27184): Could not register with Notification Hub: <!DOCTYPE html>
09-12 09:22:52.649 E/AzureNotificationHubService(27184): <html>
09-12 09:22:52.649 E/AzureNotificationHubService(27184): <head>
09-12 09:22:52.649 E/AzureNotificationHubService(27184):     <title>Web App - Unavailable</title>
09-12 09:22:52.649 E/AzureNotificationHubService(27184):     <style type="text/css">
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         html {
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             height: 100%;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             width: 100%;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         }
09-12 09:22:52.649 E/AzureNotificationHubService(27184): 
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         #feature {
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             width: 960px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             margin: 95px auto 0 auto;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             overflow: auto;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         }
09-12 09:22:52.649 E/AzureNotificationHubService(27184): 
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         #content {
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             font-family: "Segoe UI";
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             font-weight: normal;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             font-size: 22px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             color: #ffffff;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             float: left;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             width: 460px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             margin-top: 68px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             margin-left: 0px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             vertical-align: middle;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         }
09-12 09:22:52.649 E/AzureNotificationHubService(27184): 
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             #content h1 {
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 font-family: "Segoe UI Light";
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 color: #ffffff;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 font-weight: normal;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 font-size: 60px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 line-height: 48pt;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 width: 800px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             }
09-12 09:22:52.649 E/AzureNotificationHubService(27184): 
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         p a, p a:visited, p a:active, p a:hover {
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             color: #ffffff;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         }
09-12 09:22:52.649 E/AzureNotificationHubService(27184): 
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         #content a.button {
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             background: #0DBCF2;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             border: 1px solid #FFFFFF;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             color: #FFFFFF;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             display: inline-block;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             font-family: Segoe UI;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             font-size: 24px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             line-height: 46px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             margin-top: 10px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             padding: 0 15px 3px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             text-decoration: none;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         }
09-12 09:22:52.649 E/AzureNotificationHubService(27184): 
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             #content a.button img {
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 float: right;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 padding: 10px 0 0 15px;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             }
09-12 09:22:52.649 E/AzureNotificationHubService(27184): 
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             #content a.button:hover {
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 background: #1C75BC;
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             }
09-12 09:22:52.649 E/AzureNotificationHubService(27184):     </style>
09-12 09:22:52.649 E/AzureNotificationHubService(27184): </head>
09-12 09:22:52.649 E/AzureNotificationHubService(27184): <body bgcolor="#00abec">
09-12 09:22:52.649 E/AzureNotificationHubService(27184):     <div id="feature">
09-12 09:22:52.649 E/AzureNotificationHubService(27184):             <div id="content">
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 <h1 id="unavailable">Error 403 - This web app is stopped.</h1>
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 <p id="tryAgain">The web app you have attempted to reach is currently stopped and does not accept any requests. Please try to reload the page or visit it again soon.</p>
09-12 09:22:52.649 E/AzureNotificationHubService(27184):                 <p id="toAdmin">If you are the web app administrator, please find the common 403 error scenarios and resolution <a href="http://blogs.msdn.com/b/waws/archive/2016/01/05/azure-web-apps-error-403-this-web-app-is-stopped.aspx" target="_blank">here</a>. For further troubleshooting tools and recommendations, please visit <a href="https://portal.azure.com/">Azure Portal</a>.</p>
09-12 09:22:52.649 E/AzureNotificationHubService(27184):         </div>
09-12 09:22:52.649 E/AzureNotificationHubService(27184):     </div>
09-12 09:22:52.649 E/AzureNotificationHubService(27184): </body>
09-12 09:22:52.649 E/AzureNotificationHubService(27184): </html>
     
     
     
     */
