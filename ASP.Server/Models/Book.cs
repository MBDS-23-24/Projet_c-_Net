using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ASP.Server.Models
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du livre est requis.")]
        public string Nom { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0.")]
        public double Prix { get; set; }

        [Required(ErrorMessage = "Le contenu du livre est requis.")]
        public string Contenu { get; set; }

        public ICollection<Auteur> Auteurs { get; set; } = new List<Auteur>();
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

        [NotMapped]
        public int WordCount
        {
            get { return !string.IsNullOrEmpty(Contenu) ? Contenu.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length : 0; }
        }
    }
}
