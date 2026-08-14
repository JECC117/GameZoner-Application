using GameZone.Modelos.Empresa;
using GameZone.Modelos.Usuario;

namespace GameZone.Interfaces__contrato_condicion_.InterfacesEmpresa
{
    public interface InterfaceEmpresaController
    {
        Task<IEnumerable<EmpresaDTO>?> ObtenerTodasLasEmpresas();
        Task<EmpresaDTO?> ObtenerEmpresaPorId(long id);
        Task<EmpresaDTO> RegistrarEmpresa(EmpresaDTO empresaDTO);
        Task<string> InicioSesionEmpresa(EmpresaDTO empresaDTO);
        Task EditarPerfilEmpresaPorId(long id, EmpresaDTO empresaDTO);
        Task EliminarPerfilEmpresaPorId(long id);
    }
}
