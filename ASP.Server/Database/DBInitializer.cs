using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ASP.Server.Models;

namespace ASP.Server.Database
{
    public class DbInitializer
    {
        public static void Initialize(LibraryDbContext bookDbContext)
        {
            bookDbContext.Database.EnsureCreated();

            if (!bookDbContext.Genres.Any())
            {
                var genres = new List<Genre>
                {
                    new Genre { Nom = "Science-Fiction"},
                    new Genre { Nom = "Classique" },
                    new Genre { Nom = "Romance" },
                    new Genre { Nom = "Thriller" },
                    new Genre { Nom = "Fantasy" },
                    new Genre { Nom = "Horror" },
                };

                bookDbContext.Genres.AddRange(genres);
                bookDbContext.SaveChanges();
            }

            if (!bookDbContext.Auteurs.Any())
            {
                var auteurs = new List<Auteur>
                {
                    new Auteur { Nom = "Lionel Vincent" },
                    new Auteur { Nom = "Mon Ex copine" },
                    new Auteur { Nom = "Elodie Bantos" },
                    new Auteur { Nom = "Dounia Zoubid" },
                    new Auteur { Nom = "Yehoudi Vincent" },
                    new Auteur { Nom = "Paul Mirabel" },
                };

                bookDbContext.Auteurs.AddRange(auteurs);
                bookDbContext.SaveChanges();
            }

            // Initialisation des livres si nécessaire
            if (!bookDbContext.Livres.Any())
            {
                // Récupération des genres pour association
                var SF = bookDbContext.Genres.FirstOrDefault(g => g.Nom == "Science-Fiction");
                var Classic = bookDbContext.Genres.FirstOrDefault(g => g.Nom == "Classique");
                var Romance = bookDbContext.Genres.FirstOrDefault(g => g.Nom == "Romance");
                var Thriller = bookDbContext.Genres.FirstOrDefault(g => g.Nom == "Thriller");
                var Fantasy = bookDbContext.Genres.FirstOrDefault(g => g.Nom == "Fantasy");
                var Horror = bookDbContext.Genres.FirstOrDefault(g => g.Nom == "Horror");

                // Récupération des auteurs pour association
                var lionelVincent = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Lionel Vincent");
                var monExCopine = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Mon Ex copine");
                var elodieBantos = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Elodie Bantos");
                var douniaZoubid = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Dounia Zoubid");
                var yehoudiVincent = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Yehoudi Vincent");
                var paulMirabel = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Paul Mirabel");


                var books = new List<Book>
                {
                    new Book { Auteur = lionelVincent, Nom = "Foundation", Prix = 15.99, Contenu = "Premier livre de la série Foundation.", Genres = new List<Genre> { Classic } },
                    new Book { Auteur = lionelVincent, Nom = "Foundation and Empire", Prix = 12.99, Contenu = "Deuxième livre de la série Foundation.", Genres = new List<Genre> { Romance } },
                    new Book { Auteur = lionelVincent, Nom = "Second Foundation", Prix = 14.99, Contenu = "Troisième livre de la série Foundation.", Genres = new List<Genre> { SF } },
                    new Book { Auteur = monExCopine, Nom = "La vie après toi", Prix = 9.99, Contenu = "Un livre sur la vie après une rupture.", Genres = new List<Genre> { Romance } },
                    new Book { Auteur = monExCopine, Nom = "La vie après toi 2", Prix = 9.99, Contenu = "La suite du livre sur la vie après une rupture.", Genres = new List<Genre> { Thriller } },

                };

                bookDbContext.Livres.AddRange(books);
                bookDbContext.SaveChanges();
            }
        }
    }
}
