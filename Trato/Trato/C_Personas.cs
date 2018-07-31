using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
namespace Trato.Personas
{
    /* https://forums.xamarin.com/discussion/100135/json-response-parsing-in-xamarin-froms
     * EXPLICA EL FORMATO Y COMO DEBEN ESTAR LOS ATRIBUTOS PARA EL JSON
     * 
     * 
     * EN LA TARJETA CON LOS DATOS DE COBRO ENVIAR EL NOMBRE DE LA MEMBRESIA CON STRING
     * Y TAMBIEN ENVIAR  COMO NUMERO EL TIPO DE MEMBRESiA( DE 0 A 2)
         */

    public class C_Perfil
    {
        [JsonProperty("nombre")]
        string v_Nombre { get; set; }        
        [JsonProperty("domicilio")]
        string v_Domi { get; set; }
        [JsonProperty("correo")]
        string v_Correo { get; set; }
        [JsonProperty("telefono")]
        string v_Tel { get; set; }
        [JsonProperty("celular")]
        string v_Cel { get; set; }
        [JsonProperty("sangre")]
        string v_sangre { get; set; }
        [JsonProperty("sexo")]
        int v_sexo { get; set; }
        [JsonProperty("infoMuj")]
        string v_infoMujer { get; set; }
        [JsonProperty("alergias")]
        string v_alergias { get; set; }
        [JsonProperty("operaciones")]
        string v_operaciones { get; set; }
        [JsonProperty("enfermedades")]
        string v_enfer { get; set; }
        [JsonProperty("medicamentos")]
        string v_medica { get; set; }



        public C_Perfil()
        {

        }
        public C_Perfil(string _nom, string _corr, string _dom,string _tel, string _cel, string _sangr,
            string _sexo,string _aler,string _opera,string _enfer, string _medicam)
        {
            v_Nombre = _nom;
            v_Correo = _corr;
            v_Domi = _dom;
            v_Tel = _tel;
            v_Cel = _tel;
            v_sangre = _sangr;
            v_infoMujer = _sexo;
            v_alergias = _aler;
            v_operaciones = _opera;
            v_enfer = _enfer;
            v_medica = _medicam;
        }
    }

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
        #endregion

        #region Datos de la tarjeta
        /// <summary>
        /// el token que se genera desde el web view
        /// </summary>
        [JsonProperty("tokenid")]
        string v_token { get; set; }
        #endregion



