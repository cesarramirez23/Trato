using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Contacto : ContentPage
	{
		public V_Contacto ()
		{
			InitializeComponent ();
        }

        public async void Fn_Enviar(object sender, EventArgs _args)
        {
            if(Fn_Condicione())
            {

            string json = @"{";
            json += "nombre:'" + v_nombre.Text + "',\n";
                json += "correo:'" + v_correo.Text + "',\n";
                json += "mensaje:'" + v_mensaje.Text + "',\n";
                json += "}";

                JObject jsonObj = JObject.Parse(json);
                StringContent _content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");
                HttpClient _client = new HttpClient();
               // string _url = "https://useller.com.mx/trato_especial/update_perfil.php";
                try
                {
                    await DisplayAlert("Enviado", jsonObj.ToString(), "Aceptar");
                    //HttpResponseMessage _respuestphp = await _client.PostAsync(_url, _content);
                    //string _result = _respuestphp.Content.ReadAsStringAsync().Result;
                    //if(_result=="1")
                    //{

                    //}
                    //else if(_result=="0")
                    //{
                    //    await DisplayAlert("Exito ", "Correo Enviado","Aceptar");
                    //    v_nombre.Text = "";
                    //    v_correo.Text = "";
                    //    v_mensaje.Text = "";
                    //}
                    //else
                    //{
                    //    await DisplayAlert("Error", _result,"Aceptar");
                    //}
                }
                catch
                {
                    await DisplayAlert("Error", "Ocurrió un error", "Aceptar");
                }


            }
            else
            {

            }

        }
        public bool Fn_Condicione()
        {
            int cont = 0;
            if (string.IsNullOrEmpty(v_nombre.Text) || string.IsNullOrWhiteSpace(v_nombre.Text))
            {
                v_nombre.BackgroundColor = Color.Red;  cont++;
            }
            else
            {
                v_nombre.BackgroundColor = Color.Transparent;
            }

            //correo
            Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (string.IsNullOrEmpty(v_correo.Text) || string.IsNullOrWhiteSpace(v_correo.Text) || !EmailRegex.IsMatch(v_correo.Text))
            {
                v_correo.BackgroundColor = Color.Red; cont++;
            }
            else
            {
                v_correo.BackgroundColor = Color.Transparent;
            }
            //mensaje
            if (string.IsNullOrEmpty(v_mensaje.Text) || string.IsNullOrWhiteSpace(v_mensaje.Text))
            {
                v_mensaje.BackgroundColor = Color.Red; cont++;
            }
            else
            {
                v_mensaje.BackgroundColor = Color.Transparent;
            }
            if(cont>0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }





    }
}