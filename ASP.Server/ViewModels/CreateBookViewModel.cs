using ASP.Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASP.Server.ViewModels
{
    public class CreateBookViewModel
    {
        [Required(ErrorMessage = "Le nom du livre est requis.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prix doit être supérieur à 0.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0.")]
        public double Prix { get; set; }

        [Required(ErrorMessage = "Le contenu du livre est requis.")]
        public string Contenu { get; set; }

        [Required(ErrorMessage = "Au moins un genre est requis."), MinLength(1)]
        public IEnumerable<int> SelectedGenreIds { get; set; }

        public IEnumerable<Genre> AvailableGenres { get; set; }

        [Required(ErrorMessage = "Au moins un auteur est requis."), MinLength(1)]
        public List<int> SelectedAuteurIds { get; set; }

        public IEnumerable<Auteur> AvailableAuteurs { get; set; }
    }
}