        public C_RegistroPrinci()
        {

        }
        public C_RegistroPrinci(string _nom, string _rfc, DateTime _fechnac, string _lugnac, string _ocu,string _tel, string _cel,
            string _calle,string _numExt, string _numInt, string _col, string _ciud,string _mun, string _est, string _cp, 
            string _corr, int _idPer,string _nomMembre, int _idmembre, string _costo, string _token )
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
            v_token = _token;

        }
    }
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



        public C_Login()
        {
            this.v_membre = "";
            this.v_letra = "";
            this.v_conse = "";
            this.v_pass = "";
            this.v_fol = "";
        }
        public C_Login(string _membr,string _letr,string _conse, string _pass,string _fol)
        {
            this.v_membre = _membr;
            this.v_letra =_letr;
            this.v_conse =_conse;
            this.v_pass = _pass;
            this.v_fol = _fol;
        }
    }    
    class C_Ind_Fisica
    {
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
        [JsonProperty("membre")]
        int v_membre { get; set; }
        [JsonProperty("id")]
        const int Id = 0;
    public C_Ind_Fisica()
        {

            this.v_Nombre = "";
            this.v_Rfc = "";
            //4-2-2
            //AÑO - MES NUMERO- DIA
            this.v_FecNaci = "0000-00-00";
            this.v_LugNac = "";
            this.v_Ocup = "";
            this.v_Tel = "";
            this.v_Cel = "";
            this.v_Calle = "";
            this.v_NumExt = "";
            this.v_NumInt = "";
            this.v_Colonia = "";
            this.v_Ciudad = "";
            this.v_municipio = "";
            this.v_Estado = "";
            this.v_Cp = "";
            this.v_Correo = "";
        }
        public C_Ind_Fisica(string _nom,string _rfc, DateTime _nac,
           string _lugnac, string _ocu, string _tel, string _cel, string _call,
           string _ext, string _int, string _col, string _ciud, string _muni, string _est, string _cp, string _corr, int _membre)
        {
            this.v_Nombre = _nom;
            this.v_Rfc = _rfc;
            //4-2-2
            //AÑO - MES NUMERO- DIA
            this.v_FecNaci = _nac.Year.ToString() + "-" + _nac.Month.ToString() + "-" + _nac.Day.ToString();
            this.v_LugNac = _lugnac;
            this.v_Ocup = _ocu;
            this.v_Tel = _tel;
            this.v_Cel = _cel;
            this.v_Calle = _call;
            this.v_NumExt = _ext;
            this.v_NumInt = _int;
            this.v_Colonia = _col;
            this.v_Ciudad = _ciud;
            this.v_municipio = _muni;
            this.v_Estado = _est;
            this.v_Cp = _cp;
            this.v_Correo = _corr;
            this.v_membre = _membre;
        }
        
        public string Fn_GetInfo()
        {
            string _mensaje = v_Nombre + " " + v_Rfc + " " + v_FecNaci + " " + v_LugNac + " " + v_Ocup +
                " " + v_Tel + " " + v_Cel + " " + v_Calle + " " + v_NumExt + " " + v_NumInt + " " + v_Colonia + " " +
                v_Ciudad + " " + v_municipio + " " + v_Estado + " " + v_Cp + " " + v_Correo;
            return _mensaje;
        }
    }
    class C_Ind_Moral
    {
        /// <summary>
        /// nombre oi razon social
        /// </summary>
        string v_Nombre;
        [JsonProperty("rfc")]
        string v_Rfc;
        [JsonProperty("ocu")]
        string v_Ocup { get; set; }
        [JsonProperty("tel")]
        string v_Tel { get; set; }
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
        [JsonProperty("Membre")]
        int v_membre { get; set; }
        [JsonProperty("id")]
        const int Id = 1;


        public C_Ind_Moral()
        {
            this.v_Nombre = "";
            this.v_Rfc = "";
            this.v_Tel = "";
            this.v_Calle = "";
            this.v_NumExt = "";
            this.v_NumInt = "";
            this.v_Colonia = "";
            this.v_Ciudad = "";
            this.v_municipio = "";
            this.v_Estado = "";
            this.v_Cp = "";
            this.v_Ocup = "";
            this.v_Correo = "";
        }
        public C_Ind_Moral(string _nom, string _rfc, string _giro, string _tel, string _call,
           string _ext, string _int, string _col, string _ciud, string _muni, string _est, string _cp, string _corr, int _membre)
        {
            this.v_Nombre = _nom;
            this.v_Rfc = _rfc;
            this.v_Calle = _call;
            this.v_NumExt = _ext;
            this.v_NumInt = _int;
            this.v_Colonia = _col;
            this.v_Ciudad = _ciud;
            this.v_municipio = _muni;
            this.v_Estado = _est;
            this.v_Cp = _cp;
            this.v_Ocup = _giro;
            this.v_Tel = _tel;
            this.v_Correo = _corr;
            this.v_membre = _membre;
        }

        public string Fn_GetInfo()
        {
            string _mensaje = v_Nombre + " " + v_Rfc + " " + v_Calle + " " + v_NumExt + " " + v_NumInt + "  " +
                v_Colonia + " " + v_Ciudad + " " + v_municipio + " " + v_Estado + " " +
                v_Cp + " " + v_Ocup + " " + v_Tel + " " + v_Correo;

            return _mensaje;
        }

    }
    class C_Fam
    {

        string v_Nombre;
        string v_Parentesco;
        /// <summary>
        /// Fecha de nacimiento, tiene que ser dia mes año
        /// </summary>
        string v_FecNaci;
        string v_Correo;
        string v_Cel;
        const int Id = 2;
        public C_Fam()
        {
            this.v_Nombre = "";
            this.v_Parentesco = "";
            this.v_FecNaci = "0000-00-00";
            this.v_Correo = "";
            this.v_Cel = "";

        }
        public C_Fam(string _nom, string _paren, DateTime _nac,
           string _cel, string _corr)
        {
            this.v_Nombre = _nom;
            this.v_Parentesco = _paren;
            this.v_FecNaci = _nac.Year.ToString() + "-" + _nac.Month.ToString() + "-" + _nac.Day.ToString();
            this.v_Correo = _corr;
            this.v_Cel = _cel;
        }
        public string Fn_GetInfo()
        {
            return "";
        }
    }

    class C_Emp_Empleado
    {
        /// <summary>
        /// nombre de la emopresa
        /// </summary>
        string v_NombreEmp;
        /// <summary>
        /// numero de empleado en la empresa
        /// </summary>
        string v_NumeroEmp;
        /// <summary>
        /// nombre completo del empleado
        /// </summary>
        string v_Nombre;
        /// <summary>
        /// Fecha de nacimiento, tiene que ser dia mes año
        /// </summary>
        string v_FecNaci;
        string v_Correo;
        string v_Cel;
        public C_Emp_Empleado()
        {
            this.v_NombreEmp = "";
            this.v_NumeroEmp = "";
            this.v_Nombre = "";
            this.v_FecNaci = "0000-00-00";//
            this.v_Cel = "";
            this.v_Correo = "";

        }
        public C_Emp_Empleado(string _nomEmp, string _num, string _nombre, DateTime _nac,
           string _cel, string _corr)
        {

            this.v_NombreEmp = _nomEmp;
            this.v_NumeroEmp = _num;
            this.v_Nombre = _nombre;
            this.v_FecNaci = _nac.Year.ToString() + "-" + _nac.Month.ToString() + "-" + _nac.Day.ToString();
            this.v_Cel = _cel;
            this.v_Correo = _corr;
        }

        public string Fn_GetInfo()
        {
            string _mensaje = v_NombreEmp + " " + v_NumeroEmp + " " + v_Nombre + " " +
                v_FecNaci + " " + v_Correo + v_Cel;
            return _mensaje;
        }

    }
    public class C_Medico
    {
        //get y set para poder que sean binding
        [JsonProperty("nombre")]
        public string v_Nombre { get; set; }
        /// <summary>
        /// lo necesito yo para ordenarlo por orden      
        /// </summary>
        [JsonProperty("ape")]
        public string v_Apellido { get; set; }
        [JsonProperty("espe")]
        public string v_Especialidad { get; set; }
        [JsonProperty("dom")]
        public string v_Domicilio { get; set; }
        /// <summary>
        /// este lo necesito yo para el filtro
        /// </summary>
        [JsonProperty("ciudad")]
        public string v_Ciudad { get; set; }
        [JsonProperty("tel")]
        public string v_Tel { get; set; }
        [JsonProperty("correo")]
        public string v_Correo { get; set; }
        //[JsonProperty("desc")]
        //public string v_Descuento { get; set; }
        [JsonProperty("horario")]
        public string v_horario { get; set; }
        [JsonProperty("cedula")]
        public string v_cedula { get; set; }
        [JsonProperty("descrip")]
        public string v_descripcion { get; set; }
        [JsonProperty("img")]
        public string v_img { get; set; }
    }
    public class C_Servicios
    {
        //get y set para poder que sean binding
        [JsonProperty("nombre")]
        public string v_Nombre { get; set; }
        [JsonProperty("espe")]
        public string v_Especialidad { get; set; }
        [JsonProperty("dom")]
        public string v_Domicilio { get; set; }
        /// <summary>
        /// este lo necesito yo para el filtro
        /// </summary>
        [JsonProperty("ciudad")]
        public string v_Ciudad { get; set; }
        [JsonProperty("tel")]
        public string v_Tel { get; set; }
        [JsonProperty("correo")]
        public string v_Correo { get; set; }
        //[JsonProperty("desc")]
        //public string v_Descuento { get; set; }
        [JsonProperty("horario")]
        public string v_horario { get; set; }
        [JsonProperty("descrip")]
        public string v_descripcion { get; set; }
        /// <summary>
        /// imagen para mostrar
        /// </summary>
        [JsonProperty("img")]
        public string v_img { get; set; }
    }
}
