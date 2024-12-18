using AvaloniaDemosntration.Base;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace AvaloniaDemosntration.Models;
public class BookModel: ModelBase
{
    string _title;
    string _description;
    string _author;
    string _isbn;
    ObservableCollection<string> _pages;

    public BookModel()
    {
        _pages = new ObservableCollection<string>();
    }

    #region properties

    public string Title 
    {
        get => _title; 
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public string Author
    {
        get => _author;
        set => this.RaiseAndSetIfChanged(ref _author, value);
    }

    public string Isbn
    {
        get => _isbn;
        set => this.RaiseAndSetIfChanged(ref _isbn, value);
    }

    public ObservableCollection<string> Pages
    {
        get => _pages;
        set => this.RaiseAndSetIfChanged(ref _pages, value);
    }

    #endregion

    public override string Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))
            return "Book title is mandatory.";
        if (string.IsNullOrWhiteSpace(Isbn))
            return "ISBN is mandatory.";
        if (Isbn.Replace("-", string.Empty).Length != 13)
            return "ISBN must be 13 characters long.";
        if (Pages.Count == 0)
            return "Book must have atleast one page.";

        return default;
    }
}
