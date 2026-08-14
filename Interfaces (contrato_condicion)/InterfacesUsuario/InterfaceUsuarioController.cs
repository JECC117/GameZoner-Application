using GameZone.Modelos.Usuario;

namespace GameZone.Interfaces__contrato_condicion_.InterfacesUsuario;

public interface InterfaceUsuarioController
{
    Task<IEnumerable<UsuarioDTO>?> ObtenerTodosLosUsuarios();
    Task<UsuarioDTO?> ObtenerUsuarioPorId(long id);
    Task<UsuarioDTO> CrearUsuario(UsuarioDTO usuarioDTO);
    Task<string> LoginUsuario(UsuarioDTO usuarioDTO);
    Task EditarUsuarioPorId(long id, UsuarioDTO usuarioDTO);
    Task EliminarUsuarioPorId(long id);
}