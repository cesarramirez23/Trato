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
        C_Medico v_medico;
        C_Servicios v_servi;

        bool _personaa =false;

        public V_MedicoVista (C_Medico _medico)
		{
			InitializeComponent ();
            v_medico = _medico;
            nombre.Text = _medico.v_Nombre + _medico.v_Apellido;
            especial.Text = _medico.v_Especialidad;
            domicilio.Text = _medico.v_Domicilio+","+ v_medico.v_Ciudad;
            info.Text = _medico.v_descripcion;
            img.Source = _medico.v_img;
            descuento.IsVisible = false;
            _personaa = true;
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
            v_servi = _servicios;
            _personaa = false;
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
                string _dir = "https://www.google.com.mx/maps/place/" + v_medico.v_Domicilio+"," ;
                if( _personaa)
                {
                    _dir += v_medico.v_Ciudad;
                }
                else
                {
                    _dir += v_servi.v_Ciudad;
                }
                Uri _direc = new Uri(_dir);
                Device.OpenUri(_direc);
            }
            else if(Device.RuntimePlatform== Device.iOS)
            {
                string _dir = "http://maps.apple.com/?q=" + domicilio.Text + ",";
                if (_personaa)
                {
                    _dir += v_medico.v_Ciudad;
                }
                else
                {
                    _dir += v_servi.v_Ciudad;
                }
                Uri _direc = new Uri(_dir);
                Device.OpenUri(_direc);
            }
        }
        public void Fn_Click(object sender, EventArgs _args)
        {

        }

    }
}