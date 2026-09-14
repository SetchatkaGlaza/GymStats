using GymStats.ViewModels;

namespace GymStats;

public partial class FinishWorkoutPage : ContentPage
{
    public FinishWorkoutPage(FinishWorkoutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is FinishWorkoutViewModel vm)
        {
            await vm.LoadWorkoutDataAsync();
        }
    }
}