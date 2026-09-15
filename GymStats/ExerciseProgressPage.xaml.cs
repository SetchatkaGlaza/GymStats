using GymStats.ViewModels;

namespace GymStats;

public partial class ExerciseProgressPage : ContentPage
{
    public ExerciseProgressPage(ExerciseProgressViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ExerciseProgressViewModel vm)
        {
            await vm.LoadExercisesAsync();
        }
    }
}