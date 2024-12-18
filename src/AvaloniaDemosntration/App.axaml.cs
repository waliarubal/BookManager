using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaDemosntration.Services;
using AvaloniaDemosntration.ViewModels;
using AvaloniaDemosntration.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaDemosntration;

public partial class App : Application
{
    ServiceProvider _services;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// Initialize Di for service injection.
    /// </summary>
    void InitializeDi()
    {
        var services = new ServiceCollection();
        services.AddScoped<IBookService, BookService>();
        services.AddTransient<BookViewModel>();
        services.AddTransient<BooksViewModel>();

        _services = services.BuildServiceProvider();
    }

    internal TService GetService<TService>()
    {
        return _services.GetRequiredService<TService>();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Name = "Avalonia Demonstration";
        
        InitializeDi();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

}