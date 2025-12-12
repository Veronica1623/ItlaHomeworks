using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BoutiqueConsoleApp.Models;

namespace BoutiqueConsoleApp.DAO
{
    public class ProveedorDAO
    {
        private DatabaseConnection dbConnection;

        public ProveedorDAO()
        {
            dbConnection = new DatabaseConnection();
        }

        // CREATE
        public bool Agregar(Proveedor proveedor)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO Proveedores 
                                    (Nombre, Telefono, Email, Direccion) 
                                    VALUES 
                                    (@Nombre, @Telefono, @Email, @Direccion)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                        cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", proveedor.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Direccion", proveedor.Direccion ?? (object)DBNull.Value);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar proveedor: {ex.Message}");
                return false;
            }
        }

        // READ ALL
        public List<Proveedor> ObtenerTodos()
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Proveedores ORDER BY ProveedorID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Proveedor proveedor = new Proveedor
                            {
                                ProveedorID = (int)reader["ProveedorID"],
                                Nombre = reader["Nombre"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Email = reader["Email"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                FechaRegistro = (DateTime)reader["FechaRegistro"]
                            };
                            proveedores.Add(proveedor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener proveedores: {ex.Message}");
            }

            return proveedores;
        }

        // READ BY ID
        public Proveedor ObtenerPorId(int id)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Proveedores WHERE ProveedorID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Proveedor
                                {
                                    ProveedorID = (int)reader["ProveedorID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Telefono = reader["Telefono"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Direccion = reader["Direccion"].ToString(),
                                    FechaRegistro = (DateTime)reader["FechaRegistro"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar proveedor: {ex.Message}");
            }

            return null;
        }

        // UPDATE
        public bool Actualizar(Proveedor proveedor)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Proveedores SET 
                                    Nombre = @Nombre, 
                                    Telefono = @Telefono, 
                                    Email = @Email, 
                                    Direccion = @Direccion 
                                    WHERE ProveedorID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", proveedor.ProveedorID);
                        cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                        cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", proveedor.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Direccion", proveedor.Direccion ?? (object)DBNull.Value);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar proveedor: {ex.Message}");
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
                    string query = "DELETE FROM Proveedores WHERE ProveedorID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar proveedor: {ex.Message}");
                return false;
            }
        }

        // BUSCAR POR NOMBRE
        public List<Proveedor> BuscarPorNombre(string nombre)
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Proveedores WHERE Nombre LIKE @Nombre";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", $"%{nombre}%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Proveedor proveedor = new Proveedor
                                {
                                    ProveedorID = (int)reader["ProveedorID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Telefono = reader["Telefono"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Direccion = reader["Direccion"].ToString(),
                                    FechaRegistro = (DateTime)reader["FechaRegistro"]
                                };
                                proveedores.Add(proveedor);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar proveedores: {ex.Message}");
            }

            return proveedores;
        }
    }
}