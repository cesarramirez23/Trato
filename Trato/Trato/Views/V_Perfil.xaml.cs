using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms; 
using Xamarin.Forms.Xaml;

using Trato.Personas;
using Newtonsoft.Json;//json normal
using Newtonsoft.Json.Linq;// parse de string a jobject
using System.Net.Http;

using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Perfil : TabbedPage
	{
       

		public V_Perfil ()
		{
			InitializeComponent ();
            CargarGen();
            CargarMed();
        }
        public async void CargarGen()
        {
            App.Fn_CargarDatos();
            Fn_NullEntry( G_Nombre, App.v_perfil.v_Nombre);
            // si no es el titular no se muestra el rfc
            if ( int.Parse(App.v_folio)>0 )
            {
                G_rfc.Text = "";
                G_rfc.IsVisible = false;
            }
            else
            {
                Fn_NullEntry(G_rfc,App.v_perfil.v_Rfc);
            }
            if (string.IsNullOrEmpty(App.v_perfil.v_FecNaci))
            {
                G_fecha.Date = DateTime.Now;
                G_fecha.IsEnabled = true;
            }
            else
            {
                string[] fecha = App.v_perfil.v_FecNaci.Split('-');
                G_fecha.Date = new DateTime(int.Parse(fecha[0]), int.Parse(fecha[1]), int.Parse(fecha[2]));
                G_fecha.IsEnabled = false;
            }
            if ((App.v_perfil.v_idsexo<0) || (App.v_perfil.v_idsexo>1) )
            {
                G_sexoPick.IsEnabled = true;
            }
            else
            {
                G_sexoPick.SelectedIndex = App.v_perfil.v_idsexo;
                G_sexoPick.Title = G_sexoPick.SelectedIndex.ToString();
                G_sexoPick.IsEnabled = false;
            }

            Fn_NullEntry(G_lugar, App.v_perfil.v_LugNac);
            Fn_NullEntry(G_Ocu, App.v_perfil.v_Ocup);
            Fn_NullEntry(G_Tel, App.v_perfil.v_Tel);
            Fn_NullEntry(G_Cel, App.v_perfil.v_Cel);
            Fn_NullEntry(G_dom, App.v_perfil.v_Calle);
            Fn_NullEntry(G_ext,App.v_perfil.v_NumExt);
            Fn_NullEntry(G_inte,App.v_perfil.v_NumInt);
            Fn_NullEntry(G_col, App.v_perfil.v_Colonia);
            Fn_NullEntry(G_ciu,App.v_perfil.v_Ciudad);
            Fn_NullEntry(G_mun, App.v_perfil.v_municipio);
            Fn_NullEntry(G_est,App.v_perfil.v_Estado);
            Fn_NullEntry(G_cp,App.v_perfil.v_Cp);
            Fn_NullEntry(G_Correo,App.v_perfil.v_Correo);

            await Task.Delay(100);
        }
        public void Fn_NullEntry(Entry _entry, string _textos)
        {
            if (string.IsNullOrEmpty(_textos))
            {
                _entry.Text = "";
            }
            else
            {
                _entry.Text = _textos;
            }
        }
        public async void Fn_GuardarGen(object sender, EventArgs _args)
        {
            Button _buton = (Button)sender;
            _buton.IsEnabled = false;
            //se crea el json con la clase mas lel folio y membresia
            string json = @"{";
            json += "idmembre:'" + App.v_membresia+ "',\n";
            json += "idfolio:'" + App.v_folio + "',\n";
            json += "nombre:'" + App.Fn_Vacio(G_Nombre.Text) + "',\n";
            json += "rfc:'" + App.Fn_Vacio(G_rfc.Text) + "',\n";
            json += "fechanac:'" + Fn_GetFecha() + "',\n";
            json += "lugnac:'" + App.Fn_Vacio(G_lugar.Text) + "',\n";
            json += "ocu:'" + App.Fn_Vacio(G_Ocu.Text) + "',\n";
            json += "idsexo:'" + G_sexoPick.SelectedIndex + "',\n";
            json += "tel:'" + App.Fn_Vacio(G_Tel.Text) + "',\n";
            json += "cel:'" + App.Fn_Vacio(G_Cel.Text) + "',\n";
            json += "calle:'" + App.Fn_Vacio(G_dom.Text) + "',\n";
            json += "numext:'" + App.Fn_Vacio(G_ext.Text) + "',\n";
            json += "numint:'" + App.Fn_Vacio(G_inte.Text) + "',\n";
            json += "colonia:'" + App.Fn_Vacio(G_col.Text) + "',\n";
            json += "ciudad:'" + App.Fn_Vacio(G_ciu.Text) + "',\n";
            json += "municipio:'" + App.Fn_Vacio(G_mun.Text) + "',\n";
            json += "estado:'" + App.Fn_Vacio(G_est.Text) + "',\n";
            json += "cp:'" + App.Fn_Vacio(G_cp.Text) + "',\n";
            json += "correo:'" + App.Fn_Vacio(G_Correo.Text) + "',\n";
            json += "}";
            JObject jsonPer = JObject.Parse(json);
            StringContent _content = new StringContent(jsonPer.ToString(), Encoding.UTF8, "application/json");
            HttpClient _client = new HttpClient();
            string _url = "https://useller.com.mx/trato_especial/update_perfil.php";
            HttpResponseMessage _respuestphp=  await _client.PostAsync(_url, _content);
            string _result = _respuestphp.Content.ReadAsStringAsync().Result;

            if(_result=="1")
            {
                await DisplayAlert("Actualizado",  _result , "Aceptar");
                //volver a cargar la informacion
                Perf _perf = new Perf();
                _perf.v_fol = App.v_folio;
                _perf.v_membre = App.v_membresia;
                //crear el json
                string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);

                //HACIENDO EL QUERY para la info del GENERAL
                _client = new HttpClient();
                string _DirEnviar = "https://useller.com.mx/trato_especial/query_perfil.php";
                _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                //mandar el json con el post
                _respuestphp = await _client.PostAsync(_DirEnviar, _content);
                string _respuesta = await _respuestphp.Content.ReadAsStringAsync();
                C_PerfilGen _nuePer = JsonConvert.DeserializeObject<C_PerfilGen>(_respuesta);

                App.Fn_GuardarDatos(_nuePer, App.v_membresia, App.v_folio);






                //carga la info del PERFIL MEDICO
                _DirEnviar = "https://useller.com.mx/trato_especial/query_perfil_medico.php";
                _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                //mandar el json con el post
                _respuestphp = await _client.PostAsync(_DirEnviar, _content);

                _respuesta = await _respuestphp.Content.ReadAsStringAsync();
                C_PerfilMed _nuePerMEd = JsonConvert.DeserializeObject<C_PerfilMed>(_respuesta);

                App.Fn_GuardarDatos(_nuePerMEd, App.v_membresia, App.v_folio);



                CargarGen();
                CargarMed();

            }
            else if(_result=="0")
            {
                await DisplayAlert("Error en actualizar",  _result+"\n"+ jsonPer.ToString(), "Aceptar");                
            }
            else
            {
                await DisplayAlert("NO 0 NI 1",  _result + "\n" + jsonPer.ToString(), "Aceptar");                
                
            }


            //
            
            _buton.IsEnabled = true;
        }
        public async void Fn_GuardarMed(object sender, EventArgs _Args)
        {
            Button _buton = (Button)sender;
            _buton.IsEnabled = false;
            //se crea el json con la clase mas lel folio y membresia
            string json = @"{";
            json += "idmembre:'" + App.v_membresia + "',\n";
            json += "idfolio:'" + App.v_folio + "',\n";
            json += "sangre:'" + App.Fn_Vacio(M_Sangre.Text) + "',\n";
            json += "idsexo:'" +M_sexoPick.SelectedIndex + "',\n";

            if(M_sexoPick.SelectedIndex==1)
            {
                json += "infoMuj:'" +App.Fn_Vacio (M_sexo.Text)+ "',\n";
            }
            else
            {
                json += "infoMuj:'" +""+ "',\n";
            }

            if(Tog_Aler.IsToggled)
            {
                json += "alergias:'" + App.Fn_Vacio(M_Alergias.Text) + "',\n";

            }
            else
            {
                json += "alergias:'" + ""+ "',\n";
            }

            json += "operaciones:'" + App.Fn_Vacio(M_Operaciones.Text) + "',\n";


            if (Tog_Enfer.IsToggled)
            {
                json += "enfermedades:'" + App.Fn_Vacio(M_Enferme.Text) + "',\n";

            }
            else
            {
                json += "enfermedades:'" + "" + "',\n";
            }

            json += "medicamentos:'" + App.Fn_Vacio(M_Medicamentos.Text) + "',\n";
            json += "}";
            JObject jsonPer = JObject.Parse(json);
            StringContent _content = new StringContent(jsonPer.ToString(), Encoding.UTF8, "application/json");
            HttpClient _client = new HttpClient();
            string _url = "https://useller.com.mx/trato_especial/update_perfil_medico.php";
            HttpResponseMessage _respuestphp = await _client.PostAsync(_url, _content);
            string _result = _respuestphp.Content.ReadAsStringAsync().Result;



            if(_result=="1")
            {
                await DisplayAlert("Actualizado", _result, "Aceptar");
                //volver a cargar la informacion

                Perf _perf = new Perf();
                _perf.v_fol = App.v_folio;
                _perf.v_membre = App.v_membresia;
                //crear el json
                string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);

                //HACIENDO EL QUERY para la info del GENERAL
                 _client = new HttpClient();
                string _DirEnviar = "https://useller.com.mx/trato_especial/query_perfil.php";
                 _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                //mandar el json con el post
                 _respuestphp = await _client.PostAsync(_DirEnviar, _content);
                string _respuesta = await _respuestphp.Content.ReadAsStringAsync();
                C_PerfilGen _nuePer = JsonConvert.DeserializeObject<C_PerfilGen>(_respuesta);

                App.Fn_GuardarDatos(_nuePer, App.v_membresia, App.v_folio);






                //carga la info del PERFIL MEDICO
                _DirEnviar = "https://useller.com.mx/trato_especial/query_perfil_medico.php";
                _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                //mandar el json con el post
                _respuestphp = await _client.PostAsync(_DirEnviar, _content);

                _respuesta = await _respuestphp.Content.ReadAsStringAsync();
                C_PerfilMed _nuePerMEd = JsonConvert.DeserializeObject<C_PerfilMed>(_respuesta);

                App.Fn_GuardarDatos(_nuePerMEd, App.v_membresia, App.v_folio);

                CargarGen();
                CargarMed();
            }
            else if (_result == "0")
            {
                await DisplayAlert("error 0", _result+"\n"+ jsonPer.ToString(), "Aceptar");
            }
            else
            {

                await DisplayAlert("ERROR ", _result + "\n" + jsonPer.ToString(), "Aceptar");
            }

            _buton.IsEnabled = true;
        }

        public async void Fn_CargaQuery()
        {
            Perf _perf = new Perf();
            _perf.v_fol = App.v_folio;
            _perf.v_membre = App.v_membresia;
            //crear el json
            string _jsonper = JsonConvert.SerializeObject(_perf, Formatting.Indented);

            //HACIENDO EL QUERY para la info del GENERAL
            HttpClient _client = new HttpClient();
            string _DirEnviar = "https://useller.com.mx/trato_especial/query_perfil.php";
           StringContent _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
            //mandar el json con el post
            HttpResponseMessage _respuestphp = await _client.PostAsync(_DirEnviar, _content);
            string _respuesta = await _respuestphp.Content.ReadAsStringAsync();
            C_PerfilGen _nuePer = JsonConvert.DeserializeObject<C_PerfilGen>(_respuesta);
            M_Mensajes.Text += " general"+_respuesta;

            App.Fn_GuardarDatos(_nuePer, App.v_membresia, App.v_folio);






            //carga la info del PERFIL MEDICO
            _DirEnviar = "https://useller.com.mx/trato_especial/query_perfil_medico.php";
            _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
            //mandar el json con el post
           HttpResponseMessage v_respuestphp = await _client.PostAsync(_DirEnviar, _content);

            _respuesta = await v_respuestphp.Content.ReadAsStringAsync();
            C_PerfilMed _nuePerMEd = JsonConvert.DeserializeObject<C_PerfilMed>(_respuesta);
            M_Mensajes.Text +="Medico json" +_respuesta;

            App.Fn_GuardarDatos(_nuePerMEd, App.v_membresia, App.v_folio);
            M_Mensajes.Text +="Medico guardado " +App.v_perfMed.Fn_Info();
            await Task.Delay(100);
        }

        public string Fn_GetFecha()
        {
            string v_FecNaci = "";
            string _month = "";
            if (G_fecha.Date.Month < 10)
            {
                _month = "0" + G_fecha.Date.Month.ToString();
            }
            else
            {
                _month = G_fecha.Date.Month.ToString();
            }
            string _day = "";
            if (G_fecha.Date.Day < 10)
            {
                _day = "0" + G_fecha.Date.Day.ToString();
            }
            else
            {
                _day = G_fecha.Date.Day.ToString();
            }
            v_FecNaci = G_fecha.Date.Year.ToString() + "-" + _month + "-" + _day;
            return v_FecNaci;
        }
        public void Fn_PickSexo(object sender, EventArgs _args)
        {
            if(M_sexoPick.SelectedIndex==0)
            {
                M_sexo.IsVisible = false;
                M_sexolbl.IsVisible = false;
                M_sexolbl.Text = "";
                M_sexo.Text = "";
            }
            else if(M_sexoPick.SelectedIndex==1)
            {
                M_sexo.IsVisible = true;
                M_sexolbl.IsVisible = true;
                M_sexolbl.Text = "¿estas embarazada?,\n ¿tienes hijos? ¿cuantos?";

            }
        }
        public void Fn_SwiMedica(object sender, ToggledEventArgs _args)
        {
            M_Medicamentos.IsVisible = _args.Value;
            if (!_args.Value)
                M_Medicamentos.Text = "";
        }
        public void Fn_SwiAlergias(object sender, ToggledEventArgs _args)
        {
            M_Alergias.IsVisible = _args.Value;
            if (!_args.Value)
                M_Alergias.Text = "";
        }
        public void Fn_SwiEnfer(object sender, ToggledEventArgs _args)
        {
            M_Enferme.IsVisible = _args.Value;
            if (!_args.Value)
                M_Enferme.Text = "";
        }
        public void Fn_NullEntry(Editor _editor, string _textos)
        {
            if (string.IsNullOrEmpty(_textos))
            {
                _editor.Text = "";
            }
            else
            {
                _editor.Text = _textos;
            }
        }
        public async void CargarMed()
        {
            App.Fn_CargarDatos();
            M_Mensajes.Text += "Cargandosssssssssss" + Application.Current.Properties["perfMed"] as string;
            Fn_NullEntry(M_Sangre, App.v_perfMed.v_sangre);
            if ((App.v_perfMed.v_sexo < 0) || (App.v_perfMed.v_sexo > 1))
            {
                M_sexoPick.IsEnabled = true;
                if(App.v_perfMed.v_sexo==1)
                {
                    M_sexo.IsVisible = true;
                    M_sexolbl.IsVisible = true;
                    M_sexolbl.Text = "¿estas embarazada?,\n ¿tienes hijos? ¿cuantos?";
                    Fn_NullEntry(M_sexo, App.v_perfMed.v_infoMujer);
                }
                else
                {
                    M_sexo.IsVisible = false;
                    M_sexolbl.IsVisible = false;
                    M_sexolbl.Text = "";
                    Fn_NullEntry(M_sexo, App.v_perfMed.v_infoMujer);
                }
            }
            else
            {
                M_sexoPick.SelectedIndex = App.v_perfMed.v_sexo;
                M_sexoPick.Title = M_sexoPick.SelectedIndex.ToString();
                M_sexoPick.IsEnabled = false;
                if (App.v_perfMed.v_sexo == 1)
                {
                    M_sexo.IsVisible = true;
                    M_sexolbl.IsVisible = true;
                    M_sexolbl.Text = "¿estas embarazada?,\n ¿tienes hijos? ¿cuantos?";
                    Fn_NullEntry(M_sexo, App.v_perfMed.v_infoMujer);
                }
                else
                {
                    M_sexo.IsVisible = false;
                    M_sexolbl.IsVisible = false;
                    M_sexolbl.Text = "";
                }
            }


            if(string.IsNullOrEmpty( App.v_perfMed.v_alergias))
            {
                Tog_Aler.IsToggled = false;
                M_Alergias.IsVisible = false;
                Fn_NullEntry(M_Alergias, App.v_perfMed.v_alergias);
            }
            else
            {
                M_Alergias.IsVisible = true;
                Tog_Aler.IsToggled = true;
                Fn_NullEntry(M_Alergias, App.v_perfMed.v_alergias);
            }

            Fn_NullEntry(M_Operaciones, App.v_perfMed.v_operaciones);


            if (string.IsNullOrEmpty(App.v_perfMed.v_enfer))
            {
                Tog_Enfer.IsToggled = false;
                M_Enferme.IsVisible = false;
                Fn_NullEntry(M_Enferme, App.v_perfMed.v_enfer);
            }
            else
            {
                M_Enferme.IsVisible = true;
                Tog_Enfer.IsToggled = true;
                Fn_NullEntry(M_Enferme, App.v_perfMed.v_enfer);
            }

            Fn_NullEntry(M_Medicamentos, App.v_perfMed.v_medica);

            await Task.Delay(100);
        }



    }
}