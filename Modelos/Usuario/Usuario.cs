using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace GameZone.Modelos.Usuario
{
    public class Usuario
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id_User { get; set; }

        [Required]
        [NotNull]
        [Column(TypeName = "varchar(100)")]
        public string AliasUsuario { get; set; } = null!;

        [Required]
        [NotNull]
        [EmailAddress]
        [Column(TypeName = "varchar(40)")]
        public string CorreoElectronico { get; set; } = null!;

        [Required]
        [NotNull]
        [Column(TypeName = "varchar(100)")]
        public string PasswordEncriptada { get; set; } = null!;

        [Required]
        [NotNull]
        public string Rol { get; set; }= "Cliente";
        //Se asigna un valor predeterminado de "Cliente" al campo Rol, lo que significa que si no se especifica un rol al crear un nuevo usuario, se le asignará automáticamente el rol de "Cliente". Esto es útil para garantizar que todos los usuarios tengan un rol válido, incluso si no se proporciona explícitamente durante la creación del usuario.
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        //Se asigna un valor predeterminado de DateTime.UtcNow al campo FechaRegistro, lo que significa que cuando se crea un nuevo usuario, se registrará automáticamente la fecha y hora actual en formato UTC (Tiempo Universal Coordinado). Esto es útil para mantener un registro preciso de cuándo se registró cada usuario, independientemente de la zona horaria en la que se encuentren.



    }

  
}
