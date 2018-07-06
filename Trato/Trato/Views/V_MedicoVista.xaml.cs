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
        bool _focus = true;
        public V_MedicoVista (C_Medico _medico)
		{
			InitializeComponent ();
            nombre.Text = _medico.v_Nombre;
            especial.Text = _medico.v_Especialidad;
            domicilio.Text = _medico.v_Domicilio;
            info.Text = _medico.v_Info;
            img.Source = _medico.v_img;
            descuento.IsVisible = false;
            if(App.v_logeado)
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
            especial.Text = _servicios.v_Servicios;
            domicilio.Text = _servicios.v_Domicilio;
            info.Text = _servicios.v_Info;
            img.Source = _servicios.v_img;
            descuento.IsVisible = true;
            descuento.Text = _servicios.v_Descuento;
            if (App.v_logeado)
            {
                boton.IsVisible = true;
            }
            else
            {
                boton.IsVisible = false;
            }
        }

        public void Fn_Click(object sender, EventArgs _args)
        {
            _focus = !_focus;
            if(!_focus)
            {
                boton.Unfocus();
            }
            else
            {
                boton.Focus();
            }

        }

    }
}