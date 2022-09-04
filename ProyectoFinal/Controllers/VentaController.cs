using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Controllers.DTOS;
using ProyectoFinal.Model;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VentaController : ControllerBase
    {
        [HttpGet]
        [Route("GetVenas/{IdUsuario}")]
        public List<Venta> Ventas(int IdUsuario)
        {
            return VentaHandler.GetVentas(IdUsuario);
        }

        [HttpPost(Name = "CreateVentas")]
        public bool NuevaVenta([FromBody] PostVenta venta)
        {
            return VentaHandler.CreateNewSale(venta.Productos, venta.Ventas);
        }

        [HttpDelete]
        public bool EliminarVenta(int id)
        {
            return VentaHandler.DeleteVenta(id);
        }
    }
}
