using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MockPruebaTecnica.Models
{
    public class Productos
    {
        [Column("id_producto")]
        public int Id { get; set; }

        [Column("codigo_barras")]
        [MaxLength(100)]
        public string Codigo { get; set; }

        [Column("nombre_producto")]
        public string Nombre { get; set; }

        [Column("descripcion")]
        public string descripcion { get; set; }


        [Column("categoria")]
        public string categoria { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

    }
}
