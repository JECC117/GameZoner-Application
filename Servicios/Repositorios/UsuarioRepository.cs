using GameZone.DatabaseContext;
using GameZone.Interfaces__contrato_condicion_.InterfacesRepositorios;
using GameZone.Modelos.Usuario;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Servicios.Repositorios
{
    public class UsuarioRepository : InterfaceUsuarioRepositorio
    {
        private readonly GameZonerDBContext _context;

        public UsuarioRepository(GameZonerDBContext context)
        {
            _context = context; //Solo se hace la inyección de dependencias una sola vez (En los correspondientes repositorios) y se maneja el acceso a DBContext mediante inyección de dependencias.
        }

        //Metodos
        public async Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosRepo()
        {
            var Usuarios = await _context.Usuarios.ToListAsync(); //Al asignar var, intuye que el tipo mas adecuado es una lista. Y al configurarse en parametros IEnumerable, ahora la lista cumple con los parametros de la Interfaz IEnumerable.
            return Usuarios; //Adicionalmente, ToListAsync() se encarga de instanciar la lista. Asi que por eso no es necesario incluir New List()

        }

        public async Task<Usuario?> ObtenerUsuarioPorIdRepo(long id)
        {
            var UsuarioSeleccionado = await _context.Usuarios.FindAsync(id);
            return UsuarioSeleccionado;

        }

        public void CrearUsuarioRepo(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);

        }

        public async Task<Usuario?> LoginUsuarioRepo(UsuarioDTO usuarioDTO)
        {
            var UsuarioExistente= await _context.Usuarios.FirstOrDefaultAsync(ObjetoActualEnTabla => ObjetoActualEnTabla.CorreoElectronico== usuarioDTO.CorreoElectronicoDTO);
            return UsuarioExistente;
        }

        public void EliminarUsuarioPorReferenciaRepo(Usuario usuario)
        {
            _context.Remove(usuario);
        }

        public async Task<bool> GuardarCambiosAsyncRepo()
        {
            var RegistrosActualizados= await _context.SaveChangesAsync(); //SaveChangesAsync aparte de obviamente actualizar la base de datos, retorna la cantidad de registros actualizados.
            if(RegistrosActualizados > 0)
            {
                return true;   //Al implementar Repositorio, este es el origen de la escalera de Exception Stack, cada escalón me representa un destino con cada vez más alta jerarquía hasta llegar al controlador. Cada escalón necesita un Try Catch para capturar las Exceptions si llegasen a ocurrir desde el suelo (este método en el Repositorio). Para aclarar dudas, revise los Try Catch de los ClaseServicioControlador
            }
            return false;
        }

        public async Task<bool> CorreoDuplicado(UsuarioDTO usuarioDTO)
        {
            return await _context.Usuarios.AnyAsync(ObjetoActualEnTablaUsuarios => ObjetoActualEnTablaUsuarios.CorreoElectronico == usuarioDTO.CorreoElectronicoDTO);
        }

        public async Task<bool> AliasDuplicado(UsuarioDTO usuarioDTO)
        {
            return await _context.Usuarios.AnyAsync(ObjetoActualEnTablaUsuarios => ObjetoActualEnTablaUsuarios.AliasUsuario == usuarioDTO.AliasUsuarioDTO);
        }
    }
    //La documentacion de Repositorios está bastande desactualizada, pero adaptandola al contexto actual (y teniendo en cuenta que estos repositorios son los métodos asíncronos con la base de datos) DEBEMOS USAR TASK (porque recordar que Task representa una promesa)
}
