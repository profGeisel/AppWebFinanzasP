using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebFinanzasP.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net.Configuration;
using System.Web.Services.Description;
namespace AppWebFinanzasP.Controllers
{
    public class IngresoController : Controller
    {
        //String de conexion
        static string cadena = "Data Source=localhost;initial Catalog=dbFinanzasG;Integrated Security=true";

        // GET: Ingreso
        public ActionResult Login()
        {
            return View();
        }
        // GET: Registro
        public ActionResult Registrar()
        {
            return View();
        }
        // Metodo de Registrar Usuarios
        [HttpPost]
        public ActionResult Registrar(usuario objUsuario)
        {
            bool registrado;
            string mensaje;

            if (objUsuario.Contrasena == objUsuario.ConfirmarContrasena)
            {
                //contraseña encriptada
                objUsuario.Contrasena = ConvertirSha256(objUsuario.Contrasena);
            }
            else
            {
                //ViewData para enviar datos del controler a la vista
                ViewData["Mensaje"] = "Las contraseñas son diferentes";
                return View();
            }
            //Usar la cadena de conexion a la BD
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spInsertarUsuarios",cn);
                cmd.Parameters.AddWithValue("contrasena", objUsuario.Contrasena);
                cmd.Parameters.AddWithValue("nom_usuario", objUsuario.Nom_usuario);
                cmd.Parameters.AddWithValue("ap1_usuario", objUsuario.Ap1Usuario);
                cmd.Parameters.AddWithValue("ap2_usuario", objUsuario.Ap2Usuario);
                cmd.Parameters.AddWithValue("correoElectronico", objUsuario.CorreoElectronico);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                //Ejecutar procedimiento almacenado
                cmd.ExecuteNonQuery();
                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }
            ViewData["Mensaje"] = mensaje;
            if (registrado)
            {
                return RedirectToAction("Login", "Ingreso");
            }
            else
            {
                return View();
            }
        }

        // Metodo de Ingreso de Usuarios
        [HttpPost]
        public ActionResult Login(usuario objUsuario)
        {
            objUsuario.Contrasena = ConvertirSha256(objUsuario.Contrasena);
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spValidarUsuarios",cn);
                //cmd.Parameters.AddWithValue("nom_usuario", objUsuario.Nom_Usuario);
                //cmd.Parameters.AddWithValue("ap1_usuario", objUsuario.Ap1Usuario);
                //cmd.Parameters.AddWithValue("ap2_usuario", objUsuario.Ap2Usuario);
                cmd.Parameters.AddWithValue("correoElectronico", objUsuario.CorreoElectronico);
                cmd.Parameters.AddWithValue("contrasena", objUsuario.Contrasena);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                
                objUsuario.Id_usuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }
            if (objUsuario.Id_usuario != 0)
            {
                //crear sesion de usuario
                Session["usuario"] = objUsuario;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "No se ha encontrado el usuario";
                return View();
            }

        }

        public static string ConvertirSha256(string texto)
        {
            //Usar una referencia using System.Text"
            StringBuilder Sb = new StringBuilder();
            //Usar una referencia a System.Security.Cryptografhy"
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));
                
                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }
    }
}