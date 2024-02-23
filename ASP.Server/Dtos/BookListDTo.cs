using ASP.Server.Models;
using System.Collections.Generic;

namespace ASP.Server.Dtos
{
    public class BookListDTo
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public string Contenu { get; set; }
        public string AuteurNom { get; set; }
        
        public double Prix { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
    }
}
