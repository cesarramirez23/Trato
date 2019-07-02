using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Trato.Models;
//using Xamarin.Forms;
namespace Trato.Personas
{
    /* https://forums.xamarin.com/discussion/100135/json-response-parsing-in-xamarin-froms
     * EXPLICA EL FORMATO Y COMO DEBEN ESTAR LOS ATRIBUTOS PARA EL JSON
     * 
     * 
     * EN LA TARJETA CON LOS DATOS DE COBRO ENVIAR EL NOMBRE DE LA MEMBRESIA CON STRING
     * Y TAMBIEN ENVIAR  COMO NUMERO EL TIPO DE MEMBRESiA( DE 0 A 2)
         */
    [Serializable]
    public class C_PerfilGen
    {
        [JsonProperty("nombre")]
        public string v_Nombre { get; set; }
        [JsonProperty("rfc")]
        public string v_Rfc { get; set; }
        [JsonProperty("fechanac")]
        public string v_FecNaci { get; set; }
        [JsonProperty("lugnac")]
        public string v_LugNac { get; set; }
        [JsonProperty("ocu")]
        public string v_Ocup { get; set; }
        [JsonProperty("idsexo")]
        public int v_idsexo { get; set; }
        [JsonProperty("tel")]
        public string v_Tel { get; set; }
        [JsonProperty("cel")]
        public string v_Cel { get; set; }
        [JsonProperty("calle")]
        public string v_Calle { get; set; }
        [JsonProperty("numext")]
        public string v_NumExt { get; set; }
        [JsonProperty("numint")]
        public string v_NumInt { get; set; }
        [JsonProperty("colonia")]
        public string v_Colonia { get; set; }
        [JsonProperty("ciudad")]
        public string v_Ciudad { get; set; }
        [JsonProperty("municipio")]
        public string v_municipio { get; set; }
        [JsonProperty("estado")]
        public string v_Estado { get; set; }
        [JsonProperty("cp")]
        public string v_Cp { get; set; }
        [JsonProperty("correo")]
        public string v_Correo { get; set; }
        [JsonProperty("activado")]
        public string v_activo { get; set; }
        [JsonProperty("fecha_vig")]
        public string v_vig;
        [JsonProperty("total_usuarios")]
        public string v_numEmple { get; set; }
        [JsonProperty("token")]
        public string v_token  { get; set; }
        [JsonProperty("conekta_id")]
        public string v_idConekta { get; set; }
        [JsonProperty("promotor")]
        public string v_promotor { get; set; }


