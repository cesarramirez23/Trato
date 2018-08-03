using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


namespace Trato.Views
{
    /*PARA CREAR LA CONSTRASEÑA SIN NECESIDAD DE CLASES
 * 
 * string json = @"{
  CPU: 'Intel',
  Drives: [
    'DVD read/writer',
    '500 gigabyte hard drive'
  ]
}";

JObject o = JObject.Parse(json);


 */
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Opciones : TabbedPage
	{
        string v_validar = "";
        public V_Opciones ()
		{
			InitializeComponent ();
		}
        public void FN_passCambio(object sender, TextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(P_Nueva.Text) || string.IsNullOrWhiteSpace(P_Nueva.Text))
            {
                P_but.IsEnabled = false;
                P_mensaje.IsVisible = true;
                P_mensaje.Text = "Este campo no puede estar vacio o con espacios";
            }
            else
            {
                if (P_Nueva.Text != P_Nueva2.Text)
                {
                    P_but.IsEnabled = false;
                    P_mensaje.Text = "Contraseña no coincide";
                }
                else
                {
                    P_but.IsEnabled = true;
                    P_mensaje.Text = "Contraseña correcta";

                }
            }
        }
        public async void Fn_CambioPass(object sender, EventArgs _args)
        {
            if (Fn_validar(P_actual.Text, P_Nueva.Text))
            {
                await DisplayAlert("bien", "bien", "bien");
            }
            else
            {
                await DisplayAlert("Error", v_validar, "Aceptar");

            }
        }

        public bool Fn_validar(string _actual, string _nueva)
        {
            if (_actual == _nueva)
            {
                v_validar = "La nueva contraseña no puede ser la misma que la actual";
                return false;
            }
            else
            {
                Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\w)[A-Za-z\w]{8,}$");
                if (!regex.IsMatch(_nueva))
                {
                    v_validar = "Debe contener al menos una mayuscula,una minuscula y un numero";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
    
}