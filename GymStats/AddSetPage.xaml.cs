using GymStats.ViewModels;

namespace GymStats;

public partial class AddSetPage : ContentPage
{
    public AddSetPage(AddSetViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(120);
        if (BindingContext is AddSetViewModel && WeightEntry.IsVisible)
        {
            WeightEntry.Focus();
            WeightEntry.CursorPosition = WeightEntry.Text?.Length ?? 0;
        }
    }
}