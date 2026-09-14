using GymStats.ViewModels;

namespace GymStats;

public partial class AddWorkoutPage : ContentPage
{
    public AddWorkoutPage(AddWorkoutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}