        public C_PerfilGen()
        {
            v_idsexo = -1;
            v_Nombre = "";
            v_Rfc = "";
            v_FecNaci = "0000-00-00";
            v_LugNac = "";
            v_Ocup = "";
            v_Tel = "";
            v_Cel = "";
            v_Calle = "";
            v_NumExt = "";
            v_NumInt = "";
            v_Colonia = "";
            v_Ciudad = "";
            v_municipio = "";
            v_Estado = "";
            v_Cp = "";
            v_Correo= "";
            v_activo = "-23";
            v_vig = "0000-00-00";
        }
        public C_PerfilGen(string _nom, string _rfc, DateTime _fechnac, string _lugnac, string _ocu,int _idsexo,  string _tel, string _cel,
            string _calle, string _numExt, string _numInt, string _col, string _ciud, string _mun, string _est, string _cp, string _corr)
        {
            v_Nombre = _nom;
            v_Rfc = _rfc;
            string _month = "";
            if (_fechnac.Month < 10)
            {
                _month = "0" + _fechnac.Month.ToString();
            }
            else
            {
                _month = _fechnac.Month.ToString();
            }
            string _day = "";
            if (_fechnac.Day < 10)
            {
                _day = "0" + _fechnac.Day.ToString();
            }
            else
            {
                _day = _fechnac.Day.ToString();
            }
            v_FecNaci = _fechnac.Year.ToString() + "-" + _month + "-" + _day;
            v_LugNac = _lugnac;
            v_Ocup = _ocu;
            v_idsexo = _idsexo;
            v_Tel = _tel;
            v_Cel = _cel;
            v_Calle = _calle;
            v_NumExt = _numExt;
            v_NumInt = _numInt;
            v_Colonia = _col;
            v_Ciudad = _ciud;
            v_municipio = _mun;
            v_Estado = _est;
            v_Cp = _cp;
            v_Correo = _corr;
            v_activo = "-23";
        }
        public string Fn_GetDatos()
        {
            string _ret = "";
           _ret= "nombre "+v_Nombre + "  rfc  " + v_Rfc + "  fech nac" + v_FecNaci + " lugnac  " + v_LugNac + " ocu " + v_Ocup + "\n" +
                "id sexo "+ v_idsexo + "  tel " + v_Tel + " cel  " + v_Cel + "  \n" +
                "  calle  "+v_Calle + " numext " + v_NumExt + "  numint " +  v_NumInt + "  " +
                "colonia "+v_Colonia + "ciud  " + v_Ciudad + " muni" + v_municipio + "  esta" + v_Estado + " \n" +
                "cp  "+v_Cp + "  corr " + v_Correo+"   activado  "+ v_activo+"  numemple"+v_numEmple + "vigencia " + v_vig;
            return _ret;
                  
        }
    }
    [Serializable]
    public class C_PerfilMed
    {
        [JsonProperty("sangre")]
        public string v_sangre { get; set; }
        [JsonProperty("idsexo")]
        public int v_sexo { get; set; }
        [JsonProperty("infoMuj")]
        public string v_infoMujer { get; set; }
        [JsonProperty("alergias")]
        public string v_alergias { get; set; }
        [JsonProperty("operaciones")]
        public string v_operaciones { get; set; }
        [JsonProperty("enfermedades")]
        public string v_enfer { get; set; }
        [JsonProperty("medicamentos")]
        public string v_medica { get; set; }
       
        public C_PerfilMed()
        {
            v_sangre = "";
            v_sexo = -1;
            v_infoMujer = "";
            v_alergias = "";
            v_operaciones = "";
            v_enfer = "";
            v_medica = "";
        }
        public C_PerfilMed(int _idsex)
        {
            v_sangre = "";
            v_sexo = _idsex;
            v_infoMujer = "";
            v_alergias = "";
            v_operaciones = "";
            v_enfer = "";
            v_medica = "";
        }
        public C_PerfilMed(string _sangr, string _sexo, string _aler, string _opera, string _enfer, string _medicam)
        {
            v_sangre = _sangr;
            v_infoMujer = _sexo;
            v_alergias = _aler;
            v_operaciones = _opera;
            v_enfer = _enfer;
            v_medica = _medicam;
        }
        public string Fn_Info()
        {
            string ret = v_sangre + "  ---  " + v_sexo + "  ---  " + v_infoMujer + "---" + v_alergias + "----" + v_operaciones + "---" + v_enfer + "---" + v_medica;
            return ret;
        }
    }
    /// <summary>
    /// el regustro para familiares y empleados
    /// </summary>
    class C_RegistroSec
    {
        #region datos generales
        [JsonProperty("nombre")]
        string v_nombre { get; set; }
        [JsonProperty("sexo")]
        int v_sexo { get; set; }
        [JsonProperty("fecha")]
        string v_fecha { get; set; }
        [JsonProperty("cel")]
        string v_celular { get; set; }
        [JsonProperty("correo")]
        string v_correo { get; set; }
        [JsonProperty("pass")]
        string v_password { get; set; }
        [JsonProperty("idmembre")]
        string v_membre { get; set; }
        [JsonProperty("id")]
        int v_id { get; set; }
        #endregion
        #region Datos de la familiar
        [JsonProperty("parentesco")]
        string v_parentesco { get; set; }
        #endregion
        #region datos especificos de empresarial
        [JsonProperty("empresa")]
        string v_empresa { get; set; }
        [JsonProperty("folio")]
        string v_folio { get; set; }
        #endregion

