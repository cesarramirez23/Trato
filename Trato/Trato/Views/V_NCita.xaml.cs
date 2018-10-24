using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Personas;
using Trato.Varios;
using Newtonsoft.Json;
using System.Net.Http;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_NCita : ContentPage
	{
        C_Medico v_medico;
        public V_NCita(bool _nueva, C_Medico _medico)
        {
            InitializeComponent();
            if (_nueva)
            {
                v_medico = _medico;
                v_boton.IsVisible = true;
                v_nombre.Text = v_medico.v_completo;
                v_fecha.MinimumDate = DateTime.Now;
                v_fecha.MaximumDate = DateTime.Today.AddMonths(1);
                v_estado.SelectedIndex = 1;
            }
		}
        public async void Fn_Crear(object sender, EventArgs _args)
        {
            Cita _cita = new Cita(v_medico.v_membre, App.v_membresia, App.v_folio, "1",v_fecha.Date,
             v_hora.Time, App.Fn_GEtToken());
            string _json = JsonConvert.SerializeObject(_cita, Formatting.Indented);
            Console.Write("Info cita " + _json);
            HttpClient _client = new HttpClient();
            string _DirEnviar = "";
            StringContent _content= new StringContent(_json, Encoding.UTF8, "application/json");

            try
            {  //getting exception in the following line    //HttpResponseMessage upd_now_playing = await cli.PostAsync(new Uri("http://ws.audioscrobbler.com/2.0/", UriKind.RelativeOrAbsolute), tunp);
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await DisplayAlert("Exito", "Cita generada correctamente, espera la respuesta de tu doctor", "Aceptar");
                    await Navigation.PopAsync();
                }
            }
            catch(HttpRequestException ex)
            {
                    await DisplayAlert("Error",ex.ToString(), "Aceptar");
            }
        }

	}
}