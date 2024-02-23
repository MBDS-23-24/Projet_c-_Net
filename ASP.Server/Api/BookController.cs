using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.Server.Database;
using ASP.Server.Models;
using AutoMapper;
using ASP.Server.Dtos;
using AutoMapper.QueryableExtensions;

namespace ASP.Server.Api
{

    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class BookController(LibraryDbContext libraryDbContext, IMapper mapper) : ControllerBase
    {
        private readonly LibraryDbContext libraryDbContext = libraryDbContext;
        private readonly IMapper mapper = mapper;

        // - GetBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookWithoutContentDto>>> GetBooks([FromQuery] int? genreId, [FromQuery] int limit = 10, [FromQuery] int offset = 0)
        {
            // Ici je verifie la validité pour  offset et limit
            if (offset < 0 || limit > 100)
            {
                return BadRequest("Offset must be >= 0 and limit must be <= 100.");
            }

            var booksQuery = libraryDbContext.Livres
                                             .Include(b => b.Genres)
                                             .Include(b => b.Auteurs)
                                             .AsEnumerable(); 

            if (genreId.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Genres.Any(g => g.Id == genreId.Value));
            }

            // Application de la pagination et du filtrage...
            var books = booksQuery.Skip(offset).Take(limit).ToList();

            // Mappage des livres vers BookListDTo
            var booksDtos = mapper.Map<List<BookWithoutContentDto>>(books);

            int totalBooks = booksQuery.Count();

            // Ici j'ajoute les headers de pagination
            int endIndex = Math.Min(offset + limit - 1, totalBooks - 1);
            Response.Headers.Append("Pagination", $"{offset}-{endIndex}/{totalBooks}");

            return Ok(booksDtos);
        }

        // - GetBook
        //   - Entrée: Id du livre

        [HttpGet]
        public ActionResult<BookDto> GetBook([FromQuery] int id)
        {
            var book = libraryDbContext.Livres
                .Include(b => b.Genres)
                .Include(b => b.Auteurs)
                .FirstOrDefault(b => b.Id == id);

            if (book != null)
            {
                return Ok(mapper.Map<BookDto>(book));
            }
            else
            {
                return NotFound();
            }
        }
 
        // - GetGenres
        //   - Entrée: Rien
        [HttpGet]
        public ActionResult<IEnumerable<GenreDTo>> GetGenres()
        {
            var genres = libraryDbContext.Genres
                .ProjectTo<GenreDTo>(mapper.ConfigurationProvider)
                .ToList();

            return Ok(genres);
        }
        
    }
}

