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

		public V_Master()
		{
			InitializeComponent ();

            

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

  
}