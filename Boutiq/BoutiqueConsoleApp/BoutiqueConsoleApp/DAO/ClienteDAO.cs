using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BoutiqueConsoleApp.Models;

namespace BoutiqueConsoleApp.DAO
{
    public class ClienteDAO
    {
        private DatabaseConnection dbConnection;

        public ClienteDAO()
        {
            dbConnection = new DatabaseConnection();
        }

        // CREATE
        public bool Agregar(Cliente cliente)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO Clientes 
                                    (Nombre, Apellido, Telefono, Email, FechaNacimiento) 
                                    VALUES 
                                    (@Nombre, @Apellido, @Telefono, @Email, @FechaNacimiento)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                        cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", cliente.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaNacimiento",
                            cliente.FechaNacimiento.HasValue ? (object)cliente.FechaNacimiento.Value : DBNull.Value);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar cliente: {ex.Message}");
                return false;
            }
        }

        // READ ALL
        public List<Cliente> ObtenerTodos()
        {
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Clientes ORDER BY ClienteID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cliente cliente = new Cliente
                            {
                                ClienteID = (int)reader["ClienteID"],
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Email = reader["Email"].ToString(),
                                FechaNacimiento = reader["FechaNacimiento"] != DBNull.Value ?
                                    (DateTime?)reader["FechaNacimiento"] : null,
                                FechaRegistro = (DateTime)reader["FechaRegistro"]
                            };
                            clientes.Add(cliente);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
            }

            return clientes;
        }

        // READ BY ID
        public Cliente ObtenerPorId(int id)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Clientes WHERE ClienteID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Cliente
                                {
                                    ClienteID = (int)reader["ClienteID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Apellido = reader["Apellido"].ToString(),
                                    Telefono = reader["Telefono"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    FechaNacimiento = reader["FechaNacimiento"] != DBNull.Value ?
                                        (DateTime?)reader["FechaNacimiento"] : null,
                                    FechaRegistro = (DateTime)reader["FechaRegistro"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar cliente: {ex.Message}");
            }

            return null;
        }

        // UPDATE
        public bool Actualizar(Cliente cliente)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Clientes SET 
                                    Nombre = @Nombre, 
                                    Apellido = @Apellido, 
                                    Telefono = @Telefono, 
                                    Email = @Email, 
                                    FechaNacimiento = @FechaNacimiento 
                                    WHERE ClienteID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", cliente.ClienteID);
                        cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                        cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", cliente.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaNacimiento",
                            cliente.FechaNacimiento.HasValue ? (object)cliente.FechaNacimiento.Value : DBNull.Value);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar cliente: {ex.Message}");
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
                    string query = "DELETE FROM Clientes WHERE ClienteID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar cliente: {ex.Message}");
                return false;
            }
        }

        // BUSCAR POR NOMBRE
        public List<Cliente> BuscarPorNombre(string nombre)
        {
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT * FROM Clientes 
                                    WHERE Nombre LIKE @Nombre OR Apellido LIKE @Nombre";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", $"%{nombre}%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Cliente cliente = new Cliente
                                {
                                    ClienteID = (int)reader["ClienteID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Apellido = reader["Apellido"].ToString(),
                                    Telefono = reader["Telefono"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    FechaNacimiento = reader["FechaNacimiento"] != DBNull.Value ?
                                        (DateTime?)reader["FechaNacimiento"] : null,
                                    FechaRegistro = (DateTime)reader["FechaRegistro"]
                                };
                                clientes.Add(cliente);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar clientes: {ex.Message}");
            }

            return clientes;
        }
    }
}