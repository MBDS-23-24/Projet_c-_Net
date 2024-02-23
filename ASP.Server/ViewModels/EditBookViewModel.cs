using ASP.Server.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ASP.Server.ViewModels
{
    public class EditBookViewModel : CreateBookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Au moins un auteur est requis."), MinLength(1)]
        public new List<int> SelectedAuteurIds { get; set; } = new List<int>();

        public EditBookViewModel() { }

        public EditBookViewModel(Book book)
        {
            Id = book.Id;
            Nom = book.Nom;
            Prix = book.Prix;
            Contenu = book.Contenu;
            SelectedGenreIds = book.Genres.Select(g => g.Id).ToList();

            if (book.Auteurs != null)
            {
                SelectedAuteurIds = book.Auteurs.Select(a => a.Id).ToList();
            }
        }
    }
}
