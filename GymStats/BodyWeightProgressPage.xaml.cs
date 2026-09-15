using GymStats.ViewModels;

namespace GymStats;

public partial class BodyWeightProgressPage : ContentPage
{
    public BodyWeightProgressPage(BodyWeightProgressViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BodyWeightProgressViewModel vm)
        {
            await vm.LoadChartDataAsync();
        }
    }
}