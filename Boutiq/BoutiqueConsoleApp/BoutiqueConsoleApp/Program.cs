using System;
using System.Collections.Generic;
using System.Linq;
using BoutiqueConsoleApp.Models;
using BoutiqueConsoleApp.DAO;

namespace BoutiqueConsoleApp
{
    class Program
    {
        static PrendaDAO prendaDAO = new PrendaDAO();
        static ClienteDAO clienteDAO = new ClienteDAO();
        static ProveedorDAO proveedorDAO = new ProveedorDAO();
        static VentaDAO ventaDAO = new VentaDAO();
        static DescuentoDAO descuentoDAO = new DescuentoDAO();
        static DatabaseConnection dbConn = new DatabaseConnection();

        static void Main(string[] args)
        {
            Console.Title = "Sistema de Boutique - Gestión Completa";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            MostrarBienvenida();

            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                MostrarMenuPrincipal();

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MenuPrendas();
                        break;
                    case "2":
                        MenuClientes();
                        break;
                    case "3":
                        MenuProveedores();
                        break;
                    case "4":
                        MenuVentas();
                        break;
                    case "5":
                        MenuDescuentos();
                        break;
                    case "6":
                        MenuReportes();
                        break;
                    case "0":
                        salir = true;
                        Console.WriteLine("\n¡Gracias por usar el sistema de Boutique!");
                        break;
                    default:
                        Console.WriteLine("\n✗ Opción no válida");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        static void MostrarBienvenida()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                ║");
            Console.WriteLine("║   SISTEMA DE BOUTIQUE DE ROPA FEMENINA        ║");
            Console.WriteLine("║   Gestión Completa de Inventario y Ventas     ║");
            Console.WriteLine("║                                                ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝\n");

            Console.WriteLine("Probando conexión a base de datos...");
            if (dbConn.TestConnection())
            {
                Console.WriteLine("✓ Sistema listo para usar");
            }
            else
            {
                Console.WriteLine("✗ Error de conexión. Verifica la configuración.");
            }

            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static void MostrarMenuPrincipal()
        {
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║           MENÚ PRINCIPAL                       ║");
            Console.WriteLine("╠════════════════════════════════════════════════╣");
            Console.WriteLine("║ 1. Gestión de Prendas                         ║");
            Console.WriteLine("║ 2. Gestión de Clientes                        ║");
            Console.WriteLine("║ 3. Gestión de Proveedores                     ║");
            Console.WriteLine("║ 4. Realizar Venta                             ║");
            Console.WriteLine("║ 5. Gestión de Descuentos                      ║");
            Console.WriteLine("║ 6. Reportes y Estadísticas                    ║");
            Console.WriteLine("║ 0. Salir                                      ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
            Console.Write("\nSelecciona una opción: ");
        }

        #region MENÚ PRENDAS
        static void MenuPrendas()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║     GESTIÓN DE PRENDAS                ║");
                Console.WriteLine("╠════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Agregar nueva prenda               ║");
                Console.WriteLine("║ 2. Listar todas las prendas           ║");
                Console.WriteLine("║ 3. Buscar prenda por ID               ║");
                Console.WriteLine("║ 4. Actualizar prenda                  ║");
                Console.WriteLine("║ 5. Eliminar prenda                    ║");
                Console.WriteLine("║ 6. Buscar por nombre                  ║");
                Console.WriteLine("║ 0. Volver al menú principal           ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.Write("\nOpción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": AgregarPrenda(); break;
                    case "2": ListarPrendas(); break;
                    case "3": BuscarPrendaPorId(); break;
                    case "4": ActualizarPrenda(); break;
                    case "5": EliminarPrenda(); break;
                    case "6": BuscarPrendaPorNombre(); break;
                    case "0": volver = true; break;
                    default: Console.WriteLine("\n✗ Opción no válida"); break;
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresiona cualquier tecla...");
                    Console.ReadKey();
                }
            }
        }

        static void AgregarPrenda()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR NUEVA PRENDA ===\n");

