using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using AvaloniaDemosntration.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AvaloniaDemosntration.Services;
public class BookService: ReactiveObject, IBookService
{
    async Task<string> GetDataDirectory()
    {
        var window = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow;
        var storage = TopLevel.GetTopLevel(window).StorageProvider;
        var documentsPath = await storage.TryGetWellKnownFolderAsync(WellKnownFolder.Documents);
        var path = Path.Combine(documentsPath.Path.LocalPath, App.Current.Name);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return path;
    }

    public async Task<bool> Save(BookModel book)
    {
        try
        {
            var fileName = $"{book.Title.Replace(' ', '_')}.json";
            var path = Path.Combine(await GetDataDirectory(), fileName);
            var json = JsonSerializer.Serialize(book);
            File.WriteAllText(path, json);
        }
        catch
        {
            return false;
        }
       
        return true;
    }

    public async Task<bool> Remove(BookModel book)
    {
        try
        {
            var fileName = $"{book.Title.Replace(' ', '_')}.json";
            var path = Path.Combine(await GetDataDirectory(), fileName);
            if (File.Exists(path))
                File.Delete(path);
        }
        catch
        {
            return false;
        }
        
        return true;
    }

    public async Task<IList<BookModel>> GetAll()
    {
        var books = new List<BookModel>();  
        try
        {
            var path = await GetDataDirectory();
            foreach(var filePath in Directory.GetFiles(path, "*.json", SearchOption.TopDirectoryOnly))
            {
                var json = await File.ReadAllTextAsync(filePath);

                var book = JsonSerializer.Deserialize<BookModel>(json);
                books.Add(book);
            }

        }
        catch
        {
            // we are doing nothing here
        }

        return books;
    }
}
