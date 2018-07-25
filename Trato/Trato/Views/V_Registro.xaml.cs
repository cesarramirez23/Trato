using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Personas;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using conekta;
using NUnit.Framework;
namespace Trato.Views
{
    public class ParaTok
    {
        [JsonProperty("nombretarjeta")]
        string v_NombreTar { get; set; }
        [JsonProperty("numtarjeta")]
        string v_NumeroTar { get; set; }
        [JsonProperty("cvc")]
        string v_CVC { get; set; }
        [JsonProperty("mes")]
        string v_Mes { get; set; }
        [JsonProperty("ano")]
        string v_Ano { get; set; }
        [JsonProperty("id")]
        int Id { get; set; }
        [JsonProperty("Membre")]
        int v_membre { get; set; }
        public ParaTok( string _nomTar, string _num, string _cvc, string _mes, string _ano, int _id, int _mem)
        {
            
            v_NombreTar = _nomTar;
            v_NumeroTar = _num;
            v_CVC = _cvc;
            v_Mes = _mes;
            v_Ano = _ano;
            Id = _id;
            v_membre = _mem;
        }
        public ParaTok()
        { }
    }
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Registro : ContentPage
    {
        bool v_primero = false;
        string[] v_costo =
        {
            "100", "200", "300"
        };

        /// <summary>
        /// true es fisico falso es moral
        /// </summary>
        public bool v_T_Persona = true;
        protected override void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
        protected override void OnDisappearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public V_Registro(string _titulo)
        {
            InitializeComponent();
            fecha.MaximumDate = DateTime.Now;
            Persona.Text = "Persona Fisica";
            fecha.IsEnabled = v_T_Persona;
            lugar.IsEnabled = v_T_Persona;
            tel.IsEnabled = v_T_Persona;

        }
        /// <summary>
        /// true activa el registro con folio
        /// </summary>
        /// <param name="_folio"></param>
        public V_Registro(bool _folio)
        {
            InitializeComponent();
            v_primero = false;
            Browser.Source = "https://www.alsain.mx/trato_especial/pre_tarjeta_alta.php";
            if (_folio)
            {
                stackTodo.IsVisible = false;
                StackFolio.IsVisible = true;
                Fol_pass.Text = "";
                Fol_usu.Text = "";
            }
            else
            {
                stackTodo.IsVisible = true;
                StackFolio.IsVisible = false;
                fecha.MaximumDate = DateTime.Now;
                Persona.Text = "Persona Fisica";
                fecha.IsEnabled = v_T_Persona;
                lugar.IsEnabled = v_T_Persona;
                tel.IsEnabled = v_T_Persona;
            }
        }
        public V_Registro()
        {
            InitializeComponent();
            fecha.MaximumDate = DateTime.Now;
            Persona.Text = "Persona Fisica";
            fecha.IsEnabled = v_T_Persona;
            lugar.IsEnabled = v_T_Persona;
            tel.IsEnabled = v_T_Persona;
        }
        /// <summary>
        /// el switch, tru es fisico falso es moral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Cambio(object sender, EventArgs args)
        {
            v_T_Persona = !v_T_Persona;
            if (v_T_Persona)
            {
                giro.Text = "";
                giro.Placeholder = "Ocupacion";
                Persona.Text = "Persona Fisica";
                fecha.IsEnabled = true;
                lugar.IsEnabled = true;
                tel.IsEnabled = true;
                fecha.IsVisible = true;
                lugar.IsVisible = true;
                tel.IsVisible = true;
            }
            else
            {
                fecha.IsEnabled = false;
                lugar.IsEnabled = false;
                tel.IsEnabled = false;
                tel.IsVisible = false;
                fecha.IsVisible = false;
                lugar.IsVisible = false;
                giro.Text = "";
                giro.Placeholder = "Giro de la empresa";
                Persona.Text = "Persona Moral";
            }

        }

        void Fn_Drop(object sender, EventArgs _args)
        {
            mensaje.Text = tipo.SelectedItem.ToString() + "  " + v_costo[tipo.SelectedIndex];
        }
        void Fn_Max2(object sender, EventArgs _args)
        {
            Entry _temp = (Entry)sender;
            if (_temp.Text.Length > 2)
            {
                _temp.Text = _temp.Text.Remove(_temp.Text.Length - 1); // remove last char
            }
        }
        void Fn_Max18(object sender, EventArgs _args)
        {
            Entry _temp = (Entry)sender;
            if (_temp.Text.Length > 18)
            {
                _temp.Text = _temp.Text.Remove(_temp.Text.Length - 1); // remove last char
            }
        }
        public async void Registrar(object sender, EventArgs _args)
         {
            if(Fn_Condiciones())
            {

                int _persona;
                if (v_T_Persona)
                {
                    _persona = 0;
                }
                else
                {
                    _persona = 1;
                }

                if (!v_primero)
                {
                    await Browser.EvaluateJavaScriptAsync("submitbutton()");
                    v_primero = true;
                    await Task.Delay(2000);
                }
                string tokenid = await Browser.EvaluateJavaScriptAsync("submitbutton()");

                await Task.Delay(1000);
                if(string.IsNullOrEmpty( tokenid) || string.IsNullOrWhiteSpace(tokenid))
                {
                    await DisplayAlert("Error", "Error en algun dato tokenid vacio ", "aceptar");
                }
                else
                {
                    C_RegistroPrinci datosregistro = new C_RegistroPrinci(nombre.Text, rfc.Text, fecha.Date, lugar.Text, giro.Text, tel.Text, cel.Text,
                        dom.Text, ext.Text, inte.Text, col.Text, ciu.Text, mun.Text, est.Text, cp.Text, correo.Text, _persona, tipo.SelectedItem.ToString(), tipo.SelectedIndex,
                        v_costo[tipo.SelectedIndex], tokenid);

                    //se crea el json
                    string json_reg = JsonConvert.SerializeObject(datosregistro,Formatting.Indented);
                    // lo hacemos visible en la pantall
                    otroaa.Text = json_reg;
                    //damos el formato
                    StringContent v_content = new StringContent(json_reg, Encoding.UTF8, "application/json");
                    //crea el cliente
                    HttpClient v_cliente = new HttpClient();
                    //url
                    var url = "https://www.alsain.mx/trato_especial/tarjeta_alta.php";
                    HttpResponseMessage respuestaReg = await v_cliente.PostAsync(url, v_content);
                    await DisplayAlert("statusCode", respuestaReg.StatusCode.ToString(), "Aceptar");
                    if(respuestaReg.StatusCode== System.Net.HttpStatusCode.OK)
                    {
                    string content = await respuestaReg.Content.ReadAsStringAsync();
                    await DisplayAlert("Respuesta", "dice que OK"+"\n" +content, "aceptar");

                    }
                    //otroaa.Text = v_content.ToString();

                }
            }


            //if (Fn_Condiciones())
            //{

            //

            ///construir los datos de la tarkjeta que se va a enviar
            //C_Tarjeta _Tarjeta = new C_Tarjeta(Persona.Text, correo.Text, App.Fn_Vacio(tel.Text), tipo.SelectedItem.ToString(), v_costo[tipo.SelectedIndex],
            //Tar_Nombre.Text, Tar_Numero.Text, Tar_Cvc.Text, Tar_Mes.Text, Tar_Año.Text);

            //////para darle formato identado
            //string _jsonTar = JsonConvert.SerializeObject(_Tarjeta,Formatting.Indented);
            //otroaa.Text = _jsonTarjeta + "\n " + jsonCustomer;

            //otroaa.Text = _jsonTar;//MOSTRAMOS EN JSON QUE SE HIZO, nadamas para que ves que estas enviando

            // //damos el formato
            //StringContent _contTar = new StringContent(_jsonTar, Encoding.UTF8, "application/json");
            // //crea el cliente
            //HttpClient _cli = new HttpClient();
            // //cambiar el url al que se va a enviar
            //var uri = "http://192.168.0.121:80/trato_especial/pre_tarjeta_alta";
            // //se envia y esperamos respuesta
            //var result = await _cli.PostAsync(uri, _contTar);

            // HttpResponseMessage regresaphp = await _cli.PostAsync(uri, _contTar);
            // string content = await regresaphp.Content.ReadAsStringAsync();
            // string _Arr = content.Split('<')[0];
            // await DisplayAlert("Regresa post", _Arr, "nada");

            ////nombre de tarjeta numero cvc mes año    
            //ParaTok _Enviar = new ParaTok();
            //int _persona;
            //if (v_T_Persona)
            //{
            //    _persona = 0;
            //}
            //else
            //{
            //    _persona = 1;
            //}
            //_Enviar = new ParaTok(Tar_Nombre.Text, Tar_Numero.Text, Tar_Cvc.Text, Tar_Mes.Text, Tar_Año.Text, _persona, tipo.SelectedIndex);
            //var _jsonTok = JsonConvert.SerializeObject(_Enviar, Formatting.Indented);

            //StringContent _contTar = new StringContent(_jsonTok, Encoding.UTF8, "application/json");
            ////crea el cliente
            //HttpClient _cli = new HttpClient();
            ////cambiar el url al que se va a enviar
            //var uri = "http://192.168.0.121:80/trato_especial/pre_tarjeta_alta";
            ////se envia y esperamos respuesta
            ////var result = await _cli.PostAsync(uri, _contTar);

            //otroaa.Text = _jsonTok;
            //HttpResponseMessage regresaphp = await _cli.PostAsync(uri, _contTar);
            //string content = await regresaphp.Content.ReadAsStringAsync();
            //string id = content.Split('@')[1];
            ////llamar id al valor de arr
            //    await DisplayAlert("Regresa post", id, "nada");


            //    //_cli = new HttpClient();
            //    //uri = "http://192.168.0.121:80/trato_especial/";
            //    //regresaphp = await _cli.GetAsync(uri);
            //    //content = await regresaphp.Content.ReadAsStringAsync();
            //    //await DisplayAlert("Regresa get", content, "nada");


            //}
            //else
            //{
            //    await DisplayAlert("Errores", "errores", "cancel");
            //}
        }

     
        public async void Folio_Registro(object sender, EventArgs _args)
        {
            if (Fn_Condiciones(true))
            {
                C_Registro _registro = new C_Registro(Fol_folio.Text, Fol_usu.Text, Fol_pass.Text);
                //HttpClient _cliente = new HttpClient();
                string _json = JsonConvert.SerializeObject(_registro);
                StringContent _contReg= new StringContent(_json, Encoding.UTF8, "application/json");
                ////enviar el post
                //string url = "http://jsonplaceholder.typicode.com/posts";
                //HttpResponseMessage _response = await _cliente.PostAsync(url, _contReg);


                await DisplayAlert("TODO BIEN", _registro.Fn_GetInfo(), "cancel");
                
                //Application.Current.MainPage = new NavigationPage(new V_Master() { Title = "REGISTRADO" });
            }
            else
            {
                await DisplayAlert("ERROR", "mensaje de respuesta", "cancel");
            }
        }
        
        /// <summary>
        /// cuando es crear usuario, folio y contraseña
        /// </summary>
        /// <param name="_folio"></param>
        /// <returns></returns>
        bool Fn_Condiciones(bool _folio)
        {
            int _conta = 0;
            if (string.IsNullOrEmpty(Fol_folio.Text) || string.IsNullOrWhiteSpace(Fol_folio.Text))
            {
                Fol_folio.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                Fol_folio.BackgroundColor = Color.Transparent;
            }

            if (string.IsNullOrEmpty(Fol_usu.Text) || string.IsNullOrWhiteSpace(Fol_usu.Text))
            {
                Fol_usu.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                Fol_usu.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(Fol_pass.Text) || string.IsNullOrWhiteSpace(Fol_pass.Text))
            {
                Fol_pass.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                Fol_pass.BackgroundColor = Color.Transparent;
            }
            if (_conta > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// FORMULARIO COMPLETO CAMBIAR EL COLOR DEL FONDO A LOS QUE SON NECESARIOS
        /// </summary>
        /// <returns></returns>
        bool Fn_Condiciones()
        {
            int _contador = 0;

            //si es persona moral es obligatorio el rfc y giro
            if (!v_T_Persona)
            {
                if (string.IsNullOrEmpty(giro.Text) || string.IsNullOrWhiteSpace(giro.Text))
                {
                    giro.BackgroundColor = Color.Red; _contador++;
                }
                else
                {
                    giro.BackgroundColor = Color.Transparent;
                }
                if (string.IsNullOrEmpty(rfc.Text) || string.IsNullOrWhiteSpace(rfc.Text))
                {
                    rfc.BackgroundColor = Color.Red; _contador++;
                }
                else
                {
                    rfc.BackgroundColor = Color.Transparent;
                }
            }
            else//vuelve el fondo a normal para que no sea obligatorio
            {
                rfc.BackgroundColor = Color.Transparent;
                giro.BackgroundColor = Color.Transparent;
            }

            //nombre
            if (string.IsNullOrEmpty(nombre.Text) ||string.IsNullOrWhiteSpace(nombre.Text))
            {
                nombre.BackgroundColor = Color.Red;
                _contador++;
            }
            else
            {
                nombre.BackgroundColor = Color.Transparent;
            }

           
            /* PENDIENTE A PREGUNTAR
            if (string.IsNullOrEmpty(giro.Text))
            {
                giro.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                giro.BackgroundColor = Color.Transparent;
            }*/
            //calle
            if (string.IsNullOrEmpty(dom.Text) || string.IsNullOrWhiteSpace(dom.Text))
            {
                dom.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                dom.BackgroundColor = Color.Transparent;
            }//exterior
            if (string.IsNullOrEmpty(ext.Text) || string.IsNullOrWhiteSpace(ext.Text))
            {
                ext.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                ext.BackgroundColor = Color.Transparent;
            }
            //colonia
            if (string.IsNullOrEmpty(col.Text) || string.IsNullOrWhiteSpace(col.Text))
            {
                col.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                col.BackgroundColor = Color.Transparent;
            }
            //ciudad
            if (string.IsNullOrEmpty(ciu.Text) || string.IsNullOrWhiteSpace(ciu.Text))
            {
                ciu.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                ciu.BackgroundColor = Color.Transparent;
            }
            //municipio
            if (string.IsNullOrEmpty(mun.Text) || string.IsNullOrWhiteSpace(mun.Text))
            {
                mun.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                mun.BackgroundColor = Color.Transparent;
            }
            //estado
            if (string.IsNullOrEmpty(est.Text) || string.IsNullOrWhiteSpace(est.Text))
            {
                est.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                est.BackgroundColor = Color.Transparent;
            }
            //codigo postal
            if (string.IsNullOrEmpty(cp.Text) || string.IsNullOrWhiteSpace(cp.Text))
            {
                cp.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                cp.BackgroundColor = Color.Transparent;
            }
            //correo
            if (string.IsNullOrEmpty(correo.Text) || string.IsNullOrWhiteSpace(correo.Text))
            {
                correo.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                correo.BackgroundColor = Color.Transparent;
            }
            //telefono
            if (string.IsNullOrEmpty(tel.Text) || string.IsNullOrWhiteSpace(tel.Text))
            {
                tel.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                tel.BackgroundColor = Color.Transparent;
            }
            //membresia
            if (tipo.SelectedIndex < 0)
            {
                tipo.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                tipo.BackgroundColor = Color.Transparent;
            }


            //tarjeta
            //if (string.IsNullOrEmpty(Tar_Nombre.Text) || string.IsNullOrWhiteSpace(Tar_Nombre.Text))
            //{
            //    Tar_Nombre.BackgroundColor = Color.Red; _contador++;
            //}
            //else
            //{
            //    Tar_Nombre.BackgroundColor = Color.Transparent;
            //}

            //if (string.IsNullOrEmpty(Tar_Numero.Text) || string.IsNullOrWhiteSpace(Tar_Numero.Text))
            //{
            //    Tar_Numero.BackgroundColor = Color.Red; _contador++;
            //}
            //else
            //{
            //    Tar_Numero.BackgroundColor = Color.Transparent;
            //}
            //if (string.IsNullOrEmpty(Tar_Cvc.Text) || string.IsNullOrWhiteSpace(Tar_Cvc.Text))
            //{
            //    Tar_Cvc.BackgroundColor = Color.Red; _contador++;
            //}
            //else
            //{
            //    Tar_Cvc.BackgroundColor = Color.Transparent;
            //}

            //if (string.IsNullOrEmpty(Tar_Mes.Text) || string.IsNullOrWhiteSpace(Tar_Mes.Text) || Tar_Mes.Text.Length != 2)
            //{
            //    Tar_Mes.BackgroundColor = Color.Red; _contador++;
            //}
            //else
            //{
            //    Tar_Mes.BackgroundColor = Color.Transparent;
            //}
            //if (string.IsNullOrEmpty(Tar_Año.Text) || string.IsNullOrWhiteSpace(Tar_Año.Text) || Tar_Año.Text.Length != 2)
            //{
            //    Tar_Año.BackgroundColor = Color.Red; _contador++;
            //}
            //else
            //{
            //    Tar_Año.BackgroundColor = Color.Transparent;
            //}


            if (v_T_Persona)
            {
                //el lugar de nacimiento y la fecha
            }

            if (_contador > 0)
            { return false; }
            else { return true; }


        }
        /*
        string Fn_Vacio(string _valor)
        {
           
            if(string.IsNullOrEmpty(_valor) || string.IsNullOrWhiteSpace(_valor))
            {
                return "";
            }
            else
            {
                return _valor;
            }

        }*/
    }
}