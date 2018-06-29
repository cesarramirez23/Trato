using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Buscador : ContentPage
	{
		public V_Buscador ()
		{
			InitializeComponent ();
           v_mapa= new Map(
            MapSpan.FromCenterAndRadius(
                    new Position(37, -122), Distance.FromMiles(0.3)))
           {
               IsShowingUser = true,
               HeightRequest = 100,
               WidthRequest = 960,
               VerticalOptions = LayoutOptions.FillAndExpand
           };
            Pin _pin = new Pin { Type = PinType.Place, Address = "123,345", Label="Posicion 1", Position =new Position(37, -122) };
            v_mapa.Pins.Add(_pin);
            _pin = new Pin { Type = PinType.Place, Address = "direccion", Label = "Posicion 2", Position = new Position(34, -118) };
            v_mapa.Pins.Add(_pin);
        }
	}
}