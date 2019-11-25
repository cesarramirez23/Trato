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
using Trato.Models;
namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page1 : ContentPage
	{
        Regex v_Reg = new Regex(@"^([0-9]){2}/([0-9]){2}$");
        C_Pago v_pago = new C_Pago();
        public Page1 ()
		{
			InitializeComponent ();
		}
        private async void Fn_Pago(object sender, EventArgs _args)
        {
            Button _btn = (Button)sender;
            _btn.IsEnabled = false;
            Regex MembreRegex = new Regex(@"^([0-9]){4}([A-Z]){1}-([0-9]){4}$");
            if (v_Reg.IsMatch(Fecha.Text))
            {
                if(CVC.Text.Length==3  ||  CVC.Text.Length==4)
                {
                    if (NumTar.Text.Length == 16)
                    {
                        await v_pago.Fn_SetTarjeta(NumTar.Text, Fecha.Text, CVC.Text, "10");
                        //await DisplayAlert("datos bien", v_pago.ToString(), "Aceptar");
                        

                        HttpClient _client = new HttpClient();
                        string _json = JsonConvert.SerializeObject(v_pago);
                        StringContent _content = new StringContent(_json,Encoding.UTF8, "application/json");
                        string _DirEnviar = NombresAux.BASE_URL + "billpocket.php";
                        try
                        {
                            HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                            string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                            C_Mensaje _mens = JsonConvert.DeserializeObject<C_Mensaje>(_respuesta);
                            if(_mens.v_code=="1")
                            {
                                await DisplayAlert("title", "pagado", "asas");
                            }
                            else
                            {
                                Lbl_Mensaje.Text = _mens.v_message;
                               // await DisplayAlert("title",  _mens.v_code + "   " + _mens.v_message, "asas");
                            }
                            _btn.IsEnabled = true;
                        }
                        catch (Exception _ex)
                        {
                            Lbl_Mensaje.Text = "ERROR DE CONEXION";
                            await DisplayAlert("Error","ERROR DE CONEXION","Aceptar");
                            _btn.IsEnabled = true;
                        }
                    }
                    else
                    {
                        Lbl_Mensaje.Text = "Revisar numeros de la TARJETA";
                        await DisplayAlert("Error","Revisar numeros de la TARJETA","Aceptar");
                        _btn.IsEnabled = true;
                    }
                }
                else
                {
                    Lbl_Mensaje.Text = "Revisar numeros de CVC";
                    await DisplayAlert("Error", "Revisar numeros de CVC", "Aceptar");
                    _btn.IsEnabled = true;
                }
            }
            else
            {
                Lbl_Mensaje.Text = "Revisar numeros de EXPIRACION";
                await DisplayAlert("Error", "Revisar numeros de EXPIRACION", "Aceptar");
                _btn.IsEnabled = true;
            }
        }
        private void Fn_Fecha(object sender, TextChangedEventArgs e)
        {
            if(e.OldTextValue.Length==1 && e.NewTextValue.Length==2)
            {
                Fecha.Text=Fecha.Text.Insert(2,"/");
            }
            else if ((e.OldTextValue.Length == 2 && e.NewTextValue.Length == 3 )  &&  e.NewTextValue.Last()   !='/'  )
            {
                Fecha.Text = Fecha.Text.Insert(2, "/");
            }
            else if(e.OldTextValue.Length==4 && e.NewTextValue.Length==3)
            {
                Fecha.Text=Fecha.Text.Remove(2);
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
    }
}