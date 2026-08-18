using GameZone.Modelos.Usuario;

namespace GameZone.Interfaces__contrato_condicion_.InterfacesRepositorios
{
    public interface InterfaceUsuarioRepositorio
    {
        Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosRepo();
        Task<Usuario?> ObtenerUsuarioPorIdRepo(long id);
        void CrearUsuarioRepo(Usuario usuario);
        Task<Usuario?> LoginUsuarioRepo(UsuarioDTO usuarioDTO);
        void EliminarUsuarioPorReferenciaRepo(Usuario usuario);
        Task<bool> GuardarCambiosAsyncRepo();

        Task<bool> CorreoDuplicado(UsuarioDTO usuarioDTO);
        Task<bool> AliasDuplicado(UsuarioDTO usuarioDTO);
    }
}
