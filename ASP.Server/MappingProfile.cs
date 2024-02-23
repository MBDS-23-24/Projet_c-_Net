using ASP.Server.Database;
using ASP.Server.Dtos;
using ASP.Server.Models;
using ASP.Server.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace ASP.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.AuteurNom, opt => opt.MapFrom(src => src.Auteurs.Select(a => a.Nom).ToList()));
            CreateMap<Genre, GenreDTo>();
            CreateMap<Book, BookListDTo>()
                .ForMember(dest => dest.AuteurNom, opt => opt.MapFrom(src => src.Auteurs.FirstOrDefault().Nom))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Nom)));
        }
    }

}
