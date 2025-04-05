using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebFinanzasP.Models
{
	public class usuario
	{
        public int Id_usuario { get; set; }
        public string Contrasena{ get; set; }
        public string Nom_usuario { get; set; }
        public string Ap1Usuario { get; set; }
        public string Ap2Usuario { get; set; }
        public string CorreoElectronico { get; set; }

        // para confirmar si las contraseñas coinciden (proceso de seguridad)
        public string ConfirmarContrasena { get; set; }
    }
}