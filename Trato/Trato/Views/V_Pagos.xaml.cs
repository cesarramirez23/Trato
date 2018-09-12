using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net.Http;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Varios;


namespace Trato
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Pagos : ContentPage
	{
        string v_dirWeb = "https://useller.com.mx/trato_especial/paypal_test.html";
        Pagar v_infoPago;
        public V_Pagos (bool _efectivo, Pagar _pagar)
		{
			InitializeComponent ();
            if(!_efectivo)
            {
                v_infoPago = _pagar;
                P_paypal.Source = v_dirWeb;
            }

        }

        public void Cargado(object sender, WebNavigatedEventArgs _args)
        {
            if (_args.Result == WebNavigationResult.Timeout || _args.Result == WebNavigationResult.Failure)
            {
                P_mensajes.Text = "Error de Conexion";
                P_mensajes.IsVisible = true;
            }
            else if (_args.Result == WebNavigationResult.Success)
            {
                P_mensajes.IsVisible = false;
                P_paypal.IsVisible = true;
                string _json = JsonConvert.SerializeObject(v_infoPago, Formatting.Indented);
                P_paypal.EvaluateJavaScriptAsync("asadasd()");
               
            }
        }
    }
}