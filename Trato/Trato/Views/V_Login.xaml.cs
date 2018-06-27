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
    public partial class V_Login : ContentPage
    {
        protected override void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public V_Login()
        {

            InitializeComponent();
            /*
             Label monkeyNameLabel = new Label();
            // asi lo haces directamente en c   si no lo quieres asi selecciona el  xaml y lo haces como xml
            Picker picker = new Picker { Title = "Select a monkey" };
            picker.Items.Add("Baboon");
            picker.Items.Add("Capuchin Monkey");
            picker.Items.Add("Blue Monkey");
            picker.Items.Add("Squirrel Monkey");
            picker.Items.Add("Golden Lion Tamarin");
            picker.Items.Add("Howler Monkey");
            picker.Items.Add("Japanese Macaque");

            picker.SelectedIndexChanged += (sender, e) =>
            {
                int selectedIndex = picker.SelectedIndex;
                if (selectedIndex != -1)
                {
                    monkeyNameLabel.Text = picker.Items[selectedIndex];
                }
            };

            Content = new StackLayout
            {
                Margin = new Thickness(20),
                Children = {
                    new Label { Text = "Picker - Data in Items Collection", FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center },
                    picker,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            new Label { Text = "You chose:"},
                            monkeyNameLabel
                        }
                    }
                }
            };
            var layout = new StackLayout { Padding = new Thickness(5, 10) };
            this.Content = layout;
            var label = new Label { FontSize = 20 };
            var s = new FormattedString();
            s.Spans.Add(new Span { Text = "Red Bold", FontAttributes = FontAttributes.Bold });
            s.Spans.Add(new Span { Text = "Default" });
            s.Spans.Add(new Span { Text = "italic small", FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), FontAttributes = FontAttributes.Italic });
            label.FormattedText = s;
            layout.Children.Add(label);*/
            //this.Content.Layout.Chidren
        }
        public async void Fn_Registro(object sender, EventArgs _Args)
        {
            await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new V_Registro(true)) { Title = "registro" });
        }
        //public async void Fn_Login(object sender, EventArgs _args)
        public void Fn_Login(object sender, EventArgs _args)
        {
            if (Fn_Condiciones())
            {

                mensajes.Text = usu.Text + " pass " + pass.Text;
                C_Login _aenviar = new C_Login(usu.Text, pass.Text);
                DisplayAlert("Login Correcto", _aenviar.Fn_GetInfo(), "cancel");

                ////crear el json a enviar
                //string _json = JsonConvert.SerializeObject(_aenviar);
                ////crear el cliente que deja enviar por post
                //HttpClient _clien = new HttpClient();
                ////dar formato al json y enviar
                //StringContent _conten = new StringContent(_json, Encoding.UTF8, "application/json");
                ////se envia y espera resultado
                //HttpResponseMessage result = await _clien.PostAsync("direccion", _conten);
                //if(result.IsSuccessStatusCode)
                //{

                //}
                //// handling the answer  
                //var resultString = await result.Content.ReadAsStringAsync();
                //var post = JsonConvert.DeserializeObject(resultString);
                ////aca pones lo que regresa la base de datos

            }
            else
            {
                DisplayAlert("Login Fallado", "Faltan datos", "cancel");

            }
        }

        bool Fn_Condiciones()
        {
            int _conta = 0;
            if (string.IsNullOrEmpty(usu.Text))
            {
                usu.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                usu.BackgroundColor = Color.Transparent;
            }
            if (string.IsNullOrEmpty(pass.Text))
            {
                pass.BackgroundColor = Color.Red;
                _conta++;
            }
            else
            {
                pass.BackgroundColor = Color.Transparent;
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

    }
}