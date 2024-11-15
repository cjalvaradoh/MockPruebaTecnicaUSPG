using System.ComponentModel.DataAnnotations.Schema;

namespace MockPruebaTecnica.Models
{
    public class Venta
    {
        [Column("id_venta")]
        public int Id { get; set; }

        [Column("fecha_venta")]
        public DateTime Fecha { get; set; }

        [Column("total_venta")]
        public decimal Total { get; set; }

        [Column("id_cliente")]
        public int ClienteId { get; set; }

        [Column("id_producto")]
        public int ProductoId { get; set; }

        [Column("cantidad")]
        public long Cantidad { get; set; }

        public Cliente? Cliente { get; set; } = new Cliente(); // Inicialización opcional
        public Productos? Productos { get; set; } = new Productos();

    }
}
