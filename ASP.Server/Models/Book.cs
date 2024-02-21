using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ASP.Server.Models
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du livre est requis.")]
        public String Nom { get; set; }

        [Required]
        public int AuteurId { get; set; }

        [ForeignKey("AuteurId")]
        public Auteur Auteur { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0.")]

        public double Prix { get; set; }

        [Required(ErrorMessage = "Le contenu du livre est requis.")]
        public String Contenu { get; set; } 
        public IEnumerable<Genre> Genres { get; set; }
        
        public Book() { 
            Genres = new List<Genre>();
        }   

    }
}