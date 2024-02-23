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
                    new Auteur { Nom = "Yehoudi Vincent" },
                    new Auteur { Nom = "Cyril Tisserand" },
                    new Auteur { Nom = "Elodie Bantos" },
                    new Auteur { Nom = "Dounia Zoubid" },
                    new Auteur { Nom = "Abdenour Achouri" },
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
                var yehoudiVincent = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Yehoudi Vincent");
                var cyrilTisserand = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Cyril Tisserand");
                var elodieBantos = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Elodie Bantos");
                var douniaZoubid = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Dounia Zoubid");
                var abdenourAchouri = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Abdenour Achouri");
                var paulMirabel = bookDbContext.Auteurs.FirstOrDefault(a => a.Nom == "Paul Mirabel");


                var books = new List<Book>
                {
                    new Book { Nom = "Foundation",Prix = 15.99, Contenu = "Dans Fondation d'Isaac Asimov, le psychohistorien Hari Seldon fait une prédiction : dans cinq cents ans, l'Empire Galactique s'effondrera et laissera place à 30.000 ans de guerre et de barbarie, mais, selon lui, il est possible de réduire cette période en un seul millénaire.",Genres = new List<Genre> { Classic },Auteurs = new List<Auteur> { yehoudiVincent }},  
                    new Book { Nom = "Dune",Prix = 19.99, Contenu = "L'histoire se déroule dans un empire interstellaire féodal, où plusieurs maisons s'affrontent pour le contrôle de la planète Arrakis, également baptisée Dune par ses habitants. Bien que la planète soit un désert inhospitalier, elle est la seule source d'épice gériatrique, drogue nécessaire à la navigation spatiale.",Genres = new List<Genre> { Horror },Auteurs = new List<Auteur> { cyrilTisserand }},
                    new Book { Nom = "Le Seigneur des Anneaux",Prix = 25.99, Contenu = "Dans les vertes prairies du Comté, les Hobbits, ou Demi-Hommes, vivaient en paix... jusqu'au jour fatal où l'un d'entre eux, au cours de ses voyages, entra en possession de l'Anneau Unique aux immenses pouvoirs. Pour le reconquérir, Sauron, le Seigneur Sombre, va déchaîner toutes les forces du Mal... Frodo, le Porteur de l'Anneau, Gandalf, le magicien, et leurs intrépides compagnons réussiront-ils à écarter la menace qui pèse sur la Terre du Milieu ?",Genres = new List<Genre> { SF },Auteurs = new List<Auteur> { elodieBantos }},  
                    new Book { Nom = "Le Hobbit",Prix = 12.99, Contenu = "Bilbon Sacquet, un hobbit respectable et tranquille, coule des jours paisibles dans la Comté jusqu'au moment où un magicien, Gandalf « le Gris », le tire de son quotidien pour participer à une expédition lointaine avec des Nains, pour sortir un trésor des griffes du dragon Smaug.",Genres = new List<Genre> { Fantasy },Auteurs = new List<Auteur> { douniaZoubid }},  
                    new Book { Nom = "Le Nom du Vent",Prix = 18.99, Contenu = "e nom du vent est le premier de ces jours de narration, et retrace la jeunesse de Kvothe de son enfance dans la caravane de ses parents à ses années à l'université où il est entré à un âge précoce, en passant par la pauvreté et la faim. Mais Kvothe, on s'en rend vite compte, est un génie.",Genres = new List<Genre> { Romance },Auteurs = new List<Auteur> { yehoudiVincent }},  
                    new Book { Nom = "Le Trône de Fer",Prix = 22.99, Contenu = "Pendant près de trois siècles, les rois de la dynastie targaryenne ont régné sur le royaume des Sept Couronnes, mais la folie du roi Aerys II Targaryen et l'enlèvement par son fils de la jeune Lyanna Stark ont plongé le royaume dans une guerre civile qui mit fin au règne séculaire des rois dragons.",Genres = new List<Genre> { Thriller },Auteurs = new List<Auteur> { paulMirabel }}, 
                    new Book { Nom = "Harry Potter",Prix = 16.99, Contenu = " Harry Potter est un garçon ordinaire. Mais, le jour de ses onze ans, son existence bascule : un géant vient le chercher pour l'emmener dans une école de sorciers. Voler à cheval sur des balais, jeter des sorts, combattre les Trolls : Harry Potter se révèle être un sorcier vraiment doué.",Genres = new List<Genre> { SF },Auteurs = new List<Auteur> { abdenourAchouri }},
                };

                bookDbContext.Livres.AddRange(books);
                bookDbContext.SaveChanges();
            }
        }
    }
}
