using System;
using Trato.Varios;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Newtonsoft.Json;
namespace Trato.Models
{
    public class Cita
    {
        /// <summary>
        /// membresia completa
        /// </summary>
        [JsonProperty("ID_Dr")]
        public string v_doctorId { get; set; }
        [JsonProperty("espe")]
        public ObservableCollection<C_EspeTitu> v_espe = new ObservableCollection<C_EspeTitu>();
        public string v_especialidad { get; set; }
        /// <summary>
        /// membresia completa  1810I-0558
        /// </summary>
        [JsonProperty("ID_paciente")]
        public string v_pacienteId { get; set; }
        /// <summary>
        /// folio
        /// </summary>
        [JsonProperty("folio")]
        public string v_folio { get; set; }
        /// <summary>
        /// Terminada=0,Nueva=1,Pendiente=2,Aceptada=3,Cancelada=4
        /// </summary>
        [JsonProperty("estado")]
        public string v_estado { get; set; }
        /// <summary>
        /// yyyy-mm-dd
        /// </summary>
        [JsonProperty("fecha")]
        public string v_fecha { get; set; }
        [JsonIgnore]
        public DateTime v_fechaDate { get; set; }
        [JsonProperty("hora")]
        public TimeSpan v_hora { get; set; }
        [JsonProperty("nombreDr")]
        public string v_nombreDR { get; set; }
        [JsonProperty("nombrePaciente")]
        public string v_nombrePaciente { get; set; }
        [JsonProperty("tokenDr")]
        public string v_tokenDR { get; set; }
        [JsonProperty("tokenPaciente")]
        public string v_tokenPaciente { get; set; }
        /// <summary>
        /// 0 paciente 1 doctor
        /// </summary>
        [JsonProperty("tipo")]
        public string v_tipo { get; set; }
        [JsonProperty("ID_cita")]
        public string v_idCita { get; set; }
        [JsonProperty("idcalendario")]
        public string v_idCalendar;
        public Cita() { v_estado = "-1"; }
        public Cita(string _membre, string _folio, string _tipo)
        {
            v_tipo = _tipo;
            if (_tipo == "0")
            {
                v_pacienteId = _membre;
                v_folio = _folio;
            }
            else if (_tipo == "1")
            {
                v_doctorId = _membre;
            }
        }
        public Cita(string _membredr, string _membrepac, string _folio, string _estado, DateTime _fecha, TimeSpan _hora, string _tokenDr, string _tokenpac)
        {

        }
        /// <summary>
        /// para crear el json a enviar
        /// </summary>
        public Cita(string _membredr, string _membrepac, string _folio, string _estado, DateTime _fecha, TimeSpan _hora, string _tokenDoc)
        {
            v_doctorId = _membredr;
            v_pacienteId = _membrepac;
            v_folio = _folio;
            v_estado = _estado;
            v_hora = _hora;
            v_tokenDR = _tokenDoc;
            string _month = "";
            if (_fecha.Month < 10)
            {
                _month = "0" + _fecha.Month.ToString();
            }
            else
            {
                _month = _fecha.Month.ToString();
            }
            string _day = "";
            if (_fecha.Day < 10)
            {
                _day = "0" + _fecha.Day.ToString();
            }
            else
            {
                _day = _fecha.Day.ToString();
            }
            v_fecha = _fecha.Year.ToString() + "-" + _month + "-" + _day;
            v_fechaDate = _fecha;
        }
        /// <summary>
        /// para el update, tipo al que se le envia
        /// </summary>
        public Cita(string _estado, DateTime _fecha, TimeSpan _hora, string _idcita, string _tipo)
        {
            v_idCita = _idcita;
            v_estado = _estado;
            v_hora = _hora;
            string _month = "";
            if (_fecha.Month < 10)
            {
                _month = "0" + _fecha.Month.ToString();
            }
            else
            {
                _month = _fecha.Month.ToString();
            }
            string _day = "";
            if (_fecha.Day < 10)
            {
                _day = "0" + _fecha.Day.ToString();
            }
            else
            {
                _day = _fecha.Day.ToString();
            }
            v_fecha = _fecha.Year.ToString() + "-" + _month + "-" + _day;
            v_tipo = _tipo;
        }
        /// <summary>
        /// prueba de notification
        /// </summary>
        public Cita(string _estado)
        {
            v_doctorId = "1808D-0008";
            v_pacienteId = "1810I-0558";
            v_folio = "0";
            v_estado = _estado;
            v_hora = DateTime.Now.TimeOfDay;
            v_fecha = DateTime.Now.Date.Year + "-" + DateTime.Now.Date.Month + "-" + DateTime.Now.Date.Day;
            v_fechaDate = DateTime.Now.Date;

            v_nombreDR = "prueba notif";
            v_nombrePaciente = "Prueba notif paciente";
        }
        /// <summary>
        /// en la opantalla de citas colores
        /// </summary>
        [JsonIgnore]
        public Color v_color { get; set; }
        [JsonIgnore]
        public bool v_visible { get; set; }
        /// <summary>
        /// el string  Terminada=0,Nueva=1,Pendiente=2,Aceptada=3,Cancelada=4
        /// </summary>
        public string v_Estadocita { get; set; }
        /// <summary>
        /// para cambiar el color dentro de la lista visible, cambia estado cita, y formato de la fecha
        /// </summary>   
        public void Fn_CAmbioCol(int _valor)
        {
            if ((_valor % 2) == 1)
            {
                v_color = Color.FromHex("F2F2F2");
            }
            else
            {
                v_color = Color.White;
            }
            int _a = int.Parse(v_estado);
            v_Estadocita = ((EstadoCita)_a).ToString().Replace('_', ' ');
            string[] _fecha = v_fecha.Split('-');//month day year
            v_fechaDate = new DateTime(int.Parse(_fecha[0]), int.Parse(_fecha[1]), int.Parse(_fecha[2]),
                                         v_hora.Hours, v_hora.Minutes, v_hora.Seconds);
        }
        public void Fn_SetValores()
        {
            int _a = int.Parse(v_estado);
            v_Estadocita = ((EstadoCita)_a).ToString().Replace('_', ' ');
            /*if (v_fechaDate== null)
            {
            }*/
            string[] _fecha = v_fecha.Split('-');//month day year
            v_fechaDate = new DateTime(int.Parse(_fecha[0]), int.Parse(_fecha[1]), int.Parse(_fecha[2]),
                                       v_hora.Hours, v_hora.Minutes, v_hora.Seconds);
            v_especialidad = "";
            for (int i = 0; i < v_espe.Count; i++)
            {
                v_especialidad += v_espe[i].v_nombreEspec;
            }
        }
        public void Fn_SetVisible()
        {
            if (v_estado == "0")
            {
                v_visible = false;
            }
            else
            {
                v_visible = true;
            }
        }
        public string Fn_GetInfo()
        {
            string _ret = v_doctorId + "   " + v_especialidad + "  " + v_pacienteId + " " + v_folio + " " + v_estado + " " + v_fecha + " " +
                v_fechaDate + " " + v_hora + " " + v_nombreDR + " " + v_nombrePaciente + " " + v_tokenDR + " " +
                v_tokenPaciente + " " + v_tipo + " " + v_idCita;
            return _ret;
        }
    }
}
