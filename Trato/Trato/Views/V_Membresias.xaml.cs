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
            M_Ind.Text= "Ésta es la membresía ideal para una persona independiente como tu. \n"
                        +"* Asistencia Telefónica.\n"
                        +"* Acceso a Red Médica.\n"
                        + "* Consultas ilimitadas con Médicos Especialistas a $350 o $250 MNX.\n"
                        + "* Consultas ilimitadas con Médicos Generales a $100 MNX.\n"
                        +"* Hospitales y Laboratorios descuentos del 5 % al 35 %\n"
                         +"* Centros de Rehabilitación y Terapias Físicas descuentos del 10 % al 35 %\n"
                          +"* Precios preferenciales y descuentos en muchos servicios más.\n";

            M_Fam.Text = "La membresía familiar es ideal para cubrir todas las necesidades de salud y bienestar de tus seres queridos.\n"
                        + "* Asistencia Telefónica.\n"
                        + "* Acceso a Red Médica.\n"
                        + "* Consultas ilimitadas con Médicos Especialistas a $350 o $250 MNX.\n"
                        + "* Consultas ilimitadas con Médicos Generales a $100 MNX.\n"
                        + "* Hospitales y  Laboratorios descuentos del 5% al 35%\n"
                        + "* Centros de  Rehabilitación y Terapias Físicas descuentos del 10% al 35%\n"
                        + "* Precios preferenciales y descuentos en muchos servicios más.\n";

            M_Emp.Text = "Cubre las necesidades de salud y bienestar del capital mas importante de tu negocio, tu Equipo de trabajo. \n" 
                        + "* Asistencia Telefónica.\n" 
                        + "* Acceso a Red Médica.\n"
                        + "* Consultas ilimitadas con Médicos Especialistas a $350 o $250 MNX.\n"
                        + "* Consultas ilimitadas con Médicos Generales a $100 MNX.\n"
                        + "* Hospitales y  Laboratorios descuentos del 5% al 35%\n"
                        + "* Centros de  Rehabilitación y Terapias Físicas descuentos del 10% al 35%\n"
                        + "* Precios preferenciales y descuentos en muchos servicios más.\n";
            if (App.v_log == "1")
            {
                ButEmp.IsVisible = false;
                ButFam.IsVisible = false;
                ButInd.IsVisible = false;
            }
        }
        public async void Fn_Comprar(object _sender, EventArgs _args)
        {
            //await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new V_Registro(false)) { Title = "Registro" });
            await Navigation.PushAsync(new V_Registro(false,0));
        }
        public async void Fn_CompraInd(object sender, EventArgs _Args)
        {
            await Navigation.PushAsync(new V_Registro(false,0));
        }
        public async void Fn_CompraFam(object sender, EventArgs _Args)
        {
            await Navigation.PushAsync(new V_Registro(false,1));
        }
        public async void Fn_CompraEmpre(object sender, EventArgs _args)
        {
            await Navigation.PushAsync(new V_Registro(false,2));
        }
    }
}