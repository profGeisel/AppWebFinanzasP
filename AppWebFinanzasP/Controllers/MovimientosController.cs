using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebFinanzasP.Models;

namespace AppWebFinanzasP.Controllers
{
    public class MovimientosController : Controller
    {
        private DBConnection db = new DBConnection();
        // GET: Movimiento
        public ActionResult Index()
        {
            // Obtener los movimientos utilizando el procedimiento almacenado
            var movimientos = db.ObtenerMovimientos(); // Llama al método que ejecuta el SP
            return View(movimientos);  // Pasar los datos a la vista
        }

        // GET: Categorias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movimientos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movimiento movimiento)
        {
            // Creamos el objeto de respuesta
            var respuestaCreate = new RespuestaCreate();

            // Llamamos al procedimiento almacenado para insertar el movimiento
            var movimientoParam = new SqlParameter("@tMovimiento", movimiento.tMovimiento);

            try
            {
                // Ejecutamos el procedimiento almacenado
                var resultado = db.Database.ExecuteSqlCommand("EXEC spInsertarTipoMovimiento @tMovimiento", movimientoParam);

                // Si el resultado es mayor que 0, la inserción fue exitosa
                if (resultado > 0)
                {
                    respuestaCreate.Registrado = true;
                    respuestaCreate.Mensaje = "Movimiento registrado exitosamente.";

                    // Almacenamos el mensaje en TempData para redirigirlo a la vista Index
                    TempData["Mensaje"] = respuestaCreate.Mensaje;
                    TempData["Registrado"] = respuestaCreate.Registrado;

                    // Redirigimos a la acción Index
                    return RedirectToAction("Index");
                }
                else
                {
                    respuestaCreate.Registrado = false;
                    respuestaCreate.Mensaje = "Error al registrar el movimiento.";

                    // Almacenamos el mensaje de error en TempData
                    TempData["Mensaje"] = respuestaCreate.Mensaje;
                    TempData["Registrado"] = respuestaCreate.Registrado;

                    // Redirigimos a la acción Index con el mensaje
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // En caso de error, devolvemos un mensaje de error
                respuestaCreate.Registrado = false;
                respuestaCreate.Mensaje = $"Error inesperado: {ex.Message}";

                // Almacenamos el mensaje de error en TempData
                TempData["Mensaje"] = respuestaCreate.Mensaje;
                TempData["Registrado"] = respuestaCreate.Registrado;

                // Redirigimos a la acción Index con el mensaje
                return RedirectToAction("Index");
            }
        }
        

        // GET: Movimientos/Delete/5
        public ActionResult Delete(int id)
        {
            // Llamar al servicio para eliminar el movimiento
            bool movimiento = db.EliminarMovimientos(id);

            if (movimiento)
            {
                // Si la eliminación fue exitosa, redirigir a la lista de movimientos (o donde sea necesario)
                return RedirectToAction("Index");
            }
            else
            {
                // Si algo salió mal, mostrar un mensaje de error (opcional)
                ViewBag.ErrorMessage = "No se pudo eliminar el movimiento.";
                return View("Error");
            }
        }
        // Acción GET para cargar la vista con el formulario en el modal
        public ActionResult Editar(int id)
        {
            var movimiento = db.Movimientos.Find(id);
            if (movimiento == null)
            {
                return HttpNotFound();
            }
            return PartialView("_EditarModal", movimiento);
        }

        // Acción POST para procesar el formulario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Movimiento movimiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movimiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index"); // O la vista que necesites
            }
            return View(movimiento);
        }

    }
}