        public C_RegistroSec ()
        {
            v_sexo = -1;
        }
        public C_RegistroSec(string _nombre,int _sexo, DateTime _fecha, string _cel, string _correo,string _pass,string _membre,  int _id, string _paren)
        {
            v_nombre = _nombre;
            v_sexo = _sexo;
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
            v_celular = _cel;
            v_correo = _correo;
            v_password = _pass;
            v_membre = _membre;
            v_id = _id;
            v_parentesco = _paren;
            v_empresa = "";
            v_folio = "";

        }
        public C_RegistroSec(string _nombre, int _sexo, DateTime _fecha, string _cel, string _correo, string _pass, string _membre, int _id,string _empr, string _fol)
        {
            v_nombre = _nombre;
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
            v_celular = _cel;
            v_correo = _correo;
            v_password = _pass;
            v_membre = _membre;
            v_id = _id;
            v_parentesco = "";
            v_empresa = _empr;
            v_folio = _fol;
        }

    }
    /// <summary>
    /// registro total del principal, vienen los datos para pagar
    /// </summary>
    class C_RegistroPrinci
    {
        #region DAtos del usuario a registrar
        [JsonProperty("nombre")]
        string v_Nombre { get; set; }
        [JsonProperty("rfc")]
        string v_Rfc { get; set; }
        /// <summary>
        /// Fecha de nacimiento, tiene que ser dia mes año
        /// </summary>
        [JsonProperty("fechanac")]
        string v_FecNaci { get; set; }
        /// <summary>
        /// lugar de nacimiento
        /// </summary>
        [JsonProperty("lugnac")]
        string v_LugNac { get; set; }
        [JsonProperty("ocu")]
        string v_Ocup { get; set; }
        [JsonProperty("tel")]
        string v_Tel { get; set; }
        [JsonProperty("cel")]
        string v_Cel { get; set; }
        [JsonProperty("calle")]
        string v_Calle { get; set; }
        [JsonProperty("numext")]
        string v_NumExt { get; set; }
        [JsonProperty("numint")]
        string v_NumInt { get; set; }
        [JsonProperty("colonia")]
        string v_Colonia { get; set; }
        [JsonProperty("ciudad")]
        string v_Ciudad { get; set; }
        [JsonProperty("municipio")]
        string v_municipio { get; set; }
        [JsonProperty("estado")]
        string v_Estado { get; set; }
        [JsonProperty("cp")]
        string v_Cp { get; set; }
        [JsonProperty("correo")]
        string v_Correo { get; set; }
        /// <summary>
        /// 0 si es fisica, 1 persona moral
        /// </summary>
        [JsonProperty("idpersona")]
        int idPersona = 0;
        #endregion

        #region DAtos de la membresia elegida   
        /// <summary>
        /// el nombre de la membresia, personal, familiar, empresarial
        /// </summary>
        [JsonProperty("nombremembresia")]
        string v_NomMembre { get; set; }
        ////// <summary>
        ////// 0 personal, 1 familiar, 2 empresarial
        ////// </summary>
        [JsonProperty("idmembre")]
        int v_idmembre { get; set; }
        [JsonProperty("costo")]
        string v_Costo { get; set; }
        [JsonProperty("numemple")]
        int v_numEmple { get; set; }

        #endregion

        #region Datos de la tarjeta
        /// <summary>
        /// el token que se genera desde el web view
        /// </summary>
        //[JsonProperty("tokenid")]
        //string v_token { get; set; }
        #endregion



