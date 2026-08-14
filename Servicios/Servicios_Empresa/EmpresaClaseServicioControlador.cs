using AutoMapper;
using GameZone.DatabaseContext;
using GameZone.Interfaces__contrato_condicion_.InterfacesEmpresa;
using GameZone.Servicios.Token;
using GameZone.Modelos.Empresa;
using Microsoft.EntityFrameworkCore;
using GameZone.Interfaces__contrato_condicion_.InterfacesToken;

namespace GameZone.Servicios.Servicios_Empresa
{
    public class EmpresaClaseServicioControlador : InterfaceEmpresaController
    {
        private readonly IMapper _mapper;
        private readonly GameZonerDBContext _dbContext;
        private readonly InterfaceEmpresaValidarClaims _interfaceEmpresaValidarClaims;
        private readonly InterfaceTokenService _tokenEmpresaInterface;

        public EmpresaClaseServicioControlador(InterfaceEmpresaValidarClaims interfaceEmpresaClaims, IMapper Mapper, GameZonerDBContext gameZonerDBContext, InterfaceTokenService tokenService)
        {
            _interfaceEmpresaValidarClaims=interfaceEmpresaClaims; //Pido la interfaz en el constructor (normalmente 
            _mapper=Mapper;
            _dbContext=gameZonerDBContext; //Inyeccion de dependencias (Context) para mi clase
            _tokenEmpresaInterface=tokenService; 

        }

        public async Task<IEnumerable<EmpresaDTO>?> ObtenerTodasLasEmpresas()
        {
            var EmpresasCompletas = await _dbContext.Empresas.ToListAsync();
            if (EmpresasCompletas.Count>0)
            {
                var EmpresasADTO = _mapper.Map<IEnumerable<EmpresaDTO>>(EmpresasCompletas);  //Recordar que necesito mapear una coleccion, IEnumerable es la opcion mas indicada por su compatibilidad
                return EmpresasADTO;
            }
            return null;
        }

        public async Task<EmpresaDTO?> ObtenerEmpresaPorId(long id)
        {
            var EmpresaSeleccionada= await _dbContext.Empresas.FindAsync(id);
            if (EmpresaSeleccionada==null)
            {
                return null;
            }
            var EmpresaSeleccionadaDTO= _mapper.Map<EmpresaDTO>(EmpresaSeleccionada);
            return EmpresaSeleccionadaDTO;
        }

        public async Task<EmpresaDTO> RegistrarEmpresa(EmpresaDTO empresaDTO)
        {
            bool NombreRepetido= await _dbContext.Empresas.AnyAsync(ObjetoActualEmpresa => ObjetoActualEmpresa.Nombre_Empresa== empresaDTO.Nombre_EmpresaDTO);
            bool EmailRepetido= await _dbContext.Empresas.AnyAsync(ObjetoActualEmpresa => ObjetoActualEmpresa.Email_Empresa== empresaDTO.Email_EmpresaDTO);

            if (EmailRepetido && NombreRepetido)
            {
                throw new InvalidOperationException("El Email y el Nombre ya han sido registrados. Por favor use datos distintos");
            }
            if (EmailRepetido || NombreRepetido) {

                throw new InvalidOperationException("El Email o El Nombre ya han sido registrados. Por favor use datos distintos");
            }

            var EmpresaRegistrar = _mapper.Map<Empresa>(empresaDTO);

            _dbContext.Empresas.Add(EmpresaRegistrar);

            try
            {
                await _dbContext.SaveChangesAsync();

            }catch (DbUpdateException)
            {
                throw new InvalidOperationException("No se pudo actualizar la base de datos");

            }

            var EmpresaRegistradaDTO = _mapper.Map<EmpresaDTO>(EmpresaRegistrar); //Dado que necesito el objeto con Id, debo guardar los cambios a nivel de base de datos para que esos cambios se efectuen sobre el objeto y asi poder retornar el DTO CON ID.

            return EmpresaRegistradaDTO;

        }
        //Login
        public async Task<string> InicioSesionEmpresa(EmpresaDTO empresaDTO)  //Recordar que un Token es un tipo string, asi que el modulo debe ser Task<string>
        {
            var EmpresaExistente = await _dbContext.Empresas.FirstOrDefaultAsync(ObjetoActualTabla => ObjetoActualTabla.Email_Empresa== empresaDTO.Email_EmpresaDTO);
            if (EmpresaExistente== null) {

                throw new InvalidOperationException("No se puede continuar con el proceso. Necesita registrarse");
            }
            bool ComparacionPassword = BCrypt.Net.BCrypt.EnhancedVerify(empresaDTO.Password_EmpresaDTO, EmpresaExistente.PasswordEncriptada_Empresa);
            if (ComparacionPassword==false)
            {
                throw new InvalidOperationException("Error, las passwords no coinciden");
            }

            var TokenEmpresa = _tokenEmpresaInterface.GenerarTokenEmpresa(EmpresaExistente); //Invocar a la instancia de clase y al metodo para pasarle de parametro (EmpresaExistente)

            return TokenEmpresa;
        }

        //Edicion
        public async Task EditarPerfilEmpresaPorId(long id, EmpresaDTO empresaDTO)
        {
            if (_interfaceEmpresaValidarClaims.Id_Empresa.HasValue)
            {
                var IdEmpresaClaim = _interfaceEmpresaValidarClaims.Id_Empresa;
                if (id != empresaDTO.Id_EmpresaDTO || id!= IdEmpresaClaim)
                {
                    throw new InvalidOperationException("Los identificadores primarios no coinciden. Error inesperado");
                }
            }
            var EmpresaEditar = await _dbContext.Empresas.FindAsync(id);

            if (EmpresaEditar == null)
            {
                throw new InvalidOperationException("No se pudo editar. La empresa NO existe");
            }

            _mapper.Map(empresaDTO, EmpresaEditar);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("No se pudo actualizar la base de datos");
            }
        }

        public async Task EliminarPerfilEmpresaPorId(long id)
        {

            if (_interfaceEmpresaValidarClaims.Id_Empresa.HasValue)
            {
                var IdEmpresaClaim = _interfaceEmpresaValidarClaims.Id_Empresa;
                if(id != IdEmpresaClaim)
                {
                    throw new InvalidOperationException("Los identificadores no coinciden");
                }
            }

            var EmpresaEliminar = await _dbContext.Empresas.FindAsync(id);

            if (EmpresaEliminar == null)
            {
                throw new InvalidOperationException("El perfil no pudo ser eliminado. No existe el registro");
            }

            _dbContext.Empresas.Remove(EmpresaEliminar);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Error al actualizar la base de datos");
            }

        }

    }
}
