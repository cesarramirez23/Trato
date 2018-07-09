using System;
using System.Collections.Generic;
using System.Text;
namespace Trato.Personas
{
    /* https://forums.xamarin.com/discussion/116786/how-can-i-target-android-api-27-oreo-8-1 
     * 
     * 
     * EN LA TARJETA CON LOS DATOS DE COBRO ENVIAR EL NOMBRE DE LA MEMBRESIA CON STRING
     * Y TAMBIEN ENVIAR  COMO NUMERO EL TIPO DE MEMBRESiA( DE 0 A 2)
         */
    class C_Tarjeta
    {
        string v_Nombre = "";
        string v_Correo = "";
        string v_Telefono = "";
        string v_Membresia = "";
        string v_Costo = "";
        string v_NombreTar = "";
        string v_NumeroTar = "";
        string v_CVC = "";
        string v_Mes = "";
        string v_Ano = "";

        public C_Tarjeta()
        {
            v_Nombre = "";
            v_Correo = "";
            v_Telefono = "";
            v_Membresia = "";
            v_Costo = "";
            v_NombreTar = "";
            v_NumeroTar = "";
            v_CVC = "";
            v_Mes = "";
            v_Ano = "";
        }
        public C_Tarjeta(string _nom, string _corr, string _tel, string _memb,
            string _costo, string _nomTar, string _num, string _cvc, string _mes, string _ano)
        {
            v_Nombre = _nom;
            v_Correo = _corr;
            v_Telefono = _tel;
            v_Membresia = _memb;
            v_Costo = _costo;
            v_NombreTar = _nomTar;
            v_NumeroTar = _num;
            v_CVC = _cvc;
            v_Mes = _mes;
            v_Ano = _ano;
        }


    }
    class C_Login
    {
        string v_usuario = "";
        string v_pass = "";

        public C_Login()
        {
            this.v_usuario = "";
            this.v_pass = "";
        }
        public C_Login(string _usu, string _pass)
        {
            this.v_usuario = _usu;
            this.v_pass = _pass;
        }
        public string Fn_GetInfo()
        {
            string _info;
            _info = v_usuario + "  " + v_pass;
            return _info;
        }

    }
    class C_Ind_Fisica
    {

        string v_Nombre = "";
        //year dayofmonth, month   son int
        string v_Rfc;
        /// <summary>
        /// Fecha de nacimiento, tiene que ser dia mes año
        /// </summary>
        string v_FecNaci;
        /// <summary>
        /// lugar de nacimiento
        /// </summary>
        string v_LugNac;
        string v_Ocup;
        string v_Tel;
        string v_Cel;
        string v_Calle;
        string v_NumExt;
        string v_NumInt;
        string v_Colonia;
        string v_Ciudad;
        string v_municipio;
        string v_Estado;
        string v_Cp;
        string v_Correo;
        int Id = 0;
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
        public C_Ind_Fisica(string _nom, string _rfc, DateTime _nac,
           string _lugnac, string _ocu, string _tel, string _cel, string _call,
           string _ext, string _int, string _col, string _ciud, string _muni, string _est, string _cp, string _corr, int _tipo)
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
            this.Id = _tipo;
        }

        public string Fn_GetInfo()
        {
            string _mensaje = v_Nombre + " " + v_Rfc + " " + v_FecNaci + " " + v_LugNac + " " + v_Ocup +
                " " + v_Tel + " " + v_Cel + " " + v_Calle + " " + v_NumExt + " " + v_NumInt + " " + v_Colonia + " " +
                v_Ciudad + " " + v_municipio + " " + v_Estado + " " + v_Cp + " " + v_Correo;
            return _mensaje;
        }
    }
    class C_Registro
    {
        string v_folio = "";
        string v_usuario = "";
        string v_pass = "";

        public C_Registro()
        {
            v_folio = "";
            v_usuario = "";
            v_pass = "";
        }
        public C_Registro(string _fol, string _usu, string _pass)
        {
            v_folio = _fol;
            v_usuario = _usu;
            v_pass = _pass;
        }
        public string Fn_GetInfo()
        {
            string _valor = "";
            _valor = v_folio + "  " + v_usuario + "  " + v_pass;
            return _valor;
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

    class C_Ind_Moral
    {
        /// <summary>
        /// nombre oi razon social
        /// </summary>
        string v_Nombre;
        string v_Rfc;
        string v_Calle;
        string v_NumExt;
        string v_NumInt;
        string v_Colonia;
        string v_Ciudad;
        string v_municipio;
        string v_Estado;
        string v_Cp;
        string v_Giro;
        string v_Tel;
        string v_Correo;
        int Id = 1;


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
            this.v_Giro = "";
            this.v_Correo = "";
        }
        public C_Ind_Moral(string _nom, string _rfc, string _giro, string _tel, string _call,
           string _ext, string _int, string _col, string _ciud, string _muni, string _est, string _cp, string _corr, int _tipo)
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
            this.v_Giro = _giro;
            this.v_Tel = _tel;
            this.v_Correo = _corr;
            this.Id = _tipo;
        }

        public string Fn_GetInfo()
        {
            string _mensaje = v_Nombre + " " + v_Rfc + " " + v_Calle + " " + v_NumExt + " " + v_NumInt + "  " +
                v_Colonia + " " + v_Ciudad + " " + v_municipio + " " + v_Estado + " " +
                v_Cp + " " + v_Giro + " " + v_Tel + " " + v_Correo;

            return _mensaje;
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
        public string v_Nombre { get; set; }
        public string v_Apellido { get; set; }
        public string v_Especialidad { get; set; }
        public string v_Domicilio { get; set; }
        public string v_Info { get; set; }
        public string v_img { get; set; }

    }
    public class C_Servicios
    {
        public string v_Nombre { get; set; }
        public string v_Servicios{ get; set; }
        public string v_Domicilio { get; set; }
        public string v_Info { get; set; }
        public string v_Descuento { get; set; }
        public string v_img { get; set; }
    }
}
