using AutoMapper;
using GameZone.Modelos.Empresa;

namespace GameZone.Mapper
{
    public class EmpresaProfile : Profile
    {
        public EmpresaProfile()
        {
            //Mapear de Empresa a EmpresaDTO
            CreateMap<Empresa, EmpresaDTO>()
                .ForMember(dest => dest.Id_EmpresaDTO, opt => opt.MapFrom(src => src.Id_Empresa))
                .ForMember(dest => dest.Nombre_EmpresaDTO, opt => opt.MapFrom(src => src.Nombre_Empresa))
                .ForMember(dest => dest.Email_EmpresaDTO, opt => opt.MapFrom(src => src.Email_Empresa))
                .ForMember(dest => dest.Tipo_EmpresaDTO, opt => opt.MapFrom(src => src.Tipo_Empresa))
                .ForMember(dest => dest.FechaRegistroEmpresa_DTO, opt => opt.MapFrom(src => src.FechaRegistro_Empresa));


            CreateMap<EmpresaDTO, Empresa>()
                .ForMember(dest => dest.Nombre_Empresa, opt => opt.MapFrom(src => src.Nombre_EmpresaDTO))
                .ForMember(dest => dest.Email_Empresa, opt => opt.MapFrom(src => src.Email_EmpresaDTO))
                .ForMember(dest => dest.Tipo_Empresa, opt => opt.MapFrom(src => src.Tipo_EmpresaDTO))
                .ForMember(dest => dest.PasswordEncriptada_Empresa, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.EnhancedHashPassword(src.Password_EmpresaDTO))); //La funcion lambda abarca a todo el bloque de BCrypt, asi que despues de que src logre obtener el valor, Bcrypt se encargara de encriptarlo y enviarlo a destiny




        }
    }
}


// .ForMember(dest => dest.Id_UserDTO, opt => opt.MapFrom(src => src.Id_User))