using Microsoft.AspNetCore.Mvc;
using MockPruebaTecnica.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace MockPruebaTecnica.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string producto, string mesAnio)
        {
            // Incluir Producto para cargar datos de la propiedad de navegación
            var ventasQuery = _context.Ventas.Include(v => v.Producto).AsQueryable();

            // Filtro por producto
            if (!string.IsNullOrEmpty(producto))
            {
                ventasQuery = ventasQuery.Where(v => v.Producto.NombreProducto.Contains(producto));
            }

            // Filtro por mes-año
            if (!string.IsNullOrEmpty(mesAnio))
            {
                try
                {
                    var fechaFiltro = DateTime.ParseExact(mesAnio, "yyyy-MM", CultureInfo.InvariantCulture);
                    ventasQuery = ventasQuery.Where(v => v.FechaVenta.Year == fechaFiltro.Year && v.FechaVenta.Month == fechaFiltro.Month);
                }
                catch (FormatException)
                {
                    // Manejo de error en el formato de fecha
                }
            }

            // Obtener las ventas filtradas
            var ventas = await ventasQuery.ToListAsync();

            // Pasar filtros a la vista para mantener los valores
            ViewData["ProductoFiltro"] = producto;
            ViewData["MesAnioFiltro"] = mesAnio;

            return View("~/Views/Home/Index.cshtml", ventas); // Asegúrate de que esta vista recibe un modelo de tipo List<Venta>
        }
        public async Task<IActionResult> GetTotalVentas()
        {
            var totalVentas = await _context.Ventas.SumAsync(v => v.TotalVenta);
            return Json(totalVentas);
        }

        public async Task<IActionResult> GetTotalUnidades()
        {
            var totalUnidades = await _context.Ventas.SumAsync(v => v.Cantidad);
            return Json(totalUnidades);
        }

        public async Task<IActionResult> GetVentasPorMes()
        {
            var ventasPorMes = await _context.Ventas
            .Select(v => new
            {
                v.FechaVenta,
                Año = v.FechaVenta.Year,
                Mes = v.FechaVenta.Month,
                v.TotalVenta
            })
                .OrderBy(v => v.Año)
                .ThenBy(v => v.Mes)
                .ToListAsync();


            if (ventasPorMes == null || !ventasPorMes.Any())
            {
                return Json(new { mensaje = "No hay ventas disponibles para mostrar" });
            }

            return Json(ventasPorMes);
        }
    }
}