            try
            {
                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();

                Console.Write("Categoría (Vestido/Blusa/Pantalón/Falda/Accesorios): ");
                string categoria = Console.ReadLine();

                Console.Write("Talla (XS/S/M/L/XL): ");
                string talla = Console.ReadLine();

                Console.Write("Color: ");
                string color = Console.ReadLine();

                Console.Write("Precio de Compra: $");
                decimal precioCompra = decimal.Parse(Console.ReadLine());

                Console.Write("Precio de Venta: $");
                decimal precioVenta = decimal.Parse(Console.ReadLine());

                Console.Write("Stock (cantidad): ");
                int stock = int.Parse(Console.ReadLine());

                // Mostrar proveedores disponibles
                var proveedores = proveedorDAO.ObtenerTodos();
                if (proveedores.Count > 0)
                {
                    Console.WriteLine("\n--- Proveedores disponibles ---");
                    foreach (var p in proveedores)
                    {
                        Console.WriteLine($"{p.ProveedorID}. {p.Nombre}");
                    }
                }

                Console.Write("ID del Proveedor (Enter para omitir): ");
                string proveedorInput = Console.ReadLine();
                int? proveedorID = string.IsNullOrEmpty(proveedorInput) ? null : (int?)int.Parse(proveedorInput);

                Console.Write("Temporada (Primavera/Verano/Otoño/Invierno): ");
                string temporada = Console.ReadLine();

                Prenda nuevaPrenda = new Prenda(
                    nombre, categoria, talla, color,
                    precioCompra, precioVenta, stock,
                    proveedorID, temporada
                );

                if (prendaDAO.Agregar(nuevaPrenda))
                {
                    Console.WriteLine("\n✓ Prenda agregada exitosamente");
                }
                else
                {
                    Console.WriteLine("\n✗ Error al agregar la prenda");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void ListarPrendas()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE PRENDAS ===\n");

            List<Prenda> prendas = prendaDAO.ObtenerTodas();

            if (prendas.Count == 0)
            {
                Console.WriteLine("No hay prendas registradas.");
                return;
            }

            Console.WriteLine($"Total de prendas: {prendas.Count}\n");
            Console.WriteLine(new string('-', 120));

            foreach (Prenda prenda in prendas)
            {
                Console.WriteLine(prenda.ToString());
                Console.WriteLine(new string('-', 120));
            }
        }

        static void BuscarPrendaPorId()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR PRENDA POR ID ===\n");

            try
            {
                Console.Write("Ingresa el ID de la prenda: ");
                int id = int.Parse(Console.ReadLine());

                Prenda prenda = prendaDAO.ObtenerPorId(id);

                if (prenda != null)
                {
                    Console.WriteLine("\n✓ Prenda encontrada:\n");
                    MostrarDetallePrenda(prenda);
                }
                else
                {
                    Console.WriteLine("\n✗ No se encontró ninguna prenda con ese ID");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void ActualizarPrenda()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR PRENDA ===\n");

            try
            {
                Console.Write("Ingresa el ID de la prenda a actualizar: ");
                int id = int.Parse(Console.ReadLine());

                Prenda prenda = prendaDAO.ObtenerPorId(id);

                if (prenda == null)
                {
                    Console.WriteLine("\n✗ No se encontró ninguna prenda con ese ID");
                    return;
                }

                Console.WriteLine($"\nPrenda actual: {prenda.Nombre}");
                Console.WriteLine("\nIngresa los nuevos datos (Enter para mantener el valor actual):\n");

                Console.Write($"Nombre [{prenda.Nombre}]: ");
                string nombre = Console.ReadLine();
                if (!string.IsNullOrEmpty(nombre)) prenda.Nombre = nombre;

                Console.Write($"Categoría [{prenda.Categoria}]: ");
                string categoria = Console.ReadLine();
                if (!string.IsNullOrEmpty(categoria)) prenda.Categoria = categoria;

                Console.Write($"Talla [{prenda.Talla}]: ");
                string talla = Console.ReadLine();
                if (!string.IsNullOrEmpty(talla)) prenda.Talla = talla;

                Console.Write($"Color [{prenda.Color}]: ");
                string color = Console.ReadLine();
                if (!string.IsNullOrEmpty(color)) prenda.Color = color;

                Console.Write($"Precio Compra [${prenda.PrecioCompra:F2}]: $");
                string precioCompraStr = Console.ReadLine();
                if (!string.IsNullOrEmpty(precioCompraStr))
                    prenda.PrecioCompra = decimal.Parse(precioCompraStr);

                Console.Write($"Precio Venta [${prenda.PrecioVenta:F2}]: $");
                string precioVentaStr = Console.ReadLine();
                if (!string.IsNullOrEmpty(precioVentaStr))
                    prenda.PrecioVenta = decimal.Parse(precioVentaStr);

                Console.Write($"Stock [{prenda.Stock}]: ");
                string stockStr = Console.ReadLine();
                if (!string.IsNullOrEmpty(stockStr))
                    prenda.Stock = int.Parse(stockStr);

                Console.Write($"Temporada [{prenda.Temporada}]: ");
                string temporada = Console.ReadLine();
                if (!string.IsNullOrEmpty(temporada)) prenda.Temporada = temporada;

                if (prendaDAO.Actualizar(prenda))
                {
                    Console.WriteLine("\n✓ Prenda actualizada exitosamente");
                }
                else
                {
                    Console.WriteLine("\n✗ Error al actualizar la prenda");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void EliminarPrenda()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PRENDA ===\n");

            try
            {
                Console.Write("Ingresa el ID de la prenda a eliminar: ");
                int id = int.Parse(Console.ReadLine());

                Prenda prenda = prendaDAO.ObtenerPorId(id);

                if (prenda == null)
                {
                    Console.WriteLine("\n✗ No se encontró ninguna prenda con ese ID");
                    return;
                }

                Console.WriteLine($"\nPrenda: {prenda.Nombre}");
                Console.Write("\n¿Estás seguro de eliminar esta prenda? (S/N): ");
                string confirmacion = Console.ReadLine().ToUpper();

                if (confirmacion == "S")
                {
                    if (prendaDAO.Eliminar(id))
                    {
                        Console.WriteLine("\n✓ Prenda eliminada exitosamente");
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Error al eliminar la prenda");
                    }
                }
                else
                {
                    Console.WriteLine("\nEliminación cancelada");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void BuscarPrendaPorNombre()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR POR NOMBRE ===\n");

            Console.Write("Ingresa el nombre o parte del nombre: ");
            string nombre = Console.ReadLine();

            List<Prenda> prendas = prendaDAO.BuscarPorNombre(nombre);

            if (prendas.Count == 0)
            {
                Console.WriteLine("\n✗ No se encontraron prendas con ese nombre");
                return;
            }

            Console.WriteLine($"\n✓ Se encontraron {prendas.Count} prenda(s):\n");
            Console.WriteLine(new string('-', 120));

            foreach (Prenda prenda in prendas)
            {
                Console.WriteLine(prenda.ToString());
                Console.WriteLine(new string('-', 120));
            }
        }

        static void MostrarDetallePrenda(Prenda prenda)
        {
            Console.WriteLine($"ID: {prenda.PrendaID}");
            Console.WriteLine($"Nombre: {prenda.Nombre}");
            Console.WriteLine($"Categoría: {prenda.Categoria}");
            Console.WriteLine($"Talla: {prenda.Talla}");
            Console.WriteLine($"Color: {prenda.Color}");
            Console.WriteLine($"Precio Compra: ${prenda.PrecioCompra:F2}");
            Console.WriteLine($"Precio Venta: ${prenda.PrecioVenta:F2}");
            Console.WriteLine($"Stock: {prenda.Stock}");
            Console.WriteLine($"Temporada: {prenda.Temporada}");
            Console.WriteLine($"Fecha Ingreso: {prenda.FechaIngreso:dd/MM/yyyy}");
        }
        #endregion

        #region MENÚ CLIENTES
        static void MenuClientes()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║     GESTIÓN DE CLIENTES               ║");
                Console.WriteLine("╠════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Agregar nuevo cliente              ║");
                Console.WriteLine("║ 2. Listar todos los clientes          ║");
                Console.WriteLine("║ 3. Buscar cliente por ID              ║");
                Console.WriteLine("║ 4. Actualizar cliente                 ║");
                Console.WriteLine("║ 5. Eliminar cliente                   ║");
                Console.WriteLine("║ 6. Buscar por nombre                  ║");
                Console.WriteLine("║ 0. Volver                             ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.Write("\nOpción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": AgregarCliente(); break;
                    case "2": ListarClientes(); break;
                    case "3": BuscarClientePorId(); break;
                    case "4": ActualizarCliente(); break;
                    case "5": EliminarCliente(); break;
                    case "6": BuscarClientePorNombre(); break;
                    case "0": volver = true; break;
                    default: Console.WriteLine("\n✗ Opción no válida"); break;
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresiona cualquier tecla...");
                    Console.ReadKey();
                }
            }
        }

        static void AgregarCliente()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR NUEVO CLIENTE ===\n");

            try
            {
                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();

                Console.Write("Apellido: ");
                string apellido = Console.ReadLine();

                Console.Write("Teléfono: ");
                string telefono = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Fecha de Nacimiento (dd/mm/yyyy) [Enter para omitir]: ");
                string fechaStr = Console.ReadLine();
                DateTime? fechaNacimiento = null;
                if (!string.IsNullOrEmpty(fechaStr))
                {
                    fechaNacimiento = DateTime.Parse(fechaStr);
                }

                Cliente nuevoCliente = new Cliente(nombre, apellido, telefono, email, fechaNacimiento);

                if (clienteDAO.Agregar(nuevoCliente))
                {
                    Console.WriteLine("\n✓ Cliente agregado exitosamente");
                }
                else
                {
                    Console.WriteLine("\n✗ Error al agregar el cliente");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void ListarClientes()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE CLIENTES ===\n");

            List<Cliente> clientes = clienteDAO.ObtenerTodos();

            if (clientes.Count == 0)
            {
                Console.WriteLine("No hay clientes registrados.");
                return;
            }

            Console.WriteLine($"Total de clientes: {clientes.Count}\n");
            Console.WriteLine(new string('-', 100));

            foreach (Cliente cliente in clientes)
            {
                Console.WriteLine(cliente.ToString());
                if (cliente.CalcularEdad().HasValue)
                {
                    Console.WriteLine($"   Edad: {cliente.CalcularEdad()} años");
                }
                Console.WriteLine(new string('-', 100));
            }
        }

        static void BuscarClientePorId()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR CLIENTE POR ID ===\n");

            try
            {
                Console.Write("Ingresa el ID del cliente: ");
                int id = int.Parse(Console.ReadLine());

                Cliente cliente = clienteDAO.ObtenerPorId(id);

                if (cliente != null)
                {
                    Console.WriteLine("\n✓ Cliente encontrado:\n");
                    Console.WriteLine($"ID: {cliente.ClienteID}");
                    Console.WriteLine($"Nombre: {cliente.NombreCompleto}");
                    Console.WriteLine($"Teléfono: {cliente.Telefono}");
                    Console.WriteLine($"Email: {cliente.Email}");
                    if (cliente.FechaNacimiento.HasValue)
                    {
                        Console.WriteLine($"Fecha Nacimiento: {cliente.FechaNacimiento:dd/MM/yyyy}");
                        Console.WriteLine($"Edad: {cliente.CalcularEdad()} años");
                    }
                    Console.WriteLine($"Fecha Registro: {cliente.FechaRegistro:dd/MM/yyyy}");
                }
                else
                {
                    Console.WriteLine("\n✗ No se encontró ningún cliente con ese ID");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void ActualizarCliente()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR CLIENTE ===\n");

            try
            {
                Console.Write("Ingresa el ID del cliente a actualizar: ");
                int id = int.Parse(Console.ReadLine());

                Cliente cliente = clienteDAO.ObtenerPorId(id);

                if (cliente == null)
                {
                    Console.WriteLine("\n✗ No se encontró ningún cliente con ese ID");
                    return;
                }

                Console.WriteLine($"\nCliente actual: {cliente.NombreCompleto}");
                Console.WriteLine("\nIngresa los nuevos datos (Enter para mantener):\n");

                Console.Write($"Nombre [{cliente.Nombre}]: ");
                string nombre = Console.ReadLine();
                if (!string.IsNullOrEmpty(nombre)) cliente.Nombre = nombre;

                Console.Write($"Apellido [{cliente.Apellido}]: ");
                string apellido = Console.ReadLine();
                if (!string.IsNullOrEmpty(apellido)) cliente.Apellido = apellido;

                Console.Write($"Teléfono [{cliente.Telefono}]: ");
                string telefono = Console.ReadLine();
                if (!string.IsNullOrEmpty(telefono)) cliente.Telefono = telefono;

                Console.Write($"Email [{cliente.Email}]: ");
                string email = Console.ReadLine();
                if (!string.IsNullOrEmpty(email)) cliente.Email = email;

                if (clienteDAO.Actualizar(cliente))
                {
                    Console.WriteLine("\n✓ Cliente actualizado exitosamente");
                }
                else
                {
                    Console.WriteLine("\n✗ Error al actualizar el cliente");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void EliminarCliente()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR CLIENTE ===\n");

            try
            {
                Console.Write("Ingresa el ID del cliente a eliminar: ");
                int id = int.Parse(Console.ReadLine());

                Cliente cliente = clienteDAO.ObtenerPorId(id);

                if (cliente == null)
                {
                    Console.WriteLine("\n✗ No se encontró ningún cliente con ese ID");
                    return;
                }

                Console.WriteLine($"\nCliente: {cliente.NombreCompleto}");
                Console.Write("\n¿Estás seguro de eliminar este cliente? (S/N): ");
                string confirmacion = Console.ReadLine().ToUpper();

                if (confirmacion == "S")
                {
                    if (clienteDAO.Eliminar(id))
                    {
                        Console.WriteLine("\n✓ Cliente eliminado exitosamente");
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Error al eliminar el cliente");
                    }
                }
                else
                {
                    Console.WriteLine("\nEliminación cancelada");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void BuscarClientePorNombre()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR CLIENTE POR NOMBRE ===\n");

            Console.Write("Ingresa el nombre o apellido: ");
            string nombre = Console.ReadLine();

            List<Cliente> clientes = clienteDAO.BuscarPorNombre(nombre);

            if (clientes.Count == 0)
            {
                Console.WriteLine("\n✗ No se encontraron clientes");
                return;
            }

            Console.WriteLine($"\n✓ Se encontraron {clientes.Count} cliente(s):\n");
            Console.WriteLine(new string('-', 100));

            foreach (Cliente cliente in clientes)
            {
                Console.WriteLine(cliente.ToString());
                Console.WriteLine(new string('-', 100));
            }
        }
        #endregion

        #region MENÚ PROVEEDORES
        static void MenuProveedores()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║     GESTIÓN DE PROVEEDORES            ║");
                Console.WriteLine("╠════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Agregar nuevo proveedor            ║");
                Console.WriteLine("║ 2. Listar todos los proveedores       ║");
                Console.WriteLine("║ 3. Buscar proveedor por ID            ║");
                Console.WriteLine("║ 4. Actualizar proveedor               ║");
                Console.WriteLine("║ 5. Eliminar proveedor                 ║");
                Console.WriteLine("║ 6. Buscar por nombre                  ║");
                Console.WriteLine("║ 0. Volver                             ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.Write("\nOpción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": AgregarProveedor(); break;
                    case "2": ListarProveedores(); break;
                    case "3": BuscarProveedorPorId(); break;
                    case "4": ActualizarProveedor(); break;
                    case "5": EliminarProveedor(); break;
                    case "6": BuscarProveedorPorNombre(); break;
                    case "0": volver = true; break;
                    default: Console.WriteLine("\n✗ Opción no válida"); break;
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresiona cualquier tecla...");
                    Console.ReadKey();
                }
            }
        }

        static void AgregarProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR NUEVO PROVEEDOR ===\n");

            try
            {
                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();

                Console.Write("Teléfono: ");
                string telefono = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Dirección: ");
                string direccion = Console.ReadLine();

                Proveedor nuevoProveedor = new Proveedor(nombre, telefono, email, direccion);

                if (proveedorDAO.Agregar(nuevoProveedor))
                {
                    Console.WriteLine("\n✓ Proveedor agregado exitosamente");
                }
                else
                {
                    Console.WriteLine("\n✗ Error al agregar el proveedor");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void ListarProveedores()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE PROVEEDORES ===\n");

            List<Proveedor> proveedores = proveedorDAO.ObtenerTodos();

            if (proveedores.Count == 0)
            {
                Console.WriteLine("No hay proveedores registrados.");
                return;
            }

            Console.WriteLine($"Total de proveedores: {proveedores.Count}\n");
            Console.WriteLine(new string('-', 100));

            foreach (Proveedor proveedor in proveedores)
            {
                Console.WriteLine(proveedor.ToString());
                Console.WriteLine($"   Dirección: {proveedor.Direccion}");
                Console.WriteLine(new string('-', 100));
            }
        }

        static void BuscarProveedorPorId()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR PROVEEDOR POR ID ===\n");

            try
            {
                Console.Write("Ingresa el ID del proveedor: ");
                int id = int.Parse(Console.ReadLine());

                Proveedor proveedor = proveedorDAO.ObtenerPorId(id);

                if (proveedor != null)
                {
                    Console.WriteLine("\n✓ Proveedor encontrado:\n");
                    Console.WriteLine($"ID: {proveedor.ProveedorID}");
                    Console.WriteLine($"Nombre: {proveedor.Nombre}");
                    Console.WriteLine($"Teléfono: {proveedor.Telefono}");
                    Console.WriteLine($"Email: {proveedor.Email}");
                    Console.WriteLine($"Dirección: {proveedor.Direccion}");
                    Console.WriteLine($"Fecha Registro: {proveedor.FechaRegistro:dd/MM/yyyy}");
                }
                else
                {
                    Console.WriteLine("\n✗ No se encontró ningún proveedor con ese ID");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void ActualizarProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR PROVEEDOR ===\n");

            try
            {
                Console.Write("Ingresa el ID del proveedor a actualizar: ");
                int id = int.Parse(Console.ReadLine());

                Proveedor proveedor = proveedorDAO.ObtenerPorId(id);

                if (proveedor == null)
                {
                    Console.WriteLine("\n✗ No se encontró ningún proveedor con ese ID");
                    return;
                }

                Console.WriteLine($"\nProveedor actual: {proveedor.Nombre}");
                Console.WriteLine("\nIngresa los nuevos datos (Enter para mantener):\n");

                Console.Write($"Nombre [{proveedor.Nombre}]: ");
                string nombre = Console.ReadLine();
                if (!string.IsNullOrEmpty(nombre)) proveedor.Nombre = nombre;

                Console.Write($"Teléfono [{proveedor.Telefono}]: ");
                string telefono = Console.ReadLine();
                if (!string.IsNullOrEmpty(telefono)) proveedor.Telefono = telefono;

                Console.Write($"Email [{proveedor.Email}]: ");
                string email = Console.ReadLine();
                if (!string.IsNullOrEmpty(email)) proveedor.Email = email;

                Console.Write($"Dirección [{proveedor.Direccion}]: ");
                string direccion = Console.ReadLine();
                if (!string.IsNullOrEmpty(direccion)) proveedor.Direccion = direccion;

                if (proveedorDAO.Actualizar(proveedor))
                {
                    Console.WriteLine("\n✓ Proveedor actualizado exitosamente");
                }
                else
                {
                    Console.WriteLine("\n✗ Error al actualizar el proveedor");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void EliminarProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PROVEEDOR ===\n");

            try
            {
                Console.Write("Ingresa el ID del proveedor a eliminar: ");
                int id = int.Parse(Console.ReadLine());

                Proveedor proveedor = proveedorDAO.ObtenerPorId(id);

                if (proveedor == null)
                {
                    Console.WriteLine("\n✗ No se encontró ningún proveedor con ese ID");
                    return;
                }

                Console.WriteLine($"\nProveedor: {proveedor.Nombre}");
                Console.Write("\n¿Estás seguro de eliminar este proveedor? (S/N): ");
                string confirmacion = Console.ReadLine().ToUpper();

                if (confirmacion == "S")
                {
                    if (proveedorDAO.Eliminar(id))
                    {
                        Console.WriteLine("\n✓ Proveedor eliminado exitosamente");
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Error al eliminar el proveedor");
                    }
                }
                else
                {
                    Console.WriteLine("\nEliminación cancelada");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void BuscarProveedorPorNombre()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR PROVEEDOR POR NOMBRE ===\n");

            Console.Write("Ingresa el nombre: ");
            string nombre = Console.ReadLine();

            List<Proveedor> proveedores = proveedorDAO.BuscarPorNombre(nombre);

            if (proveedores.Count == 0)
            {
                Console.WriteLine("\n✗ No se encontraron proveedores");
                return;
            }

            Console.WriteLine($"\n✓ Se encontraron {proveedores.Count} proveedor(es):\n");
            Console.WriteLine(new string('-', 100));

            foreach (Proveedor proveedor in proveedores)
            {
                Console.WriteLine(proveedor.ToString());
                Console.WriteLine(new string('-', 100));
            }
        }
        #endregion

        #region MENÚ VENTAS
        static void MenuVentas()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║     SISTEMA DE VENTAS                 ║");
                Console.WriteLine("╠════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Realizar nueva venta               ║");
                Console.WriteLine("║ 2. Ver historial de ventas            ║");
                Console.WriteLine("║ 3. Ver detalle de venta               ║");
                Console.WriteLine("║ 4. Ventas por fecha                   ║");
                Console.WriteLine("║ 5. Total de ventas hoy                ║");
                Console.WriteLine("║ 0. Volver                             ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.Write("\nOpción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": RealizarVenta(); break;
                    case "2": VerHistorialVentas(); break;
                    case "3": VerDetalleVenta(); break;
                    case "4": VentasPorFecha(); break;
                    case "5": TotalVentasHoy(); break;
                    case "0": volver = true; break;
                    default: Console.WriteLine("\n✗ Opción no válida"); break;
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresiona cualquier tecla...");
                    Console.ReadKey();
                }
            }
        }

        static void RealizarVenta()
        {
            Console.Clear();
            Console.WriteLine("=== NUEVA VENTA ===\n");

            try
            {
                // 1. Seleccionar cliente
                Console.WriteLine("--- Seleccionar Cliente ---");
                var clientes = clienteDAO.ObtenerTodos();

                if (clientes.Count == 0)
                {
                    Console.WriteLine("No hay clientes registrados. Debes agregar clientes primero.");
                    return;
                }

                foreach (var c in clientes.Take(10))
                {
                    Console.WriteLine($"{c.ClienteID}. {c.NombreCompleto} - {c.Telefono}");
                }

                Console.Write("\nID del Cliente: ");
                int clienteID = int.Parse(Console.ReadLine());

                Cliente cliente = clienteDAO.ObtenerPorId(clienteID);
                if (cliente == null)
                {
                    Console.WriteLine("Cliente no encontrado");
                    return;
                }

                // 2. Crear venta
                Venta venta = new Venta
                {
                    ClienteID = clienteID,
                    NombreCliente = cliente.NombreCompleto
                };

                // 3. Agregar productos
                bool agregarMas = true;
                while (agregarMas)
                {
                    Console.WriteLine("\n--- Agregar Producto ---");

                    // Mostrar prendas disponibles
                    var prendas = prendaDAO.ObtenerTodas().Where(p => p.Stock > 0).Take(10).ToList();

                    if (prendas.Count == 0)
                    {
                        Console.WriteLine("No hay prendas disponibles en stock.");
                        break;
                    }

                    foreach (var p in prendas)
                    {
                        Console.WriteLine($"{p.PrendaID}. {p.Nombre} - ${p.PrecioVenta:F2} (Stock: {p.Stock})");
                    }

                    Console.Write("\nID de la Prenda: ");
                    int prendaID = int.Parse(Console.ReadLine());

                    Prenda prenda = prendaDAO.ObtenerPorId(prendaID);
                    if (prenda == null || prenda.Stock <= 0)
                    {
                        Console.WriteLine("Prenda no disponible");
                        continue;
                    }

                    Console.Write($"Cantidad (disponible: {prenda.Stock}): ");
                    int cantidad = int.Parse(Console.ReadLine());

                    if (cantidad > prenda.Stock)
                    {
                        Console.WriteLine($"Stock insuficiente. Solo hay {prenda.Stock} disponibles.");
                        continue;
                    }

                    DetalleVenta detalle = new DetalleVenta(
                        prendaID,
                        prenda.Nombre,
                        cantidad,
                        prenda.PrecioVenta
                    );

                    venta.Detalles.Add(detalle);
                    Console.WriteLine($"✓ {cantidad} x {prenda.Nombre} agregado");

                    Console.Write("\n¿Agregar más productos? (S/N): ");
                    agregarMas = Console.ReadLine().ToUpper() == "S";
                }

                if (venta.Detalles.Count == 0)
                {
                    Console.WriteLine("\nNo se agregaron productos. Venta cancelada.");
                    return;
                }

                // 4. Calcular totales
                venta.CalcularTotales();

                // 5. Aplicar descuento
                var descuentos = descuentoDAO.ObtenerVigentes();
                if (descuentos.Count > 0)
                {
                    Console.WriteLine("\n--- Descuentos Disponibles ---");
                    Console.WriteLine("0. No aplicar descuento");
                    for (int i = 0; i < descuentos.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {descuentos[i].Nombre} - {descuentos[i].Porcentaje}%");
                    }

                    Console.Write("\nSeleccionar descuento: ");
                    int descOpc = int.Parse(Console.ReadLine());

                    if (descOpc > 0 && descOpc <= descuentos.Count)
                    {
                        var descSeleccionado = descuentos[descOpc - 1];
                        venta.Descuento = descSeleccionado.CalcularDescuento(venta.Subtotal);
                        venta.Total = venta.Subtotal - venta.Descuento;
                    }
                }

                // 6. Método de pago
                Console.Write("\nMétodo de Pago (Efectivo/Tarjeta/Transferencia): ");
                venta.MetodoPago = Console.ReadLine();

                // 7. Resumen
                Console.WriteLine("\n" + new string('=', 50));
                Console.WriteLine("RESUMEN DE LA VENTA");
                Console.WriteLine(new string('=', 50));
                Console.WriteLine($"Cliente: {venta.NombreCliente}");
                Console.WriteLine("\nProductos:");
                foreach (var detalle in venta.Detalles)
                {
                    Console.WriteLine($"  {detalle.ToString()}");
                }
                Console.WriteLine($"\nSubtotal: ${venta.Subtotal:F2}");
                Console.WriteLine($"Descuento: ${venta.Descuento:F2}");
                Console.WriteLine($"TOTAL: ${venta.Total:F2}");
                Console.WriteLine($"Método de Pago: {venta.MetodoPago}");
                Console.WriteLine(new string('=', 50));

                Console.Write("\n¿Confirmar venta? (S/N): ");
                if (Console.ReadLine().ToUpper() == "S")
                {
                    if (ventaDAO.RegistrarVenta(venta))
                    {
                        Console.WriteLine("\n✓ VENTA REGISTRADA EXITOSAMENTE");
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Error al registrar la venta");
                    }
                }
                else
                {
                    Console.WriteLine("\nVenta cancelada");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void VerHistorialVentas()
        {
            Console.Clear();
            Console.WriteLine("=== HISTORIAL DE VENTAS ===\n");

            List<Venta> ventas = ventaDAO.ObtenerTodas();

            if (ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas.");
                return;
            }

            Console.WriteLine($"Total de ventas: {ventas.Count}\n");
            Console.WriteLine(new string('-', 100));

            foreach (Venta venta in ventas)
            {
                Console.WriteLine(venta.ToString());
                Console.WriteLine($"   Método de Pago: {venta.MetodoPago}");
                Console.WriteLine(new string('-', 100));
            }
        }

        static void VerDetalleVenta()
        {
            Console.Clear();
            Console.WriteLine("=== DETALLE DE VENTA ===\n");

            try
            {
                Console.Write("Ingresa el ID de la venta: ");
                int id = int.Parse(Console.ReadLine());

                Venta venta = ventaDAO.ObtenerPorId(id);

                if (venta == null)
                {
                    Console.WriteLine("\n✗ No se encontró la venta");
                    return;
                }

                Console.WriteLine("\n" + new string('=', 50));
                Console.WriteLine($"VENTA #{venta.VentaID}");
                Console.WriteLine(new string('=', 50));
                Console.WriteLine($"Cliente: {venta.NombreCliente}");
                Console.WriteLine($"Fecha: {venta.FechaVenta:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Método de Pago: {venta.MetodoPago}");
                Console.WriteLine("\nProductos:");
                Console.WriteLine(new string('-', 50));

                foreach (var detalle in venta.Detalles)
                {
                    Console.WriteLine(detalle.ToString());
                }

                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"Subtotal: ${venta.Subtotal:F2}");
                Console.WriteLine($"Descuento: ${venta.Descuento:F2}");
                Console.WriteLine($"TOTAL: ${venta.Total:F2}");
                Console.WriteLine(new string('=', 50));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void VentasPorFecha()
        {
            Console.Clear();
            Console.WriteLine("=== VENTAS POR FECHA ===\n");

            try
            {
                Console.Write("Fecha inicio (dd/mm/yyyy): ");
                DateTime fechaInicio = DateTime.Parse(Console.ReadLine());

                Console.Write("Fecha fin (dd/mm/yyyy): ");
                DateTime fechaFin = DateTime.Parse(Console.ReadLine());

                List<Venta> ventas = ventaDAO.ObtenerPorFechas(fechaInicio, fechaFin);

                if (ventas.Count == 0)
                {
                    Console.WriteLine("\nNo hay ventas en ese rango de fechas.");
                    return;
                }

                decimal totalVentas = ventas.Sum(v => v.Total);

                Console.WriteLine($"\n✓ Se encontraron {ventas.Count} venta(s)");
                Console.WriteLine($"Total vendido: ${totalVentas:F2}\n");
                Console.WriteLine(new string('-', 100));

                foreach (Venta venta in ventas)
                {
                    Console.WriteLine(venta.ToString());
                    Console.WriteLine(new string('-', 100));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void TotalVentasHoy()
        {
            Console.Clear();
            Console.WriteLine("=== VENTAS DEL DÍA ===\n");

            decimal total = ventaDAO.ObtenerTotalVentasHoy();

            Console.WriteLine($"Fecha: {DateTime.Now:dd/MM/yyyy}");
            Console.WriteLine($"Total vendido hoy: ${total:F2}");
        }
        #endregion

        #region MENÚ DESCUENTOS
        static void MenuDescuentos()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║     GESTIÓN DE DESCUENTOS             ║");
                Console.WriteLine("╠════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Agregar nuevo descuento            ║");
                Console.WriteLine("║ 2. Listar todos los descuentos        ║");
                Console.WriteLine("║ 3. Ver descuentos vigentes            ║");
                Console.WriteLine("║ 4. Actualizar descuento               ║");
                Console.WriteLine("║ 5. Activar/Desactivar descuento       ║");
                Console.WriteLine("║ 6. Eliminar descuento                 ║");
                Console.WriteLine("║ 0. Volver                             ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.Write("\nOpción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": AgregarDescuento(); break;
                    case "2": ListarDescuentos(); break;
                    case "3": VerDescuentosVigentes(); break;
                    case "4": ActualizarDescuento(); break;
                    case "5": CambiarEstadoDescuento(); break;
                    case "6": EliminarDescuento(); break;
                    case "0": volver = true; break;
                    default: Console.WriteLine("\n✗ Opción no válida"); break;
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresiona cualquier tecla...");
                    Console.ReadKey();
                }
            }
        }

        static void AgregarDescuento()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR NUEVO DESCUENTO ===\n");

            try
            {
                Console.Write("Nombre del descuento: ");
                string nombre = Console.ReadLine();

                Console.Write("Porcentaje (ej: 15 para 15%): ");
                decimal porcentaje = decimal.Parse(Console.ReadLine());

                Console.Write("Fecha inicio (dd/mm/yyyy): ");
                DateTime fechaInicio = DateTime.Parse(Console.ReadLine());

                Console.Write("Fecha fin (dd/mm/yyyy): ");
                DateTime fechaFin = DateTime.Parse(Console.ReadLine());

                Descuento nuevoDescuento = new Descuento(nombre, porcentaje, fechaInicio, fechaFin);

                if (descuentoDAO.Agregar(nuevoDescuento))
                {
                    Console.WriteLine("\n✓ Descuento agregado exitosamente");
                }
                else
                {
                    Console.WriteLine("\n✗ Error al agregar el descuento");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void ListarDescuentos()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE DESCUENTOS ===\n");

            List<Descuento> descuentos = descuentoDAO.ObtenerTodos();

            if (descuentos.Count == 0)
            {
                Console.WriteLine("No hay descuentos registrados.");
                return;
            }

            Console.WriteLine($"Total de descuentos: {descuentos.Count}\n");
            Console.WriteLine(new string('-', 100));

            foreach (Descuento descuento in descuentos)
            {
                Console.WriteLine(descuento.ToString());
                Console.WriteLine(new string('-', 100));
            }
        }

        static void VerDescuentosVigentes()
        {
            Console.Clear();
            Console.WriteLine("=== DESCUENTOS VIGENTES ===\n");

            List<Descuento> descuentos = descuentoDAO.ObtenerVigentes();

            if (descuentos.Count == 0)
            {
                Console.WriteLine("No hay descuentos vigentes en este momento.");
                return;
            }

            Console.WriteLine($"Descuentos activos: {descuentos.Count}\n");
            Console.WriteLine(new string('-', 100));

            foreach (Descuento descuento in descuentos)
            {
                Console.WriteLine(descuento.ToString());
                Console.WriteLine(new string('-', 100));
            }
        }

        static void ActualizarDescuento()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR DESCUENTO ===\n");

            try
            {
                Console.Write("Ingresa el ID del descuento a actualizar: ");
                int id = int.Parse(Console.ReadLine());

                Descuento descuento = descuentoDAO.ObtenerPorId(id);

                if (descuento == null)
                {
                    Console.WriteLine("\n✗ No se encontró el descuento");
                    return;
                }

                Console.WriteLine($"\nDescuento actual: {descuento.Nombre}");
                Console.WriteLine("\nIngresa los nuevos datos (Enter para mantener):\n");

                Console.Write($"Nombre [{descuento.Nombre}]: ");
                string nombre = Console.ReadLine();
                if (!string.IsNullOrEmpty(nombre)) descuento.Nombre = nombre;

                Console.Write($"Porcentaje [{descuento.Porcentaje}]: ");
                string porcStr = Console.ReadLine();
                if (!string.IsNullOrEmpty(porcStr)) descuento.Porcentaje = decimal.Parse(porcStr);

                if (descuentoDAO.Actualizar(descuento))
                {
                    Console.WriteLine("\n✓ Descuento actualizado exitosamente");
                }
                else
                {
                    Console.WriteLine("\n✗ Error al actualizar el descuento");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void CambiarEstadoDescuento()
        {
            Console.Clear();
            Console.WriteLine("=== ACTIVAR/DESACTIVAR DESCUENTO ===\n");

            try
            {
                Console.Write("Ingresa el ID del descuento: ");
                int id = int.Parse(Console.ReadLine());

                Descuento descuento = descuentoDAO.ObtenerPorId(id);

                if (descuento == null)
                {
                    Console.WriteLine("\n✗ No se encontró el descuento");
                    return;
                }

                string estadoActual = descuento.Activo ? "ACTIVO" : "INACTIVO";
                Console.WriteLine($"\nDescuento: {descuento.Nombre}");
                Console.WriteLine($"Estado actual: {estadoActual}");

                Console.Write($"\n¿Cambiar a {(descuento.Activo ? "INACTIVO" : "ACTIVO")}? (S/N): ");
                if (Console.ReadLine().ToUpper() == "S")
                {
                    if (descuentoDAO.CambiarEstado(id, !descuento.Activo))
                    {
                        Console.WriteLine("\n✓ Estado actualizado exitosamente");
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Error al actualizar el estado");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        static void EliminarDescuento()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR DESCUENTO ===\n");

            try
            {
                Console.Write("Ingresa el ID del descuento a eliminar: ");
                int id = int.Parse(Console.ReadLine());

                Descuento descuento = descuentoDAO.ObtenerPorId(id);

                if (descuento == null)
                {
                    Console.WriteLine("\n✗ No se encontró el descuento");
                    return;
                }

                Console.WriteLine($"\nDescuento: {descuento.Nombre}");
                Console.Write("\n¿Estás seguro de eliminar este descuento? (S/N): ");
                string confirmacion = Console.ReadLine().ToUpper();

                if (confirmacion == "S")
                {
                    if (descuentoDAO.Eliminar(id))
                    {
                        Console.WriteLine("\n✓ Descuento eliminado exitosamente");
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Error al eliminar el descuento");
                    }
                }
                else
                {
                    Console.WriteLine("\nEliminación cancelada");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }
        #endregion

        #region MENÚ REPORTES
        static void MenuReportes()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║     REPORTES Y ESTADÍSTICAS           ║");
                Console.WriteLine("╠════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Productos más vendidos             ║");
                Console.WriteLine("║ 2. Inventario bajo stock              ║");
                Console.WriteLine("║ 3. Resumen de ventas del mes          ║");
                Console.WriteLine("║ 4. Clientes frecuentes                ║");
                Console.WriteLine("║ 0. Volver                             ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.Write("\nOpción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": ProductosMasVendidos(); break;
                    case "2": InventarioBajoStock(); break;
                    case "3": ResumenVentasMes(); break;
                    case "4": ClientesFrecuentes(); break;
                    case "0": volver = true; break;
                    default: Console.WriteLine("\n✗ Opción no válida"); break;
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresiona cualquier tecla...");
                    Console.ReadKey();
                }
            }
        }

        static void ProductosMasVendidos()
        {
            Console.Clear();
            Console.WriteLine("=== PRODUCTOS MÁS VENDIDOS ===\n");

            var productos = ventaDAO.ObtenerProductosMasVendidos(10);

            if (productos.Count == 0)
            {
                Console.WriteLine("No hay datos de ventas.");
                return;
            }

            Console.WriteLine("Top 10 productos más vendidos:\n");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"PRODUCTO",-40} {"CANTIDAD",10}");
            Console.WriteLine(new string('-', 60));

            foreach (var (nombre, cantidad) in productos)
            {
                Console.WriteLine($"{nombre,-40} {cantidad,10}");
            }
            Console.WriteLine(new string('-', 60));
        }

        static void InventarioBajoStock()
        {
            Console.Clear();
            Console.WriteLine("=== INVENTARIO BAJO STOCK ===\n");

            List<Prenda> prendas = prendaDAO.ObtenerTodas()
                .Where(p => p.Stock <= 5)
                .OrderBy(p => p.Stock)
                .ToList();

            if (prendas.Count == 0)
            {
                Console.WriteLine("✓ Todos los productos tienen stock suficiente.");
                return;
            }

            Console.WriteLine($"⚠ {prendas.Count} producto(s) con stock bajo:\n");
            Console.WriteLine(new string('-', 100));

            foreach (Prenda prenda in prendas)
            {
                string alerta = prenda.Stock == 0 ? "[SIN STOCK]" : "[STOCK BAJO]";
                Console.WriteLine($"{alerta} {prenda.ToString()}");
                Console.WriteLine(new string('-', 100));
            }
        }

        static void ResumenVentasMes()
        {
            Console.Clear();
            Console.WriteLine("=== RESUMEN DE VENTAS DEL MES ===\n");

            DateTime inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime finMes = inicioMes.AddMonths(1).AddDays(-1);

            List<Venta> ventas = ventaDAO.ObtenerPorFechas(inicioMes, finMes);

            if (ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas en este mes.");
                return;
            }

            decimal totalVentas = ventas.Sum(v => v.Total);
            decimal totalDescuentos = ventas.Sum(v => v.Descuento);
            decimal promedioVenta = totalVentas / ventas.Count;

            Console.WriteLine($"Período: {inicioMes:dd/MM/yyyy} - {finMes:dd/MM/yyyy}");
            Console.WriteLine(new string('=', 50));
            Console.WriteLine($"Total de ventas: {ventas.Count}");
            Console.WriteLine($"Monto total: ${totalVentas:F2}");
            Console.WriteLine($"Descuentos aplicados: ${totalDescuentos:F2}");
            Console.WriteLine($"Promedio por venta: ${promedioVenta:F2}");
            Console.WriteLine(new string('=', 50));

            // Ventas por método de pago
            var ventasPorMetodo = ventas.GroupBy(v => v.MetodoPago)
                .Select(g => new { Metodo = g.Key, Total = g.Sum(v => v.Total), Cantidad = g.Count() })
                .OrderByDescending(x => x.Total)
                .ToList();

            Console.WriteLine("\nVentas por método de pago:");
            foreach (var metodo in ventasPorMetodo)
            {
                Console.WriteLine($"  {metodo.Metodo}: {metodo.Cantidad} ventas - ${metodo.Total:F2}");
            }
        }

        static void ClientesFrecuentes()
        {
            Console.Clear();
            Console.WriteLine("=== CLIENTES FRECUENTES ===\n");

            // Obtener todas las ventas con información del cliente
            List<Venta> ventas = ventaDAO.ObtenerTodas();

            if (ventas.Count == 0)
            {
                Console.WriteLine("No hay datos de ventas.");
                return;
            }

            // Agrupar por cliente
            var clientesStats = ventas
                .GroupBy(v => new { v.ClienteID, v.NombreCliente })
                .Select(g => new
                {
                    Cliente = g.Key.NombreCliente,
                    Compras = g.Count(),
                    TotalGastado = g.Sum(v => v.Total)
                })
                .OrderByDescending(c => c.Compras)
                .Take(10)
                .ToList();

            Console.WriteLine("Top 10 clientes más frecuentes:\n");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"{"CLIENTE",-30} {"COMPRAS",10} {"TOTAL GASTADO",20}");
            Console.WriteLine(new string('-', 80));

            foreach (var cliente in clientesStats)
            {
                Console.WriteLine($"{cliente.Cliente,-30} {cliente.Compras,10} ${cliente.TotalGastado,18:F2}");
            }
            Console.WriteLine(new string('-', 80));
        }
        #endregion
    }
}

