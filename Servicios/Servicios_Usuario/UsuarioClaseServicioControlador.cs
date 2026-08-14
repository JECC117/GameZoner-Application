using AutoMapper;
using GameZone.DatabaseContext;
using GameZone.Interfaces__contrato_condicion_.InterfacesToken;
using GameZone.Interfaces__contrato_condicion_.InterfacesUsuario;
using GameZone.Modelos.Usuario;
using GameZone.Servicios.Token;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Servicios.Servicios_Usuario
{
    public class UsuarioClaseServicioControlador : InterfaceUsuarioController
    {
        private readonly GameZonerDBContext _ContextHeredado;
        private readonly IMapper Mapeador; //Se maneja para la inyeccion del automapper
        private readonly InterfaceTokenService _TokenUsuarioInterface;
        private readonly InterfaceUsuarioClaims _UsuarioClaimsVerificacion; //AL INVOCAR LA INTERFAZ, TENGO ACCESO A LOS METODOS DE LAS CLASES QUE HAYAN INVOCADO A ESTA INTERFAZ (Que haya sido definida en AddScoped), Ejemplo: builder.Services.AddScoped<InterfaceUsuario, UsuarioActualAutorizadorServicio>();

        public UsuarioClaseServicioControlador(GameZonerDBContext contextHeredado, IMapper mapeador, InterfaceTokenService tokenUsuarioInterface, InterfaceUsuarioClaims interfaceUsuario) //Se genera una "instancia" tanto de DBContext y TokenUsuarioClass ya que en cada petición HTTP se construyen estos servicios y la instancia se genera en ese momento, mediante el constructor se captura esa instancia. 
        {
            _ContextHeredado=contextHeredado;
            Mapeador=mapeador;
            _TokenUsuarioInterface=tokenUsuarioInterface;
            _UsuarioClaimsVerificacion= interfaceUsuario;
        }

        //Metodos de Clase (ActionResult y por ende, Ok, BadRequest y similares) SON PROPIOS DE CONTROLADOR API
        public async Task<IEnumerable<UsuarioDTO>?> ObtenerTodosLosUsuarios() //Para las colecciones IEnumerable no se maneja ? para los objetos
        {
            var UsuariosCompletos = await _ContextHeredado.Usuarios.ToListAsync();
            if (UsuariosCompletos.Count > 0)
            {
                var UsuariosDTO = Mapeador.Map<IEnumerable<UsuarioDTO>>(UsuariosCompletos); //Si, los Mappers pueden mapear una lista entera, usando IEnumerable como interfaz
                //Algo a recalcar: Uso la sintaxis <IEnumerable<UsuarioDTO>> por comodidad. A nivel de procesos, NET genera un instanciamiento de una lista (pero sin los permisos de edicion de una lista directamente instancianda con <List<), NET genera una lista porque es la estructura de datos mas sencilla de instanciar y manipular. Instanciando un objeto DTO para cada objeto completo para finalmente agregar los DTO a esa nueva lista con solo permisos de lectura.
                //Otra cosa a aclarar: LA ASINCRONIA NO ES CONTAGIOSA. Es decir, que si para obtener la informacion debo manejar un metodo asincrono, para mapearlos (con esa misma informacion que obtuve de forma asicrona) NO NECESITO METODO ASINCRONO, la informacion ya esta ahi. El hecho de que la haya obtenido de forma asincrona no significa que el metodo que la use deba ser asicrono. Es metodo asincrono si necesita consultar informacion en la DB o alterarla, de resto, NO ES ASINCRONO.
                return UsuariosDTO;
            }
            return null;
        }

        public async Task<UsuarioDTO?> ObtenerUsuarioPorId(long id)
        {
            var UsuarioBuscado = await _ContextHeredado.Usuarios.FindAsync(id);
            if (UsuarioBuscado != null)
            {
                var UsuarioBuscadoDTO = Mapeador.Map<UsuarioDTO>(UsuarioBuscado);

                return UsuarioBuscadoDTO;
            }

            return null;
        }

        public async Task<UsuarioDTO> CrearUsuario(UsuarioDTO usuarioDTO)
        {
            bool CorreoDuplicado = await _ContextHeredado.Usuarios.AnyAsync(ObjetoActualEnTablaUsuarios => ObjetoActualEnTablaUsuarios.CorreoElectronico == usuarioDTO.CorreoElectronicoDTO);
            bool AliasDuplicado = await _ContextHeredado.Usuarios.AnyAsync(ObjetoActualEnTablaUsuarios => ObjetoActualEnTablaUsuarios.AliasUsuario == usuarioDTO.AliasUsuarioDTO);

            if (AliasDuplicado==true && CorreoDuplicado==true)
            {
                throw new InvalidOperationException("El correo y el Alias ya han sido usados. Seleccione nuevos");

            }
            else if (CorreoDuplicado== true || AliasDuplicado==true)
            {
                throw new InvalidOperationException("El correo o el Alias ya han sido usados");
            }

            var NuevoUsuario = Mapeador.Map<Usuario>(usuarioDTO);
            _ContextHeredado.Usuarios.Add(NuevoUsuario);
     
            try
            {
                await _ContextHeredado.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Error al actualizar la base de datos");   //Algo a aclarar, Cuando se realiza una instacia, en realidad lo que tenemos es un tipo de referencia. Es decir, que Cuando asignamos un objeto a una variable o lo pasamos como argumento a un método, no estamos copiando el objeto en sí, sino creando un puntero que apunta a la misma dirección dentro de la memoria Heap.
                //No hay necesidad de consultar nuevamente a la base de datos por el objeto nuevo, ya que el valor de referencia ha sido actualizado y el puntero al seguir en memoria, simplemennte se mapea de nuevo.
            }

            var NuevoUsuarioDTO = Mapeador.Map<UsuarioDTO>(NuevoUsuario);

            return NuevoUsuarioDTO;
        }
        //Login
        public async Task<string> LoginUsuario(UsuarioDTO usuarioDTO)
        {
            var UsuarioExistente = await _ContextHeredado.Usuarios.FirstOrDefaultAsync(ObjetoActualEnTablaUsuarios => ObjetoActualEnTablaUsuarios.CorreoElectronico == usuarioDTO.CorreoElectronicoDTO);

            if (UsuarioExistente == null)
            {
                throw new InvalidOperationException("El usuario no existe en la base de datos");
            }

            var VerificarContrasena = BCrypt.Net.BCrypt.EnhancedVerify(usuarioDTO.PasswordDTO, UsuarioExistente.PasswordEncriptada);

            if (VerificarContrasena == false)
            {
                throw new InvalidOperationException("La contrasena no pudo ser verificada");
            }

            var TokenUsuario = _TokenUsuarioInterface.GenerarTokenUsuario(UsuarioExistente);

            return TokenUsuario; //Recordar para siempre, EL TOKEN ES DE TIPO STRING
        }

     
        public async Task EditarUsuarioPorId(long id, UsuarioDTO usuarioDTO)
        {
            if (_UsuarioClaimsVerificacion.Id_Usuario.HasValue)
            {
                long IdUsuarioClaim = _UsuarioClaimsVerificacion.Id_Usuario.Value;

                if(IdUsuarioClaim != id || usuarioDTO.Id_UserDTO != id)
                {
                    throw new InvalidOperationException("Los identificadores primarios no coinciden");
                }

            }
            else
            {
                throw new InvalidOperationException("No se pudo detectar un valor de Id");
            }

                var UsuarioEditar = await _ContextHeredado.Usuarios.FindAsync(id);

            if (UsuarioEditar == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            Mapeador.Map(usuarioDTO, UsuarioEditar);

            try
            {
                await _ContextHeredado.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Error al actualizar la base de datos");
            }
        }

        public async Task EliminarUsuarioPorId(long id)
        {
            if (_UsuarioClaimsVerificacion.Id_Usuario.HasValue)
            {
                long UsuarioIdClaim = _UsuarioClaimsVerificacion.Id_Usuario.Value;

                if(UsuarioIdClaim != id)
                {
                    throw new InvalidOperationException("Las Ids no coinciden");
                }
            }
            else
            {
                throw new InvalidOperationException("No se pudo obtener un valor de Id");
            }

                var UsuarioEliminar = await _ContextHeredado.Usuarios.FindAsync(id);

            if (UsuarioEliminar == null)
            {
                throw new InvalidOperationException("El usuario no existe");
            }

            _ContextHeredado.Usuarios.Remove(UsuarioEliminar); //Se selecciona la Tabla en la que se desea hacer el Remove (ojito con eso). Porque tanto los metodos de Remove y Add asignan etiquetas a objetos en memoria, SaveChangesAsync es quien se encarga de realizar los cambios que alteran la base de datos

            try
            {
                await _ContextHeredado.SaveChangesAsync(); //Se emplea SaveChangesAsync sin especificar las tablas, ya que estamos actualizando de forma general
            }
            catch (DbUpdateException) {

                throw new InvalidOperationException("No se pudo actualizar la base de datos. Error inesperado");
            }
        }
    }
}