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

namespace ASP.Server.Controllers
{
    public class BookController : Controller
    {
        private readonly LibraryDbContext _libraryDbContext;

        public BookController(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
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
    }
}
