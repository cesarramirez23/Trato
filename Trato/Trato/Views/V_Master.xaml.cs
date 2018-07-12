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
using System.Data;

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
            A();
        }
        public async void A()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://maps.googleapis.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&key=AIzaSyCbunHvSQc8oP6eailMPOS9a6DJU98Ni9s");
           // var _mensajestring = response.Content.ReadAsStringAsync();            
           //List<string> _listaString= JsonConvert.DeserializeObject<List<string>>(_mensajestring);

           // string json = @"['Starcraft','Halo','Legend of Zelda']";

           // List<string> videogames = JsonConvert.DeserializeObject<List<string>>(json);


        }
        public void Fn_uno()
        {
            Detail = new NavigationPage( new V_Buscador(true) { Title = "MEDICOS" });
        }
        public void Fn_dos()
        {
            Detail = new NavigationPage( new V_Buscador(false) { Title = "SERVICIOS" });//new V_Buscador() { Title = "Buscador" };
        }

    }

  
}