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
using Trato.Models;
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
        /*
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
                catch (Exception exception)
                {
                    await DisplayAlert("Error", exception.Message, "Aceptar");
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("Error", exception.Message, "Aceptar");
            }
        }
        */
        public async void Fn_PagoOxxo(object sender, EventArgs _args)
        {
            Button _but = (Button)sender;
            _but.IsEnabled = false;

            bool _ret = await Fn_GetCodigo();

            if(_ret)
            {
                string json = @"{";
                json += "producto:'" + v_infoPago.v_nombre + "',\n";
                json += "costo:'" + v_infoPago.v_costo + "',\n";
                json += "nombre:'" + App.v_perfil.v_Nombre + "',\n";
                json += "correo:'" + App.v_perfil.v_Correo + "',\n";
                json += "tel:'" + App.v_perfil.v_Tel + "',\n";
                json += "membre:'" + v_infoPago.v_membresia + "',\n";
                json += "letra:'" + v_infoPago.v_letra + "',\n";
                json += "consecutivo:'" + v_infoPago.v_conse + "',\n";
                json += "codigo:'" + v_infoPago.v_codigo + "',\n";
                json += "}";
                JObject v_jsonInfo = JObject.Parse(json);
                string _d = v_jsonInfo.ToString();
                StringContent _content = new StringContent(v_jsonInfo.ToString(), Encoding.UTF8, "application/json");
            
                P_mensajes.Text += "\nCreando solicitud";
               // await DisplayAlert("Envia", v_jsonInfo.ToString(), "Acep");
                HttpClient _clien = new HttpClient();
                string _direc = NombresAux.BASE_URL + "prueba_conekta.php";
                try
                {
                    HttpResponseMessage _responphp = await _clien.PostAsync(_direc, _content);
                    string _resp = await _responphp.Content.ReadAsStringAsync();
                    C_Mensaje _men = JsonConvert.DeserializeObject<C_Mensaje>(_resp);
                    if(_men.v_code=="1")
                    {
                        await DisplayAlert("Aviso", "Ficha generada con éxito, cuando pagues se activará tu cuenta y sus servicios", "aceptar");    
                        await Navigation.PopAsync();
                    }
                    else if(_men.v_code=="0")
                    {
                        _but.IsEnabled = true;
                        Prom_codigo.IsEnabled = true;
                        await DisplayAlert("Aviso", _men.v_message, "Aceptar");
                    }
                }
                catch (Exception exception)
                {
                    _but.IsEnabled = true;
                    Prom_codigo.IsEnabled = true;
                    await DisplayAlert("Error","Error de conexión", "Aceptar");
                }
            }
            else
            {
                Prom_codigo.IsEnabled = true;
                _but.IsEnabled = true;
            }
        }
        public async void Fn_pagarPay(object sender, EventArgs _args)
        {
            Button _but = (Button)sender;
            _but.IsEnabled = false;

            bool _ret = await Fn_GetCodigo();
            if (_ret)
            {
                var result = await CrossPayPalManager.Current.Buy(new PayPalItem(v_infoPago.v_nombre, int.Parse(v_infoPago.v_costo), "MXN"), new Decimal(0), null, PaymentIntent.Sale);
                //var result = await CrossPayPalManager.Current.Buy(new PayPalItem(v_infoPago.v_nombre,1, "MXN"), new Decimal(0),null,PaymentIntent.Sale);
                if (result.Status == PayPalStatus.Cancelled)
                {
                    _but.IsEnabled = true;
                    Prom_codigo.IsEnabled = true;
                    P_mensajes.Text = "Operación Cancelada";
                }
                else if (result.Status == PayPalStatus.Error)
                {
                    _but.IsEnabled = true;
                    Prom_codigo.IsEnabled = true;
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
                            catch (Exception exception)
                            {
                                _but.IsEnabled = true;
                                Prom_codigo.IsEnabled = true;
                                await DisplayAlert("Error", "Error de conexión", "Aceptar");
                            }
                        }
                        catch (Exception exception)
                        {
                            _but.IsEnabled = true;
                            Prom_codigo.IsEnabled = true;
                            await DisplayAlert("Error", "Error de conexión", "Aceptar");
                        }
                    }
                    catch (Exception exception)
                    {
                        _but.IsEnabled = true;
                        Prom_codigo.IsEnabled = true;
                        await DisplayAlert("Error", "Error de conexión", "Aceptar");
                    }
                }
            }
            else
            {
                Prom_codigo.IsEnabled = true;
                _but.IsEnabled = true;
            }
        }
        async Task<bool> Fn_GetCodigo()
        {
            bool _ret = false;
            Prom_codigo.IsEnabled = false;
            if(string.IsNullOrEmpty( Prom_codigo.Text)  ||  string.IsNullOrWhiteSpace(Prom_codigo.Text))
            {
                v_infoPago.v_codigo = string.Empty;
                v_infoPago.v_IdProm = string.Empty;
                _ret =  await DisplayAlert("Aviso", "No se encuentra ningun codigo de promotor", "Continuar", "Cancelar");
            }
            else
            {
                HttpClient _client = new HttpClient();
                string json = @"{";
                json += "codigo:'" +Prom_codigo.Text + "',\n";
                json += "}";
                JObject v_jsonInfo = JObject.Parse(json);
                StringContent _content = new StringContent(v_jsonInfo.ToString(), Encoding.UTF8, "application/json");
                try
                {
                    string _dir = NombresAux.BASE_URL + "verificacion_promotor.php";

                    HttpResponseMessage _respuestphp = await _client.PostAsync(_dir, _content);
                    string _respuesta = await _respuestphp.Content.ReadAsStringAsync();
                    C_Mensaje _men = JsonConvert.DeserializeObject<C_Mensaje>(_respuesta);
                    if (_men.v_code == "1")
                    {
                        v_infoPago.v_IdProm = _men.v_message;
                        _ret =await DisplayAlert("Aviso", "Codigo Aceptado, ¿Deseas continuar?", "Sí", "No");
                    }
                    else if(_men.v_code=="0")
                    {
                        v_infoPago.v_IdProm = string.Empty ;
                        _ret =await DisplayAlert("Aviso", "Error en código de promotor, ¿Deseas continuar?", "Sí", "No");                       
                    }
                }
                catch
                {
                        v_infoPago.v_IdProm = string.Empty ;
                    _ret = await DisplayAlert("Aviso", "Error en código de promotor, ¿Deseas continuar?", "Sí", "No");
                }
            }
            v_infoPago.v_codigo = Prom_codigo.Text;
            return _ret;
        }
    }
}