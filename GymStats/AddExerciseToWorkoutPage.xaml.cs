using GymStats.ViewModels;

namespace GymStats;

public partial class AddExerciseToWorkoutPage : ContentPage
{
    public AddExerciseToWorkoutPage(AddExerciseToWorkoutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AddExerciseToWorkoutViewModel vm)
        {
            await vm.LoadExercisesAsync();
        }
    }
}