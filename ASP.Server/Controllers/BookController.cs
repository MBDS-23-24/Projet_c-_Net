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
                                             .Include(b => b.Auteur) 
                                             .ToList();
            return View(listBooks);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CreateBookViewModel
            {
                AllGenres = _libraryDbContext.Genres.ToList(),
                AllAuteurs = _libraryDbContext.Auteurs.ToList() 
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateBookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Trouvez l'auteur par ID
                var auteur = _libraryDbContext.Auteurs.Find(viewModel.AuteurId);

                if (auteur == null)
                {
                    ModelState.AddModelError("AuteurId", "Auteur invalide.");
                    viewModel.AllGenres = _libraryDbContext.Genres.ToList();
                    viewModel.AllAuteurs = _libraryDbContext.Auteurs.ToList();
                    return View(viewModel);
                }

                var book = new Book
                {
                    Nom = viewModel.Nom,
                    Auteur = auteur, 
                    Prix = viewModel.Prix,
                    Contenu = viewModel.Contenu,
                    Genres = _libraryDbContext.Genres.Where(genre => viewModel.Genres.Contains(genre.Id)).ToList()
                };

                _libraryDbContext.Livres.Add(book);
                _libraryDbContext.SaveChanges();
                return RedirectToAction(nameof(List));
            }

            // Si le modèle n'est pas valide, retourne à la vue
            viewModel.AllGenres = _libraryDbContext.Genres.ToList();
            viewModel.AllAuteurs = _libraryDbContext.Auteurs.ToList();
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var book = _libraryDbContext.Livres
                                        .Include(b => b.Genres)
                                        .Include(b => b.Auteur)
                                        .FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new EditBookViewModel(book)
            {
                AllGenres = _libraryDbContext.Genres.ToList(),
                AllAuteurs = _libraryDbContext.Auteurs.ToList()
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
                    .Include(b => b.Auteur) 
                    .FirstOrDefaultAsync(b => b.Id == viewModel.Id);

                if (bookToUpdate == null)
                {
                    return NotFound();
                }

                // Mise à jour des propriétés de base
                bookToUpdate.Nom = viewModel.Nom;
                bookToUpdate.Prix = viewModel.Prix;
                bookToUpdate.Contenu = viewModel.Contenu;
                var auteur = await _libraryDbContext.Auteurs.FindAsync(viewModel.AuteurId);
                if (auteur != null)
                {
                    bookToUpdate.Auteur = auteur;
                }
                else
                {
                    ModelState.AddModelError("", "Auteur introuvable.");
                    return View(viewModel);
                }
                if (viewModel.Genres != null && viewModel.Genres.Any())
                {
                    var selectedGenres = await _libraryDbContext.Genres
                                                                 .Where(g => viewModel.Genres.Contains(g.Id))
                                                                 .ToListAsync();
                    bookToUpdate.Genres = selectedGenres;
                }

                await _libraryDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            viewModel.AllGenres = await _libraryDbContext.Genres.ToListAsync();
            viewModel.AllAuteurs = await _libraryDbContext.Auteurs.ToListAsync();
            return RedirectToAction("List");
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
                                               .Include(b => b.Auteur)
                                               .Include(b => b.Genres)
                                               .Where(b => !selectedAuteurId.HasValue || b.Auteur.Id == selectedAuteurId)
                                               .Where(b => !selectedGenreIds.Any() || b.Genres.Any(g => selectedGenreIds.Contains(g.Id)))
                                               .ProjectTo<BookListDTo>(_mapper.ConfigurationProvider)
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
                .Include(livre => livre.Auteur)
                .GroupBy(livre => livre.Auteur.Nom)
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
