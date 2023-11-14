using API.Dtos;
using AutoMapper;
using Dominio.Entities;

namespace API.profiles;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Rol,RolDto>().ReverseMap();
        CreateMap<Usuario,UsuarioDto>().ReverseMap();
    }
}