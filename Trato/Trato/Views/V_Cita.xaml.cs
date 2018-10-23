using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Trato.Varios;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Cita : ContentPage
	{
        ObservableCollection<Cita> citas = new ObservableCollection<Cita>();
        public V_Cita()
        {
            InitializeComponent();
            DateTime _fecha = new DateTime(2018, 9, 23, 14, 00, 00);
            citas.Add(new Cita()
            {
                v_nombreDR = "nombre 1",
               // v_fecha = _fecha,
                v_hora = _fecha.TimeOfDay,                 
                v_estado = "0"

            });

            _fecha = new DateTime(2018, 9, 25, 19, 42, 00);
            citas.Add(new Cita()
            {
                v_nombreDR = "nombre 2",
               // v_fecha = _fecha,
                v_hora = _fecha.TimeOfDay,
                v_estado = "1"
            });

            _fecha = new DateTime(2018, 11, 25, 09, 42, 00);
            citas.Add(new Cita()
            {
                v_nombreDR = "nombre 2",
              //  v_fecha = _fecha,
                v_hora = _fecha.TimeOfDay,
                v_estado = "2"
            });

            _fecha = new DateTime(2018, 9, 7, 12, 12, 00);
            citas.Add(new Cita()
            {
                v_nombreDR = "nombre 2",
                //v_fecha = _fecha,
                v_hora = _fecha.TimeOfDay,
                v_estado = "3"
            });
            _fecha = new DateTime(2018, 9, 7, 12, 12, 00);
            citas.Add(new Cita()
            {
                v_nombreDR = "nombre 2",
                //v_fecha = _fecha,
                v_hora = _fecha.TimeOfDay,
                v_estado = "4"
            });

            Ordenar();
            ListaCita.ItemsSource = citas;
        }
        public void Ordenar()
        {
            IEnumerable<Cita> _temp = citas.OrderBy(x => x.v_fecha);
            citas = new ObservableCollection<Cita>(_temp);

            for (int i = 0; i < citas.Count; i++)
            {
                citas[i].Fn_CAmbioCol(i);
            }
        }
    }
}