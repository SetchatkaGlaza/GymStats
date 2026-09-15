using GymStats.ViewModels;

namespace GymStats;

public partial class MediaArchivePage : ContentPage
{
    public MediaArchivePage(MediaArchiveViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MediaArchiveViewModel vm)
        {
            await vm.LoadMediaAsync();
        }
    }
}