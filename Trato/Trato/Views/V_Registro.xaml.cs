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
using System.Text.RegularExpressions;


namespace Trato.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Registro : ContentPage
    {
       
       // bool v_primero = false;
        string[] v_costo =
        {
            "580" , "1740", "464"
        };

        //familiar 1740   indi  580    empre por persona  464

        /// <summary>
        /// true es fisico falso es moral
        /// </summary>
        public bool v_T_Persona = true;
        JObject v_jsonInfo;
        //protected override void OnAppearing()
        //{
        //    NavigationPage.SetHasNavigationBar(this, false);
        //}
        //protected override void OnDisappearing()
        //{
        //    NavigationPage.SetHasNavigationBar(this, false);
        //}
        public V_Registro(string _titulo)
        {
            InitializeComponent();
            Persona.Text = "Persona Fisica";
            fecha.IsEnabled = v_T_Persona;
            lugar.IsEnabled = v_T_Persona;
            tel.IsEnabled = v_T_Persona;
        }
        /// <summary>
        /// true activa el registro con folio
        /// </summary>
        /// <param name="_folio"></param>
        public V_Registro(bool _folio, int _index)
        {
            InitializeComponent();
            Fol_fecha.MaximumDate = DateTime.Now;
            fecha.MaximumDate= DateTime.Now; 

           // v_primero = false;
            if (_folio)
            {
                stackTodo.IsVisible = false;
                StackFolio.IsVisible = true;
                StackMen.IsVisible = false;
                Fol_pass.Text = "";
            }
            else
            {
                StackMen.IsVisible = false;
                stackTodo.IsVisible = true;
                StackFolio.IsVisible = false;
                Persona.Text = "Persona Fisica";
                StackRfc.IsVisible = false;
                tipo.SelectedIndex = _index;
                fecha.IsEnabled = v_T_Persona;
                lugar.IsEnabled = v_T_Persona;
                tel.IsEnabled = v_T_Persona;
            }
        }

        #region LAS FUNCIONES DE CREAR CUENTA
        public async void Registrar(object sender, EventArgs _args)
        {
            if (tipo.SelectedIndex < 0)
            {
                await DisplayAlert("Error", "Selecciona un tipo de membresia", "Aceptar");
            }
            else
            {
                if (tipo.SelectedIndex == 2 && PickEmple.SelectedIndex < 0)
                {
                    await DisplayAlert("Error", "Como mínimo 1 empleado, maximo 20", "Aceptar");
                }
                else
                {
                    if (Fn_Condiciones())
                    {
                        string json = @"{";
                        json += "folio:'" + App.v_folio + "',\n";
                        json += "}";
                        v_jsonInfo = JObject.Parse(json);


                        NavigationPage.SetHasNavigationBar(this, false);
                        RegPrin.IsEnabled = false;
                        StackMen.IsVisible = true;
                        Mensajes_over.Text = "Procesando Informacion";

                        int _persona;
                        if (v_T_Persona)
                        {
                            _persona = 0;
                            rfc.Text = "";
                        }
                        else
                        {
                            _persona = 1;
                        }



                        //if (!v_primero)
                        //{
                        //    await Browser.EvaluateJavaScriptAsync("submitbutton()");
                        //    v_primero = true;
                        //    await Task.Delay(2000);
                        //}
                        //se genera el token y se guarda
                        //string tokenid = await Browser.EvaluateJavaScriptAsync("submitbutton()");
                        //delay 
                        //await Task.Delay(1000);
                        //if (string.IsNullOrEmpty(tokenid) || string.IsNullOrWhiteSpace(tokenid))
                        //{
                        //    StackMen.IsVisible = false;
                        //    await DisplayAlert("Error", "Error en 1 o mas campos de la tarjeta", "aceptar");
                        //    NavigationPage.SetHasNavigationBar(this, true);
                        //    RegPrin.IsEnabled = true;
                        //}
                        //else
                        //{
                        int _precioFinal = -1;
                        C_RegistroPrinci datosregistro = new C_RegistroPrinci();
                        //tipo de membresia
                        if (tipo.SelectedIndex == 2)
                        {
                            _precioFinal = int.Parse(v_costo[tipo.SelectedIndex]) * int.Parse(PickEmple.SelectedItem.ToString());
                            datosregistro = new C_RegistroPrinci(nombre.Text, rfc.Text, fecha.Date, lugar.Text, giro.Text, tel.Text, cel.Text,
                             dom.Text, ext.Text, inte.Text, col.Text, ciu.Text, mun.Text, est.Text, cp.Text, correo.Text, _persona, tipo.SelectedItem.ToString(), tipo.SelectedIndex,
                             _precioFinal.ToString(), int.Parse(PickEmple.SelectedItem.ToString()));//,  tokenid);
                        }
                        else
                        {
                            datosregistro = new C_RegistroPrinci(nombre.Text, rfc.Text, fecha.Date, lugar.Text, giro.Text, tel.Text, cel.Text,
                              dom.Text, ext.Text, inte.Text, col.Text, ciu.Text, mun.Text, est.Text, cp.Text, correo.Text, _persona, tipo.SelectedItem.ToString(), tipo.SelectedIndex,
                              v_costo[tipo.SelectedIndex], 0);//,tokenid);
                        }
                        //se crea el json
                        string json_reg = JsonConvert.SerializeObject(datosregistro, Formatting.Indented);
                        // lo hacemos visible en la pantall
                        //Mensajes_over.Text += json_reg;
                        //damos el formato
                        StringContent v_content = new StringContent(json_reg, Encoding.UTF8, "application/json");
                        //crea el cliente
                        HttpClient v_cliente = new HttpClient();
                        //url
                        var url = "http://tratoespecial.com/tarjeta_alta.php";

                        try
                        {
                            HttpResponseMessage respuestaReg = await v_cliente.PostAsync(url, v_content);
                            // await DisplayAlert("statusCode", respuestaReg.StatusCode.ToString(), "Aceptar");
                            if (respuestaReg.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string content = await respuestaReg.Content.ReadAsStringAsync();
                                if (content == "1")
                                {
                                    Mensajes_over.Text = "Registrado correctamente, por favor revisa tu correo electronico \n para mas información";
                                    MEnu.IsVisible = true;
                                }
                                else if (content == "0")
                                {
                                    StackMen.IsVisible = false;
                                    Mensajes_over.Text = "";
                                    await DisplayAlert("Error", "Existe un error, por favor revisa tu información", "Aceptar", "cancel");
                                    NavigationPage.SetHasNavigationBar(this, true);
                                    RegPrin.IsEnabled = true;
                                }
                            }
                            else
                            {
                                string content = await respuestaReg.Content.ReadAsStringAsync();
                                //Mensajes_over.Text += respuestaReg.StatusCode.ToString() + "---" + content;
                                ReintenRegPri.IsVisible = true;
                            }
                        }
                        catch (HttpRequestException exception)
                        {
                            Mensajes_over.Text = exception.Message;
                            ReintenRegPri.IsVisible = true;
                            NavigationPage.SetHasNavigationBar(this, true);
                            RegPrin.IsEnabled = true;
                        }
                        // }//token vacio
                    }//ifcondiciones
                }// sio es empresarial que elija numero de empleados
            }//else tipo selectedindex
        }
        public async void Folio_Registro(object sender, EventArgs _args)
        {
            if (Fol_DropMembre.SelectedIndex < 0)
            {
                await DisplayAlert("Error", "Selecciona un tipo de Membresia", "Aceptar");
            }
            else if (v_sexoPick.SelectedIndex < 0)
            {
                await DisplayAlert("Error", "Selecciona tu genero", "Aceptar");
            }
            else
            {
                if (Fn_Condiciones(true))
                {// 0 familiar
                    StackMen.IsVisible = true;
                    Mensajes_over.Text = "Enviando informacion";
                    C_RegistroSec _registro = new C_RegistroSec();
                    //se crean los datos
                    if (Fol_DropMembre.SelectedIndex == 0)
                    {
                        _registro = new C_RegistroSec(Fol_Nombre.Text, v_sexoPick.SelectedIndex, Fol_fecha.Date, Fol_Cel.Text, Fol_Correo.Text, Fol_pass.Text, Fol_NumMembre.Text, 0, Fol_Parent.Text);
                    }
                    else if (Fol_DropMembre.SelectedIndex == 1)
                    {
                        _registro = new C_RegistroSec(Fol_Nombre.Text, v_sexoPick.SelectedIndex, Fol_fecha.Date, Fol_Cel.Text, Fol_Correo.Text, Fol_pass.Text, Fol_NumMembre.Text, 1, Fol_Empre.Text, Fol_Folio.Text);
                    }
                    //crear json
                    string _jsonReg = JsonConvert.SerializeObject(_registro, Formatting.Indented);
                   // Mensajes_over.Text += "\n json \n" + _jsonReg;
                    StringContent _content = new StringContent(_jsonReg, Encoding.UTF8, "application/json");
                    //crea el cliente
                    HttpClient _clien = new HttpClient();
                    //direccion a enviar
                    string _direc = "http://tratoespecial.com/crear_cuenta.php";
                    try
                    {
                        //se envia
                        HttpResponseMessage _respuestaphp = await _clien.PostAsync(_direc, _content);
                       // Mensajes_over.Text = _respuestaphp.StatusCode.ToString();
                        //leer la respuesta
                        string _respuesta = await _respuestaphp.Content.ReadAsStringAsync();
                        //Mensajes_over.Text += "\n respuesta  " + _respuesta;
                        //422 error folio    834 maximo folio fam    200 no membresia con el nombre de la empresa
                        if (_respuesta == "422")
                        {
                            await DisplayAlert("Error", " error de folio", "Aceptar");
                            Mensajes_over.Text = "Reintentar";
                        }
                        else if (_respuesta == "834")
                        {
                            await DisplayAlert("Error", "Limite de folio Excedido", "Aceptar");
                            Mensajes_over.Text = "Reintentar";
                        }
                        else if (_respuesta == "200")
                        {
                            await DisplayAlert("Error", "no coincide el numero de folio con el nombre de la empresa", "Aceptar");
                            Mensajes_over.Text = "Reintentar";
                        }
                        else if (_respuesta == "2")
                        {
                            await DisplayAlert("Error", "La cuenta del titular no está activa, contacta al titular para mas información", "Aceptar");
                            Mensajes_over.Text = "Reintentar";
                        }
                        else if (_respuesta == "0")
                        {
                            await DisplayAlert("Error", "Error por algo", "Aceptar");
                            Mensajes_over.Text = "Reintentar";
                        }
                        else if (_respuesta == "1")
                        {
                            await DisplayAlert("Bien", "Exito todo bien", "Aceptar");
                            await Navigation.PopAsync();
                        }
                        ReintenSec.IsVisible = true;
                    }
                    catch (HttpRequestException exception)
                    {
                        Mensajes_over.Text = exception.Message;
                        ReintenSec.IsVisible = true;
                    }
                }
            }

        }
        #endregion

        #region LAS FUNCIONES DE CONDICIONALES
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
            if (string.IsNullOrEmpty(nombre.Text) || string.IsNullOrWhiteSpace(nombre.Text))
            {
                nombre.BackgroundColor = Color.Red;
                _contador++;
            }
            else
            {
                nombre.BackgroundColor = Color.Transparent;
            }
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
            Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (string.IsNullOrEmpty(correo.Text) || string.IsNullOrWhiteSpace(correo.Text) || !EmailRegex.IsMatch(correo.Text))
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

            if (v_T_Persona)
            {
                //el lugar de nacimiento y la fecha
            }

            if (_contador > 0)
            { return false; }
            else { return true; }


        }
        /// <summary>
        /// cuando es crear usuario, folio y contraseña
        /// </summary>
        /// <param name="_folio"></param>
        /// <returns></returns>
        bool Fn_Condiciones(bool _folio)
        {
            int _conta = 0;
            //nombre
            if (string.IsNullOrEmpty(Fol_Nombre.Text) || string.IsNullOrWhiteSpace(Fol_Nombre.Text))
            {
                Fol_Nombre.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                Fol_Nombre.BackgroundColor = Color.Transparent;
            }
            //numero de emembresia
            if (string.IsNullOrEmpty(Fol_NumMembre.Text) || string.IsNullOrWhiteSpace(Fol_NumMembre.Text))
            {
                Fol_NumMembre.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                Fol_NumMembre.BackgroundColor = Color.Transparent;
            }
            //0 es familiar   1 empresarial
            if (Fol_DropMembre.SelectedIndex == 1)
            {
                Fol_Parent.BackgroundColor = Color.Transparent;
                if (string.IsNullOrEmpty(Fol_Folio.Text) || string.IsNullOrWhiteSpace(Fol_Folio.Text))
                {
                    Fol_Folio.BackgroundColor = Color.Red;
                    _conta++;
                }
                else
                {
                    Fol_Folio.BackgroundColor = Color.Transparent;
                }
                if (string.IsNullOrEmpty(Fol_Empre.Text) || string.IsNullOrWhiteSpace(Fol_Empre.Text))
                {
                    Fol_Empre.BackgroundColor = Color.Red;
                    _conta++;
                }
                else
                {
                    Fol_Empre.BackgroundColor = Color.Transparent;
                }
            }
            else
            {
                Fol_Folio.BackgroundColor = Color.Transparent;
                if (string.IsNullOrEmpty(Fol_Parent.Text) || string.IsNullOrWhiteSpace(Fol_Parent.Text))
                {
                    Fol_Parent.BackgroundColor = Color.Red;
                    _conta++;
                }
                else
                {
                    Fol_Parent.BackgroundColor = Color.Transparent;
                }
            }
            //password
            if (string.IsNullOrEmpty(Fol_pass.Text) || string.IsNullOrWhiteSpace(Fol_pass.Text) || (Fol_pass.Text != Fol_pass2.Text))
            {
                Fol_pass.BackgroundColor = Color.Red;
                Fol_pass2.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                Fol_pass.BackgroundColor = Color.Transparent;
                Fol_pass2.BackgroundColor = Color.Transparent;
            }
            //celular
            if (string.IsNullOrEmpty(Fol_Cel.Text) || string.IsNullOrWhiteSpace(Fol_Cel.Text))
            {
                Fol_Cel.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                Fol_Cel.BackgroundColor = Color.Transparent;
            }
            //correo
            Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (string.IsNullOrEmpty(Fol_Correo.Text) || string.IsNullOrWhiteSpace(Fol_Correo.Text) || !EmailRegex.IsMatch(Fol_Correo.Text))
            {
                Fol_Correo.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                Fol_Correo.BackgroundColor = Color.Transparent;
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
        #endregion

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
                Persona.Text = "Persona Fisica";


                //entry
                giro.Text = "";
                giro.Placeholder = "Ocupacion";
                //label ocupacion
                LblOcu.Text = "Ocupacion";


                StackFecha.IsVisible = true;
                //fecha.IsEnabled = true;
                //fecha.IsVisible = true;

                StackLugar.IsVisible = true;
                //lugar.IsEnabled = true;
                //lugar.IsVisible = true;


                StackCel.IsVisible = true;
                //cel.IsEnabled = true;
                //cel.IsVisible = true;


                StackRfc.IsVisible = false;
                //rfc.IsVisible = false;
                rfc.Text = "";


            }
            else
            {
                Persona.Text = "Persona Moral";
                
                //giro es entry
                giro.Text = "";
                giro.Placeholder = "Giro de la empresa";
                //label de ocupacuon/giro
                LblOcu.Text = "Giro";


                StackFecha.IsVisible = false;
                //fecha.IsEnabled = false;
                //fecha.IsVisible = false;

                StackLugar.IsVisible = false;
                //lugar.IsEnabled = false;
                //lugar.Text = "";


                StackCel.IsVisible = false;
                //cel.IsEnabled = false;
                //cel.IsVisible = false;
                cel.Text = "";

                StackRfc.IsVisible = true;
               // rfc.IsVisible = false;
            }

        }
        public void Fn_IrMenu(object sender, EventArgs _Args)
        {
            StackMen.IsVisible = false;
            if(App.v_log=="1")
            {
            App.Current.MainPage = new V_Master(true, "Bienvenido " + App.v_perfil.v_Nombre);
            }
            else if(App.v_log=="0")
            {
            App.Current.MainPage = new V_Master(false, "Bienvenido a Trato Especial");
            }
            else
            {
            App.Current.MainPage = new V_Master(false, "Bienvenido a Trato Especial");
            }

        }
        /// <summary>
        /// cambio en el drop de membresias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_args"></param>
        void Fn_Drop(object sender, EventArgs _args)
        {
            if (tipo.SelectedIndex==2)
            {
                StackEmple.IsVisible = true;
            }
            else
            {
                StackEmple.IsVisible = false;
            }
            mensaje.Text = tipo.SelectedItem.ToString() + "  " + v_costo[tipo.SelectedIndex]+" MXN";
        }
        void Fn_DropEmple(object sender, EventArgs _args)
        {
            int _cosFinal = 0;
            _cosFinal = int.Parse(v_costo[tipo.SelectedIndex]);
            _cosFinal *= int.Parse( PickEmple.SelectedItem.ToString());
            mensaje.Text = tipo.SelectedItem.ToString() + "  " + _cosFinal+ " MXN";
        }
        void Fn_NoNumeros(object sender, TextChangedEventArgs _args)
        {
            Entry _entry = (Entry)sender;
            if (_entry.Text.Length > 0)
            {
                char _ultimo = _entry.Text[_entry.Text.Length-1];
                if(_ultimo>47 && _ultimo<58)
                {
                    _entry.Text = _entry.Text.Remove(_entry.Text.Length - 1); // remove last char
                }
            }
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
        public void Fn_OcultarPrin(object sender, EventArgs _args)
        {
            StackMen.IsVisible = false;
            ReintenRegPri.IsVisible = false;
            Mensajes_over.Text = "";
        }
        public void Fn_ocultar(object sender, EventArgs _args)
        {
            Mensajes_over.Text = "";
            StackMen.IsVisible = false;
            ReintenSec.IsVisible = false;
        }

       


        #region FUNCIONES PARA CREAR TU CUENTA EMPRESARIAL O FAMILIAR

 
        public void Fn_Password(object sender, TextChangedEventArgs _args)
        {
            if(Fol_pass2.Text== Fol_pass.Text)
            {
                confirmar.Text = "Las contraseñas  coinciden";
            }
            else
            {
                confirmar.Text = "Las contraseñas no coinciden";
            }
        }
        void Fn_FolCambio(object sender, EventArgs _args)
        {//revisar en el xml que  familiar sea el 0  emporesarial 1
            if(Fol_DropMembre.SelectedIndex ==0)
            {//familiar
                Stack_Fam.IsVisible = true;
                Stack_Empre.IsVisible = false;
                Fol_Folio.Text = "";
            }
            else
            {
                Stack_Empre.IsVisible = true;
                Stack_Fam.IsVisible = false;
            }
        }
        #endregion
    }
}