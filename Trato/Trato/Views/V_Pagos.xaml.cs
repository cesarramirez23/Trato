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


//LoadUrl with javascript:
//webview.LoadUrl(string.Format("javascript: {0}", script));
namespace Trato
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Pagos : ContentPage
	{
        string v_dirWeb = "https://useller.com.mx/trato_especial/paypal_test.php";
        Pagar v_infoPago;
        string v_script= "<script src='https://www.paypalobjects.com/api/checkout.js'></script>";
        public V_Pagos (bool _efectivo, Pagar _pagar)
		{
			InitializeComponent ();
            v_script = "";
                if(!_efectivo)
            {
                v_infoPago = _pagar;
                P_paypal.Source = v_dirWeb;
            }
        }

        public async void Cargado(object sender, WebNavigatedEventArgs _args)
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
                await P_paypal.EvaluateJavaScriptAsync("asdasd(" + _json + ")");
                HttpClient _cli = new HttpClient();
                HttpResponseMessage _responht= await _cli.PostAsync(v_dirWeb, null);
                string _res =await  _responht.Content.ReadAsStringAsync();
                await DisplayAlert("adsd", "valor " + _res + "  ---", "SADSD");


            }
        }
    }
}