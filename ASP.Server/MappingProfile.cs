﻿using ASP.Server.Database;
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
            // Rajouter autant de ligne ici que vous avez de mapping Model <-> DTO
            // https://docs.automapper.org/en/latest/
            CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuteurNom, opt => opt.MapFrom(src => src.Auteur.Nom));
            CreateMap<Genre, GenreDTo>();
            CreateMap<Book, BookListDTo>()
            .ForMember(dest => dest.AuteurNom, opt => opt.MapFrom(src => src.Auteur.Nom))
            .ForMember(dto => dto.Genres, opt => opt.MapFrom(book => book.Genres.Select(g => g.Nom)));
        }
    }
}
