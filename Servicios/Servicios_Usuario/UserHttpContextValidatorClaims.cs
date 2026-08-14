using System.Security.Claims;
using GameZone.Interfaces__contrato_condicion_.InterfacesUsuario;



namespace GameZone.Servicios.Servicios_Usuario
{
    public class UserHttpContextValidatorClaims : InterfaceUsuarioClaims
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserHttpContextValidatorClaims(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;  //Cuando UseAuthentication valida mi Token, crea un HttpContext PARA ESE MISMO TOKEN, el cual lo capturo mediante este ContextAccessor. Listo para trabajar sobre ese Token que ahora llamamos "User" (OJO, ESTO EN EL CASO DE QUE EL USUARIO ESTE LOGUEADO)
        }   //Aclaracion: El HttpContext NO TIENE NADA QUE VER CON LOS OBJETOS O DATOS QUE YO ENVIE A UN ENDPOINT. El HttpContext.User genera claims unicamente si hay un Token. Si no hay token, sigue generando una instancia HttpContext.User pero vacia. Simplemente para aislar y que todos puedan trabajar sobre esa peticion en especifico

        //Metodos
        //Aclaracion, una propiedad de un objeto/token o lo que sea, en realidad tiene metodos default (get y set), pero puede añadirsen metodos personalizados
        public bool IsAutenticated
        {
            get
            {
                var context = _contextAccessor.HttpContext;
                if (context != null && context.User != null && context.User.Identity != null)
                {

                    return context.User.Identity.IsAuthenticated; //IsAutenticated NO es una propiedad definida por mí, es un booleano que ya estaba diseñado por Microsoft para la validación de identidad. Y lo utilizo al final (validando la identity de User).
                }
                else
                {
                    return false;
                }
            }

        }

        public long? Id_Usuario //El null-conditional operator (?) adjunto a un tipo de dato por valor (como int?, long? o bool?) convierte dicho tipo en un Nullable<T>. Permite que la variable guarde tanto sus valores numéricos/lógicos habituales como el estado especial null (ausencia de valor).
        {
            get
            {
                var context = _contextAccessor.HttpContext;

                if (context != null && context.User != null)
                {
                    var ClaimIdUsuario = context.User.FindFirst(ClaimTypes.NameIdentifier); //Al igual que como hacemos para una consulta en los controladores (_context.Usuarios.FindAsync...), NO HAY NECESIDAD DE ESPECIFICAR QUE QUIERO LOS CLAIMS, simplemente obtengo el primer objeto que cumpla con la condición.

                    if (ClaimIdUsuario != null)
                    {
                        bool EsParseableALong = long.TryParse(ClaimIdUsuario.Value, out long UsuarioID);

                        if (EsParseableALong== true)
                        {

                            return UsuarioID;
                        }
                        return null;
                    }
                    return null;
                }

                return null;
            }
        }

        public string? Nombre_Usuario
        {
            get
            {
                var context = _contextAccessor.HttpContext;
                if (context != null && context.User != null)
                {
                    var NombreUsuarioClaim = context.User.FindFirst(ClaimTypes.Name);
                    if (NombreUsuarioClaim != null)
                    {

                        return NombreUsuarioClaim.Value; //Dado que NombreUsuarioClaim me retorna UN OBJETO CLAIM, necesito únicamente extraer su valor 
                    }
                    return null;
                }

                return null;
            }
        }

        public string? Correo_Usuario
        {
            get
            {
                var context = _contextAccessor.HttpContext;
                if (context != null && context.User != null)
                {
                    var CorreoUsuarioClaim = context.User.FindFirst(ClaimTypes.Email);

                    if (CorreoUsuarioClaim != null)
                    {
                        return CorreoUsuarioClaim.Value;
                    }
                    return null;
                }

                return null;

            }
        }

        public string? Rol_Usuario
        {
            get
            {
                var context = _contextAccessor.HttpContext;
                if (context != null && context.User != null)
                {
                    var RolUsuarioClaim = context.User.FindFirst(ClaimTypes.Role);
                    if (RolUsuarioClaim != null)
                    {

                        return RolUsuarioClaim.Value;
                    }
                    return null;

                }

                return null;
            }
        }

        public bool TieneRolValido(string Rol)
        {
            var context= _contextAccessor.HttpContext;
            if(context != null && context.User != null)
            {
                return context.User.IsInRole(Rol); //Gracias a la función de IsInRole, en base al rol en forma de String que reciba la funcion "TieneRolValido", compara con el Claim que tenga el Rol y verifica si ese usuario tiene un rol valido
            }

            return false;
        }


    }
   
}
