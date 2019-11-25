using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Trato.Varios;
namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_PagoBill : ContentPage
	{
        Pagar v_infoPago;
        Regex v_Reg = new Regex(@"^([0-9]){2}/([0-9]){2}$");
        C_Pago v_pago = new C_Pago();
        bool v_paso=false;
        bool v_regresa = true;
        public V_PagoBill(Pagar _pagar)
        {
			InitializeComponent ();
            StackCodigo.IsVisible = true;
            StackTarjeta.IsVisible = false;
            v_infoPago = _pagar;
            v_regresa = false;
        }
        private async void Fn_Promotor(object sender, EventArgs _args)
        {
            Button _but = (Button)sender;
            _but.IsEnabled = false;
            bool _ret = await Fn_GetCodigo();
            if (_ret)
            {
                StackCodigo.IsVisible = false;
                StackTarjeta.IsVisible = true;
                _but.IsEnabled = true;
                v_paso = true;
                NumTar.Text = "4111111111111111";
                Fecha.Text = "12/12";
                CVC.Text = "123";
            }
            else
            {
                _but.IsEnabled = true;
            }
        }
        private async void Fn_Pago(object sender, EventArgs _args)
        {
            Button _btn = (Button)sender;
            _btn.IsEnabled = false;
            Regex MembreRegex = new Regex(@"^([0-9]){4}([A-Z]){1}-([0-9]){4}$");
            if (NumTar.Text.Length == 16)
            {
                if (v_Reg.IsMatch(Fecha.Text))
                {
                    if (CVC.Text.Length == 3 || CVC.Text.Length == 4)
                    {
                        await v_pago.Fn_SetTarjeta(NumTar.Text, Fecha.Text, CVC.Text, "10");
                        //await DisplayAlert("datos bien", v_pago.ToString(), "Aceptar");
                        HttpClient _client = new HttpClient();
                        string _json = JsonConvert.SerializeObject(v_pago);
                        StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
                        string _DirEnviar = NombresAux.BASE_URL + "billpocket.php";
                        try
                        {
                            v_regresa = true;//no puede regresar
                            HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                            string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                            C_Mensaje _mens = JsonConvert.DeserializeObject<C_Mensaje>(_respuesta);
                            if (_mens.v_code == "1")
                            {
                                await DisplayAlert("Aviso", "Pago realizado correctamente", "Aceptar");
                                Fn_Actualiza(_btn);
                            }
                            else
                            {
                                Lbl_Mensaje.Text = _mens.v_message;
                                // await DisplayAlert("title",  _mens.v_code + "   " + _mens.v_message, "asas");
                                _btn.IsEnabled = true;
                                v_regresa = false;
                            }
                        }
                        catch (Exception _ex)
                        {
                            Lbl_Mensaje.Text = "ERROR DE CONEXION";
                            await DisplayAlert("Error", "ERROR DE CONEXION", "Aceptar");
                            _btn.IsEnabled = true;
                            v_regresa = false;
                        }
                    }
                    else
                    {
                        Lbl_Mensaje.Text = "Revisar numeros de CVC";
                        await DisplayAlert("Error", "Revisar numeros de CVC", "Aceptar");
                        _btn.IsEnabled = true;
                        v_regresa = false;
                    }
                }
                else
                {
                    Lbl_Mensaje.Text = "Revisar numeros de EXPIRACION";
                    await DisplayAlert("Error", "Revisar numeros de EXPIRACION", "Aceptar");
                    _btn.IsEnabled = true;
                    v_regresa = false;
                }
            }
            else
            {
                Lbl_Mensaje.Text = "Revisar numeros de la TARJETA";
                await DisplayAlert("Error", "Revisar numeros de la TARJETA", "Aceptar");
                _btn.IsEnabled = true;
                v_regresa = false;
            }
        }
        private async void Fn_Actualiza(Button _but)
        {
            HttpClient _client = new HttpClient();
            string _direc = NombresAux.BASE_URL + "activacion.php";
            string _json = JsonConvert.SerializeObject(v_infoPago, Formatting.Indented);
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage _responphp = await _client.PostAsync(_direc, _content);
                string _respuesta = await _responphp.Content.ReadAsStringAsync();
                if (_respuesta == "1")
                {
                    await DisplayAlert("Aviso", "Perfil actualizado correctamente", "Aceptar");
                    App.Fn_GuardaValidacion(new C_Validar { v_activado = "1", v_renovacion = "0" });
                    await Navigation.PopAsync();
                }
                else
                {
                    await Navigation.PopAsync();
                }
            }
            catch (Exception exception)
            {
                Fn_Actualiza(_but);
            }
        }
        private void Fn_Fecha(object sender, TextChangedEventArgs e)
        {
            if (e.OldTextValue.Length == 1 && e.NewTextValue.Length == 2)
            {
                Fecha.Text = Fecha.Text.Insert(2, "/");
            }
            else if ((e.OldTextValue.Length == 2 && e.NewTextValue.Length == 3) && e.NewTextValue.Last() != '/')
            {
                Fecha.Text = Fecha.Text.Insert(2, "/");
            }
            else if (e.OldTextValue.Length == 4 && e.NewTextValue.Length == 3)
            {
                Fecha.Text = Fecha.Text.Remove(2);
            }
            else if (e.OldTextValue.Length == 1 && e.NewTextValue.Length == 0)
            {
                return;
            }
            else if (e.OldTextValue.Length == 4 && e.NewTextValue.Length == 5)
            {
                return;
            }
            else
            {
                return;
            }
        }
        async Task<bool> Fn_GetCodigo()
        {
            bool _ret = false;
            Prom_codigo.IsEnabled = false;
            if (string.IsNullOrEmpty(Prom_codigo.Text) || string.IsNullOrWhiteSpace(Prom_codigo.Text))
            {
                v_infoPago.v_codigo = string.Empty;
                v_infoPago.v_IdProm = string.Empty;
                _ret = await DisplayAlert("Aviso", "No se encuentra ningun codigo de promotor", "Continuar", "Cancelar");
            }
            else
            {
                HttpClient _client = new HttpClient();
                string json = @"{";
                json += "codigo:'" + Prom_codigo.Text + "',\n";
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
                        _ret = await DisplayAlert("Aviso", "Codigo Aceptado, ¿Deseas continuar?", "Sí", "No");
                    }
                    else if (_men.v_code == "0")
                    {
                        v_infoPago.v_IdProm = string.Empty;
                        _ret = await DisplayAlert("Aviso", "Error en código de promotor, ¿Deseas continuar?", "Sí", "No");
                    }
                }
                catch
                {
                    v_infoPago.v_IdProm = string.Empty;
                    _ret = await DisplayAlert("Aviso", "Error en código de promotor, ¿Deseas continuar?", "Sí", "No");
                }
            }
            v_infoPago.v_codigo = Prom_codigo.Text;
            return _ret;
        }
        protected override bool OnBackButtonPressed()
        {
            if (v_paso  )
            {
                if(!v_regresa)
                {

                StackCodigo.IsVisible = true;
                StackTarjeta.IsVisible = false;
                v_paso = false;
                Fn_LimpiaTexto();
                }
                return true;//no puede regresar
            }
            else
            {
                base.OnBackButtonPressed();
                return false;
            }
        }
        private void Fn_LimpiaTexto()
        {
            Prom_codigo.Text = "";
            NumTar.Text="";
            Fecha.Text="";
            CVC.Text = "";
        }
    }
}