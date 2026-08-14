using GameZone.Interfaces__contrato_condicion_.InterfacesEmpresa;
using Microsoft.AspNetCore.Mvc;
using GameZone.Modelos.Empresa;
using GameZone.Modelos.Usuario;
using Microsoft.AspNetCore.Authorization;

namespace GameZone.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {

        private readonly InterfaceEmpresaController _interfaceEmpresaController;
        public EmpresaController(InterfaceEmpresaController interfaceEmpresaController)
        {

            _interfaceEmpresaController = interfaceEmpresaController;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpresaDTO>>> ObtenerTodasLasEmpresasPorId()
        {
            var ListaEmpresasDTO = await _interfaceEmpresaController.ObtenerTodasLasEmpresas();
            if (ListaEmpresasDTO != null)
            {

                return Ok(ListaEmpresasDTO);
            }
            return NotFound(); //Se utiliza not Found cuando el recurso es null
        }

        [HttpGet("PerfilEmpresa/{id}")]
        public async Task<ActionResult<EmpresaDTO?>> ObtenerEmpresaPorId(long id)
        {
            var EmpresaSeleccionada = await _interfaceEmpresaController.ObtenerEmpresaPorId(id);
            if (EmpresaSeleccionada==null)
            {
                return NotFound();
            }
            return Ok(EmpresaSeleccionada);
        }

        [HttpPost]
        public async Task<ActionResult<EmpresaDTO>> RegistrarEmpresa(EmpresaDTO empresaDTO)
        {
            try
            {
                var EmpresaRegistrada = await _interfaceEmpresaController.RegistrarEmpresa(empresaDTO); //Dado que en el metodo interno se definen bloques try, declaramos la variable que invoca el metodo y el return dentro del mismo bloque try (solamente de esa manera se puede acceder a ellos en la memoria), fuera del Try, mueren.
                                                                                                         //De esta manera, si ocurre un catch en el try/catch del metodo interno, salta directamente al catch en este modulo del controlador. Esto se denomina propagacion de errores

                return CreatedAtAction(nameof(ObtenerEmpresaPorId), new { id = EmpresaRegistrada.Id_EmpresaDTO }, EmpresaRegistrada);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("EditarEmpresa/{id}")]
        [Authorize(Policy = "AccesoGlobalLogueado")]
        public async Task<IActionResult> EditarPerfilEmpresaPorId(long id, EmpresaDTO empresaDTO)
        {
            try
            {
                 await _interfaceEmpresaController.EditarPerfilEmpresaPorId(id, empresaDTO);
                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        [HttpDelete("EliminarEmpresa/{id}")]
        [Authorize(Policy = "AccesoGlobalLogueado")]
        public async Task<IActionResult> EliminarPerfilEmpresaPorId(long id)
        {
            try
            {
                await _interfaceEmpresaController.EliminarPerfilEmpresaPorId(id);
                return NoContent(); //El return NoContent() SIGUE haciendo parte del método, eso quiere decir que, si algo salta dentro del catch interno del método y el catch del controlador lo atrapa, NO DEBE LLEGAR A RETURN NOCONTENT(). Por ende, return NoContent() NO puede ir afuera del Try

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
    }
}
