using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameZone.Modelos
{
    public class Videojuego
    {
        [Key]
        //Para autoincremental
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id_Juego { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Titulo { get; set; }

        [ForeignKey("Desarrollador")]
        public long Id_De_Desarrollador { get; set; }

        [Required]
        [Column(TypeName = "varchar(1500)")]
        public string Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        //Se usa decimal para evitar problemas de precisión con los precios, especialmente si se necesitan cálculos financieros.Ademas, se usa la anotación Column para especificar el tipo de datos en la base de datos, asegurando que se almacene correctamente con dos decimales.
        public decimal Precio { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
