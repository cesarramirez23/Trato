using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms; 
using Xamarin.Forms.Xaml;

using Trato.Personas;
using Trato.Varios;
using Newtonsoft.Json;//json normal
using Newtonsoft.Json.Linq;// parse de string a jobject
using System.Net.Http;

using System.Text.RegularExpressions;
using System.Collections.Specialized;

using ZXing.Net.Mobile.Forms;

namespace Trato.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class V_Perfil : TabbedPage
	{
        private ZXingBarcodeImageView barcode;

        bool v_editando = false;
        bool v_editarMed = false;
        Pagar v_pagar;


		public  V_Perfil ()
		{
			InitializeComponent ();

            CargarGen();
            CargarMed();
            Fn_Activo();
        }
        public async void Fn_Activo()
        {
            //1 es ya
            if (App.v_perfil.v_activo == "1")
            {
                G_Editar.IsVisible = true;
                G_Pagar.IsVisible = false;
                M_Editar.IsVisible = true;
                qr_but.IsVisible = true;
                await Task.Delay(10);

            }//no esta activado falta pagar
            else if(App.v_perfil.v_activo=="0")
            {
                string prime = App.v_membresia.Split('-')[0];
                string _membre = "";
                for (int i = 0; i < prime.Length - 1; i++)
                {
                    _membre += prime[i];
                }
                string letra = prime[prime.Length - 1].ToString();
                string _conse = App.v_membresia.Split('-')[1];
                string _folio = App.v_folio;
                string _nombre = "";
                string _costo = "";
                //familiar 1740   indi  580    empre por persona  464
                if (letra=="F")
                {
                    _nombre = "Pago membresia Familiar de Trato Especial";
                    _costo = "1740";
                }
                else if(letra=="I")
                {
                    _nombre = "Pago membresia Indiviual de Trato Especial";
                    _costo = "580";
                }
                else if(letra=="E")
                {
                    _nombre = "Pago membresia Empresarial de Trato Especial";
                    _costo = "464";
                }
                else
                {
                    await DisplayAlert("Error", "error en letra", "aceptar");
                }
                v_pagar = new Pagar(_membre, letra, _conse,_folio, _costo, _nombre);

                G_Editar.IsVisible = false;
                M_Editar.IsVisible = false;
                qr_but.IsVisible = false;
                G_Pagar.IsVisible = true;
                await DisplayAlert("Aviso", "Tu cuenta no está activada, es posible que tengas acceso limitado","Cancelar");
                await DisplayAlert("Info actual", App.v_perfil.Fn_GetDatos(), "Aceptar");
                
            }
        }
        public async void Fn_PagarEfec(object sender, EventArgs _args)
        {
            await Navigation.PushAsync(new V_Pagos(true, v_pagar) { });

        }
        public async void Fn_PagarPay(object sender, EventArgs _args)
        {
            await Navigation.PushAsync(new V_Pagos(false, v_pagar) { });

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
            if(!string.IsNullOrEmpty(App.v_perfil.v_LugNac))
            {
                G_lugar.IsEnabled = false;
            }

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
        public void  Fn_EditarGen(object sender, EventArgs _args)
        {
            v_editando = !v_editando;
            if(v_editando)
            {
                G_Editar.Text = "Cancelar";
                G_Guardar.IsVisible = true;

                G_Ocu.IsEnabled = true;
                G_dom.IsEnabled = true;
                G_ext.IsEnabled = true;
                G_inte.IsEnabled = true;
                G_col.IsEnabled = true;
                G_ciu.IsEnabled = true;
                G_mun.IsEnabled = true;
                G_est.IsEnabled = true;
                G_cp.IsEnabled = true;
                G_Correo.IsEnabled = true;
                G_Tel.IsEnabled = true;
                G_Cel.IsEnabled = true;
            }
            else
            {
                G_Editar.Text = "Editar";
                G_Guardar.IsVisible = false;

                G_Ocu.IsEnabled = false;
                G_dom.IsEnabled = false;
                G_ext.IsEnabled = false;
                G_inte.IsEnabled = false;
                G_col.IsEnabled = false;
                G_ciu.IsEnabled = false;
                G_mun.IsEnabled = false;
                G_est.IsEnabled = false;
                G_cp.IsEnabled = false;
                G_Correo.IsEnabled = false;
                G_Tel.IsEnabled = false;
                G_Cel.IsEnabled = false;

                CargarGen();
            }
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
                await DisplayAlert("Actualizado", "Informacion Guardado con éxito", "Aceptar");
                Fn_EditarGen(sender,_args);
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
            _buton.IsEnabled = true;
        }
        public void Fn_EditarMed(object sender, EventArgs _args)
        {
            v_editarMed = !v_editarMed;
            if (v_editarMed)
            {
                M_Editar.Text = "Cancelar";
                M_Guardar.IsVisible = true;

                Tog_Aler.IsEnabled = true;
                Tog_Enfer.IsEnabled = true;
                M_Sangre.IsEnabled = true;
                M_sexo.IsEnabled = true;
                M_Alergias.IsEnabled = true;
                M_Operaciones.IsEnabled = true;
                M_Enferme.IsEnabled = true;
                M_Medicamentos.IsEnabled = true;
            }
            else
            {
                M_Editar.Text = "Editar";
                M_Guardar.IsVisible = false;

                Tog_Aler.IsEnabled = false;
                Tog_Enfer.IsEnabled = false;
                M_Sangre.IsEnabled = false;
                M_sexo.IsEnabled = false;
                M_Alergias.IsEnabled = false;
                M_Operaciones.IsEnabled = false;
                M_Enferme.IsEnabled = false;
                M_Medicamentos.IsEnabled = false;

                CargarMed();
            }
        }
        public async void Fn_GuardarMed(object sender, EventArgs _Args)
        {
            Button _buton = (Button)sender;
            _buton.IsEnabled = false;
            //se crea el json con la clase mas lel folio y membresia
            string json = @"{";
            json += "idmembre:'" + App.v_membresia + "',\n";
            json += "idfolio:'" + App.v_folio + "',\n";
            json += "sangre:'" + App.Fn_Vacio(M_Sangre.SelectedItem.ToString()) + "',\n";
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
            try
            {
                HttpResponseMessage _respuestphp = await _client.PostAsync(_url, _content);
                string _result = _respuestphp.Content.ReadAsStringAsync().Result;
                if (_result == "1")
                {
                    await DisplayAlert("Actualizado", "Informacion Guardado con éxito", "Aceptar");
                    Fn_EditarMed(sender, _Args);
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
                    try
                    {
                        _respuestphp = await _client.PostAsync(_DirEnviar, _content);
                        string _respuesta = await _respuestphp.Content.ReadAsStringAsync();
                        C_PerfilGen _nuePer = JsonConvert.DeserializeObject<C_PerfilGen>(_respuesta);
                        App.Fn_GuardarDatos(_nuePer, App.v_membresia, App.v_folio);
                        //carga la info del PERFIL MEDICO
                        _DirEnviar = "https://useller.com.mx/trato_especial/query_perfil_medico.php";
                        _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
                        try
                        {
                            //mandar el json con el post
                            _respuestphp = await _client.PostAsync(_DirEnviar, _content);
                            _respuesta = await _respuestphp.Content.ReadAsStringAsync();
                            C_PerfilMed _nuePerMEd = JsonConvert.DeserializeObject<C_PerfilMed>(_respuesta);
                            App.Fn_GuardarDatos(_nuePerMEd, App.v_membresia, App.v_folio);
                            CargarGen();
                            CargarMed();
                        }
                        catch (HttpRequestException exception)
                        {
                            await DisplayAlert("Error", exception.Message, "Aceptar");
                        }
                    }
                    catch (HttpRequestException exception)
                    {
                        await DisplayAlert("Error", exception.Message, "Aceptar");
                    }
                }
                else if (_result == "0")
                {
                    await DisplayAlert("error 0", _result + "\n" + jsonPer.ToString(), "Aceptar");
                }
                else
                {
                    await DisplayAlert("ERROR ", _result + "\n" + jsonPer.ToString(), "Aceptar");
                }
            }
            catch (HttpRequestException exception)
            {
                await DisplayAlert("Error",  exception.Message,"Aceptar");                
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
            App.Fn_GuardarDatos(_nuePer, App.v_membresia, App.v_folio);

            //carga la info del PERFIL MEDICO
            _DirEnviar = "https://useller.com.mx/trato_especial/query_perfil_medico.php";
            _content = new StringContent(_jsonper, Encoding.UTF8, "application/json");
            //mandar el json con el post
           HttpResponseMessage v_respuestphp = await _client.PostAsync(_DirEnviar, _content);

            _respuesta = await v_respuestphp.Content.ReadAsStringAsync();
            C_PerfilMed _nuePerMEd = JsonConvert.DeserializeObject<C_PerfilMed>(_respuesta);

            App.Fn_GuardarDatos(_nuePerMEd, App.v_membresia, App.v_folio);
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

            if(!string.IsNullOrEmpty( App.v_perfMed.v_sangre))
            {
                for(int i=0; i<M_Sangre.Items.Count; i++)
                {
                    if(App.v_perfMed.v_sangre== M_Sangre.Items[i])
                    {
                        M_Sangre.SelectedIndex = i;
                    }

                }
               // M_Sangre.SelectedIndex = App.v_perfMed.v_sangre;
                //M_Sangre.Title = M_Sangre.SelectedItem.ToString();
            }
            M_Sangre.IsEnabled = false;

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
        public  void FN_CrearQR(object sender, EventArgs _Args)
        {
            barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            barcode.BarcodeFormat = ZXing.BarcodeFormat.AZTEC;
            barcode.BarcodeOptions.Width = 700;
            barcode.BarcodeOptions.Height = 700;

            string json = @"{";
            json += "idmembre:'" + App.v_membresia + "',\n";
            json += "idfolio:'" + App.v_folio + "',\n";
            json += "}";

            // barcode.BarcodeValue = qrTexto.Text;

            JObject jsonper = JObject.Parse(json);

            barcode.BarcodeValue = jsonper.ToString();// "hola a todos"; //jsonEnviar.ToString() ;
            //qrTexto.Text = barcode.BarcodeValue;
            qr_content.Content = barcode;
            qr_but.IsEnabled = false;
        }

    }
}