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
            string _hora = "";
            string[] _horas;
            v_medico = _medico;
            //hh: mm: ss
            if(!string.IsNullOrEmpty( v_medico.v_horAper))
            {
                _horas = v_medico.v_horAper.Split(':');
                _hora = _horas[0] + ":" + _horas[1];
            }else
            {
                _hora = "N/A";
            }
            if (!string.IsNullOrEmpty(v_medico.v_horCierra))
            {
                _horas = v_medico.v_horCierra.Split(':');
                _hora += "  -  " + _horas[0] + ":" + _horas[1];
            }
            else
            {
                _hora += "  -  N/A";
            }

            nombre.Text = v_medico.v_titulo+" "+ v_medico.v_Nombre +"  "+ v_medico.v_Apellido;
            especial.Text ="Especialista en "+ v_medico.v_Especialidad;
            domicilio.Text = v_medico.v_Domicilio+","+ v_medico.v_Ciudad;
            info.Text ="Telefono: "+ v_medico.v_Tel+"\n Correo: "+ v_medico.v_Correo+
            "\nHorario: "+ _hora+
            "\nCedula Profesional: "+v_medico.v_cedula;
            img.Source = v_medico.v_img;
            descrip.Text=" " +v_medico.v_descripcion;
            _personaa = true;
            //if(App.v_log=="1")
            //{
            //    boton.IsVisible = true;
            //}
            //else
            //{
            //    boton.IsVisible = false;
            //}
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
            descrip.IsVisible = true;
           // descuento.Text = _servicios.v_Descuento;
            //"Río + Purificación + 1603,+Las + Águilas,+45080 + Zapopan,+Jal"

                
            //if (App.v_log=="1")
            //{
            //    boton.IsVisible = true;
            //}
            //else
            //{
            //    boton.IsVisible = false;
            //}
        }
        public async  void Fn_AbrirMapa(object sender, EventArgs _args)
        {
            string direcMapa = "";
            if(Device.RuntimePlatform == Device.Android)
            {
                direcMapa= "http://maps.google.com/?daddr=";
               
            }
            else if(Device.RuntimePlatform== Device.iOS)
            {
                direcMapa = "http://maps.apple.com/?q=";                
            }
            if (_personaa)
            {
                string _nuevo = "";
                for (int i = 0; i < v_medico.v_Domicilio.Length; i++)
                {
                    if (v_medico.v_Domicilio[i] != '#')
                    {
                        _nuevo += v_medico.v_Domicilio[i];
                    }
                }
                direcMapa += _nuevo + ",";
                direcMapa += v_medico.v_Ciudad;
            }
            else
            {
                string _nuevo = "";
                for (int i = 0; i < v_servi.v_Domicilio.Length; i++)
                {
                    if (v_servi.v_Domicilio[i] != '#')
                    {
                        _nuevo += v_servi.v_Domicilio[i];
                    }
                }
                direcMapa += _nuevo + ",";
                direcMapa += v_servi.v_Ciudad;
            }
            Uri _direc = new Uri(direcMapa);
            await DisplayAlert("Enviar ", direcMapa + "\n" + _direc, "aceptar");
            Device.OpenUri(_direc);
        }
    }
}