using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trato.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace Trato.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class V_Filtro : ContentPage
    {
        string[] v_titulos;
        ObservableCollection<Filtro>[] v_listas;
        List<string>[] v_filtro;
        public V_Filtro(string[] _titulos, ObservableCollection<Filtro>[] _listas)
        {
            InitializeComponent();
            v_filtro = App.Fn_Getfiltro();
            v_titulos = _titulos;
            v_listas = _listas;
            Fn_Crea();
        }
        public void Fn_Crea()
        {
            Color _azulprin = (Color)App.Current.Resources["AzulTab"];
            Color _azulLinea = (Color)App.Current.Resources["AzulPrincipal"];
            Color _letraCol = (Color)App.Current.Resources["GrisBold"];

            StackFiltro.Children.Clear();
            StackEstado.Children.Clear();
            StackCiudad.Children.Clear();
            for(int i=0; i<v_listas[0].Count; i++)
            {
                string _te = v_listas[0][i].v_texto;
                Label _texespe = new Label()
                {
                    Text = v_listas[0][i].v_texto,
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = _letraCol,
                    BackgroundColor = Color.White,
                    Margin = new Thickness(0, 0),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                if (v_filtro[0].Contains(v_listas[0][i].v_texto))
                {
                    _texespe.BackgroundColor = (Color)App.Current.Resources["AzulFondo"];
                }

                TapGestureRecognizer _tapTex = new TapGestureRecognizer();
                _tapTex.Tapped += Fn_SetEspe;
                _texespe.GestureRecognizers.Add(_tapTex);
                StackFiltro.Children.Add(_texespe);
            }

            for (int i = 0; i < v_listas[1].Count; i++)
            {
                string _te = v_listas[1][i].v_texto;
                Label _texespe = new Label()
                {
                    Text = v_listas[1][i].v_texto,
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = _letraCol,
                    BackgroundColor = Color.White,
                    Margin = new Thickness(0, 0),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                if (v_filtro[1].Contains(v_listas[1][i].v_texto))
                {
                    _texespe.BackgroundColor = (Color)App.Current.Resources["AzulFondo"];
                }

                TapGestureRecognizer _tapTex = new TapGestureRecognizer();
                _tapTex.Tapped += Fn_SetEstado;
                _texespe.GestureRecognizers.Add(_tapTex);
                StackEstado.Children.Add(_texespe);
            }
            for (int i = 0; i < v_listas[2].Count; i++)
            {
                string _te = v_listas[2][i].v_texto;
                Label _texespe = new Label()
                {
                    Text = v_listas[2][i].v_texto,
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = _letraCol,
                    BackgroundColor = Color.White,
                    Margin = new Thickness(0, 0),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                if (v_filtro[2].Contains(v_listas[2][i].v_texto))
                {
                    _texespe.BackgroundColor = (Color)App.Current.Resources["AzulFondo"];
                }

                TapGestureRecognizer _tapTex = new TapGestureRecognizer();
                _tapTex.Tapped += Fn_SetCiudad;
                _texespe.GestureRecognizers.Add(_tapTex);
                StackCiudad.Children.Add(_texespe);
            }
            
            /*
            for (int _idtitu = 0; _idtitu < v_titulos.Length; _idtitu++)
            {
                StackLayout _todo = new StackLayout() { Orientation = StackOrientation.Vertical, BackgroundColor = Color.White };
                StackLayout _stacktitulo = new StackLayout() { Orientation = StackOrientation.Horizontal, BackgroundColor = Color.White };
                Label _titulos = new Label() { Text = v_titulos[_idtitu], TextColor = _azulprin,
                    HorizontalOptions = LayoutOptions.Start, HorizontalTextAlignment = TextAlignment.Start,
                    BackgroundColor = Color.White
                };
                Image _img = new Image() { Source = "mas.png", HeightRequest = 20, WidthRequest = 20,
                    Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.EndAndExpand };
                ContentView _linea = new ContentView() { HeightRequest = 2, BackgroundColor = _azulLinea,
                    HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 5) };
                TapGestureRecognizer _gest = new TapGestureRecognizer();
                _gest.Tapped += (s, e) =>
                {
                    StackLayout _imgtap = s as StackLayout;
                    Image _imgparent = _imgtap.Children[1] as Image;
                    var _fr = _imgparent.Source as FileImageSource;

                    if (_fr.File == "mas.png")
                    {
                        _imgparent.Source = "Menos.png";
                    }
                    else
                    {
                        _imgparent.Source = "mas.png";
                    }
                    StackLayout _par = (StackLayout)_imgtap.Parent;
                    _par.Children[2].IsVisible = !_par.Children[2].IsVisible;
                };
                _stacktitulo.GestureRecognizers.Add(_gest);
                _stacktitulo.Children.Add(_titulos);
                _stacktitulo.Children.Add(_img);
                _todo.Children.Add(_stacktitulo);
                _todo.Children.Add(_linea);
                StackLayout _stackLista = new StackLayout() { Orientation = StackOrientation.Vertical,
                    IsVisible = false,
                    BackgroundColor = Color.White, Padding = new Thickness(0)
                };
                for (int _idLista = 0; _idLista < v_listas[_idtitu].Count; _idLista++)
                {
                    string _te = v_listas[_idtitu][_idLista].v_texto;
                    Label _texespe = new Label()
                    {
                        Text = v_listas[_idtitu][_idLista].v_texto,
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = _letraCol,
                        BackgroundColor = Color.White,
                        Margin = new Thickness(0, 0),
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                    };
                    if (v_filtro.Contains(v_listas[_idtitu][_idLista].v_texto))
                    {
                        _texespe.BackgroundColor = (Color)App.Current.Resources["AzulFondo"];
                    }

                    TapGestureRecognizer _tapTex = new TapGestureRecognizer();
                    _tapTex.Tapped += (s, e) =>
                     {
                         Label _label = s as Label;
                         if (_label.BackgroundColor == Color.White)
                         {
                             _label.BackgroundColor = (Color)App.Current.Resources["AzulFondo"];
                             v_filtro.Add(_label.Text);
                         }
                         else
                         {
                             _label.BackgroundColor = Color.White;
                             v_filtro.Remove(_label.Text);
                         }
                         App.Fn_GuardaFiltro(v_filtro);
                     };
                    _texespe.GestureRecognizers.Add(_tapTex);
                    _stackLista.Children.Add(_texespe);
                }
                _todo.Children.Add(_stackLista);
                StackFiltro.Children.Add(_todo);
            }
            */
        }
        private void Fn_SetEspe(object sender, EventArgs _args)
        {
            Label _label = sender as Label;
            if (_label.BackgroundColor == Color.White)
            {
                _label.BackgroundColor = (Color)App.Current.Resources["AzulFondo"];
                v_filtro[0].Add(_label.Text);
            }
            else
            {
                _label.BackgroundColor = Color.White;
                v_filtro[0].Remove(_label.Text);
            }
            App.Fn_GuardaFiltro(v_filtro);
        }
        private void Fn_SetEstado(object sender, EventArgs _args)
        {
            Label _label = sender as Label;
            if (_label.BackgroundColor == Color.White)
            {
                _label.BackgroundColor = (Color)App.Current.Resources["AzulFondo"];
                v_filtro[1].Add(_label.Text);
            }
            else
            {
                _label.BackgroundColor = Color.White;
                v_filtro[1].Remove(_label.Text);
            }
            App.Fn_GuardaFiltro(v_filtro);
        }
        private void Fn_SetCiudad(object sender, EventArgs _args)
        {
            Label _label = sender as Label;
            if (_label.BackgroundColor == Color.White)
            {
                _label.BackgroundColor = (Color)App.Current.Resources["AzulFondo"];
                v_filtro[2].Add(_label.Text);
            }
            else
            {
                _label.BackgroundColor = Color.White;
                v_filtro[2].Remove(_label.Text);
            }
            App.Fn_GuardaFiltro(v_filtro);
        }

        private void TapEspe(object sender, EventArgs e)
        {
            StackLayout _imgtap = sender as StackLayout;
            Image _imgparent = _imgtap.Children[1] as Image;
            var _fr = _imgparent.Source as FileImageSource;
            if (_fr.File == "mas.png")
            {
                _imgparent.Source = "Menos.png";
            }
            else
            {
                _imgparent.Source = "mas.png";
            }
            StackFiltro.IsVisible = !StackFiltro.IsVisible;
        }
        private void TapEstado(object sender, EventArgs e)
        {
            StackLayout _imgtap = sender as StackLayout;
            Image _imgparent = _imgtap.Children[1] as Image;
            var _fr = _imgparent.Source as FileImageSource;
            if (_fr.File == "mas.png")
            {
                _imgparent.Source = "Menos.png";
            }
            else
            {
                _imgparent.Source = "mas.png";
            }
            StackEstado.IsVisible = !StackEstado.IsVisible;
        }
        private void TapCiudad(object sender, EventArgs e)
        {
            StackLayout _imgtap = sender as StackLayout;
            Image _imgparent = _imgtap.Children[1] as Image;
            var _fr = _imgparent.Source as FileImageSource;
            if (_fr.File == "mas.png")
            {
                _imgparent.Source = "Menos.png";
            }
            else
            {
                _imgparent.Source = "mas.png";
            }
            StackCiudad.IsVisible = !StackCiudad.IsVisible;
        }
    }   
}