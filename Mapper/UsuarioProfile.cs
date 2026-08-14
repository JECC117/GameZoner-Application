using AutoMapper;
using BCrypt.Net;
using GameZone.Modelos.Usuario;


namespace GameZone.Mapper

{
    public class UsuarioProfile : Profile
    {

        //Metodo para el mapper
        public UsuarioProfile() {

            //Mapear de Usuario completo a usuario DTO
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.Id_UserDTO, opt => opt.MapFrom(src => src.Id_User))
                .ForMember(dest => dest.AliasUsuarioDTO, opt => opt.MapFrom(src => src.AliasUsuario))
                .ForMember(dest => dest.CorreoElectronicoDTO, opt => opt.MapFrom(src => src.CorreoElectronico))
                .ForMember(dest => dest.RolDTO, opt => opt.MapFrom(src => src.Rol))
                .ForMember(dest => dest.FechaRegistro_DTO, opt => opt.MapFrom(src => src.FechaRegistro));

            //Mapear de UsuarioDTO a Usuario Completo
            CreateMap<UsuarioDTO, Usuario>()
               .ForMember(dest => dest.Id_User, opt => opt.MapFrom(src => src.Id_UserDTO))
               .ForMember(dest => dest.AliasUsuario, opt => opt.MapFrom(src => src.AliasUsuarioDTO))
               .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.CorreoElectronicoDTO))
               .ForMember(dest => dest.PasswordEncriptada, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.EnhancedHashPassword(src.PasswordDTO)))
               .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.RolDTO));
             


        }
    }
}
