using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MockPruebaTecnica.Models
{
    public class Cliente
    {
        [Key]
        [Column("id_cliente")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        [Column("apellido")]
        public string Apellido { get; set; }

        [Required]
        [StringLength(100)]
        [Column("correo_electronico")]
        public string CorreoElectronico { get; set; }
    }
}

