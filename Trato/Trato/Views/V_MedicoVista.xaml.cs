using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Personas;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_MedicoVista : ContentPage
	{
        public V_MedicoVista (C_Medico _medico)
		{
			InitializeComponent ();
            nombre.Text = _medico.v_Nombre + _medico.v_Apellido;
            especial.Text = _medico.v_Especialidad;
            domicilio.Text = _medico.v_Domicilio;
            info.Text = _medico.v_descripcion;
            img.Source = _medico.v_img;
            descuento.IsVisible = false;
            if(App.v_log=="1")
            {
                boton.IsVisible = true;
            }
            else
            {
                boton.IsVisible = false;
            }
        }
        public V_MedicoVista(C_Servicios _servicios)
        {
            InitializeComponent();
            nombre.Text = _servicios.v_Nombre;
            especial.Text = _servicios.v_Especialidad;
            domicilio.Text = _servicios.v_Domicilio;
            info.Text = _servicios.v_descripcion;
            img.Source = _servicios.v_img;
            descuento.IsVisible = true;
           // descuento.Text = _servicios.v_Descuento;
            //"Río + Purificación + 1603,+Las + Águilas,+45080 + Zapopan,+Jal"

                
            if (App.v_log=="1")
            {
                boton.IsVisible = true;
            }
            else
            {
                boton.IsVisible = false;
            }
        }
        public void Fn_AbrirMapa(object sender, EventArgs _args)
        {
            if(Device.RuntimePlatform == Device.Android)
            {
                Uri _direc = new Uri("https://www.google.com.mx/maps/place/Río+Purificación+1603,+Las+Águilas,+45080+Zapopan,+Jal");
                Device.OpenUri(_direc);
            }
            else if(Device.RuntimePlatform== Device.iOS)
            {
                Uri _direc = new Uri("http://maps.apple.com/?q=Río+Purificación+1603,+Las+Águilas,+45080+Zapopan,+Jal");
                Device.OpenUri(_direc);
            }
        }
        public void Fn_Click(object sender, EventArgs _args)
        {

        }

    }
}