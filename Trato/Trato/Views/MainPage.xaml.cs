using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Trato.Personas;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
             //FN_Red();
            //List<string> keyList = new List<string>(Application.Current.Properties.Keys);
            //for(int i=0; i<keyList.Count; i++)
            //{
            //    perfil.Text += "\n" + keyList[i] ;
            //}
            //perfil.Text = Application.Current.Properties["log"] as string;
            //perfil.Text +="\n"+ Application.Current.Properties["membre"] as string;
            //perfil.Text += "\n" + Application.Current.Properties["folio"] as string;
            //perfil.Text += "\n" + Application.Current.Properties["perfGen"] as string;
            //perfil.Text += "\n" + Application.Current.Properties["perfMed"] as string;
            //perfil.Text += "\n" + Application.Current.Properties["servicios"] as string;
            //perfil.Text += "\n" + Application.Current.Properties["medicos"] as string;
            //// log    membre   folio   perfGen  perMed   servicios  medicos
        }
        public async void FN_Red()
        {
            HttpClient _cliente= new HttpClient();
            string url = "https://useller.com.mx/trato_especial/prueba_json.php";
            try
            {
                HttpResponseMessage _respuestphp=  await _cliente.PostAsync(url,null);
                perfil.Text = "enviado";
                string _respu =await  _respuestphp.Content.ReadAsStringAsync();
                ObservableCollection<C_Medico> _med = JsonConvert.DeserializeObject<ObservableCollection<C_Medico>>(_respu);
                 App.v_medicos = _med;
                perfil.Text +="\n\n\n llega "+ _respu;
                //C_Medico _med = JsonConvert.DeserializeObject<C_Medico>(_respu);
                //perfil.Text = _med.FN_GetInfo();
            }
            catch(HttpRequestException _ex)
            {
                perfil.Text = _ex.Message;
                if (Application.Current.Properties.ContainsKey("medicos"))
                {
                }
            }

        }

    }
}