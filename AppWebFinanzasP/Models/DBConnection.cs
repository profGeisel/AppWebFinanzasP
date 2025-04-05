using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AppWebFinanzasP.Models
{
	public class DBConnection : DbContext
    {
        // Constructor que define la cadena de conexión de la base de datos
        public DBConnection() : base("name=defaultConnection") { }

        // Propiedad DbSet para la tabla Categorías
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }

        public List<Movimiento> ObtenerMovimientos()
        {
            // Ejecutamos el procedimiento almacenado
            return this.Database.SqlQuery<Movimiento>("EXEC GetMovimientos").ToList();
        }
        public bool EliminarMovimientos(int id)
        {
            // Llamamos al procedimiento almacenado para eliminar el usuario
            var movimientoIdParam = new SqlParameter("@id_tMovimiento", id);
            // Ejecutamos el procedimiento almacenado
            var resultado = this.Database.ExecuteSqlCommand("EXEC spEliminarTipoMovimiento @id_tMovimiento", movimientoIdParam);

            // Si `resultado` es mayor que 0, significa que la eliminación fue exitosa

            return resultado > 0;
        }
        public bool CrearMovimientos(int movimiento)
        {
            // Llamamos al procedimiento almacenado para eliminar el usuario
            var movimientoParam = new SqlParameter("@tMovimiento", movimiento);
            // Ejecutamos el procedimiento almacenado
            var resultado = this.Database.ExecuteSqlCommand("EXEC spInsertarTipoMovimiento @tMovimiento", movimiento);

            // Si `resultado` es mayor que 0, significa que la eliminación fue exitosa

            return resultado > 0;
        }
    }
}