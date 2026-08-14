using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace GameZone.Modelos.Usuario
{
    public class UsuarioDTO
    {
            public long Id_UserDTO { get; set; }

            
            public string? AliasUsuarioDTO { get; set; }

             
            public string? CorreoElectronicoDTO { get; set; }
            
            public string? PasswordDTO { get; set; }


        public string RolDTO { get; set; } = "Cliente";

            
            public DateTime FechaRegistro_DTO { get; set; }
        
    }
}
