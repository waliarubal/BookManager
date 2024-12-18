using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace AvaloniaDemosntration.Base;

public abstract class ViewModelBase : ReactiveObject
{
    Window _window;
    bool _isBusy;

    #region properties

    public bool IsBusy 
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    #endregion

    protected Window GetMainWindow()
    {
        return (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow;
    }

    protected async Task ShowPopup(ViewModelBase viewModel)
    {
        if (_window != null)
            return;

        var typeName = viewModel.GetType().FullName.Replace("ViewModel", "View");
        var type = Type.GetType(typeName);

        var view = Activator.CreateInstance(type) as UserControl;
        view.DataContext = viewModel;

        viewModel._window = new Window
        {
            Title = "Book Manager",
            Width = 640,
            Height = 480,
            Content = view,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        await viewModel._window.ShowDialog(GetMainWindow());
    }

    protected void ClosePopup()
    {
        _window.Close();
        _window = null;
    }

    protected TService GetService<TService>()
    {
        return (App.Current as App).GetService<TService>();
    }
}
