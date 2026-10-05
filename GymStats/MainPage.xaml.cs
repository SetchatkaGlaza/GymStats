using GymStats.ViewModels;

namespace GymStats;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MainPageViewModel vm)
            await vm.LoadAsync();
    }
}