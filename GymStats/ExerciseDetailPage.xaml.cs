using GymStats.ViewModels;

namespace GymStats;

public partial class ExerciseDetailPage : ContentPage
{
    public ExerciseDetailPage(ExerciseDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ExerciseDetailViewModel vm)
        {
            await vm.LoadDataAsync();
        }
    }
}