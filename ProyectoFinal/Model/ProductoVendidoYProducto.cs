namespace ProyectoFinal.Model
{
    public class ProductoVendidoYProducto
    {
        public ProductoVendido ProductoVendido { get; set; }
        public Producto Producto { get; set; }

        public ProductoVendidoYProducto()
        {
            Producto Producto = new Producto();
            this.Producto = Producto;
            ProductoVendido ProductoVendido = new ProductoVendido();
            this.ProductoVendido = ProductoVendido;
        }
    }
}
