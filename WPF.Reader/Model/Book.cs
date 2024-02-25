using ASP.Server.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF.Reader.Model
{
  
    public class Book
    {
        public int Id { get; set; }

        public String Nom { get; set; }

        public double Prix { get; set; }

        public String Contenu { get; set; }

        public ICollection<Auteur> Auteur { get; set; } = new List<Auteur>();
        public ICollection<Genre> Genre { get; set; } = new List<Genre>();

    }
}
