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
                var LionelVincent = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Lionel Vincent");
                var monExCopine = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Mon Ex copine");
                var elodieBantos = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Elodie Bantos");
                var douniaZoubid = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Dounia Zoubid");
                var yehoudiVincent = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Yehoudi Vincent");
                var paulMirabel = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Paul Mirabel");


                var books = new List<Book>
                {
                    new Book { Nom = "Foundation",Prix = 15.99, Contenu = "Premier livre de la série Foundation.",Genres = new List<Genre> { SF },Auteurs = new List<Auteur> { LionelVincent }},  
                    new Book { Nom = "Dune",Prix = 19.99, Contenu = "Premier livre de la série Dune.",Genres = new List<Genre> { SF },Auteurs = new List<Auteur> { monExCopine }},
                    new Book { Nom = "Le Seigneur des Anneaux",Prix = 25.99, Contenu = "Premier livre de la série Le Seigneur des Anneaux.",Genres = new List<Genre> { Fantasy },Auteurs = new List<Auteur> { elodieBantos }},  
                    new Book { Nom = "Le Hobbit",Prix = 12.99, Contenu = "Premier livre de la série Le Hobbit.",Genres = new List<Genre> { Fantasy },Auteurs = new List<Auteur> { douniaZoubid }},  
                    new Book { Nom = "Le Nom du Vent",Prix = 18.99, Contenu = "Premier livre de la série Le Nom du Vent.",Genres = new List<Genre> { Fantasy },Auteurs = new List<Auteur> { yehoudiVincent }},  
                    new Book { Nom = "Le Trône de Fer",Prix = 22.99, Contenu = "Premier livre de la série Le Trône de Fer.",Genres = new List<Genre> { Fantasy },Auteurs = new List<Auteur> { paulMirabel }},   
                    new Book { Nom = "Le Trône de Fer",Prix = 22.99, Contenu = "Premier livre de la série Le Trône de Fer.",Genres = new List<Genre> { Fantasy },Auteurs = new List<Auteur> { paulMirabel }},   
                };

                bookDbContext.Livres.AddRange(books);
                bookDbContext.SaveChanges();
            }
        }
    }
}
