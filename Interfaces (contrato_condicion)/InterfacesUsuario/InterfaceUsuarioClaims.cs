using System.Security.Claims;

namespace GameZone.Interfaces__contrato_condicion_.InterfacesUsuario;

public interface InterfaceUsuarioClaims
{
    // Propiedades de solo lectura (getters)
    bool IsAutenticated { get; }  //Recordar que solo buscamos validar informacion, solo necesitamos metodo get
    long? Id_Usuario { get; }
    string? Nombre_Usuario { get; }
    string? Correo_Usuario { get; }
    string? Rol_Usuario { get; }

    // Método de verificación
    bool TieneRolValido(string Rol);
}