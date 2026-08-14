using GameZone.Modelos.Empresa;
using GameZone.Modelos.Usuario;
using System.Security.Claims;

namespace GameZone.Interfaces__contrato_condicion_.InterfacesToken
{
    public interface InterfaceTokenService
    {

        string GenerarTokenUsuario(Usuario usuario);
        string GenerarTokenEmpresa(Empresa empresa);  //Para este caso, JWTConstructor es un metodo que se invoca dentro de ambos metodos dependiendo el caso, asi que ambos deben retornar el mismo tipo de dato (en este caso, JWT es un tipo String, ya que en el return se invoca a JWTConstructor, es decir, que ambos deben tener returns compatibles).
        string JWTConstructor(List<Claim> ClaimsObjeto);
    }
}
