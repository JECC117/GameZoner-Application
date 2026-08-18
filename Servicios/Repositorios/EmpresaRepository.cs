using GameZone.DatabaseContext;
using GameZone.Interfaces__contrato_condicion_.InterfacesRepositorios;
using GameZone.Modelos.Empresa;
using GameZone.Modelos.Usuario;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace GameZone.Servicios.Repositorios
{
    public class EmpresaRepository : InterfaceEmpresaRepositorio
    {

        private readonly GameZonerDBContext _context;
        public EmpresaRepository(GameZonerDBContext gameZonerDBContext)
        {
            _context = gameZonerDBContext;

        }

        public async Task<IEnumerable<Empresa>?> ObtenerTodasLasEmpresasRepo()
        {
            var EmpresasLista = await _context.Empresas.ToListAsync();
            IEnumerable<Empresa> Empresas = EmpresasLista;

            return Empresas;
        }

        public async Task<Empresa?> ObtenerEmpresaPorIdRepo(long id)
        {
            var EmpresaSeleccionada = await _context.Empresas.FindAsync(id);
            return EmpresaSeleccionada;
        }

        public void RegistrarEmpresaRepo(Empresa empresa)
        {
            _context.Empresas.Add(empresa);
        }

        public async Task<Empresa?> InicioSesionEmpresaRepo(EmpresaDTO empresaDTO)
        {
            var EmpresaExistente = await _context.Empresas.FirstOrDefaultAsync(ObjetoActualEnTabla => ObjetoActualEnTabla.Email_Empresa == empresaDTO.Email_EmpresaDTO);
            return EmpresaExistente;
        }

        public void EliminarPerfilEmpresaPorReferenciaRepo(Empresa empresa)
        {
            _context.Empresas.Remove(empresa); 
        }

        public async Task<bool> GuardarCambiosAsyncRepo()
        {
            int EsGuardado = await _context.SaveChangesAsync();
            if(EsGuardado > 0) {

                return true;
            }

            return false;
        }

        public async Task<bool> CorreoDuplicado(EmpresaDTO empresaDTO)
        {
            return await _context.Empresas.AnyAsync(ObjetoActualEnTablaUsuarios => ObjetoActualEnTablaUsuarios.Email_Empresa == empresaDTO.Email_EmpresaDTO);
        }

        public async Task<bool> NombreDuplicado(EmpresaDTO empresaDTO)
        {
            return await _context.Empresas.AnyAsync(ObjetoActualEnTablaUsuarios => ObjetoActualEnTablaUsuarios.Nombre_Empresa == empresaDTO.Nombre_EmpresaDTO);
        }


    }
}
