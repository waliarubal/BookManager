using AvaloniaDemosntration.Base;
using AvaloniaDemosntration.Models;
using AvaloniaDemosntration.Services;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaDemosntration.ViewModels;
public class BooksViewModel: ViewModelBase
{
    IBookService _bookService;
    ObservableCollection<BookModel> _books;
    BookModel _book;
    ICommand _refresh, _add, _remove, _edit;

    public BooksViewModel()
    {
        _books = [];
    }

    #region properties

    public ObservableCollection<BookModel> Books => _books;

    public BookModel Book
    {
        get => _book;
        set => this.RaiseAndSetIfChanged(ref _book, value);
    }

    #endregion

    #region services

    public IBookService BookService
    {
        get
        {
            _bookService ??= GetService<IBookService>();
            return _bookService;
        }
    }

    #endregion

    #region commands

    public ICommand AddCommand
    {
        get
        {
            _add ??= ReactiveCommand.CreateFromTask(AddAction);
            return _add;
        }
    }

    public ICommand RemoveCommand
    {
        get
        {
            _remove ??= ReactiveCommand.CreateFromTask<BookModel>(RemoveAction);
            return _remove;
        }
    }

    public ICommand RefreshCommand
    {
        get
        {
            _refresh ??= ReactiveCommand.Create(RefreshAction);
            return _refresh;
        }
    }

    public ICommand EditCommand
    {
        get
        {
            _edit ??= ReactiveCommand.CreateFromTask<BookModel>(EditAction);
            return _edit;
        }
    }

    #endregion

    async Task AddAction()
    {
        Book = new();
        await EditAction(Book);
        await RefreshAction();
    }

    async Task RemoveAction(BookModel book)
    {
        IsBusy = true;
        if (await BookService.Remove(book))
            await RefreshAction();
        IsBusy = false;
    }

    async Task EditAction(BookModel book)
    {
        var viewModel = GetService<BookViewModel>();
        viewModel.Book = book;

        await ShowPopup(viewModel);
    }

    async Task RefreshAction()
    {
        IsBusy = true;
        Book = null;
        var books = await BookService.GetAll();
        Books.Clear();
        Books.AddRange(books);
        IsBusy = false;
    }
}
