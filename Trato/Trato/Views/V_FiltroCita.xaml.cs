using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trato.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Trato.Varios;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_FiltroCita : ContentPage
	{
        List<string>[] v_filtro;
        ObservableCollection<string> v_estados = new ObservableCollection<string>();
        public V_FiltroCita ()
		{
			InitializeComponent ();
            for (int i = 0; i < 6; i++)
            {
                v_estados.Add( ((EstadoCita)i).ToString().Replace('_', ' ') );
            }
            v_filtro = App.Fn_Getfiltro();//lo que esta guardado
            Fn_Crea();
        }
        public void Fn_Crea()
        {
            Color _azulprin = (Color)App.Current.Resources["AzulTab"];
            Color _azulLinea = (Color)App.Current.Resources["AzulPrincipal"];
            Color _letraCol = (Color)App.Current.Resources["GrisBold"];
            StackFiltro.Children.Clear();
            for (int i = 0; i < v_estados.Count; i++)
            {
                string _te = v_estados[i];
                Label _texespe = new Label()
                {
                    Text = v_estados[i],
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = _letraCol,
                    BackgroundColor = Color.White,
                    Margin = new Thickness(0, 0),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                if (v_filtro[0].Contains(v_estados[i]))
                {
                    _texespe.BackgroundColor = (Color)App.Current.Resources["AzulFondo"];
                }
                TapGestureRecognizer _tapTex = new TapGestureRecognizer();
                _tapTex.Tapped += Fn_SetEstadoCita;
                _texespe.GestureRecognizers.Add(_tapTex);
                StackFiltro.Children.Add(_texespe);
            }
        }
        private void Fn_SetEstadoCita(object sender, EventArgs _args)
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
    }
}