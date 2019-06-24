using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Trato.Models;


namespace Trato.Varios
{
    public enum EstadoCita
    {
        Terminada=0,
        Nueva=1,
        /// <summary>
        /// esperando respuesta del paciente
        /// </summary>
        Pendiente_respuesta_del_paciente = 2,
        Aceptada = 3,
        Cancelada = 4,
        /// <summary>
        /// esperando respuyesta del medico
        /// </summary>
        Pendiente_respuesta_del_medico = 5
    }
    public class Pagar
    {
        /*membre
letra
 consecutivo
 costo
 nombre que membresia*/
       [JsonProperty("membre")]
       public string v_membresia { get; set; }
        [JsonProperty("letra")]
       public string v_letra { get; set; }
       [JsonProperty("consecutivo")]
       public string v_conse { get; set; }
       [JsonProperty("costo")]
       public string v_costo { get; set; }
       [JsonProperty("nombre")]
       public string v_nombre { get; set; }
        public Pagar()
        {
            v_membresia = string.Empty;
            v_letra = string.Empty;
            v_conse = string.Empty;
            v_costo = string.Empty;
            v_nombre = string.Empty;
        }
        public Pagar(string _membre, string _letra, string _conse,  string _costo, string _nombre)
        {
            v_membresia = _membre;
            v_letra = _letra;
            v_conse = _conse;
            v_costo = _costo;
            v_nombre = _nombre;
        }
    }
    public class Banner
    {
        [JsonProperty("img")]
        public string v_img { get; set; }
        [JsonProperty("sitio")]
        public string v_sitio { get; set; }
        public Banner()
        {
            v_img = "";
            v_sitio = "";
        }
        public Banner(string _img, string _sitio)
        {
            v_img = _img;
            v_sitio = _sitio;
        }
    }
    /// <summary>
    /// clase para mostrar en el perfil, los que estan activos o no
    /// </summary>
    public class Medicamentos
    {
        /// <summary>
        /// nombre del medicamento
        /// </summary>
        [JsonProperty("nombre")]
        public string v_nombre { get; set; }
        [JsonProperty("dosis")]
        public string v_dosis { get; set; }
        [JsonProperty("periodo")]
        /// <summary>
        /// por cuantos dias 
        /// </summary>
        public float v_periodo { get; set; }
        [JsonProperty("tiempo")]
        /// <summary>
        /// cada cuantas horas
        /// </summary>
        public float v_tiempo { get; set; }
        [JsonProperty("extra")]
        public string v_extra { get; set; }
        [JsonProperty("ID_cita")]
        public string v_idCita { get; set; }
        [JsonProperty("id")]
        public string v_idMedi { get; set; }
        [JsonProperty("estado")]
        public string v_estado { get; set; }
        [JsonIgnore]
        public string v_texto { get; set; }

        public void Fn_SetTexto()
        {
            if(v_estado=="0")//todavia no empieza a tomar
            {
                v_texto = "Comenzar tratamiento";
            }
            else if(v_estado=="1")//ya tomandose
            {
                v_texto = "Terminar tratamiento";
            }else if(v_estado=="2")//ya se acabo
            {
                v_texto = "Terminado";
            }
        }
        public string Fn_Info()
        {
            string _info = "";
            _info = "nombre " + v_nombre + "\n periodo " + v_periodo + "\n tiempo " + v_tiempo + "\n extra " + v_extra;
            return _info;
        }
    }
    public class Perf
    {
        [JsonProperty("idmembre")]
        public string v_membre { get; set; }
        [JsonProperty("idfolio")]
        public string v_fol { get; set; }
        [JsonProperty("letra")]
        public string v_letra { get; set; }
        public string Fn_GetDatos()
        {
            string _sa;
            _sa = "membre " + v_membre + "\n" +
                "folio" + v_fol + "\n" +
                "letra" + v_letra+"\n";
            return _sa;
        }
    }
    public class PrefFil
    {
        public string v_texto { get; set; }
    }
    public class C_NotaMed
    {
        /// <summary>
        /// membresia completa  1810I-0558
        /// </summary>
        [JsonProperty("ID_paciente")]
        public string v_pacienteId { get; set; }
        /// <summary>
        /// folio del usuario
        /// </summary>
        [JsonProperty("folio")]
        public string v_folio { get; set; }
        [JsonProperty("ID_cita")]
        public string v_idCita { get; set; }
        [JsonProperty("cuantos")]
        public string v_cuantos { get; set; }

        [JsonProperty("nombreDr")]
        public string v_nombreDr { get; set; }
        [JsonProperty("titulo")]
        public string v_titulo { get; set; }
        /// <summary>
        /// LISTA DE MEDICAMENTOS
        /// </summary>
        [JsonProperty("medicamentos")]
        public ObservableCollection<Medicamentos> v_medic = new ObservableCollection<Medicamentos>();
        public C_NotaMed() { }
        public C_NotaMed(string _paci, string _folio, string _idcita, string _cuantos, ObservableCollection<Medicamentos> _medi)
        {
            v_pacienteId = _paci;
            v_folio = _folio;
            v_idCita = _idcita;
            v_cuantos = _cuantos;
            v_medic = _medi;
        }
    }
}
