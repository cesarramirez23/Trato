﻿using System;
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
       // string v_dirWeb = "https://tratoespecial.com/paypal_test.php";
        Pagar v_infoPago;
        public V_Pagos (bool _efectivo, Pagar _pagar,string _tokenCone)
		{
			InitializeComponent ();
            Console.WriteLine("conekta " + _tokenCone);
            v_infoPago = _pagar;
            if(_efectivo)//oxxo con conekta
            {
                P_OxxoBut.IsVisible = true;
                P_PayBut.IsVisible = false;
                if (string.IsNullOrWhiteSpace(_tokenCone)|| string.IsNullOrEmpty(_tokenCone) )
                {
                    P_mensajes.Text = "Se enviará tu ficha de pago al correo que tienes registrado";
                }
                else{
                    P_mensajes.Text = "Ya tienes una ficha generada, no puedes crear una nueva";
                    P_OxxoBut.Text = "YA TIENES UNA";
                    P_OxxoBut.IsEnabled = false;
                   
                }
            }
            else{
                P_OxxoBut.IsVisible = false;
                CrossPayPalManager.Current.SetConfig(
                    new PayPalConfiguration(PayPalEnvironment.Production, "")
                    {

                    }
                    );
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
           // Fn_GurdaPerfil();
        }
        async void Fn_GurdaPerfil(){

            HttpResponseMessage _responphp = new HttpResponseMessage();
            Perf _perf = new Perf();
            _perf.v_fol = App.v_folio;
            _perf.v_membre = App.v_membresia;
            _perf.v_letra = v_infoPago.v_letra;
            //crear el json
            string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);
            //crear el cliente
            HttpClient _clien = new HttpClient();
            string _DirEnviar = NombresAux.BASE_URL + "query_perfil.php";
            StringContent _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
            try
            {
                //mandar el json con el post
                _responphp = await _clien.PostAsync(_DirEnviar, _content);
                string _resp = await _responphp.Content.ReadAsStringAsync();
                //await DisplayAlert("llega ",_perf.Fn_GetDatos() +" "+ _resp, "aceptar");
                Personas.C_PerfilGen _nuePer = JsonConvert.DeserializeObject<Personas.C_PerfilGen>(_resp);
                //await DisplayAlert("perfil general", _resp, "aceptar");
                App.Fn_GuardarDatos(_nuePer, v_infoPago.v_membresia, App.v_folio, App.v_letra);
                _DirEnviar = NombresAux.BASE_URL + "query_perfil_medico.php";
                _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                try
                {
                    //mandar el json con el post
                    _responphp = await _clien.PostAsync(_DirEnviar, _content);
                    _resp = await _responphp.Content.ReadAsStringAsync();
                    //await DisplayAlert("perfil medico", _resp, "aceptar");
                    Personas.C_PerfilMed _nuePerMEd = JsonConvert.DeserializeObject<Personas.C_PerfilMed>(_resp);
                    //await DisplayAlert("perfil medico", _resp, "sad");
                    App.Fn_GuardarDatos(_nuePerMEd, v_infoPago.v_membresia,App.v_folio, App.v_letra);
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

        public async void Fn_PagoOxxo(object sender, EventArgs _args)
        {
            Button _but = (Button)sender;
            _but.IsEnabled = false;
            string json = @"{";
            json += "producto:'" + v_infoPago.v_nombre + "',\n";
            json += "costo:'" + v_infoPago.v_costo + "',\n";
            json += "nombre:'" + App.v_perfil.v_Nombre + "',\n";
            json += "correo:'" + App.v_perfil.v_Correo + "',\n";
            json += "tel:'" + App.v_perfil.v_Tel + "',\n";
            json += "membre:'" + v_infoPago.v_membresia + "',\n";
            json += "letra:'" + v_infoPago.v_letra + "',\n";
            json += "consecutivo:'" + v_infoPago.v_conse + "',\n";
            json += "}";
            JObject v_jsonInfo = JObject.Parse(json);
            StringContent _content = new StringContent(v_jsonInfo.ToString(), Encoding.UTF8, "application/json");
            
            P_mensajes.Text += "\nCreando solicitud";
           // await DisplayAlert("Envia", v_jsonInfo.ToString(), "Acep");
            HttpClient _clien = new HttpClient();
            string _direc = NombresAux.BASE_URL + "prueba_conekta.php";
            try
            {
                HttpResponseMessage _responphp = await _clien.PostAsync(_direc, _content);
                string _resp = await _responphp.Content.ReadAsStringAsync();
                if(_resp=="1")
                {
                    await DisplayAlert("Aviso", "Ficha generada con éxito, cuando pagues se activará tu cuenta y sus servicios", "aceptar");    
                    await Navigation.PopAsync();
                }
                else
                {
                    _but.IsEnabled = true;
                    await DisplayAlert("Aviso", _resp, "Aceptar");
                }
            }
            catch (HttpRequestException exception)
            {
                _but.IsEnabled = true;
                await DisplayAlert("Error", exception.Message, "Aceptar");
            }
        }
        public async void Fn_pagarPay(object sender, EventArgs _args)
        {
            Button _but = (Button)sender;
            _but.IsEnabled = false;
            var result = await CrossPayPalManager.Current.Buy(new PayPalItem(v_infoPago.v_nombre,int.Parse(v_infoPago.v_costo), "MXN"), new Decimal(0),null,PaymentIntent.Sale);
            //var result = await CrossPayPalManager.Current.Buy(new PayPalItem(v_infoPago.v_nombre,1, "MXN"), new Decimal(0),null,PaymentIntent.Sale);
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
                HttpClient _clien = new HttpClient();
                string _direc = NombresAux.BASE_URL + "activacion.php";
                string _json = JsonConvert.SerializeObject(v_infoPago, Formatting.Indented);
                StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage _responphp = await _clien.PostAsync(_direc, _content);

                string _noespacios = "";
                string _usutexto = App.v_membresia;
                for (int i = 0; i < _usutexto.Length; i++)
                {
                    string _temp = _usutexto[i].ToString();
                    if (_temp != " ")
                    {
                        _noespacios += _usutexto[i];
                    }
                }

                Perf _perf = new Perf();
                _perf.v_fol = App.v_folio;
                _perf.v_membre = _noespacios;
                _perf.v_letra = App.v_letra;
                    string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);
                    //crear el cliente
                    _clien = new HttpClient();
                    string _DirEnviar = NombresAux.BASE_URL + "query_perfil.php";
                 _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                    try
                    {
                        //mandar el json con el post
                        _responphp = await _clien.PostAsync(_DirEnviar, _content);
                        string _resp = await _responphp.Content.ReadAsStringAsync();
                        //await DisplayAlert("llega ",_perf.Fn_GetDatos() +" "+ _resp, "aceptar");
                        Personas.C_PerfilGen _nuePer = JsonConvert.DeserializeObject<Personas.C_PerfilGen>(_resp);
                        App.Fn_GuardarDatos(_nuePer, App.v_membresia, App.v_folio, App.v_letra);
                        _DirEnviar = NombresAux.BASE_URL + "query_perfil_medico.php";
                        _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                        try
                        {
                            //mandar el json con el post
                            _responphp = await _clien.PostAsync(_DirEnviar, _content);
                            _resp = await _responphp.Content.ReadAsStringAsync();
                            Personas.C_PerfilMed _nuePerMEd = JsonConvert.DeserializeObject<Personas.C_PerfilMed>(_resp);
                            //await DisplayAlert("perfil medico", _resp, "sad");
                            App.Fn_GuardarDatos(_nuePerMEd, App.v_membresia, App.v_folio, App.v_letra);
                            await Navigation.PopAsync();                    
                        }
                        catch (HttpRequestException exception)
                        {
                            _but.IsEnabled = true;
                            await DisplayAlert("Error", exception.Message, "Aceptar");
                        }
                    }
                    catch (HttpRequestException exception)
                    {
                            _but.IsEnabled = true;
                        await DisplayAlert("Error", exception.Message, "Aceptar");
                    }
                }
                catch (HttpRequestException exception)
                {
                            _but.IsEnabled = true;
                    await DisplayAlert("Error", exception.Message, "Aceptar");
                }
            }
        }
        public  void Cargado(object sender, WebNavigatedEventArgs _args)
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