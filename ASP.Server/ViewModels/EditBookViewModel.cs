using ASP.Server.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ASP.Server.ViewModels
{
    public class EditBookViewModel : CreateBookViewModel
    {
        public int Id { get; set; }


        public EditBookViewModel() { }

        public EditBookViewModel(Book book)
        {
            Id = book.Id;
            Nom = book.Nom;
            AuteurId = book.Auteur?.Id ?? 0; 
            Prix = book.Prix;
            Contenu = book.Contenu;
            Genres = book.Genres.Select(g => g.Id);
        }
    }
}
