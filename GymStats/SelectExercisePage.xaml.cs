using GymStats.ViewModels;

namespace GymStats;

public partial class SelectExercisePage : ContentPage
{
    public SelectExercisePage(SelectExerciseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SelectExerciseViewModel vm)
        {
            await vm.LoadExercisesAsync();
        }
    }
}