using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebFinanzasP.Models
{
	public class RespuestaCreate
	{
        public bool Registrado { get; set; }  // Indica si la operación fue exitosa
        public string Mensaje { get; set; }   // Mensaje adicional (puede ser un mensaje de éxito o error)
    }
}