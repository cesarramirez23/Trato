using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Master : MasterDetailPage
	{/*
        las funciones se le agregan en lugar de mandar un view se lo agregas a master.detail
         */
		public V_Master ()
		{
			InitializeComponent ();
		}

        public void Fn_uno()
        {
            Detail = new NavigationPage(new V_Informacion() { Title = "sadsafsaf" });
        }

	}
}