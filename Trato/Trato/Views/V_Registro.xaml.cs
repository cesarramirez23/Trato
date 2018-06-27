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

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Registro : ContentPage
    {

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
            DateTime _date = DateTime.Now;
            fecha.MaximumDate = _date;
            Titulo.Text = _titulo;
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
                DateTime _date = DateTime.Now;
                fecha.MaximumDate = _date;
                Titulo.Text = "Registro";
                Persona.Text = "Persona Fisica";
                fecha.IsEnabled = v_T_Persona;
                lugar.IsEnabled = v_T_Persona;
                tel.IsEnabled = v_T_Persona;
            }
        }
        public V_Registro()
        {
            InitializeComponent();
            DateTime _date = DateTime.Now;
            fecha.MaximumDate = _date;

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
        void Fn_Max(object sender, EventArgs _args)
        {
            Entry _temp = (Entry)sender;
            if (_temp.Text.Length > 2)
            {
                _temp.Text = _temp.Text.Remove(_temp.Text.Length - 1); // remove last char
            }
        }
        public async void Registrar(object sender, EventArgs _args)
        {

            if (Fn_Condiciones())
            {

                ///construir los datos de la tarkjeta que se va a enviar
                C_Tarjeta _Tarjeta = new C_Tarjeta(Persona.Text, correo.Text, cel.Text, tipo.SelectedItem.ToString(), v_costo[tipo.SelectedIndex],
                    Tar_Nombre.Text, Tar_Numero.Text, Tar_Cvc.Text, Tar_Mes.Text, Tar_Año.Text);
                await DisplayAlert("Datos enviados", "ya", "cancel");
            }
            else
            {
                await DisplayAlert("Errores", "errores", "cancel");

            }

            ////HttpClient _cliente = new HttpClient();
            //string _jsonTar = JsonConvert.SerializeObject(_Tarjeta);
            //StringContent _contTar = new StringContent(_jsonTar, Encoding.UTF8, "application/json");
            //////enviar el post
            //string url = "http://jsonplaceholder.typicode.com/posts";
            //HttpResponseMessage _response = await _cliente.PostAsync(url, _contTar);


            //if (v_T_Persona)
            //{
            //    C_Ind_Fisica _Usuario = new C_Ind_Fisica(nombre.Text, rfc.Text, fecha.Date, lugar.Text, giro.Text, tel.Text, cel.Text,
            //        dom.Text, ext.Text, inte.Text, col.Text, ciu.Text, mun.Text, est.Text, cp.Text, correo.Text,tipo.SelectedIndex);

            //    mensaje.Text = _Usuario.Fn_GetInfo();

            //    HttpClient _cli = new HttpClient();
            //    string jsonconv = JsonConvert.SerializeObject(_Usuario);
            //    // create the request content and define Json  
            //    var content = new StringContent(jsonconv, Encoding.UTF8, "application/json");
            //    //  send a POST request  
            //    var uri = "http://jsonplaceholder.typicode.com/posts";

            //    var result = await _cli.PostAsync(uri, content);
            //    if (result.IsSuccessStatusCode)
            //    {
            //        // si se envia todo bien
            //    }
            //    // on error throw a exception  
            //    result.EnsureSuccessStatusCode();

            //    // handling the answer  
            //    var resultString = await result.Content.ReadAsStringAsync();
            //    var post = JsonConvert.DeserializeObject(resultString);


            //    //NavigationPage.SetHasNavigationBar(this, false);


            //    //te encima una nueva pagina, pone solo el boton de regresar
            //    //await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new V_Informacion(1)) { Title = "Informacion" });
            //    //await Navigation.PushAsync(new NavigationPage(new V_Informacion(1)));
            //    await DisplayAlert("Listo", _Usuario.Fn_GetInfo(), "cancel");
            //}
            //else
            //{
            //    C_Ind_Moral _Usuario = new C_Ind_Moral(nombre.Text, rfc.Text, giro.Text, tel.Text,
            //       dom.Text, ext.Text, inte.Text, col.Text, ciu.Text, mun.Text, est.Text, cp.Text, correo.Text, tipo.SelectedIndex);
            //    string jsonconv = JsonConvert.SerializeObject(_Usuario);
            //    mensaje.Text = _Usuario.Fn_GetInfo();

            //    //NavigationPage.SetHasNavigationBar(this, false);
            //    // te encima una nueva pagina, pone solo el boton de regresar

            //    //await Navigation.PushAsync(new NavigationPage(new V_Informacion(2)));
            //    //await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new V_Informacion(1)) { Title = "Informacion" });
            //    await DisplayAlert("Listo", _Usuario.Fn_GetInfo(), "cancel");
            //}



        }

        public async void Folio_Registro(object sender, EventArgs _args)
        {
            if (Fn_Condiciones(true))
            {
                C_Registro _registro = new C_Registro(Fol_folio.Text, Fol_usu.Text, Fol_pass.Text);
                //HttpClient _cliente = new HttpClient();
                //string _json = JsonConvert.SerializeObject(_registro);
                //StringContent _contReg= new StringContent(_json, Encoding.UTF8, "application/json");
                ////enviar el post
                //string url = "http://jsonplaceholder.typicode.com/posts";
                //HttpResponseMessage _response = await _cliente.PostAsync(url, _contReg);


                await DisplayAlert("TODO BIEN", _registro.Fn_GetInfo(), "cancel");
            }
            else
            {
                await DisplayAlert("ERROR", "mensaje de respuesta", "cancel");

            }


        }
        bool Fn_Condiciones(bool _folio)
        {
            int _conta = 0;
            if (string.IsNullOrEmpty(Fol_folio.Text))
            {
                Fol_folio.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                Fol_folio.BackgroundColor = Color.Transparent;
            }

            if (string.IsNullOrEmpty(Fol_usu.Text))
            {
                Fol_usu.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                Fol_usu.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(Fol_pass.Text))
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


        bool Fn_Condiciones()
        {
            int _contador = 0;
            if (string.IsNullOrEmpty(nombre.Text))
            {
                nombre.BackgroundColor = Color.Red;
                _contador++;
            }
            else
            {
                nombre.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(rfc.Text))
            {
                rfc.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                rfc.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(giro.Text))
            {
                giro.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                giro.BackgroundColor = Color.Transparent;
            }
            //calle
            if (string.IsNullOrEmpty(dom.Text))
            {
                dom.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                dom.BackgroundColor = Color.Transparent;
            }//ext
            if (string.IsNullOrEmpty(ext.Text))
            {
                ext.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                ext.BackgroundColor = Color.Transparent;
            }
            //col 
            if (string.IsNullOrEmpty(col.Text))
            {
                col.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                col.BackgroundColor = Color.Transparent;
            }

            if (string.IsNullOrEmpty(ciu.Text))
            {
                ciu.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                ciu.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(mun.Text))
            {
                mun.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                mun.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(est.Text))
            {
                est.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                est.BackgroundColor = Color.Transparent;
            }
            //cp
            if (string.IsNullOrEmpty(cp.Text))
            {
                cp.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                cp.BackgroundColor = Color.Transparent;
            }
            //corr
            if (string.IsNullOrEmpty(correo.Text))
            {
                correo.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                correo.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(cel.Text))
            {
                cel.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                cel.BackgroundColor = Color.Transparent;
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
            if (string.IsNullOrEmpty(Tar_Nombre.Text))
            {
                Tar_Nombre.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                Tar_Nombre.BackgroundColor = Color.Transparent;
            }

            if (string.IsNullOrEmpty(Tar_Numero.Text))
            {
                Tar_Numero.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                Tar_Numero.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(Tar_Cvc.Text))
            {
                Tar_Cvc.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                Tar_Cvc.BackgroundColor = Color.Transparent;
            }

            if (string.IsNullOrEmpty(Tar_Mes.Text) || Tar_Mes.Text.Length != 2)
            {
                Tar_Mes.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                Tar_Mes.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(Tar_Año.Text) || Tar_Año.Text.Length != 2)
            {
                Tar_Año.BackgroundColor = Color.Red; _contador++;
            }
            else
            {
                Tar_Año.BackgroundColor = Color.Transparent;
            }


            if (v_T_Persona)
            {
                //el lugar de nacimiento y la fecha
            }

            if (_contador > 0)
            { return false; }
            else { return true; }


        }

    }
}