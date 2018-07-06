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
	public partial class V_Master : MasterDetailPage
	{/*
        las funciones se le agregan en lugar de mandar un view se lo agregas a master.detail
         */
         
		public V_Master()
		{
			InitializeComponent ();

        }

        public void Fn_uno()
        {
            Detail = new V_Buscador() { Title = "Buscador" };
        }
        public void Fn_dos()
        {
            Detail =  new LISTA() { Title = "Lista" };//new V_Buscador() { Title = "Buscador" };
           // Page _page= new Page();
            //_page.Layout
            //var map = new Map(
            //MapSpan.FromCenterAndRadius(
            //        new Position(37, -122), Distance.FromMiles(0.3)))
            //{
            //    IsShowingUser = true,
            //    HeightRequest = 100,
            //    WidthRequest = 960,
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};
            //var stack = new StackLayout { Spacing = 0 };
            //stack.Children.Add(map);
            //_page.Content = stack;
            //Detail = new Page()
            //Master.IsVisible = false;
        }

    }
}