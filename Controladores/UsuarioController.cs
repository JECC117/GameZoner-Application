using Microsoft.AspNetCore.Mvc;
using GameZone.Interfaces__contrato_condicion_.InterfacesUsuario;
using GameZone.Modelos.Usuario;
using Microsoft.AspNetCore.Authorization;
using GameZone.Modelos.Empresa;


namespace GameZone.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly InterfaceUsuarioController _interfaceUsuarioController;
        //NO SE MANEJA IUSERREPOSITORY DADO QUE ESTOY MANEJANDO TODO A TRAVES DE DBCONTEXT, REPOSITORY ES UNA ALTERNATIVA MAS PROFESIONAL (ACTUA COMO INTERMEDIARIO ENTRE EL CONTROLADOR Y DBCONTEXT).
        //Constructor para inyector de dependencias
        public UsuarioController(InterfaceUsuarioController interfaceUsuarioController)
        {
            _interfaceUsuarioController = interfaceUsuarioController;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>?>> ObtenerTodosLosUsuarios()
        {
            var Usuarios= await _interfaceUsuarioController.ObtenerTodosLosUsuarios(); //Dado que el metodo interno (el que usa la interfaz) es asincrono, su invocacion debe serlo tambien
            if (Usuarios != null)
            {
                return Ok(Usuarios);
            }
            return BadRequest();
            
        }

        [HttpGet("perfil/{id}")]
        public async Task<ActionResult<UsuarioDTO?>> ObtenerUsuarioPorId(long id)
        {
            var UsuarioSeleccionado = await _interfaceUsuarioController.ObtenerUsuarioPorId(id);
            if( UsuarioSeleccionado == null)
            {
                return NotFound();
            }
            return Ok(UsuarioSeleccionado);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> CrearUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                var NuevoUsuario = await _interfaceUsuarioController.CrearUsuario(usuarioDTO);
                return CreatedAtAction(nameof(ObtenerUsuarioPorId), new { id = NuevoUsuario.Id_UserDTO }, NuevoUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("EditarUsuario/{id}")]
        [Authorize(Policy = "AccesoGlobalLogueado")]
        public async Task<IActionResult> EditarUsuarioPorId(long id, UsuarioDTO usuarioDTO)
        {
            try
            {
                await _interfaceUsuarioController.EditarUsuarioPorId(id, usuarioDTO);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("EliminarUsuario/{id}")]
        [Authorize(Policy = "AccesoGlobalLogueado")]
        public async Task<IActionResult> EliminarUsuarioPorId(long id)
        {
            try {
                await _interfaceUsuarioController.EliminarUsuarioPorId(id);
            
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
    }
}