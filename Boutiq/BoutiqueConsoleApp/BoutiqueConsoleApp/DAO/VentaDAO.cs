using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BoutiqueConsoleApp.Models;

namespace BoutiqueConsoleApp.DAO
{
    public class VentaDAO
    {
        private DatabaseConnection dbConnection;

        public VentaDAO()
        {
            dbConnection = new DatabaseConnection();
        }

        // CREATE - Registrar venta completa con detalles
        public bool RegistrarVenta(Venta venta)
        {
            SqlConnection conn = null;
            SqlTransaction transaction = null;

            try
            {
                conn = dbConnection.GetConnection();
                conn.Open();
                transaction = conn.BeginTransaction();

                // 1. Insertar la venta
                string queryVenta = @"INSERT INTO Ventas 
                                     (ClienteID, Subtotal, Descuento, Total, MetodoPago) 
                                     VALUES 
                                     (@ClienteID, @Subtotal, @Descuento, @Total, @MetodoPago);
                                     SELECT CAST(SCOPE_IDENTITY() AS INT);";

                int ventaID;
                using (SqlCommand cmd = new SqlCommand(queryVenta, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@ClienteID", venta.ClienteID);
                    cmd.Parameters.AddWithValue("@Subtotal", venta.Subtotal);
                    cmd.Parameters.AddWithValue("@Descuento", venta.Descuento);
                    cmd.Parameters.AddWithValue("@Total", venta.Total);
                    cmd.Parameters.AddWithValue("@MetodoPago", venta.MetodoPago ?? (object)DBNull.Value);

                    ventaID = (int)cmd.ExecuteScalar();
                }

                // 2. Insertar detalles de la venta
                string queryDetalle = @"INSERT INTO DetalleVentas 
                                       (VentaID, PrendaID, Cantidad, PrecioUnitario, Subtotal) 
                                       VALUES 
                                       (@VentaID, @PrendaID, @Cantidad, @PrecioUnitario, @Subtotal)";

                foreach (var detalle in venta.Detalles)
                {
                    using (SqlCommand cmd = new SqlCommand(queryDetalle, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@VentaID", ventaID);
                        cmd.Parameters.AddWithValue("@PrendaID", detalle.PrendaID);
                        cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@Subtotal", detalle.Subtotal);

                        cmd.ExecuteNonQuery();
                    }

                    // 3. Actualizar stock de las prendas
                    string queryStock = "UPDATE Prendas SET Stock = Stock - @Cantidad WHERE PrendaID = @PrendaID";
                    using (SqlCommand cmd = new SqlCommand(queryStock, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                        cmd.Parameters.AddWithValue("@PrendaID", detalle.PrendaID);
                        cmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Console.WriteLine($"Error al registrar venta: {ex.Message}");
                return false;
            }
            finally
            {
                conn?.Close();
            }
        }

        // READ ALL - Con información del cliente
        public List<Venta> ObtenerTodas()
        {
            List<Venta> ventas = new List<Venta>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT v.*, 
                                    c.Nombre + ' ' + c.Apellido AS NombreCliente
                                    FROM Ventas v
                                    INNER JOIN Clientes c ON v.ClienteID = c.ClienteID
                                    ORDER BY v.VentaID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Venta venta = new Venta
                            {
                                VentaID = (int)reader["VentaID"],
                                ClienteID = (int)reader["ClienteID"],
                                NombreCliente = reader["NombreCliente"].ToString(),
                                FechaVenta = (DateTime)reader["FechaVenta"],
                                Subtotal = (decimal)reader["Subtotal"],
                                Descuento = (decimal)reader["Descuento"],
                                Total = (decimal)reader["Total"],
                                MetodoPago = reader["MetodoPago"].ToString()
                            };
                            ventas.Add(venta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ventas: {ex.Message}");
            }

            return ventas;
        }

        // READ BY ID - Con detalles
        public Venta ObtenerPorId(int id)
        {
            Venta venta = null;

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    // Obtener venta
                    string queryVenta = @"SELECT v.*, 
                                         c.Nombre + ' ' + c.Apellido AS NombreCliente
                                         FROM Ventas v
                                         INNER JOIN Clientes c ON v.ClienteID = c.ClienteID
                                         WHERE v.VentaID = @ID";

                    using (SqlCommand cmd = new SqlCommand(queryVenta, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                venta = new Venta
                                {
                                    VentaID = (int)reader["VentaID"],
                                    ClienteID = (int)reader["ClienteID"],
                                    NombreCliente = reader["NombreCliente"].ToString(),
                                    FechaVenta = (DateTime)reader["FechaVenta"],
                                    Subtotal = (decimal)reader["Subtotal"],
                                    Descuento = (decimal)reader["Descuento"],
                                    Total = (decimal)reader["Total"],
                                    MetodoPago = reader["MetodoPago"].ToString()
                                };
                            }
                        }
                    }

                    if (venta != null)
                    {
                        // Obtener detalles
                        string queryDetalles = @"SELECT dv.*, p.Nombre AS NombrePrenda
                                                FROM DetalleVentas dv
                                                INNER JOIN Prendas p ON dv.PrendaID = p.PrendaID
                                                WHERE dv.VentaID = @ID";

                        using (SqlCommand cmd = new SqlCommand(queryDetalles, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    DetalleVenta detalle = new DetalleVenta
                                    {
                                        DetalleID = (int)reader["DetalleID"],
                                        VentaID = (int)reader["VentaID"],
                                        PrendaID = (int)reader["PrendaID"],
                                        NombrePrenda = reader["NombrePrenda"].ToString(),
                                        Cantidad = (int)reader["Cantidad"],
                                        PrecioUnitario = (decimal)reader["PrecioUnitario"],
                                        Subtotal = (decimal)reader["Subtotal"]
                                    };
                                    venta.Detalles.Add(detalle);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar venta: {ex.Message}");
            }

            return venta;
        }

        // Obtener ventas por rango de fechas
        public List<Venta> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Venta> ventas = new List<Venta>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT v.*, 
                                    c.Nombre + ' ' + c.Apellido AS NombreCliente
                                    FROM Ventas v
                                    INNER JOIN Clientes c ON v.ClienteID = c.ClienteID
                                    WHERE CAST(v.FechaVenta AS DATE) BETWEEN @FechaInicio AND @FechaFin
                                    ORDER BY v.FechaVenta DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                        cmd.Parameters.AddWithValue("@FechaFin", fechaFin.Date);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Venta venta = new Venta
                                {
                                    VentaID = (int)reader["VentaID"],
                                    ClienteID = (int)reader["ClienteID"],
                                    NombreCliente = reader["NombreCliente"].ToString(),
                                    FechaVenta = (DateTime)reader["FechaVenta"],
                                    Subtotal = (decimal)reader["Subtotal"],
                                    Descuento = (decimal)reader["Descuento"],
                                    Total = (decimal)reader["Total"],
                                    MetodoPago = reader["MetodoPago"].ToString()
                                };
                                ventas.Add(venta);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ventas por fecha: {ex.Message}");
            }

            return ventas;
        }

        // Obtener total de ventas del día
        public decimal ObtenerTotalVentasHoy()
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT ISNULL(SUM(Total), 0) 
                                    FROM Ventas 
                                    WHERE CAST(FechaVenta AS DATE) = CAST(GETDATE() AS DATE)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        return (decimal)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener total de ventas: {ex.Message}");
                return 0;
            }
        }

        // Obtener productos más vendidos
        public List<(string Nombre, int Cantidad)> ObtenerProductosMasVendidos(int top = 5)
        {
            List<(string, int)> productos = new List<(string, int)>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = $@"SELECT TOP {top} p.Nombre, SUM(dv.Cantidad) AS TotalVendido
                                     FROM DetalleVentas dv
                                     INNER JOIN Prendas p ON dv.PrendaID = p.PrendaID
                                     GROUP BY p.Nombre
                                     ORDER BY TotalVendido DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add((
                                reader["Nombre"].ToString(),
                                (int)reader["TotalVendido"]
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos más vendidos: {ex.Message}");
            }

            return productos;
        }
    }
}