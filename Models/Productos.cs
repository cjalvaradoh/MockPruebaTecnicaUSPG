using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MockPruebaTecnica.Models
{
    public class Producto
    {
        [Key]
        [Column("id_producto")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("codigo_barras")]
        public string CodigoBarras { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nombre_producto")]
        public string NombreProducto { get; set; }

        [StringLength(255)]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [StringLength(50)]
        [Column("categoria")]
        public string Categoria { get; set; }

        [Required]
        [Column("precio", TypeName = "decimal(10, 2)")]
        public decimal Precio { get; set; }

    }
}
