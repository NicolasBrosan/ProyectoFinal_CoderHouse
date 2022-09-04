using ProyectoFinal.Model;

namespace ProyectoFinal.Controllers.DTOS
{
    public class PostVenta
    {
        public Venta Ventas { get; set; }
        public List<Producto> Productos { get; set; }
    }
}
