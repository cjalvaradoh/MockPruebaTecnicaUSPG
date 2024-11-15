using Microsoft.AspNetCore.Mvc;
using MockPruebaTecnica.Data;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using MockPruebaTecnica.Models;
using System.Text.RegularExpressions;

namespace MockPruebaTecnica.Controllers
{
    public class CargasController : Controller
    {
        private readonly AppDbContext _context;

        public CargasController(AppDbContext context)
        {
            _context = context;           
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "Cargar Archivo";
            return View();
        }

        private bool VentasExists(long? id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }


        [HttpPost]
        public async Task<IActionResult> CargarArchivo(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "No se seleccionó ningún archivo.";
                return BadRequest("Por favor, seleccione un archivo de Excel");
            }

            if (!file.FileName.EndsWith(".xlsx"))
            {
                return BadRequest("Por favor, seleccione un archivo de Excel");
            }

            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataTable = reader.AsDataSet().Tables[0];

                    for (int fila = 1; fila < dataTable.Rows.Count; fila++)
                    {
                        DateTime fechaVenta = DateTime.Parse(dataTable.Rows[fila][0].ToString());
                        string nombreCliente = dataTable.Rows[fila][1].ToString();
                        string apellidoCliente = dataTable.Rows[fila][2].ToString();
                        string correoCliente = dataTable.Rows[fila][3].ToString();
                        string codigoBarras = dataTable.Rows[fila][4].ToString();
                        string nombreProducto = dataTable.Rows[fila][5].ToString();
                        string descripcionProducto = dataTable.Rows[fila][6].ToString();
                        string categoriaProducto = dataTable.Rows[fila][7].ToString();
                        long cantidad = long.Parse(dataTable.Rows[fila][8].ToString());
                        decimal precio = decimal.Parse(dataTable.Rows[fila][9].ToString());
                        decimal totalVenta = cantidad * precio;

                        // Buscar o crear el cliente
                        var cliente = await _context.Clientes
                            .FirstOrDefaultAsync(c => c.Correo == correoCliente);
                        if (cliente == null)
                        {
                            // Crear nuevo cliente si no existe
                            cliente = new Cliente
                            {
                                Nombre = nombreCliente,
                                Apellido = apellidoCliente,
                                Correo = correoCliente
                            };
                            _context.Clientes.Add(cliente);
                        }

                        // Buscar o crear el producto
                        var producto = await _context.Productos
                            .FirstOrDefaultAsync(p => p.Codigo == codigoBarras);
                        if (producto == null)
                        {
                            // Crear nuevo producto si no existe
                            producto = new Productos
                            {
                                Nombre = nombreProducto,
                                descripcion = descripcionProducto,
                                categoria = categoriaProducto,
                                Codigo = codigoBarras,
                                Precio = precio
                            };
                            _context.Productos.Add(producto);
                        }

                        // Crear la venta
                        var venta = new Venta
                        {
                            Fecha = fechaVenta,
                            ClienteId = cliente.Id,
                            ProductoId = producto.Id,
                            Cantidad = cantidad,
                            Total = totalVenta
                        };

                        // Agregar la venta al contexto
                        _context.Ventas.Add(venta);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}
