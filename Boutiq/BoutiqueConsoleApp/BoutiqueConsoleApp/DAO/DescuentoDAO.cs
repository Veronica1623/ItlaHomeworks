using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BoutiqueConsoleApp.Models;

namespace BoutiqueConsoleApp.DAO
{
    public class DescuentoDAO
    {
        private DatabaseConnection dbConnection;

        public DescuentoDAO()
        {
            dbConnection = new DatabaseConnection();
        }

        // CREATE
        public bool Agregar(Descuento descuento)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO Descuentos 
                                    (Nombre, Porcentaje, FechaInicio, FechaFin, Activo) 
                                    VALUES 
                                    (@Nombre, @Porcentaje, @FechaInicio, @FechaFin, @Activo)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", descuento.Nombre);
                        cmd.Parameters.AddWithValue("@Porcentaje", descuento.Porcentaje);
                        cmd.Parameters.AddWithValue("@FechaInicio", descuento.FechaInicio);
                        cmd.Parameters.AddWithValue("@FechaFin", descuento.FechaFin);
                        cmd.Parameters.AddWithValue("@Activo", descuento.Activo);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar descuento: {ex.Message}");
                return false;
            }
        }

        // READ ALL
        public List<Descuento> ObtenerTodos()
        {
            List<Descuento> descuentos = new List<Descuento>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Descuentos ORDER BY FechaInicio DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Descuento descuento = new Descuento
                            {
                                DescuentoID = (int)reader["DescuentoID"],
                                Nombre = reader["Nombre"].ToString(),
                                Porcentaje = (decimal)reader["Porcentaje"],
                                FechaInicio = (DateTime)reader["FechaInicio"],
                                FechaFin = (DateTime)reader["FechaFin"],
                                Activo = (bool)reader["Activo"]
                            };
                            descuentos.Add(descuento);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener descuentos: {ex.Message}");
            }

            return descuentos;
        }

        // READ - Obtener descuentos vigentes
        public List<Descuento> ObtenerVigentes()
        {
            List<Descuento> descuentos = new List<Descuento>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT * FROM Descuentos 
                                    WHERE Activo = 1 
                                    AND CAST(GETDATE() AS DATE) BETWEEN FechaInicio AND FechaFin
                                    ORDER BY Porcentaje DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Descuento descuento = new Descuento
                            {
                                DescuentoID = (int)reader["DescuentoID"],
                                Nombre = reader["Nombre"].ToString(),
                                Porcentaje = (decimal)reader["Porcentaje"],
                                FechaInicio = (DateTime)reader["FechaInicio"],
                                FechaFin = (DateTime)reader["FechaFin"],
                                Activo = (bool)reader["Activo"]
                            };
                            descuentos.Add(descuento);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener descuentos vigentes: {ex.Message}");
            }

            return descuentos;
        }

        // READ BY ID
        public Descuento ObtenerPorId(int id)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Descuentos WHERE DescuentoID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Descuento
                                {
                                    DescuentoID = (int)reader["DescuentoID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Porcentaje = (decimal)reader["Porcentaje"],
                                    FechaInicio = (DateTime)reader["FechaInicio"],
                                    FechaFin = (DateTime)reader["FechaFin"],
                                    Activo = (bool)reader["Activo"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar descuento: {ex.Message}");
            }

            return null;
        }

        // UPDATE
        public bool Actualizar(Descuento descuento)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Descuentos SET 
                                    Nombre = @Nombre, 
                                    Porcentaje = @Porcentaje, 
                                    FechaInicio = @FechaInicio, 
                                    FechaFin = @FechaFin, 
                                    Activo = @Activo 
                                    WHERE DescuentoID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", descuento.DescuentoID);
                        cmd.Parameters.AddWithValue("@Nombre", descuento.Nombre);
                        cmd.Parameters.AddWithValue("@Porcentaje", descuento.Porcentaje);
                        cmd.Parameters.AddWithValue("@FechaInicio", descuento.FechaInicio);
                        cmd.Parameters.AddWithValue("@FechaFin", descuento.FechaFin);
                        cmd.Parameters.AddWithValue("@Activo", descuento.Activo);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar descuento: {ex.Message}");
                return false;
            }
        }

        // DELETE
        public bool Eliminar(int id)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM Descuentos WHERE DescuentoID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar descuento: {ex.Message}");
                return false;
            }
        }

        // Activar/Desactivar descuento
        public bool CambiarEstado(int id, bool activo)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Descuentos SET Activo = @Activo WHERE DescuentoID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@Activo", activo);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar estado del descuento: {ex.Message}");
                return false;
            }
        }
    }
}