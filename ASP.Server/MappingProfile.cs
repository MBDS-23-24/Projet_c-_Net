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
            // Domain to Dto 
            CreateMap<Book, BookDto>();
            CreateMap<Auteur, AuteurDto>();
            CreateMap<Genre, GenreDTo>();
            CreateMap<Book, BookWithoutContentDto>();

        }
    }

}
