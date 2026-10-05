using GymStats.ViewModels;

namespace GymStats;

public partial class AddExercisePage : ContentPage
{
    public AddExercisePage(AddExerciseViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}