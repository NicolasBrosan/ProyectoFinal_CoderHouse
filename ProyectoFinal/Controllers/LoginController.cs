using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Controllers.DTOS;
using ProyectoFinal.Model;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public Usuario OpenSesion([FromBody] PostLogin login)
        {
            return LoginHandler.Sesion(login.NombreUsuario, login.Contraseña);
        }
    }
}