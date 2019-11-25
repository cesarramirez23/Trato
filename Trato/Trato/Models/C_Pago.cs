﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
namespace Trato.Models
{
    class C_Pago
    {
        [JsonProperty("cardReq")]
        public C_Tarjeta v_Tarjeta { get; set; }
        [JsonProperty("amount")]
        public string v_precio { get; set; }
        public async Task Fn_SetTarjeta( string _num, string _fecha, string  _cvc, string _precio)
        {
            v_precio= _precio;
            v_Tarjeta = new C_Tarjeta();
            v_Tarjeta.v_cvc = _cvc;
            v_Tarjeta.v_numTar = _num;
            v_Tarjeta.v_fecha = _fecha;
            await Fn_Init();
        }
        Task Fn_Init()
        {
            string _val = v_Tarjeta.v_fecha[0].ToString() + v_Tarjeta.v_fecha[1].ToString() +
                v_Tarjeta.v_fecha[3].ToString() + v_Tarjeta.v_fecha[4].ToString();
       
            v_Tarjeta.v_fecha = _val;
            return Task.Delay(100);
        }
        public override string ToString()
        {
            return "Costo: " + v_precio + "\n" + v_Tarjeta.ToString();
        }
    }
    class C_Tarjeta
    {
        [JsonProperty("pan")]
        public string v_numTar { get; set; }
        [JsonProperty("expDate")]
        public string v_fecha { get; set; }
        [JsonProperty("cvv2")]
        public string v_cvc { get; set; }
        public override string ToString()
        {
            return v_numTar + "   \n" + v_fecha + "  " + v_cvc;
        }
    }
}
