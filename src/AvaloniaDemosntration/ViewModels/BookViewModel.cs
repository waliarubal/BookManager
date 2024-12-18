using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaDemosntration.Base;
using AvaloniaDemosntration.Models;
using AvaloniaDemosntration.Services;
using ReactiveUI;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaDemosntration.ViewModels;
public class BookViewModel : ViewModelBase
{
    string _error;
    BookModel _book;
    ICommand _clear, _save, _addPage, _removePage;
    readonly IBookService _bookService;

    /// <summary>
    /// Designer support.
    /// </summary>
    public BookViewModel() { }

    public BookViewModel(IBookService bookService): this()
    {
        _bookService = bookService;
        _book = new();
    }

    #region properties

    public string Error
    {
        get => _error;
        private set => this.RaiseAndSetIfChanged(ref _error, value);
    }

    public BookModel Book
    {
        get => _book;
        set => this.RaiseAndSetIfChanged(ref _book, value);
    }

    #endregion

    #region commands

    public ICommand ClearCommand
    {
        get
        {
            _clear ??= ReactiveCommand.Create(ClearAction);
            return _clear;
        }
    }

    public ICommand SaveCommand
    {
        get
        {
            _save ??= ReactiveCommand.CreateFromTask(SaveAction);
            return _save;
        }
    }

    public ICommand AddPageCommand
    {
        get
        {
            _addPage ??= ReactiveCommand.CreateFromTask(AddPageAction);
            return _addPage;
        }
    }

    public ICommand RemovePageCommand
    {
        get
        {
            _removePage ??= ReactiveCommand.Create<string>(RemovePageAction);
            return _removePage;
        }
    }

    #endregion

    void ClearAction()
    {
        Book = new();
    }

    async Task SaveAction()
    {
        Error = Book.Validate();
        if (!string.IsNullOrEmpty(Error))
            return;

        IsBusy = true;
        await _bookService.Save(Book);
        IsBusy = false;

        ClosePopup();
    }

    async Task AddPageAction()
    {
        IsBusy = true;
        var topLevel = TopLevel.GetTopLevel(GetMainWindow());

        var options = new FilePickerOpenOptions
        {
            Title = "Select Book Pages",
            AllowMultiple = true,
            FileTypeFilter = [ FilePickerFileTypes.ImageAll ]
        };
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);

        foreach (var file in files)
        {
            var path = file.Path.LocalPath;
            if (!Book.Pages.Contains(path))
                Book.Pages.Add(path);
        }
        IsBusy = false;
    }

    void RemovePageAction(string path)
    {
        Book.Pages.Remove(path);
    }
}
