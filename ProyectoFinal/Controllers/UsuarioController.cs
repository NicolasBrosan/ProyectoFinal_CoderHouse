using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Controllers.DTOS;
using ProyectoFinal.Model;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet(Name = "GetUser")]
        public Usuario GetUsuarios(string nombreUsuario)
        {
            return UsuarioHandler.GetUsuarios(nombreUsuario);
        }

        [HttpPut(Name = "UpdateUser")]
        public bool ModificarUsuario([FromBody] PutUsuario usuario)
        {
            return UsuarioHandler.UpdateUser(new Usuario
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Contraseña = usuario.Contraseña,
                Mail = usuario.Mail

            });
        }

        [HttpPost(Name = "CreateUser")]
        public bool CrearNuevoUsuario([FromBody] PostUsuario usuario)
        {
            return UsuarioHandler.CreateNewUser(new Usuario
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                NombreUsuario = usuario.NombreUsuario,
                Contraseña = usuario.Contraseña,
                Mail = usuario.Mail,
            });
        }

        [HttpDelete(Name = "DeleteUser")]
        public bool EliminarUsuario([FromBody] int id)
        {
            return UsuarioHandler.DeleteUser(id);
        }
    }
}
