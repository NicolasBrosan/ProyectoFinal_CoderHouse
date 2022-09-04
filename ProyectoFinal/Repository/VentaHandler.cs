using ProyectoFinal.Model;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoFinal.Repository
{
    public class VentaHandler
    {
        public const string connectionString = "Server=LAPTOP-GHLLNC5Q\\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True";
        public static bool CreateNewSale(List<Producto> producto, Venta venta)
        {
            bool result = false;
            var IdVentas = 0;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string queryVenta = "INSERT INTO Venta (Comentarios, IdUsuario) VALUES (@comentarios, @IdUsuario)";
                

                SqlParameter comentarios = new SqlParameter("Comentarios", System.Data.SqlDbType.VarChar) { Value = venta.Comentarios };
                SqlParameter IdUsuario = new SqlParameter("IdUsuario", System.Data.SqlDbType.Int) { Value = venta.IdUsuario };

                sqlConnection.Open();
                
                using (SqlCommand sqlCommand = new SqlCommand(queryVenta, sqlConnection))
                {
                    sqlCommand.Parameters.Add(comentarios);
                    sqlCommand.Parameters.Add(IdUsuario);

                    int numberOfRows = sqlCommand.ExecuteNonQuery();
                    if (numberOfRows > 0)
                    {
                        result = true;
                    }
                }
                if(result == true)
                {
                    var queryIdVenta = "SELECT TOP (1) [Id] FROM [Venta] ORDER BY Id Desc ";
                    using (SqlCommand sqlCommand = new SqlCommand(queryIdVenta, sqlConnection))
                    {
                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    IdVentas = Convert.ToInt32(dataReader["Id"]);
                                }
                            }
                        }
                    }

                    foreach (var prod in producto)
                    {
                        string queryProductoVendido = "INSERT INTO ProductoVendido (Stock, IdProducto,IdVenta ) VALUES (@Stock, @IdProducto, @IdVenta)";

                        SqlParameter idProducto = new SqlParameter("IdProducto", System.Data.SqlDbType.Int) { Value = prod.Id };
                        SqlParameter stock = new SqlParameter("Stock", System.Data.SqlDbType.Int) { Value = prod.Stock };
                        SqlParameter idVenta = new SqlParameter("IdVenta", System.Data.SqlDbType.Int) { Value = IdVentas };

                        using (SqlCommand sqlCommand = new SqlCommand(queryProductoVendido, sqlConnection))
                        {
                            sqlCommand.Parameters.Add(idProducto);
                            sqlCommand.Parameters.Add(idVenta);
                            sqlCommand.Parameters.Add(stock);

                            int numberOfRows = sqlCommand.ExecuteNonQuery();
                            if (numberOfRows > 0)
                            {
                                result = true;
                            }
                        }

                        string queryModificarStock = "UPDATE Producto SET Stock = (Stock - @Stock) WHERE Id = @Id";

                        SqlParameter id = new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = prod.Id };
                        SqlParameter stockB = new SqlParameter("Stock", System.Data.SqlDbType.Int) { Value = prod.Stock };


                        using (SqlCommand sqlCommand = new SqlCommand(queryModificarStock, sqlConnection))
                        {
                            sqlCommand.Parameters.Add(id);
                            sqlCommand.Parameters.Add(stockB);

                            int numberOfRows = sqlCommand.ExecuteNonQuery();
                            if (numberOfRows > 0)
                            {
                                result = true;
                            }
                        }

                    }
                }
                             

                sqlConnection.Close();
            }
            return result;
        }

        public static List<Venta> GetVentas(int IdUsuario)
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                var queryVenta = "SELECT V.Id, V.Comentarios FROM Venta AS V " +
                "INNER JOIN ProductoVendido AS PV ON PV.IdVenta = V.Id " +
                "INNER JOIN Producto AS P ON PV.IdProducto = P.Id " +
                "WHERE P.IdUsuario = @IdUsuario";

                var sqlParameter = new SqlParameter("IdUsuario", SqlDbType.Int) { Value = IdUsuario };

                using (SqlCommand sqlCommand = new SqlCommand(queryVenta, sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.Parameters.Add(sqlParameter);

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Venta venta = new Venta();
                                venta.Id = Convert.ToInt32(dataReader["Id"]);
                                venta.Comentarios = dataReader["Comentarios"].ToString();

                                ventas.Add(venta);
                            }
                        }
                    }
                }
                sqlConnection.Close();
            }
            return ventas;
        }

        public static bool DeleteVenta(int id)
        {
            bool result = false;

            using(SqlConnection sqlConnection = new SqlConnection(connectionString))
            {                
                List<ProductoVendido> listaDeProductoVendido = new List<ProductoVendido>();
                string queryDeleteProductoVendido = "SELECT * FROM ProductoVendido WHERE IdVenta = @IdVenta";
                var sqlParameter = new SqlParameter("IdVenta", SqlDbType.BigInt) { Value = id};

                sqlConnection.Open();
                using(SqlCommand sqlCommand = new SqlCommand(queryDeleteProductoVendido, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParameter);
                    using(SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();
                                productoVendido.Id = Convert.ToInt32(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt32(dataReader["IdVenta"]);

                                listaDeProductoVendido.Add(productoVendido);
                            }
                        }
                    }
                }

                foreach(var ventaProducto in listaDeProductoVendido)
                {
                    var queryProductos = "UPDATE Producto SET Stock = (Stock + @Stock) WHERE Id = @Id";
                    SqlParameter stockAgregado = new SqlParameter("Stock", System.Data.SqlDbType.Int) { Value = ventaProducto.Stock};
                    SqlParameter idDetalleProducto = new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = ventaProducto.IdProducto};
                    SqlParameter[] parameterProductoVendido = new SqlParameter[] {stockAgregado, idDetalleProducto};
                    
                    using(SqlCommand sqlCommand = new SqlCommand(queryProductos, sqlConnection))
                    {
                        sqlCommand.Parameters.AddRange(parameterProductoVendido);
                        int numberOfRows = sqlCommand.ExecuteNonQuery();
                        if (numberOfRows > 0)
                        {
                            result = true;
                        }
                    }

                }
                var queryDeletePV = "DELETE FROM ProductoVendido WHERE IdVenta = @IdVenta";
                var sqlParameterPV = new SqlParameter("IdVenta", SqlDbType.Int) { Value = id };

                using(SqlCommand sqlCommand = new SqlCommand(queryDeletePV, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParameterPV);
                    int numberOfRows = sqlCommand.ExecuteNonQuery();
                    if (numberOfRows > 0)
                    {
                        result = true;
                    }

                }

                var queryDeleteVenta = "DELETE FROM Venta WHERE Id = @Id";
                var sqlParamenterV = new SqlParameter("Id", SqlDbType.Int) { Value = id };
                using( SqlCommand sqlCommand = new SqlCommand(queryDeleteVenta, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParamenterV);
                    int numberOfRows = sqlCommand.ExecuteNonQuery();
                    if(numberOfRows > 0)
                    {
                        result = true;
                    }
                }

                sqlConnection.Close();
            }

            return result;
        }
    }
}
