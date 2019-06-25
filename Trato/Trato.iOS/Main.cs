using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using PayPal.Forms.Abstractions;
using PayPal.Forms;

namespace Trato.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
            var config = new PayPalConfiguration(PayPalEnvironment.Production, "AQrrFs8D-iSCYiAEEmO9ni3CcQ7GjgjPqSBBVhxNTmRKOnLR_Ol_qRcy2Pr4yxhwcQ2BK1BoZbzl0Hka")
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
        }
    }
}
