using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Model;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoVendidoController : ControllerBase
    {
        [HttpGet]
        [Route("GetProductsSales/{IdUsuario}")]
        public List<ProductoVendidoYProducto> GetProductsSales(int IdUsuario)
        {
            return ProductoVendidoHandler.GetProductoVendidos(IdUsuario);
        }
    }
}
