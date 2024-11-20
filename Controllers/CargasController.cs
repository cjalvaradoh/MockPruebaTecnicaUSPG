using Microsoft.AspNetCore.Mvc;
using MockPruebaTecnica.Data;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using MockPruebaTecnica.Models;
using System.Text.RegularExpressions;

namespace MockPruebaTecnica.Controllers
{
    public class CargaController : Controller
    {
        private readonly AppDbContext _context;

        public CargaController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        private bool VentaExists(long? id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> CargarArchivo(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
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
                        DateTime fecha_venta = Convert.ToDateTime(dataTable.Rows[fila][0]);
                        string nombre = dataTable.Rows[fila][1].ToString();
                        string apellido = dataTable.Rows[fila][2].ToString();
                        string correo_electronico = dataTable.Rows[fila][3].ToString();
                        string codigo_barras = dataTable.Rows[fila][4].ToString();
                        string nombre_producto = dataTable.Rows[fila][5].ToString();
                        string descripcion = dataTable.Rows[fila][6].ToString();
                        string categoria = dataTable.Rows[fila][7].ToString();
                        int cantidad = Convert.ToInt32(dataTable.Rows[fila][8]);
                        decimal precio = decimal.Parse(dataTable.Rows[fila][9].ToString());
                        decimal total_venta = decimal.Parse(dataTable.Rows[fila][10].ToString());

                        // Verificar si el producto ya existe en la base de datos
                        var producto = await _context.Productos.FirstOrDefaultAsync(p => p.CodigoBarras == codigo_barras);

                        if (producto == null)
                        {
                            // Si el producto no existe, lo creamos
                            producto = new Producto
                            {
                                CodigoBarras = codigo_barras,
                                NombreProducto = nombre_producto,
                                Descripcion = descripcion,
                                Categoria = categoria,
                                Precio = precio
                            };
                            _context.Productos.Add(producto);
                            await _context.SaveChangesAsync();
                        }

                        // Verificar si el cliente ya existe en la base de datos
                        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.CorreoElectronico == correo_electronico);

                        if (cliente == null)
                        {
                            // Si el cliente no existe, lo creamos
                            cliente = new Cliente
                            {
                                Nombre = nombre,
                                Apellido = apellido,
                                CorreoElectronico = correo_electronico
                            };
                            _context.Clientes.Add(cliente);
                            await _context.SaveChangesAsync();
                        }

                        // Crear una nueva venta con los datos extraídos
                        var nuevaVenta = new Venta
                        {
                            FechaVenta = fecha_venta,
                            IdCliente = cliente.Id,
                            IdProducto = producto.Id,
                            Cantidad = cantidad,
                            TotalVenta = total_venta
                        };

                        _context.Ventas.Add(nuevaVenta);
                    }

                    // Guardar todas las ventas en la base de datos
                    await _context.SaveChangesAsync();
                }
            }

            return Ok("Archivo procesado exitosamente y datos insertados en la base de datos");
        }
    }
}
