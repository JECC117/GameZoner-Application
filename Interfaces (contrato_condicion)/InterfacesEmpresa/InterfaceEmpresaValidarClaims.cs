using MySqlConnector.Logging;

namespace GameZone.Interfaces__contrato_condicion_.InterfacesEmpresa
{
    public interface InterfaceEmpresaValidarClaims
    {

        long? Id_Empresa { get; }
        string? Email_Empresa { get; }
        string? Name_Empresa { get; }
        bool IsAuthenticated { get; }
        bool RolValido(string Rol);
    }
}