        public C_RegistroPrinci()
        {

        }
        public C_RegistroPrinci(string _nom, string _rfc, DateTime _fechnac, string _lugnac, string _ocu,string _tel, string _cel,
            string _calle,string _numExt, string _numInt, string _col, string _ciud,string _mun, string _est, string _cp, 
            string _corr, int _idPer,string _nomMembre, int _idmembre, string _costo,int _numEmple)//, string _token )
        {
            v_Nombre = _nom;
            v_Rfc = _rfc;
            string _month = "";
            if(_fechnac.Month<10)
            {
                _month = "0" + _fechnac.Month.ToString();
            }
            else
            {
                _month = _fechnac.Month.ToString();
            }
            string _day = "";
            if (_fechnac.Day < 10)
            {
                _day = "0" + _fechnac.Day.ToString();
            }
            else
            {
                _day = _fechnac.Day.ToString();
            }
            v_FecNaci = _fechnac.Year.ToString() + "-" + _month + "-" + _day;
            v_LugNac = _lugnac;
            v_Ocup = _ocu;
            v_Tel = _tel;
            v_Cel = _cel;
            v_Calle = _calle;
            v_NumExt = _numExt;
            v_NumInt = _numInt;
            v_Colonia = _col;
            v_Ciudad = _ciud;
            v_municipio = _mun;
            v_Estado = _est;
            v_Cp = _cp;
            v_Correo = _corr;
            idPersona = _idPer;
            v_NomMembre = _nomMembre;
            v_idmembre = _idmembre;
            v_Costo = _costo;
            v_numEmple = _numEmple;
           // v_token = _token;

        }
    }
    /// <summary>
    /// enviar la info de iniciar sesion
    /// </summary>
    class C_Login
    {
        [JsonProperty("membre")]
        string v_membre { get; set; }
        [JsonProperty("letra")]
        string v_letra { get; set; }
        [JsonProperty("consecutivo")]
        string v_conse { get; set; }
        [JsonProperty("password")]
        string v_pass  { get;set;}
        [JsonProperty("folio")]
        string v_fol { get; set; }
        [JsonProperty("token")]
        string v_topken { get; set; }
        public C_Login()
        {
            this.v_membre = "";
            this.v_letra = "";
            this.v_conse = "";
            this.v_pass = "";
            this.v_fol = "";
        }
        public C_Login(string _membr, string _pass)
        {
            this.v_membre = _membr;
            this.v_pass = _pass;
        }
        public C_Login(string _membr,string _letr,string _conse, string _pass,string _fol)
        {
            this.v_membre = _membr;
            this.v_letra =_letr;
            this.v_conse =_conse;
            this.v_pass = _pass;
            this.v_fol = _fol;
        }
        public C_Login(string _membr, string _letr, string _conse, string _token)
        {
            this.v_membre = _membr;
            this.v_letra = _letr;
            this.v_conse = _conse;
            this.v_topken = _token;
        }
    }
    [Serializable]
    /// <summary>
    /// informacion a mostrar en el buscador, para solicitar citas 
    /// </summary>
    public class C_Medico
    {
        [JsonProperty("Consecutivo")]
        public string v_id { get; set; }
        [JsonProperty("ID_DR")]
        public string v_membre { get; set; }
        //get y set para poder que sean binding
        [JsonProperty("titulo")]
        public string v_titulo { get; set; }
        [JsonProperty("nombre")]
        public string v_Nombre { get; set; }
        /// <summary>
        /// lo necesito yo para ordenarlo por orden      
        /// </summary>
        [JsonProperty("ape")]
        public string v_Apellido { get; set; }
        /// <summary>
        /// la lista que se manda
        /// </summary>
        [JsonProperty("espe")]
        public ObservableCollection<Models.C_EspeTitu> v_ListaEsp;
        /// <summary>
        /// la que se muestra en la red medica
        /// </summary>
        public string v_Especialidad { get; set; }
        [JsonProperty("dom")]
        public string v_Domicilio { get; set; }
        [JsonProperty("horario")]
        public string v_horario;
        /// <summary>
        /// este lo necesito yo para el filtro
        /// </summary>
        [JsonProperty("ciudad")]
        public string v_Ciudad { get; set; }
        [JsonProperty("estado")]
        public string v_estado { get; set; }
        [JsonProperty("tel")]
        public string v_Tel { get; set; }
        [JsonProperty("correo")]
        public string v_Correo { get; set; }
        [JsonProperty("descrip")]
        public string v_descripcion { get; set; }
        [JsonProperty("idsexo")]
        public int v_idsexo { get; set; }
        [JsonProperty("activado")]
        public string v_activo { get; set; }
        [JsonProperty("fecha_vig")]
        public string v_vig;
        [JsonProperty("tokenDr")]
        public string v_tokenDR { get; set; }
        [JsonProperty("citas")]
        public string v_cita;
        public bool Fn_GetActivado()
        {
            if(v_activo=="1")
            { return true; }
            else
            { return false; }
        }
        public void Fn_SetEspec()
        {
            for (int i = 0; i < v_ListaEsp.Count;i++)
            {
                if(i==0)
                {
                    v_Especialidad = v_ListaEsp[i].v_nombreEspec;
                }
                else{
                    v_Especialidad += " , " + v_ListaEsp[i].v_nombreEspec;
                }
            }
        }
        public string v_img { get; set; }
        [JsonIgnore]
        public string v_completo { get; set; }
        public string FN_GetInfo()
        {
            string _ret;
            _ret = "tit " + v_titulo + "nom " + v_Nombre + " ape " + v_Apellido + " espe " + v_Especialidad + " dom " + v_Domicilio + " ciu " + v_Ciudad +
                " tel " + v_Tel + " corr " +
                //v_Correo + " horario" + v_horario + " ced " + v_cedula + 
                " des " + v_descripcion +
                " sexo " + v_idsexo+ "  activado" +v_activo+"  vig"+ v_vig;
            return _ret;
        }
    }
    [Serializable]
    public class C_Servicios
    {
        [JsonProperty("ID_servicio")]
        public string v_id { get; set; }
        //cambio del nombre de la variable para que en el buscador con el binding no de espacios es blanco
        [JsonProperty("nombre")]
        public string v_completo { get; set; }
        [JsonProperty("espe")]
        public string v_Especialidad { get; set; }
        [JsonProperty("dom")]
        public string v_Domicilio { get; set; }
        [JsonProperty("horario")]
        public string v_horario;
        /// <summary>
        /// este lo necesito yo para el filtro
        /// </summary>
        [JsonProperty("ciudad")]
        public string v_Ciudad { get; set; }
        [JsonProperty("estado")]
        public string v_estado { get; set; }
        [JsonProperty("tel")]
        public string v_Tel { get; set; }
        [JsonProperty("correo")]
        public string v_Correo { get; set; }
        [JsonProperty("descrip")]
        public string v_descripcion { get; set; }
        [JsonProperty("beneficios")]
        public string v_beneficio { get; set; }
        [JsonProperty("sitio")]
        public string v_sitio;
        /// <summary>
        /// imagen para mostrar
        /// </summary>
        [JsonProperty("img")]
        public string v_img { get; set; }
        [JsonProperty("activado")]
        public string v_activo { get; set; }
        [JsonProperty("fecha_vig")]
        public string v_vig;
        public bool Fn_GetActivado()
        {
            if (v_activo == "1")
            { return true; }
            else
            { return false; }
        }
    }
    [Serializable]
    /// <summary>
    /// lugares diferentes
    /// </summary>
    public class C_ServGenerales
    {
        [JsonProperty("ID_servicio")]
        public string v_id { get; set; }
        //cambio del nombre de la variable para que en el buscador con el binding no de espacios es blanco
        [JsonProperty("nombre")]
        public string v_completo { get; set; }
        /// <summary>
        /// la especialidad a lo que se dedica
        /// </summary>
        [JsonProperty("espe")]     
        public string v_Especialidad { get; set; }
        [JsonProperty("descrip")]
        public string v_descripcion { get; set; }
        [JsonProperty("beneficios")]
        public string v_beneficio { get; set; }
        [JsonProperty("dom")]
        public string v_Domicilio { get; set; }
        /// <summary>
        /// este lo necesito yo para el filtro
        /// </summary>
        [JsonProperty("ciudad")]
        public string v_Ciudad { get; set; }
        [JsonProperty("estado")]
        public string v_estado { get; set; }
        [JsonProperty("horario")]
        public string v_horario;
        [JsonProperty("tel")]
        public string v_Tel { get; set; }
        [JsonProperty("correo")]
        public string v_Correo { get; set; }
        [JsonProperty("sitio")]
        public string v_sitio;
        /// <summary>
        /// imagen para mostrar
        /// </summary>
        [JsonProperty("img")]
        public string v_img { get; set ; }
        [JsonProperty("activado")]
        public string v_activo { get; set; }
        [JsonProperty("fecha_vig")]
        public string v_vig;
        public bool Fn_GetActivado()
        {
            if (v_activo == "1")
            { return true; }
            else
            { return false; }
        }
    }
}
