using GymStats.ViewModels;

namespace GymStats;

public partial class AddWorkoutPage : ContentPage
{
    public AddWorkoutPage(AddWorkoutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AddWorkoutViewModel vm)
        {
            await vm.LoadTemplatesAsync();
        }
    }
}