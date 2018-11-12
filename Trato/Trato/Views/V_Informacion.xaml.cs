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
    public partial class V_Informacion : ContentPage
    {
       
        public V_Informacion()
        {
            InitializeComponent();
            v_info.Text = "";

            v_info.Text = "Trato Especial, es una multiplataforma de servicios, que a través de una membresía, le damos la oportunidad  a cualquier tipo de persona a tener acceso a servicios de salud, generales y bienestar integral de muy buena calidad a precios accesibles, generando a los usuarios, confianza tranquilidad y un trato especial,  bajo un modelo de suscripción anual.\n" +
                " Sin importar la edad, sexo y condición física del usuario. ";

        }
    }
}