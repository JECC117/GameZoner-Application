using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GameZone.Modelos.Empresa
{
    public class Empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id_Empresa { get; set; }

        [Required]
        [NotNull]
        [Column(TypeName = "varchar(50)")]
        public string Nombre_Empresa { get; set; } = null!;

        [Required]
        [NotNull]
        [Column(TypeName= "varchar(10)")] //Si es Publisher o Desarrolladora
        public string Tipo_Empresa { get; set; } = null!;

        [Required]
        [NotNull]
        [Column(TypeName ="varchar(100)")]
        public string PasswordEncriptada_Empresa { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Column(TypeName = "varchar(30)")]
        public string Email_Empresa { get; set; } = null!;

        [Required]
        public DateTime FechaRegistro_Empresa = DateTime.UtcNow;

        //Referencia a la tabla intermedia con Videojuegos (Relación de M:N)

    }
}
