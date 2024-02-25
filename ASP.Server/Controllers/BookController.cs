using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;
using System.Linq;
using ASP.Server.Models;
using ASP.Server.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Net.Security;
using ASP.Server.Dtos;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace ASP.Server.Controllers
{
    public class BookController : Controller
    {
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IMapper _mapper;

        public BookController(LibraryDbContext libraryDbContext, IMapper mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
        }

        public ActionResult<IEnumerable<Book>> List()
        {
            var listBooks = _libraryDbContext.Livres
                                             .Include(b => b.Genres)
                                             .Include(b => b.Auteurs) 
                                             .ToList();
            return View(listBooks);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CreateBookViewModel
            {
                AvailableGenres = _libraryDbContext.Genres.ToList(),
                AvailableAuteurs = _libraryDbContext.Auteurs.ToList() 
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateBookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Nom = viewModel.Nom,
                    Prix = viewModel.Prix,
                    Contenu = viewModel.Contenu,
                    Genres = _libraryDbContext.Genres.Where(genre => viewModel.SelectedGenreIds.Contains(genre.Id)).ToList(),
                    Auteurs = _libraryDbContext.Auteurs.Where(auteur => viewModel.SelectedAuteurIds.Contains(auteur.Id)).ToList()
                };

                _libraryDbContext.Livres.Add(book);
                _libraryDbContext.SaveChanges();
                return RedirectToAction(nameof(List));
            }

            viewModel.AvailableGenres = _libraryDbContext.Genres.ToList();
            viewModel.AvailableAuteurs = _libraryDbContext.Auteurs.ToList();
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var book = _libraryDbContext.Livres
                                        .Include(b => b.Genres)
                                        .Include(b => b.Auteurs)
                                        .FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new EditBookViewModel
            {
                Id = book.Id,
                Nom = book.Nom,
                SelectedAuteurIds = book.Auteurs.Select(a => a.Id).ToList(),
                AvailableAuteurs = _libraryDbContext.Auteurs.ToList(),
                Prix = book.Prix,
                //Pour conserver le prix initial du livre lorsqu'il est modifié
                PrixInitial = book.Prix, 
                Contenu = book.Contenu, 
                //Pour conserver le contenu initial du livre lorsqu'il est modifié
                ContenuInitial = book.Contenu,
                SelectedGenreIds = book.Genres.Select(g => g.Id).ToList(),
                AvailableGenres = _libraryDbContext.Genres.ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var bookToUpdate = await _libraryDbContext.Livres
                    .Include(b => b.Genres)
                    .Include(b => b.Auteurs)
                    .FirstOrDefaultAsync(b => b.Id == viewModel.Id);

                if (bookToUpdate == null)
                {
                    return NotFound();
                }

                bookToUpdate.Nom = viewModel.Nom;
                bookToUpdate.Prix = viewModel.Prix;
                bookToUpdate.Contenu = viewModel.Contenu;
                bookToUpdate.Auteurs.Clear();
                var selectedAuteurs = await _libraryDbContext.Auteurs
                                                             .Where(auteur => viewModel.SelectedAuteurIds.Contains(auteur.Id))
                                                             .ToListAsync();
                foreach (var auteur in selectedAuteurs)
                {
                    bookToUpdate.Auteurs.Add(auteur);
                }

                bookToUpdate.Genres.Clear();
                var selectedGenres = await _libraryDbContext.Genres
                                                            .Where(genre => viewModel.SelectedGenreIds.Contains(genre.Id))
                                                            .ToListAsync();
                foreach (var genre in selectedGenres)
                {
                    bookToUpdate.Genres.Add(genre);
                }

                await _libraryDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }

            viewModel.AvailableGenres = await _libraryDbContext.Genres.ToListAsync();
            viewModel.AvailableAuteurs = await _libraryDbContext.Auteurs.ToListAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var bookToDelete = _libraryDbContext.Livres.FirstOrDefault(b => b.Id == id);

            if (bookToDelete == null)
            {
                return NotFound(); 
            }

            _libraryDbContext.Livres.Remove(bookToDelete);
            _libraryDbContext.SaveChanges(); 
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int? selectedAuteurId, [FromQuery] List<int> selectedGenreIds)
        {
            var viewModel = new BookFilterViewModel
            {
               Books = await _libraryDbContext.Livres
                                               .Include(b => b.Auteurs)
                                               .Include(b => b.Genres)
                                               .Where(b => !selectedAuteurId.HasValue || b.Auteurs.Any(a => a.Id == selectedAuteurId)).Where(b => !selectedGenreIds.Any() || b.Genres.Any(g => selectedGenreIds.Contains(g.Id)))
                                               .ToListAsync(),
                AvailableAuteurs = await _libraryDbContext.Auteurs.ToListAsync(),
                AvailableGenres = await _libraryDbContext.Genres.ToListAsync(),
                SelectedAuteurId = selectedAuteurId,
                SelectedGenreIds = selectedGenreIds
            };

            return View(viewModel); 
        }

        public async Task<IActionResult> Statistics()
        {
            // Obtention du nombre total de livres
            int totalBooks = await _libraryDbContext.Livres.CountAsync();

    // Obtention du nombre de livres par auteur
    var booksPerAuthor = await _libraryDbContext.Livres
        .SelectMany(livre => livre.Auteurs.Select(auteur => new { livre, auteur.Nom }))
        .GroupBy(la => la.Nom)
        .Select(group => new { Author = group.Key, Count = group.Count() })
        .ToDictionaryAsync(g => g.Author, g => g.Count);

            // Obtention des statistiques sur le nombre de mots
            var wordCounts = await _libraryDbContext.Livres
                .Select(livre => livre.Contenu.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length)
                .ToListAsync();

            var maxWords = wordCounts.Max();
            var minWords = wordCounts.Min();
            var averageWords = wordCounts.Average();

            // Calcul de la médiane
            var orderedWordCounts = wordCounts.OrderBy(x => x).ToList();
            int middleIndex = orderedWordCounts.Count / 2;
            var medianWords = orderedWordCounts.Count % 2 != 0
                ? orderedWordCounts[middleIndex]
                : (orderedWordCounts[middleIndex - 1] + orderedWordCounts[middleIndex]) / 2.0;

            // Construction du viewModel avec toutes les statistiques
            var viewModel = new StatisticsViewModel
            {
                TotalBooks = totalBooks,
                BooksPerAuthor = booksPerAuthor,
                MaxWords = maxWords,
                MinWords = minWords,
                AverageWords = averageWords,
                MedianWords = medianWords
            };

            return View(viewModel);
        }





    }
}
