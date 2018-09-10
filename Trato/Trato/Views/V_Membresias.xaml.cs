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
    public partial class V_Membresias :TabbedPage
    {
        public V_Membresias()
        {
            InitializeComponent();
            M_Ind.Text= "IDEAL PARA EMPRENDEDORES, PERSONAS SOLTERAS, JÓVENES Y ADULTOS QUE GUSTEN DE UN ESTILO INDEPENDIENTE \n"
                        +"* Asistencia Telefónica.\n"
                        +"* Acceso a Red Médica.\n"
                        +"* Consultas ilimitadas con Médicos Especialistas a $250 MNX.\n"
                        +"* Consultas ilimitadas con Médicos Generales a $100 MNX.\n"
                        +"* Hospitales y Laboratorios descuentos del 5 % al 35 %\n"
                         +"* Centros de Rehabilitación y Terapias Físicas descuentos del 10 % al 35 %\n"
                          +"* Precios preferenciales y descuentos en muchos servicios más.\n";

            M_Fam.Text = "HASTA 5 FAMILIARES NO IMPORTA QUE NO VIVAN EN EL MISMO DOMICILIO, SIEMPRE Y CUANDO TENGAN PARENTESCO \n"
                   + " IDEAL PARA CUBRIR TODAS LAS NECESIDADES DE SALUD Y BIENESTAR DE TUS SERES QUERIDOS\n"
                        + "* Asistencia Telefónica.\n"
                        + "* Acceso a Red Médica.\n"
                        + "* Consultas ilimitadas con Médicos Especialistas a $250 MNX.\n"
                        + "* Consultas ilimitadas con Médicos Generales a $100 MNX.\n"
                        + "* Hospitales y  Laboratorios descuentos del 5% al 35%\n"
                        + "* Centros de  Rehabilitación y Terapias Físicas descuentos del 10% al 35%\n"
                        + "* Precios preferenciales y descuentos en muchos servicios más.\n";

            M_Emp.Text = "CUBRE LAS NECESIDADES DE SALUD Y BIENESTAR DEL CAPITAL MÁS IMPORTANTE DE TU NEGOCIO, TU EQUIPO DE TRABAJO. \n" +
                          "* Asistencia Telefónica.\n" +
                          "* Acceso a Red Médica.\n"
                        + "* Consultas ilimitadas con Médicos Especialistas a $250 MNX.\n"
                        + "* Consultas ilimitadas con Médicos Generales a $100 MNX.\n"
                        + "* Hospitales y  Laboratorios descuentos del 5% al 35%\n"
                        + "* Centros de  Rehabilitación y Terapias Físicas descuentos del 10% al 35%\n"
                        + "* Precios preferenciales y descuentos en muchos servicios más.\n";
        }
        public async void Fn_Comprar(object _sender, EventArgs _args)
        {
            //await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new V_Registro(false)) { Title = "Registro" });
            await Navigation.PushAsync(new V_Registro(false));
        }
    }
}