using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Varios;
using PayPal.Forms;
using PayPal.Forms.Abstractions;

//LoadUrl with javascript:
//webview.LoadUrl(string.Format("javascript: {0}", script));
namespace Trato
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Pagos : ContentPage
	{
       // string v_dirWeb = "https://useller.com.mx/trato_especial/paypal_test.php";
        Pagar v_infoPago;
        public V_Pagos (bool _efectivo, Pagar _pagar)
		{
			InitializeComponent ();
            v_infoPago = _pagar;
            if(_efectivo)
            {
                P_OxxoBut.IsVisible = true;
                P_mensajes.Text = "Se enviará un correo con la información necesaria";
                P_PayBut.IsVisible = false;
            }
            else{
                P_OxxoBut.IsVisible = false;
                if (CrossPayPalManager.IsInitialized)
                {
                    P_PayBut.IsEnabled = true;
                    P_mensajes.Text = "";//fecha_vig
                }
                else
                {
                    P_mensajes.Text = "Hubo un error, intentarlo mas tarde";
                    P_PayBut.IsEnabled = false;
                }
            }
        }
        public async void Fn_PagoOxxo(object sender, EventArgs _args)
        { 
            string json = @"{";
            json += "producto:'" + v_infoPago.v_nombre + "',\n";
            json += "costo:'" + v_infoPago.v_costo + "',\n";
            json += "nombre:'" + App.v_perfil.v_Nombre + "',\n";
            json += "correo:'" + App.v_perfil.v_Correo + "',\n";
            json += "tel:'" + App.v_perfil.v_Tel + "',\n";
            json += "}";
            JObject v_jsonInfo = JObject.Parse(json);
            StringContent _content = new StringContent(v_jsonInfo.ToString(), Encoding.UTF8, "application/json");

            HttpClient _clien = new HttpClient();
            string _direc = "https://useller.com.mx/trato_especial/prueba_conekta.php";
            try
            {
                HttpResponseMessage _responphp = await _clien.PostAsync(_direc, _content);
                string _resp = await _responphp.Content.ReadAsStringAsync();
                await DisplayAlert("mensaje ", _resp, "aceptar");
                await Navigation.PopAsync();
            }
            catch (HttpRequestException exception)
            {
                await DisplayAlert("Error", exception.Message, "Aceptar");
            }
        }
        public async void Fn_pagarPay(object sender, EventArgs _args)
        {
            var result = await CrossPayPalManager.Current.Buy(new PayPalItem(v_infoPago.v_nombre,int.Parse(v_infoPago.v_costo), "MXN"), new Decimal(0),null,PaymentIntent.Sale);
            if (result.Status == PayPalStatus.Cancelled)
            {
                P_mensajes.Text="Cancelled";
            }
            else if (result.Status == PayPalStatus.Error)
            {
                P_mensajes.Text = result.ErrorMessage;
            }
            else if (result.Status == PayPalStatus.Successful)
            {
                P_mensajes.Text = result.ServerResponse.Response.Id;
                HttpClient _clien = new HttpClient();
                string _direc = "https://useller.com.mx/trato_especial/activacion.php";
                string _json = JsonConvert.SerializeObject(v_infoPago, Formatting.Indented);
                StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage _responphp = await _clien.PostAsync(_direc, _content);
                    string _resp = await _responphp.Content.ReadAsStringAsync();
                    await DisplayAlert("mensaje ", _resp, "aceptar");
                    Perf _perf = new Perf();
                    _perf.v_fol = v_infoPago.v_conse;
                    _perf.v_membre =v_infoPago.v_membresia;
                    _perf.v_letra = v_infoPago.v_letra;
                    //crear el json
                    string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);
                    //crear el cliente
                    _clien = new HttpClient();
                    string _DirEnviar = "https://useller.com.mx/trato_especial/query_perfil.php";
                    _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                    try
                    {
                        //mandar el json con el post
                        _responphp = await _clien.PostAsync(_DirEnviar, _content);
                        _resp = await _responphp.Content.ReadAsStringAsync();
                        Personas.C_PerfilGen _nuePer = JsonConvert.DeserializeObject<Personas.C_PerfilGen>(_resp);
                        App.Fn_GuardarDatos(_nuePer, v_infoPago.v_membresia, v_infoPago.v_conse);
                        _DirEnviar = "https://useller.com.mx/trato_especial/query_perfil_medico.php";
                        _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                        try
                        {
                            //mandar el json con el post
                            _responphp = await _clien.PostAsync(_DirEnviar, _content);
                            _resp = await _responphp.Content.ReadAsStringAsync();
                            Personas.C_PerfilMed _nuePerMEd = JsonConvert.DeserializeObject<Personas.C_PerfilMed>(_resp);
                            await DisplayAlert("perfil", _resp, "sad");
                            App.Fn_GuardarDatos(_nuePerMEd, v_infoPago.v_membresia, v_infoPago.v_conse);
                            await Navigation.PopAsync();                    
                        }
                        catch (HttpRequestException exception)
                        {
                            await DisplayAlert("Error", exception.Message, "Aceptar");
                        }
                    }
                    catch (HttpRequestException exception)
                    {
                        await DisplayAlert("Error", exception.Message, "Aceptar");
                    }
                }
                catch (HttpRequestException exception)
                {
                    await DisplayAlert("Error", exception.Message, "Aceptar");
                }
            }
        }
        public async void Cargado(object sender, WebNavigatedEventArgs _args)
        {
            //if (_args.Result == WebNavigationResult.Timeout || _args.Result == WebNavigationResult.Failure)
            //{
            //    P_mensajes.Text = "Error de Conexion";
            //    P_mensajes.IsVisible = true;
            //}
            //else if (_args.Result == WebNavigationResult.Success)
            //{
            //    P_mensajes.IsVisible = false;
            //    P_paypal.IsVisible = true;
            //    string _json = JsonConvert.SerializeObject(v_infoPago, Formatting.Indented);
            //    await P_paypal.EvaluateJavaScriptAsync("asdasd(" + _json + ")");
            //    HttpClient _cli = new HttpClient();
            //    HttpResponseMessage _responht= await _cli.PostAsync(v_dirWeb, null);
            //    string _res =await  _responht.Content.ReadAsStringAsync();
            //    await DisplayAlert("adsd", "valor " + _res + "  ---", "SADSD");
            //}
        }
    }
}