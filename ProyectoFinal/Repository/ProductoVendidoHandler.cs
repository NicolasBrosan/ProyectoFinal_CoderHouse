using ProyectoFinal.Model;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoFinal.Repository
{
    public static class ProductoVendidoHandler
    {
        public const string connectionString = "Server=LAPTOP-GHLLNC5Q\\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True";
        public static List<ProductoVendidoYProducto> GetProductoVendidos (int IdUsuario)
        {
            List<ProductoVendidoYProducto> listaProductosVendidoYProductos = new List<ProductoVendidoYProducto>();
            var ListaProductos = ProductoHandler.GetProduct();

            foreach (var products in ListaProductos)
            {
                if(products.IdUsuario == IdUsuario)
                {
                    var queryProductoVendido = "SELECT * FROM ProductoVendido WHERE IdProducto = @IdProducto";
                    var sqlParameter = new SqlParameter("IdProducto", SqlDbType.BigInt) { Value = products.Id};

                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(queryProductoVendido, sqlConnection))
                        {
                            sqlConnection.Open();
                            sqlCommand.Parameters.Add(sqlParameter);

                            using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                            {
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        ProductoVendidoYProducto productoVendidoYProductos = new ProductoVendidoYProducto();
                                        productoVendidoYProductos.Producto = products;
                                        productoVendidoYProductos.ProductoVendido.Id = Convert.ToInt32(dataReader["Id"]);
                                        productoVendidoYProductos.ProductoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                        productoVendidoYProductos.ProductoVendido.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                        productoVendidoYProductos.ProductoVendido.IdVenta = Convert.ToInt32(dataReader["IdVenta"]);
                                        listaProductosVendidoYProductos.Add(productoVendidoYProductos);
                                        
                                    }
                                }
                                sqlConnection.Close();
                            }
                        }
                    }
                }
            }

                
            return listaProductosVendidoYProductos;
        }
            
    }
}

