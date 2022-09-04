using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NombreAppController : ControllerBase
    {
        [HttpGet]
        public string MostrarNombreApp()
        {
            return "Mi aplicacion Web";
        }
    }
}
