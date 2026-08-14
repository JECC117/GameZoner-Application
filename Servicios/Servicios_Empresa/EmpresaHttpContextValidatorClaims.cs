using GameZone.Interfaces__contrato_condicion_.InterfacesEmpresa;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Superpower;
using System.Security.Claims;

namespace GameZone.Servicios.Servicios_Empresa
{
    public class EmpresaHttpContextValidatorClaims : InterfaceEmpresaValidarClaims
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmpresaHttpContextValidatorClaims(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long? Id_Empresa
        {
            get
            {
                var context= _httpContextAccessor.HttpContext;

                if (context != null && context.User != null)
                {
                    var IdEmpresaClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                    if(IdEmpresaClaim != null)
                    {
                        bool IdParseableLong = long.TryParse(IdEmpresaClaim.Value, out long IdEmpresaClaimExistente); //Retorna en una variable de tipo long mediante out long IdEmpresaClaimExistente. Cuando son Claims y se desea hacer algo con su valor, siempre se maneja el .Value con la variable que almacene el Claim
                        if( IdParseableLong == true)
                        {
                            return IdEmpresaClaimExistente;
                        }
                            
                    }
                    return null;
                    
                }

                return null;
            }
        }

        public string? Email_Empresa { 
            get
            {
                var context = _httpContextAccessor.HttpContext; 
                if(context != null && context.User != null)
                {
                    var EmpresaEmailClaim = context.User.FindFirst(ClaimTypes.Email);
                    if (EmpresaEmailClaim != null)
                    {
                        return EmpresaEmailClaim.Value;
                    }
                    return null;
                }
                return null;
            } 
        }

        public string? Name_Empresa
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if( context != null && context.User != null)
                {
                    var EmpresaClaimNombre= context.User.FindFirst(ClaimTypes.Name);
                    if(EmpresaClaimNombre != null)
                    {
                        return EmpresaClaimNombre.Value;
                    }

                    return null;
                }
                return null;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if(context != null && context.User != null && context.User.Identity != null)  //Valida que solo retorne true si context.User.Identity NO SON NULOS (Es decir, si genuinamente hay una identidad construida en el HttpContext)
                {
                    return context.User.Identity.IsAuthenticated;
                }
                return false;
            }
        }

        public bool RolValido(string Rol)
        {
            var context= _httpContextAccessor.HttpContext;
            if(context != null && context.User != null)
            {
                return context.User.IsInRole(Rol);
            }
            return false;
        }
    }
}
