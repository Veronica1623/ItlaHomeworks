using System;
using System.Data.SqlClient;

namespace BoutiqueConsoleApp
{
    public class DatabaseConnection
    {
  

        
         private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=BoutiqueDB;Integrated Security=True;";

        
        public SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar: {ex.Message}");
                return null;
            }
        }

        
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    Console.WriteLine("✓ Conexión exitosa a la base de datos");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error de conexión: {ex.Message}");
                return false;
            }
        }
    }
}