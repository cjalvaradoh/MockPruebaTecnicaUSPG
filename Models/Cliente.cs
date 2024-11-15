using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MockPruebaTecnica.Models
{
    public class Cliente
    {
        [Column("id_cliente")]
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        [Column("nombre")]
        public string Nombre { get; set; }

        [StringLength(255)]
        [Required]
        [Column("apellido")]
        public string Apellido { get; set; }

        [StringLength(255)]
        [Column("correo_electronico")]
        public string Correo { get; set; }

        public ICollection<Venta> Ventas { get; set; } // Relacion de uno a muchos
    }
}
