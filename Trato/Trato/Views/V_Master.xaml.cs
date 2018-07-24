using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using Trato.Personas;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Master : MasterDetailPage
	{/*
        las funciones se le agregan en lugar de mandar un view se lo agregas a master.detail
         */

        bool v_primero = false;




		public V_Master()
		{
			InitializeComponent ();
            v_primero = false;
            Browser.Source = "https://www.alsain.mx/trato_especial/pre_tarjeta_alta.php";

            

           // A();
        }
        public async void A()
        {
            //C_Ind_Fisica fisi = new C_Ind_Fisica("nombresdsa", "rfcdfsf", new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc), "luga naci", "ocupacsdsad",
            //    "2134535", "345667", "calle dssads", "23", "23", "colonisadsad", "ciudad dsfdfdf", "muni dsadsdf", "estado dsad", "2345",
            //    "correro sdfdgf", 0);
            //C_Ind_Moral fisi = new C_Ind_Moral("nombresdsa", "rfcdfsf", "ocupacsdsad",
            //   "2134535", "calle dssads", "23", "23", "colonisadsad", "ciudad dsfdfdf", "muni dsadsdf", "estado dsad", "2345",
            //   "correro sdfdgf", 0);

            //C_Tarjeta fisi = new C_Tarjeta("nombre", "correo sadasd", "2134567", "personal membresia", "2344", "nombre en la tarjeta", "1223456124356567",
            //    "234", "12", "12");
            // string _jsonTar = JsonConvert.SerializeObject(fisi, Formatting.Indented);

            //HttpClient _client = new HttpClient();
            ////HACER ESTO PARA QUE ESTE EN FORMATO PARA ENVIAR

            //StringContent _contTar = new StringContent(_jsonTar, Encoding.UTF8, "application/json");
            //Uri _uri = new Uri("ww.asf");
            //await _client.PostAsync(_uri, _contTar);

            // texto.Text = _jsonTar;
            await Task.Delay(100);
        } 
        public async void Fn_Enviar(object sender, EventArgs _Args)
        {
            if(!v_primero)
            {
                await Browser.EvaluateJavaScriptAsync("submitbutton()");
                v_primero = true;
                await Task.Delay(2000);
            }
           string _result= await Browser.EvaluateJavaScriptAsync("submitbutton()");

                await Task.Delay(1000);
            await DisplayAlert("regresa","numero "+_result, "nada");
        }
        public void Fn_uno(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage( new V_Buscador(true) { Title = "MEDICOS" });
        }
        public void Fn_dos(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage( new V_Buscador(false) { Title = "SERVICIOS" });//new V_Buscador() { Title = "Buscador" };
        }
        public void Fn_Perfil(object sender, EventArgs _args)
        {
            IsPresented = false;
            Detail = new NavigationPage(new V_Perfil() { Title = "Perfil" });//new V_Buscador() { Title = "Buscador" };
        }
    }
    /*
     07-23 13:10:52.854 I/chromium(25402): [INFO:CONSOLE(0)] "Failed to load https://api.conekta.io/tokens: No 'Access-Control-Allow-Origin' header is present on the requested resource. Origin 'http://www.alsain.mx' is therefore not allowed access. The response had HTTP status code 504.", source: http://www.alsain.mx/trato_especial/pre_tarjeta_alta.php (0)
07-23 13:10:52.957 I/ViewRootImpl(25402): ViewRoot's Touch Event : ACTION_DOWN
07-23 13:10:53.024 I/ViewRootImpl(25402): ViewRoot's Touch Event : ACTION_UP
07-23 13:10:53.053 I/ViewRootImpl(25402): ViewRoot's Touch Event : ACTION_DOWN
07-23 13:10:53.095 I/ViewRootImpl(25402): ViewRoot's Touch Event : ACTION_UP
[INFO:CONSOLE(0)] "Failed to load https://api.conekta.io/tokens: No 'Access-Control-Allow-Origin' header is present on the requested resource. Origin 'http://www.alsain.mx' is therefore not allowed access. The response had HTTP status code 504.", source: http://www.alsain.mx/trato_especial/pre_tarjeta_alta.php (0)
07-23 13:10:53.166 I/chromium(25402): [INFO:CONSOLE(0)] "Failed to load https://api.conekta.io/tokens: No 'Access-Control-Allow-Origin' header is present on the requested resource. Origin 'http://www.alsain.mx' is therefore not allowed access. The response had HTTP status code 504.", source: http://www.alsain.mx/trato_especial/pre_tarjeta_alta.php (0)
Thread finished: <Thread Pool> #2
     
     */

}