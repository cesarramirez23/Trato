using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;



namespace Trato.Varios
{
    public class C_EspeTitu
    {
        [JsonProperty("ID_especialidad")]
        public string v_idespecial { get; set; }
        [JsonProperty("nombre_especialidad")]
        public string v_nombreEspec { get; set; }

        [JsonProperty("ID_titulo")]
        public string v_idtitulo { get; set; }
        [JsonProperty("nombre_titulo")]
        public string v_nombreTitulo { get; set; }
        public bool v_visible { get; set; }

    }
    public class C_Notificacion
    {
        public C_Notificacion()
        { }
        public C_Notificacion(string _titulo, string _body)
        {
            v_titulo = _titulo;
            string temp = _body.Replace("\n", Environment.NewLine);
            v_cuerpo = temp;
        }
        [JsonProperty("estado")]
        public string v_estado { get; set; }
        //Lo que se vaa a mostrar
        [JsonProperty("title")]
        public string v_titulo { get; set; }
        [JsonProperty("message")]
        public string v_cuerpo { set; get; }
    }
    public enum EstadoCita
    {
        Terminada=0,
        Nueva=1,
        Pendiente=2,
        Aceptada=3,
        Cancelada=4
    }
    public class Cita
    {
        /// <summary>
        /// membresia completa
        /// </summary>
        [JsonProperty("ID_Dr")]
        public string v_doctorId { get; set; }
        [JsonProperty("espe")]
        public ObservableCollection<C_EspeTitu> v_espe= new ObservableCollection<C_EspeTitu>();
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
        /// <value>The v tipo.</value>
        [JsonProperty("tipo")]
        public string v_tipo { get; set; }
        [JsonProperty("ID_cita")]
        public string v_idCita { get; set; }

        public Cita() { v_estado = "-1"; }
        public Cita(string _membre, string _folio, string _tipo)
        {
            v_tipo = _tipo;
            if(_tipo=="0")
            {
                v_pacienteId = _membre;
                v_folio = _folio;
            }
            else if(_tipo=="1"){
                v_doctorId = _membre;
            }
        }
        public Cita(string _membredr, string _membrepac, string _folio, string _estado, DateTime _fecha, TimeSpan _hora, string _tokenDr, string _tokenpac)
        {

        }
        /// <summary>
        /// para crear el json a enviar
        /// </summary>
        /// <param name="_membredr"></param>
        /// <param name="_membrepac"></param>
        /// <param name="_folio"></param>
        /// <param name="_estado"></param>
        /// <param name="_fecha"></param>
        /// <param name="_hora"></param>
        /// <param name="_tokenPac"></param>
        public Cita(string _membredr, string _membrepac, string _folio, string _estado, DateTime _fecha, TimeSpan _hora, string _tokenPac)
        {
            v_doctorId = _membredr;
            v_pacienteId = _membrepac;
            v_folio = _folio;
            v_estado = _estado;
            v_hora = _hora;
            v_tokenPaciente = _tokenPac;
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
        }
        /// <summary>
        /// para el update, tipo al que se le envia
        /// </summary>
        /// <param name="_estado"></param>
        /// <param name="_fecha"></param>
        /// <param name="_hora"></param>
        /// <param name="_idcita"></param>
        public Cita(string _estado, DateTime _fecha, TimeSpan _hora, string _idcita,string _tipo)
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
        /// <param name="_estado"></param>
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

        /// <summary>
        /// el string  Terminada=0,Nueva=1,Pendiente=2,Aceptada=3,Cancelada=4
        /// </summary>
        public string v_Estadocita { get; set; }
        /// <summary>
        /// para cambiar el color dentro de la lista visible, cambia estado cita, y formato de la fecha
        /// </summary>
        /// <param name="_valor"></param>       
        public void Fn_CAmbioCol(int _valor)
        {
            if ((_valor % 2) == 1)
            {
                v_color =Color.FromHex("F2F2F2");
            }
            else
            {
                v_color = Color.White;
            }
            int _a = int.Parse(v_estado);
            v_Estadocita = ((EstadoCita)_a).ToString();
            string[] _fecha = v_fecha.Split('-');//month day year
            v_fechaDate = new DateTime(int.Parse(_fecha[0]), int.Parse(_fecha[1]), int.Parse(_fecha[2]),
                                         v_hora.Hours, v_hora.Minutes, v_hora.Seconds);
        }
        public void Fn_SetValores()
        {
           int _a = int.Parse(v_estado);
            v_Estadocita = ((EstadoCita)_a).ToString();
            /*if (v_fechaDate== null)
            {
            }*/
            string[] _fecha = v_fecha.Split('-');//month day year
            v_fechaDate = new DateTime(int.Parse(_fecha[0]), int.Parse(_fecha[1]), int.Parse(_fecha[2]),
                                       v_hora.Hours, v_hora.Minutes, v_hora.Seconds);
            v_especialidad = "";
            for(int i=0; i<v_espe.Count; i++)
            {
                v_especialidad += v_espe[i].v_nombreEspec;
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
    public class Filtro
    {
        public string v_texto { get; set; }
        public Color v_color { get; set; }
        public bool v_visible { get; set; }

        public Filtro()
        {
            v_texto = "";
            v_color = new Color(.15686274509, 0.58823529411, 0.81960784313);
        }
        public Filtro(string _texto)
        {
            v_texto = _texto;
            v_color = new Color(.15686274509, 0.58823529411, 0.81960784313);
        }
        public Filtro(string _texto, bool _Activo)
        {
            v_texto = _texto;
            if (_Activo)
            {
                v_color = Color.Red;
            }
            else
            {
                v_color = new Color(.15686274509, 0.58823529411, 0.81960784313);
            }
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
