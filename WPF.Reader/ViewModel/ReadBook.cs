using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Reader.Model;
using WPF.Reader.Service;

namespace WPF.Reader.ViewModel
{
    class ReadBook : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Book CurrentBook { get; init; }

        private INavigationService _navigationService;


        public ReadBook(Book book) {
            _navigationService = Ioc.Default.GetService<INavigationService>();
            CurrentBook = book;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentBook)));
            Task.Run(async () =>
            {
                await Ioc.Default.GetService<LibraryService>().FetchBookDetails(CurrentBook.Id);

            });
        }

        public ICommand GoBackCommand => new RelayCommand(() =>
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
            }
        });


    }
    class InDesignReadBook() : ReadBook(new Book()) { }
}
