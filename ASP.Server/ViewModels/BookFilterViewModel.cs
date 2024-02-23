using ASP.Server.Dtos;
using ASP.Server.Models;
using System.Collections.Generic;

namespace ASP.Server.ViewModels
{
    public class BookFilterViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Auteur> AvailableAuteurs { get; set; }
        public IEnumerable<Genre> AvailableGenres { get; set; }
        public int? SelectedAuteurId { get; set; }
        public List<int> SelectedGenreIds { get; set; } = new List<int>();
    }
}
