using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;
using System.Linq;
using ASP.Server.Models;
using ASP.Server.ViewModels;
using System.Collections.Generic;

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
    }
}
