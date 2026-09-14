using GymStats.ViewModels;

namespace GymStats;

public partial class SelectExerciseForTemplatePage : ContentPage
{
    public SelectExerciseForTemplatePage(SelectExerciseForTemplateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SelectExerciseForTemplateViewModel vm)
        {
            await vm.LoadExercisesAsync();
        }
    }
}