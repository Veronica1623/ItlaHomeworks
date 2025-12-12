using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BoutiqueConsoleApp.Models;

namespace BoutiqueConsoleApp.DAO
{
    public class PrendaDAO
    {
        private DatabaseConnection dbConnection;

        public PrendaDAO()
        {
            dbConnection = new DatabaseConnection();
        }

        // CREATE - Agregar nueva prenda
        public bool Agregar(Prenda prenda)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO Prendas 
                                    (Nombre, Categoria, Talla, Color, PrecioCompra, 
                                     PrecioVenta, Stock, ProveedorID, Temporada) 
                                    VALUES 
                                    (@Nombre, @Categoria, @Talla, @Color, @PrecioCompra, 
                                     @PrecioVenta, @Stock, @ProveedorID, @Temporada)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", prenda.Nombre);
                        cmd.Parameters.AddWithValue("@Categoria", prenda.Categoria);
                        cmd.Parameters.AddWithValue("@Talla", prenda.Talla);
                        cmd.Parameters.AddWithValue("@Color", prenda.Color);
                        cmd.Parameters.AddWithValue("@PrecioCompra", prenda.PrecioCompra);
                        cmd.Parameters.AddWithValue("@PrecioVenta", prenda.PrecioVenta);
                        cmd.Parameters.AddWithValue("@Stock", prenda.Stock);
                        cmd.Parameters.AddWithValue("@ProveedorID",
                            prenda.ProveedorID.HasValue ? (object)prenda.ProveedorID.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Temporada", prenda.Temporada ?? (object)DBNull.Value);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar prenda: {ex.Message}");
                return false;
            }
        }

        // READ - Listar todas las prendas
        public List<Prenda> ObtenerTodas()
        {
            List<Prenda> prendas = new List<Prenda>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Prendas ORDER BY PrendaID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Prenda prenda = new Prenda
                            {
                                PrendaID = (int)reader["PrendaID"],
                                Nombre = reader["Nombre"].ToString(),
                                Categoria = reader["Categoria"].ToString(),
                                Talla = reader["Talla"].ToString(),
                                Color = reader["Color"].ToString(),
                                PrecioCompra = (decimal)reader["PrecioCompra"],
                                PrecioVenta = (decimal)reader["PrecioVenta"],
                                Stock = (int)reader["Stock"],
                                ProveedorID = reader["ProveedorID"] != DBNull.Value ?
                                    (int?)reader["ProveedorID"] : null,
                                Temporada = reader["Temporada"].ToString(),
                                FechaIngreso = (DateTime)reader["FechaIngreso"]
                            };
                            prendas.Add(prenda);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener prendas: {ex.Message}");
            }

            return prendas;
        }

        // READ - Buscar por ID
        public Prenda ObtenerPorId(int id)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Prendas WHERE PrendaID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Prenda
                                {
                                    PrendaID = (int)reader["PrendaID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Categoria = reader["Categoria"].ToString(),
                                    Talla = reader["Talla"].ToString(),
                                    Color = reader["Color"].ToString(),
                                    PrecioCompra = (decimal)reader["PrecioCompra"],
                                    PrecioVenta = (decimal)reader["PrecioVenta"],
                                    Stock = (int)reader["Stock"],
                                    ProveedorID = reader["ProveedorID"] != DBNull.Value ?
                                        (int?)reader["ProveedorID"] : null,
                                    Temporada = reader["Temporada"].ToString(),
                                    FechaIngreso = (DateTime)reader["FechaIngreso"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar prenda: {ex.Message}");
            }

            return null;
        }

        // UPDATE - Actualizar prenda
        public bool Actualizar(Prenda prenda)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Prendas SET 
                                    Nombre = @Nombre, 
                                    Categoria = @Categoria, 
                                    Talla = @Talla, 
                                    Color = @Color, 
                                    PrecioCompra = @PrecioCompra, 
                                    PrecioVenta = @PrecioVenta, 
                                    Stock = @Stock, 
                                    ProveedorID = @ProveedorID, 
                                    Temporada = @Temporada 
                                    WHERE PrendaID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", prenda.PrendaID);
                        cmd.Parameters.AddWithValue("@Nombre", prenda.Nombre);
                        cmd.Parameters.AddWithValue("@Categoria", prenda.Categoria);
                        cmd.Parameters.AddWithValue("@Talla", prenda.Talla);
                        cmd.Parameters.AddWithValue("@Color", prenda.Color);
                        cmd.Parameters.AddWithValue("@PrecioCompra", prenda.PrecioCompra);
                        cmd.Parameters.AddWithValue("@PrecioVenta", prenda.PrecioVenta);
                        cmd.Parameters.AddWithValue("@Stock", prenda.Stock);
                        cmd.Parameters.AddWithValue("@ProveedorID",
                            prenda.ProveedorID.HasValue ? (object)prenda.ProveedorID.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Temporada", prenda.Temporada ?? (object)DBNull.Value);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar prenda: {ex.Message}");
                return false;
            }
        }

        // DELETE - Eliminar prenda
        public bool Eliminar(int id)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM Prendas WHERE PrendaID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar prenda: {ex.Message}");
                return false;
            }
        }

        // Método adicional: Buscar por nombre
        public List<Prenda> BuscarPorNombre(string nombre)
        {
            List<Prenda> prendas = new List<Prenda>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Prendas WHERE Nombre LIKE @Nombre";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", $"%{nombre}%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Prenda prenda = new Prenda
                                {
                                    PrendaID = (int)reader["PrendaID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Categoria = reader["Categoria"].ToString(),
                                    Talla = reader["Talla"].ToString(),
                                    Color = reader["Color"].ToString(),
                                    PrecioCompra = (decimal)reader["PrecioCompra"],
                                    PrecioVenta = (decimal)reader["PrecioVenta"],
                                    Stock = (int)reader["Stock"],
                                    ProveedorID = reader["ProveedorID"] != DBNull.Value ?
                                        (int?)reader["ProveedorID"] : null,
                                    Temporada = reader["Temporada"].ToString(),
                                    FechaIngreso = (DateTime)reader["FechaIngreso"]
                                };
                                prendas.Add(prenda);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar prendas: {ex.Message}");
            }

            return prendas;
        }
    }
}