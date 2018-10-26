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
        Cita v_cita;
        bool v_nueva;
        public V_NCita( C_Medico _medico)
        {
            InitializeComponent();
            v_nueva = true;

                v_medico = _medico;
                v_boton.IsVisible = true;
                v_nombre.Text = v_medico.v_completo;
                v_fecha.MinimumDate = DateTime.Now;
                v_fecha.MaximumDate = DateTime.Today.AddMonths(1);
                v_estado.SelectedIndex = 1;
            
		}
        public V_NCita(Cita _cita)
        {
            InitializeComponent();
            v_nueva = false;
            v_cita = _cita;
            v_cita.Fn_SetValores();
            v_fecha.Date = v_cita.v_fechaDate;
            v_fecha.IsEnabled = false;
            v_hora.IsEnabled = false;
            v_hora.Time = v_cita.v_hora;
            v_estado.SelectedIndex = int.Parse(v_cita.v_estado);
            v_estado.IsEnabled = true;
            v_nombre.Text = v_cita.v_nombreDR;
            v_boton.Text = "Actualizar";
        }
        public async void Fn_Crear(object sender, EventArgs _args)
        {
            if(v_nueva)
            {
                Cita _cita = new Cita(v_medico.v_membre, App.v_membresia, App.v_folio, "1",v_fecha.Date,
                 v_hora.Time, App.Fn_GEtToken());
                string _json = JsonConvert.SerializeObject(_cita, Formatting.Indented);
                Console.Write("Info cita " + _json);
                await DisplayAlert("Enviar", _json, "aceptar");
                HttpClient _client = new HttpClient();
                string _DirEnviar = "http://tratoespecial.com/set_citas.php";
                StringContent _content= new StringContent(_json, Encoding.UTF8, "application/json");
                try
                {  //getting exception in the following line    //HttpResponseMessage upd_now_playing = await cli.PostAsync(new Uri("http://ws.audioscrobbler.com/2.0/", UriKind.RelativeOrAbsolute), tunp);
                    HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                    if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                        if(_respuesta=="1")
                        {
                            await DisplayAlert("Exito", "Cita generada correctamente, espera la respuesta de tu doctor", "Aceptar");
                            await Navigation.PopAsync();
                        }
                        else 
                        {
                            await DisplayAlert("Error", "No se pudo agendar tu cita, intentalo mas tarde", "Aceptar");
                        }
                    }
                }
                catch(HttpRequestException ex)
                {
                    await DisplayAlert("Error",ex.Message, "Aceptar");
                }
            }
            else//aca actualizar el estado de la cita
            {

            }

        }

	}
}