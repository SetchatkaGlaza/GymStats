using GymStats.ViewModels;

namespace GymStats;

public partial class AnthropometryPage : ContentPage
{
    public AnthropometryPage(AnthropometryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AnthropometryViewModel vm)
        {
            await vm.LoadMeasurementsAsync();
        }
    }
}