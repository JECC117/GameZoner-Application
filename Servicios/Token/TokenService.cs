using GameZone.Interfaces__contrato_condicion_.InterfacesToken;
using GameZone.Modelos.Empresa;
using GameZone.Modelos.Usuario;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameZone.Servicios.Token
{
    public class TokenService : InterfaceTokenService
    {
        private readonly IConfiguration _config;
        private readonly string? _SecretKey;
        private readonly string? _Issuer;
        private readonly string? _Audience;
        private readonly int _TokenLifetime;

        //Constructor para inicialiar las propiedades de la clase (OJO!!!, EN LOS CONSTRUCTORES SOLO SE PASAN INTERFACES COMO PARAMETROS, CLASES COMO JWTSECURITYTOKENHANDLER NO SON VALIDOS COMO PARÁMETROS YA QUE SON SIMPLES CLASES DE APOYO CON MÉTODOS ÚTILES)
        public TokenService(IConfiguration configuration) {

            _config= configuration;
            _SecretKey= _config["ApiSettings:SecretKey"];
            _TokenLifetime= _config.GetValue<int>("ApiSettings:TokenExpiracyMinutes");
            _Audience= _config["ApiSettings:Audience"];
            _Issuer= _config["ApiSettings:Issuer"];
        }

        public string GenerarTokenUsuario(Usuario usuario)  //Metodo de tipo String porque devuelve un token en formato string
        {
            var Claims = new List<Claim> {

                new Claim(ClaimTypes.NameIdentifier, usuario.Id_User.ToString()),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim(ClaimTypes.Name, usuario.AliasUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            return JWTConstructor(Claims); //El metodo debe ser de tipo String porque el return de invocar a este metodo DA UN TOKEN EN STRING (Esto aplica para cualquier metodo que invoque en return, tanto el tipo de metodo que invoco como el tipo de metodo en el que lo estoy invocando deben ser compatibles (no necesariamente iguales, pero si compatibles).
        }

        public string GenerarTokenEmpresa(Empresa empresa)
        {
            var Claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, empresa.Id_Empresa.ToString()),
                new Claim(ClaimTypes.Email, empresa.Email_Empresa),
                new Claim(ClaimTypes.Name, empresa.Nombre_Empresa),
                new Claim(ClaimTypes.Role, empresa.Tipo_Empresa)   //El tipo de empresa se utiliza para el Claim de tipo Role, el Rol_Desarrollo es una propiedad exclusiva de la tabla intermedia entre Empresa y Videojuego

            };

            return JWTConstructor(Claims);

        }

        public string JWTConstructor(List<Claim> ClaimsObjeto) {  //Se indica que recibe una lista de tipo claim llamada Claims

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_SecretKey)); //Generar una llave simetrica a partir de la clave secreta para firmar el token
            var credentials= new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature); //Generar las credenciales de firma a partir de la llave simetrica y el algoritmo de firma HMAC SHA512 (El proceso de firma se realiza 

            var Token = new JwtSecurityToken(
                issuer: _Issuer,
                audience: _Audience,
                claims: ClaimsObjeto,
                expires: DateTime.UtcNow.AddMinutes(_TokenLifetime),
                signingCredentials: credentials
                );

            //Instanciamiento de la clase encargada de Firmar
            var _jwtHandler= new JwtSecurityTokenHandler();
            return _jwtHandler.WriteToken(Token); //Generar el token en formato string y devolverlo
        }
    }
}
