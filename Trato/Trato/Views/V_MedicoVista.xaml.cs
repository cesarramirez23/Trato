using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trato.Personas;

using System.Net;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_MedicoVista : ContentPage
	{
        C_Medico v_medico;
        C_Servicios v_servi;
        C_ServGenerales v_gene;
        /// <summary>
        /// 0 medico,    1 servicios medicos,   2 servicios generales
        /// </summary>
        int v_tipo = -1;
      //  bool _personaa =false;

            /*
             
             mailto para correo
             */
        public V_MedicoVista (C_Medico _medico)
		{
			InitializeComponent ();
            sitio.IsVisible = false;
            v_medico = _medico;

            nombre.Text = v_medico.v_titulo+" "+ v_medico.v_Nombre +"  "+ v_medico.v_Apellido;
            especial.Text =v_medico.v_Especialidad;
            domicilio.Text = v_medico.v_Domicilio+","+ v_medico.v_Ciudad;
            info.Text ="Telefono: "+ v_medico.v_Tel+"\nCorreo: "+ v_medico.v_Correo+
            "\nHorario: "+ v_medico.v_horario+
            "\nCedula Profesional: "+v_medico.v_cedula;
            img.Source = v_medico.v_img;
            string conespacio = v_medico.v_descripcion.Replace("/n", Environment.NewLine);
            descrip.Text = " " + conespacio;
            //_personaa = true;
            v_tipo = 0;


            //

            if (App.v_log == "1")
            {
                boton.IsVisible = true;
            }
            //else
            //{ boton.IsVisible = false;
            //}

        }
        public V_MedicoVista(C_Servicios _servicios)
        {
            InitializeComponent();
            StackSitio.IsVisible = true;
            sitio.IsVisible = true;
            v_servi = _servicios;
            nombre.Text = v_servi.v_completo;
            especial.Text = v_servi.v_Especialidad;
            domicilio.Text = v_servi.v_Domicilio + "," + v_servi.v_Ciudad;
            info.Text = "Telefono: " + v_servi.v_Tel + "\nCorreo: " + v_servi.v_Correo +
            "\nHorario: " + v_servi.v_horario;
            sitio.Text= v_servi.v_sitio;
            img.Source = v_servi.v_img;
            string _benef = v_servi.v_beneficio.Replace("/n", Environment.NewLine);
            beneficios.Text = _benef;
            if (string.IsNullOrEmpty(v_servi.v_descripcion) || string.IsNullOrWhiteSpace(v_servi.v_descripcion))
            {
                StackDescrip.IsVisible = false;
            }
            else
            {
                StackDescrip.IsVisible = true;
                string conespacio = v_servi.v_descripcion.Replace("/n",Environment.NewLine);
                descrip.Text = " " + conespacio;

            }
            //_personaa = false;
            v_tipo = 1;

            //if (App.v_log=="1")
            //{
            //    boton.IsVisible = true;
            //}
            //else
            //{
            //    boton.IsVisible = false;
            //}
        }
        public V_MedicoVista(C_ServGenerales _gene)
        {
            InitializeComponent();
            StackSitio.IsVisible = true;
            sitio.IsVisible = true;
            v_gene = _gene;
            nombre.Text = v_gene.v_completo;
            especial.Text = v_gene.v_Especialidad;
            domicilio.Text = v_gene.v_Domicilio + "," + v_gene.v_Ciudad;
            info.Text = "Telefono: " + v_gene.v_Tel + "\nCorreo: " + v_gene.v_Correo +
            "\nHorario: " + v_gene.v_horario;
            sitio.Text =  v_gene.v_sitio;
            img.Source = v_gene.v_img;

            string _benef= v_gene.v_beneficio.Replace("/n", Environment.NewLine);            
            beneficios.Text = _benef;
            if (string.IsNullOrEmpty(v_gene.v_descripcion) || string.IsNullOrWhiteSpace(v_gene.v_descripcion))
            {
                StackDescrip.IsVisible = false;
            }
            else
            {
                string conespacio = v_gene.v_descripcion.Replace("/n", Environment.NewLine);
                descrip.Text = conespacio;
            }
            //descrip.Text = " " + v_gene.v_descripcion;
            //_personaa = false;
            v_tipo = 2;

            //if (App.v_log=="1")
            //{
            //    boton.IsVisible = true;
            //}
            //else
            //{
            //    boton.IsVisible = false;
            //}
        }

        public async void Fn_Cita(object sender, EventArgs _args)
        {
            await Navigation.PushAsync(new V_NCita(true, v_medico) { Title="Nueva Cita" });
        }

        public async void Fn_AbrirSitio(object sender, EventArgs _args)
        {
            string _valor = "";
            Uri uri ;
            if (v_tipo==1 && !string.IsNullOrEmpty( v_servi.v_sitio))
            {
                _valor = v_servi.v_sitio;
 // uri=new Uri(" https://www.google.com.mx/search?rlz=1C1CHFX_enMX782MX782&ei=oWiqW7iHCYaytQWn2JroAQ&q=open+web+direction+xamarin+forms&oq=open+web+direction+xamarin&gs_l=psy-ab.3.0.33i22i29i30k1l3.206506.214130.0.215289.30.28.2.0.0.0.178.2423.22j4.26.0....0...1c.1.64.psy-ab..2.27.2338...0j35i39k1j0i67k1j0i203k1j0i10k1j0i10i203k1j0i22i30k1j0i22i10i30k1j33i21k1j33i160k1j33i22i10i29i30k1.0.5zwAtwR6kng");
                if (Uri.TryCreate(_valor, UriKind.RelativeOrAbsolute, out uri))//se crea correctamente
                {
                    Device.OpenUri(uri);
                }
                else//un error por falta de https
                {
                    if (!Uri.TryCreate("https://" + _valor, UriKind.RelativeOrAbsolute, out uri))
                    {
                        await DisplayAlert("Aviso","No se pudo abrir el sitio web seleccionado", "Aceptar");
                    }
                    else
                    {
                        Device.OpenUri(uri);
                    }
                }
            }
            else if(v_tipo==2 && !string.IsNullOrEmpty(v_gene.v_sitio))
            {
                _valor = v_gene.v_sitio;
                if (Uri.TryCreate(_valor, UriKind.RelativeOrAbsolute, out uri))//se crea correctamente
                {
                    Device.OpenUri(uri);
                }
                else//un error por falta de https
                {
                    if (!Uri.TryCreate("https://" + _valor, UriKind.RelativeOrAbsolute, out uri))
                    {
                        await DisplayAlert("Aviso", "No se pudo abrir el sitio web seleccionado", "Aceptar");
                    }
                    else
                    {
                        Device.OpenUri(uri);
                    }
                }
            }
        }
        public async void Fn_AbrirImg(object sender, EventArgs _args)
        {
            await DisplayAlert("Img", v_gene.v_img, "Aceptar");
            Uri uri;
            if (Uri.TryCreate(v_gene.v_img, UriKind.RelativeOrAbsolute, out uri))//se crea correctamente
            {
                Device.OpenUri(uri);
            }
            else//un error por falta de https
            {
                if (!Uri.TryCreate("https://" + v_gene.v_img, UriKind.RelativeOrAbsolute, out uri))
                {
                    await DisplayAlert("Aviso", "No se pudo abrir el sitio web seleccionado", "Aceptar");
                }
                else
                {
                    Device.OpenUri(uri);
                }
            }
        }

        public  void Fn_AbrirMapa(object sender, EventArgs _args)
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
            //if (_personaa)
            if(v_tipo==0)
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
            else if(v_tipo==1)
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
            else if(v_tipo==2)
            {
                string _nuevo = "";
                for (int i = 0; i < v_gene.v_Domicilio.Length; i++)
                {
                    if (v_gene.v_Domicilio[i] != '#')
                    {
                        _nuevo += v_gene.v_Domicilio[i];
                    }
                }
                direcMapa += _nuevo + ",";
                direcMapa += v_gene.v_Ciudad;
            }
            Uri _direc = new Uri(direcMapa);

            //await DisplayAlert("sadasd", _direc.ToString(), "sadsa"); 
            Device.OpenUri(_direc);
        }
    }
}