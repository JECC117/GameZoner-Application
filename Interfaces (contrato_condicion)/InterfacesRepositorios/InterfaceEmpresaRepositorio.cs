using GameZone.Modelos.Empresa;

namespace GameZone.Interfaces__contrato_condicion_.InterfacesRepositorios
{
    public interface InterfaceEmpresaRepositorio
    {
        Task<IEnumerable<Empresa>?> ObtenerTodasLasEmpresasRepo();
        Task<Empresa?> ObtenerEmpresaPorIdRepo(long id);
        void RegistrarEmpresaRepo(Empresa empresa);
        Task<Empresa?> InicioSesionEmpresaRepo(EmpresaDTO empresaDTO);
        void EliminarPerfilEmpresaPorReferenciaRepo(Empresa empresa);
        Task<bool> GuardarCambiosAsyncRepo();
        Task<bool> NombreDuplicado(EmpresaDTO empresaDTO);
        Task<bool> CorreoDuplicado(EmpresaDTO empresaDTO);


    }
}
