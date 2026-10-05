using GymStats.ViewModels;

namespace GymStats;

public partial class MediaPage : ContentPage
{
    public MediaPage(MediaViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MediaViewModel vm)
            await vm.LoadAsync();
    }
}