using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MockPruebaTecnica.Models
{
    public class Venta
    {
        [Key]
        [Column("id_venta")]
        public int Id { get; set; }

        [Required]
        [Column("fecha_venta")]
        public DateTime FechaVenta { get; set; }

        [Required]
        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; } // Propiedad de navegación para Cliente

        [Required]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; } // Propiedad de navegación para Producto

        [Required]
        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Required]
        [Column("total_venta", TypeName = "decimal(10, 2)")]
        public decimal TotalVenta { get; set; }
    }
}
