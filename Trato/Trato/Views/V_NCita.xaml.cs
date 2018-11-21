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
        string _esta;
        public V_NCita( C_Medico _medico)
        {
            InitializeComponent();
            v_nueva = true;
            v_medico = _medico;
            v_botCrear.IsVisible = true;
            v_nombre.Text = v_medico.v_completo;
            v_fecha.MinimumDate = DateTime.Now;
            v_fecha.MaximumDate = DateTime.Today.AddMonths(1);
            v_estado.Text = (EstadoCita.Nueva).ToString();
            StackPendiente.IsVisible = false;
            StackTre.IsVisible = false;
        }
        public V_NCita(Cita _cita)
        {
            InitializeComponent();
            v_nueva = false;
            v_botCrear.IsVisible = false;
            v_cita = _cita;
            v_cita.Fn_SetValores();
            v_fecha.Date = v_cita.v_fechaDate;
            v_fecha.IsEnabled = false;
            v_hora.IsEnabled = false;
            v_hora.Time = v_cita.v_hora;
            _esta = v_cita.v_estado;
            int _a = int.Parse(v_cita.v_estado);
            v_estado.Text = ((EstadoCita)_a).ToString();
            v_nombre.Text = v_cita.v_nombreDR;
            v_fecha.MinimumDate = DateTime.Now;
            v_fecha.MaximumDate = DateTime.Today.AddMonths(1);
            Fn_Botones(v_cita.v_estado);
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
        /// <summary>
        /// Terminada = 0,  Nueva = 1, Pendiente = 2, Aceptada = 3, Cancelada = 4
        /// </summary>
        /// <param name="_valor"></param>
        private void Fn_Botones(string _valor)
        {
            switch (_valor)
            {
                case "0"://TErminada, ya paso
                    {
                        v_botAcep.IsEnabled = false;
                        v_botCambio.IsEnabled = false;
                        v_botRec.IsEnabled = false;
                    }
                    break;
                case "1"://nueva
                    {
                        v_botAcep.IsEnabled = false;
                        v_botCambio.IsEnabled = false;
                        v_botRec.IsEnabled = true;
                    }
                    break;
                case "2"://pendiente
                    {
                        v_botAcep.IsEnabled = true;
                        v_botCambio.IsEnabled = true;
                        v_botRec.IsEnabled = true;
                    }
                    break;
                case "3"://aceptada
                    {
                        v_botAcep.IsEnabled = false;
                        v_botCambio.IsEnabled = false;
                        v_botRec.IsEnabled = true;
                    }
                    break;
                case "4"://cancelada
                    {
                        v_botAcep.IsEnabled = false;
                        v_botCambio.IsEnabled = false;
                        v_botRec.IsEnabled = false;
                    }
                    break;
            }
        }
        /// <summary>
        /// Terminada = 0,  Nueva = 1, Pendiente = 2, Aceptada = 3, Cancelada = 4
        /// </summary>
        /// <param name="_nuevoestado"></param>
        private async void Fn_ActualizarInfo(string _nuevoestado)
        {
            Fn_Botones("4");
            Cita _cita = new Cita(_nuevoestado, v_fecha.Date, v_hora.Time, v_cita.v_idCita);
            string _json = JsonConvert.SerializeObject(_cita, Formatting.Indented);
          //  await DisplayAlert("Enviar", _json, "aceptar");
            HttpClient _client = new HttpClient();
            string _DirEnviar = "http://tratoespecial.com/update_citas.php";
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            try
            {  //getting exception in the following line    //HttpResponseMessage upd_now_playing = await cli.PostAsync(new Uri("http://ws.audioscrobbler.com/2.0/", UriKind.RelativeOrAbsolute), tunp);
                HttpResponseMessage _respuestaphp = await _client.PostAsync(_DirEnviar, _content);
                if (_respuestaphp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                    if (_respuesta == "1")
                    {
                        await DisplayAlert("Exito", "Cambios generados correctamente", "Aceptar");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo agendar tu cita, intentalo mas tarde", "Aceptar");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
            Fn_Botones("1");
        }
        private void Fn_Acep(object sender, EventArgs _Args)
        {
            Fn_ActualizarInfo("3");
        }
        private void Fn_Recha(object sender, EventArgs _Args)
        {
            Fn_ActualizarInfo("4");
        }
        /// <summary>
        /// Cuando quieres cambiar la fecha/hora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_Args"></param>
        private void Fn_Cambios(object sender, EventArgs _Args)
        {
            v_fecha.IsEnabled = true;
            v_hora.IsEnabled = true;
            StackPendiente.IsVisible = true;
            StackTre.IsVisible = false;
        }
        /// <summary>
        /// hacer los cambios en el calendario y reloj, luego aceptar los cambios
        /// </summary>
        private void Fn_AcepCambio(object sender, EventArgs _args)
        {
            StackPendiente.IsVisible = false;
            StackTre.IsVisible = true;
            Fn_ActualizarInfo("2");
        }
        private void Fn_CancelCambio(object sender, EventArgs _args)
        {
            v_fecha.Date = v_cita.v_fechaDate;
            v_fecha.IsEnabled = false;
            v_hora.Time = v_cita.v_hora;
            v_hora.IsEnabled = false;
            Fn_Botones(_esta);
            StackPendiente.IsVisible = false;
            StackTre.IsVisible = true;
        }
    }
}