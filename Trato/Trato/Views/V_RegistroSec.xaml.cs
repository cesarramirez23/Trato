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
	public partial class V_RegistroSec : ContentPage
	{
		public V_RegistroSec ()
		{
            InitializeComponent();
            Fol_fecha.MaximumDate = DateTime.Now;
            StackFolio.IsVisible = true;
            StackMen.IsVisible = false;
            Fol_pass.Text = "";
            
        }
        #region LAS FUNCIONES DE CREAR CUENTA
        public void Fn_IrMenu(object sender, EventArgs _Args)
        {
            StackMen.IsVisible = false;
            if (App.v_log == "1")
            {
                App.Current.MainPage = new V_Master(true, "Bienvenido " + App.v_perfil.v_Nombre);
            }
            else if (App.v_log == "0")
            {
                App.Current.MainPage = new V_Master(false, "Bienvenido a Trato Especial");
            }
            else
            {
                App.Current.MainPage = new V_Master(false, "Bienvenido a Trato Especial");
            }

        }
        public void Fn_ocultar(object sender, EventArgs _args)
        {
            Mensajes_over.Text = "";
            StackMen.IsVisible = false;
            ReintenSec.IsVisible = false;
            //StackFolio.IsVisible = true;
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
                    StackFolio.IsVisible = false;
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
                    string _direc = NombresAux.BASE_URL + "crear_cuenta.php";
                    //string _direc = NombresAux.BASE_URL + "crear_.php";
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
                    catch (Exception exception)
                    {
                        Mensajes_over.Text = "Error de Conexión";
                        ReintenSec.IsVisible = true;
                    }
                }
            }

        }
        /// <summary>
        /// cuando es crear usuario, folio y contraseña
        /// </summary>
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
        #region FUNCIONES PARA CREAR TU CUENTA EMPRESARIAL O FAMILIAR
        public void Fn_Password(object sender, TextChangedEventArgs _args)
        {
            if (string.IsNullOrEmpty(Fol_pass2.Text) || string.IsNullOrWhiteSpace(Fol_pass2.Text)

                &&  string.IsNullOrEmpty(Fol_pass.Text) || string.IsNullOrWhiteSpace(Fol_pass.Text)   )
            {
                confirmar.IsVisible = false;
            }
            else
            {
                confirmar.IsVisible = true;
                if (Fol_pass2.Text == Fol_pass.Text)
                {
                    confirmar.Text = "Las contraseñas  coinciden";
                }
                else
                {
                    confirmar.Text = "Las contraseñas no coinciden";
                }
            }
        }
        void Fn_FolCambio(object sender, EventArgs _args)
        {//revisar en el xml que  familiar sea el 0  emporesarial 1
            if(Fol_DropMembre.SelectedIndex>-1)
            {
                StackInfo.IsVisible = true;
                if (Fol_DropMembre.SelectedIndex == 0)
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
        }
        #endregion
    }
}