using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AppWebFinanzasP.Models
{
    [Table("tblTipoMovimiento")]
    public class Movimiento
	{
        [Key]
        public int id_tMovimiento { get; set; }
        public string tMovimiento { get; set; }
    }
}