using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using Windows.Web.Http;
using WPF.Reader.Model;
using WPF.Reader.OpenApi;

namespace WPF.Reader.Service
{
    public class LibraryService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>() { };

        public LibraryService()
        {
            UpdateBooks();
        }

        public void UpdateBooks()
        {
            Task.Run(async () => await FetchBooksAsync());
        }

        public async Task FetchBookDetails(int bookId)
        {
            try
            {
                string url = $"https://localhost:5001/api/Book/GetBook/?id={bookId}";
                Uri uri = new Uri(url);
                var response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var book = JsonConvert.DeserializeObject<Book>(json);

                    var Book = Books.FirstOrDefault(b => b.Id == bookId);
                    if (Book != null)
                    {
                        Book.Nom = book.Nom;
                        Book.Prix = book.Prix;
                        Book.Contenu = book.Contenu;
                        Book.Genre = book.Genre;
                        Book.Auteur = book.Auteur;
                    }
                }
                else
                {
                    Console.WriteLine($"Erreur HTTP : {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task FetchBooksAsync()
        {
            try
            {
                string url = "https://localhost:5001/api/Book/GetBooks";
                Uri uri = new Uri(url);
                var response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var newBooks = JsonConvert.DeserializeObject<List<Book>>(json);

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Books.Clear();
                        foreach (var newBook in newBooks)
                        {
                            Books.Add(newBook);
                        }
                    });
                }
                else
                {
                    Console.WriteLine($"Erreur HTTP : {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}